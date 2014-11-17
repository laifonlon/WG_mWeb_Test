
Partial Class Hottip_StockClassSimple
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing OrElse stockNo.Trim = "" Then Exit Sub
        stockNo = stockNo.Replace("$", "&").Trim

        Dim classIDs As New Generic.Dictionary(Of String, String)
        Dim classId As String = Me.GetStockClassID(stockNo, classIDs)
        If classId = "" Then
            Me.Visible = False
            Exit Sub
        End If
        sdsCollect.SelectParameters("ClassId").DefaultValue = classId
        sdsRelate.SelectParameters("StockNo").DefaultValue = stockNo

        'Dim key As String = "Hottip_StockClassSimple_" & stockNo
        'If Application(key) IsNot Nothing AndAlso _
        '    Application(key + "LastTechTime") IsNot Nothing Then
        '    If Now < Application(key + "LastTechTime") Then
        '        lblSameId.Text = Application(key)
        '        Exit Sub '會離開 Sub
        '    End If
        'End If

        'Me.AddSameClassID(classIDs)

        'Application(key) = lblSameId.Text
        'If DateDiff(DateInterval.Second, Application(key + "LastTechTime"), Now) > 10 Then
        '    Application(key + "LastTechTime") = Now.AddSeconds(10)
        'End If
    End Sub

    Private Function GetStockClassID(ByVal stockNo As String, ByRef classIDs As Generic.Dictionary(Of String, String)) As String
        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
        connection.Open()

        Dim cmdTxt As String = "SELECT Class.ClassId, StockClass.StockNo, Class.[Group], Class.Name, Class.ShortName, Class.ChangeDay As ChangeValue  FROM Class INNER JOIN StockClass ON Class.ClassId = StockClass.ClassId WHERE (StockClass.StockNo = '" & stockNo & "') AND (Class.ClassId >= 11 AND Class.ClassId < 108) ORDER BY Class.[Group] DESC"
        Dim classID As String = Me.GetMinIDFromDB(cmdTxt, connection)

        cmdTxt = cmdTxt.Replace("AND (Class.ClassId >= 11 AND Class.ClassId < 108)", "AND (Class.ClassId >1 AND Class.ClassId <= 136)")
        classIDs = Me.GetIDsFromDB(cmdTxt, connection)

        connection.Close()
        connection.Dispose()
        Return classID
    End Function

    ''' <summary>
    ''' 取得同類個股中最小的ClassID
    ''' </summary>
    Private Function GetMinIDFromDB(ByVal cmdTxt As String, connection As System.Data.SqlClient.SqlConnection) As String
        Dim command As New System.Data.SqlClient.SqlCommand(cmdTxt, connection)
        command.ExecuteNonQuery()
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)

        Dim classID As String = ""
        Dim index As Integer = Integer.MaxValue
        Dim eleIndex As Integer = -1
        For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
            Dim rowClassID As Integer = row("ClassId")
            If rowClassID < index Then
                index = rowClassID
                'classIDs.Add(index & "," & row("Name"), Format(row("ChangeValue"), "0.00%").ToString)

                '電子股
                If index >= 68 AndAlso index <= 107 Then
                    eleIndex = rowClassID
                End If
            End If
        Next
        If eleIndex <> -1 Then
            index = eleIndex
        End If

        If index <> Integer.MaxValue Then
            classID = index.ToString
        End If
        Return classID
    End Function

    ''' <summary>
    ''' 取得所有同類個股
    ''' </summary>
    Private Function GetIDsFromDB(ByVal cmdTxt As String, connection As System.Data.SqlClient.SqlConnection) As Generic.Dictionary(Of String, String)
        Dim command As New System.Data.SqlClient.SqlCommand(cmdTxt, connection)
        command.ExecuteNonQuery()
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dataSet As New Data.DataSet
        adapter.Fill(dataSet)

        Dim classIDs As New Generic.Dictionary(Of String, String)
        For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
            If IsNumeric(row("ChangeValue")) Then
                classIDs.Add(row("ClassId") & "," & row("ShortName"), Format(row("ChangeValue"), "0.00%").ToString)
            End If
        Next
        Return classIDs
    End Function

    Protected Sub gvCollect_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCollect.DataBound
        Me.SetPriceColor(gvCollect)
    End Sub

    ' ''' <summary>
    ' ''' 加入分類行情
    ' ''' </summary>
    'Private Sub AddSameClassID(ByVal classIDs As Generic.Dictionary(Of String, String))
    '    If classIDs.Count = 0 Then pnlclass.Visible = False
    '    If classIDs.Count = 0 Then Exit Sub

    '    Dim title As String = "<table  cellpadding=""0"" cellspacing=""0"" class=""gvtable"" style=""width:265px;"">"
    '    Dim templete As String = "<tr class=""gvtable_row"" style=""height:20px;"">" + _
    '        "<td><a href=""/stock/classcont.aspx?id=@IDLink1"" >@IDName1</a></td>" + _
    '        "<td><span style=""color:@IDColor1;"">@IDChange1</span></td>" + _
    '        "<td><a href=""/stock/classcont.aspx?id=@IDLink2"" >@IDName2</a></td>" + _
    '        "<td><span style=""color:@IDColor2;"">@IDChange2</span></td></tr>"

    '    Dim current As String = templete
    '    Dim index As String = ""
    '    Dim count As Integer = 0
    '    For Each idKey As String In classIDs.Keys
    '        count += 1
    '        If current.Contains("@IDName2") Then index = "2"
    '        If current.Contains("@IDName1") Then index = "1"

    '        Dim change As String = classIDs(idKey)
    '        Dim idColor As String = ""
    '        If change.Contains("-") Then
    '            change = "▼" + change.Trim("-")
    '            idColor = "Green" '跌
    '        Else
    '            change = "▲" + change
    '            idColor = "Red" '漲
    '        End If

    '        Dim keys() As String = idKey.Split(",")
    '        current = current.Replace("@IDLink" & index, keys(0))
    '        current = current.Replace("@IDName" & index, keys(1))
    '        current = current.Replace("@IDColor" & index, idColor)
    '        current = current.Replace("@IDChange" & index, change)
    '        index = ""

    '        If current.Contains("@IDName1") = False AndAlso current.Contains("@IDName2") = False AndAlso count <> classIDs.Count Then
    '            current &= templete
    '        End If
    '    Next
    '    For i As Integer = 1 To 2
    '        current = current.Replace("@IDLink" & i, "")
    '        current = current.Replace("@IDName" & i, "")
    '        current = current.Replace("@IDColor" & i, "")
    '        current = current.Replace("@IDChange" & i, "")
    '    Next

    '    lblSameId.Text = title & current & "</table>"
    'End Sub

    Protected Sub gvRelate_DataBound(sender As Object, e As System.EventArgs) Handles gvRelate.DataBound
        Me.SetPriceColor(gvRelate)
    End Sub

    Private Sub SetPriceColor(gv As GridView)
        For Each row As GridViewRow In gv.Rows
            Dim lblDiffence As Label = row.FindControl("lbDiffence")
            Dim lblPercent As Label = row.FindControl("lbPercent")
            Dim lblPrice As Label = row.FindControl("lbPrice")
            Dim lblNumber As Label = row.FindControl("lbNumber")

            Dim UpLimitePrice As Double = TwStockAccessor.Instance.GetUpLimitPrice(Val(lblPrice.Text) - Val(lblDiffence.Text))
            Dim DownLimitePrice As Double = TwStockAccessor.Instance.GetDownLimitPrice(Val(lblPrice.Text) - Val(lblDiffence.Text))

            Dim diff As Single = Val(lblPrice.Text) - Val(lblDiffence.Text)
            If diff = 0 Then
                lblPercent.Text = "0.00%"
            Else
                lblPercent.Text = Format(Val(lblDiffence.Text) / (diff), "0.00%")
            End If
            lblPercent.Text = lblPercent.Text.Replace("%", "")

            If Val(lblDiffence.Text) > 0 Then
                lblPrice.ForeColor = Drawing.Color.Red
                lblDiffence.ForeColor = Drawing.Color.Red
                lblPercent.ForeColor = Drawing.Color.Red
                lblDiffence.Text = "▲" + lblDiffence.Text
                lblPercent.Text = "+" + lblPercent.Text
            ElseIf Val(lblDiffence.Text) < 0 Then
                lblPrice.ForeColor = Drawing.Color.Green
                lblDiffence.ForeColor = Drawing.Color.Green
                lblPercent.ForeColor = Drawing.Color.Green
                lblDiffence.Text = "▼" + lblDiffence.Text.Trim("-")
            End If

            If Val(lblPrice.Text) >= UpLimitePrice Then
                lblDiffence.ForeColor = Drawing.Color.White
                lblPercent.ForeColor = Drawing.Color.White
                lblPrice.ForeColor = Drawing.Color.White
                lblDiffence.BackColor = Drawing.Color.Red
                lblPercent.BackColor = Drawing.Color.Red
                lblPrice.BackColor = Drawing.Color.Red
            ElseIf Val(lblPrice.Text) <= DownLimitePrice Then
                lblDiffence.ForeColor = Drawing.Color.White
                lblPercent.ForeColor = Drawing.Color.White
                lblPrice.ForeColor = Drawing.Color.White
                lblDiffence.BackColor = Drawing.Color.Green
                lblPercent.BackColor = Drawing.Color.Green
                lblPrice.BackColor = Drawing.Color.Green
            End If

            Dim lblVolume As Label = row.FindControl("lblVolume")
            If lblNumber.Text = "0000" Then
                lblVolume.Text &= "(億)"
            End If
        Next
    End Sub
End Class
