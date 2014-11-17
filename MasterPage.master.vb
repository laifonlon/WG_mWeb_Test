
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim h1title As String = Me.Request.Url.ToString.ToLower()

        If h1title.ToLower.Contains("blog") Then
            'Me.h1.Text = "部落格"
        ElseIf h1title.ToLower.Contains("globals.aspx") Then
            Me.h1.Text = "國際股市"
        ElseIf h1title.ToLower.Contains("twindex.aspx") Then
            Me.h1.Text = "台股資訊"
        ElseIf h1title.ToLower.Contains("stock.aspx") Then
            Me.h1.Text = "個股資訊"
        Else
            Me.h1.Text = "玩股網"
        End If

        If Request("h") IsNot Nothing Then
            pnlHead.Visible = False
            pnlMenu.Visible = False
        End If
    End Sub

End Class

