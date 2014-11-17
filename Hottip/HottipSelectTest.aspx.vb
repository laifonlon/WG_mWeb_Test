Imports System.Data
Imports System.Collections.Generic

Partial Class Hottip_HottipSelectTest
    Inherits System.Web.UI.Page


    Private mStockSelectionTemplate As String = "<table cellpadding=""0"" cellspacing=""0""><tr><td><dt>@Rule</dt><dd class=""n"">@Stocks</dd></td></tr></table>"
    Private mStockSelectionTemplateNoAd As String = "<dt>@Rule</dt><dd class=""n"">@Stocks</dd>"
    Private mStockNameTemplate2 As String = _
    "<a href=""/stock/@No"" Target=""_blank"" title=""@Name(@No)  @Price"">@Name</a>"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If MemberDataAccessor.Instance.IsLogin AndAlso MemberDataAccessor.Instance.GetMemberLevel = MemberLevel.Normal Then
                lNote.Text = "每5分鐘更新報價，非好野會員延遲一天。"
            End If

            If Now.Hour < 9 Then
                sdsLastDate.SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 Order by Date Desc) t Order by Date desc"
            End If

            Select Case MemberDataAccessor.Instance.GetMemberLevel
                Case MemberLevel.Rich, MemberLevel.Super
                    sdsLastDate.SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 1 Date FROM Selection1 Order by Date Desc) t Order by Date desc"
            End Select

            Dim myDate As String = GetDate(Request("d"))

            If myDate = "" Then
                Dim en As IEnumerator

                If Now.DayOfWeek = DayOfWeek.Saturday OrElse _
                    Now.DayOfWeek = DayOfWeek.Sunday OrElse _
                    Val(Format(Now, "HH")) > 14 Then
                    en = sdsNewestDate.Select(New DataSourceSelectArguments).GetEnumerator()
                Else
                    en = sdsLastDate.Select(New DataSourceSelectArguments).GetEnumerator()
                End If

                If en.MoveNext() Then
                    Dim row As DataRowView = en.Current
                    myDate = Format(row("LastDate"), "yyyy/MM/dd")
                End If
            Else

            End If
            tbxDate.Text = myDate

            '因飆股搜尋有可能新增/刪除條件，在這清除我的飆股暫存
            If MemberDataAccessor.Instance.IsLogin AndAlso Request("m") Is Nothing Then
                Dim cacheKey As String = myDate & "_MySearchRule_Rise_" & MemberDataAccessor.Instance.GetMemberNo
                If Session(cacheKey) IsNot Nothing Then
                    Session(cacheKey) = Nothing
                    Session.Remove(cacheKey)
                End If
            End If

            Me.LoadData(0, tbxDate.Text.Trim)
            'Me.LoadData(Val(Request("m")), tbxDate.Text.Trim)
        End If
    End Sub

    Private Function IsToday(ByVal value As String) As Boolean
        If value.Trim = Format(Now, "yyyyMMdd") Then Return True
        Return False
    End Function

    ''' <summary>
    ''' 確認並正規化日期格式
    ''' </summary>
    Private Function GetDate(ByVal value As String) As String
        If value Is Nothing Then Return ""
        If value.Trim.Length < 8 Then Return ""

        Return value.Insert(6, "/").Insert(4, "/")
    End Function

    ''' <summary>
    ''' 查詢並顯示
    ''' </summary>
    Protected Sub Query(ByVal selectDate As String, ByRef buy As String, ByRef sell As String)
        GetSelection("#" + selectDate + "#")

        '加入系統選股內容
        Dim stockNames As New Generic.List(Of String)
        Dim stockNos As New Generic.List(Of String)
        Dim tips As New Generic.List(Of String)
        Dim stocks As String = ""
        Dim tip As String = ""

        '多
        Dim rules As New Generic.List(Of SelectionRule)
        rules.Add(SelectionRule.ThroughInterval)
        rules.Add(SelectionRule.LargeVolume)
        rules.Add(SelectionRule.LargeVolumeContinue)
        rules.Add(SelectionRule.Through60Mean)
        rules.Add(SelectionRule.KDCross)
        'rules.Add(SelectionRule.Mean510Cross)
        rules.Add(SelectionRule.MeanCross)
        rules.Add(SelectionRule.Swallow)
        rules.Add(SelectionRule.Cover)
        'rules.Add(SelectionRule.Combine3)
        rules.Add(SelectionRule.Combine4)

        Dim memberLevel As MemberLevel = MemberDataAccessor.Instance.GetMemberLevel
        Dim ruleName As String = ""
        Dim ruleTip As String = ""
        Dim tipTitle As String = ""
        Dim tipText As String = ""
        Dim tipTemplete As String = "<span class=""querytip"" title='<h3>@Title</h3><p>@Text</p>'>  ? </span>"
        Dim isNotComplete As Boolean

        For Each rule As SelectionRule In rules
            stocks = ""
            tip = ""
            stockNames.Clear()
            stockNos.Clear()
            tips.Clear()
            isNotComplete = False
            For Each stockNo As String In mSelection([Enum].GetName(GetType(SelectionRule), rule) + "1")
                stockNames.Add(Application(stockNo + "_Name"))
                stockNos.Add(stockNo)
                If mChange(stockNo) > 0 Then
                    tip = Format(mDeal(stockNo), "0.00") + " ▲" + Format(mChange(stockNo), "0.00")
                Else
                    tip = Format(mDeal(stockNo), "0.00") + " ▼" + Format(mChange(stockNo), "0.00").Replace("-", "")
                End If
                tips.Add(tip)
                stocks &= mStockNameTemplate2.Replace("@No", stockNo).Replace("@Name", Application(stockNo + "_Name")).Replace("@Price", tip) + ", "

                '非好野會員不能看全部
                If Not (memberLevel = Global.MemberLevel.Rich OrElse memberLevel = Global.MemberLevel.Super OrElse MemberDataAccessor.Instance.IsAdiminstrator) AndAlso _
                     (stockNos.Count >= 5 OrElse rule = SelectionRule.LargeVolumeContinue OrElse rule = SelectionRule.ThroughInterval) Then
                    isNotComplete = True
                    Exit For
                End If
            Next
            ruleName = TwStockAccessor.Instance.GetRuleName(rule, Direct.Rise, tipTitle, tipText)
            ruleTip = tipTemplete.Replace("@Title", tipTitle).Replace("@Text", tipText)
            If tipTitle.Length = 0 AndAlso tipText.Length = 0 Then ruleTip = ""
            '非好野會員加入廣告
            If rule = SelectionRule.ThroughInterval OrElse rule = SelectionRule.LargeVolume Then
                buy &= AddStockSelection(ruleName + ruleTip, Direct.Rise, stocks, stockNames, stockNos, tips, Member.Instance.IsShowAd, AD.InnerText, isNotComplete)
            Else
                buy &= AddStockSelection(ruleName + ruleTip, Direct.Rise, stocks, stockNames, stockNos, tips, False, "", isNotComplete)
            End If
        Next

        Dim da As New twStocksTableAdapters.NewsTableAdapter
        Dim news As twStocks.NewsDataTable = da.GetData("#" + tbxDate.Text + "#")

        '插入利多新聞
        stocks = ""
        tip = ""
        stockNames.Clear()
        stockNos.Clear()
        tips.Clear()
        isNotComplete = False
        For Each row As twStocks.NewsRow In news.Rows
            If row.Effect > 0 Then
                stockNames.Add(row.Name)
                stockNos.Add(row.StockNo.ToString)
                If row.Change > 0 Then
                    tip = Format(row.Deal, "0.00") + " ▲" + Format(row.Change, "0.00")
                Else
                    tip = Format(row.Deal, "0.00") + " ▼" + Format(row.Change, "0.00").Replace("-", "")
                End If
                tips.Add(tip)
                stocks &= mStockNameTemplate2.Replace("@No", row.StockNo).Replace("@Name", row.Name).Replace("@Price", tip) + ", "

                '非好野會員不能看全部
                If Not (memberLevel = Global.MemberLevel.Rich OrElse memberLevel = Global.MemberLevel.Super OrElse MemberDataAccessor.Instance.IsAdiminstrator) AndAlso _
                     stockNos.Count >= 5 Then
                    isNotComplete = True
                    Exit For
                End If
            End If
        Next
        ruleName = TwStockAccessor.Instance.GetRuleName(SelectionRule.News, Direct.Rise, tipTitle, tipText)
        ruleTip = tipTemplete.Replace("@Title", tipTitle).Replace("@Text", tipText)
        If tipTitle.Length = 0 AndAlso tipText.Length = 0 Then ruleTip = ""
        buy &= AddStockSelection(ruleName + ruleTip, Direct.Rise, stocks, stockNames, stockNos, tips)

        '空 方條件
        rules.Remove(SelectionRule.Combine4)

        For Each rule As SelectionRule In rules
            stocks = ""
            tip = ""
            stockNames.Clear()
            stockNos.Clear()
            tips.Clear()
            isNotComplete = False
            For Each stockNo As String In mSelection([Enum].GetName(GetType(SelectionRule), rule) + "0")
                stockNames.Add(Application(stockNo + "_Name"))
                stockNos.Add(stockNo)
                If mChange(stockNo) > 0 Then
                    tip = Format(mDeal(stockNo), "0.00") + " ▲" + Format(mChange(stockNo), "0.00")
                Else
                    tip = Format(mDeal(stockNo), "0.00") + " ▼" + Format(mChange(stockNo), "0.00").Replace("-", "")
                End If
                tips.Add(tip)
                stocks &= mStockNameTemplate2.Replace("@No", stockNo).Replace("@Name", Application(stockNo + "_Name")).Replace("@Price", tip) + ", "

                '非好野會員不能看全部
                If Not (memberLevel = Global.MemberLevel.Rich OrElse memberLevel = Global.MemberLevel.Super OrElse MemberDataAccessor.Instance.IsAdiminstrator) AndAlso _
                     (stockNos.Count >= 5 OrElse rule = SelectionRule.LargeVolumeContinue OrElse rule = SelectionRule.ThroughInterval) Then
                    isNotComplete = True
                    Exit For
                End If
            Next

            ruleName = TwStockAccessor.Instance.GetRuleName(rule, Direct.Fall, tipTitle, tipText)
            ruleTip = tipTemplete.Replace("@Title", tipTitle).Replace("@Text", tipText)
            If tipTitle.Length = 0 AndAlso tipText.Length = 0 Then ruleTip = ""
            sell &= AddStockSelection(ruleName + ruleTip, Direct.Rise, stocks, stockNames, stockNos, tips, False, "", isNotComplete)
        Next

        '插入利空新聞
        stocks = ""
        tip = ""
        stockNames.Clear()
        stockNos.Clear()
        tips.Clear()
        isNotComplete = False
        For Each row As twStocks.NewsRow In news.Rows
            If row.Effect < 0 Then
                stockNames.Add(row.Name)
                stockNos.Add(row.StockNo.ToString)
                If row.Change > 0 Then
                    tip = Format(row.Deal, "0.00") + " ▲" + Format(row.Change, "0.00")
                Else
                    tip = Format(row.Deal, "0.00") + " ▼" + Format(row.Change, "0.00").Replace("-", "")
                End If
                tips.Add(tip)
                stocks &= mStockNameTemplate2.Replace("@No", row.StockNo).Replace("@Name", row.Name).Replace("@Price", tip) + ", "
            End If

            '非好野會員不能看全部
            If Not (memberLevel = Global.MemberLevel.Rich OrElse memberLevel = Global.MemberLevel.Super OrElse MemberDataAccessor.Instance.IsAdiminstrator) AndAlso _
                 stockNos.Count >= 5 Then
                isNotComplete = True
                Exit For
            End If
        Next
        ruleName = TwStockAccessor.Instance.GetRuleName(SelectionRule.News, Direct.Fall, tipTitle, tipText)
        ruleTip = tipTemplete.Replace("@Title", tipTitle).Replace("@Text", tipText)
        If tipTitle.Length = 0 AndAlso tipText.Length = 0 Then ruleTip = ""
        sell &= AddStockSelection(ruleName + ruleTip, Direct.Fall, stocks, stockNames, stockNos, tips)
    End Sub

    Private mSelection As New Generic.Dictionary(Of String, Generic.List(Of String))
    Private mDeal As New Generic.Dictionary(Of String, Double)
    Private mChange As New Generic.Dictionary(Of String, Double)
    Public Function GetSelection(ByVal queryDate As Date) As Generic.Dictionary(Of String, Generic.List(Of String))

        mSelection.Add("Combine41", New Generic.List(Of String))
        mSelection.Add("Combine40", New Generic.List(Of String))
        mSelection.Add("Combine31", New Generic.List(Of String))
        mSelection.Add("Combine30", New Generic.List(Of String))
        mSelection.Add("Cover1", New Generic.List(Of String))
        mSelection.Add("Cover0", New Generic.List(Of String))
        mSelection.Add("KDCross1", New Generic.List(Of String))
        mSelection.Add("KDCross0", New Generic.List(Of String))
        mSelection.Add("LargeVolume1", New Generic.List(Of String))
        mSelection.Add("LargeVolume0", New Generic.List(Of String))
        mSelection.Add("LargeVolumeContinue1", New Generic.List(Of String))
        mSelection.Add("LargeVolumeContinue0", New Generic.List(Of String))
        mSelection.Add("MeanCross1", New Generic.List(Of String))
        mSelection.Add("MeanCross0", New Generic.List(Of String))
        mSelection.Add("Swallow1", New Generic.List(Of String))
        mSelection.Add("Swallow0", New Generic.List(Of String))
        mSelection.Add("Through60Mean1", New Generic.List(Of String))
        mSelection.Add("Through60Mean0", New Generic.List(Of String))
        mSelection.Add("ThroughInterval1", New Generic.List(Of String))
        mSelection.Add("ThroughInterval0", New Generic.List(Of String))
        mSelection.Add("ThroughHighLow1", New Generic.List(Of String))
        mSelection.Add("ThroughHighLow0", New Generic.List(Of String))

        Dim sdsSelection As New SqlDataSource
        sdsSelection.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sdsSelection.SelectCommand = "exec [HottipSearchGetSelection]  '@Date'; "
        sdsSelection.SelectCommand = sdsSelection.SelectCommand.Replace("@Date", Format(queryDate, "yyyy-MM-dd"))

        MemberDataAccessor.Instance.Action = sdsSelection.SelectCommand
        Dim en As IEnumerator = sdsSelection.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            If Not IsDBNull(en.Current("IsThroughInterval")) AndAlso en.Current("IsThroughInterval") = 1 Then
                AddSelectionCollection(SelectionRule.ThroughInterval, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsLargeVolume")) AndAlso en.Current("IsLargeVolume") = 1 Then
                AddSelectionCollection(SelectionRule.LargeVolume, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsLargeVolumeContinue")) AndAlso en.Current("IsLargeVolumeContinue") = 1 Then
                AddSelectionCollection(SelectionRule.LargeVolumeContinue, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsSwallow")) AndAlso en.Current("IsSwallow") = 1 Then
                AddSelectionCollection(SelectionRule.Swallow, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsCover")) AndAlso en.Current("IsCover") = 1 Then
                AddSelectionCollection(SelectionRule.Cover, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsMeanCross")) AndAlso en.Current("IsMeanCross") = 1 Then
                AddSelectionCollection(SelectionRule.MeanCross, en.Current("Direction"), en.Current("StockNo"))
            End If
            'If Not IsDBNull(en.Current("IsMean510Cross")) AndAlso en.Current("IsMean510Cross") = 1 Then
            '    AddSelectionCollection(SelectionRule.Mean510Cross, en.Current("Direction"), en.Current("StockNo"))
            'End If
            If Not IsDBNull(en.Current("IsKDCross")) AndAlso en.Current("IsKDCross") = 1 Then
                AddSelectionCollection(SelectionRule.KDCross, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsThrough60Mean")) AndAlso en.Current("IsThrough60Mean") = 1 Then
                AddSelectionCollection(SelectionRule.Through60Mean, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsMean3Combine")) AndAlso en.Current("IsMean3Combine") = 1 Then
                AddSelectionCollection(SelectionRule.Combine3, en.Current("Direction"), en.Current("StockNo"))
            End If
            If Not IsDBNull(en.Current("IsMean4Combine")) AndAlso en.Current("IsMean4Combine") = 1 Then
                AddSelectionCollection(SelectionRule.Combine4, en.Current("Direction"), en.Current("StockNo"))
            End If
            'If Not IsDBNull(en.Current("IsThroughHighLow")) AndAlso en.Current("IsThroughHighLow") Then
            '    AddSelectionCollection(SelectionRule.ThroughHighLow, en.Current("Direction"), en.Current("StockNo"))
            'End If
            If Not mDeal.ContainsKey(en.Current("StockNo")) Then
                mDeal.Add(en.Current("StockNo"), en.Current("Deal"))
                mChange.Add(en.Current("StockNo"), en.Current("Change"))
            End If
        End While
        Return mSelection
    End Function

    Private Sub AddSelectionCollection(ByVal rule As SelectionRule, ByVal direct As Integer, ByVal stockNo As String)
        mSelection([Enum].GetName(GetType(SelectionRule), rule) + direct.ToString).Add(stockNo)
    End Sub

    ''' <summary>
    ''' 加入系統選股
    ''' </summary>
    Private Function AddStockSelection(ByVal rule As String, ByVal direct As Direct, ByVal stocks As String, ByVal stockNames As Generic.List(Of String), ByVal stockNos As Generic.List(Of String), _
                                  ByVal tips As Generic.List(Of String), Optional ByVal showAd As Boolean = False, Optional ByVal ad As String = "", Optional ByVal isNotComplete As Boolean = False) As String
        Dim content As String = mStockSelectionTemplate

        'Dim stocks As String = ""
        'For i As Integer = 0 To stockNames.Count - 1
        '    stocks = stocks + mStockNameTemplate2.Replace("@No", stockNos(i)).Replace("@Name", stockNames(i)).Replace("@Price", tips(i)) + ", "
        'Next

        content = content.Replace("@Rule", rule)
        If stocks = "" Then
            content = content.Replace("@Stocks", "無滿足條件的股票")
        Else
            If showAd Then
                content = content.Replace("@Stocks", stocks.TrimEnd.TrimEnd(",") + "<br />" + ad)
            Else
                If isNotComplete Then
                    content = content.Replace("@Stocks", stocks + "僅列出部分，看全部股票請加入<a target=""_blank"" href =""/mall/shopping.aspx?id=2"">好野會員</a> or <a target=""_blank"" href =""/facebook/newRegister.aspx"">先免費註冊試用三天！</a>")
                Else
                    content = content.Replace("@Stocks", stocks.TrimEnd.TrimEnd(","))
                End If
            End If

        End If

        'Select Case direct
        '    Case direct.Rise
        '        content = content.Replace("@Color", "DC0300")
        '    Case direct.Fall
        '        content = content.Replace("@Color", "00A002")
        'End Select

        'mStockSelection = content

        Return content
    End Function

    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuery.Click
        Me.LoadData(Val(Request("m")), tbxDate.Text.Trim)
    End Sub

    ''' <summary>
    ''' 載入資料
    ''' </summary>
    Private Sub LoadData(ByVal mode As Integer, ByVal selectDate As String)
        Select Case mode
            Case 1 '我的飆股
                Me.LoadHottipRule(selectDate)
                'aTab1.Attributes("class") = "sexybutton sexysimple sexyred2"
                tab1.Style("display") = "block"
            Case 2 '熱門選股
                Me.LoadMyRule(selectDate)
                'aTab2.Attributes("class") = "sexybutton sexysimple sexyred2"
                tab2.Style("display") = "block"
            Case Else '俏秘書選股
                Dim isShowAd As String = Member.Instance.IsShowAd.ToString
                Dim timeKey As String = selectDate + "_HottipSelect_LastTime1" + isShowAd

                If Application(timeKey) IsNot Nothing AndAlso Now < Application(timeKey) Then
                Else
                    Dim buyContext As String = ""
                    Dim sellContext As String = ""
                    Me.Query(selectDate, buyContext, sellContext)

                    Application(selectDate + "_HottipSelect_Buy1" + isShowAd) = buyContext
                    Application(selectDate + "_HottipSelect_Sell1" + isShowAd) = sellContext

                    If Val(Format(Now, "HH")) > 8 AndAlso Val(Format(Now, "HH")) < 14 Then
                        Application(timeKey) = Now.AddMinutes(5)
                    Else
                        Application(timeKey) = Now.AddMinutes(30)
                    End If
                End If

                Buy.Text = Application(selectDate + "_HottipSelect_Buy1" + isShowAd)
                Sell.Text = Application(selectDate + "_HottipSelect_Sell1" + isShowAd)
                'Me.LoadHottipSelect(selectDate)
                'aTab0.Attributes("class") = "sexybutton sexysimple sexyred2"
                tab0.Style("display") = "block"
        End Select
    End Sub

    ''' <summary>
    ''' 載入俏秘書選股
    ''' </summary>
    Private Sub LoadHottipSelect(ByVal selectDate As String)
        '一般會員 CacheKey 的會員編號用0代表
        Dim memberNo As String = "506"

        Dim cacheKeyRise As String = selectDate & "_HottipSelect_Rise_" & memberNo
        Dim cacheKeyFall As String = selectDate & "_HottipSelect_Fall_" & memberNo

        If Cache(cacheKeyRise) Is Nothing OrElse Cache(cacheKeyFall) Is Nothing Then
            Dim buy1 As String = ""
            Dim sell1 As String = ""
            Me.Query(selectDate, buy1, sell1)

            '更新時間
            Dim minute As Integer = Me.GetUpdateMinute(False)
            'If memberNo = "0" Then minute = 60

            Cache.Add(cacheKeyRise, buy1 _
                     , Nothing, DateTime.Now.AddMinutes(minute), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)

            Cache.Add(cacheKeyFall, sell1 _
                      , Nothing, DateTime.Now.AddMinutes(minute), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If

        Buy.Text = TryCast(Cache(cacheKeyRise), String)
        Sell.Text = TryCast(Cache(cacheKeyFall), String)
    End Sub

    ''' <summary>
    ''' 載入熱門選股
    ''' </summary>
    Private Sub LoadHottipRule(ByVal selectDate As String)
        '一般會員 CacheKey 的會員編號用0代表
        Dim memberNo As String = "0"
        If MemberDataAccessor.Instance.IsLogin AndAlso _
            MemberDataAccessor.Instance.GetMemberLevel <> MemberLevel.Normal Then
            memberNo = "506"
        End If

        Dim cacheKeyRise As String = selectDate & "_HottipRule_Rise_" & memberNo
        Dim cacheKeyFall As String = selectDate & "_HottipRule_Fall_" & memberNo

        If Cache(cacheKeyRise) Is Nothing OrElse Cache(cacheKeyFall) Is Nothing Then
            Dim cmd As String = "SELECT RuleId,Name,SQL FROM HottipRule Where MemberNo='506' Order by [Order]"
            Dim rules As Dictionary(Of String, Dictionary(Of String, String)) = Me.GetRules(selectDate, cmd)

            '更新時間
            Dim minute As Integer = Me.GetUpdateMinute(True)
            If memberNo = "0" Then minute = 60

            Cache.Add(cacheKeyRise, Me.GetTable(rules("Rise"), Direct.Rise) _
                      , Nothing, DateTime.Now.AddMinutes(minute), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)

            Cache.Add(cacheKeyFall, Me.GetTable(rules("Fall"), Direct.Fall) _
                      , Nothing, DateTime.Now.AddMinutes(minute), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If

        Buy1.Text = TryCast(Cache(cacheKeyRise), String)
        Sell1.Text = TryCast(Cache(cacheKeyFall), String)
    End Sub

    ''' <summary>
    ''' 載入我的飆股
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadMyRule(ByVal selectDate As String)
        '一般會員顯示加入好野會員
        If MemberDataAccessor.Instance.IsLogin = False OrElse _
            MemberDataAccessor.Instance.GetMemberLevel = MemberLevel.Normal Then
            Buy2.Text = mStockSelectionTemplate.Replace("@Rule", "&nbsp;").Replace("@Stocks", "看我的飆股請先加入<a target=""_blank"" href =""/mall/shopping.aspx?id=2"">好野會員</a> or <a target=""_blank"" href =""/facebook/newRegister.aspx"">先免費註冊試用三天！</a>")
            Exit Sub
        End If

        Dim memberNo As String = MemberDataAccessor.Instance.GetMemberNo

        Dim cacheKeyRise As String = selectDate & "_MySearchRule_Rise_" & memberNo
        If TryCast(Session(cacheKeyRise), String) Is Nothing OrElse Now > Session(cacheKeyRise + "_Deadline") Then
            Dim rules As New Dictionary(Of String, String)
            Dim cmd As String = "SELECT  RuleId,Name,SQL  FROM HottipRule " + _
            "WHERE (MemberNo = '" & memberNo & "' )"
            Me.AddMyRule(memberNo, cmd, selectDate, selectDate, rules)

            If rules.Count = 0 Then
                Buy2.Text = mStockSelectionTemplate.Replace("@Rule", "&nbsp;").Replace("@Stocks", "您尚未新增飆股設定，請先至<a href =""/hottipsearch.aspx"">飆股搜尋</a>頁面進行設定。")
                Exit Sub
            End If

            '更新時間
            Session(cacheKeyRise) = Me.GetTable(rules, Direct.Rise)
            Session(cacheKeyRise + "_Deadline") = Now.AddMinutes(Me.GetUpdateMinute(False))
        End If

        Buy2.Text = TryCast(Session(cacheKeyRise), String)
    End Sub

    ''' <summary>
    ''' 取得Cache更新時間(分鐘)
    ''' </summary>
    Private Function GetUpdateMinute(ByVal isHottipRule As Boolean) As Integer
        Dim min As Integer = 0
        If Now.DayOfWeek <> DayOfWeek.Saturday AndAlso _
            Now.DayOfWeek <> DayOfWeek.Sunday AndAlso _
             (Now.Hour > 8 AndAlso Now.Hour <= 14) Then
            If Now.Hour >= 9 Then
                If isHottipRule Then
                    min = 15 '盤中, 熱門選股
                Else
                    min = 5 '盤中
                End If
            Else
                min = 10 '接近盤中10分
            End If
        Else
            min = 60 '1小時
        End If
        Return min
    End Function

    ''' <summary>
    ''' 加入我的飆股RuleID (因有手機跟Email)
    ''' </summary>
    Private Sub AddMyRule(ByVal memberNo As String, ByVal cmd As String, ByVal startDate As String, ByVal endDate As String, ByRef rules As Dictionary(Of String, String))
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        'sds.SelectCommand = "SELECT RuleId,Name,SQL FROM HottipRule Where MemberNo='" & MemberDataAccessor.Instance.GetMemberNo & "' Order by [Order]"
        sds.SelectCommand = cmd

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        Dim key As String = ""
        While en.MoveNext
            key = en.Current("RuleId") & "<?>" & en.Current("Name")
            If rules.ContainsKey(key) = False Then
                rules.Add(key, Me.ReSQL(en.Current("SQL"), startDate, endDate))
            End If
        End While
    End Sub

    ''' <summary>
    ''' 根據Rule Name區分多或空，塞到不同陣列
    ''' </summary>
    Private Function GetRules(ByVal selectDate As String, ByVal cmd As String) As Dictionary(Of String, Dictionary(Of String, String))
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = cmd
        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        Dim rules As New Dictionary(Of String, Dictionary(Of String, String))
        rules.Add("Fall", New Dictionary(Of String, String))
        rules.Add("Rise", New Dictionary(Of String, String))
        While en.MoveNext
            If en.Current("Name").ToString.Contains("空") Then
                If rules("Fall").ContainsKey(en.Current("Name")) = False Then
                    rules("Fall").Add(en.Current("Name"), Me.ReSQL(en.Current("SQL"), selectDate, selectDate))
                End If
            Else
                If rules("Rise").ContainsKey(en.Current("Name")) = False Then
                    rules("Rise").Add(en.Current("Name"), Me.ReSQL(en.Current("SQL"), selectDate, selectDate))
                End If
            End If
        End While
        Return rules
    End Function

    ''' <summary>
    ''' 修正SQL 日期區間 顯示資料行
    ''' </summary>
    Private Function ReSQL(ByVal sql As String, ByVal startDate As String, ByVal endDate As String) As String
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
        Return sql
    End Function

    ''' <summary>
    ''' 將查詢到的所有個股組成Table
    ''' </summary>
    Private Function GetTable(ByVal rules As Dictionary(Of String, String), ByVal direct As Direct) As String
        Dim newTable As String = ""

        Dim sdsSelection As New SqlDataSource
        sdsSelection.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString

        Dim stockNames As New List(Of String)
        Dim stockNos As New List(Of String)
        Dim tips As New List(Of String)
        Dim stocks, tip As String

        Dim memberLevel As MemberLevel = MemberDataAccessor.Instance.GetMemberLevel
        Dim isRichMember As Boolean
        If memberLevel = Global.MemberLevel.Rich OrElse memberLevel = Global.MemberLevel.Super OrElse MemberDataAccessor.Instance.IsAdiminstrator Then
            isRichMember = True
        End If

        Dim ruleKeys() As String
        For Each key As String In rules.Keys
            stocks = ""
            tip = ""
            stockNames.Clear()
            stockNos.Clear()
            tips.Clear()

            'Rule SQL
            sdsSelection.SelectCommand = rules(key)

            '個股
            Dim enRule As IEnumerator = sdsSelection.Select(New DataSourceSelectArguments).GetEnumerator
            While enRule.MoveNext
                Dim stockName As String = enRule.Current("Name")
                Dim stockNo As String = enRule.Current("StockNo")
                stockNames.Add(stockName)
                stockNos.Add(stockNo)

                StockLoader.Instance.LoadPrice(stockNo)

                Dim deal As Single = Application(stockNo + "_Deal")
                Dim change As Single = Application(stockNo + "_Change")

                'Tip
                If change > 0 Then
                    tip = Format(deal, "0.00") + " ▲" + Format(change, "0.00")
                Else
                    tip = Format(deal, "0.00") + " ▼" + Format(change, "0.00").Replace("-", "")
                End If
                tips.Add(tip)
                stocks &= mStockNameTemplate2.Replace("@No", stockNo).Replace("@Name", stockName).Replace("@Price", tip) + ", "
            End While

            'Rule Name
            ruleKeys = Regex.Split(key, "<?>", RegexOptions.IgnoreCase)
            newTable &= AddStockSelection2(ruleKeys(ruleKeys.Length - 1), direct, stocks, stockNames, stockNos, tips, False, "", isRichMember)
        Next

        Return newTable
    End Function

    Private Function AddStockSelection2(ByVal rule As String, ByVal direct As Direct, ByVal stocks As String, ByVal stockNames As Generic.List(Of String), ByVal stockNos As Generic.List(Of String), _
                                  ByVal tips As Generic.List(Of String), Optional ByVal showAd As Boolean = False, Optional ByVal ad As String = "", Optional ByVal isRichMember As Boolean = False) As String
        Dim content As String = mStockSelectionTemplate

        showAd = False '取消廣告

        'Dim stocks As String = ""
        'For i As Integer = 0 To stockNames.Count - 1
        '    stocks = stocks + mStockNameTemplate2.Replace("@No", stockNos(i)).Replace("@Name", stockNames(i)).Replace("@Price", tips(i)) + ", "
        'Next

        content = content.Replace("@Rule", rule)
        If isRichMember = True Then
            If stocks = "" Then
                content = content.Replace("@Stocks", "無滿足條件的股票")
            Else
                If showAd Then
                    content = content.Replace("@Stocks", stocks.TrimEnd.TrimEnd(",") + "<br/>" + ad)
                Else
                    content = content.Replace("@Stocks", stocks.TrimEnd.TrimEnd(","))
                End If
            End If
        Else
            content = content.Replace("@Stocks", "看全部股票請加入<a target=""_blank"" href =""/mall/shopping.aspx?id=2"">好野會員</a> or <a target=""_blank"" href =""/facebook/newRegister.aspx"">先免費註冊試用三天！</a>")
        End If

        Return content
    End Function


End Class
