Imports System.Data

Partial Class Hottip_HottipPage3
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        Me.LoadHottipGroupName() '載入群組名稱下拉選單

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not MemberDataAccessor.Instance.IsLogin Then
            Response.Redirect("/Login.aspx?GoBackUrl=" + Server.UrlEncode(Request.RawUrl))
            'Throw New Exception("會員未登入")
        End If

        If Not IsPostBack Then

            Dim memberNo As String = MemberDataAccessor.Instance.GetMemberNo()
            Dim group As String = Me.GetGroup

            sdsCollect.SelectParameters("MemberNo").DefaultValue = memberNo
            sdsCollect.SelectParameters("Group").DefaultValue = Val(group)

            lvMyCollect.DataSource = Me.GetData(memberNo, group)
            lvMyCollect.DataBind()

        End If

        '最下方廣告 是否顯示
        Mob_300x2501.Visible = Member.Instance.IsShowAd()

    End Sub

    Private Function GetData(memberNo As String, group As String) As DataTable
        Dim cacheKey As String = "Hottip_CollectPage3_DataTable" & memberNo & group
        If Cache(cacheKey) Is Nothing Then
            Cache.Add(cacheKey, Me.GetDataFromDB(memberNo, group), Nothing, DateTime.Now.AddMinutes(1), _
                      System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If
        Return TryCast(Cache(cacheKey), DataTable)
    End Function

    Private Function GetDataFromDB(ByVal memberNo As String, ByVal group As String) As DataTable
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Dim collectNos As New Generic.List(Of String)
        Dim stockNos As String = Me.GetCollectStock(collectNos)
        stockNos = stockNos.Replace("@DBName", "Stock")
        If stockNos.Trim.Length = 0 Then Return New DataTable

        'Exright從資料庫撈最近兩年的資料，但只取最新的一筆
        'MonthFinancial只取最新的一筆
        'Dim template As String = "SELECT TOP(1) Stock.StockNo, Stock.Name,  ISNULL(Stock.EPS,0) as EPS,  ISNULL(Stock.PriceVolume,0) as PriceVolume, ISNULL(Stock.NPTT,0)as NPTT, ISNULL(SUBSTRING(CONVERT(varchar(12),Exright.Date,102), 1,4),0) as Date1, ISNULL(Exright.FDRatio*10,0) as FDR, ISNULL(Exright.CashDividend,0)as CashDividend,case Stock.Deal when 0 then 0 else ( ISNULL(Exright.CashDividend,0) / Stock.Deal)*100 end AS CashDividendRate,  ISNULL(SUBSTRING(CONVERT(varchar(12),MonthFinancial.Date,111), 1,7),0) as Date2,   ISNULL(PreMonthRevenueDiff,0) as PreMonthRevenueDiff ,  ISNULL(PreYearMonthRevenueDiff,0)PreYearMonthRevenueDiff,  ISNULL(PreTotalRevenueDiff,0) as PreTotalRevenueDiff" & _
        '           " FROM Stock  Left JOIN Exright ON (Exright.StockNo = Stock.StockNo)" & _
        '           " Left JOIN MonthFinancial ON (Stock.StockNo = MonthFinancial.StockNo)" & _
        '           " Where Stock.StockNo='@StockNo' Order By Date1 desc, Date2 desc"
        Dim template As String = "exec [HottipCollectPage3] '@StockNo';"
        Dim cmd As String = ""
        For Each no As String In collectNos
            '擋掉不是台股的
            If IsNumeric(no) Then
                cmd &= template.Replace("@StockNo", no) & " "
            End If
        Next

        Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)

        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)

        Dim t As New DataTable
        Me.SetTableColumn(t)

        Dim lastNo As String = ""
        Dim lastYear As String = ""
        Dim currentNos As New Generic.List(Of String)
        For Each stockTable As DataTable In dataSet.Tables

            For Each row As Data.DataRow In stockTable.Rows

                If lastNo = row("StockNo") Then
                    Dim newFDR As Single = Val(t.Rows(t.Rows.Count - 1)("FDR")) '舊值
                    If Val(row("FDR")) > 0 Then newFDR = Val(row("FDR")) '新值

                    Dim newCashDividend As Single = Val(t.Rows(t.Rows.Count - 1)("CashDividend"))
                    If Val(row("CashDividend")) > 0 Then newCashDividend = Val(row("CashDividend"))

                    Dim newCashDividendRate As Single = Val(t.Rows(t.Rows.Count - 1)("CashDividendRate"))
                    If Val(row("CashDividendRate")) > 0 Then newCashDividendRate = Val(row("CashDividendRate"))

                    t.Rows.RemoveAt(t.Rows.Count - 1)

                    '同年的除權息日可能不同
                    If lastYear = row("Date1").ToString Then
                        '同年股利
                        t.Rows.Add(row("StockNo"), row("Name"), Val(row("EPS")), Val(row("PriceVolume")), Val(row("NPTT")) _
                                        , newFDR, newCashDividend, newCashDividendRate, Val(row("PreMonthRevenueDiff")) _
                                        , Val(row("PreYearMonthRevenueDiff")), Val(row("PreTotalRevenueDiff")), row("Date1"), row("Date2"))

                    Else
                        t.Rows.Add(row("StockNo"), row("Name"), Val(row("EPS")), Val(row("PriceVolume")), Val(row("NPTT")) _
                                        , Val(row("FDR")), Val(row("CashDividend")), Val(row("CashDividendRate")), Val(row("PreMonthRevenueDiff")) _
                                        , Val(row("PreYearMonthRevenueDiff")), Val(row("PreTotalRevenueDiff")), row("Date1"), row("Date2"))
                    End If

                Else
                    lastNo = row("StockNo")

                    t.Rows.Add(row("StockNo"), row("Name"), Val(row("EPS")), Val(row("PriceVolume")), Val(row("NPTT")) _
                                    , Val(row("FDR")), Val(row("CashDividend")), Val(row("CashDividendRate")), Val(row("PreMonthRevenueDiff")) _
                                    , Val(row("PreYearMonthRevenueDiff")), Val(row("PreTotalRevenueDiff")), row("Date1"), row("Date2"))
                End If

                If currentNos.Contains(row("StockNo")) = False Then currentNos.Add(row("StockNo"))

                lastYear = row("Date1").ToString
            Next
        Next

        command.Dispose()
        connection.Close()
        connection.Dispose()

        '根據個人追蹤的排序, 重新加入Table
        Dim sortTable As DataTable = Me.GetSortTable(t, currentNos, collectNos)
        Return sortTable
    End Function

    Private Sub SetTableColumn(t As DataTable)
        t.Columns.Add("StockNo")
        t.Columns.Add("Name")
        t.Columns.Add("EPS", System.Type.GetType("System.Single"))
        t.Columns.Add("PriceVolume", System.Type.GetType("System.Int32"))
        t.Columns.Add("NPTT", System.Type.GetType("System.Single"))
        t.Columns.Add("FDR", System.Type.GetType("System.Single"))
        t.Columns.Add("CashDividend", System.Type.GetType("System.Single"))
        t.Columns.Add("CashDividendRate", System.Type.GetType("System.Single"))
        t.Columns.Add("PreMonthRevenueDiff", System.Type.GetType("System.Single"))
        t.Columns.Add("PreYearMonthRevenueDiff", System.Type.GetType("System.Single"))
        t.Columns.Add("PreTotalRevenueDiff", System.Type.GetType("System.Single"))
        t.Columns.Add("Date1")
        t.Columns.Add("Date2")
    End Sub

    ''' <summary>
    ''' 根據個人追蹤的排序, 重新排序
    ''' </summary>
    Private Function GetSortTable(oldTable As DataTable, oldNos As Generic.List(Of String), sortNos As Generic.List(Of String)) As DataTable
        Dim sortTable As New DataTable
        For Each col As DataColumn In oldTable.Columns
            sortTable.Columns.Add(col.ColumnName, col.DataType)
        Next

        For Each collectNo As String In sortNos
            Dim index As Integer = oldNos.IndexOf(collectNo)
            If index > -1 Then
                Dim drTmp As DataRow = sortTable.NewRow()
                drTmp.ItemArray = oldTable.Rows(index).ItemArray
                sortTable.Rows.Add(drTmp)
            End If
        Next
        Return sortTable
    End Function

    Private Function GetCollectStock(ByRef nos As Generic.List(Of String)) As String
        Dim stockNos As String = ""
        For Each row As GridViewRow In gvNo.Rows
            Dim lblNo As Label = row.FindControl("lblNo")
            nos.Add(lblNo.Text)
            If stockNos = "" Then
                stockNos &= " @DBName.StockNo='" & lblNo.Text & "' "
            Else
                stockNos &= " Or @DBName.StockNo='" & lblNo.Text & "' "
            End If
        Next
        Return stockNos
    End Function

    Private Function GetGroup() As String
        Dim group As Integer = Val(Request("g"))
        If group = 0 Then group = 1
        Return group.ToString
    End Function

    ''' <summary>
    ''' 載入群組名稱下拉選單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadHottipGroupName()

        Dim groupNames As Generic.List(Of String) = Hottip.Instance.GetGroupName(MemberDataAccessor.Instance.GetMemberNo)

        ddlGroupList.Items.Clear()
        For index As Integer = 0 To groupNames.Count - 1
            Dim gName As String = groupNames(index)
            Dim gNameIndex As Integer = index + 1
            ddlGroupList.Items.Add(gName)
            ddlGroupList.Items(ddlGroupList.Items.Count - 1).Value = gNameIndex
        Next

        If Not String.IsNullOrEmpty(Request("g")) Then
            ddlGroupList.SelectedValue = Request("g")
        End If

    End Sub

    'Protected Sub btnMove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Protected Sub ddlGroup3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroupList.SelectedIndexChanged

        Dim ParamIndex As Integer = Request.RawUrl.IndexOf("?g=")
        If ParamIndex > 0 Then
            Response.Redirect(Request.RawUrl.Substring(0, ParamIndex) + "?g=" + ddlGroupList.SelectedValue)
        Else
            Response.Redirect(Request.RawUrl + "?g=" + ddlGroupList.SelectedValue)
        End If

    End Sub

    Protected Sub ddlFunctionList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFunctionList.SelectedIndexChanged
        Response.Redirect("HottipPage" + ddlFunctionList.SelectedValue + ".aspx?g=" + ddlGroupList.SelectedValue)
    End Sub

    Protected Sub lvMyCollect_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMyCollect.DataBound

        If lvMyCollect.Items.Count = 0 Then Exit Sub

        Dim collectStockNos As String = ""
        For Each row As ListViewDataItem In lvMyCollect.Items

            Dim ltlStockNo As Literal = row.FindControl("ltlStockNo")

        Next

    End Sub

End Class
