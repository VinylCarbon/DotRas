//--------------------------------------------------------------------------
// <copyright file="RasSlipInfo.cs" company="Jeff Winn">
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

#if (!WIN2K8)
    /// <summary>
    /// Contains the result of a Serial Line Internet Protocol (SLIP) projection operation. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasSlipInfo
    {
        #region Fields

        private int _errorCode;
        private IPAddress _ipAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasSlipInfo"/> class.
        /// </summary>
        /// <param name="errorCode">The error code (if any) that occurred.</param>
        /// <param name="ipAddress">The client IP address on the connection.</param>
        internal RasSlipInfo(int errorCode, IPAddress ipAddress)
        {
            this._errorCode = errorCode;
            this._ipAddress = ipAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error code (if any) that occurred.
        /// </summary>
        public int ErrorCode
        {
            get { return this._errorCode; }
        }
        
        /// <summary>
        /// Gets the client IP address on the connection.
        /// </summary>
        public IPAddress IPAddress
        {
            get { return this._ipAddress; }
        }

        #endregion
    }
#endif
}