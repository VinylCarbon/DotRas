//--------------------------------------------------------------------------
// <copyright file="RasEntryType.cs" company="Jeff Winn">
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
    /// Defines the entry types.
    /// </summary>
    public enum RasEntryType
    {
        /// <summary>
        /// No entry type specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Phone line.
        /// </summary>
        Phone = 1,

        /// <summary>
        /// Virtual Private Network.
        /// </summary>
        Vpn = 2,
#if (!WIN2K8)
        /// <summary>
        /// Direct serial or parallel connection.
        /// </summary>
        Direct = 3,
#endif
        /// <summary>
        /// Connection Manager (CM) connection.
        /// </summary>
        /// <remarks>This member is reserved for system use only.</remarks>
        Internet = 4,
#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Broadband connection.
        /// </summary>
        Broadband = 5
#endif
    }
}