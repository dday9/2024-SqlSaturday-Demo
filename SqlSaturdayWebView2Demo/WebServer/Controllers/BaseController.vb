Imports System.Text.Json

Namespace WebServer.Controllers

    Public MustInherit Class BaseController

        Public ReadOnly TableName As String

        Public Delegate Function Route(data As WebView2Request) As WebView2Response

        Public MustOverride ReadOnly Property Routes As Dictionary(Of String, Route)

        Public Sub New(tableName As String)
            Me.TableName = tableName
        End Sub

        Public Overridable Function Create(request As WebView2Request) As WebView2Response
            If (Not My.Application.Database.Tables.Contains(TableName)) Then
                Return WebView2Response.NotFound(TableName)
            End If

            Dim incoming As DataRow
            Dim table = My.Application.Database.Tables(TableName)
            Try
                incoming = JsonToDataRow(request.Body, table)
            Catch ex As Exception
                Return WebView2Response.BadRequest("Incoming data cannot be converted to a DataRow.")
            End Try

            Dim pk = table.PrimaryKey.FirstOrDefault()
            If (pk Is Nothing) Then
                Return WebView2Response.InternalServerError("The schema does not have a primary key.")
            End If

            incoming.Item(pk) = Guid.NewGuid()
            table.Rows.Add(incoming)
            table.AcceptChanges()

            Dim body = DataRowToJson(incoming)
            Return WebView2Response.Ok(body)
        End Function

        Public Overridable Function Delete(request As WebView2Request) As WebView2Response
            If (Not My.Application.Database.Tables.Contains(TableName)) Then
                Return WebView2Response.NotFound(TableName)
            End If

            Dim id = request.TryGetIdFromUrl()
            If (id Is Nothing) Then
                Dim errorMessage = $"No ID was passed to the URL: {request.Url}"
                Return WebView2Response.BadRequest(errorMessage)
            End If

            Dim table = My.Application.Database.Tables(TableName)
            Dim row = table.Rows.Find(id)
            If (row Is Nothing) Then
                Return WebView2Response.NotFound(id.ToString)
            End If

            table.Rows.Remove(row)
            table.AcceptChanges()

            Return WebView2Response.NoContent()
        End Function

        Public Overridable Function GetAll(request As WebView2Request) As WebView2Response
            If (Not My.Application.Database.Tables.Contains(TableName)) Then
                Return WebView2Response.NotFound(TableName)
            End If

            Dim table = My.Application.Database.Tables(TableName)
            Dim rows = table.Rows
            Dim body = DataRowsToJson(rows)
            Return WebView2Response.Ok(body)
        End Function

        Public Overridable Function GetById(request As WebView2Request) As WebView2Response
            If (Not My.Application.Database.Tables.Contains(TableName)) Then
                Return WebView2Response.NotFound(TableName)
            End If

            Dim id = request.TryGetIdFromUrl()
            If (id Is Nothing) Then
                Dim errorMessage = $"No ID was passed to the URL: {request.Url}"
                Return WebView2Response.BadRequest(errorMessage)
            End If

            Dim table = My.Application.Database.Tables(TableName)
            Dim row = table.Rows.Find(id)
            If (row Is Nothing) Then
                Return WebView2Response.NotFound(id.ToString)
            End If

            Dim body = DataRowToJson(row)
            Return WebView2Response.Ok(body)
        End Function

        Public Overridable Function Update(request As WebView2Request) As WebView2Response
            If (Not My.Application.Database.Tables.Contains(TableName)) Then
                Return WebView2Response.NotFound(TableName)
            End If

            Dim incoming As DataRow
            Dim table = My.Application.Database.Tables(TableName)
            Try
                incoming = JsonToDataRow(request.Body, table)
            Catch ex As Exception
                Return WebView2Response.BadRequest("Incoming data cannot be converted to a DataRow.")
            End Try

            Dim pk = table.PrimaryKey.FirstOrDefault()
            If (pk Is Nothing) Then
                Return WebView2Response.InternalServerError("The schema does not have a primary key.")
            End If

            Dim id = incoming.Item(pk)
            Dim existing = table.Rows.Find(id)
            If (existing Is Nothing) Then
                Return WebView2Response.NotFound($"Unable to find {TableName} by id: {id}")
            End If

            For Each column As DataColumn In table.Columns
                If (column.ReadOnly) Then
                    Continue For
                End If

                existing.Item(column) = incoming.Item(column)
            Next
            table.AcceptChanges()

            Dim body = DataRowToJson(existing)
            Return WebView2Response.Ok(body)
        End Function

        Public Shared Function JsonToDataRow(literal As String, table As DataTable) As DataRow
            Dim keyValuePairs = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(literal)
            Dim row As DataRow = table.NewRow()
            For Each kvp In keyValuePairs
                Dim value = kvp.Value
                Dim columnType = table.Columns(kvp.Key).DataType
                If (columnType = GetType(Guid)) Then
                    value = Guid.Parse(value.ToString())
                ElseIf (columnType = GetType(Integer)) Then
                    value = Convert.ToInt32(value.ToString())
                ElseIf (columnType = GetType(Decimal)) Then
                    value = Convert.ToDecimal(value.ToString())
                End If
                row(kvp.Key) = value
            Next
            Return row
        End Function

        Public Shared Function DataRowToJson(row As DataRow) As String
            Dim keyValuePairs = New Dictionary(Of String, Object)
            For Each column As DataColumn In row.Table.Columns
                keyValuePairs.Add(column.ColumnName, row(column))
            Next
            Dim json = JsonSerializer.Serialize(keyValuePairs)
            Return json
        End Function

        Private Shared Function DataRowsToJson(rows As DataRowCollection) As String
            Dim rowsAsDictionary = New List(Of Dictionary(Of String, Object))
            For Each row As DataRow In rows
                Dim keyValuePairs = New Dictionary(Of String, Object)
                For Each column As DataColumn In row.Table.Columns
                    keyValuePairs.Add(column.ColumnName, row(column))
                Next
                rowsAsDictionary.Add(keyValuePairs)
            Next
            Dim json = JsonSerializer.Serialize(rowsAsDictionary)
            Return json
        End Function

    End Class

End Namespace
