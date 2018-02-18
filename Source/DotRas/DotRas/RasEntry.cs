//--------------------------------------------------------------------------
// <copyright file="RasEntry.cs" company="Jeff Winn">
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
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net;
    using System.Security.Permissions;
    using DotRas.Properties;

    /// <summary>
    /// Represents a remote access service (RAS) entry. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("Name = {Name}, PhoneNumber = {PhoneNumber}")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public sealed class RasEntry : MarshalByRefObject, ICloneable
    {
        #region Fields

        private RasPhoneBook _owner;

        private string _name;
        private RasEntryOptions _options;
        private int _countryId;
        private int _countryCode;
        private string _areaCode;
        private string _phoneNumber;
        private Collection<string> _alternatePhoneNumbers;

        private IPAddress _ipAddress;
        private IPAddress _dnsAddress;
        private IPAddress _dnsAddressAlt;
        private IPAddress _winsAddress;
        private IPAddress _winsAddressAlt;

        private int _frameSize;
        private RasNetworkProtocols _networkProtocols;
        private RasFramingProtocol _framingProtocol;

        private string _script;

        private string _autoDialDll;
        private string _autoDialFunc;

        private RasDevice _device;

        private string _x25PadType;
        private string _x25Address;
        private string _x25Facilities;
        private string _x25UserData;
        private int _channels;

        private int _reserved1;
        private int _reserved2;

        private RasSubEntryCollection _subEntries;
        private RasDialMode _dialMode;
        private int _dialExtraPercent;
        private int _dialExtraSampleSeconds;
        private int _hangUpExtraPercent;
        private int _hangUpExtraSampleSeconds;

        private int _idleDisconnectSeconds;

        private RasEntryType _entryType;
        private RasEncryptionType _encryptionType;
        private int _customAuthKey;
        private Guid _id;

        private string _customDialDll;
        private RasVpnStrategy _vpnStrategy;

#if (WINXP || WINXPSP2 || WIN2K8)
        private RasEntryExtendedOptions _extendedOptions;
        private int _reservedOptions;
        private string _dnsSuffix;
        private int _tcpWindowSize;
        private string _prerequisitePhoneBook;
        private string _prerequisiteEntryName;
        private int _redialCount;
        private int _redialPause;
#endif
#if (WIN2K8)
        private IPAddress _ipv6DnsAddress;
        private IPAddress _ipv6DnsAddressAlt;
        private int _ipv4InterfaceMetric;
        private int _ipv6InterfaceMetric;
#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasEntry"/> class.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        /// <exception cref="System.ArgumentException"><paramref name="name"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        public RasEntry(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ThrowHelper.ThrowArgumentException("name", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            this._name = name;
            this._x25Address = string.Empty;
            this._x25PadType = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the owner of the entry.
        /// </summary>
        public RasPhoneBook Owner
        {
            get { return this._owner; }
            internal set { this._owner = value; }
        }

        /// <summary>
        /// Gets the name of the entry.
        /// </summary>
        public string Name
        {
            get { return this._name; }
            private set { this._name = value; }
        }

        /// <summary>
        /// Gets or sets the entry options.
        /// </summary>
        public RasEntryOptions Options
        {
            get { return this._options; }
            set { this._options = value; }
        }

        /// <summary>
        /// Gets or sets the country/region identifier.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.UseCountryAndAreaCodes"/> flag.</remarks>
        public int CountryId
        {
            get { return this._countryId; }
            set { this._countryId = value; }
        }

        /// <summary>
        /// Gets or sets the country/region code portion of the phone number.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.UseCountryAndAreaCodes"/> flag.</remarks>
        public int CountryCode
        {
            get { return this._countryCode; }
            set { this._countryCode = value; }
        }

        /// <summary>
        /// Gets or sets the area code.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.UseCountryAndAreaCodes"/> flag.</remarks>
        public string AreaCode
        {
            get { return this._areaCode; }
            set { this._areaCode = value; }
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { this._phoneNumber = value; }
        }

        /// <summary>
        /// Gets a collection of alternate phone numbers that are dialed in the order listed if the primary number fails.
        /// </summary>
        public Collection<string> AlternatePhoneNumbers
        {
            get
            {
                if (this._alternatePhoneNumbers == null)
                {
                    this._alternatePhoneNumbers = new Collection<string>();
                }

                return this._alternatePhoneNumbers;
            }

            internal set
            {
                this._alternatePhoneNumbers = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.SpecificIPAddress"/> flag.</remarks>
        public IPAddress IPAddress
        {
            get { return this._ipAddress; }
            set { this._ipAddress = value; }
        }

        /// <summary>
        /// Gets or sets the IP address of the DNS server.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.SpecificNameServers"/> flag.</remarks>
        public IPAddress DnsAddress
        {
            get { return this._dnsAddress; }
            set { this._dnsAddress = value; }
        }

        /// <summary>
        /// Gets or sets the IP address of an alternate DNS server.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.SpecificNameServers"/> flag.</remarks>
        public IPAddress DnsAddressAlt
        {
            get { return this._dnsAddressAlt; }
            set { this._dnsAddressAlt = value; }
        }

        /// <summary>
        /// Gets or sets the IP address of the WINS server.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.SpecificNameServers"/> flag.</remarks>
        public IPAddress WinsAddress
        {
            get { return this._winsAddress; }
            set { this._winsAddress = value; }
        }

        /// <summary>
        /// Gets or sets the IP address of an alternate WINS server.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.Options"/> sets the <see cref="RasEntryOptions.SpecificNameServers"/> flag.</remarks>
        public IPAddress WinsAddressAlt
        {
            get { return this._winsAddressAlt; }
            set { this._winsAddressAlt = value; }
        }

        /// <summary>
        /// Gets or sets the network protocol frame size.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.FramingProtocol"/> sets the <see cref="RasFramingProtocol.Slip"/> flag.</remarks>
        public int FrameSize
        {
            get { return this._frameSize; }
            set { this._frameSize = value; }
        }

        /// <summary>
        /// Gets or sets the network protocols to negotiate.
        /// </summary>
        public RasNetworkProtocols NetworkProtocols
        {
            get { return this._networkProtocols; }
            set { this._networkProtocols = value; }
        }

        /// <summary>
        /// Gets or sets the framing protocol used by the server.
        /// </summary>
        /// <remarks>To use Compressed SLIP, set the <see cref="RasFramingProtocol.Slip"/> flag, and set the <see cref="RasEntryOptions.IPHeaderCompression"/> flag on the <see cref="RasEntry.Options"/> property.</remarks>
        public RasFramingProtocol FramingProtocol
        {
            get { return this._framingProtocol; }
            set { this._framingProtocol = value; }
        }

        /// <summary>
        /// Gets or sets the path of the script file.
        /// </summary>
        /// <remarks>To indicate a SWITCH.INF script name, set the first character to "[".</remarks>
        public string Script
        {
            get { return this._script; }
            set { this._script = value; }
        }

        /// <summary>
        /// Gets or sets the path to the custom-dial DLL.
        /// </summary>
        [Obsolete("This member is no longer used. The CustomDialDll property should be used instead.")]
        public string AutoDialDll
        {
            get { return this._autoDialDll; }
            set { this._autoDialDll = value; }
        }

        /// <summary>
        /// Gets or sets the name of the callback function for the customized AutoDial handler.
        /// </summary>
        [Obsolete("This member is no longer used.")]
        public string AutoDialFunc
        {
            get { return this._autoDialFunc; }
            set { this._autoDialFunc = value; }
        }

        /// <summary>
        /// Gets or sets the remote access device.
        /// </summary>
        /// <remarks>To retrieve a list of available devices, use the <see cref="RasDevice.GetDevices"/> method.</remarks>
        public RasDevice Device
        {
            get { return this._device; }
            set { this._device = value; }
        }

        /// <summary>
        /// Gets or sets the X.25 PAD type.
        /// </summary>
        /// <remarks>This member should be an empty string unless the entry should dial using an X.25 PAD. This member maps to a section name in PAD.INF.</remarks>
        public string X25PadType
        {
            get { return this._x25PadType; }
            set { this._x25PadType = value; }
        }

        /// <summary>
        /// Gets or sets the X.25 address to connect to.
        /// </summary>
        /// <remarks>This member should be an empty string unless the entry should dial using an X.25 PAD or native X.25 device.</remarks>
        public string X25Address
        {
            get { return this._x25Address; }
            set { this._x25Address = value; }
        }

        /// <summary>
        /// Gets or sets the facilities to request from the X.25 host upon connection.
        /// </summary>
        /// <remarks>This member is ignored if the <see cref="RasEntry.X25Address"/> is an empty string.</remarks>
        public string X25Facilities
        {
            get { return this._x25Facilities; }
            set { this._x25Facilities = value; }
        }

        /// <summary>
        /// Gets or sets the additional connection information supplied to the X.25 host upon connection.
        /// </summary>
        /// <remarks>This member is ignored if the <see cref="RasEntry.X25Address"/> is an empty string.</remarks>
        public string X25UserData
        {
            get { return this._x25UserData; }
            set { this._x25UserData = value; }
        }

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        /// <remarks>This member is reserved for future use.</remarks>
        public int Channels
        {
            get { return this._channels; }
            set { this._channels = value; }
        }

        /// <summary>
        /// Gets or sets a reserved value.
        /// </summary>
        /// <remarks>This value must be zero.</remarks>
        public int Reserved1
        {
            get { return this._reserved1; }
            set { this._reserved1 = value; }
        }

        /// <summary>
        /// Gets or sets a reserved value.
        /// </summary>
        /// <remarks>This value must be zero.</remarks>
        public int Reserved2
        {
            get { return this._reserved2; }
            set { this._reserved2 = value; }
        }

        /// <summary>
        /// Gets the collection of multilink subentries associated with this entry.
        /// </summary>
        public RasSubEntryCollection SubEntries
        {
            get
            {
                if (this._subEntries == null)
                {
                    this._subEntries = new RasSubEntryCollection(this);
                }

                return this._subEntries;
            }
        }

        /// <summary>
        /// Gets or sets the dial mode for the multilink subentries associated with this entry.
        /// </summary>
        public RasDialMode DialMode
        {
            get { return this._dialMode; }
            set { this._dialMode = value; }
        }

        /// <summary>
        /// Gets or sets the percent of total bandwidth that must be used before additional subentries are dialed.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.DialMode"/> sets the <see cref="RasDialMode.DialAsNeeded"/> flag.</remarks>
        public int DialExtraPercent
        {
            get { return this._dialExtraPercent; }
            set { this._dialExtraPercent = value; }
        }

        /// <summary>
        /// Gets or sets the number of seconds the number of seconds before additional subentries are connected.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.DialMode"/> sets the <see cref="RasDialMode.DialAsNeeded"/> flag.</remarks>
        public int DialExtraSampleSeconds
        {
            get { return this._dialExtraSampleSeconds; }
            set { this._dialExtraSampleSeconds = value; }
        }

        /// <summary>
        /// Gets or sets the percent of total bandwidth used before subentries are disconnected.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.DialMode"/> sets the <see cref="RasDialMode.DialAsNeeded"/> flag.</remarks>
        public int HangUpExtraPercent
        {
            get { return this._hangUpExtraPercent; }
            set { this._hangUpExtraPercent = value; }
        }

        /// <summary>
        /// Gets or sets the number of seconds before subentries are disconnected.
        /// </summary>
        /// <remarks>This member is ignored unless <see cref="RasEntry.DialMode"/> sets the <see cref="RasDialMode.DialAsNeeded"/> flag.</remarks>
        public int HangUpExtraSampleSeconds
        {
            get { return this._hangUpExtraSampleSeconds; }
            set { this._hangUpExtraSampleSeconds = value; }
        }

        /// <summary>
        /// Gets or sets the number of seconds after which the connection is terminated due to inactivity.
        /// </summary>
        /// <remarks>See the <see cref="DotRas.RasIdleDisconnectTimeout"/> class for possible values.</remarks>
        public int IdleDisconnectSeconds
        {
            get { return this._idleDisconnectSeconds; }
            set { this._idleDisconnectSeconds = value; }
        }

        /// <summary>
        /// Gets or sets the type of phone book entry.
        /// </summary>
        public RasEntryType EntryType
        {
            get { return this._entryType; }
            set { this._entryType = value; }
        }

        /// <summary>
        /// Gets or sets the type of encryption to use with the connection.
        /// </summary>
        /// <remarks>This member does not affect how passwords are encrypted. Whether passwords are encrypted and how passwords are encrypted is determined by the authentication protocol.</remarks>
        public RasEncryptionType EncryptionType
        {
            get { return this._encryptionType; }
            set { this._encryptionType = value; }
        }

        /// <summary>
        /// Gets or sets the authentication key provided to the Extensible Authentication Protocol (EAP) vendor.
        /// </summary>
        public int CustomAuthKey
        {
            get { return this._customAuthKey; }
            set { this._customAuthKey = value; }
        }

        /// <summary>
        /// Gets the id of the phone book entry.
        /// </summary>
        public Guid Id
        {
            get { return this._id; }
            internal set { this._id = value; }
        }

        /// <summary>
        /// Gets or sets the path for the dynamic link library (DLL) that implements the custom-dialing functions.
        /// </summary>
        public string CustomDialDll
        {
            get { return this._customDialDll; }
            set { this._customDialDll = value; }
        }

        /// <summary>
        /// Gets or sets the VPN strategy to use when dialing a VPN connection.
        /// </summary>
        public RasVpnStrategy VpnStrategy
        {
            get { return this._vpnStrategy; }
            set { this._vpnStrategy = value; }
        }
#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Gets or sets additional connection options.
        /// </summary>
        /// <remarks>Setting the <see cref="RasEntryExtendedOptions.SecureFileAndPrint"/> and <see cref="RasEntryExtendedOptions.SecureClientForMSNet"/> flags has the additional effect of disabling NBT probing for the connection, which is the equivalent of the <see cref="RasEntryExtendedOptions.DisableNbtOverIP"/> flag.</remarks>
        public RasEntryExtendedOptions ExtendedOptions
        {
            get { return this._extendedOptions; }
            set { this._extendedOptions = value; }
        }

        /// <summary>
        /// Gets or sets reserved options.
        /// </summary>
        /// <remarks>This member is not used.</remarks>
        public int ReservedOptions
        {
            get { return this._reservedOptions; }
            set { this._reservedOptions = value; }
        }

        /// <summary>
        /// Gets or sets the Domain Name Service (DNS) suffix for the connection.
        /// </summary>
        public string DnsSuffix
        {
            get { return this._dnsSuffix; }
            set { this._dnsSuffix = value; }
        }

        /// <summary>
        /// Gets or sets the TCP window size for all TCP sessions that run over this connection.
        /// </summary>
        /// <remarks>The TCP window size on Windows Vista is determined by the TCP stack automatically, this member is ignored.</remarks>
        public int TcpWindowSize
        {
            get { return this._tcpWindowSize; }
            set { this._tcpWindowSize = value; }
        }

        /// <summary>
        /// Gets or sets the path to a phone book (PBK) file.
        /// </summary>
        /// <remarks>This member is only used for virtual private network (VPN) connections.</remarks>
        public string PrerequisitePhoneBook
        {
            get { return this._prerequisitePhoneBook; }
            set { this._prerequisitePhoneBook = value; }
        }

        /// <summary>
        /// Gets or sets the entry name that will be dialed from the prerequisite phone book.
        /// </summary>
        /// <remarks>This member is only used for virtual private network (VPN) connections.</remarks>
        public string PrerequisiteEntryName
        {
            get { return this._prerequisiteEntryName; }
            set { this._prerequisiteEntryName = value; }
        }

        /// <summary>
        /// Gets or sets the number of times RAS attempts to redial a connection.
        /// </summary>
        public int RedialCount
        {
            get { return this._redialCount; }
            set { this._redialCount = value; }
        }

        /// <summary>
        /// Gets or sets the number of seconds to wait between redial attempts.
        /// </summary>
        public int RedialPause
        {
            get { return this._redialPause; }
            set { this._redialPause = value; }
        }
#endif
#if (WIN2K8)
        /// <summary>
        /// Gets or sets the IPv6 address of the preferred DNS server.
        /// </summary>
        public IPAddress IPv6DnsAddress
        {
            get { return this._ipv6DnsAddress; }
            set { this._ipv6DnsAddress = value; }
        }

        /// <summary>
        /// Gets or sets the IPv6 address of the alternate DNS server.
        /// </summary>
        public IPAddress IPv6DnsAddressAlt
        {
            get { return this._ipv6DnsAddressAlt; }
            set { this._ipv6DnsAddressAlt = value; }
        }

        /// <summary>
        /// Gets or sets the metric of the IPv4 stack for this interface.
        /// </summary>
        public int IPv4InterfaceMetric
        {
            get { return this._ipv4InterfaceMetric; }
            set { this._ipv4InterfaceMetric = value; }
        }

        /// <summary>
        /// Gets or sets the metric of the IPv6 stack for this interface.
        /// </summary>
        public int IPv6InterfaceMetric
        {
            get { return this._ipv6InterfaceMetric; }
            set { this._ipv6InterfaceMetric = value; }
        }
#endif

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new dial-up entry.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        /// <param name="phoneNumber">The phone number to dial.</param>
        /// <param name="device">Required. An <see cref="DotRas.RasDevice"/> to use for connecting.</param>
        /// <returns>A new <see cref="DotRas.RasEntry"/> object.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="name"/> or <paramref name="serverAddress"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="device"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public static RasEntry CreateDialUpEntry(string name, string phoneNumber, RasDevice device)
        {
            if (string.IsNullOrEmpty(name))
            {
                ThrowHelper.ThrowArgumentException("name", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                ThrowHelper.ThrowArgumentException("phoneNumber", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (device == null)
            {
                ThrowHelper.ThrowArgumentNullException("device");
            }

            RasEntry entry = new RasEntry(name);

            entry.Device = device;
            entry.DialMode = RasDialMode.None;
            entry.EntryType = RasEntryType.Phone;
            entry.FramingProtocol = RasFramingProtocol.Ppp;
            entry.IdleDisconnectSeconds = RasIdleDisconnectTimeout.Default;
            entry.NetworkProtocols = RasNetworkProtocols.IP;

#if (WINXP || WINXPSP2 || WIN2K8)
            entry.RedialCount = 3;
            entry.RedialPause = 60;
#endif
#if (WIN2K8)
            entry.NetworkProtocols |= RasNetworkProtocols.IPv6;
#endif

            entry.Options = RasEntryOptions.None;
            entry.PhoneNumber = phoneNumber;
            entry.VpnStrategy = RasVpnStrategy.Default;

            return entry;
        }

        /// <summary>
        /// Creates a new virtual private network (VPN) entry.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        /// <param name="serverAddress">The server address to connect to.</param>
        /// <param name="strategy">The virtual private network (VPN) strategy of the connection.</param>
        /// <param name="device">Required. An <see cref="DotRas.RasDevice"/> to use for connecting.</param>
        /// <returns>A new <see cref="DotRas.RasEntry"/> object.</returns>
        /// <remarks>The device for this connection is typically a WAN Miniport (L2TP) or WAN Miniport (PPTP).</remarks>
        /// <exception cref="System.ArgumentException"><paramref name="name"/> or <paramref name="serverAddress"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="device"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public static RasEntry CreateVpnEntry(string name, string serverAddress, RasVpnStrategy strategy, RasDevice device)
        {
            if (string.IsNullOrEmpty(name))
            {
                ThrowHelper.ThrowArgumentException("name", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (string.IsNullOrEmpty(serverAddress))
            {
                ThrowHelper.ThrowArgumentException("serverAddress", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (device == null)
            {
                ThrowHelper.ThrowArgumentNullException("device");
            }

            RasEntry entry = new RasEntry(name);

            entry.Device = device;
            entry.EncryptionType = RasEncryptionType.Require;
            entry.EntryType = RasEntryType.Vpn;
            entry.FramingProtocol = RasFramingProtocol.Ppp;
            entry.NetworkProtocols = RasNetworkProtocols.IP;
            entry.Options = RasEntryOptions.RemoteDefaultGateway | RasEntryOptions.ModemLights | RasEntryOptions.RequireEncryptedPassword | RasEntryOptions.PreviewUserPassword | RasEntryOptions.PreviewDomain | RasEntryOptions.ShowDialingProgress;

#if (WINXP || WINXPSP2 || WIN2K8)
            entry.RedialCount = 3;
            entry.RedialPause = 60;
            entry.ExtendedOptions = RasEntryExtendedOptions.DoNotNegotiateMultilink | RasEntryExtendedOptions.ReconnectIfDropped;
#endif
#if (WIN2K8)
            entry.ExtendedOptions |= RasEntryExtendedOptions.IPv6RemoteDefaultGateway | RasEntryExtendedOptions.UseTypicalSettings;
            entry.NetworkProtocols |= RasNetworkProtocols.IPv6;
#endif
            entry.PhoneNumber = serverAddress;
            entry.VpnStrategy = strategy;

            return entry;
        }

        /// <summary>
        /// Creates a copy of this <see cref="RasEntry"/>.
        /// </summary>
        /// <returns>A new <see cref="DotRas.RasEntry"/> object.</returns>
        public object Clone()
        {
            RasEntry retval = new RasEntry(this.Name);

            if (this.AlternatePhoneNumbers != null && this.AlternatePhoneNumbers.Count > 0)
            {
                retval.AlternatePhoneNumbers = new Collection<string>();
                foreach (string value in this.AlternatePhoneNumbers)
                {
                    retval.AlternatePhoneNumbers.Add(value);
                }
            }

            retval.AreaCode = this.AreaCode;

#pragma warning disable 0618
            retval.AutoDialDll = this.AutoDialDll;
            retval.AutoDialFunc = this.AutoDialFunc;
#pragma warning restore 0618

            retval.Channels = this.Channels;
            retval.CountryCode = this.CountryCode;
            retval.CountryId = this.CountryId;
            retval.CustomAuthKey = this.CustomAuthKey;
            retval.CustomDialDll = this.CustomDialDll;
            retval.Device = this.Device;
            retval.DialExtraPercent = this.DialExtraPercent;
            retval.DialExtraSampleSeconds = this.DialExtraSampleSeconds;
            retval.DialMode = this.DialMode;
            retval.DnsAddress = this.DnsAddress;
            retval.DnsAddressAlt = this.DnsAddressAlt;
            retval.EncryptionType = this.EncryptionType;
            retval.EntryType = this.EntryType;
            retval.FrameSize = this.FrameSize;
            retval.FramingProtocol = this.FramingProtocol;
            retval.HangUpExtraPercent = this.HangUpExtraPercent;
            retval.HangUpExtraSampleSeconds = this.HangUpExtraSampleSeconds;
            retval.IdleDisconnectSeconds = this.IdleDisconnectSeconds;
            retval.IPAddress = this.IPAddress;
            retval.NetworkProtocols = this.NetworkProtocols;
            retval.Options = this.Options;
            retval.PhoneNumber = this.PhoneNumber;
            retval.Reserved1 = this.Reserved1;
            retval.Reserved2 = this.Reserved2;
            retval.Script = this.Script;

            if (this.SubEntries != null && this.SubEntries.Count > 0)
            {
                foreach (RasSubEntry subEntry in this.SubEntries)
                {
                    retval.SubEntries.Add((RasSubEntry)subEntry.Clone());
                }
            }

            retval.VpnStrategy = this.VpnStrategy;
            retval.WinsAddress = this.WinsAddress;
            retval.WinsAddressAlt = this.WinsAddressAlt;
            retval.X25Address = this.X25Address;
            retval.X25Facilities = this.X25Facilities;
            retval.X25PadType = this.X25PadType;
            retval.X25UserData = this.X25UserData;

#if (WINXP || WINXPSP2 || WIN2K8)
            retval.ExtendedOptions = this.ExtendedOptions;
            retval.ReservedOptions = this.ReservedOptions;
            retval.DnsSuffix = this.DnsSuffix;
            retval.TcpWindowSize = this.TcpWindowSize;
            retval.PrerequisitePhoneBook = this.PrerequisitePhoneBook;
            retval.PrerequisiteEntryName = this.PrerequisiteEntryName;
            retval.RedialCount = this.RedialCount;
            retval.RedialPause = this.RedialPause;
#endif
#if (WIN2K8)
            retval.IPv6DnsAddress = this.IPv6DnsAddress;
            retval.IPv6DnsAddressAlt = this.IPv6DnsAddressAlt;
            retval.IPv4InterfaceMetric = this.IPv4InterfaceMetric;
            retval.IPv6InterfaceMetric = this.IPv6InterfaceMetric;
#endif

            return retval;
        }

        /// <summary>
        /// Clears the stored credentials for the entry.
        /// </summary>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        public bool ClearCredentials()
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.userName = string.Empty;
            credentials.password = string.Empty;
            credentials.domain = string.Empty;

            credentials.options = NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain;

            return RasHelper.SetCredentials(this.Owner.Path, this.Name, credentials, true);
        }

#if (WINXP || WINXPSP2 || WIN2K8)

        /// <summary>
        /// Clears the stored credentials for the entry.
        /// </summary>
        /// <param name="key">The pre-shared key whose value to clear.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        public bool ClearCredentials(RasPreSharedKey key)
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.password = string.Empty;

            switch (key)
            {
                case RasPreSharedKey.Client:
                    credentials.options = NativeMethods.RASCM.PreSharedKey;
                    break;

                case RasPreSharedKey.Ddm:
                    credentials.options = NativeMethods.RASCM.DdmPreSharedKey;
                    break;

                case RasPreSharedKey.Server:
                    credentials.options = NativeMethods.RASCM.ServerPreSharedKey;
                    break;
            }

            return RasHelper.SetCredentials(this.Owner.Path, this.Name, credentials, true);
        }

#endif

        /// <summary>
        /// Retrieves the credentials for the entry.
        /// </summary>
        /// <returns>The credentials stored in the entry, otherwise a null reference (<b>Nothing</b> in Visual Basic) if the credentials did not exist.</returns>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public NetworkCredential GetCredentials()
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            return RasHelper.GetCredentials(this.Owner.Path, this.Name, NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
        }

#if (WINXP || WINXPSP2 || WIN2K8)

        /// <summary>
        /// Retrieves the credentials for the entry.
        /// </summary>
        /// <param name="key">The pre-shared key to retrieve.</param>
        /// <returns>The credentials stored in the entry, otherwise a null reference (<b>Nothing</b> in Visual Basic) if the credentials did not exist.</returns>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public NetworkCredential GetCredentials(RasPreSharedKey key)
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            NativeMethods.RASCM options = NativeMethods.RASCM.None;
            switch (key)
            {
                case RasPreSharedKey.Client:
                    options = NativeMethods.RASCM.PreSharedKey;
                    break;

                case RasPreSharedKey.Server:
                    options = NativeMethods.RASCM.ServerPreSharedKey;
                    break;

                case RasPreSharedKey.Ddm:
                    options = NativeMethods.RASCM.DdmPreSharedKey;
                    break;
            }

            return RasHelper.GetCredentials(this.Owner.Path, this.Name, options);
        }

#endif

        /// <summary>
        /// Renames the entry.
        /// </summary>
        /// <param name="newEntryName">Required. The new name of the entry.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="newEntryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic) or <paramref name="newEntryName"/> is invalid.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The current user does not have permissions to the phone book specified.</exception>
        public bool Rename(string newEntryName)
        {
            if (string.IsNullOrEmpty(newEntryName))
            {
                ThrowHelper.ThrowArgumentException("newEntryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            bool retval = false;

            if (this.Owner == null)
            {
                this.Name = newEntryName;
                retval = true;
            }
            else
            {
                if (!RasHelper.IsValidEntryName(this.Owner, newEntryName))
                {
                    ThrowHelper.ThrowArgumentException("newEntryName", Resources.Argument_InvalidEntryName, newEntryName);
                }

                if (RasHelper.RenameEntry(this.Owner, this.Name, newEntryName))
                {
                    this.Owner.Entries.ChangeKey(this, newEntryName);
                    this.Name = newEntryName;

                    retval = true;
                }
            }

            return retval;
        }

        /// <summary>
        /// Removes the entry from the phone book.
        /// </summary>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        public bool Remove()
        {
            bool retval = false;

            if (this.Owner != null)
            {
                retval = this.Owner.Entries.Remove(this);
            }

            return retval;
        }

        /// <summary>
        /// Updates the entry.
        /// </summary>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        public bool Update()
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            return RasHelper.SetEntryProperties(this.Owner, this);
        }

        /// <summary>
        /// Updates the user credentials for the entry.
        /// </summary>
        /// <param name="value">An <see cref="System.Net.NetworkCredential"/> object containing user credentials.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public bool UpdateCredentials(NetworkCredential value)
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            if (value == null)
            {
                ThrowHelper.ThrowArgumentNullException("value");
            }

            return this.InternalSetCredentials(value, false);
        }

#if (WINXP || WINXPSP2 || WIN2K8)       

        /// <summary>
        /// Updates the user credentials for the entry.
        /// </summary>
        /// <param name="value">An <see cref="System.Net.NetworkCredential"/> object containing user credentials.</param>
        /// <param name="storeCredentialsForAllUsers"><b>true</b> if the credentials should be stored for all users, otherwise <b>false</b>.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.InvalidOperationException">The entry is not associated with a phone book.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public bool UpdateCredentials(NetworkCredential value, bool storeCredentialsForAllUsers)
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            if (value == null)
            {
                ThrowHelper.ThrowArgumentNullException("value");
            }

            return this.InternalSetCredentials(value, storeCredentialsForAllUsers);
        }

        /// <summary>
        /// Updates the user credentials for the entry.
        /// </summary>
        /// <param name="key">The pre-shared key to update.</param>
        /// <param name="value">The value to set.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        public bool UpdateCredentials(RasPreSharedKey key, string value)
        {
            if (this.Owner == null)
            {
                ThrowHelper.ThrowInvalidOperationException(Resources.Exception_EntryNotInPhoneBook);
            }

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.password = value;

            switch (key)
            {
                case RasPreSharedKey.Client:
                    credentials.options = NativeMethods.RASCM.PreSharedKey;
                    break;

                case RasPreSharedKey.Server:
                    credentials.options = NativeMethods.RASCM.ServerPreSharedKey;
                    break;

                case RasPreSharedKey.Ddm:
                    credentials.options = NativeMethods.RASCM.DdmPreSharedKey;
                    break;
            }

            return RasHelper.SetCredentials(this.Owner.Path, this.Name, credentials, false);
        }

#endif

        /// <summary>
        /// Updates the user credentials for the entry.
        /// </summary>
        /// <param name="value">An <see cref="System.Net.NetworkCredential"/> object containing user credentials.</param>
        /// <param name="storeCredentialsForAllUsers"><b>true</b> if the credentials should be stored for all users, otherwise <b>false</b>.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        private bool InternalSetCredentials(NetworkCredential value, bool storeCredentialsForAllUsers)
        {
            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.userName = value.UserName;
            credentials.password = value.Password;
            credentials.domain = value.Domain;
            credentials.options = NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain;

#if (WINXP || WINXPSP2 || WIN2K8)
            if (storeCredentialsForAllUsers)
            {
                credentials.options |= NativeMethods.RASCM.DefaultCredentials;
            }
#endif

            return RasHelper.SetCredentials(this.Owner.Path, this.Name, credentials, false);
        }

        #endregion
    }
}