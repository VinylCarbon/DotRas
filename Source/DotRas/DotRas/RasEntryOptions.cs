//--------------------------------------------------------------------------
// <copyright file="RasEntryOptions.cs" company="Jeff Winn">
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
    /// Defines the connection options for entries.
    /// </summary>
    [Flags]
    public enum RasEntryOptions : uint
    {
        /// <summary>
        /// No entry options specified.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The country id, country code, and area code members are used to construct the phone number.
        /// </summary>
        UseCountryAndAreaCodes = 0x1,

        /// <summary>
        /// The IP address specified by the entry will be used for the connection.
        /// </summary>
        SpecificIPAddress = 0x2,

        /// <summary>
        /// The DNS addresses and WINS addresses specified by the entry will be used for the connection.
        /// </summary>
        SpecificNameServers = 0x4,

        /// <summary>
        /// IP header compression will be used on PPP (Point-to-point) connections.
        /// </summary>
        IPHeaderCompression = 0x8,

        /// <summary>
        /// The default route for IP packets is through the dial-up adapter while the connection is active.
        /// </summary>
        RemoteDefaultGateway = 0x10,

        /// <summary>
        /// The remote access service (RAS) will disable the PPP LCP extensions.
        /// </summary>
        DisableLcpExtensions = 0x20,

        /// <summary>
        /// The remote access service (RAS) displays a terminal window for user input before dialing the connection.
        /// </summary>
        /// <remarks>This member is only used when the entry is dialed by the component.</remarks>
        TerminateBeforeDial = 0x40,

        /// <summary>
        /// The remote access service displays a terminal window for user input after dialing the connection.
        /// </summary>
        /// <remarks>This member is only used when the entry is dialed by the component.</remarks>
        TerminateAfterDial = 0x80,

        /// <summary>
        /// The remote access service (RAS) will display a status monitor in the taskbar.
        /// </summary>
        ModemLights = 0x100,

        /// <summary>
        /// The software compression will be negotiated by the link.
        /// </summary>
        SoftwareCompression = 0x200,

        /// <summary>
        /// Only secure password schemes can be used to authenticate the client with the server.
        /// </summary>
        RequireEncryptedPassword = 0x400,

        /// <summary>
        /// Only the Microsoft secure password scheme (MSCHAP) can be used to authenticate the client with the server.
        /// </summary>
        RequireMSEncryptedPassword = 0x800,

        /// <summary>
        /// Data encryption must be negotiated successfully or the connection should be dropped.
        /// </summary>
        /// <remarks>This flag is ignored unless <see cref="RasEntryOptions.RequireMSEncryptedPassword"/> is also set.</remarks>
        RequireDataEncryption = 0x1000,

        /// <summary>
        /// The remote access service (RAS) logs on to the network after the point-to-point connection is established.
        /// </summary>
        NetworkLogOn = 0x2000,

        /// <summary>
        /// The remote access service (RAS) uses the username, password, and domain of the currently logged on user when dialing this entry.
        /// </summary>
        /// <remarks>This flag is ignored unless the <see cref="RasEntryOptions.RequireMSEncryptedPassword"/> is also set.</remarks>
        UseLogOnCredentials = 0x4000,

        /// <summary>
        /// Indicates when an alternate phone number connects successfully, that number will become the primary phone number. 
        /// </summary>
        PromoteAlternates = 0x8000,

        /// <summary>
        /// Check for an existing remote file system and remote printer bindings before making a connection to this phone book entry.
        /// </summary>
        SecureLocalFiles = 0x10000,

        /// <summary>
        /// Indicates the Extensible Authentication Protocol (EAP) must be supported for authentication.
        /// </summary>
        RequireEap = 0x20000,

        /// <summary>
        /// Indicates the Password Authentication Protocol (PAP) must be supported for authentication.
        /// </summary>
        RequirePap = 0x40000,

        /// <summary>
        /// Indicates Shiva's Password Authentication Protocol (SPAP) must be supported for authentication.
        /// </summary>
        RequireSpap = 0x80000,

        /// <summary>
        /// The connection will use custom encryption.
        /// </summary>
        Custom = 0x100000,

        /// <summary>
        /// The remote access dialer should display the phone number being dialed.
        /// </summary>
        PreviewPhoneNumber = 0x200000,

        /// <summary>
        /// Indicates all modems on the computer will share the same phone number.
        /// </summary>
        SharedPhoneNumbers = 0x800000,

        /// <summary>
        /// The remote access dialer should display the username and password prior to dialing.
        /// </summary>
        PreviewUserPassword = 0x1000000,

        /// <summary>
        /// The remote access dialer should display the domain name prior to dialing.
        /// </summary>
        PreviewDomain = 0x2000000,

        /// <summary>
        /// The remote access dialer will display its progress while establishing the connection.
        /// </summary>
        ShowDialingProgress = 0x4000000,

        /// <summary>
        /// Indicates the Challenge Handshake Authentication Protocol (CHAP) must be supported for authentication.
        /// </summary>
        RequireChap = 0x8000000,

        /// <summary>
        /// Indicates the Challenge Handshake Authentication Protocol (CHAP) must be supported for authentication.
        /// </summary>
        RequireMSChap = 0x10000000,

        /// <summary>
        /// Indicates the Challenge Handshake Authentication Protocol (CHAP) version 2 must be supported for authentication.
        /// </summary>
        RequireMSChap2 = 0x20000000,

        /// <summary>
        /// Indicates MSCHAP must also send the LanManager hashed password.
        /// </summary>
        /// <remarks>This flag requires that <see cref="RasEntryOptions.RequireMSChap"/> must also be set.</remarks>
        RequireWin95MSChap = 0x40000000,

        /// <summary>
        /// The remote access service (RAS) must invoke a custom scripting assembly after establishing a connection to the server.
        /// </summary>
        CustomScript = 0x80000000
    }
}