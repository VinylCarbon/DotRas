//--------------------------------------------------------------------------
// <copyright file="RasNetworkProtocols.cs" company="Jeff Winn">
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
    /// Defines the network protocols used for negotiation.
    /// </summary>
    [Flags]
    public enum RasNetworkProtocols
    {
        /// <summary>
        /// No network protocol specified.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Negotiate the NetBEUI protocol.
        /// </summary>
        [Obsolete("This member is no longer supported.")]
        NetBeui = 0x1,

        /// <summary>
        /// Negotiate the IPX protocol.
        /// </summary>
        Ipx = 0x2,

        /// <summary>
        /// Negotiate the IPv4 protocol.
        /// </summary>
        IP = 0x4,
#if (WIN2K8)
        /// <summary>
        /// Negotiate the IPv6 protocol.
        /// </summary>
        IPv6 = 0x8
#endif
    }
}