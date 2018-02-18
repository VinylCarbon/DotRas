//--------------------------------------------------------------------------
// <copyright file="RasConnectionOptions.cs" company="Jeff Winn">
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

#if (WINXP || WINXPSP2 || WIN2K8)

    /// <summary>
    /// Defines the remote access connection options.
    /// </summary>
    [Flags]
    public enum RasConnectionOptions
    {
        /// <summary>
        /// No connection options specified.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Specifies the connection is available to all users.
        /// </summary>
        AllUsers = 0x1,

        /// <summary>
        /// Specifies the credentials used for the connection are the default credentials.
        /// </summary>
        GlobalCredentials = 0x2,

        /// <summary>
        /// Specifies the owner of the connection is known.
        /// </summary>
        OwnerKnown = 0x4,

        /// <summary>
        /// Specifies the owner of the connection matches the current user.
        /// </summary>
        OwnerMatch = 0x8
    }

#endif
}