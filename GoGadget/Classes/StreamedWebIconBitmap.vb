Imports System.Net
Imports System.IO

Public Class StreamedWebIconBitmap
    Inherits BitmapBase

    Private Function GetSafePathName(URL As String) As String
        Return URL.Replace("\", Nothing) _
            .Replace("/", Nothing) _
            .Replace(":", Nothing) _
            .Replace("|", Nothing) _
            .Replace("*", Nothing) _
            .Replace("?", Nothing) _
            .Replace("""", Nothing) _
            .Replace(">", Nothing) _
            .Replace("<", Nothing)
    End Function

    Sub New(path As String)
        Dim request As WebRequest = WebRequest.Create(New Uri(path, UriKind.Absolute))
        request.Timeout = 500
        request.Credentials = CredentialCache.DefaultNetworkCredentials
        Dim Response As WebResponse = request.GetResponse
        If Not IsNothing(Response) Then
            'Let's cache the image.
            'ONLY Download if lastModified date is different.
            Dim CachePath As String = IO.Path.Combine({My.Computer.FileSystem.SpecialDirectories.Temp, "GoGadgetCache"})
            Dim SafeFilename As String = GetSafePathName(path)
            Dim FullPathName As String = CachePath & "\" & SafeFilename
            If Not IO.Directory.Exists(CachePath) Then IO.Directory.CreateDirectory(CachePath)
            Dim doDownload As Boolean = False
            Dim RemoteModifiedDate As DateTime = New DateTime()
            Dim FI As New IO.FileInfo(FullPathName)
            If IO.File.Exists(FullPathName) Then
                Dim LocalModifiedDate As DateTime = FI.CreationTime
                Dim DateText As String = Response.Headers("Last-Modified")
                DateTime.TryParse(DateText, RemoteModifiedDate)
                If LocalModifiedDate < RemoteModifiedDate Then
                    doDownload = True
                End If
            Else
                doDownload = True
            End If
            If doDownload Then
                Dim Stream As IO.Stream = Response.GetResponseStream()
                Dim BinaryReader As New BinaryReader(Stream)
                Dim ByteBufferSize As Integer = 4096
                Dim Bytes() As Byte = New Byte(ByteBufferSize) {}
                Dim ReadBytes As Integer = 1

                Using FS As New FileStream(FullPathName, FileMode.Create, FileAccess.Write)
                    Do While ReadBytes > 0
                        ReadBytes = BinaryReader.Read(Bytes, 0, ByteBufferSize - 1)
                        FS.Write(Bytes, 0, ReadBytes)
                    Loop
                End Using
                BinaryReader.Close()

                'Update Date
                'FI.CreationTime = RemoteModifiedDate
            End If
            Image.BeginInit()
            Image.UriSource = New Uri(FullPathName, UriKind.RelativeOrAbsolute)
            'Setting it to none will allow the application to use 0 ram
            Image.CacheOption = BitmapCacheOption.None
            Image.EndInit()
            ImgSource = DirectCast(Image, ImageSource)
        End If
    End Sub
End Class
