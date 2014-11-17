
Partial Class HomePage_PostViwer
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         
    End Sub

    Protected Sub lvBlog_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvBlog.DataBound
        If IsPostBack = True Then Exit Sub

        Dim count As Integer = 1

        Dim listView As ListView = sender
        For Each item In listView.Items
            Dim lblBlogId As Label = item.FindControl("lblBlogId")
            Dim lblArticleId As Label = item.FindControl("lblArticleId")
            Dim lblTopPic As Label = item.FindControl("lblTopPic")
            Dim lblMemberNo As Label = item.FindControl("lblMemberNo")
            Dim lblTagId As Label = item.FindControl("lblTagId")
            Dim lblTagName As Label = item.FindControl("lblTagName")
            Dim lblPublishTime As Label = item.FindControl("lblPublishTime")
            'Dim lblArticleText As Label = item.FindControl("lblArticleText")
            Dim lblIsSell As Label = item.FindControl("lblIsSell")
            Dim lblIsShareSell As Label = item.FindControl("lblIsShareSell")
            Dim lblPreviewWordCount As Label = item.FindControl("lblPreviewWordCount")
            Dim lblGood As Label = item.FindControl("lblGood")
            'Dim lblOutlineText As Label = item.FindControl("lblOutlineText")
            Dim lblAd As Label = item.FindControl("lblAd")
            Dim lblArticleTitle As Label = item.FindControl("lblArticleTitle")

            Dim recommandTime As String = ""
            Dim PublishDate As DateTime = lblPublishTime.Text
            Dim ts As TimeSpan = DateTime.Now.Subtract(PublishDate)
            If ts.Days > 0 Then
                recommandTime = ts.Days.ToString + " 天前&nbsp;"
            ElseIf ts.Hours > 0 Then
                recommandTime = ts.Hours.ToString + "時前&nbsp;"
            ElseIf ts.Minutes > 0 Then
                recommandTime = ts.Minutes.ToString + "分前&nbsp;"
            ElseIf ts.Milliseconds > 0 Then
                recommandTime = ts.Seconds.ToString + "秒前&nbsp;"
            End If

            Dim imgUrl As String = lblTopPic.Text
            If imgUrl = "" Then
                imgUrl = MemberDataAccessor.Instance.GetHeadPicUrl(lblMemberNo.Text)
            End If
            Dim lblImg As Literal = item.FindControl("lblImg")
            lblImg.Text = "<img src=""" + imgUrl + """ width=""80px"" height=""80px"" />"

            Dim lblArticleTitleText As Literal = item.FindControl("lblArticleTitleText")
            Dim strTitle As String = lblArticleTitle.Text
 

            Dim maxLen As Integer = 22 '設定標題長度
            Dim TitleResult As String = ""
            Dim counter As Integer = 0
            Dim chr As Char
            Dim RealLen As Integer = System.Text.Encoding.GetEncoding("Big5").GetBytes(strTitle).Length
            For Each chr In strTitle
                counter += System.Text.Encoding.GetEncoding("Big5").GetBytes(chr.ToString()).Length
                If counter <= maxLen Then
                    TitleResult = TitleResult + chr
                Else
                    Exit For
                End If
            Next
            If RealLen > maxLen Then
                TitleResult += "..."
            End If
            lblArticleTitleText.Text = TitleResult
 
            Dim lblText As Literal = item.FindControl("lblText")
            Dim content As String = "<a href=""" + "blog.aspx?bid=" + lblBlogId.Text + """ class=""pvtext"" style=""color:black;font-size:13px; line-height:16px;padding-top:5px;"">"

            Dim articletext As String = ""
            Dim outlinetext As String = ""

            GetData(lblBlogId.Text, articletext, outlinetext)

            Dim newText As String = ""
            If CBool(lblIsSell.Text) = True OrElse outlinetext.Length > 0 Then
                newText = Regex.Replace(System.Web.HttpUtility.HtmlDecode(outlinetext), "<(.|\n)*?>", "").Replace("...", "").Replace(vbCrLf, "").Replace(vbTab, "").Replace(" ", "").Replace("&nbsp;", "")
                content += Strings.Left(newText, 45).Replace("...", "")
            Else
                newText = Regex.Replace(System.Web.HttpUtility.HtmlDecode(articletext), "<(.|\n)*?>", "").Replace("...", "").Replace(vbCrLf, "").Replace(vbTab, "").Replace(" ", "").Replace("&nbsp;", "")
                content += Strings.Left(newText, 45).Replace("...", "")
            End If

            content += "...</a>"
          
            lblText.Text = content

            count += 1
        Next

    End Sub

    Private Sub GetData(ByVal blogid As String, ByRef articletext As String, ByRef outlinetext As String)
        Dim key1 As String = "PostView_articletext_" + blogid
        If Application(key1) IsNot Nothing AndAlso _
            Application(key1 + "LastTechTime") IsNot Nothing Then
            If Now < Application(key1 + "LastTechTime") Then
                articletext = Application(key1)
            End If
        End If

        Dim key2 As String = "PostView_outlinetext_" + blogid
        If Application(key2) IsNot Nothing AndAlso _
            Application(key2 + "LastTechTime") IsNot Nothing Then
            If Now < Application(key2 + "LastTechTime") Then
                outlinetext = Application(key2)
            End If
        End If

        If articletext <> "" AndAlso outlinetext <> "" Then
            Exit Sub
        End If

        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()
        Dim cmd As String = "SELECT ArticleText, Preview FROM Blog WHERE BlogId=" & blogid

        Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
        command.ExecuteNonQuery()
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)

        Dim members As New ArrayList

        For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
            Try
                articletext = row("ArticleText").ToString
                outlinetext = row("Preview").ToString
                 
                Application(key1) = articletext
                Application(key1 + "LastTechTime") = Now.AddMinutes(15)

                Application(key2) = outlinetext
                Application(key2 + "LastTechTime") = Now.AddMinutes(15)
            Catch ex As Exception

            End Try
        Next

        command.Dispose()
        connection.Close()
        connection.Dispose()
    End Sub
 
End Class
