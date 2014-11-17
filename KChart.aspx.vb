
Partial Class KChart
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing Then Exit Sub
        stockNo = stockNo.Replace("$", "&")

        Dim timeKey As String = stockNo + "Price_LastTime"
        If Application(timeKey) Is Nothing Then
            'Response.Redirect("/stock/loaddata.aspx?f=price&no=" + stockNo.Replace("&", "$") + "&url=" + Request.Url.ToString.Replace("&", "$"))
            StockLoader.Instance.LoadPrice(stockNo)
        End If
        If Application(stockNo + "_Name") Is Nothing Then Exit Sub
        Me.Title = Application(stockNo + "_Name") + "(" + stockNo.ToUpper + ") - 動態K線圖 - 玩股網 手機版"

        Dim tVolume As String = "量"
        If stockNo.Contains("S2TWZ1") Then tVolume = "OI" '未平倉縮寫

        Dim value As Single = Val(Application(stockNo + "_TotalVolume"))
        Dim tValue As String = value
        If value > 1000 Then tValue = Format(value / 1000, "#,###,###,###,###.0") & "(千)"
        If value > 1000000 Then tValue = Format(value / 1000000, "#,###,###,###,###.0") & "(百萬)"
        If value > 100000000 Then tValue = Format(value / 100000000, "#,###,###,###,###.0") & "(億)"

        a.Text = _
        "<table width=""317px"" class=""charttable"" cellpadding=""0"" cellspacing=""0""><tr><td colspan=""2"">" + Format(Application(stockNo + "_Time"), "MM/dd") + "</td>" + _
        "</td><td class=""name"">收</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_Deal")) + _
        "</td><td class=""name"">" + tVolume + "</td><td class=""val"">" + tValue + _
        "</tr><tr></td><td class=""name"">開</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_Open")) + _
        "</td><td class=""name"">高</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_High")) + _
        "</td><td class=""name"">低</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_Low")) + _
        "</td></tr><tr>" + Me.GetMean(stockNo.Trim) + _
        "</td></tr></table>"
    End Sub

    Private Function GetPrice(price As Single) As String
        If price > 1000 Then
            Return Format(price, "0.0").ToString
        Else
            Return price.ToString
        End If
    End Function

    Private Function GetMean(ByVal stockNo As String) As String
        Dim mean As String
        If IsNumeric(stockNo) = True Then '台股
            Me.GetPrice(Application(stockNo + "_Mean60"))
            mean = _
        "<td class=""name"">5日</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_Mean5")) + _
        "</td><td class=""name"">20日</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_Mean20")) + _
        "</td><td class=""name"">60日</td><td class=""val"">" + Me.GetPrice(Application(stockNo + "_Mean60"))
        Else
            Dim mean5() As String = GetData("mean5").Split(",")
            Dim lastMean5() As String = mean5(mean5.Length - 1).Replace("]", "").Split(",")
            Dim mean10() As String = GetData("mean10").Split(",")
            Dim lastMean10() As String = mean10(mean10.Length - 1).Replace("]", "").Split(",")
            Dim mean20() As String = GetData("mean20").Split(",")
            Dim lastMean20() As String = mean20(mean20.Length - 1).Replace("]", "").Split(",")
            Dim mean60() As String = GetData("mean60").Split(",")
            Dim lastMean60() As String = mean60(mean60.Length - 1).Replace("]", "").Split(",")
            Dim mean120() As String = GetData("mean120").Split(",")
            Dim lastMean120() As String = mean120(mean120.Length - 1).Replace("]", "").Split(",")
            Dim K9() As String = GetData("K9").Split(",")
            Dim lastK9() As String = K9(K9.Length - 1).Replace("]", "").Split(",")
            Dim D9() As String = GetData("D9").Split(",")
            Dim lastD9() As String = D9(D9.Length - 1).Replace("]", "").Split(",")
            mean = _
        "</td><td class=""name"">5日</td><td>" + Me.GetPrice(CSng(lastMean5(lastMean5.Length - 1).Trim)) + _
        "</td><td class=""name"">20日</td><td>" + Me.GetPrice(CSng(lastMean20(lastMean20.Length - 1).Trim)) + _
        "</td><td class=""name"">60日</td><td>" + Me.GetPrice(CSng(lastMean60(lastMean60.Length - 1).Trim))
        End If

        Return mean
    End Function

    Public Function GetData(ByVal type As String) As String
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing Then Return "WantGoo玩股網 最棒的理財網站"
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If Request("m") Is Nothing AndAlso Request("h") IsNot Nothing Then
            If Application(stockNo + "_Tech0.25Y_LastTime") Is Nothing OrElse Now > Application(stockNo + "_Tech0.25Y_LastTime") Then
                'StockLoader.Instance.LoadTech1Y(stockNo)
                StockLoader.Instance.LoadTechDaily(stockNo, 0.25)
            End If

            Dim time As String = "_0.25Y"

            Select Case type
                Case "Volume"
                    Return Application(stockNo + "_Volume" + time)
                Case "Mean5Volume"
                    Return Application(stockNo + "_Mean5Volume" + time)
                Case "Mean20Volume"
                    Return Application(stockNo + "_Mean20Volume" + time)
                Case "0000"
                    Return Application(stockNo + "_DailyK" + time)
                Case "mean5"
                    Return Application(stockNo + "_mean5" + time)
                Case "mean10"
                    Return Application(stockNo + "_mean10" + time)
                Case "mean20"
                    Return Application(stockNo + "_mean20" + time)
                Case "mean60"
                    Return Application(stockNo + "_mean60" + time)
                Case "mean120"
                    Return Application(stockNo + "_mean120" + time)
                Case "mean240"
                    Return Application(stockNo + "_mean240" + time)
                Case "K9"
                    Return Application(stockNo + "_K9" + time)
                Case "D9"
                    Return Application(stockNo + "_D9" + time)
                Case "Deduct20MA"
                    Return Application(stockNo + "_Deduct20MA")
                Case "Deduct60MA"
                    Return Application(stockNo + "_Deduct60MA")
                Case "Deduct20MAValue"
                    Return Application(stockNo + "_Deduct20MAValue")
                Case "Deduct60MAValue"
                    Return Application(stockNo + "_Deduct60MAValue")
            End Select

        ElseIf Request("m") Is Nothing OrElse Request("m") = "j" Then

            If Application(stockNo + "_Tech0.25Y_LastTime") Is Nothing OrElse Now > Application(stockNo + "_Tech0.25Y_LastTime") Then
                'StockLoader.Instance.LoadTech1Y(stockNo)
                StockLoader.Instance.LoadTechDaily(stockNo, 0.25)
            End If

            Dim time As String = "_0.25Y"

            Select Case type
                Case "Volume"
                    Return Application(stockNo + "_Volume" + time)
                Case "Mean5Volume"
                    Return Application(stockNo + "_Mean5Volume" + time)
                Case "Mean20Volume"
                    Return Application(stockNo + "_Mean20Volume" + time)
                Case "0000"
                    Return Application(stockNo + "_DailyK" + time)
                Case "mean5"
                    Return Application(stockNo + "_mean5" + time)
                Case "mean10"
                    Return Application(stockNo + "_mean10" + time)
                Case "mean20"
                    Return Application(stockNo + "_mean20" + time)
                Case "mean60"
                    Return Application(stockNo + "_mean60" + time)
                Case "mean120"
                    Return Application(stockNo + "_mean120" + time)
                Case "mean240"
                    Return Application(stockNo + "_mean240" + time)
                Case "K9"
                    Return Application(stockNo + "_K9" + time)
                Case "D9"
                    Return Application(stockNo + "_D9" + time)
                Case "Deduct20MA"
                    Return Application(stockNo + "_Deduct20MA")
                Case "Deduct60MA"
                    Return Application(stockNo + "_Deduct60MA")
                Case "Deduct20MAValue"
                    Return Application(stockNo + "_Deduct20MAValue")
                Case "Deduct60MAValue"
                    Return Application(stockNo + "_Deduct60MAValue")
            End Select

        ElseIf Request("m") IsNot Nothing AndAlso Request("m").Trim.ToLower = "all" Then

            If Application(stockNo + "_Tech_LastTime") Is Nothing OrElse Now > Application(stockNo + "_Tech_LastTime") Then
                StockLoader.Instance.LoadTech(stockNo)
            End If

            Select Case type
                Case "Volume"
                    Return Application(stockNo + "_Volume_All")
                Case "Mean5Volume"
                    Return Application(stockNo + "_Mean5Volume_All")
                Case "Mean20Volume"
                    Return Application(stockNo + "_Mean20Volume_All")
                Case "0000"
                    Return Application(stockNo + "_DailyK_All")
                Case "mean5"
                    Return Application(stockNo + "_Mean5_All")
                Case "mean10"
                    Return Application(stockNo + "_Mean10_All")
                Case "mean20"
                    Return Application(stockNo + "_Mean20_All")
                Case "mean60"
                    Return Application(stockNo + "_Mean60_All")
                Case "mean120"
                    Return Application(stockNo + "_Mean120")
                Case "mean240"
                    Return Application(stockNo + "_Mean240_All")
                Case "K9"
                    Return Application(stockNo + "_K9_All")
                Case "D9"
                    Return Application(stockNo + "_D9_All")
                Case "Deduct20MA"
                    Return Application(stockNo + "_Deduct20MA")
                Case "Deduct60MA"
                    Return Application(stockNo + "_Deduct60MA")
                Case "Deduct20MAValue"
                    Return Application(stockNo + "_Deduct20MAValue")
                Case "Deduct60MAValue"
                    Return Application(stockNo + "_Deduct60MAValue")
            End Select

        Else

            If Application(stockNo + "_Tech6M_LastTime") Is Nothing OrElse Now > Application(stockNo + "_Tech6M_LastTime") Then
                'Response.Redirect("/stock/loaddata.aspx?f=tech6m&no=" + stockNo.Replace("&", "$") + "&url=" + Request.Url.ToString.Replace("&", "$"))
                StockLoader.Instance.LoadTech6M(stockNo)
            End If
            Dim time As String = "_6M"
            Select Case type
                Case "Volume"
                    Return Application(stockNo + "_Volume" + time)
                Case "Mean5Volume"
                    Return Application(stockNo + "_Mean5Volume" + time)
                Case "Mean20Volume"
                    Return Application(stockNo + "_Mean20Volume" + time)
                Case "0000"
                    Return Application(stockNo + "_DailyK" + time)
                Case "mean5"
                    Return Application(stockNo + "_mean5" + time)
                Case "mean10"
                    Return Application(stockNo + "_mean10" + time)
                Case "mean20"
                    Return Application(stockNo + "_mean20" + time)
                Case "mean60"
                    Return Application(stockNo + "_mean60" + time)
                Case "mean120"
                    Return Application(stockNo + "_mean120" + time)
                Case "mean240"
                    Return Application(stockNo + "_mean240" + time)
                Case "K9"
                    Return Application(stockNo + "_K9" + time)
                Case "D9"
                    Return Application(stockNo + "_D9" + time)
                Case "Deduct20MA"
                    Return Application(stockNo + "_Deduct20MA")
                Case "Deduct60MA"
                    Return Application(stockNo + "_Deduct60MA")
                Case "Deduct20MAValue"
                    Return Application(stockNo + "_Deduct20MAValue")
                Case "Deduct60MAValue"
                    Return Application(stockNo + "_Deduct60MAValue")
            End Select
        End If

        Return ""
    End Function

    Public Function GetTitle() As String
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing Then Return "WantGoo玩股網 最棒的理財網站"
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If Application(stockNo + "_Name") IsNot Nothing Then
            Return Application(stockNo + "_Name") + " (" + stockNo.ToUpper + ")"
        End If

        'Response.Redirect("/stock/loaddata.aspx?f=name&url=" + Request.Url.ToString)
        StockLoader.Instance.LoadStockName()

        Return Application(stockNo + "_Name") + " (" + stockNo.ToUpper + ")"
    End Function

    Public Function GetDataTitle() As String
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing Then Return "WantGoo玩股網 最棒的理財網站"
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If Application(stockNo + "_Name") IsNot Nothing Then
            Return Application(stockNo + "_Name")
        End If

        'Response.Redirect("/stock/loaddata.aspx?f=name&url=" + Request.Url.ToString)
        StockLoader.Instance.LoadStockName()

        Return Application(stockNo + "_Name")
    End Function

    Public Function GetVolumeTitle() As String
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing Then Return "成交量"

        If IsNumeric(stockNo) Then
            If stockNo = "0000" Then Return "成交量(億)"
            Return "成交量(張)"
        ElseIf stockNo.Contains("S2TWZ1") Then
            Return "未平倉"
        Else
            '全球指數
            Return "成交量"
        End If
        Return "成交量"
    End Function

     
End Class
