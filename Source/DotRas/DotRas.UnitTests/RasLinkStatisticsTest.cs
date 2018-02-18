﻿//--------------------------------------------------------------------------
// <copyright file="RasLinkStatisticsTest.cs" company="Jeff Winn">
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

namespace DotRas.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for <see cref="DotRas.RasLinkStatistics"/> and is intended to contain all associated unit tests.
    /// </summary>
    [TestClass]
    public class RasLinkStatisticsTest : UnitTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasLinkStatisticsTest"/> class.
        /// </summary>
        public RasLinkStatisticsTest()
        {
        }

        #endregion

        #region Properties

        #region BytesTransmitted

        /// <summary>
        /// Tests the BytesTransmitted property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void BytesTransmittedTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.BytesTransmitted;

            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Tests the BytesTransmitted property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void BytesTransmittedOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.BytesTransmitted;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BytesReceived

        /// <summary>
        /// Tests the BytesReceived property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void BytesReceivedTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.BytesReceived;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the BytesReceived property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void BytesReceivedOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.BytesReceived;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region FramesTransmitted

        /// <summary>
        /// Tests the FramesTransmitted property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void FramesTransmittedTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.FramesTransmitted;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the FramesTransmitted property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void FramesTransmittedOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.FramesTransmitted;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region FramesReceived

        /// <summary>
        /// Tests the FramesReceived property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void FramesReceivedTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.FramesReceived;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the FramesReceived property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void FramesReceivedOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, expected, 0, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.FramesReceived;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region CrcError

        /// <summary>
        /// Tests the CrcError property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void CrcErrorTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, expected, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.CrcError;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CrcError property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void CrcErrorOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, expected, 0, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.CrcError;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region TimeoutError

        /// <summary>
        /// Tests the TimeoutError property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void TimeoutErrorTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, expected, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.TimeoutError;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CrcError property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void TimeoutErrorOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, expected, 0, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.TimeoutError;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region AlignmentError

        /// <summary>
        /// Tests the AlignmentError property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void AlignmentErrorTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, expected, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.AlignmentError;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AlignmentError property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void AlignmentErrorOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, expected, 0, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.AlignmentError;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region HardwareOverrunError

        /// <summary>
        /// Tests the HardwareOverrunError property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void HardwareOverrunErrorTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, expected, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.HardwareOverrunError;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the HardwareOverrunError property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void HardwareOverrunErrorOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, expected, 0, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.HardwareOverrunError;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region FramingError

        /// <summary>
        /// Tests the FramingError property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void FramingErrorTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, expected, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.FramingError;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the FramingError property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void FramingErrorOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, expected, 0, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.FramingError;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BufferOverrunError

        /// <summary>
        /// Tests the BufferOverrunError property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void BufferOverrunErrorTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, expected, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.BufferOverrunError;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the BufferOverrunError property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void BufferOverrunErrorOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, expected, 0, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.BufferOverrunError;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region CompressionRatioIn

        /// <summary>
        /// Tests the CompressionRatioIn property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void CompressionRatioInTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.CompressionRatioIn;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CompressionRatioIn property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void CompressionRatioInOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected, 0, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.CompressionRatioIn;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region CompressionRatioOut

        /// <summary>
        /// Tests the CompressionRatioOut property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void CompressionRatioOutTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.CompressionRatioOut;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CompressionRatioOut property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void CompressionRatioOutOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected, 0, TimeSpan.FromMilliseconds(0));
            long actual = target.CompressionRatioOut;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region LinkSpeed

        /// <summary>
        /// Tests the LinkSpeed property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void LinkSpeedTest()
        {
            long expected = 1;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected, TimeSpan.FromMilliseconds(0));
            long actual = target.LinkSpeed;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the CompressionRatioOut property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void LinkSpeedOverflowTest()
        {
            long expected = uint.MaxValue;

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected, TimeSpan.FromMilliseconds(0));
            long actual = target.LinkSpeed;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ConnectionDuration

        /// <summary>
        /// Tests the ConnectionDuration property to ensure the same value is returned as was passed to the constructor.
        /// </summary>
        [TestMethod]
        public void ConnectionDurationTest()
        {
            TimeSpan expected = TimeSpan.FromMilliseconds(1);

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected);
            TimeSpan actual = target.ConnectionDuration;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the ConnectionDuration property to ensure the data type used can handle the maximum value for an unsigned 32-bit integer.
        /// </summary>
        [TestMethod]
        public void ConnectionDurationOverflowTest()
        {
            TimeSpan expected = TimeSpan.FromMilliseconds(uint.MaxValue);

            RasLinkStatistics target = new RasLinkStatistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, expected);
            TimeSpan actual = target.ConnectionDuration;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Test Methods Init

        /// <summary>
        /// Initializes the test instance.
        /// </summary>
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #endregion
    }
}
