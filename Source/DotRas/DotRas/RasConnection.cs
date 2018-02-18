//--------------------------------------------------------------------------
// <copyright file="RasConnection.cs" company="Jeff Winn">
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
    using System.Diagnostics;
    using DotRas.Properties;

    /// <summary>
    /// Represents a remote access connection. This class cannot be inherited.
    /// </summary>
    /// <example>
    /// The following example shows how to use a <see cref="RasDialer"/> object to retrieve active connections.
    /// <code lang="C#">
    /// using (RasDialer dialer = new RasDialer())
    /// {
    ///    ReadOnlyCollection&lt;RasConnection&gt; connections = dialer.GetActiveConnections();
    /// }
    /// </code>
    /// <code lang="VB.NET">
    /// Dim dialer As RasDialer
    /// Try
    ///    dialer = New RasDialer
    ///    Dim connections As ReadOnlyCollection(Of RasConnection) = dialer.GetActiveConnections()
    /// Finally
    ///    If dialer IsNot Nothing Then
    ///        dialer.Dispose()
    ///    End If
    /// End Try
    /// </code>
    /// </example>
    [DebuggerDisplay("EntryName = {EntryName}")]
    public sealed class RasConnection : MarshalByRefObject
    {
        #region Fields

        private RasHandle _handle;
        private string _entryName;
        private RasDevice _device;
        private string _phoneBookPath;
        private int _subEntryId;
        private Guid _entryId;
#if (WINXP || WINXPSP2 || WIN2K8)
        private RasConnectionOptions _connectionOptions;
        private Luid _sessionId;
#endif
#if (WIN2K8)
        private Guid _correlationId;
#endif
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasConnection"/> class.
        /// </summary>
        internal RasConnection()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the handle of the connection.
        /// </summary>
        public RasHandle Handle
        {
            get { return this._handle; }
            internal set { this._handle = value; }
        }

        /// <summary>
        /// Gets the name of the phone book entry used to establish the remote access connection.
        /// </summary>
        /// <remarks>If the connection was established without using an entry name, this member contains a PERIOD (.) followed by the phone number.</remarks>
        public string EntryName
        {
            get { return this._entryName; }
            internal set { this._entryName = value; }
        }

        /// <summary>
        /// Gets the device through which the connection has been established.
        /// </summary>
        public RasDevice Device
        {
            get { return this._device; }
            internal set { this._device = value; }
        }

        /// <summary>
        /// Gets the full path and filename to the phone book (PBK) containing the entry for this connection.
        /// </summary>
        public string PhoneBookPath
        {
            get { return this._phoneBookPath; }
            internal set { this._phoneBookPath = value; }
        }

        /// <summary>
        /// Gets the one-based subentry index of the connected link in a multilink connection.
        /// </summary>
        public int SubEntryId
        {
            get { return this._subEntryId; }
            internal set { this._subEntryId = value; }
        }

        /// <summary>
        /// Gets the <see cref="System.Guid"/> that represents the phone book entry.
        /// </summary>
        public Guid EntryId
        {
            get { return this._entryId; }
            internal set { this._entryId = value; }
        }

#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Gets any flags associated with the connection.
        /// </summary>
        public RasConnectionOptions ConnectionOptions
        {
            get { return this._connectionOptions; }
            internal set { this._connectionOptions = value; }
        }

        /// <summary>
        /// Gets the logon session id in which the connection was established.
        /// </summary>
        public Luid SessionId
        {
            get { return this._sessionId; }
            internal set { this._sessionId = value; }
        }
#endif
#if (WIN2K8)
        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public Guid CorrelationId
        {
            get { return this._correlationId; }
            internal set { this._correlationId = value; }
        }
#endif

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the connection status.
        /// </summary>
        /// <returns>An <see cref="DotRas.RasConnectionStatus"/> containing connection status information.</returns>
        public RasConnectionStatus GetConnectionStatus()
        {
            return RasHelper.GetConnectionStatus(this.Handle);
        }

        /// <summary>
        /// Retrieves accumulated statistics for the connection.
        /// </summary>
        /// <returns>A <see cref="DotRas.RasLinkStatistics"/> object containing connection statistics.</returns>
        public RasLinkStatistics GetConnectionStatistics()
        {
            return RasHelper.GetConnectionStatistics(this.Handle);
        }

        /// <summary>
        /// Clears any accumulated statistics for the connection.
        /// </summary>
        /// <returns><b>true</b> if the function succeeds, otherwise <b>false</b>.</returns>
        public bool ClearConnectionStatistics()
        {
            return RasHelper.ClearConnectionStatistics(this.Handle);
        }

        /// <summary>
        /// Clears any accumulated statistics for the link in a multilink connection.
        /// </summary>
        /// <returns><b>true</b> if the function succeeds, otherwise <b>false</b>.</returns>
        public bool ClearLinkStatistics()
        {
            return RasHelper.ClearLinkStatistics(this.Handle, this.SubEntryId);
        }

        /// <summary>
        /// Retrieves accumulated statistics for the link in a multilink connection.
        /// </summary>
        /// <returns>A <see cref="DotRas.RasLinkStatistics"/> object containing connection statistics.</returns>
        public RasLinkStatistics GetLinkStatistics()
        {
            return RasHelper.GetLinkStatistics(this.Handle, this.SubEntryId);
        }

#if (WIN2K8)
        /// <summary>
        /// Retrieves the network access protection (NAP) status for a remote access connection.
        /// </summary>
        /// <returns>A <see cref="DotRas.RasNapStatus"/> object containing the NAP status.</returns>
        public RasNapStatus GetNapStatus()
        {
            return RasHelper.GetNapStatus(this.Handle);
        }
#endif

        /// <summary>
        /// Retrieves information about a remote access projection operation.
        /// </summary>
        /// <param name="projectionType">The protocol of interest.</param>
        /// <returns>The resulting projection information, otherwise null reference (<b>Nothing</b> in Visual Basic) if the protocol was not found.</returns>
        public object GetProjectionInfo(RasProjectionType projectionType)
        {
            return RasHelper.GetProjectionInfo(this.Handle, projectionType);
        }

        /// <summary>
        /// Retrieves a connection handle for a subentry of a multilink connection.
        /// </summary>
        /// <param name="subEntryId">The one-based index of the subentry to whose handle to retrieve.</param>
        /// <returns>The handle of the subentry if available, otherwise a null reference (<b>Nothing</b> in Visual Basic).</returns>
        /// <exception cref="System.ArgumentException"><paramref name="subEntryId"/> cannot be less than or equal to zero.</exception>
        public RasHandle GetSubEntryHandle(int subEntryId)
        {
            if (subEntryId <= 0)
            {
                ThrowHelper.ThrowArgumentException("subEntryId", Resources.Argument_ValueCannotBeLessThanOrEqualToZero);
            }

            return RasHelper.GetSubEntryHandle(this.Handle, subEntryId);
        }

        /// <summary>
        /// Terminates the remote access connection.
        /// </summary>
        /// <returns><b>true</b> if the connection was terminated successfully, otherwise <b>false</b>.</returns>
        public bool HangUp()
        {
            return RasHelper.HangUp(this.Handle);
        }

        #endregion
    }
}