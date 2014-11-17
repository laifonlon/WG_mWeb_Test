
Partial Class SubNews
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim stockno As String = "0000"
        If Request("no") IsNot Nothing Then stockno = Request("no")
 
        If IsNumeric(stockno) And stockno <> "0000" Then
            sdsNews.SelectParameters("StockNo").DefaultValue = stockno
        ElseIf stockno = "0000" Or stockno = "WTX$" Or stockno = "WMT$" Or stockno = "WTE$" Or stockno = "WTF$" Then
            sdsNews.SelectParameters("StockNo").DefaultValue = "0000"
        Else
            sdsNews.SelectParameters("StockNo").DefaultValue = "DJI"
        End If
    End Sub
 
    Protected Sub lv_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv.DataBound
        If Me.lv.Items.Count = 0 Then
            Me.lblnews_note.Visible = "true"
            Me.lblnews_note.Text = "尚無新聞"
            Exit Sub
        End If

        For Each row As ListViewDataItem In lv.Items

            'Dim lblEffect As Label = row.FindControl("lblEffect")
            'If Val(lblEffect.Text) > 0 Then
            '    lblEffect.Text = "多"
            '    lblEffect.ForeColor = Drawing.Color.Red
            'ElseIf Val(lblEffect.Text) < 0 Then
            '    lblEffect.Text = "空"
            '    lblEffect.ForeColor = Drawing.Color.Green
            'Else
            '    lblEffect.Text = "-"
            'End If

            Dim lblTime As Literal = row.FindControl("lbltime")
            Dim PubDate As DateTime = lblTime.Text
            Dim ts As TimeSpan = DateTime.Now.Subtract(PubDate)

            If ts.Days > 0 And ts.Days <= 30 Then
                lblTime.Text = ts.Days.ToString + "天前"
            ElseIf ts.Days = 0 And ts.Hours > 0 Then
                lblTime.Text = ts.Hours.ToString + "時前"
            ElseIf ts.Hours = 0 And ts.Minutes > 0 Then
                lblTime.Text = ts.Minutes.ToString + "分前"
            ElseIf ts.Minutes = 0 And ts.Milliseconds > 0 Then
                lblTime.Text = ts.Seconds.ToString + "秒前"
            Else
                lblTime.Text = Format(PubDate, "yyyy/MM/dd").ToString()
            End If

            Dim lttitle As Literal = row.FindControl("lttitle")
            'Dim lttext As Literal = row.FindControl("lttext")
            Dim lbltitle As Label = row.FindControl("lbltitle")
            Dim lbltext As Literal = row.FindControl("lbltext")

            lbltitle.Text = Me.GetLimitText(lttitle.Text, 30)
            'lbltext.Text = Me.GetLimitText(lttext.Text, 70)
        Next
    End Sub

    Private Function GetLimitText(oldText As String, maxLength As Integer) As String
        Dim strTitle As String = Regex.Replace(System.Web.HttpUtility.HtmlDecode(oldText), "<(.|\n)*?>", "").Replace(" ", "").Replace("&nbsp;", "")
 
        Dim textResult As String = ""
        Dim counter As Integer = 0
        Dim chr As Char
        Dim RealLen As Integer = System.Text.Encoding.GetEncoding("Big5").GetBytes(strTitle).Length
        For Each chr In strTitle
            counter += System.Text.Encoding.GetEncoding("Big5").GetBytes(chr.ToString()).Length
            If counter <= maxLength Then
                textResult = textResult + chr
            Else
                Exit For
            End If
        Next
        If RealLen > maxLength Then
            textResult += "..."
        End If
        Return textResult
    End Function
End Class
