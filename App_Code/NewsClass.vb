Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections
Imports System.Web
Imports System.Web.Caching

Public Class NewsClass

    Public Shared str_Connstr_twstock As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString1").ToString()

    ''' <summary>
    ''' 取得自選股的相關新聞
    ''' </summary>
    ''' <param name="memberNo"></param>
    ''' <param name="groupNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function aboutNewsForHottipCollection(ByVal memberNo As Integer, ByVal groupNo As Integer) As String

        '最後回傳的相關新聞主體HTML
        Dim sbReturnData As New StringBuilder(500)
        ''要尋找的暫存機制(保留15分)
        ' Dim cacheAboutNews As New Cache
        '從資料庫或暫存取回的新聞
        Dim dt_StockNews As New DataTable

        '看看有沒有暫存的新聞內容
        Try

            dt_StockNews = DirectCast(System.Web.HttpContext.Current.Cache.Get("aboutnews_" + memberNo.ToString() + "_" + groupNo.ToString()), DataTable)
        Catch ex As Exception
            dt_StockNews = Nothing
        End Try

        '萬一沒有新聞 或是 取回的新聞低於五則
        '就重新取回新聞資料
        '再存入暫存
        If dt_StockNews Is Nothing OrElse dt_StockNews.Rows.Count < 5 Then
            Dim p(1) As SqlParameter

            Dim p0 As New SqlParameter("@memberno", SqlDbType.Int)
            p0.Direction = ParameterDirection.Input
            p0.Value = memberNo
            p(0) = p0

            Dim p1 As New SqlParameter("@groupno", SqlDbType.Int)
            p1.Direction = ParameterDirection.Input
            p1.Value = groupNo
            p(1) = p1

            dt_StockNews = KevinDataAccess.getSqlDataReturnDataTable(str_Connstr_twstock, _
                                                                "aboutNewsForHottipCollection", _
                                                                 CommandType.StoredProcedure, _
                                                                 p)

            System.Web.HttpContext.Current.Cache.Insert("aboutnews_" + memberNo.ToString() + "_" + groupNo.ToString(), dt_StockNews, Nothing, DateTime.UtcNow.AddMinutes(15), Cache.NoSlidingExpiration)
        End If

        '拼湊頁面的html
        sbReturnData.Append("<ul class='list-nws'>")

        If dt_StockNews.Rows.Count > 0 Then

            Dim int_tempstock1 As Integer = 0

            For i As Integer = 0 To dt_StockNews.Rows.Count - 1

                '<li><a href="#"><h2>台股盤後─摩台結算沒在怕！金蛇8600跳恰恰 拚封關連16年紅</h2><span class="nw-dt">02/05 20:35</span></a></li>

                sbReturnData.Append("<li>")

                If dt_StockNews.Columns.Count = 5 Then
                    '/stock/index.aspx?StockNo=0000
                    If Integer.TryParse(dt_StockNews.Rows(i)("stockno").ToString().Substring(0, 1), int_tempstock1) = True AndAlso dt_StockNews.Rows(i)("stockno").ToString() <> "0000" Then
                        sbReturnData.Append(" [<a target=""_blank"" href=""/stock/" + dt_StockNews.Rows(i)("stockno").ToString() + """ title=""" + dt_StockNews.Rows(i)("stockname").ToString() + """ >")
                        sbReturnData.Append("<h2>" + dt_StockNews.Rows(i)("stockname").ToString() + "</h2>")
                        'sbReturnData.Append("</a>] ")
                    Else
                        sbReturnData.Append(" [<a target=""_blank"" href=""/stock/index.aspx?StockNo=" + dt_StockNews.Rows(i)("stockno").ToString() + """ title=""" + dt_StockNews.Rows(i)("stockname").ToString() + """ >")
                        sbReturnData.Append("<h2>" + dt_StockNews.Rows(i)("stockname").ToString() + "</h2>")
                        'sbReturnData.Append("</a>] ")
                    End If
                End If

                sbReturnData.Append("<a target=""_blank"" href=""http://www.wantgoo.com/news/detail.aspx?id=" + dt_StockNews.Rows(i)("id").ToString() + "&c=" + System.Web.HttpUtility.UrlEncode("總覽") + """ title=""" + dt_StockNews.Rows(i)("Headline").ToString() + """>")
                sbReturnData.Append("<h2>" + dt_StockNews.Rows(i)("Headline").ToString() + "</h2>")
                'sbReturnData.Append("</a>")

                '<span class="nw-dt">02/05 20:35</span></a>
                sbReturnData.Append("<span class='nw-dt'>" + DirectCast(dt_StockNews.Rows(i)("Datetime"), DateTime).ToString("MM/dd HH:mm") + "</span>")
                sbReturnData.Append("</a>")

                sbReturnData.Append("</li>")
            Next
            
            sbReturnData.Append("<li>")
            sbReturnData.Append("<a target=""_blank"" href=""http://www.wantgoo.com/news"" title=""財經新聞首頁"">")
            sbReturnData.Append("<h2>瀏覽更多財經新聞.....</h2>")
            sbReturnData.Append("</a>")
            sbReturnData.Append("</li>")
        Else
            sbReturnData.Append("<li>")
            sbReturnData.Append("<a target=""_blank"" href=""http://www.wantgoo.com/news"" title=""財經新聞首頁"">")
            sbReturnData.Append("<h2>抱歉，目前可能沒有相關新聞，請前往財經新聞頁瀏覽相關新聞內容，謝謝。</h2>")
            sbReturnData.Append("</a>")
            sbReturnData.Append("</li>")
        End If

        sbReturnData.Append("</ul>")

        'Return "<div id=""aboutnews""><h2>相關新聞</h2>" + sbReturnData.ToString() + "</div>"
        Return sbReturnData.ToString()
    End Function


End Class
