Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Web.Hosting

' 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class GetFiles
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function UploadFile(f As Byte(), rawUrl As String, fileName As String) As String

        Dim DefaultDirectory = HostingEnvironment.MapPath("~/UploadFiles/")

        If Not String.IsNullOrEmpty(rawUrl) Then
            DefaultDirectory = HostingEnvironment.MapPath("~/" + rawUrl.TrimStart("/"))
        End If

        If Not Directory.Exists(DefaultDirectory) Then
            Directory.CreateDirectory(DefaultDirectory)
        End If

        Dim ms As MemoryStream = New MemoryStream(f)
        Dim fs As FileStream = New FileStream(DefaultDirectory + fileName, FileMode.Create)
        ms.WriteTo(fs)

        ms.Close()
        fs.Close()
        fs.Dispose()

        Return DefaultDirectory + fileName

    End Function

End Class