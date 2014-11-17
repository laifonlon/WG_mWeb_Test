Imports Microsoft.VisualBasic

Public Class CountRecord
    Private Shared mObj As New CountRecord
    Public Shared ReadOnly Property Instance() As CountRecord
        Get
            Return mObj
        End Get
    End Property

    Private mHottipRuleSession As New HybridDictionary
    Public Function IsHottipRuleViewed(ByVal ruleID As String) As Boolean
        Dim key As String = ruleID + "_" + HttpContext.Current.Session.SessionID
        If mHottipRuleSession.Contains(key) Then
            mHottipRuleSession(key) = mHottipRuleSession(key) + 1
            Return True
        Else
            mHottipRuleSession.Add(key, 1)
            Return False
        End If
    End Function

    Private mBlogSession As New HybridDictionary
    Public Function IsBlogViewed(ByVal blogID As String) As Boolean
        Dim key As String = blogID + "_" + HttpContext.Current.Session.SessionID
        If mBlogSession.Contains(key) Then
            mBlogSession(key) = mBlogSession(key) + 1
            Return True
        Else
            mBlogSession.Add(key, 1)
            Return False
        End If
    End Function

    Private mBookReadSession As New HybridDictionary
    Public Function IsBookRead(ByVal id As String) As Boolean
        Dim key As String = id + "_" + HttpContext.Current.Session.SessionID
        If mBookReadSession.Contains(key) Then
            mBookReadSession(key) = mBookReadSession(key) + 1
            Return True
        Else
            mBookReadSession.Add(key, 1)
            Return False
        End If
    End Function

    Private mBookDownloadSession As New HybridDictionary
    Public Function IsBookDownload(ByVal id As String) As Boolean
        Dim key As String = id + "_" + HttpContext.Current.Session.SessionID
        If mBookDownloadSession.Contains(key) Then
            mBookDownloadSession(key) = mBookDownloadSession(key) + 1
            Return True
        Else
            mBookDownloadSession.Add(key, 1)
            Return False
        End If
    End Function

    Private mBookViewedSession As New HybridDictionary
    Public Function IsBookViewed(ByVal id As String) As Boolean
        Dim key As String = id + "_" + HttpContext.Current.Session.SessionID
        If mBookViewedSession.Contains(key) Then
            mBookViewedSession(key) = mBookViewedSession(key) + 1
            Return True
        Else
            mBookViewedSession.Add(key, 1)
            Return False
        End If
    End Function

    Private mNoPhoneModeSession As New StringCollection
    Public Function IsPhoneMode() As Boolean
        Dim key As String = HttpContext.Current.Session.SessionID
        If mNoPhoneModeSession.Contains(key) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub SetPhoneMode(ByVal isPhoneMode As Boolean)
        Dim key As String = HttpContext.Current.Session.SessionID
        If Not mNoPhoneModeSession.Contains(key) AndAlso Not isPhoneMode Then
            mNoPhoneModeSession.Add(key)
        ElseIf mNoPhoneModeSession.Contains(key) AndAlso isPhoneMode Then
            mNoPhoneModeSession.Remove(key)
        End If
    End Sub

End Class
