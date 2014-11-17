Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Web.Script.Serialization

' 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class MobileGetData
    Inherits System.Web.Services.WebService

    Private twStockConnectString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
    Private WantGooConnection As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function


#Region "「飆股搜尋」"

    '<WebMethod(enableSession:=True, CacheDuration:=30)> _
    <WebMethod(enableSession:=True)> _
    Public Sub Hottip()

        Dim cacheTimeFlag_Member_Second As Integer = 10
        Dim cacheTimeFlag_DataType_Second As Integer = 20

        '設定輸出格式為json格式
        Me.Context.Response.ContentType = "application/json"

        '暫存 by MemberNo
        Dim cacheKey As String = String.Empty
        Dim cacheKey_TimeFlag As String = String.Empty
        cacheKey = "MobileHottip_" + MemberDataAccessor.Instance.GetMemberNo
        cacheKey_TimeFlag = "MobileHottipTimeFlag_" + MemberDataAccessor.Instance.GetMemberNo
        If Application(cacheKey_TimeFlag) IsNot Nothing Then
            If Application(cacheKey_TimeFlag) <= Now Then
                Application(cacheKey) = Nothing
                Application(cacheKey_TimeFlag) = Now.AddSeconds(cacheTimeFlag_Member_Second)
            End If
        End If

        '暫存 by MemberNo
        If Application(cacheKey) IsNot Nothing And Application(cacheKey_TimeFlag) IsNot Nothing Then

            '從Application取得資料
            '暫存 by MemberNo
            Me.Context.Response.Write(Application(cacheKey))

        Else

            '從邏輯取得資料

            Dim sdsNewestDate_SelectCommand As String = "SELECT MAX(Date) AS LastDate FROM Selection1 With(NoLock)"
            Dim sdsLastDate_SelectCommand As String = "Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 With(NoLock) Order by Date Desc) t Order by Date"
            Dim exec_SelectCommand As String = String.Empty

            '早上9點之前，則取前一天的資料
            If Now.Hour < 9 Then
                sdsLastDate_SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 With(NoLock) Order by Date Desc) t Order by Date desc"
            End If

            '暫存 by DataType
            Dim cacheKey_DataType As String = "MobileHottip_Normal"
            Dim cacheKey_DataType_TimeFlag As String = "MobileHottip_Normal_TimeFlag"

            Select Case MemberDataAccessor.Instance.GetMemberLevel
                '是否為儲值會員
                Case MemberLevel.Rich, MemberLevel.Super
                    sdsLastDate_SelectCommand = "Select Top 1 Date as LastDate From (SELECT distinct top 1 Date FROM Selection1 With(NoLock) Order by Date Desc) t Order by Date desc"
                    '暫存 by DataType
                    cacheKey_DataType = "MobileHottip_Rich"
                    cacheKey_DataType_TimeFlag = "MobileHottip_Rich_TimeFlag"
            End Select

            '是否為假日
            If Now.DayOfWeek = DayOfWeek.Saturday OrElse _
                Now.DayOfWeek = DayOfWeek.Sunday OrElse _
               Val(Format(Now, "HH")) > 14 Then
                exec_SelectCommand = sdsNewestDate_SelectCommand
                cacheKey_DataType = "MobileHottip_Holiday"
                cacheKey_DataType_TimeFlag = "MobileHottip_Holiday_TimeFlag"
            Else
                exec_SelectCommand = sdsLastDate_SelectCommand
            End If

            If Application(cacheKey_DataType_TimeFlag) IsNot Nothing Then
                If Application(cacheKey_DataType_TimeFlag) <= Now Then
                    '暫存 by DataType
                    Application(cacheKey_DataType) = Nothing
                    Application(cacheKey_DataType_TimeFlag) = Now.AddSeconds(cacheTimeFlag_DataType_Second)
                End If
            End If

            Dim HottipDateText As String = String.Empty
            Dim HottipGood As String = String.Empty
            Dim HottipBad As String = String.Empty

            Dim jsonList As New Generic.Dictionary(Of String, String)

            '暫存 by DataType
            If Application(cacheKey_DataType) IsNot Nothing And Application(cacheKey_DataType_TimeFlag) IsNot Nothing Then

                '從Application取得資料
                '暫存 by DataType
                jsonList = Application(cacheKey_DataType)

            Else

                '從DB取得資料

                Dim connection As New System.Data.SqlClient.SqlConnection(twStockConnectString)
                connection.Open()
                Try

                    Dim sqlcmd As String = exec_SelectCommand
                    Dim command As New System.Data.SqlClient.SqlCommand(sqlcmd, connection)

                    Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

                    Try
                        While reader.Read

                            'lblHottipDate.Text = Format(reader.Item("LastDate"), "yyyy-MM-dd")
                            HottipDateText = Format(reader.Item("LastDate"), "yyyy-MM-dd")
                            Hottip_btnQuery_Click(HottipDateText, HottipGood, HottipBad)

                            jsonList.Add("HottipGood", HottipGood)
                            jsonList.Add("HottipBad", HottipBad)

                            '暫存 by DataType
                            Application(cacheKey_DataType) = jsonList
                            Application(cacheKey_DataType_TimeFlag) = Now.AddSeconds(60)

                        End While
                    Catch ex As Exception
                        'Debug.WriteDB("Exception GetGoodTwStock", "", ex.Message, ex.StackTrace)
                    Finally
                        reader.Close()
                        command.Dispose()
                    End Try

                Catch ex As Exception
                    'Debug.WriteDB("Exception GetGoodTwStock", "", ex.Message, ex.StackTrace)
                Finally
                    connection.Close()
                    connection.Dispose()
                End Try

            End If

            'Dim jsonList As New Generic.Dictionary(Of String, String)
            'jsonList.Add("HottipGood", HottipGood)
            'jsonList.Add("HottipBad", HottipBad)

            '輸出json格式
            Dim serializer As New JavaScriptSerializer()
            Dim jsonString As String = serializer.Serialize(jsonList)

            '暫存 by MemberNo
            Application(cacheKey) = jsonString
            Application(cacheKey_TimeFlag) = Now.AddSeconds(30)

            Me.Context.Response.Write(jsonString)

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

    Protected Sub Hottip_btnQuery_Click(HottipDateText As String, ByRef HottipGood As String, ByRef HottipBad As String)
        HottipQuery(HottipDateText)

        '加入系統選股內容
        Dim stockNames As New Generic.List(Of String)
        Dim stockNos As New Generic.List(Of String)

        '多
        Dim rules As New Generic.List(Of SelectionRule)
        rules.Add(SelectionRule.ThroughInterval) '突破整理區間
        rules.Add(SelectionRule.LargeVolume) '爆量長紅
        rules.Add(SelectionRule.Through60Mean) '突破季線
        'rules.Add(SelectionRule.Swallow) '多頭吞噬
        rules.Add(SelectionRule.KDCross) 'KD黃金交叉
        rules.Add(SelectionRule.MeanCross) '5與20日均線黃金交叉

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
        HottipGood = mStockSelection

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
        HottipBad = mStockSelection_2

    End Sub

    Private HottipSelectionTable As twStocks.Selection1DataTable
    Private Sub HottipQuery(HottipDateText As String)
        HottipSelectionTable = TwStockAccessor.Instance.Query("#" + HottipDateText + "#")
    End Sub

    Public Function HottipGetSelection(ByVal rule As SelectionRule, ByVal direction As Integer) As twStocks.Selection1Row()
        Return HottipSelectionTable.Select("[Is" + [Enum].GetName(GetType(SelectionRule), rule) + "] = 1 And [Direction] = " + direction.ToString)
    End Function

#End Region
End Class