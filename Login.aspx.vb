
Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub Login1_LoggingIn(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) Handles Login1.LoggingIn

    End Sub

    Protected Sub Login1_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoggedIn

        UpdateSessionId()

        Dim ma As New MemberAuthority
        ma.Login(Login1.UserName, Login1.RememberMeSet)
        ma.WriteCookie(Login1.UserName, 7)
        
        Dim urlGoBack As String = "/"
        If Not String.IsNullOrEmpty(Request("GoBackUrl")) AndAlso Not Request("GoBackUrl").Contains("Login.aspx") Then
            urlGoBack = Server.UrlDecode(Request("GoBackUrl"))
        ElseIf Request.Cookies("GoBackUrl") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.Cookies("GoBackUrl").Value) AndAlso Not Request.Cookies("GoBackUrl").Value.Contains("Login.aspx") Then
            '進入登入頁之前的網址，取得暫存在cookie
            urlGoBack = Server.UrlDecode(Request.Cookies("GoBackUrl").Value)
        End If
        '進入登入頁之前的網址，清除暫存在cookie
        Debug.WriteCookie("GoBackUrl", "", 1)
        '導回進入登入頁之前的網址
        Response.Redirect(urlGoBack)

    End Sub

    Private Sub UpdateSessionId()
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Dim cmd As String = "Update Member Set SessionId='" + Session.SessionID + "' WHERE MemberNo=" & MemberDataAccessor.Instance.GetMemberNo
        Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
        command.ExecuteNonQuery()
        command.Dispose()

        connection.Close()
        connection.Dispose()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Form.DefaultFocus = Me.Login1.FindControl("UserName").UniqueID
        Me.Form.DefaultButton = Me.Login1.FindControl("LoginButton").UniqueID

        If IsPostBack Then Exit Sub

        '紀錄登入前的網址
        Dim refUrl As Uri = Request.UrlReferrer

        If refUrl IsNot Nothing AndAlso Not String.IsNullOrEmpty(refUrl.ToString) AndAlso Not refUrl.ToString.Contains("Login.aspx") Then
            '進入登入頁之前的網址，暫存在cookie
            Debug.WriteCookie("GoBackUrl", Server.UrlEncode(refUrl.ToString), 30)
        Else
            Debug.WriteCookie("GoBackUrl", "/", 30)
        End If

        If MemberDataAccessor.Instance.IsLogin Then

            Dim urlGoBack As String = "/"
            If Not String.IsNullOrEmpty(Request("GoBackUrl")) AndAlso Not Request("GoBackUrl").Contains("Login.aspx") Then
                urlGoBack = Server.UrlDecode(Request("GoBackUrl"))
            ElseIf Request.Cookies("GoBackUrl") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.Cookies("GoBackUrl").Value) AndAlso Not Request.Cookies("GoBackUrl").Value.Contains("Login.aspx") Then
                '進入登入頁之前的網址，取得暫存在cookie
                urlGoBack = Server.UrlDecode(Request.Cookies("GoBackUrl").Value)
            End If
            '進入登入頁之前的網址，清除暫存在cookie
            Debug.WriteCookie("GoBackUrl", "", 1)
            '導回進入登入頁之前的網址
            Response.Redirect(urlGoBack)
            Exit Sub

        End If

        If Request("id") IsNot Nothing AndAlso Request("id").Trim <> "" Then
            Login1.UserName = Request("id").Trim
        End If

    End Sub

    Protected Sub Login1_LoginError(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoginError
        WriteLoginLog(Login1.UserName + vbTab + Login1.Password)
    End Sub

    Private mIsLogWriting As Boolean
    Private mLogTemp As String = ""
    ''' <summary>
    ''' 寫登入Log
    ''' </summary>
    Public Sub WriteLoginLog(ByVal from As String)
        mLogTemp = mLogTemp + Format(Now, "HH:mm:ss") + vbTab + _
         MemberDataAccessor.Instance.GetClientIP() + vbTab + from + vbCrLf

        If mIsLogWriting Then Exit Sub
        mIsLogWriting = True

        Dim logPath As String = Server.MapPath("~\tmp\Login\")
        If Not My.Computer.FileSystem.DirectoryExists(logPath) Then
            My.Computer.FileSystem.CreateDirectory(logPath)
        End If

        Dim filename As String = logPath + Format(Now, "yyyyMMdd") & "error.log"

        Try
            My.Computer.FileSystem.WriteAllText(filename, mLogTemp, True)
        Catch ex As Exception
        End Try

        mLogTemp = ""
        mIsLogWriting = False
    End Sub

    Protected Sub Login1_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate

        Dim LoginMessage As Label = Me.Login1.FindControl("ltMessage")
        LoginMessage.Text = ""
        LoginMessage.Visible = False

        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Dim cmd As String = "SELECT [Email],[Password],[NickName],[MemberNo]  FROM [Member] where UserName = '" + Login1.UserName + "'"
        Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
        Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader
        While reader.Read
            If reader.Item("Password") = Login1.Password Then
                e.Authenticated = True
            Else
                e.Authenticated = False
                LoginMessage.Text = "帳號密碼錯誤，請確認。"
                LoginMessage.Visible = True
            End If
        End While

        reader.Close()

        command.Dispose()
        connection.Close()
        connection.Dispose()

    End Sub

    Sub wirteCookie(ByVal name As String, ByVal values As String, expireDayCount As Integer)

        Dim myCookie As HttpCookie
        myCookie = New HttpCookie(name, values)
        myCookie.Expires = Now.AddDays(expireDayCount)
        HttpContext.Current.Response.Cookies.Add(myCookie)

    End Sub

    Protected Sub btnRegistration_Click(sender As Object, e As EventArgs)
        Response.Redirect("http://www.wantgoo.com/Registration.aspx")
    End Sub
End Class
