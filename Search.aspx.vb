Imports System.Data

Partial Class Search
    Inherits System.Web.UI.Page


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
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

    Private mStockTemp As String = _
    "<a class=""search"" href=""/stock.aspx?no=@No"">@Name @No</a>"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim arguments As New DataSourceSelectArguments
        Dim result As IEnumerator = sdsStocksearch.Select(arguments).GetEnumerator
        Dim stocks As String = ""
        While result.MoveNext
            stocks = stocks + mStockTemp.Replace("@Name", result.Current("Name")).Replace("@No", result.Current("StockNo")) + ", "
        End While
        lblresult.Text = stocks
    End Sub

    Protected Overrides Sub OnPreInit(e As System.EventArgs)
        ' added for compatibility issues with chrome 
        If (Request.UserAgent IsNot Nothing AndAlso (Request.UserAgent.IndexOf("AppleWebKit") > 0)) Then
            ClientTarget = "uplevel"
        End If

        MyBase.OnPreInit(e)
    End Sub

End Class
