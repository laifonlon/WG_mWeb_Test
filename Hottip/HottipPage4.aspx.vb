
Partial Class Hottip_HottipPage4
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        Me.LoadHottipGroupName() '載入群組名稱下拉選單

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not MemberDataAccessor.Instance.IsLogin Then
            Response.Redirect("/Login.aspx?GoBackUrl=" + Server.UrlEncode(Request.RawUrl))
            'Throw New Exception("會員未登入")
        End If

        If Not IsPostBack Then

            Dim group As String = Me.GetGroup

            sdsCollect.SelectParameters("MemberNo").DefaultValue = MemberDataAccessor.Instance.GetMemberNo
            sdsCollect.SelectParameters("Group").DefaultValue = Val(group)

        End If

        '最下方廣告 是否顯示
        Mob_300x2501.Visible = Member.Instance.IsShowAd()

    End Sub

    Private Function GetGroup() As String
        Dim group As Integer = Val(Request("g"))
        If group = 0 Then group = 1
        Return group.ToString
    End Function

    ''' <summary>
    ''' 載入群組名稱下拉選單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadHottipGroupName()

        Dim groupNames As Generic.List(Of String) = Hottip.Instance.GetGroupName(MemberDataAccessor.Instance.GetMemberNo)

        ddlGroupList.Items.Clear()
        For index As Integer = 0 To groupNames.Count - 1
            Dim gName As String = groupNames(index)
            Dim gNameIndex As Integer = index + 1
            ddlGroupList.Items.Add(gName)
            ddlGroupList.Items(ddlGroupList.Items.Count - 1).Value = gNameIndex
        Next

        If Not String.IsNullOrEmpty(Request("g")) Then
            ddlGroupList.SelectedValue = Request("g")
        End If

    End Sub

    'Protected Sub btnMove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Protected Sub ddlGroup3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroupList.SelectedIndexChanged

        Dim ParamIndex As Integer = Request.RawUrl.IndexOf("?g=")
        If ParamIndex > 0 Then
            Response.Redirect(Request.RawUrl.Substring(0, ParamIndex) + "?g=" + ddlGroupList.SelectedValue)
        Else
            Response.Redirect(Request.RawUrl + "?g=" + ddlGroupList.SelectedValue)
        End If

    End Sub

    Protected Sub ddlFunctionList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFunctionList.SelectedIndexChanged
        Response.Redirect("HottipPage" + ddlFunctionList.SelectedValue + ".aspx?g=" + ddlGroupList.SelectedValue)
    End Sub

    Protected Sub lvMyCollect_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMyCollect.DataBound

        If lvMyCollect.Items.Count = 0 Then Exit Sub

        Dim collectStockNos As String = ""
        For Each row As ListViewDataItem In lvMyCollect.Items

            Dim ltlStockNo As Literal = row.FindControl("ltlStockNo")

        Next

    End Sub

End Class
