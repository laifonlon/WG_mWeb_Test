 
Partial Class Stock_MainIndex
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadData()
    End Sub

    Private Sub LoadData()
        Dim key5Day As String = "Stock_MainIndex_5Day"
        Dim key20Day As String = "Stock_MainIndex_20Day"
        Dim key60Day As String = "Stock_MainIndex_60Day"

        If Cache(key5Day) Is Nothing OrElse Cache(key20Day) Is Nothing OrElse Cache(key60Day) Is Nothing Then
            Dim data5Day, data20Day, data60Day As String
            Me.LoadMainIndex(data5Day, data20Day, data60Day)
            Cache.Add(key5Day, data5Day, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, _
                                    System.Web.Caching.CacheItemPriority.Normal, Nothing)
            Cache.Add(key20Day, data20Day, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, _
                                System.Web.Caching.CacheItemPriority.Normal, Nothing)
            Cache.Add(key60Day, data60Day, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, _
                                System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If

        Me.lMain1.Text = Cache(key5Day)
        Me.lMain2.Text = Cache(key20Day)
        Me.lMain3.Text = Cache(key60Day)
    End Sub

    Private Sub LoadMainIndex(ByRef data5Day As String, ByRef data20Day As String, ByRef data60Day As String)
        Dim table5 As String = "<h4 class=""chart_t"">主要指數近一週績效表現</h4><ul class=""chartbg"">@TableText</ul>"
        Dim table20 As String = "<h4 class=""chart_t"">主要指數近一月績效表現</h4><ul class=""chartbg"">@TableText</ul>"
        Dim table60 As String = "<h4 class=""chart_t"">主要指數近一季績效表現</h4><ul class=""chartbg"">@TableText</ul>"
        Dim mainNames As New Generic.List(Of String)
        Dim main5Values, main20Values, main60Values As New Generic.List(Of Single)
        Me.LoadDataFromDB(mainNames, main5Values, main20Values, main60Values)

        Dim table5Text As String = ""
        Dim table20Text As String = ""
        Dim table60Text As String = ""
        Dim barRatio5 As Single = 0.1
        Dim barRatio20 As Single = 0.1
        Dim barRatio60 As Single = 0.3
        Dim maxWidth As Integer = 60
        For i As Integer = 0 To mainNames.Count - 1
            If (Math.Abs(main5Values(i)) / barRatio5) > maxWidth Then barRatio5 = Math.Abs(main5Values(i)) / maxWidth
            If (Math.Abs(main20Values(i)) / barRatio20) > maxWidth Then barRatio20 = Math.Abs(main20Values(i)) / maxWidth
            If (Math.Abs(main60Values(i)) / barRatio60) > maxWidth Then barRatio60 = Math.Abs(main60Values(i)) / maxWidth
        Next

        For i As Integer = 0 To mainNames.Count - 1
            table5Text &= Me.GetCssBar(mainNames(i), main5Values(i), i, barRatio5)
            table20Text &= Me.GetCssBar(mainNames(i), main20Values(i), i, barRatio20)
            table60Text &= Me.GetCssBar(mainNames(i), main60Values(i), i, barRatio60)
        Next

        data5Day = table5.Replace("@TableText", table5Text)
        data20Day = table20.Replace("@TableText", table20Text)
        data60Day = table60.Replace("@TableText", table60Text)
    End Sub

    Private Function GetCssBar(ByVal name As String, ByVal value As Single, ByVal index As Integer, ByVal barRatio As Single) As String
        Dim tableText As String = _
"<li><span class=""l"">@IndexLeftValue</span>" & _
"<em class=""itemhd"">@IndexName</em>" & _
"<span class=""r"">@IndexRightValue</span></li>"

        Dim indexValue As String = ""
        Dim bar As String = ""
        Dim width As Single = Math.Abs(value) / barRatio
        If value > 0 Then
            indexValue = "<span class=""cr@IndexCount"" style=""width:@IndexWidthpx;""></span>@IndexRatio%"
            bar &= tableText.Replace("@IndexLeftValue", "").Replace("@IndexRightValue", indexValue).Replace("@IndexName", name).Replace("@IndexWidth", width).Replace("@IndexRatio", value).Replace("@IndexCount", (index + 1).ToString)
        Else
            indexValue = "@IndexRatio%<span class=""cr@IndexCount"" style=""width:@IndexWidthpx;""></span>"
            bar &= tableText.Replace("@IndexLeftValue", indexValue).Replace("@IndexRightValue", "").Replace("@IndexName", name).Replace("@IndexWidth", width).Replace("@IndexRatio", value).Replace("@IndexCount", (index + 1).ToString)
        End If
        Return bar
    End Function

    Private Sub LoadDataFromDB(ByVal mainNames As Generic.List(Of String), ByVal main5Values As Generic.List(Of Single), ByVal main20Values As Generic.List(Of Single), ByVal main60Values As Generic.List(Of Single))
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Dim cmdTxt As String = "SELECT StockNo,Name,Change5Rate,Change20Rate,Change60Rate FROM Stock " & _
                "Where StockNo='0000' Or StockNo='DJI' Or StockNo='KOR' Or StockNo='NKI' Or StockNo='SHI' Or StockNo='HSI' Or StockNo='FTH' Or StockNo='DAX' "
        Dim command As New System.Data.SqlClient.SqlCommand(cmdTxt, connection)
        command.ExecuteNonQuery()
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)
        Dim tempNos, tempNames As New Generic.List(Of String)
        Dim tempMain5Values, tempMain20Values, tempMain60Values As New Generic.List(Of Single)
        For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
            tempNos.Add(row("StockNo").ToString.Trim)
            tempNames.Add(row("Name"))
            tempMain5Values.Add(Format(row("Change5Rate") * 100, "0.00"))
            tempMain20Values.Add(Format(row("Change20Rate") * 100, "0.00"))
            tempMain60Values.Add(Format(row("Change60Rate") * 100, "0.00"))
        Next

        Dim orderNos() As String = {"0000", "KOR", "NKI", "SHI", "HSI", "DJI", "DAX", "FTH"}
        For Each no As String In orderNos
            Dim index As Integer = tempNos.IndexOf(no)
            mainNames.Add(tempNames(index))
            main5Values.Add(tempMain5Values(index))
            main20Values.Add(tempMain20Values(index))
            main60Values.Add(tempMain60Values(index))
        Next

        connection.Close()
        connection.Dispose()
    End Sub
 
End Class
