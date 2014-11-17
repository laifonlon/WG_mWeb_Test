Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Web

Partial Class topic
    Inherits System.Web.UI.Page


    Public cid As Integer
    Public pp As Integer

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        cid = KevinDataAccess.getQueryStringForInt("id")
        pp = KevinDataAccess.getQueryStringForInt("p", "up", 1)
        If cid = "23" Then
            Response.Redirect("http://www.wantgoo.com/45485.aspx")
            Exit Sub
        End If
        '@IsHide integer , 
        '@Start integer , 
        '@end integer , 
        '@clubid integer

        Dim p0 As New SqlParameter("@IsHide", SqlDbType.Int)
        p0.Direction = ParameterDirection.Input
        p0.Value = 0

        Dim p1 As New SqlParameter("@Start", SqlDbType.Int)
        p1.Direction = ParameterDirection.Input
        p1.Value = (pp - 1) * 20 + 1

        Dim p2 As New SqlParameter("@end", SqlDbType.Int)
        p2.Direction = ParameterDirection.Input
        p2.Value = pp * 20

        Dim p3 As New SqlParameter("@clubid", SqlDbType.Int)
        p3.Direction = ParameterDirection.Input
        p3.Value = cid


        If Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.None OrElse _
             Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Expired Then
            p0.Value = 0
        ElseIf Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Normal Then
            p0.Value = 0
        ElseIf Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Chairman OrElse _
            Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Administrator Then
            p0.Value = 1
        Else
            p0.Value = 0
        End If


        Dim p(3) As SqlParameter

        p(0) = p0
        p(1) = p1
        p(2) = p2
        p(3) = p3

        Dim dt As New DataTable()


        Try
            dt = DirectCast(System.Web.HttpContext.Current.Cache.Get("dt_ClubPAgeList_" + p0.Value.ToString() + "_" + p1.Value.ToString() + "_" + p2.Value.ToString() + "_" + p3.Value.ToString()), DataTable)
        Catch ex As Exception
            dt = Nothing
        End Try


        If dt Is Nothing OrElse dt.Rows.Count < 5 Then
            dt = KevinDataAccess.getSqlDataReturnDataTable(KevinDataAccess.str_Connstr_Club, "ClubListForMobile", CommandType.StoredProcedure, p)

            System.Web.HttpContext.Current.Cache.Insert("dt_ClubPAgeList_" + p0.Value.ToString() + "_" + p1.Value.ToString() + "_" + p2.Value.ToString() + "_" + p3.Value.ToString(), dt, Nothing, DateTime.UtcNow.AddMinutes(5), Cache.NoSlidingExpiration)

        End If

        If dt.Rows.Count > 0 Then
            Literal_title.Text = dt.Rows(0)("Name").ToString()

			Me.Header.Title = dt.Rows(0)("Name").ToString() + " - 社團 - 玩股網  http://m.wantgoo.com"
			 
            '<a href="#">上一頁</a></span><span class="pgs"><select name=""><option value="">9</option><option value="">10</option></select></span><span class="nt"><a href="#">下一頁</a>
            Dim page_sb As New StringBuilder(50)

            page_sb.Append("<span class=""pv"">")
            page_sb.Append("<a href=""/topic.aspx?id=" + cid.ToString() + "&p=1"">第一頁</a>")
            page_sb.Append("</span>")

            page_sb.Append("<span class=""pv"">")
            page_sb.Append("<a href=""/topic.aspx?id=" + cid.ToString() + "&p=" + IIf(pp <= 1, "1", (pp - 1).ToString()) + """>上一頁</a>")
            page_sb.Append("</span>")
            page_sb.Append("<span class=""pgs"">")
            page_sb.Append("<select name="""" onchange=""javascript:changepage('" + cid.ToString() + "',this.value)"">")

            Dim pagei As Integer = 5

            While pagei >= 1
                If pp - pagei >= 1 Then
                    page_sb.Append("<option value=""" + (pp - pagei).ToString() + """>" + (pp - pagei).ToString() + "</option>")

                End If
                pagei -= 1
            End While

            page_sb.Append("<option value=""" + pp.ToString() + """ selected>" + pp.ToString() + "</option>")

            pagei = 1
            While pagei <= 5
                If pp + pagei <= dt.Rows(0)("pagecounts") Then
                    page_sb.Append("<option value=""" + (pp + pagei).ToString() + """>" + (pp + pagei).ToString() + "</option>")

                End If
                pagei += 1
            End While

            page_sb.Append("</select>")
            page_sb.Append("</span>")
            page_sb.Append("<span class=""nt"">")
            page_sb.Append("<a href=""/topic.aspx?id=" + cid.ToString() + "&p=" + IIf(pp >= dt.Rows(0)("pagecounts"), dt.Rows(0)("pagecounts").ToString(), (pp + 1).ToString()) + """>下一頁</a>")
            page_sb.Append("</span>")


            Literal_pagelink.Text = page_sb.ToString()
            Literal_pagelink2.Text = page_sb.ToString()

            Dim main_sb As New StringBuilder(500)

            For i As Integer = 0 To dt.Rows.Count - 1
                Try

                    If dt.Rows(i)("isHide").ToString().ToLower() = "true" AndAlso _
                        (Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Chairman _
                         OrElse Club.Instance.GetStatus(cid, MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Administrator) Then

                        main_sb.Append("<li>")
                        main_sb.Append("<a href=""/topicdetail.aspx?p=1&id=" + cid.ToString() + "&tid=" + dt.Rows(i)("TopicID").ToString() + """>")
                        main_sb.Append("<h2 class=""tit"">" + IIf(dt.Rows(i)("isTop").ToString().ToLower() = "true", "[置頂]", "") + IIf(dt.Rows(i)("isPublic").ToString().ToLower() = "true", "[公開]", "") + IIf(dt.Rows(i)("isHide").ToString().ToLower() = "true", "[隱藏]", "") + dt.Rows(i)("text").ToString() + "</h2>")
                        main_sb.Append("<cite class=""dt"">" + Date.Parse(dt.Rows(i)("Time")).ToString("yyy-MM-dd hh:mm") + "</cite>")
                        main_sb.Append("</a>")
                        main_sb.Append("</li>")

                    ElseIf dt.Rows(i)("isHide").ToString().ToLower() = "false" Then
                        main_sb.Append("<li>")
                        main_sb.Append("<a href=""/topicdetail.aspx?p=1&id=" + cid.ToString() + "&tid=" + dt.Rows(i)("TopicID").ToString() + """>")
                        main_sb.Append("<h2 class=""tit"">" + IIf(dt.Rows(i)("isTop").ToString().ToLower() = "true", "[置頂]", "") + IIf(dt.Rows(i)("isPublic").ToString().ToLower() = "true", "[公開]", "") + dt.Rows(i)("text").ToString() + "</h2>")
                        main_sb.Append("<cite class=""dt"">" + Date.Parse(dt.Rows(i)("Time")).ToString("yyy-MM-dd hh:mm") + "</cite>")
                        main_sb.Append("</a>")
                        main_sb.Append("</li>")
                    End If

                Catch ex As Exception

                End Try

            Next
            Literal_main.Text = main_sb.ToString()
        End If

    End Sub
End Class
