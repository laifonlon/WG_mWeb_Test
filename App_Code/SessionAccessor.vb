Imports Microsoft.VisualBasic

Public Class SessionAccessor
    Private Shared mObj As New SessionAccessor
    Public Shared ReadOnly Property Instance() As SessionAccessor
        Get
            Return mObj
        End Get
    End Property

    Public Sub SetValue(ByVal key As Key, ByVal value As Object)
        HttpContext.Current.Session([Enum].GetName(GetType(Key), key)) = value
    End Sub

    Public Function GetValue(ByVal key As Key) As Object
        If HttpContext.Current.Session Is Nothing Then
            Return ""
        End If
        Return HttpContext.Current.Session([Enum].GetName(GetType(Key), key))
    End Function

    Public Sub Clear()
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session.Abandon()
    End Sub
End Class

Public Enum Key
    UserName = 0
    MemberNo = 1
    NickName = 2
    IsLogin = 3
    TalkMode
    TalkPageIndex
    TalkTime
End Enum
