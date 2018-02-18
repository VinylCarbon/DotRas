Imports DotRas

Public Class MainForm

    Private Sub LoadConnectionsComboBox()
        Me.ConnectionsComboBox.Items.Add(New ComboBoxItem("Choose one...", Nothing))

        For Each connection As RasConnection In Me.Dialer.GetActiveConnections()
            Me.ConnectionsComboBox.Items.Add(New ComboBoxItem(connection.EntryName, connection.EntryId))
        Next
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.LoadConnectionsComboBox()
    End Sub

    Private Sub GetAddressButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetAddressButton.Click
        For Each connection As RasConnection In Me.Dialer.GetActiveConnections()
            If connection.EntryId = CType(CType(Me.ConnectionsComboBox.SelectedValue, ComboBoxItem).Value, Guid) Then
                Dim ipAddress As RasIPInfo = CType(connection.GetProjectionInfo(RasProjectionType.IP), RasIPInfo)

                If ipAddress IsNot Nothing Then
                    Me.ClientAddressTextBox.Text = ipAddress.IPAddress.ToString()
                    Me.ServerAddressTextBox.Text = ipAddress.ServerIPAddress.ToString()
                End If
            End If
        Next
    End Sub

    Private Sub ConnectionsComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectionsComboBox.SelectedIndexChanged
        Me.GetAddressButton.Enabled = Me.ConnectionsComboBox.SelectedIndex > 1
    End Sub

End Class