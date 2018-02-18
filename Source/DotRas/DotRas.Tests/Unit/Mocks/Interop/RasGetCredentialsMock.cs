//--------------------------------------------------------------------------
// <copyright file="RasGetCredentialsMock.cs" company="Jeff Winn">
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
    /// Represents a mock instance of the <see cref="UnsafeNativeMethods.GetCredentials"/> method. This class cannot be inherited.
    /// </summary>
    internal sealed class RasGetCredentialsMock : PInvokeMock
    {
        #region Fields

        private NativeMethods.RASCREDENTIALS _credentials;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasGetCredentialsMock"/> class.
        /// </summary>
        public RasGetCredentialsMock()
        {
            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the credentials to return.
        /// </summary>
        public NativeMethods.RASCREDENTIALS Credentials
        {
            get { return this._credentials; }
            set { this._credentials = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the mock instance.
        /// </summary>
        protected override void Initialize()
        {
            Isolate.Fake.StaticMethods<UnsafeNativeMethods>();
            Isolate.WhenCalled(() => UnsafeNativeMethods.GetCredentials(null, null, IntPtr.Zero)).DoInstead(new Func<MethodCallContext, int>(this.Execute));

            base.Initialize();
        }

        /// <summary>
        /// Called when the method is executed by the test.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <returns>The result of the p/invoke operation.</returns>
        private int Execute(MethodCallContext context)
        {
            IntPtr lpCredentials = (IntPtr)context.Parameters[2];

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.userName = this.Credentials.userName;
            credentials.password = this.Credentials.password;
            credentials.domain = this.Credentials.domain;
            credentials.options = this.Credentials.options;

            Marshal.StructureToPtr(credentials, lpCredentials, true);

            return this.ReturnValue;
        }

        #endregion
    }
}