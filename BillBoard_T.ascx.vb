
Partial Class HomePage_BillBoard_T
    Inherits System.Web.UI.UserControl
 
    Private Sub BoundData(lv As ListView)
        For Each item In lv.Items
            Dim shortName As HyperLink = item.FindControl("hN")
            Dim lblChange As Label = item.FindControl("lC")
            If lblChange IsNot Nothing AndAlso shortName IsNot Nothing Then
                Dim change As Double = Val(lblChange.Text)

                If change > 0 Then
                    If change > 0.02 Then
                        lblChange.BackColor = Drawing.Color.FromArgb(220, 3, 0)
                        lblChange.ForeColor = Drawing.Color.White
                    Else
                        lblChange.ForeColor = Drawing.Color.FromArgb(220, 3, 0)
                    End If
                    lblChange.Text = "▲" + Format(change, "0.00%")

                ElseIf change < 0 Then
                    If change < -0.02 Then
                        lblChange.BackColor = Drawing.Color.Green
                        lblChange.ForeColor = Drawing.Color.White
                    Else
                        lblChange.ForeColor = Drawing.Color.Green
                    End If
                    lblChange.Text = "▼" + Format(change, "0.00%").Trim("-")
                Else
                    lblChange.Text = "0.00%"
                End If
            End If
        Next
    End Sub

    Private Sub GetClass(lv As ListView, isUp As Boolean)
        Dim key As String = "HomePage_BillBoard_T_" & lv.ID & isUp.ToString
        If Cache(key) Is Nothing Then
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
            connection.Open()

            Dim order As String = "Desc"
            If isUp = False Then order = "ASC"
            Dim cmdTxt As String = "SELECT TOP(10) [ShortName], [ClassId],[ChangeDay] FROM [Class] Where [Group] = 2 and classID<>39 Order By ChangeDay " & order
            Dim command As New System.Data.SqlClient.SqlCommand(cmdTxt, connection)
            command.ExecuteNonQuery()
            Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
            Dim dataSet As New Data.DataSet
            adapter.Fill(dataSet)

            connection.Close()
            connection.Dispose()

            Dim hour As Integer = Now.Hour
            If hour >= 8 AndAlso hour < 14 Then '台股 9:00~13:30
                Cache.Add(key, Me.GetTransDatatable(dataSet.Tables.Item(0)), Nothing, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, _
                               System.Web.Caching.CacheItemPriority.Normal, Nothing)
            Else
                Cache.Add(key, Me.GetTransDatatable(dataSet.Tables.Item(0)), Nothing, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, _
                                   System.Web.Caching.CacheItemPriority.Normal, Nothing)
            End If
        End If

        lv.DataSource = Cache(key)
        lv.DataBind()
        Me.BoundData(lv)
    End Sub

    ''' <summary>
    ''' 資料表轉置
    ''' 配合Listview的呈現方式
    ''' |0 1|         |0 3|
    ''' |2 3|  => |1 4| 
    ''' |4 5|         |2 5|
    ''' </summary>
    Private Function GetTransDatatable(oldDataTable As Data.DataTable) As Data.DataTable
        '新資料表先設定格式大小
        '舊資料表先塞入順序編號1,2,3,4,5......
        Dim newTable As New Data.DataTable
        Dim count As Integer = 1
        oldDataTable.Columns.Add("Index", GetType(Integer))
        For Each row As Data.DataRow In oldDataTable.Rows
            row("Index") = count

            If count = 1 Then
                For Each col As Data.DataColumn In row.Table.Columns
                    Dim newColumn As New Data.DataColumn(col.ColumnName)
                    newColumn.DataType = col.DataType
                    newTable.Columns.Add(newColumn)
                Next
            End If
            newTable.Rows.Add(newTable.NewRow())
            count += 1
        Next

        '資料表轉置
        Dim indexs() As Integer = {0, 5, 1, 6, 2, 7, 3, 8, 4, 9}
        Dim rowIndex As Integer = 0
        For Each index As Integer In indexs
            Dim newRow As Data.DataRow = newTable.Rows(rowIndex)

            For Each col As Data.DataColumn In newRow.Table.Columns
                newRow(col.ColumnName) = oldDataTable.Rows(index)(col.ColumnName)
            Next
            rowIndex += 1
        Next

        Return newTable
    End Function

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.GetClass(lv1_1, True)
        Me.GetClass(lv1_2, False)
    End Sub
End Class
