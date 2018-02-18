//--------------------------------------------------------------------------
// <copyright file="RasAutoDialManager.cs" company="Jeff Winn">
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
    using System.ComponentModel;
    using System.Drawing;
    using System.Security.Permissions;

    /// <summary>
    /// Provides methods to interact with the remote access service (RAS) AutoDial mapping database. This class cannot be inherited.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RasAutoDialManager), "DotRas.RasAutoDialManager.bmp")]
    public sealed class RasAutoDialManager : Component
    {
        #region Fields

        private RasAutoDialAddressCollection _addresses;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasAutoDialManager"/> class.
        /// </summary>
        public RasAutoDialManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasAutoDialManager"/> class.
        /// </summary>
        /// <param name="container">An <see cref="System.ComponentModel.IContainer"/> that will contain this component.</param>
        public RasAutoDialManager(IContainer container)
        {
            if (container != null)
            {
                container.Add(this);
            }
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the collection of addresses in the AutoDial database.
        /// </summary>
        [Browsable(false)]
        public RasAutoDialAddressCollection Addresses
        {
            get
            {
                if (this._addresses == null)
                {
                    this._addresses = new RasAutoDialAddressCollection();
                    this._addresses.Load();
                }

                return this._addresses;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether AutoDial displays a dialog box to query the user before creating a connection.
        /// </summary>
        /// <remarks><b>true</b> and the AutoDial database has the entry to dial, AutoDial creates a connection without displaying the dialog box.</remarks>
        [DefaultValue(false)]
        [SRCategory("CatBehavior")]
        [SRDescription("RADMDisableConnectionQueryDesc")]
        public bool DisableConnectionQuery
        {
            get
            {
                return RasHelper.GetAutoDialParameter(NativeMethods.RASADP.DisableConnectionQuery) != 0;
            }

            set
            {
                int actual = 0;

                if (value)
                {
                    actual = 1;
                }

                RasHelper.SetAutoDialParameter(NativeMethods.RASADP.DisableConnectionQuery, actual);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the system disables all AutoDial connections for the current logon session.
        /// </summary>
        /// <remarks><b>true</b> if the AutoDial connections are disabled, otherwise <b>false</b>.</remarks>
        [DefaultValue(false)]
        [SRCategory("CatBehavior")]
        [SRDescription("RADMLogOnSessionDisableDesc")]
        public bool LogOnSessionDisable
        {
            get
            {
                return RasHelper.GetAutoDialParameter(NativeMethods.RASADP.LogOnSessionDisable) != 0;
            }

            set
            {
                int actual = 0;

                if (value)
                {
                    actual = 1;
                }

                RasHelper.SetAutoDialParameter(NativeMethods.RASADP.LogOnSessionDisable, actual);
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of addresses that AutoDial stores in the registry.
        /// </summary>
        /// <remarks>The default value is 100.</remarks>
        [DefaultValue(100)]
        [SRCategory("CatBehavior")]
        [SRDescription("RADMSavedAddressesLimitDesc")]
        public int SavedAddressesLimit
        {
            get
            {
                return RasHelper.GetAutoDialParameter(NativeMethods.RASADP.SavedAddressesLimit);
            }

            set
            {
                RasHelper.SetAutoDialParameter(NativeMethods.RASADP.SavedAddressesLimit, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of time (in seconds) between AutoDial connection attempts.
        /// </summary>
        /// <remarks>When an AutoDial connection attempt fails, the AutoDial service disables subsequent attempts to reach the same address for the timeout period. The default value is 5 seconds.</remarks>
        [DefaultValue(5)]
        [SRCategory("CatBehavior")]
        [SRDescription("RADMFailedConnectionTimeoutDesc")]
        public int FailedConnectionTimeout
        {
            get
            {
                return RasHelper.GetAutoDialParameter(NativeMethods.RASADP.FailedConnectionTimeout);
            }

            set
            {
                RasHelper.SetAutoDialParameter(NativeMethods.RASADP.FailedConnectionTimeout, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of time (in seconds) before the connection attempt is aborted.
        /// </summary>
        /// <remarks>Before attempting an AutoDial connection, the system will display a dialog asking the user to confirm the system should dial.</remarks>
        [DefaultValue(60)]
        [SRCategory("CatBehavior")]
        [SRDescription("RADMConnectionQueryTimeoutDesc")]
        public int ConnectionQueryTimeout
        {
            get
            {
                return RasHelper.GetAutoDialParameter(NativeMethods.RASADP.ConnectionQueryTimeout);
            }

            set
            {
                RasHelper.SetAutoDialParameter(NativeMethods.RASADP.ConnectionQueryTimeout, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the AutoDial status for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dialingLocation">The TAPI dialing location to update.</param>
        /// <param name="enabled"><b>true</b> to enable AutoDial, otherwise <b>false</b> to disable it.</param>
        /// <returns><b>true</b> if the operation was successful, otherwise <b>false</b>.</returns>
        [RegistryPermission(SecurityAction.Demand, Unrestricted = true)]
        public bool UpdateAutoDialStatus(int dialingLocation, bool enabled)
        {
            return RasHelper.SetAutoDialEnable(dialingLocation, enabled);
        }

        /// <summary>
        /// Indicates whether AutoDial is enabled for a specific TAPI dialing location.
        /// </summary>
        /// <param name="dialingLocation">The dialing location whose AutoDial status to retrieve.</param>
        /// <returns><b>true</b> if the AutoDial feature is currently enabled for the dialing location, otherwise <b>false</b>.</returns>
        [RegistryPermission(SecurityAction.Demand, Unrestricted = true)]
        public bool IsAutoDialEnabled(int dialingLocation)
        {
            return RasHelper.GetAutoDialEnable(dialingLocation);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}