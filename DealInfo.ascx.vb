
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Me.LoadData()
        End If
    End Sub
    ''' <summary>
    ''' 加入所有個股到我的追蹤
    ''' </summary>
    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAll.Click

        If Not MemberDataAccessor.Instance.IsLogin Then
            Response.Redirect("/Login.aspx")
            Throw New Exception("會員未登入")
        End If

        If Not MemberDataAccessor.Instance.IsLogin OrElse _
        MemberDataAccessor.Instance.GetMemberNo = "0" Then Exit Sub

        Dim stockNo As String
        stockNo = Request("no")
        If Not String.IsNullOrEmpty(stockNo) Then
            sdsCollect.InsertParameters("MemberNo").DefaultValue = MemberDataAccessor.Instance.GetMemberNo
            sdsCollect.InsertParameters("StockNo").DefaultValue = stockNo
            sdsCollect.InsertParameters("Group").DefaultValue = "1"
            sdsCollect.Insert()
            Response.Redirect("/Hottip/HottipPage1.aspx")
        End If

    End Sub

    Private Sub LoadData()
        If lHead.Text.Contains("個股報價") Then
            '顯示台股的排版
            fvTw.Visible = True
            fv.Visible = False
            fvTw.DataSourceID = "sdsStock"
        Else
            '台股以外
            fvTw.Visible = False
            fv.Visible = True
            fv.DataSourceID = "sdsStock"
        End If

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
        Dim view As FormView = Nothing
        If fvTw.Visible = True Then
            view = fvTw
        Else
            view = fv
        End If

        Dim lblStockNo As Label = view.FindControl("lblStockNo")
        Dim lblDeal As Label = view.FindControl("lblDeal")
        Dim lblChange As Label = view.FindControl("lblChange")
        Dim lblHigh As Label = view.FindControl("lblHigh")
        Dim lblOpen As Label = view.FindControl("lblOpen")
        Dim lblTotalVolume As Label = view.FindControl("lblTotalVolume")
        Dim lblPercentage As Label = view.FindControl("lblPercentage")
        Dim lblLow As Label = view.FindControl("lblLow")
        Dim lblLast As Label = view.FindControl("lblLast")
        Dim lblTime As Label = view.FindControl("lblTime")
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
