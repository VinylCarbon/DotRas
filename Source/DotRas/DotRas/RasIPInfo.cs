//--------------------------------------------------------------------------
// <copyright file="RasIPInfo.cs" company="Jeff Winn">
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
    using System.Net;

    /// <summary>
    /// Contains the result of an IP projection operation. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasIPInfo
    {
        #region Fields

        private int _errorCode;
        private IPAddress _ipAddress;
        private IPAddress _serverIPAddress;
        private RasIPOptions _options;
        private RasIPOptions _serverOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasIPInfo"/> class.
        /// </summary>
        /// <param name="errorCode">The error code (if any) that occurred.</param>
        /// <param name="ipAddress">The client IP address.</param>
        /// <param name="serverIPAddress">The server IP address.</param>
        /// <param name="options">The IPCP options for the local computer.</param>
        /// <param name="serverOptions">The IPCP options for the remote computer.</param>
        internal RasIPInfo(int errorCode, IPAddress ipAddress, IPAddress serverIPAddress, RasIPOptions options, RasIPOptions serverOptions)
        {
            this._errorCode = errorCode;
            this._ipAddress = ipAddress;
            this._serverIPAddress = serverIPAddress;
            this._options = options;
            this._serverOptions = serverOptions;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error code (if any) that occurred.
        /// </summary>
        /// <remarks>This member indicates the actual fatal error (if any) that occurred during the control protocol negotiation, the error that prevented the projection from completing successfully.</remarks>
        public int ErrorCode
        {
            get { return this._errorCode; }
        }

        /// <summary>
        /// Gets the client IP address.
        /// </summary>
        public IPAddress IPAddress
        {
            get { return this._ipAddress; }
        }

        /// <summary>
        /// Gets the server IP address.
        /// </summary>
        public IPAddress ServerIPAddress
        {
            get { return this._serverIPAddress; }
        }

        /// <summary>
        /// Gets the IPCP options for the local computer.
        /// </summary>
        public RasIPOptions Options
        {
            get { return this._options; }
        }

        /// <summary>
        /// Gets the IPCP options for the remote computer.
        /// </summary>
        public RasIPOptions ServerOptions
        {
            get { return this._serverOptions; }
        }

        #endregion
    }
}