Public Class TimeTracker
#Region "Properties"
    Private Property _StartedRecording As DateTime
    Public ReadOnly Property StartedRecording As DateTime
        Get
            Return _StartedRecording
        End Get
    End Property
    Private Property _StoppedRecording As DateTime
    Public ReadOnly Property StoppedRecording As DateTime
        Get
            Return _StoppedRecording
        End Get
    End Property
    Private Property TrackedTime As TimeSpan
    Public ReadOnly Property Result As String
        Get
            Return If(TrackedTime.Hours > 0, TrackedTime.Hours & "Hrs ", Nothing) & _
                If(TrackedTime.Minutes > 0, TrackedTime.Minutes & "Mns ", Nothing) & _
                If(TrackedTime.Seconds > 0, TrackedTime.Seconds & "Scs ", Nothing) & _
                If(TrackedTime.Milliseconds > 0, TrackedTime.Milliseconds & "Ms ", Nothing)
        End Get
    End Property
#End Region
    Public Sub [Start]()
        _StartedRecording = DateTime.Now
    End Sub
    Public Sub [Stop]()
        _StoppedRecording = DateTime.Now
        TrackedTime = If(StartedRecording.TimeOfDay > StoppedRecording.TimeOfDay, StartedRecording.TimeOfDay - StoppedRecording.TimeOfDay, StoppedRecording.TimeOfDay - StartedRecording.TimeOfDay)
    End Sub
    Public Function [StopAndReturn]() As String
        _StoppedRecording = DateTime.Now
        TrackedTime = If(StartedRecording.TimeOfDay > StoppedRecording.TimeOfDay, StartedRecording.TimeOfDay - StoppedRecording.TimeOfDay, StoppedRecording.TimeOfDay - StartedRecording.TimeOfDay)
        Return Result
    End Function
End Class
