//--------------------------------------------------------------------------
// <copyright file="RasCcpInfo.cs" company="Jeff Winn">
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
    /// Contains the results of a Compression Control Protocol (CCP) projection operation. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasCcpInfo
    {
        #region Fields

        private int _errorCode;
        private RasCompressionType _compressionAlgorithm;
        private RasCompressionOptions _options;
        private RasCompressionType _serverCompressionAlgorithm;
        private RasCompressionOptions _serverOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasCcpInfo"/> class.
        /// </summary>
        /// <param name="errorCode">The error code (if any) that occurred.</param>
        /// <param name="compressionAlgorithm">The compression algorithm in use by the client.</param>
        /// <param name="options">The compression options on the client.</param>
        /// <param name="serverCompressionAlgorithm">The compression algorithm in use by the server.</param>
        /// <param name="serverOptions">The compression options on the server.</param>
        internal RasCcpInfo(int errorCode, RasCompressionType compressionAlgorithm, RasCompressionOptions options, RasCompressionType serverCompressionAlgorithm, RasCompressionOptions serverOptions)
        {
            this._errorCode = errorCode;
            this._compressionAlgorithm = compressionAlgorithm;
            this._options = options;
            this._serverCompressionAlgorithm = serverCompressionAlgorithm;
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
        /// Gets the compression algorithm in use by the client.
        /// </summary>
        public RasCompressionType CompressionAlgorithm
        {
            get { return this._compressionAlgorithm; }
        }

        /// <summary>
        /// Gets the compression options on the client.
        /// </summary>
        public RasCompressionOptions Options
        {
            get { return this._options; }
        }

        /// <summary>
        /// Gets the compression algorithm in use by the server.
        /// </summary>
        public RasCompressionType ServerCompressionAlgorithm
        {
            get { return this._serverCompressionAlgorithm; }
        }

        /// <summary>
        /// Gets the compression options on the server.
        /// </summary>
        public RasCompressionOptions ServerOptions
        {
            get { return this._serverOptions; }
        }

        #endregion
    }
}