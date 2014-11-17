Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Public Class Club
    Private Shared mObj As New Club
    Public Shared ReadOnly Property Instance() As Club
        Get
            Return mObj
        End Get
    End Property

    Private mJoinDate As New Generic.Dictionary(Of String, Generic.Dictionary(Of String, DateTime))
    Private mDeadline As New Generic.Dictionary(Of String, Generic.Dictionary(Of String, DateTime))
    Private mStopDeadline As New Generic.Dictionary(Of String, Generic.Dictionary(Of String, DateTime))
    Private mStatus As New Generic.Dictionary(Of String, Generic.Dictionary(Of String, ClubStatus))
    Private mChairman As New Generic.Dictionary(Of String, Generic.List(Of String))
    Private mMembers As New Generic.Dictionary(Of String, Generic.List(Of String))
    Private mMemberCount As New Generic.Dictionary(Of String, Integer)
    Private mName As New Generic.Dictionary(Of String, String)
    Private mEstablishTime As New Generic.Dictionary(Of String, DateTime)
    Private mTopicCount As New Generic.Dictionary(Of String, Integer)
    Private mDetailCount As New Generic.Dictionary(Of String, Integer)
    Private mHideTopicCount As New Generic.Dictionary(Of String, Integer)

    Private mClubConnectString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("ClubConnection").ConnectionString

    ''' <summary>
    ''' 加入會員資料
    ''' </summary>
    Public Sub AddClubMember(ByVal clubID As String, ByVal memberNo As String, ByVal joinDate As DateTime, ByVal deadline As DateTime, ByVal stopDeadline As DateTime, ByVal status As ClubStatus)
        If memberNo <= 0 Then Exit Sub
        If Not mStatus.ContainsKey(clubID) Then
            mStatus.Add(clubID, New Generic.Dictionary(Of String, ClubStatus))
            mDeadline.Add(clubID, New Generic.Dictionary(Of String, DateTime))
            mStopDeadline.Add(clubID, New Generic.Dictionary(Of String, DateTime))
            mJoinDate.Add(clubID, New Generic.Dictionary(Of String, DateTime))
            mChairman.Add(clubID, New Generic.List(Of String))
            mMembers.Add(clubID, New Generic.List(Of String))
        End If

        If mStatus(clubID).ContainsKey(memberNo) Then
            mStatus(clubID)(memberNo) = status
            mDeadline(clubID)(memberNo) = deadline
            mStopDeadline(clubID)(memberNo) = stopDeadline
            mJoinDate(clubID)(memberNo) = joinDate
        Else
            mStatus(clubID).Add(memberNo, status)
            mDeadline(clubID).Add(memberNo, deadline)
            mStopDeadline(clubID).Add(memberNo, stopDeadline)
            mJoinDate(clubID).Add(memberNo, joinDate)
        End If

        If status = ClubStatus.Chairman Then
            If Not mChairman(clubID).Contains(memberNo) Then
                mChairman(clubID).Add(memberNo)
            End If
        ElseIf Not mMembers(clubID).Contains(memberNo) Then
            mMembers(clubID).Add(memberNo)
        End If
    End Sub

    ''' <summary>
    ''' 加入社團資料
    ''' </summary>
    Public Sub AddClub(ByVal clubID As String, ByVal Name As String, ByVal establishTime As DateTime, ByVal topicCount As Integer, ByVal detailCount As Integer, ByVal hideTopicCount As Integer)
        If Not mName.ContainsKey(clubID) Then
            mName.Add(clubID, Name)
            mEstablishTime.Add(clubID, establishTime)
            mTopicCount.Add(clubID, topicCount)
            mDetailCount.Add(clubID, detailCount)
            mHideTopicCount.Add(clubID, hideTopicCount)
        Else
            mName(clubID) = Name
            mEstablishTime(clubID) = establishTime
            mTopicCount(clubID) = topicCount
            mDetailCount(clubID) = detailCount
            mHideTopicCount(clubID) = hideTopicCount
        End If
    End Sub

    ''' <summary>
    ''' 取得社團名稱
    ''' </summary>
    Public Function GetClubName(ByVal clubID As String) As String
        If mName.Count = 0 Then LoadClubData()

        If mName.ContainsKey(clubID) Then
            Return mName(clubID)
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' 取得社團建立時間
    ''' </summary>
    Public Function GetClubEstablishTime(ByVal clubID As String) As DateTime
        If mEstablishTime.Count = 0 Then LoadClubData()

        If mEstablishTime.ContainsKey(clubID) Then
            Return mEstablishTime(clubID)
        Else
            Return Now
        End If
    End Function

    ''' <summary>
    ''' 取得會員等級
    ''' </summary>
    Public Function GetStatus(ByVal clubID As String, ByVal memberNo As String) As ClubStatus
        If mStatus.Count = 0 OrElse mDeadline.Count = 0 OrElse mJoinDate.Count = 0 Then
            LoadClubMembership()
        End If

        If MemberDataAccessor.Instance.IsAdiminstrator Then Return ClubStatus.Administrator
        If Not mStatus.ContainsKey(clubID) Then Return ClubStatus.None
        If mStatus(clubID).ContainsKey(memberNo) Then Return mStatus(clubID)(memberNo)

        Return ClubStatus.None
    End Function

    ''' <summary>
    ''' 取得社團會員會期
    ''' </summary>
    Public Function GetDeadline(ByVal clubID As String, ByVal memberNo As String) As DateTime
        If mStatus.Count = 0 OrElse mDeadline.Count = 0 OrElse mJoinDate.Count = 0 Then
            LoadClubMembership()
        End If

        If mDeadline(clubID).ContainsKey(memberNo) Then Return mDeadline(clubID)(memberNo)

        Return Now
    End Function

    ''' <summary>
    ''' 取得社團會員會期
    ''' </summary>
    Public Function GetStopDeadline(ByVal clubID As String, ByVal memberNo As String) As DateTime
        If mStatus.Count = 0 OrElse mDeadline.Count = 0 OrElse mJoinDate.Count = 0 Then
            LoadClubMembership()
        End If

        If mStopDeadline(clubID).ContainsKey(memberNo) Then Return mStopDeadline(clubID)(memberNo)

        Return Now
    End Function

    ''' <summary>
    ''' 取得社團會員加入時間
    ''' </summary>
    Public Function GetJoinDate(ByVal clubID As String, ByVal memberNo As String) As DateTime
        If mStatus.Count = 0 OrElse mDeadline.Count = 0 OrElse mJoinDate.Count = 0 Then
            LoadClubMembership()
        End If

        If mJoinDate(clubID).ContainsKey(memberNo) Then Return mJoinDate(clubID)(memberNo)

        Return Now
    End Function

    ''' <summary>
    ''' 取得社團主席名單
    ''' </summary>
    Public Function GetChairman(ByVal clubID As String) As Generic.List(Of String)
        If mChairman.Count = 0 Then LoadClubMembership()
        If Not mChairman.ContainsKey(clubID) Then
            Dim chairmans As New Generic.List(Of String)
            chairmans.Add(0)
            Return chairmans
        End If

        Return mChairman(clubID)
    End Function

    ''' <summary>
    ''' 會員是否為社團主席
    ''' </summary>
    Public Function IsChairman(ByVal memberNo As String) As Integer
        If mChairman.Count = 0 Then LoadClubMembership()
        For Each clubID As String In mChairman.Keys
            If mChairman(clubID).Contains(memberNo) Then Return Val(clubID)
        Next
        Return -1
    End Function

    ''' <summary>
    ''' 取得社團成員名單
    ''' </summary>
    Public Function GetMembers(ByVal clubID As String) As Generic.List(Of String)
        If mMembers.Count = 0 Then LoadClubMembership()
        If Not mMembers.ContainsKey(clubID) Then Return New Generic.List(Of String)
        Return mMembers(clubID)
    End Function

    ''' <summary>
    ''' 取得社團VIP成員數
    ''' </summary>
    Public Function GetMemberCount(ByVal clubID As String) As Integer
        If Not mMemberCount Is Nothing Then
            If mMemberCount.ContainsKey(clubID) Then
                Return mMemberCount(clubID)
            Else
                Dim c As Integer
                If Not mStatus Is Nothing Then
                    Try
                        For Each status As ClubStatus In mStatus(clubID).Values
                            If status = ClubStatus.Normal OrElse status = ClubStatus.Stop Then
                                c = c + 1
                            End If
                        Next
                        Return c
                    Catch
                    End Try

                End If
            End If
        End If
    End Function

    ''' <summary>
    ''' 取得討論主題數目
    ''' </summary>
    Public Function GetTopicCount(ByVal clubID As String) As Integer
        If mTopicCount.Count = 0 Then LoadClubData()

        If mTopicCount.ContainsKey(clubID) Then
            Return mTopicCount(clubID)
        Else
            Return 0
        End If
    End Function
    Public Sub SetTopicCount(ByVal clubID As String, ByVal count As Integer)
        If mTopicCount.ContainsKey(clubID) Then
            mTopicCount(clubID) = count
        Else
            mTopicCount.Add(clubID, count)
        End If
    End Sub
    Public Sub Plus1toTopicCount(ByVal clubID As String)
        If mTopicCount.ContainsKey(clubID) Then
            mTopicCount(clubID) = mTopicCount(clubID) + 1
        Else
            mTopicCount.Add(clubID, 1)
        End If
    End Sub
    Public Sub Minus1toTopicCount(ByVal clubID As String)
        If mTopicCount.ContainsKey(clubID) Then
            mTopicCount(clubID) = mTopicCount(clubID) - 1
        Else
            mTopicCount.Add(clubID, 0)
        End If
    End Sub

    ''' <summary>
    ''' 取得隱藏主題數目
    ''' </summary>
    Public Function GetHideTopicCount(ByVal clubID As String) As Integer
        If mHideTopicCount.Count = 0 Then LoadClubData()

        If mHideTopicCount.ContainsKey(clubID) Then
            Return mHideTopicCount(clubID)
        Else
            Return 0
        End If
    End Function
    Public Sub SetHideTopicCount(ByVal clubID As String, ByVal count As Integer)
        If mHideTopicCount.ContainsKey(clubID) Then
            mHideTopicCount(clubID) = count
        Else
            mHideTopicCount.Add(clubID, count)
        End If
    End Sub
    Public Sub Plus1toHideTopicCount(ByVal clubID As String)
        If mHideTopicCount.ContainsKey(clubID) Then
            mHideTopicCount(clubID) = mTopicCount(clubID) + 1
        Else
            mHideTopicCount.Add(clubID, 1)
        End If
    End Sub
    Public Sub Minus1toHideTopicCount(ByVal clubID As String)
        If mHideTopicCount.ContainsKey(clubID) Then
            mHideTopicCount(clubID) = mTopicCount(clubID) - 1
        Else
            mHideTopicCount.Add(clubID, 0)
        End If
    End Sub

    ''' <summary>
    ''' 取得討論數目
    ''' </summary>
    Public Function GetDetailCount(ByVal clubID As String) As Integer
        If mDetailCount.Count = 0 Then LoadClubData()

        If mDetailCount.ContainsKey(clubID) Then
            Return mDetailCount(clubID)
        Else
            Return 0
        End If
    End Function
    Public Sub SetDetailCount(ByVal clubID As String, ByVal count As Integer)
        If mDetailCount.ContainsKey(clubID) Then
            mDetailCount(clubID) = count
        Else
            mDetailCount.Add(clubID, count)
        End If
    End Sub
    Public Sub Plus1toDetailCount(ByVal clubID As String)
        If mDetailCount.ContainsKey(clubID) Then
            mDetailCount(clubID) = mTopicCount(clubID) + 1
        Else
            mDetailCount.Add(clubID, 1)
        End If
    End Sub
    Public Sub Minus1toDetailCount(ByVal clubID As String)
        If mDetailCount.ContainsKey(clubID) Then
            mDetailCount(clubID) = mTopicCount(clubID) - 1
        Else
            mDetailCount.Add(clubID, 0)
        End If
    End Sub

    Private mIsLoadingMembership As Boolean = False

    ''' <summary>
    ''' 載入社團會員資料
    ''' </summary>
    Public Sub LoadClubMembership(Optional ByVal memberNo As String = Nothing)

        If memberNo Is Nothing OrElse Val(memberNo) <= 0 Then

            '避免載入時資源衝突，使用旗標來判斷是否正在載入
            If mIsLoadingMembership Then Exit Sub
            mIsLoadingMembership = True

            mMembers.Clear()
            mStopDeadline.Clear()
            mDeadline.Clear()
            mJoinDate.Clear()
            mStatus.Clear()
            mChairman.Clear()

            Dim sdsAllClubMember As New SqlDataSource
            sdsAllClubMember.ConnectionString = mClubConnectString
            sdsAllClubMember.SelectCommand = "SELECT MemberNo,ClubID ,StopDeadline,Deadline,Status,JoinDate FROM ClubMember"

            Dim en As IEnumerator = sdsAllClubMember.Select(New DataSourceSelectArguments).GetEnumerator
            Dim status As ClubStatus
            Dim joinDate As DateTime
            Dim deadline As DateTime
            Dim stopDeadline As DateTime

            While en.MoveNext
                joinDate = en.Current("JoinDate")
                deadline = en.Current("Deadline")
                stopDeadline = en.Current("StopDeadline")

                If en.Current("Status").ToString.Trim = "Chairman" Then
                    status = ClubStatus.Chairman
                ElseIf stopDeadline > Now Then
                    status = ClubStatus.Stop
                ElseIf deadline > Now Then
                    status = ClubStatus.Normal
                ElseIf deadline <= Now Then
                    status = ClubStatus.Expired
                Else
                    status = ClubStatus.None
                End If

                AddClubMember(en.Current("ClubID"), en.Current("MemberNo"), joinDate, deadline, stopDeadline, status)

            End While

            mIsLoadingMembership = False

        Else
            Dim sdsClubMember As New SqlDataSource
            sdsClubMember.ConnectionString = mClubConnectString
            Dim sql As String = "SELECT MemberNo,ClubID,StopDeadline ,Deadline,Status,JoinDate FROM ClubMember Where MemberNo = @MemberNo"
            sdsClubMember.SelectCommand = sql.Replace("@MemberNo", memberNo)

            Dim en As IEnumerator = sdsClubMember.Select(New DataSourceSelectArguments).GetEnumerator
            Dim status As ClubStatus
            Dim joinDate As DateTime
            Dim deadline As DateTime
            Dim stopDeadline As DateTime

            While en.MoveNext
                joinDate = en.Current("JoinDate")
                deadline = en.Current("Deadline")
                stopDeadline = en.Current("StopDeadline")
                If en.Current("Status") = "Chairman" Then
                    status = ClubStatus.Chairman
                ElseIf stopDeadline > Now Then
                    status = ClubStatus.Stop
                ElseIf deadline > Now Then
                    status = ClubStatus.Normal
                ElseIf deadline <= Now Then
                    status = ClubStatus.Expired
                Else
                    status = ClubStatus.None
                End If

                AddClubMember(en.Current("ClubID"), en.Current("MemberNo"), joinDate, deadline, stopDeadline, status)
            End While

        End If

    End Sub

    ''' <summary>
    ''' 載入社團資料
    ''' </summary>
    Public Sub LoadClubData()

        '避免載入時資源衝突，使用旗標來判斷是否正在載入
        With HttpContext.Current
            If .Application("IsLoadingClubData") Is Nothing Then .Application("IsLoadingClubData") = False

            If .Application("IsLoadingClubData") Then
                System.Threading.Thread.Sleep(1000)
                .Response.Redirect(.Request.Url.ToString)
                Exit Sub
            End If
            .Application("IsLoadingClubData") = True
        End With

        mName.Clear()
        mEstablishTime.Clear()
        mTopicCount.Clear()
        mDetailCount.Clear()
        mHideTopicCount.Clear()

        Dim sdsClub As New SqlDataSource
        sdsClub.ConnectionString = mClubConnectString
        sdsClub.SelectCommand = "SELECT [ClubID] ,[Name],[Intro],[ChairmanNo],[TopicCount] ,[DetailCount] ,[TopicCountHide],[MemberCount] ,[Fee30Day]  ,[Fee90Day] ,[Fee180Day],[Fee360Day] ,[EstablishTime] FROM [Club]"

        Dim en As IEnumerator = sdsClub.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            With Club.Instance
                .AddClub(en.Current("ClubID"), en.Current("Name"), en.Current("EstablishTime"), en.Current("TopicCount"), en.Current("DetailCount"), en.Current("TopicCountHide"))
            End With
        End While

        HttpContext.Current.Application("IsLoadingClubData") = False

        'With HttpContext.Current
        '    If .Request.Url Is Nothing Then
        '        .Server.Transfer("\club\loaddata.aspx?f=loadclub")
        '    Else
        '        .Server.Transfer("\club\loaddata.aspx?f=loadclub&url=" + .Request.Url.ToString.Replace("&", "$"))
        '    End If
        'End With
    End Sub

#Region "發文訊息"
    Private Class MemberData
        Public Number As String
        Public Email As String
        Public Phone As String
        Public CanSMS As Integer
        Public IsEmail As Integer
        Public IsWantGooMail As Integer
        Public IsFollow As Integer
        Public StopDeadline As Date
    End Class

    Private Sub WriteBugMemberNo(ByVal log As String)
        Dim dir As String = "C:\WantGoo\Wantgoooooo1\songlog\ClubTemp\BugMemberNo\"
        If IO.Directory.Exists(dir) = False Then
            IO.Directory.CreateDirectory(dir)
        End If
        Dim filename As String = dir + Format(Now, "yyyyMMdd") & ".txt"
        My.Computer.FileSystem.WriteAllText(filename, Now.ToString & vbCrLf & log & vbCrLf & "---------------------------------" & vbCrLf, True)
    End Sub

    Private Sub WriteError(ByVal log As String)
        Dim dir As String = "C:\WantGoo\Wantgoooooo1\songlog\ClubTemp\Error\"
        If IO.Directory.Exists(dir) = False Then
            IO.Directory.CreateDirectory(dir)
        End If
        Dim filename As String = dir + Format(Now, "yyyyMMdd") & ".txt"
        My.Computer.FileSystem.WriteAllText(filename, Now.ToString & vbCrLf & log & vbCrLf & "---------------------------------" & vbCrLf, True)
    End Sub

    Private Function GetFollows(ByVal topicId As String, ByVal connection As System.Data.SqlClient.SqlConnection) As Generic.List(Of String)
        Dim list As New Generic.List(Of String)

        If topicId = "0" Then Return list

        Dim cmdTxt As String = "select * from topicfollow where topicid=" & topicId

        Dim cmd As New SqlClient.SqlCommand(cmdTxt, connection)
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(cmd)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)


        For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
            list.Add(row("memberno").ToString)
        Next

        Return list
    End Function

    Private Function GetClubMemberData(ByVal clubId As String, ByVal topicid As String) As Generic.List(Of MemberData)
        Dim list As New Generic.List(Of MemberData)

        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("ClubConnection").ConnectionString)
        connection.Open()

        Dim memberNos As New Generic.List(Of String)

        Dim canSMSs As New Generic.List(Of Integer)
        Dim isEmails As New Generic.List(Of Integer)
        Dim isWantGooMails As New Generic.List(Of Integer)
        Dim stopDeadlines As New Generic.List(Of Date)
        Dim isFollows As New Generic.List(Of Integer)

        Try
            Dim follows As Generic.List(Of String) = GetFollows(topicid, connection)

            Dim cmdTxt As String = "Select * From ClubMember Where ClubID=" & clubId

            Dim cmd As New SqlClient.SqlCommand(cmdTxt, connection)

            'Dim reader As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader

            Dim adapter As New System.Data.SqlClient.SqlDataAdapter(cmd)
            Dim dataSet As New Data.DataSet
            adapter.Fill(dataSet)

            Dim has As Boolean = False

            For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows

                If IsDBNull(row("Deadline")) = False Then
                    Dim deadline As Date = CDate(row("Deadline"))

                    If deadline.Subtract(Now).TotalMinutes > 0 Then

                        If IsDBNull(row("MemberNo")) = False Then
                            Dim memberno As String = row("MemberNo").ToString
                            memberNos.Add(memberno)

                            If IsDBNull(row("IsSms")) = False Then
                                canSMSs.Add(Val(row("IsSms")))
                            Else
                                canSMSs.Add(0)
                            End If

                            If IsDBNull(row("IsEmail")) = False Then
                                isEmails.Add(Val(row("IsEmail")))
                            Else
                                isEmails.Add(0)
                            End If

                            If IsDBNull(row("isWantGooMail")) = False Then
                                isWantGooMails.Add(Val(row("isWantGooMail")))
                            Else
                                isWantGooMails.Add(0)
                            End If

                            If IsDBNull(row("StopDeadline")) = False Then
                                stopDeadlines.Add(CDate(row("StopDeadline")))
                            Else
                                stopDeadlines.Add(New Date(1911, 1, 1))
                            End If

                            If follows.Contains(memberno) Then
                                isFollows.Add(1)
                            Else
                                isFollows.Add(0)
                            End If

                            'If topicid = "0" Then
                            '    isFollows.Add(0)
                            'Else
                            '    If IsFollow(topicid, memberno, connection) Then
                            '        isFollows.Add(1)
                            '    Else
                            '        isFollows.Add(0)
                            '    End If
                            'End If

                        End If
                    End If
                End If

            Next
        Catch ex As Exception
            WriteError("GetMemberData : " & vbCrLf & ex.ToString)
        Finally
            connection.Close()
            connection.Dispose()
        End Try

        For index As Integer = 0 To memberNos.Count - 1
            Dim data As New MemberData
            data.Number = memberNos(index)
            data.CanSMS = canSMSs(index)
            data.IsEmail = isEmails(index)
            data.IsWantGooMail = isWantGooMails(index)
            data.StopDeadline = stopDeadlines(index)
            data.IsFollow = isFollows(index)

            If data.StopDeadline.Subtract(Now).TotalSeconds > 0 Then
            Else
                If MemberDataAccessor.Instance.GetMemberData(memberNos(index)) IsNot Nothing Then
                    data.Email = MemberDataAccessor.Instance.GetMemberData(memberNos(index)).Email
                    data.Phone = MemberDataAccessor.Instance.GetMemberData(memberNos(index)).Cellphone
                    list.Add(data)
                Else
                    WriteBugMemberNo(vbCrLf & memberNos(index))
                End If
            End If
        Next

        Return list
    End Function

    Public Sub RunPostMessage(ByVal clubId As String, ByVal topicid As String, ByVal masterNo As String, ByVal url As String, ByVal title As String, ByVal content As String, _
                              ByVal isSMS As Boolean, ByVal isAdd As Boolean, ByVal isMessage As Boolean, ByVal isReplay As Boolean)

        Try
            ''外部信箱
            Dim MailConnection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("MailConnection").ConnectionString)
            MailConnection.Open()

            Dim tempContent As String = String.Empty
            Dim lengthContent As Integer = 100
            If (content.Length >= lengthContent) Then
                tempContent = content.Substring(0, lengthContent)
            Else
                tempContent = content.Substring(0, content.Length)
            End If
            tempContent = tempContent.Replace(",", "，").Replace("''", "’")

            Dim Params = clubId.ToString() + "," + "0" + "," + masterNo.ToString() + "," + url.Replace(",", "，").Replace("''", "’") + "," + title.Replace(",", "，").Replace("''", "’") + "," + tempContent + "," + isSMS.ToString() + "," + isAdd.ToString() + "," + isMessage.ToString() + "," + isReplay.ToString()
            Dim cmdMailTxt As String = String.Format("exec [dbo].[MailQueueNewInsert] N'NewClubPost',N'{0}';", Params)
            'Debug.WriteCookie("cmdMailTxt", cmdMailTxt)
            Dim cmdMail As New SqlClient.SqlCommand(cmdMailTxt, MailConnection)
            cmdMail.ExecuteNonQuery()
            MailConnection.Close()
            MailConnection.Dispose()
            cmdMail.Dispose()
        Catch ex As Exception
            'Debug.WriteDB("App_Code - RunPostMessage - Exception", "", ex.Message, ex.StackTrace)
            'Debug.WriteCookie("CreateUserWizard1_CreatedUser_Exception_StackTrace", "Time：" + Now.ToString() + "…" + ex.StackTrace)
        End Try

        Dim ppath As String = "C:\WantGoo\Wantgoooooo1\songlog\ClubTemp\RunPostMessage\" & masterNo
        If IO.Directory.Exists(ppath) = False Then
            IO.Directory.CreateDirectory(ppath)
        End If
        Dim filename As String = ppath & "\" & Format(Now, "yyyyMMdd") & ".log"

        Dim log As String = title & vbTab & content & vbTab & isSMS.ToString & vbTab & isAdd.ToString & vbTab & isMessage.ToString

        My.Computer.FileSystem.WriteAllText(filename, Now.ToString & vbCrLf & log & vbCrLf & "---------------------------------" & vbCrLf, True)

        Dim memberDatas As Generic.List(Of MemberData) = GetClubMemberData(clubId, topicid)

        '內部信箱
        SendInsideMailV2(New Object() {memberDatas, masterNo, url, title, content, isAdd, isMessage, isReplay})

        '右下訊息
        SendNewMessage(New Object() {memberDatas, masterNo, url, title, content, isAdd, isMessage, isReplay})

        If isSMS AndAlso isReplay = False Then
            '簡訊
            Dim t3 As New Threading.Thread(AddressOf SendSMSV2)
            t3.Start(New Object() {memberDatas, masterNo, url, title, content, isAdd, isMessage, isSMS, isReplay})
        End If

        ''外部信箱
        'Dim t1 As New Threading.Thread(AddressOf SendOutsideMailV2)
        't1.Start(New Object() {memberDatas, masterNo, url, title, content, isAdd, isMessage, isReplay})

        ''內部信箱
        'Dim t2 As New Threading.Thread(AddressOf SendInsideMailV2)
        't2.Start(New Object() {memberDatas, masterNo, url, title, content, isAdd, isMessage, isReplay})

        ''右下訊息
        'Dim t4 As New Threading.Thread(AddressOf SendNewMessage)
        't4.Start(New Object() {memberDatas, masterNo, url, title, content, isAdd, isMessage, isReplay})

    End Sub

    Private mSubject As String = "@Master@Action-@ArticleTitle"

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
  "            <span style=""font-size:12px;"">親愛的 忠實會員 您好， </span><br />" + _
  "        </div>" + _
  "        <div style=""font-size:12px; color:Black; padding:16px; text-align:left; "">" + _
  "@Content" + _
  "         </div>" + _
  "    </td>" + _
  "</tr>" + _
  "<tr>" + _
  "    <td style="" text-align:left;"">" + _
  "        <div style=""font-size:12px; color:Gray; line-height:18px; margin-top:5px;"">" + _
  "            <span>若需任何服務及幫助，請與 service@wantgoo.com Wantgoo Team聯絡，謝謝！</span>" + _
  "        </div>" + _
  "    </td>" + _
  "</tr>" + _
  "</table>"

    Private Sub SendOutsideMailV2(ByVal objs As Object)

        Try
            Dim memberDatas As Generic.List(Of MemberData) = objs(0)
            Dim masterNo As String = objs(1)
            Dim url As String = objs(2)
            Dim title As String = objs(3)
            Dim content As String = objs(4)
            Dim isAdd As Boolean = objs(5)
            Dim isMessage As Boolean = objs(6)
            Dim isReplay As Boolean = objs(7)

            Dim action As String = "新增文章"

            If isAdd = True Then
                action = "補充了文章"
            End If

            If isMessage = True Then
                action = "有新訊息"
            End If

            If isReplay Then
                action = "提醒您有新回應"
            End If

            Dim index As Integer = 0

            Dim logDir As String = "C:\WantGoo\Wantgoooooo1\songlog\ClubTemp\OutsideMailLog\" & masterNo.ToString
            If IO.Directory.Exists(logDir) = False Then
                IO.Directory.CreateDirectory(logDir)
            End If

            For Each data As MemberData In memberDatas
                Try
                    If data.IsEmail = True AndAlso (data.Number <> masterNo Or (isMessage = True)) AndAlso _
                        ((isReplay = True AndAlso data.IsFollow = 1) Or isReplay = False) Then

                        Dim memberNo As String = data.Number
                        Dim toMail As String = data.Email

                        Dim toNickName As String = MemberDataAccessor.Instance.GetMemberNickName(memberNo).Trim

                        Dim masterName As String = MemberDataAccessor.Instance.GetMemberNickName(masterNo).Trim

                        Dim toSubject As String = mSubject.Replace("@Master", masterName).Replace("@ArticleTitle", title).Replace("@Action", action)

                        Dim text As String = masterName & action & ", 快來看看說了什麼：<br />" + _
                                "<div align=""center"" style=""font-weight: bold""><a href=""" & url & """>『" + Left(title, 10) + "』</a></div><br />" + url + _
                                "<br/>" + Left(content, 30) + "..." + _
                                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />" + _
                                "有任何問題，歡迎您來信玩股客服：service@wantgoo.com<br />" + _
                                "謝謝，<br />" + _
                                "Wantgoo Team<br /><br />" + _
                                "注意：本郵件是透過系統產生與發送，請勿直接回覆。"
                        Dim toContent As String = mHtmlTemplate.Replace("忠實會員", toNickName)
                        toContent = toContent.Replace("@Content", text)

                        My.Computer.FileSystem.WriteAllText(logDir & "\" & Now.Year.ToString & Format(Now.Month, "00") & Format(Now.Day, "00") & ".txt", Now.ToString & vbTab & toSubject & vbTab & toMail & vbCrLf, True)

                        MailServiceNew.Instance.SendHtmlMail(toMail, toNickName, toSubject, toContent)
                    End If
                Catch ex As Exception
                    WriteError("SendOutsideMailV2(" & index.ToString & ") : " & vbCrLf & ex.ToString)
                End Try

                index += 1
            Next
        Catch ex As Exception
            WriteError("SendOutsideMailV2 Function : " & vbCrLf & ex.ToString)
        End Try
    End Sub

    Private Sub SendInsideMailV2(ByVal objs As Object)
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("MailConnection").ConnectionString)
        connection.Open()

        Try
            Dim memberDatas As Generic.List(Of MemberData) = objs(0)
            Dim masterNo As String = objs(1)
            Dim url As String = objs(2)
            Dim title As String = objs(3)
            Dim content As String = objs(4)
            Dim isAdd As Boolean = objs(5)
            Dim isMessage As Boolean = objs(6)
            Dim isReplay As Boolean = objs(7)

            Dim action As String = "新增文章"

            If isAdd = True Then
                action = "補充了文章"
            End If

            If isMessage = True Then
                action = "有新訊息"
            End If

            If isReplay Then
                action = "提醒您有新回應"
            End If

            Dim masterName As String = MemberDataAccessor.Instance.GetMemberNickName(masterNo).Trim

            Dim toSubject As String = mSubject.Replace("@Master", masterName).Replace("@ArticleTitle", title).Replace("@Action", action)
            toSubject = Left(toSubject, 45) & "..."
            Dim toContent As String = masterName & action & ", 快來看看說了什麼<br />" + _
                "<div align=""center""><a href=""" & url & """>『" + Left(title, 10) + "』</a></div><br />" + url + _
                "<br/>" + Left(content, 30) + "..." + _
                "<br />精彩好文，千萬別錯過。<br /><br />" + _
                "如果您無法直接點選，請複製網址到瀏覽器開啟。<br /><br />"
            toContent = HttpUtility.HtmlEncode(toContent)

            Dim subCount As Integer = 0
            Dim totalCount As Integer = 0
            Dim ToMemberListString As String = String.Empty
            For Each data As MemberData In memberDatas

                subCount = subCount + 1
                totalCount = totalCount + 1

                If data.IsWantGooMail = True AndAlso (data.Number <> masterNo Or (isMessage = True)) AndAlso _
                    ((isReplay = True AndAlso data.IsFollow = 1) Or isReplay = False) Then

                    Dim toMemberNo As String = data.Number
                    ToMemberListString += (toMemberNo + ";")
                End If

                If subCount >= 100 Or totalCount >= memberDatas.Count Then

                    ToMemberListString.TrimEnd(";")

                    Dim cmdTxt As String = String.Empty
                    cmdTxt += "exec [Mail].[dbo].[InsideMailQueueInsert] "
                    cmdTxt += "   N'InsideMailClubNew'" + ", "
                    cmdTxt += "  " + masterNo + ", "
                    cmdTxt += "   N'" + ToMemberListString + "', "
                    cmdTxt += "   N'" + toSubject + "', "
                    cmdTxt += "   N'" + toContent + "'"
                    'Debug.WriteDB("App_Code - Club - SendInsideMailV2 - cmdTxt", "", cmdTxt)
                    'Debug.WriteCookie("App_Code - Club - SendInsideMailV2 - cmdTxt", cmdTxt)
                    Dim cmd As New SqlClient.SqlCommand(cmdTxt, connection)
                    cmd.ExecuteNonQuery()

                    ToMemberListString = String.Empty
                    subCount = 0

                End If

            Next

        Catch ex As Exception
            'WriteError("SendInsideMailV2 Function : " & vbCrLf & ex.ToString)
            Debug.WriteDB("App_Code - Club -  SendInsideMailV2 - Exception", "title：" + objs(3), ex.Message, ex.StackTrace)
            'Debug.WriteCookie("App_Code - Club -  SendInsideMailV2 - Exception", ex.StackTrace)
        End Try

        connection.Close()
        connection.Dispose()
    End Sub

    Private Sub SendNewMessage(ByVal objs As Object)
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("MailConnection").ConnectionString)
        connection.Open()

        Try
            Dim memberDatas As Generic.List(Of MemberData) = objs(0)
            Dim masterNo As String = objs(1)
            Dim url As String = objs(2)
            Dim title As String = objs(3)
            Dim content As String = objs(4)
            Dim isAdd As Boolean = objs(5)
            Dim isMessage As Boolean = objs(6)
            Dim isReplay As Boolean = objs(7)

            Dim action As String = "新增文章"

            If isAdd = True Then
                action = "補充了文章"
            End If

            If isMessage = True Then
                action = "有新訊息"
            End If

            If isReplay Then
                action = "提醒您有新回應"
            End If

            Dim subCount As Integer = 0
            Dim totalCount As Integer = 0
            Dim ToMemberListString As String = String.Empty

            Dim toSubject As String = url
            Dim toContent As String = Left(title, 10) & " - " & action

            For Each data As MemberData In memberDatas

                subCount = subCount + 1
                totalCount = totalCount + 1

                'Try
                If data.IsWantGooMail = True AndAlso (data.Number <> masterNo Or (isMessage = True)) AndAlso _
                   ((isReplay = True AndAlso data.IsFollow = 1) Or isReplay = False) Then

                    Dim toMemberNo As String = data.Number
                    ToMemberListString += (toMemberNo + ";")

                End If

                If subCount >= 100 Or totalCount >= memberDatas.Count Then

                    ToMemberListString.TrimEnd(";")

                    Dim cmdTxt As String = String.Empty
                    cmdTxt += "exec [Mail].[dbo].[InsideMailQueueInsert] "
                    cmdTxt += "   N'InsideMailClubNewMessage'" + ", "
                    cmdTxt += "  " + masterNo + ", "
                    cmdTxt += "   N'" + ToMemberListString + "', "
                    cmdTxt += "   N'" + toSubject + "', "
                    cmdTxt += "   N'" + toContent + "'"
                    'Debug.WriteDB("App_Code - Club - cmdTxt", "", cmdTxt)
                    'Debug.WriteCookie("App_Code - Club - SendNewMessage - cmdTxt", cmdTxt)
                    Dim cmd As New SqlClient.SqlCommand(cmdTxt, connection)
                    cmd.ExecuteNonQuery()

                    ToMemberListString = String.Empty
                    subCount = 0

                End If

            Next

        Catch ex As Exception
            'WriteError("SendNewMessage Function : " & vbCrLf & ex.ToString)
            Debug.WriteDB("App_Code - Club -  SendNewMessage - Exception", "title：" + objs(3), ex.Message, ex.StackTrace)
            'Debug.WriteCookie("App_Code - Club - SendNewMessage - Exception", ex.StackTrace)
        End Try

        connection.Close()
        connection.Dispose()
    End Sub

    Private Sub SendSMSV2(ByVal objs As Object)
        Try
            Dim memberDatas As Generic.List(Of MemberData) = objs(0)
            Dim masterNo As String = objs(1)
            Dim url As String = objs(2)
            Dim title As String = objs(3)
            Dim content As String = objs(4)
            Dim isAdd As Boolean = objs(5)
            Dim isMessage As Boolean = objs(6)
            Dim isSMS As Boolean = objs(7)
            Dim isReplay As Boolean = objs(8)

            Dim dir As String = "C:\WantGoo\Wantgoooooo1\songlog\ClubTemp\SMSRESULT\" & masterNo
            If IO.Directory.Exists(dir) = False Then
                IO.Directory.CreateDirectory(dir)
            End If
            Dim filename As String = dir & "\" & Format(Now, "yyyyMMdd") & ".log"

            Dim index As Integer = 0

            For Each data As MemberData In memberDatas
                Try
                    If data.Number <> masterNo Or (isMessage = True) Then
                        If isSMS = True AndAlso data.Phone.Trim <> "" AndAlso data.CanSMS = True AndAlso isMessage = False Then
                            Try
                                Dim memberNo As String = data.Number
                                Dim toPhone As String = data.Phone

                                Dim toNickName As String = MemberDataAccessor.Instance.GetMemberNickName(memberNo).Trim

                                Dim masterName As String = MemberDataAccessor.Instance.GetMemberNickName(masterNo).Trim
                                Select Case masterName
                                    Case "短線飆派軍團長KPMG"
                                        masterName = "軍團長"
                                End Select

                                content = content.Replace("，", ",").Replace("。", ".")

                                Dim messager As New PhoneMessager

                                Dim message As String = masterName & "[" & title & "]:" & content

                                If isAdd = True Then
                                    message = masterName & "補充[" & title & "]:" & content
                                End If

                                If isMessage = True Then
                                    message = masterName & "說:" & title
                                End If

                                If isReplay Then
                                    message = masterName & "提醒:" & title
                                End If

                                Dim mmes As Generic.List(Of String) = GetMessageArray(message, 90)
                                'If mmes.Count = 1 Then
                                '    message = mmes(0)
                                'Else
                                '    message = mmes(0) + "..."
                                'End If

                                For Each msg As String In mmes
                                    message = msg

                                    Dim errorMessage As String = ""

                                    Dim isBlack As Boolean = False

                                    Dim isSuccess As Boolean = False

                                    Dim messageid As String = ""

                                    If MemberDataAccessor.Instance.GetMyGold(MemberDataAccessor.Instance.GetUserName(memberNo)) > 0 Then
                                        If messager.Send(memberNo, message, toPhone, errorMessage, messageid, isBlack) = True Then
                                            isSuccess = True
                                            MemberDataAccessor.Instance.AddMemberGold(memberNo, -1, GoldType.Sms, 0)
                                        End If

                                        Dim context As String = isSuccess & vbTab & messageid & vbTab & memberNo & vbTab & toPhone & vbTab & Now.ToString & vbTab & "成功" & vbTab & errorMessage & vbTab & message & vbCrLf
                                        My.Computer.FileSystem.WriteAllText(filename, context, True)
                                    Else
                                        My.Computer.FileSystem.WriteAllText(filename, "False" & vbTab & "-1" & vbTab & memberNo & vbTab & toPhone & vbTab & Now.ToString & vbTab & "好評不夠" & vbTab & "" & vbTab & message & vbCrLf, True)
                                    End If

                                    If isMessage = False Then
                                        Exit For
                                    End If
                                Next
                            Catch ex As Exception
                                My.Computer.FileSystem.WriteAllText(filename, "False" & vbTab & "-1" & vbTab & data.Number & vbTab & data.Phone & vbTab & Now.ToString & vbTab & ex.ToString & "" & vbCrLf, True)
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    WriteError("SendSMSV2(" & index.ToString & ") : " & vbCrLf & ex.ToString)
                End Try

                index += 1
            Next
        Catch ex As Exception
            WriteError("SendSMSV2 Function : " & vbCrLf & ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' 將簡訊內容分段,每則最多96字
    ''' </summary>
    Private Function GetMessageArray(ByVal message As String, Optional ByVal wordsLimit As Integer = 90) As Generic.List(Of String)
        Dim messageList As New Generic.List(Of String)

        If System.Text.Encoding.Default.GetBytes(message).Length < wordsLimit Then
            messageList.Add(message)
            Return messageList
        End If

        Dim len As Integer
        Dim mes As String = ""
        For Each ch As Char In message.ToCharArray
            len = len + System.Text.Encoding.Default.GetBytes(ch).Length
            mes = mes + ch

            If len > wordsLimit Then
                len = 0
                messageList.Add(mes)
                mes = ""
            End If
        Next
        If mes <> "" Then messageList.Add(mes)

        Return messageList
    End Function


    Private Class MessageResp
        Public RespMessage As String = "None"
        Public CellPhone As String = "None"
        Public Time As String = ""
        Public IsAccepted As Boolean = False
        Public DELIVRD As String = "None"
    End Class

    ''' <summary>
    ''' 送手機簡訊
    ''' </summary>
    ''' <remarks>
    ''' 1. 所有含 * 的Tag 為必需填寫之參數，其他Tag 不填寫時系統將會自動帶入內定值。
    ''' 2. encoding 等於lbig5 時，每67 個字扣1 通簡訊通數,lascii 則是每153 個字扣1 通簡訊通數
    ''' 3. 長簡訊功能有部份電信業者（如PHS）及部份手機無法支援，將會發送失敗。
    ''' 4. 發送全球國際簡訊，每則門號扣3 通簡訊通數
    ''' </remarks>
    Private Class PhoneMessager

        Public Function Send(ByVal memberno As String, ByVal message As String, ByVal phoneNumber As String, ByRef errorMessage As String, ByRef msgid As String, ByVal isBack As Boolean) As Boolean
            If isBack = True Then
                Return True
            End If

            Dim url As String = _
            "http://api.twsms.com/send_sms.php?" & _
            "username=scotthuang&password=kai5168" & _
            "&type=now&encoding=big5" & _
            "&mobile=" & phoneNumber & _
            "&message=" & message & _
            "&vldtme=3600"

            Dim result As String = QueryWebPage(url, System.Text.Encoding.GetEncoding("BIG5"))
            msgid = Val(result.Replace("resp=", "")).ToString

            errorMessage = ""

            If result.Contains("-1") Then
                errorMessage = "找不到msgid 或此預約簡訊已經送出無法刪除"
            ElseIf result.Contains("-2") Then
                errorMessage = "帳號或密碼錯誤"
            ElseIf result.Contains("-3") Then
                errorMessage = "預約的簡訊尚未送出"
            ElseIf result.Contains("-4") Then
                errorMessage = "type TAG 設定錯誤"
            ElseIf result.Contains("-5") Then
                errorMessage = "手機端尚未回傳訊息"
            ElseIf result.Contains("-6") Then
                errorMessage = "傳送到電信中心失敗" & result
            ElseIf result.Contains("-7") Then
                errorMessage = "沒有簡碼（雙向系統使用）"
            ElseIf result.Contains("-8") Then
                errorMessage = "沒有回傳訊息（雙向系統使用）"
            ElseIf result.Contains("-9") Then
                errorMessage = "sdate or edate 設定錯誤（兩者必須同時設定）"
            ElseIf result.Contains("-10") Then
                errorMessage = "沒有失敗回補紀錄"
            ElseIf result.Contains("-11") Then
                errorMessage = "帳號停用"
            ElseIf result.Contains("-12") Then
                errorMessage = "簡訊疑似有詐財或色情內容"
            ElseIf result.Contains("-13") Then
                errorMessage = "新密碼少於8 個字元"
            End If

            If errorMessage <> "" Then
                WriteLog(memberno, message, phoneNumber, errorMessage)
                Return False
            End If

            Return True
        End Function

        Public Function GetMessageReposen(ByVal messageid As String) As MessageResp

            Dim url As String = "http://api.twsms.com/query_sms.php?username=scotthuang&password=kai5168&type=now&msgid=" + messageid
            Dim result As String = QueryWebPage(url, System.Text.Encoding.GetEncoding("BIG5"))
            Dim cc() As String = result.Replace("resp=", "").Split(",")

            Dim resp As New MessageResp

            If cc.Length = 5 Then

                With resp

                    Select Case Val(cc(0))
                        Case 0
                            .RespMessage = "手機端已經收到簡訊或刪除預約簡訊成功"
                        Case -1
                            .RespMessage = "找不到msgid 或此預約簡訊已經送出無法刪除"
                        Case -2
                            .RespMessage = "帳號或密碼錯誤"
                        Case -3
                            .RespMessage = "預約的簡訊尚未送出"
                        Case -4
                            .RespMessage = "type TAG 設定錯誤"
                        Case -5
                            .RespMessage = "手機端尚未回傳訊息"
                        Case -6
                            .RespMessage = "傳送到電信中心失敗"
                        Case -7
                            .RespMessage = "沒有簡碼（雙向系統使用）"
                        Case -8
                            .RespMessage = "沒有回傳訊息（雙向系統使用）"
                        Case -9
                            .RespMessage = "sdate or edate 設定錯誤（兩者必須同時設定）"
                        Case -10
                            .RespMessage = "沒有失敗回補紀錄"
                        Case -11
                            .RespMessage = "帳號停用"
                        Case -12
                            .RespMessage = "簡訊疑似有詐財或色情內容"
                        Case -13
                            .RespMessage = "新密碼少於8 個字元"
                    End Select

                    .CellPhone = cc(1)

                    '.Time = New Date(Val(cc(2).Substring(0, 4)), Val(cc(2).Substring(4, 2)), Val(cc(2).Substring(6, 2)), _
                    '                 Val(cc(2).Substring(8, 2)), Val(cc(2).Substring(10, 2)), Val(cc(2).Substring(12, 2)))

                    .Time = cc(2)

                    If Val(cc(3)) = 0 Then
                        .IsAccepted = True
                    Else
                        .IsAccepted = False
                    End If

                    Select Case cc(4)
                        Case "DELIVRD"
                            .DELIVRD = "已經接收簡訊"
                        Case "EXPIRED"
                            .DELIVRD = "Message validity period has expired"
                        Case "DELETED"
                            .DELIVRD = "Message has been deleted"
                        Case "UNDELIV"
                            .DELIVRD = "Message is undeliverable"
                        Case "ACCEPTD"
                            .DELIVRD = "Message is in accepted state"
                        Case "UNKNOWN"
                            .DELIVRD = "Message is in invalid state"
                        Case "REJECTD"
                            .DELIVRD = "Message is in a rejected state"
                        Case "SYNTAXE"
                            .DELIVRD = "Syntax Error"
                        Case "MOBERROR"
                            .DELIVRD = "Mobile Phone Error"
                        Case "MSGERROR"
                            .DELIVRD = "Message Error"
                        Case "OTHERROR"
                            .DELIVRD = "System Error"
                        Case "REJERROR"
                            .DELIVRD = "被關鍵字過濾系統擋掉的簡訊"
                    End Select

                End With


            End If

            Return resp
        End Function

        Private Function QueryWebPage(ByVal url As String, ByVal encoding As System.Text.Encoding) As String
            Dim request As HttpWebRequest = HttpWebRequest.Create(url)
            Dim response As HttpWebResponse = request.GetResponse
            Dim sr As StreamReader = New StreamReader(response.GetResponseStream, encoding)
            Dim content As String = sr.ReadToEnd
            sr.Close()
            request = Nothing
            response = Nothing

            Return content
        End Function

        Private mLogPath As String = "C:\WantGoo\Wantgoooooo1\songlog\ClubTemp\SMSLOG\"
        Private Sub WriteLog(ByVal memberno As String, ByVal message As String, ByVal phoneNumber As String, ByVal errorMessage As String)
            If Not My.Computer.FileSystem.DirectoryExists(mLogPath) Then
                My.Computer.FileSystem.CreateDirectory(mLogPath)
            End If
            Dim filename As String = mLogPath + Format(Now, "yyyyMMdd") & ".log"
            Dim content As String = Format(Now, "HH:mm") + vbTab + phoneNumber + vbTab + message + vbTab + errorMessage + vbTab + memberno + vbCrLf
            My.Computer.FileSystem.WriteAllText(filename, content, True)
        End Sub
    End Class
#End Region

End Class

Public Enum ClubStatus
    Chairman = 0 '主席
    Normal = 1 '社團會員
    Expired = 2 '過期會員
    None = 3 '非社團會員
    Administrator = 4 '管理員
    [Stop] = 5 '暫停會期
End Enum
