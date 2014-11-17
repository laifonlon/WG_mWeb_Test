
Partial Class Rate
    Inherits System.Web.UI.Page

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Val(lblUpdateInterval.Text) = 0 Then
            lblElapseTime.Text = ""
        Else
            lblElapseTime.Text = lblUpdateInterval.Text + " 秒 更新"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "time" + Me.ClientID, mScript.Replace("@ID", Me.ClientID).Replace("@Control", lblElapseTime.ClientID) + "showTime_" + Me.ClientID + "();", True)
        End If
 
        LoadData()
    End Sub

#Region "匯率資料"
    Private mRateKey As String = "Rate_Value_"
    ''' <summary>
    ''' 匯率名稱
    ''' </summary>
    Private Property RateName(ByVal id As String) As String
        Get
            Dim key As String = mRateKey + id + "_Name"
            If Application(key) Is Nothing Then Me.LoadRateName()
            If Application(key) Is Nothing Then Application(key) = id
            Return Application(key)
        End Get
        Set(ByVal value As String)
            Application(mRateKey + id + "_Name") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率區域
    ''' </summary>
    Private Property RateMarket(ByVal id As String) As String
        Get
            Dim key As String = mRateKey + id + "_Market"
            If Application(key) Is Nothing Then Me.LoadRateName()
            If Application(key) Is Nothing Then Application(key) = ""
            Return Application(key)
        End Get
        Set(ByVal value As String)
            Application(mRateKey + id + "_Market") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率昨收價
    ''' </summary>
    Private Property RateLast(ByVal fromID As String, ByVal toID As String) As Single
        Get
            Dim key As String = mRateKey + fromID + toID + "_Last"
            If Application(key) Is Nothing Then Me.LoadRateLast(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
        Set(ByVal value As Single)
            Application(mRateKey + fromID + toID + "_Last") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率最新報價
    ''' </summary>
    Private Property RateValue(ByVal fromID As String, ByVal toID As String) As Single
        Get
            Dim key As String = mRateKey + fromID + toID + "_Value"
            If Application(key) Is Nothing Then Me.LoadRate(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
        Set(ByVal value As Single)
            Application(mRateKey + fromID + toID + "_Value") = value
        End Set
    End Property

    ''' <summary>
    ''' 匯率最新日期
    ''' </summary>
    Private Property RateTime(ByVal fromID As String, ByVal toID As String) As Date
        Get
            Dim key As String = mRateKey + fromID + toID + "_Time"
            If Application(key) Is Nothing Then Me.LoadRate(fromID, toID)
            If Application(key) Is Nothing Then Application(key) = 0
            Return Application(key)
        End Get
        Set(ByVal value As Date)
            Application(mRateKey + fromID + toID + "_Time") = value
        End Set
    End Property

    ''' <summary>
    ''' 讀取所有匯率的名稱
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRateName()
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = "SELECT ID, Name, Market  from RateID"

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Me.RateName(en.Current("ID")) = en.Current("Name")
            Me.RateMarket(en.Current("ID")) = ""
        End While
    End Sub

    ''' <summary>
    ''' 讀取所有匯率的昨收價
    ''' </summary>
    Private Sub LoadRateLast(ByVal fromID As String, ByVal toID As String)
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = "select TOP 1 Date, Rate from RateHistory Where FromID = '" & fromID & "' And ToID = '" & toID & "' and DATEDIFF(day, RateHistory.date, GETDATE())>=1 Order By Date Desc"

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Me.RateLast(fromID, toID) = en.Current("Rate")
        End While
    End Sub

    ''' <summary>
    ''' 讀取所有匯率的最新報價
    ''' </summary>
    Private Sub LoadRate(ByVal fromID As String, ByVal toID As String)
        Dim sds As New SqlDataSource
        sds.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString
        sds.SelectCommand = "Select FromID, ToID, Date, Rate From Rate Where FromID = '" & fromID & "' And ToID = '" & toID & "'"

        Dim en As IEnumerator = sds.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            Me.RateTime(fromID, toID) = en.Current("Date")
            Me.RateValue(fromID, toID) = en.Current("Rate")
        End While
    End Sub
#End Region

    Private mTemplate As String = _
    "<tr>" + vbCrLf + _
    "<td class=""name""><a href=""rateinfo.aspx?id=@StockNo"" target=""_blank"">@Name</a></td>" + vbCrLf + _
    "<td class=""price""><span style=""color:@Color;"">@Price</span></td>" + vbCrLf + _
    "<td class=""change""><span style=""color:@Color;"">@Change</span></td>" + vbCrLf + _
    "<td class=""percent""><span style=""color:@Color;"">@Percent</span></td>" + vbCrLf + _
    "<td class=""time""><span >@Time</span></td>" + vbCrLf + _
    "</tr>"

    Private Sub LoadData()
        Dim key As String = "Rate_Rate_Table"
        If Application(key) IsNot Nothing AndAlso _
            Application(key + "LastTechTime") IsNot Nothing Then
            If Now < Application(key + "LastTechTime") Then
                a.Text = Application(key)
                Exit Sub
            End If
        End If

        Dim name As String = "'"
        Dim current As String = ""
        Dim en As IEnumerator = sdsRate.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            'name = Me.RateName(en.Current("FromID")) & "/" & Me.RateName(en.Current("ToID"))
            name = Me.RateName(en.Current("ToID"))

            current = mTemplate.Replace("@Name", name)
            current = current.Replace("@StockNoID", en.Current("FromID") & en.Current("ToID"))
            current = current.Replace("@StockNo", en.Current("FromID") & en.Current("ToID"))

            Dim lastRate As Single = Me.RateLast(en.Current("FromID"), en.Current("ToID"))
            Dim change As Single = en.Current("Rate") - lastRate

            Dim percent As String = ""
            If lastRate > 0 Then
                percent = Format((change / lastRate), "0.00%")
            Else
                percent = "0.00"
            End If

            '顯示位數
            Dim rate As Single = en.Current("Rate")
            If rate > 999.9 Then
                current = current.Replace("@Price", CInt(rate))
            ElseIf rate > 99.9 AndAlso rate <= 999.9 Then
                current = current.Replace("@Price", Format(rate, "0.0"))
            ElseIf rate < 1 Then
                current = current.Replace("@Price", Format(rate, "0.000"))
            Else
                current = current.Replace("@Price", Format(rate, "0.00"))
            End If

            change = Format(change, "0.0000")
            Dim changeText As String = ""
            If change > 0 Then
                current = current.Replace("@Color", "red")
                percent = "+" & percent
                changeText = "▲" & change
            ElseIf change < 0 Then
                current = current.Replace("@Color", "#0ADF0A")
                changeText = "▼" & change
            Else
                current = current.Replace("@Color", "#F39C04")
                changeText = "-"
                current = current.Replace("@Percent", "-")
            End If

            '時間
            current = current.Replace("@Time", Format(en.Current("Date"), "HH:mm"))

            current = current.Replace("@Change", changeText)
            current = current.Replace("@Percent", percent)
            a.Text = a.Text + current
        End While

        a.Text = "<table cellpadding=""0"" cellspacing=""0"" class=""tb"">" + a.Text + "</table>"
        Application(key) = a.Text
        Application(key + "LastTechTime") = Now.AddMinutes(2)
    End Sub
End Class
