Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Collections.Generic

Public Class MemberDataAccessor

    Private Shared mObj As New MemberDataAccessor
    Public Shared ReadOnly Property Instance() As MemberDataAccessor
        Get
            Return mObj
        End Get
    End Property

    Public ReadOnly Property IsLogin() As Boolean
        Get
            With HttpContext.Current
                If .Request.Cookies("UserName") Is Nothing Then
                    FormsAuthentication.SignOut()
                    HttpContext.Current.Session.Clear()
                    HttpContext.Current.Session("IsLogin") = False
                    Return False
                End If
            End With
            If HttpContext.Current.Session("IsLogin") Is Nothing Or HttpContext.Current.Session("MemberNo") Is Nothing Or HttpContext.Current.Session("UserName") Is Nothing Then
                AutoLogin()
            End If
            If HttpContext.Current.Session("IsLogin") = False Then
                Return False
            ElseIf GetMemberNo() = "0" Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public Sub New()
        InitialMemberData()
    End Sub

    Public Property Action() As String
        Get
            If HttpContext.Current.Session Is Nothing OrElse _
            HttpContext.Current.Session("UserAction") Is Nothing Then Return ""
            Return HttpContext.Current.Session("UserAction")
        End Get
        Set(ByVal value As String)
            If HttpContext.Current.Session IsNot Nothing Then
                HttpContext.Current.Session("UserAction") = value
            End If
        End Set
    End Property

#Region "登入驗證"
    ' ''' <summary>
    ' ''' 自動登入狀態
    ' ''' </summary>
    'Public Sub SetAutoLoginState(ByVal userName As String, ByVal isAutoLogin As Boolean)
    '    Dim da As New WantGooTableAdapters.QueriesTableAdapter
    '    da.UpdateAutoLoginState(userName, True, GetClientIP)

    '    With HttpContext.Current
    '        .Session("IsLogin") = True
    '        .Session("UserName") = userName
    '        .Session("MemberNo") = GetMemberNo(userName)
    '    End With

    'End Sub

    ''' <summary>
    ''' 自動登入
    ''' </summary>
    Public Sub AutoLogin()
        With HttpContext.Current
            If .Session("IsLogin") Then Exit Sub
            If .Request.Cookies("UserName") Is Nothing Then Exit Sub

            If .Request.Url IsNot Nothing Then
                .Response.Redirect("/CMFunction/autologin.aspx?url=" + .Request.Url.ToString.Replace("&", "$"))
            Else
                .Response.Redirect("/CMFunction/autologin.aspx?url=http://www.wantgoo.com")
            End If
        End With

    End Sub

    ''' <summary>
    ''' 登出
    ''' </summary>
    Public Sub SignOut()
        WriteLoginLog("SignOut")

        Dim ma As New MemberAuthority
        ma.ClearCookie()

        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.UpdateAutoLoginState(GetUserName, False, GetClientIP)

        FormsAuthentication.SignOut()
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session("IsLogin") = False
    End Sub

    Private mIsLogWriting As Boolean
    Private mLogTemp As String = ""
    ''' <summary>
    ''' 寫登入Log
    ''' </summary>
    Public Sub WriteLoginLog(ByVal from As String)
        mLogTemp = mLogTemp + Format(Now, "HH:mm:ss") + vbTab + _
       GetMemberNo() + vbTab + _
       GetMemberNickName() + vbTab + _
        GetClientIP() + vbTab + from + vbCrLf

        If mIsLogWriting Then Exit Sub
        mIsLogWriting = True

        Dim logPath As String = "C:\WantGoo\Wantgoooooo1\tmp\Login\"
        If Not My.Computer.FileSystem.DirectoryExists(logPath) Then
            My.Computer.FileSystem.CreateDirectory(logPath)
        End If

        Dim filename As String = logPath + Format(Now, "yyyyMMdd") & ".log"

        Try
            My.Computer.FileSystem.WriteAllText(filename, mLogTemp, True)
        Catch ex As Exception
        End Try

        mLogTemp = ""
        mIsLogWriting = False
    End Sub
#End Region


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

#Region "會員資料"

    Private Sub InitialMemberData()

        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()
        Try
            Dim cmd As String = "SELECT UserName, MemberNo, NickName, UploadPicAdd,BlogName FROM Member"
            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            While reader.Read

                Dim UserName As String = reader.Item("UserName")
                Dim MemberNo As Integer = Integer.Parse(reader.Item("MemberNo"))
                Dim NickName As String = reader.Item("NickName")
                Dim UploadPicAdd As String = reader.Item("UploadPicAdd")
                Dim BlogName As String = reader.Item("BlogName")

                If Not mMemberNoTable.ContainsKey(UserName.Trim) Then
                    mMemberNoTable.Add(UserName.Trim, MemberNo.ToString)
                End If

                If Not mNickNameTable.ContainsKey(MemberNo.ToString) Then
                    mNickNameTable.Add(MemberNo.ToString, NickName.Trim)
                End If

                If Not mUserNameTable.ContainsKey(MemberNo.ToString) Then
                    mUserNameTable.Add(MemberNo.ToString, UserName.Trim)
                End If

                If MemberNo.ToString <> BlogName.Trim Then

                    If Not mBlogNameTable.ContainsKey(MemberNo.ToString) Then
                        mBlogNameTable.Add(MemberNo.ToString, BlogName.Trim)
                    End If

                    If Not mMemberNoBlogName.ContainsKey(BlogName.Trim.ToLower) Then
                        mMemberNoBlogName.Add(BlogName.Trim.ToLower, MemberNo.ToString)
                    End If

                End If

                If Not mMemberPic.ContainsKey(MemberNo) Then
                    If UploadPicAdd IsNot Nothing AndAlso _
                       UploadPicAdd.ToString.Trim <> "" Then
                        mMemberPic.Add(MemberNo, "http://img.wantgoo.com/WantGooFiles/uploadfiles/" + UploadPicAdd.Trim)
                    Else
                        mMemberPic.Add(MemberNo, "http://www.wantgoo.com/image/displaydefault1.png")
                    End If
                End If

            End While

            reader.Close()

        Catch ex As Exception
            'WantGooApiFrameWork.Common.LogInfo.WriteLog2DB("MemberDataAccessor.InitialMemberData", ex, "System.Debug", "System")
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        'Dim da As New WantGooTableAdapters.MemberTableAdapter
        'Dim tb As WantGoo.MemberDataTable = da.GetData

        'For Each row As WantGoo.MemberRow In tb.Rows

        '    If Not mMemberNoTable.ContainsKey(row.UserName.Trim) Then
        '        mMemberNoTable.Add(row.UserName.Trim, row.MemberNo.ToString)
        '    End If

        '    If Not mNickNameTable.ContainsKey(row.MemberNo.ToString) Then
        '        mNickNameTable.Add(row.MemberNo.ToString, row.NickName.Trim)
        '    End If

        '    If Not mUserNameTable.ContainsKey(row.MemberNo.ToString) Then
        '        mUserNameTable.Add(row.MemberNo.ToString, row.UserName.Trim)
        '    End If

        '    If row.MemberNo.ToString <> row.BlogName.Trim Then

        '        If Not mBlogNameTable.ContainsKey(row.MemberNo.ToString) Then
        '            mBlogNameTable.Add(row.MemberNo.ToString, row.BlogName.Trim)
        '        End If

        '        If Not mMemberNoBlogName.ContainsKey(row.BlogName.Trim.ToLower) Then
        '            mMemberNoBlogName.Add(row.BlogName.Trim.ToLower, row.MemberNo.ToString)
        '        End If

        '    End If

        '    If Not mMemberPic.ContainsKey(row.MemberNo) Then
        '        If row.UploadPicAdd IsNot Nothing AndAlso _
        '           row.UploadPicAdd.ToString.Trim <> "" Then
        '            mMemberPic.Add(row.MemberNo, "http://cache.wantgoo.com/uploadfiles/" + row.UploadPicAdd.Trim)
        '        Else
        '            mMemberPic.Add(row.MemberNo, "http://cache.wantgoo.com/image/displaydefault1.png")
        '        End If
        '    End If
        'Next

    End Sub

    Public Function GetAllMemberNo() As ICollection
        Return mMemberNoTable.Values
    End Function

    Public Function GetAllUserName() As ICollection
        Return mUserNameTable.Values
    End Function

    Private mMemberNoTable As New Hashtable
    ''' <summary>
    ''' 取得會員編號
    ''' </summary>
    Public ReadOnly Property MemberNo() As String
        Get
            If HttpContext.Current.Session("MemberNo") Is Nothing Then
                Return "0"
            End If
            Return HttpContext.Current.Session("MemberNo")
        End Get
    End Property
    Public Overloads Function GetMemberNo() As String
        If HttpContext.Current.Session("MemberNo") Is Nothing Then
            Return "0"
        End If
        Return HttpContext.Current.Session("MemberNo")
    End Function
    Public Overloads Function GetMemberNo(ByVal userName As String) As String
        If mMemberNoTable.ContainsKey(userName) AndAlso Val(mMemberNoTable(userName)) > 0 Then
            Return mMemberNoTable(userName)
        Else
            Dim memberNo As Integer
            Try
                Dim da As New WantGooTableAdapters.QueriesTableAdapter
                da.GetMemberNo(userName, memberNo)
            Catch ex As Exception
                LogWriter.Instance.WriteLog("從資料庫讀取MembeNO錯誤" + vbCrLf + ex.Message)
            End Try

            SyncLock mMemberNoTable
                If mMemberNoTable.ContainsKey(userName.Trim) Then
                    mMemberNoTable(userName.Trim) = memberNo.ToString
                Else
                    mMemberNoTable.Add(userName.Trim, memberNo.ToString)
                End If

                If Not mUserNameTable.ContainsKey(memberNo.ToString) Then
                    mUserNameTable.Add(memberNo.ToString, userName.Trim)
                End If
            End SyncLock

            Return memberNo.ToString
        End If
    End Function

    Private mUserNameTable As New Hashtable
    ''' <summary>
    ''' 取得會員名稱
    ''' </summary>
    Public ReadOnly Property UserName() As String
        Get
            If HttpContext.Current.Session("UserName") Is Nothing Then
                Return ""
            End If
            Return HttpContext.Current.Session("UserName").Trim
        End Get
    End Property
    Public Function GetUserName(ByVal memberNo As String) As String
        If memberNo Is Nothing Then Return ""

        If mUserNameTable.ContainsKey(memberNo) AndAlso mUserNameTable(memberNo) <> "" Then
            Return mUserNameTable(memberNo).Trim
        Else
            Dim userName As String = ""
            Try
                Dim da As New WantGooTableAdapters.QueriesTableAdapter
                da.GetUserName(memberNo, userName)
            Catch ex As Exception
            End Try

            SyncLock mUserNameTable
                If mUserNameTable.ContainsKey(memberNo.Trim) Then
                    mUserNameTable(memberNo.Trim) = userName.Trim
                Else
                    mUserNameTable.Add(memberNo.Trim, userName.Trim)
                End If

                If Not mMemberNoTable.ContainsKey(userName) Then
                    mMemberNoTable.Add(userName.Trim, memberNo.Trim)
                End If
            End SyncLock

            Return userName.Trim
        End If
    End Function
    Public Function GetUserName() As String
        If HttpContext.Current.Session("UserName") Is Nothing Then
            Return ""
        End If
        Return HttpContext.Current.Session("UserName").Trim
    End Function

    Private mNickNameTable As New Hashtable
    ''' <summary>
    ''' 取得會員匿稱
    ''' </summary>
    Public Function GetMemberNickName(ByVal memberNo As String) As String
        If memberNo Is Nothing OrElse memberNo = "0" Then Return ""

        Dim userName As String = GetUserName(memberNo)

        If mNickNameTable.ContainsKey(memberNo) AndAlso mNickNameTable(memberNo) <> "" Then
            Return mNickNameTable(memberNo).Trim
        Else
            Dim nickName As String = ""
            Try
                Dim da As New WantGooTableAdapters.QueriesTableAdapter
                da.GetMemberNickName(userName, nickName)
            Catch ex As Exception
            End Try

            SyncLock mNickNameTable
                If mNickNameTable.ContainsKey(memberNo.Trim) Then
                    mNickNameTable(memberNo.Trim) = nickName.Trim
                Else
                    mNickNameTable.Add(memberNo.Trim, nickName.Trim)
                End If
            End SyncLock

            Return nickName.Trim
        End If
    End Function
    Public Function GetMemberNickName() As String
        If GetMemberNickName(GetMemberNo) Is Nothing Then
            Return ""
        End If
        Return GetMemberNickName(GetMemberNo).Trim
    End Function
    Public Sub ReNickName(ByVal newNickName As String)
        mNickNameTable.Remove(GetMemberNo)
        mNickNameTable.Add(GetMemberNo, newNickName)
    End Sub
    Public Sub ReNickName(ByVal memberNo As String, ByVal newNickName As String)
        mNickNameTable.Remove(memberNo)
        mNickNameTable.Add(memberNo, newNickName)
    End Sub

    ''' <summary>
    ''' 取得BLOG名稱
    ''' </summary>
    Private mBlogNameTable As New Hashtable
    Public Function GetBlogName(ByVal memberNo As String) As String
        If memberNo Is Nothing OrElse memberNo = "0" Then Return "0"

        If mBlogNameTable.ContainsKey(memberNo) Then
            Return mBlogNameTable(memberNo)
        Else
            Return memberNo
        End If
    End Function
    Public Function GetBlogName() As String
        Return GetBlogName(GetMemberNo).Trim
    End Function

    ''' <summary>
    ''' 變更BLOG名稱
    ''' </summary>
    Public Sub ReBlogName(ByVal memberNo As String, ByVal newName As String)
        Dim oldName As String
        If mBlogNameTable.Contains(memberNo) Then
            oldName = mBlogNameTable(memberNo)

            mBlogNameTable.Remove(memberNo)
            mMemberNoBlogName.Remove(oldName)
        End If

        mBlogNameTable.Add(memberNo, newName)
        mMemberNoBlogName.Add(newName, memberNo)
    End Sub

    ''' <summary>
    ''' 由Blog名稱 取得會員編號
    ''' </summary>
    Private mMemberNoBlogName As New Hashtable
    Public Function GetMemberNobyBlogName(ByVal name As String) As String
        If name Is Nothing OrElse name = "" Then Return GetMemberNo()
        If IsNumeric(name) Then Return name

        If mMemberNoBlogName.ContainsKey(name.Trim.ToLower) Then
            Return mMemberNoBlogName(name.Trim.ToLower).Trim
        Else
            Return "0"
        End If
    End Function

    ''' <summary>
    ''' 修改密碼
    ''' </summary>
    Public Function ChangePasswrod(ByVal newPassword As String) As Boolean
        Dim state As Integer
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.ChangePassword(GetUserName, newPassword, state)
        Return True
    End Function

    ''' <summary>
    ''' 取得會員資料
    ''' </summary>
    Public Function GetMemberData() As WantGoo.SelectMemberDataRow
        Return GetMemberData(GetMemberNo)
    End Function

    ''' <summary>
    ''' 取得會員資料
    ''' </summary>
    Public Function GetMemberData(ByVal memberNo As Integer) As WantGoo.SelectMemberDataRow
        Dim da As New WantGooTableAdapters.SelectMemberDataTableAdapter
        Dim table As DataTable = da.GetData(memberNo)
        If table.Rows.Count = 0 Then Return Nothing
        Return table.Rows(0)
    End Function

    ''' <summary>
    ''' 取得使用者自訂區塊
    ''' </summary>
    Public Function GetMemberDefineBlock() As WantGoo.SelectMemberDefineBlockDataTable
        Return GetMemberDefineBlock(GetMemberNo)
    End Function

    ''' <summary>
    ''' 取得使用者自訂區塊
    ''' </summary>
    Public Function GetMemberDefineBlock(ByVal memberNo As String) As WantGoo.SelectMemberDefineBlockDataTable
        Dim da As New WantGooTableAdapters.SelectMemberDefineBlockTableAdapter
        Return da.GetData(memberNo)
    End Function

    ''' <summary>
    ''' 取得匿稱
    ''' </summary>
    Public Function GetMyNickName() As String
        Return GetMemberNickName(GetMemberNo).Trim
    End Function

    ''' <summary>
    ''' 取得會員人數
    ''' </summary>
    Public Function GetMemberCount() As Integer
        Dim count As Integer
        If HttpContext.Current.Application("MemberCount") Is Nothing OrElse _
         HttpContext.Current.Application("MemberCount") = 0 Then
            Dim da As New WantGooTableAdapters.QueriesTableAdapter
            da.GetMemberCount(count)
            HttpContext.Current.Application("MemberCount") = count
        Else
            count = HttpContext.Current.Application("MemberCount")
        End If

        Return count
    End Function

    Public Function IsSuperMember() As Boolean
        If GetMemberLevel() = MemberLevel.Super Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsRichMember() As Boolean
        Dim level As MemberLevel = GetMemberLevel()
        If level = MemberLevel.Rich OrElse level = MemberLevel.Super Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetSuperAdiminstratorList() As List(Of String)
        Dim SuperAdiminstratorList As List(Of String) = New List(Of String)
        SuperAdiminstratorList.Add("119") '楚狂人
        SuperAdiminstratorList.Add("356") '小俏妞
        SuperAdiminstratorList.Add("67867") '玩股神探
        Return SuperAdiminstratorList
    End Function

    Public Function IsSuperAdiminstrator() As Boolean
        If GetSuperAdiminstratorList().Contains(GetMemberNo()) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetAdiminstratorList() As List(Of String)
        Dim SuperAdiminstratorList As List(Of String) = New List(Of String)
        SuperAdiminstratorList.Add("81287") '玩股客服
        Return SuperAdiminstratorList
    End Function

    Public Function IsAdiminstrator() As Boolean
        '是 Adiminstrator 或 SuperAdiminstrator
        If GetAdiminstratorList().Contains(GetMemberNo()) Or GetSuperAdiminstratorList().Contains(GetMemberNo()) Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Function IsDeveloper() As Boolean
        'If GetMemberNo() = "506" Then Return True
        'If GetMemberNo() = "18291" Then Return True
        'If GetMemberNo() = "736" Then Return True
        If GetMemberNo() = "43602" Then Return True
        Return False
    End Function

    Public Function IsCellphoneApproved() As Boolean
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Try
            Dim cmd As String = "SELECT IsCellphoneApproved FROM dbo.Member WHERE MemberNo = '" & GetMemberNo() & "'"
            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)

            Dim result As Integer = 0
            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            While reader.Read
                result = Val(reader.Item("IsCellphoneApproved"))
                Exit While
            End While

            reader.Close()

            If result = 2 Then
                Return True
            End If
        Catch ex As Exception
            Return False
        Finally
            connection.Close()
            connection.Dispose()
        End Try
        Return False
    End Function

    Private mMemberPic As New Generic.Dictionary(Of String, String)
    ''' <summary>
    ''' 取得會員大頭圖片路徑
    ''' </summary>
    Public Function GetHeadPicUrl(ByVal memberNo As String) As String
        If mMemberPic.ContainsKey(memberNo) Then Return mMemberPic(memberNo)

        Dim row As WantGoo.SelectMemberDataRow = GetMemberData(memberNo)
        If row IsNot Nothing AndAlso _
           row.UploadPicAdd IsNot Nothing AndAlso _
           row.UploadPicAdd.ToString.Trim <> "" Then
            mMemberPic.Add(memberNo, "http://cache.wantgoo.com/uploadfiles/" + row.UploadPicAdd.Trim)
            Return "http://cache.wantgoo.com/uploadfiles/" + row.UploadPicAdd.Trim
        End If

        mMemberPic.Add(memberNo, "http://cache.wantgoo.com/image/displaydefault1.png")
        Return "http://cache.wantgoo.com/image/displaydefault1.png"
    End Function

    Public Function GetHeadPicUrl() As String
        Return GetHeadPicUrl(GetMemberNo)
    End Function

    ''' <summary>
    ''' 更新指定的會員頭像路徑到記憶體
    ''' </summary>
    Public Sub UpdateHeadPicUrl(ByVal memberNo As String)
        mMemberPic.Remove(memberNo)
        GetHeadPicUrl(memberNo)
    End Sub
    Public Sub UpdateHeadPicUrl()
        mMemberPic.Remove(GetMemberNo)
        GetHeadPicUrl(GetMemberNo)
    End Sub

    Private mEmailApprovedList As New Generic.List(Of String)
    ''' <summary>
    ''' 判斷會員Eamil是否已認證
    ''' </summary>
    Public Function IsEmailApproved(ByVal memberNo As String) As Boolean
        If Val(memberNo) = 0 Then Return False

        If mEmailApprovedList.Contains(memberNo.Trim) Then Return True

        Dim row As WantGoo.SelectMemberDataRow = GetMemberData(memberNo)
        If row Is Nothing Then Return False

        Dim isApproved As Boolean = row("IsEmailApproved")
        If isApproved Then mEmailApprovedList.Add(memberNo.Trim)

        Return isApproved
    End Function
    Public Function IsEmailApproved() As Boolean
        Return IsEmailApproved(GetMemberNo)
    End Function

    ''' <summary>
    ''' 取得會員等級
    ''' </summary>
    Public Function GetMemberLevel() As MemberLevel
        Return GetMemberLevel(GetMemberNo)
    End Function

    Public Function GetMemberLevel(ByVal memberNo As String, Optional ByRef days As Integer = 0) As MemberLevel
        If memberNo Is Nothing OrElse Val(memberNo) <= 0 Then Return MemberLevel.Normal

        Dim row As WantGoo.SelectMemberDataRow = GetMemberData(memberNo)
        If row Is Nothing Then Return MemberLevel.Normal

        If row.SuperUserDeadline > Now Then
            days = row.SuperUserDeadline.Subtract(Now).TotalDays
            Return MemberLevel.Super
        End If

        If row.RichUserDeadline > Now Then
            days = row.RichUserDeadline.Subtract(Now).TotalDays
            Return MemberLevel.Rich
        End If

        If Member.Instance.IsHottipDeveloper Then
            days = Member.Instance.GetHottipDevelopDeadline.Subtract(Now).TotalDays
            Return MemberLevel.Developer
        End If

        Return MemberLevel.Normal
    End Function

    ''' <summary>
    ''' 好野會員到期日
    ''' </summary>
    Public Function GetRichUserDeadline(ByVal memberNo As String) As Date
        If memberNo Is Nothing OrElse Val(memberNo) <= 0 Then Return Now

        Dim row As WantGoo.SelectMemberDataRow = GetMemberData(memberNo)
        If row Is Nothing Then Return Now
        Return row.RichUserDeadline
    End Function
    Public Function GetRichUserDeadline() As Date
        Return GetRichUserDeadline(GetMemberNo)
    End Function

    ''' <summary>
    ''' 永富會員到期日
    ''' </summary>
    Public Function GetSuperUserDeadline(ByVal memberNo As String) As Date
        If memberNo Is Nothing OrElse Val(memberNo) <= 0 Then Return Now

        Dim row As WantGoo.SelectMemberDataRow = GetMemberData(memberNo)
        If row Is Nothing Then Return Now
        Return row.SuperUserDeadline
    End Function
    Public Function GetSuperUserDeadline() As Date
        Return GetSuperUserDeadline(GetMemberNo)
    End Function

#End Region

#Region "股票"
    ''' <summary>
    ''' 取得會員持股
    ''' </summary>
    Public Function GetMyStock() As DataTable
        Dim da As New WantGooTableAdapters.SelectMemberStocksTableAdapter
        Return da.GetData(GetUserName)
    End Function

    ''' <summary>
    ''' 取得會員持股摘要
    ''' </summary>
    Public Function GetMyStockSummary() As DataTable
        Return GetMyStockSummary(GetUserName)
    End Function

    ''' <summary>
    ''' 取得會員持股摘要
    ''' </summary>
    Public Function GetMyStockSummary(ByVal userName As String) As DataTable
        Dim da As New WantGooTableAdapters.SelectMemberStocksSummaryTableAdapter
        Return da.GetData(userName)
    End Function

    ''' <summary>
    ''' 取得會員下單列表
    ''' </summary>
    Public Function GetMyOrders() As DataTable
        Dim da As New WantGooTableAdapters.SelectMemberOrderTableAdapter
        Return da.GetData(GetUserName)
    End Function

    ''' <summary>
    ''' 取得會員成交記錄
    ''' </summary>
    Public Function GetMyTradeRecords(ByVal stockNo As Integer, ByVal queryDate As Date) As DataTable
        Dim da As New WantGooTableAdapters.SelectMemberTradeRecordTableAdapter
        Return da.GetData(GetUserName, stockNo, queryDate)
    End Function

    ''' <summary>
    ''' 會員下單
    ''' </summary>
    Public Function PlaceOrder(ByVal stockNo As Integer, ByVal price As Double, ByVal volume As Integer, ByRef orderId As Integer) As Boolean
        Dim returnValue As Integer
        Try
            Dim da As New WantGooTableAdapters.QueriesTableAdapter
            da.PlaceOrderByUserName(GetUserName, stockNo, volume, price, orderId, returnValue)
        Catch ex As Exception
            LogWriter.Instance.WriteLog(ex.Message)
        End Try

        mErrorDescription = ""

        If returnValue = 0 Then
            Return True
        ElseIf returnValue = 1 Then
            mErrorDescription = " 現金不足"
        ElseIf returnValue = 2 Then
            mErrorDescription = " 暫停交易中"
        ElseIf returnValue = 3 Then
            mErrorDescription = " 流通股數不足, 禁止空單"
        ElseIf returnValue = 4 Then
            mErrorDescription = " 下市停止交易"
        End If
        Return False
    End Function

    ''' <summary>
    ''' 錯誤訊息
    ''' </summary>
    Private mErrorDescription As String = ""
    Public ReadOnly Property ErrorDescription() As String
        Get
            Return mErrorDescription
        End Get
    End Property

    ''' <summary>
    ''' 取消下單
    ''' </summary>
    Public Function CancelOrder(ByVal orderId As Integer) As Boolean
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        Dim returnValue As Integer = _
        da.CancelOrder(GetUserName, orderId)

        If returnValue = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region "資金"
    ''' <summary>
    ''' 取得會員資金
    ''' </summary>
    Public Overloads Function GetMyMoney() As Integer
        Return GetMyMoney(GetUserName)
    End Function

    ''' <summary>
    ''' 取得會員資金
    ''' </summary>
    Public Overloads Function GetMyMoney(ByVal userName As String) As Integer
        Dim money As Decimal
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetMemberMoney(userName, money)
        Return CInt(Math.Floor(CDbl(money)))
    End Function

    ''' <summary>
    ''' 取得會員金幣
    ''' </summary>
    Public Function GetMyGold() As Integer
        Return GetMyGold(GetUserName)
    End Function
    Public Function GetMyGold(ByVal userName As String) As Integer
        If userName = "" Then Return 0
        Dim money As Decimal
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetMemberGold(userName, money)
        Return CInt(Math.Floor(CDbl(money)))
    End Function

    ''' <summary>
    ''' 加或扣會員資金
    ''' </summary>
    Public Sub AddMyMoney(ByVal money As Double, Optional ByVal moneyType As MoneyType = Global.MoneyType.Casino)
        Dim memberNo As Integer = Val(GetMemberNo())
        If memberNo <= 0 Then Exit Sub
        AddMemberMoney(memberNo, money, moneyType)
    End Sub

    ''' <summary>
    ''' 加或扣會員資金
    ''' </summary>
    Public Sub AddMemberMoney(ByVal memberNo As Integer, ByVal money As Double, Optional ByVal moneyType As MoneyType = Global.MoneyType.Casino)
        If memberNo <= 0 Then Exit Sub
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.AddMemberMoney(memberNo, money, moneyType)
    End Sub

    ''' <summary>
    ''' 加或扣會員金幣
    ''' </summary>
    Public Sub AddMemberGold(ByVal memberNo As Integer, ByVal gold As Double, ByVal type As GoldType, ByVal fromMember As Integer)
        If memberNo <= 0 Then Exit Sub
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.AddMemberGold(memberNo, gold, type, fromMember)
    End Sub
    Public Sub AddMemberGold(ByVal gold As Double, ByVal type As GoldType, ByVal fromMember As Integer)
        Dim memberNo As Integer = Val(GetMemberNo())
        If memberNo <= 0 Then Exit Sub
        AddMemberGold(memberNo, gold, type, fromMember)
    End Sub

    ''' <summary>
    ''' 加或扣進階會員會期
    ''' </summary>
    Public Sub ExtendPayUserDeadline(ByVal memberNo As Integer, ByVal level As MemberLevel, ByVal days As Integer)
        If memberNo <= 0 Then Exit Sub
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.ExtendPayUserDeadline(memberNo, level, days)
    End Sub
    Public Sub ExtendPayUserDeadline(ByVal memberNo As Integer, ByVal level As MemberLevel, ByVal days As Integer, ByVal source As Integer)
        If memberNo <= 0 Then Exit Sub
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString
        Dim command As String = "Exec dbo.ExtendPayUserDeadline @MemberNo,@MemberType,@Days,@Source"
        command = command.Replace("@MemberNo", memberNo.ToString)
        command = command.Replace("@MemberType", CInt(level).ToString)
        command = command.Replace("@Days", days.ToString)
        command = command.Replace("@Source", source.ToString)
        sds.SelectCommand = command
        sds.Select(New DataSourceSelectArguments)
    End Sub

    ''' <summary>
    ''' 取得會員總財產
    ''' </summary>
    Public Overloads Function GetMemberAsset(ByVal memberNo As String) As Long
        Dim asset As Decimal
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetMemberAsset(memberNo, asset)
        Return asset
    End Function

    ''' <summary>
    ''' 取得會員證券帳戶金額
    ''' </summary>
    Public Overloads Function GetMyStockAccount() As Long
        Return GetMyStockAccount(GetUserName)
    End Function

    ''' <summary>
    ''' 取得會員證券帳戶金額
    ''' </summary>
    Public Overloads Function GetMyStockAccount(ByVal userName As String) As Long
        If userName = "" Then Return 0
        Dim money As Decimal
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetMemberStockAccount(userName, money)
        Return CLng(money)
    End Function

    ''' <summary>
    ''' 取得會員可下單餘額
    ''' </summary>
    Public Overloads Function GetMyMoneyCanOrder() As Long
        Return GetMyMoneyCanOrder(GetUserName)
    End Function

    ''' <summary>
    ''' 取得會員可下單餘額
    ''' </summary>
    Public Overloads Function GetMyMoneyCanOrder(ByVal userName As String) As Long
        If userName = "" Then Return 0
        Dim money As Decimal
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.GetMemberMoneyCanOrder(userName, money)
        Return CLng(money)
    End Function
#End Region


    ''' <summary>
    ''' 轉帳服務
    ''' </summary>
    Public Function TransferAccount(ByVal money As Integer, _
                                            ByVal fromAccount As AccountType, _
                                            ByVal toAccount As AccountType, _
                                            ByVal comment As String) As Boolean
        If GetUserName() = "" Then Return False

        Dim state As Integer
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.TransferAccount(GetUserName, money, _
                             [Enum].GetName(GetType(AccountType), fromAccount), _
                             [Enum].GetName(GetType(AccountType), toAccount), comment, state)
        If state = 1 Then '轉帳成功
            Return True
        ElseIf state = 2 Then '帳戶金額不足
            Return False
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 建立會員帳號
    ''' </summary>
    Public Function CreateMember(ByVal userName As String, ByVal lastName As String, ByVal firstName As String, ByVal sex As Integer, ByVal password As String, ByVal nickname As String, ByVal email As String, ByVal refNo As Integer) As Boolean
        Dim state As Integer
        Dim da As New WantGooTableAdapters.QueriesTableAdapter
        da.CreateMember(userName, lastName, firstName, sex, password, nickname, email, refNo, state)

        Dim myCookie As HttpCookie
        myCookie = New HttpCookie("CreateMember_state", state.ToString())
        myCookie.Domain = ".wantgoo.com"
        myCookie.Expires = Now.AddMinutes(3)
        HttpContext.Current.Response.Cookies.Add(myCookie)

        If state = 1 Then '建立成功
            HttpContext.Current.Application("MemberCount") = GetMemberCount() + 1
            Return True
        ElseIf state = 2 Then '帳戶已存在
            Return False
        Else
            Return False
        End If
    End Function

#Region "會員註冊歡迎信(FB)"
    Public Sub SendWelcomeMail(Optional ByVal isFacebook As Boolean = True)
        SendWelcomeMail(GetMemberNo, isFacebook)
    End Sub

    Public Sub SendWelcomeMail(ByVal MemberNo As String, Optional ByVal isFacebook As Boolean = True)

        Dim password As String = MemberDataAccessor.Instance.GetMemberData(MemberNo)("Password")
        Dim nickName As String = MemberDataAccessor.Instance.GetMemberNickName(MemberNo)
        Dim subject As String = "歡迎加您加入玩股網！"
        Dim content As String = _
                "玩股網誠摯歡迎您的加入！<br />" + _
                "您在玩股網的帳號：" + MemberDataAccessor.Instance.GetUserName() + "<br />" + _
                "密碼：" + password + "<br />" + _
                "@FacebookNote<br />" + _
                "@WantGooContent" + _
                "<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        Dim wantgoocontent As String = ""

        If My.Computer.FileSystem.FileExists("C:\WantGoo\Wantgoooooo1\mall\email0.txt") Then
            wantgoocontent = My.Computer.FileSystem.ReadAllText("C:\WantGoo\Wantgoooooo1\mall\email0.txt")
        End If

        content = content.Replace("@WantGooContent", wantgoocontent)

        If isFacebook Then
            content = content.Replace("@FacebookNote", "若您不使用Facebook帳號登入時，可以使用此組帳號密碼登入玩股網。<br />")
        Else
            content = content.Replace("@FacebookNote", "")
        End If
        content = mHtmlTemplate.Replace("@Content", content).Replace("忠實讀者", nickName)

        MailServiceNew.Instance.SendHtmlMail(MemberDataAccessor.Instance.GetUserName(MemberNo), nickName, subject, content)
    End Sub

#End Region

#Region "會員認證信"
    Public Sub SendActiveMail(ByVal userName As String)

        'Debug.WriteCookie("MemberDataAccessor_SendActiveMail01", DateTime.Now.ToString())
        Action = "SendActiveMail"

        Dim memberNo As String = GetMemberNo(userName)
        Dim nickName As String = GetMemberNickName(memberNo)
        Dim subject As String = "玩股網註冊啟動"
        Dim mail As String = nickName + " 你好，" + vbCrLf + vbCrLf + _
                "你最近註冊了玩股網帳號。 要完成你的玩股網註冊程序，請按下列連結：" + vbCrLf + _
                "http://www.wantgoo.com/active.aspx?memberno=" + memberNo + _
                "&code=" + GetActiveCode(userName) + vbCrLf + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。" + vbCrLf + vbCrLf + _
                "若您手機未完成認證，會無法使用玩股網全部功能，請按下列連結進行認證：" + vbCrLf + _
                "http://www.wantgoo.com/PersonalPage/PhoneCertification.aspx" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。" + vbCrLf + vbCrLf + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com" + vbCrLf + vbCrLf + _
                "謝謝，" + vbCrLf + "Wantgoo Team" + vbCrLf + vbCrLf + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        'Debug.WriteCookie("MemberDataAccessor_SendActiveMail02", DateTime.Now.ToString())
        MailService.Instance.SendMail(userName, nickName, subject, mail)

        'If userName.ToLower.EndsWith("yahoo.com") Then
        '    MailService.Instance.SendMail(userName + ".tw", nickName, subject, mail)
        'ElseIf userName.ToLower.EndsWith("yahoo.com.tw") Then
        '    MailService.Instance.SendMail(userName.ToLower.Replace("@yahoo.com.tw", "@yahoo.com"), nickName, subject, mail)
        'End If
    End Sub

    Private Function GetActiveCode(ByVal userName As String) As String
        Dim data As Byte() = Encoding.ASCII.GetBytes(userName.ToLower)
        For i As Integer = 0 To data.Length - 1
            data(i) -= (i + 3) ^ 3 Mod 30
            If data(i) <= 65 Then data(i) += 40
            If data(i) >= 122 Then data(i) -= 20
        Next

        Return Encoding.ASCII.GetString(data).Replace("/", "1").Replace("\", "2").Replace("'", "3").Replace("^", "4").Replace("[", "5").Replace("]", "6").Replace("_", "7").Replace("<", "8").Replace(">", "9").Replace(":", "0").Replace("'", "a").Replace("?", "b").Replace("`", "c").Replace("-", "d")
    End Function

    Public Function ActiveAccount(ByVal memberNo As String, ByVal activeCode As String) As Boolean
        If GetActiveCode(GetUserName(memberNo)).Trim = activeCode.Trim Then
            Action = "ApproveEamil"
            Dim da As New WantGooTableAdapters.QueriesTableAdapter
            da.ApproveEmail(memberNo)
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region "會員密碼信"
    Public Function SendPasswordMail(ByVal userName As String) As Boolean
        Dim memberno As String = GetMemberNo(userName)
        If Val(memberno) = 0 Then Return False
        Dim password As String = GetMemberData(memberno)("Password")
        Dim nickName As String = GetMemberNickName(memberno)
        Dim subject As String = "玩股網密碼查詢"
        Dim mail As String = nickName + " 您好，" + vbCrLf + vbCrLf + _
        "您的玩股網密碼：" + password + vbCrLf + vbCrLf + _
        "有任何問題，歡迎您來信玩股客服：service@wantgoo.com" + vbCrLf + vbCrLf + _
        "謝謝，" + vbCrLf + "Wantgoo Team"

        MailService.Instance.SendMail(userName, nickName, subject, mail)

        'If userName.ToLower.EndsWith("yahoo.com") Then
        '    MailService.Instance.SendMail(userName + ".tw", nickName, subject, mail)
        'ElseIf userName.ToLower.EndsWith("yahoo.com.tw") Then
        '    MailService.Instance.SendMail(userName.ToLower.Replace("@yahoo.com.tw", "@yahoo.com"), nickName, subject, mail)
        'End If
        Return True
    End Function
#End Region

#Region "會員邀請信"
    Public Function GetInviteMailBody(ByVal fromName As String, ByVal toName As String) As String
        Dim memberNo As String = GetMemberNo()
        Dim nickName As String = GetMemberNickName()
        Dim mail As String = "Dear " + toName + " ，" + vbCrLf + vbCrLf + _
                "我正在玩股網上學習投資理財，" + vbCrLf + _
                "這裡不只有虛擬的股票市場可以練習，還有很多高手傳授投資經驗和很多明牌股票可以參考。" + vbCrLf + vbCrLf + _
                "我覺得還不錯，所以想邀請你一起來。" + vbCrLf + vbCrLf + _
                "註冊頁連結" + vbCrLf + _
                "http://www.wantgoo.com/facebook/newRegister.aspx" + vbCrLf + _
                "如果無法直接點選，可以複製網址到瀏覽器來開啟。" + vbCrLf + vbCrLf + _
                "也可以用facebook帳號直接登入" + vbCrLf + _
                "http://apps.facebook.com/wantgoo" + vbCrLf + vbCrLf + _
                "這是我在玩股網上的個人頁。別忘了加我當好友" + vbCrLf + _
                "http://www.wantgoo.com/PersonalPage/MyData.aspx?MemberNo=" + memberNo + vbCrLf + vbCrLf + _
                "下面是玩股網簡介：" + vbCrLf + _
                "玩股網是擬真的虛擬金融市場，以及專屬投資人和投機客的社群。 " + vbCrLf + _
                "新手可以藉由玩股股市學習操作股票，高手也可以和其他高手一起討論進階技巧求進步，甚至寫投資教學文章還可以賺臺幣。" + vbCrLf + vbCrLf + _
                "玩股網WantGoo  http://www.wantgoo.com" + vbCrLf + vbCrLf + _
                "                                                                 Best Regards" + vbCrLf + _
                fromName + vbCrLf + vbCrLf + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        Return mail
    End Function

    Public Function SendInviteMail(ByVal fromName As String, ByVal fromAddress As String, _
                    ByVal toName As String, ByVal toAddress As String, Optional ByVal additionText As String = "") As Boolean
        If mMemberNoTable.ContainsKey(toAddress) Then Return False

        Action = "SendInviteMail"

        If additionText <> "" Then
            additionText = additionText + vbCrLf + vbCrLf
        End If

        Dim memberNo As String = GetMemberNo()
        Dim nickName As String = GetMemberNickName()
        Dim subject As String = "邀請你一起來玩股網投資致富"
        Dim mail As String = "Dear " + toName + " ，" + vbCrLf + vbCrLf + _
                additionText + _
                "我正在玩股網上學習投資理財，" + vbCrLf + _
                "這裡不只有虛擬的股票市場可以練習，還有很多高手傳授投資經驗和很多明牌股票可以參考。" + vbCrLf + vbCrLf + _
                "我覺得還不錯，所以想邀請你一起來。" + vbCrLf + vbCrLf + _
                "註冊頁連結" + vbCrLf + _
                "http://www.wantgoo.com/facebook/newRegister.aspx?ref=" + GetMemberNo() + vbCrLf + _
                "如果無法直接點選，可以複製網址到瀏覽器來開啟。" + vbCrLf + vbCrLf + _
                "也可以用facebook帳號直接登入" + vbCrLf + _
                "http://apps.facebook.com/wantgoo" + vbCrLf + vbCrLf + _
                "這是我在玩股網上的個人頁。別忘了加我當好友" + vbCrLf + _
                "http://www.wantgoo.com/PersonalPage/MyData.aspx?MemberNo=" + memberNo + vbCrLf + vbCrLf + _
                "下面是玩股網簡介：" + vbCrLf + _
                "玩股網是擬真的虛擬金融市場，以及專屬投資人和投機客的社群。 " + vbCrLf + _
                "新手可以藉由玩股股市學習操作股票，高手也可以和其他高手一起討論進階技巧求進步，甚至寫投資教學文章還可以賺臺幣。" + vbCrLf + vbCrLf + _
                "玩股網WantGoo  http://www.wantgoo.com" + vbCrLf + vbCrLf + _
                "                                                                 Best Regards" + vbCrLf + _
                fromName + vbCrLf + vbCrLf + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        MailService.Instance.SendMail(toAddress, toName, subject, mail, fromAddress, fromName)

        Return True
    End Function

    Public Sub SendSTMPMail( _
    ByVal mailClient As System.Net.Mail.SmtpClient, ByVal sFromEmail As String, _
    ByVal sPassword As String, ByVal sFromName As String, _
    ByVal sToName As String, ByVal sToEmail As String, _
    ByVal sHeader As String, ByVal sMessage As String)

        Dim thread As New Threading.Thread(AddressOf ThreadSendMail)

        Dim args(7) As Object
        args(0) = mailClient
        args(1) = sFromEmail
        args(2) = sPassword
        args(3) = sFromName
        args(4) = sToName
        args(5) = sToEmail
        args(6) = sHeader
        args(7) = sMessage

        thread.Start(args)


        ''Create Mail Message Object with content that you want to send with mail.
        'Dim MyMailMessage As New System.Net.Mail.MailMessage(sFromEmail, sToEmail, sHeader, sMessage)

        'MyMailMessage.IsBodyHtml = False

        ''Proper Authentication Details need to be passed when sending email from gmail
        'Dim mailAuthentication As New System.Net.NetworkCredential(sFromEmail, sPassword)

        ''Smtp Mail server of Gmail is "smpt.gmail.com" and it uses port no. 587
        ''For different server like yahoo this details changes and you can
        ''get it from respective server.

        ''Enable SSL
        'mailClient.EnableSsl = True

        'mailClient.UseDefaultCredentials = False

        'mailClient.Credentials = mailAuthentication

        'mailClient.Send(MyMailMessage)
    End Sub

    Private Sub ThreadSendMail(ByVal args() As Object)
        Dim mailClient As System.Net.Mail.SmtpClient = args(0)
        SyncLock mailClient
            Dim sFromEmail As String = args(1)
            Dim sPassword As String = args(2)
            Dim sFromName As String = args(3)
            Dim sToName As String = args(4)
            Dim sToEmail As String = args(5)
            Dim sHeader As String = args(6)
            Dim sMessage As String = args(7)

            'Create Mail Message Object with content that you want to send with mail.
            Dim MyMailMessage As New System.Net.Mail.MailMessage(sFromEmail, sToEmail, sHeader, sMessage)

            MyMailMessage.IsBodyHtml = False

            'Proper Authentication Details need to be passed when sending email from gmail
            Dim mailAuthentication As New System.Net.NetworkCredential(sFromEmail, sPassword)

            'Smtp Mail server of Gmail is "smpt.gmail.com" and it uses port no. 587
            'For different server like yahoo this details changes and you can
            'get it from respective server.

            'Enable SSL
            mailClient.EnableSsl = True

            mailClient.UseDefaultCredentials = False

            mailClient.Credentials = mailAuthentication

            mailClient.Send(MyMailMessage)
        End SyncLock
    End Sub

    Public Function CheckIsMembered(ByVal email As String) As Boolean
        Return mMemberNoTable.Contains(email.Trim)
    End Function
#End Region

#Region "訂閱信"
    Private mHtmlTemplate As String = _
      "<table align=""center"" bgcolor=""#ffffff"" cellpadding=""0"" cellspacing=""0"" width=""650"" style="" font-family: Arial, Helvetica, sans-serif;"">" + _
      "<tr>" + _
      "    <td bgcolor=""#ff6633"" width=""650"" style=""padding:3px; border:1px solid #ff3300;" + _
      "        color:#fff; font-weight:bold; text-align:left;"">" + _
      "        <span>&nbsp;Wantgoo 玩股網</span>" + _
      "    </td>" + _
      "</tr>" + _
      "<tr>" + _
      "    <td width=""650"" style=""padding:3px; border:1px solid #fcce7c; border-top:none; text-align:left; line-height:22px;"">" + _
      "        <div style=""padding:5px;"">" + _
      "            <span style=""font-size:12px;"">親愛的 忠實讀者 您好， </span><br />" + _
      "        </div>" + _
      "        <div style=""font-size:12px; color:Black; padding:16px; text-align:left; "">" + _
      "@Content" + _
      "            <br /><br /><a href=""http://www.wantgoo.com/personalpage/subscribeblog.aspx"">若您不想再收到通知信，請到此取消訂閱。</a>" + _
      "         </div>" + _
      "    </td>" + _
      "</tr>" + _
      "</table>"

    Public Sub SendSubcribeMail(Optional ByVal memberNo As String = Nothing)
        Action = "SendSubcribeMail"

        If memberNo Is Nothing Then memberNo = GetMemberNo()

        Dim t3 As New Threading.Thread(AddressOf SendSubcribeForwardMailThread)
        t3.Start(memberNo)

        Dim t4 As New Threading.Thread(AddressOf SendSubcribeReverseMailThread)
        t4.Start(memberNo)

        Dim t1 As New Threading.Thread(AddressOf SendSubcribeMailThread1)
        t1.Start(memberNo)

        Dim t2 As New Threading.Thread(AddressOf SendSubcribeMailThread2)
        t2.Start(memberNo)

    End Sub

    Private Sub SendSubcribeMailThread1(ByVal memberNo As String)

        Dim daEmails As New WantGooTableAdapters.SubscribeTableAdapter
        Dim daBlog As New WantGooTableAdapters.BlogTableAdapter

        Dim blogRow As WantGoo.BlogRow = daBlog.GetData(memberNo).Rows(0)

        Dim articleText As String = "..."

        Dim contextText As String = ""
        If blogRow.IsSell = False Then
            contextText = "<br />" + Left(HtmlCleaner.Instance.RemoveHtmlTag(System.Web.HttpUtility.HtmlDecode(blogRow.ArticleText)), 200) + "..." + "<br /><br />"
        End If

        Dim nickName As String = GetMemberNickName(memberNo)
        Dim subject As String = nickName + " 有新文章了－" + Left(blogRow.ArticleTitle, 10) + articleText
        Dim content As String = _
                "<a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + """>" + nickName + "</a>" + " 在玩股網發佈了最新的文章：<br />" + _
                "<div align=""center"" style=""font-weight: bold""><a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + """>『" + blogRow.ArticleTitle + "』</a></div><br />" + _
                contextText + _
                "http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "謝謝，<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)

        Dim i As Integer
        Dim emails As DataRowCollection = daEmails.GetData(memberNo).Rows
        For Each row As WantGoo.SubscribeRow In emails
            If row.Email.ToLower.Contains("yahoo") OrElse _
           row.Email.ToLower.Contains("hinet") OrElse _
           row.Email.ToLower.Contains("pchome") Then
            Else
                MailServiceNew.Instance.SendHtmlMail(row.Email, "", subject, content)
            End If
            i = i + 1
            If i > emails.Count / 2 Then Exit Sub
        Next

    End Sub

    Private Sub SendSubcribeMailThread2(ByVal memberNo As String)

        Dim daEmails As New WantGooTableAdapters.SubscribeTableAdapter
        Dim daBlog As New WantGooTableAdapters.BlogTableAdapter

        Dim blogRow As WantGoo.BlogRow = daBlog.GetData(memberNo).Rows(0)

        Dim articleText As String = "..."

        Dim contextText As String = ""
        If blogRow.IsSell = False Then
            contextText = "<br />" + Left(HtmlCleaner.Instance.RemoveHtmlTag(System.Web.HttpUtility.HtmlDecode(blogRow.ArticleText)), 200) + "..." + "<br /><br />"
        End If

        Dim nickName As String = GetMemberNickName(memberNo)
        Dim subject As String = nickName + " 有新文章了－" + Left(blogRow.ArticleTitle, 10) + articleText
        Dim content As String = _
                "<a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + """>" + nickName + "</a>" + " 在玩股網發佈了最新的文章：<br />" + _
                "<div align=""center"" style=""font-weight: bold""><a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + """>『" + blogRow.ArticleTitle + "』</a></div><br />" + _
                contextText + _
                "http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "謝謝，<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)

        Dim emails As New Generic.List(Of String)
        For Each row As WantGoo.SubscribeRow In daEmails.GetData(memberNo)
            emails.Add(row.Email)
        Next
        emails.Reverse()

        Dim i As Integer
        For Each email As String In emails
            If email.ToLower.Contains("yahoo") OrElse _
            email.ToLower.Contains("hinet") OrElse _
            email.ToLower.Contains("pchome") Then
            Else
                MailServiceNew.Instance.SendHtmlMail(email, "", subject, content.Replace("@EMail", email))
            End If
            i = i + 1
            If i > emails.Count / 2 Then Exit Sub
        Next
    End Sub

    Private Sub SendSubcribeForwardMailThread(ByVal memberNo As String)

        Dim daEmails As New WantGooTableAdapters.SubscribeTableAdapter
        Dim daBlog As New WantGooTableAdapters.BlogTableAdapter

        Dim blogRow As WantGoo.BlogRow = daBlog.GetData(memberNo).Rows(0)

        Dim articleText As String = "..."

        Dim contextText As String = ""
        If blogRow.IsSell = False Then
            contextText = "<br />" + Left(HtmlCleaner.Instance.RemoveHtmlTag(System.Web.HttpUtility.HtmlDecode(blogRow.ArticleText)), 200) + "..." + "<br /><br />"
        End If

        Dim nickName As String = GetMemberNickName(memberNo)
        Dim subject As String = nickName + " 有新文章了－" + Left(blogRow.ArticleTitle, 10) + articleText
        Dim content As String = _
                "<a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + """>" + nickName + "</a>" + " 在玩股網發佈了最新的文章：<br />" + _
                "<div align=""center"" style=""font-weight: bold""><a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + """>『" + blogRow.ArticleTitle + "』</a></div><br />" + _
                contextText + _
                "http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "謝謝，<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)
        Dim i As Integer
        Dim emails As DataRowCollection = daEmails.GetData(memberNo).Rows
        For Each row As WantGoo.SubscribeRow In emails
            If row.Email.ToLower.Contains("yahoo") OrElse _
            row.Email.ToLower.Contains("hinet") OrElse _
            row.Email.ToLower.Contains("pchome") Then
                MailServiceNew.Instance.SendHtmlMail(row.Email, "", subject, content)
            End If
            i = i + 1
            If i > emails.Count / 2 Then Exit Sub
        Next
    End Sub

    Private Sub SendSubcribeReverseMailThread(ByVal memberNo As String)

        Dim daEmails As New WantGooTableAdapters.SubscribeTableAdapter
        Dim daBlog As New WantGooTableAdapters.BlogTableAdapter

        Dim blogRow As WantGoo.BlogRow = daBlog.GetData(memberNo).Rows(0)

        Dim articleText As String = "..."

        Dim contextText As String = ""
        If blogRow.IsSell = False Then
            contextText = "<br />" + Left(HtmlCleaner.Instance.RemoveHtmlTag(System.Web.HttpUtility.HtmlDecode(blogRow.ArticleText)), 200) + "..." + "<br /><br />"
        End If

        Dim nickName As String = GetMemberNickName(memberNo)
        Dim subject As String = nickName + " 有新文章了－" + Left(blogRow.ArticleTitle, 10) + articleText
        Dim content As String = _
                "<a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + """>" + nickName + "</a>" + " 在玩股網發佈了最新的文章：<br />" + _
                "<div align=""center"" style=""font-weight: bold""><a href=""http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + """>『" + blogRow.ArticleTitle + "』</a></div><br />" + _
                contextText + _
                "http://www.wantgoo.com/" + GetBlogName(memberNo) + "/" + blogRow.ArticleID.ToString + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "謝謝，<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)

        Dim emails As New Generic.List(Of String)
        For Each row As WantGoo.SubscribeRow In daEmails.GetData(memberNo)
            emails.Add(row.Email)
        Next
        emails.Reverse()

        Dim i As Integer
        For Each email As String In emails
            If email.ToLower.Contains("yahoo") OrElse _
            email.ToLower.Contains("hinet") OrElse _
            email.ToLower.Contains("pchome") Then
                MailServiceNew.Instance.SendHtmlMail(email, "", subject, content.Replace("@EMail", email))
            End If
            i = i + 1
            If i > emails.Count / 2 Then Exit Sub
        Next
    End Sub

    ''' <summary>
    ''' 訂閱成功通知信
    ''' </summary>
    Public Sub SendSubcribeSuccess(ByVal mail As String, ByVal memberNo As String)
        Dim thread As New Threading.Thread(AddressOf SendSubcribeSuccessThread)
        Dim params As New Generic.List(Of String)
        params.Add(mail)
        params.Add(memberNo)
        params.Add(Me.GetHotArticle(memberNo))

        '新文章的數目與文章連結
        Dim lists() As String = Me.GetNewArticle(memberNo)
        If lists Is Nothing OrElse lists.Length <> 2 Then
            Dim tmps(1) As String
            lists = tmps
            lists(0) = "0" '數目
            lists(1) = "" '連結
        End If
        params.Add(lists(0))
        params.Add(lists(1))

        thread.Start(params)
    End Sub

    Private Sub SendSubcribeSuccessThread(ByVal params As Generic.List(Of String))
        Dim mail As String = params(0)
        Dim memberNo As String = params(1)
        Dim hotArticelList As String = params(2)
        Dim newArticleCount As String = params(3)
        Dim newArticleText As String = params(4)
        Dim nickname As String = MemberDataAccessor.Instance.GetMemberNickName(memberNo)
        Dim blogLink As String = "http://www.wantgoo.com/" + MemberDataAccessor.Instance.GetBlogName(memberNo)

        Dim newArticleTitle As String = "作者目前尚未有文章，敬請期待新文章。"
        If Val(newArticleCount) > 0 Then
            newArticleTitle = "最新" & newArticleCount & "篇文章，別再錯過了！"
        End If

        Dim subject As String = "恭喜您成功訂閱了「" + nickname + "」電子報，內附" + nickname + "最熱門文章"
        Dim content As String = _
                "您己成功訂閱「" + nickname + "」的電子報" + _
                "<br />只要" + nickname + "有新文章就會立即送信通知您，讓您零時差看到最新的文章。" + _
                "<br />如果您喜歡" + nickname + "的文章，我想您的親朋好友也會喜歡，介紹給他們吧！" + _
                "<br /><a href=""" & blogLink & """>" & blogLink & "</a>" & "<br/>" + _
                "<div style=""margin-bottom:20px;""></div><span style=""font-size:14px;font-weight :bold;"">" & newArticleTitle & "</span><br />" + newArticleText + _
                "<div style=""margin-bottom:20px;""></div><span style=""font-size:14px;font-weight :bold;"">本月最受讀者歡迎的文章:</span><br />" + hotArticelList + _
                "<br /><br />有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "玩股網團隊<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)
        MailServiceNew.Instance.SendHtmlMail(mail, "", subject, content)

        'Me.Literal1.Text = content
        ''If mail.ToLower.EndsWith("yahoo.com") Then
        ''    MailServiceNew.Instance.SendHtmlMail(mail + ".tw", nickname, subject, content)
        ''ElseIf mail.ToLower.EndsWith("yahoo.com.tw") Then
        ''    MailServiceNew.Instance.SendHtmlMail(mail.ToLower.Replace("@yahoo.com.tw", "@yahoo.com"), nickname, subject, content)
        ''End If
    End Sub

    ''' <summary>
    ''' 作者本月最熱文章
    ''' </summary>
    Private Function GetHotArticle(ByVal memberNo As String) As String
        Dim cacheKey As String = "EMailSubcribeSuccess_HotArticle_" & memberNo

        If HttpContext.Current.Cache(cacheKey) Is Nothing Then
            Dim sds As New SqlDataSource
            sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString

            sds.SelectCommand = "SELECT TOP(10) Blog.ViewCount as VC, Blog.ReCommendCount as RC, Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,30) as ArticleTitle , Blog.ArticleText, Blog.PublishTime, Blog.ViewCount, Blog.RecommendCount, Blog.Show, substring(Member.NickName,1,5) as NickName FROM Blog INNER JOIN Member ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND DateDiff(Month, Blog.PublishTime,GETDATE())=0  And (Member.IshideBlog <> 1)  AND Member.MemberNo = @MemberNo ORDER BY Blog.ViewCount DESC "
            sds.SelectCommand = sds.SelectCommand.Replace("@MemberNo", memberNo)

            Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
            Dim index As Integer = 1
            Dim articelList As String = ""
            While en.MoveNext
                Dim articleHref As String = "http://www.wantgoo.com/" & en.Current("MN").ToString & "/" & en.Current("AID").ToString
                articelList &= index.ToString & ". " & "<a href=""" & articleHref & """>" & en.Current("ArticleTitle") & "</a>" & "<br/>"
                index += 1
            End While
            HttpContext.Current.Cache.Add(cacheKey, articelList, Nothing, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If

        Return HttpContext.Current.Cache(cacheKey)
    End Function

    ''' <summary>
    ''' 作者最新文章
    ''' </summary>
    Private Function GetNewArticle(ByVal memberNo As String) As String()
        Dim cacheKey As String = "EMailSubcribeSuccess_NewArticle_" & memberNo

        If HttpContext.Current.Cache(cacheKey) Is Nothing Then
            Dim sds As New SqlDataSource
            sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString
            sds.SelectCommand = " SELECT TOP(10) Blog.ViewCount as VC, Blog.ReCommendCount as RC, Blog.BlogID, Blog.MemberNo as MN, Blog.ArticleID as AID, substring(Blog.ArticleTitle,1,30) as ArticleTitle , Blog.ArticleText, Blog.PublishTime, Blog.ViewCount, Blog.RecommendCount, Blog.Show, substring(Member.NickName,1,5) as NickName FROM Blog INNER JOIN Member ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0)  And (Member.IshideBlog <> 1)  AND Member.MemberNo = @MemberNo ORDER BY Blog.PublishTime DESC "
            sds.SelectCommand = sds.SelectCommand.Replace("@MemberNo", memberNo)

            Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
            Dim index As Integer = 0
            Dim articelList As String = ""
            While en.MoveNext
                index += 1
                Dim articleHref As String = "http://www.wantgoo.com/" & en.Current("MN").ToString & "/" & en.Current("AID").ToString
                articelList &= index.ToString & ". " & "<a href=""" & articleHref & """>" & en.Current("ArticleTitle") & "</a>" & "<br/>"
            End While
            Dim lists(1) As String
            lists(0) = index
            lists(1) = articelList
            HttpContext.Current.Cache.Add(cacheKey, lists, Nothing, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If
        Return HttpContext.Current.Cache(cacheKey)
    End Function
#End Region

    Public Sub SendReplyMail(ByVal authNo As String, ByVal readNo As String, _
                         ByVal authIsReply As Boolean, ByVal focusNos As ArrayList, _
                         ByVal blogTitle As String, ByVal articleID As String)

        Dim authorMail As String = GetUserName(authNo)
        Dim authorNickName As String = MemberDataAccessor.Instance.GetMemberNickName(authNo)

        Dim readerNickName As String = "玩股會員"
        If readNo <> "0" AndAlso readNo <> "" Then
            readerNickName = MemberDataAccessor.Instance.GetMemberNickName(readNo)
        End If

        If (authNo <> readNo) OrElse (authNo = "736" AndAlso readNo = "736") Then
            Dim t1 As New Threading.Thread(AddressOf SendSubcribeMailThreadByAuthReply)
            t1.Start(New Object() {authNo, readNo, blogTitle, articleID})
        End If

        For Each member As Object In focusNos
            If (authNo <> member) OrElse (authNo = "736" AndAlso member = "736") Then
                Dim t1 As New Threading.Thread(AddressOf SendSubcribeMailThreadByFocusReply)
                t1.Start(New Object() {authNo, member.ToString, blogTitle, articleID})
            End If
        Next
    End Sub

#Region "通知作者有人回應"
    Private Sub SendSubcribeMailThreadByAuthReply(ByVal objs As Object)
        Dim authNo As String = objs(0)
        Dim readNo As String = objs(1)
        Dim blogTitle As String = objs(2)
        Dim articleID As String = objs(3)

        Dim authNick As String = GetMemberNickName(authNo)
        Dim readNick As String = GetMemberNickName(readNo)

        Dim daEmails As New WantGooTableAdapters.SubscribeTableAdapter

        Dim subject As String = "您的文章－" + Left(blogTitle, 10) + "..." + "有新回應"
        Dim content As String = _
                "<a href=""http://www.wantgoo.com/" + MemberDataAccessor.Instance.GetBlogName(readNo) + "/profile"">" + readNick + "</a>" + "在您的文章[" + blogTitle + "]上有新回應, 快來看看他說了什麼：<br />" + _
                "<div align=""center"" style=""font-weight: bold""><a href=""http://www.wantgoo.com/" + GetBlogName(authNo) + "/" + articleID + """>『" + blogTitle + "』</a></div><br />" + _
                "http://www.wantgoo.com/" + GetBlogName(authNo) + "/" + articleID + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "謝謝，<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)
        content = content.Replace("忠實讀者", authNick)

        Dim authMail As String = GetUserName(authNo)
        MailServiceNew.Instance.SendHtmlMail(authMail, "", subject, content.Replace("@EMail", authMail))
    End Sub
#End Region

#Region "通知追蹤者有人回應"
    Private Sub SendSubcribeMailThreadByFocusReply(ByVal objs As Object)
        Dim authNo As String = objs(0)
        Dim focusNo As String = objs(1)
        Dim blogTitle As String = objs(2)
        Dim articleID As String = objs(3)

        Dim authNick As String = GetMemberNickName(authNo)
        Dim focusNick As String = GetMemberNickName(focusNo)

        Dim daEmails As New WantGooTableAdapters.SubscribeTableAdapter

        Dim subject As String = authNick + "的文章－" + Left(blogTitle, 10) + "..." + "有新回應"
        Dim content As String = _
                "<a href=""http://www.wantgoo.com/" + MemberDataAccessor.Instance.GetBlogName(authNo) + "/profile"">" + authNick + "</a>" + "的文章[" + blogTitle + "]上有新回應, 快來看看說了什麼：<br />" + _
                "<div align=""center"" style=""font-weight: bold""><a href=""http://www.wantgoo.com/" + MemberDataAccessor.Instance.GetBlogName(authNo) + "/" + articleID + """>『" + blogTitle + "』</a></div><br />" + _
                "http://www.wantgoo.com/" + MemberDataAccessor.Instance.GetBlogName(authNo) + "/" + articleID + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                "謝謝，<br />" + _
                "Wantgoo Team<br /><br />" + _
                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"

        content = mHtmlTemplate.Replace("@Content", content)
        content = content.Replace("忠實讀者", focusNick)

        Dim focusMail As String = GetUserName(focusNo)
        MailServiceNew.Instance.SendHtmlMail(focusMail, "", subject, content.Replace("@EMail", focusMail))
    End Sub
#End Region

#Region "站內信未讀"
    ''' <summary>
    ''' 站內信未讀取數目
    ''' </summary>
    ReadOnly Property NonReadMailCount(ByVal memberNo As String) As Integer
        Get
            If HttpContext.Current.Application("NonReadMailCount_" & memberNo) Is Nothing Then
                HttpContext.Current.Application("NonReadMailCount_" & memberNo) = Me.LoadNonReadMailCount(memberNo)
            End If
            Return Val(HttpContext.Current.Application("NonReadMailCount_" & memberNo))
        End Get
    End Property

    ''' <summary>
    ''' 清除站內信未讀取數目的記憶體
    ''' </summary>
    Public Sub ClearNonReadMailCount(ByVal memberNo As String)
        HttpContext.Current.Application("NonReadMailCount_" & memberNo) = Nothing
        HttpContext.Current.Application.Remove("NonReadMailCount_" & memberNo)
    End Sub

    ''' <summary>
    ''' 站內信未讀取數目
    ''' </summary>
    Private Function LoadNonReadMailCount(ByVal memberNo As String) As Integer
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString
        sds.SelectCommand = "SELECT count([MailID]) as MailCount FROM [Mail] WHERE (([ToMemberNo] = " + memberNo + ") AND ([Readed] = 'False')) " + Me.GetBackMemberString
        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        Dim mailCount As Integer = 0
        If en.MoveNext Then
            mailCount = Val(en.Current("MailCount"))
        End If

        Return mailCount
    End Function

    Private Function GetBackMemberString() As String
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Dim content As String = ""

        Try
            Dim cmd As String = "select backmemberno from backmember where memberno=" + MemberDataAccessor.Instance.GetMemberNo
            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)


            Dim reader As System.Data.SqlClient.SqlDataReader = command.ExecuteReader

            While reader.Read
                content += " and Mail.MemberNo <> " + reader.Item("backmemberno").ToString
            End While

            reader.Close()
        Catch ex As Exception
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        Return content
    End Function
#End Region

End Class

Public Enum AccountType
    Money = 0
    StockAccount = 1
    Gold = 2
End Enum

Public Enum MoneyType
    Salary = 0
    Casino = 1
    StoreValue = 2
    Reference = 3
End Enum

Public Enum GoldType
    BuyArticle = 0
    SellArticle = 1
    StoreValue = 2
    ChangeNT = 3
    Donate = 4
    Sms = 5
    Action = 6
    JoinAction = 7
    BuyRich = 10
    BuyMoney = 11
    Gambling = 12
    ClubFee = 13
End Enum

Public Enum MemberLevel
    Normal = 0
    WellOff = 1
    Rich = 2
    Super = 3
    Developer = 4
End Enum


