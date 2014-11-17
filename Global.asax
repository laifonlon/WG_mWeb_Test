<%@ Application Language="VB" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 應用程式啟動時執行的程式碼
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 應用程式關閉時執行的程式碼
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' 發生未處理錯誤時執行的程式碼
        If Request.Path = "/ChartImg.axd" Then Exit Sub
        Dim lastUrl As String = ""
        If Request.UrlReferrer IsNot Nothing Then lastUrl = Request.UrlReferrer.ToString
        
        Dim IsLogin As Boolean = False
        Dim MemberNo As String = "0"
        Dim UserName As String = "NotLogin"
        If HttpContext.Current.Session IsNot Nothing AndAlso MemberDataAccessor.Instance.IsLogin Then
            IsLogin = True
            MemberNo = MemberDataAccessor.Instance.GetMemberNo
            UserName = My.User.Name
        End If

        Dim ex As Exception = Server.GetLastError()
        If ex IsNot Nothing Then
            
            Dim Message As String = "發生錯誤的網頁:{0}錯誤訊息:{1}詳細資訊:{2}" '堆疊內容:{2}"
            Message = String.Format(Message, Request.Path + Environment.NewLine, ex.GetBaseException.Message + Environment.NewLine, Environment.NewLine + ex.GetBaseException.StackTrace + Environment.NewLine + Environment.NewLine) ', Environment.NewLine + ex.StackTrace)
            Message += "Action : " + MemberDataAccessor.Instance.Action + vbCrLf + _
                                        "輸入頁 : " + Request.Url.ToString + vbCrLf + _
                                        "前一頁 : " + lastUrl + vbCrLf + _
                                        "MemberNo : " + MemberNo + vbCrLf + _
                                        "UserName :  " + UserName + vbCrLf + _
                                        "IP : " + MemberDataAccessor.Instance.GetClientIP + vbCrLf + _
                                        Now.ToString + vbCrLf + _
                                        "-----------------------------------------------------------------------------------------------------------------------" + vbCrLf
            LogWriter.Instance.WriteLog(Message)
            Server.ClearError() '執行這行會變成全白畫面
            'Response.ContentEncoding = Encoding.UTF8
            'Response.Write("<br /><div style='text-align:center;'>很抱歉，系統暫時發生錯誤，請稍後再試。<br /><br /><a href='/'>回玩股網首頁</a></div>")
        
            ex = Nothing
            lastUrl = ""
            Message = ""
        End If
        
        MemberDataAccessor.Instance.Action = ""
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 啟動新工作階段時執行的程式碼
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 工作階段結束時執行的程式碼。 
        ' 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        ' 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        ' 或 SQLServer，就不會引發這個事件。
    End Sub
        
    Private counter As Integer = -1
    Private lockObject As New Object
    Private ipCounterList As New Generic.Dictionary(Of String, Integer)
    
    Private Sub IpRequestRecord(ByVal sender As Object)
        
        'Dim httpApplication As HttpApplication = sender
        'Dim context As HttpContext = httpApplication.Context
        Dim ipAddress As String = GetUserIP(sender)
        
        If ipAddress IsNot Nothing And ipAddress.Length > 0 Then
            SyncLock lockObject
                If HttpContext.Current.Application("IpRequestRecord") IsNot Nothing Then
                    ipCounterList = HttpContext.Current.Application("IpRequestRecord")
                End If
                If Not ipCounterList.ContainsKey(ipAddress) Then
                    ipCounterList.Add(ipAddress, 1)
                Else
                    Dim count As Integer = ipCounterList(ipAddress) + 1
                    ipCounterList(ipAddress) = count
                    HttpContext.Current.Application("IpRequestRecord") = ipCounterList
                End If
            End SyncLock
        End If
        
    End Sub
    
    Private Function GetUserIP(ByVal sender As Object) As String
        Dim ipList As String = String.Empty
        Dim httpApplication As HttpApplication = sender
        Dim context As HttpContext = httpApplication.Context
        
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
    
    Private BlockIpList As New Dictionary(Of String, Boolean)
    Private Sub SetBlockIpList()
        SyncLock lockObject
            BlockIpList = New Dictionary(Of String, Boolean)
            BlockIpList.Add("157.55.35.48", True)
            BlockIpList.Add("157.55.32.28", True)
            BlockIpList.Add("157.55.32.102", True)
            HttpContext.Current.Application("BlockIpList") = BlockIpList
        End SyncLock
    End Sub
    
    
    Private BlockIpRequestUrlList As New Generic.Dictionary(Of String, Generic.Dictionary(Of String, Integer))
    Private Sub BlockIpListProcess(ByVal sender As Object)
        
        Dim userIP = GetUserIP(sender)
        Dim isBlockIP = False
        
        If HttpContext.Current.Application("BlockIpList") IsNot Nothing Then
            BlockIpList = HttpContext.Current.Application("BlockIpList")
        End If
        
        If BlockIpList.ContainsKey(userIP) Then
            isBlockIP = True
        End If
        
        If isBlockIP Then
            Dim httpApplication As HttpApplication = sender
            Dim context As HttpContext = httpApplication.Context
            Dim rawUrl As String = context.Request.RawUrl
            'Response.Write("rawUrl = " + rawUrl)
            'Dim ipAddress As String = GetUserIP(sender)
            
            Dim BlockIpRequestUrlCountList As New Generic.Dictionary(Of String, Integer)
            SyncLock lockObject
                If HttpContext.Current.Application("BlockIpRequestUrlList") IsNot Nothing Then
                    BlockIpRequestUrlList = HttpContext.Current.Application("BlockIpRequestUrlList")
                End If
                If Not BlockIpRequestUrlList.ContainsKey(userIP) Then
                    BlockIpRequestUrlCountList.Add(rawUrl, 1)
                    BlockIpRequestUrlList.Add(userIP, BlockIpRequestUrlCountList)
                Else
                    BlockIpRequestUrlCountList = BlockIpRequestUrlList(userIP)
                    If BlockIpRequestUrlCountList.ContainsKey(rawUrl) Then
                        Dim count As Integer = BlockIpRequestUrlCountList(rawUrl) + 1
                        BlockIpRequestUrlCountList(rawUrl) = count
                    Else
                        BlockIpRequestUrlCountList.Add(rawUrl, 1)
                    End If
                    BlockIpRequestUrlList(userIP) = BlockIpRequestUrlCountList
                    HttpContext.Current.Application("BlockIpRequestUrlList") = BlockIpRequestUrlList
                End If
            End SyncLock
            Response.End()
        End If
        
    End Sub
    
    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ''****維修時，全站導向sorry頁。
        'If Not Request.RawUrl.Contains("sorry.html") Then
        '    Response.Redirect("~/sorry.html")
        'End If
    End Sub
    
    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        Try
        Catch ex As Exception
        End Try
    End Sub
    
    Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
        Try
        Catch ex As Exception
        End Try
    End Sub
       
</script>