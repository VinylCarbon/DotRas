//--------------------------------------------------------------------------
// <copyright file="RasConnectionStatus.cs" company="Jeff Winn">
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

    /// <summary>
    /// Represents the current status of a remote access connection. This class cannot be inherited.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("PhoneNumber = {PhoneNumber}, ConnectionState = {ConnectionState}")]
    public sealed class RasConnectionStatus
    {
        #region Fields

        private RasConnectionState _connectionState;
        private int _errorCode;
        private string _errorMessage;
        private RasDevice _device;
        private string _phoneNumber;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasConnectionStatus"/> class.
        /// </summary>
        /// <param name="connectionState">The current connection state.</param>
        /// <param name="errorCode">The error code that occurred which caused a failed connection attempt.</param>
        /// <param name="errorMessage">The error message of the <paramref name="errorCode"/> that occurred.</param>
        /// <param name="device">The device through which the connection has been established.</param>
        /// <param name="phoneNumber">The phone number being dialed for this connection.</param>
        internal RasConnectionStatus(RasConnectionState connectionState, int errorCode, string errorMessage, RasDevice device, string phoneNumber)
        {
            this._connectionState = connectionState;
            this._errorCode = errorCode;
            this._errorMessage = errorMessage;
            this._device = device;
            this._phoneNumber = phoneNumber;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current connection state.
        /// </summary>
        public RasConnectionState ConnectionState
        {
            get { return this._connectionState; }
        }

        /// <summary>
        /// Gets the error code (if any) that occurred which caused a failed connection attempt.
        /// </summary>
        public int ErrorCode
        {
            get { return this._errorCode; }
        }

        /// <summary>
        /// Gets the error message for the <see cref="ErrorCode"/> that occurred.
        /// </summary>
        public string ErrorMessage
        {
            get { return this._errorMessage; }
        }

        /// <summary>
        /// Gets the device through which the connection has been established.
        /// </summary>
        public RasDevice Device
        {
            get { return this._device; }
        }

        /// <summary>
        /// Gets the phone number dialed for this specific connection.
        /// </summary>
        public string PhoneNumber
        {
            get { return this._phoneNumber; }
        }

        #endregion
    }
}