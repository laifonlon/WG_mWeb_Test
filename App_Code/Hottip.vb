Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Collections.Generic

Public Class Hottip

    Private Shared mObj As New Hottip
    Public Shared ReadOnly Property Instance() As Hottip
        Get
            Return mObj
        End Get
    End Property

#Region "個股追蹤的群組名稱"
    Private mGroupName As New Dictionary(Of String, List(Of String))
    Public Sub ClearGroupName(memberNo As String)
        If mGroupName.ContainsKey(memberNo) = True Then
            mGroupName(memberNo) = Nothing
        End If
    End Sub

    Public Sub AddGroupName(memberNo As String, names As List(Of String))
        If mGroupName.ContainsKey(memberNo) = True Then
            mGroupName(memberNo) = names
        Else
            mGroupName.Add(memberNo, names)
        End If
    End Sub

    Public Function GetGroupName(ByVal memberNo As String) As List(Of String)
        Dim groupNames = New List(Of String)

        If mGroupName.ContainsKey(memberNo) = True AndAlso mGroupName(memberNo) IsNot Nothing Then
            groupNames = mGroupName(memberNo)
        Else
            Me.LoadGroupName()
        End If

        If groupNames.Count < 10 Then
            '預設的群組名稱
            For index As Integer = 1 To 10
                groupNames.Add("群組" & index)
            Next
            mGroupName(memberNo) = groupNames
        End If

        Return groupNames
    End Function

    Private Sub LoadGroupName()
        With HttpContext.Current
            If .Request.Url Is Nothing Then
                .Response.Redirect("\hottip\loaddata.aspx?f=groupname")
            Else
                .Response.Redirect("\hottip\loaddata.aspx?f=groupname&url=" + .Request.Url.ToString.Replace("&", "$"))
            End If
        End With
    End Sub
#End Region

End Class
