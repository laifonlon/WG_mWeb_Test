
Partial Class AppStock
    Inherits System.Web.UI.Page
 
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim pageTitle As String = ""
        Select Case Request("g")
            Case "a" '亞洲
                pageTitle = "亞洲股市"
                sv3.ShowUpdate = True
                p2.Visible = True
            Case "f" '期貨
                pageTitle = "期貨"
                sv4.ShowUpdate = True
                p3.Visible = True
            Case "o" '能源
                pageTitle = "能源"
                sv5.ShowUpdate = True
                p4.Visible = True
            Case "e" '匯率
                pageTitle = "匯率"
                er1.ShowUpdate = True
                p5.Visible = True
            Case "u" '歐洲
                pageTitle = "歐洲股市"
                sv2.ShowUpdate = True
                p6.Visible = True
            Case "p" '農產品
                pageTitle = "農產品"
                sv6.ShowUpdate = True
                p7.Visible = True
            Case Else '美洲
                pageTitle = "美洲股市"
                sv1.ShowUpdate = True
                p1.Visible = True
        End Select

        Page.Title = pageTitle & "指數行情 - 玩股網 手機版 Wantgoo.com"
    End Sub

End Class

