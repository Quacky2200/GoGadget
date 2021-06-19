Public Class frmQuickRename
    Public found As Integer
    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        frmDock.Database(1, 0, 0)(found) = TextBox1.Text
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If (e.KeyCode = Keys.Enter) = True Then
            frmDock.Database(1, 0, 0)(found) = TextBox1.Text
            My.Settings.Save()
            Me.Close()
        ElseIf (e.KeyCode = Keys.Escape) = True Then
            Me.Close()
        End If
    End Sub
End Class