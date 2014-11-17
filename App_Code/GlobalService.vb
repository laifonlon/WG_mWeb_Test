Imports System
Imports System.Web
Imports System.Collections
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.Odbc
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports Newtonsoft.Json

' 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://m.wantgoo.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class GlobalService
    Inherits System.Web.Services.WebService

    Private Function IsDeadline(ByVal key As String, ByVal second As Integer) As Boolean
        If Application(key & "_Deadline") Is Nothing Then
            Application(key & "_Deadline") = Now.ToString
            Return True
        End If

        Dim time As Date = Application(key & "_Deadline")

        If Now.Subtract(time).TotalSeconds > second Then
            Application(key & "_Deadline") = Now.ToString
            Return True
        End If

        Return False
    End Function

    <WebMethod(Description:="Get GlobalData")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function GetGlobalData(ByVal market As Integer) As String
        Dim result As String = ""

        If IsDeadline("GlobalApp_" & market.ToString, 5) = True Then
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
            connection.Open()
            Try
                Dim cmd As String = ""

                cmd = "SELECT Market, StockNo as No, Name, Deal, Change, [Open], High, Low, TotalVolume, CONVERT(VARCHAR(5) , Time , 108) as TT FROM Stock WHERE appGroup=" & market.ToString & " order by appIndex desc"

                Select Case market 'modes
                    Case 0 '總覽s
                        cmd = "SELECT Market , StockNo as No, Name, Deal, Change, [Open], High, Low, TotalVolume, CONVERT(VARCHAR(5) , Time , 108) as TT FROM Stock WHERE "
                        cmd &= "appGroup=1 or appGroup=2 or appGroup=3 or appGroup=4 or appGroup=6 or appGroup=7 or appGroup=9 or appGroup=10"
                        cmd &= " order by appIndex desc"
                    Case 1 '美洲
                    Case 2 '亞洲
                    Case 3 '歐洲
                    Case 4 '期貨
                    Case 5 '匯率

                    Case 6 '農產品
                    Case 7 '能源
                    Case 8 '美國重要個股
                    Case 9 '
                    Case 10 '
                End Select

                Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
                command.ExecuteNonQuery()
                Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
                Dim dataSet As New Data.DataSet
                adapter.Fill(dataSet)

                Application("GlobalApp_" & market.ToString) = JsonConvert.SerializeObject(dataSet, Formatting.Indented)
            Catch ex As Exception
                result = ex.ToString
            End Try
            connection.Close()
            connection.Dispose()
        End If
        result = Application("GlobalApp_" & market.ToString)

        Return result
    End Function

    <WebMethod(Description:="Get ExchangeRate")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function GetExchangeRate() As String
        Dim result As String = ""

        If IsDeadline("App_ExchangeRate", 60) = True Then
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
            connection.Open()
            Try
                Dim cmd As String = ""

                cmd = "SELECT Name as No, SubName as Name, CRIn, CROut, SERIn, SEROut, CONVERT(VARCHAR(5) , Time , 108) as TT FROM ExchangeRate order by Gindex"

                Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
                command.ExecuteNonQuery()
                Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
                Dim dataSet As New Data.DataSet
                adapter.Fill(dataSet)

                Application("App_ExchangeRate") = JsonConvert.SerializeObject(dataSet, Formatting.Indented)
            Catch ex As Exception
                result = ex.ToString
            End Try
            connection.Close()
            connection.Dispose()
        End If
        result = Application("App_ExchangeRate")

        Return result
    End Function

    <WebMethod(Description:="Get NotificationList")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function GetNotificationList() As String
        Dim result As String = ""

        If IsDeadline("App_NotificationList", 120) = True Then
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
            connection.Open()
            Try
                Dim cmd As String = ""

                cmd = "SELECT BlogID as NId,  ArticleTitle as Title, CONVERT(VARCHAR, PublishTime, 120 ) as TT, '2' as Link, '0' as IsNew FROM Blog Where MemberNo='59020' order by BlogID desc"

                Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
                command.ExecuteNonQuery()
                Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
                Dim dataSet As New Data.DataSet
                adapter.Fill(dataSet)

                For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
                    row("Link") = "http://www.wantgoo.com/phone/article.aspx?bd=" & row("NId").ToString
                Next

                Application("App_NotificationList") = JsonConvert.SerializeObject(dataSet, Formatting.Indented)
            Catch ex As Exception
                result = ex.ToString
            End Try
            connection.Close()
            connection.Dispose()
        End If
        result = Application("App_NotificationList")

        Return result
    End Function

    <WebMethod(Description:="Get StockNews")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function GetStockNews(ByVal stockNo As String) As String
        Dim result As String = ""

        If IsDeadline("App_" & stockNo & "_News", 120) = True Then
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
            connection.Open()
            Try
                Dim cmd As String = ""

                'cmd = "SELECT TOP(10) News.Time as TT , News.Title as Title, News.Text as Text , News.Link as Link, News.Effect,StockNews.NewsId,StockNews.StockNo FROM News INNER JOIN StockNews ON News.NewsId = StockNews.NewsId WHERE (StockNews.StockNo = @StockNo) ORDER BY News.Time DESC"

                cmd = "SELECT TOP(10) News.Time, 'ttttttttt' as TT, News.Title as Title, News.Text as Text , News.Link as Link FROM News INNER JOIN StockNews ON News.NewsId = StockNews.NewsId WHERE (StockNews.StockNo ='" + stockNo + "') ORDER BY News.Time DESC"

                Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
                command.ExecuteNonQuery()
                Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
                Dim dataSet As New Data.DataSet
                adapter.Fill(dataSet)

                For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
                    row("Text") = GetLimitText(row("Text"), 100).Replace(" ", "")

                    Dim PubDate As DateTime = CDate(row("Time").ToString)
                    Dim ts As TimeSpan = DateTime.Now.Subtract(PubDate)
                    If ts.Days > 0 And ts.Days <= 30 Then
                        row("TT") = ts.Days.ToString + "天前"
                    ElseIf ts.Days = 0 And ts.Hours > 0 Then
                        row("TT") = ts.Hours.ToString + "時前"
                    ElseIf ts.Hours = 0 And ts.Minutes > 0 Then
                        row("TT") = ts.Minutes.ToString + "分前"
                    ElseIf ts.Minutes = 0 And ts.Milliseconds > 0 Then
                        row("TT") = ts.Seconds.ToString + "秒前"
                    Else
                        row("TT") = Format(PubDate, "yyyy/MM/dd").ToString()
                    End If
                Next

                Application("App_" & stockNo & "_News") = JsonConvert.SerializeObject(dataSet, Formatting.Indented)
            Catch ex As Exception
                Application("App_" & stockNo & "_News") = ex.ToString
            End Try
            connection.Close()
            connection.Dispose()
        End If

        result = Application("App_" & stockNo & "_News")

        Return result
    End Function

    Private Function GetLimitText(ByVal oldText As String, ByVal maxLength As Integer) As String
        Dim strTitle As String = Regex.Replace(System.Web.HttpUtility.HtmlDecode(oldText), "<(.|\n)*?>", "").Replace(" ", "").Replace("&nbsp;", "")

        Dim textResult As String = ""
        Dim counter As Integer = 0
        Dim chr As Char
        Dim RealLen As Integer = System.Text.Encoding.GetEncoding("Big5").GetBytes(strTitle).Length
        For Each chr In strTitle
            counter += System.Text.Encoding.GetEncoding("Big5").GetBytes(chr.ToString()).Length
            If counter <= maxLength Then
                textResult = textResult + chr
            Else
                Exit For
            End If
        Next
        If RealLen > maxLength Then
            textResult += "..."
        End If
        Return textResult
    End Function

    <WebMethod(Description:="Get RealChartImage")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function GetRealChartImage(ByVal stockNo As String) As String
        Dim result As String = ""

        stockNo = stockNo.Replace("$", "&")

        If IsDeadline("App_" & stockNo & "_RealChart", 120) = True Then
            Try
                Dim imageUrl As String = ""
                If stockNo = "0000" Or Val(stockNo) > 0 Then
                    imageUrl = TwStockAccessor.Instance.GetRealTimeChart(stockNo, 320, 170) & "&chco=ffff00&chls=2&chf=bg,s,000000"
                Else
                    imageUrl = TwStockAccessor.Instance.GetRealTimeChartByGlobal(stockNo, 320, 170) & "&chco=ffff00&chls=2&chf=bg,s,000000"
                End If
                Application("App_" & stockNo & "_RealChart") = imageUrl
            Catch ex As Exception
                Application("App_" & stockNo & "_RealChart") = ex.ToString
            End Try
        End If

        result = Application("App_" & stockNo & "_RealChart")

        Return stockNo & "$D$" & result
    End Function

    <WebMethod(Description:="Get BookList")> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function GetBookList() As String
        Dim result As String = ""

        If IsDeadline("App" & "_Books", 120) = True Then
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
            connection.Open()

            Try
                Dim cmd As String = "SELECT TOP (50) 'ttttttttt' as TT, 'nnnnnnn' as Master, Blog.BlogID, Blog.MemberNo, Blog.ArticleTitle, Blog.PublishTime as Time, Blog.TopPic FROM Blog WHERE IsSell=0 and (IsTopIndex > 0 or IsFreePhone=1) Order By PublishTime Desc"

                Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
                command.ExecuteNonQuery()
                Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
                Dim dataSet As New Data.DataSet
                adapter.Fill(dataSet)

                For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
                    Try
                        row("Master") = MemberDataAccessor.Instance.GetMemberData(Val((row("MemberNo")))).NickName.Trim

                        Dim PubDate As DateTime = CDate(row("Time").ToString)
                        Dim ts As TimeSpan = DateTime.Now.Subtract(PubDate)
                        If ts.Days > 0 And ts.Days <= 30 Then
                            row("TT") = ts.Days.ToString + "天前"
                        ElseIf ts.Days = 0 And ts.Hours > 0 Then
                            row("TT") = ts.Hours.ToString + "時前"
                        ElseIf ts.Hours = 0 And ts.Minutes > 0 Then
                            row("TT") = ts.Minutes.ToString + "分前"
                        ElseIf ts.Minutes = 0 And ts.Milliseconds > 0 Then
                            row("TT") = ts.Seconds.ToString + "秒前"
                        Else
                            row("TT") = Format(PubDate, "yyyy/MM/dd").ToString()
                        End If
                    Catch ex As Exception

                    End Try
                Next

                Application("App" & "_Books") = JsonConvert.SerializeObject(dataSet, Formatting.Indented)
            Catch ex As Exception
                Application("App" & "_Books") = ex.ToString
            End Try

            connection.Dispose()
            connection.Close()
        End If

        result = Application("App" & "_Books")

        Return result
    End Function
End Class