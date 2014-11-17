
Partial Class TwIndexRelate
    Inherits System.Web.UI.UserControl

    Protected Sub gvCollect_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles g.DataBound
        If g.Rows.Count = 0 Then Me.Visible = False

        For Each row As GridViewRow In g.Rows
            Dim lblChange As Literal = row.FindControl("lblChange")
            Dim lblPercent As Literal = row.FindControl("lblPercent")
            Dim lblPrice As Literal = row.FindControl("lblPrice")
            'Dim lblNumber As Literal = row.FindControl("lblNumber")

            If Val(lblPrice.Text) > 0 Then
                lblPercent.Text = Format(Val(lblChange.Text) / (Val(lblPrice.Text) - Val(lblChange.Text)), "0.00%")
            Else
                lblPercent.Text = "0.00%"
            End If

            '顯示位數
            If Val(lblPrice.Text) > 99.9 Then
                lblPrice.Text = Format(Val(lblPrice.Text), "0.0")
            End If

            If Val(lblChange.Text) > 0 Then
                lblPrice.Text = "<span style=""color:#DC0300;"">" + lblPrice.Text + "</span>"
                lblChange.Text = "<span style=""color:#DC0300;"">▲" + lblChange.Text + "</span>"
                lblPercent.Text = "<span style=""color:#DC0300;"">+" + lblPercent.Text + "</span>"
            ElseIf Val(lblChange.Text) < 0 Then
                lblPrice.Text = "<span style=""color:#008000;"">" + lblPrice.Text + "</span>"
                lblChange.Text = "<span style=""color:#008000;"">▼" + lblChange.Text.Trim("-") + "</span>"
                lblPercent.Text = "<span style=""color:#008000;"">" + lblPercent.Text + "</span>"
            End If

        Next
    End Sub
End Class
