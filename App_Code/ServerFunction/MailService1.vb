Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.Text.RegularExpressions
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data

''' <summary>
''' 提供寄信功能,與管理
''' </summary>
Public Class MailServiceNew

    Private Shared mObj As New MailServiceNew
    Public Shared ReadOnly Property Instance() As MailServiceNew
        Get
            Return mObj
        End Get
    End Property

    Private mWantGooSmtpIp As String = "114.34.3.97"
    Private mWantGooSmtpPort As Integer = 4567

    Private mSmtpServerIp As String = "smtp.gmail.com"
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

#Region "Club"
    Public Function SendHtmlMailByClub(ByVal masterNo As String, ByVal toAddress As String, ByVal toName As String, _
                             ByVal subject As String, ByVal body As String, _
                             Optional ByVal attachments As List(Of String) = Nothing, _
                             Optional ByVal fromAddress As String = "noreply@wantgoo.com", _
                             Optional ByVal fromName As String = "玩股網") As Boolean
        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False

        Dim [from] As New MailAddress(fromAddress, fromName)
        Dim [to] As New MailAddress(toAddress, toName)
        Dim mail As New MailMessage([from], [to])

        mail.Subject = subject
        mail.Body = body
        mail.IsBodyHtml = True
        mail.BodyEncoding = Text.Encoding.GetEncoding("BIG5")

        If attachments IsNot Nothing AndAlso attachments.Count > 0 Then
            For Each file As String In attachments
                mail.Attachments.Add(New Net.Mail.Attachment(file))
            Next
        End If

        GetGMailID()
        SendByGmailByClub(masterNo, mail)

        Return True
    End Function

    Private Sub SendByGmailByClub(ByVal masterNo As String, ByVal mail As MailMessage)
        Dim mailClient As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        mailClient.EnableSsl = True
        mailClient.UseDefaultCredentials = False
        mailClient.Credentials = New System.Net.NetworkCredential(mGMailID.ID, mPassword)


        If mail.To.Count > 0 Then
            Try
                mailClient.Send(mail)
                WriteLogByClub(masterNo, mGMailID.ID, mail.To(0).Address, mail.Subject)
            Catch ex As SmtpException
                WriteLogByClub(masterNo, mGMailID.ID + vbTab + ex.Message, mail.To(0).Address, mail.Subject)
            Finally
                mGMailID.SendCount = mGMailID.SendCount + 1
            End Try
        End If
    End Sub

    Private mClubLogPath As String = "C:\WantGoo\Wantgoooooo1\ClubTemp\Club"

    Private Sub WriteLogByClub(ByVal masterNo As String, ByVal message As String, ByVal email As String, ByVal title As String)
        If Not My.Computer.FileSystem.DirectoryExists(mClubLogPath & "\" & masterNo) Then
            My.Computer.FileSystem.CreateDirectory(mClubLogPath & "\" & masterNo)
        End If
        If title <> "" Then title = title + vbTab
        Dim filename As String = mClubLogPath & "\" & masterNo & "\" & Format(Now, "yyyyMMdd") & ".log"
        Dim content As String = Format(Now, "HH:mm") + vbTab + email + vbTab + title + message + vbCrLf
        If mIsLogWriting Then
            mLogTemp = mLogTemp + content
        Else
            mIsLogWriting = True
            If content <> "" Then
                Try
                    My.Computer.FileSystem.WriteAllText(filename, mLogTemp, True)
                Catch ex As Exception
                End Try
            End If

            mLogTemp = ""

            Try
                My.Computer.FileSystem.WriteAllText(filename, content, True)
            Catch ex As Exception
            End Try

            mIsLogWriting = False
        End If
    End Sub
#End Region
    Private mHtmlTemplate As String = _
"<table align=""center"" bgcolor=""#ffffff"" cellpadding=""0"" cellspacing=""0"" width=""650"" style="" font-family: Arial, Helvetica, sans-serif;"">" + _
"<tr>" + _
"    <td bgcolor=""#ff6633"" width=""650"" style=""padding:3px; border:1px solid #ff3300;" + _
"        color:#fff; font-weight:bold; text-align:left;"">" + _
"        <span>&nbsp;Wantgoo 玩股網</span>" + _
"    </td>" + _
"</tr>" + _
"<tr>" + _
"    <td width=""650"" style=""padding:3px; border:1px solid #fcce7c; border-top:none; text-align:left; line-height:22px;"">" + _
"        <div style=""padding:5px;"">" + _
"            <span style=""font-size:12px;"">親愛的 @Name 您好， </span><br />" + _
"        </div>" + _
"        <div style=""font-size:12px; color:Black; padding:16px; text-align:left; "">" + _
"@Content" + _
"         </div>" + _
"    </td>" + _
"</tr>" + _
"<tr>" + _
"    <td style="" text-align:left;"">" + _
"        <div style=""font-size:12px; color:Gray; line-height:18px; margin-top:5px;"">" + _
"            <span>若需任何服務及幫助，請與 service@wantgoo.com Wantgoo Team聯絡，謝謝！</span>" + _
"        </div>" + _
"    </td>" + _
"</tr>" + _
"</table>"

    Public Function SendHtmlMail(ByVal toAddress As String, ByVal toName As String, _
                             ByVal subject As String, ByVal body As String, _
                             Optional ByVal attachments As List(Of String) = Nothing, _
                             Optional ByVal fromAddress As String = "noreply@wantgoo.com", _
                             Optional ByVal fromName As String = "玩股網", Optional ByVal useFormat As Boolean = False) As Boolean

        'Debug.WriteCookie("MailServiceNew_SendHtmlMail01", DateTime.Now.ToString())
        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False
        If useFormat Then
            body = mHtmlTemplate.Replace("@Name", toName).Replace("@Content", body)
        End If

        Dim [from] As New MailAddress(fromAddress, fromName)
        Dim [to] As New MailAddress(toAddress, toName)
        Dim mail As New MailMessage([from], [to])

        mail.Subject = subject
        mail.Body = body
        mail.IsBodyHtml = True
        mail.BodyEncoding = Text.Encoding.GetEncoding("BIG5")

        'Debug.WriteCookie("MailServiceNew_SendHtmlMail02", DateTime.Now.ToString())

        MailService.InsertSendMailNewQueue("MailServiceNew_SendHtmlMail", toAddress, subject, body)

        'If attachments IsNot Nothing AndAlso attachments.Count > 0 Then
        '    For Each file As String In attachments
        '        mail.Attachments.Add(New Net.Mail.Attachment(file))
        '    Next
        'End If

        'If toAddress.ToLower.Contains("yahoo") OrElse _
        '   toAddress.ToLower.Contains("hinet") OrElse _
        '   toAddress.ToLower.Contains("pchome") Then
        '    GetGMailID()
        '    SendByGmail(mail)
        'Else
        '    SendByWantGoo(mail)
        'End If
        'Debug.WriteCookie("MailServiceNew_SendHtmlMail03", DateTime.Now.ToString())

        Return True
    End Function

    Public Function SendHtmlMail(ByVal toAddress As String, ByVal toName As String, _
                         ByVal BccList As List(Of String), _
                         ByVal subject As String, ByVal body As String, _
                         Optional ByVal attachments As List(Of String) = Nothing, _
                         Optional ByVal fromAddress As String = "noreply@wantgoo.com", _
                         Optional ByVal fromName As String = "玩股網") As Boolean

        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False

        Dim [from] As New MailAddress(fromAddress, fromName)
        Dim [to] As New MailAddress(toAddress, toName)
        Dim mail As New MailMessage([from], [to])

        For Each item As String In BccList
            mail.Bcc.Add(item)
        Next

        mail.Subject = subject
        mail.Body = body
        mail.IsBodyHtml = True
        mail.BodyEncoding = Text.Encoding.GetEncoding("BIG5")

        If attachments IsNot Nothing AndAlso attachments.Count > 0 Then
            For Each file As String In attachments
                mail.Attachments.Add(New Net.Mail.Attachment(file))
            Next
        End If

        If toAddress.ToLower.Contains("yahoo") OrElse _
        toAddress.ToLower.Contains("hinet") OrElse _
       toAddress.ToLower.Contains("pchome") Then
            GetGMailID()
            SendByGmail(mail)
        Else
            SendByWantGoo(mail)
        End If

        Return True
    End Function

    Private Sub SendByGmail(ByVal mail As MailMessage)
        Dim mailClient As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        mailClient.EnableSsl = True
        mailClient.UseDefaultCredentials = False
        mailClient.Credentials = New System.Net.NetworkCredential(mGMailID.ID, mPassword)


        If mail.To.Count > 0 Then
            Try
                mailClient.Send(mail)
                WriteLog(mGMailID.ID, mail.To(0).Address, mail.Subject)
            Catch ex As SmtpException
                WriteLog(mGMailID.ID + vbTab + ex.Message, mail.To(0).Address, mail.Subject)
            Finally
                mGMailID.SendCount = mGMailID.SendCount + 1
            End Try
        End If
    End Sub

    Private Sub SendByWantGoo(ByVal mail As MailMessage)
        Dim mailClient As New SmtpClient(mWantGooSmtpIp, mWantGooSmtpPort)
        mailClient.EnableSsl = False
        mailClient.UseDefaultCredentials = True

        If mail.To.Count > 0 Then
            Try
                mailClient.Send(mail)
                WriteLog("Wantgoo", mail.To(0).Address, mail.Subject)
            Catch ex As SmtpException
                WriteLog("Wantgoo   " + ex.Message, mail.To(0).Address, mail.Subject)
            End Try
        End If
    End Sub

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

    Public Function SendTextMail(ByVal toAddress As String, ByVal toName As String, _
                             ByVal subject As String, ByVal body As String, _
                             Optional ByVal attachments As List(Of String) = Nothing, _
                             Optional ByVal fromAddress As String = "noreply@wantgoo.com", _
                             Optional ByVal fromName As String = "玩股網") As Boolean
        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False

        GetGMailID()

        Dim [from] As New MailAddress(mGMailID.ID, fromName)
        Dim [to] As New MailAddress(toAddress, toName)
        Dim mail As New MailMessage([from], [to])

        mail.Subject = subject
        mail.Body = body
        If attachments IsNot Nothing AndAlso attachments.Count > 0 Then
            For Each file As String In attachments
                mail.Attachments.Add(New Net.Mail.Attachment(file))
            Next
        End If

        Dim mailClient As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        mailClient.EnableSsl = True
        mailClient.UseDefaultCredentials = False
        mailClient.Credentials = New System.Net.NetworkCredential(mGMailID.ID, mPassword)
        'mailClient.EnableSsl = False
        'mailClient.UseDefaultCredentials = True
        mailClient.Send(mail)

        mGMailID.SendCount = mGMailID.SendCount + 1
        Return True
    End Function

    Public Function SendMail(ByVal toAddress As String, ByVal toName As String, _
                             ByVal subject As String, ByVal body As String, _
                             Optional ByVal fromAddress As String = "noreply@wantgoo.com", _
                             Optional ByVal fromName As String = "玩股網") As Boolean

        If toAddress Is Nothing OrElse toAddress.Trim = "" Then Return False
        If Not EmailAddressCheck(toAddress) Then Return False

        GetGMailID()

        Dim [from] As New MailAddress(mGMailID.ID, fromName)
        Dim [to] As New MailAddress(toAddress, toName)
        Dim mail As New MailMessage([from], [to])

        mail.Subject = subject
        mail.Body = body

        Dim mailClient As New SmtpClient(mSmtpServerIp, mSmtpServerPort)
        mailClient.EnableSsl = True
        mailClient.UseDefaultCredentials = False
        mailClient.Credentials = New System.Net.NetworkCredential(mGMailID.ID, mPassword)
        'mailClient.EnableSsl = False
        'mailClient.UseDefaultCredentials = True
        mailClient.Send(mail)

        mGMailID.SendCount = mGMailID.SendCount + 1
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

    Private mIsLogWriting As Boolean
    Private mLogTemp As String = ""
    Private mLogPath As String = "C:\WantGoo\Wantgoooooo1\tmp\Mail\"
    Private Sub WriteLog(ByVal message As String, ByVal email As String, ByVal title As String)
        If Not My.Computer.FileSystem.DirectoryExists(mLogPath) Then
            My.Computer.FileSystem.CreateDirectory(mLogPath)
        End If
        If title <> "" Then title = title + vbTab
        Dim filename As String = mLogPath + Format(Now, "yyyyMMdd") & ".log"
        Dim content As String = Format(Now, "HH:mm:ss") + vbTab + email + vbTab + title + message + vbCrLf
        If mIsLogWriting Then
            mLogTemp = mLogTemp + content
        Else
            mIsLogWriting = True
            If content <> "" Then
                Try
                    My.Computer.FileSystem.WriteAllText(filename, mLogTemp, True)
                Catch ex As Exception
                End Try
            End If

            mLogTemp = ""

            Try
                My.Computer.FileSystem.WriteAllText(filename, content, True)
            Catch ex As Exception
            End Try

            mIsLogWriting = False
        End If
    End Sub

End Class

