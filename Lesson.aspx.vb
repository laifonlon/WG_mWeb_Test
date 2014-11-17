
Partial Class Lesson
    Inherits System.Web.UI.Page


    Private Function toOther(ByVal id As String) As Boolean
        Select Case id
            Case "99"
                Return True
        End Select
        Return False
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ConnectionString)
            connection.Open()

            Dim cmd As String = "SELECT id,shortaddress,name,starttime,endtime,teacher,mode, IsSpecial, ActionUrl FROM dbo.Courses where isShow=1 ORDER BY starttime"
            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
            command.ExecuteNonQuery()
            Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
            Dim dataSet As New System.Data.DataSet
            adapter.Fill(dataSet)

            Dim context As String = "<table class=""tblesson"">"

            For Each row As System.Data.DataRow In dataSet.Tables.Item(0).Rows
                Dim startTime As Date = row("starttime")
                Dim endtime As Date = row("endtime")
                Dim url As String = ""
                Try
                    If IsDBNull("ActionUrl") = False Then
                        url = row("ActionUrl").ToString
                    End If

                Catch ex As Exception

                End Try
                Dim isSpecial As Boolean = False
                If IsDBNull(row("IsSpecial")) = False Then
                    isSpecial = CBool(row("IsSpecial").ToString)
                End If
                Try
                    If endtime.Subtract(Now).TotalHours > 0 Then
                        If isSpecial = False Then
                            Dim mode As Integer = Val(row("mode"))
                            If mode = 1 Then
                                context += "<tr><td style=""width:65px;"">" + row("shortaddress").ToString + "</td><td><a href=""/lessoninfo.aspx?id=" + row("id").ToString + """>" + row("name").ToString + "</a></td><td style=""text-align:right;width:35px;font-size:12px;"">" + startTime.Month.ToString + "/" + startTime.Day.ToString + "<td/></tr>"
                            ElseIf mode = 0 Then
                                context += "<tr><td style=""width:65px;"">" + row("shortaddress").ToString + "</td><td><a href=""/lessoninfo.aspx?id=" + row("id").ToString + """>" + row("name").ToString + "</a></td><td style=""text-align:right;width:35px;font-size:12px;"">" + startTime.Month.ToString + "/" + startTime.Day.ToString + "<td/></tr>"
                            ElseIf mode = 2 AndAlso url = "" AndAlso startTime.Subtract(Now).TotalHours > 0 Then
                                context += "<tr><td style=""width:65px;"">" + row("shortaddress").ToString + "</td><td><a href=""/lessoninfo.aspx?id=" + row("id").ToString + """>" + row("name").ToString + "</a></td><td style=""text-align:right;width:35px;font-size:12px;"">" + startTime.Month.ToString + "/" + startTime.Day.ToString + "<td/></tr>"
                            End If
                        End If
                    End If
                Catch ex As Exception

                End Try

            Next

            Dim isOther As Boolean = False

            For Each row As System.Data.DataRow In dataSet.Tables.Item(0).Rows
                Dim starttime As Date = row("starttime")
                Dim endtime As Date = row("endtime")
                Dim mode As Integer = Val(row("mode"))

                Dim isSpecial As Boolean = False
                If IsDBNull(row("IsSpecial")) = False Then
                    '取消別人的課程廣告
                    'isSpecial = CBool(row("IsSpecial").ToString)
                End If

                If isSpecial = True Then
                    Dim url As String = ""
                    Try
                        If IsDBNull("ActionUrl") = False Then
                            url = row("ActionUrl").ToString
                        End If

                    Catch ex As Exception

                    End Try

                    If endtime.Subtract(Now).TotalHours > 0 Then
                        If isOther = False Then
                            context += "<tr><td><div style=""width:100%; height:1px; background-color:#C0C0C0; margin-top:5px; margin-buttom:5px;""></div></td><td><div style=""width:100%; height:1px; background-color:#C0C0C0; margin-top:5px; margin-buttom:5px;""></div></td><td><div style=""width:100%; height:1px; background-color:#C0C0C0; margin-top:5px; margin-buttom:5px;""></div></td></tr>"
                            isOther = True
                        End If
                        If url = "" Then
                            context += "<tr><td style=""width:65px;"">" + row("shortaddress").ToString + "</td><td><a href=""/mall/lesson/lessonbuy.aspx?id=" + row("id").ToString + """>" + row("name").ToString + "</a></td><td style=""text-align:right;width:35px;font-size:12px;"">" + starttime.Month.ToString + "/" + starttime.Day.ToString + "<td/></tr>"
                        Else
                            Dim time As String = starttime.Month.ToString + "/" + starttime.Day.ToString
                            Dim id As Integer = Val(row("id"))
                            Select Case id
                                Case 176
                                    time = "每週日"
                                Case 177
                                    time = "每週六"
                            End Select
                            context += "<tr><td style=""width:65px;"">" + row("shortaddress").ToString + "</td><td><a href=""" + url + """" + row("id").ToString + """>" + row("name").ToString + "</a></td><td style=""text-align:right;width:35px;"">" + time + "<td/></tr>"
                        End If
                    End If
                End If
            Next

            lblLesson.Text = context + "</table>"

            connection.Close()
            connection.Dispose()
        Catch ex As Exception
            If MemberDataAccessor.Instance.IsAdiminstrator Then
                Response.Write(ex.ToString)
            End If
        End Try

    End Sub
End Class
