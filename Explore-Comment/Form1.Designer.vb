<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.AllInOneBTN = New System.Windows.Forms.Button()
        Me.UserBox = New System.Windows.Forms.TextBox()
        Me.PassBox = New System.Windows.Forms.TextBox()
        Me.Good = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CommentsBox = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'AllInOneBTN
        '
        Me.AllInOneBTN.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.AllInOneBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AllInOneBTN.Location = New System.Drawing.Point(0, 132)
        Me.AllInOneBTN.Name = "AllInOneBTN"
        Me.AllInOneBTN.Size = New System.Drawing.Size(126, 23)
        Me.AllInOneBTN.TabIndex = 1
        Me.AllInOneBTN.Text = "Browse Comments"
        Me.AllInOneBTN.UseVisualStyleBackColor = True
        '
        'UserBox
        '
        Me.UserBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UserBox.Location = New System.Drawing.Point(0, 0)
        Me.UserBox.Name = "UserBox"
        Me.UserBox.Size = New System.Drawing.Size(126, 20)
        Me.UserBox.TabIndex = 2
        Me.UserBox.Text = "User"
        Me.UserBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PassBox
        '
        Me.PassBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PassBox.Location = New System.Drawing.Point(0, 19)
        Me.PassBox.Name = "PassBox"
        Me.PassBox.Size = New System.Drawing.Size(126, 20)
        Me.PassBox.TabIndex = 3
        Me.PassBox.Text = "Pass"
        Me.PassBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.PassBox.UseSystemPasswordChar = True
        '
        'Good
        '
        Me.Good.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Good.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Good.Location = New System.Drawing.Point(0, 154)
        Me.Good.Name = "Good"
        Me.Good.Size = New System.Drawing.Size(126, 20)
        Me.Good.TabIndex = 4
        Me.Good.Text = "0"
        Me.Good.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TextBox1.Location = New System.Drawing.Point(0, 173)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(126, 20)
        Me.TextBox1.TabIndex = 5
        Me.TextBox1.Text = "0"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CommentsBox
        '
        Me.CommentsBox.FormattingEnabled = True
        Me.CommentsBox.Location = New System.Drawing.Point(0, 38)
        Me.CommentsBox.Name = "CommentsBox"
        Me.CommentsBox.Size = New System.Drawing.Size(126, 95)
        Me.CommentsBox.TabIndex = 6
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(126, 193)
        Me.Controls.Add(Me.CommentsBox)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Good)
        Me.Controls.Add(Me.PassBox)
        Me.Controls.Add(Me.UserBox)
        Me.Controls.Add(Me.AllInOneBTN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Explore-C"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AllInOneBTN As Button
    Friend WithEvents UserBox As TextBox
    Friend WithEvents PassBox As TextBox
    Friend WithEvents Good As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents CommentsBox As ListBox
End Class
