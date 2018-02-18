//--------------------------------------------------------------------------
// <copyright file="RasSetEntryPropertiesMock.cs" company="Jeff Winn">
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
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// Represents a mock instance of the <see cref="UnsafeNativeMethods.SetEntryProperties"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasSetEntryPropertiesMock : PInvokeMock
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasSetEntryPropertiesMock"/> class.
        /// </summary>
        /// <param name="returnValue">The value to return when the method is called.</param>
        public RasSetEntryPropertiesMock(int returnValue)
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
            Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
            Isolate.WhenCalled(() => UnsafeNativeMethods.SetEntryProperties(null, null, IntPtr.Zero, 0, IntPtr.Zero, 0)).WillReturn(this.ReturnValue);

            base.Initialize();
        }

        #endregion
    }
}