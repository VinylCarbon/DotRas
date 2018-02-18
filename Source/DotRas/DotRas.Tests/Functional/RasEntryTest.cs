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

namespace DotRas.Tests.Functional
{
    using System;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for <see cref="DotRas.RasEntry"/> and is intended to contain all associated functional tests.
    /// </summary>
    [TestClass]
    public class RasEntryTest : FunctionalTest
    {
        #region Fields

        private string _entryName;
        private RasPhoneBook _phoneBook;
        private RasEntry _entry;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEntryTest"/> class.
        /// </summary>
        public RasEntryTest()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the test instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._entryName = Guid.NewGuid().ToString();

            this._phoneBook = new RasPhoneBook();
            this._phoneBook.Open();

            this._entry = new RasEntry(this._entryName);
            this._entry.Device = RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn, false);
            this._entry.EncryptionType = RasEncryptionType.Require;
            this._entry.EntryType = RasEntryType.Vpn;
            this._entry.FramingProtocol = RasFramingProtocol.Ppp;
            this._entry.NetworkProtocols = RasNetworkProtocols.IP;
            this._entry.PhoneNumber = IPAddress.Loopback.ToString();
            this._entry.VpnStrategy = RasVpnStrategy.Default;

            this._phoneBook.Entries.Add(this._entry);
        }

        /// <summary>
        /// Performs cleanup for the current test instance.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            if (this._phoneBook.Entries.Contains(this._entry))
            {
                this._phoneBook.Entries.Remove(this._entry);
            }

            this._phoneBook.Dispose();
        }

        /// <summary>
        /// Tests the ClearCredentials method to ensure the credentials will be cleared as expected.
        /// </summary>
        [TestMethod]
        public void ClearCredentialsTest()
        {
            bool expected = true;
            bool actual = this._entry.ClearCredentials();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the GetCredentials method to ensure the expected credentials are returned.
        /// </summary>
        [TestMethod]
        public void GetCredentialsTest()
        {
            NetworkCredential expected = null;
            NetworkCredential actual = this._entry.GetCredentials();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Remove method to ensure the entry is removed from the phone book.
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            bool expected = true;
            bool actual = this._entry.Remove();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the Rename method to ensure the entry will be renamed as expected.
        /// </summary>
        [TestMethod]
        public void RenameTest()
        {
            string name = this._entry.Name;
            string newEntryName = Guid.NewGuid().ToString();

            bool expected = true;
            bool actual = this._entry.Rename(newEntryName);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(this._entry.Name, newEntryName);
        }

        /// <summary>
        /// Tests the Update method to ensure the entry will update properly.
        /// </summary>
        [TestMethod]
        public void UpdateTest()
        {
            bool expected = true;
            bool actual = this._entry.Update();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the UpdateCredentials method to ensure the credentials will update.
        /// </summary>
        [TestMethod]
        public void UpdateCredentialsTest()
        {
            NetworkCredential credentials = new NetworkCredential("Test", "User", "Domain");
            bool expected = true;

            bool actual = this._entry.UpdateCredentials(credentials);

            Assert.AreEqual(expected, actual);
        }

#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Tests the UpdateCredentials(NetworkCredential, bool) method to ensure the credentials will update.
        /// </summary>
        [TestMethod]
        public void UpdateCredentials1Test()
        {
            NetworkCredential credentials = new NetworkCredential("Test", "User", "Domain");
            bool storeCredentialsForAllUsers = true;
            bool expected = true;

            bool actual = this._entry.UpdateCredentials(credentials, storeCredentialsForAllUsers);

            Assert.AreEqual(expected, actual);
        }
#endif
        #endregion
    }
}