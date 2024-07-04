Imports System.Text.Json
Imports System.Web

Namespace WebServer

    Public Class WebView2Request

        Public Property Body As String
        Public Property Method As String
        Public Property Origin As String
        Public Property Url As String

        Public Shared Function FromJson(literal As String) As WebView2Request
            Dim request = JsonSerializer.Deserialize(Of WebView2Request)(literal)
            AssertRequest(request)

            Return request
        End Function

        Public Shared Sub AssertRequest(request As WebView2Request)
            If (request Is Nothing) Then
                Throw New ArgumentNullException(NameOf(request))
            End If

            If (String.IsNullOrWhiteSpace(request.Method)) Then
                Throw New ArgumentOutOfRangeException(NameOf(request), $"{NameOf(request.Method)} cannot be null.")
            End If

            Dim acceptableMethods = {"GET", "POST", "PUT", "PATCH", "DELETE"}
            If (Not acceptableMethods.Any(Function(acceptableMethod) request.Method.Equals(acceptableMethod, StringComparison.OrdinalIgnoreCase))) Then
                Throw New ArgumentOutOfRangeException(NameOf(request), $"{NameOf(request.Method)} is not an acceptable value: {request.Method}.")
            End If

            If (String.IsNullOrWhiteSpace(request.Body) AndAlso Not request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)) Then
                Throw New ArgumentOutOfRangeException(NameOf(request), $"{NameOf(request.Method)} requires a body.")
            End If
        End Sub

        Private Shared Function GetQueryStringParameters(url As String) As Specialized.NameValueCollection
            Dim baseUri = New Uri("http://www.example.com/")
            Dim parsedUri As Uri = Nothing
            If (Not Uri.TryCreate(baseUri, url, parsedUri)) Then
                Return New Specialized.NameValueCollection()
            End If

            Dim queryStringParameters = HttpUtility.ParseQueryString(parsedUri.Query)
            Return queryStringParameters
        End Function

        Public Function TryGetController() As String
            Dim baseUri = New Uri("http://www.example.com/")
            Dim parsedUri As Uri = Nothing
            If (Not Uri.TryCreate(baseUri, Url, parsedUri)) Then
                Return String.Empty
            End If

            Return parsedUri.Segments.Skip(1).FirstOrDefault().Replace("/", String.Empty)
        End Function

        Public Function TryGetIdFromUrl() As String
            Dim queryStringParameters = GetQueryStringParameters(Url)
            Dim param = queryStringParameters.Get("id")
            Return param
        End Function

        Public Function TryGetRoute() As String
            Dim baseUri = New Uri("http://www.example.com/")
            Dim parsedUri As Uri = Nothing
            If (Not Uri.TryCreate(baseUri, Url, parsedUri)) Then
                Return String.Empty
            End If

            Return parsedUri.Segments.Skip(2).FirstOrDefault()
        End Function

    End Class

End Namespace
