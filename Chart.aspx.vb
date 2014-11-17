
Partial Class Chart
    Inherits System.Web.UI.Page
 
    Private Function GetStockNo() As String
        Dim stockNo As String = Request("no")
        If stockNo Is Nothing OrElse stockNo.Length > 10 Then stockNo = "0000"
        Return stockNo
    End Function

    Public Function GetTitle() As String
        Dim stockNo As String = Me.GetStockNo
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If Application(stockNo + "_Name") IsNot Nothing Then
            Return Application(stockNo + "_Name") + " (" + stockNo.ToUpper + ")"
        End If

        'Response.Redirect("/stock/loaddata.aspx?f=name&url=" + Request.Url.ToString)
        StockLoader.Instance.LoadStockName()

        Return Application(stockNo + "_Name") + " (" + stockNo.ToUpper + ")"

        Return stockNo
    End Function

    Public Function GetLast(ByVal type As String) As String
        Dim stockNo As String = Me.GetStockNo
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        Dim key As String = stockNo + "_" + type
        Dim timeKey As String = stockNo + "_Price_LastTime"
        If Application(key) IsNot Nothing AndAlso Application(timeKey) IsNot Nothing Then
            If Now < Application(timeKey) Then
                Return Application(key).ToString
            End If
        End If

        'Response.Redirect("/stock/loaddata.aspx?f=price&no=" + stockNo.Replace("&", "$") + "&url=" + Request.Url.ToString.Replace("&", "$"))
        StockLoader.Instance.LoadPrice(stockNo)

        Return Application(key).ToString
    End Function

    Public Function GetLimit(ByVal type As String) As String
        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)
        Select Case type
            Case "Volume"
                If mVolume IsNot Nothing Then Return mVolume
            Case "Price"
                If mPrice IsNot Nothing Then Return mPrice
            Case "RaiseLimit"
                Return "[" + New Date(Now.Year, Now.Month, Now.Day, 9, 0, 1).Subtract(dt1970).TotalMilliseconds.ToString + ", " + GetLast("RaiseLimit") + "]"
            Case "FallLimit"
                Return "[" + New Date(Now.Year, Now.Month, Now.Day, 13, 30, 0).Subtract(dt1970).TotalMilliseconds.ToString + ", " + GetLast("FallLimit") + "]"
        End Select
        Return ""
    End Function

    Private mVolume As String
    Private mPrice As String
    Public Function GetData(ByVal type As String) As String
        Select Case type
            Case "Volume"
                If mVolume IsNot Nothing Then Return mVolume
            Case "Price"
                If mPrice IsNot Nothing Then Return mPrice
        End Select

        Dim stockNo As String = Me.GetStockNo
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        Dim key As String = stockNo + "_realtime_" + type
        Dim timeKey As String = stockNo + "_lastRealtime"

        If Application(key) IsNot Nothing AndAlso Application(timeKey) IsNot Nothing Then
            If Now < Application(timeKey) Then
                Return Application(key)
            End If
        End If

        sdsRealTimeData.SelectParameters("StockNo").DefaultValue = stockNo
        Dim dt1970 As DateTime = New DateTime(1970, 1, 1)
        Dim time As DateTime
        Dim lastTime As DateTime
        Dim sbPrice As New StringBuilder
        Dim sbVolume As New StringBuilder

        Dim en As IEnumerator = sdsRealTimeData.Select(New DataSourceSelectArguments).GetEnumerator
        While en.MoveNext
            'sbVolume.Append("[" + New Date(Now.Year, Now.Month, Now.Day, 9, 0, 1).Subtract(dt1970).TotalMilliseconds.ToString + ",0],")
            time = en.Current("Time")
            If time.Subtract(lastTime).TotalSeconds >= 60 OrElse _
                (en.Current("Volume") IsNot DBNull.Value AndAlso en.Current("Volume") > 0 AndAlso IsNumeric(stockNo) AndAlso stockNo <> "0000") Then
                sbPrice.Append("[")
                sbPrice.Append(time.Subtract(dt1970).TotalMilliseconds.ToString)
                sbPrice.Append(",")
                sbPrice.Append(en.Current("Deal"))
                sbPrice.Append("],")

                If en.Current("Volume") IsNot DBNull.Value AndAlso en.Current("Volume") > 0 Then
                    sbVolume.Append("[")
                    sbVolume.Append(time.Subtract(dt1970).TotalMilliseconds.ToString)
                    sbVolume.Append(",")
                    sbVolume.Append(en.Current("Volume") / 1000)
                    sbVolume.Append("],")
                End If

                lastTime = time
            End If
        End While

        'sbVolume.Append("[" + New Date(Now.Year, Now.Month, Now.Day, 13, 30, 0).Subtract(dt1970).TotalMilliseconds.ToString + ",0],")

        mPrice = sbPrice.ToString.Trim(",")
        mVolume = sbVolume.ToString.Trim(",")
        Application(stockNo + "_realtime_Volume") = mVolume
        Application(stockNo + "_realtime_Price") = mPrice
        Application(timeKey) = Now.AddSeconds(60)

        Select Case type
            Case "Volume"
                If mVolume IsNot Nothing Then Return mVolume
            Case "Price"
                If mPrice IsNot Nothing Then Return mPrice
        End Select

        Return ""
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim stockNo As String = Me.GetStockNo
        stockNo = Left(stockNo, 10).Trim.Replace("$", "&")

        If IsNumeric(stockNo) And stockNo <> "0000" Then
            WithVolume.Visible = True
        Else
            '全球指數
            NoVolume.Visible = True
        End If

        Me.Title = Me.GetTitle & "即時報價 - 玩股網手機版"
    End Sub
End Class
