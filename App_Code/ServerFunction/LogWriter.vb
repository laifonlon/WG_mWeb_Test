Imports Microsoft.VisualBasic

Public Class LogWriter
    Private Shared mObj As New LogWriter
    Public Shared ReadOnly Property Instance() As LogWriter
        Get
            Return mObj
        End Get
    End Property

    Private Shared WebRootPath As String = HttpContext.Current.Server.MapPath("~")
    Private Shared WriteLogPath As String = WebRootPath + "ErrorLog\"

    Public Sub WriteLog(ByVal description As String)

        If Not My.Computer.FileSystem.DirectoryExists(WriteLogPath) Then
            IO.Directory.CreateDirectory(WriteLogPath)
        End If

        Dim fileName As String = WriteLogPath & Format(Now, "yyyyMMdd-HH") & ".txt"

        Try
            My.Computer.FileSystem.WriteAllText(fileName, description, True)
        Catch ex As Exception
        End Try

    End Sub
End Class
