
Partial Class RateInfo
    Inherits System.Web.UI.Page

    Private mScript As String = _
    "var isFocus = 1;" + _
    "function showTime_@ID() {" + _
    "var t = parseInt(document.getElementById(""@Control"").innerHTML, 10);" + _
    "    t -= 1;" + _
    "    if (t > 0) {" + _
    "        document.getElementById(""@Control"").innerHTML = t +"" 秒 更新"";" + _
    "        setTimeout(""showTime_@ID()"", 1000);" + _
    "    }" + _
    "    else {" + _
    "       if (isFocus == 1) {" + _
    "          location.reload(); }" + _
    "       else {" + _
    "        document.getElementById(""@Control"").innerHTML = ""1 秒 更新"";" + _
    "         setTimeout(""showTime_@ID()"", 1000); }" + _
    "     }" + _
    "}"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("ID") Is Nothing OrElse Request("ID").Length <> 6 Then
            Response.Redirect("http://www.wantgoo.com/globalfinance.aspx")
            Exit Sub
        End If

        If Val(lblUpdateInterval.Text) = 0 Then
            lblElapseTime.Text = ""
        Else
            lblElapseTime.Text = lblUpdateInterval.Text + " 秒 更新"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "time" + Me.ClientID, mScript.Replace("@ID", Me.ClientID).Replace("@Control", lblElapseTime.ClientID) + "showTime_" + Me.ClientID + "();", True)
        End If

        Dim fromID As String = ""
        Dim toID As String = ""
        If Me.GetID(fromID, toID) = False Then
            'Response.Redirect("\stock\nostock.htm")
            'If Request.UrlReferrer Is Nothing Then
            '    Response.Write("Ooops! 沒有此代號的匯率資料喔!<br/><br/><a href=""/stock/searchstock.aspx"">查詢台股/美股</a><br/><br/><a href=""/"">回首頁</a>")
            'Else
            '    Response.Write("Ooops! 沒有此代號的匯率資料喔!<br/><br/><a href=""/stock/searchstock.aspx"">查詢台股/美股</a><br/><br/><a href=""" + Request.UrlReferrer.ToString + """>回上一頁</a>")
            'End If
            Response.Redirect("http://www.wantgoo.com/globalfinance.aspx")
            Exit Sub
        End If

        '     Dim stocktab2 As String = Me.Request.Url.ToString.ToLower()
        '     Dim tab2 As String = "<div id=""tabs_stocksub""><ul>" + _
        '"<li id=""@style1""><a href=""/stock/index.aspx?stockno=@no""><span>即時走勢</span></a></li>" + _
        '"<li id=""@style5""><a href=""/stock/news.aspx?stockno=0000""><span>即時新聞</span></a></li>" + _
        '"</ul></div>"

        '     tab2 = tab2.Replace("@no", "0000")
        '     If stocktab2.Contains("corpinfom") Then
        '         tab2 = tab2.Replace("@style1", "current")
        '     ElseIf stocktab2.Contains("chart") Then
        '         tab2 = tab2.Replace("@style2", "current")
        '     ElseIf stocktab2.Contains("news") Then
        '         tab2 = tab2.Replace("@style5", "current")
        '     End If
        '     tabs.Text = tab2

        Me.LoadTitle(fromID, toID)

        Me.SetTimer()
    End Sub

    Private Sub SetMeta()
        Dim title As String = Me.GetTitle
        Page.Title = title + " - 國際匯率 - WantGoo 玩股網"

        'Description
        Dim desc As New HtmlMeta()
        desc.Name = "description"
        desc.Content = title & "匯率即時走勢,Stock Money 國際匯率 國際指數 股票 基金 期貨 投資 理財 即時國際股市期貨報價 人工智慧選股功能 高手盤勢個股分析"
        Page.Header.Controls.Add(desc)

        'Keyword
        Dim keywords As New HtmlMeta()
        keywords.Name = "keywords"
        keywords.Content = title & "匯率即時走勢,匯率歷史走勢,理財,投資"
        Page.Header.Controls.Add(keywords)

        'og Title
        Dim ogTitle As New HtmlMeta
        ogTitle.Name = "og:title"
        ogTitle.Content = Page.Title
        Page.Header.Controls.Add(ogTitle)

        'og Description
        Dim ogdesc As New HtmlMeta()
        ogdesc.Name = "og:description"
        ogdesc.Content = desc.Content
        Page.Header.Controls.Add(ogdesc)
    End Sub

    Private Sub SetTimer()
        If Now.DayOfWeek <> DayOfWeek.Sunday OrElse Now.DayOfWeek <> DayOfWeek.Sunday Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "time", "showTime();", True)
            lblTime.Text = "<span align=""center"" id=""ElapseTime"">120</span>"
        End If
    End Sub

    Public Function GetHottipProfit() As String
        Dim stockNo As String = "0000"
        If stockNo Is Nothing OrElse stockNo.Trim = "" OrElse _
            Application(stockNo + "_Market") = 99 OrElse _
            StockLoader.Instance.GetName(stockNo) Is Nothing OrElse _
            StockLoader.Instance.GetName(stockNo) = "" Then Return ""

        Dim timeKey As String = stockNo + "_HottipProfit_LastTime"
        Dim key As String = stockNo + "_HottipProfit"

        If Application(timeKey) IsNot Nothing AndAlso _
     Now < Application(timeKey) Then
            Return Application(key)
        End If


        Dim template As String = "<p style=""color:@Color; line-height:16px; margin-bottom:5px;""><a style=""color:#3b5998;text-decoration:none;"" href=""/stock/chart.aspx?StockNo=@StockNo&h=@ID"" target=""_blank"">@StockName</a> <a style=""color:#3b5998;text-decoration:none;"" href=""/stock/chart.aspx?StockNo=@StockNo&h=@ID"" target=""_blank"">@Rule</a><br/>@Job@Days天後，大@Direct @Profit</p>"
        Dim hottip As String = "<div style=""width:98%; margin-top:10px;border: 1px solid #A6324D; text-align:left;""><div style="" background-color:#A6324D; font-weight:bold; padding:3px 5px 4px;""><span style=""color:#ffffff;"">飆股搜尋系統績效實例</span></div><div style=""background: none repeat scroll 0 0 #FFF3FA; width:98%; padding:5px 0 5px 5px;"">"
        Dim hasData As Boolean
        Dim direct As String
        Dim color As String
        Dim stocks As New Generic.List(Of String)
        Dim count As Integer
        Dim job As String
        Dim stockNoHottip As String
        Dim sdsHottipProfit As New SqlDataSource
        sdsHottipProfit.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sdsHottipProfit.SelectCommand = "select * from (SELECT top 100 stockNo, Date, [Rule], Direction, BuyPrice, MaxPrice, MaxDate, Profit, Days, ID FROM HottipProfit Where  IsSuccess = 1 and (Profit > 0.3 or Profit <-0.3) Order by ABS(Profit) desc) t order by NEWID()"
        Dim en As IEnumerator = sdsHottipProfit.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            stockNoHottip = en.Current("StockNo")
            If Not stocks.Contains(stockNoHottip) Then

                If en.Current("Direction") = 1 Then
                    direct = "漲"
                    color = "red"
                    job = "買進"
                Else
                    direct = "跌"
                    color = "green"
                    job = "賣空"
                End If

                hottip = hottip + template.Replace("@Date", Format(en.Current("Date"), "M/d")).Replace("@StockName", Application(stockNoHottip + "_Name")).Replace("@StockNo", stockNoHottip).Replace("@ID", en.Current("ID")).Replace("@Rule", en.Current("Rule")).Replace("@Job", job).Replace("@Days", en.Current("Days")).Replace("@Direct", direct).Replace("@Profit", Format(en.Current("Profit"), "0.00%")).Replace("@Color", color)

                hasData = True
                stocks.Add(stockNoHottip)

                If count >= 3 Then Exit While
                count = count + 1
            End If
        End While

        If hasData Then
            hottip = hottip + "<br><a style=""color:#3b5998;text-decoration:none;"" href=""/facebook/newRegister.aspx"" target=""_top"">免費註冊</a>試用三天<a style=""color:#3b5998;text-decoration:none;"" href=""/hottipsearch.aspx"" target=""_top"">飆股搜尋系統</a><br><br></div>"
            Application(key) = hottip
            Application(timeKey) = Now.AddHours(1)
        End If

        Return hottip
    End Function

    Private Function GetID(ByRef fromID As String, ByRef toID As String) As Boolean
        Dim key As String = Request("ID")
        fromID = Left(key, 3)
        toID = Right(key, 3)
        If Me.RateName(fromID).Length = 0 OrElse Me.RateName(toID).Length = 0 Then
            Return False
        End If
        Return True
    End Function

#Region "匯率資料"
    Private mRateKey As String = "Rate_Value_"
    ''' <summary>
    ''' 匯率名稱
    ''' </summary>
    Private Property RateName(ByVal id As String) As String
        Get
            Dim key As String = mRateKey + id + "_Name"
            If Application(key) Is Nothing Then Me.LoadRateName()
            If Application(key) Is Nothing Then Application(key) = id
            Return Application(key)
        End Get
        Set(ByVal value As String)
            Application(mRateKey + id + "_Name") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率區域
    ''' </summary>
    Private Property RateMarket(ByVal id As String) As String
        Get
            Dim key As String = mRateKey + id + "_Market"
            If Application(key) Is Nothing Then Me.LoadRateName()
            If Application(key) Is Nothing Then Application(key) = ""
            Return Application(key)
        End Get
        Set(ByVal value As String)
            Application(mRateKey + id + "_Market") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率昨收價
    ''' </summary>
    Private Property RateLast(ByVal fromID As String, ByVal toID As String) As Single
        Get
            Dim key As String = mRateKey + fromID + toID + "_Last"
            If Application(key) Is Nothing Then Me.LoadRateLast(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
        Set(ByVal value As Single)
            Application(mRateKey + fromID + toID + "_Last") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率最新報價
    ''' </summary>
    Private Property RateValue(ByVal fromID As String, ByVal toID As String) As Single
        Get
            Dim key As String = mRateKey + fromID + toID + "_Value"
            If Application(key) Is Nothing Then Me.LoadRate(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
        Set(ByVal value As Single)
            Application(mRateKey + fromID + toID + "_Value") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率最新日期
    ''' </summary>
    Private Property RateTime(ByVal fromID As String, ByVal toID As String) As Date
        Get
            Dim key As String = mRateKey + fromID + toID + "_Time"
            If Application(key) Is Nothing Then Me.LoadRate(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
        Set(ByVal value As Date)
            Application(mRateKey + fromID + toID + "_Time") = value
        End Set
    End Property

    ''' <summary>
    ''' 讀取所有匯率的名稱
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRateName()
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = "SELECT ID, Name, Market  from RateID"

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Me.RateName(en.Current("ID")) = en.Current("Name")
            Me.RateMarket(en.Current("ID")) = ""
        End While
    End Sub

    ''' <summary>
    ''' 讀取所有匯率的昨收價
    ''' </summary>
    Private Sub LoadRateLast(ByVal fromID As String, ByVal toID As String)
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = "select TOP 1 Date, Rate from RateHistory Where FromID = '" & fromID & "' And ToID = '" & toID & "' and DATEDIFF(day, RateHistory.date, GETDATE())>=1 Order By Date Desc"

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Me.RateLast(fromID, toID) = en.Current("Rate")
        End While
    End Sub

    ''' <summary>
    ''' 讀取所有匯率的最新報價
    ''' </summary>
    Private Sub LoadRate(ByVal fromID As String, ByVal toID As String)
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = "Select FromID, ToID, Date, Rate From Rate Where FromID = '" & fromID & "' And ToID = '" & toID & "'"

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Me.RateTime(fromID, toID) = en.Current("Date")
            Me.RateValue(fromID, toID) = en.Current("Rate")
        End While
    End Sub

    ''' <summary>
    ''' 匯率昨收價-For HighChart
    ''' </summary>
    Public ReadOnly Property RateLast() As Single
        Get
            Dim fromID As String = ""
            Dim toID As String = ""
            If Me.GetID(fromID, toID) = False Then Return 0

            Dim key As String = mRateKey + fromID + toID + "_Last"
            If Application(key) Is Nothing Then Me.LoadRateLast(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
    End Property
#End Region

    Private mTitleIndex As String = _
"<table cellpadding=""0"" cellspacing=""0"" style=""font-weight:bold; margin-left:10px; width:97%; text-align: left;"">" + _
"<tr><td style=""font-size:18px;"">@Name&nbsp;" + _
"<span style=""font-size:13px; color:#666666;"">(@StockNo)</span>&nbsp;&nbsp;" + _
"<span style=""font-size:18px; color:@Color;"">@Deal</span>&nbsp;" + _
"<span style=""font-size:18px; color:@Color;"">@Change</span>" + _
"<span style=""font-size:13px; color:#333333;"">(<span style=""color:@Color;"">@Ratio</span>)</span>" + _
"<span style=""font-size:11px; color:#999999;float:right;margin-top: 7px;"">時間:@Time</span>" + _
"</tr></table>"

    Private mTemplate As String = "<div style=""font-size:18px;"">@Name(@StockNo)<span style=""font-size:11px; color:#999999;float:right;margin-top:5px;"">當地:@Time</span></div>" + _
"<table cellpadding=""0"" cellspacing=""0"" style=""font-weight:bold;width:100%; text-align: left;"">" + _
"<tr><td style=""font-size:18px;"">" + _
"<span style=""font-size:18px; color:@Color;"">@Deal</span>&nbsp;" + _
"<span style=""font-size:18px; color:@Color;"">@Change</span>" + _
"<span style=""font-size:13px; color:#333333;""> (<span style=""color:@Color;"">@Ratio</span>)</span>"

    Private Sub LoadTitle(ByVal fromID As String, ByVal toID As String)
        Dim key As String = "Rate_Index_Table"
        If Application(key) IsNot Nothing AndAlso _
            Application(key + "LastTechTime") IsNot Nothing Then
            If Now < Application(key + "LastTechTime") Then
                'b.Text = Application(key)
                'Exit Sub
            End If
        End If

        Dim name As String = Me.RateName(fromID) & "/" & Me.RateName(toID)
        Dim data As String = mTemplate.Replace("@Name", name)

        Dim newRate As Single = Me.RateValue(fromID, toID)
        Dim lastRate As Single = Me.RateLast(fromID, toID)
        Dim change As Single = newRate - lastRate
        Dim percent As Single = 0
        If lastRate > 0 Then
            percent = change / lastRate
        End If

        data = data.Replace("@StockNo", fromID & "/" & toID)

        '顯示位數
        If newRate > 99.9 Then
            data = data.Replace("@Deal", Format(newRate, "0.0"))
        ElseIf newRate < 1 Then
            data = data.Replace("@Deal", Format(newRate, "0.0000"))
        Else
            data = data.Replace("@Deal", Format(newRate, "0.00"))
        End If

        change = Format(change, "0.0000")
        If change > 0 Then
            data = data.Replace("@Color", "red")
            data = data.Replace("@Change", "▲" & change)
            data = data.Replace("@Ratio", "+" & Format(percent, "0.00%"))
        ElseIf change < 0 Then
            data = data.Replace("@Color", "green")
            data = data.Replace("@Change", "▼" & change)
            data = data.Replace("@Ratio", Format(percent, "0.00%"))
        Else
            data = data.Replace("@Color", "#F39C04")
            data = data.Replace("@Change", "0.00")
            data = data.Replace("@Ratio", "0%")
        End If

        data = data.Replace("@Time", Format(Me.RateTime(fromID, toID), "MM/dd HH:mm"))
        b.Text = data

        Application(key) = b.Text
        Application(key + "LastTechTime") = Now.AddMinutes(2)
    End Sub

#Region "即時走勢圖"
    Public Function GetTitle() As String
        Dim fromID As String = ""
        Dim toID As String = ""
        If Me.GetID(fromID, toID) = False Then Return ""

        Return Me.RateName(fromID) & "/" & Me.RateName(toID) + " (" + fromID + "/" + toID + ")"
    End Function

    ''' <summary>
    ''' 即時走勢圖的資料
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRateChart() As String
        Dim fromID As String = ""
        Dim toID As String = ""
        If Me.GetID(fromID, toID) = False Then Return ""

        Dim key As String = "Rate_Realtime_" & fromID & toID
        Dim timeKey As String = key + "_lastRealtime"
        If Application(key) IsNot Nothing AndAlso Application(timeKey) IsNot Nothing Then
            If Now < Application(timeKey) Then
                Return Application(key)
            End If
        End If

        sdsRTRate.SelectParameters("FromID").DefaultValue = fromID
        sdsRTRate.SelectParameters("ToID").DefaultValue = toID
        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)
        Dim time As DateTime
        Dim lastTime As DateTime
        Dim sbPrice As New StringBuilder

        Dim en As IEnumerator = sdsRTRate.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            time = en.Current("Date")
            If time.Subtract(lastTime).TotalSeconds >= 60 Then
                sbPrice.Append("[")
                sbPrice.Append(time.Subtract(dt1970).TotalMilliseconds.ToString)
                sbPrice.Append(",")
                sbPrice.Append(en.Current("Rate"))
                sbPrice.Append("],")

                lastTime = time
            End If
        End While

        Application(key) = sbPrice.ToString.Trim(",")
        Application(timeKey) = Now.AddMinutes(2)

        Return Application(key)
    End Function
#End Region

#Region "歷史匯率走勢圖"
    ''' <summary>
    ''' 讀取歷史匯率 - For HighChart
    ''' </summary>
    Public Function GetHistoryRate() As String
        Dim fromID As String = ""
        Dim toID As String = ""
        If Me.GetID(fromID, toID) = False Then Return ""

        Dim key As String = "Rate_History_" & fromID & toID
        Dim timeKey As String = key + "_lastHistory"
        If Application(key) IsNot Nothing AndAlso Application(timeKey) IsNot Nothing Then
            If Now < Application(timeKey) Then
                Return Application(key)
            End If
        End If

        sdsHistoryRate.SelectParameters("FromID").DefaultValue = fromID
        sdsHistoryRate.SelectParameters("ToID").DefaultValue = toID
        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)
        Dim time As DateTime
        Dim sbPrice As New StringBuilder

        Dim en As IEnumerator = sdsHistoryRate.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            time = en.Current("Date")
            sbPrice.Append("[")
            sbPrice.Append(time.Subtract(dt1970).TotalMilliseconds.ToString)
            sbPrice.Append(",")
            sbPrice.Append(en.Current("Rate"))
            sbPrice.Append("],")
        End While

        Application(key) = sbPrice.ToString.Trim(",")
        Application(timeKey) = Now.AddDays(1)

        Return Application(key)
    End Function
#End Region
End Class
