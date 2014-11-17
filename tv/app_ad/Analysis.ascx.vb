
Partial Class HomePage_Analysis
    Inherits System.Web.UI.UserControl

    Protected Sub fv_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles fv.DataBound
        Dim d1 As Label = fv.FindControl("d1")
        Dim d2 As Label = fv.FindControl("d2")
        Dim a3 As Label = fv.FindControl("a3")
        Dim a5 As Label = fv.FindControl("a5")
        'Dim luck As Label = fv.FindControl("luck")

        'Dim raiseK As Label = fv.FindControl("raiseK")
        'Dim fallK As Label = fv.FindControl("fallK")
        Dim diffK As Label = fv.FindControl("diffK")
        'Dim luck2 As Label = fv.FindControl("luck2")

        Dim d1v As Double = Val(d1.Text.Trim("%"))
        Dim d2v As Double = Val(d2.Text.Trim("%"))
        Dim a3v As Double = Val(a3.Text.Trim("%"))
        Dim a5v As Double = Val(a5.Text.Trim("%"))
        Dim diffKv As Double = Val(diffK.Text)

        If d1v >= 50 Then
            d1.ForeColor = Drawing.Color.Red
        Else
            d1.ForeColor = Drawing.Color.Green
        End If

        If d2v >= 50 Then
            d2.ForeColor = Drawing.Color.Red
        Else
            d2.ForeColor = Drawing.Color.Green
        End If

        If a3v >= 50 Then
            a3.ForeColor = Drawing.Color.Red
        Else
            a3.ForeColor = Drawing.Color.Green
        End If

        If a5v >= 50 Then
            a5.ForeColor = Drawing.Color.Red
        Else
            a5.ForeColor = Drawing.Color.Green
        End If

    End Sub
End Class
