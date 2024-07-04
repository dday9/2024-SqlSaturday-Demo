Namespace Services.Logger

    Public Class DebugLogger
        Implements ILogger

        Public Sub LogInformation(value As String) Implements ILogger.LogInformation
            LogSeverity(value, "Information")
        End Sub

        Public Sub LogWarning(value As String) Implements ILogger.LogWarning
            LogSeverity(value, "Warning")
        End Sub

        Public Sub LogError(value As String) Implements ILogger.LogError
            LogSeverity(value, "Error")
        End Sub

        Private Sub LogSeverity(value As String, severity As String)
            Debug.WriteLine($"{Date.Now:HH:mm} - {severity} - {value}")
        End Sub

    End Class

End Namespace