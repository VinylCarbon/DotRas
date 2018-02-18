//--------------------------------------------------------------------------
// <copyright file="RasEntryNameValidator.cs" company="Jeff Winn">
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
    using DotRas.Properties;

    /// <summary>
    /// Validates the format of an entry name for a phone book. This class cannot be inherited.
    /// </summary>
    /// <remarks>The name must contain at least one non-whitespace alphanumeric character.</remarks>
    public sealed class RasEntryNameValidator
    {
        #region Fields

        private int _errorCode;
        private string _errorMessage;
        private bool _isValid;
        private bool _allowExistingEntries;
        private bool _allowNonExistantPhoneBook;
        private string _entryName;
        private string _phoneBookPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasEntryNameValidator"/> class.
        /// </summary>
        public RasEntryNameValidator()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error code, if any, that occurred during validation.
        /// </summary>
        public int ErrorCode
        {
            get { return this._errorCode; }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get { return this._errorMessage; }
            set { this._errorMessage = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the entry name is valid.
        /// </summary>
        public bool IsValid
        {
            get { return this._isValid; }
            set { this._isValid = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether existing entries will be allowed.
        /// </summary>
        public bool AllowExistingEntries
        {
            get { return this._allowExistingEntries; }
            set { this._allowExistingEntries = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether nonexistant phone books are allowed.
        /// </summary>
        public bool AllowNonExistantPhoneBook
        {
            get { return this._allowNonExistantPhoneBook; }
            set { this._allowNonExistantPhoneBook = value; }
        }

        /// <summary>
        /// Gets or sets the entry name to validate.
        /// </summary>
        public string EntryName
        {
            get { return this._entryName; }
            set { this._entryName = value; }
        }

        /// <summary>
        /// Gets or sets the phone book path to validate the entry name against.
        /// </summary>
        public string PhoneBookPath
        {
            get { return this._phoneBookPath; }
            set { this._phoneBookPath = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates the condition it checks, and updates the <see cref="IsValid"/> property.
        /// </summary>
        public void Validate()
        {
            try
            {
                int errorCode = SafeNativeMethods.RasValidateEntryName(this.PhoneBookPath, this.EntryName);

                if (errorCode == NativeMethods.SUCCESS || (this.AllowExistingEntries && errorCode == NativeMethods.ERROR_ALREADY_EXISTS) || (this.AllowNonExistantPhoneBook && errorCode == NativeMethods.ERROR_CANNOT_OPEN_PHONEBOOK))
                {
                    this._errorCode = NativeMethods.SUCCESS;
                    this._errorMessage = null;
                }
                else
                {
                    this._errorCode = errorCode;
                    this._errorMessage = RasHelper.GetRasErrorString(errorCode);
                }

                this._isValid = this._errorCode == NativeMethods.SUCCESS;
            }
            catch (EntryPointNotFoundException)
            {
                ThrowHelper.ThrowNotSupportedException(Resources.Exception_NotSupportedOnPlatform);
            }
        }

        #endregion
    }
}