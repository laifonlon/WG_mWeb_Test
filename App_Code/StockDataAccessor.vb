Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data

Public Class StockDataAccessor

#Region "Singleton"
    Private Shared mObj As New StockDataAccessor
    Public Shared ReadOnly Property Instance() As StockDataAccessor
        Get
            Return mObj
        End Get
    End Property
#End Region

    Public Const RamDiskPath As String = "C:\WantGoo\Wantgoooooo1\Image\"
    Private mCompanyTable As New WantGoo.CompanyDataTable
 
    Public Sub New()
        GetDiagramUpdateIntervalFromDB()
        GetPriceLimitFromDB()
        UpdateCompanyData()

        HttpContext.Current.Application("StockUpdateTime") = Now.AddSeconds(-21)

        'UpdateBasicChart()

        'AddHandler mTimer.Tick, AddressOf TimerTickEventHandler
        'mTimer.Interval = mDiagramUpdateInterval * 1000
        'mTimer.Enabled = True
    End Sub

    Public Sub Update()
        Me.UpdateCompanyData()
    End Sub

    ''' <summary>
    ''' 更新公司股價相關資料
    ''' </summary>
    Private Sub UpdateCompanyData()

        If HttpContext.Current.Application("StockUpdateTime") IsNot Nothing Then
            Dim updateTime As Date = HttpContext.Current.Application("StockUpdateTime")
            If updateTime.AddSeconds(10) > Now Then Exit Sub
        End If

        HttpContext.Current.Application("StockUpdateTime") = Now

        Dim da As New WantGooTableAdapters.CompanyTableAdapter
        mCompanyTable.Clear()
        da.Fill(mCompanyTable)
    End Sub

#Region "系統設定"

    ''' <summary>
    ''' 圖片更新間隔時間
    ''' </summary>
    Private mDiagramUpdateInterval As Integer = 1
    ''' <summary>
    ''' 從資料庫取得圖片更新間隔時間
    ''' </summary>
    Private Sub GetDiagramUpdateIntervalFromDB()
        Dim connectionString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString
        Dim connection As New SqlClient.SqlConnection(connectionString)
        Dim command As New SqlClient.SqlCommand()
        Dim getUpdateInterval As String = "Select Value From SystemState WHERE (KeyName = N'DiagramUpdateInterval')"

        command.Connection = connection
        command.CommandText = getUpdateInterval

        Try
            connection.Open()
            mDiagramUpdateInterval = command.ExecuteScalar()
        Catch ex As Exception
        Finally
            connection.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 漲跌幅限制
    ''' </summary>
    Private mPriceLimit As Integer = 10

    ''' <summary>
    ''' 從資料庫取得漲跌幅限制
    ''' </summary>
    Private Sub GetPriceLimitFromDB()
        Dim connectionString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString
        Dim connection As New SqlClient.SqlConnection(connectionString)
        Dim command As New SqlClient.SqlCommand()
        Dim sql As String = "Select Value From SystemState WHERE (KeyName = N'PriceLimit')"

        command.Connection = connection
        command.CommandText = sql

        Try
            connection.Open()
            mPriceLimit = command.ExecuteScalar()
        Catch ex As Exception
        Finally
            connection.Close()
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' 取得股票代碼與名稱 1000 痿軟
    ''' </summary>
    Private mStockNos As ArrayList
    Public ReadOnly Property GetStockNos() As ArrayList
        Get
            If mStockNos Is Nothing Then mStockNos = New ArrayList
            If mStockNos.Count < 20 Then
                mStockNos.Clear()
                UpdateCompanyData()
                For Each row As WantGoo.CompanyRow In mCompanyTable.Rows
                    mStockNos.Add(row.StockNo.ToString + " " + row.Name)
                Next
            End If
            Return mStockNos
        End Get
    End Property

    ''' <summary>
    ''' 取得昨收價
    ''' </summary>
    Public Function GetLastPrice(ByVal stockNo As String) As Double
        Dim row As DataRow = mCompanyTable.Rows.Find(CInt(Val(stockNo)))
        If row Is Nothing Then Return 0
        Return Val(row("LastPrice"))
    End Function

    ''' <summary>
    ''' 取得漲停價
    ''' </summary>
    Public Function GetUpLimitPrice(ByVal stockNo As String) As Double
        Dim row As DataRow = mCompanyTable.Rows.Find(CInt(Val(stockNo)))
        If row Is Nothing Then Return 0

        Dim lastPrice As Double = Val(row("LastPrice"))
        Dim basicRangePrice As Double = GetBasicRangePrice(lastPrice)
        Dim price As Double = lastPrice * (mPriceLimit / 100) + lastPrice

        If basicRangePrice = 0 Then Return Val(Format(price, "0.00"))

        Return Val(Format(Math.Floor(price / basicRangePrice) * basicRangePrice, "0.00"))
    End Function

    ''' <summary>
    ''' 取得跌停價
    ''' </summary>
    Public Function GetDownLimitPrice(ByVal stockNo As String) As Double
        Dim row As DataRow = mCompanyTable.Rows.Find(CInt(Val(stockNo)))
        If row Is Nothing Then Return 0

        Dim lastPrice As Double = Val(row("LastPrice"))
        If lastPrice = 0 Then GetCurrentPrice(stockNo)
        Dim basicRangePrice As Double = GetBasicRangePrice(lastPrice)
        Dim price As Double = lastPrice * (-mPriceLimit / 100) + lastPrice

        If basicRangePrice = 0 Then Return Val(Format(price, "0.00"))

        Return Val(Format(Math.Ceiling(price / basicRangePrice) * basicRangePrice, "0.00"))
    End Function

    ''' <summary>
    ''' 取得目前成交價
    ''' </summary>
    Public Function GetCurrentPrice(ByVal stockNo As String) As Double
        UpdateCompanyData()
        Dim row As DataRow = mCompanyTable.Rows.Find(CInt(Val(stockNo)))
        If row Is Nothing Then Return 0
        Return Val(row("DealPrice"))
    End Function

    ''' <summary>
    ''' 取得基本跳動值
    ''' </summary>
    Public Function GetBasicRangePrice(ByVal price As Double) As Double
        If price > 1000 Then
            Return 5
        ElseIf price > 500 Then
            Return 1
        ElseIf price > 100 Then
            Return 0.5
        ElseIf price > 50 Then
            Return 0.1
        ElseIf price > 10 Then
            Return 0.05
        ElseIf price > 0 Then
            Return 0.01
        Else
            Return 0.01
        End If
    End Function

    ''' <summary>
    ''' 公司名稱 "痿軟"
    ''' </summary>
    Public Function GetCompanyName(ByVal stockNo As String) As String
        Dim row As DataRow = mCompanyTable.Rows.Find(CInt(Val(stockNo)))
        If row Is Nothing Then Return 0
        Dim value As String = CStr(row("Name"))
        Return value
    End Function

    ''' <summary>
    ''' 查詢指定資料數值
    ''' </summary>
    Public Function GetMarketValue(ByVal stockNo As String, ByVal key As String) As Double
        Dim row As DataRow = mCompanyTable.Rows.Find(CInt(Val(stockNo)))
        If row Is Nothing Then Return 0
        Dim value As Double = Val(row(key))
        Return value
    End Function

    ''' <summary>
    ''' 取得熱門前五名個股
    ''' </summary>
    Public Function GetTop5Stocks(ByVal type As Top5Type) As WantGoo.SelectTop5StocksDataTable
        Dim table As DataTable = Nothing

        If HttpContext.Current.Application("TopStockUpdateTime") IsNot Nothing Then
            Dim updateTime As Date = HttpContext.Current.Application("TopStockUpdateTime")

            Select Case type
                Case Top5Type.PriceFall
                    table = HttpContext.Current.Application("Top5StocksPriceFall")
                Case Top5Type.PriceRaise
                    table = HttpContext.Current.Application("Top5StocksPriceRaise")
                Case Top5Type.Volume
                    table = HttpContext.Current.Application("Top5StocksVolume")
            End Select

            If table IsNot Nothing AndAlso updateTime.AddSeconds(30) > Now Then Return table
        End If

        HttpContext.Current.Application("TopStockUpdateTime") = Now

        Dim mAdapter As New WantGooTableAdapters.SelectTop5StocksTableAdapter

        HttpContext.Current.Application("Top5StocksPriceFall") = mAdapter.GetData([Enum].GetName(GetType(Top5Type), Top5Type.PriceFall))
        HttpContext.Current.Application("Top5StocksPriceRaise") = mAdapter.GetData([Enum].GetName(GetType(Top5Type), Top5Type.PriceRaise))
        HttpContext.Current.Application("Top5StocksVolume") = mAdapter.GetData([Enum].GetName(GetType(Top5Type), Top5Type.Volume))

        Select Case type
            Case Top5Type.PriceFall
                table = HttpContext.Current.Application("Top5StocksPriceFall")
            Case Top5Type.PriceRaise
                table = HttpContext.Current.Application("Top5StocksPriceRaise")
            Case Top5Type.Volume
                table = HttpContext.Current.Application("Top5StocksVolume")
        End Select

        Return table
    End Function

#Region "玩股大盤指數"

    ''' <summary>
    ''' 取得玩股加權指數
    ''' </summary>
    Public Function GetWGIndex(ByRef openIndex As Double, _
                                           ByRef currentIndex As Double, _
                                           ByRef changeIndex As Double, _
                                           ByRef changeRatio As Double) As Boolean

        If HttpContext.Current.Application("WGIndexStockUpdateTime") IsNot Nothing Then
            Dim updateTime As Date = HttpContext.Current.Application("WGIndexStockUpdateTime")

            If updateTime.AddSeconds(30) > Now Then
                openIndex = HttpContext.Current.Application("OpenIndex")
                currentIndex = HttpContext.Current.Application("CurrentIndex")
                changeIndex = HttpContext.Current.Application("ChangeIndex")
                changeRatio = HttpContext.Current.Application("ChangeRatio")
                Return True
            End If
        End If

        HttpContext.Current.Application("WGIndexStockUpdateTime") = Now

        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetNowWGIndex(openIndex, currentIndex, changeIndex, changeRatio)

        HttpContext.Current.Application("OpenIndex") = openIndex
        HttpContext.Current.Application("CurrentIndex") = currentIndex
        HttpContext.Current.Application("ChangeIndex") = changeIndex
        HttpContext.Current.Application("ChangeRatio") = changeRatio
        Return True
    End Function

    ''' <summary>
    ''' 取得玩股指數資訊
    ''' </summary>
    Public Function GetWGStockInfo(ByRef buyCount As Integer, _
                                            ByRef buyTimes As Integer, _
                                            ByRef sellCount As Integer, _
                                            ByRef sellTimes As Integer, _
                                            ByRef dealCount As Integer, _
                                            ByRef dealTimes As Integer, _
                                            ByRef upCount As Integer, _
                                            ByRef limitUpCount As Integer, _
                                            ByRef downCount As Integer, _
                                            ByRef limitDownCount As Integer, _
                                            ByRef undealCount As Integer, _
                                            ByRef unchangedCount As Integer, _
                                            ByRef tradeVolume As Double) As Boolean

        If HttpContext.Current.Application("WGInfoStockUpdateTime") IsNot Nothing Then
            Dim updateTime As Date = HttpContext.Current.Application("WGInfoStockUpdateTime")

            If updateTime.AddSeconds(30) > Now Then
                buyCount = HttpContext.Current.Application("BuyCount")
                buyTimes = HttpContext.Current.Application("BuyTimes")
                sellCount = HttpContext.Current.Application("SellCount")
                sellTimes = HttpContext.Current.Application("SellTimes")
                dealCount = HttpContext.Current.Application("DealCount")
                dealTimes = HttpContext.Current.Application("DealTimes")
                upCount = HttpContext.Current.Application("UpCount")
                limitUpCount = HttpContext.Current.Application("LimitUpCount")
                downCount = HttpContext.Current.Application("DownCount")
                limitDownCount = HttpContext.Current.Application("LimitDownCount")
                undealCount = HttpContext.Current.Application("UndealCount")
                unchangedCount = HttpContext.Current.Application("UnchangedCount")
                tradeVolume = HttpContext.Current.Application("TradeVolume")
                Return True
            End If
        End If

        HttpContext.Current.Application("WGInfoStockUpdateTime") = Now

        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetWGStockInfo(buyCount, buyTimes, _
                                            sellCount, sellTimes, _
                                            dealCount, dealTimes, _
                                            upCount, limitUpCount, _
                                            downCount, limitDownCount, _
                                            undealCount, unchangedCount, _
                                            tradeVolume)

        HttpContext.Current.Application("BuyCount") = buyCount
        HttpContext.Current.Application("BuyTimes") = buyTimes
        HttpContext.Current.Application("SellCount") = sellCount
        HttpContext.Current.Application("SellTimes") = sellTimes
        HttpContext.Current.Application("DealCount") = dealCount
        HttpContext.Current.Application("DealTimes") = dealTimes
        HttpContext.Current.Application("UpCount") = upCount
        HttpContext.Current.Application("LimitUpCount") = limitUpCount
        HttpContext.Current.Application("DownCount") = downCount
        HttpContext.Current.Application("LimitDownCount") = limitDownCount
        HttpContext.Current.Application("UndealCount") = undealCount
        HttpContext.Current.Application("UnchangedCount") = unchangedCount
        HttpContext.Current.Application("TradeVolume") = tradeVolume
        Return True
    End Function

#End Region


    Public Enum Top5Type
        PriceRaise = 0
        PriceFall = 1
        Volume = 2
    End Enum

#Region "台股代號"
    Private mTWStockNos As Generic.List(Of String) = Nothing
    Public ReadOnly Property TWStockNos As Generic.List(Of String)
        Get
            If mTWStockNos Is Nothing Then Me.LoadTWStock()
            Return mTWStockNos
        End Get
    End Property

    Private mTWStockNames As Generic.List(Of String) = Nothing
    Public ReadOnly Property TWStockNames As Generic.List(Of String)
        Get
            If mTWStockNames Is Nothing Then Me.LoadTWStock()
            Return mTWStockNames
        End Get
    End Property

    Private Sub LoadTWStock()
        With HttpContext.Current
            If .Request.Url Is Nothing Then
                .Response.Redirect("\stock\LoadStocks.aspx?f=tw")
            Else
                .Response.Redirect("\stock\LoadStocks.aspx?f=tw&url=" + .Request.Url.ToString.Replace("&", "$"))
            End If
        End With
    End Sub

    Public Sub SetTWStock(nos As Generic.List(Of String), names As Generic.List(Of String))
        mTWStockNames = names
        mTWStockNos = nos
    End Sub
#End Region
     
    Public Function GetPrice(stockNo As String) As Single
        If HttpContext.Current.Application(stockNo + "_Deal") Is Nothing Then Me.LoadPrice(stockNo)
        Return HttpContext.Current.Application(stockNo + "_Deal")
    End Function
 
    Public Function GetChange(stockNo As String) As Single
        If HttpContext.Current.Application(stockNo + "_Change") Is Nothing Then Me.LoadPrice(stockNo)
        Return HttpContext.Current.Application(stockNo + "_Change")
    End Function
 
    Public Function GetChangeRatio(stockNo As String) As Single
        If HttpContext.Current.Application(stockNo + "_ChangeRatio") Is Nothing Then Me.LoadPrice(stockNo)
        Return HttpContext.Current.Application(stockNo + "_ChangeRatio")
    End Function

    Public Sub GetPrice(stockNo As String, ByRef price As Single, ByRef chage As Single, ByRef changeRatio As Single)
        price = HttpContext.Current.Application(stockNo + "_Deal")
        chage = HttpContext.Current.Application(stockNo + "_Change")
        changeRatio = HttpContext.Current.Application(stockNo + "_ChangeRatio")
    End Sub

    Public Sub SetPrice(stockNo As String, price As Single, chage As Single, changeRatio As Single)
        HttpContext.Current.Application(stockNo + "_Deal") = price
        HttpContext.Current.Application(stockNo + "_Change") = chage
        HttpContext.Current.Application(stockNo + "_ChangeRatio") = changeRatio
    End Sub

    Public Sub LoadPrice(stockNo As String)
        With HttpContext.Current
            If .Request.Url Is Nothing Then
                .Response.Redirect("\stock\LoadData.aspx?f=price&No=" & stockNo)
            Else
                .Response.Redirect("\stock\LoadData.aspx?f=price&No=" & stockNo & "&url=" + .Request.Url.ToString.Replace("&", "$"))
            End If
        End With
    End Sub
End Class
