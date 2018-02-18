//--------------------------------------------------------------------------
// <copyright file="RasDialExtensionsOptions.cs" company="Jeff Winn">
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
    /// Defines the remote access dial extensions options.
    /// </summary>
    [Flags]
    public enum RasDialExtensionsOptions
    {
        /// <summary>
        /// No dial extension options have been specified.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Use the prefix and suffix that is in the RAS phone book.
        /// </summary>
        /// <remarks>If no entry name was specified during dialing, this member is ignored.</remarks>
        UsePrefixSuffix = 0x1,

        /// <summary>
        /// Accept paused states.
        /// </summary>
        /// <remarks>If this flag has not been set, the dialer reports a fatal error if it enters a paused state.</remarks>
        PausedStates = 0x2,

        /// <summary>
        /// Ignore the modem speaker setting that is in the RAS phone book, and use the setting specified by the <see cref="RasDialExtensionsOptions.SetModemSpeaker"/> flag.
        /// </summary>
        /// <remarks>If no entry name was specified during dialing, the choice is between using a default setting or if the <see cref="RasDialExtensionsOptions.SetModemSpeaker"/> flag.</remarks>
        IgnoreModemSpeaker = 0x4,

        /// <summary>
        /// Sets the modem speaker on.
        /// </summary>
        /// <remarks>If <see cref="RasDialExtensionsOptions.IgnoreModemSpeaker"/> is not set, this member is ignored, and the modem speaker is based on the phone book or default setting.</remarks>
        SetModemSpeaker = 0x8,

        /// <summary>
        /// Ignore the software compression setting that is in the RAS phone book, and uses the setting specified by the <see cref="RasDialExtensionsOptions.SetSoftwareCompression"/> flag.
        /// </summary>
        /// <remarks>If no entry name was specified during dialing, the choice is between using a default setting or if the <see cref="RasDialExtensionsOptions.SetSoftwareCompression"/> flag.</remarks>
        IgnoreSoftwareCompression = 0x10,

        /// <summary>
        /// Use software compression.
        /// </summary>
        /// <remarks>If <see cref="RasDialExtensionsOptions.IgnoreSoftwareCompression"/> is not set, this member is ignored, and sets the software compression state based on the phone book or default setting.</remarks>
        SetSoftwareCompression = 0x20,

        /// <summary>
        /// Undocumented flag.
        /// </summary>
        DisableConnectedUI = 0x40,

        /// <summary>
        /// Undocumented flag.
        /// </summary>
        DisableReconnectUI = 0x80,

        /// <summary>
        /// Undocumented flag.
        /// </summary>
        DisableReconnect = 0x100,

        /// <summary>
        /// Undocumented flag.
        /// </summary>
        NoUser = 0x200,

        /// <summary>
        /// Used internally by the <see cref="RasDialDialog"/> class so that a Windows-95-style logon script is executed in a terminal window visible to the user.
        /// </summary>
        /// <remarks>Applications should not set this flag.</remarks>
        PauseOnScript = 0x400,

        /// <summary>
        /// Undocumented flag.
        /// </summary>
        Router = 0x800,

        /// <summary>
        /// Dial normally instead of calling the RasCustomDial entry point of the custom dialer.
        /// </summary>
        CustomDial = 0x1000,
#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Specifies that RasDial should invoke a custom-scripting DLL after establishing the connection to the server.
        /// </summary>
        UseCustomScripting = 0x2000
#endif
    }
}