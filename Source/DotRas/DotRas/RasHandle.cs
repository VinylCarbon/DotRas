//--------------------------------------------------------------------------
// <copyright file="RasHandle.cs" company="Jeff Winn">
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
    using System.Security.Permissions;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Represents a wrapper class for remote access service (RAS) handles. This class cannot be inherited.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public sealed class RasHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasHandle"/> class.
        /// </summary>
        public RasHandle()
            : base(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasHandle"/> class.
        /// </summary>
        /// <param name="handle">The handle to use.</param>
        internal RasHandle(IntPtr handle)
            : base(false)
        {
            this.SetHandle(handle);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases the handle.
        /// </summary>
        /// <returns><b>true</b> if the handle was released successfully, otherwise <b>false</b>.</returns>
        /// <remarks>This method will never release the handle, doing so would disconnect the client when the object is finalized.</remarks>
        protected override bool ReleaseHandle()
        {
            return true;
        }

        #endregion
    }
}