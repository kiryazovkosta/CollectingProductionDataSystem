<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestForm
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
        Me.btnTest = New System.Windows.Forms.Button()
        Me.txtTrace = New System.Windows.Forms.TextBox()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnError = New System.Windows.Forms.Button()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnTest
        '
        Me.btnTest.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTest.Location = New System.Drawing.Point(589, 429)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(108, 21)
        Me.btnTest.TabIndex = 0
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'txtTrace
        '
        Me.txtTrace.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTrace.Location = New System.Drawing.Point(11, 21)
        Me.txtTrace.MaxLength = 3276700
        Me.txtTrace.Multiline = True
        Me.txtTrace.Name = "txtTrace"
        Me.txtTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTrace.Size = New System.Drawing.Size(686, 393)
        Me.txtTrace.TabIndex = 1
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Location = New System.Drawing.Point(475, 429)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(108, 21)
        Me.btnClear.TabIndex = 2
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnError
        '
        Me.btnError.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnError.Location = New System.Drawing.Point(133, 429)
        Me.btnError.Name = "btnError"
        Me.btnError.Size = New System.Drawing.Size(108, 21)
        Me.btnError.TabIndex = 5
        Me.btnError.Text = "Test ErrorMessage"
        Me.btnError.UseVisualStyleBackColor = True
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(87, 429)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(40, 21)
        Me.btnRun.TabIndex = 0
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Location = New System.Drawing.Point(41, 429)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(40, 21)
        Me.btnStop.TabIndex = 0
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 462)
        Me.Controls.Add(Me.btnError)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.txtTrace)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.btnTest)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents txtTrace As System.Windows.Forms.TextBox
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnError As System.Windows.Forms.Button
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnStop As System.Windows.Forms.Button
End Class
