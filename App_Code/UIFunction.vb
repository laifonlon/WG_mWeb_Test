Imports Microsoft.VisualBasic

Public Class UIFunction
#Region "Singleton"
    Private Shared mObj As New UIFunction
    Public Shared ReadOnly Property Instance() As UIFunction
        Get
            Return mObj
        End Get
    End Property
#End Region

    '*********************************************************************
    '
    ' ShowMessage() 函式
    '
    ' 公用函式，傳入欲顯示之字串，以對話視窗顯示出後轉址到所只定之位置
    '
    '*********************************************************************
    Sub ShowMessage(ByVal Message As String, ByVal Page As System.Web.UI.Page, ByVal Redirect As String)
        Dim JavaScript As String
        JavaScript = "<SCRIPT Language='JavaScript'>"
        JavaScript += "window.alert('"
        JavaScript += Message & "');"
        JavaScript += "document.location='" & Redirect & "';"
        JavaScript += "</SCRIPT>"
        Page.Response.Write(JavaScript)
        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", JavaScript, True)
    End Sub


    ''' <summary> 
    ''' 彈出提示訊息。 
    ''' </summary> 
    ''' <param name="Message">訊息文字。</param> 
    Public Sub MsgBox(ByVal message As String, ByVal page As UI.Page)
        message = Strings.Replace(message, "'", "\'") '處理單引號 
        message = Strings.Replace(message, vbNewLine, "\n") '處理換行 
        Dim script As String = String.Format("alert('{0}');", message)
        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", script, True)
    End Sub

    ''' <summary> 
    ''' 彈出提示訊息。 
    ''' </summary> 
    ''' <param name="Message">訊息文字。</param> 
    Public Sub MsgBox(ByVal message As String, ByVal control As UI.Control)
        message = Strings.Replace(message, "'", "\'") '處理單引號 
        message = Strings.Replace(message, vbNewLine, "\n") '處理換行 
        Dim script As String = String.Format("alert('{0}');", message)
        ScriptManager.RegisterStartupScript(control, control.GetType(), "alert", script, True)
    End Sub

    ''' <summary> 
    ''' 彈出提示訊息。 
    ''' </summary> 
    Public Sub PopMsgInsideUpdatePanel(ByVal message As String, _
       Optional ByVal intervalCheckFocus As String = "100", _
       Optional ByVal timeStayTop As String = "5000")
        Dim js As String
        js = "    subWin = alert('" & Replace(message, "'", "\x27") & "');" & _
            "    timerID = setInterval('if (subWin != undefined) subWin.focus();', " & intervalCheckFocus & ");" & _
            "    setTimeout('clearInterval(timerID)', " & timeStayTop & ");"
        ScriptManager.RegisterClientScriptBlock(CType(HttpContext.Current.Handler, Page), GetType(String), "js", js, True)
    End Sub

    Public Shared Function GetTimeDescription(ByVal time As Date) As String
        Dim ts As TimeSpan = DateTime.Now.Subtract(time)
        If ts.Days > 365 Then
            Return Math.Floor(ts.Days / 365).ToString + " 年前"
        ElseIf ts.Days > 30 Then
            Return Math.Floor(ts.Days / 30).ToString + " 月前"
        ElseIf ts.Days > 1 Then
            Return ts.Days.ToString + " 天前"
        ElseIf ts.Days > 0 Then
            Return "昨天" + Format(time, "HH:mm")
        ElseIf ts.Hours > 7 Then
            Return Format(time, "HH:mm") 'ts.Hours.ToString + " 時前"
        ElseIf ts.Hours > 0 Then
            Return ts.Hours.ToString + " 時前"
            'ElseIf ts.Minutes > 5 Then
            '    Return Format(time, "HH:mm") 'ts.Minutes.ToString + " 分前"
        ElseIf ts.Minutes > 0 Then
            Return ts.Minutes.ToString + " 分前"
        ElseIf ts.Milliseconds > 0 Then
            Return ts.Seconds.ToString + " 秒前"
        Else
            Return time.ToString.Trim
        End If
    End Function

    Public Shared Function GetShortTimeDescription(ByVal time As Date) As String
        Dim ts As TimeSpan = DateTime.Now.Subtract(time)
        If ts.Days > 365 Then
            Return Math.Floor(ts.Days / 365).ToString + " 年前"
        ElseIf ts.Days > 30 Then
            Return Math.Floor(ts.Days / 30).ToString + " 月前"
        ElseIf ts.Days > 1 Then
            Return ts.Days.ToString + " 天前"
            'ElseIf ts.Days > 0 Then
            '    Return "昨天" + Format(time, "HH:mm")
            'ElseIf ts.Hours > 7 Then
            '    Return Format(time, "HH:mm") 'ts.Hours.ToString + " 時前"
        ElseIf ts.Hours > 0 Then
            Return ts.Hours.ToString + " 時前"
            'ElseIf ts.Minutes > 5 Then
            '    Return Format(time, "HH:mm") 'ts.Minutes.ToString + " 分前"
        ElseIf ts.Minutes > 0 Then
            Return ts.Minutes.ToString + " 分前"
        ElseIf ts.Milliseconds > 0 Then
            Return ts.Seconds.ToString + " 秒前"
        Else
            Return time.ToString.Trim
        End If
    End Function

End Class
