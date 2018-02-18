//--------------------------------------------------------------------------
// <copyright file="PhoneBookFileNameEditor.cs" company="Jeff Winn">
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

namespace DotRas.Design
{
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;
    using DotRas.Properties;

    /// <summary>
    /// Provides a user interface for selecting a phone book file name. This class cannot be inherited.
    /// </summary>
    public sealed class PhoneBookFileNameEditor : FileNameEditor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.Design.PhoneBookFileNameEditor"/> class.
        /// </summary>
        public PhoneBookFileNameEditor()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the open file dialog when it is created.
        /// </summary>
        /// <param name="openFileDialog">The <see cref="System.Windows.Forms.OpenFileDialog"/> to use to select a file name.</param>
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);

            openFileDialog.Filter = Resources.PhoneBookFileFilter;
        }

        #endregion
    }
}