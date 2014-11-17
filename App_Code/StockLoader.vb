Imports Microsoft.VisualBasic

Public Class StockLoader
#Region "Singleton"
    Private Shared mObj As New StockLoader
    Public Shared ReadOnly Property Instance() As StockLoader
        Get
            Return mObj
        End Get
    End Property
#End Region

    Private twStockConnectString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString

    Public Function GetName(ByVal stockNo As String) As String
        stockNo = stockNo.Replace("$", "&")
        Return HttpContext.Current.Application(stockNo.Trim + "_Name")
    End Function

    Public Sub LoadStockName()
        Dim key As String = ""
        Dim sdsStockName As New SqlDataSource
        sdsStockName.ConnectionString = twStockConnectString
        sdsStockName.SelectCommand = "SELECT Name,StockNo,EnglishFullName From Stock"
        Dim en As IEnumerator = sdsStockName.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            HttpContext.Current.Application(en.Current("StockNo").ToString.Trim + "_Name") = en.Current("Name")
            If IsDBNull(en.Current("EnglishFullName")) = True Then
                HttpContext.Current.Application(en.Current("StockNo").ToString.Trim + "_EnglishFullName") = ""
            Else
                HttpContext.Current.Application(en.Current("StockNo").ToString.Trim + "_EnglishFullName") = en.Current("EnglishFullName")
            End If
        End While
    End Sub

    ''' <summary>
    ''' 個股技術日K
    ''' </summary>
    Public Sub LoadTech5M(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_Tech5M_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
             Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sb0000 As New StringBuilder
        Dim sbVolume As New StringBuilder
        Dim sbMean5Volume As New StringBuilder
        Dim sbMean20Volume As New StringBuilder
        Dim sbMean5 As New StringBuilder
        Dim sbMean10 As New StringBuilder
        Dim sbMean20 As New StringBuilder
        Dim sbMean60 As New StringBuilder
        Dim sbMean120 As New StringBuilder
        Dim sbMean240 As New StringBuilder
        Dim sbK9 As New StringBuilder
        Dim sbD9 As New StringBuilder
        Dim sbStrength1 As New StringBuilder
        Dim sbStrength2 As New StringBuilder
        Dim sbStrength3 As New StringBuilder
        Dim sbStrength4 As New StringBuilder
        Dim sbStrength5 As New StringBuilder
        Dim sbMa3Ma6 As New StringBuilder
        Dim ma3Ma6 As New Queue
        Dim close As New Queue
        Dim close0 As Double
        Dim close3 As Double
        Dim currentDate As Date

        Dim sdsTech5M As New SqlDataSource
        sdsTech5M.ConnectionString = twStockConnectString
        sdsTech5M.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-150,getdate()) Order by [Date]"
        sdsTech5M.SelectCommand = sdsTech5M.SelectCommand.Replace("@StockNo", stockNo)
        Dim en As IEnumerator = sdsTech5M.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String

        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            sbVolume.Append("[")
            sbVolume.Append(span)
            sbVolume.Append(",")
            If IsNumeric(stockNo) Then
                sbVolume.Append(CInt(en.Current("Volume") / 1000))
            Else
                sbVolume.Append(en.Current("Volume"))
            End If
            sbVolume.Append("],")

            If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
                Else
                    sbMean5Volume.Append(en.Current("Mean5Volume"))
                End If
                sbMean5Volume.Append("],")
            Else
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",0],")
            End If

            If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
                Else
                    sbMean20Volume.Append(en.Current("Mean20Volume"))
                End If
                sbMean20Volume.Append("],")
            Else
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",0],")
            End If

            sb0000.Append("[")
            sb0000.Append(span)
            sb0000.Append(",")
            sb0000.Append(en.Current("Open"))
            sb0000.Append(",")
            sb0000.Append(en.Current("High"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Low"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Close"))
            sb0000.Append("],")

            If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
                sbMean5.Append("[")
                sbMean5.Append(span)
                sbMean5.Append(",")
                sbMean5.Append(Format(en.Current("mean5"), "0.00"))
                sbMean5.Append("],")
            End If

            If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
                sbMean10.Append("[")
                sbMean10.Append(span)
                sbMean10.Append(",")
                sbMean10.Append(Format(en.Current("mean10"), "0.00"))
                sbMean10.Append("],")
            End If

            If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
                sbMean20.Append("[")
                sbMean20.Append(span)
                sbMean20.Append(",")
                sbMean20.Append(Format(en.Current("mean20"), "0.00"))
                sbMean20.Append("],")
            End If

            If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
                sbMean60.Append("[")
                sbMean60.Append(span)
                sbMean60.Append(",")
                sbMean60.Append(Format(en.Current("mean60"), "0.00"))
                sbMean60.Append("],")
            End If

            If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
                sbMean120.Append("[")
                sbMean120.Append(span)
                sbMean120.Append(",")
                sbMean120.Append(Format(en.Current("mean120"), "0.00"))
                sbMean120.Append("],")
            End If

            If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
                sbMean240.Append("[")
                sbMean240.Append(span)
                sbMean240.Append(",")
                sbMean240.Append(Format(en.Current("mean240"), "0.00"))
                sbMean240.Append("],")
            End If

            If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
                sbK9.Append("[")
                sbK9.Append(span)
                sbK9.Append(",")
                sbK9.Append(Format(en.Current("K9"), "0.0"))
                sbK9.Append("],")
            End If

            If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
                sbD9.Append("[")
                sbD9.Append(span)
                sbD9.Append(",")
                sbD9.Append(Format(en.Current("D9"), "0.0"))
                sbD9.Append("],")
            End If

            If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
                sbStrength1.Append("[")
                sbStrength1.Append(span)
                sbStrength1.Append(",")
                sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
                sbStrength1.Append("],")
            End If

            If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
                sbStrength2.Append("[")
                sbStrength2.Append(span)
                sbStrength2.Append(",")
                sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
                sbStrength2.Append("],")
            End If

            If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
                sbStrength3.Append("[")
                sbStrength3.Append(span)
                sbStrength3.Append(",")
                sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
                sbStrength3.Append("],")
            End If

            If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
                sbStrength4.Append("[")
                sbStrength4.Append(span)
                sbStrength4.Append(",")
                sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
                sbStrength4.Append("],")
            End If

            If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
                sbStrength5.Append("[")
                sbStrength5.Append(span)
                sbStrength5.Append(",")
                sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
                sbStrength5.Append("],")
            End If

            'Ma3 - Ma6
            close.Enqueue(en.Current("Close"))
            If close.Count >= 4 Then
                close0 = en.Current("Close")
                close3 = close.Dequeue
                ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
            End If
            If ma3Ma6.Count > 3 Then
                sbMa3Ma6.Append("[")
                sbMa3Ma6.Append(span)
                sbMa3Ma6.Append(",")
                sbMa3Ma6.Append(ma3Ma6.Dequeue)
                sbMa3Ma6.Append("],")
            End If
        End While

        For i As Integer = 1 To 3
            If ma3Ma6.Count = 0 Then Exit For
            sbMa3Ma6.Append("[")
            sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
            sbMa3Ma6.Append(",")
            sbMa3Ma6.Append(ma3Ma6.Dequeue)
            sbMa3Ma6.Append("],")
        Next

        With HttpContext.Current
            .Application(stockNo + "_Volume_5M") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0],[" + Now.AddDays(2).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_Mean5Volume_5M") = sbMean5Volume.ToString.Trim(",")
            .Application(stockNo + "_Mean20Volume_5M") = sbMean20Volume.ToString.Trim(",")
            .Application(stockNo + "_DailyK_5M") = sb0000.ToString.Trim(",")
            .Application(stockNo + "_Mean5_5M") = sbMean5.ToString.Trim(",")
            .Application(stockNo + "_Mean10_5M") = sbMean10.ToString.Trim(",")
            .Application(stockNo + "_Mean20_5M") = sbMean20.ToString.Trim(",")
            .Application(stockNo + "_Mean60_5M") = sbMean60.ToString.Trim(",")
            .Application(stockNo + "_Mean120_5M") = sbMean120.ToString.Trim(",")
            .Application(stockNo + "_Mean240_5M") = sbMean240.ToString.Trim(",")
            .Application(stockNo + "_K9_5M") = sbK9.ToString.Trim(",")
            .Application(stockNo + "_D9_5M") = sbD9.ToString.Trim(",")
            .Application(stockNo + "_Strength1_5M") = sbStrength1.ToString.Trim(",")
            .Application(stockNo + "_Strength2_5M") = sbStrength2.ToString.Trim(",")
            .Application(stockNo + "_Strength3_5M") = sbStrength3.ToString.Trim(",")
            .Application(stockNo + "_Strength4_5M") = sbStrength4.ToString.Trim(",")
            .Application(stockNo + "_Strength5_5M") = sbStrength5.ToString.Trim(",")
            .Application(stockNo + "_Ma3Ma6_5M") = sbMa3Ma6.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

        Dim sdsDeduct As New SqlDataSource
        sdsDeduct.ConnectionString = twStockConnectString
        sdsDeduct.SelectCommand = "Select Top 1 [Date],[Close] From (SELECT Top 20 [Date],[Close] From HistoryPriceDaily WHERE StockNo = '@StockNo' Order by [Date] Desc) t Order by [Date]"
        sdsDeduct.SelectCommand = sdsDeduct.SelectCommand.Replace("@StockNo", stockNo)
        en = sdsDeduct.Select(New DataSourceSelectArguments).GetEnumerator
        If en.MoveNext Then
            HttpContext.Current.Application(stockNo + "_Deduct20MA") = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString
            HttpContext.Current.Application(stockNo + "_Deduct20MAValue") = Format(en.Current("Close"), "0.00").Replace(".00", "").Trim("0")
        End If
        sdsDeduct.SelectCommand = "Select Top 1 [Date],[Close] From (SELECT Top 60 [Date],[Close] From HistoryPriceDaily WHERE StockNo = '@StockNo' Order by [Date] Desc) t Order by [Date]"
        sdsDeduct.SelectCommand = sdsDeduct.SelectCommand.Replace("@StockNo", stockNo)
        en = sdsDeduct.Select(New DataSourceSelectArguments).GetEnumerator
        If en.MoveNext Then
            HttpContext.Current.Application(stockNo + "_Deduct60MA") = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString
            HttpContext.Current.Application(stockNo + "_Deduct60MAValue") = Format(en.Current("Close"), "0.00").Replace(".00", "").Trim("0")
        End If

    End Sub

    Public Sub LoadTech6M(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_Tech6M_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
             Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sb0000 As New StringBuilder
        Dim sbVolume As New StringBuilder
        Dim sbMean5Volume As New StringBuilder
        Dim sbMean20Volume As New StringBuilder
        Dim sbMean5 As New StringBuilder
        Dim sbMean10 As New StringBuilder
        Dim sbMean20 As New StringBuilder
        Dim sbMean60 As New StringBuilder
        Dim sbMean120 As New StringBuilder
        Dim sbMean240 As New StringBuilder
        Dim sbK9 As New StringBuilder
        Dim sbD9 As New StringBuilder
        Dim sbStrength1 As New StringBuilder
        Dim sbStrength2 As New StringBuilder
        Dim sbStrength3 As New StringBuilder
        Dim sbStrength4 As New StringBuilder
        Dim sbStrength5 As New StringBuilder
        Dim sbMa3Ma6 As New StringBuilder
        Dim ma3Ma6 As New Queue
        Dim close As New Queue
        Dim close0 As Double
        Dim close3 As Double
        Dim currentDate As Date

        Dim sdsTech6M As New SqlDataSource
        sdsTech6M.ConnectionString = twStockConnectString
        sdsTech6M.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-180,getdate()) Order by [Date]"
        sdsTech6M.SelectCommand = sdsTech6M.SelectCommand.Replace("@StockNo", stockNo)
        Dim en As IEnumerator = sdsTech6M.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String

        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            sbVolume.Append("[")
            sbVolume.Append(span)
            sbVolume.Append(",")
            If IsNumeric(stockNo) Then
                sbVolume.Append(CInt(en.Current("Volume") / 1000))
            Else
                sbVolume.Append(en.Current("Volume"))
            End If
            sbVolume.Append("],")

            If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
                Else
                    sbMean5Volume.Append(en.Current("Mean5Volume"))
                End If
                sbMean5Volume.Append("],")
            Else
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",0],")
            End If

            If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
                Else
                    sbMean20Volume.Append(en.Current("Mean20Volume"))
                End If
                sbMean20Volume.Append("],")
            Else
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",0],")
            End If

            sb0000.Append("[")
            sb0000.Append(span)
            sb0000.Append(",")
            sb0000.Append(en.Current("Open"))
            sb0000.Append(",")
            sb0000.Append(en.Current("High"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Low"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Close"))
            sb0000.Append("],")

            If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
                sbMean5.Append("[")
                sbMean5.Append(span)
                sbMean5.Append(",")
                sbMean5.Append(Format(en.Current("mean5"), "0.00"))
                sbMean5.Append("],")
            End If

            If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
                sbMean10.Append("[")
                sbMean10.Append(span)
                sbMean10.Append(",")
                sbMean10.Append(Format(en.Current("mean10"), "0.00"))
                sbMean10.Append("],")
            End If

            If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
                sbMean20.Append("[")
                sbMean20.Append(span)
                sbMean20.Append(",")
                sbMean20.Append(Format(en.Current("mean20"), "0.00"))
                sbMean20.Append("],")
            End If

            If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
                sbMean60.Append("[")
                sbMean60.Append(span)
                sbMean60.Append(",")
                sbMean60.Append(Format(en.Current("mean60"), "0.00"))
                sbMean60.Append("],")
            End If

            If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
                sbMean120.Append("[")
                sbMean120.Append(span)
                sbMean120.Append(",")
                sbMean120.Append(Format(en.Current("mean120"), "0.00"))
                sbMean120.Append("],")
            End If

            If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
                sbMean240.Append("[")
                sbMean240.Append(span)
                sbMean240.Append(",")
                sbMean240.Append(Format(en.Current("mean240"), "0.00"))
                sbMean240.Append("],")
            End If

            If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
                sbK9.Append("[")
                sbK9.Append(span)
                sbK9.Append(",")
                sbK9.Append(Format(en.Current("K9"), "0.0"))
                sbK9.Append("],")
            End If

            If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
                sbD9.Append("[")
                sbD9.Append(span)
                sbD9.Append(",")
                sbD9.Append(Format(en.Current("D9"), "0.0"))
                sbD9.Append("],")
            End If

            If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
                sbStrength1.Append("[")
                sbStrength1.Append(span)
                sbStrength1.Append(",")
                sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
                sbStrength1.Append("],")
            End If

            If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
                sbStrength2.Append("[")
                sbStrength2.Append(span)
                sbStrength2.Append(",")
                sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
                sbStrength2.Append("],")
            End If

            If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
                sbStrength3.Append("[")
                sbStrength3.Append(span)
                sbStrength3.Append(",")
                sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
                sbStrength3.Append("],")
            End If

            If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
                sbStrength4.Append("[")
                sbStrength4.Append(span)
                sbStrength4.Append(",")
                sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
                sbStrength4.Append("],")
            End If

            If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
                sbStrength5.Append("[")
                sbStrength5.Append(span)
                sbStrength5.Append(",")
                sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
                sbStrength5.Append("],")
            End If

            'Ma3 - Ma6
            close.Enqueue(en.Current("Close"))
            If close.Count >= 4 Then
                close0 = en.Current("Close")
                close3 = close.Dequeue
                ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
            End If
            If ma3Ma6.Count > 3 Then
                sbMa3Ma6.Append("[")
                sbMa3Ma6.Append(span)
                sbMa3Ma6.Append(",")
                sbMa3Ma6.Append(ma3Ma6.Dequeue)
                sbMa3Ma6.Append("],")
            End If
        End While

        For i As Integer = 1 To 3
            If ma3Ma6.Count = 0 Then Exit For
            sbMa3Ma6.Append("[")
            sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
            sbMa3Ma6.Append(",")
            sbMa3Ma6.Append(ma3Ma6.Dequeue)
            sbMa3Ma6.Append("],")
        Next

        With HttpContext.Current
            .Application(stockNo + "_Volume_6M") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0],[" + Now.AddDays(2).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_Mean5Volume_6M") = sbMean5Volume.ToString.Trim(",")
            .Application(stockNo + "_Mean20Volume_6M") = sbMean20Volume.ToString.Trim(",")
            .Application(stockNo + "_DailyK_6M") = sb0000.ToString.Trim(",")
            .Application(stockNo + "_Mean5_6M") = sbMean5.ToString.Trim(",")
            .Application(stockNo + "_Mean10_6M") = sbMean10.ToString.Trim(",")
            .Application(stockNo + "_Mean20_6M") = sbMean20.ToString.Trim(",")
            .Application(stockNo + "_Mean60_6M") = sbMean60.ToString.Trim(",")
            .Application(stockNo + "_Mean120_6M") = sbMean120.ToString.Trim(",")
            .Application(stockNo + "_Mean240_6M") = sbMean240.ToString.Trim(",")
            .Application(stockNo + "_K9_6M") = sbK9.ToString.Trim(",")
            .Application(stockNo + "_D9_6M") = sbD9.ToString.Trim(",")
            .Application(stockNo + "_Strength1_6M") = sbStrength1.ToString.Trim(",")
            .Application(stockNo + "_Strength2_6M") = sbStrength2.ToString.Trim(",")
            .Application(stockNo + "_Strength3_6M") = sbStrength3.ToString.Trim(",")
            .Application(stockNo + "_Strength4_6M") = sbStrength4.ToString.Trim(",")
            .Application(stockNo + "_Strength5_6M") = sbStrength5.ToString.Trim(",")
            .Application(stockNo + "_Ma3Ma6_6M") = sbMa3Ma6.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

    End Sub

    Public Sub LoadTech1Y(ByVal stockNo As String)
        LoadTechDaily(stockNo, 1)
        'Dim timeKey As String = stockNo + "_Tech1Y_LastTime"

        'If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
        '     Now < HttpContext.Current.Application(timeKey) Then
        '    Exit Sub
        'End If

        'Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        'Dim sb0000 As New StringBuilder
        'Dim sbVolume As New StringBuilder
        'Dim sbMean5Volume As New StringBuilder
        'Dim sbMean20Volume As New StringBuilder
        'Dim sbMean5 As New StringBuilder
        'Dim sbMean10 As New StringBuilder
        'Dim sbMean20 As New StringBuilder
        'Dim sbMean60 As New StringBuilder
        'Dim sbMean120 As New StringBuilder
        'Dim sbMean240 As New StringBuilder
        'Dim sbK9 As New StringBuilder
        'Dim sbD9 As New StringBuilder
        'Dim sbStrength1 As New StringBuilder
        'Dim sbStrength2 As New StringBuilder
        'Dim sbStrength3 As New StringBuilder
        'Dim sbStrength4 As New StringBuilder
        'Dim sbStrength5 As New StringBuilder
        'Dim sbMa3Ma6 As New StringBuilder
        'Dim ma3Ma6 As New Queue
        'Dim close As New Queue
        'Dim close0 As Double
        'Dim close3 As Double
        'Dim currentDate As Date

        'Dim sdsTech1Y As New SqlDataSource
        'sdsTech1Y.ConnectionString = twStockConnectString
        'sdsTech1Y.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-366,getdate()) Order by [Date]"
        'sdsTech1Y.SelectCommand = sdsTech1Y.SelectCommand.Replace("@StockNo", stockNo)
        'Dim en As IEnumerator = sdsTech1Y.Select(New DataSourceSelectArguments).GetEnumerator
        'Dim span As String

        'While en.MoveNext
        '    span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

        '    sbVolume.Append("[")
        '    sbVolume.Append(span)
        '    sbVolume.Append(",")
        '    If IsNumeric(stockNo) Then
        '        sbVolume.Append(CInt(en.Current("Volume") / 1000))
        '    Else
        '        sbVolume.Append(en.Current("Volume"))
        '    End If
        '    sbVolume.Append("],")

        '    If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
        '        Else
        '            sbMean5Volume.Append(en.Current("Mean5Volume"))
        '        End If
        '        sbMean5Volume.Append("],")
        '    Else
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",0],")
        '    End If

        '    If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
        '        Else
        '            sbMean20Volume.Append(en.Current("Mean20Volume"))
        '        End If
        '        sbMean20Volume.Append("],")
        '    Else
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",0],")
        '    End If

        '    sb0000.Append("[")
        '    sb0000.Append(span)
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Open"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("High"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Low"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Close"))
        '    sb0000.Append("],")

        '    If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
        '        sbMean5.Append("[")
        '        sbMean5.Append(span)
        '        sbMean5.Append(",")
        '        sbMean5.Append(Format(en.Current("mean5"), "0.00"))
        '        sbMean5.Append("],")
        '    End If

        '    If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
        '        sbMean10.Append("[")
        '        sbMean10.Append(span)
        '        sbMean10.Append(",")
        '        sbMean10.Append(Format(en.Current("mean10"), "0.00"))
        '        sbMean10.Append("],")
        '    End If

        '    If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
        '        sbMean20.Append("[")
        '        sbMean20.Append(span)
        '        sbMean20.Append(",")
        '        sbMean20.Append(Format(en.Current("mean20"), "0.00"))
        '        sbMean20.Append("],")
        '    End If

        '    If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
        '        sbMean60.Append("[")
        '        sbMean60.Append(span)
        '        sbMean60.Append(",")
        '        sbMean60.Append(Format(en.Current("mean60"), "0.00"))
        '        sbMean60.Append("],")
        '    End If

        '    If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
        '        sbMean120.Append("[")
        '        sbMean120.Append(span)
        '        sbMean120.Append(",")
        '        sbMean120.Append(Format(en.Current("mean120"), "0.00"))
        '        sbMean120.Append("],")
        '    End If

        '    If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
        '        sbMean240.Append("[")
        '        sbMean240.Append(span)
        '        sbMean240.Append(",")
        '        sbMean240.Append(Format(en.Current("mean240"), "0.00"))
        '        sbMean240.Append("],")
        '    End If

        '    If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
        '        sbK9.Append("[")
        '        sbK9.Append(span)
        '        sbK9.Append(",")
        '        sbK9.Append(Format(en.Current("K9"), "0.0"))
        '        sbK9.Append("],")
        '    End If

        '    If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
        '        sbD9.Append("[")
        '        sbD9.Append(span)
        '        sbD9.Append(",")
        '        sbD9.Append(Format(en.Current("D9"), "0.0"))
        '        sbD9.Append("],")
        '    End If

        '    If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
        '        sbStrength1.Append("[")
        '        sbStrength1.Append(span)
        '        sbStrength1.Append(",")
        '        sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
        '        sbStrength1.Append("],")
        '    End If

        '    If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
        '        sbStrength2.Append("[")
        '        sbStrength2.Append(span)
        '        sbStrength2.Append(",")
        '        sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
        '        sbStrength2.Append("],")
        '    End If

        '    If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
        '        sbStrength3.Append("[")
        '        sbStrength3.Append(span)
        '        sbStrength3.Append(",")
        '        sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
        '        sbStrength3.Append("],")
        '    End If

        '    If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
        '        sbStrength4.Append("[")
        '        sbStrength4.Append(span)
        '        sbStrength4.Append(",")
        '        sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
        '        sbStrength4.Append("],")
        '    End If

        '    If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
        '        sbStrength5.Append("[")
        '        sbStrength5.Append(span)
        '        sbStrength5.Append(",")
        '        sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
        '        sbStrength5.Append("],")
        '    End If

        '    'Ma3 - Ma6
        '    close.Enqueue(en.Current("Close"))
        '    If close.Count >= 4 Then
        '        close0 = en.Current("Close")
        '        close3 = close.Dequeue
        '        ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
        '    End If
        '    If ma3Ma6.Count > 3 Then
        '        sbMa3Ma6.Append("[")
        '        sbMa3Ma6.Append(span)
        '        sbMa3Ma6.Append(",")
        '        sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '        sbMa3Ma6.Append("],")
        '    End If
        'End While

        'For i As Integer = 1 To 3
        '    If ma3Ma6.Count = 0 Then Exit For
        '    sbMa3Ma6.Append("[")
        '    sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        '    sbMa3Ma6.Append(",")
        '    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '    sbMa3Ma6.Append("],")
        'Next

        'With HttpContext.Current
        '    .Application(stockNo + "_Volume_1Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        '    .Application(stockNo + "_Mean5Volume_1Y") = sbMean5Volume.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20Volume_1Y") = sbMean20Volume.ToString.Trim(",")
        '    .Application(stockNo + "_DailyK_1Y") = sb0000.ToString.Trim(",")
        '    .Application(stockNo + "_Mean5_1Y") = sbMean5.ToString.Trim(",")
        '    .Application(stockNo + "_Mean10_1Y") = sbMean10.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20_1Y") = sbMean20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean60_1Y") = sbMean60.ToString.Trim(",")
        '    .Application(stockNo + "_Mean120_1Y") = sbMean120.ToString.Trim(",")
        '    .Application(stockNo + "_Mean240_1Y") = sbMean240.ToString.Trim(",")
        '    .Application(stockNo + "_K9_1Y") = sbK9.ToString.Trim(",")
        '    .Application(stockNo + "_D9_1Y") = sbD9.ToString.Trim(",")
        '    .Application(stockNo + "_Strength1_1Y") = sbStrength1.ToString.Trim(",")
        '    .Application(stockNo + "_Strength2_1Y") = sbStrength2.ToString.Trim(",")
        '    .Application(stockNo + "_Strength3_1Y") = sbStrength3.ToString.Trim(",")
        '    .Application(stockNo + "_Strength4_1Y") = sbStrength4.ToString.Trim(",")
        '    .Application(stockNo + "_Strength5_1Y") = sbStrength5.ToString.Trim(",")
        '    .Application(stockNo + "_Ma3Ma6_1Y") = sbMa3Ma6.ToString.Trim(",")

        '    .Application(timeKey) = Now.AddMinutes(30)
        'End With

    End Sub

    Public Sub LoadTech2Y(ByVal stockNo As String)
        LoadTechDaily(stockNo, 2)
        'Dim timeKey As String = stockNo + "_Tech2Y_LastTime"

        'If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
        '     Now < HttpContext.Current.Application(timeKey) Then
        '    Exit Sub
        'End If

        'Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        'Dim sb0000 As New StringBuilder
        'Dim sbVolume As New StringBuilder
        'Dim sbMean5Volume As New StringBuilder
        'Dim sbMean20Volume As New StringBuilder
        'Dim sbMean5 As New StringBuilder
        'Dim sbMean10 As New StringBuilder
        'Dim sbMean20 As New StringBuilder
        'Dim sbMean60 As New StringBuilder
        'Dim sbMean120 As New StringBuilder
        'Dim sbMean240 As New StringBuilder
        'Dim sbK9 As New StringBuilder
        'Dim sbD9 As New StringBuilder
        'Dim sbStrength1 As New StringBuilder
        'Dim sbStrength2 As New StringBuilder
        'Dim sbStrength3 As New StringBuilder
        'Dim sbStrength4 As New StringBuilder
        'Dim sbStrength5 As New StringBuilder
        'Dim sbMa3Ma6 As New StringBuilder
        'Dim ma3Ma6 As New Queue
        'Dim close As New Queue
        'Dim close0 As Double
        'Dim close3 As Double
        'Dim currentDate As Date

        'Dim sdsTech2Y As New SqlDataSource
        'sdsTech2Y.ConnectionString = twStockConnectString
        'sdsTech2Y.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-731,getdate()) Order by [Date]"
        'sdsTech2Y.SelectCommand = sdsTech2Y.SelectCommand.Replace("@StockNo", stockNo)
        'Dim en As IEnumerator = sdsTech2Y.Select(New DataSourceSelectArguments).GetEnumerator
        'Dim span As String

        'While en.MoveNext
        '    span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

        '    sbVolume.Append("[")
        '    sbVolume.Append(span)
        '    sbVolume.Append(",")
        '    If IsNumeric(stockNo) Then
        '        sbVolume.Append(CInt(en.Current("Volume") / 1000))
        '    Else
        '        sbVolume.Append(en.Current("Volume"))
        '    End If
        '    sbVolume.Append("],")

        '    If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
        '        Else
        '            sbMean5Volume.Append(en.Current("Mean5Volume"))
        '        End If
        '        sbMean5Volume.Append("],")
        '    Else
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",0],")
        '    End If

        '    If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
        '        Else
        '            sbMean20Volume.Append(en.Current("Mean20Volume"))
        '        End If
        '        sbMean20Volume.Append("],")
        '    Else
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",0],")
        '    End If

        '    sb0000.Append("[")
        '    sb0000.Append(span)
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Open"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("High"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Low"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Close"))
        '    sb0000.Append("],")

        '    If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
        '        sbMean5.Append("[")
        '        sbMean5.Append(span)
        '        sbMean5.Append(",")
        '        sbMean5.Append(Format(en.Current("mean5"), "0.00"))
        '        sbMean5.Append("],")
        '    End If

        '    If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
        '        sbMean10.Append("[")
        '        sbMean10.Append(span)
        '        sbMean10.Append(",")
        '        sbMean10.Append(Format(en.Current("mean10"), "0.00"))
        '        sbMean10.Append("],")
        '    End If

        '    If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
        '        sbMean20.Append("[")
        '        sbMean20.Append(span)
        '        sbMean20.Append(",")
        '        sbMean20.Append(Format(en.Current("mean20"), "0.00"))
        '        sbMean20.Append("],")
        '    End If

        '    If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
        '        sbMean60.Append("[")
        '        sbMean60.Append(span)
        '        sbMean60.Append(",")
        '        sbMean60.Append(Format(en.Current("mean60"), "0.00"))
        '        sbMean60.Append("],")
        '    End If

        '    If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
        '        sbMean120.Append("[")
        '        sbMean120.Append(span)
        '        sbMean120.Append(",")
        '        sbMean120.Append(Format(en.Current("mean120"), "0.00"))
        '        sbMean120.Append("],")
        '    End If

        '    If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
        '        sbMean240.Append("[")
        '        sbMean240.Append(span)
        '        sbMean240.Append(",")
        '        sbMean240.Append(Format(en.Current("mean240"), "0.00"))
        '        sbMean240.Append("],")
        '    End If

        '    If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
        '        sbK9.Append("[")
        '        sbK9.Append(span)
        '        sbK9.Append(",")
        '        sbK9.Append(Format(en.Current("K9"), "0.0"))
        '        sbK9.Append("],")
        '    End If

        '    If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
        '        sbD9.Append("[")
        '        sbD9.Append(span)
        '        sbD9.Append(",")
        '        sbD9.Append(Format(en.Current("D9"), "0.0"))
        '        sbD9.Append("],")
        '    End If

        '    If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
        '        sbStrength1.Append("[")
        '        sbStrength1.Append(span)
        '        sbStrength1.Append(",")
        '        sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
        '        sbStrength1.Append("],")
        '    End If

        '    If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
        '        sbStrength2.Append("[")
        '        sbStrength2.Append(span)
        '        sbStrength2.Append(",")
        '        sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
        '        sbStrength2.Append("],")
        '    End If

        '    If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
        '        sbStrength3.Append("[")
        '        sbStrength3.Append(span)
        '        sbStrength3.Append(",")
        '        sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
        '        sbStrength3.Append("],")
        '    End If

        '    If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
        '        sbStrength4.Append("[")
        '        sbStrength4.Append(span)
        '        sbStrength4.Append(",")
        '        sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
        '        sbStrength4.Append("],")
        '    End If

        '    If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
        '        sbStrength5.Append("[")
        '        sbStrength5.Append(span)
        '        sbStrength5.Append(",")
        '        sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
        '        sbStrength5.Append("],")
        '    End If

        '    'Ma3 - Ma6
        '    close.Enqueue(en.Current("Close"))
        '    If close.Count >= 4 Then
        '        close0 = en.Current("Close")
        '        close3 = close.Dequeue
        '        ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
        '    End If
        '    If ma3Ma6.Count > 3 Then
        '        sbMa3Ma6.Append("[")
        '        sbMa3Ma6.Append(span)
        '        sbMa3Ma6.Append(",")
        '        sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '        sbMa3Ma6.Append("],")
        '    End If
        'End While

        'For i As Integer = 1 To 3
        '    If ma3Ma6.Count = 0 Then Exit For
        '    sbMa3Ma6.Append("[")
        '    sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        '    sbMa3Ma6.Append(",")
        '    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '    sbMa3Ma6.Append("],")
        'Next

        'With HttpContext.Current
        '    .Application(stockNo + "_Volume_2Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        '    .Application(stockNo + "_Mean5Volume_2Y") = sbMean5Volume.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20Volume_2Y") = sbMean20Volume.ToString.Trim(",")
        '    .Application(stockNo + "_DailyK_2Y") = sb0000.ToString.Trim(",")
        '    .Application(stockNo + "_Mean5_2Y") = sbMean5.ToString.Trim(",")
        '    .Application(stockNo + "_Mean10_2Y") = sbMean10.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20_2Y") = sbMean20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean60_2Y") = sbMean60.ToString.Trim(",")
        '    .Application(stockNo + "_Mean120_2Y") = sbMean120.ToString.Trim(",")
        '    .Application(stockNo + "_Mean240_2Y") = sbMean240.ToString.Trim(",")
        '    .Application(stockNo + "_K9_2Y") = sbK9.ToString.Trim(",")
        '    .Application(stockNo + "_D9_2Y") = sbD9.ToString.Trim(",")
        '    .Application(stockNo + "_Strength1_2Y") = sbStrength1.ToString.Trim(",")
        '    .Application(stockNo + "_Strength2_2Y") = sbStrength2.ToString.Trim(",")
        '    .Application(stockNo + "_Strength3_2Y") = sbStrength3.ToString.Trim(",")
        '    .Application(stockNo + "_Strength4_2Y") = sbStrength4.ToString.Trim(",")
        '    .Application(stockNo + "_Strength5_2Y") = sbStrength5.ToString.Trim(",")
        '    .Application(stockNo + "_Ma3Ma6_2Y") = sbMa3Ma6.ToString.Trim(",")

        '    .Application(timeKey) = Now.AddMinutes(30)
        'End With

    End Sub

    Public Sub LoadTech3Y(ByVal stockNo As String)
        LoadTechDaily(stockNo, 3)
        'Dim timeKey As String = stockNo + "_Tech3Y_LastTime"

        'If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
        '     Now < HttpContext.Current.Application(timeKey) Then
        '    Exit Sub
        'End If

        'Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        'Dim sb0000 As New StringBuilder
        'Dim sbVolume As New StringBuilder
        'Dim sbMean5Volume As New StringBuilder
        'Dim sbMean20Volume As New StringBuilder
        'Dim sbMean5 As New StringBuilder
        'Dim sbMean10 As New StringBuilder
        'Dim sbMean20 As New StringBuilder
        'Dim sbMean60 As New StringBuilder
        'Dim sbMean120 As New StringBuilder
        'Dim sbMean240 As New StringBuilder
        'Dim sbK9 As New StringBuilder
        'Dim sbD9 As New StringBuilder
        'Dim sbStrength1 As New StringBuilder
        'Dim sbStrength2 As New StringBuilder
        'Dim sbStrength3 As New StringBuilder
        'Dim sbStrength4 As New StringBuilder
        'Dim sbStrength5 As New StringBuilder
        'Dim sbMa3Ma6 As New StringBuilder
        'Dim ma3Ma6 As New Queue
        'Dim close As New Queue
        'Dim close0 As Double
        'Dim close3 As Double
        'Dim currentDate As Date

        'Dim sdsTech3Y As New SqlDataSource
        'sdsTech3Y.ConnectionString = twStockConnectString
        'sdsTech3Y.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-1096,getdate()) Order by [Date]"
        'sdsTech3Y.SelectCommand = sdsTech3Y.SelectCommand.Replace("@StockNo", stockNo)
        'Dim en As IEnumerator = sdsTech3Y.Select(New DataSourceSelectArguments).GetEnumerator
        'Dim span As String

        'While en.MoveNext
        '    span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

        '    sbVolume.Append("[")
        '    sbVolume.Append(span)
        '    sbVolume.Append(",")
        '    If IsNumeric(stockNo) Then
        '        sbVolume.Append(CInt(en.Current("Volume") / 1000))
        '    Else
        '        sbVolume.Append(en.Current("Volume"))
        '    End If
        '    sbVolume.Append("],")

        '    If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
        '        Else
        '            sbMean5Volume.Append(en.Current("Mean5Volume"))
        '        End If
        '        sbMean5Volume.Append("],")
        '    Else
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",0],")
        '    End If

        '    If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
        '        Else
        '            sbMean20Volume.Append(en.Current("Mean20Volume"))
        '        End If
        '        sbMean20Volume.Append("],")
        '    Else
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",0],")
        '    End If

        '    sb0000.Append("[")
        '    sb0000.Append(span)
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Open"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("High"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Low"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Close"))
        '    sb0000.Append("],")

        '    If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
        '        sbMean5.Append("[")
        '        sbMean5.Append(span)
        '        sbMean5.Append(",")
        '        sbMean5.Append(Format(en.Current("mean5"), "0.00"))
        '        sbMean5.Append("],")
        '    End If

        '    If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
        '        sbMean10.Append("[")
        '        sbMean10.Append(span)
        '        sbMean10.Append(",")
        '        sbMean10.Append(Format(en.Current("mean10"), "0.00"))
        '        sbMean10.Append("],")
        '    End If

        '    If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
        '        sbMean20.Append("[")
        '        sbMean20.Append(span)
        '        sbMean20.Append(",")
        '        sbMean20.Append(Format(en.Current("mean20"), "0.00"))
        '        sbMean20.Append("],")
        '    End If

        '    If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
        '        sbMean60.Append("[")
        '        sbMean60.Append(span)
        '        sbMean60.Append(",")
        '        sbMean60.Append(Format(en.Current("mean60"), "0.00"))
        '        sbMean60.Append("],")
        '    End If

        '    If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
        '        sbMean120.Append("[")
        '        sbMean120.Append(span)
        '        sbMean120.Append(",")
        '        sbMean120.Append(Format(en.Current("mean120"), "0.00"))
        '        sbMean120.Append("],")
        '    End If

        '    If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
        '        sbMean240.Append("[")
        '        sbMean240.Append(span)
        '        sbMean240.Append(",")
        '        sbMean240.Append(Format(en.Current("mean240"), "0.00"))
        '        sbMean240.Append("],")
        '    End If

        '    If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
        '        sbK9.Append("[")
        '        sbK9.Append(span)
        '        sbK9.Append(",")
        '        sbK9.Append(Format(en.Current("K9"), "0.0"))
        '        sbK9.Append("],")
        '    End If

        '    If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
        '        sbD9.Append("[")
        '        sbD9.Append(span)
        '        sbD9.Append(",")
        '        sbD9.Append(Format(en.Current("D9"), "0.0"))
        '        sbD9.Append("],")
        '    End If

        '    If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
        '        sbStrength1.Append("[")
        '        sbStrength1.Append(span)
        '        sbStrength1.Append(",")
        '        sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
        '        sbStrength1.Append("],")
        '    End If

        '    If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
        '        sbStrength2.Append("[")
        '        sbStrength2.Append(span)
        '        sbStrength2.Append(",")
        '        sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
        '        sbStrength2.Append("],")
        '    End If

        '    If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
        '        sbStrength3.Append("[")
        '        sbStrength3.Append(span)
        '        sbStrength3.Append(",")
        '        sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
        '        sbStrength3.Append("],")
        '    End If

        '    If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
        '        sbStrength4.Append("[")
        '        sbStrength4.Append(span)
        '        sbStrength4.Append(",")
        '        sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
        '        sbStrength4.Append("],")
        '    End If

        '    If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
        '        sbStrength5.Append("[")
        '        sbStrength5.Append(span)
        '        sbStrength5.Append(",")
        '        sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
        '        sbStrength5.Append("],")
        '    End If

        '    'Ma3 - Ma6
        '    close.Enqueue(en.Current("Close"))
        '    If close.Count >= 4 Then
        '        close0 = en.Current("Close")
        '        close3 = close.Dequeue
        '        ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
        '    End If
        '    If ma3Ma6.Count > 3 Then
        '        sbMa3Ma6.Append("[")
        '        sbMa3Ma6.Append(span)
        '        sbMa3Ma6.Append(",")
        '        sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '        sbMa3Ma6.Append("],")
        '    End If
        'End While

        'For i As Integer = 1 To 3
        '    If ma3Ma6.Count = 0 Then Exit For
        '    sbMa3Ma6.Append("[")
        '    sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        '    sbMa3Ma6.Append(",")
        '    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '    sbMa3Ma6.Append("],")
        'Next

        'With HttpContext.Current
        '    .Application(stockNo + "_Volume_3Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        '    .Application(stockNo + "_Mean5Volume_3Y") = sbMean5Volume.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20Volume_3Y") = sbMean20Volume.ToString.Trim(",")
        '    .Application(stockNo + "_DailyK_3Y") = sb0000.ToString.Trim(",")
        '    .Application(stockNo + "_Mean5_3Y") = sbMean5.ToString.Trim(",")
        '    .Application(stockNo + "_Mean10_3Y") = sbMean10.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20_3Y") = sbMean20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean60_3Y") = sbMean60.ToString.Trim(",")
        '    .Application(stockNo + "_Mean120_3Y") = sbMean120.ToString.Trim(",")
        '    .Application(stockNo + "_Mean240_3Y") = sbMean240.ToString.Trim(",")
        '    .Application(stockNo + "_K9_3Y") = sbK9.ToString.Trim(",")
        '    .Application(stockNo + "_D9_3Y") = sbD9.ToString.Trim(",")
        '    .Application(stockNo + "_Strength1_3Y") = sbStrength1.ToString.Trim(",")
        '    .Application(stockNo + "_Strength2_3Y") = sbStrength2.ToString.Trim(",")
        '    .Application(stockNo + "_Strength3_3Y") = sbStrength3.ToString.Trim(",")
        '    .Application(stockNo + "_Strength4_3Y") = sbStrength4.ToString.Trim(",")
        '    .Application(stockNo + "_Strength5_3Y") = sbStrength5.ToString.Trim(",")
        '    .Application(stockNo + "_Ma3Ma6_3Y") = sbMa3Ma6.ToString.Trim(",")

        '    .Application(timeKey) = Now.AddMinutes(30)
        'End With

    End Sub

    Public Sub LoadTech4Y(ByVal stockNo As String)
        LoadTechDaily(stockNo, 4)
        'Dim timeKey As String = stockNo + "_Tech4Y_LastTime"

        'If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
        '     Now < HttpContext.Current.Application(timeKey) Then
        '    Exit Sub
        'End If

        'Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        'Dim sb0000 As New StringBuilder
        'Dim sbVolume As New StringBuilder
        'Dim sbMean5Volume As New StringBuilder
        'Dim sbMean20Volume As New StringBuilder
        'Dim sbMean5 As New StringBuilder
        'Dim sbMean10 As New StringBuilder
        'Dim sbMean20 As New StringBuilder
        'Dim sbMean60 As New StringBuilder
        'Dim sbMean120 As New StringBuilder
        'Dim sbMean240 As New StringBuilder
        'Dim sbK9 As New StringBuilder
        'Dim sbD9 As New StringBuilder
        'Dim sbStrength1 As New StringBuilder
        'Dim sbStrength2 As New StringBuilder
        'Dim sbStrength3 As New StringBuilder
        'Dim sbStrength4 As New StringBuilder
        'Dim sbStrength5 As New StringBuilder
        'Dim sbMa3Ma6 As New StringBuilder
        'Dim ma3Ma6 As New Queue
        'Dim close As New Queue
        'Dim close0 As Double
        'Dim close3 As Double
        'Dim currentDate As Date

        'Dim sdsTech4Y As New SqlDataSource
        'sdsTech4Y.ConnectionString = twStockConnectString
        'sdsTech4Y.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-1461,getdate()) Order by [Date]"
        'sdsTech4Y.SelectCommand = sdsTech4Y.SelectCommand.Replace("@StockNo", stockNo)
        'Dim en As IEnumerator = sdsTech4Y.Select(New DataSourceSelectArguments).GetEnumerator
        'Dim span As String

        'While en.MoveNext
        '    span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

        '    sbVolume.Append("[")
        '    sbVolume.Append(span)
        '    sbVolume.Append(",")
        '    If IsNumeric(stockNo) Then
        '        sbVolume.Append(CInt(en.Current("Volume") / 1000))
        '    Else
        '        sbVolume.Append(en.Current("Volume"))
        '    End If
        '    sbVolume.Append("],")

        '    If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
        '        Else
        '            sbMean5Volume.Append(en.Current("Mean5Volume"))
        '        End If
        '        sbMean5Volume.Append("],")
        '    Else
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",0],")
        '    End If

        '    If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
        '        Else
        '            sbMean20Volume.Append(en.Current("Mean20Volume"))
        '        End If
        '        sbMean20Volume.Append("],")
        '    Else
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",0],")
        '    End If

        '    sb0000.Append("[")
        '    sb0000.Append(span)
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Open"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("High"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Low"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Close"))
        '    sb0000.Append("],")

        '    If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
        '        sbMean5.Append("[")
        '        sbMean5.Append(span)
        '        sbMean5.Append(",")
        '        sbMean5.Append(Format(en.Current("mean5"), "0.00"))
        '        sbMean5.Append("],")
        '    End If

        '    If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
        '        sbMean10.Append("[")
        '        sbMean10.Append(span)
        '        sbMean10.Append(",")
        '        sbMean10.Append(Format(en.Current("mean10"), "0.00"))
        '        sbMean10.Append("],")
        '    End If

        '    If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
        '        sbMean20.Append("[")
        '        sbMean20.Append(span)
        '        sbMean20.Append(",")
        '        sbMean20.Append(Format(en.Current("mean20"), "0.00"))
        '        sbMean20.Append("],")
        '    End If

        '    If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
        '        sbMean60.Append("[")
        '        sbMean60.Append(span)
        '        sbMean60.Append(",")
        '        sbMean60.Append(Format(en.Current("mean60"), "0.00"))
        '        sbMean60.Append("],")
        '    End If

        '    If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
        '        sbMean120.Append("[")
        '        sbMean120.Append(span)
        '        sbMean120.Append(",")
        '        sbMean120.Append(Format(en.Current("mean120"), "0.00"))
        '        sbMean120.Append("],")
        '    End If

        '    If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
        '        sbMean240.Append("[")
        '        sbMean240.Append(span)
        '        sbMean240.Append(",")
        '        sbMean240.Append(Format(en.Current("mean240"), "0.00"))
        '        sbMean240.Append("],")
        '    End If

        '    If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
        '        sbK9.Append("[")
        '        sbK9.Append(span)
        '        sbK9.Append(",")
        '        sbK9.Append(Format(en.Current("K9"), "0.0"))
        '        sbK9.Append("],")
        '    End If

        '    If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
        '        sbD9.Append("[")
        '        sbD9.Append(span)
        '        sbD9.Append(",")
        '        sbD9.Append(Format(en.Current("D9"), "0.0"))
        '        sbD9.Append("],")
        '    End If

        '    If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
        '        sbStrength1.Append("[")
        '        sbStrength1.Append(span)
        '        sbStrength1.Append(",")
        '        sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
        '        sbStrength1.Append("],")
        '    End If

        '    If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
        '        sbStrength2.Append("[")
        '        sbStrength2.Append(span)
        '        sbStrength2.Append(",")
        '        sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
        '        sbStrength2.Append("],")
        '    End If

        '    If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
        '        sbStrength3.Append("[")
        '        sbStrength3.Append(span)
        '        sbStrength3.Append(",")
        '        sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
        '        sbStrength3.Append("],")
        '    End If

        '    If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
        '        sbStrength4.Append("[")
        '        sbStrength4.Append(span)
        '        sbStrength4.Append(",")
        '        sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
        '        sbStrength4.Append("],")
        '    End If

        '    If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
        '        sbStrength5.Append("[")
        '        sbStrength5.Append(span)
        '        sbStrength5.Append(",")
        '        sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
        '        sbStrength5.Append("],")
        '    End If

        '    'Ma3 - Ma6
        '    close.Enqueue(en.Current("Close"))
        '    If close.Count >= 4 Then
        '        close0 = en.Current("Close")
        '        close3 = close.Dequeue
        '        ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
        '    End If
        '    If ma3Ma6.Count > 3 Then
        '        sbMa3Ma6.Append("[")
        '        sbMa3Ma6.Append(span)
        '        sbMa3Ma6.Append(",")
        '        sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '        sbMa3Ma6.Append("],")
        '    End If
        'End While

        'For i As Integer = 1 To 3
        '    If ma3Ma6.Count = 0 Then Exit For
        '    sbMa3Ma6.Append("[")
        '    sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        '    sbMa3Ma6.Append(",")
        '    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '    sbMa3Ma6.Append("],")
        'Next

        'With HttpContext.Current
        '    .Application(stockNo + "_Volume_4Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        '    .Application(stockNo + "_Mean5Volume_4Y") = sbMean5Volume.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20Volume_4Y") = sbMean20Volume.ToString.Trim(",")
        '    .Application(stockNo + "_DailyK_4Y") = sb0000.ToString.Trim(",")
        '    .Application(stockNo + "_Mean5_4Y") = sbMean5.ToString.Trim(",")
        '    .Application(stockNo + "_Mean10_4Y") = sbMean10.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20_4Y") = sbMean20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean60_4Y") = sbMean60.ToString.Trim(",")
        '    .Application(stockNo + "_Mean120_4Y") = sbMean120.ToString.Trim(",")
        '    .Application(stockNo + "_Mean240_4Y") = sbMean240.ToString.Trim(",")
        '    .Application(stockNo + "_K9_4Y") = sbK9.ToString.Trim(",")
        '    .Application(stockNo + "_D9_4Y") = sbD9.ToString.Trim(",")
        '    .Application(stockNo + "_Strength1_4Y") = sbStrength1.ToString.Trim(",")
        '    .Application(stockNo + "_Strength2_4Y") = sbStrength2.ToString.Trim(",")
        '    .Application(stockNo + "_Strength3_4Y") = sbStrength3.ToString.Trim(",")
        '    .Application(stockNo + "_Strength4_4Y") = sbStrength4.ToString.Trim(",")
        '    .Application(stockNo + "_Strength5_4Y") = sbStrength5.ToString.Trim(",")
        '    .Application(stockNo + "_Ma3Ma6_4Y") = sbMa3Ma6.ToString.Trim(",")

        '    .Application(timeKey) = Now.AddMinutes(30)
        'End With

    End Sub

    Public Sub LoadTech5Y(ByVal stockNo As String)
        LoadTechDaily(stockNo, 5)
        'Dim timeKey As String = stockNo + "_Tech5Y_LastTime"

        'If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
        '     Now < HttpContext.Current.Application(timeKey) Then
        '    Exit Sub
        'End If

        'Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        'Dim sb0000 As New StringBuilder
        'Dim sbVolume As New StringBuilder
        'Dim sbMean5Volume As New StringBuilder
        'Dim sbMean20Volume As New StringBuilder
        'Dim sbMean5 As New StringBuilder
        'Dim sbMean10 As New StringBuilder
        'Dim sbMean20 As New StringBuilder
        'Dim sbMean60 As New StringBuilder
        'Dim sbMean120 As New StringBuilder
        'Dim sbMean240 As New StringBuilder
        'Dim sbK9 As New StringBuilder
        'Dim sbD9 As New StringBuilder
        ''Dim sbStrength1 As New StringBuilder
        ''Dim sbStrength2 As New StringBuilder
        ''Dim sbStrength3 As New StringBuilder
        ''Dim sbStrength4 As New StringBuilder
        ''Dim sbStrength5 As New StringBuilder
        ''Dim sbMa3Ma6 As New StringBuilder
        ''Dim ma3Ma6 As New Queue
        ''Dim close As New Queue
        ''Dim close0 As Double
        ''Dim close3 As Double
        ''Dim currentDate As Date

        'Dim sdsTech5Y As New SqlDataSource
        'sdsTech5Y.ConnectionString = twStockConnectString
        'sdsTech5Y.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-1826,getdate()) Order by [Date]"
        ''sdsTech5Y.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-1826,getdate()) Order by [Date]"
        'sdsTech5Y.SelectCommand = sdsTech5Y.SelectCommand.Replace("@StockNo", stockNo)
        'Dim en As IEnumerator = sdsTech5Y.Select(New DataSourceSelectArguments).GetEnumerator
        'Dim span As String

        'While en.MoveNext
        '    span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

        '    sbVolume.Append("[")
        '    sbVolume.Append(span)
        '    sbVolume.Append(",")
        '    If IsNumeric(stockNo) Then
        '        sbVolume.Append(CInt(en.Current("Volume") / 1000))
        '    Else
        '        sbVolume.Append(en.Current("Volume"))
        '    End If
        '    sbVolume.Append("],")

        '    If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
        '        Else
        '            sbMean5Volume.Append(en.Current("Mean5Volume"))
        '        End If
        '        sbMean5Volume.Append("],")
        '    Else
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",0],")
        '    End If

        '    If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
        '        Else
        '            sbMean20Volume.Append(en.Current("Mean20Volume"))
        '        End If
        '        sbMean20Volume.Append("],")
        '    Else
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",0],")
        '    End If

        '    sb0000.Append("[")
        '    sb0000.Append(span)
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Open"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("High"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Low"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Close"))
        '    sb0000.Append("],")

        '    If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
        '        sbMean5.Append("[")
        '        sbMean5.Append(span)
        '        sbMean5.Append(",")
        '        sbMean5.Append(Format(en.Current("mean5"), "0.00"))
        '        sbMean5.Append("],")
        '    End If

        '    If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
        '        sbMean10.Append("[")
        '        sbMean10.Append(span)
        '        sbMean10.Append(",")
        '        sbMean10.Append(Format(en.Current("mean10"), "0.00"))
        '        sbMean10.Append("],")
        '    End If

        '    If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
        '        sbMean20.Append("[")
        '        sbMean20.Append(span)
        '        sbMean20.Append(",")
        '        sbMean20.Append(Format(en.Current("mean20"), "0.00"))
        '        sbMean20.Append("],")
        '    End If

        '    If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
        '        sbMean60.Append("[")
        '        sbMean60.Append(span)
        '        sbMean60.Append(",")
        '        sbMean60.Append(Format(en.Current("mean60"), "0.00"))
        '        sbMean60.Append("],")
        '    End If

        '    If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
        '        sbMean120.Append("[")
        '        sbMean120.Append(span)
        '        sbMean120.Append(",")
        '        sbMean120.Append(Format(en.Current("mean120"), "0.00"))
        '        sbMean120.Append("],")
        '    End If

        '    If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
        '        sbMean240.Append("[")
        '        sbMean240.Append(span)
        '        sbMean240.Append(",")
        '        sbMean240.Append(Format(en.Current("mean240"), "0.00"))
        '        sbMean240.Append("],")
        '    End If

        '    If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
        '        sbK9.Append("[")
        '        sbK9.Append(span)
        '        sbK9.Append(",")
        '        sbK9.Append(Format(en.Current("K9"), "0.0"))
        '        sbK9.Append("],")
        '    End If

        '    If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
        '        sbD9.Append("[")
        '        sbD9.Append(span)
        '        sbD9.Append(",")
        '        sbD9.Append(Format(en.Current("D9"), "0.0"))
        '        sbD9.Append("],")
        '    End If

        '    'If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
        '    '    sbStrength1.Append("[")
        '    '    sbStrength1.Append(span)
        '    '    sbStrength1.Append(",")
        '    '    sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
        '    '    sbStrength1.Append("],")
        '    'End If

        '    'If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
        '    '    sbStrength2.Append("[")
        '    '    sbStrength2.Append(span)
        '    '    sbStrength2.Append(",")
        '    '    sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
        '    '    sbStrength2.Append("],")
        '    'End If

        '    'If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
        '    '    sbStrength3.Append("[")
        '    '    sbStrength3.Append(span)
        '    '    sbStrength3.Append(",")
        '    '    sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
        '    '    sbStrength3.Append("],")
        '    'End If

        '    'If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
        '    '    sbStrength4.Append("[")
        '    '    sbStrength4.Append(span)
        '    '    sbStrength4.Append(",")
        '    '    sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
        '    '    sbStrength4.Append("],")
        '    'End If

        '    'If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
        '    '    sbStrength5.Append("[")
        '    '    sbStrength5.Append(span)
        '    '    sbStrength5.Append(",")
        '    '    sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
        '    '    sbStrength5.Append("],")
        '    'End If

        '    ''Ma3 - Ma6
        '    'close.Enqueue(en.Current("Close"))
        '    'If close.Count >= 4 Then
        '    '    close0 = en.Current("Close")
        '    '    close3 = close.Dequeue
        '    '    ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
        '    'End If
        '    'If ma3Ma6.Count > 3 Then
        '    '    sbMa3Ma6.Append("[")
        '    '    sbMa3Ma6.Append(span)
        '    '    sbMa3Ma6.Append(",")
        '    '    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '    '    sbMa3Ma6.Append("],")
        '    'End If
        'End While

        ''For i As Integer = 1 To 3
        ''    If ma3Ma6.Count = 0 Then Exit For
        ''    sbMa3Ma6.Append("[")
        ''    sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        ''    sbMa3Ma6.Append(",")
        ''    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        ''    sbMa3Ma6.Append("],")
        ''Next

        'With HttpContext.Current
        '    .Application(stockNo + "_Volume_5Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        '    .Application(stockNo + "_Mean5Volume_5Y") = sbMean5Volume.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20Volume_5Y") = sbMean20Volume.ToString.Trim(",")
        '    .Application(stockNo + "_DailyK_5Y") = sb0000.ToString.Trim(",")
        '    .Application(stockNo + "_Mean5_5Y") = sbMean5.ToString.Trim(",")
        '    .Application(stockNo + "_Mean10_5Y") = sbMean10.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20_5Y") = sbMean20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean60_5Y") = sbMean60.ToString.Trim(",")
        '    .Application(stockNo + "_Mean120_5Y") = sbMean120.ToString.Trim(",")
        '    .Application(stockNo + "_Mean240_5Y") = sbMean240.ToString.Trim(",")
        '    .Application(stockNo + "_K9_5Y") = sbK9.ToString.Trim(",")
        '    .Application(stockNo + "_D9_5Y") = sbD9.ToString.Trim(",")
        '    '.Application(stockNo + "_Strength1_5Y") = sbStrength1.ToString.Trim(",")
        '    '.Application(stockNo + "_Strength2_5Y") = sbStrength2.ToString.Trim(",")
        '    '.Application(stockNo + "_Strength3_5Y") = sbStrength3.ToString.Trim(",")
        '    '.Application(stockNo + "_Strength4_5Y") = sbStrength4.ToString.Trim(",")
        '    '.Application(stockNo + "_Strength5_5Y") = sbStrength5.ToString.Trim(",")
        '    '.Application(stockNo + "_Ma3Ma6_5Y") = sbMa3Ma6.ToString.Trim(",")

        '    .Application(timeKey) = Now.AddMinutes(30)
        'End With

    End Sub

    Public Sub LoadTech10Y(ByVal stockNo As String)
        LoadTechDaily(stockNo, 10)
        'Dim timeKey As String = stockNo + "_Tech10Y_LastTime"

        'If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
        '     Now < HttpContext.Current.Application(timeKey) Then
        '    Exit Sub
        'End If

        'Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        'Dim sb0000 As New StringBuilder
        'Dim sbVolume As New StringBuilder
        'Dim sbMean5Volume As New StringBuilder
        'Dim sbMean20Volume As New StringBuilder
        'Dim sbMean3 As New StringBuilder
        'Dim sbMean6 As New StringBuilder
        'Dim sbMean5 As New StringBuilder
        'Dim sbMean10 As New StringBuilder
        'Dim sbMean20 As New StringBuilder
        'Dim sbMean60 As New StringBuilder
        'Dim sbMean120 As New StringBuilder
        'Dim sbMean240 As New StringBuilder
        'Dim sbK9 As New StringBuilder
        'Dim sbD9 As New StringBuilder
        'Dim sbStrength1 As New StringBuilder
        'Dim sbStrength2 As New StringBuilder
        'Dim sbStrength3 As New StringBuilder
        'Dim sbStrength4 As New StringBuilder
        'Dim sbStrength5 As New StringBuilder
        'Dim sbMa3Ma6 As New StringBuilder
        'Dim sbMa5Ma20 As New StringBuilder
        'Dim sbMean13 As New StringBuilder
        'Dim sbMean13p1Std As New StringBuilder
        'Dim sbMean13p2Std As New StringBuilder
        'Dim sbMean13m1Std As New StringBuilder
        'Dim sbMean13m2Std As New StringBuilder
        'Dim sbMean20p2Std As New StringBuilder
        'Dim sbMean20m2Std As New StringBuilder
        'Dim ma3Ma6 As New Queue
        'Dim ma5Ma20 As New Queue
        'Dim closeBuffer36 As New Queue
        'Dim closeBuffer520 As New Queue
        'Dim close0 As Double
        'Dim close3 As Double
        'Dim close15 As Double

        'Dim currentDate As Date

        'Dim sdsTech As New SqlDataSource
        'sdsTech.ConnectionString = twStockConnectString
        'sdsTech.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean3, Mean6, Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5, Mean13, [Mean13+1Std], [Mean13-1Std], [Mean13+2Std], [Mean13-2Std], [Mean20+2Std], [Mean20-2Std] FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-3650,getdate()) Order by [Date]"
        'sdsTech.SelectCommand = sdsTech.SelectCommand.Replace("@StockNo", stockNo)
        'Dim en As IEnumerator = sdsTech.Select(New DataSourceSelectArguments).GetEnumerator
        'Dim span As String

        'While en.MoveNext
        '    currentDate = en.Current("Date")
        '    span = currentDate.Subtract(dt1970).TotalMilliseconds.ToString

        '    sbVolume.Append("[")
        '    sbVolume.Append(span)
        '    sbVolume.Append(",")
        '    If IsNumeric(stockNo) Then
        '        sbVolume.Append(CInt(en.Current("Volume") / 1000))
        '    Else
        '        sbVolume.Append(en.Current("Volume"))
        '    End If
        '    sbVolume.Append("],")

        '    If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
        '        Else
        '            sbMean5Volume.Append(en.Current("Mean5Volume"))
        '        End If
        '        sbMean5Volume.Append("],")
        '    Else
        '        sbMean5Volume.Append("[")
        '        sbMean5Volume.Append(span)
        '        sbMean5Volume.Append(",0],")
        '    End If

        '    If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",")
        '        If IsNumeric(stockNo) Then
        '            sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
        '        Else
        '            sbMean20Volume.Append(en.Current("Mean20Volume"))
        '        End If
        '        sbMean20Volume.Append("],")
        '    Else
        '        sbMean20Volume.Append("[")
        '        sbMean20Volume.Append(span)
        '        sbMean20Volume.Append(",0],")
        '    End If

        '    sb0000.Append("[")
        '    sb0000.Append(span)
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Open"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("High"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Low"))
        '    sb0000.Append(",")
        '    sb0000.Append(en.Current("Close"))
        '    sb0000.Append("],")

        '    If en.Current("mean3") IsNot System.DBNull.Value AndAlso en.Current("mean3") > 0.001 Then
        '        sbMean3.Append("[")
        '        sbMean3.Append(span)
        '        sbMean3.Append(",")
        '        sbMean3.Append(Format(en.Current("mean3"), "0.00"))
        '        sbMean3.Append("],")
        '    End If

        '    If en.Current("mean6") IsNot System.DBNull.Value AndAlso en.Current("mean6") > 0.001 Then
        '        sbMean6.Append("[")
        '        sbMean6.Append(span)
        '        sbMean6.Append(",")
        '        sbMean6.Append(Format(en.Current("mean6"), "0.00"))
        '        sbMean6.Append("],")
        '    End If

        '    If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
        '        sbMean5.Append("[")
        '        sbMean5.Append(span)
        '        sbMean5.Append(",")
        '        sbMean5.Append(Format(en.Current("mean5"), "0.00"))
        '        sbMean5.Append("],")
        '    End If

        '    If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
        '        sbMean10.Append("[")
        '        sbMean10.Append(span)
        '        sbMean10.Append(",")
        '        sbMean10.Append(Format(en.Current("mean10"), "0.00"))
        '        sbMean10.Append("],")
        '    End If

        '    If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
        '        sbMean20.Append("[")
        '        sbMean20.Append(span)
        '        sbMean20.Append(",")
        '        sbMean20.Append(Format(en.Current("mean20"), "0.00"))
        '        sbMean20.Append("],")
        '    End If

        '    If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
        '        sbMean60.Append("[")
        '        sbMean60.Append(span)
        '        sbMean60.Append(",")
        '        sbMean60.Append(Format(en.Current("mean60"), "0.00"))
        '        sbMean60.Append("],")
        '    End If

        '    If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
        '        sbMean120.Append("[")
        '        sbMean120.Append(span)
        '        sbMean120.Append(",")
        '        sbMean120.Append(Format(en.Current("mean120"), "0.00"))
        '        sbMean120.Append("],")
        '    End If

        '    If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
        '        sbMean240.Append("[")
        '        sbMean240.Append(span)
        '        sbMean240.Append(",")
        '        sbMean240.Append(Format(en.Current("mean240"), "0.00"))
        '        sbMean240.Append("],")
        '    End If

        '    If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
        '        sbK9.Append("[")
        '        sbK9.Append(span)
        '        sbK9.Append(",")
        '        sbK9.Append(Format(en.Current("K9"), "0.0"))
        '        sbK9.Append("],")
        '    End If

        '    If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
        '        sbD9.Append("[")
        '        sbD9.Append(span)
        '        sbD9.Append(",")
        '        sbD9.Append(Format(en.Current("D9"), "0.0"))
        '        sbD9.Append("],")
        '    End If

        '    '多空力道
        '    If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
        '        sbStrength1.Append("[")
        '        sbStrength1.Append(span)
        '        sbStrength1.Append(",")
        '        sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
        '        sbStrength1.Append("],")
        '    End If
        '    If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
        '        sbStrength2.Append("[")
        '        sbStrength2.Append(span)
        '        sbStrength2.Append(",")
        '        sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
        '        sbStrength2.Append("],")
        '    End If
        '    If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
        '        sbStrength3.Append("[")
        '        sbStrength3.Append(span)
        '        sbStrength3.Append(",")
        '        sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
        '        sbStrength3.Append("],")
        '    End If
        '    If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
        '        sbStrength4.Append("[")
        '        sbStrength4.Append(span)
        '        sbStrength4.Append(",")
        '        sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
        '        sbStrength4.Append("],")
        '    End If
        '    If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
        '        sbStrength5.Append("[")
        '        sbStrength5.Append(span)
        '        sbStrength5.Append(",")
        '        sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
        '        sbStrength5.Append("],")
        '    End If

        '    '布林軌道
        '    If en.Current("Mean20+2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean20+2Std") > 0.001 Then
        '        sbMean20p2Std.Append("[")
        '        sbMean20p2Std.Append(span)
        '        sbMean20p2Std.Append(",")
        '        sbMean20p2Std.Append(Format(en.Current("Mean20+2Std"), "0.00"))
        '        sbMean20p2Std.Append("],")
        '    End If
        '    If en.Current("Mean20-2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean20-2Std") > 0.001 Then
        '        sbMean20m2Std.Append("[")
        '        sbMean20m2Std.Append(span)
        '        sbMean20m2Std.Append(",")
        '        sbMean20m2Std.Append(Format(en.Current("Mean20-2Std"), "0.00"))
        '        sbMean20m2Std.Append("],")
        '    End If

        '    '天羅地網
        '    If en.Current("Mean13") IsNot System.DBNull.Value AndAlso en.Current("Mean13") > 0.001 Then
        '        sbMean13.Append("[")
        '        sbMean13.Append(span)
        '        sbMean13.Append(",")
        '        sbMean13.Append(Format(en.Current("Mean13"), "0.00"))
        '        sbMean13.Append("],")
        '    End If
        '    If en.Current("Mean13+1Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13+1Std") > 0.001 Then
        '        sbMean13p1Std.Append("[")
        '        sbMean13p1Std.Append(span)
        '        sbMean13p1Std.Append(",")
        '        sbMean13p1Std.Append(Format(en.Current("Mean13+1Std"), "0.00"))
        '        sbMean13p1Std.Append("],")
        '    End If
        '    If en.Current("Mean13-1Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13-1Std") > 0.001 Then
        '        sbMean13m1Std.Append("[")
        '        sbMean13m1Std.Append(span)
        '        sbMean13m1Std.Append(",")
        '        sbMean13m1Std.Append(Format(en.Current("Mean13-1Std"), "0.00"))
        '        sbMean13m1Std.Append("],")
        '    End If
        '    If en.Current("Mean13+2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13+2Std") > 0.001 Then
        '        sbMean13p2Std.Append("[")
        '        sbMean13p2Std.Append(span)
        '        sbMean13p2Std.Append(",")
        '        sbMean13p2Std.Append(Format(en.Current("Mean13+2Std"), "0.00"))
        '        sbMean13p2Std.Append("],")
        '    End If
        '    If en.Current("Mean13-2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13-2Std") > 0.001 Then
        '        sbMean13m2Std.Append("[")
        '        sbMean13m2Std.Append(span)
        '        sbMean13m2Std.Append(",")
        '        sbMean13m2Std.Append(Format(en.Current("Mean13-2Std"), "0.00"))
        '        sbMean13m2Std.Append("],")
        '    End If

        '    'Ma3 - Ma6
        '    closeBuffer36.Enqueue(en.Current("Close"))
        '    If closeBuffer36.Count >= 4 Then
        '        close0 = en.Current("Close")
        '        close3 = closeBuffer36.Dequeue
        '        ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
        '    End If
        '    If ma3Ma6.Count > 3 Then
        '        sbMa3Ma6.Append("[")
        '        sbMa3Ma6.Append(span)
        '        sbMa3Ma6.Append(",")
        '        sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '        sbMa3Ma6.Append("],")
        '    End If

        '    'Ma5 - Ma20
        '    closeBuffer520.Enqueue(en.Current("Close"))
        '    If closeBuffer520.Count >= 16 Then
        '        close0 = en.Current("Close")
        '        close15 = closeBuffer520.Dequeue
        '        ma5Ma20.Enqueue(Format((close0 * 4 - close3) / 3, "0.00"))
        '    End If
        '    If ma5Ma20.Count > 5 Then
        '        sbMa5Ma20.Append("[")
        '        sbMa5Ma20.Append(span)
        '        sbMa5Ma20.Append(",")
        '        sbMa5Ma20.Append(ma5Ma20.Dequeue)
        '        sbMa5Ma20.Append("],")
        '    End If
        'End While

        'For i As Integer = 1 To 3 'Ma3 - Ma6
        '    If ma3Ma6.Count = 0 Then Exit For
        '    sbMa3Ma6.Append("[")
        '    sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        '    sbMa3Ma6.Append(",")
        '    sbMa3Ma6.Append(ma3Ma6.Dequeue)
        '    sbMa3Ma6.Append("],")
        'Next

        'For i As Integer = 1 To 5 'Ma5 - Ma20
        '    If ma5Ma20.Count = 0 Then Exit For
        '    sbMa5Ma20.Append("[")
        '    sbMa5Ma20.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
        '    sbMa5Ma20.Append(",")
        '    sbMa5Ma20.Append(ma5Ma20.Dequeue)
        '    sbMa5Ma20.Append("],")
        'Next

        'With HttpContext.Current
        '    .Application(stockNo + "_Volume_10Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        '    .Application(stockNo + "_Mean5Volume_10Y") = sbMean5Volume.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20Volume_10Y") = sbMean20Volume.ToString.Trim(",")
        '    .Application(stockNo + "_DailyK_10Y") = sb0000.ToString.Trim(",")
        '    .Application(stockNo + "_Mean3_10Y") = sbMean3.ToString.Trim(",")
        '    .Application(stockNo + "_Mean6_10Y") = sbMean6.ToString.Trim(",")
        '    .Application(stockNo + "_Mean5_10Y") = sbMean5.ToString.Trim(",")
        '    .Application(stockNo + "_Mean10_10Y") = sbMean10.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20_10Y") = sbMean20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean60_10Y") = sbMean60.ToString.Trim(",")
        '    .Application(stockNo + "_Mean120_10Y") = sbMean120.ToString.Trim(",")
        '    .Application(stockNo + "_Mean240_10Y") = sbMean240.ToString.Trim(",")
        '    .Application(stockNo + "_K9_10Y") = sbK9.ToString.Trim(",")
        '    .Application(stockNo + "_D9_10Y") = sbD9.ToString.Trim(",")
        '    .Application(stockNo + "_Strength1_10Y") = sbStrength1.ToString.Trim(",")
        '    .Application(stockNo + "_Strength2_10Y") = sbStrength2.ToString.Trim(",")
        '    .Application(stockNo + "_Strength3_10Y") = sbStrength3.ToString.Trim(",")
        '    .Application(stockNo + "_Strength4_10Y") = sbStrength4.ToString.Trim(",")
        '    .Application(stockNo + "_Strength5_10Y") = sbStrength5.ToString.Trim(",")
        '    .Application(stockNo + "_Ma3Ma6_10Y") = sbMa3Ma6.ToString.Trim(",")
        '    .Application(stockNo + "_Ma5Ma20_10Y") = sbMa5Ma20.ToString.Trim(",")
        '    .Application(stockNo + "_Mean13_10Y") = sbMean13.ToString.Trim(",")
        '    .Application(stockNo + "_Mean13+1Std_10Y") = sbMean13p1Std.ToString.Trim(",")
        '    .Application(stockNo + "_Mean13-1Std_10Y") = sbMean13m1Std.ToString.Trim(",")
        '    .Application(stockNo + "_Mean13+2Std_10Y") = sbMean13p2Std.ToString.Trim(",")
        '    .Application(stockNo + "_Mean13-2Std_10Y") = sbMean13m2Std.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20+2Std_10Y") = sbMean20p2Std.ToString.Trim(",")
        '    .Application(stockNo + "_Mean20-2Std_10Y") = sbMean20m2Std.ToString.Trim(",")

        '    .Application(timeKey) = Now.AddMinutes(30)
        'End With

        'Dim sdsDeduct As New SqlDataSource
        'sdsDeduct.ConnectionString = twStockConnectString
        'sdsDeduct.SelectCommand = "Select Top 1 [Date],[Close] From (SELECT Top 20 [Date],[Close] From HistoryPriceDaily WHERE StockNo = '@StockNo' Order by [Date] Desc) t Order by [Date]"
        'sdsDeduct.SelectCommand = sdsDeduct.SelectCommand.Replace("@StockNo", stockNo)
        'en = sdsDeduct.Select(New DataSourceSelectArguments).GetEnumerator
        'If en.MoveNext Then
        '    HttpContext.Current.Application(stockNo + "_Deduct20MA") = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString
        '    HttpContext.Current.Application(stockNo + "_Deduct20MAValue") = Format(en.Current("Close"), "0.00").Replace(".00", "").Trim("0")
        'End If
        'sdsDeduct.SelectCommand = "Select Top 1 [Date],[Close] From (SELECT Top 60 [Date],[Close] From HistoryPriceDaily WHERE StockNo = '@StockNo' Order by [Date] Desc) t Order by [Date]"
        'sdsDeduct.SelectCommand = sdsDeduct.SelectCommand.Replace("@StockNo", stockNo)
        'en = sdsDeduct.Select(New DataSourceSelectArguments).GetEnumerator
        'If en.MoveNext Then
        '    HttpContext.Current.Application(stockNo + "_Deduct60MA") = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString
        '    HttpContext.Current.Application(stockNo + "_Deduct60MAValue") = Format(en.Current("Close"), "0.00").Replace(".00", "").Trim("0")
        'End If

    End Sub

    Public Sub LoadTechDaily(ByVal stockNo As String, ByVal years As Single)
        Dim timeKey As String = stockNo + "_Tech" + years.ToString + "Y_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
             Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sb0000 As New StringBuilder
        Dim sbVolume As New StringBuilder
        Dim sbMean5Volume As New StringBuilder
        Dim sbMean20Volume As New StringBuilder
        Dim sbMean3 As New StringBuilder
        Dim sbMean6 As New StringBuilder
        Dim sbMean5 As New StringBuilder
        Dim sbMean10 As New StringBuilder
        Dim sbMean20 As New StringBuilder
        Dim sbMean60 As New StringBuilder
        Dim sbMean120 As New StringBuilder
        Dim sbMean240 As New StringBuilder
        Dim sbK9 As New StringBuilder
        Dim sbD9 As New StringBuilder
        Dim sbStrength1 As New StringBuilder
        Dim sbStrength2 As New StringBuilder
        Dim sbStrength3 As New StringBuilder
        Dim sbStrength4 As New StringBuilder
        Dim sbStrength5 As New StringBuilder
        Dim sbMa3Ma6 As New StringBuilder
        Dim sbMa5Ma20 As New StringBuilder
        Dim sbMean13 As New StringBuilder
        Dim sbMean13p1Std As New StringBuilder
        Dim sbMean13p2Std As New StringBuilder
        Dim sbMean13m1Std As New StringBuilder
        Dim sbMean13m2Std As New StringBuilder
        Dim sbMean20p2Std As New StringBuilder
        Dim sbMean20m2Std As New StringBuilder
        Dim ma3Ma6 As New Queue
        Dim ma5Ma20 As New Queue
        Dim closeBuffer36 As New Queue
        Dim closeBuffer520 As New Queue
        Dim close0 As Double
        Dim close3 As Double
        Dim close15 As Double

        Dim currentDate As Date

        Dim sdsTech As New SqlDataSource
        sdsTech.ConnectionString = twStockConnectString
        sdsTech.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean3, Mean6, Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5, Mean13, [Mean13+1Std], [Mean13-1Std], [Mean13+2Std], [Mean13-2Std], [Mean20+2Std], [Mean20-2Std] FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-" + (365 * years).ToString + ",getdate()) Order by [Date]"
        sdsTech.SelectCommand = sdsTech.SelectCommand.Replace("@StockNo", stockNo)
        Dim en As IEnumerator = sdsTech.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String

        While en.MoveNext
            currentDate = en.Current("Date")
            span = currentDate.Subtract(dt1970).TotalMilliseconds.ToString

            sbVolume.Append("[")
            sbVolume.Append(span)
            sbVolume.Append(",")
            If IsNumeric(stockNo) Then
                sbVolume.Append(CInt(en.Current("Volume") / 1000))
            Else
                sbVolume.Append(en.Current("Volume"))
            End If
            sbVolume.Append("],")

            If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
                Else
                    sbMean5Volume.Append(en.Current("Mean5Volume"))
                End If
                sbMean5Volume.Append("],")
            Else
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",0],")
            End If

            If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
                Else
                    sbMean20Volume.Append(en.Current("Mean20Volume"))
                End If
                sbMean20Volume.Append("],")
            Else
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",0],")
            End If

            sb0000.Append("[")
            sb0000.Append(span)
            sb0000.Append(",")
            sb0000.Append(en.Current("Open"))
            sb0000.Append(",")
            sb0000.Append(en.Current("High"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Low"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Close"))
            sb0000.Append("],")

            If en.Current("mean3") IsNot System.DBNull.Value AndAlso en.Current("mean3") > 0.001 Then
                sbMean3.Append("[")
                sbMean3.Append(span)
                sbMean3.Append(",")
                sbMean3.Append(Format(en.Current("mean3"), "0.00"))
                sbMean3.Append("],")
            End If

            If en.Current("mean6") IsNot System.DBNull.Value AndAlso en.Current("mean6") > 0.001 Then
                sbMean6.Append("[")
                sbMean6.Append(span)
                sbMean6.Append(",")
                sbMean6.Append(Format(en.Current("mean6"), "0.00"))
                sbMean6.Append("],")
            End If

            If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
                sbMean5.Append("[")
                sbMean5.Append(span)
                sbMean5.Append(",")
                sbMean5.Append(Format(en.Current("mean5"), "0.00"))
                sbMean5.Append("],")
            End If

            If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
                sbMean10.Append("[")
                sbMean10.Append(span)
                sbMean10.Append(",")
                sbMean10.Append(Format(en.Current("mean10"), "0.00"))
                sbMean10.Append("],")
            End If

            If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
                sbMean20.Append("[")
                sbMean20.Append(span)
                sbMean20.Append(",")
                sbMean20.Append(Format(en.Current("mean20"), "0.00"))
                sbMean20.Append("],")
            End If

            If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
                sbMean60.Append("[")
                sbMean60.Append(span)
                sbMean60.Append(",")
                sbMean60.Append(Format(en.Current("mean60"), "0.00"))
                sbMean60.Append("],")
            End If

            If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
                sbMean120.Append("[")
                sbMean120.Append(span)
                sbMean120.Append(",")
                sbMean120.Append(Format(en.Current("mean120"), "0.00"))
                sbMean120.Append("],")
            End If

            If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
                sbMean240.Append("[")
                sbMean240.Append(span)
                sbMean240.Append(",")
                sbMean240.Append(Format(en.Current("mean240"), "0.00"))
                sbMean240.Append("],")
            End If

            If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
                sbK9.Append("[")
                sbK9.Append(span)
                sbK9.Append(",")
                sbK9.Append(Format(en.Current("K9"), "0.0"))
                sbK9.Append("],")
            End If

            If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
                sbD9.Append("[")
                sbD9.Append(span)
                sbD9.Append(",")
                sbD9.Append(Format(en.Current("D9"), "0.0"))
                sbD9.Append("],")
            End If

            '多空力道
            If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
                sbStrength1.Append("[")
                sbStrength1.Append(span)
                sbStrength1.Append(",")
                sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
                sbStrength1.Append("],")
            End If
            If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
                sbStrength2.Append("[")
                sbStrength2.Append(span)
                sbStrength2.Append(",")
                sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
                sbStrength2.Append("],")
            End If
            If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
                sbStrength3.Append("[")
                sbStrength3.Append(span)
                sbStrength3.Append(",")
                sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
                sbStrength3.Append("],")
            End If
            If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
                sbStrength4.Append("[")
                sbStrength4.Append(span)
                sbStrength4.Append(",")
                sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
                sbStrength4.Append("],")
            End If
            If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
                sbStrength5.Append("[")
                sbStrength5.Append(span)
                sbStrength5.Append(",")
                sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
                sbStrength5.Append("],")
            End If

            '布林軌道
            If en.Current("Mean20+2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean20+2Std") > 0.001 Then
                sbMean20p2Std.Append("[")
                sbMean20p2Std.Append(span)
                sbMean20p2Std.Append(",")
                sbMean20p2Std.Append(Format(en.Current("Mean20+2Std"), "0.00"))
                sbMean20p2Std.Append("],")
            End If
            If en.Current("Mean20-2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean20-2Std") > 0.001 Then
                sbMean20m2Std.Append("[")
                sbMean20m2Std.Append(span)
                sbMean20m2Std.Append(",")
                sbMean20m2Std.Append(Format(en.Current("Mean20-2Std"), "0.00"))
                sbMean20m2Std.Append("],")
            End If

            '天羅地網
            If en.Current("Mean13") IsNot System.DBNull.Value AndAlso en.Current("Mean13") > 0.001 Then
                sbMean13.Append("[")
                sbMean13.Append(span)
                sbMean13.Append(",")
                sbMean13.Append(Format(en.Current("Mean13"), "0.00"))
                sbMean13.Append("],")
            End If
            If en.Current("Mean13+1Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13+1Std") > 0.001 Then
                sbMean13p1Std.Append("[")
                sbMean13p1Std.Append(span)
                sbMean13p1Std.Append(",")
                sbMean13p1Std.Append(Format(en.Current("Mean13+1Std"), "0.00"))
                sbMean13p1Std.Append("],")
            End If
            If en.Current("Mean13-1Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13-1Std") > 0.001 Then
                sbMean13m1Std.Append("[")
                sbMean13m1Std.Append(span)
                sbMean13m1Std.Append(",")
                sbMean13m1Std.Append(Format(en.Current("Mean13-1Std"), "0.00"))
                sbMean13m1Std.Append("],")
            End If
            If en.Current("Mean13+2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13+2Std") > 0.001 Then
                sbMean13p2Std.Append("[")
                sbMean13p2Std.Append(span)
                sbMean13p2Std.Append(",")
                sbMean13p2Std.Append(Format(en.Current("Mean13+2Std"), "0.00"))
                sbMean13p2Std.Append("],")
            End If
            If en.Current("Mean13-2Std") IsNot System.DBNull.Value AndAlso en.Current("Mean13-2Std") > 0.001 Then
                sbMean13m2Std.Append("[")
                sbMean13m2Std.Append(span)
                sbMean13m2Std.Append(",")
                sbMean13m2Std.Append(Format(en.Current("Mean13-2Std"), "0.00"))
                sbMean13m2Std.Append("],")
            End If

            'Ma3 - Ma6
            closeBuffer36.Enqueue(en.Current("Close"))
            If closeBuffer36.Count >= 4 Then
                close0 = en.Current("Close")
                close3 = closeBuffer36.Dequeue
                ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
            End If
            If ma3Ma6.Count > 3 Then
                sbMa3Ma6.Append("[")
                sbMa3Ma6.Append(span)
                sbMa3Ma6.Append(",")
                sbMa3Ma6.Append(ma3Ma6.Dequeue)
                sbMa3Ma6.Append("],")
            End If

            'Ma5 - Ma20
            closeBuffer520.Enqueue(en.Current("Close"))
            If closeBuffer520.Count >= 16 Then
                close0 = en.Current("Close")
                close15 = closeBuffer520.Dequeue
                ma5Ma20.Enqueue(Format((close0 * 4 - close3) / 3, "0.00"))
            End If
            If ma5Ma20.Count > 5 Then
                sbMa5Ma20.Append("[")
                sbMa5Ma20.Append(span)
                sbMa5Ma20.Append(",")
                sbMa5Ma20.Append(ma5Ma20.Dequeue)
                sbMa5Ma20.Append("],")
            End If
        End While

        For i As Integer = 1 To 3 'Ma3 - Ma6
            If ma3Ma6.Count = 0 Then Exit For
            sbMa3Ma6.Append("[")
            sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
            sbMa3Ma6.Append(",")
            sbMa3Ma6.Append(ma3Ma6.Dequeue)
            sbMa3Ma6.Append("],")
        Next

        For i As Integer = 1 To 5 'Ma5 - Ma20
            If ma5Ma20.Count = 0 Then Exit For
            sbMa5Ma20.Append("[")
            sbMa5Ma20.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
            sbMa5Ma20.Append(",")
            sbMa5Ma20.Append(ma5Ma20.Dequeue)
            sbMa5Ma20.Append("],")
        Next

        With HttpContext.Current
            .Application(stockNo + "_Volume_" + years.ToString + "Y") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_Mean5Volume_" + years.ToString + "Y") = sbMean5Volume.ToString.Trim(",")
            .Application(stockNo + "_Mean20Volume_" + years.ToString + "Y") = sbMean20Volume.ToString.Trim(",")
            .Application(stockNo + "_DailyK_" + years.ToString + "Y") = sb0000.ToString.Trim(",")
            .Application(stockNo + "_Mean3_" + years.ToString + "Y") = sbMean3.ToString.Trim(",")
            .Application(stockNo + "_Mean6_" + years.ToString + "Y") = sbMean6.ToString.Trim(",")
            .Application(stockNo + "_Mean5_" + years.ToString + "Y") = sbMean5.ToString.Trim(",")
            .Application(stockNo + "_Mean10_" + years.ToString + "Y") = sbMean10.ToString.Trim(",")
            .Application(stockNo + "_Mean20_" + years.ToString + "Y") = sbMean20.ToString.Trim(",")
            .Application(stockNo + "_Mean60_" + years.ToString + "Y") = sbMean60.ToString.Trim(",")
            .Application(stockNo + "_Mean120_" + years.ToString + "Y") = sbMean120.ToString.Trim(",")
            .Application(stockNo + "_Mean240_" + years.ToString + "Y") = sbMean240.ToString.Trim(",")
            .Application(stockNo + "_K9_" + years.ToString + "Y") = sbK9.ToString.Trim(",")
            .Application(stockNo + "_D9_" + years.ToString + "Y") = sbD9.ToString.Trim(",")
            .Application(stockNo + "_Strength1_" + years.ToString + "Y") = sbStrength1.ToString.Trim(",")
            .Application(stockNo + "_Strength2_" + years.ToString + "Y") = sbStrength2.ToString.Trim(",")
            .Application(stockNo + "_Strength3_" + years.ToString + "Y") = sbStrength3.ToString.Trim(",")
            .Application(stockNo + "_Strength4_" + years.ToString + "Y") = sbStrength4.ToString.Trim(",")
            .Application(stockNo + "_Strength5_" + years.ToString + "Y") = sbStrength5.ToString.Trim(",")
            .Application(stockNo + "_Ma3Ma6_" + years.ToString + "Y") = sbMa3Ma6.ToString.Trim(",")
            .Application(stockNo + "_Ma5Ma20_" + years.ToString + "Y") = sbMa5Ma20.ToString.Trim(",")
            .Application(stockNo + "_Mean13_10Y") = sbMean13.ToString.Trim(",")
            .Application(stockNo + "_Mean13+1Std_" + years.ToString + "Y") = sbMean13p1Std.ToString.Trim(",")
            .Application(stockNo + "_Mean13-1Std_" + years.ToString + "Y") = sbMean13m1Std.ToString.Trim(",")
            .Application(stockNo + "_Mean13+2Std_" + years.ToString + "Y") = sbMean13p2Std.ToString.Trim(",")
            .Application(stockNo + "_Mean13-2Std_" + years.ToString + "Y") = sbMean13m2Std.ToString.Trim(",")
            .Application(stockNo + "_Mean20+2Std_" + years.ToString + "Y") = sbMean20p2Std.ToString.Trim(",")
            .Application(stockNo + "_Mean20-2Std_" + years.ToString + "Y") = sbMean20m2Std.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

        Dim sdsDeduct As New SqlDataSource
        sdsDeduct.ConnectionString = twStockConnectString
        sdsDeduct.SelectCommand = "Select Top 1 [Date],[Close] From (SELECT Top 20 [Date],[Close] From HistoryPriceDaily WHERE StockNo = '@StockNo' Order by [Date] Desc) t Order by [Date]"
        sdsDeduct.SelectCommand = sdsDeduct.SelectCommand.Replace("@StockNo", stockNo)
        en = sdsDeduct.Select(New DataSourceSelectArguments).GetEnumerator
        If en.MoveNext Then
            HttpContext.Current.Application(stockNo + "_Deduct20MA") = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString
            HttpContext.Current.Application(stockNo + "_Deduct20MAValue") = Format(en.Current("Close"), "0.00").Replace(".00", "").Trim("0")
        End If
        sdsDeduct.SelectCommand = "Select Top 1 [Date],[Close] From (SELECT Top 60 [Date],[Close] From HistoryPriceDaily WHERE StockNo = '@StockNo' Order by [Date] Desc) t Order by [Date]"
        sdsDeduct.SelectCommand = sdsDeduct.SelectCommand.Replace("@StockNo", stockNo)
        en = sdsDeduct.Select(New DataSourceSelectArguments).GetEnumerator
        If en.MoveNext Then
            HttpContext.Current.Application(stockNo + "_Deduct60MA") = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString
            HttpContext.Current.Application(stockNo + "_Deduct60MAValue") = Format(en.Current("Close"), "0.00").Replace(".00", "").Trim("0")
        End If

    End Sub

    Public Sub LoadTechWeek(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_TechWeek_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
             Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sb0000 As New StringBuilder
        Dim sbVolume As New StringBuilder
        Dim sbMean5Volume As New StringBuilder
        Dim sbMean20Volume As New StringBuilder
        Dim sbMean5 As New StringBuilder
        Dim sbMean10 As New StringBuilder
        Dim sbMean20 As New StringBuilder
        Dim sbMean60 As New StringBuilder
        Dim sbMean120 As New StringBuilder
        Dim sbMean240 As New StringBuilder
        Dim sbK9 As New StringBuilder
        Dim sbD9 As New StringBuilder

        Dim sdsTech As New SqlDataSource
        sdsTech.ConnectionString = twStockConnectString
        sdsTech.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV FROM  HistoryPriceWeekly WHERE (StockNo = '@StockNo') and [Date] > dateadd(d,-3650,getdate()) Order by [Date]"
        sdsTech.SelectCommand = sdsTech.SelectCommand.Replace("@StockNo", stockNo)
        Dim en As IEnumerator = sdsTech.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String

        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            sbVolume.Append("[")
            sbVolume.Append(span)
            sbVolume.Append(",")
            If IsNumeric(stockNo) Then
                sbVolume.Append(CInt(en.Current("Volume") / 1000))
            Else
                sbVolume.Append(en.Current("Volume"))
            End If
            sbVolume.Append("],")

            If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
                Else
                    sbMean5Volume.Append(en.Current("Mean5Volume"))
                End If
                sbMean5Volume.Append("],")
            Else
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",0],")
            End If

            If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
                Else
                    sbMean20Volume.Append(en.Current("Mean20Volume"))
                End If
                sbMean20Volume.Append("],")
            Else
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",0],")
            End If

            sb0000.Append("[")
            sb0000.Append(span)
            sb0000.Append(",")
            sb0000.Append(en.Current("Open"))
            sb0000.Append(",")
            sb0000.Append(en.Current("High"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Low"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Close"))
            sb0000.Append("],")

            If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
                sbMean5.Append("[")
                sbMean5.Append(span)
                sbMean5.Append(",")
                sbMean5.Append(Format(en.Current("mean5"), "0.00"))
                sbMean5.Append("],")
            End If

            If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
                sbMean10.Append("[")
                sbMean10.Append(span)
                sbMean10.Append(",")
                sbMean10.Append(Format(en.Current("mean10"), "0.00"))
                sbMean10.Append("],")
            End If

            If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
                sbMean20.Append("[")
                sbMean20.Append(span)
                sbMean20.Append(",")
                sbMean20.Append(Format(en.Current("mean20"), "0.00"))
                sbMean20.Append("],")
            End If

            If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
                sbMean60.Append("[")
                sbMean60.Append(span)
                sbMean60.Append(",")
                sbMean60.Append(Format(en.Current("mean60"), "0.00"))
                sbMean60.Append("],")
            End If

            If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
                sbMean120.Append("[")
                sbMean120.Append(span)
                sbMean120.Append(",")
                sbMean120.Append(Format(en.Current("mean120"), "0.00"))
                sbMean120.Append("],")
            End If

            If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
                sbMean240.Append("[")
                sbMean240.Append(span)
                sbMean240.Append(",")
                sbMean240.Append(Format(en.Current("mean240"), "0.00"))
                sbMean240.Append("],")
            End If

            If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
                sbK9.Append("[")
                sbK9.Append(span)
                sbK9.Append(",")
                sbK9.Append(Format(en.Current("K9"), "0.0"))
                sbK9.Append("],")
            End If

            If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
                sbD9.Append("[")
                sbD9.Append(span)
                sbD9.Append(",")
                sbD9.Append(Format(en.Current("D9"), "0.0"))
                sbD9.Append("],")
            End If

        End While

        With HttpContext.Current
            .Application(stockNo + "_Volume_Week") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_Mean5Volume_Week") = sbMean5Volume.ToString.Trim(",")
            .Application(stockNo + "_Mean20Volume_Week") = sbMean20Volume.ToString.Trim(",")
            .Application(stockNo + "_DailyK_Week") = sb0000.ToString.Trim(",")
            .Application(stockNo + "_Mean5_Week") = sbMean5.ToString.Trim(",")
            .Application(stockNo + "_Mean10_Week") = sbMean10.ToString.Trim(",")
            .Application(stockNo + "_Mean20_Week") = sbMean20.ToString.Trim(",")
            .Application(stockNo + "_Mean60_Week") = sbMean60.ToString.Trim(",")
            .Application(stockNo + "_Mean120_Week") = sbMean120.ToString.Trim(",")
            .Application(stockNo + "_Mean240_Week") = sbMean240.ToString.Trim(",")
            .Application(stockNo + "_K9_Week") = sbK9.ToString.Trim(",")
            .Application(stockNo + "_D9_Week") = sbD9.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

    End Sub

    Public Sub LoadTechMonth(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_TechMonth_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
             Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sb0000 As New StringBuilder
        Dim sbVolume As New StringBuilder
        Dim sbMean5Volume As New StringBuilder
        Dim sbMean20Volume As New StringBuilder
        Dim sbMean5 As New StringBuilder
        Dim sbMean10 As New StringBuilder
        Dim sbMean20 As New StringBuilder
        Dim sbMean60 As New StringBuilder
        Dim sbMean120 As New StringBuilder
        Dim sbMean240 As New StringBuilder
        Dim sbK9 As New StringBuilder
        Dim sbD9 As New StringBuilder

        Dim sdsTech As New SqlDataSource
        sdsTech.ConnectionString = twStockConnectString
        sdsTech.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV FROM  HistoryPriceMonthly WHERE (StockNo = '@StockNo') Order by [Date]"
        sdsTech.SelectCommand = sdsTech.SelectCommand.Replace("@StockNo", stockNo)
        Dim en As IEnumerator = sdsTech.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String

        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            sbVolume.Append("[")
            sbVolume.Append(span)
            sbVolume.Append(",")
            If IsNumeric(stockNo) Then
                sbVolume.Append(CInt(en.Current("Volume") / 1000))
            Else
                sbVolume.Append(en.Current("Volume"))
            End If
            sbVolume.Append("],")

            If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
                Else
                    sbMean5Volume.Append(en.Current("Mean5Volume"))
                End If
                sbMean5Volume.Append("],")
            Else
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",0],")
            End If

            If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
                Else
                    sbMean20Volume.Append(en.Current("Mean20Volume"))
                End If
                sbMean20Volume.Append("],")
            Else
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",0],")
            End If

            sb0000.Append("[")
            sb0000.Append(span)
            sb0000.Append(",")
            sb0000.Append(en.Current("Open"))
            sb0000.Append(",")
            sb0000.Append(en.Current("High"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Low"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Close"))
            sb0000.Append("],")

            If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") > 0.001 Then
                sbMean5.Append("[")
                sbMean5.Append(span)
                sbMean5.Append(",")
                sbMean5.Append(Format(en.Current("mean5"), "0.00"))
                sbMean5.Append("],")
            End If

            If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") > 0.001 Then
                sbMean10.Append("[")
                sbMean10.Append(span)
                sbMean10.Append(",")
                sbMean10.Append(Format(en.Current("mean10"), "0.00"))
                sbMean10.Append("],")
            End If

            If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") > 0.001 Then
                sbMean20.Append("[")
                sbMean20.Append(span)
                sbMean20.Append(",")
                sbMean20.Append(Format(en.Current("mean20"), "0.00"))
                sbMean20.Append("],")
            End If

            If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") > 0.001 Then
                sbMean60.Append("[")
                sbMean60.Append(span)
                sbMean60.Append(",")
                sbMean60.Append(Format(en.Current("mean60"), "0.00"))
                sbMean60.Append("],")
            End If

            If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") > 0.001 Then
                sbMean120.Append("[")
                sbMean120.Append(span)
                sbMean120.Append(",")
                sbMean120.Append(Format(en.Current("mean120"), "0.00"))
                sbMean120.Append("],")
            End If

            If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") > 0.001 Then
                sbMean240.Append("[")
                sbMean240.Append(span)
                sbMean240.Append(",")
                sbMean240.Append(Format(en.Current("mean240"), "0.00"))
                sbMean240.Append("],")
            End If

            If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
                sbK9.Append("[")
                sbK9.Append(span)
                sbK9.Append(",")
                sbK9.Append(Format(en.Current("K9"), "0.0"))
                sbK9.Append("],")
            End If

            If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
                sbD9.Append("[")
                sbD9.Append(span)
                sbD9.Append(",")
                sbD9.Append(Format(en.Current("D9"), "0.0"))
                sbD9.Append("],")
            End If

        End While

        With HttpContext.Current
            .Application(stockNo + "_Volume_Month") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_Mean5Volume_Month") = sbMean5Volume.ToString.Trim(",")
            .Application(stockNo + "_Mean20Volume_Month") = sbMean20Volume.ToString.Trim(",")
            .Application(stockNo + "_DailyK_Month") = sb0000.ToString.Trim(",")
            .Application(stockNo + "_Mean5_Month") = sbMean5.ToString.Trim(",")
            .Application(stockNo + "_Mean10_Month") = sbMean10.ToString.Trim(",")
            .Application(stockNo + "_Mean20_Month") = sbMean20.ToString.Trim(",")
            .Application(stockNo + "_Mean60_Month") = sbMean60.ToString.Trim(",")
            .Application(stockNo + "_Mean120_Month") = sbMean120.ToString.Trim(",")
            .Application(stockNo + "_Mean240_Month") = sbMean240.ToString.Trim(",")
            .Application(stockNo + "_K9_Month") = sbK9.ToString.Trim(",")
            .Application(stockNo + "_D9_Month") = sbD9.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

    End Sub

    Public Sub LoadTech(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_Tech_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
             Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sb0000 As New StringBuilder
        Dim sbVolume As New StringBuilder
        Dim sbMean5Volume As New StringBuilder
        Dim sbMean20Volume As New StringBuilder
        Dim sbMean5 As New StringBuilder
        Dim sbMean10 As New StringBuilder
        Dim sbMean20 As New StringBuilder
        Dim sbMean60 As New StringBuilder
        Dim sbMean120 As New StringBuilder
        Dim sbMean240 As New StringBuilder
        Dim sbK9 As New StringBuilder
        Dim sbD9 As New StringBuilder
        Dim sbStrength1 As New StringBuilder
        Dim sbStrength2 As New StringBuilder
        Dim sbStrength3 As New StringBuilder
        Dim sbStrength4 As New StringBuilder
        Dim sbStrength5 As New StringBuilder
        Dim sbMa3Ma6 As New StringBuilder
        Dim ma3Ma6 As New Queue
        Dim close As New Queue
        Dim close0 As Double
        Dim close3 As Double
        Dim currentDate As Date

        Dim sdsTechAll As New SqlDataSource
        sdsTechAll.ConnectionString = twStockConnectString
        sdsTechAll.SelectCommand = "SELECT [Open], High, Low, [Close], Volume, [Date], Mean5, Mean10, Mean20, Mean60, Mean120, Mean240, Mean5Volume, Mean20Volume, K9, D9, RSV, Strength1, Strength2, Strength3, Strength4, Strength5 FROM  HistoryPriceDaily WHERE (StockNo = '@StockNo') Order by [Date]"
        sdsTechAll.SelectCommand = sdsTechAll.SelectCommand.Replace("@StockNo", stockNo)
        Dim en As IEnumerator = sdsTechAll.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String

        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            sbVolume.Append("[")
            sbVolume.Append(span)
            sbVolume.Append(",")
            If IsNumeric(stockNo) Then
                sbVolume.Append(CInt(en.Current("Volume") / 1000))
            Else
                sbVolume.Append(en.Current("Volume"))
            End If

            sbVolume.Append("],")

            If en.Current("Mean5Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean5Volume") <> 0 Then
                sbMean5Volume.Append("[")
                sbMean5Volume.Append(span)
                sbMean5Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean5Volume.Append(en.Current("Mean5Volume") / 1000)
                Else
                    sbMean5Volume.Append(en.Current("Mean5Volume"))
                End If
                sbMean5Volume.Append("],")
            End If

            If en.Current("Mean20Volume") IsNot System.DBNull.Value AndAlso en.Current("Mean20Volume") <> 0 Then
                sbMean20Volume.Append("[")
                sbMean20Volume.Append(span)
                sbMean20Volume.Append(",")
                If IsNumeric(stockNo) Then
                    sbMean20Volume.Append(CInt(en.Current("Mean20Volume") / 1000))
                Else
                    sbMean20Volume.Append(en.Current("Mean20Volume"))
                End If
                sbMean20Volume.Append("],")
            End If

            sb0000.Append("[")
            sb0000.Append(span)
            sb0000.Append(",")
            sb0000.Append(en.Current("Open"))
            sb0000.Append(",")
            sb0000.Append(en.Current("High"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Low"))
            sb0000.Append(",")
            sb0000.Append(en.Current("Close"))
            sb0000.Append("],")

            If en.Current("mean5") IsNot System.DBNull.Value AndAlso en.Current("mean5") <> 0 Then
                sbMean5.Append("[")
                sbMean5.Append(span)
                sbMean5.Append(",")
                sbMean5.Append(Format(en.Current("mean5"), "0.00"))
                sbMean5.Append("],")
            End If

            If en.Current("mean10") IsNot System.DBNull.Value AndAlso en.Current("mean10") <> 0 Then
                sbMean10.Append("[")
                sbMean10.Append(span)
                sbMean10.Append(",")
                sbMean10.Append(Format(en.Current("mean10"), "0.00"))
                sbMean10.Append("],")
            End If

            If en.Current("mean20") IsNot System.DBNull.Value AndAlso en.Current("mean20") <> 0 Then
                sbMean20.Append("[")
                sbMean20.Append(span)
                sbMean20.Append(",")
                sbMean20.Append(Format(en.Current("mean20"), "0.00"))
                sbMean20.Append("],")
            End If

            If en.Current("mean60") IsNot System.DBNull.Value AndAlso en.Current("mean60") <> 0 Then
                sbMean60.Append("[")
                sbMean60.Append(span)
                sbMean60.Append(",")
                sbMean60.Append(Format(en.Current("mean60"), "0.00"))
                sbMean60.Append("],")
            End If

            If en.Current("mean120") IsNot System.DBNull.Value AndAlso en.Current("mean120") <> 0 Then
                sbMean120.Append("[")
                sbMean120.Append(span)
                sbMean120.Append(",")
                sbMean120.Append(Format(en.Current("mean120"), "0.00"))
                sbMean120.Append("],")
            End If

            If en.Current("mean240") IsNot System.DBNull.Value AndAlso en.Current("mean240") <> 0 Then
                sbMean240.Append("[")
                sbMean240.Append(span)
                sbMean240.Append(",")
                sbMean240.Append(Format(en.Current("mean240"), "0.00"))
                sbMean240.Append("],")
            End If

            If en.Current("K9") IsNot System.DBNull.Value AndAlso en.Current("K9") <> 0 Then
                sbK9.Append("[")
                sbK9.Append(span)
                sbK9.Append(",")
                sbK9.Append(Format(en.Current("K9"), "0.0"))
                sbK9.Append("],")
            End If

            If en.Current("D9") IsNot System.DBNull.Value AndAlso en.Current("D9") <> 0 Then
                sbD9.Append("[")
                sbD9.Append(span)
                sbD9.Append(",")
                sbD9.Append(Format(en.Current("D9"), "0.0"))
                sbD9.Append("],")
            End If

            If en.Current("Strength1") IsNot System.DBNull.Value AndAlso en.Current("Strength1") > 0.001 Then
                sbStrength1.Append("[")
                sbStrength1.Append(span)
                sbStrength1.Append(",")
                sbStrength1.Append(Format(en.Current("Strength1"), "0.00"))
                sbStrength1.Append("],")
            End If

            If en.Current("Strength2") IsNot System.DBNull.Value AndAlso en.Current("Strength2") > 0.001 Then
                sbStrength2.Append("[")
                sbStrength2.Append(span)
                sbStrength2.Append(",")
                sbStrength2.Append(Format(en.Current("Strength2"), "0.00"))
                sbStrength2.Append("],")
            End If

            If en.Current("Strength3") IsNot System.DBNull.Value AndAlso en.Current("Strength3") > 0.001 Then
                sbStrength3.Append("[")
                sbStrength3.Append(span)
                sbStrength3.Append(",")
                sbStrength3.Append(Format(en.Current("Strength3"), "0.00"))
                sbStrength3.Append("],")
            End If

            If en.Current("Strength4") IsNot System.DBNull.Value AndAlso en.Current("Strength4") > 0.001 Then
                sbStrength4.Append("[")
                sbStrength4.Append(span)
                sbStrength4.Append(",")
                sbStrength4.Append(Format(en.Current("Strength4"), "0.00"))
                sbStrength4.Append("],")
            End If

            If en.Current("Strength5") IsNot System.DBNull.Value AndAlso en.Current("Strength5") > 0.001 Then
                sbStrength5.Append("[")
                sbStrength5.Append(span)
                sbStrength5.Append(",")
                sbStrength5.Append(Format(en.Current("Strength5"), "0.00"))
                sbStrength5.Append("],")
            End If

            'Ma3 - Ma6
            close.Enqueue(en.Current("Close"))
            If close.Count >= 4 Then
                close0 = en.Current("Close")
                close3 = close.Dequeue
                ma3Ma6.Enqueue(Format(close0 * 2 - close3, "0.00"))
            End If
            If ma3Ma6.Count > 3 Then
                sbMa3Ma6.Append("[")
                sbMa3Ma6.Append(span)
                sbMa3Ma6.Append(",")
                sbMa3Ma6.Append(ma3Ma6.Dequeue)
                sbMa3Ma6.Append("],")
            End If
        End While

        For i As Integer = 1 To 3
            If ma3Ma6.Count = 0 Then Exit For
            sbMa3Ma6.Append("[")
            sbMa3Ma6.Append(currentDate.AddDays(i).Subtract(dt1970).TotalMilliseconds.ToString)
            sbMa3Ma6.Append(",")
            sbMa3Ma6.Append(ma3Ma6.Dequeue)
            sbMa3Ma6.Append("],")
        Next

        With HttpContext.Current
            .Application(stockNo + "_Volume_All") = sbVolume.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_Mean5Volume_All") = sbMean5Volume.ToString.Trim(",")
            .Application(stockNo + "_Mean20Volume_All") = sbMean20Volume.ToString.Trim(",")
            .Application(stockNo + "_DailyK_All") = sb0000.ToString.Trim(",")
            .Application(stockNo + "_Mean5_All") = sbMean5.ToString.Trim(",")
            .Application(stockNo + "_Mean10_All") = sbMean10.ToString.Trim(",")
            .Application(stockNo + "_Mean20_All") = sbMean20.ToString.Trim(",")
            .Application(stockNo + "_Mean60_All") = sbMean60.ToString.Trim(",")
            .Application(stockNo + "_Mean120_All") = sbMean120.ToString.Trim(",")
            .Application(stockNo + "_Mean240_All") = sbMean240.ToString.Trim(",")
            .Application(stockNo + "_K9_All") = sbK9.ToString.Trim(",")
            .Application(stockNo + "_D9_All") = sbD9.ToString.Trim(",")
            .Application(stockNo + "_Strength1_All") = sbStrength1.ToString.Trim(",")
            .Application(stockNo + "_Strength2_All") = sbStrength2.ToString.Trim(",")
            .Application(stockNo + "_Strength3_All") = sbStrength3.ToString.Trim(",")
            .Application(stockNo + "_Strength4_All") = sbStrength4.ToString.Trim(",")
            .Application(stockNo + "_Strength5_All") = sbStrength5.ToString.Trim(",")
            .Application(stockNo + "_Ma3Ma6_All") = sbMa3Ma6.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

    End Sub

    ''' <summary>
    ''' 外資期貨留倉
    ''' </summary>
    Public Sub LoadFuturesPosition()
        Dim timeKey As String = "FuturesPosition_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
            Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sbForeign As New StringBuilder
        Dim sbING As New StringBuilder
        Dim sbDealer As New StringBuilder

        Dim sdsFuturesPosition As New SqlDataSource
        sdsFuturesPosition.ConnectionString = twStockConnectString
        sdsFuturesPosition.SelectCommand = "SELECT [Name],[Time],[PPForeign],[PPING],[PPDealer] FROM [twStocks].[dbo].[Futures] where Name=N'臺股期貨' and [Time] > dateadd(d,-365,getdate()) Order by [Time]"
        Dim en As IEnumerator = sdsFuturesPosition.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String
        While en.MoveNext
            span = en.Current("Time").Subtract(dt1970).TotalMilliseconds.ToString

            If en.Current("PPForeign") IsNot System.DBNull.Value AndAlso en.Current("PPForeign") <> 0 Then
                sbForeign.Append("[")
                sbForeign.Append(span)
                sbForeign.Append(",")
                sbForeign.Append(en.Current("PPForeign"))
                sbForeign.Append("],")
            End If

            If en.Current("PPING") IsNot System.DBNull.Value AndAlso en.Current("PPING") <> 0 Then
                sbDealer.Append("[")
                sbDealer.Append(span)
                sbDealer.Append(",")
                sbDealer.Append(en.Current("PPING"))
                sbDealer.Append("],")
            End If

            If en.Current("PPDealer") IsNot System.DBNull.Value AndAlso en.Current("PPDealer") <> 0 Then
                sbING.Append("[")
                sbING.Append(span)
                sbING.Append(",")
                sbING.Append(en.Current("PPDealer"))
                sbING.Append("],")
            End If
        End While

        With HttpContext.Current
            .Application("FuturesPosition_Foreign") = sbForeign.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application("FuturesPosition_ING") = sbING.ToString.Trim(",")
            .Application("FuturesPosition_Dealer") = sbDealer.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

    End Sub

    ''' <summary>
    ''' 外資買賣超
    ''' </summary>
    Public Sub LoadCorporateBuy(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_CorporateBuy_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
            Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sbForeign As New StringBuilder
        Dim sbING As New StringBuilder
        Dim sbDealer As New StringBuilder

        Dim sdsCorporate As New SqlDataSource
        sdsCorporate.ConnectionString = twStockConnectString
        sdsCorporate.SelectCommand = "SELECT [StockNo],[Date],[BuyForeign],[SellForeign],[SumForeign],[BuyING],[SellING],[SumING],[BuyDealer],[SellDealer],[SumDealer],[Name] FROM [twStocks].[dbo].[TWTU] Where StockNo= '@StockNo' and [Date] > dateadd(d,-365,getdate()) Order by [Date]"
        sdsCorporate.SelectCommand = sdsCorporate.SelectCommand.Replace("@StockNo", stockNo)

        Dim en As IEnumerator = sdsCorporate.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String
        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            If en.Current("SumForeign") IsNot System.DBNull.Value AndAlso en.Current("SumForeign") <> 0 Then
                sbForeign.Append("[")
                sbForeign.Append(span)
                sbForeign.Append(",")
                If stockNo = "0000" Then
                    sbForeign.Append(Format(en.Current("SumForeign") / 100000000, "0.0"))
                Else
                    sbForeign.Append(Format(en.Current("SumForeign") / 1000, "0"))
                End If
                sbForeign.Append("],")
            End If

            If en.Current("SumING") IsNot System.DBNull.Value AndAlso en.Current("SumING") <> 0 Then
                sbING.Append("[")
                sbING.Append(span)
                sbING.Append(",")
                If stockNo = "0000" Then
                    sbING.Append(Format(en.Current("SumING") / 100000000, "0.0"))
                Else
                    sbING.Append(Format(en.Current("SumING") / 1000, "0"))
                End If
                sbING.Append("],")
            End If

            If en.Current("SumDealer") IsNot System.DBNull.Value AndAlso en.Current("SumDealer") <> 0 Then
                sbDealer.Append("[")
                sbDealer.Append(span)
                sbDealer.Append(",")
                If stockNo = "0000" Then
                    sbDealer.Append(Format(en.Current("SumDealer") / 100000000, "0.0"))
                Else
                    sbDealer.Append(Format(en.Current("SumDealer") / 1000, "0"))
                End If
                sbDealer.Append("],")
            End If
        End While

        With HttpContext.Current
            .Application(stockNo + "_BuyForeign") = sbForeign.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
            .Application(stockNo + "_BuyING") = sbING.ToString.Trim(",")
            .Application(stockNo + "_BuyDealer") = sbDealer.ToString.Trim(",")

            .Application(timeKey) = Now.AddMinutes(30)
        End With

    End Sub

    ''' <summary>
    ''' 外資持股比例
    ''' </summary>
    Public Sub LoadForeignHolding(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_ForeignHolding_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
            Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sbForeign As New StringBuilder

        Dim sdsForeignHolding As New SqlDataSource
        sdsForeignHolding.ConnectionString = twStockConnectString
        sdsForeignHolding.SelectCommand = "SELECT [Time],[AFR] FROM [twStocks].[dbo].[ForeignHolding] where StockNo = '@StockNo' and [Time] > dateadd(d,-365,getdate()) order by Time"
        sdsForeignHolding.SelectCommand = sdsForeignHolding.SelectCommand.Replace("@StockNo", stockNo)

        Dim en As IEnumerator = sdsForeignHolding.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String
        While en.MoveNext
            span = en.Current("Time").Subtract(dt1970).TotalMilliseconds.ToString

            If en.Current("AFR") IsNot System.DBNull.Value AndAlso en.Current("AFR") <> 0 Then
                sbForeign.Append("[")
                sbForeign.Append(span)
                sbForeign.Append(",")
                If stockNo = "0000" Then
                    sbForeign.Append(en.Current("AFR"))
                Else
                    sbForeign.Append(en.Current("AFR"))
                End If
                sbForeign.Append("],")
            End If

        End While

        'HttpContext.Current.Application(stockNo + "_ForeignHolding") = sbForeign.ToString + "[" + Now.AddDays(1).Subtract(dt1970).TotalMilliseconds.ToString + ",0]"
        HttpContext.Current.Application(stockNo + "_ForeignHolding") = sbForeign.ToString.Trim(",")


        HttpContext.Current.Application(timeKey) = Now.AddMinutes(30)
    End Sub

    ''' <summary>
    ''' 融資券
    ''' </summary>
    Public Sub LoadFinance(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_Finance_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
            Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sbBuy As New StringBuilder
        Dim sbSell As New StringBuilder
        Dim sbRatio As New StringBuilder

        Dim sdsFinance As New SqlDataSource
        sdsFinance.ConnectionString = twStockConnectString
        If stockNo <> "0000" Then
            sdsFinance.SelectCommand = "SELECT Date,Today1,Today2 FROM Finances Where StockNo = '@StockNo'  and [Date] > dateadd(d,-730,getdate()) Order by [Date]"
            sdsFinance.SelectCommand = sdsFinance.SelectCommand.Replace("@StockNo", stockNo)
        Else
            sdsFinance.SelectCommand = "SELECT d1.Date,round( d1.Today1 /100000,1) as Today1,Today2 " & _
                "FROM Finances INNER JOIN (SELECT Date,Today1 FROM Finances AS Finances_1 WHERE (StockNo = '0000A')) AS d1 ON Finances.Date = d1.Date " & _
                "Where StockNo = '0000' and Finances.[Date] > dateadd(d,-730,getdate()) Order by [Date]"
        End If

        Dim en As IEnumerator = sdsFinance.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String
        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            If en.Current("Today1") IsNot System.DBNull.Value AndAlso en.Current("Today1") <> 0 Then
                sbBuy.Append("[")
                sbBuy.Append(span)
                sbBuy.Append(",")
                sbBuy.Append(en.Current("Today1"))
                sbBuy.Append("],")
            End If

            If en.Current("Today2") IsNot System.DBNull.Value Then
                sbSell.Append("[")
                sbSell.Append(span)
                sbSell.Append(",")
                sbSell.Append(en.Current("Today2"))
                sbSell.Append("],")
            Else
                sbSell.Append("[")
                sbSell.Append(span)
                sbSell.Append(",0],")
            End If

        End While

        HttpContext.Current.Application(stockNo + "_Finance_Buy") = sbBuy.ToString.Trim(",")
        HttpContext.Current.Application(stockNo + "_Finance_Sell") = sbSell.ToString.Trim(",")
        'HttpContext.Current.Application(stockNo + "_Finance_Ratio") = sbRatio.ToString.Trim(",")

        If Now.DayOfWeek = DayOfWeek.Saturday OrElse _
            Now.DayOfWeek = DayOfWeek.Sunday OrElse _
            Val(Format(Now.Hour, "HH")) < 18 Then
            HttpContext.Current.Application(timeKey) = Now.AddHours(3)
        Else
            HttpContext.Current.Application(timeKey) = Now.AddMinutes(30)
        End If


    End Sub
    Public Sub Load0000Finance()
        Dim timeKey As String = "0000_Finance_LastTime"

        If HttpContext.Current.Application(timeKey) IsNot Nothing AndAlso _
            Now < HttpContext.Current.Application(timeKey) Then
            Exit Sub
        End If

        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)

        Dim sbBuy As New StringBuilder
        Dim sbSell As New StringBuilder
        Dim sbRatio As New StringBuilder

        Dim sdsFinance As New SqlDataSource
        sdsFinance.ConnectionString = twStockConnectString
        sdsFinance.SelectCommand = "SELECT Date,Today1,Today2 FROM Finances Where StockNo = '0000A'  and [Date] > dateadd(d,-730,getdate()) Order by [Date]"

        Dim en As IEnumerator = sdsFinance.Select(New DataSourceSelectArguments).GetEnumerator
        Dim span As String
        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            If en.Current("Today1") IsNot System.DBNull.Value AndAlso en.Current("Today1") <> 0 Then
                sbBuy.Append("[")
                sbBuy.Append(span)
                sbBuy.Append(",")
                sbBuy.Append(Format(en.Current("Today1") / 100000, "0.0"))
                sbBuy.Append("],")
            End If

        End While

        HttpContext.Current.Application("0000_Finance_Buy") = sbBuy.ToString.Trim(",")

        sdsFinance.SelectParameters("StockNo").DefaultValue = "0000"
        en = sdsFinance.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            span = en.Current("Date").Subtract(dt1970).TotalMilliseconds.ToString

            If en.Current("Today2") IsNot System.DBNull.Value AndAlso en.Current("Today2") <> 0 Then
                sbSell.Append("[")
                sbSell.Append(span)
                sbSell.Append(",")
                sbSell.Append(en.Current("Today2"))
                sbSell.Append("],")
            End If

        End While
        HttpContext.Current.Application("0000_Finance_Sell") = sbSell.ToString.Trim(",")

        If Now.DayOfWeek = DayOfWeek.Saturday OrElse _
            Now.DayOfWeek = DayOfWeek.Sunday OrElse _
            Val(Format(Now.Hour, "HH")) < 18 Then
            HttpContext.Current.Application(timeKey) = Now.AddHours(3)
        Else
            HttpContext.Current.Application(timeKey) = Now.AddMinutes(30)
        End If

    End Sub

    ''' <summary>
    ''' 股價資訊
    ''' </summary>
    Public Sub LoadPrice(ByVal stockNo As String)
        Dim timeKey As String = stockNo + "_Price_LastTime"
        With HttpContext.Current

            If .Application(timeKey) IsNot Nothing AndAlso _
                Now < .Application(timeKey) Then
                Exit Sub
            End If
            Dim sdsStock As New SqlDataSource
            sdsStock.ConnectionString = twStockConnectString
            sdsStock.SelectCommand = "SELECT Stock.Market, Stock.Deal, Stock.Last, Stock.[Open], Stock.Buy, Stock.Up1Price, Stock.Sell, Stock.Down1Price, Stock.High, Stock.Low, Stock.Change, Stock.SingleVolume, Stock.TotalVolume, Stock.EPS, Stock.Mean60Distance, Stock.Mean60DistanceRate, Stock.NoNewHighLowDates,ISNULL(Stock.PriceVolume,0) as PriceVolume, ISNULL(Stock.NPTT,0) as NPTT, Stock.Up1Price, Stock.Up1Volume, Stock.Down1Price, Stock.Down1Volume,Stock.Time, Stock.RankDuration, Stock.Mean5, Stock.Mean10, Stock.Mean20, Stock.Mean60, Stock.Mean120, Stock.Mean240,ISNULL(Stock.Mean5Volume,0) as Mean5Volume,ISNULL(Stock.Mean20Volume,0) as Mean20Volume,Stock.K9,Stock.D9, Market.Name AS MarketName FROM Stock INNER JOIN Market ON Stock.Market = Market.MarketId WHERE (Stock.StockNo = '@StockNo')"
            sdsStock.SelectCommand = sdsStock.SelectCommand.Replace("@StockNo", stockNo)
            Dim en As IEnumerator = sdsStock.Select(New DataSourceSelectArguments).GetEnumerator
            If en.MoveNext Then

                Dim market As Integer = en.Current("Market")
                .Application(stockNo + "_Market") = market
                .Application(stockNo + "_Time") = en.Current("Time")
                .Application(stockNo + "_Deal") = en.Current("Deal")
                Dim last As Double = en.Current("Deal") - en.Current("Change")
                .Application(stockNo + "_Last") = last
                .Application(stockNo + "_Open") = en.Current("Open")
                .Application(stockNo + "_High") = en.Current("High")
                .Application(stockNo + "_Low") = en.Current("Low")
                .Application(stockNo + "_Change") = en.Current("Change")
                If en.Current("Change") = 0 Then
                    .Application(stockNo + "_ChangeRatio") = 0
                Else
                    .Application(stockNo + "_ChangeRatio") = en.Current("Change") / (en.Current("Deal") - en.Current("Change"))
                End If
                If IsNumeric(stockNo) Then
                    .Application(stockNo + "_TotalVolume") = en.Current("TotalVolume") / 1000
                Else
                    .Application(stockNo + "_TotalVolume") = en.Current("TotalVolume")
                End If
                If IsNumeric(stockNo) Then
                    .Application(stockNo + "_Mean5Volume") = en.Current("Mean5Volume") / 1000
                ElseIf en.Current("Mean5Volume") IsNot DBNull.Value Then
                    .Application(stockNo + "_Mean5Volume") = en.Current("Mean5Volume")
                End If
                If IsNumeric(stockNo) Then
                    .Application(stockNo + "_Mean20Volume") = en.Current("Mean20Volume") / 1000
                ElseIf en.Current("Mean20Volume") IsNot DBNull.Value Then
                    .Application(stockNo + "_Mean20Volume") = en.Current("Mean20Volume")
                End If
                .Application(stockNo + "_Mean5") = en.Current("Mean5")
                .Application(stockNo + "_Mean10") = en.Current("Mean10")
                .Application(stockNo + "_Mean20") = en.Current("Mean20")
                .Application(stockNo + "_Mean60") = en.Current("Mean60")
                .Application(stockNo + "_Mean120") = en.Current("Mean120")
                .Application(stockNo + "_Mean240") = en.Current("Mean240")
                If en.Current("K9") IsNot DBNull.Value Then
                    .Application(stockNo + "_K9") = en.Current("K9")
                End If
                If en.Current("D9") IsNot DBNull.Value Then
                    .Application(stockNo + "_D9") = en.Current("D9")
                End If

                If IsNumeric(stockNo) AndAlso stockNo <> "0000" Then
                    .Application(stockNo + "_Sell") = en.Current("Sell")
                    .Application(stockNo + "_Buy") = en.Current("Buy")
                    .Application(stockNo + "_Up1Price") = en.Current("Up1Price")
                    .Application(stockNo + "_Up1Volume") = en.Current("Up1Volume")
                    .Application(stockNo + "_Down1Price") = en.Current("Down1Price")
                    .Application(stockNo + "_Down1Volume") = en.Current("Down1Volume")
                    .Application(stockNo + "_SingleVolume") = en.Current("SingleVolume")
                    .Application(stockNo + "_EPS") = en.Current("EPS")
                    .Application(stockNo + "_Mean60Distance") = en.Current("Mean60Distance")
                    .Application(stockNo + "_Mean60DistanceRate") = en.Current("Mean60DistanceRate")
                    .Application(stockNo + "_NoNewHighLowDates") = en.Current("NoNewHighLowDates")
                    .Application(stockNo + "_PriceVolume") = en.Current("PriceVolume")
                    .Application(stockNo + "_NPTT") = en.Current("NPTT")
                    .Application(stockNo + "_MarketName") = en.Current("MarketName")
                    .Application(stockNo + "_RankDuration") = en.Current("RankDuration")
                    .Application(stockNo + "_RaiseLimit") = TwStockAccessor.Instance.GetUpLimitPrice(last)
                    .Application(stockNo + "_FallLimit") = TwStockAccessor.Instance.GetDownLimitPrice(last)

                    Dim high As Double = en.Current("High")
                    If high > last AndAlso high > 0 Then
                        .Application(stockNo + "_max") = high
                    Else
                        .Application(stockNo + "_max") = last
                    End If

                    Dim low As Double = en.Current("Low")
                    If low < last AndAlso low > 0 Then
                        .Application(stockNo + "_min") = low
                    Else
                        .Application(stockNo + "_min") = last
                    End If
                End If

                If stockNo = "0000" Then
                    Dim sdsVolumeEstimate As New SqlDataSource
                    sdsVolumeEstimate.ConnectionString = twStockConnectString
                    sdsVolumeEstimate.SelectCommand = "Select Top 1 Rate From TradeVolumeEstimate Where [Time] >= Cast(GETDATE() as time(7))"
                    en = sdsVolumeEstimate.Select(New DataSourceSelectArguments).GetEnumerator
                    If en.MoveNext Then
                        .Application("0000_VolumeEstimate") = en.Current("Rate") * .Application("0000_TotalVolume")
                    End If
                End If

                If IsNumeric(stockNo) Then
                    Me.LoadMarketCapital(stockNo)

                    Dim sdsStockClass As New SqlDataSource
                    sdsStockClass.ConnectionString = twStockConnectString
                    sdsStockClass.SelectCommand = "SELECT Top 1 Class.Name, Class.ChangeDay FROM Class INNER JOIN StockClass ON Class.ClassId = StockClass.ClassId WHERE (StockClass.StockNo = '@StockNo') AND Class.[Group] > 1 order by [Group]"
                    sdsStockClass.SelectCommand = sdsStockClass.SelectCommand.Replace("@StockNo", stockNo)
                    en = sdsStockClass.Select(New DataSourceSelectArguments).GetEnumerator
                    If en.MoveNext Then
                        .Application(stockNo + "_ClassName") = en.Current("Name").ToString.Replace(.Application(stockNo + "_MarketName"), "")
                        If en.Current("ChangeDay") Is DBNull.Value Then
                            .Application(stockNo + "_ClassChangeRate") = 0
                        Else
                            .Application(stockNo + "_ClassChangeRate") = en.Current("ChangeDay")
                        End If
                    End If
                End If

                If ((Now.Hour >= 8 AndAlso Now.Hour <= 13) AndAlso market < 10 AndAlso market <> 3) OrElse _
                    market = 3 OrElse _
                    ((Now.Hour >= 8 AndAlso Now.Hour <= 15) AndAlso market = 10) OrElse _
                    ((Now.Hour >= 18 OrElse Now.Hour <= 6) AndAlso market = 12) OrElse _
                    ((Now.Hour >= 13 AndAlso Now.Hour <= 23) AndAlso market = 11) OrElse _
                    (market >= 20 AndAlso market <= 40) OrElse _
                    (stockNo.Trim = "S2TWZ1") Then
                    .Application(timeKey) = Now.AddSeconds(10)
                    .Application(stockNo + "_RealTimePrice_IsUpdate") = True
                Else
                    .Application(timeKey) = Now.AddMinutes(30)
                    .Application(stockNo + "_RealTimePrice_IsUpdate") = False
                End If

            End If

            'StockDataAccessor.Instance.SetPrice(stockNo, .Application(stockNo + "_Deal"), _
            '                                        .Application(stockNo + "_Change"), .Application(stockNo + "_ChangeRatio"))
        End With
    End Sub

    ''' <summary>
    ''' 市值
    ''' </summary>
    Private Sub LoadMarketCapital(stockNo As String)
        If IsNumeric(stockNo) = False OrElse stockNo = "0000" Then Exit Sub

        Dim timeKey As String = stockNo + "_CompanyProfile_LastTime"
        With HttpContext.Current

            If .Application(timeKey) IsNot Nothing AndAlso _
                Now < .Application(timeKey) Then
                Exit Sub
            End If

            Dim sdsCompany As New SqlDataSource
            sdsCompany.ConnectionString = twStockConnectString
            sdsCompany.SelectCommand = "SELECT CASE ISNUMERIC(TDR) WHEN 1 THEN TDR ELSE '0' END AS TDR FROM CompanyProfile WHERE (CompanyProfile.StockNo = '@StockNo')"
            sdsCompany.SelectCommand = sdsCompany.SelectCommand.Replace("@StockNo", stockNo)
            Dim en As IEnumerator = sdsCompany.Select(New DataSourceSelectArguments).GetEnumerator
            If en.MoveNext Then

                If .Application(stockNo + "_Deal") IsNot Nothing AndAlso IsDBNull(en.Current("TDR")) = False Then

                    Dim tdr As String = en.Current("TDR").ToString.Trim
                    If IsNumeric(tdr) Then
                        .Application(stockNo + "_MarketCapital") = (.Application(stockNo + "_Deal") * CSng(tdr) / 1000000)
                    End If

                End If
            End If

            If (Now.Hour >= 8 AndAlso Now.Hour <= 13) Then
                .Application(timeKey) = Now.AddMinutes(30)
            Else
                .Application(timeKey) = Now.AddHours(1)
            End If
        End With
    End Sub

    ''' <summary>
    ''' 三大法人最新買賣超與融資券
    ''' </summary>
    Public Sub LoadTWTU()
        With HttpContext.Current

            Dim timeKey As String = "Twtu_Finance_LastTime"
            If .Application(timeKey) IsNot Nothing AndAlso _
                Now < .Application(timeKey) Then
                Exit Sub
            End If

            Dim sdsTwtuNew As New SqlDataSource
            sdsTwtuNew.ConnectionString = twStockConnectString
            sdsTwtuNew.SelectCommand = "SELECT TWTU.BuyForeign, TWTU.SellForeign, TWTU.SumForeign, TWTU.BuyING, TWTU.SellING, TWTU.SumING, TWTU.BuyDealer, TWTU.SellDealer, TWTU.SumDealer, TWTU.StockNo FROM TWTU INNER JOIN Stock ON TWTU.StockNo = Stock.StockNo WHERE (TWTU.Date = (SELECT MAX(Date) FROM TWTU)) AND (Stock.Market < 5)"

            Dim stockNo As String = ""
            Dim en As IEnumerator = sdsTwtuNew.Select(New DataSourceSelectArguments).GetEnumerator
            While en.MoveNext
                stockNo = en.Current("StockNo")

                .Application(stockNo + "BuyForeign") = en.Current("BuyForeign")
                .Application(stockNo + "SellForeign") = en.Current("SellForeign")
                .Application(stockNo + "SumForeign") = en.Current("SumForeign")
                .Application(stockNo + "BuyING") = en.Current("BuyING")
                .Application(stockNo + "SellING") = en.Current("SellING")
                .Application(stockNo + "SumING") = en.Current("SumING")
                .Application(stockNo + "BuyDealer") = en.Current("BuyDealer")
                .Application(stockNo + "SellDealer") = en.Current("SellDealer")
                .Application(stockNo + "SumDealer") = en.Current("SumDealer")
                .Application(stockNo + "SumTwtu") = en.Current("SumDealer") + en.Current("SumING") + en.Current("SumForeign")
            End While

            Dim sdsFinanceNew As New SqlDataSource
            sdsFinanceNew.ConnectionString = twStockConnectString
            sdsFinanceNew.SelectCommand = "SELECT Diff, Limit2, Before2, Today2, Repay2, Sell2, Buy2, Limit1, Before1, Today1, Repay1, Buy1, Sell1, Date, StockNo FROM Finances Where Date = (Select MAX(Date) From Finances)"

            en = sdsFinanceNew.Select(New DataSourceSelectArguments).GetEnumerator
            While en.MoveNext
                stockNo = en.Current("StockNo")

                .Application(stockNo + "Limit2") = en.Current("Limit2")
                .Application(stockNo + "Before2") = en.Current("Before2")
                .Application(stockNo + "Today2") = en.Current("Today2")
                .Application(stockNo + "Repay2") = en.Current("Repay2")
                .Application(stockNo + "Sell2") = en.Current("Sell2")
                .Application(stockNo + "Buy2") = en.Current("Buy2")
                .Application(stockNo + "Limit1") = en.Current("Limit1")
                .Application(stockNo + "Before1") = en.Current("Before1")
                .Application(stockNo + "Today1") = en.Current("Today1")
                .Application(stockNo + "Repay1") = en.Current("Repay1")
                .Application(stockNo + "Buy1") = en.Current("Buy1")
                .Application(stockNo + "Sell1") = en.Current("Sell1")
            End While

            .Application(timeKey) = Now.AddMinutes(30)
        End With
    End Sub

End Class
