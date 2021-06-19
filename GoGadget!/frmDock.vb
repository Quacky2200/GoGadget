Imports System.Runtime.InteropServices
Imports IWshRuntimeLibrary
Imports System.Xml
Imports Microsoft.Win32
Public Class frmDock
    Public found As Integer
    Public Database(1, 1, 1)
    Public WithEvents Placer As New PictureBox
    Sub DockColorUpdate()
        If My.Settings.UseWindowsTheme = True Then
            Dim GetColourThread As New System.Threading.Thread(Sub()
                                                                   Dim Filename1 As String = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False).GetValue("Wallpaper").ToString
                                                                   My.Settings.Colour = ListSameColours(Image.FromFile(If(Filename1.Contains("%windir%"), Filename1.Replace("%windir%", "C:\Windows"), Filename1)))
                                                               End Sub)
            GetColourThread.Start()
            frmDockBackground.Background()
            My.Settings.Save()
        End If
    End Sub
    Private Sub MeClosing() Handles Me.FormClosing
        Save()
    End Sub
    Private Sub Loading() Handles MyBase.Load
        AddHandler SystemEvents.UserPreferenceChanged, AddressOf DockColorUpdate
        DockColorUpdate()
        pDock.AllowDrop = True
        With frmDockBackground
            .Height = Screen.PrimaryScreen.WorkingArea.Height
            .Width = 50
            .Top = Screen.PrimaryScreen.WorkingArea.Top
            .Left = Screen.PrimaryScreen.Bounds.Size.Width - .Width
            .Show()
        End With
        Width = 50
        Left = Screen.PrimaryScreen.Bounds.Size.Width - Width
        Top = frmDockBackground.Top
        TopMost = True
        Initialise() 'Load the dock
        Height = Screen.PrimaryScreen.WorkingArea.Height
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function ShowWindowAsync(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    <DllImport("user32.dll")>
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    Public Sub AppEvents(ByVal Sender As Object, ByVal e As MouseEventArgs)
        found = Database(1, 1, 1).IndexOf(Sender)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If Database(1, 1, 1)(found).BackColor <> If(My.Settings.Comparison = 50, ControlPaint.Dark(My.Settings.Colour, 0.15), ControlPaint.LightLight(My.Settings.Colour)) Then
                Dim Program() As Process = Process.GetProcessesByName(Mid(Database(0, 0, 1)(found), Database(0, 0, 1)(found).ToString.LastIndexOf("\") + 2).Replace(".exe", ""))
                For Each p In Process.GetProcesses
                    If p.ProcessName = Mid(Database(0, 0, 1)(found), Database(0, 0, 1)(found).ToString.LastIndexOf("\") + 2).Replace(".exe", "") Then
                        ShowWindowAsync(p.MainWindowHandle, 9)
                        SetForegroundWindow(p.MainWindowHandle)
                    End If
                Next
            Else
                LaunchToolStripMenuItem.PerformClick()
            End If
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            Help.Stop()
            Help.Interval = 1200
            For i = 0 To ContextMenuStrip1.Items.Count - 1
                ContextMenuStrip1.Items(i).Visible = True
            Next i
            Try
                ContextMenuStrip1.Items.Item(0).Text = "Launch " + Database(1, 0, 0)(found)
                ContextMenuStrip1.Show(Int(Screen.PrimaryScreen.Bounds.Width - 50) - ContextMenuStrip1.Width, Database(1, 1, 1)(found).Top + 11)
                ContextMenuStrip1.Opacity = 0.9
                frmDockBackground.tmrDetector.Stop()
            Catch
            End Try
            If Database(1, 1, 1)(found).BackColor <> ControlPaint.LightLight(My.Settings.Colour) Then
                NewWindowToolStripMenuItem1.Visible = True
                CloseToolStripMenuItem2.Visible = True
                LaunchToolStripMenuItem.Visible = False
                RenameToolStripMenuItem.Visible = True
                RenameToolStripMenuItem.Text = Database(1, 0, 0)(found)
                ChangeIconToolStripMenuItem.Visible = False
                RemoveToolStripMenuItem.Visible = False
            Else
                NewWindowToolStripMenuItem1.Visible = False
                CloseToolStripMenuItem2.Visible = False
            End If
        End If
    End Sub
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            If Not Me.DesignMode Then cp.ExStyle = cp.ExStyle Or &H80
            Return cp
        End Get
    End Property
    Sub Save()
        Try
            Debug.Print("Saving to..." & My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml")
            Dim Writer As New System.IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml")
            Writer.WriteLine("<Dock>")
            For i = 0 To Database(1, 0, 0).Count - 1
                Writer.WriteLine("<App title='" & Database(1, 0, 0)(i) & "'>")
                Writer.WriteLine("<image>" & Database(0, 1, 0)(i) & "</image>")
                Writer.WriteLine("<path>" & Database(0, 0, 1)(i) & "</path>")
                Writer.WriteLine("</App>")
            Next
            Writer.WriteLine("</Dock>")
            Writer.Close()
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub
    Sub AddApp(ByVal name As String, ByVal imagepath As String, ByVal path As String, Optional ByRef place As Integer = -1, Optional ByRef dragcolor As Color = Nothing)
        IconSelect.Stop()
        pDock.Controls.Remove(Placer)
        If Database(1, 1, 1).Count <> 0 And place <> -1 Then
            Database(1, 0, 0).Insert(place, name)
            Database(0, 1, 0).Insert(place, imagepath)
            Database(0, 0, 1).Insert(place, path)
        Else
            Database(1, 0, 0).add(name)
            Database(0, 1, 0).add(imagepath)
            Database(0, 0, 1).add(path)
        End If
        Dim NewApp = New PictureBox 'Give a new picturebox
        With NewApp
            .BackColor = If(dragcolor <> Nothing, dragcolor, If(My.Settings.Comparison = 50, ControlPaint.Dark(My.Settings.Colour, 0.15), ControlPaint.LightLight(My.Settings.Colour)))
            Try
                If Database(0, 1, 0)(Database(1, 1, 1).count).Contains("Img\") Then
                    .Image = Image.FromFile(Database(0, 1, 0)(If(place <> -1, place, Database(1, 1, 1).count))) 'Use this to load html bookmarks
                    .SizeMode = PictureBoxSizeMode.StretchImage
                ElseIf Database(0, 1, 0)(If(place <> -1, place, Database(1, 1, 1).count)) = "default" Then
                    .Image = My.Resources.Norm
                    .SizeMode = PictureBoxSizeMode.StretchImage
                Else
                    .Image = RetrieveIcon(Database(0, 1, 0)(If(place <> -1, place, Database(1, 1, 1).count)))
                    .SizeMode = PictureBoxSizeMode.CenterImage
                End If
            Catch ex As Exception
                .Image = My.Resources.Norm 'Use this image whenever something might happen :/
                .SizeMode = PictureBoxSizeMode.StretchImage
            End Try
            .Size = New Point(44, 44)
            If place <> -1 Then
                .Location = New Point(3, place * 47 + 3)
            Else
                .Location = New Point(3, Database(1, 1, 1).Count * 47 + 3)
            End If
            .Name = name
            pDock.Controls.Add(NewApp)
            If place <> -1 Then
                Database(1, 1, 1).Insert(place, NewApp)
            Else
                Database(1, 1, 1).add(NewApp)
            End If
            AddHandler .MouseClick, AddressOf AppEvents 'check what the user wants to do with the app
            AddHandler .MouseHover, AddressOf AppLabel 'Add timer to detect if person wants the app name
            AddHandler .MouseLeave, AddressOf AppLabelRemove 'Remove the app once the mouse leaves
            AddHandler .MouseMove, AddressOf AppDrag 'If the left mouse buttons down, move the app
        End With
        Save()
    End Sub
    Function ListSameColours(ByVal i As Bitmap) As Color
        Dim ListColors As New List(Of String)
        Dim Popular As New List(Of Long) ' Used to found Mean colour
        Dim Scale As Point = New Point(1, 1)
        If i.Height > 100 And i.Width > 100 Then
            Scale = New Point(i.Width / My.Settings.Comparison, i.Height / My.Settings.Comparison)
        End If
        Dim b As New Bitmap(i)
        For x = b.Size.Width / 4 * 1 - 5 To b.Size.Width / 4 * 3 + 5 Step Scale.X 'Get most of the icon's width for the colours
            For y = b.Size.Height / 4 * 1 - 5 To b.Size.Height / 4 * 3 + 5 Step Scale.Y 'Get most of the height
                If Not ListColors.IndexOf(b.GetPixel(x, y).ToArgb.ToString) = -1 Then
                    Popular.Add(ListColors.IndexOf(b.GetPixel(x, y).ToArgb.ToString)) 'Add similar colours to a new list - adding the location of the color
                Else
                    ListColors.Add(b.GetPixel(x, y).ToArgb.ToString) 'But list other colours so if it's found next time...
                End If
            Next
        Next
        Dim Mean As Long
        For res = 0 To Popular.Count - 1
            Mean += Popular(res)
        Next
        Mean = Mean / Popular.Count
        Debug.Print("Rounded Colour: " & Mean)
        Return Color.FromArgb(ListColors(Mean))
    End Function
    Sub Initialise()
        Database(1, 0, 0) = New List(Of String) 'Name
        Database(0, 1, 0) = New List(Of String) 'Picture
        Database(0, 0, 1) = New List(Of String) 'path
        Database(1, 1, 1) = New List(Of PictureBox)
        Dim l As New List(Of String)
        Dim DockXMLReader As New XmlTextReader(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml")
        Try
            Do While (DockXMLReader.Read())
                Select Case DockXMLReader.NodeType
                    Case XmlNodeType.Element 'Display beginning of element.
                        If DockXMLReader.HasAttributes Then 'If attributes exist
                            While DockXMLReader.MoveToNextAttribute()
                                'Display attribute name and value.
                                l.Add(DockXMLReader.Value) 'App Name
                            End While
                        End If
                    Case XmlNodeType.Text 'Display the text in each element.
                        l.Add(DockXMLReader.Value)
                End Select
            Loop
        Catch
        End Try
        DockXMLReader.Close()
        Try
            For i = 0 To l.Count - 1 Step 3
                AddApp(l(i), l(i + 1), l(i + 2))
            Next
        Catch
            MsgBox("Program Crashed on start up, Click OK to restart")
            Application.Restart()
        End Try
        Dim Status As New Timer
        With Status
            .Interval = 100
            AddHandler .Tick, Sub()
                                  Status.Interval = 2500
                                  For i = 0 To Database(1, 1, 1).count - 1
                                      Try
                                          If Process.GetProcessesByName(Mid(Database(0, 0, 1)(i), Database(0, 0, 1)(i).ToString.LastIndexOf("\") + 2).Replace(".exe", "")).Length <> 0 Then
                                              If Database(1, 1, 1)(i).BackColor = If(My.Settings.Comparison = 50, ControlPaint.Dark(My.Settings.Colour, 0.15), ControlPaint.LightLight(My.Settings.Colour)) Then
                                                  Database(1, 1, 1)(i).BackColor = ControlPaint.Dark(ListSameColours(Database(1, 1, 1)(i).Image), 0.05)
                                              End If
                                          Else
                                              Database(1, 1, 1)(i).BackColor = If(My.Settings.Comparison = 50, ControlPaint.Dark(My.Settings.Colour, 0.15), ControlPaint.LightLight(My.Settings.Colour))
                                          End If
                                      Catch
                                      End Try

                                  Next
                              End Sub
            .Start()
        End With
    End Sub
    <System.Runtime.InteropServices.DllImport("shell32.dll")> Shared Function _
    ExtractAssociatedIcon(ByVal hInst As IntPtr, ByVal lpIconPath As String,
                           ByRef lpiIcon As Integer) As IntPtr
    End Function
    Function RetrieveIcon(ByRef str As String) As Image
        Dim hIcon As IntPtr
        hIcon = ExtractAssociatedIcon(Me.Handle, str, 0)
        Return Icon.FromHandle(hIcon).ToBitmap
    End Function
    Sub AppLabelRemove(ByVal sender As Object, ByVal e As System.EventArgs)
        Help.Stop()
        Help.Interval = 1200
        If RenameToolStripMenuItem.Visible = False And CloseToolStripMenuItem2.Visible = False Then
            ContextMenuStrip1.Hide()
        End If
    End Sub
    Private Sub ContextMenuStrip1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContextMenuStrip1.MouseLeave
        RenameToolStripMenuItem.Text = "Rename"
        If Not ContextMenuStrip1.Bounds.Contains(MousePosition) Then
            ContextMenuStrip1.Hide() 'We want to hide this when they hover off the control
            frmDockBackground.tmrDetector.Start() 'and hide the dock if cursor's not there within 1 second
        End If
    End Sub
    Private Sub RenameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenameToolStripMenuItem.Click
        If RenameToolStripMenuItem.Text = "Rename" Then
            frmQuickRename.found = found 'Give the found variable to the program to use locally
            frmQuickRename.Text = "Quick Rename for " + Database(1, 0, 0)(found)
            frmQuickRename.Show()
            frmDockBackground.tmrDetector.Start() 'And hide...
        End If
    End Sub
    Private Sub LaunchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LaunchToolStripMenuItem.Click
        Try
            Process.Start(Database(0, 0, 1)(found))
        Catch
            Dim msg As MsgBoxResult = MsgBox("This item cannot be found, Remove?", MsgBoxStyle.YesNo, "Attention!")
            If msg = MsgBoxResult.Yes Then
                RemoveToolStripMenuItem.PerformClick()
            End If
        End Try
        frmDockBackground.tmrDetector.Start() 'And hide again.
    End Sub
    Private Sub ChangeIconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeIconToolStripMenuItem.Click
        Dim o As New OpenFileDialog
        o.ShowDialog()
        If o.FileName.Length > 1 Then
            Database(0, 1, 0)(found) = o.FileName 'We get the filename from OpenFileDialog1 and store it here
            Database(1, 1, 1)(found).Image = System.Drawing.Image.FromFile(o.FileName)
        Else
        End If
        My.Settings.Save() 'Save the changes
        frmDockBackground.tmrDetector.Start() 'Always do this so it wont bug out
    End Sub
    Private Sub RemoveToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveToolStripMenuItem.Click
        pDock.Controls.Remove(Database(1, 1, 1)(found)) 'Remove it from the control
        Database(1, 0, 0).remove(Database(1, 0, 0)(found))
        Database(0, 1, 0).remove(Database(0, 1, 0)(found))
        Database(0, 0, 1).remove(Database(0, 0, 1)(found))
        Database(1, 1, 1).remove(Database(1, 1, 1)(found))
        Save()
        SortAfterRemix() 'Sortafterremix wont sort the top one :/
        frmDockBackground.tmrDetector.Start() 'Start the dock detection and hide if no mouse cursor is there
    End Sub
    Sub SortAfterRemix()
        For i = 0 To Database(1, 1, 1).Count - 1
            If Database(1, 1, 1)(i).Top <> i * 47 + 3 Then
                Do Until Database(1, 1, 1)(i).Top = i * 47 + 3
                    Database(1, 1, 1)(i).Top -= 1
                Loop
            End If
        Next
    End Sub
    Private Sub pDock_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pDock.GotFocus
        Me.Focus()
    End Sub
    Private Sub pDock_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pDock.DragLeave
        Me.Controls.Remove(Placer)
    End Sub
    Sub pDock_Drag_Drop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles pDock.DragEnter, MyBase.DragEnter
        e.Effect = DragDropEffects.Copy
        e.Effect = DragDropEffects.Link
        ' e.Effect = DragDropEffects.Move
    End Sub
    Sub pDock_Drag_Drop_Event(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles pDock.DragDrop, MyBase.DragDrop
        Dim example = e.Data.GetData(DataFormats.Text)
        Debug.Print(example)
        If example = "" Then
            Try
                Dim MyFiles() As String = e.Data.GetData(DataFormats.FileDrop)
                For i = 0 To MyFiles.Count - 1
                    Module1.GetIcon(MyFiles(i))
                Next
            Catch
            End Try
        Else
            Debug.Print("Downloading icon")
            If example.ToLower.Contains("http") Then
                frmDockBackground.tmrDetector.Stop()
                Me.Placer.Image = My.Resources.load
                Dim iframe As New WebBrowser
                Dim filename = New Uri(example).Host.ToString.Replace(".", "_")
                Me.Controls.Add(iframe)
                iframe.ScriptErrorsSuppressed = True
                iframe.Location = New Point(Me.Height, Me.Width)
                iframe.Size = New Point(0, 0)
                iframe.Navigate(example)
                Dim correctfilename = "Img\" + filename + ".ico"
                While Not My.Computer.FileSystem.FileExists("Img\" + filename + ".ico") And Not iframe.DocumentTitle.Length > 1
                    Dim webclient As New Net.WebClient
                    Dim favicon As String = "http://" + New Uri(example).Host + "/favicon.ico"
                    If My.Computer.FileSystem.DirectoryExists("Img") = False Then
                        My.Computer.FileSystem.CreateDirectory("Img")
                    End If
                    Try
                        webclient.DownloadFile(favicon, correctfilename)
                    Catch
                        correctfilename = "default"
                        'Me.AddApp(example, correctfilename, example)
                        Exit While
                    End Try
                End While
                AddHandler iframe.DocumentCompleted, Sub()
                                                         If Me.Placer.Location.Y <> Database(1, 1, 1).Count * 47 + 3 Then
                                                             For i = 0 To Database(1, 1, 1).Count - 1
                                                                 If i * 47 + 3 = Placer.Top Then
                                                                     Me.AddApp(iframe.DocumentTitle, correctfilename, example, i)
                                                                     i = Database(1, 1, 1).Count
                                                                 End If
                                                             Next
                                                         Else
                                                             Me.AddApp(iframe.DocumentTitle, correctfilename, example)
                                                         End If
                                                         Me.Controls.Remove(iframe)
                                                         iframe.Dispose()
                                                     End Sub
            End If
        End If
        SortAfterRemix()
        frmDockBackground.tmrDetector.Start()
    End Sub
    Private Sub NewWindowToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewWindowToolStripMenuItem1.Click
        Process.Start(Database(0, 0, 1)(found))
        frmDockBackground.tmrDetector.Start()
    End Sub

    Private Sub CloseTimeout_Handler(Sender As Timer, e As EventArgs)
        For Each p In Process.GetProcesses
            If p.ProcessName = Mid(Database(0, 0, 1)(found), Database(0, 0, 1)(found).ToString.LastIndexOf("\") + 2).Replace(".exe", "") Then
                Dim res As MsgBoxResult = MsgBox("Program hasn't shutdown yet, kill instead?", MsgBoxStyle.YesNo, "Attention!")
                If res = MsgBoxResult.Yes Then
                    Try
                        p.Kill()
                    Catch
                        MsgBox("Need permissions!?!?!?!")
                    End Try
                Else
                    Sender.Stop()
                End If
            End If
            Sender.Stop()
        Next
    End Sub

    Private Sub CloseToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem2.Click
        For Each p In Process.GetProcesses
            If p.ProcessName = Mid(Database(0, 0, 1)(found), Database(0, 0, 1)(found).ToString.LastIndexOf("\") + 2).Replace(".exe", "") Then
                p.CloseMainWindow()
                Dim timer1 As New Timer With {.Interval = 4000}
                AddHandler timer1.Tick, AddressOf CloseTimeout_Handler
                timer1.Start()
            End If
        Next p
        frmDockBackground.tmrDetector.Start()
    End Sub

    Private Sub Help_Tick(ByVal sender As Timer, ByVal e As System.EventArgs) Handles Help.Tick
        If sender.Interval = 1200 Then
            If frmDockBackground.hidden = False Then
                ContextMenuStrip1.Items(0).Text = Database(1, 0, 0)(found) 'Get the name of the app
                For i = 1 To ContextMenuStrip1.Items.Count - 1
                    ContextMenuStrip1.Items(i).Visible = False 'But we will hide the options so it gives us a simple interface
                Next i
                ContextMenuStrip1.Show(Screen.PrimaryScreen.Bounds.Width - ContextMenuStrip1.Width - Me.Width, Database(1, 1, 1)(found).Top + 11)
            End If
            sender.Interval = 200
        ElseIf sender.Interval = 200 Then
            sender.Stop()
            sender.Interval = 1200
        End If
    End Sub
    Public WithEvents IconSelect As New Timer
    Sub Pick() Handles IconSelect.Tick
        If MouseButtons = Windows.Forms.MouseButtons.None Then
            If pDock.Controls.IndexOf(Placer) <> -1 Then
                pDock.Controls.Remove(Placer)
            End If
            IconSelect.Stop()
            frmDockBackground.tmrDetector.Start()
        End If
        If Database(1, 1, 1).Count <> 0 Then
            If MousePosition.Y > Database(1, 1, 1)(Database(1, 1, 1).Count - 1).Bottom Then
                Placer.Location = New Point(3, Database(1, 1, 1).Count * 47 + 3)
            Else
                For i = 0 To Database(1, 1, 1).Count - 1
                    Dim Apps As PictureBox = Database(1, 1, 1)(i)
                    If Apps.Location.Y > MousePosition.Y And MousePosition.X > Me.Left Then
                        Placer.Location = New Point(3, i * 47 + 3)
                        Moveup(i)
                        i = Database(1, 1, 1).Count - 1
                    Else
                        Database(1, 1, 1)(i).Location = New Point(3, (i) * 47 + 3)
                    End If
                Next
            End If
            If pDock.Controls.IndexOf(Placer) = -1 And MousePosition.X > Me.Left Then
                MakePlacer()
                If frmDockBackground.hidden = True Then
                    frmDockBackground.tmrDockHide.Start()
                End If
            End If
        Else
            MakePlacer()
        End If
    End Sub
    Sub Moveup(ByVal int As Integer)
        For i = int To Database(1, 1, 1).Count - 1
            Database(1, 1, 1)(i).Location = New Point(3, (i + 1) * 47 + 3)
        Next
    End Sub
    Sub MakePlacer()
        Placer = New PictureBox
        Placer.Size = New Point(44, 44)
        Placer.Left = 0
        Placer.BackColor = If(My.Settings.Comparison = 50, ControlPaint.Dark(My.Settings.Colour, 0.15), ControlPaint.LightLight(My.Settings.Colour))
        Placer.SizeMode = PictureBoxSizeMode.StretchImage ' stretch animation
        pDock.Controls.Add(Placer)
        Placer.Show()
        Placer.Location = New Point(3, 3)
    End Sub
    Sub AppLabel(ByVal sender As Object, ByVal e As System.EventArgs)
        found = Database(1, 1, 1).IndexOf(sender)
        Help.Start()
    End Sub
    Dim Dragging As Boolean = False
    Sub AppDrag(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left And Dragging = False Then
            Dragging = True
            Try
                Dim f As New Form
                Dim AppName = Database(1, 0, 0)(found)
                Dim AppPicture = Database(0, 1, 0)(found)
                Dim AppLink = Database(0, 0, 1)(found)
                f.BackColor = Database(1, 1, 1)(found).BackColor
                f.ShowInTaskbar = False
                f.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                f.BackgroundImageLayout = ImageLayout.Center
                f.BackgroundImage = Database(1, 1, 1)(found).Image
                f.Show()
                f.TopLevel = True
                f.TopMost = True
                f.BringToFront()
                f.Size = New Point(44, 44)
                f.Opacity = 0.8
                Database(1, 0, 0).remove(Database(1, 0, 0)(found))
                Database(0, 1, 0).remove(Database(0, 1, 0)(found))
                Database(0, 0, 1).remove(Database(0, 0, 1)(found))
                pDock.Controls.Remove(Database(1, 1, 1)(found))
                Database(1, 1, 1).remove(Database(1, 1, 1)(found))
                frmDockBackground.tmrDragDrop.Stop()
                If pDock.Controls.IndexOf(Placer) = -1 Then
                    MakePlacer()
                End If
                IconSelect.Start()
                Dim t As New Timer
                With t
                    .Interval = 10
                    .Start()
                    AddHandler .Tick, Sub()
                                          If MouseButtons = Windows.Forms.MouseButtons.Left Then
                                              Help.Stop()
                                              If MousePosition.X > Me.Left Then
                                                  If f.Visible = False Then
                                                      f.Show()
                                                  End If
                                                  f.Location = New Point(MousePosition.X + 3, MousePosition.Y + 3)
                                              Else
                                                  f.Hide()
                                              End If
                                          Else
                                              If MousePosition.X > Me.Left Then
                                                  If Placer.Top = Database(1, 1, 1).Count * 47 + 3 Then
                                                      AddApp(AppName, AppPicture, AppLink, -1, f.BackColor)
                                                  Else
                                                      For i = 0 To Database(1, 1, 1).Count - 1
                                                          If i * 47 + 3 = Placer.Top Then
                                                              AddApp(AppName, AppPicture, AppLink, i, f.BackColor)
                                                              i = Database(1, 1, 1).Count
                                                          End If
                                                      Next
                                                  End If
                                              End If
                                              f.Hide()
                                              f.Dispose()
                                              IconSelect.Stop()
                                              Save()
                                              pDock.Controls.Remove(Placer)
                                              Dragging = False
                                              t.Stop()
                                          End If
                                      End Sub
                End With
                frmDockBackground.tmrDragDrop.Start()
            Catch
                Dragging = False
            End Try
        End If
    End Sub
    Sub MoveDown()
        For i = 0 To Database(1, 1, 1).Count - 1
            Database(1, 1, 1)(i).top += 49 + 3
        Next
    End Sub
End Class


