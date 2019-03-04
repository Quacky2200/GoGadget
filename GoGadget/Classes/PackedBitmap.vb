Public Class PackedBitmap
    Inherits BitmapBase
    Sub New(path As String)
        Dim PathUri As New System.Uri(path, UriKind.RelativeOrAbsolute)
        Image.BeginInit()
        Image.UriSource = PathUri
        Image.CacheOption = BitmapCacheOption.OnDemand
        Image.EndInit()
        ImgSource = DirectCast(Image, ImageSource)
    End Sub
End Class
