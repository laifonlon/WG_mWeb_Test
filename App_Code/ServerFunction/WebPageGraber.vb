Imports Microsoft.VisualBasic
Imports System.Net
Imports System.IO

Public Class WebPageGraber

    Public Overloads Function QueryWebPage(ByVal url As String, ByVal encoding As System.Text.Encoding) As String
        Dim request As HttpWebRequest = HttpWebRequest.Create(url)
        Dim response As HttpWebResponse = request.GetResponse
        Dim sr As StreamReader = New StreamReader(response.GetResponseStream, encoding)
        Dim content As String = sr.ReadToEnd
        sr.Close()
        request = Nothing
        response = Nothing

        Return content
    End Function

    Public Overloads Function QueryWebPage(ByVal url As String) As IO.Stream
        Dim request As HttpWebRequest = HttpWebRequest.Create(url)
        Dim response As HttpWebResponse = request.GetResponse

        Return response.GetResponseStream
    End Function

End Class
