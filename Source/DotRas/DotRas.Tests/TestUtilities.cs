//--------------------------------------------------------------------------
// <copyright file="TestUtilities.cs" company="Jeff Winn">
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

namespace DotRas.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DotRas.Design;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains utility methods for the test assembly.
    /// </summary>
    internal static class TestUtilities
    {
        /// <summary>
        /// Strips all non-alphanumeric characters from the value specified.
        /// </summary>
        /// <param name="value">The value to strip.</param>
        /// <returns>A <see cref="System.String"/> whose non-alphanumeric characters have been removed.</returns>
        public static string StripNonAlphaNumericChars(string value)
        {
            string retval = null;

            if (value != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in value)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        sb.Append(c);
                    }
                }

                retval = sb.ToString();
            }

            return retval;
        }

        /// <summary>
        /// Asserts a <see cref="DotRas.RasEntry"/> object.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void AssertEntry(RasEntry expected, RasEntry actual)
        {
            CollectionAssert.AreEqual(expected.AlternatePhoneNumbers, actual.AlternatePhoneNumbers);

            Assert.AreEqual(expected.AreaCode, actual.AreaCode);

#pragma warning disable 0618
            Assert.AreEqual(expected.AutoDialDll, actual.AutoDialDll);
            Assert.AreEqual(expected.AutoDialFunc, actual.AutoDialFunc);
#pragma warning restore 0618

            Assert.AreEqual(expected.Channels, actual.Channels);
            Assert.AreEqual(expected.CountryCode, actual.CountryCode);
            Assert.AreEqual(expected.CountryId, actual.CountryId);
            Assert.AreEqual(expected.CustomAuthKey, actual.CustomAuthKey);
            Assert.AreEqual(expected.CustomDialDll, actual.CustomDialDll);

            AssertDevice(expected.Device, actual.Device);

            Assert.AreEqual(expected.DialExtraPercent, actual.DialExtraPercent);
            Assert.AreEqual(expected.DialExtraSampleSeconds, actual.DialExtraSampleSeconds);
            Assert.AreEqual(expected.DialMode, actual.DialMode);
            Assert.AreEqual(expected.DnsAddress, actual.DnsAddress);
            Assert.AreEqual(expected.DnsAddressAlt, actual.DnsAddressAlt);
            Assert.AreEqual(expected.EncryptionType, actual.EncryptionType);
            Assert.AreEqual(expected.EntryType, actual.EntryType);
            Assert.AreEqual(expected.FrameSize, actual.FrameSize);
            Assert.AreEqual(expected.FramingProtocol, actual.FramingProtocol);
            Assert.AreEqual(expected.HangUpExtraPercent, actual.HangUpExtraPercent);
            Assert.AreEqual(expected.HangUpExtraSampleSeconds, actual.HangUpExtraSampleSeconds);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.IdleDisconnectSeconds, actual.IdleDisconnectSeconds);
            Assert.AreEqual(expected.IPAddress, actual.IPAddress);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.NetworkProtocols, actual.NetworkProtocols);
            Assert.AreEqual(expected.Options, actual.Options);

            if ((expected.Owner != null && actual.Owner == null) || (expected.Owner == null && actual.Owner != null))
            {
                Assert.Fail("The entry owner did not match.");
            }

            Assert.AreEqual(expected.PhoneNumber, actual.PhoneNumber);
            Assert.AreEqual(expected.Reserved1, actual.Reserved1);
            Assert.AreEqual(expected.Reserved2, actual.Reserved2);
            Assert.AreEqual(expected.Script, actual.Script);

            AssertRasCollection<RasSubEntry>(expected.SubEntries, actual.SubEntries);

            Assert.AreEqual(expected.VpnStrategy, actual.VpnStrategy);
            Assert.AreEqual(expected.WinsAddress, actual.WinsAddress);
            Assert.AreEqual(expected.WinsAddressAlt, actual.WinsAddressAlt);
            Assert.AreEqual(expected.X25Address, actual.X25Address);
            Assert.AreEqual(expected.X25Facilities, actual.X25Facilities);
            Assert.AreEqual(expected.X25PadType, actual.X25PadType);
            Assert.AreEqual(expected.X25UserData, actual.X25UserData);

#if (WINXP || WINXPSP2 || WIN2K8)
            Assert.AreEqual(expected.ExtendedOptions, actual.ExtendedOptions);
            Assert.AreEqual(expected.ReservedOptions, actual.ReservedOptions);
            Assert.AreEqual(expected.DnsSuffix, actual.DnsSuffix);
            Assert.AreEqual(expected.TcpWindowSize, actual.TcpWindowSize);
            Assert.AreEqual(expected.PrerequisitePhoneBook, actual.PrerequisitePhoneBook);
            Assert.AreEqual(expected.PrerequisiteEntryName, actual.PrerequisiteEntryName);
            Assert.AreEqual(expected.RedialCount, actual.RedialCount);
            Assert.AreEqual(expected.RedialPause, actual.RedialPause);
#endif
#if (WIN2K8)
            Assert.AreEqual(expected.IPv6DnsAddress, actual.IPv6DnsAddress);
            Assert.AreEqual(expected.IPv6DnsAddressAlt, actual.IPv6DnsAddressAlt);
            Assert.AreEqual(expected.IPv4InterfaceMetric, actual.IPv4InterfaceMetric);
            Assert.AreEqual(expected.IPv6InterfaceMetric, actual.IPv6InterfaceMetric);
#endif
        }

        /// <summary>
        /// Asserts a <see cref="DotRas.Design.RasCollection&lt;TObject&gt;"/> object.
        /// </summary>
        /// <typeparam name="TObject">The type of object contained in the collection.</typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void AssertRasCollection<TObject>(RasCollection<TObject> expected, RasCollection<TObject> actual)
            where TObject : class
        {
            if ((expected != null && actual == null) || (expected == null && actual != null) || (expected != null && actual != null && expected.Count != actual.Count))
            {
                Assert.Fail("The collections do not match.");
            }
            else if (expected != null && actual != null)
            {
                for (int index = 0; index < expected.Count; index++)
                {
                    Assert.AreEqual<TObject>(expected[index], actual[index]);
                }
            }
        }

        /// <summary>
        /// Asserts a <see cref="DotRas.RasDevice"/> object.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void AssertDevice(RasDevice expected, RasDevice actual)
        {
            if ((expected != null && actual == null) || (expected == null && actual != null))
            {
                Assert.Fail("The devices did not match.");
            }
            else if (expected != null && actual != null)
            {
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.DeviceType, actual.DeviceType);
            }
        }
    }
}