Imports System.Runtime.InteropServices
Imports IWshRuntimeLibrary
Public Class frmDock
    Public WithEvents Placer As New PictureBox
    Private Sub Loading(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetUp()
        TopMost = True
        Refresh1()
        frmDockBackground.Show()
        Height = Screen.PrimaryScreen.WorkingArea.Height
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub
    Public Sub AppMove(ByVal Sender As Object, ByVal e As MouseEventArgs)
        found = App1.IndexOf(Sender)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If found = 0 Then
                frmDockAdd.Show()
            Else
                LaunchToolStripMenuItem.PerformClick()
            End If
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            If found = 0 Then
                frmDockTools.Show()
            Else
                For i = 0 To ContextMenuStrip1.Items.Count - 1
                    ContextMenuStrip1.Items(i).Visible = True
                Next i
                DockFunctions.times = 0
                Try
                    ContextMenuStrip1.Items.Item(0).Text = "Launch " + My.Settings.name(found)
                    ContextMenuStrip1.Show(Int(Screen.PrimaryScreen.Bounds.Width - 50) - ContextMenuStrip1.Width, Border1(found).Top + 12)
                    ContextMenuStrip1.Opacity = 0.9
                    frmDockBackground.tmrDetector.Stop()
                Catch
                End Try
            End If
        End If
    End Sub
    Dim WithEvents tmrIconMove As New Timer
    Dim int1 As Integer
    Sub tmrIconMove_Tick() Handles tmrIconMove.Tick
        Try
            If int1 = 48 Then
                int1 = 0
                tmrIconMove.Stop()
            Else
                For i = found To My.Settings.name.Count - 1
                    Border1(i).Top -= 3
                Next
                int1 += 3
            End If
        Catch
        End Try

    End Sub
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            If Not Me.DesignMode Then cp.ExStyle = cp.ExStyle Or &H80
            Return cp
        End Get
    End Property
    Sub pDock_Drag_Drop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles pDock.DragEnter, MyBase.DragEnter
        e.Effect = DragDropEffects.All
        frmDockBackground.tmrDetector.Start()
    End Sub
    Sub pDock_Drag_Drop_Event(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles pDock.DragDrop, MyBase.DragDrop
        e.Effect = DragDropEffects.Copy
        Dim example = e.Data.GetData(DataFormats.Text) 'Text, html etc
        Dim ex2() As String = e.Data.GetData(DataFormats.FileDrop) 'Files dropped
        If Not IsNothing(ex2) Then
            For i = 0 To ex2.Count - 1
                Dim str As String = Mid(ex2(i), ex2(i).LastIndexOf(".") + 1)
                If ex2(i).Contains("http://") Then
                    RetrieveWebObject(ex2(i))
                ElseIf ex2(i).EndsWith(".exe") Or ex2(i).EndsWith(".lnk") Then
                    RetrieveExecutable(ex2(i))
                ElseIf ex2(i).EndsWith("\") Then
                    MsgBox("Found Folder")
                ElseIf ex2(i).EndsWith(Str) Then
                    MsgBox("Unknown File")
                End If
            Next
        ElseIf Not IsNothing(example) Then
            frmDockBackground.tmrDetector.Stop()
            If example.Contains("http") Then
                RetrieveWebObject(example)
            Else
                Dim MyFiles() As String = e.Data.GetData(DataFormats.FileDrop)
                For i = 0 To MyFiles.Count - 1
                    If My.Settings.link.IndexOf(MyFiles(i)) < 0 Then
                        If MyFiles(i).EndsWith(".lnk") Or MyFiles(i).EndsWith(".exe") Then
                            RetrieveExecutable(example)
                        End If
                    End If
                Next
            End If
        End If
        Try
            pDock.Controls.Remove(Placer)
            Placer.Dispose()
        Catch ex As Exception
            'Already disposed
        End Try
        frmDockBackground.tmrDetector.Start()
    End Sub
    Public Sub SetUp()
        pDock.AllowDrop = True
        frmDockBackground.Height = Screen.PrimaryScreen.WorkingArea.Height
        frmDockBackground.Width = 50
        Width = 50
        frmDockBackground.Top = Screen.PrimaryScreen.WorkingArea.Top
        frmDockBackground.Left = Screen.PrimaryScreen.Bounds.Size.Width - frmDockBackground.Width
        Left = Screen.PrimaryScreen.Bounds.Size.Width - Width
        Top = frmDockBackground.Top
    End Sub
    Sub EnableAppDrag(ByVal sender As Object, ByVal e As System.EventArgs)
        'pop out the icon
        'delete the old icon
        'make make where icons going to go, e.g. below hovered one
        'mouseup - put icon where it is
        Dim name, link, picture As String
        found = App1.IndexOf(sender)
        name = My.Settings.name(found)
        link = My.Settings.link(found)
        picture = My.Settings.picturelink(found)
        Dim f1 As Integer = found
        Dim TempFrm As New Form
        TempFrm.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        TempFrm.Size = Border1(0).Size
        TempFrm.BackColor = Me.BackColor
        TempFrm.TransparencyKey = Me.BackColor
        TempFrm.Controls.Add(New PictureBox)
        TempFrm.Controls.Add(New PictureBox)
        TempFrm.Controls(0).BackgroundImage = Border1(found).Image
        TempFrm.Controls(0).BackgroundImageLayout = ImageLayout.Stretch
        TempFrm.Controls(0).Size = Border1(0).Size
        TempFrm.Controls(1).Left = 2
        TempFrm.Controls(1).Top = 2
        TempFrm.Controls(1).BackgroundImage = App1(found).Image
        TempFrm.Controls(1).BackColor = Color.White
        TempFrm.Controls(1).BackgroundImageLayout = ImageLayout.Stretch
        TempFrm.Controls(1).Size = App1(0).Size
        TempFrm.Controls(1).BringToFront()
        TempFrm.ShowInTaskbar = False
        TempFrm.ShowIcon = False
        TempFrm.Location = New Point(MousePosition.X - Border1(0).Width / 2, MousePosition.Y - Border1(0).Height / 2)
        Dim timer1 As New Timer
        timer1.Interval = 375
        AddHandler timer1.Tick, Sub()
                                    Dim ref As Integer = If(Math.Round(MousePosition.Y / 44) - 1 > My.Settings.name.Count - 1, My.Settings.name.Count - 1, Math.Round(MousePosition.Y / 44) - 1)
                                    If MouseButtons = Windows.Forms.MouseButtons.Left Then
                                        If timer1.Interval = 375 Then
                                            'Run once 
                                            RenameToolStripMenuItem1.PerformClick()
                                            pDock.Controls.Remove(Placer)
                                            Placer.Dispose()
                                            Placer = New PictureBox
                                            Placer.Size = Border1(0).Size
                                            Placer.BackgroundImage = Image.FromFile("spacer.png")
                                            pDock.Controls.Add(Placer)
                                            Placer.Show()
                                            Placer.SendToBack()
                                        End If
                                        timer1.Interval = 10
                                        TempFrm.Show()
                                        TempFrm.TopMost = True
                                        TempFrm.BringToFront()
                                        TempFrm.Location = New Point(MousePosition.X - Border1(0).Width / 2, MousePosition.Y - Border1(0).Height / 2)
                                        If ref > 0 And MousePosition.X > Screen.PrimaryScreen.Bounds.Width - Me.Width Then
                                            Placer.Location = New Point(Border1(0).Left, Border1(ref).Top + 22)
                                        End If
                                    Else
                                        timer1.Interval = 1000
                                        timer1.Stop()
                                        If TempFrm.Visible Then
                                            If MousePosition.X > Screen.PrimaryScreen.Bounds.Width - Me.Width Then
                                                My.Settings.name.Insert(ref + 1, name)
                                                My.Settings.picturelink.Insert(ref + 1, picture)
                                                My.Settings.link.Insert(ref + 1, link)
                                                My.Settings.Save()
                                                Refresh1()
                                            End If
                                        End If
                                        pDock.Controls.Remove(Placer)
                                        TempFrm.Close()
                                    End If
                                End Sub
        timer1.Start()
    End Sub
    Sub Refresh1()
        If My.Settings.name.Count = My.Settings.link.Count And My.Settings.link.Count = My.Settings.picturelink.Count Then
            'Reconfigure
            pDock.Controls.Clear() 'Clear the control as it's easier to remove the Border1/apps
            My.Settings.Reload()
            'Adding apps
            For i = 0 To My.Settings.name.Count - 1
                ApplyApp(i, My.Settings.name.Item(i), My.Settings.link.Item(i), My.Settings.picturelink.Item(i), False)
            Next i
        Else
            MsgBox("User.config is corrupt! Dock Resetting...") 'We cant make apps if the lists in my.settings are unbalanced
            For i = 1 To 500 ' I reset them 
                Try
                    My.Settings.link.Remove(My.Settings.link(i))
                    My.Settings.name.Remove(My.Settings.name(i))
                    My.Settings.picturelink.Remove(My.Settings.picturelink(i))
                Catch ex As Exception
                    i = 500
                End Try
            Next
            My.Settings.Save()
            Application.Restart()
        End If
    End Sub
    Private Sub ContextMenuStrip1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContextMenuStrip1.MouseLeave
        If Not ContextMenuStrip1.Bounds.Contains(MousePosition) Then
            ContextMenuStrip1.Hide() 'We want to hide this when they hover off the control
            frmDockBackground.tmrDetector.Start() 'and hide the dock if no cursor's there within 1 second
        End If
    End Sub
    Private Sub RenameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenameToolStripMenuItem.Click
        frmQuickRename.found = found 'Give the found variable to the program to use locally
        frmQuickRename.Text = "Quick Rename for " + My.Settings.name.Item(found)
        frmQuickRename.Show()
        frmDockBackground.tmrDetector.Start() 'And hide...
    End Sub
    Private Sub LaunchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LaunchToolStripMenuItem.Click
        'Process.Start(My.Settings.link.Item(found)) 'starting it in a defualt web browser
        Try
            Process.Start(My.Settings.link.Item(found))
        Catch
            Dim msg As MsgBoxResult = MsgBox("This item cannot be found, Remove?", MsgBoxStyle.YesNo, "Attention!")
            If msg = MsgBoxResult.Yes Then
                RenameToolStripMenuItem1.PerformClick()
            End If
        End Try
        frmDockBackground.tmrDetector.Start() 'And hide again.
    End Sub
    Private Sub ChangeIconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeIconToolStripMenuItem.Click
        frmDockAdd.OpenFileDialog1.ShowDialog()
        If frmDockAdd.OpenFileDialog1.FileName.Length > 1 Then
            My.Settings.picturelink.Item(found) = frmDockAdd.OpenFileDialog1.FileName 'We get the filename from OpenFileDialog1 and store it here
            App1(found).Image = System.Drawing.Image.FromFile(frmDockAdd.OpenFileDialog1.FileName)
        Else
            Dim m As MsgBoxResult = MsgBox("You either haven't picked a filename or clicked Cancel, Pick again?", MsgBoxStyle.YesNo, "Attention!")
            If m = MsgBoxResult.Yes Then
                ChangeIconToolStripMenuItem.PerformClick() 'We can loop it easily by doing it like this
            End If
        End If
        My.Settings.Save() 'Save the changes
        frmDockBackground.tmrDetector.Start() 'Always do this so it wont bug out
    End Sub
    Private Sub RenameToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenameToolStripMenuItem1.Click
        tmrHover.Stop()
        pDock.Controls.Remove(Border1(found)) 'Remove it from the control
        My.Settings.link.Remove(My.Settings.link.Item(found)) 'Remove it from the link section
        My.Settings.picturelink.Remove(My.Settings.picturelink.Item(found)) 'Remove the picture
        My.Settings.name.Remove(My.Settings.name.Item(found))
        My.Settings.Save() 'Remove the name
        Border1(found).Controls.Remove(App1(found)) 'Remove the picture
        Border1.Remove(Border1(found))
        App1.Remove(App1(found))
        'Save it then refresh with...
        tmrIconMove.Interval = 10 'This timer will move the icons 
        tmrIconMove.Start() 'and after will refresh so all the apps will be detected later on - flush the old used app
        frmDockBackground.tmrDetector.Start() 'Start the dock detection and hide if no mouse cursor is there
    End Sub
    Private Sub pDock_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pDock.GotFocus
        Me.Focus()
    End Sub
    Private Sub pDock_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pDock.DragLeave
        Me.Controls.Remove(Placer)
    End Sub
End Class

