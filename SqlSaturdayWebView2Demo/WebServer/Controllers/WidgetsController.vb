Namespace WebServer.Controllers

    Public Class WidgetsController
        Inherits BaseController

        Public Sub New(tableName As String)
            MyBase.New(tableName)
            _routes = New Dictionary(Of String, Route) From {
                {NameOf(Create), New Route(AddressOf Create)},
                {NameOf(GetAll), New Route(AddressOf GetAll)},
                {"Get", New Route(AddressOf GetById)},
                {NameOf(Delete), New Route(AddressOf Delete)},
                {NameOf(Update), New Route(AddressOf Update)}
            }
        End Sub

        Private ReadOnly _routes As Dictionary(Of String, Route)
        Public Overrides ReadOnly Property Routes As Dictionary(Of String, Route)
            Get
                Return _routes
            End Get
        End Property

    End Class

End Namespace
