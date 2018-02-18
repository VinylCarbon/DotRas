//--------------------------------------------------------------------------
// <copyright file="RasLcpInfo.cs" company="Jeff Winn">
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

    /// <summary>
    /// Contains the result of a Link Control Protocol (LCP) multilink projection operation. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasLcpInfo
    {
        #region Fields

        private bool _bundled;
        private int _errorCode;
        private RasLcpAuthenticationType _authenticationProtocol;
        private RasLcpAuthenticationDataType _authenticationData;
        private int _eapTypeId;
        private RasLcpAuthenticationType _serverAuthenticationProtocol;
        private RasLcpAuthenticationDataType _serverAuthenticationData;
        private int _serverEapTypeId;
        private bool _multilink;
        private int _terminateReason;
        private int _serverTerminateReason;
        private string _replyMessage;
        private RasLcpOptions _options;
        private RasLcpOptions _serverOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasLcpInfo"/> class.
        /// </summary>
        /// <param name="bundled"><b>true</b> if the connection is composed of multiple links, otherwise <b>false</b>.</param>
        /// <param name="errorCode">The error code (if any) that occurred.</param>
        /// <param name="authenticationProtocol">The authentication protocol used to authenticate the client.</param>
        /// <param name="authenticationData">The authentication data about the authentication protocol used by the client.</param>
        /// <param name="eapTypeId">The type id of the Extensible Authentication Protocol (EAP) used to authenticate the local computer.</param>
        /// <param name="serverAuthenticatonProtocol">The authentication protocol used to authenticate the server.</param>
        /// <param name="serverAuthenticationData">The authentication data about the authentication protocol used by the server.</param>
        /// <param name="serverEapTypeId">The type id of the Extensible Authentication Protocol (EAP) used to authenticate the remote computer.</param>
        /// <param name="multilink"><b>true</b> if the connection supports multilink, otherwise <b>false</b>.</param>
        /// <param name="terminateReason">The reason the client terminated the connection.</param>
        /// <param name="serverTerminateReason">The reason the server terminated the connection.</param>
        /// <param name="replyMessage">The message (if any) from the authentication protocol success/failure packet.</param>
        /// <param name="options">The additional options for the local computer.</param>
        /// <param name="serverOptions">The additional options for the remote computer.</param>
        internal RasLcpInfo(bool bundled, int errorCode, RasLcpAuthenticationType authenticationProtocol, RasLcpAuthenticationDataType authenticationData, int eapTypeId, RasLcpAuthenticationType serverAuthenticatonProtocol, RasLcpAuthenticationDataType serverAuthenticationData, int serverEapTypeId, bool multilink, int terminateReason, int serverTerminateReason, string replyMessage, RasLcpOptions options, RasLcpOptions serverOptions)
        {
            this._bundled = bundled;
            this._errorCode = errorCode;
            this._authenticationProtocol = authenticationProtocol;
            this._authenticationData = authenticationData;
            this._eapTypeId = eapTypeId;
            this._serverAuthenticationProtocol = serverAuthenticatonProtocol;
            this._serverAuthenticationData = serverAuthenticationData;
            this._serverEapTypeId = serverEapTypeId;
            this._multilink = multilink;
            this._terminateReason = terminateReason;
            this._serverTerminateReason = serverTerminateReason;
            this._replyMessage = replyMessage;
            this._options = options;
            this._serverOptions = serverOptions;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the connection is composed of multiple links.
        /// </summary>
        public bool Bundled
        {
            get { return this._bundled; }
        }

        /// <summary>
        /// Gets the error code (if any) that occurred.
        /// </summary>
        public int ErrorCode
        {
            get { return this._errorCode; }
        }

        /// <summary>
        /// Gets the authentication protocol used to authenticate the client.
        /// </summary>
        public RasLcpAuthenticationType AuthenticationProtocol
        {
            get { return this._authenticationProtocol; }
        }

        /// <summary>
        /// Gets the authentication data about the authentication protocol used by the client.
        /// </summary>
        public RasLcpAuthenticationDataType AuthenticationData
        {
            get { return this._authenticationData; }
        }

        /// <summary>
        /// Gets the type id of the Extensible Authentication Protocol (EAP) used to authenticate the local computer.
        /// </summary>
        /// <remarks>This member is valid only if <see cref="RasLcpInfo.AuthenticationProtocol"/> is <see cref="RasLcpAuthenticationType.Eap"/>.</remarks>
        public int EapTypeId
        {
            get { return this._eapTypeId; }
        }

        /// <summary>
        /// Gets the authentication protocol used to authenticate the server.
        /// </summary>
        public RasLcpAuthenticationType ServerAuthenticationProtocol
        {
            get { return this._serverAuthenticationProtocol; }
        }

        /// <summary>
        /// Gets the authentication data about the authentication protocol used by the server.
        /// </summary>
        public RasLcpAuthenticationDataType ServerAuthenticationData
        {
            get { return this._serverAuthenticationData; }
        }

        /// <summary>
        /// Gets the type id of the Extensible Authentication Protocol (EAP) used to authenticate the remote computer.
        /// </summary>
        /// <remarks>This member is valid only if <see cref="RasLcpInfo.ServerAuthenticationProtocol"/> is <see cref="RasLcpAuthenticationType.Eap"/>.</remarks>
        public int ServerEapTypeId
        {
            get { return this._serverEapTypeId; }
        }

        /// <summary>
        /// Gets a value indicating whether the connection supports multilink.
        /// </summary>
        public bool Multilink
        {
            get { return this._multilink; }
        }

        /// <summary>
        /// Gets the reason the client terminated the connection.
        /// </summary>
        /// <remarks>This member always has a return value of zero.</remarks>
        public int TerminateReason
        {
            get { return this._terminateReason; }
        }

        /// <summary>
        /// Gets the reason the server terminated the connection.
        /// </summary>
        /// <remarks>This member always has a return value of zero.</remarks>
        public int ServerTerminateReason
        {
            get { return this._serverTerminateReason; }
        }

        /// <summary>
        /// Gets the message (if any) from the authentication protocol success/failure packet.
        /// </summary>
        public string ReplyMessage
        {
            get { return this._replyMessage; }
        }

        /// <summary>
        /// Gets the additional options for the local computer.
        /// </summary>
        public RasLcpOptions Options
        {
            get { return this._options; }
        }

        /// <summary>
        /// Gets the additional options for the remote computer.
        /// </summary>
        public RasLcpOptions ServerOptions
        {
            get { return this._serverOptions; }
        }

        #endregion
    }
}