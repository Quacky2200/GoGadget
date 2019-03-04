Public MustInherit Class AnimatorBase

#Region "Properties"
    Public Property isRunning As Boolean
        Get
            Return _isRunning
        End Get
        Set(value As Boolean)
            _isRunning = value
        End Set
    End Property
    Private _isRunning As Boolean = False
#End Region

    Public Overridable Sub [Start]()
        isRunning = True
    End Sub

    Public Overridable Sub [Stop]()
        isRunning = False
    End Sub

    Public Overridable Sub Toggle()
        If isRunning Then
            [Stop]()
        Else
            [Start]()
        End If
    End Sub

End Class
