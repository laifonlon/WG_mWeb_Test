
Partial Class ExchangeRate
    Inherits System.Web.UI.UserControl

    Public Property ShowUpdate() As Boolean = False

    Private mScript As String = _
     "var isFocus = 1;" + _
     "function showTime_@ID() {" + _
     "var t = parseInt(document.getElementById(""@Control"").innerHTML, 10);" + _
     "    t -= 1;" + _
     "    if (t > 0) {" + _
     "        document.getElementById(""@Control"").innerHTML = t +"" 秒 更新"";" + _
     "        setTimeout(""showTime_@ID()"", 1000);" + _
     "    }" + _
     "    else {" + _
     "       if (isFocus == 1) {" + _
     "          location.reload(); }" + _
     "       else {" + _
     "        document.getElementById(""@Control"").innerHTML = ""1 秒 更新"";" + _
     "         setTimeout(""showTime_@ID()"", 1000); }" + _
     "     }" + _
     "}"

    Private mTemplate As String = _
    "<tr class=""gvtable_row"">" + _
    "<td><a alt=""/Stock/ExchangeRateInfo.aspx?n=@Name&s=@SubName"">@SubName</a></td>" + _
    "<td>@現買</td>" + _
    "<td>@現賣</td>" + _
    "<td>@即買</td>" + _
    "<td>@即賣</td>" + _
    "<td>@時間</td>" + _
    "</tr>"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If ShowUpdate = True Then
            lblElapseTime.Text = "30 秒 更新"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "time" + Me.ClientID, mScript.Replace("@ID", Me.ClientID).Replace("@Control", lblElapseTime.ClientID) + "showTime_" + Me.ClientID + "();", True)
        End If

        Dim key As String = "Mobile_ExchangeRate"
        If Application(key) IsNot Nothing AndAlso _
            Application(key + "_LastTime") IsNot Nothing Then
            If Now < Application(key + "_LastTime") Then
                a.Text = Application(key)
                Exit Sub
            End If
        End If

        a.Text = ""
        Dim current As String = ""
        Dim en As IEnumerator = sdsExchangeRate.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            current = mTemplate
            With en
                current = current.Replace("@Name", .Current("Name").trim)
                current = current.Replace("@SubName", .Current("SubName").trim)
                current = current.Replace("@時間", Format(.Current("Time"), "HH:mm"))
                If Val(.Current("CRIn")) = 0 Then
                    current = current.Replace("@現買", "-----")
                ElseIf Val(.Current("CRIn")) >= 10 Then
                    current = current.Replace("@現買", Format(.Current("CRIn"), "0.00"))
                Else
                    current = current.Replace("@現買", Format(.Current("CRIn"), "0.000"))
                End If
                If Val(.Current("CROut")) = 0 Then
                    current = current.Replace("@現賣", "-----")
                ElseIf Val(.Current("CROut")) >= 10 Then
                    current = current.Replace("@現賣", Format(.Current("CROut"), "0.00"))
                Else
                    current = current.Replace("@現賣", Format(.Current("CROut"), "0.000"))
                End If

                If Val(.Current("SERIn")) = 0 Then
                    current = current.Replace("@即買", "-----")
                ElseIf Val(.Current("SERIn")) >= 10 Then
                    current = current.Replace("@即買", Format(.Current("SERIn"), "0.00"))
                Else
                    current = current.Replace("@即買", Format(.Current("SERIn"), "0.000"))
                End If
                If Val(.Current("SEROut")) = 0 Then
                    current = current.Replace("@即賣", "-----")
                ElseIf Val(.Current("SEROut")) >= 10 Then
                    current = current.Replace("@即賣", Format(.Current("SEROut"), "0.00"))
                Else
                    current = current.Replace("@即賣", Format(.Current("SEROut"), "0.000"))
                End If
                a.Text = a.Text + current
            End With
        End While

        Application(key) = a.Text
        If Now.DayOfWeek = DayOfWeek.Saturday OrElse _
            Now.DayOfWeek = DayOfWeek.Sunday OrElse _
            Now.Hour < 9 OrElse Val(Format(Now, "HH")) > 17 Then
            Application(key + "_LastTime") = Now.AddMinutes(30)
        Else
            Application(key + "_LastTime") = Now.AddSeconds(30)
        End If

    End Sub

End Class

