﻿Imports System.Data

Partial Class Services_book
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim blgoID As String = Val(Request("bid"))
        sdsArticle.SelectParameters("BId").DefaultValue = blgoID

        'lblFoucsCount.Text = GetFocusCount(GetBlogID())
        '增加瀏覽次數 
        If Not CountRecord.Instance.IsBlogViewed(blgoID) AndAlso _
            Not IsPostBack Then
            sdsArticle.UpdateParameters("BlogID").DefaultValue = blgoID
            sdsArticle.Update()
        End If
    End Sub

    Protected Sub fv_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles fv.DataBound
        If fv.DataItemCount = 0 Then Exit Sub
        Dim lArticleTitle As Label = fv.FindControl("lArticleTitle")
        Dim lArticleText As Label = fv.FindControl("lArticleText")

        lArticleText.Text = Me.LoadArticle

        Dim blgoID As Integer = Val(Request("bid"))
        Me.GetArticleList(blgoID)

        Dim lMemberNo As Label = fv.FindControl("lMemberNo")
        Dim lArticleID As Label = fv.FindControl("lArticleID")
        Dim fburl As String = "http://www.wantgoo.com/" + MemberDataAccessor.Instance.GetBlogName(lMemberNo.Text.Trim) + "/" + lArticleID.Text
        'lbllike1.Text = "<div class=""fb-like"" data-href=""" + fburl + """ data-send=""false"" data-layout=""button_count"" data-width=""90"" data-show-faces=""false""></div>"
        'lblGPlus.Text = "<g:plusone href=""" + "http://www.wantgoo.com/book.aspx?p=" & blgoID & """ size=""medium""></g:plusone>"

    End Sub

#Region "Load Article"
    Private Function LoadArticle() As String
        Dim arguments As New DataSourceSelectArguments
        Dim result As IEnumerator = sdsArticle.Select(arguments).GetEnumerator

        If result.MoveNext() Then
            Dim row As DataRowView = result.Current

            Dim title As String = row("ArticleTitle").ToString
            Me.Title = title & " 部落格文章 - 玩股網手機版"
            Dim nickName As String = "<a style=""color:#1057cc; text-decoration:none; font-size:13px;"" href=""http://www.wantgoo.com/" + row("MemberNo").ToString + """>點我看&nbsp;" + row("NickName").ToString.Trim + " 的部落格</a>"
            Dim publishTime As String = row("PublishTime").ToString
            Dim content As String = Server.HtmlDecode(row("ArticleText"))

            'Dim myReg As New Regex("<(?!(br)|(div)|(a)|(img)|(table)|(tr)|(td))[^>]*?>")
            'content = myReg.Replace(content, "")
            'content = content.Replace("&nbsp;", "")

            content = Regex.Replace(content, "<strong style[^>]*>", "")
            content = Regex.Replace(content, "<strong[^>]*>", "")
            content = content.Replace("</strong>", "")

            content = Regex.Replace(content, "<span[^>]*>", "")
            content = content.Replace("</span>", "")

            content = Regex.Replace(content, "<p style[^>]*>", "")
            content = Regex.Replace(content, "<p [^>]*>", "")
            ' content = content.Replace("</p>", "")

            content = Regex.Replace(content, "<b style[^>]*>", "")
            content = Regex.Replace(content, "<b [^>]*>", "")
            content = Regex.Replace(content, "<b>", "")
            'content = content.Replace("</b>", "")

            content = Regex.Replace(content, "style=", "")
            content = Regex.Replace(content, "href=""/", "href=""http://www.wantgoo.com/")
            content = DoImage(content)

            'content = Regex.Replace(content, "&nbsp;", "")

            Dim isLike As Boolean = CBool(row("IsSellShare"))

            Dim isSell As Boolean = CBool(row("IsSell"))

            Dim sellGold As Integer = CInt(row("Gold"))

            If isSell = True AndAlso sellGold > 0 Then

                'Dim url As String = "http://www.wantgoo.com/phone/IsBuyArticle.aspx?bd=" + Request("bid").ToString + "&id=" + Request("id").ToString + "&pw=" + Request("pw").ToString
                'Dim grabber As New WebPageGraber
                'Dim isBuyInfo As String = grabber.QueryWebPage(url, System.Text.Encoding.UTF8)

                'If isBuyInfo = "<wt>1</wt>" Then
                'Else
                '    Dim IsPreview As Boolean = CBool(row("IsPreview"))
                '    If IsPreview = True Then
                '        Dim previewcount As Integer = CInt(row("PreviewWordCount"))
                '        content = GetSellInfo(sellGold) + Left(content, previewcount) + "..."
                '    End If
                'End If

                Dim isPreview As Boolean = CBool(row("IsPreview"))
                If isPreview Then
                    Dim previewcount As Integer = CInt(row("PreviewWordCount"))
                    content = Server.HtmlDecode(row("Preview") + "其餘內容隱藏...<br/><br/>") + GetSellInfo(sellGold) + Left(content, previewcount)
                Else
                    content = "內容隱藏..."
                End If
            End If

            Return content
        End If
        Return ""
    End Function

    Private Function GetSellInfo(ByVal price As Integer) As String
        Dim result As String = ""

        result += "<table>"
        result += "<td>" + "普通會員" + "</td>"
        result += "<td>售價" + price.ToString + "好評" + "</td>"
        result += "</tr>"
        result += "<tr style=""color:red;"">"
        result += "<td>" + "好野會員" + "</td>"
        result += "<td>特價" + CInt(price * 0.8).ToString + "好評" + "</td>"
        result += "</tr>"
        result += "</table>"

        Return result
    End Function

#Region "Image Resize"
    Private Function DoImage(ByVal txt As String) As String
        Try
            Dim sIndexs As ArrayList = GetImageStartIndexs(txt)
            Dim preEIndexs As ArrayList = GetImageEndIndexs(txt)

            Dim eIndexs As New ArrayList
            For index As Integer = 0 To sIndexs.Count - 1
                Dim isOK As Boolean = False
                For Each eIndex As Integer In preEIndexs
                    If isOK = False AndAlso eIndex > sIndexs(index) Then
                        eIndexs.Add(eIndex)
                        isOK = True
                    End If
                Next
            Next

            Dim imgs As ArrayList = GetImageString(txt, sIndexs, eIndexs)
            Dim links As New ArrayList

            For Each str As String In imgs
                Dim fitStr As String = str
                links.Add(GenerateLink(fitStr))
            Next

            Dim orgs As New ArrayList
            Dim s As Integer = 0
            Dim subLength As Integer = 1
            For index As Integer = 0 To sIndexs.Count - 1
                If index - 1 > -1 Then
                    s = eIndexs(index - 1)
                End If
                subLength = sIndexs(index) - s
                orgs.Add(txt.Substring(s, subLength))
            Next

            Dim endIndex As Integer = eIndexs(sIndexs.Count - 1)
            Dim count As Integer = 1
            While endIndex <= sIndexs(sIndexs.Count - 1)
                endIndex = eIndexs(sIndexs.Count - 1 + count)
                count += 1
            End While

            subLength = endIndex
            If endIndex + 1 < txt.Length Then
                orgs.Add(txt.Substring(endIndex, txt.Length - endIndex))
            End If

            Dim newTxt As String = ""
            For index As Integer = 0 To orgs.Count - 1
                If links.Count > index Then
                    newTxt &= orgs(index) & links(index)
                Else
                    newTxt &= orgs(index)
                End If
            Next

            If txt.Trim.Length > 0 Then
                If newTxt.Length = 0 Then
                    Return txt
                End If
            End If

            Return newTxt
        Catch ex As Exception
            Return txt
        End Try
        Return txt
    End Function

    Private Function GetImageStartIndexs(ByVal txt As String) As ArrayList
        Dim imgS As New ArrayList

        For index As Integer = 0 To txt.Length - 1
            If index + 4 < txt.Length Then
                If txt.Substring(index, 5) = "<img " Then
                    imgS.Add(index)
                End If
            End If
        Next

        Return imgS
    End Function

    Private Function GetImageEndIndexs(ByVal txt As String) As ArrayList
        Dim imgE As New ArrayList

        For index As Integer = 0 To txt.Length - 1
            If index < txt.Length Then
                If txt.Substring(index, 1) = ">" Then
                    imgE.Add(index + 1)
                End If
            End If
        Next

        Return imgE
    End Function

    Private Function GetImageString(ByVal txt As String, ByVal sIndexs As ArrayList, ByVal eIndexs As ArrayList) As ArrayList
        Dim results As New ArrayList
        For index As Integer = 0 To sIndexs.Count - 1
            Dim endIndex As Integer = eIndexs(index)
            Dim count As Integer = 1
            While endIndex <= sIndexs(index)
                endIndex = eIndexs(index + count)
                count += 1
            End While
            Dim result As String = txt.Substring(sIndexs(index), endIndex - sIndexs(index))

            If result.Contains("/UpdateFiles/") AndAlso result.Contains("www.wantgoo.com/UpdateFiles/") = False Then
                result = result.Replace("/UpdateFiles/", "http://www.wantgoo.com/UpdateFiles/")
            End If

            results.Add(result)
        Next
        Return results
    End Function

    Private Function GenerateLink(ByVal txt As String) As String
        Dim s As Integer = 0
        Dim e As Integer = 0

        Dim isSrc As Boolean = False

        For index As Integer = 0 To txt.Length - 1
            If index + 3 < txt.Length Then
                If txt.Substring(index, 4) = "src=" Then
                    s = index
                    isSrc = True
                End If
            End If
            If isSrc = True Then
                If index < txt.Length Then
                    If txt.Substring(index, 1) = ">" Then
                        e = index + 1
                    End If
                End If
            End If
        Next

        Dim lastIndex As Integer = txt.IndexOf("/>")
        If lastIndex = -1 Then lastIndex = txt.IndexOf(">")

        Dim src As String = txt.Substring(s + 4, lastIndex - s - 4)

        Dim newIMG As String = txt.Substring(0, lastIndex)

        Dim styleIndex As Integer = newIMG.IndexOf("style=")

        If styleIndex > 0 Then
            newIMG = newIMG.Substring(0, styleIndex)
        End If

        If newIMG.Contains("src=") = False Then
            newIMG = newIMG & ("src=" & src)
        End If

        Dim link As String = ""

        Dim isFck As Boolean = False
        Dim srcs As String() = src.Split("/")
        For Each symbol As String In srcs
            If symbol.Trim.ToLower = "fckeditor" Then
                isFck = True
                Exit For
            End If
        Next

        If isFck = True Then
            link = "<div></div><a href=" + src + "><img src=" & src & " onload=""javascript:ReImgWid(this)"" /></a><div></div>"
        Else
            link = "<div></div><a href=" + src + "><img  src=" & src & " onload=""javascript:ReImgWid(this)"" /></a><div></div>"
        End If

        Return link
    End Function
#End Region
#End Region

    Private Sub GetArticleList(ByVal blogID As Integer)
        Dim maxBlogID As Integer = blogID + 5
        Dim minBlogID As Integer = blogID - 50
        sdsArticleList.SelectCommand = "SELECT TOP(9) Blog.BlogID, Blog.MemberNo, Blog.ArticleID, substring(Blog.ArticleTitle,1,20) as ArticleTitle , Blog.ArticleText, Blog.PublishTime, Blog.ViewCount, Blog.RecommendCount, Blog.Show, substring(Member.NickName,1,5) as NickName FROM Blog INNER JOIN Member ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND  (Member.IshideBlog <> 1) AND (Blog.IsSell = 0) AND (Blog.BlogID <" & maxBlogID & " AND Blog.BlogID >" & minBlogID & "AND Blog.BlogID<>" & blogID & ") ORDER BY Blog.PublishTime DESC"
    End Sub
End Class
