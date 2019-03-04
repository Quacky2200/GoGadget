Public NotInheritable Class PopularColour
    Private _color As Color
    Public Sub New(Image As BitmapImage)
        Dim AllContainedPixelColors As New Dictionary(Of System.Drawing.Color, Integer)
        Dim stride = Image.PixelWidth * 4 'Make the strides to get each individual pixel
        Dim size = Image.PixelHeight * stride 'Make the size for the array
        Dim parray(size) As Byte 'Make temp color array to store each color pixel seperately
        Image.CopyPixels(parray, stride, 0) 'Copy the pixels to the array
        For x = 0 To Image.PixelWidth - 4 'Go through each pixel in width
            For y = 0 To Image.PixelHeight - 4 'go through the y pixels
                Dim i As Integer = y * stride + 4 * x 'Get the necessary pixel
                Dim c As System.Drawing.Color = System.Drawing.Color.FromArgb(parray(i + 3), parray(i + 2), parray(i + 1), parray(i))
                If Not AllContainedPixelColors.ContainsKey(c) AndAlso c.GetSaturation > 0.35 Then
                    AllContainedPixelColors.Add(c, 1)
                ElseIf AllContainedPixelColors.ContainsKey(c) Then
                    AllContainedPixelColors.Item(c) += 1
                End If
            Next y
        Next x
        Dim ColorFound As System.Drawing.Color = AllContainedPixelColors.Where( _
            Function(x) AllContainedPixelColors.Values.Max < 4 AndAlso x.Key.GetHue > 0.6 AndAlso x.Key.A > 200 _
                Or x.Value = AllContainedPixelColors.Values.Max).FirstOrDefault.Key
        If ColorFound.A = 0 Then
            _color = Colors.WhiteSmoke
        Else
            _color = Color.FromArgb(ColorFound.A, ColorFound.R, ColorFound.G, ColorFound.B)
        End If
    End Sub
    Public ReadOnly Property Color As Color
        Get
            Return _color
        End Get
    End Property

End Class
Public MustInherit Class BitmapBase
#Region "Properties"
    Private Property _Image As BitmapImage
    Public Property Image() As BitmapImage
        Get
            If IsNothing(_Image) Then
                _Image = New BitmapImage
            End If
            Return _Image
        End Get
        Set(value As BitmapImage)
            value = _Image
        End Set
    End Property
    Public Property ImgSource As ImageSource
    Private Property _AllContainedPixelColors As New Dictionary(Of System.Drawing.Color, Integer)
    Public ReadOnly Property PopularColor As Color
        Get
            Return (New PopularColour(Image)).Color
        End Get
    End Property
#End Region
    Public Function ToBitmapImage(img As System.Drawing.Image) As BitmapImage
        Dim ms As New IO.MemoryStream
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Image.BeginInit()
        Image.StreamSource = ms
        Image.EndInit()
        Return Image
    End Function
End Class
