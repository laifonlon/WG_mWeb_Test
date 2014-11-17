
Partial Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim ma As New MemberAuthority
        ma.SignOut()
        'SignOut()
        Response.Redirect("/", True)
    End Sub
End Class
