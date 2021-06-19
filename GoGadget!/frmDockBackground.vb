Public Class frmDockBackground
    Public hidden As Boolean
    Dim revolutions As Integer
    Private Sub Background_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        LineShape1.Y1 = 0 'Start at the top of the screen
        LineShape1.Y2 = Screen.PrimaryScreen.WorkingArea.Height ' and end at the bottom
        Background() 'set up the background
        tmrDetector.Start() 'We also hide it by default, makes it look cleaner but we detect the mouse cursor so we have a second to see the dock.
    End Sub
    Sub LineShape1_MouseLeave() Handles LineShape1.MouseLeave
        'However we need to know where the mouse is so we see where the mouse is when you hover and leave the line
        'then we can start the detector which will run each second to see if the mouse is over the dock
        'if not it will hide therefor eliminating dodgy detection with mouse hovers on objects
        If frmDock.Bounds.Contains(MousePosition) Then
            tmrDetector.Start()'Named propery aswell unlike some of the bad work that was done.
            frmDock.Focus() 'Prevent the background getting on top off the dock
        End If
    End Sub
    Sub LineShape1_MouseHover() Handles LineShape1.MouseHover
        'Here we detect where the mouse is and because it starts up hidden, 
        'we will automatically assume it will be hidden when someone hovers on the grey line
        If hidden = True Then
            tmrDockHide.Start()
        End If
    End Sub
    Sub LineShape1_MouseClick() Handles LineShape1.Click
        frmDock.BringToFront()
    End Sub
    Sub Background() ' We use this to set the background later when we change the settings but we also load it at the start
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.BackColor = My.Settings.Colour
        Me.Opacity = My.Settings.opacity
    End Sub
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            'This hides the program from the alt tab mechanism and the start bar has been done as well - makes it look professional :)
            Dim cp As CreateParams = MyBase.CreateParams
            If Not Me.DesignMode Then cp.ExStyle = cp.ExStyle Or &H80
            Return cp
        End Get
    End Property
    Sub tmrDockHide_Tick() Handles tmrDockHide.Tick
        If hidden = False Then 'if it's not hidden then we show the dock
            If revolutions = 45 Then 'then we ask if it has pulled it in 9 times
                LineShape1.Enabled = True 'and set what happens when thats done
                Me.Left += 3
                frmDock.Left += 3
                hidden = True
                LineShape1.BorderWidth = 3 'Make it easier to hover over
                tmrDockHide.Stop()
            Else
                Me.Left += 5
                frmDock.Left += 5
                revolutions += 5
            End If
        Else 'However if it's hidden, we'll show it!
            If revolutions = 0 Then 'We then ask the same question as last but in reverse because the number is now 45
                Me.Left -= 3 'We align it specifically just because it looks better
                frmDock.Left -= 3
                hidden = False ' we also have to change the status of the visibility since we changing the position/width - if i get to width...
                'Thats because the forms visibility is a bit different
                LineShape1.BorderWidth = 1
                frmDock.Opacity = 1
                tmrDockHide.Stop() 'We must always stop this
            Else
                Me.Left -= 5 'Make the background come out
                frmDock.Left -= 5 'as well as the actual dock applications
                revolutions -= 5
            End If
        End If
    End Sub
    Private Sub tmrDetector_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDetector.Tick
        If Not frmDock.Bounds.Contains(MousePosition) And Not Me.Bounds.Contains(MousePosition) And Not frmDock.ContextMenuStrip1.Bounds.Contains(MousePosition) Then
            tmrDockHide.Start()
            frmDock.SortAfterRemix()
            frmDock.pDock.Controls.Remove(frmDock.Placer)
            Try
                frmDock.pDock.Controls.Remove(frmDock.Placer)
            Catch ex As Exception
            End Try
            sender.stop()
        End If
    End Sub

    Sub tmrDragDrop_Tick() Handles tmrDragDrop.Tick
        If (MouseButtons = Windows.Forms.MouseButtons.Left) = True And MousePosition.X > Me.Left And frmDock.pDock.Controls.IndexOf(frmDock.Placer) = -1 Then
            If hidden = True Then
                If MousePosition.X >= Screen.PrimaryScreen.Bounds.Width - 2 Then
                    tmrDockHide.Start()
                End If
            End If
            If frmDock.IconSelect.Enabled = False Then
                frmDock.MakePlacer()
            End If
            If frmDock.Database(1, 1, 1).Count <> 0 Then
                frmDock.IconSelect.Start()
                frmDock.IconSelect.Interval = 10
            End If
        ElseIf MousePosition.X < Me.Left Then
            frmDock.pDock.Controls.Remove(frmDock.Placer)
        End If
    End Sub
    Sub IgnoreFocus(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown
        frmDock.Focus()
        frmDock.BringToFront()
        If e.Button = Windows.Forms.MouseButtons.Right Then
            frmDockTools.Show()
        End If
    End Sub
End Class