Imports System.IO
Imports System.Text.Json
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports SqlSaturdayWebView2Demo.WebServer
Imports SqlSaturdayWebView2Demo.WebServer.Controllers

Public Class FormMain

    Private ReadOnly _controllerMaps As Dictionary(Of String, BaseController) = BuildControllerMaps()

    Private Async Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Await ConfigureWebView2(WebView2Main)
        OpenWebPage(WebView2Main, "WebPages/Inventory/Index.html")
    End Sub

    Private Function BuildControllerMaps() As Dictionary(Of String, BaseController)
        Dim widgetTypeController = New WidgetTypesController("WidgetTypes")
        Dim widgetController = New WidgetsController("Widgets")
        Dim inventoriesController = New WidgetsController("Inventory")
        Return New Dictionary(Of String, BaseController) From {
            {widgetTypeController.TableName, widgetTypeController},
            {widgetController.TableName, widgetController},
            {"Inventories", inventoriesController}
        }
    End Function

    Private Async Sub WebView2Main_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs) Handles WebView2Main.WebMessageReceived
        Dim container = TryCast(sender, WebView2)
        If (container Is Nothing) Then
            Return
        End If

        Await container.EnsureCoreWebView2Async(Nothing)
        Dim messageJson = e.WebMessageAsJson()
        Dim request As WebView2Request = Nothing
        Dim response = WebView2Response.InternalServerError()
        Try
            request = JsonSerializer.Deserialize(Of WebView2Request)(messageJson)
        Catch ex As Exception
            response = WebView2Response.BadRequest("The payload is not a valid request.")
            My.Application.Logger.LogError(ex.ToString())
        End Try

        If (request IsNot Nothing) Then
            Dim controller = request.TryGetController()
            Dim route = request.TryGetRoute()
            If (Not _controllerMaps.ContainsKey(controller)) Then
                Dim payload = $"{If(String.IsNullOrWhiteSpace(controller), "No controller", "Invalid controller")} passed in: {request.Url}"
                response = WebView2Response.NotFound(payload)
            Else
                Dim mappedController = _controllerMaps(controller)
                If (Not mappedController.Routes.ContainsKey(route)) Then
                    Dim payload = $"{If(String.IsNullOrWhiteSpace(controller), "No route", "Invalid route")} passed in: {request.Url}"
                    response = WebView2Response.NotFound(payload)
                Else
                    Dim mappedRoute = mappedController.Routes(route)
                    Try
                        response = mappedRoute(request)
                    Catch ex As Exception
                        response = WebView2Response.InternalServerError("Something went wrong.")
                        My.Application.Logger.LogError(ex.ToString())
                    End Try
                End If
            End If
        End If

        Await container.CoreWebView2.ExecuteScriptAsync($"dispatchMessageReceivedEvent({messageJson}, {response});")
    End Sub

    Private Async Function ConfigureWebView2(container As WebView2) As Task
        Await container.EnsureCoreWebView2Async(Nothing)
        Dim webAssets = AssertApplicationDirectoryPath("WebAssets")
        Dim webPages = AssertApplicationDirectoryPath("WebPages")
        container.CoreWebView2.SetVirtualHostNameToFolderMapping("web-assets.local", webAssets, CoreWebView2HostResourceAccessKind.Allow)
        container.CoreWebView2.SetVirtualHostNameToFolderMapping("web-pages.local", webPages, CoreWebView2HostResourceAccessKind.Allow)
        container.CoreWebView2.Settings.IsWebMessageEnabled = True
    End Function

    Private Sub OpenWebPage(container As WebView2, ParamArray filePathParts() As String)
        Dim fileContentPath = AssertApplicationFilePath(filePathParts)
        Dim url = New Uri(fileContentPath).ToString()
        container.CoreWebView2.Navigate(url)
    End Sub

    Private Shared Function AssertApplicationFilePath(ParamArray filePathParts() As String) As String
        Dim filePaths = {Application.StartupPath}.Concat(filePathParts).ToArray()
        Dim contentPath = Path.Combine(filePaths)
        If (Not File.Exists(contentPath)) Then
            Throw New ArgumentOutOfRangeException(NameOf(filePathParts), $"The following file does not exit: {contentPath}")
        End If

        Return contentPath
    End Function

    Private Shared Function AssertApplicationDirectoryPath(ParamArray filePathParts() As String) As String
        Dim paths = {Application.StartupPath}.Concat(filePathParts).ToArray()
        Dim contentPath = Path.Combine(paths)
        If (Not Directory.Exists(contentPath)) Then
            Throw New ArgumentOutOfRangeException(NameOf(filePathParts), $"The following file does not exit: {contentPath}")
        End If
        Return contentPath
    End Function

End Class
