//--------------------------------------------------------------------------
// <copyright file="RasConnectionEventArgs.cs" company="Jeff Winn">
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
    /// Provides data for remote access service (RAS) connection events.
    /// </summary>
    public class RasConnectionEventArgs : EventArgs
    {
        #region Fields

        private RasConnection _connection;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="connection">Required. The <see cref="DotRas.RasConnection"/> that caused the event.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="connection"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        internal RasConnectionEventArgs(RasConnection connection)
        {
            if (connection == null)
            {
                ThrowHelper.ThrowArgumentNullException("connection");
            }

            this._connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the connection that caused the event.
        /// </summary>
        public RasConnection Connection
        {
            get { return this._connection; }
        }

        #endregion
    }
}