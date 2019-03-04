Public Class MainMenuItems
    Inherits Dictionary(Of String, Action)
    Sub New()
        'Me.Add("Options", Sub()
        '                      MsgBox("Not Implemented Yet")
        '                  End Sub)
        'If Main.VoiceCommand.IsEnabled Then
        '    Me.Add("Show Voice Command Info", Sub()
        '                                          MsgBox("Not Implemented Yet")
        '                                      End Sub)
        '    Me.Add("Turn Off Voice Command", Sub()
        '                                         Main.VoiceCommand.Stop()
        '                                     End Sub)
        'Else
        '    Me.Add("Turn On Voice Command", Sub()
        '                                        Main.VoiceCommand.Start()
        '                                    End Sub)
        'End If
        Me.Add("Feedback", Sub()
                               Process.Start("https://www.surveymonkey.com/s/8FBJMTG")
                           End Sub)
        If My.Settings.Dock_Hide Then
            Me.Add("Stop Hiding", Sub()
                                      My.Settings.Dock_Hide = False
                                      My.Settings.Save()
                                  End Sub)
        Else
            Me.Add("Start Hiding", Sub()
                                       My.Settings.Dock_Hide = True
                                       My.Settings.Save()
                                   End Sub)
        End If
        If My.Settings.Wallpaper_Color Then
            Me.Add("Stop Colour Detection", Sub()
                                                My.Settings.Wallpaper_Color = False
                                                My.Settings.Wallpaper_LastMod = New Date(2000, 1, 1)
                                                My.Settings.Save()
                                                Main.DockColorThread.Abort()
                                                Main.SetDockColor(Colors.Black)
                                            End Sub)
        Else
            Me.Add("Start Colour Detection", Sub()
                                                 My.Settings.Wallpaper_Color = True
                                                 My.Settings.Save()
                                                 Main.NewDockColourThread()
                                             End Sub)
        End If
       
        Me.Add("Quit", Sub()
                           End
                       End Sub)
    End Sub
End Class
