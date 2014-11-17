Imports System.Data

Partial Class Hottip_HottipResult
    Inherits System.Web.UI.Page

    Dim mWatch As New System.Diagnostics.Stopwatch

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not MemberDataAccessor.Instance.IsLogin Then
            Response.Redirect("/Login.aspx?GoBackUrl=" + Server.UrlEncode(Request.RawUrl))
            'Throw New Exception("會員未登入")
        End If

        Dim Type As String = Request("Type")
        Dim RuleId As String = Request("RuleId")
        Dim RuleName As String = Request("RuleName")
        Dim Direction As String = Request("direction")
        Dim RadioDate As String = Request("rd")
        Dim CommandName As String = Request("CommandName")

        ltRuleName.Text = RuleName
        ltDirection.Text = Direction
        ltRadioDate.Text = RadioDate

        If String.IsNullOrEmpty(RuleId) AndAlso String.IsNullOrEmpty(Type) AndAlso String.IsNullOrEmpty(RuleName) AndAlso String.IsNullOrEmpty(Direction) AndAlso String.IsNullOrEmpty(RadioDate) Then
            Throw New Exception("有參數值缺少")
        Else

            If Type = "MyRule" Then
                lvMyRule_RowCommand(CommandName, RuleId, RuleName)
            End If

            If Type = "MySubscribe" Then
                lvMySubscribe_ItemCommand(CommandName, RuleId, RuleName)
            End If

        End If

        '最下方廣告 是否顯示
        Mob_300x2501.Visible = Member.Instance.IsShowAd()

    End Sub

#Region "我設定的選股工具 - 搜尋"

    Protected Sub lvMyRule_RowCommand(CommandName As String, RuleId As String, RuleName As String)

        If Not MemberDataAccessor.Instance.IsLogin OrElse MemberDataAccessor.Instance.GetMemberNo = "0" Then Exit Sub

        Dim anchor As String = "#myrule"
        Dim controlName As String = "btnName"
        'Select e.CommandName
        Select Case CommandName
            Case "search"
            Case "update", "ask"
                controlName = "btnUpdate"
        End Select

        If RuleId Is Nothing Then Exit Sub

        'Select Case e.CommandName
        Select Case CommandName

            Case "search"
                anchor = "#rule"
                If MemberDataAccessor.Instance.IsAdiminstrator AndAlso _
                    MemberDataAccessor.Instance.MemberNo IsNot Nothing Then
                    'Request("MemberNo") IsNot Nothing 
                    anchor = "?memberno=" + MemberDataAccessor.Instance.MemberNo + anchor
                    'anchor = "?memberno=" + Request("MemberNo") + anchor
                End If

                Dim sql As String = ""
                Dim rule As String = ""

                'sdsRule.SelectParameters("RuleId").DefaultValue = lblRuleId.Text
                sdsRule.SelectParameters("RuleId").DefaultValue = RuleId
                Dim result As IEnumerator = sdsRule.Select(New DataSourceSelectArguments).GetEnumerator
                If result.MoveNext() Then
                    Dim row As DataRowView = result.Current
                    sql = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;"
                    sql += row("SQL")
                    rule = row("Rule")
                End If
                If sql Is Nothing OrElse sql.Trim = "" Then Exit Sub

                SetDatePeriod(sql) '日期區間,
                sql = ReSQL(sql) '修正SQL 顯示資料行
                DataColumn(sql) '修正查詢資料列
                JoinTable(sql) '修正JoinTable

                mWatch.Reset()
                mWatch.Start()

                MemberDataAccessor.Instance.Action = sql
                lblQuery.Text = sql
                sdsCollect.SelectCommand = sql
                lvMyRule.DataBind()
                Session("Search_Log") = RuleName

            Case "update"

            Case "ask"

        End Select

    End Sub


#End Region

#Region "訂閱的選股工具 - 搜尋"

    Protected Sub lvMySubscribe_ItemCommand(CommandName As String, RuleId As String, RuleName As String)

        With MemberDataAccessor.Instance
            If (Not .IsLogin OrElse .GetMemberNo = "0") Then
                Exit Sub
            End If
        End With

        Dim sql As String = ""
        Dim rule As String = ""

        sdsRule.SelectParameters("RuleId").DefaultValue = RuleId
        Dim result As IEnumerator = sdsRule.Select(New DataSourceSelectArguments).GetEnumerator
        If result.MoveNext() Then
            Dim row As DataRowView = result.Current
            sql = row("SQL")
            rule = row("Rule")
        End If
        If sql Is Nothing OrElse sql.Trim = "" Then Exit Sub

        SetDatePeriod(sql) '修正SQL 日期區間
        sql = ReSQL(sql) '修正顯示資料行(舊版)
        DataColumn(sql) '修正查詢資料列
        JoinTable(sql) '修正JoinTable

        mWatch.Reset()
        mWatch.Start()

        MemberDataAccessor.Instance.Action = sql
        lblQuery.Text = sql
        sdsCollect.SelectCommand = sql
        lvMyRule.DataBind()

        Session("Search_Log") = RuleName

    End Sub

    Private mAutoExpandScript As String = "AutoExpand('KD');AutoExpand('除權息');AutoExpand('均線');"

#End Region


#Region "組合SQL查詢字串"

    ''' <summary>
    ''' 宣告SQL資料行
    ''' </summary>
    Private Sub DataColumn(ByRef query As String)

        Dim direction As String = Request("direction")
        Dim tmpColume As String = ""
        If direction = "多頭" Then
            tmpColume = " where direction=1 and [date] = @EndDate and stockno = stock.stockno "
        ElseIf direction = "空頭" Then
            tmpColume = " where direction=0 and [date] = @EndDate and stockno = stock.stockno "
        End If
        Dim column As String = _
        "Stock.Name, Stock.StockNo, Stock.Deal, Stock.Change, " + _
        "Stock.Change / (Stock.Deal - Stock.Change) as ChangeRate, " + _
        "ABS(Stock.High-Stock.Low) / (Stock.Deal - Stock.Change) as AmplitudeRate, " + _
        "case when Stock.Change > 0 then '▲' when Stock.Change < 0 then '▼' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),Stock.Change ))) as Change2, " + _
        "case when Stock.Change > 0 then 'r' when Stock.Change < 0 then 'g' else '' End as ColorStyle, " + _
        "case when Stock.Change > 0 then '+' when Stock.Change < 0 then '-' else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *Stock.Change/ Stock.Last ))) as perc," + _
        "Stock.TotalVolume, " + _
        "Stock.Mean5Volume, " + _
        "Stock.Mean20Volume, " + _
        "Stock.NPTT, " + _
        "Stock.Mean60DistanceRate, " + _
        "Stock.Mean20DistanceRate, " + _
        "Stock.EPS, " + _
        "Stock.RankDuration, " + _
        "(Stock.Deal - Stock.Last5DPrice)/Stock.Last5DPrice as WeekChange, " + _
        "Fundamentals.OperatingMargin, " + _
        "Fundamentals.ProfitMargin, " + _
        "Fundamentals.CashRate, " + _
        "Fundamentals.TaxRate, " + _
        "cast(replace(replace(CompanyProfile.PaidinCapital,',',''),N'元','') as float )/100000000 as 資本額, " + _
        "cast(replace(replace(CompanyProfile.TDR,',',''),N'股','') as float )*Stock.deal/100000000 as 市值, " + _
        "Fundamentals.MonthIncreaseVsLastMonth as 月營收vs上月, " + _
        "Fundamentals.MonthIncreaseVsLastYear as 月營收vs去年, " + _
        "Fundamentals.TotalIncreaseVsLastYear as 累計營收vs去年, " + _
        "Fundamentals.EpsSeason as EPS季, " + _
        "Fundamentals.Eps as EPS年, " + _
        "Stock.TotalDSH/NULLIF(cast(replace(replace(CompanyProfile.TDR,',',''),N'股','') as float ), 0) as 董監持股, " + _
        "Fundamentals.ForeignStockRatio as 外資持股, " + _
        "Fundamentals.FocusRatioNoForeign as 大戶持股, " + _
        "Fundamentals.FocusRatio as 籌碼集中, " + _
        "(select risepercent From twstocks.dbo.selection1 " + tmpColume + " ) as risepercent  "

        Dim startPos As Integer = query.IndexOf("SELECT Distinct") + 16
        Dim endPos As Integer = query.IndexOf("FROM")
        If startPos > 1 AndAlso endPos > startPos Then
            query = query.Remove(startPos, endPos - startPos)
            query = query.Insert(startPos, column)
        End If

    End Sub

    ''' <summary>
    ''' 宣告Join資料表
    ''' </summary>
    Private Sub JoinTable(ByRef query As String)

        '基本面
        If Not query.Contains("JOIN Fundamentals") Then
            query = query.Replace("Selection1.StockNo = Stock.StockNo", "Selection1.StockNo = Stock.StockNo Left OUTER JOIN Fundamentals ON Stock.StockNo = Fundamentals.StockNo ")
        End If

        '基本資料
        If Not query.Contains("JOIN CompanyProfile") Then
            query = query.Replace("Selection1.StockNo = Stock.StockNo", "Selection1.StockNo = Stock.StockNo Left OUTER JOIN CompanyProfile ON Stock.StockNo = CompanyProfile.StockNo ")
        End If

    End Sub

    '移除六日的選項
    Public Function getDateWeekNot6_7(ByVal todayOrYesterday As String) As Date
        todayOrYesterday = todayOrYesterday.ToLower()

        If todayOrYesterday = "today" Then
            If DateTime.Now.DayOfWeek = DayOfWeek.Sunday Then
                Return DateTime.Now.AddDays(-2)
            ElseIf DateTime.Now.DayOfWeek = DayOfWeek.Saturday Then
                Return DateTime.Now.AddDays(-1)
            Else


                If DateTime.Now.DayOfWeek = DayOfWeek.Monday And DateTime.Now.Hour < 9 Then
                    Return DateTime.Now.AddDays(-3)
                ElseIf DateTime.Now.Hour < 9 Then
                    Return DateTime.Now.AddDays(-1)
                Else
                    Return DateTime.Now
                End If

            End If
        End If

        If todayOrYesterday = "yesterday" Then
            If DateTime.Now.DayOfWeek = DayOfWeek.Sunday Then
                Return DateTime.Now.AddDays(-3)
            ElseIf DateTime.Now.DayOfWeek = DayOfWeek.Saturday Then
                Return DateTime.Now.AddDays(-2)
            ElseIf DateTime.Now.DayOfWeek = DayOfWeek.Monday And DateTime.Now.Hour < 9 Then
                Return DateTime.Now.AddDays(-4)
            ElseIf DateTime.Now.DayOfWeek = DayOfWeek.Monday Then
                Return DateTime.Now.AddDays(-3)
            Else

                If DateTime.Now.DayOfWeek = DayOfWeek.Tuesday And DateTime.Now.Hour < 9 Then
                    Return DateTime.Now.AddDays(-4)
                ElseIf DateTime.Now.Hour < 9 Then
                    Return DateTime.Now.AddDays(-2)
                Else
                    Return DateTime.Now.AddDays(-1)
                End If
            End If
        End If

        Return DateTime.Now

    End Function

    ''' <summary>
    ''' 設定SQL日期區間 
    ''' </summary>
    Private Sub SetDatePeriod(ByRef sql As String)

        Dim selectedDate As String = String.Empty
        Dim startDate As String = Request("sd")
        Dim endDate As String = Request("ed")

        If Request("rd") IsNot Nothing And Request("rd") <> "" Then
            selectedDate = Request("rd")
        End If

        Dim today As String = Format(getDateWeekNot6_7("today"), "yyyy/MM/dd")
        Dim yestoday As String = Format(getDateWeekNot6_7("yesterday"), "yyyy/MM/dd")
        Dim lastWeek As String = Format(getDateWeekNot6_7("today").AddDays(-7), "yyyy/MM/dd")
        Dim en As IEnumerator = sdsDate.Select(New DataSourceSelectArguments).GetEnumerator

        If selectedDate = "今日" Then
            startDate = today
            endDate = today
            'lblDate.InnerHtml = "今日"
        ElseIf selectedDate = "昨日" Then
            startDate = yestoday
            endDate = yestoday
            'lblDate.InnerHtml = "昨日"
        ElseIf selectedDate = "近一周" Then
            startDate = lastWeek
            endDate = today
            'lblDate.InnerHtml = "近一周"
        Else
            'lblDate.InnerHtml = startDate.Replace("2013/", "") + " ~ " + endDate.Replace("2013/", "")
        End If

        '日期區間
        Dim pos As Integer = sql.IndexOf(">= '20")
        If pos > 1 Then
            sql = sql.Remove(pos + 4, 10)
            sql = sql.Insert(pos + 4, startDate)
        End If
        pos = sql.IndexOf("@StartDate = '20")
        If pos > 1 Then
            sql = sql.Remove(pos + 14, 10)
            sql = sql.Insert(pos + 14, startDate)
        End If

        pos = sql.IndexOf("<= '20")
        If pos > 1 Then
            sql = sql.Remove(pos + 4, 10)
            sql = sql.Insert(pos + 4, endDate)
        End If
        pos = sql.IndexOf("@EndDate = '20")
        If pos > 1 Then
            sql = sql.Remove(pos + 12, 10)
            sql = sql.Insert(pos + 12, endDate)
        End If
        pos = sql.IndexOf("@CompareDate = '20")
        If pos > 1 Then
            sql = sql.Remove(pos + 16, 10)
            sql = sql.Insert(pos + 16, endDate)
        End If

    End Sub

    ''' <summary>
    ''' 更換顯示資料列項目(舊版SQL)
    ''' </summary>
    Public Function ReSQL(ByVal sql As String) As String

        '更換顯示資料列項目
        sql = sql.Replace("NoNewHighLowDates,", "NPTT,")

        Return sql
    End Function

#End Region

    ''' <summary>
    ''' 加入所有個股到我的追蹤
    ''' </summary>
    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAll.Click

        If Not MemberDataAccessor.Instance.IsLogin OrElse _
        MemberDataAccessor.Instance.GetMemberNo = "0" Then Exit Sub

        Dim stockNo As Label
        Dim checkbox As CheckBox
        For Each row As ListViewItem In lvMyRule.Items
            'stockNo = row.i(0).Text
            stockNo = row.FindControl("stockNo")
            checkbox = row.FindControl("select")
            If checkbox IsNot Nothing AndAlso checkbox.Checked AndAlso stockNo.Text <> "" Then
                sdsCollect.InsertParameters("MemberNo").DefaultValue = MemberDataAccessor.Instance.GetMemberNo
                sdsCollect.InsertParameters("StockNo").DefaultValue = stockNo.Text
                sdsCollect.InsertParameters("Group").DefaultValue = "1"
                'sdsCollect.InsertParameters("Group").DefaultValue = Group.SelectedValue
                sdsCollect.Insert()
            End If
        Next
        CheckAll.Checked = False
        sdsCollect.SelectCommand = lblQuery.Text
        Response.Redirect("/Hottip/HottipPage1.aspx")

    End Sub

End Class
