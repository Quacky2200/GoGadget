Imports System.Runtime.InteropServices

Public Class RightClickMainMenuItems
    Inherits Dictionary(Of String, Action)
    Sub New(App As AppControl)
        If Not App.isRunning Then
            Me.Add("Open " & If(App.AppName.Length >= 40, Mid(App.AppName, 1, 37).Trim() & "...", App.AppName), _
                   Sub()
                       App.Open()
                   End Sub)
            Me.Add("Remove", Sub()
                                 App.Remove()
                             End Sub)
        ElseIf App.isRunning Then
            Me.Add("Show " & If(App.AppName.Length >= 40, Mid(App.AppName, 1, 37).Trim() & "...", App.AppName), _
                   Sub()
                       App.ShowWindow()
                   End Sub)
            Me.Add("New Window", Sub()
                                     App.Open()
                                 End Sub)
            If App.isPinned AndAlso App.AppName <> "GoGadget" Then
                Me.Add("Remove", Sub()
                                     App.isPinned = False
                                     Main.Save()
                                 End Sub)
            Else
                Me.Add("Add", Sub()
                                  App.isPinned = True
                                  Main.Save()
                              End Sub)
            End If
            If App.Name <> "GoGadget" AndAlso App.Type = AppControlType.Application Then
                Me.Add("Kill Process", Sub()
                                           App.Kill()
                                       End Sub)
            End If
            Me.Add("Close", Sub()
                                App.Close()
                            End Sub)
            End If
            Me.Add("Show Info", Sub()
                                    If IsNothing(Main.Info) Then
                                        Main.Info = New InformationWindow(App) 'Show if it has been disposed
                                    Else
                                        Main.Info.SetValues(App) 'Change the current values if the information window is active
                                    End If
                                    Main.Info.Show()
                                End Sub)

    End Sub
End Class
