Imports System.Data

Partial Class m
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim h1title As String = Me.Request.Url.ToString.Trim.ToLower()

        If h1title.Contains("blog") Then
            'Me.h1.Text = "部落格"
        ElseIf h1title.Contains("globals.aspx") Then
            Me.h1.Text = "國際股市"
        ElseIf h1title.Contains("twindex.aspx") Then
            Me.h1.Text = "台股資訊"
        ElseIf h1title.Contains("stock.aspx") Then
            Me.h1.Text = "個股資訊"
        ElseIf h1title.Contains("article.aspx") Then
            Me.h1.Text = "精選文章"
        ElseIf h1title.Contains("lesson.aspx") OrElse h1title.Contains("lessoninfo.aspx") Then
            Me.h1.Text = "理財課程"
        Else
            Me.h1.Text = "玩股網"
        End If

        Try
            If Session("IsLogin") Is Nothing Or Session("UserName") Is Nothing Or Session("MemberNo") Is Nothing Then
                Dim ma As New MemberAuthority
                ma.SignOut()
            End If
        Catch ex As Exception
            'My.Computer.FileSystem.WriteAllText("log.txt", ex.Message, True)
        End Try

        '搜尋Bar
        Dim mStockTemp As String = _
        "<a class=""search"" href=""/stock.aspx?no=@No"">@Name @No</a>"
        Dim arguments As New DataSourceSelectArguments
        Dim result As IEnumerator = sdsStocksearch.Select(arguments).GetEnumerator
        Dim stocks As String = ""
        While result.MoveNext
            stocks = stocks + mStockTemp.Replace("@Name", result.Current("Name")).Replace("@No", result.Current("StockNo")) + ", "
        End While
        lblresult.Text = stocks

    End Sub

    Protected Sub btnsearch_Click(sender As Object, e As EventArgs) Handles btnsearch.Click
        Dim url As String = "index.aspx?no="
        If tbxKeyWord.Text.Trim = "0" OrElse tbxKeyWord.Text.Trim = "0000" Then
            url = url + "0000"
            Response.Redirect(url)
            Exit Sub
        ElseIf Val(tbxKeyWord.Text.Trim) > 0 Then
            sdsSearchStockNo.SelectParameters("KeyWord1").DefaultValue = tbxKeyWord.Text.Trim
        Else
            sdsSearchStockNo.SelectParameters("KeyWord2").DefaultValue = "%" + tbxKeyWord.Text.Trim + "%"
        End If

        Dim result As IEnumerator = sdsSearchStockNo.Select(New DataSourceSelectArguments).GetEnumerator
        If result.MoveNext Then
            Dim row As DataRowView = result.Current
            If IsNumeric(row("StockNo").ToString) AndAlso row("StockNo").ToString <> "0000" Then
                url = "stock.aspx?no="
            End If
            url = url + row("StockNo").ToString
        Else
            url = url + "0000"
        End If

        Response.Redirect(url)

    End Sub

End Class

