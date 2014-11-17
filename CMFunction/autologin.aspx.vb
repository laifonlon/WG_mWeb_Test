
Partial Class CMFunction_autologin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AutoLogin()

        Dim url As String = Request("url")
        If url Is Nothing Then url = Request.UrlReferrer.ToString

        If url IsNot Nothing Then
            If url.ToLower.EndsWith("default.aspx") Then
                Response.Redirect("/")
            ElseIf url.ToLower.Contains("personalpage/myblogpost.aspx") Then
                Response.Redirect("/" + GetBlogName(url) + "/" + GetArticleID(url))
            ElseIf url.ToLower.EndsWith("globalfinance.aspx") Then
                'Response.Redirect("/global.aspx")
            ElseIf url.ToLower.EndsWith("personalpage/mydata.aspx") Then
                Response.Redirect("/" + GetBlogName(url) + "/profile")
            ElseIf url.ToLower.Contains("mag.aspx") Then
                Response.Redirect("/mag/" + GetID(url))
            Else
                Response.Redirect(url.Replace("$", "&"))
            End If
        End If
    End Sub

    ''' <summary>
    ''' 自動登入
    ''' </summary>
    Public Sub AutoLogin()
        If Session("IsLogin") Then Exit Sub

        Session("IsLogin") = False

        If Request.Cookies("UserName") Is Nothing Then Exit Sub

        MemberDataAccessor.Instance.Action = "AutoLogin"

        Dim userName As String = Request.Cookies("UserName").Value

        Dim memberNO As String = MemberDataAccessor.Instance.GetMemberNo(userName)
        Select Case memberNO
            Case "33536" '不自動登入的會員
                Dim ma As New MemberAuthority
                ma.ClearCookie()
                Exit Sub
        End Select

        Dim ip As String = " "
        Dim isAutoLogin As Boolean = False
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetAutoLoginState(userName, isAutoLogin, ip)
        isAutoLogin = True

        If isAutoLogin Then 'AndAlso _
            'ip.Trim = MemberDataAccessor.Instance.GetClientIP().Trim Then
            da.UpdateAutoLoginState(userName, True, MemberDataAccessor.Instance.GetClientIP)
            FormsAuthentication.SetAuthCookie(userName, True)
            Session("IsLogin") = True
            Session("UserName") = userName
            Session("MemberNo") = MemberDataAccessor.Instance.GetMemberNo(userName)

            Dim ma As New MemberAuthority
            ma.WriteCookie(userName, 7)

            MemberDataAccessor.Instance.WriteLoginLog("AutoLogin")
        Else
            Dim ma As New MemberAuthority
            ma.ClearCookie()
        End If

    End Sub

    Private Function GetBlogName(ByVal url As String) As String
        Dim symble() As Char = {"$", "&", "=", "?"}
        Dim data() As String = url.Split(symble)
        Dim blogName As String = ""
        Dim memberNo As String = ""

        For Each item As String In data
            If blogName = "Next" Then
                blogName = item
                Exit For
            ElseIf memberNo = "Next" Then
                memberNo = item
                Exit For
            End If
            If item.ToLower = "name" Then
                blogName = "Next"
            ElseIf item.ToLower = "memberno" Then
                memberNo = "Next"
            End If
        Next

        If memberNo <> "" Then
            Return MemberDataAccessor.Instance.GetBlogName(memberNo)
        Else
            Return blogName
        End If

    End Function

    Private Function GetArticleID(ByVal url As String) As String
        Dim symble() As Char = {"$", "&", "=", "?"}
        Dim data() As String = url.Split(symble)
        Dim articleID As String = ""

        For Each item As String In data
            If articleID = "Next" Then
                articleID = item
                Exit For
            End If
            If item.ToLower = "articleid" Then
                articleID = "Next"
            End If
        Next

        Return articleID
    End Function

    Private Function GetID(ByVal url As String) As String
        Dim symble() As Char = {"$", "&", "=", "?"}
        Dim data() As String = url.Split(symble)
        Dim id As String = ""

        For Each item As String In data
            If id = "Next" Then
                id = item
                Exit For
            End If
            If item.ToLower = "id" Then
                id = "Next"
            End If
        Next

        Return id
    End Function

End Class
