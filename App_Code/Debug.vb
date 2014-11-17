Imports Microsoft.VisualBasic
Imports System.Data

Public Class Debug

    Public Shared Sub WriteCookie(Name As String, Value As String, Optional LifeTimeMinute As Integer = 3, Optional Domain As String = ".wantgoo.com")
        Dim myCookie As HttpCookie
        myCookie = New HttpCookie(Name, Value)
        myCookie.Domain = Domain
        myCookie.Expires = Now.AddMinutes(LifeTimeMinute)
        HttpContext.Current.Response.Cookies.Add(myCookie)
    End Sub

    Public Shared Sub WriteDB(Name As String, Memo As String, Optional exMessage As String = "", Optional exStackTrace As String = "")
        Dim IP As String = Debug.GetUserIP()
        Try
            Dim Connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("LogConnection").ConnectionString)
            Connection.Open()
            Dim cmdText As String = String.Format("exec [dbo].[WebSiteLog_Insert] N'{0}',N'{1}',N'{2}',N'{3}',N'{4}';", Name, exMessage, exStackTrace, Memo, IP)

            Dim cmdMail As New SqlClient.SqlCommand(cmdText, Connection)
            cmdMail.ExecuteNonQuery()
            Connection.Close()
            Connection.Dispose()
            cmdMail.Dispose()
        Catch ex As Exception
            Debug.WriteCookie("Exception StackTrace Debug.WriteDB", "Time：" + Now.ToString() + "…" + ex.StackTrace)
        End Try
    End Sub

    Public Shared Function GetUserIP() As String

        Dim ipList As String = String.Empty
        Dim context As HttpContext = HttpContext.Current

        If context.Request.ServerVariables("HTTP_X_FORWARDED_FOR") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            Return ipList
        End If

        If context.Request.Headers("True-Client-IP") IsNot Nothing Then
            ipList = context.Request.Headers("True-Client-IP")
            Return ipList
        End If

        If context.Request.Headers("X-ClientSide") IsNot Nothing Then
            ipList = context.Request.Headers("X-ClientSide")
            Return ipList
        End If

        If context.Request.ServerVariables("HTTP_CLIENT_IP") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_CLIENT_IP")
            Return ipList
        End If

        If context.Request.ServerVariables("HTTP_X_FORWARDED") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_X_FORWARDED")
            Return ipList
        End If

        If context.Request.ServerVariables("HTTP_X_CLUSTER_CLIENT_IP") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_X_CLUSTER_CLIENT_IP")
            Return ipList
        End If

        If context.Request.ServerVariables("HTTP_FORWARDED_FOR") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_FORWARDED_FOR")
            Return ipList
        End If

        If context.Request.ServerVariables("HTTP_FORWARDED") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_FORWARDED")
            Return ipList
        End If

        If context.Request.ServerVariables("HTTP_VIA") IsNot Nothing Then
            ipList = context.Request.ServerVariables("HTTP_VIA")
            Return ipList
        End If

        If context.Request.UserHostAddress IsNot Nothing Then
            ipList = context.Request.UserHostAddress
            Return ipList
        End If

        Return "No-Ip"

    End Function

End Class
