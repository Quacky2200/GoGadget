Imports System.Security.Permissions

Public Class frmDockTools
    Dim color1, color2, color3 As Integer
    Dim comparison As Integer
    Dim usewindowtheming As Boolean
    Dim opacity1 As Double
    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        SetColour()
    End Sub
    Sub SetColour()
        Dim colour As Color = Color.FromArgb(TrackBar3.Value, TrackBar2.Value, TrackBar4.Value, TrackBar6.Value)
        My.Settings.Colour = colour
        My.Settings.Save()
        For i = 0 To frmDock.Database(1, 1, 1).count - 1
            frmDock.Database(1, 1, 1)(i).BackColor = If(My.Settings.Comparison = 50, ControlPaint.Dark(My.Settings.Colour, 0.15), ControlPaint.LightLight(My.Settings.Colour))
        Next
        frmDockBackground.Background()
    End Sub
    Private Sub TrackBar4_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar4.Scroll
        SetColour()
    End Sub
    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        SetColour()
    End Sub
    Private Sub TrackBar6_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar6.Scroll
        SetColour()
    End Sub
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Settings.UseWindowsTheme = CheckBox1.Checked
        My.Settings.opacity = TrackBar1.Value / 100
        If CheckBox2.Checked = False And CheckBox3.Checked = False Then
            CheckBox2.Checked = True
        End If
        If CheckBox2.Checked = True Then
            My.Settings.Comparison = 50
        Else
            My.Settings.Comparison = 97
        End If
        My.Settings.Save()
        Me.Close()
    End Sub
    Dim Orig As System.Drawing.Color
    Dim OrigOp As Double
    Private Sub Tools_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        My.Settings.Reload()
        Orig = My.Settings.Colour
        OrigOp = My.Settings.opacity
        Me.TopMost = True
        frmDockBackground.tmrDetector.Stop()
        CheckBox1.Checked = My.Settings.UseWindowsTheme
        usewindowtheming = My.Settings.UseWindowsTheme
        comparison = My.Settings.Comparison
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        My.Settings.Colour = Orig
        My.Settings.opacity = OrigOp
        My.Settings.Comparison = comparison
        My.Settings.UseWindowsTheme = usewindowtheming
        My.Settings.Save()
        frmDockBackground.Background()
        Me.Close()
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        frmDockBackground.Opacity = TrackBar1.Value / 100
    End Sub
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        My.Settings.Comparison = 97
        My.Settings.Save()
        CheckBox2.Checked = False
        frmDock.DockColorUpdate()
    End Sub
    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        My.Settings.Comparison = 50
        My.Settings.Save()
        CheckBox3.Checked = False
        frmDock.DockColorUpdate()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            My.Settings.UseWindowsTheme = True
            My.Settings.Save()
            frmDock.DockColorUpdate()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim form1 As New Form
        With form1
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            If My.Settings.CustomCompare = New Point(0, 0) And My.Settings.CustomSize = New Point(0, 0) Then
                .Height = Screen.PrimaryScreen.Bounds.Height / 4
                .Width = Screen.PrimaryScreen.Bounds.Width / 4
                .Location = New Point(0, 0)
            End If
            .BackColor = Color.DarkRed
            .Opacity = 0.8
            AddHandler .Paint, AddressOf PaintSquare
            Dim Pointx As Point
            .Controls.Add(New Label)
            .Controls.Item(0).Text = "Close"
            AddHandler .Controls.Item(0).Click, Sub()
                                                    form1.Close()
                                                    My.Settings.CustomCompare = form1.Location
                                                    My.Settings.CustomSize = form1.Size()
                                                    My.Settings.Save()
                                                End Sub
            AddHandler .MouseClick, Sub()
                                        Pointx = New Point(form1.Left - MousePosition.X, form1.Top - MousePosition.Y)
                                    End Sub
            AddHandler .MouseMove, Sub()

                                       If MouseButtons = Windows.Forms.MouseButtons.Left Then
                                           form1.Location = New Point(MousePosition.X + Pointx.X, MousePosition.Y + Pointx.Y)
                                       End If
                                   End Sub

            .Show()
        End With
    End Sub
    Sub PaintSquare(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(Pens.Red, 0, 0, 0, Me.Height) 'Left Line
        e.Graphics.DrawLine(Pens.Red, 0, Me.Height - 1, Me.Width, Me.Height - 1) 'Bottom Line
        e.Graphics.DrawLine(Pens.Red, Me.Width - 1, 0, Me.Width - 1, Me.Height) 'Right Line
        e.Graphics.DrawLine(Pens.Red, 0, 1, Me.Width, 1)
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        My.Settings.Comparison = 40
        My.Settings.opacity = 0.8
        My.Settings.CustomCompare = New Point(0, 0)
        My.Settings.CustomSize = New Point(0, 0)
        My.Settings.UseWindowsTheme = True
        My.Settings.Save()
        Tools_Load(Me, Nothing)
    End Sub
End Class
