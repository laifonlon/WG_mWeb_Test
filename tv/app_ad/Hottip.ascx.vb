Imports System.Data

Partial Class HomePage_Hottip
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Now.Hour < 9 Then
            sdsLastDate.SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 Order by Date Desc) t Order by Date desc"
        End If

        Select Case MemberDataAccessor.Instance.GetMemberLevel
            Case MemberLevel.Rich, MemberLevel.Super
                sdsLastDate.SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 1 Date FROM Selection1 Order by Date Desc) t Order by Date desc"
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
            lblDate.Text = Format(row("LastDate"), "MM/dd")
            btnQuery_Click(Nothing, Nothing)
        End If
    End Sub

    Private mStockSelectionTemplate As String = _
    "<tr>" + _
    "    <td class=""lft w25"" style=""color: #@Color;"" >@Rule</td>" + _
    "    <td class=""lft"">@Stocks</td>" + _
    "</tr>"

    Private mStockNameTemplate2 As String = _
    "<a href=""stock.aspx?no=@No"">@Name</a>"
    Private mStockSelection As String = ""
    Private mStockSelection_2 As String = ""

    ''' <summary>
    ''' 加入系統選股
    ''' </summary>
    Private Sub AddStockSelection(ByVal rule As String, ByVal direct As Direct, ByVal stockNames As Generic.List(Of String), ByVal stockNos As Generic.List(Of String))
        Dim content As String = mStockSelectionTemplate
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

    Private Sub AddStockSelection_2(ByVal rule As String, ByVal direct As Direct, ByVal stockNames As Generic.List(Of String), ByVal stockNos As Generic.List(Of String))
        Dim content As String = mStockSelectionTemplate
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

    'Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
    '    Calendar1.Visible = False
    '    tbxDate.Text = Format(Calendar1.SelectedDate, "yyyy/MM/dd")
    '    btnQuery_Click(Nothing, Nothing)
    'End Sub

    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnQuery.Click
        'Calendar1.Visible = False
        Query()

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

        'Select Case MemberDataAccessor.Instance.GetHottipLevel
        '    Case HottipLevel.Level1
        '        rules.Add(SelectionRule.ThroughInterval)
        '        rules.Add(SelectionRule.LargeVolume)
        '        rules.Add(SelectionRule.Through60Mean)
        '        rules.Add(SelectionRule.Swallow)
        '        rules.Add(SelectionRule.MeanCross)
        '    Case HottipLevel.Level2
        '        rules.Add(SelectionRule.LargeVolume)
        '        rules.Add(SelectionRule.Through60Mean)
        '        rules.Add(SelectionRule.MeanCross)
        '    Case HottipLevel.Level3
        '        rules.Add(SelectionRule.MeanCross)
        '    Case HottipLevel.Free
        '        rules.Add(SelectionRule.MeanCross)
        'End Select

        For Each rule As SelectionRule In rules
            stockNames.Clear()
            stockNos.Clear()
            For Each row As twStocks.Selection1Row In GetSelection(rule, 1)
                stockNames.Add(row.Name)
                If row.StockNo < 100 Then
                    stockNos.Add(row.StockNo.ToString)
                Else
                    stockNos.Add(row.StockNo.ToString)
                End If
            Next

            AddStockSelection(TwStockAccessor.Instance.GetRuleName_short(rule, Direct.Rise), Direct.Rise, stockNames, stockNos)
        Next

        lblContent.Text = mStockSelection
        '空
        'rules.Clear()
        'Select Case MemberDataAccessor.Instance.GetHottipLevel
        '    Case HottipLevel.Level1
        '        rules.Add(SelectionRule.ThroughInterval)
        '        rules.Add(SelectionRule.LargeVolume)
        '        rules.Add(SelectionRule.Through60Mean)
        '        rules.Add(SelectionRule.Swallow)
        '        rules.Add(SelectionRule.MeanCross)
        '    Case HottipLevel.Level2
        '        rules.Add(SelectionRule.LargeVolume)
        '        rules.Add(SelectionRule.Through60Mean)
        '    Case HottipLevel.Level3
        '    Case HottipLevel.Free
        'End Select

        For Each rule As SelectionRule In rules
            stockNames.Clear()
            stockNos.Clear()
            For Each row As twStocks.Selection1Row In GetSelection(rule, 0)
                stockNames.Add(row.Name)
                If row.StockNo < 100 Then
                    stockNos.Add(row.StockNo.ToString)
                Else
                    stockNos.Add(row.StockNo.ToString)
                End If
            Next
            AddStockSelection_2(TwStockAccessor.Instance.GetRuleName_short(rule, Direct.Fall), Direct.Fall, stockNames, stockNos)
        Next

        lblContent2.Text = mStockSelection_2
    End Sub

    'Protected Sub btnSelectDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectDate.Click
    '    Calendar1.Visible = True
    '    Calendar1.SelectedDate = Today
    'End Sub

    Private mSelectionTable As twStocks.Selection1DataTable
    Private Sub Query()
        mSelectionTable = TwStockAccessor.Instance.Query("#" + lblDate.Text + "#")
    End Sub
    Public Function GetSelection(ByVal rule As SelectionRule, ByVal direction As Integer) As twStocks.Selection1Row()
        Return mSelectionTable.Select("[Is" + [Enum].GetName(GetType(SelectionRule), rule) + "] = 1 And [Direction] = " + direction.ToString)
    End Function

End Class