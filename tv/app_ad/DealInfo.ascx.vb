
Partial Class DealInfo
    Inherits System.Web.UI.UserControl

    Property Head As String
        Get
            Return Me.lHead.Text
        End Get
        Set(value As String)
            Me.lHead.Text = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Me.LoadData()
        End If
    End Sub

    Private Sub LoadData()
        Dim cStockNo As String = "0000"
        If Request("no") IsNot Nothing Then
            cStockNo = Request("no").Replace("$", "&").Trim()
        End If

        If cStockNo.Length < 10 Then
            sdsStock.SelectParameters("StockNo").DefaultValue = cStockNo
            DisplayColor()
        End If
    End Sub

    Private Sub DisplayColor()

        Dim lblStockNo As Label = fv.FindControl("lblStockNo")
        Dim lblDeal As Label = fv.FindControl("lblDeal")
        Dim lblChange As Label = fv.FindControl("lblChange")
        Dim lblHigh As Label = fv.FindControl("lblHigh")
        Dim lblOpen As Label = fv.FindControl("lblOpen")
        Dim lblTotalVolume As Label = fv.FindControl("lblTotalVolume")
        Dim lblPercentage As Label = fv.FindControl("lblPercentage")
        Dim lblLow As Label = fv.FindControl("lblLow")
        Dim lblLast As Label = fv.FindControl("lblLast")
        Dim lblEPS As Label = fv.FindControl("lblEPS")
        Dim lblTime As Label = fv.FindControl("lblTime")
        Dim time As Date = CDate(lblTime.Text)
        lblTime.Text = time.Month & "/" & time.Day & " " & Format(time.Hour, "00") & ":" & Format(time.Minute, "00")
   
        Me.lUpdateTime.Text = lblTime.Text
        If IsNumeric(lblStockNo.Text) = False Then
            Me.lUpdateTime.Text &= "(當地時間)"
        End If

        Dim stockNo As String = lblStockNo.Text
        Dim dealPrice As Double = Val(lblDeal.Text)
        Dim differencePrice As Double = Val(lblChange.Text)
        Dim highPrice As Double = Val(lblHigh.Text)
        Dim lowPrice As Double = Val(lblLow.Text)
        Dim openPrice As Double = Val(lblOpen.Text)

        Dim last As Double = Val(lblLast.Text)
        Dim UpLimitePrice As Double = TwStockAccessor.Instance.GetUpLimitPrice(last)
        Dim DownLimitePrice As Double = TwStockAccessor.Instance.GetDownLimitPrice(last)
 
        lblDeal.Text = Format(dealPrice, "0.00")
        lblChange.Text = Format(differencePrice, "0.00")

        lblHigh.Text = Format(highPrice, "0.00")
        lblLow.Text = Format(lowPrice, "0.00")
        lblOpen.Text = Format(openPrice, "0.00")
        lblLast.Text = Format(last, "0.00")

        '最高價
        If highPrice > last Then
            lblHigh.ForeColor = Drawing.Color.Red
            If highPrice >= UpLimitePrice Then lblHigh.BackColor = Drawing.Color.FromArgb(255, 192, 192)
        ElseIf highPrice < last Then
            lblHigh.ForeColor = Drawing.Color.Green
            If highPrice <= DownLimitePrice Then lblHigh.BackColor = Drawing.Color.FromArgb(192, 255, 192)
        ElseIf highPrice = last Then
            lblHigh.ForeColor = Drawing.Color.Black
        End If
        If highPrice = 0 Then lblHigh.Text = "----"

        '最低價
        If lowPrice > last Then
            lblLow.ForeColor = Drawing.Color.Red
            If lowPrice >= UpLimitePrice Then lblLow.BackColor = Drawing.Color.FromArgb(255, 192, 192)
        ElseIf lowPrice < last Then
            lblLow.ForeColor = Drawing.Color.Green
            If lowPrice <= DownLimitePrice Then lblLow.BackColor = Drawing.Color.FromArgb(192, 255, 192)
        ElseIf lowPrice = last Then
            lblLow.ForeColor = Drawing.Color.Black
        End If
        If lowPrice = 0 Then lblLow.Text = "----"

        '開盤價
        If openPrice > last Then
            lblOpen.ForeColor = Drawing.Color.Red
            If openPrice >= UpLimitePrice Then lblOpen.BackColor = Drawing.Color.FromArgb(255, 192, 192)
        ElseIf openPrice < last Then
            lblOpen.ForeColor = Drawing.Color.Green
            If openPrice <= DownLimitePrice Then lblOpen.BackColor = Drawing.Color.FromArgb(192, 255, 192)
        ElseIf openPrice = last Then
            lblOpen.ForeColor = Drawing.Color.Black
        End If
        If openPrice = 0 Then lblOpen.Text = "----"

        If differencePrice > 0 Then
            lblChange.Text = "+" + lblChange.Text.Trim.Trim("+")
            lblDeal.ForeColor = Drawing.Color.Red
            lblChange.ForeColor = Drawing.Color.Red
            lblPercentage.ForeColor = Drawing.Color.Red
            If dealPrice >= UpLimitePrice Then
                lblDeal.BackColor = Drawing.Color.FromArgb(255, 192, 192)
                lblChange.BackColor = Drawing.Color.FromArgb(255, 192, 192)
                lblPercentage.BackColor = Drawing.Color.FromArgb(255, 192, 192)
            End If
        ElseIf differencePrice < 0 Then
            lblDeal.ForeColor = Drawing.Color.Green
            lblChange.ForeColor = Drawing.Color.Green
            lblPercentage.ForeColor = Drawing.Color.Green
            If dealPrice <= DownLimitePrice Then
                lblDeal.BackColor = Drawing.Color.FromArgb(192, 255, 192)
                lblChange.BackColor = Drawing.Color.FromArgb(192, 255, 192)
                lblPercentage.BackColor = Drawing.Color.FromArgb(192, 255, 192)
            End If

        ElseIf differencePrice = 0 Then
            lblDeal.ForeColor = Drawing.Color.Black
            lblChange.ForeColor = Drawing.Color.Black
            lblPercentage.ForeColor = Drawing.Color.Black

        End If

        '成交量
        If stockNo = "0000" Then
            lblTotalVolume.Text = Format((lblTotalVolume.Text) / 1000, "#,###,##0.0")
        ElseIf IsNumeric(stockNo) Then
            lblTotalVolume.Text = Format((lblTotalVolume.Text) / 1000, "#,###,##0")
        Else
            lblTotalVolume.Text = "--"
        End If
    End Sub
End Class
