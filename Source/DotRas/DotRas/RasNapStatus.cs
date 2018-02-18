﻿//--------------------------------------------------------------------------
// <copyright file="RasNapStatus.cs" company="Jeff Winn">
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

#if (WIN2K8)
    /// <summary>
    /// Represents the current network access protection (NAP) status of a remote access connection. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasNapStatus
    {
        #region Fields

        private RasIsolationState _isolationState;
        private DateTime _probationTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasNapStatus"/> class.
        /// </summary>
        /// <param name="isolationState">The isolation state for the remote access connection.</param>
        /// <param name="probationTime">The time required for the connection to come out of quarantine.</param>
        internal RasNapStatus(RasIsolationState isolationState, DateTime probationTime)
        {           
            this._isolationState = isolationState;
            this._probationTime = probationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the isolation state.
        /// </summary>
        public RasIsolationState IsolationState
        {
            get { return this._isolationState; }
        }

        /// <summary>
        /// Gets the probation time.
        /// </summary>
        /// <remarks>Specifies the time required for the connection to come out of quarantine after which the connection will be dropped.</remarks>
        public DateTime ProbationTime
        {
            get { return this._probationTime; }
        }

        #endregion
    }
#endif
}