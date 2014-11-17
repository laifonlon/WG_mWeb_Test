Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Web

Partial Class topicdetail
    Inherits System.Web.UI.Page


    Public cid As Integer
    Public tid As Integer
    Public pp As Integer

    Public adminParam As String = "no"
    Public membernoParam As String = "no"
    Public mNo As Integer = 0

    Public cacheID As String = ""

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        cid = KevinDataAccess.getQueryStringForInt("id")
        tid = KevinDataAccess.getQueryStringForInt("tid")
        pp = KevinDataAccess.getQueryStringForInt("p")
        If pp < 1 Then
            pp = 1
        End If
        If cid = "23" Then
            Response.Redirect("http://www.wantgoo.com/45485.aspx")
            Exit Sub
        End If

        If tid < 1 Then
            Response.Redirect("/club.aspx")
        End If




        If Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Chairman OrElse _
                MemberDataAccessor.Instance.IsAdiminstrator Then
            adminParam = "yes"
            membernoParam = "yes"
        ElseIf Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Normal Then
            adminParam = "no"
            membernoParam = "yes"
        ElseIf Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.None OrElse _
         Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Expired Then

            ' "<div class=""emptydata"">您沒有閱讀權限，請先加入社團。 <a href=""/club/join.aspx?c=" + cid + """>點我加入社團</a></div>"
            adminParam = "no"
            membernoParam = "no"
        ElseIf Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Stop Then
            If Club.Instance.GetStopDeadline(cid, MemberDataAccessor.Instance.GetMemberNo) > Now Then

                ' "<div class=""emptydata"">您的社團會期暫停中。</div>"
                adminParam = "no"
                membernoParam = "stop"
            Else
                adminParam = "no"
                membernoParam = "no"
            End If
        End If

        mNo = MemberDataAccessor.Instance.GetMemberNo

        '   Response.Write(mNo.ToString() + "-------")

        If (adminParam = "yes" OrElse membernoParam = "yes") AndAlso mNo > 0 Then
            msgTxt.Enabled = True
            msgTxt.Visible = True
            btnPost.Enabled = True
            btnPost.Visible = True
        Else
            msgTxt.Enabled = False
            msgTxt.Visible = False
            btnPost.Enabled = False
            btnPost.Visible = False
        End If
        'Create Proc showClubTopicDetailForMobile
        '(
        '	@clubid integer , 
        '	@topicid integer , 
        '	@admin nvarchar(10) , 
        '	@memberno nvarchar(10) , 
        '	@start int , 
        '	@end int
        ')

        'exec showClubTopicDetailForMobile 9 , 15655,'yes','yes',1,20

        Dim p0 As New SqlParameter("@clubid", SqlDbType.Int)
        p0.Direction = ParameterDirection.Input
        p0.Value = cid

        Dim p1 As New SqlParameter("@topicid", SqlDbType.Int)
        p1.Direction = ParameterDirection.Input
        p1.Value = tid

        Dim p2 As New SqlParameter("@admin", SqlDbType.NVarChar, 10)
        p2.Direction = ParameterDirection.Input
        p2.Value = adminParam

        Dim p3 As New SqlParameter("@memberno", SqlDbType.NVarChar, 10)
        p3.Direction = ParameterDirection.Input
        p3.Value = membernoParam

        Dim p4 As New SqlParameter("@Start", SqlDbType.Int)
        p4.Direction = ParameterDirection.Input
        p4.Value = (pp - 1) * 10 + 1

        Dim p5 As New SqlParameter("@end", SqlDbType.Int)
        p5.Direction = ParameterDirection.Input
        p5.Value = pp * 10

        Dim p(5) As SqlParameter
        p(0) = p0
        p(1) = p1
        p(2) = p2
        p(3) = p3
        p(4) = p4
        p(5) = p5


        Dim dt As New DataTable

        cacheID = "dt_ClubTopicDetail_" + p0.Value.ToString() + "_" + p1.Value.ToString() + "_" + p2.Value.ToString() + "_" + p3.Value.ToString() + "_" + p4.Value.ToString() + "_" + p5.Value.ToString()

        If Me.IsPostBack = False Then

            Try
                dt = DirectCast(System.Web.HttpContext.Current.Cache.Get("dt_ClubTopicDetail_" + p0.Value.ToString() + "_" + p1.Value.ToString() + "_" + p2.Value.ToString() + "_" + p3.Value.ToString() + "_" + p4.Value.ToString() + "_" + p5.Value.ToString()), DataTable)
            Catch ex As Exception
                dt = Nothing
            End Try

            If dt Is Nothing OrElse dt.Rows.Count < 1 Then
                dt = KevinDataAccess.getSqlDataReturnDataTable(KevinDataAccess.str_Connstr_Club, "showClubTopicDetailForMobile", CommandType.StoredProcedure, p)

                System.Web.HttpContext.Current.Cache.Insert("dt_ClubTopicDetail_" + p0.Value.ToString() + "_" + p1.Value.ToString() + "_" + p2.Value.ToString() + "_" + p3.Value.ToString() + "_" + p4.Value.ToString() + "_" + p5.Value.ToString(), dt, Nothing, DateTime.UtcNow.AddSeconds(5), Cache.NoSlidingExpiration)

            End If

            If dt.Rows.Count > 0 Then
                Literal_ClubName.Text = "<a href=""/topic.aspx?p=1&id=" + cid.ToString() + """>" + dt.Rows(0)("ClubName").ToString() + "</a>"
                LiteralTopicTitle.Text = dt.Rows(0)("TopicTitle").ToString()

				Me.Header.Title = dt.Rows(0)("TopicTitle").ToString() + " - " + dt.Rows(0)("ClubName").ToString() + " - 社團 - 玩股網  http://m.wantgoo.com"

                Dim sb As New StringBuilder(500)

                For i As Integer = 0 To dt.Rows.Count - 1

                    sb.Append("<li>")
                    sb.Append("<div class=""art-info"">")
                    sb.Append("<span class=""hdpic""><img src=""" + dt.Rows(i)("pics").ToString() + """ alt="""" width=""45"" height=""45"" /></span>")
                    sb.Append("<h4 class=""usr"">" + dt.Rows(i)("nickname").ToString() + "</h4>")
                    sb.Append("<cite class=""rdt"">" + DirectCast(dt.Rows(i)("Time"), Date).ToString("yyyy-MM-dd HH:mm") + " #" + (i + 1).ToString() + "</cite> ")
                    sb.Append("</div>")
                    sb.Append("<div class=""art-inner"">")
                    sb.Append("<p>" + System.Web.HttpUtility.HtmlDecode(dt.Rows(i)("Text").ToString()) + "</p>")
                    sb.Append("</div>")
                    sb.Append("</li>")
                Next

                If dt.Rows.Count = 1 And dt.Rows(0)("Text").ToString().Trim() = "" Then
                    msgTxt.Enabled = False
                    msgTxt.Visible = False
                    btnPost.Enabled = False
                    btnPost.Visible = False
                End If
                LiteralTopicContent.Text = sb.ToString()



                Dim page_sb As New StringBuilder(50)

                'page_sb.Append("<span class=""pv"">")
                'page_sb.Append("<a href=""/topicdetail.aspx?tid=" + tid.ToString() + "&id=" + cid.ToString() + "&p=1"">第一頁</a>")
                'page_sb.Append("</span>")

                page_sb.Append("<span class=""pv"">")
                page_sb.Append("<a href=""/topicdetail.aspx?tid=" + tid.ToString() + "&id=" + cid.ToString() + "&p=" + IIf(pp <= 1, "1", (pp - 1).ToString()) + """>上一頁</a>")
                page_sb.Append("</span>")
                page_sb.Append("<span class=""pgs"">")
                page_sb.Append("<select name="""" onchange=""javascript:changepage('" + cid.ToString() + "'," + tid.ToString() + ",this.value)"">")


                For i As Integer = 1 To dt.Rows(0)("pagecounts")
                    page_sb.Append("<option value=""" + (i).ToString() + """  " + IIf(i = pp, "selected", "") + ">" + (i).ToString() + "</option>")

                Next
                'Dim pagei As Integer = 5

                'While pagei >= 1
                '    If pp - pagei >= 1 Then

                '    End If
                '    pagei -= 1
                'End While

                'page_sb.Append("<option value=""" + pp.ToString() + """ selected>" + pp.ToString() + "</option>")

                'pagei = 1
                'While pagei <= 5
                '    If pp + pagei <= dt.Rows(0)("pagecounts") Then
                '        page_sb.Append("<option value=""" + (pp + pagei).ToString() + """>" + (pp + pagei).ToString() + "</option>")

                '    End If
                '    pagei += 1
                'End While

                page_sb.Append("</select>")
                page_sb.Append("</span>")
                page_sb.Append("<span class=""nt"">")
                page_sb.Append("<a href=""/topicdetail.aspx?tid=" + tid.ToString() + "&id=" + cid.ToString() + "&p=" + IIf(pp >= dt.Rows(0)("pagecounts"), dt.Rows(0)("pagecounts").ToString(), (pp + 1).ToString()) + """>下一頁</a>")
                page_sb.Append("</span>")

                page_sb.Append("<span class=""pv"">")
                page_sb.Append("<a href=""/topicdetail.aspx?tid=" + tid.ToString() + "&id=" + cid.ToString() + "&p=" + dt.Rows(0)("pagecounts").ToString() + """>最後一頁</a>")
                page_sb.Append("</span>")

                Literal_pagelink.Text = page_sb.ToString()
                Literal_pagelink2.Text = page_sb.ToString()
            End If
        End If


    End Sub

    Protected Sub btnPost_Click(sender As Object, e As System.EventArgs) Handles btnPost.Click
        Dim msg As String = KevinDataAccess.checkSQL(msgTxt.Text)
        If msg.Length > 1000 Then
            msg = msg.Substring(0, 1000)
        End If
        mNo = MemberDataAccessor.Instance.GetMemberNo

        If (adminParam = "yes" Or membernoParam = "yes") AndAlso mNo > 0 Then


            Dim p0 As New SqlParameter("@clubid", SqlDbType.Int)
            p0.Direction = ParameterDirection.Input
            p0.Value = cid

            Dim p1 As New SqlParameter("@topicid", SqlDbType.Int)
            p1.Direction = ParameterDirection.Input
            p1.Value = tid

            Dim p2 As New SqlParameter("@memberno", SqlDbType.Int)
            p2.Direction = ParameterDirection.Input
            p2.Value = mNo

            Dim p3 As New SqlParameter("@msg", SqlDbType.NVarChar, 1000)
            p3.Direction = ParameterDirection.Input
            p3.Value = msg

            Dim p4 As New SqlParameter("@superadmin", SqlDbType.NVarChar, 10)
            p4.Direction = ParameterDirection.Input
            p4.Value = IIf(mNo = 119 Or mNo = 356, "true", "")

            Dim p(4) As SqlParameter
            p(0) = p0
            p(1) = p1
            p(2) = p2
            p(3) = p3
            p(4) = p4
            KevinDataAccess.executeSqlAndNoReturn(KevinDataAccess.str_Connstr_Club, "addClubTopicDetailForMobile", CommandType.StoredProcedure, p)

            If cacheID <> "" Then
                System.Web.HttpContext.Current.Cache.Remove(cacheID)
            End If

            Response.Redirect(Request.Url.AbsoluteUri)
        End If
        'Create Proc [dbo].[addClubTopicDetailForMobile]
        '(
        '	@clubid int , 
        '	@topicid int ,  
        '	@memberno int , 
        '	@msg nvarchar(1000)
        ')

    End Sub
End Class
