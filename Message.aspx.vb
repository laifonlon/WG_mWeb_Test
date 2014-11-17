
Partial Class Message
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Dim en As IEnumerator = sdsOfficalPost.Select(New DataSourceSelectArguments).GetEnumerator
        Dim splitLine As String = "<Line>"
        Dim splitKey As String = "<key>"
        Dim msg As String = ""
        While en.MoveNext
            msg &= en.Current("BlogID") & splitKey & _
                            Format(en.Current("PublishTime"), "yyyy-MM-dd HH:mm:ss") & splitKey & _
                            en.Current("ArticleTitle") & splitKey & _
                            "http://www.wantgoo.com/" & en.Current("MemberNo") & "/" & en.Current("ArticleID") & splitLine
        End While

        If msg.Length > 0 Then
            Response.Write(msg & "<body>")
        End If

    End Sub
End Class
