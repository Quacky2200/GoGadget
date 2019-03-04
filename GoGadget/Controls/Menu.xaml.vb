Imports System.Windows.Media.Animation
Public Enum MenuOrientation As Integer
    FromTop = 0
    FromRight = 1
    FromBottom = 2
    FromLeft = 3
End Enum
Public Class Menu
#Region "Properties"
    Private Property _Orientation As MenuOrientation
    Public Property Orientation As MenuOrientation
        Get
            Return _Orientation
        End Get
        Set(value As MenuOrientation)
            _Orientation = value
            Select Case value
                Case MenuOrientation.FromTop
                    MenuArrow.VerticalAlignment = Windows.VerticalAlignment.Top
                    MenuArrow.HorizontalAlignment = Windows.HorizontalAlignment.Center
                    MenuButtonContainer.Margin = New Thickness(0, 10, 0, 0)
                Case MenuOrientation.FromRight
                    MenuArrow.HorizontalAlignment = Windows.HorizontalAlignment.Right
                    MenuButtonContainer.Margin = New Thickness(0, 0, 10, 0)
                Case MenuOrientation.FromBottom
                    MenuArrow.VerticalAlignment = Windows.VerticalAlignment.Bottom
                    MenuArrow.HorizontalAlignment = Windows.HorizontalAlignment.Center
                    MenuButtonContainer.Margin = New Thickness(0, 0, 0, 10)

                Case MenuOrientation.FromLeft
                    MenuArrow.VerticalAlignment = Windows.VerticalAlignment.Center
                    MenuArrow.HorizontalAlignment = Windows.HorizontalAlignment.Left
                    MenuButtonContainer.Margin = New Thickness(10, 0, 0, 0)
            End Select
        End Set
    End Property
    Private Property _MenuBackgroundColor As Color = ColorConverter.ConvertFromString("#FF515151")
    Public Property Location As Point
        Get
            Return New Point(Me.Left, Me.Top)
        End Get
        Set(value As Point)
            Me.Left = value.X
            Me.Top = value.Y
        End Set
    End Property
    Public InitialisedPoint As Point
#End Region
    Public Sub New(Location As Point, ActionMenu As Dictionary(Of String, Action), Orientation As MenuOrientation)
        InitializeComponent()
        For Each MenuItem In ActionMenu
            Dim MB As New MenuButton(MenuItem.Key, MenuItem.Value)
            MB.Margin = New Thickness(0, 30 * ReturnMenuButtons.Count, 0, 0)
            MenuButtonContent.Children.Add(MB)
        Next
        Me.Orientation = Orientation
        InitialisedPoint = Location
    End Sub
    Private Sub Menu_Loaded() Handles Me.Loaded
        Locate()
    End Sub
    Private Function ReturnMenuButtons() As List(Of MenuButton)
        Return MenuButtonContent.Children.OfType(Of MenuButton).ToList
    End Function
    Private Sub Mouse_Leave() Handles Me.MouseLeave
        Dim RunOnce As New RunOnceTimer(700, Sub() Dispatcher.Invoke(Sub()
                                                                         Me.Hide()
                                                                     End Sub))
    End Sub
    Private Sub Locate()
        Select Case Orientation
            Case MenuOrientation.FromTop
                Me.Location = New Point(InitialisedPoint.X + (Me.Width / 2), InitialisedPoint.Y)
            Case MenuOrientation.FromRight
                Dim Y As Integer = InitialisedPoint.Y - (Me.Height / 2)
                MenuArrow.VerticalAlignment = If(Y < 0, Windows.VerticalAlignment.Top, Windows.VerticalAlignment.Center)
                MenuArrow.Margin = If(Y < 0, New Thickness(0, MenuArrow.Height / 2 + 5, 0, 0), Nothing)
                Y = If(Y < 0, 0, Y)
                Me.Location = New Point(InitialisedPoint.X - Me.Width, Y)
            Case MenuOrientation.FromBottom
                Me.Location = New Point(InitialisedPoint.X + (Me.Width / 2), InitialisedPoint.Y + Me.Height)
            Case MenuOrientation.FromLeft
                Dim Y As Integer = InitialisedPoint.Y - (Me.Height / 2)
                MenuArrow.VerticalAlignment = If(Y < 0, Windows.VerticalAlignment.Top, Windows.VerticalAlignment.Center)
                MenuArrow.Margin = If(Y < 0, New Thickness(0, InitialisedPoint.Y, 0, 0), Nothing)
                Y = If(Y < 0, 0, Y)
                Me.Location = New Point(InitialisedPoint.X, Y)
        End Select
    End Sub
End Class