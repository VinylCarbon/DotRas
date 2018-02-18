//--------------------------------------------------------------------------
// <copyright file="RasDialer.cs" company="Jeff Winn">
//      Copyright (c) Jeff Winn. All rights reserved.
//
//      The use and distribution terms for this software is covered by the
//      GNU Library General Public License (LGPL) v2.1 which can be found
//      in the License.rtf at the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by
//      the terms of this license.
//
//      You must not remove this notice, or any other, from this software.
// </copyright>
//--------------------------------------------------------------------------

namespace DotRas
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Net;
    using System.Security.Permissions;
    using System.Threading;
    using System.Windows.Forms;
    using DotRas.Design;
    using DotRas.Properties;
    using Timer = System.Threading.Timer;

    /// <summary>
    /// Provides an interface to the remote access service (RAS) dialer. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Failure to dispose of this component may result in a connection attempt hanging, which will require the machine to be restarted before the connection is released.
    /// </para>
    /// <para>
    /// When using <b>RasDialer</b> to dial connections asynchronously, ensure the SynchronizingObject property is set if thread synchronization is required. If this is not done, you may get cross-thread exceptions thrown from the component.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example shows how to use a <b>RasDialer</b> component to dial an existing entry.
    /// <code lang="C#">
    /// using (RasDialer dialer = new RasDialer())
    /// {
    ///    dialer.EntryName = "My Connection";
    ///    dialer.Dial();
    /// }
    /// </code>
    /// <code lang="VB.NET">
    /// Dim dialer As RasDialer
    /// Try
    ///    dialer = New RasDialer
    ///    dialer.EntryName = "My Connection"
    ///    dialer.Dial()
    /// Finally
    ///    If (dialer IsNot Nothing) Then
    ///        dialer.Dispose()
    ///    End If
    /// End Try
    /// </code>
    /// </example>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RasDialer), "DotRas.RasDialer.bmp")]
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed partial class RasDialer : RasComponent
    {
        #region Fields

        /// <summary>
        /// Defines the object used to synchronize the component.
        /// </summary>
        private readonly object _syncRoot = new object();

        private string _entryName;
        private int _subEntryId;
        private string _phoneBookPath;
        private string _phoneNumber;
        private string _callbackNumber;
        private RasEapOptions _eapOptions;
        private RasEapInfo _eapData;
        private RasDialExtensionsOptions _options;
        private IWin32Window _owner;
        private int _timeout;

        private AsyncOperation _asyncOp;
        private SendOrPostCallback _dialCompletedCallback;
        private NativeMethods.RasDialFunc2 _rasDialCallback;
        private TimerCallback _timeoutCallback;
        private Timer _timer;

        /// <summary>
        /// Contains a value indicating whether a dialing operation is currently in progress.
        /// </summary>
        private bool _dialing;

        /// <summary>
        /// Contains a value indicating whether the connection attempt was cancelled.
        /// </summary>
        private bool _cancelled;

        /// <summary>
        /// Contains the handle for the connection currently in progress.
        /// </summary>
        private RasHandle _handle;        

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasDialer"/> class.
        /// </summary>
        public RasDialer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasDialer"/> class.
        /// </summary>
        /// <param name="container">An <see cref="System.ComponentModel.IContainer"/> that will contain the component.</param>
        public RasDialer(IContainer container)
            : base(container)
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the connection state changes.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RDStateChangedDesc")]
        public event EventHandler<StateChangedEventArgs> StateChanged;

        /// <summary>
        /// Occurs when the asynchronous dial operation has completed.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RDDialCompletedDesc")]
        public event EventHandler<DialCompletedEventArgs> DialCompleted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the full path (including filename) of a phone book.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("RDPhoneBookPathDesc")]
        [Editor(typeof(PhoneBookFileNameEditor), typeof(UITypeEditor))]
        public string PhoneBookPath
        {
            get { return this._phoneBookPath; }
            set { this._phoneBookPath = value; }
        }

        /// <summary>
        /// Gets or sets the name of the entry to dial.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("REDEntryNameDesc")]
        public string EntryName
        {
            get { return this._entryName; }
            set { this._entryName = value; }
        }

        /// <summary>
        /// Gets or sets the phone number to dial.
        /// </summary>
        /// <remarks>This value is not required when an entry name has been provided. Additionally, it will override any existing phone number if an entry name has been provided.</remarks>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("REDPhoneNumberDesc")]
        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { this._phoneNumber = value; }
        }

        /// <summary>
        /// Gets or sets the callback number.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("RDCallbackNumberDesc")]
        public string CallbackNumber
        {
            get { return this._callbackNumber; }
            set { this._callbackNumber = value; }
        }

        /// <summary>
        /// Gets or sets the one-based index of the subentry to dial.
        /// </summary>
        [DefaultValue(0)]
        [SRCategory("CatData")]
        [SRDescription("RDSubEntryIdDesc")]
        public int SubEntryId
        {
            get { return this._subEntryId; }
            set { this._subEntryId = value; }
        }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        [DefaultValue(typeof(RasDialExtensionsOptions), "None")]
        [SRCategory("CatBehavior")]
        [SRDescription("RDEntryOptionsDesc")]
        public RasDialExtensionsOptions Options
        {
            get { return this._options; }
            set { this._options = value; }
        }

        /// <summary>
        /// Gets or sets the extensible authentication protocol (EAP) data.
        /// </summary>
        [Browsable(false)]
        public RasEapInfo EapData
        {
            get { return this._eapData; }
            set { this._eapData = value; }
        }

        /// <summary>
        /// Gets or sets the extensible authentication protocol (EAP) options.
        /// </summary>
        [DefaultValue(typeof(RasEapOptions), "None")]
        [SRCategory("CatBehavior")]
        [SRDescription("RDEapOptionsDesc")]
        public RasEapOptions EapOptions
        {
            get { return this._eapOptions; }
            set { this._eapOptions = value; }
        }

        /// <summary>
        /// Gets or sets the parent window.
        /// </summary>
        /// <remarks>This object is used for dialog box creation and centering when a security DLL has been defined.</remarks>
        [DefaultValue(null)]
        [SRCategory("CatBehavior")]
        [SRDescription("RDOwnerDesc")]
        public IWin32Window Owner
        {
            get { return this._owner; }
            set { this._owner = value; }
        }

        /// <summary>
        /// Gets or sets the length of time (in milliseconds) until the asynchronous connection attempt times out.
        /// </summary>
        [DefaultValue(System.Threading.Timeout.Infinite)]
        [SRCategory("CatBehavior")]
        [SRDescription("RDTimeoutDesc")]
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a read-only list of active connections.
        /// </summary>
        /// <returns>A new read-only collection of <see cref="DotRas.RasConnection"/> objects, or an empty collection if no active connections were found.</returns>
        public ReadOnlyCollection<RasConnection> GetActiveConnections()
        {
            return RasHelper.GetActiveConnections();
        }

        /// <summary>
        /// Dials the connection.
        /// </summary>
        /// <returns>The handle of the connection.</returns>
        /// <exception cref="System.ArgumentException"><see cref="PhoneBookPath"/> cannot be an empty string or null reference (<b>Nothing</b> in Visual Basic) when an entry name has been provided.</exception>
        /// <exception cref="System.InvalidOperationException">A phone number or entry name is required to dial.</exception>
        public RasHandle Dial()
        {
            return this.Dial(null);
        }

        /// <summary>
        /// Dials the connection.
        /// </summary>
        /// <param name="credentials">An <see cref="System.Net.NetworkCredential"/> containing user credentials.</param>
        /// <returns>The handle of the connection.</returns>
        /// <exception cref="System.ArgumentException"><see cref="PhoneBookPath"/> cannot be an empty string or null reference (<b>Nothing</b> in Visual Basic) when an entry name has been provided.</exception>
        /// <exception cref="System.InvalidOperationException">A phone number or entry name is required to dial.</exception>
        public RasHandle Dial(NetworkCredential credentials)
        {
            return this.InternalDial(credentials, false);
        }

        /// <summary>
        /// Dials the connection asynchronously.
        /// </summary>
        /// <returns>The handle of the connection.</returns>
        /// <exception cref="System.ArgumentException"><see cref="PhoneBookPath"/> cannot be an empty string or null reference (<b>Nothing</b> in Visual Basic) when an entry name has been provided.</exception>
        /// <exception cref="System.InvalidOperationException">A phone number or entry name is required to dial.</exception>
        public RasHandle DialAsync()
        {
            return this.DialAsync(null);
        }

        /// <summary>
        /// Dials the connection asynchronously.
        /// </summary>
        /// <param name="credentials">An <see cref="System.Net.NetworkCredential"/> containing user credentials.</param>
        /// <returns>The handle of the connection.</returns>
        /// <exception cref="System.ArgumentException"><see cref="PhoneBookPath"/> cannot be an empty string or null reference (<b>Nothing</b> in Visual Basic) when an entry name has been provided.</exception>
        /// <exception cref="System.InvalidOperationException">A phone number or entry name is required to dial.</exception>
        public RasHandle DialAsync(NetworkCredential credentials)
        {
            return this.InternalDial(credentials, true);
        }

        /// <summary>
        /// Cancels the asynchronous dial operation.
        /// </summary>
        public void DialAsyncCancel()
        {
            lock (this._syncRoot)
            {
                if (this._dialing && !this._cancelled)
                {
                    this._cancelled = true;
                    this.Abort();

                    this.PostCompleted(null, true, false, false);
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DotRas.RasDialer"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {          
                if (this._dialing)
                {
                    // The component is currently dialing a connection, abort the connection attempt.
                    this.Abort();

                    this._handle = null;
                    this._asyncOp = null;
                }

                if (this._timer != null)
                {
                    this._timer.Dispose();
                    this._timer = null;
                }

                this._dialCompletedCallback = null;
                this._rasDialCallback = null;
                this._timeoutCallback = null;
            }          

            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected override void InitializeComponent()
        {
            this._timeout = System.Threading.Timeout.Infinite;

            this._dialCompletedCallback = new SendOrPostCallback(this.DialCompletedCallback);
            this._timeoutCallback = new TimerCallback(this.TimeoutCallback);
            this._rasDialCallback = new NativeMethods.RasDialFunc2(this.RasDialCallback);

            base.InitializeComponent();
        }

        /// <summary>
        /// Performs the dialing operation.
        /// </summary>
        /// <param name="credentials">An <see cref="System.Net.NetworkCredential"/> containing user credentials.</param>
        /// <param name="asynchronous"><b>true</b> if the dialing operation should be asynchronous, otherwise <b>false</b>.</param>
        /// <returns>The handle of the connection.</returns>
        /// <exception cref="System.ArgumentException"><see cref="PhoneBookPath"/> cannot be an empty string or null reference (<b>Nothing</b> in Visual Basic) when an entry name has been provided.</exception>
        /// <exception cref="System.InvalidOperationException">A phone number or entry name is required to dial.</exception>
        private RasHandle InternalDial(NetworkCredential credentials, bool asynchronous)
        {
            if (string.IsNullOrEmpty(this.EntryName) && string.IsNullOrEmpty(this.PhoneNumber))
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_PhoneNumberOrEntryNameRequired);
            }

            if (!string.IsNullOrEmpty(this.EntryName) && string.IsNullOrEmpty(this.PhoneBookPath))
            {
                ThrowHelper.ThrowArgumentException("PhoneBookPath", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            lock (this._syncRoot)
            {
                // NOTE: The synchronization object MUST be locked prior to testing of the component is already busy.
                // WARNING! Ensure no exceptions are thrown because existing dial attempts are already in progress. Doing so leaves the
                // connection open and cannot be closed if the application is terminated.
                if (!this._dialing)
                {                    
                    this._dialing = true;
                    this._cancelled = false;

                    try
                    {
                        NativeMethods.RASDIALPARAMS parameters = new NativeMethods.RASDIALPARAMS();
                        parameters.callbackId = IntPtr.Zero;
                        parameters.subEntryId = this.SubEntryId;

                        if (!string.IsNullOrEmpty(this.CallbackNumber))
                        {
                            parameters.callbackNumber = this.CallbackNumber;
                        }

                        if (!string.IsNullOrEmpty(this.EntryName))
                        {
                            parameters.entryName = this.EntryName;
                        }

                        if (!string.IsNullOrEmpty(this.PhoneNumber))
                        {
                            parameters.phoneNumber = this.PhoneNumber;
                        }

                        if (credentials == null)
                        {
                            // Attempt to use any credentials stored for the entry since the caller didn't explicitly specify anything.
                            NetworkCredential storedCredentials = RasHelper.GetCredentials(this.PhoneBookPath, this.EntryName, NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);

                            if (storedCredentials != null)
                            {
                                parameters.userName = storedCredentials.UserName;
                                parameters.password = storedCredentials.Password;
                                parameters.domain = storedCredentials.Domain;

                                storedCredentials = null;
                            }
                        }
                        else
                        {
                            parameters.userName = credentials.UserName;
                            parameters.password = credentials.Password;
                            parameters.domain = credentials.Domain;
                        }

                        NativeMethods.RASDIALEXTENSIONS extensions = new NativeMethods.RASDIALEXTENSIONS();
                        extensions.options = this.Options;

                        if (this.EapData != null)
                        {
                            NativeMethods.RASEAPINFO rasEapInfo = new NativeMethods.RASEAPINFO();
                            rasEapInfo.eapData = this.EapData.EapData;
                            rasEapInfo.sizeOfEapData = this.EapData.SizeOfEapData;

                            extensions.eapInfo = rasEapInfo;
                        }

                        if (this.Owner != null)
                        {
                            extensions.handle = this.Owner.Handle;
                        }

                        NativeMethods.RasDialFunc2 callback = null;
                        if (asynchronous)
                        {
                            callback = this._rasDialCallback;

                            this._asyncOp = AsyncOperationManager.CreateOperation(null);

                            if (this._timer != null)
                            {
                                // Dispose of any existing timer if the component is being reused.
                                this._timer.Dispose();
                                this._timer = null;
                            }

                            if (this._timeout != System.Threading.Timeout.Infinite)
                            {
                                // A timeout has been requested, create the timer used to handle the connection timeout.
                                this._timer = new Timer(this._timeoutCallback, null, this.Timeout, System.Threading.Timeout.Infinite);
                            }
                        }

                        this._handle = RasHelper.Dial(this.PhoneBookPath, parameters, extensions, callback, this.EapOptions);

                        if (!asynchronous)
                        {
                            // The synchronous dialing operation has completed, reset the dialing flag so the component can be reused.
                            this._dialing = false;
                        }
                    }
                    catch (Exception)
                    {
                        // An exception was thrown when the component was attempting to dial a connection. Reset the dialing flag so the component can be reused.
                        this._dialing = false;
                        throw;
                    }
                }
            }

            return this._handle;
        }

        /// <summary>
        /// Aborts the dial operation currently in progress.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        private void Abort()
        {
            lock (this._syncRoot)
            {
                if (this._handle != null && !this._handle.IsInvalid)
                {
                    RasHelper.HangUp(this._handle);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="RasDialer.DialCompleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="DotRas.DialCompletedEventArgs"/> containing event data.</param>
        private void OnDialCompleted(DialCompletedEventArgs e)
        {
            this.RaiseEvent<DialCompletedEventArgs>(this.DialCompleted, e);
        }

        /// <summary>
        /// Raises the <see cref="RasDialer.StateChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="DotRas.StateChangedEventArgs"/> containing event data.</param>
        private void OnStateChanged(StateChangedEventArgs e)
        {
            this.RaiseEvent<StateChangedEventArgs>(this.StateChanged, e);
        }

        /// <summary>
        /// Notifies the asynchronous operation in progress the operation has completed.
        /// </summary>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled"><b>true</b> if the asynchronous operation was cancelled, otherwise <b>false</b>.</param>
        /// <param name="timedOut"><b>true</b> if the operation timed out, otherwise <b>false</b>.</param>
        /// <param name="connected"><b>true</b> if the connection attempt successfully connected, otherwise <b>false</b>.</param>
        private void PostCompleted(Exception error, bool cancelled, bool timedOut, bool connected)
        {
            lock (this._syncRoot)
            {
                this._dialing = false;

                if (this._asyncOp != null)
                {
                    this._asyncOp.PostOperationCompleted(this._dialCompletedCallback, new DialCompletedEventArgs(error, cancelled, timedOut, connected, this._handle));

                    this._asyncOp = null;
                    this._handle = null;
                    this._timer = null;
                }
            }
        }

        /// <summary>
        /// Signaled by the asynchronous operation when the operation has completed.
        /// </summary>
        /// <param name="state">The object passed to the delegate.</param>
        private void DialCompletedCallback(object state)
        {
            DialCompletedEventArgs e = (DialCompletedEventArgs)state;
            this.OnDialCompleted(e);
        }

        /// <summary>
        /// Signaled by the internal <see cref="System.Threading.Timer"/> when the timeout duration has expired.
        /// </summary>
        /// <param name="state">An object containing application specific information</param>
        private void TimeoutCallback(object state)
        {
            // This lock must remain to prevent the timeout occurring before the dialing process has begun if the user
            // sets the timeout at 0 to start immediately.
            lock (this._syncRoot)
            {
                if (this._dialing && !this._cancelled)
                {
                    this.Abort();
                    this.PostCompleted(new TimeoutException(Resources.Exception_OperationTimedOut), false, true, false);
                }
            }
        }

        /// <summary>
        /// Signaled by the remote access service of the current state of the pending connection attempt.
        /// </summary>
        /// <param name="callbackId">An application defined value that was passed to the remote access service.</param>
        /// <param name="subEntryId">The one-based subentry index for the phone book entry associated with this connection.</param>
        /// <param name="dangerousHandle">The native handle to the connection.</param>
        /// <param name="message">The type of event that has occurred.</param>
        /// <param name="state">The state the remote access connection process is about to enter.</param>
        /// <param name="errorCode">The error that has occurred. If no error has occurred the value is zero.</param>
        /// <param name="extendedErrorCode">Any extended error information for certain non-zero values of <paramref name="errorCode"/>.</param>
        /// <returns><b>true</b> to continue to receive callback notifications, otherwise <b>false</b>.</returns>
        private bool RasDialCallback(int callbackId, int subEntryId, IntPtr dangerousHandle, int message, RasConnectionState state, int errorCode, int extendedErrorCode)
        {
            bool retval = true;

            lock (this._syncRoot)
            {
                if (!this._dialing || this._cancelled)
                {
                    retval = false;
                }
                else
                {
                    string errorMessage = null;
                    if (errorCode != NativeMethods.SUCCESS)
                    {
                        errorMessage = RasHelper.GetRasErrorString(errorCode);
                    }

                    StateChangedEventArgs e = new StateChangedEventArgs(
                        callbackId, 
                        subEntryId, 
                        new RasHandle(dangerousHandle), 
                        state, 
                        errorCode, 
                        errorMessage, 
                        extendedErrorCode);

                    this.OnStateChanged(e);

                    if (state == RasConnectionState.Connected)
                    {
                        this.PostCompleted(null, false, false, true);
                    }
                    else if (errorCode != NativeMethods.SUCCESS)
                    {
                        this.Abort();
                        this.PostCompleted(new RasDialException(errorCode, extendedErrorCode), false, false, false);

                        retval = false;
                    }
                }
            }

            return retval;
        }

        #endregion
    }
}