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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDock))
        Me.pDock = New System.Windows.Forms.Panel()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LaunchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewWindowToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Help = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pDock
        '
        Me.pDock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pDock.Location = New System.Drawing.Point(0, 0)
        Me.pDock.Name = "pDock"
        Me.pDock.Size = New System.Drawing.Size(21, 10)
        Me.pDock.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.BackColor = System.Drawing.Color.White
        Me.ContextMenuStrip1.DropShadowEnabled = False
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LaunchToolStripMenuItem, Me.RenameToolStripMenuItem, Me.ChangeIconToolStripMenuItem, Me.RemoveToolStripMenuItem, Me.NewWindowToolStripMenuItem1, Me.CloseToolStripMenuItem2})
        Me.ContextMenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowImageMargin = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(121, 136)
        '
        'LaunchToolStripMenuItem
        '
        Me.LaunchToolStripMenuItem.BackColor = System.Drawing.Color.White
        Me.LaunchToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.LaunchToolStripMenuItem.Name = "LaunchToolStripMenuItem"
        Me.LaunchToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.LaunchToolStripMenuItem.Text = "Launch"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'ChangeIconToolStripMenuItem
        '
        Me.ChangeIconToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.ChangeIconToolStripMenuItem.Name = "ChangeIconToolStripMenuItem"
        Me.ChangeIconToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.ChangeIconToolStripMenuItem.Text = "Change Icon"
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.ForeColor = System.Drawing.Color.Black
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.RemoveToolStripMenuItem.Text = "Remove"
        '
        'NewWindowToolStripMenuItem1
        '
        Me.NewWindowToolStripMenuItem1.Name = "NewWindowToolStripMenuItem1"
        Me.NewWindowToolStripMenuItem1.Size = New System.Drawing.Size(120, 22)
        Me.NewWindowToolStripMenuItem1.Text = "New Window"
        Me.NewWindowToolStripMenuItem1.Visible = False
        '
        'CloseToolStripMenuItem2
        '
        Me.CloseToolStripMenuItem2.Name = "CloseToolStripMenuItem2"
        Me.CloseToolStripMenuItem2.Size = New System.Drawing.Size(120, 22)
        Me.CloseToolStripMenuItem2.Text = "Close"
        Me.CloseToolStripMenuItem2.Visible = False
        '
        'Help
        '
        Me.Help.Interval = 1200
        '
        'frmDock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gray
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(21, 10)
        Me.Controls.Add(Me.pDock)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
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
    Friend WithEvents RemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewWindowToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Help As System.Windows.Forms.Timer

End Class
