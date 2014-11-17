Imports System.Data

Partial Class _index_s
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If MemberDataAccessor.Instance.IsAdiminstrator AndAlso MemberDataAccessor.Instance.GetMemberNo = "43602" Then Me.btnClear.Visible = True

        'Dim key As String = "HomePage_Headline"
        'If Cache(key) Is Nothing OrElse Val(Request("clear")) = 999 Then
        '    Cache.Add(key, Me.LoadHeadLine(), Nothing, Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration, _
        '                                System.Web.Caching.CacheItemPriority.Normal, Nothing)
        'End If

        If IsPostBack = False Then
            '精選文章
            lbGoodHeadLine.Text = GetGoodHeadLine()
            lbGoodArticle.Text = GetGoodArticle()
            lbGoodNews.Text = GetGoodNews()
            lbGoodTwStock.Text = GetGoodTwStock()
            '台股資訊
            lbRealTimeChart.Text = GetRealTimeChart()
            LoadDealData()
            '國際股市
            lbInterNationalStock.Text = GetInterNationalStock()
            '飆股搜尋
            '這塊因為不能是靜態頁面，所以改從Ajax取資料。
            'Hottip()
            '大盤即時力道
            '因排版關系，改從SqlDataSource抓資料x4
            'TwStockGroupingGetClass(lv1_1, True)
            'TwStockGroupingGetClass(lv1_2, False)
            '個股排行
            lbTwStockRank_Up.Text = TwStockRank("台股分類_個股漲")
            lbTwStockRank_Down.Text = TwStockRank("台股分類_個股跌")
            lbTwStockRank_Volumn.Text = TwStockRank("台股分類_個股量")
            '法人資券
            lbFinance_Up.Text = TwFinanceBearish("法人資券_融資增")
            lbFinance_Down.Text = TwFinanceBearish("法人資券_融資減")
            lbBearish_Up.Text = TwFinanceBearish("法人資券_融券增")
            lbBearish_Down.Text = TwFinanceBearish("法人資券_融券減")
            '法人買賣超
            lbTwLegalPerson_Foreign_Add.Text = TwLegalPerson("法人資券_外資買")
            lbTwLegalPerson_Foreign_Reduce.Text = TwLegalPerson("法人資券_外資賣")
            lbTwLegalPerson_ING_Add.Text = TwLegalPerson("法人資券_投信買")
            lbTwLegalPerson_ING_Reduce.Text = TwLegalPerson("法人資券_投信賣")
            lbTwLegalPerson_Dealer_Add.Text = TwLegalPerson("法人資券_自營買")
            lbTwLegalPerson_Dealer_Reduce.Text = TwLegalPerson("法人資券_自營賣")
            '績效統計
            PerformanceStatisticLoadData()

        End If

    End Sub

#Region "績效統計"

    Private Sub PerformanceStatisticLoadData()
        Dim key5Day As String = "Stock_MainIndex_5Day"
        Dim key20Day As String = "Stock_MainIndex_20Day"
        Dim key60Day As String = "Stock_MainIndex_60Day"

        If Cache(key5Day) Is Nothing OrElse Cache(key20Day) Is Nothing OrElse Cache(key60Day) Is Nothing Then
            Dim data5Day, data20Day, data60Day As String
            PerformanceStatisticLoadMainIndex(data5Day, data20Day, data60Day)
            Cache.Add(key5Day, data5Day, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, _
                                    System.Web.Caching.CacheItemPriority.Normal, Nothing)
            Cache.Add(key20Day, data20Day, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, _
                                System.Web.Caching.CacheItemPriority.Normal, Nothing)
            Cache.Add(key60Day, data60Day, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, _
                                System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If

        Me.lbPerformanceStatistic01.Text = Cache(key5Day)
        Me.lbPerformanceStatistic02.Text = Cache(key20Day)
        Me.lbPerformanceStatistic03.Text = Cache(key60Day)
    End Sub

    Private Sub PerformanceStatisticLoadMainIndex(ByRef data5Day As String, ByRef data20Day As String, ByRef data60Day As String)
        Dim table5 As String = "<h4 class=""chart_t"">主要指數近一週績效表現</h4><ul class=""chartbg"">@TableText</ul>"
        Dim table20 As String = "<h4 class=""chart_t"">主要指數近一月績效表現</h4><ul class=""chartbg"">@TableText</ul>"
        Dim table60 As String = "<h4 class=""chart_t"">主要指數近一季績效表現</h4><ul class=""chartbg"">@TableText</ul>"
        Dim mainNames As New Generic.List(Of String)
        Dim main5Values, main20Values, main60Values As New Generic.List(Of Single)
        PerformanceStatisticLoadDataFromDB(mainNames, main5Values, main20Values, main60Values)

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
            table5Text &= Me.PerformanceStatisticGetCssBar(mainNames(i), main5Values(i), i, barRatio5)
            table20Text &= Me.PerformanceStatisticGetCssBar(mainNames(i), main20Values(i), i, barRatio20)
            table60Text &= Me.PerformanceStatisticGetCssBar(mainNames(i), main60Values(i), i, barRatio60)
        Next

        data5Day = table5.Replace("@TableText", table5Text)
        data20Day = table20.Replace("@TableText", table20Text)
        data60Day = table60.Replace("@TableText", table60Text)
    End Sub

    Private Function PerformanceStatisticGetCssBar(ByVal name As String, ByVal value As Single, ByVal index As Integer, ByVal barRatio As Single) As String
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

    Private Sub PerformanceStatisticLoadDataFromDB(ByVal mainNames As Generic.List(Of String), ByVal main5Values As Generic.List(Of Single), ByVal main20Values As Generic.List(Of Single), ByVal main60Values As Generic.List(Of Single))
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

#End Region

#Region "法人買賣超"

    ''' <summary>
    ''' 法人買賣超
    ''' </summary>
    ''' <param name="TypeName">法人資券_外資買、法人資券_外資賣、法人資券_投信買、法人資券_投信賣、法人資券_自營買、法人資券_自營賣</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TwLegalPerson(TypeName As String) As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStr(TypeName, "")
            '"法人資券_外資買"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyForeign, TWTU.SellForeign, TWTU.SumTotal, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080'  AND Stock.Last > 0 AND (Stock.Market <=1) AND TWTU.Date = (select max(date) from TWTU) ORDER BY TWTU.SumTotal DESC
            '"法人資券_外資賣"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyForeign, TWTU.SellForeign, TWTU.SumTotal, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080'  AND Stock.Last > 0 AND (Stock.Market <=1) AND TWTU.Date = (select max(date) from TWTU) ORDER BY TWTU.SumTotal
            '"法人資券_投信買"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyING, TWTU.SellING, TWTU.SumTotal, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080' AND (Stock.Market <=1) AND TWTU.Date= (select max(date) from TWTU) ORDER BY TWTU.SumTotal DESC
            '"法人資券_投信賣"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyING, TWTU.SellING, TWTU.SumTotal, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080' AND (Stock.Market <=1) AND TWTU.Date= (select max(date) from TWTU) ORDER BY TWTU.SumTotal
            '"法人資券_自營買"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, TWTU.BuyDealer, TWTU.SellDealer, TWTU.SumTotal FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND Stock.Last > 0 AND (CAST(TWTU.Date AS date) = (SELECT MAX(Date) AS Expr1 FROM TWTU AS TWTU_1)) ORDER BY TWTU.SumTotal DESC
            '"法人資券_自營賣"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, TWTU.BuyDealer, TWTU.SellDealer, TWTU.SumTotal FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND Stock.Last > 0 AND (CAST(TWTU.Date AS date) = (SELECT MAX(Date) AS Expr1 FROM TWTU AS TWTU_1)) ORDER BY TWTU.SumTotal
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            Dim Name As String = String.Empty
            Dim StockNo As String = String.Empty
            Dim Deal As String = String.Empty
            Dim Change2 As String = String.Empty
            Dim SumTotal As String = String.Empty
            sbContent.Append("")

            Try
                While reader.Read

                    'Name, StockNo, Deal, ColorStyle,   Change2,  perc , TotalVolume

                    Name = reader.Item("Name")
                    StockNo = reader.Item("StockNo")
                    Deal = reader.Item("Deal")
                    Change2 = reader.Item("Change2")
                    SumTotal = reader.Item("SumTotal")

                    sbContent.Append("<tr>")
                    sbContent.Append("<td><a title='" + Name + "(" + StockNo + ") " + Deal + " " + Change2 + "' href='/stock.aspx?no=" + StockNo + "'>" + Name + "</a></td>")
                    sbContent.Append("<td>" + SumTotal + "</td>")
                    sbContent.Append("</tr>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception TwStockRank " + TypeName, "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

        Catch ex As Exception
            Debug.WriteDB("Exception TwStockRank " + TypeName, "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

#End Region

#Region "法人資券"

    ''' <summary>
    ''' 法人資券_融資、融券
    ''' </summary>
    ''' <param name="TypeName">法人資券_融資增、法人資券_融資減、法人資券_融券增、法人資券_融券減</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TwFinanceBearish(TypeName As String) As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStr(TypeName, "")
            '--"法人資券_融資增"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today1 - Finances.Before1) as Diff1, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff1 DESC
            '--"法人資券_融資減"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today1 - Finances.Before1) as Diff1, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff1
            '--"法人資券_融券增"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today2 - Finances.Before2) as Diff2, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff2 DESC
            '--"法人資券_融券減"
            '    SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today2 - Finances.Before2) as Diff2, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff2
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            Dim Name As String = String.Empty
            Dim StockNo As String = String.Empty
            Dim Deal As String = String.Empty
            Dim Change2 As String = String.Empty
            Dim Diff1 As String = String.Empty
            sbContent.Append("")

            Try
                While reader.Read

                    'Name, StockNo, Deal, ColorStyle,   Change2,  perc , TotalVolume

                    Name = reader.Item("Name")
                    StockNo = reader.Item("StockNo")
                    Deal = reader.Item("Deal")
                    Change2 = reader.Item("Change2")
                    Diff1 = reader.Item("Diff1")

                    sbContent.Append("<tr>")
                    sbContent.Append("<td><a title='" + Name + "(" + StockNo + ") " + Deal + " " + Change2 + "' href='/stock.aspx?no=" + StockNo + "'>" + Name + "</a></td>")
                    sbContent.Append("<td>" + Diff1 + "</td>")
                    sbContent.Append("</tr>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception TwStockRank " + TypeName, "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

        Catch ex As Exception
            Debug.WriteDB("Exception TwStockRank " + TypeName, "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

#End Region

#Region "個股排行"

    ''' <summary>
    ''' TwStockRank
    ''' </summary>
    ''' <param name="TypeName">"台股分類_個股漲"、"台股分類_個股跌"、"台股分類_個股量"</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TwStockRank(TypeName As String) As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStr(TypeName, "")
            '--"台股分類_個股漲"
            '    SELECT Top (5) Name, StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2,  case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume FROM Stock with (nolock) WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by  Change/ Last DESC
            '--"台股分類_個股跌"
            '    SELECT Top (5) Name, StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2,  case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume FROM Stock with (nolock) WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by  Change/ Last
            '--"台股分類_個股量"
            '    SELECT Top (5) Name, StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2,  case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume FROM Stock with (nolock) WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by  TotalVolume DESC
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            Dim Name As String = String.Empty
            Dim StockNo As String = String.Empty
            Dim Deal As String = String.Empty
            Dim ColorStyle As String = String.Empty
            Dim Change2 As String = String.Empty
            Dim perc As String = String.Empty
            Dim TotalVolume As String = String.Empty
            sbContent.Append("")

            Try
                While reader.Read

                    'Name, StockNo, Deal, ColorStyle,   Change2,  perc , TotalVolume

                    Name = reader.Item("Name")
                    StockNo = reader.Item("StockNo")
                    Deal = reader.Item("Deal")
                    ColorStyle = reader.Item("ColorStyle")
                    Change2 = reader.Item("Change2")
                    perc = reader.Item("perc")
                    TotalVolume = reader.Item("TotalVolume")

                    sbContent.Append("<tr>")
                    sbContent.Append("<td><a title='" + Name + "(" + StockNo + ")' href='/stock.aspx?no=" + StockNo + "'>" + Name + "</a></td>")
                    sbContent.Append("<td class='" + ColorStyle + "'>" + Deal + "</td>")
                    sbContent.Append("<td class='" + ColorStyle + "'>" + Change2 + "</td>")
                    sbContent.Append("<td class='" + ColorStyle + "'>" + perc + "</td>")
                    sbContent.Append("<td>" + TotalVolume + "</td>")
                    sbContent.Append("</tr>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception TwStockRank " + TypeName, "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

        Catch ex As Exception
            Debug.WriteDB("Exception TwStockRank " + TypeName, "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

#End Region

#Region "台股資訊"

    Private Sub LoadDealData()

        fvTwStock.Visible = True
        fvTwStock.DataSourceID = "sdsStock"

        Dim cStockNo As String = "0000"
        If Request("no") IsNot Nothing Then
            cStockNo = Request("no").Replace("$", "&").Trim()
        End If

        If cStockNo.Length < 10 Then
            sdsStock.SelectParameters("StockNo").DefaultValue = cStockNo
            DisplayColor()
        End If
    End Sub

    Protected Sub fvTwStock_DataBound(sender As Object, e As EventArgs) Handles fvTwStock.DataBound

        '預估收盤
        Dim em As IEnumerator = sdsStock.Select(New DataSourceSelectArguments).GetEnumerator
        '放假暫時拿掉預估量
        Dim es As IEnumerator = sdsVolumeEstimate.Select(New DataSourceSelectArguments).GetEnumerator
            If em.MoveNext Then
                If em.Current("StockNo") = "0000" Then
                    If es.MoveNext Then
                        Dim rate As Double = es.Current("Rate")
                        Dim openTime As New Date(Now.Year, Now.Month, Now.Day, 9, 0, 0)
                        Dim closeTime As New Date(Now.Year, Now.Month, Now.Day, 13, 30, 0)
                        '9:00~13:30
                    If (DateDiff(DateInterval.Minute, openTime, Now) >= 0) AndAlso (DateDiff(DateInterval.Minute, closeTime, Now) < 0) Then
                        lbVolumeEstimate.Text = " <br />(預估收盤)" + Format(em.Current("TotalVolume") * rate / 1000, "#,###,##0.0(億)")
                    End If
                    End If
                End If
            End If

    End Sub

    Private Sub DisplayColor()
        Dim view As FormView = Nothing
        view = fvTwStock
        Dim lblStockNo As Label = view.FindControl("lblStockNo")
        Dim lblDeal As Label = view.FindControl("lblDeal")
        Dim lblChange As Label = view.FindControl("lblChange")
        Dim lblHigh As Label = view.FindControl("lblHigh")
        Dim lblOpen As Label = view.FindControl("lblOpen")
        Dim lblTotalVolume As Label = view.FindControl("lblTotalVolume")
        Dim lblPercentage As Label = view.FindControl("lblPercentage")
        Dim lblLow As Label = view.FindControl("lblLow")
        Dim lblLast As Label = view.FindControl("lblLast")
        Dim lblTime As Label = view.FindControl("lblTime")
        Dim time As Date = CDate(lblTime.Text)
        lblTime.Text = time.Month & "/" & time.Day & " " & Format(time.Hour, "00") & ":" & Format(time.Minute, "00")

        Me.lUpdateTime.Text = lblTime.Text
        If IsNumeric(lblStockNo.Text) = False Then
            Me.lUpdateTime.Text &= "(當地時間)"
        End If

        Dim stockNo As String = lblStockNo.Text
        Dim dealPrice As Double = Val(lblDeal.Text)
        Dim differencePrice As Double = Val(lblChange.Text)
        Dim highPrice As Double = Val(lblHigh.Text)
        Dim lowPrice As Double = Val(lblLow.Text)
        Dim openPrice As Double = Val(lblOpen.Text)

        Dim last As Double = Val(lblLast.Text)
        Dim UpLimitePrice As Double = TwStockAccessor.Instance.GetUpLimitPrice(last)
        Dim DownLimitePrice As Double = TwStockAccessor.Instance.GetDownLimitPrice(last)

        lblDeal.Text = Format(dealPrice, "0.00")
        lblChange.Text = Format(differencePrice, "0.00")

        lblHigh.Text = Format(highPrice, "0.00")
        lblLow.Text = Format(lowPrice, "0.00")
        lblOpen.Text = Format(openPrice, "0.00")
        lblLast.Text = Format(last, "0.00")

        '最高價
        If highPrice > last Then
            lblHigh.ForeColor = Drawing.Color.Red
            If highPrice >= UpLimitePrice Then lblHigh.BackColor = Drawing.Color.FromArgb(255, 192, 192)
        ElseIf highPrice < last Then
            lblHigh.ForeColor = Drawing.Color.Green
            If highPrice <= DownLimitePrice Then lblHigh.BackColor = Drawing.Color.FromArgb(192, 255, 192)
        ElseIf highPrice = last Then
            lblHigh.ForeColor = Drawing.Color.Black
        End If
        If highPrice = 0 Then lblHigh.Text = "----"

        '最低價
        If lowPrice > last Then
            lblLow.ForeColor = Drawing.Color.Red
            If lowPrice >= UpLimitePrice Then lblLow.BackColor = Drawing.Color.FromArgb(255, 192, 192)
        ElseIf lowPrice < last Then
            lblLow.ForeColor = Drawing.Color.Green
            If lowPrice <= DownLimitePrice Then lblLow.BackColor = Drawing.Color.FromArgb(192, 255, 192)
        ElseIf lowPrice = last Then
            lblLow.ForeColor = Drawing.Color.Black
        End If
        If lowPrice = 0 Then lblLow.Text = "----"

        '開盤價
        If openPrice > last Then
            lblOpen.ForeColor = Drawing.Color.Red
            If openPrice >= UpLimitePrice Then lblOpen.BackColor = Drawing.Color.FromArgb(255, 192, 192)
        ElseIf openPrice < last Then
            lblOpen.ForeColor = Drawing.Color.Green
            If openPrice <= DownLimitePrice Then lblOpen.BackColor = Drawing.Color.FromArgb(192, 255, 192)
        ElseIf openPrice = last Then
            lblOpen.ForeColor = Drawing.Color.Black
        End If
        If openPrice = 0 Then lblOpen.Text = "----"

        If differencePrice > 0 Then
            lblChange.Text = "+" + lblChange.Text.Trim.Trim("+")
            lblDeal.ForeColor = Drawing.Color.Red
            lblChange.ForeColor = Drawing.Color.Red
            lblPercentage.ForeColor = Drawing.Color.Red
            If dealPrice >= UpLimitePrice Then
                lblDeal.BackColor = Drawing.Color.FromArgb(255, 192, 192)
                lblChange.BackColor = Drawing.Color.FromArgb(255, 192, 192)
                lblPercentage.BackColor = Drawing.Color.FromArgb(255, 192, 192)
            End If
        ElseIf differencePrice < 0 Then
            lblDeal.ForeColor = Drawing.Color.Green
            lblChange.ForeColor = Drawing.Color.Green
            lblPercentage.ForeColor = Drawing.Color.Green
            If dealPrice <= DownLimitePrice Then
                lblDeal.BackColor = Drawing.Color.FromArgb(192, 255, 192)
                lblChange.BackColor = Drawing.Color.FromArgb(192, 255, 192)
                lblPercentage.BackColor = Drawing.Color.FromArgb(192, 255, 192)
            End If

        ElseIf differencePrice = 0 Then
            lblDeal.ForeColor = Drawing.Color.Black
            lblChange.ForeColor = Drawing.Color.Black
            lblPercentage.ForeColor = Drawing.Color.Black

        End If

        '成交量
        If stockNo = "0000" Then
            lblTotalVolume.Text = Format((lblTotalVolume.Text) / 1000, "#,###,##0.0")
        ElseIf IsNumeric(stockNo) Then
            lblTotalVolume.Text = Format((lblTotalVolume.Text) / 1000, "#,###,##0")
        Else
            lblTotalVolume.Text = "--"
        End If
    End Sub

    Private Function GetRealTimeChart() As String

        Dim resultContent As String = String.Empty

        Dim stockno As String = "0000"
        If Request("no") IsNot Nothing Then
            stockno = Request("no").Replace("-", "").Replace("=", "").Trim
            stockno = stockno.Replace("$", "&")
        End If

        Dim txt As String = ""
        Dim imageUrl As String = ""
        If Val(stockno) > 0 Then
            imageUrl = TwStockAccessor.Instance.GetRealTimeChart(stockno, 350, 130)
        Else
            'imageUrl = TwStockAccessor.Instance.GetRealTimeChartByGlobal(stockno, 350, 130)
            imageUrl = TwStockAccessor.Instance.GetRealTimeChart(stockno, 350, 130)
        End If

        Try
            GetCurrentUrl(imageUrl)

            If imageUrl.Length < 100 Then
                'Exit Function
                Return resultContent
            End If
            resultContent = "<a href=""/TwIndex.aspx""><img src=""" + imageUrl + """ width=""300px""></a>"
        Catch ex As Exception
            Debug.WriteDB("Exception GetRealTimeChart", "", ex.Message, ex.StackTrace)
        End Try

        Return resultContent

    End Function

    Private Sub GetCurrentUrl(ByRef imageUrl As String)

        Dim dir As String = ""
        Dim filename As String = "C:\WantGoo\Wantgoooooo1\tmp\twgoogleindex.txt"
        If imageUrl.Length < 100 Then
            If IO.File.Exists(filename) = True Then
                imageUrl = My.Computer.FileSystem.ReadAllText(filename)
            End If
        Else
            If Now.Hour = 13 AndAlso (Now.Minute > 29 AndAlso Now.Minute < 32) Then
                My.Computer.FileSystem.WriteAllText(filename, imageUrl, False)
            End If
            'My.Computer.FileSystem.WriteAllText(filename, imageUrl, False)
        End If

    End Sub

#End Region

#Region "國際股市"

    Private Function GetInterNationalStock() As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStr("國際股市", "")
            'sqlcmd = "SELECT Top 7 [ID] ,[Datetime] ,[Category] ,substring(Headline,1,18) as ArticleTitle ,[Author] ,[IsHeadline] ,[Story] ,[Keywords],substring(Headline,1,40) as ArticleTitle2 From NowNews.dbo.cnYESNews with (nolock) where (Category like N'%台股%'  or Category like N'%台灣%') and headline not like N'%[表%]%' and headline not like N'%◆%' and headline not like N'%**%'  Order by id desc"
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            sbContent.Append("")
            sbContent.Append("<div id='wg-gobl' class='mblock'>")
            sbContent.Append("<h2 class='mbk-hds gdt'>國際股市</h2>")
            sbContent.Append("<div class='tb-fm tb-txtrt'>")
            sbContent.Append("<table cellspacing='0' cellpadding='0' class='bgline'>")
            sbContent.Append("<thead><tr><th>股市</th><th>股價</th><th>漲跌</th><th>比例</th></tr> </thead>")
            sbContent.Append("<tbody>")

            Dim colorStyle As String = "r"
            Dim change As Double = 0
            Try
                While reader.Read
                    '                <tr>
                    '                    <td><a href='http://www.wantgoo.com/stock/index.aspx?stockno=S2TWZ1'>摩台指</a></td>
                    '                    <td class='g'>296.9</td>
                    '                    <td class='g'>▼7.10</td>
                    '                    <td class='g'>-2.34%</td>
                    '                </tr>
                    change = Double.Parse(reader.Item("Change"))
                    If change > 0 Then
                        colorStyle = "r"
                    Else
                        colorStyle = "g"
                    End If
                    Dim StockNo As String = reader.Item("StockNo")
                    If StockNo = "WTX&" Then '特例：台指期的ID要改一個字
                        StockNo = "WTX$"
                    End If
                    sbContent.Append("")
                    sbContent.Append("<tr>")
                    sbContent.Append("<td><a href='/index.aspx?no=" + StockNo + "'>" + reader.Item("Name") + "</a></td>")
                    sbContent.Append("<td class='" + colorStyle + "'>" + reader.Item("Deal").ToString() + "</td>")
                    sbContent.Append("<td class='" + colorStyle + "'>" + reader.Item("Change2") + "</td>")
                    sbContent.Append("<td class='" + colorStyle + "'>" + reader.Item("perc") + "%</td>")
                    sbContent.Append("</tr>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception GetInterNationalStock", "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

            sbContent.Append("</tbody></table></div>")
            sbContent.Append("<div class='readmore'><a href='/Globals.aspx' class='btn-i'>看更多國際股市資訊 +</a></div>")
            sbContent.Append("</div>")

        Catch ex As Exception
            Debug.WriteDB("Exception GetInterNationalStock", "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

#End Region

#Region "精選文章"

    Private Function GetGoodTwStock() As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("NowNewsConnection").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStr("news台股", "")
            'sqlcmd = "SELECT Top 7 [ID] ,[Datetime] ,[Category] ,substring(Headline,1,18) as ArticleTitle ,[Author] ,[IsHeadline] ,[Story] ,[Keywords],substring(Headline,1,40) as ArticleTitle2 From NowNews.dbo.cnYESNews with (nolock) where (Category like N'%台股%'  or Category like N'%台灣%') and headline not like N'%[表%]%' and headline not like N'%◆%' and headline not like N'%**%'  Order by id desc"
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            sbContent.Append("")
            sbContent.Append("<div id='tabinner_4' style='display: none;'>")
            sbContent.Append("<ul class='list-nws'>")

            Try
                While reader.Read

                    sbContent.Append("")
                    sbContent.Append("<li><a href='")
                    sbContent.Append("http://www.wantgoo.com/news/detail.aspx?id=" + reader.Item("ID").ToString() + "&c=%e7%86%b1%e9%96%80%e6%96%b0%e8%81%9e")
                    sbContent.Append("'><h2>")
                    sbContent.Append(reader.Item("ArticleTitle"))
                    sbContent.Append("</h2>")
                    sbContent.Append("</a></li>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception GetGoodTwStock", "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

            sbContent.Append("</ul>")
            'sbContent.Append("<div class='readmore'><a href='/club.aspx' class='btn-i'>看更多玩股社團文章 +</a></div>")
            sbContent.Append("</div>")

        Catch ex As Exception
            Debug.WriteDB("Exception GetGoodTwStock", "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

    Private Function GetGoodNews() As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("NowNewsConnection").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStrArticle("news", "熱門")
            'sqlcmd = "SELECT TOP 7 [ID] ,[Datetime] ,[Category] ,substring(Headline,1,18) as ArticleTitle ,[Author] ,[IsHeadline] ,[Story] ,[Keywords],substring(Headline,1,40) as ArticleTitle2 FROM [cnYESNews] with (nolock) Where IsHeadLine = 1 and datetime > dateadd(day , -10,getdate())  Order By Datetime Desc "
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            sbContent.Append("")
            sbContent.Append("<div id='tabinner_3' style='display: none;'>")
            sbContent.Append("<ul class='list-nws'>")

            Try
                While reader.Read

                    sbContent.Append("")
                    sbContent.Append("<li><a href='")
                    sbContent.Append("http://www.wantgoo.com/news/detail.aspx?id=" + reader.Item("ID").ToString() + "&c=%e7%86%b1%e9%96%80%e6%96%b0%e8%81%9e")
                    sbContent.Append("'><h2>")
                    sbContent.Append(reader.Item("ArticleTitle"))
                    sbContent.Append("</h2>")
                    sbContent.Append("</a></li>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception GetGoodNews", "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

            sbContent.Append("</ul>")
            'sbContent.Append("<div class='readmore'><a href='/club.aspx' class='btn-i'>看更多玩股社團文章 +</a></div>")
            sbContent.Append("</div>")

        Catch ex As Exception
            Debug.WriteDB("Exception GetGoodNews", "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

    Private Function GetGoodArticle() As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStrArticle("今日最熱", "")
            'sqlcmd = "SELECT TOP(7) Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,20) as ArticleTitle FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND Blog.IsHide=0 AND Blog.PublishTime > DATEADD(d,-1,GETDATE())  And (Member.IshideBlog <> 1) ORDER BY Blog.ViewCount DESC"
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            sbContent.Append("")
            sbContent.Append("<div id='tabinner_2' style='display: none;'>")
            sbContent.Append("<ul class='list-nws'>")

            Try
                While reader.Read

                    sbContent.Append("")
                    sbContent.Append("<li><a href='")
                    sbContent.Append("/blog.aspx?bid=")
                    sbContent.Append(reader.Item("BlogID"))
                    sbContent.Append("'><h2>")
                    sbContent.Append(reader.Item("ArticleTitle"))
                    sbContent.Append("</h2>")
                    sbContent.Append("</a></li>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception GetGoodArticle", "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

            sbContent.Append("</ul>")
            sbContent.Append("<div class='readmore'><a href='/Article.aspx' class='btn-i'>看更多精選財經文章 +</a></div>")
            sbContent.Append("</div>")

        Catch ex As Exception
            Debug.WriteDB("Exception GetGoodArticle", "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

    Private Function GetGoodHeadLine() As String

        Dim resultContent As String = String.Empty
        Dim sbContent As StringBuilder = New StringBuilder()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Try
            Dim sqlcmd As String = doSqlcmdStr("玩股精選_NEW", "")
            'sqlcmd = "SELECT Top(7) BlogID,case when len(ArticleTitle) > 28 then left(ArticleTitle,28) + '...' else ArticleTitle End as ArticleTitle FROM WantGoo.dbo.Blog with (nolock) WHERE (IsCopy = 0) AND (Show = 1) AND (IsTopIndex > 0) AND (IsDelete = 0 or IsDelete is NuLL) ORDER BY IsTopIndex DESC,PublishTime DESC"
            Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            sbContent.Append("")
            sbContent.Append("<div id='tabinner_1' style='display: block;'>")
            sbContent.Append("<ul class='list-nws'>")

            Try
                While reader.Read

                    sbContent.Append("")
                    sbContent.Append("<li><a href='")
                    sbContent.Append("/blog.aspx?bid=")
                    sbContent.Append(reader.Item("BlogID"))
                    sbContent.Append("'><h2>")
                    sbContent.Append(reader.Item("ArticleTitle"))
                    sbContent.Append("</h2>")
                    sbContent.Append("</a></li>")

                End While
            Catch ex As Exception
                Debug.WriteDB("Exception GetHeadLine", "", ex.Message, ex.StackTrace)
            Finally
                reader.Close()
                command.Dispose()
            End Try

            sbContent.Append("</ul>")
            sbContent.Append("<div class='readmore'><a href='/Article.aspx' class='btn-i'>看更多頭條資訊 +</a></div>")
            sbContent.Append("</div>")

        Catch ex As Exception
            Debug.WriteDB("Exception GetHeadLine", "", ex.Message, ex.StackTrace)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        resultContent = sbContent.ToString()

        Return resultContent

    End Function

#End Region

#Region "SQL Commands"

    Function doSqlcmdStr(ByVal maintypes As String, ByVal detailtypes As String, Optional ByVal thirdtypes As String = "", Optional ByVal fouthtypes As String = "") As String

        Dim sqlcmd As String = ""
        If maintypes = "頭版頭條" Then
            sqlcmd = "SELECT [Index], Image, Url, Title, [Content], BlogID, MemberNo FROM WantGoo.dbo.HeadLine with (nolock) "
        ElseIf maintypes = "news台股" Then
            '最新台股新聞     排除公告 統計 表格
            sqlcmd = "SELECT Top 7 [ID] ,substring(Headline,1,18) as ArticleTitle From NowNews.dbo.cnYESNews with (nolock) where (Category like N'%台股%'  or Category like N'%台灣%') and headline not like N'%[表%]%' and headline not like N'%◆%' and headline not like N'%**%'  Order by id desc"
        ElseIf maintypes = "頭版頭條_NEW" Then
            sqlcmd = "SELECT [Index], Image, Url, Title, BlogID, MemberNo FROM WantGoo.dbo.HeadLine with (nolock) "
        ElseIf maintypes = "玩股精選_NEW" Then
            sqlcmd = "SELECT Top(7) BlogID,case when len(ArticleTitle) > 28 then left(ArticleTitle,28) + '...' else ArticleTitle End as ArticleTitle FROM WantGoo.dbo.Blog with (nolock) WHERE (IsCopy = 0) AND (Show = 1) AND (IsTopIndex > 0) AND (IsDelete = 0 or IsDelete is NuLL) ORDER BY IsTopIndex DESC,PublishTime DESC"
        ElseIf maintypes = "飆股搜尋" Then
            If detailtypes = "日期" Then
                sqlcmd = "SELECT convert(varchar(8),MAX(Date),112) AS LastDate FROM Selection1 with (nolock) where stockno = '0000' and datepart(dw , date) in (2,3,4,5,6)"
            ElseIf (detailtypes = "多" Or detailtypes = "空") Then
                Dim sql_1 As String = ""
                Dim sql_2 As String = ""
                If detailtypes = "多" Then
                    sql_1 = " and direction =1 "
                    If thirdtypes = "突破整理區間" Then
                        sql_2 = " and isThroughInterval = 1 "
                    ElseIf thirdtypes = "爆量長紅" Then
                        sql_2 = " and IsLargeVolume = 1 "
                    ElseIf thirdtypes = "突破季線" Then
                        sql_2 = " and IsThrough60Mean = 1 "
                    ElseIf thirdtypes = "KD黃金交叉" Then
                        sql_2 = " and IsKDCross = 1 "
                    ElseIf thirdtypes = "5與20日均線黃金交叉" Then
                        sql_2 = " and IsMeanCross = 1 "
                    End If
                ElseIf detailtypes = "空" Then
                    sql_1 = " and direction =0 "
                    If thirdtypes = "跌破整理區間" Then
                        sql_2 = " and isThroughInterval = 1 "
                    ElseIf thirdtypes = "爆量長黑" Then
                        sql_2 = " and IsLargeVolume = 1 "
                    ElseIf thirdtypes = "跌破季線" Then
                        sql_2 = " and IsThrough60Mean = 1 "
                    ElseIf thirdtypes = "KD死亡交叉" Then
                        sql_2 = " and IsKDCross = 1 "
                    ElseIf thirdtypes = "5與20日均線死亡交叉" Then
                        sql_2 = " and IsMeanCross = 1 "
                    End If
                End If
                sqlcmd = " Select Top 5 a.StockNo , b.Name , a.Date , Convert( Decimal(10,2),b.Deal) as Deal, Convert(Decimal(10,2),b.Change) as Change , '" + detailtypes + "' as Direction , '" + thirdtypes + "' as typename ,  b.Name  + ' (' + a.StockNo + ') ' + COnvert( varchar(10) , Convert( Decimal(10,2),b.Deal) ) + Case When b.Change > 0 then ' ▲' + COnvert( varchar(10) , Convert(Decimal(10,2),b.Change) ) When b.Change < 0 then ' ▼' + COnvert( varchar(10) , abs(Convert(Decimal(10,2),b.Change) ) )else '0.00' End as tip  From Selection1 a with (nolock)  "
                sqlcmd += " inner join Stock b with (nolock)  on a.StockNo = b.StockNo "
                sqlcmd += " and Date = '" + fouthtypes + "' "
                sqlcmd += sql_1
                sqlcmd += sql_2
                sqlcmd += " Order by a.StockNo "
            End If

        ElseIf maintypes = "玩股精選_1" Then
            sqlcmd = "SELECT Top(4) MemberNo,ArticleID ,TopPic,case when len(ArticleTitle) > 28 then left(ArticleTitle,28) + '...' else ArticleTitle End as ArticleTitle,Substring(Blog.ArticleText,1,2500) as ArticleText FROM WantGoo.dbo.Blog with (nolock) WHERE (IsCopy = 0) AND (Show = 1) AND (IsTopIndex > 0) AND (IsDelete = 0 or IsDelete is NuLL) ORDER BY IsTopIndex DESC,PublishTime DESC"
        ElseIf maintypes = "玩股精選_2" Then
            sqlcmd = "SELECT Top(10) MemberNo,ArticleID ,TopPic,case when len(ArticleTitle) > 14 then left(ArticleTitle,14) + '...' else ArticleTitle End as ArticleTitle FROM WantGoo.dbo.Blog with (nolock) WHERE (IsCopy = 0) AND (Show = 1) AND (IsTopIndex > 0) AND (IsDelete = 0 or IsDelete is NuLL) ORDER BY IsTopIndex DESC,PublishTime DESC"
        ElseIf maintypes = "台股分類_漲" Then
            sqlcmd = "SELECT TOP(10) [ShortName], [ClassId],case when ChangeDay > 0 then '▲' when ChangeDay < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay)))  as [ChangeDay2]  FROM [Class] with (nolock) Where [Group] = 2 and classID<>39 Order By ChangeDay desc "
        ElseIf maintypes = "台股分類_跌" Then
            sqlcmd = "SELECT TOP(10) [ShortName], [ClassId],case when ChangeDay > 0 then '▲' when ChangeDay < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay)))  as [ChangeDay2]  FROM [Class] with (nolock) Where [Group] = 2 and classID<>39 Order By ChangeDay  "
        ElseIf maintypes = "台股分類_個股漲" Then
            sqlcmd = "SELECT Top (5) Name, StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2,  case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume FROM Stock with (nolock) WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by  Change/ Last DESC"
        ElseIf maintypes = "台股分類_個股跌" Then
            sqlcmd = "SELECT Top (5) Name, StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2,  case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume FROM Stock with (nolock) WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by  Change/ Last"
        ElseIf maintypes = "台股分類_個股量" Then
            sqlcmd = "SELECT Top (5) Name, StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2,  case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume FROM Stock with (nolock) WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by  TotalVolume DESC"
        ElseIf maintypes = "美股分類_漲" Then
            sqlcmd = "SELECT TOP(10) [ShortName], [ClassId],case when ChangeDay > 0 then '▲' when ChangeDay < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay)))  as [ChangeDay2]  FROM [Class] with (nolock) Where [Group] = 7 and classID<>39 Order By ChangeDay desc "
        ElseIf maintypes = "美股分類_跌" Then
            sqlcmd = "SELECT TOP(10) [ShortName], [ClassId],case when ChangeDay > 0 then '▲' when ChangeDay < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay)))  as [ChangeDay2]  FROM [Class] with (nolock) Where [Group] = 7 and classID<>39 Order By ChangeDay  "
        ElseIf maintypes = "美股分類_個股漲" Then
            sqlcmd = "SELECT Distinct Top (5) Name, Stock.StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume ,Change/ Last FROM Stock with (nolock) INNER JOIN StockClass ON Stock.StockNo = StockClass.StockNo WHERE (StockClass.ClassId = '242') or (StockClass.ClassId = '243') or (StockClass.ClassId = '244') or (StockClass.ClassId = '245') or (StockClass.ClassId = '246') or (StockClass.ClassId = '247') or (StockClass.ClassId = '248') or (StockClass.ClassId = '249') or (StockClass.ClassId = '250') or (StockClass.ClassId = '251') or (StockClass.ClassId = '252') or (StockClass.ClassId = '253') or (StockClass.ClassId = '254')   Order by  Change/ Last DESC"
        ElseIf maintypes = "美股分類_個股跌" Then
            sqlcmd = "SELECT Distinct Top (5) Name, Stock.StockNo, Deal,  case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc ,TotalVolume ,Change/ Last FROM Stock with (nolock) INNER JOIN StockClass ON Stock.StockNo = StockClass.StockNo WHERE (StockClass.ClassId = '242') or (StockClass.ClassId = '243') or (StockClass.ClassId = '244') or (StockClass.ClassId = '245') or (StockClass.ClassId = '246') or (StockClass.ClassId = '247') or (StockClass.ClassId = '248') or (StockClass.ClassId = '249') or (StockClass.ClassId = '250') or (StockClass.ClassId = '251') or (StockClass.ClassId = '252') or (StockClass.ClassId = '253') or (StockClass.ClassId = '254')   Order by  Change/ Last"
        ElseIf maintypes = "法人資券_日期" Then
            sqlcmd = "SELECT Top 1 Time From Futures with (nolock) order by Time desc"
        ElseIf maintypes = "法人資券_外資買" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyForeign, TWTU.SellForeign, Cast(TWTU.SumForeign/1000 as int)[SumTotal], TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080'  AND Stock.Last > 0 AND (Stock.Market <=1) AND TWTU.Date = (select max(date) from TWTU) ORDER BY TWTU.SumForeign DESC"
        ElseIf maintypes = "法人資券_外資賣" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyForeign, TWTU.SellForeign, Cast(TWTU.SumForeign/1000 as int)[SumTotal], TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080'  AND Stock.Last > 0 AND (Stock.Market <=1) AND TWTU.Date = (select max(date) from TWTU) ORDER BY TWTU.SumForeign"
        ElseIf maintypes = "法人資券_投信買" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyING, TWTU.SellING, Cast(TWTU.SumING/1000 as int)[SumTotal], TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080' AND (Stock.Market <=1) AND TWTU.Date= (select max(date) from TWTU) ORDER BY TWTU.SumING DESC"
        ElseIf maintypes = "法人資券_投信賣" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.BuyING, TWTU.SellING, Cast(TWTU.SumING/1000 as int)[SumTotal], TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE Stock.StockNo <> '0000' AND Stock.StockNo <> '0081' AND Stock.StockNo <> '0080' AND (Stock.Market <=1) AND TWTU.Date= (select max(date) from TWTU) ORDER BY TWTU.SumING"
        ElseIf maintypes = "法人資券_自營買" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, TWTU.BuyDealer, TWTU.SellDealer, Cast(TWTU.SumDealer/1000 as int)[SumTotal] FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND Stock.Last > 0 AND (CAST(TWTU.Date AS date) = (SELECT MAX(Date) AS Expr1 FROM TWTU AS TWTU_1)) ORDER BY TWTU.SumDealer DESC "
        ElseIf maintypes = "法人資券_自營賣" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , TWTU.StockNo, TWTU.Date, TWTU.Name, Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, TWTU.BuyDealer, TWTU.SellDealer, Cast(TWTU.SumDealer/1000 as int)[SumTotal] FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND Stock.Last > 0 AND (CAST(TWTU.Date AS date) = (SELECT MAX(Date) AS Expr1 FROM TWTU AS TWTU_1)) ORDER BY TWTU.SumDealer"
        ElseIf maintypes = "法人資券_融資增" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today1 - Finances.Before1) as Diff1, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff1 DESC"
        ElseIf maintypes = "法人資券_融資減" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today1 - Finances.Before1) as Diff1, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff1"
        ElseIf maintypes = "法人資券_融券增" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today2 - Finances.Before2) as Diff1, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff1 DESC"
        ElseIf maintypes = "法人資券_融券減" Then
            sqlcmd = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT TOP (5) case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , Stock.Market, Stock.Deal, Stock.Change, Stock.Last, Stock.TotalVolume, Finances.StockNo, Stock.Name, Finances.Date, Finances.Before1, Finances.Today1, (Finances.Today2 - Finances.Before2) as Diff1, Finances.Before2, Finances.Today2 FROM Stock INNER JOIN Finances ON Stock.StockNo = Finances.StockNo WHERE (Stock.StockNo <> '0000') AND (Stock.StockNo <> '0081') AND (Stock.StockNo <> '0080') AND (Stock.Market <=1) AND cast(Finances.Date as date) = (select max(date) from Finances) ORDER BY Diff1"
        ElseIf maintypes = "多空力道" Then
            sqlcmd = "twmarketStockPushPull"
        ElseIf maintypes = "指數績效統計" Then
            sqlcmd = "marketIndexCompare"
        ElseIf maintypes = "休市預告" And detailtypes = "台灣" Then
            sqlcmd = "SELECT Name, Holiday, HolidayContent FROM GlobalClosedMarket WHERE (DATEPART(month, Holiday) = DATEPART(month, GETDATE())) AND (Number = 'TWSE')"
        ElseIf maintypes = "休市預告" And detailtypes = "全球" Then
            sqlcmd = "globalMarketCloseDay"
        ElseIf maintypes = "國際股市" Then
            sqlcmd = "SELECT ltrim(rtrim(StockNo)) as StockNo, Name, Deal, Change, Case when Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, Time, ShowTopOrder , case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2 , case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc FROM Stock WHERE stockNo='WTX&' or stockNo='S2TWZ1' or stockNo='DJI' or stockNo='NAS' or stockNo='NKI' or stockNo='KOR' or stockNo='SHI' or stockNo='HSI' or stockNo='FTH' or stockNo='CAC' Order by ShowTopOrder"
        ElseIf maintypes = "台股大盤線圖" Then
            sqlcmd = "HomePage_twChart_data"
        ElseIf maintypes = "台股大盤資料" Then
            sqlcmd = "SELECT Name, StockNo, Deal, Change, convert(decimal(10,2),TotalVolume/1000.0) as totalVol ,Last,DateDiff(Day,Time,getdate()) as Time , convert(decimal(10,2),TotalVolume/1000.0 * (Select Top 1 Rate From TradeVolumeEstimate Where [Time] >= Cast(GETDATE() as time(7)))) as maybeVol , case when Change > 0 then '▲' when Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Change ))) as Change2, case when Change > 0 then '+' when Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Change/ Last ))) as perc FROM Stock with (nolock) WHERE (StockNo = '0000')"
        ElseIf maintypes = "三大法人" Then
            sqlcmd = "SELECT Top 1 Date, StockNo,  convert(decimal(10,2),SumForeign/100000000.0) as SumForeign, convert(decimal(10,2),SumING/100000000.0) as SumING,  convert(decimal(10,2),SumDealer/100000000.0) as SumDealer FROM TWTU WHERE (StockNo = '0000') Order by Date Desc"
        ElseIf maintypes = "玩股專欄" Then
            sqlcmd = "SELECT Top (1) Blog.MemberNo, Blog.BlogID, Blog.ArticleID, substring(Blog.ArticleTitle,1,30) as ArticleTitle , Blog.ArticleText, Blog.ViewCount, Blog.PublishTime FROM WantGoo.dbo.Blog Where MemberNo=67867 ORDER BY Blog.PublishTime DESC"
        ElseIf maintypes = "理財課程" Then
            sqlcmd = "SELECT id,shortaddress,name,starttime,endtime,teacher,mode, IsSpecial, ActionUrl FROM WantGoo.dbo.Courses where isShow=1 and endtime > getdate() ORDER BY starttime"
        End If

        Return sqlcmd

    End Function

    Function doSqlcmdStrArticle(ByVal maintypes As String, ByVal detailtypes As String) As String

        Dim sqlcmd As String = ""

        If maintypes = "news" Then

            '左側新聞區塊

            Dim newsCount As Integer = 10
            Dim condition As String = ""

            If detailtypes = "陸股" Then
                detailtypes = "滬深股"
            End If

            If detailtypes.Contains("熱門") Then
                newsCount = 7
                condition = "IsHeadLine = 1 and datetime > dateadd(day , -10,getdate()) "
            Else
                condition = "IsHeadLine = 0 and Category like N'%" + detailtypes + "%' and datetime > dateadd(day , -14,getdate()) "
            End If
            sqlcmd = "SELECT TOP " + newsCount.ToString() + " [ID] ,[Datetime] ,[Category] ,substring(Headline,1,18) as ArticleTitle ,[Author] ,[IsHeadline] ,[Story] ,[Keywords],substring(Headline,1,40) as ArticleTitle2 FROM [cnYESNews] with (nolock) Where " + condition + " Order By Datetime Desc"

        ElseIf maintypes = "blognewpaper" Then
            '最新文章
            sqlcmd = " SELECT TOP(199) Member.blogname, Blog.FocusIndex as FID, Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,16) as ArticleTitle , Blog.PublishTime, substring(Member.NickName,1,5) as NickName FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND (Blog.Tag <> 800) And (Member.IshideBlog <> 1) And (Blog.IsHide = 0)  ORDER BY Blog.FocusIndex DESC, Blog.PublishTime DESC "
        ElseIf maintypes = "blogmoneypaper" Then
            '好評文章
            sqlcmd = " SELECT TOP(199) Member.blogname, Blog.FocusIndex as FID, Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,16) as ArticleTitle , Blog.PublishTime, substring(Member.NickName,1,5) as NickName FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND Blog.IsSell=1 AND Blog.Gold>0 AND (Blog.Tag <> 800) And (Member.IshideBlog <> 1) And (Blog.IsHide = 0) ORDER BY Blog.FocusIndex DESC, Blog.PublishTime DESC "
        ElseIf maintypes = "clubpaper" Then
            '社團文章
            sqlcmd = " SELECT TOP (225) [TopicID],[ClubID],[Text],[Time],[MemberNo],[IsHide],[IsTop],[Count],[LastReplyTime],[LastReplyMemberNo],[ClassId],[IsPublic] , b.bName , b.bFullName , c.cblogname FROM [Topic]  a with (nolock) inner join (select ClubID as bClubID, left(Name,5) as bName , Name as bFullName From [dbo].[Club] with (nolock)) b on a.ClubID  = b.bClubID inner join (select memberno as cmemberno, blogname as cblogname From wantgoo.dbo.member with (nolock) ) c on a.memberno = c.cmemberno where(IsHide = 0) and MemberNo<>'736' and b.bclubid not in ('11','16','19') order by Time desc "

        ElseIf maintypes = "今日最熱" Then
            'sqlcmd = "SELECT TOP(9) Member.blogname,Member.UploadPicAdd as Pic,Blog.ViewCount as VC, Blog.ReCommendCount as RC, Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,20) as ArticleTitle , Blog.PublishTime, Blog.ViewCount, Blog.RecommendCount, Blog.PublishTime, Blog.Show, substring(Member.NickName,1,5) as NickName ,substring(Blog.ArticleTitle,1,40) as ArticleTitle2 FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND Blog.IsHide=0 AND Blog.PublishTime > DATEADD(d,-1,GETDATE())  And (Member.IshideBlog <> 1) ORDER BY Blog.ViewCount DESC"
            sqlcmd = "SELECT TOP(7) Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,20) as ArticleTitle FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND Blog.IsHide=0 AND Blog.PublishTime > DATEADD(d,-1,GETDATE())  And (Member.IshideBlog <> 1) ORDER BY Blog.ViewCount DESC"
        ElseIf maintypes = "七日最熱" Then
            sqlcmd = "SELECT TOP(9) Member.blogname,Member.UploadPicAdd as Pic,Blog.ViewCount as VC, Blog.ReCommendCount as RC, Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,20) as ArticleTitle , Blog.PublishTime, Blog.ViewCount, Blog.RecommendCount, Blog.PublishTime, Blog.Show, substring(Member.NickName,1,5) as NickName FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND Blog.IsHide=0 AND Blog.PublishTime > DATEADD(d,-7,GETDATE())  And (Member.IshideBlog <> 1) ORDER BY Blog.ViewCount DESC"
        ElseIf maintypes = "頭版頭條" Then
            sqlcmd = "SELECT Blog.ViewCount as VC, Blog.ReCommendCount as RC, [Index], Image, Url, Title, HeadLine.BlogID, HeadLine.MemberNo as MN , c.nickname FROM HeadLine with (nolock) inner Join Blog with (nolock) On Blog.BlogID = HeadLine.BlogID inner join (select memberno as cmemberno , left(nickname,5) as nickname From wantgoo.dbo.member  with (nolock)) c on blog.memberno = c.cmemberno"
        ElseIf maintypes = "玩股精選" Then
            sqlcmd = "SELECT Top(10) Blog.ViewCount as VC, Blog.ReCommendCount as RC, BlogId,Good,Blog.Tag,BlogTagClass.BlogTagId,BlogTagClass.TagNoSplit,MemberNo as MN,TopPic,substring(Blog.ArticleTitle,1,18) as ArticleTitle,PublishTime,IsSell,IsSellShare,PreviewWordCount, ArticleID as AID , left(c.nickname,5) as nickname , c.blogname FROM Blog with (nolock) INNER JOIN BlogTagClass  with (nolock) ON Blog.Tag = BlogTagClass.BlogTagId inner join (select memberno as cmemberno , nickname,blogname From wantgoo.dbo.member with (nolock)) c on blog.memberno = c.cmemberno WHERE (IsCopy = 0) AND (Show = 1) AND (IsTopIndex > 0) AND (IsDelete = 0 or IsDelete is NuLL) ORDER BY IsTopIndex DESC,PublishTime DESC"
        ElseIf maintypes = "玩股專欄" Then
            sqlcmd = "SELECT Top 10 Blog.ViewCount as VC, Blog.ReCommendCount as RC,Blog.ArticleTitle as ArticleTitle, Blog.PublishTime, Blog.MemberNo as MN, Blog.ArticleID as AID, Member.UploadPicAdd as Pic, substring(Member.NickName,1,15) as NickName FROM Blog with (nolock) INNER JOIN Member with (nolock) ON Blog.MemberNo = Member.MemberNo WHERE IsColumn = 1 And (Blog.Show = 'True') AND (Blog.IsCopy = 0) ORDER BY PublishTime Desc"
        ElseIf maintypes = "人氣作家" Then
            sqlcmd = "SELECT Blog.ViewCount as VC, Blog.ReCommendCount as RC,Blog.ArticleTitle as ArticleTitle, Blog.PublishTime, Blog.MemberNo as MN, Blog.ArticleID as AID, BillBoard.BlogScore, Member_1.UploadPicAdd as Pic, substring(Member_1.NickName,1,15) as NickName FROM Blog with (nolock) INNER JOIN (SELECT MAX(PublishTime) AS Expr1, MemberNo FROM Blog AS Blog_1 with (nolock) WHERE PublishTime >= dateadd(day , -7 ,getdate()) and (MemberNo IN (SELECT TOP (10) MemberNo FROM BillBoard AS BillBoard_1 with (nolock) WHERE (BlogScore > 0) AND (MemberNo <> 119)  AND (Show = 1) AND (MemberNo NOT IN (SELECT MemberNo FROM Member with (nolock) WHERE (IsHideBlog = 1))) ORDER BY BlogScore Desc)) AND (IsCopy = 0) GROUP BY MemberNo) AS derivedtbl_1 ON Blog.PublishTime = derivedtbl_1.Expr1 AND Blog.MemberNo = derivedtbl_1.MemberNo INNER JOIN BillBoard ON Blog.MemberNo = BillBoard.MemberNo INNER JOIN Member AS Member_1 with (nolock) ON Blog.MemberNo = Member_1.MemberNo ORDER BY BillBoard.BlogRank"
        ElseIf maintypes = "優質作家" Then
            sqlcmd = "SELECT Blog.ViewCount as VC, Blog.ReCommendCount as RC,Blog.ArticleTitle as ArticleTitle, Blog.PublishTime, Blog.MemberNo as MN, Blog.ArticleID as AID, Member_1.UploadPicAdd as Pic, substring(Member_1.NickName,1,15) as NickName FROM Blog INNER JOIN (SELECT MAX(PublishTime) AS Expr1, MemberNo FROM Blog  AS Blog_1 with (nolock) WHERE PublishTime >= dateadd(day , -7 ,getdate()) and (MemberNo IN (SELECT TOP (10) MemberNo FROM Member  with (nolock) WHERE(IsHideBlog <> 1) ORDER BY Experience30Days DESC)) AND Show=1 AND (IsCopy = 0) GROUP BY MemberNo) AS derivedtbl_1 ON Blog.PublishTime = derivedtbl_1.Expr1 AND Blog.MemberNo = derivedtbl_1.MemberNo INNER JOIN Member AS Member_1  with (nolock) ON Blog.MemberNo = Member_1.MemberNo Order by Member_1.Experience30Days desc"
        End If

        Return sqlcmd

    End Function

#End Region

#Region "「飆股搜尋」"

    Private Sub Hottip()

        If Now.Hour < 9 Then
            sdsLastDate.SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 With(NoLock) Order by Date Desc) t Order by Date desc"
        End If

        Select Case MemberDataAccessor.Instance.GetMemberLevel
            Case MemberLevel.Rich, MemberLevel.Super
                sdsLastDate.SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 1 Date FROM Selection1 With(NoLock) Order by Date Desc) t Order by Date desc"
        End Select

        Dim result As IEnumerator
        If Now.DayOfWeek = DayOfWeek.Saturday OrElse _
            Now.DayOfWeek = DayOfWeek.Sunday OrElse _
           Val(Format(Now, "HH")) > 14 Then
            result = sdsNewestDate.Select(New DataSourceSelectArguments).GetEnumerator()
        Else
            result = sdsLastDate.Select(New DataSourceSelectArguments).GetEnumerator()
        End If

        If result.MoveNext() Then
            Dim row As DataRowView = result.Current
            lblHottipDate.Text = Format(row("LastDate"), "yyyy-MM-dd")
            Hottip_btnQuery_Click(Nothing, Nothing)
        End If

    End Sub

    Private HottipStockSelectionTemplate As String = _
    "<tr>" + _
    "    <td>@Rule</td>" + _
    "    <td>@Stocks</td>" + _
    "</tr>"

    Private mStockNameTemplate2 As String = _
    "<a href=""/stock.aspx?no=@No"">@Name</a>"
    Private mStockSelection As String = ""
    Private mStockSelection_2 As String = ""

    ''' <summary>
    ''' 加入系統選股
    ''' </summary>
    Private Sub HottipAddStockSelection(ByVal rule As String, ByVal direct As Direct, ByVal stockNames As Generic.List(Of String), ByVal stockNos As Generic.List(Of String))
        Dim content As String = HottipStockSelectionTemplate
        Dim stocks As String = ""
        Dim stockCount As Integer = stockNames.Count - 1

        '判斷是否在盤中
        Dim isTrading As Boolean
        If Now.DayOfWeek <> DayOfWeek.Saturday AndAlso _
        Now.DayOfWeek <> DayOfWeek.Sunday AndAlso _
        (Now.Hour > 9 AndAlso Now.Hour < 14 OrElse _
        (Now.Hour = 13 AndAlso Now.Minute < 30)) Then
            isTrading = True
        End If

        If stockCount >= 4 Then
            stockCount = 4
        End If

        For i As Integer = 0 To stockCount
            stocks = stocks + mStockNameTemplate2.Replace("@No", stockNos(i)).Replace("@Name", stockNames(i)) + ", "
        Next

        content = content.Replace("@Rule", rule)
        If stocks = "" Then
            content = content.Replace("@Stocks", "無滿足條件的股票")
        Else
            content = content.Replace("@Stocks", stocks.TrimEnd.TrimEnd(","))
        End If

        Select Case direct
            Case direct.Rise
                content = content.Replace("@Color", "8f0222")
            Case direct.Fall
                content = content.Replace("@Color", "008000")
        End Select

        mStockSelection = mStockSelection + content
    End Sub

    Private Sub HottipAddStockSelection_2(ByVal rule As String, ByVal direct As Direct, ByVal stockNames As Generic.List(Of String), ByVal stockNos As Generic.List(Of String))
        Dim content As String = HottipStockSelectionTemplate
        Dim stocks As String = ""
        Dim stockCount As Integer = stockNames.Count - 1

        '判斷是否在盤中
        Dim isTrading As Boolean
        If Now.DayOfWeek <> DayOfWeek.Saturday AndAlso _
        Now.DayOfWeek <> DayOfWeek.Sunday AndAlso _
        (Now.Hour > 9 AndAlso Now.Hour < 14 OrElse _
        (Now.Hour = 13 AndAlso Now.Minute < 30)) Then
            isTrading = True
        End If

        If stockCount >= 4 Then
            stockCount = 4
        End If

        For i As Integer = 0 To stockCount
            stocks = stocks + mStockNameTemplate2.Replace("@No", stockNos(i)).Replace("@Name", stockNames(i)) + ", "
        Next

        content = content.Replace("@Rule", rule)
        If stocks = "" Then
            content = content.Replace("@Stocks", "無滿足條件的股票")
        Else
            content = content.Replace("@Stocks", stocks.TrimEnd.TrimEnd(","))
        End If

        Select Case direct
            Case direct.Rise
                content = content.Replace("@Color", "DC0300")
            Case direct.Fall
                content = content.Replace("@Color", "008000")
        End Select

        mStockSelection_2 = mStockSelection_2 + content
    End Sub

    Protected Sub Hottip_btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        HottipQuery()

        '加入系統選股內容
        Dim stockNames As New Generic.List(Of String)
        Dim stockNos As New Generic.List(Of String)

        '多
        Dim rules As New Generic.List(Of SelectionRule)
        rules.Add(SelectionRule.ThroughInterval)
        rules.Add(SelectionRule.LargeVolume)
        rules.Add(SelectionRule.Through60Mean)
        rules.Add(SelectionRule.Swallow)
        rules.Add(SelectionRule.MeanCross)

        For Each rule As SelectionRule In rules
            stockNames.Clear()
            stockNos.Clear()
            For Each row As twStocks.Selection1Row In HottipGetSelection(rule, 1)
                stockNames.Add(row.Name)
                If row.StockNo < 100 Then
                    stockNos.Add(row.StockNo.ToString)
                Else
                    stockNos.Add(row.StockNo.ToString)
                End If
            Next

            HottipAddStockSelection(TwStockAccessor.Instance.GetRuleName_short(rule, Direct.Rise), Direct.Rise, stockNames, stockNos)
        Next

        'lblHottipGood.Text = mStockSelection

        For Each rule As SelectionRule In rules
            stockNames.Clear()
            stockNos.Clear()
            For Each row As twStocks.Selection1Row In HottipGetSelection(rule, 0)
                stockNames.Add(row.Name)
                If row.StockNo < 100 Then
                    stockNos.Add(row.StockNo.ToString)
                Else
                    stockNos.Add(row.StockNo.ToString)
                End If
            Next
            HottipAddStockSelection_2(TwStockAccessor.Instance.GetRuleName_short(rule, Direct.Fall), Direct.Fall, stockNames, stockNos)
        Next

        'lblHottipBad.Text = mStockSelection_2
    End Sub

    Private HottipSelectionTable As twStocks.Selection1DataTable
    Private Sub HottipQuery()
        HottipSelectionTable = TwStockAccessor.Instance.Query("#" + lblHottipDate.Text + "#")
    End Sub

    Public Function HottipGetSelection(ByVal rule As SelectionRule, ByVal direction As Integer) As twStocks.Selection1Row()
        Return HottipSelectionTable.Select("[Is" + [Enum].GetName(GetType(SelectionRule), rule) + "] = 1 And [Direction] = " + direction.ToString)
    End Function

#End Region

#Region "大盤即時力道"

    Protected Sub fvGeneralStock_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles fvGeneralStock.DataBound
        Dim d1 As Label = fvGeneralStock.FindControl("d1")
        Dim d2 As Label = fvGeneralStock.FindControl("d2")
        Dim a3 As Label = fvGeneralStock.FindControl("a3")
        Dim a5 As Label = fvGeneralStock.FindControl("a5")
        Dim diffK As Label = fvGeneralStock.FindControl("diffK")

        Dim d1v As Double = Val(d1.Text.Trim("%"))
        Dim d2v As Double = Val(d2.Text.Trim("%"))
        Dim a3v As Double = Val(a3.Text.Trim("%"))
        Dim a5v As Double = Val(a5.Text.Trim("%"))
        Dim diffKv As Double = Val(diffK.Text)

        If d1v >= 50 Then
            d1.ForeColor = Drawing.Color.Red
        Else
            d1.ForeColor = Drawing.Color.Green
        End If

        If d2v >= 50 Then
            d2.ForeColor = Drawing.Color.Red
        Else
            d2.ForeColor = Drawing.Color.Green
        End If

        If a3v >= 50 Then
            a3.ForeColor = Drawing.Color.Red
        Else
            a3.ForeColor = Drawing.Color.Green
        End If

        If a5v >= 50 Then
            a5.ForeColor = Drawing.Color.Red
        Else
            a5.ForeColor = Drawing.Color.Green
        End If

    End Sub

#End Region

#Region "台股分類"

    Private Sub TwStockGroupingBoundData(lv As ListView)
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

    Private Sub TwStockGroupingGetClass(lv As ListView, isUp As Boolean)
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
                Cache.Add(key, Me.TwStockGroupingGetTransDatatable(dataSet.Tables.Item(0)), Nothing, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, _
                               System.Web.Caching.CacheItemPriority.Normal, Nothing)
            Else
                Cache.Add(key, Me.TwStockGroupingGetTransDatatable(dataSet.Tables.Item(0)), Nothing, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, _
                                   System.Web.Caching.CacheItemPriority.Normal, Nothing)
            End If
        End If

        lv.DataSource = Cache(key)
        lv.DataBind()
        Me.TwStockGroupingBoundData(lv)
    End Sub

    ''' <summary>
    ''' 資料表轉置
    ''' 配合Listview的呈現方式
    ''' |0 1|         |0 3|
    ''' |2 3|  => |1 4| 
    ''' |4 5|         |2 5|
    ''' </summary>
    Private Function TwStockGroupingGetTransDatatable(oldDataTable As Data.DataTable) As Data.DataTable
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

#End Region


End Class
