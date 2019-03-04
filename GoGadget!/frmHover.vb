Public Class frmHover
    Public title As String
    Private Sub frmHover_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmHover_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Hide()
        frmDockBackground.tmrDetector.Start()
    End Sub

    Private Sub frmHover_MouseHocover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseHover
        Me.Focus()
        tmrHover.Stop()
        frmDockBackground.tmrDetector.Stop()
    End Sub

    Private Sub frmHover_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        e.Graphics.Clear(Color.Transparent)
        e.Graphics.DrawString(title, Me.Font, Brushes.White, 5, 5)
    End Sub
End Class