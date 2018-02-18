//--------------------------------------------------------------------------
// <copyright file="RasPhoneBook.cs" company="Jeff Winn">
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
    using System.IO;
    using System.Security.Permissions;
    using DotRas.Design;
    using DotRas.Properties;

    /// <summary>
    /// Represents a remote access service (RAS) phone book. This class cannot be inherited.
    /// </summary>
    /// <remarks>There are multiple phone books in use by Windows at any given point in time and this class can only manage one phone book per instance. If you add an entry to the all user's profile phone book, attempting to manipulate it with the current user's profile phone book opened will result in failure. Entries will not be located, and changes made to the phone book will not be recognized by the instance.</remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RasPhoneBook), "DotRas.RasPhoneBook.bmp")]
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed partial class RasPhoneBook : RasComponent
    {
        #region Fields

        private string _path;
        private RasPhoneBookType _phoneBookType;
        private RasEntryCollection _entries;
        private bool _enableFileWatcher;
        private FileSystemWatcher _watcher;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasPhoneBook"/> class.
        /// </summary>
        public RasPhoneBook()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.RasPhoneBook"/> class.
        /// </summary>
        /// <param name="container">An <see cref="System.ComponentModel.IContainer"/> that will contain the component.</param>
        public RasPhoneBook(IContainer container)
            : base(container)
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the phone book has changed.
        /// </summary>
        /// <remarks>This event may be raised multiple times depending on how the file was changed.</remarks>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBChangedDesc")]
        public event EventHandler<EventArgs> Changed;

        /// <summary>
        /// Occurs when the phone book has been deleted.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBDeletedDesc")]
        public event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// Occurs when the phone book has been renamed.
        /// </summary>
        [SRCategory("CatBehavior")]
        [SRDescription("RPBRenamedDesc")]
        public event EventHandler<RenamedEventArgs> Renamed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the full path (including filename) of the phone book.
        /// </summary>
        [Browsable(false)]
        public string Path
        {
            get { return this._path; }
            private set { this._path = value; }
        }

        /// <summary>
        /// Gets the type of phone book.
        /// </summary>
        [Browsable(false)]
        public RasPhoneBookType PhoneBookType
        {
            get { return this._phoneBookType; }
            private set { this._phoneBookType = value; }
        }

        /// <summary>
        /// Gets the collection of entries within the phone book.
        /// </summary>
        [Browsable(false)]
        public RasEntryCollection Entries
        {
            get
            {
                if (this._entries == null)
                {
                    this._entries = new RasEntryCollection(this);
                }

                return this._entries;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the phone book file will be monitored for external changes.
        /// </summary>
        [DefaultValue(false)]
        [SRCategory("CatBehavior")]
        [SRDescription("RPBEnableFileWatcherDesc")]
        public bool EnableFileWatcher
        {
            get
            {
                return this._enableFileWatcher;
            }

            set
            {
                this._enableFileWatcher = value;

                if (!string.IsNullOrEmpty(this.Path))
                {
                    // The phone book has already been opened, update the setting on the watcher.
                    this._watcher.EnableRaisingEvents = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines the full path (including filename) of the phone book.
        /// </summary>
        /// <param name="phoneBookType">The type of phone book to locate.</param>
        /// <returns>The full path (including filename) of the phone book.</returns>
        /// <remarks><see cref="RasPhoneBookType.Custom"/> will always return a null reference (<b>Nothing</b> in Visual Basic).</remarks>
        public static string GetPhoneBookPath(RasPhoneBookType phoneBookType)
        {
            string retval = null;

            if (phoneBookType != RasPhoneBookType.Custom)
            {
                Environment.SpecialFolder folder = Environment.SpecialFolder.CommonApplicationData;
                if (phoneBookType == RasPhoneBookType.User)
                {
                    folder = Environment.SpecialFolder.ApplicationData;
                }

                retval = System.IO.Path.Combine(Environment.GetFolderPath(folder), "Microsoft\\Network\\Connections\\Pbk\\rasphone.pbk");
            }

            return retval;
        }

        /// <summary>
        /// Opens the phone book.
        /// </summary>
        /// <remarks>This method opens the existing default phone book in the All Users profile, or creates a new phone book if the file does not already exist.</remarks>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public void Open()
        {
            this.Open(false);
        }

        /// <summary>
        /// Opens the phone book.
        /// </summary>
        /// <param name="openUserPhoneBook"><b>true</b> to open the phone book in the user's profile; otherwise, <b>false</b> to open the system phone book in the All Users profile.</param>
        /// <remarks>This method opens an existing phone book or creates a new phone book if the file does not already exist.</remarks>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public void Open(bool openUserPhoneBook)
        {
            RasPhoneBookType phoneBookType = RasPhoneBookType.AllUsers;
            if (openUserPhoneBook)
            {
                phoneBookType = RasPhoneBookType.User;
            }

            this.Open(RasPhoneBook.GetPhoneBookPath(phoneBookType));
            this.PhoneBookType = phoneBookType;
        }

        /// <summary>
        /// Opens the phone book.
        /// </summary>
        /// <param name="phoneBookPath">The full path (including filename) of a phone book.</param>
        /// <remarks>This method opens an existing phone book or creates a new phone book if the file does not already exist.</remarks>
        /// <exception cref="System.ArgumentException"><paramref name="phoneBookPath"/> is an empty string or null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to perform the action requested.</exception>
        public void Open(string phoneBookPath)
        {
            if (string.IsNullOrEmpty(phoneBookPath))
            {
                ThrowHelper.ThrowArgumentException("phoneBookPath", Resources.Argument_StringCannotBeNullOrEmpty);
            }

            FileInfo file = new FileInfo(phoneBookPath);
            if (string.IsNullOrEmpty(file.Name))
            {
                ThrowHelper.ThrowArgumentException("phoneBookPath", Resources.Argument_InvalidFileName);
            }

            this.Path = phoneBookPath;
            this.PhoneBookType = RasPhoneBookType.Custom;

            this._watcher.Path = file.DirectoryName;
            this._watcher.Filter = file.Name;
            this._watcher.EnableRaisingEvents = this.EnableFileWatcher;

            if (file.Exists)
            {
                this.Entries.Load();
            }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected override void InitializeComponent()
        {
            this._watcher = new System.IO.FileSystemWatcher();
            this._watcher.BeginInit();

            this._watcher.Renamed += new System.IO.RenamedEventHandler(this._watcher_Renamed);
            this._watcher.Deleted += new System.IO.FileSystemEventHandler(this._watcher_Deleted);
            this._watcher.Changed += new System.IO.FileSystemEventHandler(this._watcher_Changed);

            this._watcher.EndInit();

            base.InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="RasPhoneBook.Changed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnChanged(EventArgs e)
        {
            this.RaiseEvent<EventArgs>(this.Changed, e);
        }

        /// <summary>
        /// Raises the <see cref="RasPhoneBook.Deleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnDeleted(EventArgs e)
        {
            this.RaiseEvent<EventArgs>(this.Deleted, e);
        }

        /// <summary>
        /// Raises the <see cref="RasPhoneBook.Renamed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnRenamed(RenamedEventArgs e)
        {
            this.RaiseEvent<RenamedEventArgs>(this.Renamed, e);
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.Entries.Load();

            this.OnChanged(EventArgs.Empty);
        }

        private void _watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            this.Entries.Load();

            this.OnDeleted(EventArgs.Empty);
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                this.Path = e.FullPath;

                // Force the file watcher to disable temporarily while the file being monitored is updated.
                this._watcher.EnableRaisingEvents = false;
                this._watcher.Filter = e.Name;
                this._watcher.EnableRaisingEvents = this.EnableFileWatcher;
            }

            this.OnRenamed(e);
        }

        #endregion
    }
}