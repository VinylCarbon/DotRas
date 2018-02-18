using System;
using System.Net;
using System.Windows.Forms;
using DotRas;

namespace DotRas.Samples.CreateAndDialVpnEntry
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Defines the name of the entry being used by the example.
        /// </summary>
        public const string EntryName = "VPN Connection";

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the user clicks the Create Entry button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void CreateEntryButton_Click(object sender, EventArgs e)
        {
            // This opens the phonebook so it can be used. Different overloads here will determine where the phonebook is opened/created.
            this.AllUsersPhoneBook.Open();

            // Create the entry that will be used by the dialer to dial the connection. Entries can be created manually, however the static methods on
            // the RasEntry class shown below contain default information matching that what is set by Windows for each platform.
            RasEntry entry = RasEntry.CreateVpnEntry(EntryName, IPAddress.Loopback.ToString(), RasVpnStrategy.Default,
                RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn));

            // Add the new entry to the phone book.
            this.AllUsersPhoneBook.Entries.Add(entry);           
        }

        /// <summary>
        /// Occurs when the user clicks the Dial button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void DialButton_Click(object sender, EventArgs e)
        {            
            // This button will be used to dial the connection.
            this.Dialer.EntryName = EntryName;
            this.Dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);

            try
            {
                // NOTE: The entry MUST be in the phone book before the connection can be dialed.
                // Begin dialing the connection; this will raise events from the dialer instance.
                this.Dialer.DialAsync(new NetworkCredential("Test", "User"));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Occurs when the dialer state changes during a connection attempt.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="DotRas.StateChangedEventArgs"/> containing event data.</param>
        private void Dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            this.StatusTextBox.AppendText(string.Format("{0}\r\n", e.State.ToString()));
        }

        /// <summary>
        /// Occurs when the dialer has completed a dialing operation.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="DotRas.DialCompletedEventArgs"/> containing event data.</param>
        private void Dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.StatusTextBox.AppendText("Cancelled!");
            }
            else if (e.TimedOut)
            {
                this.StatusTextBox.AppendText("Connection attempt timed out!");
            }
            else if (e.Error != null)
            {
                this.StatusTextBox.AppendText(e.Error.ToString());
            }
            else if (e.Connected)
            {
                this.StatusTextBox.AppendText("Connection successful!");
            }
        }
    }
}
