//--------------------------------------------------------------------------
// <copyright file="RasErrorEventArgs.cs" company="Jeff Winn">
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
    /// Provides data for remote access service (RAS) error events.
    /// </summary>
    [Serializable]
    public class RasErrorEventArgs : EventArgs
    {
        #region Fields

        private int _errorCode;
        private string _errorMessage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasErrorEventArgs"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that occurred.</param>
        /// <param name="errorMessage">The error message associated with the error code.</param>
        internal RasErrorEventArgs(int errorCode, string errorMessage)
        {
            this._errorCode = errorCode;
            this._errorMessage = errorMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the extended error code (if any) that occurred.
        /// </summary>
        public int ErrorCode
        {
            get { return this._errorCode; }
        }

        /// <summary>
        /// Gets the error message for the error code that occurred.
        /// </summary>
        public string ErrorMessage
        {
            get { return this._errorMessage; }
        }

        #endregion
    }
}