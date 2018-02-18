//--------------------------------------------------------------------------
// <copyright file="DialCompletedEventArgs.cs" company="Jeff Winn">
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

    /// <summary>
    /// Provides data for the <see cref="RasDialer.DialCompleted"/> event. This class cannot be inherited.
    /// </summary>
    public sealed class DialCompletedEventArgs : AsyncCompletedEventArgs
    {
        #region Fields

        private bool _timedOut;
        private bool _connected;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.DialCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled"><b>true</b> if the asynchronous operation was cancelled, otherwise <b>false</b>.</param>
        /// <param name="timedOut"><b>true</b> if the operation timed out, otherwise <b>false</b>.</param>
        /// <param name="connected"><b>true</b> if the connection attempt successfully connected, otherwise <b>false</b>.</param>
        /// <param name="userState">The optional user-supplied state object.</param>
        internal DialCompletedEventArgs(Exception error, bool cancelled, bool timedOut, bool connected, object userState)
            : base(error, cancelled, userState)
        {
            this._timedOut = timedOut;
            this._connected = connected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the asynchronous dial attempt timed out.
        /// </summary>
        public bool TimedOut
        {
            get { return this._timedOut; }
        }

        /// <summary>
        /// Gets a value indicating whether the connection attempt successfully connected.
        /// </summary>
        public bool Connected
        {
            get { return this._connected; }
        }

        #endregion
    }
}