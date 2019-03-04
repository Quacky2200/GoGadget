Imports System.Windows.Media.Animation
Public Class SlideAnimator
    Inherits AnimatorBase
#Region "Properties"
    Public Property WindowInstance As Window
    Public Property SlideMovement = 60
    Public Property SlideSpeed = 0.3
    Private SlideAnimator As DoubleAnimation
    Private WidthAnimator As DoubleAnimation
    Public Overrides Sub [Start]()
        If Not isRunning Then
            MyBase.[Start]()
            SlideAnimator = New DoubleAnimation(WindowInstance.Left - SlideMovement, TimeSpan.FromSeconds(SlideSpeed))
            WidthAnimator = New DoubleAnimation(WindowInstance.Width - SlideMovement, TimeSpan.FromSeconds(SlideSpeed))
            Dim SB As New Storyboard With {.Duration = TimeSpan.FromSeconds(SlideSpeed)}
            AddHandler SB.Completed, Sub() isRunning = False
            Storyboard.SetTargetProperty(SlideAnimator, New PropertyPath(Window.LeftProperty))
            Storyboard.SetTargetProperty(WidthAnimator, New PropertyPath(Window.WidthProperty))
            SB.Children.Add(SlideAnimator)
            SB.Children.Add(WidthAnimator)
            SB.Begin(WindowInstance)
        End If
    End Sub
    Public Overrides Sub Toggle()
        If Not isRunning Then
            SlideMovement = If(SlideMovement > 0, -(SlideMovement), Math.Abs(SlideMovement))
            [Start]()
        End If
    End Sub
    Sub New(WindowInstance As Window)
        Me.WindowInstance = WindowInstance
    End Sub
#End Region
End Class
