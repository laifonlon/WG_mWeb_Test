
Partial Class ImportantIndex
    Inherits System.Web.UI.UserControl
    Private mTemplate As String = _
  "<li><a href=""index.aspx?no=@StockNo""><span class=""in ww30"">@Name</span><span class=""ix ww25"">@Price</span><span class=""up ww25"" style=""color:@ChangeColor;"">@Change</span><span class=""ut ww20"" style=""color:@ChangeColor;"">@Percent</span></a></li>"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim key As String = "Mobile_ImportantIndex"
        If Application(key) IsNot Nothing AndAlso _
            Application(key + "LastTechTime") IsNot Nothing Then
            If Now < Application(key + "LastTechTime") Then
                a.Text = Application(key)
                Exit Sub
            End If
        End If

        Dim en As IEnumerator = sdsStock.Select(New DataSourceSelectArguments).GetEnumerator
        Dim current As String = ""
        While en.MoveNext
            current = mTemplate
            current = current.Replace("@StockNo", en.Current("StockNo").trim.Replace("&", "$"))
            current = current.Replace("@Name", en.Current("Name"))
            current = current.Replace("@Price", Format(en.Current("Deal"), "0.0"))

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
                percent = "+" + percent
                change = "▲" + change
            ElseIf en.Current("Change") < 0 Then
                current = current.Replace("@PriceColor", "green")
                current = current.Replace("@ChangeColor", "green")
                change = "▼" + change.Trim("-")
            End If

            current = current.Replace("@Change", change)
            current = current.Replace("@Percent", percent)

            a.Text = a.Text + current
        End While

        Application(key) = a.Text
        Application(key + "LastTechTime") = Now.AddSeconds(30)
    End Sub
End Class
