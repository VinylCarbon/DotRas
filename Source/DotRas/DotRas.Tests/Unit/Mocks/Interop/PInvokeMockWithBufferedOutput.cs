//--------------------------------------------------------------------------
// <copyright file="PInvokeMockWithBufferedOutput.cs" company="Jeff Winn">
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

    /// <summary>
    /// Provides the base class for all p/invoke mock methods that copy objects into an output buffer. This class must be inherited.
    /// </summary>
    /// <typeparam name="TObject">The type of object being copied into the buffer.</typeparam>
    internal abstract class PInvokeMockWithBufferedOutput<TObject> : PInvokeMock
    {
        #region Fields

        private TObject[] _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PInvokeMockWithBufferedOutput&lt;TObject&gt;"/> class.
        /// </summary>
        protected PInvokeMockWithBufferedOutput()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of objects to place in the buffer.
        /// </summary>
        public TObject[] Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies the objects into the buffer.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <param name="ptr">The pointer where the data is being copied.</param>
        /// <param name="lpCb">The size of the buffer.</param>
        protected void CopyObjectsToBuffer(MethodCallContext context, IntPtr ptr, IntPtr lpCb)
        {
            int size = Marshal.SizeOf(typeof(TObject));
            long expectedSize = size * this.Value.Length;

            if (lpCb.ToInt64() != expectedSize && expectedSize > 0)
            {
                this.UpdateBufferSize(context, new IntPtr(expectedSize));
                this.ReturnValue = NativeMethods.ERROR_BUFFER_TOO_SMALL;
            }
            else if (this.Value.Length > 0)
            {
                Utilities.CopyObjectsToPtr<TObject>(this.Value, ptr, ref size);
                this.UpdateOutputCount(context, new IntPtr(this.Value.Length));

                this.ReturnValue = NativeMethods.SUCCESS;
            }
            else
            {
                this.UpdateOutputCount(context, IntPtr.Zero);

                this.ReturnValue = NativeMethods.SUCCESS;
            }
        }

        /// <summary>
        /// Updates the buffer size of the mock context.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <param name="value">The new value.</param>
        protected abstract void UpdateBufferSize(MethodCallContext context, IntPtr value);

        /// <summary>
        /// Updates the number of objects in the buffer for the mock context.
        /// </summary>
        /// <param name="context">The context of the mock.</param>
        /// <param name="value">The new value.</param>
        protected abstract void UpdateOutputCount(MethodCallContext context, IntPtr value);

        #endregion
    }
}