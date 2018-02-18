//--------------------------------------------------------------------------
// <copyright file="RasGetAutodialEnableMock.cs" company="Jeff Winn">
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
    using TypeMock;
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// Represents a mock instance of the <see cref="UnsafeNativeMethods.GetAutodialEnable"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasGetAutodialEnableMock : PInvokeMock
    {
        #region Fields

        private bool _enabled;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasGetAutodialEnableMock"/> class.
        /// </summary>
        public RasGetAutodialEnableMock()
        {
            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the dialing location is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return this._enabled; }
            set { this._enabled = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the mock instance.
        /// </summary>
        protected override void Initialize()
        {
            bool lpfEnabled = false;

            Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
            Isolate.WhenCalled(() => UnsafeNativeMethods.GetAutodialEnable(0, ref lpfEnabled)).DoInstead(new Func<MethodCallContext, int>(this.Execute));

            base.Initialize();
        }

        /// <summary>
        /// Called when the method is executed by the test.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <returns>The result of the p/invoke operation.</returns>
        private int Execute(MethodCallContext context)
        {
            context.Parameters[1] = this.Enabled;

            return this.ReturnValue;
        }

        #endregion
    }
}