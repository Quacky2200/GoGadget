Imports System.Windows.Media.Animation

Public Class RotationAnimator
    Inherits AnimatorBase

#Region "Properties"
    Public Property RotateTransformInstance As RotateTransform

    Public Property WiggleSpectrum As Double
        Get
            Return _WiggleSpectrum
        End Get
        Set(value As Double)
            _WiggleSpectrum = value
            Dim HalfSpectrum As Double = _WiggleSpectrum / 2
            _CachedStartDegree = -HalfSpectrum
            _CachedEndDegree = HalfSpectrum
        End Set
    End Property
    Private _WiggleSpectrum As Double = 30

    Public Property WiggleSpeed As Double
        Get
            Return _WiggleSpeed
        End Get
        Set(value As Double)
            _WiggleSpeed = value
            _CachedSpeed = TimeSpan.FromSeconds(_WiggleSpeed)
        End Set
    End Property
    Private _WiggleSpeed As Double = 0.1

    Private ReadOnly Property CachedSpeed As TimeSpan
        Get
            Return _CachedSpeed
        End Get
    End Property
    Private _CachedSpeed As TimeSpan = TimeSpan.FromSeconds(WiggleSpeed)

    Private ReadOnly Property CachedStartDegree As Double
        Get
            Return _CachedStartDegree
        End Get
    End Property
    Private _CachedStartDegree As Double = -(WiggleSpectrum / 2)

    Private ReadOnly Property CachedEndDegree As Double
        Get
            Return _CachedEndDegree
        End Get
    End Property
    Private _CachedEndDegree As Double = (WiggleSpectrum / 2)

#End Region

#Region "Animations"
    ''' <summary>
    ''' --NOTES--
    ''' 
    ''' </summary>
    ''' <remarks></remarks>

    Private WithEvents StartRotationAnimation As DoubleAnimation
    Private WithEvents StopRotationAnimation As DoubleAnimation
    Private WithEvents RotationAnimation As DoubleAnimation
    Private WithEvents ShadowAnimation As DoubleAnimation
    Private WithEvents ColorAnimation As ColorAnimation

    Private Stopping As Boolean = False
    Private Starting As Boolean = False
    Private isAtStart As Boolean = False

    Private Sub RotationAnimation_Completed(sender As Object, e As EventArgs)
        'If we are still notifying, Continue Animation or just exit.
        If isRunning AndAlso Not Stopping Then
            If isAtStart Then
                isAtStart = False
                RotationAnimation = New DoubleAnimation(CachedEndDegree, CachedSpeed) _
                    With {.EasingFunction = New SineEase With {.EasingMode = EasingMode.EaseInOut}}
            Else
                isAtStart = True
                RotationAnimation = New DoubleAnimation(CachedStartDegree, CachedSpeed) _
                    With {.EasingFunction = New SineEase With {.EasingMode = EasingMode.EaseInOut}}
            End If
            AddHandler RotationAnimation.Completed, AddressOf RotationAnimation_Completed
            RotateTransformInstance.BeginAnimation(RotateTransform.AngleProperty, RotationAnimation)
        End If
    End Sub

    ''' <summary>
    ''' Raised after it has positioned itself to 135 degrees.
    ''' </summary>
    Private Sub StartRotationAnimation_Completed(sender As Object, e As EventArgs) Handles StartRotationAnimation.Completed
        isRunning = True
        Starting = False
        isAtStart = True
        RotationAnimation = New DoubleAnimation(CachedStartDegree, CachedSpeed) _
                    With {.EasingFunction = New SineEase With {.EasingMode = EasingMode.EaseInOut}}
        AddHandler RotationAnimation.Completed, AddressOf RotationAnimation_Completed
        RotateTransformInstance.BeginAnimation(RotateTransform.AngleProperty, RotationAnimation)
    End Sub

    ''' <summary>
    ''' Raised after it has positioned itself to 90 degrees.
    ''' </summary>
    Private Sub StopRotationAnimation_Completed(sender As Object, e As EventArgs) Handles StopRotationAnimation.Completed
        isRunning = False
        Stopping = False
    End Sub

#End Region

    Public Overrides Sub [Start]()
        If Not isRunning And Not Starting Then
            Starting = True
            StartRotationAnimation = New DoubleAnimation(CachedStartDegree, CachedSpeed)
            RotateTransformInstance.BeginAnimation(RotateTransform.AngleProperty, StartRotationAnimation)
        End If
    End Sub

    Public Overrides Sub [Stop]()
        If isRunning And Not Stopping Then
            Stopping = True
            StopRotationAnimation = New DoubleAnimation(0, CachedSpeed) With { _
            .EasingFunction = New SineEase With {.EasingMode = EasingMode.EaseInOut}}
            RotateTransformInstance.BeginAnimation(RotateTransform.AngleProperty, StopRotationAnimation)
        End If
    End Sub

    Public Overrides Sub Toggle()
        If isRunning Then
            [Stop]()
        ElseIf Not Starting Then
            [Start]()
        End If
    End Sub

    Public Sub New(rotateTransform As RotateTransform)
        Me.RotateTransformInstance = rotateTransform
    End Sub
End Class
