
Partial Class StockViewr
    Inherits System.Web.UI.UserControl

    Public Property Title() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal value As String)
            lblTitle.Text = value
        End Set
    End Property

    Public Property UpdateInterval() As Integer
        Get
            Return Val(lblUpdateInterval.Text)
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then
                lblUpdateInterval.Text = 0
                Exit Property
            End If

            lblUpdateInterval.Text = (value + 1).ToString
        End Set
    End Property

    Public Property Market() As Integer
        Get
            Return Val(lblMarket.Text)
        End Get
        Set(ByVal value As Integer)
            lblMarket.Text = value.ToString
        End Set
    End Property

    Public Property ShowCount() As Integer
        Get
            Return Val(lblShowCount.Text)
        End Get
        Set(ByVal value As Integer)
            lblShowCount.Text = value.ToString
        End Set
    End Property

    Public Property HideCount() As Integer
        Get
            Return Val(lblHideCount.Text)
        End Get
        Set(ByVal value As Integer)
            lblHideCount.Text = value.ToString
        End Set
    End Property

    Public Property StartUpdateTime() As Integer
        Get
            Return Val(lblStartUpdateTime.Text)
        End Get
        Set(ByVal value As Integer)
            lblStartUpdateTime.Text = value.ToString
        End Set
    End Property

    Public Property EndUpdateTime() As Integer
        Get
            Return Val(lblEndUpdateTIme.Text)
        End Get
        Set(ByVal value As Integer)
            lblEndUpdateTIme.Text = value.ToString
        End Set
    End Property

    Public Property ShowAd() As Boolean
        Get
            Return CBool(lblShowAd.Text)
        End Get
        Set(ByVal value As Boolean)
            lblShowAd.Text = value.ToString
        End Set
    End Property

    Public Property Language() As String
        Get
            Return lblLanguage.Text
        End Get
        Set(ByVal value As String)
            lblLanguage.Text = value
        End Set
    End Property

    Public Property ShowUpdate() As Boolean
        Get
            If lblShowUpdate.Text.ToLower = "false" Then
                Return False
            Else
                Return True
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                lblShowUpdate.Text = "true"
            Else
                lblShowUpdate.Text = "false"
            End If
        End Set
    End Property

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
        'If Me.IsMobile(Request.Headers("user-agent")) = False Then
        '    Response.Redirect("http://www.wantgoo.com/global.aspx")
        '    Exit Sub
        'End If
        Update.NavigateUrl = Request.Url.ToString

        If Now.DayOfWeek = DayOfWeek.Sunday OrElse _
            (Now.DayOfWeek = DayOfWeek.Saturday AndAlso Now.Hour > 12) OrElse _
            (Now.DayOfWeek = DayOfWeek.Monday AndAlso Now.Hour < 7) Then
            lblUpdateInterval.Text = (0).ToString
        End If

        If EndUpdateTime > StartUpdateTime AndAlso _
           Not (Val(Format(Now, "HH")) >= StartUpdateTime AndAlso _
                    Val(Format(Now, "HH")) <= EndUpdateTime) Then
            lblUpdateInterval.Text = (0).ToString
        End If

        If StartUpdateTime > EndUpdateTime AndAlso _
             (Val(Format(Now, "HH")) >= EndUpdateTime AndAlso _
            Val(Format(Now, "HH")) <= StartUpdateTime) Then
            lblUpdateInterval.Text = (0).ToString
        End If

        If Title = "" Then
            p.Visible = False
        End If

        'If Val(lblUpdateInterval.Text) = 0 OrElse Title = "" OrElse Not ShowUpdate Then
        '    lblElapseTime.Text = ""
        'Else
        '    lblElapseTime.Text = lblUpdateInterval.Text + " 秒 更新"
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "time" + Me.ClientID, mScript.Replace("@ID", Me.ClientID).Replace("@Control", lblElapseTime.ClientID) + "showTime_" + Me.ClientID + "();", True)
        'End If

        If lblLanguage.Text.ToLower = "en" Then
            Update.Text = "Update"
        End If

        If Not ShowUpdate Then
            Update.Visible = False
        End If

        If ShowCount = 0 Then
            a.Visible = False
        Else
            LoadData()
        End If

    End Sub

    Private Function IsMobile(userAgent As String) As Boolean
        If userAgent Is Nothing Then Return False
        Dim mobileTypes As String() = {"iphone", "ipod", "android", "webos", "mobile", "phone", "pda", "blackberry", "windows ce", "symbian", "wap", "googlebot-mobile"}

        For Each key As String In mobileTypes
            If (userAgent.ToLower.IndexOf(key) <> -1) Then
                Return True
            End If
        Next
        Return False
    End Function

    '此樣式在波羅地海指數有用到, 更改時須注意
    Private mTemplate As String = _
  "<li id=""@StockNoID"">" + _
  "<a href=""index.aspx?no=@StockNo"">" + _
  "<span class=""in ww30"">@Name</span>" + _
  "<span id=""@StockNoID_Price"" class=""ix ww25"">@Price</span>" + _
  "<span id=""@StockNoID_Change"" class=""up ww25"" style=""color:@ChangeColor;"">@Change</span>" + _
  "<span id=""@StockNoID_Percent"" class=""ut ww20"" style=""color:@PercentColor;"">@Percent</span>" + _
  "</a>" + _
  "</li>"
    '"<td class=""percent""><span style=""color:@PercentColor;"">@Percent</span></td>" + vbCrLf + _

    Private Sub LoadData()
        Dim key As String = "Mobile_GlobalFinance_" + Market.ToString + "_" + ShowCount.ToString + "_" + HideCount.ToString
        If Application(key) IsNot Nothing AndAlso _
            Application(key + "LastTechTime") IsNot Nothing Then
            If Now < Application(key + "LastTechTime") Then
                a.Text = Application(key)
                Exit Sub
            End If
        End If

        sdsStock.SelectParameters("Count").DefaultValue = lblShowCount.Text
        sdsStock.SelectParameters("Market").DefaultValue = lblMarket.Text
        Dim index As Integer
        Dim current As String = ""
        Dim en As IEnumerator = sdsStock.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            index = index + 1
            If HideCount >= index Then
            Else
                'If index = 1 Then
                '    current = m1stTemplate
                'Else
                current = mTemplate
                'End If

                Select Case en.Current("StockNo").trim
                    Case "BDI", "BCI", "BPI", "BSI"
                        current = current.Replace("<a href=""index.aspx?no=@StockNo""><span class=""in ww30"">@Name", "<a><span class=""in ww30"">" + en.Current("Name"))
                    Case Else
                        If Language = "en" AndAlso en.Current("EnglishName") IsNot DBNull.Value Then
                            current = current.Replace("@Name", en.Current("EnglishName"))
                        Else
                            current = current.Replace("@Name", en.Current("Name"))
                        End If

                        current = current.Replace("@StockNoID", en.Current("StockNo").trim.Replace("&", ""))
                        current = current.Replace("@StockNo", en.Current("StockNo").trim.Replace("&", "$"))
                End Select

                Dim percent As String = ""
                Dim change As String = ""

                If en.Current("Deal") > 0 Then
                    percent = Format(en.Current("Change") / (en.Current("Deal") - en.Current("Change")), "0.00%")
                Else
                    percent = "0.00"
                End If

                '顯示位數
                If en.Current("Deal") > 99.9 Then
                    current = current.Replace("@Price", Format(en.Current("Deal"), "0.0"))
                ElseIf en.Current("Deal") < 1 Then
                    current = current.Replace("@Price", Format(en.Current("Deal"), "0.0000"))
                Else
                    current = current.Replace("@Price", Format(en.Current("Deal"), "0.00"))
                End If

                If Math.Abs(en.Current("Change")) > 0.5 Then
                    change = Format(en.Current("Change"), "0.0")
                ElseIf Math.Abs(en.Current("Change")) > 0.01 Then
                    change = Format(en.Current("Change"), "0.00")
                ElseIf en.Current("Change") = 0 Then
                    change = "0"
                End If

                If en.Current("Change") > 0 Then
                    current = current.Replace("@PriceColor", "red")
                    current = current.Replace("@ChangeColor", "red")
                    current = current.Replace("@PercentColor", "red")
                    percent = "+" + percent
                    change = "▲" + change
                ElseIf en.Current("Change") < 0 Then
                    current = current.Replace("@PriceColor", "green")
                    current = current.Replace("@ChangeColor", "green")
                    current = current.Replace("@PercentColor", "green")
                    change = "▼" + change.Trim("-")
                End If

                '時間
                If en.Current("StockNo") = "TEDSP" Then
                    current = current.Replace("@Time", Format(en.Current("Time"), "MM/dd"))
                Else
                    current = current.Replace("@Time", Format(en.Current("Time"), "HH:mm"))
                End If

                current = current.Replace("@Change", change)
                current = current.Replace("@Percent", percent)
                a.Text = a.Text + current
            End If
        End While

        Application(key) = a.Text
        If lblUpdateInterval.Text = "0" Then
            Application(key + "LastTechTime") = Now.AddMinutes(15)
        Else
            Application(key + "LastTechTime") = Now.AddSeconds(15)
        End If
    End Sub

#Region "廣告"
    '    Private mAd As String = _
    '"<div>" + _
    '"<script type=""text/javascript""><!-- " + vbCrLf + _
    '"google_ad_client = ""pub-1654611749822059""; " + vbCrLf + _
    '"/* 468x60, 已建立 2011/5/9 */ " + vbCrLf + _
    '"google_ad_slot = ""5472048281""; " + vbCrLf + _
    '"google_ad_width = 305; " + vbCrLf + _
    '"google_ad_height = 60; " + vbCrLf + _
    '"//-->" + vbCrLf + _
    '"</script>" + vbCrLf + _
    '"<script type=""text/javascript"" " + vbCrLf + _
    '"src=""http://pagead2.googlesyndication.com/pagead/show_ads.js"">" + vbCrLf + _
    '"</script>" + vbCrLf + _
    '"</div>"

    '    Private Sub InsertADRow(ByVal rowIndex As Integer)
    '        If g.Rows.Count < rowIndex Then Exit Sub
    '        Dim adRow As New GridViewRow(0, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
    '        Dim adCell As New TableCell
    '        adCell.ColumnSpan = 5
    '        adCell.Text = mAd

    '        adRow.Cells.Add(adCell)
    '        g.Controls(0).Controls.AddAt(rowIndex, adRow)

    '    End Sub
#End Region

End Class