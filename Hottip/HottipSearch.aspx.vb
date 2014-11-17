Imports System.Data

Partial Class Hottip_HottipSearch
    Inherits System.Web.UI.Page

    Dim mWatch As New System.Diagnostics.Stopwatch

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        RadioButtonSelectedDay()
        RadioButtonDirection()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not MemberDataAccessor.Instance.IsLogin Then
            Response.Redirect("/Login.aspx?GoBackUrl=" + Server.UrlEncode(Request.RawUrl))
            Throw New Exception("會員未登入")
        End If

        With MemberDataAccessor.Instance
            If MemberDataAccessor.Instance.IsAdiminstrator AndAlso _
                Request("MemberNo") IsNot Nothing Then
                sdsMyRule.SelectParameters("MemberNo").DefaultValue = Request("MemberNo") '"23795"
                sdsMySubscribe.SelectParameters("MemberNo").DefaultValue = Request("MemberNo")
            Else
                sdsMyRule.SelectParameters("MemberNo").DefaultValue = .GetMemberNo '"23795"
                sdsMySubscribe.SelectParameters("MemberNo").DefaultValue = .GetMemberNo
            End If
        End With

        '最下方廣告 是否顯示
        Mob_300x2501.Visible = Member.Instance.IsShowAd()

    End Sub

    Private Sub RadioButtonDirection()
        Dim RadioButtonDirection As String = Request("direction")
        'RadioButtonList_Direction.SelectedValue = RadioButtonDirection
        radBuy.Checked = False
        radSell.Checked = False
        Select Case RadioButtonDirection
            Case "多頭"
                radBuy.Checked = True
            Case "空頭"
                radSell.Checked = True
            Case Else
                radBuy.Checked = True
        End Select
    End Sub

    Private Sub RadioButtonSelectedDay()
        Dim RadioButtonSelectedDay As String = Request("rd")
        'RadioButtonList_Date.SelectedValue = RadioButtonSelectedDay
        radWeek.Checked = False
        radToday.Checked = False
        radYestoday.Checked = False
        Select Case RadioButtonSelectedDay
            Case "近一周"
                radWeek.Checked = True
            Case "今日"
                radToday.Checked = True
            Case "昨日"
                radYestoday.Checked = True
            Case Else
                radWeek.Checked = True
        End Select
    End Sub

    Protected Sub RadioButtonDirection_CheckedChanged(sender As Object, e As EventArgs)

        Dim nowUrl As String = Request.RawUrl
        Dim direction As String = "多頭"

        Dim rd As String = "近一周"
        If Request("rd") IsNot Nothing AndAlso Request("rd") <> "" Then
            rd = Request("rd")
        End If

        Dim thisRadioButton As RadioButton = sender

        Select Case thisRadioButton.Text
            Case "多頭"
                direction = thisRadioButton.Text
            Case "空頭"
                direction = thisRadioButton.Text
            Case Else
                direction = thisRadioButton.Text
        End Select

        Response.Redirect("/Hottip/HottipSearch.aspx?direction=" + direction + "&rd=" + rd)

    End Sub

    Protected Sub RadioButtonDay_CheckedChanged(sender As Object, e As EventArgs)

        Dim nowUrl As String = Request.RawUrl
        Dim rd As String = "近一周"

        Dim direction As String = "多頭"
        If Request("direction") IsNot Nothing AndAlso Request("direction") <> "" Then
            direction = Request("direction")
        End If

        Dim thisRadioButton As RadioButton = sender

        Select Case thisRadioButton.Text
            Case "近一周"
                rd = thisRadioButton.Text
            Case "今日"
                rd = thisRadioButton.Text
            Case "昨日"
                rd = thisRadioButton.Text
            Case Else
                rd = thisRadioButton.Text
        End Select

        Response.Redirect("/Hottip/HottipSearch.aspx?direction=" + direction + "&rd=" + rd)

    End Sub

    Public Function GetRequestString(RequestName As String, DefaultName As String) As String
        Dim ReturnValue As String = String.Empty
        If Not String.IsNullOrEmpty(Request(RequestName)) Then
            ReturnValue = Request(RequestName)
        Else
            ReturnValue = DefaultName
        End If
        Return ReturnValue
    End Function

End Class
