//--------------------------------------------------------------------------
// <copyright file="RasEnumConnectionsMock.cs" company="Jeff Winn">
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
    /// Represents a mock instance of the <see cref="SafeNativeMethods.EnumConnections"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasEnumConnectionsMock : PInvokeMockWithBufferedOutput<NativeMethods.RASCONN>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEnumConnectionsMock"/> class.
        /// </summary>
        public RasEnumConnectionsMock()
            : this(NativeMethods.SUCCESS)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEnumConnectionsMock"/> class.
        /// </summary>
        /// <param name="returnValue">The value to return when the method is called.</param>
        public RasEnumConnectionsMock(int returnValue)
        {
            this.ReturnValue = returnValue;

            this.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the mock instance.
        /// </summary>
        protected override void Initialize()
        {
            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcConnections = IntPtr.Zero;

            Isolate.Fake.StaticMethods<SafeNativeMethods>();

            if (this.ReturnValue == NativeMethods.SUCCESS)
            {
                Isolate.WhenCalled(() => SafeNativeMethods.EnumConnections(IntPtr.Zero, ref lpCb, ref lpcConnections)).DoInstead(new Func<MethodCallContext, int>(this.Execute));
            }
            else
            {
                Isolate.WhenCalled(() => SafeNativeMethods.EnumConnections(IntPtr.Zero, ref lpCb, ref lpcConnections)).WillReturn(this.ReturnValue);
            }

            base.Initialize();
        }

        /// <summary>
        /// Updates the buffer size of the mock context.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <param name="value">The new value.</param>
        protected override void UpdateBufferSize(MethodCallContext context, IntPtr value)
        {
            context.Parameters[1] = value;
        }

        /// <summary>
        /// Updates the number of objects in the buffer for the mock context.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <param name="value">The new value.</param>
        protected override void UpdateOutputCount(MethodCallContext context, IntPtr value)
        {
            context.Parameters[2] = value;
        }

        /// <summary>
        /// Called when the method is executed by the test.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <returns>The result of the p/invoke operation.</returns>
        private int Execute(MethodCallContext context)
        {
            IntPtr lpRasConn = (IntPtr)context.Parameters[0];
            IntPtr lpCb = (IntPtr)context.Parameters[1];
            IntPtr lpcConnections = (IntPtr)context.Parameters[2];

            this.CopyObjectsToBuffer(context, lpRasConn, lpCb);

            return this.ReturnValue;
        }

        #endregion
    }
}