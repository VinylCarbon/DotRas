//--------------------------------------------------------------------------
// <copyright file="RasEntryTest.cs" company="Jeff Winn">
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

namespace DotRas.Tests.Unit
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Net;
    using DotRas;
    using DotRas.Tests.Unit.Mocks.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// This is a test class for <see cref="DotRas.RasEntry"/> and is intended to contain all associated unit tests.
    /// </summary>
    [Isolated]
    [TestClass]
    public class RasEntryTest : UnitTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEntryTest"/> class.
        /// </summary>
        public RasEntryTest()
        {
        }

        #endregion

        #region Methods

        #region Constructor Tests

        /// <summary>
        /// Tests the RasEntry constructor to ensure an object is created successfully.
        /// </summary>
        [TestMethod]
        public void RasEntryConstructorTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);

            Assert.AreEqual(name, target.Name);
        }

        /// <summary>
        /// Tests the RasEntry constructor to ensure an argument exception is thrown when an empty entry name is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RasEntryConstructorArgumentExceptionTest()
        {
            string name = string.Empty;

            RasEntry target = new RasEntry(name);
        }

        /// <summary>
        /// Tests the RasEntry constructor to ensure an ArgumentException is thrown when the entry name is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RasEntryNullEntryNameConstructorTest()
        {
            string name = null;

            RasEntry target = new RasEntry(name);
        }

        #endregion

        #region Property Tests

        /// <summary>
        /// Tests the X25UserData property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void X25UserDataTest()
        {
            string name = "Test Entry";
            string expected = "12345";

            RasEntry target = new RasEntry(name);
            target.X25UserData = expected;

            string actual = target.X25UserData;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the X25PadType property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void X25PadTypeTest()
        {
            string name = "Test Entry";
            string expected = "12345";

            RasEntry target = new RasEntry(name);
            target.X25PadType = expected;

            string actual = target.X25PadType;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the X25Facilities property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void X25FacilitiesTest()
        {
            string name = "Test Entry";
            string expected = "12345";

            RasEntry target = new RasEntry(name);
            target.X25Facilities = expected;

            string actual = target.X25Facilities;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the X25Address property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void X25AddressTest()
        {
            string name = "Test Entry";
            string expected = "12345";

            RasEntry target = new RasEntry(name);
            target.X25Address = expected;

            string actual = target.X25Address;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the WinsAddressAlt property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void WinsAddressAltTest()
        {
            string name = "Test Entry";
            IPAddress expected = IPAddress.Loopback;

            RasEntry target = new RasEntry(name);
            target.WinsAddressAlt = expected;

            IPAddress actual = target.WinsAddressAlt;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the WinsAddress property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void WinsAddressTest()
        {
            string name = "Test Entry";
            IPAddress expected = IPAddress.Loopback;

            RasEntry target = new RasEntry(name);
            target.WinsAddress = expected;

            IPAddress actual = target.WinsAddress;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the VpnStrategy property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void VpnStrategyTest()
        {
            string name = "Test Entry";
            RasVpnStrategy expected = RasVpnStrategy.L2tpFirst;

            RasEntry target = new RasEntry(name);
            target.VpnStrategy = expected;

            RasVpnStrategy actual = target.VpnStrategy;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the SubEntries property to ensure the property returns an empty collection.
        /// </summary>
        [TestMethod]
        public void SubEntriesTest()
        {
            string name = "Test Entry";
            int expected = 0;

            RasEntry target = new RasEntry(name);
            int actual = target.SubEntries.Count;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Script property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void ScriptTest()
        {
            string name = "Test Entry";
            string expected = string.Empty;

            RasEntry target = new RasEntry(name);
            target.Script = expected;

            string actual = target.Script;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Reserved2 property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void Reserved2Test()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.Reserved2 = expected;

            int actual = target.Reserved2;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Reserved1 property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void Reserved1Test()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.Reserved1 = expected;

            int actual = target.Reserved1;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the PhoneNumber property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void PhoneNumberTest()
        {
            string name = "Test Entry";
            string expected = "127.0.0.1";

            RasEntry target = new RasEntry(name);
            target.PhoneNumber = expected;

            string actual = expected;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Owner property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void OwnerTest()
        {
            string name = "Test Entry";
            RasPhoneBook expected = new RasPhoneBook();

            RasEntry target = new RasEntry(name);
            target.Owner = expected;

            RasPhoneBook actual = target.Owner;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Owner property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void OptionsTest()
        {
            string name = "Test Entry";
            RasEntryOptions expected = RasEntryOptions.DisableLcpExtensions | RasEntryOptions.IPHeaderCompression;

            RasEntry target = new RasEntry(name);
            target.Options = expected;

            RasEntryOptions actual = target.Options;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the NetworkProtocols property to ensure the value returned is the same as what was set.
        /// </summary>
        [TestMethod]
        public void NetworkProtocolsTest()
        {
            string name = "Test Entry";
            RasNetworkProtocols expected = RasNetworkProtocols.IP;

            RasEntry target = new RasEntry(name);
            target.NetworkProtocols = expected;

            RasNetworkProtocols actual = target.NetworkProtocols;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Name property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            string expected = "Test Entry";

            RasEntry target = new RasEntry(expected);

            string actual = target.Name;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the IPAddress property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void IPAddressTest()
        {
            string name = "Test Entry";
            IPAddress expected = IPAddress.Loopback;

            RasEntry target = new RasEntry(name);
            target.IPAddress = expected;

            IPAddress actual = target.IPAddress;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the IdleDisconnectSeconds property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void IdleDisconnectSecondsTest()
        {
            string name = "Test Entry";
            int expected = RasIdleDisconnectTimeout.Disabled;

            RasEntry target = new RasEntry(name);
            target.IdleDisconnectSeconds = expected;

            int actual = target.IdleDisconnectSeconds;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Id property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void IdTest()
        {
            string name = "Test Entry";
            Guid expected = Guid.NewGuid();

            RasEntry target = new RasEntry(name);
            target.Id = expected;

            Guid actual = target.Id;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the HangUpExtraSampleSeconds property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void HangUpExtraSampleSecondsTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.HangUpExtraSampleSeconds = expected;

            int actual = target.HangUpExtraSampleSeconds;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the HangUpExtraPercent property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void HangUpExtraPercentTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.HangUpExtraPercent = expected;

            int actual = target.HangUpExtraPercent;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the FramingProtocol property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void FramingProtocolTest()
        {
            string name = "Test Entry";
            RasFramingProtocol expected = RasFramingProtocol.Ppp;

            RasEntry target = new RasEntry(name);
            target.FramingProtocol = expected;

            RasFramingProtocol actual = target.FramingProtocol;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the FramingProtocol property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void FrameSizeTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.FrameSize = expected;

            int actual = target.FrameSize;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the EntryType property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void EntryTypeTest()
        {
            string name = "Test Entry";
            RasEntryType expected = RasEntryType.Vpn;

            RasEntry target = new RasEntry(name);
            target.EntryType = expected;

            RasEntryType actual = target.EntryType;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the EncryptionType property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void EncryptionTypeTest()
        {
            string name = "Test Entry";
            RasEncryptionType expected = RasEncryptionType.Require;

            RasEntry target = new RasEntry(name);
            target.EncryptionType = expected;

            RasEncryptionType actual = target.EncryptionType;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the DnsAddressAlt property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void DnsAddressAltTest()
        {
            string name = "Test Entry";
            IPAddress expected = IPAddress.Loopback;

            RasEntry target = new RasEntry(name);
            target.DnsAddressAlt = expected;

            IPAddress actual = target.DnsAddressAlt;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the DnsAddress property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void DnsAddressTest()
        {
            string name = "Test Entry";
            IPAddress expected = IPAddress.Loopback;

            RasEntry target = new RasEntry(name);
            target.DnsAddress = expected;

            IPAddress actual = target.DnsAddress;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the DialMode property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void DialModeTest()
        {
            string name = "Test Entry";
            RasDialMode expected = RasDialMode.DialAsNeeded;

            RasEntry target = new RasEntry(name);
            target.DialMode = expected;

            RasDialMode actual = target.DialMode;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the DialExtraSampleSeconds property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void DialExtraSampleSecondsTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.DialExtraSampleSeconds = expected;

            int actual = target.DialExtraSampleSeconds;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the DialExtraPercent property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void DialExtraPercentTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.DialExtraPercent = expected;

            int actual = target.DialExtraPercent;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Device property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void DeviceTest()
        {
            string name = "Test Entry";
            RasDevice expected = RasDevice.Create(name, RasDeviceType.Vpn);

            RasEntry target = new RasEntry(name);
            target.Device = expected;

            RasDevice actual = target.Device;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CustomDialDll property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void CustomDialDllTest()
        {
            string name = "Test Entry";
            string expected = "Test.dll";

            RasEntry target = new RasEntry(name);
            target.CustomDialDll = expected;

            string actual = target.CustomDialDll;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CustomAuthKey property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void CustomAuthKeyTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.CustomAuthKey = expected;

            int actual = target.CustomAuthKey;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CountryId property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void CountryIdTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.CountryId = expected;

            int actual = target.CountryId;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CountryCode property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void CountryCodeTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.CountryCode = expected;

            int actual = target.CountryCode;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Channels property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void ChannelsTest()
        {
            string name = "Test Entry";
            int expected = int.MaxValue;

            RasEntry target = new RasEntry(name);
            target.Channels = expected;

            int actual = target.Channels;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AutoDialFunc property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void AutoDialFuncTest()
        {
            string name = "Test Entry";
            string expected = "TestFunc";

#pragma warning disable 0618
            RasEntry target = new RasEntry(name);
            target.AutoDialFunc = expected;

            string actual = target.AutoDialFunc;
#pragma warning restore 0618

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AutoDialDll property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void AutoDialDllTest()
        {
            string name = "Test Entry";
            string expected = "Test.dll";

#pragma warning disable 0618
            RasEntry target = new RasEntry(name);
            target.AutoDialDll = expected;

            string actual = target.AutoDialDll;
#pragma warning restore 0618

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AreaCode property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void AreaCodeTest()
        {
            string name = "Test Entry";
            string expected = "123";

            RasEntry target = new RasEntry(name);
            target.AreaCode = expected;

            string actual = target.AreaCode;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AlternatePhoneNumbers property to ensure it matches the value passed into the constructor.
        /// </summary>
        [TestMethod]
        public void AlternatePhoneNumbersTest()
        {
            string name = "Test Entry";
            Collection<string> expected = new Collection<string>();
            expected.Add("555-555-1234");
            expected.Add("555-555-2345");

            RasEntry target = new RasEntry(name);
            target.AlternatePhoneNumbers = expected;

            Collection<string> actual = target.AlternatePhoneNumbers;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method Tests

        /// <summary>
        /// Tests the UpdateCredentials method to ensure an InvalidOperationException is thrown when the entry does not belong to a phone book.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateCredentialsInvalidOperationExceptionTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            target.UpdateCredentials(new NetworkCredential("Test", "User"));
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure the credentials are updated successfully.
        /// </summary>
        [TestMethod]
        public void UpdateCredentialsTest()
        {
            string name = "Test Entry";
            NetworkCredential credentials = new NetworkCredential("Test", "User");
            
            bool expected = true;
            
            RasEntry target = Isolate.Fake.Instance<RasEntry>(Members.CallOriginal, ConstructorWillBe.Called, new object[] { name });
            target.Owner = new RasPhoneBook();

            Isolate.NonPublic.WhenCalled(target, "InternalSetCredentials").WillReturn(true);

            bool actual = target.UpdateCredentials(credentials);

            Assert.AreEqual(expected, actual);

            Isolate.Verify.NonPublic.WasCalled(target, "InternalSetCredentials").WithArguments(credentials, new object[] { false });
        }

#if (WINXP || WINXPSP2 || WIN2K8)

        /// <summary>
        /// Tests the UpdateCredentials method to ensure the client pre-shared key is updated successfully.
        /// </summary>
        [TestMethod]
        public void UpdateCredentials2ClientPreSharedKeyTest()
        {
            string name = "Test Entry";

            bool expected = true;

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.UpdateCredentials(RasPreSharedKey.Client, "value");

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure the DDM pre-shared key is updated successfully.
        /// </summary>
        [TestMethod]
        public void UpdateCredentials2DdmPreSharedKeyTest()
        {
            string name = "Test Entry";

            bool expected = true;

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.UpdateCredentials(RasPreSharedKey.Ddm, "value");

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure the client pre-shared key is updated successfully.
        /// </summary>
        [TestMethod]
        public void UpdateCredentials2ServerPreSharedKeyTest()
        {
            string name = "Test Entry";

            bool expected = true;

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.UpdateCredentials(RasPreSharedKey.Server, "value");

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure the credentials for all users are updated successfully.
        /// </summary>
        [TestMethod]
        public void UpdateCredentials1Test()
        {
            string name = "Test Entry";
            NetworkCredential credentials = new NetworkCredential("Test", "User");

            bool expected = true;

            RasEntry target = Isolate.Fake.Instance<RasEntry>(Members.CallOriginal, ConstructorWillBe.Called, new object[] { name });
            target.Owner = new RasPhoneBook();

            Isolate.NonPublic.WhenCalled(target, "InternalSetCredentials").WillReturn(true);

            bool actual = target.UpdateCredentials(credentials, true);

            Assert.AreEqual(expected, actual);

            Isolate.Verify.NonPublic.WasCalled(target, "InternalSetCredentials").WithArguments(credentials, new object[] { true });
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure an exception is thrown when the owner is a null reference.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateCredentials1NullOwnerTest()
        {
            string name = "Test Entry";
            NetworkCredential credentials = new NetworkCredential("Test", "User");

            RasEntry target = new RasEntry(name);
            target.UpdateCredentials(credentials, true);
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure an exception is thrown when the credentials are a null reference.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateCredentials1NullCredentialsTest()
        {
            string name = "Test Entry";
            NetworkCredential credentials = null;

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            target.UpdateCredentials(credentials, true);
        }
#endif

        /// <summary>
        /// Tests the InternalSetCredentials method to ensure it will properly update user credentials.
        /// </summary>
        [TestMethod]
        public void InternalSetCredentialsTest()
        {
            string name = "Test Entry";
            NetworkCredential credentials = new NetworkCredential("Test", "User");

            bool expected = true;

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, new NativeMethods.RASCREDENTIALS(), false)).WillReturn(true);

            RasEntry_Accessor target = new RasEntry_Accessor(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.InternalSetCredentials(credentials, false);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the InternalSetCredentials method to ensure it will properly update credentials for all users.
        /// </summary>
        [TestMethod]
        public void InternalSetCredentialsForAllUsersTest()
        {
            string name = "Test Entry";
            NetworkCredential credentials = new NetworkCredential("Test", "User");

            bool expected = true;

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, new NativeMethods.RASCREDENTIALS(), false)).WillReturn(true);

            RasEntry_Accessor target = new RasEntry_Accessor(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.InternalSetCredentials(credentials, true);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure it throws a null reference exception when null credentials are passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateCredentialsWithNullCredentialsArgumentNullExceptionTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            target.UpdateCredentials(null);
        }

        /// <summary>
        /// Tests the Update method to ensure it properly updates the entry.
        /// </summary>
        [TestMethod]
        public void UpdateTest()
        {
            string name = "Test Entry";
            RasPhoneBook owner = new RasPhoneBook();

            bool expected = true;

            RasEntry target = new RasEntry(name);
            target.Owner = owner;

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetEntryProperties(null, null)).WillReturn(true);

            bool actual = target.Update();

            Assert.AreEqual(expected, actual);

            Isolate.Verify.WasCalledWithExactArguments(() => RasHelper.SetEntryProperties(owner, target));
        }

        /// <summary>
        /// Tests the update method to ensure an exception is thrown when the entry does not belong to a phone book.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateInvalidOperationExceptionTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            target.Update();
        }

        /// <summary>
        /// Tests the Rename method to ensure an exception is thrown when the new entry name is a null reference.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RenameNullEntryNameExceptionTest()
        {
            string name = "Test Entry";
            string newEntryName = null;

            RasEntry target = new RasEntry(name);
            target.Rename(newEntryName);
        }

        /// <summary>
        /// Tests the Rename method to ensure an exception is thrown when the entry name is an empty string.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RenameEmptyEntryNameExceptionTest()
        {
            string name = "Test Entry";
            string newEntryName = string.Empty;

            RasEntry target = new RasEntry(name);
            target.Rename(newEntryName);
        }

        /// <summary>
        /// Tests the Rename method to ensure an entry not in a phone book can be renamed.
        /// </summary>
        [TestMethod]
        public void RenameEntryNotInPhoneBookTest()
        {
            string name = "Test Entry";
            string newEntryName = "New Entry";
            bool expected = true;

            RasEntry target = new RasEntry(name);
            bool actual = target.Rename(newEntryName);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(newEntryName, target.Name);
        }

        /////// <summary>
        /////// Tests the Rename method to ensure it will rename an entry and update the name property.
        /////// </summary>
        ////[TestMethod]
        ////public void RenameTest()
        ////{
        ////    string name = "Test Entry";
        ////    string newEntryName = "New Entry";
        ////    RasPhoneBook owner = new RasPhoneBook();

        ////    bool expected = true;

        ////    Isolate.Fake.StaticMethods<RasHelper>();
        ////    Isolate.WhenCalled(() => RasHelper.IsValidEntryName(null, null, null)).WillReturn(true);
        ////    Isolate.WhenCalled(() => RasHelper.RenameEntry(null, null, null)).WillReturn(true);

        ////    RasEntry target = new RasEntry(name);
        ////    target.Owner = owner;

        ////    Isolate.WhenCalled(() => target.Owner.Entries.ChangeKey(target, newEntryName)).IgnoreCall();

        ////    bool actual = target.Rename(newEntryName);

        ////    Assert.AreEqual(expected, actual);
        ////    Assert.AreEqual(newEntryName, target.Name);
        ////}

        /////// <summary>
        /////// Tests the Rename method with an invalid entry name.
        /////// </summary>
        ////[TestMethod]
        ////[ExpectedException(typeof(ArgumentException))]
        ////public void RenameInvalidEntryNameTest()
        ////{
        ////    string name = "Test Entry";
        ////    string newEntryName = ".\\Test*!";
        ////    RasPhoneBook owner = new RasPhoneBook();

        ////    bool expected = false;

        ////    try
        ////    {
        ////        Isolate.Fake.StaticMethods<RasHelper>();
        ////        Isolate.WhenCalled(() => RasHelper.IsValidEntryName(null, null)).WillReturn(false);

        ////        RasEntry target = new RasEntry(name);
        ////        target.Owner = owner;

        ////        bool actual = target.Rename(newEntryName);

        ////        Assert.AreEqual(expected, actual);
        ////    }
        ////    finally
        ////    {
        ////        Isolate.Verify.WasCalledWithExactArguments(() => RasHelper.IsValidEntryName(null, newEntryName));
        ////    }
        ////}

        /// <summary>
        /// Tests the Remove method to ensure it removes an entry.
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            string name = "Test Entry";
            RasPhoneBook owner = new RasPhoneBook();
            bool expected = true;

            RasEntry target = new RasEntry(name);
            target.Owner = owner;

            Isolate.WhenCalled(() => target.Owner.Entries.Remove(null)).WillReturn(true);

            bool actual = target.Remove();

            Assert.AreEqual(expected, actual);

            Isolate.Verify.WasCalledWithExactArguments(() => target.Owner.Entries.Remove(target));
        }

        /// <summary>
        /// Tests the GetCredentials method to ensure the credentials are returned.
        /// </summary>
        [TestMethod]
        public void GetCredentialsTest()
        {
            string name = "Test Entry";

            NetworkCredential expected = new NetworkCredential("Test", "User");
            
            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.GetCredentials(null, null, NativeMethods.RASCM.None)).WillReturn(expected);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            NetworkCredential actual = target.GetCredentials();

            Assert.AreEqual(expected, actual);
        }

#if (WINXP || WINXPSP2 || WIN2K8)

        /// <summary>
        /// Tests the TryGetCredentials method to ensure an invalid operation exception is thrown when the phone book is not set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetCredentials1InvalidOperationExceptionTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            NetworkCredential actual = target.GetCredentials(RasPreSharedKey.Client);
        }

        /// <summary>
        /// Tests the TryGetCredentials method to ensure the expected pre-shared key is returned.
        /// </summary>
        [TestMethod]
        public void GetCredentials1ClientPreSharedKeyTest()
        {
            string name = "Test Entry";

            NetworkCredential expected = new NetworkCredential(string.Empty, "********", string.Empty);
          
            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.GetCredentials(null, null, NativeMethods.RASCM.None)).WillReturn(expected);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            NetworkCredential actual = target.GetCredentials(RasPreSharedKey.Client);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the TryGetCredentials method to ensure the expected pre-shared key is returned.
        /// </summary>
        [TestMethod]
        public void GetCredentials1DdmPreSharedKeyTest()
        {
            string name = "Test Entry";

            NetworkCredential expected = new NetworkCredential(string.Empty, "********", string.Empty);            

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.GetCredentials(null, null, NativeMethods.RASCM.None)).WillReturn(expected);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            NetworkCredential actual = target.GetCredentials(RasPreSharedKey.Ddm);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the TryGetCredentials method to ensure the expected pre-shared key is returned.
        /// </summary>
        [TestMethod]
        public void GetCredentials1ServerPreSharedKeyTest()
        {
            string name = "Test Entry";

            NetworkCredential expected = new NetworkCredential(string.Empty, "********", string.Empty);            

            Isolate.WhenCalled(() => RasHelper.GetCredentials(null, null, NativeMethods.RASCM.None)).WillReturn(expected);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            NetworkCredential actual = target.GetCredentials(RasPreSharedKey.Server);

            Assert.AreEqual(expected, actual);
        }

#endif

        /// <summary>
        /// Tests the GetCredentials method to ensure an InvalidOperationException is thrown when the entry is not in a phone book.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetCredentialsInvalidOperationExceptionTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            target.GetCredentials();
        }

        /// <summary>
        /// Tests the CreateVpnEntry method to ensure an ArgumentException is thrown when the entry name is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVpnEntryWithEmptyNameArgumentExceptionTest()
        {
            string name = string.Empty;
            string serverAddress = "127.0.0.1";
            RasVpnStrategy strategy = RasVpnStrategy.Default;
            RasDevice device = RasDevice.Create("WAN Miniport (PPTP)", RasDeviceType.Vpn);

            RasEntry.CreateVpnEntry(name, serverAddress, strategy, device);
        }

        /// <summary>
        /// Tests the CreateVpnEntry method to ensure an ArgumentException is thrown when the server address is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVpnEntryWithEmptyServerAddressArgumentExceptionTest()
        {
            string name = "Test Entry";
            string serverAddress = string.Empty;
            RasVpnStrategy strategy = RasVpnStrategy.Default;
            RasDevice device = RasDevice.Create("WAN Miniport (PPTP)", RasDeviceType.Vpn);

            RasEntry.CreateVpnEntry(name, serverAddress, strategy, device);
        }

        /// <summary>
        /// Tests the CreateVpnEntry method to ensure an ArgumentNullException is thrown when device is a null reference.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateVpnEntryWithNullDeviceArgumentExceptionTest()
        {
            string name = "Test Entry";
            string serverAddress = "127.0.0.1";
            RasVpnStrategy strategy = RasVpnStrategy.Default;
            RasDevice device = null;

            RasEntry.CreateVpnEntry(name, serverAddress, strategy, device);
        }

        /// <summary>
        /// Tests the CreateVpnEntry method to ensure a new default VPN entry is created.
        /// </summary>
        [TestMethod]
        public void CreateVpnEntryTest()
        {
            string name = "Test Entry";
            string serverAddress = "127.0.0.1";
            RasVpnStrategy strategy = RasVpnStrategy.L2tpFirst;
            RasDevice device = RasDevice.Create("WAN Miniport (PPTP)", RasDeviceType.Vpn);

            RasEntry expected = new RasEntry(name);
            expected.Device = device;
            expected.EncryptionType = RasEncryptionType.Require;
            expected.EntryType = RasEntryType.Vpn;
            expected.FramingProtocol = RasFramingProtocol.Ppp;
            expected.NetworkProtocols = RasNetworkProtocols.IP;
            expected.Options = RasEntryOptions.RemoteDefaultGateway | RasEntryOptions.ModemLights | RasEntryOptions.RequireEncryptedPassword | RasEntryOptions.PreviewUserPassword | RasEntryOptions.PreviewDomain | RasEntryOptions.ShowDialingProgress;

#if (WINXP || WINXPSP2 || WIN2K8)
            expected.RedialCount = 3;
            expected.RedialPause = 60;
            expected.ExtendedOptions = RasEntryExtendedOptions.DoNotNegotiateMultilink | RasEntryExtendedOptions.ReconnectIfDropped;
#endif
#if (WIN2K8)
            expected.ExtendedOptions |= RasEntryExtendedOptions.IPv6RemoteDefaultGateway | RasEntryExtendedOptions.UseTypicalSettings;
            expected.NetworkProtocols |= RasNetworkProtocols.IPv6;
#endif

            expected.PhoneNumber = serverAddress;
            expected.VpnStrategy = strategy;

            RasEntry actual = RasEntry.CreateVpnEntry(name, serverAddress, strategy, device);

            TestUtilities.AssertEntry(expected, actual);
        }

        /// <summary>
        /// Tests the CreateDialUpEntry method to ensure a new default dialup entry is created.
        /// </summary>
        [TestMethod]
        public void CreateDialUpEntryTest()
        {
            string name = "Test Entry";
            string phoneNumber = "555-555-1234";
            RasDevice device = RasDevice.Create("My Modem", RasDeviceType.Modem);

            RasEntry expected = new RasEntry(name);
            expected.Device = device;
            expected.DialMode = RasDialMode.None;
            expected.EntryType = RasEntryType.Phone;
            expected.FramingProtocol = RasFramingProtocol.Ppp;
            expected.IdleDisconnectSeconds = RasIdleDisconnectTimeout.Default;
            expected.NetworkProtocols = RasNetworkProtocols.IP;
#if (WINXP || WINXPSP2 || WIN2K8)
            expected.RedialCount = 3;
            expected.RedialPause = 60;
#endif
#if (WIN2K8)
            expected.NetworkProtocols |= RasNetworkProtocols.IPv6;
#endif
            expected.Options = RasEntryOptions.None;
            expected.PhoneNumber = phoneNumber;
            expected.VpnStrategy = RasVpnStrategy.Default;

            RasEntry actual = RasEntry.CreateDialUpEntry(name, phoneNumber, device);

            TestUtilities.AssertEntry(expected, actual);
        }

        /// <summary>
        /// Tests the CreateDialUpEntry method to ensure an exception is thrown for empty entry names.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDialUpEntryWithEmptyNameTest()
        {
            string name = string.Empty;
            string phoneNumber = "555-555-1234";
            RasDevice device = RasDevice.Create("My Modem", RasDeviceType.Modem);

            RasEntry.CreateDialUpEntry(name, phoneNumber, device);
        }

        /// <summary>
        /// Tests the CreateDialUpEntry method to ensure an exception is thrown for empty phone numbers.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDialUpEntryWithEmptyPhoneNumberTest()
        {
            string name = "Test Entry";
            string phoneNumber = string.Empty;
            RasDevice device = RasDevice.Create("My Modem", RasDeviceType.Modem);

            RasEntry.CreateDialUpEntry(name, phoneNumber, device);
        }

        /// <summary>
        /// Tests the CreateDialUpEntry method to ensure an exception is thrown for null devices.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDialUpEntryWithNullDeviceTest()
        {
            string name = "Test Entry";
            string phoneNumber = "555-555-1234";
            RasDevice device = null;

            RasEntry.CreateDialUpEntry(name, phoneNumber, device);
        }

        /// <summary>
        /// Tests the ClearCredentials method to ensure the credentials get cleared as expected.
        /// </summary>
        [TestMethod]
        public void ClearCredentialsTest()
        {
            string name = "Test Entry";

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.options = NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain;

            bool expected = true;

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.ClearCredentials();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the ClearCredentials method to ensure an InvalidOperationException is thrown when the entry does not belong to a phone book.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ClearCredentialsInvalidOperationExceptionTest()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            target.ClearCredentials();
        }

#if (WINXP || WINXPSP2 || WIN2K8)

        /// <summary>
        /// Tests the ClearCredentials method to ensure an invalid operation exception is thrown when the entry does not belong to a phone book.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ClearCredentialsInvalidOperationException()
        {
            string name = "Test Entry";

            RasEntry target = new RasEntry(name);
            target.ClearCredentials(RasPreSharedKey.Client);
        }

        /// <summary>
        /// Tests the ClearCredentials method to ensure the client pre-shared key is cleared successfully.
        /// </summary>
        [TestMethod]
        public void ClearCredentials1ClientPreSharedKeyTest()
        {
            string name = "Test Entry";
            bool expected = true;

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.ClearCredentials(RasPreSharedKey.Client);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the ClearCredentials method to ensure the DDM pre-shared key is cleared successfully.
        /// </summary>
        [TestMethod]
        public void ClearCredentials1DdmPreSharedKeyTest()
        {
            string name = "Test Entry";
            bool expected = true;

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook(); 
            
            bool actual = target.ClearCredentials(RasPreSharedKey.Ddm);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the ClearCredentials method to ensure the server pre-shared key is cleared successfully.
        /// </summary>
        [TestMethod]
        public void ClearCredentials1ServerPreSharedKeyTest()
        {
            string name = "Test Entry";
            bool expected = true;

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();

            Isolate.Fake.StaticMethods<RasHelper>();
            Isolate.WhenCalled(() => RasHelper.SetCredentials(null, null, credentials, false)).WillReturn(true);

            RasEntry target = new RasEntry(name);
            target.Owner = new RasPhoneBook();

            bool actual = target.ClearCredentials(RasPreSharedKey.Server);

            Assert.AreEqual(expected, actual);
        }

#endif

        /// <summary>
        /// Tests the Clone method to ensure an object is returned as expected.
        /// </summary>
        [TestMethod]
        public void CloneTest()
        {
            RasEntry target = new RasEntry("Test");

            RasEntry actual = (RasEntry)target.Clone();

            Assert.AreEqual(target.Name, actual.Name);
        }

        #endregion

        #endregion
    }
}