
Partial Class Margin
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim stockno As String = "0000"
        If Request("no") IsNot Nothing Then stockno = Request("no")
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If stockNo = "0000" Then
            fv0000.Visible = True
        Else
            fv.Visible = True
            sdsStock_marginadd.SelectParameters("StockNo").DefaultValue = stockNo
        End If
    End Sub

    'Protected Sub fv_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles fv.DataBound
    '    If fv.DataItemCount = 0 Then Exit Sub

    '    Dim marginadiff As Label = fv.FindControl("lblmarginadiff")
    '    Dim shortdiff As Label = fv.FindControl("lblshortdiff")
    '    Dim Price As Label = fv.FindControl("lblPrice")
    '    Dim Percent As Label = fv.FindControl("lblPercent")

    '    If Val(marginadiff.Text) > 0 Then
    '        marginadiff.ForeColor = Drawing.Color.FromArgb(141, 0, 0)
    '    ElseIf Val(marginadiff.Text) < 0 Then
    '        marginadiff.ForeColor = Drawing.Color.FromArgb(10, 127, 37)
    '    Else
    '    End If

    '    If Val(shortdiff.Text) > 0 Then
    '        shortdiff.ForeColor = Drawing.Color.FromArgb(141, 0, 0)
    '    ElseIf Val(shortdiff.Text) < 0 Then
    '        shortdiff.ForeColor = Drawing.Color.FromArgb(10, 127, 37)
    '    Else
    '    End If

    '    If Val(Percent.Text.Trim("%")) > 0 Then
    '        Price.ForeColor = Drawing.Color.FromArgb(141, 0, 0)
    '        Percent.ForeColor = Drawing.Color.FromArgb(141, 0, 0)
    '        Percent.Text = "+" + Percent.Text
    '    ElseIf Val(Percent.Text.Trim("%")) < 0 Then
    '        Price.ForeColor = Drawing.Color.FromArgb(10, 127, 37)
    '        Percent.ForeColor = Drawing.Color.FromArgb(10, 127, 37)
    '    Else
    '    End If

    'End Sub
End Class
