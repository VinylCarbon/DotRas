//--------------------------------------------------------------------------
// <copyright file="SafeNativeMethods.cs" company="Jeff Winn">
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
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Contains the safe remote access service (RAS) API function declarations.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal class SafeNativeMethods
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DotRas.SafeNativeMethods"/> class from being created.
        /// </summary>
        private SafeNativeMethods()
        {
        }

        #endregion

        /// <summary>
        /// Clears any accumulated statistics for the specified RAS connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int ClearConnectionStatistics(RasHandle hRasConn)
        {
            return SafeNativeMethods.RasClearConnectionStatistics(hRasConn);
        }

        /// <summary>
        /// Clears any accumulated statistics for the specified link in a RAS multilink connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="subEntryId">The subentry index that corresponds to the link for which to clear statistics.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int ClearLinkStatistics(RasHandle hRasConn, int subEntryId)
        {
            return SafeNativeMethods.RasClearLinkStatistics(hRasConn, subEntryId);
        }

        /// <summary>
        /// Retrieves information on the current status of the specified remote access connection handle.
        /// </summary>
        /// <param name="hRasConn">The handle to check.</param>
        /// <param name="lpRasConnStatus">Pointer to a <see cref="DotRas.RasConnectionStatus"/> structure that upon return contains the status information for the handle specified by <paramref name="hRasConn"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int GetConnectStatus(RasHandle hRasConn, IntPtr lpRasConnStatus)
        {
            return SafeNativeMethods.RasGetConnectStatus(hRasConn, lpRasConnStatus);
        }

        /// <summary>
        /// Lists all active remote access service (RAS) connections.
        /// </summary>
        /// <param name="lpRasConn">Pointer to a buffer that, on output, receives an array of <see cref="DotRas.RasConnection"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasConn"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcConnections">Upon return, contains the number of phone book entries written to the buffer specified by <paramref name="lpRasConn"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int EnumConnections(IntPtr lpRasConn, ref IntPtr lpCb, ref IntPtr lpcConnections)
        {
            return SafeNativeMethods.RasEnumConnections(lpRasConn, ref lpCb, ref lpcConnections);
        }

        /// <summary>
        /// Lists all available remote access capable devices.
        /// </summary>
        /// <param name="lpRasDevInfo">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASDEVINFO"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasDevInfo"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcDevices">Upon return, contains the number of device entries written to the buffer specified by <paramref name="lpRasDevInfo"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int EnumDevices(IntPtr lpRasDevInfo, ref IntPtr lpCb, ref IntPtr lpcDevices)
        {
            return SafeNativeMethods.RasEnumDevices(lpRasDevInfo, ref lpCb, ref lpcDevices);
        }

        /// <summary>
        /// Retrieves country/region specific dialing information from the Windows telephony list of countries/regions.
        /// </summary>
        /// <param name="lpRasCtryInfo">Pointer to a <see cref="NativeMethods.RASCTRYINFO"/> structure that upon output receives the country/region dialing information.</param>
        /// <param name="lpdwSize">Pointer to a variable that, on input, specifies the size, in bytes, of the buffer pointed to by <paramref name="lpRasCtryInfo"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        public static int GetCountryInfo(IntPtr lpRasCtryInfo, ref IntPtr lpdwSize)
        {
            return SafeNativeMethods.RasGetCountryInfo(lpRasCtryInfo, ref lpdwSize);
        }

        /// <summary>
        /// Allocates a new locally unique identifier.
        /// </summary>
        /// <param name="pLuid">Pointer to a <see cref="DotRas.Luid"/> structure that upon return, receives the generated LUID instance.</param>
        /// <returns><b>true</b> if the function succeeds, otherwise <b>false</b>.</returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocateLocallyUniqueId(
            IntPtr pLuid);

        /// <summary>
        /// Specifies an event object that the system sets to the signaled state when a RAS connection changes.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="hEvent">The handle of an event object.</param>
        /// <param name="dwFlags">Specifies the RAS event that causes the system to signal the event specified by the <paramref name="hEvent"/> parameter.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasConnectionNotification(
            RasHandle hRasConn,
            SafeHandle hEvent,
            NativeMethods.RASCN dwFlags);

        /// <summary>
        /// Retrieves accumulated statistics for the specified connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="lpStatistics">Pointer to a <see cref="NativeMethods.RAS_STATS"/> structure which will receive the statistics.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetConnectionStatistics(
            RasHandle hRasConn,
            IntPtr lpStatistics);

        /// <summary>
        /// Establishes a remote access connection between a client and a server.
        /// </summary>
        /// <param name="lpRasDialExtensions">Pointer to a <see cref="NativeMethods.RASDIALEXTENSIONS"/> structure containing extended feature information.</param>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpRasDialParams">Pointer to a <see cref="NativeMethods.RASDIALPARAMS"/> structure containing calling parameters for the connection.</param>
        /// <param name="dwNotifierType">Specifies the nature of the <paramref name="lpvNotifier"/> argument. If <paramref name="lpvNotifier"/> is null (<b>Nothing</b> in Visual Basic) this argument is ignored.</param>
        /// <param name="lpvNotifier">Specifies the callback used during the dialing process.</param>
        /// <param name="lphRasConn">Upon return, contains the handle to the RAS connection.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasDial(
            IntPtr lpRasDialExtensions,
            string lpszPhonebook,
            IntPtr lpRasDialParams,
            NativeMethods.RasNotifierType dwNotifierType,
            Delegate lpvNotifier,
            out RasHandle lphRasConn);

        /// <summary>
        /// Frees the memory buffer returned by the <see cref="SafeNativeMethods.RasGetEapUserIdentity"/> method.
        /// </summary>
        /// <param name="lpRasEapUserIdentity">Pointer to the <see cref="NativeMethods.RASEAPUSERIDENTITY"/> structure.</param>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern void RasFreeEapUserIdentity(
            IntPtr lpRasEapUserIdentity);

        /// <summary>
        /// Retrieves Extensible Authentication Protocol (EAP) identity information for the current user.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /// <param name="lpszEntryName">The name of an existing entry within the phone book.</param>
        /// <param name="dwFlags">Specifies any flags that qualify the authentication process.</param>
        /// <param name="hwnd">Handle to the parent window for the UI dialog.</param>
        /// <param name="lpRasEapUserIdentity">Pointer to a buffer that upon return contains the EAP user identity information.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetEapUserIdentity(
            string lpszPhonebook,
            string lpszEntryName,
            RasEapOptions dwFlags,
            IntPtr hwnd,
            ref IntPtr lpRasEapUserIdentity);

        /////// <summary>
        /////// Retrieves the entry dial parameters for a connection.
        /////// </summary>
        /////// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference, the default phone book is used.</param>
        /////// <param name="lpRasDialParams">Pointer to a <see cref="NativeMethods.RASDIALPARAMS"/> structure that upon return contains the dial parameters for a connection.</param>
        /////// <param name="lpfPassword">Upon return, indicates whether the function retrieved the password associated with the username for the phone book entry.</param>
        /////// <returns>If the function succeeds, the return value is zero.</returns>
        ////[DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        ////public static extern int RasGetEntryDialParams(
        ////    string lpszPhonebook,
        ////    IntPtr lpRasDialParams,
        ////    [MarshalAs(UnmanagedType.Bool)] ref bool lpfPassword);

        /// <summary>
        /// Returns an error message string for a specified RAS error value.
        /// </summary>
        /// <param name="uErrorValue">The error value of interest.</param>
        /// <param name="lpszErrorString">Required. The buffer that will receive the error string.</param>
        /// <param name="cBufSize">Specifies the size, in characters, of the buffer pointed to by <paramref name="lpszErrorString"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetErrorString(
            int uErrorValue,
            [In, Out] string lpszErrorString,
            int cBufSize);

        /// <summary>
        /// Retrieves accumulated statistics for the specified link in a RAS multilink connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="subEntryId">The subentry index that corresponds to the link for which to retrieve statistics.</param>
        /// <param name="lpRasStatistics">Pointer to a <see cref="NativeMethods.RAS_STATS"/> structure which will receive the statistics.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetLinkStatistics(
            RasHandle hRasConn,
            int subEntryId,
            IntPtr lpRasStatistics);

#if (WIN2K8)
        /// <summary>
        /// Retrieves the network access protection (NAP) status for a remote access connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="pNapState">Pointer to a <see cref="NativeMethods.RASNAPSTATE"/> structure </param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetNapStatus(
            RasHandle hRasConn,
            IntPtr pNapState);
#endif

        /// <summary>
        /// Obtains information about a remote access projection operation for a specified remote access component protocol.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="rasprojection">The <see cref="DotRas.RasProjectionType"/> that identifies the protocol of interest.</param>
        /// <param name="lpProjection">Pointer to a buffer that receives the information specified by the <paramref name="rasprojection"/> parameter.</param>
        /// <param name="lpCb">Upon input specifies the size in bytes of the buffer pointed to by <paramref name="lpProjection"/>, upon output receives the size of the buffer needed to contain the projection information.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetProjectionInfo(
            RasHandle hRasConn,
            RasProjectionType rasprojection,
            IntPtr lpProjection,
            ref IntPtr lpCb);

        /// <summary>
        /// Retrieves a connection handle for a subentry of a multilink connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="subEntryId">The one-based index of the subentry to whose handle to retrieve.</param>
        /// <param name="lphRasConn">Upon return, contains the handle to the subentry connection.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasGetSubEntryHandle(
            RasHandle hRasConn,
            int subEntryId,
            out RasHandle lphRasConn);

        /// <summary>
        /// Terminates a remote access connection.
        /// </summary>
        /// <param name="hRasConn">The handle to terminate.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll")]
        public static extern int RasHangUp(
            RasHandle hRasConn);

        /// <summary>
        /// Indicates whether the entry name is valid for the phone book specified.
        /// </summary>
        /// <param name="lpszPhonebook">The full path and filename of a phone book file. If this parameter is a null reference (<b>Nothing</b> in Visual Basic), the default phone book is used.</param>
        /// <param name="lpszEntryName">The entry name to validate.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RasValidateEntryName(
            string lpszPhonebook,
            string lpszEntryName);

        /// <summary>
        /// Clears any accumulated statistics for the specified RAS connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasClearConnectionStatistics(
            RasHandle hRasConn);

        /// <summary>
        /// Clears any accumulated statistics for the specified link in a RAS multilink connection.
        /// </summary>
        /// <param name="hRasConn">The handle to the connection.</param>
        /// <param name="subEntryId">The subentry index that corresponds to the link for which to clear statistics.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasClearLinkStatistics(
            RasHandle hRasConn,
            int subEntryId);

        /// <summary>
        /// Lists all active remote access service (RAS) connections.
        /// </summary>
        /// <param name="lpRasConn">Pointer to a buffer that, on output, receives an array of <see cref="DotRas.RasConnection"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasConn"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcConnections">Upon return, contains the number of phone book entries written to the buffer specified by <paramref name="lpRasConn"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasEnumConnections(
            [In, Out] IntPtr lpRasConn,
            ref IntPtr lpCb,
            ref IntPtr lpcConnections);

        /// <summary>
        /// Lists all available remote access capable devices.
        /// </summary>
        /// <param name="lpRasDevInfo">Pointer to a buffer that, on output, receives an array of <see cref="NativeMethods.RASDEVINFO"/> structures.</param>
        /// <param name="lpCb">Upon return, contains the size in bytes of the buffer specified by <paramref name="lpRasDevInfo"/>. Upon return contains the number of bytes required to successfully complete the call.</param>
        /// <param name="lpcDevices">Upon return, contains the number of device entries written to the buffer specified by <paramref name="lpRasDevInfo"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasEnumDevices(
            [In, Out] IntPtr lpRasDevInfo,
            ref IntPtr lpCb,
            ref IntPtr lpcDevices);

        /// <summary>
        /// Retrieves information on the current status of the specified remote access connection handle.
        /// </summary>
        /// <param name="hRasConn">The handle to check.</param>
        /// <param name="lpRasConnStatus">Pointer to a <see cref="DotRas.RasConnectionStatus"/> structure that upon return contains the status information for the handle specified by <paramref name="hRasConn"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasGetConnectStatus(
            RasHandle hRasConn,
            IntPtr lpRasConnStatus);

        /// <summary>
        /// Retrieves country/region specific dialing information from the Windows telephony list of countries/regions.
        /// </summary>
        /// <param name="lpRasCtryInfo">Pointer to a <see cref="NativeMethods.RASCTRYINFO"/> structure that upon output receives the country/region dialing information.</param>
        /// <param name="lpdwSize">Pointer to a variable that, on input, specifies the size, in bytes, of the buffer pointed to by <paramref name="lpRasCtryInfo"/>.</param>
        /// <returns>If the function succeeds, the return value is zero.</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RasGetCountryInfo(
            [In, Out] IntPtr lpRasCtryInfo,
            ref IntPtr lpdwSize);
    }
}