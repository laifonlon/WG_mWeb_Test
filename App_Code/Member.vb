Imports Microsoft.VisualBasic

Public Class Member

#Region "Singleton"
    Private Shared mObj As New Member
    Public Shared ReadOnly Property Instance() As Member
        Get
            Return mObj
        End Get
    End Property
#End Region

    Private mConnectString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString

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

    ''' <summary>
    ''' 是否被封鎖部落格
    ''' </summary>
    Public Function IsHideBlog(Optional ByVal memberNo As String = Nothing) As Boolean
        If memberNo Is Nothing Then memberNo = Me.MemberNo
        If memberNo = "0" Then Return True

        Dim sds As New SqlDataSource With {.ConnectionString = mConnectString}
        sds.SelectCommand = "SELECT IsHideBlog From Member Where MemberNo = " + memberNo
        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Return en.Current("IsHideBlog")
        End While

        Return True
    End Function

    ''' <summary>
    ''' 是否顯示廣告
    ''' </summary>
    Public Function IsShowAd(ByVal memberNo As String) As Boolean
        If memberNo = "0" Then Return True

        If HttpContext.Current.Session("IsShowAd") Is Nothing OrElse _
            HttpContext.Current.Session("IsShowAd_Deadline") Is Nothing OrElse _
             HttpContext.Current.Session("IsShowAd_Deadline") < Now Then
            SetShowAdFlag(memberNo)
        End If
        Return HttpContext.Current.Session("IsShowAd")

    End Function

    Public Function IsShowAd() As Boolean
        Return IsShowAd(MemberNo)
    End Function

    ''' <summary>
    ''' 設定是否顯示廣告旗標
    ''' </summary>
    Public Sub SetShowAdFlag(ByVal memberNo As String)
        With MemberDataAccessor.Instance
            Dim level As MemberLevel = .GetMemberLevel(memberNo)
            If level = MemberLevel.Rich Then
                HttpContext.Current.Session("IsShowAd") = False
                HttpContext.Current.Session("IsShowAd_Deadline") = .GetRichUserDeadline(memberNo)

            ElseIf level = MemberLevel.Super Then
                HttpContext.Current.Session("IsShowAd") = False
                HttpContext.Current.Session("IsShowAd_Deadline") = .GetSuperUserDeadline(memberNo)

            Else
                HttpContext.Current.Session("IsShowAd") = True
                HttpContext.Current.Session("IsShowAd_Deadline") = Now.AddDays(1)
            End If

            If .IsAdiminstrator Then
                HttpContext.Current.Session("IsShowAd") = True
                HttpContext.Current.Session("IsShowAd_Deadline") = Now.AddDays(1)
            End If
        End With

    End Sub


#Region "飆股市集開發人員會期"
    Private mIsHottipDeveloperTable As New Hashtable
    ''' <summary>
    ''' 是否具有飆股市集開發人員會期
    ''' </summary>
    Public Function IsHottipDeveloper(Optional ByVal memberNo As String = Nothing) As Boolean
        If memberNo Is Nothing Then memberNo = MemberDataAccessor.Instance.GetMemberNo()
        If memberNo = "0" Then Return False

        If mIsHottipDeveloperTable.ContainsKey(memberNo) Then
            Return mIsHottipDeveloperTable(memberNo)
        End If

        Dim deadline As Date = GetHottipDevelopDeadline(memberNo)

        If deadline > Now Then
            mIsHottipDeveloperTable.Add(memberNo, True)
            Return True
        Else
            mIsHottipDeveloperTable.Add(memberNo, False)
            Return False
        End If

    End Function

    Private mHottipDevelopDeadlineTable As New Hashtable
    ''' <summary>
    ''' 飆股市集開發者會期
    ''' </summary>
    Public Function GetHottipDevelopDeadline(Optional ByVal memberNo As String = Nothing) As Date

        If memberNo Is Nothing Then memberNo = MemberDataAccessor.Instance.GetMemberNo()
        If memberNo = "0" Then Return Now

        If mHottipDevelopDeadlineTable.ContainsKey(memberNo) Then
            Return mHottipDevelopDeadlineTable(memberNo)
        End If

        Dim sds As New SqlDataSource
        sds.ConnectionString = mConnectString
        sds.SelectCommand = "SELECT DevelopDeadline From Hottip Where MemberNo = @MemberNo"
        sds.SelectCommand = sds.SelectCommand.Replace("@MemberNo", memberNo)

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        If Not en.MoveNext Then Return Now

        Dim deadline As Date = en.Current("DevelopDeadline")
        mHottipDevelopDeadlineTable.Add(memberNo, deadline)

        Return deadline
    End Function

    Public Sub LoadHottipDevelopDeadline(Optional ByVal memberNo As String = Nothing)
        If memberNo Is Nothing Then memberNo = MemberDataAccessor.Instance.GetMemberNo()
        If memberNo = "0" Then Exit Sub

        mIsHottipDeveloperTable.Remove(memberNo)
        mHottipDevelopDeadlineTable.Remove(memberNo)

        IsHottipDeveloper(memberNo)
    End Sub
#End Region
End Class
