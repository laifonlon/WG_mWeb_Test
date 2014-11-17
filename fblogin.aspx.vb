Imports System.Security.Cryptography
Imports System.Net.Mail

Partial Class Facebook_fblogin
    Inherits System.Web.UI.Page

    Private Enum CreateFacebookMemberState
        Fail = 99 '失敗
        MailFail = 98
        OK = 1 '建立成功
        FacebookIdExist = 2 '帳號已存在
        PasswrodInvalid = 3 '帳號已存在, 但密碼錯誤, 請輸入玩股網密碼
        FacebookIdExistAndEmailiInvalid = 4 '此FacebookI己註冊過, 但輸入Email註冊Email不同
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim urlGoBack As String = "/"
        If Not String.IsNullOrEmpty(Request("GoBackUrl")) AndAlso Not Request("GoBackUrl").Contains("Login.aspx") Then
            urlGoBack = Server.UrlDecode(Request("GoBackUrl"))
        ElseIf Request.Cookies("GoBackUrl") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.Cookies("GoBackUrl").Value) AndAlso Not Request.Cookies("GoBackUrl").Value.Contains("Login.aspx") Then
            '進入登入頁之前的網址，取得暫存在cookie
            urlGoBack = Server.UrlDecode(Request.Cookies("GoBackUrl").Value)
        End If

        Dim state As CreateFacebookMemberState = CreateFacebookMemberState.FacebookIdExistAndEmailiInvalid

        Dim at As String = Request("a")
        Dim facebookId As String = Request("u")
        Dim email As String = Left(Request("m"), 50)
        Dim nickname As String = Left(Request("n"), 30)
        Dim lastName As String = Left(Request("l"), 30)
        Dim firstName As String = Left(Request("f"), 30)

        If Val(facebookId) = 0 Then
            '進入登入頁之前的網址，清除暫存在cookie
            Debug.WriteCookie("GoBackUrl", "", 1)
            '導回進入登入頁之前的網址
            Response.Redirect(urlGoBack)
            Exit Sub
        End If

        'If email.Contains("@") = False Then
        '    Response.Redirect("/")
        '    Exit Sub
        'End If

        Dim sex As Integer = 1
        If Request("s").ToString <> "male" Then
            sex = 2
        End If

        Dim method As String = "https://api.facebook.com/method/users.isAppUser?uid=" + facebookId + "&access_token=" + at + "&format=json"
        Dim grabber As New WebPageGraber
        Dim result As String = grabber.QueryWebPage(method, System.Text.Encoding.UTF8)
        If result.Contains("true") = True Then

            Dim username As String = GetUserName(facebookId)
            If username <> "" Then
                Login(username)
                If Request("r") IsNot Nothing Then
                    Select Case Request("r").ToString
                        Case "22"
                            Response.Redirect("/mall/car.aspx")
                            Exit Sub
                    End Select
                End If
                
                '進入登入頁之前的網址，清除暫存在cookie
                Debug.WriteCookie("GoBackUrl", "", 1)
                '導回進入登入頁之前的網址
                Response.Redirect(urlGoBack)

                Exit Sub
            End If

            method = "https://api.facebook.com/method/users.getInfo?uids=" + facebookId + "&fields=email&access_token=" + at + "&format=json"
            result = grabber.QueryWebPage(method, System.Text.Encoding.UTF8)
            Dim ss() As String = result.Split("""")
            Dim checkmail As String = ss(5).Replace("\u0040", "@")
            'If checkmail <> email Then
            '    Response.Redirect("/login.aspx")
            'End If

            Dim pd As New RandomPassword
            Dim password As String = pd.GetRandomPassword(7)
            Dim da As New WantGooTableAdapters.QueriesTableAdapter
            da.CreateMemberForFacebook(email, lastName, firstName, Val(sex), password, nickname, email, facebookId, 0, state)

            'If email = "darkshut@pchome.com.tw" Then
            '    Response.Write(facebookId & "-" & state.ToString)
            '    Exit Sub
            'End If

            If state = CreateFacebookMemberState.OK Then
                Dim status As Integer
                If Membership.GetUser(email) Is Nothing Then
                    Membership.CreateUser(email, password, email, "你最喜歡的網站", "玩股網", True, status)
                End If
                Login(email)
                MemberDataAccessor.Instance.SendWelcomeMail()
                StreamPublish(nickname, facebookId, at)
                If Request("r") IsNot Nothing Then
                    Select Case Request("r").ToString
                        Case "22"
                            Response.Redirect("/mall/car.aspx")
                            Exit Sub
                    End Select
                End If
                Response.Redirect("/RegistrationOK.aspx?f=1")
                Exit Sub
            End If
            If state = CreateFacebookMemberState.FacebookIdExist Then
                Login(email)
                If Request("r") IsNot Nothing Then
                    If Request("r") IsNot Nothing Then
                        Select Case Request("r").ToString
                            Case "22"
                                Response.Redirect("/mall/car.aspx")
                                Exit Sub
                        End Select
                    End If
                    
                    '進入登入頁之前的網址，清除暫存在cookie
                    Debug.WriteCookie("GoBackUrl", "", 1)
                    '導回進入登入頁之前的網址
                    Response.Redirect(urlGoBack)

                    Exit Sub
                Else
                    
                    '進入登入頁之前的網址，清除暫存在cookie
                    Debug.WriteCookie("GoBackUrl", "", 1)
                    '導回進入登入頁之前的網址
                    Response.Redirect(urlGoBack)

                    Exit Sub
                End If
                Exit Sub
            End If
        End If

        Response.Redirect("/facebook/newRegister.aspx")
    End Sub

    Private Function GetUserName(ByVal facebookId As String) As String
        Dim result As String = ""

        If Val(facebookId) = 0 Then
            Return ""
        End If

        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()
        Try
            Dim cmd As String = "select username from member where facebookid='" + facebookId + "'"
            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)

            Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
            Dim dataSet As New Data.DataSet
            adapter.Fill(dataSet)
            For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
                result = row("username").ToString.Trim
            Next
        Catch ex As Exception

        End Try
        connection.Close()
        connection.Dispose()

        Return result
    End Function

    Private Sub Login(ByVal email As String)
        Dim ma As New MemberAuthority
        ma.Login(email, True)
        ma.WriteCookie(email, 7)
    End Sub

    Private Shared Sub StreamPublish(ByVal nickname As String, ByVal fid As String, ByVal at As String)
        Dim message As String = "我剛在玩股網註冊，一起來玩股網學投資賺大錢吧！"
        Dim imgUrl As String = "http://2.bp.blogspot.com/-78xwQapTwS8/Thbeq61C3OI/AAAAAAAACNQ/TyQVdKhp14s/s1600/WG-logo-32b.png"
        Dim title As String = "WantGoo 玩股網"
        Dim caption As String = "最即時國際股市和台股報價(100%同步)、國內外財經新聞解析、一鍵選出強勢股功能、多位投資高手駐站教你操作、每支股票都有超過五年以上的動態K線圖。"
        Dim description As String = "用Facebook帳號註冊玩股網會員送價值100元的選股系統無限用三天、停損停利點自動通知、詳盡自選股資訊、...等好康。"
        description &= "和" & nickname & "一起來玩股網學投資賺大錢吧!"
        Dim attachment As String = "{ 'href': '/facebook/newRegister.aspx" + _
        "', 'name': '" + title + "', 'caption': '" + caption + _
        "', 'description': '" + description + "', 'media': [{ 'type': 'image', 'src': '" + imgUrl + _
        "', 'href': '/facebook/newRegister.aspx" + "' }]}"
        Dim method As String = "https://api.facebook.com/method/stream.publish?message=" + message + "&attachment=" + attachment + "&uid=" + fid + "&access_token=" + at
        Dim grabber As New WebPageGraber
        Dim result As String = grabber.QueryWebPage(method, System.Text.Encoding.UTF8)
    End Sub

    Private Class RandomPassword
        Private rngp As New RNGCryptoServiceProvider()
        Private rb As Byte() = New Byte(3) {}

        ''' <summary>
        ''' 產生一個非負數的亂數
        ''' </summary>
        Private Function [Next]() As Integer
            rngp.GetBytes(rb)
            Dim value As Integer = BitConverter.ToInt32(rb, 0)
            If value < 0 Then
                value = -value
            End If
            Return value
        End Function
        ''' <summary>
        ''' 產生一個非負數且最大值 max 以下的亂數
        ''' </summary>
        ''' <param name="max">最大值</param>
        Private Function [Next](ByVal max As Integer) As Integer
            rngp.GetBytes(rb)
            Dim value As Integer = BitConverter.ToInt32(rb, 0)
            value = value Mod (max + 1)
            If value < 0 Then
                value = -value
            End If
            Return value
        End Function
        ''' <summary>
        ''' 產生一個非負數且最小值在 min 以上最大值在 max 以下的亂數
        ''' </summary>
        ''' <param name="min">最小值</param>
        ''' <param name="max">最大值</param>
        Private Function [Next](ByVal min As Integer, ByVal max As Integer) As Integer
            Dim value As Integer = [Next](max - min) + min
            Return value
        End Function

        Public Function GetRandomPassword(ByVal length As Integer) As String
            Dim sb As New StringBuilder()
            Dim chars As Char() = "0123456789abcdefghijkmnpqrstuvwxyz".ToCharArray()

            For i As Integer = 0 To length - 1
                sb.Append(chars(Me.[Next](chars.Length - 1)))
            Next
            Dim Password As String = sb.ToString()
            Return Password
        End Function
    End Class
End Class
