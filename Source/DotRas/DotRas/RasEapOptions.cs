//--------------------------------------------------------------------------
// <copyright file="RasEapOptions.cs" company="Jeff Winn">
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
    /// Defines flags that are used to qualify the authentication process.
    /// </summary>
    [Flags]
    public enum RasEapOptions
    {
        /// <summary>
        /// No flags are used.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the authentication protocol should not bring up a graphical user interface. If this flag is not present,
        /// it is okay for the protocol to display a user interface.
        /// </summary>
        NonInteractive = 0x2,

        /// <summary>
        /// Specifies that the user data is obtained from WinLogon.
        /// </summary>
        LogOn = 0x4,

        /// <summary>
        /// Specifies that the user should be prompted for identity information before dialing.
        /// </summary>
        Preview = 0x8
    }
}