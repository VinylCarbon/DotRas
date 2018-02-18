//--------------------------------------------------------------------------
// <copyright file="RasCompressionOptions.cs" company="Jeff Winn">
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
    /// Defines the remote access service (RAS) compression options.
    /// </summary>
    /// <remarks>The members <see cref="RasCompressionOptions.Encryption56Bit"/>, <see cref="RasCompressionOptions.Encryption40Bit"/>, and <see cref="RasCompressionOptions.Encryption128Bit"/> are used when a connection is made over Layer 2 Tunneling Protocol (L2TP), and the connection uses IPSec encryption.</remarks>
    [Flags]
    public enum RasCompressionOptions
    {
        /// <summary>
        /// No compression options in use.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Compression without encryption.
        /// </summary>
        CompressionOnly = 0x1,

        /// <summary>
        /// Microsoft Point-to-Point Encryption (MPPE) in stateless mode.
        /// </summary>
        /// <remarks>The session key is changed after every packet. This mode improves performance on high latency networks, or networks that experience significant packet loss.</remarks>
        HistoryLess = 0x2,

        /// <summary>
        /// Microsoft Point-to-Point Encryption (MPPE) using 56 bit keys.
        /// </summary>
        Encryption56Bit = 0x10,

        /// <summary>
        /// Microsoft Point-to-Point Encryption (MPPE) using 40 bit keys.
        /// </summary>
        Encryption40Bit = 0x20,

        /// <summary>
        /// Microsoft Point-to-Point Encryption (MPPE) using 128 bit keys.
        /// </summary>
        Encryption128Bit = 0x40
    }
}
