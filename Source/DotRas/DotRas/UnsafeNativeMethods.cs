//--------------------------------------------------------------------------
// <copyright file="UnsafeNativeMethods.cs" company="Jeff Winn">
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
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Contains the unsafe remote access service (RAS) API function declarations.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal class UnsafeNativeMethods
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DotRas.UnsafeNativeMethods"/> class from being created.
        /// </summary>
        private UnsafeNativeMethods()
        {
        }

        #endregion

        /// <summary>
        /// Deletes an entry from a phone book.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the entry to be deleted.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int DeleteEntry(string lpszPhonebook, string lpszEntryName)
        {
            return UnsafeNativeMethods.RasDeleteEntry(lpszPhonebook, lpszEntryName);
        }

        /// <summary>
        /// Lists all addresses in the AutoDial mapping database.
        /// </summary>
        /// <param name="lppAddresses">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASAUTODIALENTRY"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lppAddresses"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcAddresses">Upon return, contains the number of address strings written to the buffer specified by <paramref name="lppAddresses"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>        
        public static int EnumAutodialAddresses(IntPtr lppAddresses, ref IntPtr lpCb, ref IntPtr lpcAddresses)
        {
            return UnsafeNativeMethods.RasEnumAutodialAddresses(lppAddresses, ref lpCb, ref lpcAddresses);
        }

        /// <summary>
        /// Retrieves user credentials associated with a specified remote access phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="lpCredentials">Pointer to a <see cref="NativeMethods.RASCREDENTIALS"/> structure that upon return contains the requested credentials for the phone book entry.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int GetCredentials(string lpszPhonebook, string lpszEntryName, IntPtr lpCredentials)
        {
            return UnsafeNativeMethods.RasGetCredentials(lpszPhonebook, lpszEntryName, lpCredentials);
        }

        /// <summary>
        /// Retrieves information about the entries associated with a network address in the AutoDial mapping database.
        /// </summary>
        /// <param name="lpszAddress">The address for which information is being requested.</param>
        /// <param name="lpdwReserved">Reserved. This argument must be zero.</param>
        /// <param name="lpAddresses">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASAUTODIALENTRY"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasSubEntry"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcEntries">Upon return, contains the number of phone book entries written to the buffer specified by <paramref name="lpRasEntryName"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int GetAutodialAddress(string lpszAddress, IntPtr lpdwReserved, IntPtr lpAddresses, ref IntPtr lpCb, ref IntPtr lpcEntries)
        {
            return UnsafeNativeMethods.RasGetAutodialAddress(lpszAddress, lpdwReserved, lpAddresses, ref lpCb, ref lpcEntries);
        }

        /// <summary>
        /// Indicates whether the AutoDial feature is enabled for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dwDialingLocation">The identifier of the TAPI dialing location.</param>
        /// <param name="lpfEnabled">Pointer to a <see cref="System.Boolean"/> that upon return indicates whether AutoDial is enabled for the specified dialing location.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int GetAutodialEnable(int dwDialingLocation, ref bool lpfEnabled)
        {
            return UnsafeNativeMethods.RasGetAutodialEnable(dwDialingLocation, ref lpfEnabled);
        }

        /// <summary>
        /// Retrieves the value of an AutoDial parameter.
        /// </summary>
        /// <param name="dwKey">The AutoDial parameter to retrieve.</param>
        /// <param name="lpvValue">Pointer to a buffer that receives the value for the specified parameter.</param>
        /// <param name="lpdwcbValue">On input, contains the size, in bytes, of the <paramref name="lpvValue"/> buffer. Upon return, contains the actual size of the value written to the buffer.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int GetAutodialParam(NativeMethods.RASADP dwKey, IntPtr lpvValue, ref int lpdwcbValue)
        {
            return UnsafeNativeMethods.RasGetAutodialParam(dwKey, lpvValue, ref lpdwcbValue);
        }

        /// <summary>
        /// Sets the user credentials for a phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the entry whose credentials to set.</param>
        /// <param name="lpCredentials">Pointer to an <see cref="NativeMethods.RASCREDENTIALS"/> object containing user credentials.</param>
        /// <param name="fClearCredentials"><b>true</b> clears existing credentials by setting them to an empty string, otherwise <b>false</b>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int SetCredentials(string lpszPhonebook, string lpszEntryName, IntPtr lpCredentials, bool fClearCredentials)
        {
            return UnsafeNativeMethods.RasSetCredentials(lpszPhonebook, lpszEntryName, lpCredentials, fClearCredentials);
        }

        /// <summary>
        /// Sets the connection information for an entry within a phone book, or creates a new phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="lpRasEntry">Pointer to a buffer that, upon return, contains a <see cref="NativeMethods.RASENTRY"/> structure containing entry information.</param>
        /// <param name="dwEntryInfoSize">Specifies the size of the <paramref name="lpRasEntry"/> buffer.</param>
        /// <param name="lpbDeviceInfo">The parameter is not used.</param>
        /// <param name="dwDeviceInfoSize">The parameter is not used.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int SetEntryProperties(string lpszPhonebook, string lpszEntryName, IntPtr lpRasEntry, int dwEntryInfoSize, IntPtr lpbDeviceInfo, int dwDeviceInfoSize)
        {
            return UnsafeNativeMethods.RasSetEntryProperties(lpszPhonebook, lpszEntryName, lpRasEntry, dwEntryInfoSize, lpbDeviceInfo, dwDeviceInfoSize);
        }

        /// <summary>
        /// Copies a memory block from one location to another.
        /// </summary>
        /// <param name="destination">A pointer to the starting address of the move destination.</param>
        /// <param name="source">A pointer to the starting address of the block of memory to be moved.</param>
        /// <param name="length">The size of the memory block to move, in bytes.</param>
        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(
            IntPtr destination,
            IntPtr source,
            IntPtr length);

#if (WINXP || WINXPSP2 || WIN2K8)
        /// <summary>
        /// Deletes a subentry from the specified phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the entry to be deleted.</param>
        /// <param name="subEntryId">The one-based index of the subentry to delete.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasDeleteSubEntry(
            string lpszPhonebook,
            string lpszEntryName,
            int subEntryId);
#endif

        /// <summary>
        /// Establishes a remote access connection using a specified phone book entry. This function displays a stream of dialog boxes that indicate the state of the connection operation.
        /// </summary>
        /// <param name="lpszPhoneBook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="lpszPhoneNumber">The phone number that overrides the numbers stored in the phone book entry.</param>
        /// <param name="lpInfo">A <see cref="NativeMethods.RASDIALDLG"/> structure containing input and output parameters.</param>
        /// <returns><b>true</b> if the function establishes a remote access connection, otherwise <b>false</b>.</returns>
        [DllImport("rasdlg.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RasDialDlg(
            string lpszPhoneBook,
            string lpszEntryName,
            string lpszPhoneNumber,
            ref NativeMethods.RASDIALDLG lpInfo);

        /// <summary>
        /// Displays a dialog box used to manipulate phone book entries.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the entry to be created or modified.</param>
        /// <param name="lpInfo">An <see cref="NativeMethods.RASENTRYDLG"/> structure containing additional input/output parameters.</param>
        /// <returns><b>true</b> if the user creates, copies, or edits an entry, otherwise <b>false</b>.</returns>
        [DllImport("rasdlg.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RasEntryDlg(
            string lpszPhonebook,
            string lpszEntryName,
            ref NativeMethods.RASENTRYDLG lpInfo);

        /// <summary>
        /// Lists all entry names in a remote access phone-book.
        /// </summary>
        /// <param name="reserved">Reserved; this parameter must be a null reference.</param>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpRasEntryName">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASENTRYNAME"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasEntryName"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcEntries">Upon return, contains the number of phone book entries written to the buffer specified by <paramref name="lpRasEntryName"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasEnumEntries(
            IntPtr reserved,
            string lpszPhonebook,
            [In, Out] IntPtr lpRasEntryName,
            ref IntPtr lpCb,
            ref IntPtr lpcEntries);

        /// <summary>
        /// Retrieves information for an existing phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="lpRasEntry">Pointer to a buffer that, upon return, contains a <see cref="NativeMethods.RASENTRY"/> structure containing entry information.</param>
        /// <param name="dwEntryInfoSize">Specifies the size of the <paramref name="lpRasEntry"/> buffer.</param>
        /// <param name="lpbDeviceInfo">The parameter is not used.</param>
        /// <param name="dwDeviceInfoSize">The parameter is not used.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetEntryProperties(
            string lpszPhonebook,
            string lpszEntryName,
            [In, Out] IntPtr lpRasEntry,
            ref IntPtr dwEntryInfoSize,
            IntPtr lpbDeviceInfo,
            IntPtr dwDeviceInfoSize);

        /// <summary>
        /// Retrieves information about a subentry for the specified phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="dwSubEntry">The one-based index of the subentry to retrieve.</param>
        /// <param name="lpRasSubEntry">Pointer to a buffer that, upon return, contains a <see cref="NativeMethods.RASSUBENTRY"/> structure containing subentry information.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasSubEntry"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpbDeviceConfig">The parameter is not used.</param>
        /// <param name="lpcbDeviceConfig">The parameter is not used.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetSubEntryProperties(
            string lpszPhonebook,
            string lpszEntryName,
            int dwSubEntry,
            [In, Out] IntPtr lpRasSubEntry,
            ref IntPtr lpCb,
            IntPtr lpbDeviceConfig,
            IntPtr lpcbDeviceConfig);

        /// <summary>
        /// Displays the main dial-up networking dialog box.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the phone book entry to initially highlight.</param>
        /// <param name="lpInfo">An <see cref="NativeMethods.RASPBDLG"/> structure containing additional input/output parameters.</param>
        /// <returns><b>true</b> if the user dials an entry successfully, otherwise <b>false</b>.</returns>
        [DllImport("rasdlg.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RasPhonebookDlg(
            string lpszPhonebook,
            string lpszEntryName,
            ref NativeMethods.RASPBDLG lpInfo);

        /// <summary>
        /// Renames an existing entry in a phone book.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszOldEntryName">The name of the entry to rename.</param>
        /// <param name="lpszNewEntryName">The new name of the entry.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasRenameEntry(
            string lpszPhonebook,
            string lpszOldEntryName,
            string lpszNewEntryName);

        /// <summary>
        /// Updates an address in the AutoDial mapping database.
        /// </summary>
        /// <param name="lpszAddress">The address for which information is being updated.</param>
        /// <param name="dwReserved">Reserved. This value must be zero.</param>
        /// <param name="lppAddresses">Pointer to an array of <see cref="NativeMethods.RASAUTODIALENTRY"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lppAddresses"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcEntries">Upon return, contains the number of phone book entries written to the buffer specified by <paramref name="lppAddresses"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasSetAutodialAddress(
            string lpszAddress,
            int dwReserved,
            IntPtr lppAddresses,
            int lpCb,
            int lpcEntries);

        /// <summary>
        /// Enables or disables the AutoDial feature for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dwDialingLocation">The TAPI dialing location to update.</param>
        /// <param name="fEnabled"><b>true</b> to enable the AutoDial feature, otherwise <b>false</b> to disable it.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasSetAutodialEnable(
            int dwDialingLocation,
            [MarshalAs(UnmanagedType.Bool)]
            bool fEnabled);

        /// <summary>
        /// Sets the value of an AutoDial parameter.
        /// </summary>
        /// <param name="dwKey">The parameter whose value to set.</param>
        /// <param name="lpvValue">A pointer containing the new value of the parameter.</param>
        /// <param name="dwcbValue">The size of the buffer.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasSetAutodialParam(
            NativeMethods.RASADP dwKey,
            IntPtr lpvValue,
            int dwcbValue);

        /// <summary>
        /// Sets the subentry connection information of a specified phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="dwSubEntry">The one-based index of the subentry to set.</param>
        /// <param name="lpRasSubEntry">Pointer to a buffer that, upon return, contains a <see cref="NativeMethods.RASSUBENTRY"/> structure containing subentry information.</param>
        /// <param name="dwcbRasSubEntry">Specifies the size of the <paramref name="lpRasEntry"/> buffer.</param>
        /// <param name="lpbDeviceConfig">The parameter is not used.</param>
        /// <param name="dwcbDeviceConfig">The parameter is not used.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasSetSubEntryProperties(
            string lpszPhonebook,
            string lpszEntryName,
            int dwSubEntry,
            IntPtr lpRasSubEntry,
            int dwcbRasSubEntry,
            IntPtr lpbDeviceConfig,
            int dwcbDeviceConfig);

        /// <summary>
        /// Deletes an entry from a phone book.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the entry to be deleted.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasDeleteEntry(
            string lpszPhonebook,
            string lpszEntryName);

        /// <summary>
        /// Lists all addresses in the AutoDial mapping database.
        /// </summary>
        /// <param name="lppAddresses">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASAUTODIALENTRY"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lppAddresses"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcAddresses">Upon return, contains the number of address strings written to the buffer specified by <paramref name="lppAddresses"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasEnumAutodialAddresses(
            [In, Out] IntPtr lppAddresses,
            ref IntPtr lpCb,
            ref IntPtr lpcAddresses);

        /// <summary>
        /// Retrieves information about the entries associated with a network address in the AutoDial mapping database.
        /// </summary>
        /// <param name="lpszAddress">The address for which information is being requested.</param>
        /// <param name="lpdwReserved">Reserved. This argument must be zero.</param>
        /// <param name="lpAddresses">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASAUTODIALENTRY"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasSubEntry"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcEntries">Upon return, contains the number of phone book entries written to the buffer specified by <paramref name="lpRasEntryName"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasGetAutodialAddress(
            string lpszAddress,
            IntPtr lpdwReserved,
            [In, Out] IntPtr lpAddresses,
            ref IntPtr lpCb,
            ref IntPtr lpcEntries);

        /// <summary>
        /// Indicates whether the AutoDial feature is enabled for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dwDialingLocation">The identifier of the TAPI dialing location.</param>
        /// <param name="lpfEnabled">Pointer to a <see cref="System.Boolean"/> that upon return indicates whether AutoDial is enabled for the specified dialing location.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasGetAutodialEnable(
            int dwDialingLocation,
            [MarshalAs(UnmanagedType.Bool)]
            ref bool lpfEnabled);

        /// <summary>
        /// Retrieves the value of an AutoDial parameter.
        /// </summary>
        /// <param name="dwKey">The AutoDial parameter to retrieve.</param>
        /// <param name="lpvValue">Pointer to a buffer that receives the value for the specified parameter.</param>
        /// <param name="lpdwcbValue">On input, contains the size, in bytes, of the <paramref name="lpvValue"/> buffer. Upon return, contains the actual size of the value written to the buffer.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasGetAutodialParam(
            NativeMethods.RASADP dwKey,
            IntPtr lpvValue,
            ref int lpdwcbValue);

        /// <summary>
        /// Retrieves user credentials associated with a specified remote access phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="lpCredentials">Pointer to a <see cref="NativeMethods.RASCREDENTIALS"/> structure that upon return contains the requested credentials for the phone book entry.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasGetCredentials(
            string lpszPhonebook,
            string lpszEntryName,
            [In, Out] IntPtr lpCredentials);

        /// <summary>
        /// Sets the user credentials for a phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of the entry whose credentials to set.</param>
        /// <param name="lpCredentials">Pointer to an <see cref="NativeMethods.RASCREDENTIALS"/> object containing user credentials.</param>
        /// <param name="fClearCredentials"><b>true</b> clears existing credentials by setting them to an empty string, otherwise <b>false</b>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasSetCredentials(
            string lpszPhonebook,
            string lpszEntryName,
            IntPtr lpCredentials,
            [MarshalAs(UnmanagedType.Bool)] bool fClearCredentials);

        /// <summary>
        /// Sets the connection information for an entry within a phone book, or creates a new phone book entry.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="lpRasEntry">Pointer to a buffer that, upon return, contains a <see cref="NativeMethods.RASENTRY"/> structure containing entry information.</param>
        /// <param name="dwEntryInfoSize">Specifies the size of the <paramref name="lpRasEntry"/> buffer.</param>
        /// <param name="lpbDeviceInfo">The parameter is not used.</param>
        /// <param name="dwDeviceInfoSize">The parameter is not used.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasSetEntryProperties(
            string lpszPhonebook,
            string lpszEntryName,
            IntPtr lpRasEntry,
            int dwEntryInfoSize,
            IntPtr lpbDeviceInfo,
            int dwDeviceInfoSize);
    }
}