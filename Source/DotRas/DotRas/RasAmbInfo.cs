//--------------------------------------------------------------------------
// <copyright file="RasAmbInfo.cs" company="Jeff Winn">
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
    /// Contains the results of a Authentication Message Block (AMB) projection operation. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasAmbInfo
    {
        #region Fields

        private int _errorCode;
        private string _netBiosErrorMessage;
        private byte _lana;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasAmbInfo"/> class.
        /// </summary>
        /// <param name="errorCode">The error code (if any) that occurred.</param>
        /// <param name="netBiosErrorMessage">The NetBIOS error message (if applicable).</param>
        /// <param name="lana">The NetBIOS network adapter identifier on which the remote access connection was established.</param>
        internal RasAmbInfo(int errorCode, string netBiosErrorMessage, byte lana)
        {
            this._errorCode = errorCode;
            this._netBiosErrorMessage = netBiosErrorMessage;
            this._lana = lana;
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
        /// Gets the NetBIOS error message (if applicable).
        /// </summary>
        public string NetBiosErrorMessage
        {
            get { return this._netBiosErrorMessage; }
        }

        /// <summary>
        /// Gets the NetBIOS network adapter identifier on which the remote access connection was established.
        /// </summary>
        /// <remarks>This member contains <see cref="Byte.MaxValue"/> if a connection was not established.</remarks>
        public byte Lana
        {
            get { return this._lana; }
        }

        #endregion
    }
}