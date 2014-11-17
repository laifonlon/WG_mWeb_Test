Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class MemberAuthority

    ''' <summary>
    ''' 自動登入狀態
    ''' </summary>
    Public Sub Login(ByVal userName As String, Optional ByVal isAutoLogin As Boolean = True)
        'Dim dba As New DB.Accessor
        'dba.ExecStoredProcedure(DB.Source.WantGoo, "")

        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.UpdateAutoLoginState(userName, isAutoLogin, GetClientIP)

        With HttpContext.Current
            .Session("IsLogin") = True
            .Session("UserName") = userName
            .Session("MemberNo") = MemberDataAccessor.Instance.GetMemberNo(userName)
        End With

        MemberDataAccessor.Instance.WriteLoginLog("Login")
    End Sub

    ''' <summary>
    ''' 取得訪問者IP
    ''' </summary>
    Public Function GetClientIP() As String
        Dim strIPAddr As String = ""
        With HttpContext.Current.Request
            If .ServerVariables("HTTP_X_FORWARDED_FOR") = "" Or InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), "unknown") > 0 Then
                strIPAddr = .ServerVariables("REMOTE_ADDR")
            ElseIf InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ",") > 0 Then
                strIPAddr = Mid(.ServerVariables("HTTP_X_FORWARDED_FOR"), 1, InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ",") - 1)
            ElseIf InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ";") > 0 Then
                strIPAddr = Mid(.ServerVariables("HTTP_X_FORWARDED_FOR"), 1, InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ";") - 1)
            Else
                strIPAddr = .ServerVariables("HTTP_X_FORWARDED_FOR")
            End If
        End With
        Return Mid(strIPAddr, 1, 30).Trim
    End Function

    ''' <summary>
    ''' 登出
    ''' </summary>
    Public Sub SignOut()

        MemberDataAccessor.Instance.WriteLoginLog("SignOut")

        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        With MemberDataAccessor.Instance
            da.UpdateAutoLoginState(.GetUserName, False, .GetClientIP)
        End With

        '刪除Cookie
        ClearCookie()

        With HttpContext.Current
            FormsAuthentication.SignOut()
            .Session.Clear()
            .Session("IsLogin") = False
        End With

    End Sub

#Region "Cooke"
    Public Function WriteCookie(ByVal userName As String, Optional ByVal cookieExpires As Integer = 1) As Boolean
        With MemberDataAccessor.Instance

            Dim myCookie As HttpCookie

            myCookie = New HttpCookie("UserName", .GetUserName())
            myCookie.Domain = ".wantgoo.com"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

            Dim nickname As String = ""
            Dim len As Integer
            For Each ch As Char In .GetMemberNickName.ToCharArray
                len = len + System.Text.Encoding.Default.GetBytes(ch).Length
                nickname += ch

                If len > 12 Then
                    nickname += "..."
                    Exit For
                End If
            Next
            myCookie = New HttpCookie("NickName", HttpContext.Current.Server.UrlEncode(nickname))
            myCookie.Domain = ".wantgoo.com"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

            myCookie = New HttpCookie("UnReadMailCount", .NonReadMailCount(.GetMemberNo))
            myCookie.Domain = ".wantgoo.com"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

            myCookie = New HttpCookie("Member_No", .GetMemberNo(userName))
            myCookie.Domain = ".wantgoo.com"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

            myCookie = New HttpCookie("AdState", "Show")
            myCookie.Domain = ".wantgoo.com"
            If Not Member.Instance.IsShowAd Then myCookie.Value = "Non"
            'If .IsRichMember() AndAlso Not .IsAdiminstrator Then myCookie.Value = "Non"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

            myCookie = New HttpCookie("MemberLevel", "Normal")
            myCookie.Domain = ".wantgoo.com"
            If .IsRichMember() Then myCookie.Value = "Rich"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

            myCookie = New HttpCookie("IsLogin", "True")
            myCookie.Domain = ".wantgoo.com"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)


            myCookie = New HttpCookie("urls", "m.wantgoo.com")
            myCookie.Domain = ".wantgoo.com"
            myCookie.Expires = Now.AddDays(cookieExpires)
            HttpContext.Current.Response.Cookies.Add(myCookie)

        End With
        Return True
    End Function

    Public Sub ClearCookie()
        With HttpContext.Current
            If (Not .Request.Cookies("UserName") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("UserName")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("NickName") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("NickName")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("UnReadMailCount") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("UnReadMailCount")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("Member_No") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Member_No")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("AdState") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("AdState")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("MemberLevel") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("MemberLevel")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("IsLogin") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("IsLogin")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

            If (Not .Request.Cookies("urls") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("urls")
                myCookie.Domain = ".wantgoo.com"
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                .Response.Cookies.Add(myCookie)
            End If

        End With


    End Sub

#End Region


End Class
