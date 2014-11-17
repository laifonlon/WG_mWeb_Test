
Partial Class Three
    Inherits System.Web.UI.UserControl
 
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim stockno As String = "0000"
        If Request("no") IsNot Nothing Then stockno = Request("no")
        stockno = Left(stockno, 10).Trim.Replace("$", "&")
 
        sdsmain.SelectParameters("StockNo").DefaultValue = stockNo
    End Sub

    Protected Sub gvStock_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvStock.DataBound
        Dim stockno As String = "0000"
        If Request("no") IsNot Nothing Then stockno = Request("no")
        'lblLike.Text = "<iframe src=""//www.facebook.com/plugins/like.php?href=http%3A%2F%2Fwww.wantgoo.com%2Fstock%three1.aspx%3FStockNo%3D" + stockno + "&amp;send=false&amp;layout=button_count&amp;width=150&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21&amp;appId=259338940175"" scrolling=""no"" frameborder=""0"" style=""border:none; overflow:hidden; width:150px; height:21px;"" allowTransparency=""true""></iframe>"

        For Each row As GridViewRow In gvStock.Rows
            Dim lblSum As Label = row.FindControl("lblSum")
            Dim lblSuma As Label = row.FindControl("lblSuma")
            Dim lblSumForeign As Label = row.FindControl("lblSumForeign")
            Dim lblSumForeigna As Label = row.FindControl("lblSumForeigna")
            Dim lblSumING As Label = row.FindControl("lblSumING")
            Dim lblSumINGa As Label = row.FindControl("lblSumINGa")
            Dim lblSumDealer As Label = row.FindControl("lblSumDealer")
            Dim lblSumDealera As Label = row.FindControl("lblSumDealera")

            If IsNumeric(stockno) And stockno <> "0000" Then
                lblSum.Visible = True
                lblSumForeign.Visible = True
                lblSumING.Visible = True
                lblSumDealer.Visible = True
                If Val(lblSum.Text) > 0 Then
                    lblSum.ForeColor = Drawing.Color.Red
                ElseIf Val(lblSum.Text) < 0 Then
                    lblSum.ForeColor = Drawing.Color.Green
                Else
                    lblSum.ForeColor = Drawing.Color.Black
                End If
            ElseIf stockno = "0000" Then
                lblSuma.Visible = True
                lblSumForeigna.Visible = True
                lblSumINGa.Visible = True
                lblSumDealera.Visible = True
                If Val(lblSuma.Text) > 0 Then
                    lblSuma.ForeColor = Drawing.Color.Red
                ElseIf Val(lblSuma.Text) < 0 Then
                    lblSuma.ForeColor = Drawing.Color.Green
                Else
                    lblSuma.ForeColor = Drawing.Color.Black
                End If
            End If
        Next

        If gvStock.Rows.Count = 0 Then Exit Sub
        gvStock.Rows(0).CssClass = "highlight gvtable_row"
    End Sub
End Class
