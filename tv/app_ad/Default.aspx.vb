
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If MemberDataAccessor.Instance.IsAdiminstrator AndAlso MemberDataAccessor.Instance.GetMemberNo = "43602" Then Me.btnClear.Visible = True

        Dim key As String = "HomePage_Headline"
        If Cache(key) Is Nothing Then
            Cache.Add(key, Me.LoadHeadLine(), Nothing, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration, _
                                        System.Web.Caching.CacheItemPriority.Normal, Nothing)
        End If
        lHeadLine.Text = Cache(key)
    End Sub

    ''' <summary>
    ''' 頭版頭條
    ''' </summary>
    Private Function LoadHeadLine() As String
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Dim cmdTxt As String = "SELECT [Index], Image, Url, Title, [Content], BlogID, MemberNo FROM HeadLine"
        Dim command As New System.Data.SqlClient.SqlCommand(cmdTxt, connection)
        command.ExecuteNonQuery()
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)

        connection.Close()
        connection.Dispose()

        Dim templete As String = _
               "<li style=""width:100%;""><table class=""tbhead"" cellpadding=""0"" cellspacing=""0""><tr>" & _
               "<td class=""hl_img""><a href='@blogUrl'><img src=""@imageUrl"" width=""80px"" height=""80px"" /></a></td>" & _
               "<td class=""hl_c"">" & _
               "<table class=""hl_tb"">" & _
               "<tr><td><h3><a href='@blogUrl'>@Title</a></h3></td></tr></table>" & _
               "<div><a href='@blogUrl'>@Content</a></div>" & _
               "</td></tr></table></li>"

        Dim headline As String = ""
        For Each row As System.Data.DataRow In dataSet.Tables.Item(0).Rows
            headline &= templete.Replace("@blogUrl", "blog.aspx?bid=" & row("BlogID")).Replace("@imageUrl", row("Image")).Replace("@Title", row("Title")).Replace("@Content", row("Content"))
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
