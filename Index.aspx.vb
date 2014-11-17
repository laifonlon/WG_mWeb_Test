
Partial Class index
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Me.LoadData()
        End If
    End Sub

    Private Sub LoadData()
        If Request("no") IsNot Nothing Then

            Dim cStockNo As String = Request("no").Replace("$", "&").Trim

            '轉台股網址
            If IsNumeric(cStockNo) Then
                Response.Redirect("stock.aspx?no=" + cStockNo)
            End If

            Me.Title = Me.GetTitle(cStockNo) & " 即時國際走勢 - 玩股網手機版"

            If cStockNo.Length < 10 Then

                'Dim imgUrl As String = GetGRealTimeChart(cStockNo, maxTime, 300, 125)

                Select Case Request("no").Replace("$", "&").Trim
                    Case "RUT", "RXS", "XAX", "XMI", "IBC", "IPS", "MER", "MEX"
                        'lblRealChat.Text = ""
                        lblTechChat.Text = ""
                    Case "B1RR&", "B1W&", "C1CC&", "C1CT&", "C1KC&", "JRU&", _
                        "N1CL&", "JCO&", "JKE&", "N1NG&"
                        'lblRealChat.Text = "<img src=""" + imgUrl + """>"
                        lblTechChat.Text = ""
                    Case Else
                        'lblRealChat.Text = "<img src=""" + imgUrl + """>"

                        If cStockNo.IndexOf("&") >= 0 Then
                            lblTechChat.Text = "<a href=""kchart.aspx?no=" + cStockNo.Replace("&", "$") + """><img width=""300px"" src=""" + "http://www.wantgoo.com/stock/Image/" + cStockNo + "sdk.png" + """></a>"
                        Else

                            lblTechChat.Text = "<a href=""kchart.aspx?no=" + cStockNo.Replace("&", "$") + """><img width=""300px"" src=""" + "http://img.wantgoo.com/stock/Image/" + cStockNo + "sdk.png" + """></a>"
                        End If

                End Select
            End If
        End If
    End Sub

    Public Function GetTitle(stockNo As String) As String
        If stockNo Is Nothing Then Return "WantGoo玩股網 最棒的理財網站"
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If Application(stockNo + "_Name") Is Nothing Then StockLoader.Instance.LoadStockName()

        Return Application(stockNo + "_Name") + " (" + stockNo.ToUpper + ")"
    End Function
End Class


