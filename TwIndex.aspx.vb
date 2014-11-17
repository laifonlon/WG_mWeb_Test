
Partial Class TwIndex
    Inherits System.Web.UI.Page
 
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Me.LoadData()
        End If
    End Sub

    Private Sub LoadData()
        Dim cStockNo As String = "0000"
        lblTechChat.Text = "<a href=""kchart.aspx?no=" + cStockNo.Replace("&", "$") + """><img width=""300px"" src=""" + "http://img.wantgoo.com/stock/Image/" + cStockNo + "sdk.png" + """></a>"
    End Sub
End Class

