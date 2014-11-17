
Partial Class Default2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If MemberDataAccessor.Instance.IsAdiminstrator AndAlso MemberDataAccessor.Instance.GetMemberNo = "43602" Then Me.btnClear.Visible = True

        Dim key As String = "HomePage_Headline"
        If Cache(key) Is Nothing Then
            Cache.Add(key, Me.LoadHeadLine(), Nothing, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration, _
                                        System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If
        lHeadLine.Text = Cache(key)
    End Sub

    ''' <summary>
    ''' 頭版頭條
    ''' </summary>
    Private Function LoadHeadLine() As String
        Dim headline As String = ""

        Dim file As String = "C:\WantGoo\Wantgoooooo1\HomePage\headline.txt"
        Dim fileText As String = My.Computer.FileSystem.ReadAllText(file)
        Dim splitKey() As String = {"<=END=>"}
        Dim fileTexts() As String = fileText.Split(splitKey, StringSplitOptions.RemoveEmptyEntries)

        For Each txt As String In fileTexts
            If txt.Trim.Length > 0 Then
                Dim txts() As String = txt.Trim.Split(vbCrLf)

                Dim imageUrl, blogUrl, title, context, idFB, idGPlus As String
                For index As Integer = 0 To txts.Length - 1
                    Dim data As String = txts(index).Trim
                    Select Case index
                        Case 0 '圖片
                            imageUrl = data
                        Case 1 '部落格網址
                            blogUrl = data
                            'idFB = Me.GetFB(data)
                            'idGPlus = Me.GetGPlus(data)
                        Case 2 '標題
                            title = data
                        Case 3 '內容
                            context = GetLimitText(data, 160)
                        Case 4 'BlogID
                            blogUrl = "blog.aspx?bid=" & data
                    End Select
                Next

                Dim templete As String = _
                       "<li style=""width:100%;""><table class=""tbhead"" cellpadding=""0"" cellspacing=""0""><tr>" & _
                       "<td class=""hl_img""><a href='" & blogUrl & "'><img src=""" & imageUrl & """ width=""80px"" height=""80px"" /></a></td>" & _
                       "<td class=""hl_c"">" & _
                       "<table class=""hl_tb"">" & _
                       "<tr><td><h3><a href='" & blogUrl & "'>" & title & "</a></h3>" & _
                       "</td><td>" & idFB & "</td>" & _
                       "<td>" & idGPlus & "</td></tr></table>" & _
                       "<div>" & _
                       "<a href='" & blogUrl & "'>" & context & "</a></div>" & _
                       "</td></tr></table></li>"

                headline &= templete
            End If
        Next
        Return headline
    End Function

    Private Function GetLimitText(oldText As String, maxLengeth As Integer) As String
        Dim newText As String = ""
        Dim counter As Integer = 0
        Dim chr As Char
        Dim RealLen As Integer = System.Text.Encoding.GetEncoding("Big5").GetBytes(oldText).Length
        For Each chr In oldText
            counter += System.Text.Encoding.GetEncoding("Big5").GetBytes(chr.ToString()).Length
            If counter <= maxLengeth Then
                newText = newText + chr
            Else
                Exit For
            End If
        Next
        If RealLen > maxLengeth Then
            newText += "..."
        End If
        Return newText
    End Function

End Class
