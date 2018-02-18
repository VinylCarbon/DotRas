//--------------------------------------------------------------------------
// <copyright file="RasLcpOptions.cs" company="Jeff Winn">
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
    /// Defines the Link Control Protocol (LCP) options.
    /// </summary>
    [Flags]
    public enum RasLcpOptions
    {
        /// <summary>
        /// No LCP options used.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Protocol Field Compression.
        /// </summary>
        Pfc = 0x1,

        /// <summary>
        /// Address and Control Field Compression.
        /// </summary>
        Acfc = 0x2,

        /// <summary>
        /// Short Sequence Number Header Format.
        /// </summary>
        Sshf = 0x4,

        /// <summary>
        /// DES 56-bit encryption.
        /// </summary>
        Des56 = 0x8,

        /// <summary>
        /// Triple DES encryption.
        /// </summary>
        TripleDes = 0x10,
#if (WIN2K8)
        /// <summary>
        /// AES 128-bit encryption.
        /// </summary>
        Aes128 = 0x20,

        /// <summary>
        /// AES 256-bit encryption.
        /// </summary>
        Aes256 = 0x40
#endif
    }
}