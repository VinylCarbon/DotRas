//--------------------------------------------------------------------------
// <copyright file="RasEntryExtendedOptions.cs" company="Jeff Winn">
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
    /// Defines the additional connection options for entries.
    /// </summary>
    [Flags]
    public enum RasEntryExtendedOptions
    {
        /// <summary>
        /// No additional entry options specified.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Prevents remote users from using file and print servers over the connection.
        /// </summary>
        SecureFileAndPrint = 0x1,

        /// <summary>
        /// Equivalent of clearing the Client for Microsoft Networks checkbox in the connection properties 
        /// dialog box on the networking tab.
        /// </summary>
        SecureClientForMSNet = 0x2,

        /// <summary>
        /// Changes the default behavior to not negotiate multilink.
        /// </summary>
        DoNotNegotiateMultilink = 0x4,

        /// <summary>
        /// Use the default credentials to access network resources.
        /// </summary>
        DoNotUseRasCredentials = 8,

        /// <summary>
        /// Use a pre-shared key for IPSec authentication.
        /// </summary>
        /// <remarks>This member is only used by L2TP/IPSec VPN connections.</remarks>
        UsePreSharedKey = 0x10,

        /// <summary>
        /// Indicates the connection is to the Internet.
        /// </summary>
        Internet = 0x20,

        /// <summary>
        /// Disables NBT probing for this connection.
        /// </summary>
        DisableNbtOverIP = 0x40,

        /// <summary>
        /// Ignore the device settings specified by the phone book entry.
        /// </summary>
        UseGlobalDeviceSettings = 0x80,

        /// <summary>
        /// Automatically attempts to re-establish the connection if the connection is lost.
        /// </summary>
        ReconnectIfDropped = 0x100,

        /// <summary>
        /// Use the same set of phone numbers for all subentries in a multilink connection.
        /// </summary>
        SharePhoneNumbers = 0x200,
#if (WIN2K8)
        /// <summary>
        /// Indicates the routing compartments feature is enabled.
        /// </summary>
        SecureRoutingCompartment = 0x400,

        /// <summary>
        /// Configures a VPN connection to use the typical settings for authentication and encryption for the RAS connection.
        /// </summary>
        UseTypicalSettings = 0x800,

        /// <summary>
        /// Uses the IPv6 DNS address and alternate DNS address for the connection.
        /// </summary>
        IPv6SpecificNameServer = 0x1000,

        /// <summary>
        /// Indicates the default route for IPv6 packets is through the PPP connection when the connection is active.
        /// </summary>
        IPv6RemoteDefaultGateway = 0x2000,

        /// <summary>
        /// Registers the IP address with the DNS server when connected.
        /// </summary>
        RegisterIPWithDns = 0x4000,

        /// <summary>
        /// The DNS suffix for this connection should be used for DNS registration.
        /// </summary>
        UseDnsSuffixForRegistration = 0x8000,

        /// <summary>
        /// Indicates the administrator is allowed to statically set the interface metric of the IPv4 stack for this interface.
        /// </summary>
        IPv4ExplicitMetric = 0x10000,

        /// <summary>
        /// Indicates the administrator is allowed to statically set the interface metric of the IPv6 stack for this interface.
        /// </summary>
        IPv6ExplicitMetric = 0x20000,

        /// <summary>
        /// The IKE validation check will not be performed.
        /// </summary>
        DisableIkeNameEkuCheck = 0x40000
#endif
    }
#endif
}