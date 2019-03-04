Public Class InformationWindow
    Public Sub New(ByVal App As AppControl)
        InitializeComponent()
        SetValues(App)
    End Sub

    Sub SetValues(ByVal App As AppControl)
        Dim DarkerColor As Color = App.Icon.PopularColor 'Get the most popular color for the darkest color (we change this below)
        DarkerColor.R -= If((DarkerColor.R - 60) < 0, DarkerColor.R, 60) 'We make each color darker if we can
        DarkerColor.G -= If((DarkerColor.G - 60) < 0, DarkerColor.G, 60)
        DarkerColor.B -= If((DarkerColor.B - 60) < 0, DarkerColor.B, 60)
        IconDark.Color = App.Icon.PopularColor 'Get the most popular color for the highlight
        IconLight.Color = DarkerColor 'Then we set the darker color to the icon 
        IconName.Content = App.AppName
        IconLocation.Content = "Location: " & App.AppPath
        IconPreview.Source = App.IconImageBrush.ImageSource 'App.Icon.ImgSource
        IconType.Content = "Type: " & App.Type.ToString
        IconSpecial.Content = If(App.Type = AppControlType.Application, "Version: " & FileVersionInfo.GetVersionInfo(App.AppPath).FileVersion & "     Creator: " & FileVersionInfo.GetVersionInfo(App.AppPath).CompanyName, If(App.Type = AppControlType.URL, "URL: " & App.AppPath, "Last Modified: " & FileIO.FileSystem.GetFileInfo(App.AppPath).LastWriteTime))
        Me.Left = (System.Windows.SystemParameters.PrimaryScreenWidth / 2) - (Me.Width / 2)
        Me.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - (Me.Height / 2)
    End Sub
    Private Sub Me_Drag() Handles Me.MouseDown
        Me.Close()
    End Sub
End Class
