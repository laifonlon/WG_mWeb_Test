
Partial Class AutoRefresh
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MemberDataAccessor.Instance.IsLogin Then
        End If
    End Sub
End Class
