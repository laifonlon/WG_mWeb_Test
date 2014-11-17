Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Web

Partial Class ClubM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim club_dt As New DataTable

        Dim thisClub As String = ""

        Dim j As Integer = 1

        club_dt = ClubClassForMobile.ClubHomeList()

		Me.Header.Title = "社團 - 玩股網  http://m.wantgoo.com"
		
        Dim sb As New StringBuilder(500)
        For i As Integer = 0 To club_dt.Rows.Count - 1

            If ((club_dt.Rows(i)("ClubID") = "11" Or club_dt.Rows(i)("ClubID") = "15" Or club_dt.Rows(i)("ClubID") = "16" Or club_dt.Rows(i)("ClubID") = "19") _
                    AndAlso (Club.Instance.GetStatus(club_dt.Rows(i)("ClubID"), MemberDataAccessor.Instance.GetMemberNo) = ClubStatus.Normal OrElse _
                 MemberDataAccessor.Instance.IsAdiminstrator)) OrElse (club_dt.Rows(i)("ClubID") <> "11" AndAlso club_dt.Rows(i)("ClubID") <> "15" AndAlso club_dt.Rows(i)("ClubID") <> "16" AndAlso club_dt.Rows(i)("ClubID") <> "19") Then

                If thisClub <> club_dt.Rows(i)("ClubID").ToString() Then
                    j = 1
                    thisClub = club_dt.Rows(i)("ClubID").ToString()

                    sb.Append("<li>")
                    sb.Append("<div class=""wgclub-br"">")
                    sb.Append("<span class=""hdpic""><a href=""/topic.aspx?p=1&id=" + club_dt.Rows(i)("ClubID").ToString() + """><img src=""" + club_dt.Rows(i)("pics") + """ alt="""" width=""100"" height=""100"" /></a></span>")
                    sb.Append("<h2 class=""tit2""><a href=""/topic.aspx?p=1&id=" + club_dt.Rows(i)("ClubID").ToString() + """ title=""" + club_dt.Rows(i)("Name").ToString() + """>" + club_dt.Rows(i)("Name").ToString() + "</a></h2>")
                End If

                If j <= 3 Then
                    sb.Append("<h3 class=""tit3""><a href=""/topicDetail.aspx?p=1&id=" + club_dt.Rows(i)("ClubID").ToString() + "&tid=" + club_dt.Rows(i)("TopicID").ToString() + """>" + club_dt.Rows(i)("Text").ToString() + "</a></h3>")
                End If

                If j = 3 Then
                    sb.Append("</div>")
                    sb.Append("</li>")
                End If

                j += 1

            End If

        Next


        Literal_ClubHome.Text = sb.ToString()
    End Sub
End Class
