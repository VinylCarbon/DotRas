//--------------------------------------------------------------------------
// <copyright file="RasLinkStatistics.cs" company="Jeff Winn">
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
    /// Represents connection link statistics for a remote access connection. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class RasLinkStatistics
    {
        #region Fields

        private int _bytesTransmitted;
        private int _bytesReceived;
        private int _framesTransmitted;
        private int _framesReceived;
        private int _crcError;
        private int _timeoutError;
        private int _alignmentError;
        private int _hardwareOverrunError;
        private int _framingError;
        private int _bufferOverrunError;
        private int _compressionRatioIn;
        private int _compressionRatioOut;
        private int _linkSpeed;
        private TimeSpan _connectionDuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasLinkStatistics"/> class.
        /// </summary>
        /// <param name="bytesTransmitted">The number of bytes transmitted.</param>
        /// <param name="bytesReceived">The number of bytes received.</param>
        /// <param name="framesTransmitted">The number of frames transmitted.</param>
        /// <param name="framesReceived">The number of frames received.</param>
        /// <param name="crcError">The number of cyclic redundancy check (CRC) errors that have occurred.</param>
        /// <param name="timeoutError">The number of timeout errors that have occurred.</param>
        /// <param name="alignmentError">The number of alignment errors that have occurred.</param>
        /// <param name="hardwareOverrunError">The number of hardware overrun errors that have occurred.</param>
        /// <param name="framingError">The number of framing errors that have occurred.</param>
        /// <param name="bufferOverrunError">The number of buffer overrun errors that have occurred.</param>
        /// <param name="compressionRatioIn">The compression ratio for data received on this connection or link.</param>
        /// <param name="compressionRatioOut">The compression ratio for data transmitted on this connection or link.</param>
        /// <param name="linkSpeed">The speed of the link, in bits per second.</param>
        /// <param name="connectionDuration">The length of time that the connection has been connected.</param>
        internal RasLinkStatistics(int bytesTransmitted, int bytesReceived, int framesTransmitted, int framesReceived, int crcError, int timeoutError, int alignmentError, int hardwareOverrunError, int framingError, int bufferOverrunError, int compressionRatioIn, int compressionRatioOut, int linkSpeed, TimeSpan connectionDuration)
        {
            this._bytesTransmitted = bytesTransmitted;
            this._bytesReceived = bytesReceived;
            this._framesTransmitted = framesTransmitted;
            this._framesReceived = framesReceived;
            this._crcError = crcError;
            this._timeoutError = timeoutError;
            this._alignmentError = alignmentError;
            this._hardwareOverrunError = hardwareOverrunError;
            this._framingError = framingError;
            this._bufferOverrunError = bufferOverrunError;
            this._compressionRatioIn = compressionRatioIn;
            this._compressionRatioOut = compressionRatioOut;
            this._linkSpeed = linkSpeed;
            this._connectionDuration = connectionDuration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of bytes transmitted.
        /// </summary>
        public int BytesTransmitted
        {
            get { return this._bytesTransmitted; }
        }

        /// <summary>
        /// Gets the number of bytes received.
        /// </summary>
        public int BytesReceived
        {
            get { return this._bytesReceived; }
        }

        /// <summary>
        /// Gets the number of frames transmitted.
        /// </summary>
        public int FramesTransmitted
        {
            get { return this._framesTransmitted; }
        }

        /// <summary>
        /// Gets the number of frames received.
        /// </summary>
        public int FramesReceived
        {
            get { return this._framesReceived; }
        }

        /// <summary>
        /// Gets the number of cyclic redundancy check (CRC) errors that have occurred.
        /// </summary>
        public int CrcError
        {
            get { return this._crcError; }
        }

        /// <summary>
        /// Gets the number of timeout errors that have occurred.
        /// </summary>
        public int TimeoutError
        {
            get { return this._timeoutError; }
        }

        /// <summary>
        /// Gets the number of alignment errors that have occurred.
        /// </summary>
        public int AlignmentError
        {
            get { return this._alignmentError; }
        }

        /// <summary>
        /// Gets the number of hardware overrun errors that have occurred.
        /// </summary>
        public int HardwareOverrunError
        {
            get { return this._hardwareOverrunError; }
        }

        /// <summary>
        /// Gets the number of framing errors that have occurred.
        /// </summary>
        public int FramingError
        {
            get { return this._framingError; }
        }

        /// <summary>
        /// Gets the number of buffer overrun errors that have occurred.
        /// </summary>
        public int BufferOverrunError
        {
            get { return this._bufferOverrunError; }
        }

        /// <summary>
        /// Gets the compression ratio for data received on this connection or link.
        /// </summary>
        /// <remarks>This member is valid only for a single link connection, or a single link in a multilink connection.</remarks>
        public int CompressionRatioIn
        {
            get { return this._compressionRatioIn; }
        }

        /// <summary>
        /// Gets the compression ratio for data transmitted on this connection or link.
        /// </summary>
        /// <remarks>This member is valid only for a single link connection, or a single link in a multilink connection.</remarks>
        public int CompressionRatioOut
        {
            get { return this._compressionRatioOut; }
        }

        /// <summary>
        /// Gets the speed of the link, in bits per second.
        /// </summary>
        public int LinkSpeed
        {
            get { return this._linkSpeed; }
        }

        /// <summary>
        /// Gets the length of time that the connection has been connected.
        /// </summary>
        public TimeSpan ConnectionDuration
        {
            get { return this._connectionDuration; }
        }

        #endregion
    }
}