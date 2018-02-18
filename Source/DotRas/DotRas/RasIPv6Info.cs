//--------------------------------------------------------------------------
// <copyright file="RasIPv6Info.cs" company="Jeff Winn">
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

#if (WIN2K8)

    /// <summary>
    /// Contains the result of an IPv6 projection operation. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasIPv6Info
    {
        #region Fields

        private int _errorCode;
        private byte[] _localInterfaceIdentifier;
        private byte[] _peerInterfaceIdentifier;
        private byte[] _localCompressionProtocol;
        private byte[] _peerCompressionProtocol;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasIPv6Info"/> class.
        /// </summary>
        /// <param name="errorCode">The error code (if any) that occurred.</param>
        /// <param name="localInterfaceIdentifier">The local 64-bit IPv6 interface identifier.</param>
        /// <param name="peerInterfaceIdentifier">The remote 64-bit IPv6 interface identifier.</param>
        /// <param name="localCompressionProtocol">The local compression protocol.</param>
        /// <param name="peerCompressionProtocol">The remote compression protocol.</param>
        internal RasIPv6Info(int errorCode, byte[] localInterfaceIdentifier, byte[] peerInterfaceIdentifier, byte[] localCompressionProtocol, byte[] peerCompressionProtocol)
        {
            this._errorCode = errorCode;
            this._localInterfaceIdentifier = localInterfaceIdentifier;
            this._peerInterfaceIdentifier = peerInterfaceIdentifier;
            this._localCompressionProtocol = localCompressionProtocol;
            this._peerCompressionProtocol = peerCompressionProtocol;
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
        /// Gets the local 64-bit IPv6 interface identifier.
        /// </summary>
        public byte[] LocalInterfaceIdentifier
        {
            get { return this._localInterfaceIdentifier; }
        }

        /// <summary>
        /// Gets the remote 64-bit IPv6 interface identifier.
        /// </summary>
        public byte[] PeerInterfaceIdentifier
        {
            get { return this._peerInterfaceIdentifier; }
        }

        /// <summary>
        /// Gets the local compression protocol.
        /// </summary>
        /// <remarks>Reserved for future use.</remarks>
        public byte[] LocalCompressionProtocol
        {
            get { return this._localCompressionProtocol; }
        }

        /// <summary>
        /// Gets the remote compression protocol.
        /// </summary>
        /// <remarks>Reserved for future use.</remarks>
        public byte[] PeerCompressionProtocol
        {
            get { return this._peerCompressionProtocol; }
        }

        #endregion
    }

#endif
}