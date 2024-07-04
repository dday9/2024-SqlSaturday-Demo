Namespace Services.Logger

    Public Interface ILogger

        Sub LogInformation(value As String)
        Sub LogWarning(value As String)
        Sub LogError(value As String)

    End Interface

End Namespace