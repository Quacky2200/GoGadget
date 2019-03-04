<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDock
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
        Me.components = New System.ComponentModel.Container()
        Me.pDock = New System.Windows.Forms.Panel()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LaunchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pDock
        '
        Me.pDock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pDock.Location = New System.Drawing.Point(0, 0)
        Me.pDock.Name = "pDock"
        Me.pDock.Size = New System.Drawing.Size(50, 10)
        Me.pDock.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.BackColor = System.Drawing.Color.White
        Me.ContextMenuStrip1.DropShadowEnabled = False
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LaunchToolStripMenuItem, Me.RenameToolStripMenuItem, Me.ChangeIconToolStripMenuItem, Me.RenameToolStripMenuItem1})
        Me.ContextMenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowImageMargin = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(117, 92)
        '
        'LaunchToolStripMenuItem
        '
        Me.LaunchToolStripMenuItem.BackColor = System.Drawing.Color.White
        Me.LaunchToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.LaunchToolStripMenuItem.Name = "LaunchToolStripMenuItem"
        Me.LaunchToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.LaunchToolStripMenuItem.Text = "Launch"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'ChangeIconToolStripMenuItem
        '
        Me.ChangeIconToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.ChangeIconToolStripMenuItem.Name = "ChangeIconToolStripMenuItem"
        Me.ChangeIconToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.ChangeIconToolStripMenuItem.Text = "Change Icon"
        '
        'RenameToolStripMenuItem1
        '
        Me.RenameToolStripMenuItem1.ForeColor = System.Drawing.Color.Black
        Me.RenameToolStripMenuItem1.Name = "RenameToolStripMenuItem1"
        Me.RenameToolStripMenuItem1.Size = New System.Drawing.Size(116, 22)
        Me.RenameToolStripMenuItem1.Text = "Remove"
        '
        'frmDock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gray
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(50, 10)
        Me.Controls.Add(Me.pDock)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximumSize = New System.Drawing.Size(3000, 3000)
        Me.Name = "frmDock"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "GoGadget!"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.Gray
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pDock As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents LaunchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeIconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem

End Class
