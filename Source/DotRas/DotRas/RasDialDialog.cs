//--------------------------------------------------------------------------
// <copyright file="RasDialDialog.cs" company="Jeff Winn">
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
    using System.Drawing.Design;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using DotRas.Design;
    using DotRas.Properties;

    /// <summary>
    /// Prompts the user to dial a phone book entry. This class cannot be inherited.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RasDialDialog), "DotRas.RasDialDialog.bmp")]
    public sealed class RasDialDialog : RasCommonDialog
    {
        #region Fields

        private string _phoneBookPath;
        private string _entryName;
        private string _phoneNumber;
        private int _subEntryId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasDialDialog"/> class.
        /// </summary>
        public RasDialDialog()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the full path (including filename) to the phone book.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("RDPhoneBookPathDesc")]
        [Editor(typeof(PhoneBookFileNameEditor), typeof(UITypeEditor))]
        public string PhoneBookPath
        {
            get { return this._phoneBookPath; }
            set { this._phoneBookPath = value; }
        }

        /// <summary>
        /// Gets or sets the name of the entry to be dialed.
        /// </summary>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("REDEntryNameDesc")]
        public string EntryName
        {
            get { return this._entryName; }
            set { this._entryName = value; }
        }

        /// <summary>
        /// Gets or sets the phone number to dial.
        /// </summary>
        /// <remarks>This value overrides the numbers stored in the phone book.</remarks>
        [DefaultValue(null)]
        [SRCategory("CatData")]
        [SRDescription("RDDPhoneNumberDesc")]
        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { this._phoneNumber = value; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the subentry to dial.
        /// </summary>
        [DefaultValue(0)]
        [SRCategory("CatData")]
        [SRDescription("RDSubEntryIdDesc")]
        public int SubEntryId
        {
            get { return this._subEntryId; }
            set { this._subEntryId = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets all <see cref="RasDialDialog"/> properties to their default values.
        /// </summary>
        public override void Reset()
        {
            this.PhoneBookPath = null;
            this.EntryName = null;
            this.PhoneNumber = null;
            this.SubEntryId = 0;

            base.Reset();
        }

        /// <summary>
        /// Overridden. Displays the modal dialog.
        /// </summary>
        /// <param name="hwndOwner">The handle of the window that owns the dialog box.</param>
        /// <returns><b>true</b> if the user completed the entry successfully, otherwise <b>false</b>.</returns>
        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.SafeSubWindows)]
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            NativeMethods.RASDIALDLG dlg = new NativeMethods.RASDIALDLG();
            dlg.size = Marshal.SizeOf(typeof(NativeMethods.RASDIALDLG));
            dlg.hwndOwner = hwndOwner;
            dlg.subEntryId = this.SubEntryId;

            if (this.Location != Point.Empty)
            {
                dlg.left = this.Location.X;
                dlg.top = this.Location.Y;

                dlg.flags |= NativeMethods.RASDDFLAG.PositionDlg;
            }

            bool retval = false;
            try
            {
                retval = UnsafeNativeMethods.RasDialDlg(this.PhoneBookPath, this.EntryName, this.PhoneNumber, ref dlg);
                if (!retval && dlg.error != NativeMethods.SUCCESS)
                {
                    RasErrorEventArgs e = new RasErrorEventArgs(dlg.error, RasHelper.GetRasErrorString(dlg.error));
                    this.OnError(e);
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

        #endregion
    }
}