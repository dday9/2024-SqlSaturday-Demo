Imports System.Net
Imports System.Text.Json
Imports System.Text.Json.Serialization

Namespace WebServer

    Public Class WebView2Response

        Public Sub New(responseBody As String, responseStatus As Integer)
            Body = responseBody
            Status = responseStatus
        End Sub

        Public Property Body As String

        <JsonPropertyName("Ok")>
        Public ReadOnly Property IsOk As Boolean
            Get
                Return Status >= 200 AndAlso Status <= 299
            End Get
        End Property

        Public Property Status As Integer

        Public Overrides Function ToString() As String
            Dim response = JsonSerializer.Serialize(Me)
            Return response
        End Function

        ' 200s
        Public Shared Function Ok(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.OK)
        End Function

        Public Shared Function Created(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.Created)
        End Function

        Public Shared Function NoContent() As WebView2Response
            Return New WebView2Response(String.Empty, HttpStatusCode.NoContent)
        End Function

        ' 400s
        Public Shared Function BadRequest(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.BadRequest)
        End Function

        Public Shared Function Unauthorized(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.Unauthorized)
        End Function

        Public Shared Function Forbidden(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.Forbidden)
        End Function

        Public Shared Function NotFound(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.NotFound)
        End Function

        Public Shared Function MethodNotAllowed(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.MethodNotAllowed)
        End Function

        Public Shared Function Conflict(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.Conflict)
        End Function

        ' 500s
        Public Shared Function InternalServerError() As WebView2Response
            Return InternalServerError("Oops, something went wrong.")
        End Function

        Public Shared Function InternalServerError(body As String) As WebView2Response
            Return New WebView2Response(body, HttpStatusCode.InternalServerError)
        End Function

    End Class

End Namespace
