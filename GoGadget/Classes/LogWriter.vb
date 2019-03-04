Public Class DebugWriter
    Private WithEvents Writer As IO.StreamWriter
    Sub New(folder As String)
        If Not IO.Directory.Exists(folder) Then
            Try
                IO.Directory.CreateDirectory(folder)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End If
        Writer = New IO.StreamWriter(folder & If(folder.EndsWith("\"), Nothing, "\") & "Debug.txt")
    End Sub
    Private Function Return_Date_Time_Str()
        Return Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
    End Function
    Public Sub Print(str As String)
        Writer.WriteLine(Return_Date_Time_Str() & " " & str & "@" & (New StackTrace).GetFrame(1).GetMethod.ToString)
    End Sub
    Public Sub [Stop]()
        Writer.Close()
    End Sub

End Class
