Imports Microsoft.VisualBasic

Public Class HtmlCleaner

#Region "Singleton"
    Private Shared mObj As New HtmlCleaner
    Public Shared ReadOnly Property Instance() As HtmlCleaner
        Get
            Return mObj
        End Get
    End Property

    Public Function RemoveSpace(ByVal article As String) As String
        If article Is Nothing OrElse article.Trim = "" Then Return ""

        Return article.Replace("&nbsp;", "").Trim
    End Function

    Public Function RemoveHtmlTag(ByVal count As Integer, ByVal article As String) As String
        Return RemoveHtmlTag(article, count)
    End Function

    Public Function RemoveHtmlTag(ByVal article As String, Optional ByVal count As Integer = 0) As String
        If article Is Nothing OrElse article.Trim = "" Then Return ""

        Dim removedContent As String = Regex.Replace(System.Web.HttpUtility.HtmlDecode(article), "<(.|\n)*?>", "").Replace(" ", "").Replace("&nbsp;", "")

        If count <= 0 Then
            Return removedContent
        Else
            Return Left(removedContent, count)
        End If
    End Function

    ''' <summary>
    ''' 去除圖片顯示
    ''' </summary>
    Public Function RemovePicture(ByVal content As String) As String
        If content Is Nothing OrElse content.Trim = "" Then Return ""

        MemberDataAccessor.Instance.Action = "BolgRemovePhoto"

        Dim removedContent As String = content.ToLower
        Dim picPosition As Integer = removedContent.IndexOf("<img")
        Dim endPosition As Integer

        While picPosition >= 0
            endPosition = removedContent.IndexOf(">", picPosition)
            If picPosition >= endPosition Then Exit While
            removedContent = Left(removedContent, picPosition) + Right(removedContent, removedContent.Length - endPosition - 1)
            picPosition = removedContent.IndexOf("<img")
        End While

        Return removedContent
    End Function

    Public Function ContainIllegalWord(ByVal content As String) As Boolean
        If (content.ToLower.Contains("<textarea") AndAlso _
            content.ToLower.Contains(">")) OrElse _
            content.ToLower.Contains("</textarea>") Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' 去除不完整的HtmlTag
    ''' </summary>
    Public Function RemoveLastNoCompleteTag(ByVal content As String) As String
        If content Is Nothing OrElse content.Trim = "" Then Return ""

        MemberDataAccessor.Instance.Action = "HtmlRemoveNoCompleteTag"

        Dim removedContent As String = content
        Dim startPosition As Integer = removedContent.LastIndexOf("<")
        Dim endPosition As Integer = removedContent.LastIndexOf(">", startPosition)
        If startPosition >= endPosition Then
            removedContent = Left(removedContent, startPosition)
        End If

        Return removedContent
    End Function

    ''' <summary>
    ''' 去除註解
    ''' </summary>
    Public Function RemoveMark(ByVal content As String) As String
        If content Is Nothing OrElse content.Trim = "" Then Return ""

        MemberDataAccessor.Instance.Action = "HtmlRemoveMark"

        Dim removedContent As String = content.ToLower
        Dim picPosition As Integer = removedContent.IndexOf("<!--")
        Dim endPosition As Integer

        While picPosition >= 0
            endPosition = removedContent.IndexOf("-->", picPosition)
            If picPosition >= endPosition Then Exit While
            removedContent = Left(removedContent, picPosition) + Right(removedContent, removedContent.Length - endPosition - 1)
            picPosition = removedContent.IndexOf("<!--")
        End While

        Return removedContent
    End Function

    '''  <summary> 
    ''' 清理Word生成的冗余HTML  
    '''  </summary> 
    Public Function CleanWordHtml(ByVal html As String) As String
        Dim sc As New StringCollection()
        ' get rid of unnecessary tag spans (comments and title) 
        sc.Add(" <!--(\w|\W)+?-->")
        sc.Add(" <title>(\w|\W)+? </title>")
        ' Get rid of classes and styles 
        sc.Add("\s?class=\w+")
        sc.Add("\s+style='[^']+'")
        ' Get rid of unnecessary tags 
        'sc.Add(@" <(meta|link|/?o:|/?style|/?div|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>"); 
        sc.Add(" <(meta|link|/?o:|/?style|/?font|/?strong|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>")
        ' Get rid of empty paragraph tags 
        sc.Add("( <[^>]+>)+ ( </\w+>)+")
        ' remove bizarre v: element attached to  <img> tag 
        sc.Add("\s+v:\w+=""[^""]+""")
        ' remove extra lines 
        sc.Add("(\n\r){2,}")
        For Each s As String In sc
            html = Regex.Replace(html, s, "", RegexOptions.IgnoreCase)
        Next
        Return html
    End Function
#End Region
End Class
