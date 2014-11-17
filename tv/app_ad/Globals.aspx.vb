
Partial Class globals2
    Inherits System.Web.UI.Page

    Protected Overrides Sub OnPreInit(e As System.EventArgs)
        ' added for compatibility issues with chrome 
        If (Request.UserAgent IsNot Nothing AndAlso (Request.UserAgent.IndexOf("AppleWebKit") > 0)) Then
            ClientTarget = "uplevel"
        End If

        MyBase.OnPreInit(e)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        sv3.ShowUpdate = True
        sv1.ShowUpdate = True
        'Select Case Request("g")
        '    Case "a" '亞洲
        '        sv3.ShowUpdate = True
        '        p2.Visible = True
        '        tab_2.CssClass = "active"
        '    Case "f" '期貨
        '        sv4.ShowUpdate = True
        '        p3.Visible = True
        '        tab_3.CssClass = "active"
        '    Case "o" '其他
        '        sv5.ShowUpdate = True
        '        p4.Visible = True
        '        tab_4.CssClass = "active"
        '    Case "e" '匯率
        '        er1.ShowUpdate = True
        '        p5.Visible = True
        '        tab_5.CssClass = "active"
        '    Case Else '美歐
        '        sv1.ShowUpdate = True
        '        p1.Visible = True
        '        tab_1.CssClass = "active"
        'End Select
    End Sub

    'Protected Sub tab_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_1.Click
    '    Response.Redirect("globals.aspx")
    'End Sub

    'Protected Sub tab_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_2.Click
    '    Response.Redirect("globals.aspx?g=a")
    'End Sub

    'Protected Sub tab_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_3.Click
    '    Response.Redirect("globals.aspx?g=f")
    'End Sub

    'Protected Sub tab_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_4.Click
    '    Response.Redirect("globals.aspx?g=o")
    'End Sub

    'Protected Sub tab_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_5.Click
    '    Response.Redirect("globals.aspx?g=e")
    'End Sub
End Class
