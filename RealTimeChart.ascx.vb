
Partial Class RealTimeChart
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim stockno As String = "0000"
        If Request("no") IsNot Nothing Then
            stockno = Request("no").Replace("-", "").Replace("=", "").Trim
            stockno = stockno.Replace("$", "&")
        End If

        Dim txt As String = ""

        Dim imageUrl As String = ""

        If Val(stockno) > 0 Then
            imageUrl = TwStockAccessor.Instance.GetRealTimeChart(stockno, 350, 130)
        Else
            'imageUrl = TwStockAccessor.Instance.GetRealTimeChartByGlobal(stockno, 350, 130)
            imageUrl = TwStockAccessor.Instance.GetRealTimeChart(stockno, 350, 130)
        End If

        'Literal1.Text = txt
        Try
            '  GetCurrentUrl(imageUrl)

            If imageUrl.Length < 100 Then
                Exit Sub
            End If

            lblImg.Text = "<a href=""/chart.aspx?no=" & stockno.Replace("&", "$") & "" + """><img src=""" + imageUrl + """ width=""300px""></a>"
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetCurrentUrl(ByRef imageUrl As String)

        Dim dir As String = ""

        Dim filename As String = "C:\WantGoo\Wantgoooooo1\tmp\twgoogleindex.txt"

        If imageUrl.Length < 100 Then

            If IO.File.Exists(filename) = True Then

                imageUrl = My.Computer.FileSystem.ReadAllText(filename)

            End If

        Else

            If Now.Hour = 13 AndAlso (Now.Minute > 29 AndAlso Now.Minute < 32) Then
                My.Computer.FileSystem.WriteAllText(filename, imageUrl, False)
            End If

            'My.Computer.FileSystem.WriteAllText(filename, imageUrl, False)

        End If

    End Sub
End Class
