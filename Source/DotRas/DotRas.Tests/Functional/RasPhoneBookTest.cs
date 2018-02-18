//--------------------------------------------------------------------------
// <copyright file="RasPhoneBookTest.cs" company="Jeff Winn">
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
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for <see cref="DotRas.RasPhoneBook"/> and is intended to contain all related functional tests.
    /// </summary>
    [TestClass]
    public class RasPhoneBookTest : FunctionalTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasPhoneBookTest"/> class.
        /// </summary>
        public RasPhoneBookTest()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the RasPhoneBook component to ensure it can create a custom phone book.
        /// </summary>
        [TestMethod]
        public void CreateCustomPhoneBookTest()
        {
            string path = string.Format(
                "C:\\{0}.pbk",
                TestUtilities.StripNonAlphaNumericChars(Guid.NewGuid().ToString()));

            try
            {
                RasPhoneBook pbk = new RasPhoneBook();
                pbk.Open(path);

                RasEntry entry = RasEntry.CreateVpnEntry("Test Entry", "127.0.0.1", RasVpnStrategy.Default, RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn, false));
                if (entry != null)
                {
                    pbk.Entries.Add(entry);
                }

                Assert.IsTrue(File.Exists(path), "The phone book file was not found at the expected location. '{0}'", path);
            }
            finally
            {
                if (File.Exists(path))
                {
                    // The file was created successfully, delete it before the test completes.
                    File.Delete(path);
                }
            }
        }

        #endregion
    }
}