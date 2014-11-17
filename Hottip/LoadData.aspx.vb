
Partial Class Hottip_LoadData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim f As String = Request("f")
        If f Is Nothing OrElse f.Trim = "" Then Exit Sub

        Select Case f.ToLower.Trim
            Case "groupname"
                Me.LoadGroupName()
        End Select

        If Request("url") IsNot Nothing Then
            Response.Redirect(Request("url").Replace("$", "&"))
        ElseIf Request.UrlReferrer IsNot Nothing Then
            Response.Redirect(Request.UrlReferrer.ToString)
        End If
    End Sub

#Region "個股追蹤的群組名稱"
    Private Sub LoadGroupName()
        Dim memberNo As String = MemberDataAccessor.Instance.GetMemberNo
        Dim names As Generic.List(Of String) = Me.LoadGroup(memberNo)
        Hottip.Instance.AddGroupName(memberNo, names)
    End Sub

    Private Function LoadGroup(memberNo As String) As Generic.List(Of String)
        Dim key As String = "HottipCollectGroup_" & memberNo
        Dim timeKey As String = key + "LastTechTime"

        If Application(key) IsNot Nothing AndAlso _
            Application(timeKey) IsNot Nothing AndAlso Now < Application(timeKey) Then
            Return Application(key)
        End If

        Application(key) = GetGroupNameFromDB(memberNo)
        Application(key + "LastTechTime") = Now.AddDays(1)

        Return Application(key)
    End Function

    Private Function GetGroupNameFromDB(memberNo As String) As Generic.List(Of String)
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()
        Dim cmd As String = "IF Not Exists (Select MemberNo From CollectGroup Where MemberNo = @MemberNo) " & _
                                                "INSERT INTO CollectGroup (MemberNo) VALUES (@MemberNo) " & _
                                                "ELSE  Select * From CollectGroup Where MemberNo = @MemberNo"
        cmd = cmd.Replace("@MemberNo", memberNo)

        Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        Try
            adapter.Fill(dataSet)
        Catch ex As Exception
        Finally
            command.Dispose()
            connection.Close()
            connection.Dispose()
        End Try
        If dataSet.Tables.Count = 0 Then Return New Generic.List(Of String)

        Dim groupNames As New Generic.List(Of String)
        For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
            If row("MemberNo") = memberNo Then
                For Each col As Data.DataColumn In row.Table.Columns
                    If col.DataType.IsValueType = False Then
                        Dim name As String = row(col.ColumnName)
                        groupNames.Add(name)
                    End If
                Next
            End If
        Next

        Return groupNames
    End Function
#End Region

End Class
