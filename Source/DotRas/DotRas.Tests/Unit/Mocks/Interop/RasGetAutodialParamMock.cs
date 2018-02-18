//--------------------------------------------------------------------------
// <copyright file="RasGetAutodialParamMock.cs" company="Jeff Winn">
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
    /// Represents a mock instance of the <see cref="UnsafeNativeMethods.GetAutodialParam"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasGetAutodialParamMock : PInvokeMock
    {
        #region Fields

        private int _value;
        private bool _returnIncorrectSize;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasGetAutodialParamMock"/> class.
        /// </summary>
        public RasGetAutodialParamMock()
            : this(NativeMethods.SUCCESS)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasGetAutodialParamMock"/> class.
        /// </summary>
        /// <param name="returnValue">The value to return when the method is called.</param>
        public RasGetAutodialParamMock(int returnValue)
        {
            this.ReturnValue = returnValue;

            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to be set in the pointer.
        /// </summary>
        public int Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the size output will be incorrect.
        /// </summary>
        public bool ReturnIncorrectSize
        {
            get { return this._returnIncorrectSize; }
            set { this._returnIncorrectSize = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the mock instance.
        /// </summary>
        protected override void Initialize()
        {
            int lpdwcbValue = 0;

            Isolate.Fake.StaticMethods<UnsafeNativeMethods>();

            if (this.ReturnValue == NativeMethods.SUCCESS)
            {
                Isolate.WhenCalled(() => UnsafeNativeMethods.GetAutodialParam(NativeMethods.RASADP.ConnectionQueryTimeout, IntPtr.Zero, ref lpdwcbValue)).DoInstead(new Func<MethodCallContext, int>(this.Execute));
            }
            else
            {
                Isolate.WhenCalled(() => UnsafeNativeMethods.GetAutodialParam(NativeMethods.RASADP.ConnectionQueryTimeout, IntPtr.Zero, ref lpdwcbValue)).WillReturn(this.ReturnValue);
            }

            base.Initialize();
        }

        /// <summary>
        /// Called when the method is executed by the test.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <returns>The result of the p/invoke operation.</returns>
        private int Execute(MethodCallContext context)
        {
            IntPtr lpvValue = (IntPtr)context.Parameters[1];
            int size = (int)context.Parameters[2];

            if (size < sizeof(int))
            {
                context.Parameters[2] = sizeof(int);
                this.ReturnValue = NativeMethods.ERROR_BUFFER_TOO_SMALL;
            }
            else
            {
                Marshal.WriteInt32(lpvValue, this.Value);

                if (this.ReturnIncorrectSize)
                {
                    context.Parameters[2] = sizeof(long);
                }
                else
                {
                    context.Parameters[2] = sizeof(int);
                }

                this.ReturnValue = NativeMethods.SUCCESS;
            }

            return this.ReturnValue;
        }

        #endregion
    }
}