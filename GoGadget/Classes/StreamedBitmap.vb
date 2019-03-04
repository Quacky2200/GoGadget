Imports System.Windows.Interop

Public Class StreamedBitmap
    Inherits BitmapBase
    Sub New(Path As String)
        Dim fileinf As New IO.FileInfo(Path)
        Dim bool = fileinf.Exists
        If IO.File.Exists(Path) And Not Path.ToLower.EndsWith(".exe") Then
            If Path.ToLower.EndsWith(".ico") Then
                Dim StrmBitmap As StreamedBitmap
                Dim ms As New IO.MemoryStream
                Using FileStream As New IO.FileStream(Path, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim OrigionalImage As System.Drawing.Image = System.Drawing.Image.FromStream(FileStream)
                    Dim CreateCanvas As New System.Drawing.Bitmap(42, 42)
                    Dim TheGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(CreateCanvas)
                    TheGraphics.DrawImage(OrigionalImage, 0, 0, 42, 42)
                    OrigionalImage = CreateCanvas
                    OrigionalImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    StrmBitmap = New StreamedBitmap(ms)
                    Me.Image = StrmBitmap.Image
                    Me.ImgSource = StrmBitmap.ImgSource
                    Exit Sub
                End Using
            End If
            Me.Image.BeginInit()
            Me.Image.StreamSource = New IO.FileStream(Path, IO.FileMode.Open, IO.FileAccess.Read)
            Me.Image.CacheOption = BitmapCacheOption.OnLoad
            Me.Image.EndInit()
            Me.Image.StreamSource.Close()
            ImgSource = DirectCast(Me.Image, ImageSource)
        ElseIf Path.ToLower.Contains(".exe") Then
            Me.Image = RetrieveIcon(Path)
            ImgSource = DirectCast(Me.Image, ImageSource)
        End If
    End Sub
    Sub New(FileStr As IO.MemoryStream)
        Me.Image.BeginInit()
        Me.Image.StreamSource = FileStr
        Me.Image.CacheOption = BitmapCacheOption.OnLoad
        Me.Image.EndInit()
        Me.Image.StreamSource.Close()
        ImgSource = DirectCast(Me.Image, ImageSource)
    End Sub
    <System.Runtime.InteropServices.DllImport("shell32.dll")> Shared Function _
    ExtractAssociatedIcon(ByVal hInst As IntPtr, ByVal lpIconPath As String, _
                           ByRef lpiIcon As Integer) As IntPtr
    End Function
    Function RetrieveIcon(ByRef str As String) As BitmapImage
        Dim hIcon As IntPtr
        hIcon = ExtractAssociatedIcon(New WindowInteropHelper(Application.Current.MainWindow).Handle, str, 0)
        Return ToBitmapImage(System.Drawing.Icon.FromHandle(hIcon).ToBitmap)
        'System.Drawing.Icon.ExtractAssociatedIcon(str).ToBitmap()
    End Function
    Function ResizeBitmap(img As String) As StreamedBitmap
        Dim StrmBitmap As StreamedBitmap
        Dim ms As New IO.MemoryStream
        Using FileStream As New IO.FileStream(img, IO.FileMode.Open, IO.FileAccess.Read)
            Dim OrigionalImage As System.Drawing.Image = System.Drawing.Image.FromStream(FileStream)
            Dim CreateCanvas As New System.Drawing.Bitmap(100, 100)
            Dim TheGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(CreateCanvas)
            TheGraphics.DrawImage(OrigionalImage, 0, 0, 100, 100)
            OrigionalImage = CreateCanvas
            OrigionalImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            StrmBitmap = New StreamedBitmap(ms)
        End Using
        Return StrmBitmap
    End Function
End Class
