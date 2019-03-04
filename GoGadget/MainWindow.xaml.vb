Imports System.Windows.Media.Animation
Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports System.Threading
Public Class Dock
    Public Enum Orientation As Short
        Right = 0
        Left = 1
        Top = 2
        Bottom = 3
    End Enum
    Public Enum ShowApps As Short
        All = 0
        Pinned = 1
        Running = 2
        Pinned_Running = 3
        Files = 4
        URLs = 5
    End Enum
End Class
Class MainWindow
    'NEW TODO August 2014:
    'TODO: Prevent window overlapping
    'TODO: Handle Windows Explorer windows (use pinvoke getwindows)
    'TODO: Show AppName hover menu
    'TODO: Play Music - detect if music is playing?
    'TODO: Multiple windows
    'TODO: Launchpad/Start Menu type application
    'TODO: Extensions? 
    'TODO: Multiple dock positions
    'TODO: Virtual Folder/Folder
    'TODO: Automatic updater? Too late perhaps?
    'TODO: Multimedia playback w/ videos?
    'TODO: Hide window from Alt-Tab list
    'TODO: Voice Control
    'HELP:
    '-> TASK means what the code below should do, it's not a TODO list
    '-> TODO means code/ideas that need/want to be done
    'TODO Nov 2014:
    'Enhance code, get rid of unwanted code...



    'NEW TODO November 2014:
    'Build dock class
    'Wallpaper colour/static blur
    'Dock hide/Window dodge?
    'Certain apps only
    'Disable animations
#Region "Properties"
    Private DockSlide As New SlideAnimator(Me)
    Private WithEvents DockDetector As New Timers.Timer(2300)
    Public MainMenu As New Menu(New Point(0, 0), New MainMenuItems, MenuOrientation.FromRight)
    Public Ready As Boolean = False
    Public ReadOnly Property Apps As AppControl()
        Get
            Return DockContent.Children.OfType(Of AppControl)().ToArray()
        End Get
    End Property
    Function ReturnCurrentWindowProcesses() As Process()
        Return Process.GetProcesses().Where(Function(x) x.MainWindowHandle <> 0).ToArray()
    End Function
#End Region
#Region "Startup"
    Private Sub Dock_Closing() Handles Dock.Closing
        DockColorThread.Abort()
        IconStatusThread.Abort()
    End Sub
    Private Sub Dock_LayoutUpdated(sender As Object, e As EventArgs) Handles Dock.LayoutUpdated
        If Me.Width <= 0 And Not DockDetector.Enabled Then
            DockDetector.Start()
        End If
    End Sub
    Private Sub Dock_Load() Handles Dock.Loaded
        Debug.Print("Dock Initialising...")
        Align()
        DockColorThread.Start()
        IconStatusThread.Start()
    End Sub
    Sub Align()
        'TODO: Multiple Screen Locations
        'TODO: Multiple Dock Positions
        Me.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Me.Width
        Me.Top = 0
        Me.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    End Sub
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Open()
    End Sub
#End Region
#Region "Information Window"
    Public WithEvents Info As InformationWindow
    Private Sub Info_close() Handles Info.Closing
        Info = Nothing
    End Sub
#End Region
#Region "Dock Hide Behaviour"
    Private Sub Dock_DetectToHide(Sender As Timers.Timer, e As System.EventArgs) Handles DockDetector.Elapsed
        Dispatcher.Invoke( _
                   Sub()
                       Try
                           If My.Settings.Dock_Hide And DockSlide.SlideMovement > 0 And Apps.Where(Function(x) x.Animations.Where(Function(ex) ex.isRunning).Count > 0).Count < 1 And Not MainMenu.IsVisible And Not isDragging Then
                               'If there are no app animations and the mainmenu is not visible, andddd.... we're not dragging, then check the mouse
                               Align()
                               'Let's align our dock first
                               Dim R As New Rect(Me.Left, Me.Top, Me.Width, Me.Height)
                               Dim Mouse As New Rect(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 5, 5)
                               'By making our two Rectanges, we can detect if the mouse is intercepting the main rectangle R
                               If Mouse.IntersectsWith(R) Then
                                   Sender.Stop()
                                   'If it does then stop the detector, we don't need to run it
                               Else
                                   DockSlide.Toggle() 'Start the hiding process
                                   Sender.Interval = 10 'Do it immediately
                                   Sort() 'sort the items while we're at it
                               End If
                           ElseIf DockSlide.SlideMovement <= 0 Then
                               'if hidden and the mouse goes to the far right of the screen and...if the dockslide animation is not running then we reset the timer interval, stop the timer, align the dock and start the animation
                               If Not My.Settings.Dock_Hide Or System.Windows.Forms.Cursor.Position.X >= System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 1 And Not DockSlide.isRunning Then
                                   Align()
                                   Sender.Interval = 2300
                                   DockSlide.Toggle()
                                   Sender.Stop()
                               End If
                           End If
                       Catch ex As Exception
                           Debug.Print("Dock (Hide Behaviour) Failed." & ex.Message)
                       End Try
                   End Sub)
    End Sub
    Private Sub Dock_FocusOut() Handles Dock.MouseLeave, Dock.LostFocus
        If Not DockDetector.Enabled And My.Settings.Dock_Hide Then DockDetector.Start()
    End Sub
#End Region
#Region "Dock Colour Behaviour"
    Public DockColorThread As New Thread(AddressOf WallpaperCheck_Thread)
    Private Sub WallpaperCheck_Thread()
        Dispatcher.Invoke( _
            Sub()
                If Not My.Settings.Wallpaper_Color And Not Ready Then
                    SetDockColor(Colors.Black)
                ElseIf My.Settings.Wallpaper_Color Then
                    Check_Wallpaper()
                End If
            End Sub)
        Thread.Sleep(1000)
        WallpaperCheck_Thread()
    End Sub
    Private Sub Check_Wallpaper()
        Dim Wallpaper_Location = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False).GetValue("Wallpaper").ToString
        Dim Wallpaper_LastWrite = (New IO.FileInfo(Wallpaper_Location)).LastWriteTimeUtc
        If Wallpaper_LastWrite > My.Settings.Wallpaper_LastMod Then
            SetDockColor(Picture_Colour(Wallpaper_Location))
            My.Settings.Wallpaper_LastMod = Wallpaper_LastWrite
            My.Settings.Save()
        End If
    End Sub
    Private Function Picture_Colour(file As String) As Color
        Dim StrmBitmap As StreamedBitmap
        Dim ms As New IO.MemoryStream
        Using FileStream As New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.Read)
            Dim OrigionalImage As System.Drawing.Image = System.Drawing.Image.FromStream(FileStream)
            Dim CreateCanvas As New System.Drawing.Bitmap(60, 60)
            Dim TheGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(CreateCanvas)
            TheGraphics.DrawImage(OrigionalImage, 0, 0, 60, 60)
            OrigionalImage = CreateCanvas
            OrigionalImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            StrmBitmap = New StreamedBitmap(ms)
        End Using
        Return (New PopularColour(StrmBitmap.Image)).Color
    End Function
    Public Sub SetDockColor(NewColor As Color)
        If NewColor = CType(DockBorder.Background, SolidColorBrush).Color Then Exit Sub
        NewColor.A = My.Settings.Dock_Transparency
        Dim ColorStoryboard As Storyboard = Me.FindResource("DockColorAnimation")
        Dim ColorSet As ColorAnimation = ColorStoryboard.Children.Item(0)
        If DockBorder.Opacity = 0 Then 'On Start-up, show the dock
            Dim ShadowAnimation As New DoubleAnimation(1, ColorSet.Duration)
            Storyboard.SetTarget(ShadowAnimation, DockBorder)
            Storyboard.SetTargetProperty(ShadowAnimation, New PropertyPath(Border.OpacityProperty))
            ColorStoryboard.Children.Add(ShadowAnimation)
            Ready = True
            If My.Settings.Dock_Hide Then DockDetector.Start() : DockDetector.Interval = 150
        End If
        ColorSet.From = CType(CType(DockBorder.Background, Brush), SolidColorBrush).Color
        ColorSet.To = NewColor
        Storyboard.SetTarget(ColorStoryboard, DockBorder)
        ColorStoryboard.Begin()
    End Sub
    Public Sub NewDockColourThread()
        DockColorThread = New Thread(AddressOf WallpaperCheck_Thread)
        DockColorThread.start()
    End Sub
#End Region
#Region "Add Running (non pinned) apps"

#End Region
#Region "Icon Status Behaviour"
    Private IconStatusThread As New Thread(AddressOf UpdateIconStatuses)
    Private Sub UpdateIconStatuses()
        Dispatcher.Invoke(Sub()
                              If Ready Then
                                  'TASK: Light up all the current processes that have MainWindow handles and keep track of the ones that were turned on to prevent them being switched off
                                  Dim SkipTorchOutIDList As New List(Of String)
                                  For Each Proc As Process In ReturnCurrentWindowProcesses()
                                      Dim Result As AppControl() = Apps.Where(Function(x) x.Type = AppControlType.Application AndAlso Mid(x.AppPath.ToLower, x.AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing).ToLower = Proc.ProcessName.ToLower).ToArray
                                      If Result.Length > 0 Then
                                          Result.ToList.ForEach(Sub(App As AppControl)
                                                                    SkipTorchOutIDList.Add(App.ID)
                                                                    App.isRunning = True
                                                                End Sub)
                                      ElseIf Proc.ProcessName <> "explorer" And Proc.ProcessName <> "dwm" Then
                                          Try
                                              Debug.Print("added " & Proc.ProcessName)
                                              Dim TempApp As New AppControl(Proc.MainModule.FileName, DockContent.Children.Count) With {.isPinned = False}
                                              DockContent.Children.Add(TempApp)
                                              SkipTorchOutIDList.Add(TempApp.ID)
                                              Sort()
                                          Catch ex As Exception

                                          End Try
                                      End If
                                  Next
                                  Dim BrowserLocations As New SHDocVw.ShellWindows
                                  'TASK: Light up all the URL's that are currently open in Internet Explorer and keep track of the ones that were turned on to prevent them being switched off
                                  For Each App In Apps.Where(Function(x) x.Type = AppControlType.URL AndAlso x.isPinned AndAlso BrowserLocations.OfType(Of SHDocVw.InternetExplorer).ToArray.Where(Function(ie) ie.LocationURL = x.AppPath).Count <> 0)
                                      SkipTorchOutIDList.Add(App.ID)
                                      App.isRunning = True
                                  Next
                                  'TASK: Extinguish the applications and URLS that aren't running and delete those that are weren't added and are not running
                                  For Each App As AppControl In Apps.Where(Function(x) Not SkipTorchOutIDList.Contains(x.ID))
                                      App.isRunning = False
                                      If Not App.isPinned Then App.Remove()
                                  Next
                                  SkipTorchOutIDList.Clear()
                              End If
                          End Sub)
        Thread.Sleep(1000)
        UpdateIconStatuses()

        'Apps.Where(Function(x) x.Type = AppControlType.Application Or x.Type = AppControlType.URL).ToList.ForEach( _
        '    Sub(App As AppControl)
        '        'Go through each app if the app is an application or a URL
        '        Dim BrowserLocations As New SHDocVw.ShellWindows
        '        If App.Type = AppControlType.Application Then
        '            Dim Proc As Process() = Process.GetProcessesByName(Mid(App.AppPath, App.AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing))
        '            If Proc.Where(Function(Windows) Windows.MainWindowHandle <> 0).ToArray.Length > 0 Then
        '                'If the app has a window and the process is running
        '                If Not App.isRunning Then App.isRunning = True
        '            End If
        '        ElseIf App.Type = AppControlType.URL AndAlso BrowserLocations.OfType(Of SHDocVw.InternetExplorer).ToArray.Where(Function(ie) ie.LocationURL = App.AppPath).Count <> 0 Then
        '            'If the app is a URL and if the URL is open in Internet Explorer (count <> 1) then it's running
        '            If Not App.isRunning Then App.isRunning = True
        '        Else
        '            App.isRunning = False
        '        End If
        '    End Sub)
        'Process.GetProcesses.Where(Function(x) x.MainWindowHandle <> 0).ToList.ForEach( _
        '    Sub(Proc As Process)
        '        Debug.Print("Apparently " & Proc.ProcessName & " is not added")
        '        '    Dim m As MsgBoxResult = MsgBox("Do you want to add TEMP: " & Proc.ProcessName, MsgBoxStyle.YesNo, "*COUGH*")
        '        '    If m = MsgBoxResult.Yes Then DockContent.Children.Add(New AppControl(Proc.MainModule.FileName, DockContent.Children.Count))
        '    End Sub)
        ''TODO: Add apps that are not in the App List
    End Sub
#End Region
#Region "Icon Self-Sort"
    Public Sub Sort()
        Dim SB As New Storyboard
        For i = 0 To Apps.Count - 1
            If Apps(i).Margin.Top <> Apps(i).Height * i Then
                Dim DBAnim As New ThicknessAnimation(Apps(i).Margin, New Thickness(0, Apps(i).Height * i, 0, 0), TimeSpan.FromSeconds(0.35))
                Storyboard.SetTargetProperty(DBAnim, New PropertyPath(FrameworkElement.MarginProperty))
                Storyboard.SetTarget(DBAnim, Apps(i))
                SB.Children.Add(DBAnim)
            End If
        Next
        SB.Begin()
    End Sub
#End Region
#Region "Drag 'n' Drop"
    Private Dummy As New UIElement
    Private UIDragSource As UIElement
    Private isMouseDown As Boolean = False
    Private isDragging As Boolean = False
    Private _StartPoint As Point
    Private Sub DockContent_DragOver(sender As Object, e As System.Windows.DragEventArgs) Handles DockContent.DragEnter
        e.Effects = Windows.DragDropEffects.All
        If e.Data.GetDataPresent("UIElement") Then
            e.Effects = Windows.DragDropEffects.Move
        End If
    End Sub
    Private Sub DockContent_DragOnto(sender As Object, e As System.Windows.DragEventArgs) Handles DockContent.Drop, DockBorder.Drop
        Dim example = e.Data.GetData(DataFormats.Text)
        If Not IsNothing(example) Then
            DockContent.Children.Add(New AppControl(example, DockContent.Children.Count)) 'HTML link
        ElseIf e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each Str As String In e.Data.GetData(DataFormats.FileDrop)
                DockContent.Children.Add(New AppControl(Str, DockContent.Children.Count)) 'Normal File
            Next
        ElseIf e.Data.GetDataPresent("UIElement") AndAlso isDragging AndAlso isMouseDown Then
            Dim droptarget As UIElement = TryCast(e.Source, UIElement)
            Dim droptargetIndex As Integer = -1, i As Integer = 0
            For Each element As UIElement In Apps()
                If element.Equals(droptarget) Then
                    droptargetIndex = i
                    Exit For
                End If
                i += 1
            Next
            Try
                If droptargetIndex <> -1 Then
                    DockContent.Children.Remove(UIDragSource)
                    DockContent.Children.Insert(droptargetIndex, UIDragSource)
                End If
            Catch
            End Try
            isMouseDown = False
            isDragging = False
            UIDragSource.ReleaseMouseCapture()
        End If
        Save()
        Sort()
    End Sub
    Private Sub DockContent_DragEnter(sender As Object, e As Windows.DragEventArgs) Handles DockContent.DragEnter, DockBorder.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effects = Windows.DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent("UIElement") Then
            e.Effects = DragDropEffects.Move
        End If
    End Sub
    Private Sub DockContent_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles DockContent.PreviewMouseLeftButtonDown
        If e.Source IsNot DockContent Then
            isMouseDown = True
            _StartPoint = e.GetPosition(DockContent)
        End If
    End Sub
    Private Sub DockContent_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles DockContent.PreviewMouseLeftButtonUp
        isMouseDown = False
        isDragging = False
        Try : UIDragSource.ReleaseMouseCapture() : Catch : End Try
    End Sub
    Private Sub DockContent_PreviewMouseMove(sender As Object, e As Input.MouseEventArgs) Handles DockContent.PreviewMouseMove
        If isMouseDown Then
            If (isDragging = False) AndAlso ((Math.Abs(e.GetPosition(DockContent).X - _StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance) OrElse (Math.Abs(e.GetPosition(DockContent).Y - _StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)) Then
                isDragging = True
                UIDragSource = TryCast(e.Source, UIElement)
                UIDragSource.CaptureMouse()
                DragDrop.DoDragDrop(Dummy, New DataObject("UIElement", e.Source), DragDropEffects.Move)
            End If
        End If
    End Sub
#End Region
#Region "Main Menu Behaviour"
    Private Sub DockBorder_RightClick(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles DockBorder.MouseRightButtonUp
        If Not IsNothing(MainMenu) Then
            MainMenu.Hide()
            MainMenu = Nothing
        End If
        MainMenu = New Menu(New Point(Me.Left, System.Windows.Forms.Cursor.Position.Y), New MainMenuItems, MenuOrientation.FromRight)
        MainMenu.Name = "MainMenu"
        MainMenu.Show()
    End Sub
#End Region
    '#Region "Aero"
    '    <System.Runtime.InteropServices.DllImport("dwmapi.dll", EntryPoint:="#127", PreserveSig:=False)> Public Shared Sub DwmGetColorizationParameters(ByRef parameters As WDM_COLORIZATION_PARAMS)
    '    End Sub
    '    <System.Runtime.InteropServices.DllImport("dwmapi.dll", EntryPoint:="#131", PreserveSig:=False)> Public Shared Sub DwmSetColorizationParameters(ByRef parameters As WDM_COLORIZATION_PARAMS, Optional ByVal uUnknown As UInteger = True)
    '    End Sub

    '    Public Structure WDM_COLORIZATION_PARAMS
    '        Public Color1 As UInteger
    '        Public Color2 As UInteger 'Disabled because unsure of what it does and it doesn't seem to work??? :S
    '        Public Intensity As Integer
    '        Public Brightness As UInteger
    '        Public Transparency As UInteger
    '        Public GlassRelection As UInteger
    '        Public Transparency_Disabled As Boolean 'Set to true for no transparency, like windows 8? LOL
    '    End Structure
    '#End Region
#Region "Open & Save Functionality"
    Public Sub Save()
        Try
            Debug.Print("Saving to..." & My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml @" & (New StackTrace).GetFrame(1).GetMethod.ToString)
            Dim Writer As New System.IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml")
            Writer.WriteLine("<Dock>" & Environment.NewLine)
            Apps.Where(Function(x) x.isPinned).ToList().ForEach( _
                Sub(x As AppControl)
                    Writer.Write("<App title='" & x.AppName & "'>" & Environment.NewLine & _
                                 "<image>" & x.IconPath & "</image>" & Environment.NewLine & _
                                 "<path>" & x.AppPath.ToString & "</path>" & Environment.NewLine & _
                                 "</App>" & Environment.NewLine)
                End Sub)
            Writer.WriteLine("</Dock>")
            Writer.Close()
        Catch ex As Exception
            Debug.Print("Cannot save apps!")
        End Try
    End Sub
    Private Sub Open()
        Dim TemporaryXMLList As New List(Of String)
        Dim DockXMLReader As New System.Xml.XmlTextReader(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml")
        Try
            Do While (DockXMLReader.Read())
                Select Case DockXMLReader.NodeType
                    Case System.Xml.XmlNodeType.Element 'Display beginning of element.
                        If DockXMLReader.HasAttributes Then 'If attributes exist
                            While DockXMLReader.MoveToNextAttribute()
                                'Display attribute name and value.
                                TemporaryXMLList.Add(DockXMLReader.Value)
                                'App Name
                            End While
                        End If
                    Case System.Xml.XmlNodeType.Text 'Display the text in each element.
                        TemporaryXMLList.Add(DockXMLReader.Value)
                End Select
            Loop
        Catch ex As IO.FileNotFoundException
            If IO.Directory.Exists("C:\Users\" & Environment.UserName & "\AppData\Roaming\GoGadget_\GoGadget!\1.0.0.0\") Then
                Debug.Print("Found old GoGadget_ XML File")
                My.Computer.FileSystem.CopyFile("C:\Users\" & Environment.UserName & "\AppData\Roaming\GoGadget_\GoGadget!\1.0.0.0\Dock_Elements.xml", My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml")
                Debug.Print("Re-opening old apps from GoGadget 1 /or 2")
                Open()
            End If
            Debug.Print("Open function returns: no XML file found.")
        Catch ex As Exception
            For i = 0 To TemporaryXMLList.Count - 1 Step 3
                DockContent.Children.Add(New AppControl(TemporaryXMLList(i), TemporaryXMLList(i + 2), TemporaryXMLList(i + 1), 0))
                ' Debug.Print(TemporaryXMLList(i) & " " & TemporaryXMLList(i + 2) & " " & TemporaryXMLList(i + 1))
            Next
            Sort()
            Save()
            MsgBox("Sorry, some of your objects could not be recovered from an XML failure. However, I have corrected myself and have saved up until the point of failure")
        End Try
        DockXMLReader.Close()
        DockContent.Children.Clear()
        Try
            For i = 0 To TemporaryXMLList.Count - 1 Step 3
                DockContent.Children.Add(New AppControl(TemporaryXMLList(i), TemporaryXMLList(i + 2), TemporaryXMLList(i + 1), 0))
                'Debug.Print(TemporaryXMLList(i) & " " & TemporaryXMLList(i + 2) & " " & TemporaryXMLList(i + 1))
            Next
            Sort()
        Catch
            Debug.Print("Cannot open apps, delete XML (" & My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Dock_Elements.xml)")
        End Try
    End Sub
#End Region
#Region "Voice Recognition"
    ' Private VoiceReg As New Recognitioneng("Jarvis", New test2)
#End Region

End Class


