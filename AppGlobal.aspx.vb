
Partial Class AppGlobal
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'Select Case Request("g")
        '    Case "a" '亞洲
        '        sv3.ShowUpdate = True
        '        p2.Visible = True
        '    Case "f" '期貨
        '        sv4.ShowUpdate = True
        '        p3.Visible = True
        '    Case "o" '其他
        '        sv5.ShowUpdate = True
        '        p4.Visible = True
        '    Case "e" '匯率
        '        er1.ShowUpdate = True
        '        p5.Visible = True
        '    Case Else '美歐
        '        sv1.ShowUpdate = True
        '        p1.Visible = True
        'End Select

        p2.Visible = True
        p3.Visible = True
        p4.Visible = True
        p5.Visible = True
        p1.Visible = True
    End Sub

End Class
