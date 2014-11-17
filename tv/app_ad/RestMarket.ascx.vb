
Partial Class RestMarket
    Inherits System.Web.UI.UserControl
    Private mTemplate As String = "<li><a><span class=""in ww20"">@Holiday</span><span>@Name</span></a></li>"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim key As String = "Mobile_RestMarket"
        If Application(key) IsNot Nothing AndAlso _
            Application(key + "LastTechTime") IsNot Nothing Then
            If Now < Application(key + "LastTechTime") Then
                a.Text = Application(key)
                Exit Sub
            End If
        End If

        Dim en As IEnumerator = sdsRest.Select(New DataSourceSelectArguments).GetEnumerator
        Dim current As String = ""
        While en.MoveNext
            current = mTemplate
            current = current.Replace("@Holiday", Format(en.Current("Holiday"), "MM/dd"))
            current = current.Replace("@Name", en.Current("Name"))

            a.Text = a.Text + current
        End While

        Application(key) = a.Text
        Application(key + "LastTechTime") = Now.AddHours(3)
    End Sub
End Class
