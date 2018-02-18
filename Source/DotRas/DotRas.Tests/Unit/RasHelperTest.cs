//--------------------------------------------------------------------------
// <copyright file="RasHelperTest.cs" company="Jeff Winn">
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
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Runtime.InteropServices;
    using DotRas;
    using DotRas.Tests.Unit.Mocks.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TypeMock.ArrangeActAssert;
    
    /// <summary>
    /// This is a test class for <see cref="DotRas.RasHelper"/> and is intended to contain all associated unit tests.
    /// </summary>
    [TestClass]
    public class RasHelperTest : UnitTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasHelperTest"/> class.
        /// </summary>
        public RasHelperTest()
        {
        }

        #endregion

        #region Methods

        #region ClearConnectionStatistics

        /// <summary>
        /// Tests the ClearConnectionStatistics method to ensure the statistics are cleared successfully.
        /// </summary>
        [TestMethod]
        public void ClearConnectionStatisticsTest()
        {
            bool expected = true;

            using (RasClearConnectionStatisticsMock mock = new RasClearConnectionStatisticsMock(NativeMethods.SUCCESS))
            {
                bool actual = RasHelper.ClearConnectionStatistics(new RasHandle(new IntPtr(1)));

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the ClearConnectionStatistics method to ensure an InvalidHandleException is thrown when an invalid handle is used.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidHandleException))]
        public void ClearConnectionStatisticsInvalidHandleTest()
        {
            bool expected = false;

            using (RasClearConnectionStatisticsMock mock = new RasClearConnectionStatisticsMock(NativeMethods.ERROR_INVALID_HANDLE))
            {
                bool actual = RasHelper.ClearConnectionStatistics(NativeMethods.INVALID_HANDLE_VALUE);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the ClearConnectionStatistics method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void ClearConnectionStatisticsUnexpectedResultTest()
        {
            bool expected = false;

            using (RasClearConnectionStatisticsMock mock = new RasClearConnectionStatisticsMock(NativeMethods.ERROR_INVALID_PARAMETER))
            {
                bool actual = RasHelper.ClearConnectionStatistics(NativeMethods.INVALID_HANDLE_VALUE);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the ClearConnectionStatistics method to ensure a NotSupportedException is thrown when an entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ClearConnectionStatisticsNotSupportedTest()
        {
            try
            {
                Isolate.Fake.StaticMethods<SafeNativeMethods>();
                Isolate.WhenCalled(() => SafeNativeMethods.ClearConnectionStatistics(null)).WillThrow(new EntryPointNotFoundException());

                RasHelper.ClearConnectionStatistics(new RasHandle());
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region ClearLinkStatistics

        /// <summary>
        /// Tests the ClearLinkStatistics method to ensure it clears the link statistics as expected.
        /// </summary>
        [TestMethod]
        public void ClearLinkStatisticsTest()
        {
            bool expected = true;

            using (RasClearLinkStatisticsMock mock = new RasClearLinkStatisticsMock(NativeMethods.SUCCESS))
            {
                bool actual = RasHelper.ClearLinkStatistics(new RasHandle(new IntPtr(1)), 1);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the ClearLinkStatistics method to ensure an ArgumentException is thrown when the subentry id is less than or equal to zero.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ClearLinkStatisticsSubEntryIdLessThanOrEqualToZeroTest()
        {
            bool expected = false;

            using (RasClearLinkStatisticsMock mock = new RasClearLinkStatisticsMock(NativeMethods.SUCCESS))
            {
                bool actual = RasHelper.ClearLinkStatistics(new RasHandle(new IntPtr(1)), 0);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the ClearLinkStatistics method to ensure an InvalidHandleException is thrown when an invalid handle is used.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidHandleException))]
        public void ClearLinkStatisticsInvalidHandleTest()
        {
            using (RasClearLinkStatisticsMock mock = new RasClearLinkStatisticsMock(NativeMethods.ERROR_INVALID_HANDLE))
            {
                RasHelper.ClearLinkStatistics(NativeMethods.INVALID_HANDLE_VALUE, 1);
            }
        }

        /// <summary>
        /// Tests the ClearLinkStatistics method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void ClearLinkStatisticsUnexpectedResultTest()
        {
            bool expected = false;

            using (RasClearLinkStatisticsMock mock = new RasClearLinkStatisticsMock(NativeMethods.ERROR_INVALID_PARAMETER))
            {
                bool actual = RasHelper.ClearLinkStatistics(new RasHandle(new IntPtr(1)), 1);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the ClearLinkStatistics method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ClearLinkStatisticsNotSupportedTest()
        {
            try
            {
                Isolate.Fake.StaticMethods<SafeNativeMethods>();
                Isolate.WhenCalled(() => SafeNativeMethods.ClearLinkStatistics(null, 1)).WillThrow(new EntryPointNotFoundException());

                RasHelper.ClearLinkStatistics(new RasHandle(), 1);
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region DeleteEntry

        /// <summary>
        /// Tests the DeleteEntry method to ensure an ArgumentNullException is thrown for null a phoneBook argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteEntryNullPhoneBookTest()
        {
            RasHelper.DeleteEntry(null, "Test Entry");
        }

        /// <summary>
        /// Tests the DeleteEntry method to ensure an ArgumentException is thrown for an empty phoneBook argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteEntryEmptyPhoneBookTest()
        {
            RasHelper.DeleteEntry(string.Empty, "Test Entry");
        }

        /// <summary>
        /// Tests the DeleteEntry method to ensure an ArgumentException is thrown for an empty entryName argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteEntryNullEntryNameTest()
        {
            RasHelper.DeleteEntry("C:\\Test.pbk", null);
        }

        /// <summary>
        /// Tests the DeleteEntry method to ensure an ArgumentException is thrown for an empty entryName argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteEntryEmptyEntryNameTest()
        {
            RasHelper.DeleteEntry("C:\\Test.pbk", string.Empty);
        }
        
        /// <summary>
        /// Tests the DeleteEntry method to ensure an UnauthorizedAccessException is thrown when an access denied error is returned from the call.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteEntryAccessDeniedTest()
        {
            using (RasDeleteEntryMock mock = new RasDeleteEntryMock(NativeMethods.ERROR_ACCESS_DENIED))
            {
                RasHelper.DeleteEntry("C:\\Test.pbk", "Test Entry");
            }
        }

        /// <summary>
        /// Tests the DeleteEntry method to ensure it can properly delete an entry.
        /// </summary>
        [TestMethod]
        public void DeleteEntryTest()
        {
            bool expected = true;

            using (RasDeleteEntryMock mock = new RasDeleteEntryMock(NativeMethods.SUCCESS))
            {
                bool actual = RasHelper.DeleteEntry("C:\\Test.pbk", "Test Entry");

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the DeleteEntry method to ensure a RasException is thrown when an unexpected result occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void DeleteEntryUnexpectedResultTest()
        {
            using (RasDeleteEntryMock mock = new RasDeleteEntryMock(NativeMethods.ERROR_CANNOT_FIND_PHONEBOOK_ENTRY))
            {
                RasHelper.DeleteEntry("C:\\Test.pbk", "Test Entry");
            }
        }

        /// <summary>
        /// Tests the DeleteEntry method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DeleteEntryNotSupportedTest()
        {
            try
            {
                Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.DeleteEntry(null, null)).WillThrow(new EntryPointNotFoundException());

                RasHelper.DeleteEntry("C:\\Test.pbk", "Test Entry");
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetActiveConnections

        /// <summary>
        /// Tests the GetActiveConnections method to ensure a RasException is thrown when an unexpected result occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetActiveConnectionsUnexpectedResultTest()
        {
            using (RasEnumConnectionsMock target = new RasEnumConnectionsMock(NativeMethods.ERROR_INVALID_PARAMETER))
            {
                RasHelper.GetActiveConnections();
            }
        }

        /// <summary>
        /// Tests the GetActiveConnections method to ensure it can properly return an empty collection when no active connections are present.
        /// </summary>
        [TestMethod]
        public void GetActiveConnectionsWithNoActiveConnectionsTest()
        {
            NativeMethods.RASCONN[] expected = new NativeMethods.RASCONN[0];

            using (RasEnumConnectionsMock target = new RasEnumConnectionsMock())
            {
                target.Value = expected;
                target.ReturnValue = NativeMethods.SUCCESS;

                ReadOnlyCollection<RasConnection> actual = RasHelper.GetActiveConnections();

                Assert.AreEqual(expected.Length, actual.Count);
            }
        }

        /// <summary>
        /// Tests the GetActiveConnections method to ensure it can properly return a collection of active connections.
        /// </summary>
        [TestMethod]
        public void GetActiveConnectionsTest()
        {
            int size = Marshal.SizeOf(typeof(NativeMethods.RASCONN));
            Random rand = new Random();

            NativeMethods.RASCONN[] expected = new NativeMethods.RASCONN[2];

            expected[0] = new NativeMethods.RASCONN();
            expected[0].size = size;
            expected[0].deviceName = "WAN Miniport (PPTP)";
            expected[0].deviceType = RasDeviceType.Vpn;
            expected[0].entryId = Guid.NewGuid();
            expected[0].entryName = "Test VPN Entry";
            expected[0].handle = new IntPtr(rand.Next(1, int.MaxValue));
            expected[0].phoneBook = "C:\\Test.pbk";
            expected[0].subEntryId = 0;
#if (WINXP || WINXPSP2 || WIN2K8)
            expected[0].connectionOptions = RasConnectionOptions.AllUsers;
            expected[0].sessionId = new Luid();
#endif
#if (WIN2K8)
            expected[0].correlationId = Guid.NewGuid();
#endif

            expected[1] = new NativeMethods.RASCONN();
            expected[1].size = size;
            expected[1].deviceName = "My POTS Modem";
            expected[1].deviceType = RasDeviceType.Modem;
            expected[1].entryId = Guid.NewGuid();
            expected[1].entryName = "Test Modem Entry";
            expected[1].handle = new IntPtr(rand.Next(1, int.MaxValue));
            expected[1].phoneBook = "C:\\Test.pbk";
            expected[1].subEntryId = 0;
#if (WINXP || WINXPSP2 || WIN2K8)
            expected[1].connectionOptions = RasConnectionOptions.AllUsers;
            expected[1].sessionId = new Luid();
#endif
#if (WIN2K8)
            expected[1].correlationId = Guid.NewGuid();
#endif

            using (RasEnumConnectionsMock target = new RasEnumConnectionsMock())
            {
                target.Value = expected;
                target.ReturnValue = NativeMethods.SUCCESS;

                ReadOnlyCollection<RasConnection> actual = RasHelper.GetActiveConnections();

                Assert.AreEqual(expected.Length, actual.Count);

                for (int index = 0; index < expected.Length; index++)
                {
                    NativeMethods.RASCONN objA = expected[index];
                    RasConnection objB = actual[index];

                    Assert.AreEqual(objA.deviceName, objB.Device.Name);
                    Assert.AreEqual(objA.deviceType, objB.Device.DeviceType);
                    Assert.AreEqual(objA.entryId, objB.EntryId);
                    Assert.AreEqual(objA.entryName, objB.EntryName);
                    Assert.AreEqual(objA.handle, objB.Handle.DangerousGetHandle());
                    Assert.AreEqual(objA.phoneBook, objB.PhoneBookPath);
                    Assert.AreEqual(objA.subEntryId, objB.SubEntryId);

#if (WINXP || WINXPSP2 || WIN2K8)
                    Assert.AreEqual(objA.connectionOptions, objB.ConnectionOptions);
                    Assert.AreEqual(objA.sessionId, objB.SessionId);
#endif
#if (WIN2K8)
                    Assert.AreEqual(objA.correlationId, objB.CorrelationId);
#endif
                }
            }
        }

        /// <summary>
        /// Tests the GetActiveConnections method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetActiveConnectionsNotSupportedTest()
        {
            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcConnections = IntPtr.Zero;

            try
            {
                Isolate.Fake.StaticMethods<SafeNativeMethods>();
                Isolate.WhenCalled(() => SafeNativeMethods.EnumConnections(IntPtr.Zero, ref lpCb, ref lpcConnections)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetActiveConnections();
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetAutoDialAddress

        /// <summary>
        /// Tests the GetAutoDialAddress method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetAutoDialAddressUnexpectedResultTest()
        {
            using (RasGetAutodialAddressMock mock = new RasGetAutodialAddressMock(NativeMethods.ERROR_INVALID_PARAMETER))
            {
                RasHelper.GetAutoDialAddress("Test");
            }
        }

        /// <summary>
        /// Tests the GetAutoDialAddress method to ensure a null reference is returned when the file is not found.
        /// </summary>
        [TestMethod]
        public void GetAutoDialAddressFileNotFoundTest()
        {
            RasAutoDialAddress expected = null;

            using (RasGetAutodialAddressMock mock = new RasGetAutodialAddressMock(NativeMethods.ERROR_FILE_NOT_FOUND))
            {
                RasAutoDialAddress actual = RasHelper.GetAutoDialAddress("Test");

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialAddress method to ensure the correct values are returned as expected.
        /// </summary>
        [TestMethod]
        public void GetAutoDialAddressTest()
        {
            NativeMethods.RASAUTODIALENTRY[] expected = new NativeMethods.RASAUTODIALENTRY[2];
            expected[0] = new NativeMethods.RASAUTODIALENTRY();
            expected[0].dialingLocation = 0;
            expected[0].entryName = "Test Entry 1";
            expected[0].options = 0;

            expected[1] = new NativeMethods.RASAUTODIALENTRY();
            expected[1].dialingLocation = 1;
            expected[1].entryName = "Test Entry 2";
            expected[1].options = 0;

            using (RasGetAutodialAddressMock mock = new RasGetAutodialAddressMock())
            {
                mock.ReturnValue = NativeMethods.SUCCESS;
                mock.Value = expected;

                string address = "Test";
                RasAutoDialAddress actual = RasHelper.GetAutoDialAddress(address);

                Assert.AreEqual(address, actual.Address);
                Assert.AreEqual(expected[0].dialingLocation, actual.Entries[0].DialingLocation);
                Assert.AreEqual(expected[0].entryName, actual.Entries[0].EntryName);

                Assert.AreEqual(expected[1].dialingLocation, actual.Entries[1].DialingLocation);
                Assert.AreEqual(expected[1].entryName, actual.Entries[1].EntryName);
            }
        }
        
        /// <summary>
        /// Tests the GetAutoDialAddress method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetAutoDialAddressNotSupportedTest()
        {
            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcEntries = IntPtr.Zero;

            try
            {
                Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.GetAutodialAddress(null, IntPtr.Zero, IntPtr.Zero, ref lpCb, ref lpcEntries)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetAutoDialAddress(null);
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetAutoDialAddresses

        /// <summary>
        /// Tests the GetAutoDialAddresses method to ensure the expected data is returned.
        /// </summary>
        [TestMethod]
        public void GetAutoDialAddressesTest()
        {
            Collection<string> expected = new Collection<string>();
            expected.Add("1234567890");
            expected.Add("0987654321");

            using (RasEnumAutodialAddressesMock mock = new RasEnumAutodialAddressesMock())
            {
                mock.Value = expected;

                Collection<string> actual = RasHelper.GetAutoDialAddresses();

                Assert.AreEqual(expected[0], actual[0]);
                Assert.AreEqual(expected[1], actual[1]);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialAddresses method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetAutoDialAddressesUnexpectedResultTest()
        {
            Collection<string> expected = null;

            using (RasEnumAutodialAddressesMock mock = new RasEnumAutodialAddressesMock(NativeMethods.ERROR_INVALID_PARAMETER))
            {
                Collection<string> actual = RasHelper.GetAutoDialAddresses();

                CollectionAssert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialAddresses method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetAutoDialAddressesNotSupportedTest()
        {
            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcAddresses = IntPtr.Zero;

            try
            {
                Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.EnumAutodialAddresses(IntPtr.Zero, ref lpCb, ref lpcAddresses)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetAutoDialAddresses();
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetAutoDialEnable

        /// <summary>
        /// Tests the GetAutoDialEnable method to ensure the method returns the value expected.
        /// </summary>
        [TestMethod]
        public void GetAutoDialEnableTest()
        {
            bool expected = true;

            using (RasGetAutodialEnableMock mock = new RasGetAutodialEnableMock())
            {
                mock.Enabled = expected;
                mock.ReturnValue = NativeMethods.SUCCESS;

                bool actual = RasHelper.GetAutoDialEnable(0);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialEnable method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetAutoDialEnableUnexpectedResultTest()
        {
            bool expected = true;

            using (RasGetAutodialEnableMock mock = new RasGetAutodialEnableMock())
            {
                mock.ReturnValue = NativeMethods.ERROR_INVALID_PARAMETER;

                bool actual = RasHelper.GetAutoDialEnable(0);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialEnable method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetAutoDialEnableNotSupportedTest()
        {
            bool enabled = false;

            try
            {
                Isolate.Fake.StaticMethods<SafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.GetAutodialEnable(0, ref enabled)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetAutoDialEnable(0);
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetAutoDialParameter

        /// <summary>
        /// Tests the GetAutoDialParameter method to ensure the value returned is expected.
        /// </summary>
        [TestMethod]
        public void GetAutoDialParameterTest()
        {
            int expected = 10;

            using (RasGetAutodialParamMock mock = new RasGetAutodialParamMock())
            {
                mock.ReturnIncorrectSize = false;
                mock.Value = expected;

                int actual = RasHelper.GetAutoDialParameter(NativeMethods.RASADP.ConnectionQueryTimeout);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialParameter method to ensure an InvalidOperationException is thrown when the parameter size returned is incorrect.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAutoDialParameterIncorrectSizeTest()
        {
            int expected = 10;

            using (RasGetAutodialParamMock mock = new RasGetAutodialParamMock())
            {
                mock.ReturnIncorrectSize = true;
                mock.Value = 0;

                int actual = RasHelper.GetAutoDialParameter(NativeMethods.RASADP.ConnectionQueryTimeout);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialParameter method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetAutoDialParameterUnexpectedResultTest()
        {
            int expected = 0;

            using (RasGetAutodialParamMock mock = new RasGetAutodialParamMock(NativeMethods.ERROR_INVALID_PARAMETER))
            {
                int actual = RasHelper.GetAutoDialParameter(NativeMethods.RASADP.ConnectionQueryTimeout);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the GetAutoDialParameter method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetAutoDialParameterNotSupportedTest()
        {
            int lpdwcbValue = 0;

            try
            {
                Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.GetAutodialParam(NativeMethods.RASADP.DisableConnectionQuery, IntPtr.Zero, ref lpdwcbValue)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetAutoDialParameter(NativeMethods.RASADP.DisableConnectionQuery);
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetConnectionStatus

        /// <summary>
        /// Tests the GetConnectStatus method to ensure the connection state is returned as expected.
        /// </summary>
        [TestMethod]
        public void GetConnectionStatusTest()
        {
            NativeMethods.RASCONNSTATUS expected = new NativeMethods.RASCONNSTATUS();
            expected.connectionState = RasConnectionState.AllDevicesConnected;
            expected.deviceName = "WAN Miniport (PPTP)";
            expected.deviceType = RasDeviceType.Vpn;
            expected.errorCode = NativeMethods.SUCCESS;
            expected.phoneNumber = "127.0.0.1";
            
            using (RasGetConnectStatusMock mock = new RasGetConnectStatusMock())
            {
                mock.Status = expected;
                mock.ReturnValue = NativeMethods.SUCCESS;

                RasConnectionStatus actual = RasHelper.GetConnectionStatus(new RasHandle(new IntPtr(1)));

                Assert.AreEqual(expected.connectionState, actual.ConnectionState);
                Assert.AreEqual(expected.deviceName, actual.Device.Name);
                Assert.AreEqual(expected.deviceType, actual.Device.DeviceType);
                Assert.AreEqual(expected.errorCode, actual.ErrorCode);
                Assert.AreEqual(null, actual.ErrorMessage);
                Assert.AreEqual(expected.phoneNumber, actual.PhoneNumber);
            }
        }

        /// <summary>
        /// Tests the GetConnectionStatus method to ensure an InvalidHandleException is thrown for invalid handles.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidHandleException))]
        public void GetConnectionStatusInvalidHandleTest()
        {
            NativeMethods.RASCONNSTATUS expected = new NativeMethods.RASCONNSTATUS();
            expected.connectionState = RasConnectionState.AllDevicesConnected;
            expected.deviceName = "WAN Miniport (PPTP)";
            expected.deviceType = RasDeviceType.Vpn;
            expected.errorCode = NativeMethods.SUCCESS;
            expected.phoneNumber = "127.0.0.1";

            using (RasGetConnectStatusMock mock = new RasGetConnectStatusMock())
            {
                mock.Status = expected;
                mock.ReturnValue = NativeMethods.ERROR_INVALID_HANDLE;

                RasConnectionStatus actual = RasHelper.GetConnectionStatus(NativeMethods.INVALID_HANDLE_VALUE);
            }
        }

        /// <summary>
        /// Tests the GetConnectionStatus method to ensure an error message is included when the error code has been set.
        /// </summary>
        [TestMethod]
        public void GetConnectionStatusErrorCodeTest()
        {
            NativeMethods.RASCONNSTATUS expected = new NativeMethods.RASCONNSTATUS();
            expected.connectionState = RasConnectionState.Authenticate;
            expected.deviceName = "WAN Miniport (PPTP)";
            expected.deviceType = RasDeviceType.Vpn;
            expected.errorCode = NativeMethods.ERROR_PROTOCOL_NOT_CONFIGURED;
            expected.phoneNumber = "127.0.0.1";

            using (RasGetConnectStatusMock mock = new RasGetConnectStatusMock())
            {
                mock.Status = expected;
                mock.ReturnValue = NativeMethods.SUCCESS;

                RasConnectionStatus actual = RasHelper.GetConnectionStatus(new RasHandle(new IntPtr(1)));

                Assert.AreEqual(expected.connectionState, actual.ConnectionState);
                Assert.AreEqual(expected.deviceName, actual.Device.Name);
                Assert.AreEqual(expected.deviceType, actual.Device.DeviceType);
                Assert.AreEqual(expected.errorCode, actual.ErrorCode);

                Assert.IsTrue(actual.ErrorMessage != null && actual.ErrorMessage.Length > 0);

                Assert.AreEqual(expected.phoneNumber, actual.PhoneNumber);
            }
        }
        
        /// <summary>
        /// Tests the GetConnectionStatus method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetConnectionStatusUnexpectedResultTest()
        {
            using (RasGetConnectStatusMock mock = new RasGetConnectStatusMock())
            {
                mock.ReturnValue = NativeMethods.ERROR_INVALID_PARAMETER;

                RasHelper.GetConnectionStatus(new RasHandle(new IntPtr(1)));
            }
        }

        /// <summary>
        /// Tests the GetConnectionStatus method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetConnectionStatusNotSupportedTest()
        {
            try
            {
                Isolate.Fake.StaticMethods<SafeNativeMethods>();
                Isolate.WhenCalled(() => SafeNativeMethods.GetConnectStatus(new RasHandle(), IntPtr.Zero)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetConnectionStatus(new RasHandle());
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetCredentials

        /// <summary>
        /// Tests the GetCredentials method to ensure the credentials are returned as expected.
        /// </summary>
        [TestMethod]
        public void GetCredentialsTest()
        {
            NativeMethods.RASCREDENTIALS expected = new NativeMethods.RASCREDENTIALS();
            expected.userName = "User";
            expected.password = "12345";
            expected.domain = "Test";
            expected.options = NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain;

            using (RasGetCredentialsMock mock = new RasGetCredentialsMock())
            {
                mock.Credentials = expected;
                mock.ReturnValue = NativeMethods.SUCCESS;

                NetworkCredential actual = RasHelper.GetCredentials("C:\\Test.pbk", "Test Entry", NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);

                Assert.AreEqual(expected.userName, actual.UserName);
                Assert.AreEqual(expected.password, actual.Password);
                Assert.AreEqual(expected.domain, actual.Domain);
            }
        }
        
        /// <summary>
        /// Tests the GetCredentials method to ensure a null reference is resulted when the credentials are not found.
        /// </summary>
        [TestMethod]
        public void GetCredentialsNotFoundTest()
        {
            NetworkCredential expected = null;

            using (RasGetCredentialsMock mock = new RasGetCredentialsMock())
            {
                mock.ReturnValue = NativeMethods.ERROR_FILE_NOT_FOUND;

                NetworkCredential actual = RasHelper.GetCredentials("C:\\Test.pbk", "Test Entry", NativeMethods.RASCM.UserName);

                Assert.AreEqual(expected, actual);
            }
        }
        
        /// <summary>
        /// Tests the GetCredentials method to ensure an ArgumentException is thrown for null a phone book argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCredentialsNullPhoneBookTest()
        {
            RasHelper.GetCredentials(null, "Test Entry", NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
        }

        /// <summary>
        /// Tests the GetCredentials method to ensure an ArgumentException is thrown for an empty phone book argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCredentialsEmptyPhoneBookTest()
        {
            RasHelper.GetCredentials(string.Empty, "Test Entry", NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
        }
        
        /// <summary>
        /// Tests the GetCredentials method to ensure an ArgumentException is thrown for a null entry name argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCredentialsNullEntryNameTest()
        {
            RasHelper.GetCredentials("C:\\Test.pbk", null, NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
        }

        /// <summary>
        /// Tests the GetCredentials method to ensure an ArgumentException is thrown for an empty entry name argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCredentialsEmptyEntryNameTest()
        {
            RasHelper.GetCredentials("C:\\Test.pbk", string.Empty, NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
        }

        /// <summary>
        /// Tests the GetCredentials method to ensure a RasException is thrown when an unexpected result occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetCredentialsUnexpectedResultTest()
        {
            using (RasGetCredentialsMock mock = new RasGetCredentialsMock())
            {
                mock.ReturnValue = NativeMethods.ERROR_CANNOT_FIND_PHONEBOOK_ENTRY;

                RasHelper.GetCredentials("C:\\Test.pbk", "Test Entry", NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
            }
        }

        /// <summary>
        /// Tests the GetCredentials method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetCredentialsNotSupportedTest()
        {
            try
            {
                Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.GetCredentials(null, null, IntPtr.Zero)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetCredentials("C:\\Test.pbk", "Test Entry", NativeMethods.RASCM.UserName | NativeMethods.RASCM.Password | NativeMethods.RASCM.Domain);
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region GetDevices

        /// <summary>
        /// Tests the GetDevices method to ensure the expected devices are returned.
        /// </summary>
        [TestMethod]
        public void GetDevicesTest()
        {
            NativeMethods.RASDEVINFO[] expected = new NativeMethods.RASDEVINFO[2];
            expected[0] = new NativeMethods.RASDEVINFO();
            expected[0].name = "WAN Miniport (PPTP)";
            expected[0].type = RasDeviceType.Vpn;

            expected[1] = new NativeMethods.RASDEVINFO();
            expected[1].name = "WAN Miniport (L2TP)";
            expected[1].type = RasDeviceType.Vpn;

            using (RasEnumDevicesMock target = new RasEnumDevicesMock())
            {
                target.ReturnValue = NativeMethods.SUCCESS;
                target.Value = expected;

                ReadOnlyCollection<RasDevice> actual = RasHelper.GetDevices();

                Assert.AreEqual(expected.Length, actual.Count);

                for (int index = 0; index < expected.Length; index++)
                {
                    Assert.AreEqual(expected[index].name, actual[index].Name);
                    Assert.AreEqual(expected[index].type, actual[index].DeviceType);
                }
            }
        }

        /// <summary>
        /// Tests the GetDevices method to ensure an empty collection is returned when no devices are found on the system.
        /// </summary>
        [TestMethod]
        public void GetDevicesWithNoDevicesTest()
        {
            NativeMethods.RASDEVINFO[] expected = new NativeMethods.RASDEVINFO[0];

            using (RasEnumDevicesMock target = new RasEnumDevicesMock())
            {
                target.ReturnValue = NativeMethods.SUCCESS;
                target.Value = expected;

                ReadOnlyCollection<RasDevice> actual = RasHelper.GetDevices();

                Assert.AreEqual(expected.Length, actual.Count);
            }
        }

        /// <summary>
        /// Tests the GetDevices method to ensure a RasException is thrown when an unexpected result is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void GetDevicesUnexpectedResultTest()
        {
            NativeMethods.RASDEVINFO[] expected = new NativeMethods.RASDEVINFO[0];

            using (RasEnumDevicesMock target = new RasEnumDevicesMock(NativeMethods.ERROR_CANNOT_FIND_PHONEBOOK_ENTRY))
            {
                RasHelper.GetDevices();
            }
        }

        /// <summary>
        /// Tests the GetDevices method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetDevicesNotSupportedTest()
        {
            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcDevices = IntPtr.Zero;

            try
            {
                Isolate.Fake.StaticMethods<SafeNativeMethods>();
                Isolate.WhenCalled(() => SafeNativeMethods.EnumDevices(IntPtr.Zero, ref lpCb, ref lpcDevices)).WillThrow(new EntryPointNotFoundException());

                RasHelper.GetDevices();
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #region SetCredentials

        /// <summary>
        /// Tests the SetCredentials method to ensure an ArgumentException is thrown for a null phone book argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetCredentialsNullPhoneBookTest()
        {
            RasHelper.SetCredentials(null, "Test Entry", new NativeMethods.RASCREDENTIALS(), true);
        }

        /// <summary>
        /// Tests the SetCredentials method to ensure an ArgumentException is thrown for an empty phone book argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetCredentialsEmptyPhoneBookTest()
        {
            RasHelper.SetCredentials(string.Empty, "Test Entry", new NativeMethods.RASCREDENTIALS(), true);
        }

        /// <summary>
        /// Tests the SetCredentials method to ensure an ArgumentException is thrown for a null entry name argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetCredentialsNullEntryNameTest()
        {
            RasHelper.SetCredentials("C:\\Test.pbk", null, new NativeMethods.RASCREDENTIALS(), true);
        }

        /// <summary>
        /// Tests the SetCredentials method to ensure an ArgumentException is thrown for an empty entry name argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetCredentialsEmptyEntryNameTest()
        {
            RasHelper.SetCredentials("C:\\Test.pbk", string.Empty, new NativeMethods.RASCREDENTIALS(), true);
        }

        /// <summary>
        /// Tests the SetCredentials method to ensure the credentials are updated as expected.
        /// </summary>
        [TestMethod]
        public void SetCredentialsTest()
        {
            bool expected = true;

            using (RasSetCredentialsMock mock = new RasSetCredentialsMock(NativeMethods.SUCCESS))
            {
                NativeMethods.RASCREDENTIALS c = new NativeMethods.RASCREDENTIALS();
                c.userName = "User";
                c.password = "Password";
                c.domain = "Domain";

                bool actual = RasHelper.SetCredentials("C:\\Test.pbk", "Test Entry", c, false);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the SetCredentials method to ensure a RasException is thrown when an unexpected result occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RasException))]
        public void SetCredentialsUnexpectedResultTest()
        {
            bool expected = false;

            using (RasSetCredentialsMock mock = new RasSetCredentialsMock(NativeMethods.ERROR_CANNOT_FIND_PHONEBOOK_ENTRY))
            {
                NativeMethods.RASCREDENTIALS c = new NativeMethods.RASCREDENTIALS();
                c.userName = "User";
                c.password = "Password";
                c.domain = "Domain";

                bool actual = RasHelper.SetCredentials("C:\\Test.pbk", "Test Entry", c, false);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests the SetCredentials method to ensure a NotSupportedException is thrown when the entry point is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetCredentialsNotSupportedTest()
        {
            try
            {
                Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
                Isolate.WhenCalled(() => UnsafeNativeMethods.SetCredentials(null, null, IntPtr.Zero, false)).WillThrow(new EntryPointNotFoundException());

                RasHelper.SetCredentials("C:\\Test.pbk", "Test Entry", new NativeMethods.RASCREDENTIALS(), false);
            }
            finally
            {
                Isolate.CleanUp();
            }
        }

        #endregion

        #endregion
    }
}