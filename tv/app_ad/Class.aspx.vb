
Partial Class _Class
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Request("id") Is Nothing Then Exit Sub

        Dim classId As String = Val(Request("id")).ToString
        sdsClass.SelectParameters("ClassId").DefaultValue = classId
        sdsCollect.SelectParameters("ClassId").DefaultValue = classId

        Dim en As IEnumerator = sdsClass.Select(New DataSourceSelectArguments).GetEnumerator
        If en.MoveNext Then
            lTitle.Text = en.Current("Name")
            Page.Title = lTitle.Text & " - 玩股網 手機版"
        End If
    End Sub

    Protected Sub gv1_DataBound(sender As Object, e As System.EventArgs) Handles gv1.DataBound
        For Each row As GridViewRow In gv1.Rows
            Dim lblDiffence As Label = row.FindControl("lblDiffence")
            Dim lblLast As Label = row.FindControl("lblLast")
            'Dim lblOpen As Label = row.FindControl("lblOpen")
            'Dim lblHigh As Label = row.FindControl("lblHigh")
            'Dim lblLow As Label = row.FindControl("lblLow")
            Dim lblPercent As Label = row.FindControl("lblPercent")
            Dim lblPrice As Label = row.FindControl("lblPrice")
            'Dim lblDistancePercent As Label = row.FindControl("lblDistancePercent")
            'Dim lblDistance As Label = row.FindControl("lblDistance")
            Dim lblNumber As Label = row.FindControl("lblNumber")
            Dim UpLimitePrice As Double = TwStockAccessor.Instance.GetUpLimitPrice(Val(lblPrice.Text) - Val(lblDiffence.Text))
            Dim DownLimitePrice As Double = TwStockAccessor.Instance.GetDownLimitPrice(Val(lblPrice.Text) - Val(lblDiffence.Text))

            If Val(lblPrice.Text) > Val(lblLast.Text) Then
                lblPrice.ForeColor = Drawing.Color.Red
            ElseIf Val(lblPrice.Text) < Val(lblLast.Text) Then
                lblPrice.ForeColor = Drawing.Color.Green
            End If

            If Val(lblDiffence.Text) > 0 Then
                lblDiffence.ForeColor = Drawing.Color.Red
                lblPercent.ForeColor = Drawing.Color.Red
                lblDiffence.Text = "▲" + lblDiffence.Text
                lblPercent.Text = "+" + lblPercent.Text
            ElseIf Val(lblDiffence.Text) < 0 Then
                lblDiffence.ForeColor = Drawing.Color.Green
                lblPercent.ForeColor = Drawing.Color.Green
                lblDiffence.Text = "▼" + lblDiffence.Text.Trim("-")
            End If

            If Val(lblPrice.Text) >= UpLimitePrice Then
                lblDiffence.ForeColor = Drawing.Color.White
                lblPercent.ForeColor = Drawing.Color.White
                lblPrice.ForeColor = Drawing.Color.White
                lblDiffence.BackColor = Drawing.Color.FromArgb(255, 30, 30)
                lblPercent.BackColor = Drawing.Color.FromArgb(255, 30, 30)
                lblPrice.BackColor = Drawing.Color.FromArgb(255, 30, 30)
            ElseIf Val(lblPrice.Text) <= DownLimitePrice Then
                lblDiffence.ForeColor = Drawing.Color.White
                lblPercent.ForeColor = Drawing.Color.White
                lblPrice.ForeColor = Drawing.Color.White
                lblDiffence.BackColor = Drawing.Color.Green
                lblPercent.BackColor = Drawing.Color.Green
                lblPrice.BackColor = Drawing.Color.Green
            End If

            'If Val(lblOpen.Text) > Val(lblLast.Text) Then
            '    lblOpen.ForeColor = Drawing.Color.Red
            'ElseIf Val(lblOpen.Text) < Val(lblLast.Text) Then
            '    lblOpen.ForeColor = Drawing.Color.Green
            'End If

            'If Val(lblHigh.Text) > Val(lblLast.Text) Then
            '    lblHigh.ForeColor = Drawing.Color.Red
            'ElseIf Val(lblHigh.Text) < Val(lblLast.Text) Then
            '    lblHigh.ForeColor = Drawing.Color.Green
            'End If

            'If Val(lblLow.Text) > Val(lblLast.Text) Then
            '    lblLow.ForeColor = Drawing.Color.Red
            'ElseIf Val(lblLow.Text) < Val(lblLast.Text) Then
            '    lblLow.ForeColor = Drawing.Color.Green
            'End If

            '顯示位數
            If Val(lblLast.Text) > 99.9 Then
                lblPrice.Text = Format(Val(lblPrice.Text), "0.0")
                'lblHigh.Text = Format(Val(lblHigh.Text), "0.0")
                'lblLow.Text = Format(Val(lblLow.Text), "0.0")
                'lblLast.Text = Format(Val(lblLast.Text), "0.0")
                'lblOpen.Text = Format(Val(lblOpen.Text), "0.0")
            End If

            Dim lblVolume As Label = row.FindControl("lblVolume")
            If lblNumber.Text = "0000" Then
                lblVolume.Text &= "(億)"
            End If
        Next
 
    End Sub
End Class
