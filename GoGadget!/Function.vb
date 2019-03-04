Imports IWshRuntimeLibrary
Module DockFunctions
    Public App1 As New List(Of PictureBox)
    Public Border1 As New List(Of PictureBox)
    Public found As Integer
    Public Function RetrieveWebObject(ByRef uri As String) As Boolean
        If Not My.Settings.link.Contains(uri) Then
            frmDock.Placer.Image = Image.FromFile("load.gif")
            Dim frame As New Form
            Dim iframe As New WebBrowser
            Dim filename = New Uri(uri).Host.ToString.Replace(".", "_")
            frame.Controls.Add(iframe)
            frame.Show()
            frame.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            iframe.ScriptErrorsSuppressed = True
            frame.Location = New Point(0, 0)
            frame.Size = New Point(0, 0)
            frame.ShowInTaskbar = False
            iframe.Navigate(uri)
            iframe.Dock = DockStyle.Fill
            Dim str As String = uri
            AddHandler iframe.DocumentCompleted, Sub()
                                                     ApplyApp(Border1.Count, iframe.DocumentTitle, str, "Img\" + filename + ".ico", True)
                                                     frame.Close()
                                                     frame.Dispose()
                                                 End Sub
            While Not My.Computer.FileSystem.FileExists("Img\" + filename + ".ico") And Not iframe.DocumentTitle.Length > 1
                Try
                    Dim webclient As New Net.WebClient
                    Dim favicon As String = "http://" + New Uri(uri).Host + "/favicon.ico"
                    If My.Computer.FileSystem.DirectoryExists("Img") = False Then
                        My.Computer.FileSystem.CreateDirectory("Img")
                    End If
                    webclient.DownloadFile(favicon, "Img\" + filename + ".ico")
                Catch ex As Exception
                    AddHandler iframe.DocumentCompleted, Sub()
                                                             If iframe.Url = New Uri(str) Then
                                                                 iframe.Navigate("http://" + New Uri(str).Host + "/favicon.ico")
                                                                 frame.FormBorderStyle = FormBorderStyle.Fixed3D
                                                                 frame.Size = New Point(300, 300)
                                                             Else
                                                                 iframe.ShowSaveAsDialog()
                                                             End If
                                                         End Sub
                End Try
            End While
        End If
    End Function
    Public Function MoveObjects(ByRef Application As Integer) As Boolean

    End Function
    Public Function RetrieveExecutable(ByRef ExString As String) As Boolean
        Try
            Dim name As String = Mid(ExString, ExString.LastIndexOf("\") + 2, ExString.Length)
            Dim exepath As String
            Try
                name = name.Replace(".exe", "")
                name = name.Replace(".lnk", "")
            Catch ex As Exception
            End Try
            If ExString.EndsWith(".lnk") Then
                Dim w As New WshShell
                Dim lnk As WshShortcut
                lnk = w.CreateShortcut(ExString)
                exepath = lnk.TargetPath
            Else
                exepath = ExString
            End If
            ApplyApp(Border1.Count, name, exepath, exepath, True)
            frmDock.pDock.Controls.Remove(frmDock.Placer)
            Return True
        Catch
            Return False
        End Try
    End Function
    Public Function Colouring(ByRef Opacity As Double, ByRef SelectedColor As Color) As Boolean
        Return False 'Not implemented yet
    End Function
    Public Function ApplyApp(ByVal id As Integer, ByVal name As String, ByVal url As String, ByVal source As String, ByVal addtosettings As Boolean) As Boolean
        If addtosettings = False Then
            App1.Add(New PictureBox) 'Give a new picturebox
            App1(id).BackColor = Color.Transparent 'make it transparent for images
            Try
                App1(id).Image = System.Drawing.Image.FromFile(My.Settings.picturelink.Item(id)) 'Detect if image is fualty
            Catch
                App1(id).Image = System.Drawing.Image.FromFile("norm.png") 'use this image if it's fualty - default
            End Try 'use this image if it's fualty - default
            Try
                App1(id).Image = ReturnIcon(My.Settings.picturelink.Item(id), 0).ToBitmap
            Catch
            End Try
            App1(id).SizeMode = PictureBoxSizeMode.StretchImage
            App1(id).Height = 40
            App1(id).Width = 40
            App1(id).Left = 2
            App1(id).Top = 2
            Border1.Add(New PictureBox)
            Border1(id).Name = My.Settings.name.Item(id)
            App1(id).Name = My.Settings.name.Item(id)
            Border1(id).BackColor = Color.Transparent
            Border1(id).Controls.Add(App1(id))
            Border1(id).SizeMode = PictureBoxSizeMode.StretchImage
            Dim b As New Bitmap(44, 44)
            Dim g As Graphics = Graphics.FromImage(b)
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(Image.FromFile("Back3.png"), 0, 0, 44, 44)
            Border1(id).Image = b
            Border1(id).Anchor = AnchorStyles.Right + AnchorStyles.Top
            Border1(id).Left = 3
            If id = 0 Then
                Border1(id).Top = 3
            Else
                Border1(id).Top = Border1(id - 1).Top + 48
            End If
            Border1(id).Height = 44
            Border1(id).Width = 44
            frmDock.pDock.Controls.Add(Border1(id))
            AddHandler App1(id).Click, AddressOf frmDock.AppMove
            AddHandler App1(id).MouseHover, AddressOf AppLabel
            AddHandler App1(id).MouseLeave, AddressOf AppLabelRemove 'Adding some event handlers
            AddHandler App1(id).MouseDown, AddressOf frmDock.EnableAppDrag
        ElseIf addtosettings = True Then
            My.Settings.name.Add(name)
            My.Settings.picturelink.Add(source)
            My.Settings.link.Add(url)
            My.Settings.Save()
            ApplyApp(id, name, url, source, False)
        End If
    End Function
    Declare Function extracticon Lib "shell32.dll" Alias "ExtractIconExA" (ByVal lpszFile As String, ByVal nIconIndex As Integer, ByRef phiconLarge As Integer, ByRef phiconSmall As Integer, ByVal nIcons As Integer) As Integer
    Private Function ReturnIcon(ByVal Path As String, ByVal index As Integer, Optional ByVal small As Boolean = False) As Icon
        Dim bigicon As Integer
        Dim smallicon As Integer
        extracticon(Path, index, bigicon, smallicon, 1)
        If bigicon = 0 Then
            extracticon(Path, 0, bigicon, smallicon, 1)
        End If
        If bigicon <> 0 Then
            If small = False Then
                Return Icon.FromHandle(bigicon)
            Else
                Return Icon.FromHandle(smallicon)
            End If
        Else
            Return Nothing
        End If
    End Function
    Public WithEvents tmrHover As New Timer
    Sub AppLabel(ByVal sender As Object, ByVal e As System.EventArgs) 'This gets our label name
        found = App1.IndexOf(sender)
        times = 0
        tmrHover.Interval = 1000
        tmrHover.Start()
    End Sub
    Sub AppLabelRemove(ByVal sender As Object, ByVal e As System.EventArgs)
        'Remove the Applabel when we stop hovering on the app unless the timeout occured
        If frmDock.ContextMenuStrip1.Items(1).Visible = False Then
            frmDock.ContextMenuStrip1.Hide()
            For i = 0 To frmDock.ContextMenuStrip1.Items.Count - 1
                frmDock.ContextMenuStrip1.Items(i).Visible = True 'And we'll show them as well just to prevent hidden items
            Next i
            tmrHover.Stop() 'Stop the hover timer before it crashes
        End If
    End Sub
    Public times As Integer
    Sub tmrHover_tick() Handles tmrHover.Tick 'Used to show the app name after hovering for 1/2 seconds
        Try
            If frmDock.ContextMenuStrip1.Items(1).Visible = False Then
                If times = 0 Then
                    frmDock.ContextMenuStrip1.Items(0).Text = My.Settings.name.Item(found) 'Get the name of the app
                    For i = 1 To frmDock.ContextMenuStrip1.Items.Count - 1
                        frmDock.ContextMenuStrip1.Items(i).Visible = False 'But we will hide the options so it gives us a simple interface
                        'Cheekily brilliant...lol - as long as it saves some coding in the mean while
                    Next i
                    frmDock.ContextMenuStrip1.Show(Int(Screen.PrimaryScreen.Bounds.Width - 50) - frmDock.ContextMenuStrip1.Width, Border1(found).Top + 12)
                    times += 1
                ElseIf times = 2 Then

                    frmDock.ContextMenuStrip1.Hide() 'But we'll hide it after a few seconds so it doesnt get in the way
                    For i = 0 To frmDock.ContextMenuStrip1.Items.Count - 1
                        frmDock.ContextMenuStrip1.Items(i).Visible = True 'and we'll enable the contextmenu again
                    Next i
                    times = 0
                    tmrHover.Stop()
                Else
                    times += 1
                End If
            Else
                tmrHover.Stop()
            End If
        Catch
        End Try
    End Sub
End Module
