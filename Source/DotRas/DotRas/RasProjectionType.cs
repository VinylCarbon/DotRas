//--------------------------------------------------------------------------
// <copyright file="RasProjectionType.cs" company="Jeff Winn">
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
    /// Defines the projection types.
    /// </summary>
    public enum RasProjectionType
    {
        /// <summary>
        /// Authentication Message Block (AMB) protocol.
        /// </summary>
        Amb = 0x10000,

        /// <summary>
        /// NetBEUI Framer (NBF) protocol.
        /// </summary>
        Nbf = 0x803F,

        /// <summary>
        /// Internetwork Packet Exchange (IPX) control protocol.
        /// </summary>
        Ipx = 0x802B,

        /// <summary>
        /// Internet Protocol (IP) control protocol.
        /// </summary>
        IP = 0x8021,

        /// <summary>
        /// Compression Control Protocol (CCP).
        /// </summary>
        Ccp = 0x80FD,

        /// <summary>
        /// Link Control Protocol (LCP).
        /// </summary>
        Lcp = 0xC021,

#if (WIN2K8)
        /// <summary>
        /// Internet Protocol Version 6 (IPv6) control protocol.
        /// </summary>
        IPv6 = 0x8057,
#else
        /// <summary>
        /// Serial Line Internet Protocol (SLIP).
        /// </summary>
        Slip = 0x20000
#endif
    }
}