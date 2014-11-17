
Partial Class LessonInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            LoadContext()

            'LoadContext1()

        End If
    End Sub

    'Private Sub LoadContext1()

    '    Dim connection As New System.Data.SqlClient.SqlConnection("Data Source=localhost\WantGoo;Initial Catalog=WANTGOO;Integrated Security=True;Max Pool Size=3000")
    '    connection.Open()

    '    Dim cmd As String = "SELECT shortaddress,name,description,starttime,endtime,teacher,actionlist,actiondescription,price,addressname,address,map,bigmap,traffic,note FROM dbo.Courses WHERE id=" + Val(Request("id")).ToString + "and mode='1' ORDER BY endTime DESC"
    '    Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
    '    command.ExecuteNonQuery()
    '    Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
    '    Dim dataSet As New System.Data.DataSet
    '    adapter.Fill(dataSet)

    '    For Each row As System.Data.DataRow In dataSet.Tables.Item(0).Rows

    '        lblShortAddress.Text = row("shortaddress").ToString
    '        lblName.Text = row("name").ToString
    '        lblDescription.Text = row("description").ToString
    '        lblTeacher.Text = row("teacher").ToString

    '        Dim startTime As Date = row("StartTime")
    '        Dim endTime As Date = row("EndTime")
    '        'lblActionTime.Text = startTime.Year.ToString + "年" + startTime.Month.ToString + "月" + startTime.Day.ToString + "日" + "(" + GetWeekString(startTime.DayOfWeek) + ")" + _
    '        '"&nbsp;" + GetStartHourString(startTime.Hour) + ":" + Format(startTime.Minute, "00") + "到" + GetEndHourString(endTime.Hour) + ":" + Format(endTime.Minute, "00") + "以後 課程長達" + endTime.Subtract(startTime).TotalHours.ToString + "個小時"

    '        lblActionTime.Text = "<span style=""color:#eb6100;"">" + _
    '        startTime.Year.ToString + "</span>年<span style=""color:#eb6100;"">" + _
    '        startTime.Month.ToString + "</span>月<span style=""color:#eb6100;"">" + _
    '        startTime.Day.ToString + "</span>日(" + GetWeekString(startTime.DayOfWeek) + ")<span style=""color:#eb6100;"">" + _
    '        GetStartHourString(startTime.Hour) + ":" + Format(startTime.Minute, "00") + "-" + GetEndHourString(endTime.Hour) + ":" + Format(endTime.Minute, "00") + _
    '        "以後</span>"

    '        Dim actioncontext As String = row("actionlist").ToString

    '        Dim actions As String() = actioncontext.Split("_")
    '        If actions.Length > 3 Then
    '            actioncontext = ""
    '            Dim index As Integer = 1
    '            For Each act As String In actions
    '                actioncontext &= index.ToString & "." & act & "<div></div>"
    '                index += 1
    '            Next
    '        End If

    '        If row("actiondescription").ToString <> "" Then
    '            lblActionList.Text = row("actiondescription").ToString + "<br/><br/>" + actioncontext
    '        Else
    '            lblActionList.Text = actioncontext
    '        End If

    '        lblActionList.Text = lblActionList.Text.Replace("<pre>", "")
    '        lblActionList.Text = lblActionList.Text.Replace("</pre>", "<br/>")
    '        lblActionList.Text = lblActionList.Text.Replace("pre", "")
    '        'lblActionList.Text = RemoveHtml(lblActionList.Text)

    '        lblPrice.Text = row("price").ToString

    '        lblAddressName.Text = row("addressname").ToString

    '        lblAddress.Text = row("address").ToString

    '        lblMap.Text = row("map").ToString.Replace("width=""425"" height=""350""", "width=""280"" height=""280""")

    '        'lblBigMap.Text = "<a href=""" + row("bigmap").ToString + """ style=""color:#0000FF;text-align:left"">檢視較大的地圖</a>"

    '        lblTraffic.Text = row("traffic").ToString

    '        lblNote.Text = row("note").ToString

    '        'lblCompany.Text = "玩股網"

    '        Exit For
    '    Next

    '    connection.Close()
    '    connection.Dispose()

    'End Sub

    Private Function GetStartHourString(ByVal hour As Integer) As String
        If hour > 12 Then
            Return "下午 " + (hour - 12).ToString
        End If
        Return "上午 " + hour.ToString
    End Function

    Private Function GetEndHourString(ByVal hour As Integer) As String
        If hour > 12 Then
            Return (hour - 12).ToString
        End If
        Return hour.ToString
    End Function

    Private Function GetWeekString(ByVal day As System.DayOfWeek) As String
        Select Case day
            Case System.DayOfWeek.Friday
                Return "星期五"
            Case System.DayOfWeek.Monday
                Return "星期一"
            Case System.DayOfWeek.Saturday
                Return "星期六"
            Case System.DayOfWeek.Sunday
                Return "星期日"
            Case System.DayOfWeek.Thursday
                Return "星期四"
            Case System.DayOfWeek.Tuesday
                Return "星期二"
            Case System.DayOfWeek.Wednesday
                Return "星期三"
        End Select
        Return ""
    End Function

    Private Function RemoveHtml(ByVal content As String) As String
        content = Regex.Replace(content, "<strong style[^>]*>", "")
        content = Regex.Replace(content, "<strong[^>]*>", "")
        content = content.Replace("</strong>", "")

        content = Regex.Replace(content, "<span[^>]*>", "")
        content = content.Replace("</span>", "")

        content = Regex.Replace(content, "<p style[^>]*>", "")
        content = Regex.Replace(content, "<p [^>]*>", "")
        content = content.Replace("</p>", "")

        content = Regex.Replace(content, "<b style[^>]*>", "")
        content = Regex.Replace(content, "<b [^>]*>", "")
        content = Regex.Replace(content, "<b>", "")
        content = content.Replace("</b>", "")

        content = content.Replace(" ", "")
        content = content.Replace("&nbsp;", "")
        content = content.Replace("<br>", "")
        content = content.Replace("<br/>", "")

        content = Regex.Replace(content, "style=", "")

        Return content
    End Function

    Private Sub LoadContext()

        Dim connection As New System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
        connection.Open()

        Dim cmd As String = "SELECT * FROM dbo.Courses WHERE id=" + Val(Request("id")).ToString
        Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
        command.ExecuteNonQuery()
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New System.Data.DataSet
        adapter.Fill(dataSet)

        Dim startTime As Date
        Dim minCount As Integer = 0
        Dim teacher As String = ""
        Dim teacherNo As String = ""
        Dim mode As Integer = 0
        Dim price As Integer = 0

        For Each row As System.Data.DataRow In dataSet.Tables.Item(0).Rows
            lblName.Text = row("name").ToString
            lblTeacher.Text = row("teacher").ToString

            lblTeacherBook.Text = RemoveHtml(row("teacherbook").ToString)
            lblTeacherNote.Text = RemoveHtml(row("teachernote").ToString)

            If lblTeacherBook.Text.Trim = "" Then
                pnlBook.Visible = False
            End If

            startTime = CDate(row("starttime"))
            Dim endTime As Date = CDate(row("endtime"))

            minCount = Val(row("MinCount"))

            teacher = row("teacher").ToString
            teacherNo = row("teacherno").ToString
            price = Val(row("Price"))
            mode = Val(row("mode"))

            lblActionTime.Text = "<span style=""color:#eb6100;"">" + _
            startTime.Year.ToString + "</span>年<span style=""color:#eb6100;"">" + _
            startTime.Month.ToString + "</span>月<span style=""color:#eb6100;"">" + _
            startTime.Day.ToString + "</span>日(" + GetWeekString(startTime.DayOfWeek) + ")<span>" + _
            GetStartHourString(startTime.Hour) + ":" + Format(startTime.Minute, "00") + "-" + GetEndHourString(endTime.Hour) + ":" + Format(endTime.Minute, "00") + _
            "以後</span>"

            Dim actioncontext As String = row("actionlist").ToString

            Dim actions As String() = actioncontext.Split("_")
            If actions.Length > 3 Then
                actioncontext = ""
                Dim index As Integer = 1
                For Each act As String In actions
                    actioncontext &= index.ToString & "." & act & "<div></div>"
                    index += 1
                Next
            End If

            If row("actiondescription").ToString <> "" Then
                lblActionList.Text = row("actiondescription").ToString + "<br/><br/>" + actioncontext
            Else
                lblActionList.Text = actioncontext
            End If

            lblActionList.Text = lblActionList.Text.Replace("<pre>", "")
            lblActionList.Text = lblActionList.Text.Replace("</pre>", "<br/>")
            lblActionList.Text = lblActionList.Text.Replace("pre", "")
            'lblActionList.Text = RemoveHtml(lblActionList.Text)

            lblAddressName.Text = row("address").ToString
            lblMapAddress.text = lblAddressName.Text
            lblMap.Text = row("map").ToString.Replace("width=""425"" height=""350""", "width=""300"" height=""300""")

            lblTraffic.Text = row("traffic").ToString

            lblNote.Text = row("note").ToString
 
            If mode = 1 Then
                If Val(row("price")) = 0 Then
                    lblPrice.Text = row("description")
                Else
                    lblPrice.Text = "現場繳費，僅收<span style=""color:red;"">" & row("price").ToString & "</span>塊場地費。"
                End If
                btnJoinTop.Visible = False

            ElseIf mode = 0 Then
                hlPrice.NavigateUrl = "http://www.wantgoo.com/mall/lesson/lessonbuy.aspx?id=" & Val(Request("id")).ToString
                hlPrice.Text = "點我到玩股網報名付費課程"
             End If

            'If CInt(row("mode")) = 1 Then
            '    btnJoinTop.Visible = False
            '    btnJoinDown.Visible = False
            '    btnJoin.Visible = False
            'End If
        Next
    End Sub

    Protected Sub btnJoinTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJoinTop.Click
        Response.Redirect("http://www.wantgoo.com/mall/lesson/lessonbuy.aspx?id=" + Val(Request("id")).ToString)
    End Sub

    'Protected Sub btnJoinDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJoinDown.Click
    '    Response.Redirect("http://www.wantgoo.com/mall/lesson/lessonbuy.aspx?id=" + Val(Request("id")).ToString)
    'End Sub

    'Protected Sub btnJoin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJoin.Click
    '    Response.Redirect("http://www.wantgoo.com/mall/lesson/lessonbuy.aspx?id=" + Val(Request("id")).ToString)
    'End Sub
End Class
