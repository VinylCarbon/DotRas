//--------------------------------------------------------------------------
// <copyright file="RasEnumAutodialAddressesMock.cs" company="Jeff Winn">
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
    using System.Collections.ObjectModel;
    using System.Runtime.InteropServices;
    using System.Text;
    using TypeMock;
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// Represents a mock instance of the <see cref="UnsafeNativeMethods.GetAutodialAddresses"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasEnumAutodialAddressesMock : PInvokeMock
    {
        #region Fields

        private Collection<string> _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEnumAutodialAddressesMock"/> class.
        /// </summary>
        public RasEnumAutodialAddressesMock()
            : this(NativeMethods.SUCCESS)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEnumAutodialAddressesMock"/> class.
        /// </summary>
        /// <param name="returnValue">The value to return when the method is called.</param>
        public RasEnumAutodialAddressesMock(int returnValue)
        {
            this.ReturnValue = returnValue;

            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of strings to return to the caller.
        /// </summary>
        public Collection<string> Value
        {
            get
            {
                if (this._value == null)
                {
                    this._value = new Collection<string>();
                }

                return this._value;
            }

            set
            {
                this._value = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the mock instance.
        /// </summary>
        protected override void Initialize()
        {
            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcAddresses = IntPtr.Zero;

            Isolate.Fake.StaticMethods<UnsafeNativeMethods>();

            if (this.ReturnValue == NativeMethods.SUCCESS)
            {
                Isolate.WhenCalled(() => UnsafeNativeMethods.EnumAutodialAddresses(IntPtr.Zero, ref lpCb, ref lpcAddresses)).DoInstead(new Func<MethodCallContext, int>(this.Execute));
            }
            else
            {
                Isolate.WhenCalled(() => UnsafeNativeMethods.EnumAutodialAddresses(IntPtr.Zero, ref lpCb, ref lpcAddresses)).WillReturn(this.ReturnValue);
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
            IntPtr lppAddresses = (IntPtr)context.Parameters[0];
            IntPtr lpCb = (IntPtr)context.Parameters[1];
            IntPtr lpcAddresses = (IntPtr)context.Parameters[2];

            StringBuilder sb = new StringBuilder();
            foreach (string value in this.Value)
            {
                sb.Append(value).Append('\x00');
            }

            int expectedSize = (sb.Length * 2) + (this.Value.Count * IntPtr.Size);
            if (lpCb.ToInt64() < expectedSize)
            {
                context.Parameters[1] = new IntPtr(expectedSize);
                this.ReturnValue = NativeMethods.ERROR_BUFFER_TOO_SMALL;
            }
            else if (this.Value.Count > 0)
            {
                IntPtr lpAddresses = IntPtr.Zero;
                try
                {
                    lpAddresses = Marshal.StringToHGlobalUni(sb.ToString());

                    IntPtr lpDestination = new IntPtr(lppAddresses.ToInt64() + (this.Value.Count * IntPtr.Size));
                    UnsafeNativeMethods.CopyMemory(lpDestination, lpAddresses, new IntPtr(sb.Length * 2));
                }
                finally
                {
                    if (lpAddresses != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(lpAddresses);
                    }
                }

                context.Parameters[2] = new IntPtr(this.Value.Count);
                this.ReturnValue = NativeMethods.SUCCESS;
            }

            return this.ReturnValue;
        }

        #endregion
    }
}