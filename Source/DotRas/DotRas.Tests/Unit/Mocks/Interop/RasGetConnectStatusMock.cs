//--------------------------------------------------------------------------
// <copyright file="RasGetConnectStatusMock.cs" company="Jeff Winn">
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

namespace DotRas.Tests.Unit.Mocks.Interop
{
    using System;
    using System.Runtime.InteropServices;
    using TypeMock;
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// Represents a mock instance of the <see cref="SafeNativeMethods.GetConnectStatus"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasGetConnectStatusMock : PInvokeMock
    {
        #region Fields

        private NativeMethods.RASCONNSTATUS _status;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasGetConnectStatusMock"/> class.
        /// </summary>
        public RasGetConnectStatusMock()
        {
            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the connection status to return.
        /// </summary>
        public NativeMethods.RASCONNSTATUS Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the mock instance.
        /// </summary>
        protected override void Initialize()
        {
            Isolate.Fake.StaticMethods<SafeNativeMethods>();
            Isolate.WhenCalled(() => SafeNativeMethods.GetConnectStatus(null, IntPtr.Zero)).DoInstead(new Func<MethodCallContext, int>(this.Execute));

            base.Initialize();
        }

        /// <summary>
        /// Called when the method is executed by the test.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <returns>The result of the p/invoke operation.</returns>
        private int Execute(MethodCallContext context)
        {
            IntPtr lpRasConnStatus = (IntPtr)context.Parameters[1];

            NativeMethods.RASCONNSTATUS s = new NativeMethods.RASCONNSTATUS();
            s.connectionState = this.Status.connectionState;
            s.deviceName = this.Status.deviceName;
            s.deviceType = this.Status.deviceType;
            s.errorCode = this.Status.errorCode;
            s.phoneNumber = this.Status.phoneNumber;

            Marshal.StructureToPtr(s, lpRasConnStatus, true);

            return this.ReturnValue;
        }

        #endregion
    }
}