Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.Linq
Imports System.Data

''' <summary>
''' 提供寄信功能,與管理
''' </summary>
Public Class MailService

    Private Shared mObj As New MailService
    Public Shared ReadOnly Property Instance() As MailService
        Get
            Return mObj
        End Get
    End Property

    Private mSmtpServerIp As String = "smtp.gmail.com" '"124.150.130.6"
    Private mSmtpServerPort As Integer = 587 '4567
    Private mID As String = "noreply@wantgoo.com"
    Private mPassword As String = "wantgoo123!"

    Private mIDs As New Generic.List(Of GMailID)
    Private mGMailID As GMailID

    Public Sub New()

        mIDs.Add(New GMailID("noreply@wantgoo.com"))
        For i As Integer = 0 To 99
            mIDs.Add(New GMailID("noreply" + Format(i, "000") + "@wantgoo.com"))
        Next
        mGMailID = mIDs(0)
    End Sub

    Public Function SendMail(ByVal toAddress As String, ByVal toName As String, _
                             ByVal subject As String, ByVal body As String, _
                             Optional ByVal fromAddress As String = "service@wantgoo.com", _
                             Optional ByVal fromName As String = "玩股網") As Boolean


        'Debug.WriteCookie("MailService_SendMail01", DateTime.Now.ToString())
        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False

        If body Is Nothing OrElse body.Trim = "" Then Return False

        'GetGMailID()

        'Dim [from] As New MailAddress(mGMailID.ID, fromName)
        'Dim [to] As New MailAddress(toAddress, toName)
        'Dim mail As New MailMessage([from], [to])

        'mail.Subject = subject
        'mail.Body = body

        'Dim mailClient As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        'mailClient.EnableSsl = True
        'mailClient.UseDefaultCredentials = False
        'mailClient.Credentials = New System.Net.NetworkCredential(mGMailID.ID, mPassword)
        'mailClient.EnableSsl = False
        'mailClient.UseDefaultCredentials = True

        'Debug.WriteCookie("MailService_SendMail02", DateTime.Now.ToString())

        'Debug.WriteCookie("body", DateTime.Now.ToString() + " - " + body)
        'Debug.WriteCookie("body Replace vbCrLf", DateTime.Now.ToString() + " - " + body.Replace(vbCrLf, "<br />"))

        InsertSendMailNewQueue("MailService_SendMail", toAddress, subject, body.Replace(vbCrLf, "<br />"))
        'mailClient.Send(mail)

        'Debug.WriteCookie("MailService_SendMail03", DateTime.Now.ToString())

        'mGMailID.SendCount = mGMailID.SendCount + 1
        Return True
    End Function

    Public Shared Function InsertSendMailNewQueue(Type As String, ToAddress As String, Subject As String, Body As String) As Boolean
        'Debug.WriteCookie("MailService_InsertSendMailNewQueue", DateTime.Now.ToString())
        Try
            Dim MailConnection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("MailConnection").ConnectionString)
            MailConnection.Open()
            Dim cmdMailTxt As String = String.Format("exec [dbo].[SendMailNewQueueInsert] N'{0}',N'{1}',N'{2}',N'{3}';", Type, ToAddress, Subject, Body)
            'Debug.WriteCookie("cmdMailTxt MailService_InsertSendMailNewQueue", cmdMailTxt)
            Dim cmdMail As New SqlClient.SqlCommand(cmdMailTxt, MailConnection)
            cmdMail.ExecuteNonQuery()
            MailConnection.Close()
            MailConnection.Dispose()
            cmdMail.Dispose()
        Catch ex As Exception
            'Debug.WriteCookie("Exception_Source SQL " + Type, "Time：" + Now.ToString() + "…" + ex.Source)
            'Debug.WriteCookie("Exception_Message SQL " + Type, "Time：" + Now.ToString() + "…" + ex.Message)
            'Debug.WriteCookie("Exception_StackTrace SQL " + Type, "Time：" + Now.ToString() + "…" + ex.StackTrace)
        End Try
        Return True
    End Function


    Public Function GetGMailID() As GMailID
        For Each item As GMailID In mIDs
            If mGMailID.SendCount > item.SendCount Then
                mGMailID = item
            End If
        Next
        Return mGMailID
    End Function


    Public Function SendMailByArticale(ByVal toAddress As String, ByVal toName As String, _
                             ByVal subject As String, ByVal body As String, _
                             Optional ByVal fromAddress As String = "service@wantgoo.com", _
                             Optional ByVal fromName As String = "玩股網") As Boolean

        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False

        Dim [from] As New MailAddress(fromAddress, fromName)
        Dim [to] As New MailAddress(toAddress, toName)
        Dim mail As New MailMessage([from], [to])


        mail.Subject = subject

        Dim start As String = "<html><head><title>" & subject & "</title></head><body>"
        Dim [end] As String = "</body></html>"

        body = start & body & [end]

        mail.Body = body

        Dim mailClient As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        mailClient.EnableSsl = True
        mailClient.UseDefaultCredentials = False
        mailClient.Credentials = New System.Net.NetworkCredential(mID, mPassword)
        'mailClient.EnableSsl = False
        'mailClient.UseDefaultCredentials = True
        mail.IsBodyHtml = True
        mailClient.Send(mail)

        Return True
    End Function


    Public Function SendMail(ByVal toAddress As Generic.List(Of String), _
                         ByVal subject As String, ByVal body As String, _
                         Optional ByVal fromAddress As String = "service@wantgoo.com", _
                         Optional ByVal fromName As String = "玩股網") As Boolean

        If toAddress Is Nothing OrElse toAddress.Count = 0 Then Return False

        Dim [from] As New MailAddress(fromAddress, fromName)
        Dim [to] As MailAddress
        Dim mail As New MailMessage
        For Each address As String In toAddress
            [to] = New MailAddress(address)
            mail.Bcc.Add([to])
        Next

        mail.From = [from]
        mail.Subject = subject
        mail.Body = body

        Dim client As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        client.EnableSsl = False
        client.UseDefaultCredentials = True
        client.Send(mail)

        Return True
    End Function

    Public Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

End Class

Public Class GMailID
    Public Sub New(ByVal id As String)
        mID = id
    End Sub

    Private mID As String = ""
    Public Property ID() As String
        Get
            Return mID
        End Get
        Set(ByVal value As String)
            mID = value
        End Set
    End Property

    Private mSendCount As Integer
    Public Property SendCount() As Integer
        Get
            Return mSendCount
        End Get
        Set(ByVal value As Integer)
            mSendCount = value
        End Set
    End Property
End Class
