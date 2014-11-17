
Partial Class Hottip_HottipPage2
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
        'Me.DisableColumn()

        Dim hasTAIEX As Boolean = False
        For Each row As ListViewDataItem In lvMyCollect.Items
            Dim lblSNo As Literal = row.FindControl("ltlStockNo")
            Dim marginadiff As Label = row.FindControl("lblmarginadiff")
            Dim shortdiff As Label = row.FindControl("lblshortdiff")
            Dim lblSumForeign As Label = row.FindControl("lblSumForeign")
            Dim lblSumForeigna As Label = row.FindControl("lblSumForeigna")
            Dim lblSumING As Label = row.FindControl("lblSumING")
            Dim lblSumINGa As Label = row.FindControl("lblSumINGa")
            Dim lblSumDealer As Label = row.FindControl("lblSumDealer")
            Dim lblSumDealera As Label = row.FindControl("lblSumDealera")

            If lblSNo.Text = "0000" Then hasTAIEX = True

            If Val(marginadiff.Text) > 0 Then
                marginadiff.ForeColor = Drawing.Color.FromArgb(141, 0, 0)
            ElseIf Val(marginadiff.Text) < 0 Then
                marginadiff.ForeColor = Drawing.Color.FromArgb(10, 127, 37)
            Else
            End If

            If Val(shortdiff.Text) > 0 Then
                shortdiff.ForeColor = Drawing.Color.FromArgb(141, 0, 0)
            ElseIf Val(shortdiff.Text) < 0 Then
                shortdiff.ForeColor = Drawing.Color.FromArgb(10, 127, 37)
            Else
            End If

            Dim stockno As String = lblSNo.Text
            'If IsNumeric(stockno) And stockno <> "0000" Then
            '    lblSumForeign.Visible = True
            '    lblSumING.Visible = True
            '    lblSumDealer.Visible = True
            'Else
            '    lblSumForeigna.Visible = True
            '    lblSumINGa.Visible = True
            '    lblSumDealera.Visible = True
            'End If
        Next

        If hasTAIEX = True Then
            Me.UpdateTAIEX()
        End If

    End Sub

    ''' <summary>
    ''' 更新加權指數的資券進出行情
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateTAIEX()
        For Each row As ListViewDataItem In lvMyCollect.Items
            Dim lblNumber As Literal = row.FindControl("ltlStockNo")

            If lblNumber.Text.Trim = "0000" Then
                Dim en As IEnumerator = sdsTAIEX.Select(New DataSourceSelectArguments).GetEnumerator
                If en.MoveNext Then
                    Dim lblMargina As Label = row.FindControl("lblmargina")
                    Dim lblMarginadiff As Label = row.FindControl("lblmarginadiff")
                    Dim lblMarginb As Label = row.FindControl("lblmarginb")
                    Dim lblShortdiff As Label = row.FindControl("lblshortdiff")
                    Dim lblMarginr As Label = row.FindControl("lblmarginr")
                    Dim lblDiff As Label = row.FindControl("lbldiff")
                    Dim lblDiffrate As Label = row.FindControl("lbldiffrate")
                    'Dim lblVolume As Label = row.FindControl("lblVolume")

                    lblMargina.Text = Format(en.Current("Today1") / 100000, "#,##0.0") & "(億)" '資餘
                    lblMarginadiff.Text = Format(en.Current("Diff1") / 100000, "#,##0.0") & "(億)"  '資增
                    lblMarginb.Text = Format(en.Current("Today2"), "#,##0")  '券餘
                    lblShortdiff.Text = Format(en.Current("Diff2"), "#,##0") '券增
                    lblMarginr.Text = "-" '券資比
                    lblDiff.Text = "-" '資券互抵
                    lblDiffrate.Text = "-" '當沖率
                    'lblVolume.Text = Format(en.Current("Volume") / 1000, "#,###,###,##0.0") & "(億)" '成交量
                End If

                Exit For
            End If
        Next
    End Sub

End Class
