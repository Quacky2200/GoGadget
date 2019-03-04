Imports System.Windows.Media.Animation
Imports System.Net
Imports System.IO
Imports IWshRuntimeLibrary
Imports System.Runtime.InteropServices
Imports Microsoft

Public Enum AppControlType As Short
    Application = 0
    URL = 1
    Folder = 2
    File = 3
    Picture = 4
    'Video = 5
    'Music = 6
End Enum
Public Class AppControl
#Region "Intialisation"
    Sub New(AppPath As String, i As Integer)
        InitializeComponent()
        _AppPath = AppPath
        FindType()
        Me.Margin = New Thickness(0, (Me.Height + 5) * i + 5, 0, 0)
    End Sub
    Sub New(Name As String, AppPath As String, IconPath As String, i As Integer)
        InitializeComponent()
        Me.IconPath = IconPath
        _AppPath = AppPath
        FindType()
        _AppName = Name
        Me.Margin = New Thickness(0, (Me.Height + 5) * i + 5, 0, 0)
    End Sub
    Private Sub FindType()
        Select Case Type
            Case AppControlType.Application
                AppName = AppPath.Remove(0, AppPath.LastIndexOf("\") + 1)
                AppName = AppName.Remove(AppName.LastIndexOf("."), AppName.Length - AppName.LastIndexOf("."))
                IconPath = "pack://application:,,,/GoGadget;component/images/app2.png"
                IconPath = GetIcon(AppPath)
            Case AppControlType.File
                AppName = AppPath.Remove(0, AppPath.LastIndexOf("\") + 1)
                AppName = AppName.Remove(AppName.LastIndexOf("."), AppName.Length - AppName.LastIndexOf("."))
                IconPath = "pack://application:,,,/GoGadget;component/images/file2.png"
                'get file icon?
            Case AppControlType.Picture
                AppName = AppPath.Remove(0, AppPath.LastIndexOf("\") + 1)
                AppName = AppName.Remove(AppName.LastIndexOf("."), AppName.Length - AppName.LastIndexOf("."))
                IconPath = AppPath
            Case AppControlType.Folder
                AppName = AppPath.Remove(0, AppPath.LastIndexOf("\") + 1)
                IconPath = "pack://application:,,,/GoGadget;component/images/folder2.png"
            Case AppControlType.URL
                IconPath = "pack://application:,,,/GoGadget;component/images/earth2.png"
                Dim str As String = ""
                Dim html As String = ""
                Try
                    Dim request As WebRequest = WebRequest.Create(New Uri(AppPath, UriKind.Absolute))
                    Dim Response As WebResponse = request.GetResponse
                    Dim Stream As Stream = Response.GetResponseStream
                    Dim StreamR As New StreamReader(Stream)
                    html = StreamR.ReadToEnd
                    str = html
                    str = str.Remove(0, str.IndexOf("<title"))
                    str = str.Remove(str.IndexOf("</title>"), str.Length - str.IndexOf("</title>"))
                    str = str.Remove(0, 1)
                    str = Mid(str, str.IndexOf(">") + 1).Trim().Replace("<", Nothing).Replace(">", Nothing)
                    'html = html.Replace("""", "'")
                    'Dim FaviconPattern As String = "shortcut icon"
                    'Dim Faviconstr As String = Mid(html, html.IndexOf(FaviconPattern))
                    'MsgBox(Faviconstr)
                    'Faviconstr = Mid(Faviconstr, Faviconstr.LastIndexOf("<")).Replace(FaviconPattern, Nothing)
                    'Faviconstr = Mid(Faviconstr, Faviconstr.IndexOf("""", Nothing), Faviconstr.LastIndexOf("""", Nothing))
                    'MsgBox(Faviconstr)
                    'Stop
                Catch ex As Exception
                    AppName = AppPath
                End Try
                AppName = If(str = "", New Uri(AppPath).Host, str)
                IconPath = "http://" + New Uri(AppPath).Host + "/favicon.ico"
        End Select
    End Sub
#End Region
#Region "Icon Properties"
    Private _IconPath As String
    Public Property IconPath As String
        Get
            Return _IconPath
        End Get
        Set(value As String)
            Try
                If value.ToLower.StartsWith("http://") Or value.ToLower.StartsWith("https://") Then
                    Icon = CType(New StreamedWebIconBitmap(value), BitmapBase)
                ElseIf value.Contains("pack://") Then
                    Icon = CType(New PackedBitmap(value), BitmapBase)
                Else
                    Icon = CType(New StreamedBitmap(value), BitmapBase)
                End If
                _IconPath = value
            Catch ex As Exception
                value = "pack://application:,,,/GoGadget;component/images/app2.png"
                Icon = CType(New PackedBitmap(value), BitmapBase)
                _IconPath = value
            End Try
        End Set
    End Property
    Private Property _Icon As BitmapBase
    Public Property Icon As BitmapBase
        Get
            Return _Icon
        End Get
        Set(value As BitmapBase)
            _Icon = value
            IconImageBrush.ImageSource = _Icon.ImgSource
        End Set
    End Property
    Function GetIcon(ByRef Reference As String) As String
        If Reference.ToLower.EndsWith(".exe") Then
            Return Reference
            Exit Function
        End If
        Dim w As New WshShell 'Used to create the shortcut
        Dim lnk As WshShortcut = w.CreateShortcut(Reference)
        'Create a shortcut to get the origional file path to the icon and exe
        Dim Icon1 As String
        _AppPath = If(AppPath.EndsWith(".lnk") AndAlso lnk.TargetPath.Length > 1 AndAlso Not lnk.TargetPath.ToLower.Contains("icon"), lnk.TargetPath, AppPath)
        If lnk.IconLocation.Contains(",") = True Then 'Icons are normally found with a comma so we get rid of it
            If lnk.TargetPath = "" Then
                Icon1 = Reference
            Else
                Icon1 = lnk.TargetPath
                Try
                    ' Icon1 = Icon1.Replace("%ProgramFiles%", My.Computer.FileSystem.SpecialDirectories.ProgramFiles)
                    'Icon1 = Icon1.Replace("%systemroot%", "C:\Windows\")
                Catch ex As Exception
                End Try
            End If
        Else
            Icon1 = lnk.IconLocation
            Try
                Icon1 = Icon1.Replace("%ProgramFiles%", My.Computer.FileSystem.SpecialDirectories.ProgramFiles)
                'Icon1 = Icon1.Replace("%systemroot%", "C:\Windows")
            Catch ex As Exception
            End Try

            'Icon1 = If(lnk.IconLocation.Contains("%ProgramFiles%") = True, My.Computer.FileSystem.SpecialDirectories.ProgramFiles & lnk.IconLocation.Replace("%ProgramFiles%", ""), lnk.IconLocation)
        End If
        If AppPath.Contains("C:\ProgramData\Microsoft\Windows\Start Menu\Programs") AndAlso Not lnk.TargetPath.Contains("C:\Windows\System32") Then
            Dim reference2 As String = AppPath.Replace("C:\ProgramData\Microsoft\Windows\Start Menu\Programs", "C:\Program Files")
            reference2 = Mid(reference2, 1, reference2.LastIndexOf("\"))
            Dim c As New List(Of String)
            Dim AppNameBlock() As String = AppName.Split(" ")
            For Each f In My.Computer.FileSystem.GetFiles(reference2, FileIO.SearchOption.SearchAllSubDirectories)
                If Path.GetExtension(f).ToLower = ".exe" Then
                    For Each Word In AppNameBlock
                        If Path.GetFileNameWithoutExtension(f).ToLower.Contains(Word.ToLower) Then
                            c.Add(f)
                        End If
                    Next
                End If
            Next
            If c.Count > 0 Then _AppPath = c(0)
            Main.Save()
            'AppPath = c(0)
        End If
        Return If(lnk.TargetPath = "", Reference, lnk.TargetPath)
    End Function
#End Region
#Region "General App Properties"
    Public isPinned As Boolean = True 
    Private _isRunning As Boolean = False
    Public Property isRunning As Boolean
        Get
            Return _isRunning
        End Get
        Set(value As Boolean)
            If value <> _isRunning Then
                _isRunning = value 'Set the value if the value is different (no point setting an already existing value)
            Else
                Exit Property
            End If
            If _isRunning Then
                IconHighlightColor.Color = Icon.PopularColor 'Get the most popular color for the highlight
                Dim DarkerColor As Color = Icon.PopularColor 'Get the most popular color for the darkest color (we change this below)
                DarkerColor.R -= If((DarkerColor.R - 60) < 0, DarkerColor.R, 60) 'We make each color darker if we can
                DarkerColor.G -= If((DarkerColor.G - 60) < 0, DarkerColor.G, 60)
                DarkerColor.B -= If((DarkerColor.B - 60) < 0, DarkerColor.B, 60)
                IconColor.Color = DarkerColor 'Then we set the darker color to the icon
            Else
                IconColor.Color = Color.FromArgb(102, 17, 17, 17) 'Reset the color when it's not running
                'IconHighlightColor.Color = Color.FromArgb(51, 18, 18, 18)
                IconHighlightColor.Color = Colors.Transparent
            End If

        End Set
    End Property
    Public Property AppName As String
    Private _AppPath As String
    Public Property AppPath As String
        Get
            Return _AppPath
        End Get
        Set(value As String)
            _AppPath = AppPath
        End Set
    End Property
    Public Property Animations As New List(Of AnimatorBase)
    Public ReadOnly Property ID As String
        Get
            Return _ID
        End Get
    End Property
    Private _ID As String = Guid.NewGuid.ToString
    Public ReadOnly Property Type As AppControlType
        Get
            If AppPath.ToLower.StartsWith("http://") Or AppPath.ToLower.StartsWith("https://") Then
                Return AppControlType.URL
            ElseIf AppPath.ToLower.EndsWith(".exe") Or AppPath.ToLower.EndsWith(".lnk") Then
                Return AppControlType.Application
            ElseIf IO.Directory.Exists(AppPath) Then
                Return AppControlType.Folder
            ElseIf AppPath.ToLower.EndsWith(".png") Or AppPath.ToLower.EndsWith(".jpg") Or AppPath.ToLower.EndsWith(".jpeg") Or AppPath.ToLower.EndsWith(".gif") Or AppPath.ToLower.EndsWith(".bmp") Then
                Return AppControlType.Picture
            Else
                Return AppControlType.File
            End If
            Return AppControlType.Application
        End Get
    End Property
    Public ReadOnly Property ProcessName As String
        Get
            Return Mid(AppPath.ToLower, AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing).ToLower
        End Get
    End Property

#End Region
#Region "App Events"
    Private Sub AppControl_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Animations.Add(New RotationAnimator(Me.Rotate))
        Animations.ForEach(Sub(x As AnimatorBase)
                               x.Start()
                               Dim RunOnce As New RunOnceTimer(900, Sub() Dispatcher.Invoke(Sub()
                                                                                                x.Stop()
                                                                                                Animations.Remove(x)
                                                                                            End Sub))
                           End Sub)
    End Sub
    Sub AppControl_Clicked() Handles Me.MouseLeftButtonUp
        For Each Animation As RotationAnimator In Animations
            If Animation.isRunning Then Animation.Stop()
        Next
        Try
            If isRunning And Type = AppControlType.Application Then
                Dim P As Process = Process.GetProcessesByName(Mid(AppPath, AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing)).FirstOrDefault
                ShowWindowAsync(P.MainWindowHandle, 9)
                SetForegroundWindow(P.MainWindowHandle)
            ElseIf isRunning And Type = AppControlType.URL Then
                Dim P As Process = Process.GetProcessesByName("iexplore").FirstOrDefault
                ShowWindowAsync(P.MainWindowHandle, 9)
                SetForegroundWindow(P.MainWindowHandle)
            Else
                Process.Start(AppPath)
            End If
        Catch
            If Type = AppControlType.Application Then
                Try
                    Process.Start(IconPath)
                Catch ex As Exception
                    Debug.Print("Couldn't start application")
                End Try
            End If
        End Try
    End Sub
    Sub AppControl_RightClicked(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseRightButtonUp
        Dim Delay As New RunOnceTimer(4, Sub() Dispatcher.Invoke(Sub()
                                                                     If Not IsNothing(Main.MainMenu) Then
                                                                         Main.MainMenu.Hide()
                                                                         Main.MainMenu = Nothing
                                                                     End If
                                                                     Main.MainMenu = New Menu(New Point(Main.Left, _
                                                                          Me.Margin.Top + (Me.Height / 2)), New RightClickMainMenuItems(Me), _
                                                                          MenuOrientation.FromRight)
                                                                     Main.MainMenu.Name = "RightClick"
                                                                     Main.MainMenu.Show()
                                                                 End Sub))
    End Sub
#End Region
#Region "App Commands"
    Public Sub Open()
        Try
            Process.Start(AppPath)
        Catch
        End Try
    End Sub
    Public Sub ShowWindow()
        Try
            If Type = AppControlType.Application Then
                Dim P As Process = Process.GetProcessesByName(Mid(AppPath, AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing)).FirstOrDefault
                ShowWindowAsync(P.MainWindowHandle, 9)
                SetForegroundWindow(P.MainWindowHandle)
            Else
                Dim P As Process = Process.GetProcessesByName("iexplore").FirstOrDefault
                ShowWindowAsync(P.MainWindowHandle, 9)
                SetForegroundWindow(P.MainWindowHandle)
            End If
        Catch
        End Try
    End Sub
    Public Sub Close()
        Try
            If Type = AppControlType.Application Then
                Dim P As Process = Process.GetProcessesByName(Mid(AppPath, AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing)).FirstOrDefault
                P.CloseMainWindow()
            Else
                Dim BrowserLocations As New SHDocVw.ShellWindows
                BrowserLocations.OfType(Of SHDocVw.InternetExplorer).ToArray.Where(Function(ie) ie.LocationURL = AppPath).FirstOrDefault.Quit()
            End If
        Catch
        End Try
    End Sub
    Public Sub Remove()
        Dim AppGrid As Grid = Me.Parent
        AppGrid.Children.Remove(Me)
        Main.Save()
        Main.Sort()
    End Sub
    Public Sub Kill()
        Try
            Dim P As Process = Process.GetProcessesByName(Mid(AppPath, AppPath.LastIndexOf("\") + 2).Replace(".exe", Nothing)).FirstOrDefault
            P.Kill()
        Catch
        End Try
    End Sub

#End Region
#Region "Explorer Window DLL"
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function ShowWindowAsync(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    <DllImport("user32.dll")> _
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
#End Region
End Class
'OLD...
'Public Class Folder
'    Public Name As String
'    Public Directory As String
'    Public Folders As New List(Of Folder)
'    Public Files As New List(Of File)
'    Sub New(destination As String)
'        Directory = destination
'        Name = Path.GetDirectoryName(Directory)
'        For Each FileStr In FileIO.FileSystem.GetFiles(destination)
'            Files.Add(New File(FileStr))
'        Next
'        For Each Folderstr In FileIO.FileSystem.GetDirectories(destination)
'            Folders.Add(New Folder(Folderstr))
'        Next
'    End Sub
'End Class
'Public Class File
'    Public Name As String
'    Public Type As String
'    Public Filename As String
'    Sub New(file As String)
'        Name = Path.GetFileNameWithoutExtension(file)
'        Type = Path.GetExtension(file)
'        Filename = file
'    End Sub
'End Class