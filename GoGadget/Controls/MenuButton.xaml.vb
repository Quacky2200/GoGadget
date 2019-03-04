Imports System.Windows.Media.Animation

Public Class MenuButton
    Sub New(ButtonText As String, ClickAction As Action)
        InitializeComponent()
        ButtonLabel.Content = ButtonText
        AddHandler Me.MouseLeftButtonUp, Sub()
                                             Dim Main As Menu = CType(CType(CType(CType(Me.Parent, Grid).Parent, Border).Parent, Grid).Parent, Grid).Parent
                                             Main.Hide()
                                             ClickAction.Invoke()
                                         End Sub
    End Sub
End Class
