Imports System.IO
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ApplicationServices
Imports SqlSaturdayWebView2Demo.Services.Logger
Imports SqlSaturdayWebView2Demo.SqlSaturdayWebView2Demo

Namespace My

    Partial Friend Class MyApplication

        Private _database As DataSetExample
        Public ReadOnly Property Database As DataSetExample
            Get
                If (_database Is Nothing) Then
                    _database = New DataSetExample()
                End If
                Return _database
            End Get
        End Property

        Private _logger As ILogger
        Public ReadOnly Property Logger As ILogger
            Get
                If (_logger Is Nothing) Then
                    _logger = New ConsoleLogger()
                End If
                Return _logger
            End Get
        End Property

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            Dim serializedData = Settings.DatabaseExample
            If (Not String.IsNullOrWhiteSpace(serializedData)) Then
                Try
                    _database = DeserializeData(serializedData)
                Catch ex As Exception
                    _logger.LogError($"Unable to load the database. Exception: {ex}")
                End Try
            End If
        End Sub

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown
            If (_database Is Nothing) Then
                Return
            End If

            Try
                Dim serializedData = SerializeData(_database)
                Settings.DatabaseExample = serializedData
                Settings.Save()
            Catch ex As Exception
                _logger.LogError($"Unable to save the database. Exception: {ex}")
            End Try
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            e.ExitApplication = False
            _logger.LogError($"Unexpected error occurred. Exception: {e.Exception}")
        End Sub


        Private Shared Function SerializeData(database As DataSetExample) As String
            Dim serializedData = String.Empty

            Using memoryStream = New MemoryStream()
                Dim xmlSerializer = New XmlSerializer(database.GetType())
                xmlSerializer.Serialize(memoryStream, database)
                memoryStream.Position = 0

                Using reader = New StreamReader(memoryStream)
                    serializedData = reader.ReadToEnd()
                End Using
            End Using

            Return serializedData
        End Function

        Private Shared Function DeserializeData(serializedData As String) As DataSetExample
            Dim database = New DataSetExample()

            Using memoryStream = New MemoryStream(Text.Encoding.UTF8.GetBytes(serializedData))
                Dim xmlSerializer = New XmlSerializer(database.GetType())
                database = DirectCast(xmlSerializer.Deserialize(memoryStream), DataSetExample)
            End Using

            Return database
        End Function


    End Class

End Namespace
