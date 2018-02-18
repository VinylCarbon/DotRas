<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Dialer = New DotRas.RasDialer(Me.components)
        Me.AllUsersPhoneBook = New DotRas.RasPhoneBook(Me.components)
        Me.StatusTextBox = New System.Windows.Forms.TextBox
        Me.DialButton = New System.Windows.Forms.Button
        Me.CreateEntryButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Dialer
        '
        Me.Dialer.EapData = Nothing
        Me.Dialer.SynchronizingObject = Me
        '
        'StatusTextBox
        '
        Me.StatusTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusTextBox.Location = New System.Drawing.Point(12, 12)
        Me.StatusTextBox.Multiline = True
        Me.StatusTextBox.Name = "StatusTextBox"
        Me.StatusTextBox.Size = New System.Drawing.Size(321, 220)
        Me.StatusTextBox.TabIndex = 3
        '
        'DialButton
        '
        Me.DialButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DialButton.Location = New System.Drawing.Point(177, 294)
        Me.DialButton.Name = "DialButton"
        Me.DialButton.Size = New System.Drawing.Size(75, 23)
        Me.DialButton.TabIndex = 5
        Me.DialButton.Text = "&Dial"
        Me.DialButton.UseVisualStyleBackColor = True
        '
        'CreateEntryButton
        '
        Me.CreateEntryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CreateEntryButton.Location = New System.Drawing.Point(258, 294)
        Me.CreateEntryButton.Name = "CreateEntryButton"
        Me.CreateEntryButton.Size = New System.Drawing.Size(75, 23)
        Me.CreateEntryButton.TabIndex = 4
        Me.CreateEntryButton.Text = "&Create Entry"
        Me.CreateEntryButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(345, 329)
        Me.Controls.Add(Me.DialButton)
        Me.Controls.Add(Me.CreateEntryButton)
        Me.Controls.Add(Me.StatusTextBox)
        Me.Name = "MainForm"
        Me.Text = "Create and Dial VPN Entry Example"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Dialer As DotRas.RasDialer
    Friend WithEvents AllUsersPhoneBook As DotRas.RasPhoneBook
    Private WithEvents DialButton As System.Windows.Forms.Button
    Private WithEvents CreateEntryButton As System.Windows.Forms.Button
    Private WithEvents StatusTextBox As System.Windows.Forms.TextBox

End Class
