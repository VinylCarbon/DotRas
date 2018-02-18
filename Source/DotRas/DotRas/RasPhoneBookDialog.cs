﻿//--------------------------------------------------------------------------
// <copyright file="RasPhoneBookDialog.cs" company="Jeff Winn">
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
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using DotRas.Design;
    using DotRas.Properties;

    /// <summary>
    /// Displays the primary Dial-Up Networking dialog box. This class cannot be inherited.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RasPhoneBookDialog), "DotRas.RasPhoneBookDialog.bmp")]
    public sealed class RasPhoneBookDialog : RasCommonDialog
    {
        #region Fields

        private string _phoneBookPath;
        private string _entryName;
        private NativeMethods.RasPBDlgFunc _rasPhonebookDlgCallback;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasPhoneBookDialog"/> class.
        /// </summary>
        public RasPhoneBookDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user creates a new entry or copies an existing entry.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBDAddedEntryDesc")]
        public event EventHandler<EventArgs> AddedEntry;
        
        /// <summary>
        /// Occurs when the user changes an existing phone book entry.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBDChangedEntryDesc")]
        public event EventHandler<EventArgs> ChangedEntry;

        /// <summary>
        /// Occurs when the user successfully dials an entry.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBDDialedEntryDesc")]
        public event EventHandler<EventArgs> DialedEntry;

        /// <summary>
        /// Occurs when the user removes a phone book entry.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBDRemovedEntryDesc")]
        public event EventHandler<EventArgs> RemovedEntry;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the full path (including file name) to the phone book.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("REDPhoneBookPathDesc")]
        public string PhoneBookPath
        {
            get { return this._phoneBookPath; }
            set { this._phoneBookPath = value; }
        }

        /// <summary>
        /// Gets or sets the name of the entry to initially highlight.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("RPBDEntryNameDesc")]
        public string EntryName
        {
            get { return this._entryName; }
            set { this._entryName = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets all <see cref="RasPhoneBookDialog"/> properties to their default values.
        /// </summary>
        public override void Reset()
        {
            this.PhoneBookPath = null;
            this.EntryName = null;

            base.Reset();
        }

        /// <summary>
        /// Overridden. Displays the modal dialog.
        /// </summary>
        /// <param name="hwndOwner">The handle of the window that owns the dialog box.</param>
        /// <returns><b>true</b> if the user completed the entry successfully, otherwise <b>false</b>.</returns>
        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.SafeSubWindows)]
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            NativeMethods.RASPBDLG dlg = new NativeMethods.RASPBDLG();
            dlg.size = Marshal.SizeOf(typeof(NativeMethods.RASPBDLG));
            dlg.hwndOwner = hwndOwner;
            dlg.callback = this._rasPhonebookDlgCallback;
            dlg.callbackId = IntPtr.Zero;
            dlg.reserved = IntPtr.Zero;
            dlg.reserved2 = IntPtr.Zero;

            if (this.Location != Point.Empty)
            {
                dlg.left = this.Location.X;
                dlg.top = this.Location.Y;

                dlg.flags |= NativeMethods.RASPBDFLAG.PositionDlg;
            }

            bool retval = false;
            try
            {
                retval = UnsafeNativeMethods.RasPhonebookDlg(this.PhoneBookPath, this.EntryName, ref dlg);
                if (!retval && dlg.error != NativeMethods.SUCCESS)
                {
                    this.OnError(new RasErrorEventArgs(dlg.error, RasHelper.GetRasErrorString(dlg.error)));
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            catch (SecurityException)
            {
                ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
            }

            return retval;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DotRas.RasPhoneBookDialog"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._rasPhonebookDlgCallback = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this._rasPhonebookDlgCallback = new NativeMethods.RasPBDlgFunc(this.RasPhonebookDlgCallback);
        }

        /// <summary>
        /// Raises the <see cref="AddedEntry"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnAddedEntry(EventArgs e)
        {
            if (this.AddedEntry != null)
            {
                this.AddedEntry(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DialedEntry"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnDialedEntry(EventArgs e)
        {
            if (this.DialedEntry != null)
            {
                this.DialedEntry(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ChangedEntry"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnChangedEntry(EventArgs e)
        {
            if (this.ChangedEntry != null)
            {
                this.ChangedEntry(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="RemovedEntry"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnRemovedEntry(EventArgs e)
        {
            if (this.RemovedEntry != null)
            {
                this.RemovedEntry(this, e);
            }
        }

        /// <summary>
        /// Signaled by the remote access service of user activity while the dialog box is open.
        /// </summary>
        /// <param name="callbackId">An application defined value that was passed to the RasPhonebookDlg function.</param>
        /// <param name="eventType">The event that occurred.</param>
        /// <param name="message">A string whose value depends on the <paramref name="eventType"/> parameter.</param>
        /// <param name="data">Pointer to an additional buffer argument whose value depends on the <paramref name="eventType"/> parameter.</param>
        private void RasPhonebookDlgCallback(int callbackId, NativeMethods.RASPBDEVENT eventType, string message, IntPtr data)
        {
            switch (eventType)
            {
                case NativeMethods.RASPBDEVENT.AddEntry:
                    this.OnAddedEntry(EventArgs.Empty);
                    break;

                case NativeMethods.RASPBDEVENT.DialEntry:
                    this.OnDialedEntry(EventArgs.Empty);
                    break;

                case NativeMethods.RASPBDEVENT.EditEntry:
                    this.OnChangedEntry(EventArgs.Empty);
                    break;

                case NativeMethods.RASPBDEVENT.RemoveEntry:
                    this.OnRemovedEntry(EventArgs.Empty);
                    break;
            }
        }

        #endregion
    }
}