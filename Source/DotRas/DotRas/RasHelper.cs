//--------------------------------------------------------------------------
// <copyright file="RasHelper.cs" company="Jeff Winn">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;
    using DotRas.Properties;

    /// <summary>
    /// Provides methods to interact with the remote access service (RAS) application programming interface.
    /// </summary>
    internal class RasHelper
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DotRas.RasHelper"/> class from being created.
        /// </summary>
        private RasHelper()
        {
        }

        #endregion

        /// <summary>
        /// Establishes a remote access connection between a client and a server.
        /// </summary>
        /// <param name="phoneBook">The full path and filename of a phone book. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="parameters">A <see cref="NativeMethods.RASDIALPARAMS"/> structure containing calling parameters for the connection.</param>
        /// <param name="extensions">A <see cref="NativeMethods.RASDIALEXTENSIONS"/> structure containing extended feature information.</param>
        /// <param name="callback">A <see cref="NativeMethods.RasDialFunc2"/> delegate to notify during the connection process.</param>
        /// <param name="eapOptions">Specifies options to use during authentication.</param>
        /// <returns>The handle of the connection.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="parameters"/> contains an empty or null reference (<b>Nothing</b> in Visual Basic) for both the entry name and phone numbers.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasHandle Dial(string phoneBook, NativeMethods.RASDIALPARAMS parameters, NativeMethods.RASDIALEXTENSIONS extensions, NativeMethods.RasDialFunc2 callback, RasEapOptions eapOptions)
        {
            if (string.IsNullOrEmpty(parameters.entryName) && string.IsNullOrEmpty(parameters.phoneNumber))
            {
                ThrowHelper.ThrowArgumentException("parameters", Resources.Argument_EmptyEntryNameAndPhoneNumber);
            }

            if (phoneBook != null && phoneBook.Length == 0)
            {
                // The phone book path provided was an empty string, set it to null to use the default phone book.
                phoneBook = null;
            }

            IntPtr lpRasDialExtensions = IntPtr.Zero;
            RasHandle handle = null;

            NativeMethods.RASEAPUSERIDENTITY identity = new NativeMethods.RASEAPUSERIDENTITY();
            try
            {
                int extensionsSize = Marshal.SizeOf(typeof(NativeMethods.RASDIALEXTENSIONS));

                extensions = new NativeMethods.RASDIALEXTENSIONS();
                extensions.size = extensionsSize;

                bool eapUserIdentityRetrieved = false;
                try
                {
                    if (!string.IsNullOrEmpty(parameters.entryName))
                    {
                        eapUserIdentityRetrieved = RasHelper.TryGetEapUserIdentity(phoneBook, parameters.entryName, eapOptions, extensions.handle, out identity);
                        if (eapUserIdentityRetrieved)
                        {
                            // The EAP information was returned for this connection, copy the information into the dial extensions.
                            NativeMethods.RASEAPINFO rasEapInfo = new NativeMethods.RASEAPINFO();
                            rasEapInfo.sizeOfEapData = identity.sizeOfEapData;
                            rasEapInfo.eapData = identity.eapData;

                            extensions.eapInfo = rasEapInfo;
                        }
                    }

                    lpRasDialExtensions = Marshal.AllocHGlobal(extensionsSize);
                    Marshal.StructureToPtr(extensions, lpRasDialExtensions, true);

                    IntPtr lpRasDialParams = IntPtr.Zero;
                    try
                    {
                        int parametersSize = Marshal.SizeOf(typeof(NativeMethods.RASDIALPARAMS));
                        parameters.size = parametersSize;

                        lpRasDialParams = Marshal.AllocHGlobal(parametersSize);
                        Marshal.StructureToPtr(parameters, lpRasDialParams, true);

                        int ret = SafeNativeMethods.RasDial(lpRasDialExtensions, phoneBook, lpRasDialParams, NativeMethods.RasNotifierType.RasDialFunc2, callback, out handle);
                        if (ret != NativeMethods.SUCCESS)
                        {
                            // There was an error during the connection attempt, the handle must be released.
                            if (!handle.IsInvalid)
                            {
                                RasHelper.HangUp(handle);
                            }

                            ThrowHelper.ThrowRasException(ret);
                        }
                    }
                    catch (EntryPointNotFoundException)
                    {
                        ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                    }
                    finally
                    {
                        if (lpRasDialParams != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(lpRasDialParams);
                        }
                    }
                }
                finally
                {
                    if (eapUserIdentityRetrieved)
                    {
                        // Free any EAP identity information that was obtained earlier during the connection process if available.
                        RasHelper.FreeEapUserIdentity(identity);
                    }
                }
            }
            finally
            {
                if (lpRasDialExtensions != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpRasDialExtensions);
                }
            }

            return handle;
        }

        /// <summary>
        /// Indicates the current AutoDial status for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dialingLocation">The dialing location whose status to check.</param>
        /// <returns><b>true</b> if the AutoDial feature is currently enabled for the dialing location, otherwise <b>false</b>.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool GetAutoDialEnable(int dialingLocation)
        {
            bool retval = false;

            try
            {
                int ret = UnsafeNativeMethods.GetAutodialEnable(dialingLocation, ref retval);
                if (ret != NativeMethods.SUCCESS)
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Retrieves the value of an AutoDial parameter.
        /// </summary>
        /// <param name="parameter">The parameter whose value to retrieve.</param>
        /// <returns>The value of the parameter.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static int GetAutoDialParameter(NativeMethods.RASADP parameter)
        {
            int retval = 0;
            int size = 0;

            bool retry = false;
            do
            {
                IntPtr pValue = IntPtr.Zero;
                try
                {
                    pValue = Marshal.AllocHGlobal(size);

                    int ret = UnsafeNativeMethods.GetAutodialParam(parameter, pValue, ref size);
                    if (ret == NativeMethods.SUCCESS)
                    {
                        if (size == sizeof(int))
                        {
                            retval = Marshal.ReadInt32(pValue);
                        }
                        else
                        {
                            ThrowHelper.ThrowInvalidOperationException(Resources.Exception_UnexpectedSizeReturned);
                        }

                        retry = false;
                    }
                    else if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                    {
                        retry = true;
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                finally
                {
                    if (pValue != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pValue);
                    }
                }
            }
            while (retry);

            return retval;
        }

        /// <summary>
        /// Clears any accumulated statistics for the specified remote access connection.
        /// </summary>
        /// <param name="handle">The handle to the connection.</param>
        /// <returns><b>true</b> if the function succeeds, otherwise <b>false</b>.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool ClearConnectionStatistics(RasHandle handle)
        {
            bool retval = false;

            try
            {
                int ret = SafeNativeMethods.ClearConnectionStatistics(handle);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else if (ret == NativeMethods.ERROR_INVALID_HANDLE)
                {
                    ThrowHelper.ThrowInvalidHandleException(handle, "handle", Resources.Argument_InvalidHandle);
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Clears any accumulated statistics for the specified link in a remote access multilink connection.
        /// </summary>
        /// <param name="handle">The handle to the connection.</param>
        /// <param name="subEntryId">The subentry index that corresponds to the link for which to clear statistics.</param>
        /// <returns><b>true</b> if the function succeeds, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="subEntryId"/> must be greater than zero.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool ClearLinkStatistics(RasHandle handle, int subEntryId)
        {
            if (subEntryId <= 0)
            {
                ThrowHelper.ThrowArgumentException("subEntryId", Resources.Argument_ValueCannotBeLessThanOrEqualToZero);
            }

            bool retval = false;

            try
            {
                int ret = SafeNativeMethods.ClearLinkStatistics(handle, subEntryId);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else if (ret == NativeMethods.ERROR_INVALID_HANDLE)
                {
                    ThrowHelper.ThrowInvalidHandleException(handle, "handle", Resources.Argument_InvalidHandle);
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Deletes an entry from a phone book.
        /// </summary>
        /// <param name="phoneBook">Required. The full path (including file name) of the phone book.</param>
        /// <param name="entryName">Required. The name of the entry to delete.</param>
        /// <returns><b>true</b> if the entry was deleted, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="phoneBook"/> or <paramref name="entryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool DeleteEntry(string phoneBook, string entryName)
        {
            if (string.IsNullOrEmpty(phoneBook))
            {
                ThrowHelper.ThrowArgumentException("phoneBook", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            bool retval = false;

            try
            {
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, phoneBook);
                permission.Demand();

                int ret = UnsafeNativeMethods.DeleteEntry(phoneBook, entryName);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else if (ret == NativeMethods.ERROR_ACCESS_DENIED)
                {
                    ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            catch (SecurityException)
            {
                ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
            }

            return retval;
        }

#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Deletes a subentry from the specified phone book entry.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> containing the entry.</param>
        /// <param name="entry">Required. The <see cref="DotRas.RasEntry"/> containing the subentry to be deleted.</param>
        /// <param name="subEntryId">The one-based index of the subentry to delete.</param>
        /// <returns><b>true</b> if the function succeeds, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> or <paramref name="entry"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool DeleteSubEntry(RasPhoneBook phoneBook, RasEntry entry, int subEntryId)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            if (entry == null)
            {
                ThrowHelper.ThrowArgumentNullException("entry");
            }

            if (subEntryId <= 0)
            {
                ThrowHelper.ThrowArgumentException("subEntryId", Resources.Argument_ValueCannotBeLessThanOrEqualToZero);
            }

            bool retval = false;

            try
            {
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, phoneBook.Path);
                permission.Demand();

                retval = UnsafeNativeMethods.RasDeleteSubEntry(phoneBook.Path, entry.Name, subEntryId) == NativeMethods.SUCCESS;
            }
            catch (SecurityException)
            {
                ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
            }

            return retval;
        }
#endif

        /// <summary>
        /// Retrieves a read-only list of active connections.
        /// </summary>
        /// <returns>A new read-only collection of <see cref="DotRas.RasConnection"/> objects, or an empty collection if no active connections were found.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static ReadOnlyCollection<RasConnection> GetActiveConnections()
        {
            ReadOnlyCollection<RasConnection> retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASCONN));
            IntPtr lpCb = new IntPtr(size);
            IntPtr lpcConnections = IntPtr.Zero;

            bool retry = false;

            do
            {
                NativeMethods.RASCONN conn = new NativeMethods.RASCONN();
                conn.size = size;

                IntPtr pConnections = IntPtr.Zero;
                try
                {
                    pConnections = Marshal.AllocHGlobal(lpCb);
                    Marshal.StructureToPtr(conn, pConnections, true);

                    int ret = SafeNativeMethods.EnumConnections(pConnections, ref lpCb, ref lpcConnections);
                    if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                    {
                        retry = true;
                    }
                    else if (ret == NativeMethods.SUCCESS)
                    {
                        retry = false;

                        NativeMethods.RASCONN[] connections = Utilities.CreateArrayOfType<NativeMethods.RASCONN>(
                            pConnections, size, lpcConnections.ToInt32());
                        RasConnection[] tempArray = null;

                        if (connections == null || connections.Length == 0)
                        {
                            tempArray = new RasConnection[0];
                        }
                        else
                        {
                            tempArray = new RasConnection[connections.Length];

                            for (int index = 0; index < connections.Length; index++)
                            {
                                NativeMethods.RASCONN current = connections[index];

                                RasConnection item = new RasConnection();

                                item.Handle = new RasHandle(current.handle);
                                item.EntryName = current.entryName;
                                item.Device = RasDevice.Create(current.deviceName, current.deviceType);
                                item.PhoneBookPath = current.phoneBook;
                                item.SubEntryId = current.subEntryId;
                                item.EntryId = current.entryId;
#if (WINXP || WINXPSP2 || WIN2K8)
                                item.ConnectionOptions = current.connectionOptions;
                                item.SessionId = current.sessionId;
#endif
#if (WIN2K8)
                                item.CorrelationId = current.correlationId;
#endif

                                tempArray[index] = item;
                            }
                        }

                        retval = new ReadOnlyCollection<RasConnection>(tempArray);
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                finally
                {
                    if (pConnections != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pConnections);
                    }
                }
            } 
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves information about the entries associated with a network address in the AutoDial mapping database.
        /// </summary>
        /// <param name="address">The address to retrieve.</param>
        /// <returns>A new <see cref="DotRas.RasAutoDialAddress"/> object.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasAutoDialAddress GetAutoDialAddress(string address)
        {
            RasAutoDialAddress retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASAUTODIALENTRY));
            IntPtr lpCb = new IntPtr(size);
            IntPtr lpcEntries = IntPtr.Zero;

            bool retry = false;

            do
            {
                NativeMethods.RASAUTODIALENTRY entry = new NativeMethods.RASAUTODIALENTRY();
                entry.size = size;

                IntPtr pAddresses = IntPtr.Zero;
                try
                {
                    pAddresses = Marshal.AllocHGlobal(lpCb);
                    Marshal.StructureToPtr(entry, pAddresses, true);

                    int ret = UnsafeNativeMethods.GetAutodialAddress(address, IntPtr.Zero, pAddresses, ref lpCb, ref lpcEntries);
                    if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                    {
                        retry = true;
                    }
                    else if (ret == NativeMethods.SUCCESS)
                    {
                        retry = false;

                        NativeMethods.RASAUTODIALENTRY[] entries = Utilities.CreateArrayOfType<NativeMethods.RASAUTODIALENTRY>(pAddresses, size, lpcEntries.ToInt32());
                        retval = new RasAutoDialAddress(address);

                        if (entries != null || entries.Length > 0)
                        {
                            for (int index = 0; index < entries.Length; index++)
                            {
                                NativeMethods.RASAUTODIALENTRY current = entries[index];
                                retval.Entries.Add(new RasAutoDialEntry(current.dialingLocation, current.entryName));
                            }
                        }
                    }
                    else if (ret == NativeMethods.ERROR_FILE_NOT_FOUND)
                    {
                        retry = false;
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                finally
                {
                    if (pAddresses != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pAddresses);
                    }
                }
            }
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves a collection of addresses in the AutoDial mapping database.
        /// </summary>
        /// <returns>A new collection of <see cref="DotRas.RasAutoDialAddress"/> objects, or an empty collection if no addresses were found.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static Collection<string> GetAutoDialAddresses()
        {
            Collection<string> retval = null;

            IntPtr lpCb = IntPtr.Zero;
            IntPtr lpcAddresses = IntPtr.Zero;

            bool retry = false;

            do
            {
                IntPtr pAddresses = IntPtr.Zero;

                try
                {
                    pAddresses = Marshal.AllocHGlobal(lpCb);

                    int ret = UnsafeNativeMethods.EnumAutodialAddresses(pAddresses, ref lpCb, ref lpcAddresses);
                    if (ret == NativeMethods.SUCCESS)
                    {
                        if (lpcAddresses.ToInt32() > 0)
                        {
                            int count = lpcAddresses.ToInt32();
                            retval = Utilities.CreateStringCollection(pAddresses, count * IntPtr.Size, count);
                        }

                        retry = false;
                    }
                    else if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                    {
                        retry = true;
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                finally
                {
                    if (pAddresses != IntPtr.Zero)
                    {
                        Marshal.AllocHGlobal(pAddresses);
                    }
                }
            }
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves the connection status for the handle specified.
        /// </summary>
        /// <param name="handle">The remote access connection handle to retrieve.</param>
        /// <returns>A <see cref="DotRas.RasConnectionStatus"/> object containing connection status information.</returns>
        /// <exception cref="DotRas.InvalidHandleException"><paramref name="handle"/> is not a valid handle.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasConnectionStatus GetConnectionStatus(RasHandle handle)
        {
            RasConnectionStatus retval = null;

            IntPtr lpRasConnStatus = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(NativeMethods.RASCONNSTATUS));

                NativeMethods.RASCONNSTATUS status = new NativeMethods.RASCONNSTATUS();
                status.size = size;

                lpRasConnStatus = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(status, lpRasConnStatus, true);

                int ret = SafeNativeMethods.GetConnectStatus(handle, lpRasConnStatus);
                if (ret == NativeMethods.ERROR_INVALID_HANDLE)
                {
                    ThrowHelper.ThrowInvalidHandleException(handle, "handle", Resources.Argument_InvalidHandle);
                }
                else if (ret == NativeMethods.SUCCESS)
                {
                    status = (NativeMethods.RASCONNSTATUS)Marshal.PtrToStructure(lpRasConnStatus, typeof(NativeMethods.RASCONNSTATUS));

                    string errorMessage = null;
                    if (status.errorCode != NativeMethods.SUCCESS)
                    {
                        errorMessage = RasHelper.GetRasErrorString(status.errorCode);
                    }

                    retval = new RasConnectionStatus(
                        status.connectionState,
                        status.errorCode,
                        errorMessage,
                        RasDevice.Create(status.deviceName, status.deviceType),
                        status.phoneNumber);
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (lpRasConnStatus != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpRasConnStatus);
                }
            }

            return retval;
        }

        /// <summary>
        /// Retrieves user credentials associated with a specified remote access phone book entry.
        /// </summary>
        /// <param name="phoneBook">Required. The full path (including filename) of the phone book containing the entry.</param>
        /// <param name="entryName">Required. The name of the entry whose credentials to retrieve.</param>
        /// <param name="options">The options to request.</param>
        /// <returns>The credentials stored in the entry, otherwise a null reference (<b>Nothing</b> in Visual Basic) if the credentials did not exist.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="phoneBook"/> or <paramref name="entryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static NetworkCredential GetCredentials(string phoneBook, string entryName, NativeMethods.RASCM options)
        {
            if (string.IsNullOrEmpty(phoneBook))
            {
                ThrowHelper.ThrowArgumentException("phoneBook", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            NetworkCredential retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASCREDENTIALS));

            NativeMethods.RASCREDENTIALS credentials = new NativeMethods.RASCREDENTIALS();
            credentials.size = size;
            credentials.options = options;

            IntPtr pCredentials = IntPtr.Zero;
            try
            {
                pCredentials = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(credentials, pCredentials, true);

                try
                {
                    FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, phoneBook);
                    permission.Demand();

                    int ret = UnsafeNativeMethods.GetCredentials(phoneBook, entryName, pCredentials);
                    if (ret == NativeMethods.SUCCESS)
                    {
                        credentials = (NativeMethods.RASCREDENTIALS)Marshal.PtrToStructure(pCredentials, typeof(NativeMethods.RASCREDENTIALS));
                        if (credentials.options != NativeMethods.RASCM.None)
                        {
                            retval = new NetworkCredential(
                                credentials.userName,
                                credentials.password,
                                credentials.domain);
                        }
                    }
                    else if (ret != NativeMethods.ERROR_FILE_NOT_FOUND)
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                catch (SecurityException)
                {
                    ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                }
            }
            finally
            {
                if (pCredentials != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pCredentials);
                }
            }

            return retval;
        }

        /// <summary>
        /// Lists all available remote access capable devices.
        /// </summary>
        /// <returns>A new collection of <see cref="DotRas.RasDevice"/> objects.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static ReadOnlyCollection<RasDevice> GetDevices()
        {
            ReadOnlyCollection<RasDevice> retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASDEVINFO));
            IntPtr lpCb = new IntPtr(size);
            IntPtr lpcDevices = IntPtr.Zero;

            bool retry = false;

            do
            {
                NativeMethods.RASDEVINFO device = new NativeMethods.RASDEVINFO();
                device.size = size;

                IntPtr pDevices = IntPtr.Zero;
                try
                {
                    pDevices = Marshal.AllocHGlobal(lpCb);
                    Marshal.StructureToPtr(device, pDevices, true);

                    int ret = SafeNativeMethods.EnumDevices(pDevices, ref lpCb, ref lpcDevices);
                    if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                    {
                        retry = true;
                    }
                    else if (ret == NativeMethods.SUCCESS)
                    {
                        retry = false;

                        NativeMethods.RASDEVINFO[] devices = Utilities.CreateArrayOfType<NativeMethods.RASDEVINFO>(pDevices, size, lpcDevices.ToInt32());
                        RasDevice[] tempArray = null;

                        if (devices == null || devices.Length == 0)
                        {
                            tempArray = new RasDevice[0];
                        }
                        else
                        {
                            tempArray = new RasDevice[devices.Length];

                            for (int index = 0; index < devices.Length; index++)
                            {
                                NativeMethods.RASDEVINFO current = devices[index];

                                tempArray[index] = RasDevice.Create(
                                    current.name,
                                    current.type);
                            }
                        }

                        retval = new ReadOnlyCollection<RasDevice>(tempArray);
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                finally
                {
                    if (pDevices != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pDevices);
                    }
                }
            } 
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves the entry properties for an entry within a phone book.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> containing the entry.</param>
        /// <param name="entryName">Required. The name of an entry to retrieve.</param>
        /// <returns>A <see cref="DotRas.RasEntry"/> object.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="entryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasEntry GetEntryProperties(RasPhoneBook phoneBook, string entryName)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            RasEntry retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASENTRY));
            bool retry = false;

            IntPtr lpCb = new IntPtr(size);
            do
            {
                NativeMethods.RASENTRY entry = new NativeMethods.RASENTRY();
                entry.size = size;

                IntPtr lpRasEntry = IntPtr.Zero;
                try
                {
                    lpRasEntry = Marshal.AllocHGlobal(lpCb);
                    Marshal.StructureToPtr(entry, lpRasEntry, true);

                    try
                    {
                        FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, phoneBook.Path);
                        permission.Demand();

                        int ret = UnsafeNativeMethods.RasGetEntryProperties(phoneBook.Path, entryName, lpRasEntry, ref lpCb, IntPtr.Zero, IntPtr.Zero);
                        if (ret == NativeMethods.SUCCESS)
                        {
                            entry = (NativeMethods.RASENTRY)Marshal.PtrToStructure(lpRasEntry, typeof(NativeMethods.RASENTRY));

                            retval = new RasEntry(entryName);

                            if (entry.alternateOffset > 0)
                            {
                                retval.AlternatePhoneNumbers = Utilities.CreateStringCollection(lpRasEntry, entry.alternateOffset, 0);
                            }

                            if (entry.subentries > 1)
                            {
                                // The first subentry in the collection is always the default entry, need to check if there are two or
                                // more subentries before loading the collection.
                                retval.SubEntries.Load(phoneBook, entry.subentries - 1);
                            }

                            retval.AreaCode = entry.areaCode;

// This warning is being disabled since the object is being loaded by the Win32 API and must have the
// data placed into the object.
#pragma warning disable 0618
                            retval.AutoDialDll = entry.autoDialDll;
                            retval.AutoDialFunc = entry.autoDialFunc;
#pragma warning restore 0618

                            retval.Channels = entry.channels;
                            retval.CountryCode = entry.countryCode;
                            retval.CountryId = entry.countryId;
                            retval.CustomAuthKey = entry.customAuthKey;
                            retval.CustomDialDll = entry.customDialDll;
                            retval.Device = RasDevice.Create(entry.deviceName, entry.deviceType);
                            retval.DialExtraPercent = entry.dialExtraPercent;
                            retval.DialExtraSampleSeconds = entry.dialExtraSampleSeconds;
                            retval.DialMode = entry.dialMode;
                            retval.DnsAddress = new IPAddress(entry.dnsAddress.addr);
                            retval.DnsAddressAlt = new IPAddress(entry.dnsAddressAlt.addr);
                            retval.EncryptionType = entry.encryptionType;
                            retval.EntryType = entry.entryType;
                            retval.FrameSize = entry.frameSize;
                            retval.FramingProtocol = entry.framingProtocol;
                            retval.HangUpExtraPercent = entry.hangUpExtraPercent;
                            retval.HangUpExtraSampleSeconds = entry.hangUpExtraSampleSeconds;
                            retval.Id = entry.id;
                            retval.IdleDisconnectSeconds = entry.idleDisconnectSeconds;
                            retval.IPAddress = new IPAddress(entry.ipAddress.addr);
                            retval.NetworkProtocols = entry.networkProtocols;
                            retval.Options = entry.options;
                            retval.PhoneNumber = entry.phoneNumber;
                            retval.Reserved1 = entry.reserved1;
                            retval.Reserved2 = entry.reserved2;
                            retval.Script = entry.script;
                            retval.VpnStrategy = entry.vpnStrategy;
                            retval.WinsAddress = new IPAddress(entry.winsAddress.addr);
                            retval.WinsAddressAlt = new IPAddress(entry.winsAddressAlt.addr);
                            retval.X25Address = entry.x25Address;
                            retval.X25Facilities = entry.x25Facilities;
                            retval.X25PadType = entry.x25PadType;
                            retval.X25UserData = entry.x25UserData;

#if (WINXP || WINXPSP2 || WIN2K8)
                            retval.ExtendedOptions = entry.options2;
                            retval.ReservedOptions = entry.options3;
                            retval.DnsSuffix = entry.dnsSuffix;
                            retval.TcpWindowSize = entry.tcpWindowSize;
                            retval.PrerequisitePhoneBook = entry.prerequisitePhoneBook;
                            retval.PrerequisiteEntryName = entry.prerequisiteEntryName;
                            retval.RedialCount = entry.redialCount;
                            retval.RedialPause = entry.redialPause;
#endif
#if (WIN2K8)
                            retval.IPv6DnsAddress = new IPAddress(entry.ipv6DnsAddress.addr);
                            retval.IPv6DnsAddressAlt = new IPAddress(entry.ipv6DnsAddressAlt.addr);
                            retval.IPv4InterfaceMetric = entry.ipv4InterfaceMetric;
                            retval.IPv6InterfaceMetric = entry.ipv6InterfaceMetric;
#endif
                            retry = false;
                        }
                        else if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                        {
                            retry = true;
                        }
                        else
                        {
                            ThrowHelper.ThrowRasException(ret);
                        }
                    }
                    catch (EntryPointNotFoundException)
                    {
                        ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                    }
                    catch (SecurityException)
                    {
                        ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                    }
                }
                finally
                {
                    if (lpRasEntry != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(lpRasEntry);
                    }
                }
            }
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves a connection handle for a subentry of a multilink connection.
        /// </summary>
        /// <param name="handle">The handle of the connection.</param>
        /// <param name="subEntryId">The one-based index of the subentry to whose handle to retrieve.</param>
        /// <returns>The handle of the subentry if available, otherwise a null reference (<b>Nothing</b> in Visual Basic).</returns>
        /// <exception cref="System.ArgumentException"><paramref name="subEntryId"/> cannot be less than or equal to zero.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="handle"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasHandle GetSubEntryHandle(RasHandle handle, int subEntryId)
        {
            if (Utilities.IsHandleInvalidOrClosed(handle))
            {
                ThrowHelper.ThrowArgumentNullException("handle");
            }

            if (subEntryId <= 0)
            {
                ThrowHelper.ThrowArgumentException("subEntryId", Resources.Argument_ValueCannotBeLessThanOrEqualToZero);
            }

            RasHandle retval = null;

            try
            {
                RasHandle tempHandle = null;
                int ret = SafeNativeMethods.RasGetSubEntryHandle(handle, subEntryId, out tempHandle);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = tempHandle;
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Retrieves the subentry properties for an entry within a phone book.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> containing the entry.</param>
        /// <param name="entry">Required. The <see cref="DotRas.RasEntry"/> containing the subentry.</param>
        /// <param name="subEntryId">The zero-based index of the subentry to retrieve.</param>
        /// <returns>A new <see cref="DotRas.RasSubEntry"/> object.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> or <paramref name="entry"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasSubEntry GetSubEntryProperties(RasPhoneBook phoneBook, RasEntry entry, int subEntryId)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            if (entry == null)
            {
                ThrowHelper.ThrowArgumentNullException("entry");
            }

            RasSubEntry retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASSUBENTRY));
            bool retry = false;

            IntPtr lpCb = new IntPtr(size);
            do
            {
                NativeMethods.RASSUBENTRY subentry = new NativeMethods.RASSUBENTRY();
                subentry.size = size;

                IntPtr lpRasSubEntry = IntPtr.Zero;
                try
                {
                    lpRasSubEntry = Marshal.AllocHGlobal(lpCb);
                    Marshal.StructureToPtr(subentry, lpRasSubEntry, true);

                    try
                    {
                        FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, phoneBook.Path);
                        permission.Demand();

                        int ret = UnsafeNativeMethods.RasGetSubEntryProperties(phoneBook.Path, entry.Name, subEntryId + 2, lpRasSubEntry, ref lpCb, IntPtr.Zero, IntPtr.Zero);
                        if (ret == NativeMethods.SUCCESS)
                        {
                            subentry = (NativeMethods.RASSUBENTRY)Marshal.PtrToStructure(lpRasSubEntry, typeof(NativeMethods.RASSUBENTRY));

                            retval = new RasSubEntry();

                            retval.Device = RasDevice.Create(subentry.deviceName, subentry.deviceType);
                            retval.PhoneNumber = subentry.phoneNumber;

                            if (subentry.alternateOffset > 0)
                            {
                                retval.AlternatePhoneNumbers = Utilities.CreateStringCollection(lpRasSubEntry, subentry.alternateOffset, 0);
                            }

                            retry = false;
                        }
                        else if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                        {
                            retry = true;
                        }
                        else
                        {
                            ThrowHelper.ThrowRasException(ret);
                        }
                    }
                    catch (EntryPointNotFoundException)
                    {
                        ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                    }
                    catch (SecurityException)
                    {
                        ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                    }
                }
                finally
                {
                    if (lpRasSubEntry != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(lpRasSubEntry);
                    }
                }
            }
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves a list of entry names within a phone book.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> whose entry names to retrieve.</param>
        /// <returns>An array of <see cref="NativeMethods.RASENTRYNAME"/> structures, or a null reference if the phone-book was not found.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static NativeMethods.RASENTRYNAME[] GetEntryNames(RasPhoneBook phoneBook)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            NativeMethods.RASENTRYNAME[] retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASENTRYNAME));
            IntPtr lpCb = new IntPtr(size);
            IntPtr lpcEntries = IntPtr.Zero;

            bool retry = false;

            do
            {
                NativeMethods.RASENTRYNAME entry = new NativeMethods.RASENTRYNAME();
                entry.size = size;

                IntPtr pEntries = IntPtr.Zero;
                try
                {
                    pEntries = Marshal.AllocHGlobal(lpCb);
                    Marshal.StructureToPtr(entry, pEntries, true);

                    try
                    {
                        FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, phoneBook.Path);
                        permission.Demand();

                        int ret = UnsafeNativeMethods.RasEnumEntries(IntPtr.Zero, phoneBook.Path, pEntries, ref lpCb, ref lpcEntries);
                        if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                        {
                            retry = true;
                        }
                        else if (ret == NativeMethods.SUCCESS)
                        {
                            retry = false;

                            int entries = lpcEntries.ToInt32();
                            if (entries > 0)
                            {
                                retval = Utilities.CreateArrayOfType<NativeMethods.RASENTRYNAME>(pEntries, size, entries);
                            }
                        }
                        else
                        {
                            ThrowHelper.ThrowRasException(ret);
                        }
                    }
                    catch (EntryPointNotFoundException)
                    {
                        ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                    }
                    catch (SecurityException)
                    {
                        ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                    }
                }
                finally
                {
                    if (pEntries != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pEntries);
                    }
                }
            } 
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves accumulated statistics for the specified connection.
        /// </summary>
        /// <param name="handle">The handle to the connection.</param>
        /// <returns>A <see cref="DotRas.RasLinkStatistics"/> structure containing connection statistics.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasLinkStatistics GetConnectionStatistics(RasHandle handle)
        {
            if (Utilities.IsHandleInvalidOrClosed(handle))
            {
                ThrowHelper.ThrowArgumentException("handle", Resources.Argument_InvalidHandle);
            }

            RasLinkStatistics retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RAS_STATS));

            NativeMethods.RAS_STATS stats = new NativeMethods.RAS_STATS();
            stats.size = size;

            IntPtr lpStatistics = IntPtr.Zero;
            try
            {
                lpStatistics = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(stats, lpStatistics, true);

                int ret = SafeNativeMethods.RasGetConnectionStatistics(handle, lpStatistics);
                if (ret == NativeMethods.SUCCESS)
                {
                    stats = (NativeMethods.RAS_STATS)Marshal.PtrToStructure(lpStatistics, typeof(NativeMethods.RAS_STATS));

                    retval = new RasLinkStatistics(
                        stats.bytesTransmitted,
                        stats.bytesReceived,
                        stats.framesTransmitted,
                        stats.framesReceived,
                        stats.crcError,
                        stats.timeoutError,
                        stats.alignmentError,
                        stats.hardwareOverrunError,
                        stats.framingError,
                        stats.bufferOverrunError,
                        stats.compressionRatioIn,
                        stats.compressionRatioOut,
                        stats.linkSpeed,
                        TimeSpan.FromMilliseconds(stats.connectionDuration));
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (lpStatistics != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpStatistics);
                }
            }

            return retval;
        }

        /// <summary>
        /// Retrieves country/region specific dialing information from the Windows Telephony list of countries/regions for a specific country id.
        /// </summary>
        /// <param name="countryId">The country id to retrieve.</param>
        /// <param name="nextCountryId">Upon output, contains the next country id from the list; otherwise zero for the last country/region in the list.</param>
        /// <returns>A new <see cref="DotRas.RasCountry"/> object.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasCountry GetCountry(int countryId, out int nextCountryId)
        {
            RasCountry retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASCTRYINFO));
            IntPtr lpdwSize = new IntPtr(size);
            nextCountryId = 0;

            bool retry = false;

            do
            {
                NativeMethods.RASCTRYINFO country = new NativeMethods.RASCTRYINFO();
                country.size = size;
                country.countryId = countryId;

                IntPtr lpRasCtryInfo = IntPtr.Zero;
                try
                {
                    lpRasCtryInfo = Marshal.AllocHGlobal(lpdwSize);
                    Marshal.StructureToPtr(country, lpRasCtryInfo, true);

                    int ret = SafeNativeMethods.GetCountryInfo(lpRasCtryInfo, ref lpdwSize);
                    if (ret == NativeMethods.ERROR_BUFFER_TOO_SMALL)
                    {
                        retry = true;
                    }
                    else if (ret == NativeMethods.SUCCESS)
                    {
                        retry = false;
                        country = (NativeMethods.RASCTRYINFO)Marshal.PtrToStructure(lpRasCtryInfo, typeof(NativeMethods.RASCTRYINFO));

                        nextCountryId = country.nextCountryId;

                        string name = string.Empty;
                        if (country.countryNameOffset > 0)
                        {
                            name = Marshal.PtrToStringUni(new IntPtr(lpRasCtryInfo.ToInt64() + country.countryNameOffset));
                        }

                        retval = new RasCountry(
                            country.countryId,
                            country.countryCode,
                            name);
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                finally
                {
                    if (lpRasCtryInfo != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(lpRasCtryInfo);
                    }
                }
            }
            while (retry);

            return retval;
        }

        /// <summary>
        /// Retrieves accumulated statistics for the specified link in a RAS multilink connection.
        /// </summary>
        /// <param name="handle">The handle to the connection.</param>
        /// <param name="subEntryId">The one-based index that corresponds to the link for which to retrieve statistics.</param>
        /// <returns>A <see cref="DotRas.RasLinkStatistics"/> structure containing connection statistics.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="subEntryId"/> must be greater than zero.</exception>
        /// <exception cref="DotRas.InvalidHandleException"><paramref name="handle"/> is a null reference (<b>Nothing</b> in Visual Basic) or an invalid handle.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasLinkStatistics GetLinkStatistics(RasHandle handle, int subEntryId)
        {
            if (Utilities.IsHandleInvalidOrClosed(handle))
            {
                ThrowHelper.ThrowInvalidHandleException(handle, "handle", Resources.Argument_InvalidHandle);
            }

            if (subEntryId <= 0)
            {
                ThrowHelper.ThrowArgumentException("subEntryId", Resources.Argument_ValueCannotBeLessThanOrEqualToZero);
            }

            RasLinkStatistics retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RAS_STATS));

            NativeMethods.RAS_STATS stats = new NativeMethods.RAS_STATS();
            stats.size = size;

            IntPtr lpRasLinkStatistics = IntPtr.Zero;
            try
            {
                lpRasLinkStatistics = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(stats, lpRasLinkStatistics, true);

                int ret = SafeNativeMethods.RasGetLinkStatistics(handle, subEntryId, lpRasLinkStatistics);
                if (ret == NativeMethods.SUCCESS)
                {
                    stats = (NativeMethods.RAS_STATS)Marshal.PtrToStructure(lpRasLinkStatistics, typeof(NativeMethods.RAS_STATS));

                    retval = new RasLinkStatistics(
                        stats.bytesTransmitted,
                        stats.bytesReceived,
                        stats.framesTransmitted,
                        stats.framesReceived,
                        stats.crcError,
                        stats.timeoutError,
                        stats.alignmentError,
                        stats.hardwareOverrunError,
                        stats.framingError,
                        stats.bufferOverrunError,
                        stats.compressionRatioIn,
                        stats.compressionRatioOut,
                        stats.linkSpeed,
                        TimeSpan.FromMilliseconds(stats.connectionDuration));
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (lpRasLinkStatistics != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpRasLinkStatistics);
                }
            }

            return retval;
        }

        /// <summary>
        /// Terminates a remote access connection.
        /// </summary>
        /// <param name="handle">The remote access connection handle to terminate.</param>
        /// <returns><b>true</b> if the connection was terminated, otherwise <b>false</b>.</returns>
        /// <exception cref="DotRas.InvalidHandleException"><paramref name="handle"/> is not a valid handle.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool HangUp(RasHandle handle)
        {
            bool retval = false;

            try
            {
                int ret = SafeNativeMethods.RasHangUp(handle);
                if (ret == NativeMethods.ERROR_INVALID_HANDLE)
                {
                    ThrowHelper.ThrowInvalidHandleException(handle, "handle", Resources.Argument_InvalidHandle);
                }
                else if (ret == NativeMethods.SUCCESS)
                {
                    Thread.Sleep(3000);

                    // Mark the handle as invalid to prevent it from being used elsewhere in the assembly.
                    handle.SetHandleAsInvalid();

                    retval = true;
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Frees the memory buffer of an EAP user identity.
        /// </summary>
        /// <param name="rasEapUserIdentity">The <see cref="NativeMethods.RASEAPUSERIDENTITY"/> structure to free.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void FreeEapUserIdentity(NativeMethods.RASEAPUSERIDENTITY rasEapUserIdentity)
        {
            try
            {
                int size = Marshal.SizeOf(typeof(NativeMethods.RASEAPUSERIDENTITY));

                IntPtr lpRasEapUserIdentity = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(rasEapUserIdentity, lpRasEapUserIdentity, true);

                SafeNativeMethods.RasFreeEapUserIdentity(lpRasEapUserIdentity);
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
        }

        /// <summary>
        /// Retrieves any Extensible Authentication Protocol (EAP) user identity information if available.
        /// </summary>
        /// <param name="phoneBook">The full path and filename of a phone book. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="entryName">Required. The name of the entry in the phone book being connected.</param>
        /// <param name="eapOptions">Specifies options to use during authentication.</param>
        /// <param name="handle">Handle to the parent window for the UI dialog (if needed).</param>
        /// <param name="rasEapUserIdentity">Upon return, contains the Extensible Authentication Protocol (EAP) user identity information.</param>
        /// <returns><b>true</b> if the user identity information was returned, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="entryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool TryGetEapUserIdentity(string phoneBook, string entryName, RasEapOptions eapOptions, IntPtr handle, out NativeMethods.RASEAPUSERIDENTITY rasEapUserIdentity)
        {
            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            bool retval = false;

            rasEapUserIdentity = new NativeMethods.RASEAPUSERIDENTITY();

            IntPtr lpRasEapUserIdentity = IntPtr.Zero;
            try
            {
                int ret = SafeNativeMethods.RasGetEapUserIdentity(phoneBook, entryName, eapOptions, handle, ref lpRasEapUserIdentity);
                if (ret == NativeMethods.ERROR_INTERACTIVE_MODE)
                {
                    ThrowHelper.ThrowArgumentException("options", Resources.Argument_EapOptionsRequireInteractiveMode);
                }
                else if (ret == NativeMethods.ERROR_INVALID_FUNCTION_FOR_ENTRY)
                {
                    // The protocol being used by the entry does not support EAP, therefore no EAP information is needed.
                    retval = false;
                }
                else if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;

                    // Valid EAP information was returned, marshal the pointer back into the structure.
                    rasEapUserIdentity = (NativeMethods.RASEAPUSERIDENTITY)Marshal.PtrToStructure(lpRasEapUserIdentity, typeof(NativeMethods.RASEAPUSERIDENTITY));
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (lpRasEapUserIdentity != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpRasEapUserIdentity);
                }
            }

            return retval;
        }

#if (WIN2K8)
        /// <summary>
        /// Retrieves the network access protection (NAP) status for a remote access connection.
        /// </summary>
        /// <param name="handle">The handle of the connection.</param>
        /// <returns>A <see cref="DotRas.RasNapStatus"/> object containing the NAP status.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static RasNapStatus GetNapStatus(RasHandle handle)
        {
            if (Utilities.IsHandleInvalidOrClosed(handle))
            {
                ThrowHelper.ThrowArgumentException("handle", Resources.Argument_InvalidHandle);
            }

            RasNapStatus retval = null;

            int size = Marshal.SizeOf(typeof(NativeMethods.RASNAPSTATE));

            NativeMethods.RASNAPSTATE napState = new NativeMethods.RASNAPSTATE();
            napState.size = size;

            IntPtr pNapState = IntPtr.Zero;
            try
            {
                pNapState = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(napState, pNapState, true);

                int ret = SafeNativeMethods.RasGetNapStatus(handle, pNapState);
                if (ret == NativeMethods.SUCCESS)
                {
                    napState = (NativeMethods.RASNAPSTATE)Marshal.PtrToStructure(pNapState, typeof(NativeMethods.RASNAPSTATE));

                    long fileTime = napState.probationTime.dwHighDateTime << 0x20 | napState.probationTime.dwLowDateTime;
                    
                    retval = new RasNapStatus(
                        napState.isolationState,
                        DateTime.FromFileTime(fileTime));
                }
                else if (ret == NativeMethods.ERROR_INVALID_HANDLE)
                {
                    ThrowHelper.ThrowInvalidHandleException(handle, "handle", Resources.Argument_InvalidHandle);
                }
                else if (ret == NativeMethods.ERROR_NOT_NAP_CAPABLE)
                {
                    ThrowHelper.ThrowInvalidOperationException(Resources.Exception_HandleNotNapCapable);
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (pNapState != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pNapState);
                }
            }

            return retval;
        }
#endif

        /// <summary>
        /// Retrieves information about a remote access projection operation for a connection.
        /// </summary>
        /// <param name="handle">The handle of the connection.</param>
        /// <param name="projectionType">The protocol of interest.</param>
        /// <returns>The resulting projection information, otherwise null reference (<b>Nothing</b> in Visual Basic) if the protocol was not found.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="handle"/> is not a valid handle.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static object GetProjectionInfo(RasHandle handle, RasProjectionType projectionType)
        {
            if (Utilities.IsHandleInvalidOrClosed(handle))
            {
                ThrowHelper.ThrowArgumentException("handle", Resources.Argument_InvalidHandle);
            }

            int size = 0;

            object retval = null;
            object structure = null;

            switch (projectionType)
            {
                case RasProjectionType.Amb:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASAMB));

                    NativeMethods.RASAMB amb = new NativeMethods.RASAMB();
                    amb.size = size;

                    structure = amb;
                    break;

                case RasProjectionType.Ccp:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASPPPCCP));

                    NativeMethods.RASPPPCCP ccp = new NativeMethods.RASPPPCCP();
                    ccp.size = size;

                    structure = ccp;
                    break;

                case RasProjectionType.IP:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASPPPIP));

                    NativeMethods.RASPPPIP ip = new NativeMethods.RASPPPIP();
                    ip.size = size;

                    structure = ip;
                    break;

                case RasProjectionType.Ipx:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASPPPIPX));

                    NativeMethods.RASPPPIPX ipx = new NativeMethods.RASPPPIPX();
                    ipx.size = size;

                    structure = ipx;
                    break;

                case RasProjectionType.Lcp:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASPPPLCP));

                    NativeMethods.RASPPPLCP lcp = new NativeMethods.RASPPPLCP();
                    lcp.size = size;

                    structure = lcp;
                    break;

                case RasProjectionType.Nbf:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASPPPNBF));

                    NativeMethods.RASPPPNBF nbf = new NativeMethods.RASPPPNBF();
                    nbf.size = size;

                    structure = nbf;
                    break;

#if (WIN2K8)
                case RasProjectionType.IPv6:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASPPPIPV6));

                    NativeMethods.RASPPPIPV6 ipv6 = new NativeMethods.RASPPPIPV6();
                    ipv6.size = size;

                    structure = ipv6;
                    break;
#else
                case RasProjectionType.Slip:
                    size = Marshal.SizeOf(typeof(NativeMethods.RASSLIP));

                    NativeMethods.RASSLIP slip = new NativeMethods.RASSLIP();
                    slip.size = size;

                    structure = slip;
                    break;
#endif
            }

            IntPtr lpCb = new IntPtr(size);

            IntPtr lpProjection = IntPtr.Zero;
            try
            {
                lpProjection = Marshal.AllocHGlobal(lpCb);
                Marshal.StructureToPtr(structure, lpProjection, true);

                int ret = SafeNativeMethods.RasGetProjectionInfo(handle, projectionType, lpProjection, ref lpCb);
                if (ret == NativeMethods.SUCCESS)
                {
                    switch (projectionType)
                    {
                        case RasProjectionType.Amb:
                            NativeMethods.RASAMB amb = (NativeMethods.RASAMB)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASAMB));

                            retval = new RasAmbInfo(
                                amb.errorCode,
                                amb.netBiosError,
                                amb.lana);

                            break;

                        case RasProjectionType.Ccp:
                            NativeMethods.RASPPPCCP ccp = (NativeMethods.RASPPPCCP)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASPPPCCP));

                            retval = new RasCcpInfo(
                                ccp.errorCode,
                                ccp.compressionAlgorithm,
                                ccp.options,
                                ccp.serverCompressionAlgorithm,
                                ccp.serverOptions);

                            break;

                        case RasProjectionType.IP:
                            NativeMethods.RASPPPIP ip = (NativeMethods.RASPPPIP)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASPPPIP));

                            retval = new RasIPInfo(
                                ip.errorCode,
                                IPAddress.Parse(ip.ipAddress),
                                IPAddress.Parse(ip.serverIPAddress),
                                ip.options,
                                ip.serverOptions);

                            break;

                        case RasProjectionType.Ipx:
                            NativeMethods.RASPPPIPX ipx = (NativeMethods.RASPPPIPX)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASPPPIPX));

                            retval = new RasIpxInfo(
                                ipx.errorCode,
                                IPAddress.Parse(ipx.ipxAddress));

                            break;

                        case RasProjectionType.Lcp:
                            NativeMethods.RASPPPLCP lcp = (NativeMethods.RASPPPLCP)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASPPPLCP));

                            retval = new RasLcpInfo(
                                lcp.bundled,
                                lcp.errorCode,
                                lcp.authenticationProtocol,
                                lcp.authenticationData,
                                lcp.eapTypeId,
                                lcp.serverAuthenticationProtocol,
                                lcp.serverAuthenticationData,
                                lcp.serverEapTypeId,
                                lcp.multilink,
                                lcp.terminateReason,
                                lcp.serverTerminateReason,
                                lcp.replyMessage,
                                lcp.options,
                                lcp.serverOptions);

                            break;

                        case RasProjectionType.Nbf:
                            NativeMethods.RASPPPNBF nbf = (NativeMethods.RASPPPNBF)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASPPPNBF));

                            retval = new RasNbfInfo(
                                nbf.errorCode,
                                nbf.netBiosErrorCode,
                                nbf.netBiosErrorMessage,
                                nbf.workstationName,
                                nbf.lana);

                            break;

#if (WIN2K8)
                        case RasProjectionType.IPv6:
                            NativeMethods.RASPPPIPV6 ipv6 = (NativeMethods.RASPPPIPV6)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASPPPIPV6));

                            retval = new RasIPv6Info(
                                ipv6.errorCode,
                                ipv6.localInterfaceIdentifier,
                                ipv6.peerInterfaceIdentifier,
                                ipv6.localCompressionProtocol,
                                ipv6.peerCompressionProtocol);

                            break;
#else
                        case RasProjectionType.Slip:
                            NativeMethods.RASSLIP slip = (NativeMethods.RASSLIP)Marshal.PtrToStructure(lpProjection, typeof(NativeMethods.RASSLIP));

                            retval = new RasSlipInfo(
                                slip.errorCode,
                                IPAddress.Parse(slip.ipAddress));

                            break;
#endif
                    }
                }
                else if (ret != NativeMethods.ERROR_INVALID_PARAMETER && ret != NativeMethods.ERROR_PROTOCOL_NOT_CONFIGURED)
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (lpProjection != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpProjection);
                }
            }

            return retval;
        }

        /// <summary>
        /// Retrieves an error message for a specified RAS error code.
        /// </summary>
        /// <param name="errorCode">The error code to retrieve.</param>
        /// <returns>An <see cref="System.String"/> with the error message, otherwise a null reference (<b>Nothing</b> in Visual Basic) if the error code was not found.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static string GetRasErrorString(int errorCode)
        {
            string retval = null;

            if (errorCode > 0)
            {
                try
                {
                    string buffer = new string('\x00', 512);
                    int ret = SafeNativeMethods.RasGetErrorString(errorCode, buffer, buffer.Length);
                    if (ret == NativeMethods.SUCCESS)
                    {
                        retval = buffer.Substring(0, buffer.IndexOf('\x00'));
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
            }

            return retval;
        }

        /// <summary>
        /// Indicates whether the entry name is valid for the phone book specified.
        /// </summary>
        /// <param name="phoneBook">Required. An <see cref="DotRas.RasPhoneBook"/> to validate the name against.</param>
        /// <param name="entryName">Required. The name of an entry to check.</param>
        /// <returns><b>true</b> if the entry name is valid, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="entryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool IsValidEntryName(RasPhoneBook phoneBook, string entryName)
        {
            return RasHelper.IsValidEntryName(phoneBook, entryName, null);
        }

        /// <summary>
        /// Indicates whether the entry name is valid for the phone book specified.
        /// </summary>
        /// <param name="phoneBook">Required. An <see cref="DotRas.RasPhoneBook"/> to validate the name against.</param>
        /// <param name="entryName">Required. The name of an entry to check.</param>
        /// <param name="acceptableResults">Any additional results that are considered acceptable results from the call.</param>
        /// <returns><b>true</b> if the entry name is valid, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="entryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool IsValidEntryName(RasPhoneBook phoneBook, string entryName, params int[] acceptableResults)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }
            
            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            bool retval = false;

            try
            {
                int ret = SafeNativeMethods.RasValidateEntryName(phoneBook.Path, entryName);

                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else if (acceptableResults != null && acceptableResults.Length > 0)
                {
                    for (int index = 0; index < acceptableResults.Length; index++)
                    {
                        if (acceptableResults[index] == ret)
                        {
                            retval = true;
                            break;
                        }
                    }
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Renames an existing entry in a phone book.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> containing the entry to be renamed.</param>
        /// <param name="entryName">Required. The name of an entry to rename.</param>
        /// <param name="newEntryName">Required. The new name of the entry.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="entryName"/> or <paramref name="newEntryName"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool RenameEntry(RasPhoneBook phoneBook, string entryName, string newEntryName)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (string.IsNullOrEmpty(newEntryName))
            {
                ThrowHelper.ThrowArgumentException("newEntryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            bool retval = false;

            try
            {
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, phoneBook.Path);
                permission.Demand();

                int ret = UnsafeNativeMethods.RasRenameEntry(phoneBook.Path, entryName, newEntryName);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else if (ret == NativeMethods.ERROR_CANNOT_FIND_PHONEBOOK_ENTRY)
                {
                    ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_InvalidEntryName, entryName);
                }
                else if (ret == NativeMethods.ERROR_ACCESS_DENIED)
                {
                    ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                }
                else if (ret == NativeMethods.ERROR_ALREADY_EXISTS)
                {
                    ThrowHelper.ThrowArgumentException("newEntryName", Resources.Argument_EntryAlreadyExists);
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            catch (SecurityException)
            {
                ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
            }

            return retval;
        }

        /// <summary>
        /// Updates an address in the AutoDial mapping database.
        /// </summary>
        /// <param name="address">The address to update.</param>
        /// <param name="entries">A collection of <see cref="DotRas.RasAutoDialEntry"/> objects containing the entries for the <paramref name="address"/> specified.</param>
        /// <returns><b>true</b> if the update was successful, otherwise <b>false</b>.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool SetAutoDialAddress(string address, Collection<RasAutoDialEntry> entries)
        {
            bool retval = false;

            IntPtr pEntries = IntPtr.Zero;
            try
            {
                int count = 0;
                int totalSize = 0;

                if (entries != null && entries.Count > 0)
                {
                    // Reset the existing item so the new object being passed in isn't simply appended to any existing entries.
                    SetAutoDialAddress(address, null);

                    count = entries.Count;
                    int size = Marshal.SizeOf(typeof(NativeMethods.RASAUTODIALENTRY));

                    // Copy the entries into the struct array that will be used.
                    NativeMethods.RASAUTODIALENTRY[] autoDialEntries = new NativeMethods.RASAUTODIALENTRY[entries.Count];
                    for (int index = 0; index < autoDialEntries.Length; index++)
                    {
                        RasAutoDialEntry current = entries[index];
                        if (current != null)
                        {
                            NativeMethods.RASAUTODIALENTRY item = new NativeMethods.RASAUTODIALENTRY();
                            item.size = size;
                            item.dialingLocation = current.DialingLocation;
                            item.entryName = current.EntryName;

                            autoDialEntries[index] = item;
                        }
                    }

                    pEntries = Utilities.CopyObjectsToNewPtr<NativeMethods.RASAUTODIALENTRY>(autoDialEntries, ref size, out totalSize);
                }

                int ret = UnsafeNativeMethods.RasSetAutodialAddress(address, 0, pEntries, totalSize, count);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else if (ret == NativeMethods.ERROR_FILE_NOT_FOUND)
                {
                    retval = false;
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (pEntries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pEntries);
                }
            }
                      
            return retval;
        }

        /// <summary>
        /// Enables or disables the AutoDial feature for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dialingLocation">The TAPI dialing location to update.</param>
        /// <param name="enabled"><b>true</b> to enable the AutoDial feature, otherwise <b>false</b> to disable it.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool SetAutoDialEnable(int dialingLocation, bool enabled)
        {
            bool retval = false;

            try
            {
                int ret = UnsafeNativeMethods.RasSetAutodialEnable(dialingLocation, enabled);
                if (ret == NativeMethods.SUCCESS)
                {
                    retval = true;
                }
                else
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }

            return retval;
        }

        /// <summary>
        /// Sets the value of an AutoDial parameter.
        /// </summary>
        /// <param name="parameter">The parameter whose value to set.</param>
        /// <param name="value">The new value of the parameter.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void SetAutoDialParameter(NativeMethods.RASADP parameter, int value)
        {
            int size = Marshal.SizeOf(typeof(int));

            IntPtr pValue = IntPtr.Zero;
            try
            {
                pValue = Marshal.AllocHGlobal(size);
                Marshal.WriteInt32(pValue, value);

                int ret = UnsafeNativeMethods.RasSetAutodialParam(parameter, pValue, size);
                if (ret != NativeMethods.SUCCESS)
                {
                    ThrowHelper.ThrowRasException(ret);
                }
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
            finally
            {
                if (pValue != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pValue);
                }
            }
        }

        /// <summary>
        /// Sets the entry properties for an existing phone book entry, or creates a new entry.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> that will contain the entry.</param>
        /// <param name="value">An <see cref="DotRas.RasEntry"/> object whose properties to set.</param>        
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> or <paramref name="value"/> are a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool SetEntryProperties(RasPhoneBook phoneBook, RasEntry value)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            if (value == null)
            {
                ThrowHelper.ThrowArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(value.Name))
            {
                ThrowHelper.ThrowArgumentException("Entry name", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (!RasHelper.IsValidEntryName(phoneBook, value.Name, NativeMethods.ERROR_ALREADY_EXISTS, NativeMethods.ERROR_CANNOT_OPEN_PHONEBOOK))
            {
                ThrowHelper.ThrowArgumentException("entry name", Resources.Argument_InvalidEntryName, value.Name);
            }

            // Ensure the entry meets the minimum requirements to create or update a phone book entry.
            if ((string.IsNullOrEmpty(value.PhoneNumber) && value.AlternatePhoneNumbers.Count == 0) || (value.Device == null || (value.Device != null && (string.IsNullOrEmpty(value.Device.DeviceType) || string.IsNullOrEmpty(value.Device.Name)))) || value.FramingProtocol == RasFramingProtocol.None || value.EntryType == RasEntryType.None)
            {
                ThrowHelper.ThrowArgumentException("entry", Resources.Argument_MissingRequiredInfo);
            }

            bool retval = false;
            int size = Marshal.SizeOf(typeof(NativeMethods.RASENTRY));
            int lpCb = size;

            IntPtr lpRasEntry = IntPtr.Zero;
            try
            {
                NativeMethods.RASENTRY entry = new NativeMethods.RASENTRY();
                entry.size = size;

// This warning is being disabled since the object is being loaded by the Win32 API and must have the
// data placed into the object to prevent erasing any existing data.
#pragma warning disable 0618
                entry.autoDialDll = value.AutoDialDll;
                entry.autoDialFunc = value.AutoDialFunc;
#pragma warning restore 0618

                entry.areaCode = value.AreaCode;
                entry.channels = value.Channels;
                entry.countryCode = value.CountryCode;
                entry.countryId = value.CountryId;
                entry.customAuthKey = value.CustomAuthKey;
                entry.customDialDll = value.CustomDialDll;

                if (value.Device != null)
                {
                    entry.deviceName = value.Device.Name;
                    entry.deviceType = value.Device.DeviceType;
                }

                entry.dialExtraPercent = value.DialExtraPercent;
                entry.dialExtraSampleSeconds = value.DialExtraSampleSeconds;
                entry.dialMode = value.DialMode;
                entry.dnsAddress = Utilities.GetRasIPAddress(value.DnsAddress);
                entry.dnsAddressAlt = Utilities.GetRasIPAddress(value.DnsAddressAlt);
                entry.encryptionType = value.EncryptionType;
                entry.entryType = value.EntryType;
                entry.frameSize = value.FrameSize;
                entry.framingProtocol = value.FramingProtocol;
                entry.hangUpExtraPercent = value.HangUpExtraPercent;
                entry.hangUpExtraSampleSeconds = value.HangUpExtraSampleSeconds;
                entry.id = value.Id;
                entry.idleDisconnectSeconds = value.IdleDisconnectSeconds;
                entry.ipAddress = Utilities.GetRasIPAddress(value.IPAddress);
                entry.networkProtocols = value.NetworkProtocols;
                entry.options = value.Options;
                entry.phoneNumber = value.PhoneNumber;
                entry.reserved1 = value.Reserved1;
                entry.reserved2 = value.Reserved2;
                entry.script = value.Script;

                // This member should be set to zero and the subentries should be added after the entry has been created.
                entry.subentries = 0;

                entry.vpnStrategy = value.VpnStrategy;
                entry.winsAddress = Utilities.GetRasIPAddress(value.WinsAddress);
                entry.winsAddressAlt = Utilities.GetRasIPAddress(value.WinsAddressAlt);
                entry.x25Address = value.X25Address;
                entry.x25Facilities = value.X25Facilities;
                entry.x25PadType = value.X25PadType;
                entry.x25UserData = value.X25UserData;

#if (WINXP || WINXPSP2 || WIN2K8)
                entry.options2 = value.ExtendedOptions;
                entry.options3 = 0;
                entry.dnsSuffix = value.DnsSuffix;
                entry.tcpWindowSize = value.TcpWindowSize;
                entry.prerequisitePhoneBook = value.PrerequisitePhoneBook;
                entry.prerequisiteEntryName = value.PrerequisiteEntryName;
                entry.redialCount = value.RedialCount;
                entry.redialPause = value.RedialPause;
#endif
#if (WIN2K8)
                entry.ipv4InterfaceMetric = value.IPv4InterfaceMetric;
                entry.ipv6DnsAddress = Utilities.GetRasIPv6Address(value.IPv6DnsAddress);
                entry.ipv6DnsAddressAlt = Utilities.GetRasIPv6Address(value.IPv6DnsAddressAlt);
                entry.ipv6InterfaceMetric = value.IPv6InterfaceMetric;
#endif

                int alternatesLength = 0;
                string alternatesList = Utilities.BuildStringList(value.AlternatePhoneNumbers, '\x00', out alternatesLength);
                if (alternatesLength > 0)
                {
                    lpCb = size + alternatesLength;
                    entry.alternateOffset = size;
                }

                lpRasEntry = Marshal.AllocHGlobal(lpCb);
                Marshal.StructureToPtr(entry, lpRasEntry, true);

                if (alternatesLength > 0)
                {
                    // Now that the pointer has been allocated, copy the string to the location.
                    Utilities.CopyString(lpRasEntry, size, alternatesList, alternatesLength);
                }

                try
                {
                    FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, phoneBook.Path);
                    permission.Demand();

                    int ret = UnsafeNativeMethods.SetEntryProperties(phoneBook.Path, value.Name, lpRasEntry, lpCb, IntPtr.Zero, 0);
                    if (ret == NativeMethods.ERROR_ACCESS_DENIED)
                    {
                        ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                    }
                    else if (ret == NativeMethods.ERROR_INVALID_PARAMETER)
                    {
                        ThrowHelper.ThrowArgumentException("entry", Resources.Argument_MissingRequiredInfo);
                    }
                    else if (ret == NativeMethods.SUCCESS)
                    {
                        retval = true;

                        if (value.SubEntries.Count > 0)
                        {
                            // The entry has subentries associated with it, add them to the phone book.
                            for (int index = 0; index < value.SubEntries.Count; index++)
                            {
                                RasSubEntry subEntry = value.SubEntries[index];
                                if (subEntry != null)
                                {
                                    RasHelper.SetSubEntryProperties(value.Owner, value, index, subEntry);
                                }
                            }
                        }

                        if (value.Id == Guid.Empty)
                        {
                            // The entry being set is new, update any properties that need an existing entry.
                            RasEntry newEntry = null;
                            try
                            {
                                // Grab the entry from the phone book.
                                newEntry = RasHelper.GetEntryProperties(phoneBook, value.Name);
                                value.Id = newEntry.Id;
                            }
                            finally
                            {
                                newEntry = null;
                            }
                        }
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                catch (SecurityException)
                {
                    ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                }
            }
            finally
            {
                if (lpRasEntry != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpRasEntry);
                }
            }

            return retval;
        }

        /// <summary>
        /// Sets the subentry properties for an existing subentry, or creates a new subentry.
        /// </summary>
        /// <param name="phoneBook">Required. The <see cref="DotRas.RasPhoneBook"/> that will contain the entry.</param>
        /// <param name="entry">Required. The <see cref="DotRas.RasEntry"/> whose subentry to set.</param>
        /// <param name="subEntryId">The zero-based index of the subentry to set.</param>
        /// <param name="value">An <see cref="DotRas.RasSubEntry"/> object whose properties to set.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> or <paramref name="entry"/> or <paramref name="value"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool SetSubEntryProperties(RasPhoneBook phoneBook, RasEntry entry, int subEntryId, RasSubEntry value)
        {
            if (phoneBook == null)
            {
                ThrowHelper.ThrowArgumentNullException("phoneBook");
            }

            if (value == null)
            {
                ThrowHelper.ThrowArgumentNullException("value");
            }

            if (entry == null)
            {
                ThrowHelper.ThrowArgumentNullException("entry");
            }

            bool retval = false;
            int size = Marshal.SizeOf(typeof(NativeMethods.RASSUBENTRY));
            int lpCb = size;

            IntPtr lpRasSubEntry = IntPtr.Zero;
            try
            {
                NativeMethods.RASSUBENTRY subentry = new NativeMethods.RASSUBENTRY();
                subentry.size = size;
                subentry.phoneNumber = value.PhoneNumber;

                if (value.Device != null)
                {
                    subentry.deviceName = value.Device.Name;
                    subentry.deviceType = value.Device.DeviceType;
                }

                int alternatesLength = 0;
                string alternatesList = Utilities.BuildStringList(value.AlternatePhoneNumbers, '\x00', out alternatesLength);
                if (alternatesLength > 0)
                {
                    lpCb = size + alternatesLength;
                    subentry.alternateOffset = size;
                }

                lpRasSubEntry = Marshal.AllocHGlobal(lpCb);
                Marshal.StructureToPtr(subentry, lpRasSubEntry, true);

                if (alternatesLength > 0)
                {
                    Utilities.CopyString(lpRasSubEntry, size, alternatesList, alternatesLength);
                }

                try
                {
                    FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, phoneBook.Path);
                    permission.Demand();

                    int ret = UnsafeNativeMethods.RasSetSubEntryProperties(phoneBook.Path, entry.Name, subEntryId + 2, lpRasSubEntry, lpCb, IntPtr.Zero, 0);
                    if (ret == NativeMethods.SUCCESS)
                    {
                        retval = true;
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                catch (SecurityException)
                {
                    ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                }
            }
            finally
            {
                if (lpRasSubEntry != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpRasSubEntry);
                }
            }

            return retval;
        }

        /// <summary>
        /// Sets the user credentials for a phone book entry.
        /// </summary>
        /// <param name="phoneBook">The full path and filename of a phone book. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="entryName">The name of the entry whose credentials to set.</param>
        /// <param name="credentials">An <see cref="NativeMethods.RASCREDENTIALS"/> object containing user credentials.</param>
        /// <param name="clearCredentials"><b>true</b> clears existing credentials by setting them to an empty string, otherwise <b>false</b>.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="phoneBook"/> or <paramref name="entry"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool SetCredentials(string phoneBook, string entryName, NativeMethods.RASCREDENTIALS credentials, bool clearCredentials)
        {
            if (string.IsNullOrEmpty(phoneBook))
            {
                ThrowHelper.ThrowArgumentException("phoneBook", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            if (string.IsNullOrEmpty(entryName))
            {
                ThrowHelper.ThrowArgumentException("entryName", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            int size = Marshal.SizeOf(typeof(NativeMethods.RASCREDENTIALS));
            bool retval = false;

            IntPtr pCredentials = IntPtr.Zero;
            try
            {
                credentials.size = size;

                pCredentials = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(credentials, pCredentials, true);

                try
                {
                    FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, phoneBook);
                    permission.Demand();

                    int ret = UnsafeNativeMethods.SetCredentials(phoneBook, entryName, pCredentials, clearCredentials);
                    if (ret == NativeMethods.SUCCESS)
                    {
                        retval = true;
                    }
                    else
                    {
                        ThrowHelper.ThrowRasException(ret);
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
                }
                catch (SecurityException)
                {
                    ThrowHelper.ThrowUnauthorizedAccessException(Resources.Exception_AccessDeniedBySecurity);
                }
            }
            finally
            {
                if (pCredentials != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pCredentials);
                }
            }

            return retval;
        }
    }
}