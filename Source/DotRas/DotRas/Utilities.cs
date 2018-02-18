//--------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Jeff Winn">
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
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using DotRas.Properties;

    /// <summary>
    /// Contains utility methods for the assembly.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Determines whether the handle is invalid or closed.
        /// </summary>
        /// <param name="handle">A <see cref="DotRas.RasHandle"/> to check.</param>
        /// <returns><b>true</b> if the handle is invalid or closed, otherwise <b>false</b>.</returns>
        public static bool IsHandleInvalidOrClosed(RasHandle handle)
        {
            return handle == null || handle.IsInvalid || handle.IsClosed;
        }

        /// <summary>
        /// Creates a new array of <typeparamref name="T"/> objects contained at the pointer specified.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the pointer.</typeparam>
        /// <param name="ptr">Required. An <see cref="System.IntPtr"/> containing data.</param>
        /// <param name="size">Required. The size of each item at the pointer</param>
        /// <param name="count">Required. The number of items at the pointer.</param>
        /// <returns>An new array of <typeparamref name="T"/> objects.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static T[] CreateArrayOfType<T>(IntPtr ptr, int size, int count)
        {
            T[] retval = new T[count];

            for (int pos = 0; pos < count; pos++)
            {
                IntPtr tempPtr = new IntPtr(ptr.ToInt64() + (pos * size));

                retval[pos] = (T)Marshal.PtrToStructure(tempPtr, typeof(T));
            }

            return retval;
        }

        /// <summary>
        /// Copies an existing array to a new pointer.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the array.</typeparam>
        /// <param name="array">The array of objects to copy.</param>
        /// <param name="size">Upon return contains the size of each object in the array.</param>
        /// <param name="totalSize">Upon return contains the total size of the buffer.</param>
        /// <returns>The pointer to the structures.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static IntPtr CopyObjectsToNewPtr<T>(T[] array, ref int size, out int totalSize)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException("array");
            }

            if (size == 0)
            {
                size = Marshal.SizeOf(typeof(T));
            }

            totalSize = array.Length * size;

            IntPtr ptr = Marshal.AllocHGlobal(totalSize);
            CopyObjectsToPtr<T>(array, ptr, ref size);

            return ptr;
        }

        /// <summary>
        /// Copies an existing array to a pointer.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the array.</typeparam>
        /// <param name="array">The array of objects to copy.</param>
        /// <param name="ptr">The <see cref="System.IntPtr"/> the array will be copied to.</param>
        /// <param name="size">Upon return contains the size of each object in the array.</param>
        public static void CopyObjectsToPtr<T>(T[] array, IntPtr ptr, ref int size)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException("array");
            }

            if (size == 0)
            {
                size = Marshal.SizeOf(typeof(T));
            }

            for (int pos = 0; pos < array.Length; pos++)
            {
                IntPtr tempPtr = new IntPtr(ptr.ToInt64() + (pos * size));

                Marshal.StructureToPtr(array[pos], tempPtr, true);
            }
        }

        /// <summary>
        /// Creates a new collection of strings contained at the pointer specified.
        /// </summary>
        /// <param name="ptr">The <see cref="System.IntPtr"/> where the data is located in memory.</param>
        /// <param name="offset">The offset from <paramref name="ptr"/> where the data is located.</param>
        /// <param name="count">The number of strings in the collection.</param>
        /// <returns>A new collection of strings.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static Collection<string> CreateStringCollection(IntPtr ptr, int offset, int count)
        {
            Collection<string> retval = new Collection<string>();

            IntPtr pItem = new IntPtr(ptr.ToInt64() + offset);

            bool abort = false;
            int pos = 0;

            do
            {
                string item = Marshal.PtrToStringUni(pItem);
                if (string.IsNullOrEmpty(item))
                {
                    break;
                }
                else
                {
                    retval.Add(item);

                    pItem = new IntPtr(pItem.ToInt64() + (item.Length * 2) + 2);
                    pos++;
                }
            }
            while ((count != 0 && pos < count) && !abort);

            return retval;
        }

        /// <summary>
        /// Copies a string to the pointer at the offset specified.
        /// </summary>
        /// <param name="ptr">The pointer where the string should be copied.</param>
        /// <param name="offset">The offset from the pointer where the string will be copied.</param>
        /// <param name="value">The string to copy to the pointer.</param>
        /// <param name="length">The length of the string to copy.</param>
        /// <exception cref="System.ArgumentException"><paramref name="length"/> is longer than the <paramref name="value"/> specified.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is a null reference (<b>Nothing</b> in Visual Basic).<paramref name=""/></exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void CopyString(IntPtr ptr, int offset, string value, int length)
        {
            if (value == null)
            {
                ThrowHelper.ThrowArgumentNullException("value");
            }

            if (length > value.Length)
            {
                ThrowHelper.ThrowArgumentException("length", Resources.Argument_ValueLongerThanString);
            }

            IntPtr pDestination = new IntPtr(ptr.ToInt64() + offset);

            IntPtr pSource = IntPtr.Zero;
            try
            {
                pSource = Marshal.StringToHGlobalUni(value);
                if (pSource != IntPtr.Zero)
                {
                    UnsafeNativeMethods.CopyMemory(pDestination, pSource, new IntPtr(length));
                }
            }
            finally
            {
                if (pSource != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pSource);
                }
            }
        }

        /// <summary>
        /// Builds a string list from the collection of strings provided.
        /// </summary>
        /// <param name="collection">The collection of strings to use.</param>
        /// <param name="separatorChar">The character used to separate the strings in the collection.</param>
        /// <param name="length">Upon return, contains the length of the resulting string.</param>
        /// <returns>The concatenated collection of strings.</returns>
        public static string BuildStringList(Collection<string> collection, char separatorChar, out int length)
        {
            StringBuilder sb = new StringBuilder();

            if (collection != null && collection.Count > 0)
            {
                foreach (string value in collection)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        sb.Append(value).Append(separatorChar);
                    }
                }

                sb.Append(separatorChar);
            }

            length = sb.Length * 2;

            return sb.ToString();
        }

        /// <summary>
        /// Gets a <see cref="NativeMethods.RASIPADDR"/> for the <paramref name="value"/> specified.
        /// </summary>
        /// <param name="value">Required. An <see cref="System.Net.IPAddress"/> to use.</param>
        /// <returns>A new <see cref="NativeMethods.RASIPADDR"/> structure.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="value"/> is the wrong address family.</exception>
        public static NativeMethods.RASIPADDR GetRasIPAddress(IPAddress value)
        {
            NativeMethods.RASIPADDR retval = new NativeMethods.RASIPADDR();

            if (value != null)
            {
                if (value.AddressFamily != AddressFamily.InterNetwork)
                {
                    ThrowHelper.ThrowArgumentException("value", Resources.Argument_IncorrectAddressFamily);
                }

                if (value == null)
                {
                    retval.addr = IPAddress.Any.GetAddressBytes();
                }
                else
                {
                    retval.addr = value.GetAddressBytes();
                }
            }

            return retval;
        }

#if (WIN2K8)
        /// <summary>
        /// Gets a <see cref="NativeMethods.RASIPV6ADDR"/> for the <paramref name="value"/> specified.
        /// </summary>
        /// <param name="value">Required. An <see cref="System.Net.IPAddress"/> to use.</param>
        /// <returns>A new <see cref="NativeMethods.RASIPADDR"/> structure.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="value"/> is the wrong address family.</exception>
        public static NativeMethods.RASIPV6ADDR GetRasIPv6Address(IPAddress value)
        {
            NativeMethods.RASIPV6ADDR retval = new NativeMethods.RASIPV6ADDR();

            if (value != null)
            {
                if (value.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    ThrowHelper.ThrowArgumentException("value", Resources.Argument_IncorrectAddressFamily);
                }

                if (value == null)
                {
                    retval.addr = IPAddress.IPv6Any.GetAddressBytes();
                }
                else
                {
                    retval.addr = value.GetAddressBytes();
                }
            }

            return retval;
        }
#endif
    }
}