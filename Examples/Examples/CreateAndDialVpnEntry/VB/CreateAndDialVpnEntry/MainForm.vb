Imports System.Net
Imports DotRas

Public Class MainForm
    Public Const EntryName As String = "VPN Connection"

    Private Sub CreateEntryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateEntryButton.Click
        ' This opens the phonebook so it can be used. Different overloads here will determine where the phonebook is opened/created.
        Me.AllUsersPhoneBook.Open()

        ' Create the entry that will be used by the dialer to dial the connection. Entries can be created manually, however the static methods on
        ' the RasEntry class shown below contain default information matching that what is set by Windows for each platform.
        Dim entry As RasEntry = RasEntry.CreateVpnEntry(EntryName, IPAddress.Loopback.ToString(), RasVpnStrategy.Default, RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn))

        ' Add the new entry to the phone book.
        Me.AllUsersPhoneBook.Entries.Add(entry)
    End Sub

    Private Sub DialButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DialButton.Click
        ' This button will be used to dial the connection.
        Me.Dialer.EntryName = EntryName
        Me.Dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers)

        Try
            ' NOTE: The entry MUST be in the phone book before the connection can be dialed.
            ' Begin dialing the connection; this will raise events from the dialer instance.
            Me.Dialer.DialAsync(New NetworkCredential("Test", "User"))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Dialer_StateChanged(ByVal sender As System.Object, ByVal e As DotRas.StateChangedEventArgs) Handles Dialer.StateChanged
        Me.StatusTextBox.AppendText(e.State.ToString() + Chr(13) + Chr(10))
    End Sub

    Private Sub Dialer_DialCompleted(ByVal sender As System.Object, ByVal e As DotRas.DialCompletedEventArgs) Handles Dialer.DialCompleted
        If (e.Cancelled) Then
            Me.StatusTextBox.AppendText("Cancelled!")
        ElseIf (e.TimedOut) Then
            Me.StatusTextBox.AppendText("Connection attempt timed out!")
        ElseIf (e.Error IsNot Nothing) Then
            Me.StatusTextBox.AppendText(e.Error.ToString())
        ElseIf (e.Connected) Then
            Me.StatusTextBox.AppendText("Connection successful!")
        End If
    End Sub

End Class
