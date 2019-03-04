Public Class frmDockAdd
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Sub AddWeblink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PictureBox1.ImageLocation = "Norm.png"
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not TextBox1.Text = "" Then
            Select Case True
                Case TextBox2.Text.Contains("http://"), TextBox2.Text.Contains(".com"), TextBox2.Text.Contains(".co.uk"), TextBox2.Text.Contains(".net"), TextBox2.Text.Contains(".org"), TextBox2.Text.Contains(".biz"), TextBox2.Text.Contains(".tv"), TextBox2.Text.Contains(".co"), TextBox2.Text.Contains(".info"), TextBox2.Text.Contains(".me"), TextBox2.Text.Contains(".mobi"), TextBox2.Text.Contains(".de"), TextBox2.Text.Contains(".eu"), TextBox2.Text.Contains(".es"), TextBox2.Text.Contains(".fr"), TextBox2.Text.Contains("fm"), TextBox2.Text.Contains(".it"), TextBox2.Text.Contains(".org.uk"), TextBox2.Text.Contains(".tk")
                        Try
                            Try
                                My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.CurrentDirectory + "\Img")
                            Catch
                                Debug.Print("already a folder")
                            End Try
                            Dim wc As New System.Net.WebClient
                            AddHandler wc.DownloadFileCompleted, AddressOf Complete
                            Try
                                wc.DownloadFileAsync(New Uri(TextBox2.Text + "/favicon.ico"), My.Computer.FileSystem.CurrentDirectory + "\Img\" + TextBox1.Text + ".ico")
                            Catch ex As Exception
                                MsgBox("Error! " + ex.Message)
                            End Try
                        Catch ex As Exception
                            My.Settings.picturelink.Add("norm.png")
                            MsgBox("Could not automatically retrieve Favicon! " + ex.Message)
                        End Try
                Case Else
                    ApplyApp(My.Settings.name.Count, TextBox1.Text, TextBox2.Text, PictureBox1.ImageLocation, True)
                    Me.Close()
                    frmDockBackground.tmrDockHide.Start()
            End Select
        Else
            MsgBox("Must have a name", MsgBoxStyle.Information, "Name is invalid")
        End If
    End Sub
    Sub Complete()
        Try
            PictureBox1.Image = System.Drawing.Image.FromFile(My.Computer.FileSystem.CurrentDirectory + "\Img\" + TextBox1.Text + ".ico")
        Catch
        End Try
        ApplyApp(My.Settings.name.Count, TextBox1.Text, TextBox2.Text, My.Computer.FileSystem.CurrentDirectory + "\Img\" + TextBox1.Text + ".ico", True)
        Me.Close()
        frmDockBackground.tmrDockHide.Start()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        OpenFileDialog1.ShowDialog()
        OpenFileDialog1.Title = "Select Picture"
        PictureBox1.Image = System.Drawing.Image.FromFile(OpenFileDialog1.FileName)
    End Sub
End Class