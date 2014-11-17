
Partial Class m
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim h1title As String = Me.Request.Url.ToString.ToLower()

        If h1title.ToLower.Contains("blog") Or h1title.ToLower.Contains("article") Then
            Me.h1.Text = "玩股網 必讀理財教學好文"
            tab_4.CssClass = "active"
        ElseIf h1title.ToLower.Contains("globals.aspx") Or h1title.ToLower.Contains("index.aspx?") Then
            Me.h1.Text = "玩股網 即時國際股市指數"
            tab_3.CssClass = "active"
        ElseIf h1title.ToLower.Contains("twindex.aspx") Then
            Me.h1.Text = "玩股網 台股資訊"
            tab_2.CssClass = "active"
        ElseIf h1title.ToLower.Contains("stock.aspx") Then
            Me.h1.Text = "玩股網 個股資訊"
            tab_2.CssClass = "active"
        ElseIf h1title.ToLower.Contains("search.aspx") Then
            Me.h1.Text = "玩股網 股票搜尋"
            tab_5.CssClass = "active"
        Else
            Me.h1.Text = "玩股網"
            tab_1.CssClass = "active"
        End If
    End Sub

End Class

