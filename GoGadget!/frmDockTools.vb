Imports System.Security.Permissions

Public Class frmDockTools
    Dim color1, color2, color3 As Integer
    Dim opacity1 As Double
    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        SetColour()
    End Sub
    Sub SetColour()
        Dim colour As Color = Color.FromArgb(TrackBar3.Value, TrackBar2.Value, TrackBar4.Value, TrackBar6.Value)
        My.Settings.Colour = colour
        My.Settings.opacity = TrackBar1.Value / 100
        My.Settings.Save()
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
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        My.Settings.Colour = Orig
        My.Settings.opacity = OrigOp
        My.Settings.Save()
        frmDockBackground.Background()
        Me.Close()
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        SetColour()
    End Sub
End Class
