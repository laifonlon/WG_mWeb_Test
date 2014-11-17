Imports Microsoft.VisualBasic

Public Class RealTimeChart

#Region "Title"
    Private mTitle As String = ""
    Public Property Title() As String
        Get
            Return mTitle
        End Get
        Set(ByVal value As String)
            mTitle = value
        End Set
    End Property

    Private mTitleColor As String = "0000FF"
    Public Property TitleColor() As String
        Get
            Return mTitleColor
        End Get
        Set(ByVal value As String)
            mTitleColor = value
        End Set
    End Property

    Private mTitleSize As Integer = 13
    Public Property TitleSize() As Integer
        Get
            Return mTitleSize
        End Get
        Set(ByVal value As Integer)
            mTitleSize = value
        End Set
    End Property

    Private mUpdateTime As String = ""
    Public Property UpdateTime() As String
        Get
            Return mUpdateTime
        End Get
        Set(ByVal value As String)
            mUpdateTime = value
        End Set
    End Property

    Private Function GetTitleScript() As String
        If mTitle.Trim = "" Then Return ""
        Dim script As String = "&chtt=" + mTitle
        If mUpdateTime.TrimEnd <> "" Then
            script = script + " " + mUpdateTime.Trim
        End If
        script = script + "&chts=" + mTitleColor + "," + mTitleSize.ToString
        Return script
    End Function

#End Region

    Private mWidth As Integer = 600
    Public Property Width() As Integer
        Get
            Return mWidth
        End Get
        Set(ByVal value As Integer)
            mWidth = value
        End Set
    End Property

    Private mHeight As Integer = 250
    Public Property Height() As Integer
        Get
            Return mHeight
        End Get
        Set(ByVal value As Integer)
            mHeight = value
        End Set
    End Property

    Private Function GetSize() As String
        Dim script As String = "&chs=" + mWidth.ToString + "x" + mHeight.ToString
        Return script
    End Function

    Private mLineWidth As Double = 1
    Public Property LineWidth() As Double
        Get
            Return mLineWidth
        End Get
        Set(ByVal value As Double)
            mLineWidth = value
        End Set
    End Property

#Region "Line"

    Private mPrices As New Generic.List(Of Double)
    Private mTimes As New Generic.List(Of Date)
    Private mVolume As New Generic.List(Of Integer)

    Private mStartTime As Date
    Public Property StartTime() As Date
        Get
            Return mStartTime
        End Get
        Set(ByVal value As Date)
            mStartTime = value
        End Set
    End Property

    Private mEndTime As Date
    Public Property EndTime() As Date
        Get
            Return mEndTime
        End Get
        Set(ByVal value As Date)
            mEndTime = value
        End Set
    End Property

    Private mMax As Double
    Public Property Max() As Double
        Get
            Return mMax
        End Get
        Set(ByVal value As Double)
            mMax = value
        End Set
    End Property

    Private mMin As Double = 9999999
    Public Property Min() As Double
        Get
            Return mMin
        End Get
        Set(ByVal value As Double)
            mMin = value
        End Set
    End Property

    Private mOpen As Double = 0
    Public Property Open() As Double
        Get
            Return mOpen
        End Get
        Set(ByVal value As Double)
            mOpen = value
        End Set
    End Property

    Private mColor As String = "000000"
    Private mhasVolume As Boolean

    ''' <summary>
    ''' 設定股價資料數据
    ''' </summary>
    Public Sub SetData(ByVal prices As Generic.List(Of Double), _
                                  ByVal times As Generic.List(Of Date), _
                                  ByVal volume As Generic.List(Of Integer), _
                                  ByVal color As String)
        Me.mPrices.Clear()
        Me.mTimes.Clear()
        Me.mVolume.Clear()
        Me.mPrices.AddRange(prices)
        Me.mTimes.AddRange(times)
        Me.mVolume.AddRange(volume)
        mColor = color
        If mVolume.Count > 0 Then mhasVolume = True
    End Sub

    Public Sub SetUpdateTimeIsLastData()
        If mTimes.Count = 0 Then Exit Sub
        mUpdateTime = Format(mTimes(mTimes.Count - 1), "(MM/dd HH:mm)")
    End Sub

    Private mIsMaxMinAuto As Boolean
    Public Sub AutoSetMaxMin()
        If mPrices.Count = 0 Then Exit Sub
        mMax = 0
        mMin = 99999999
        For Each value As Double In mPrices
            mMax = Math.Max(value, mMax)
            mMin = Math.Min(value, mMin)
        Next

        If (mMax - mOpen) > (mOpen - mMin) Then
            mMin = mOpen - (mMax - mOpen)
        Else
            mMax = mOpen + (mOpen - mMin)
        End If

        mIsMaxMinAuto = True
    End Sub

    Private mIsTimeAxisAuto As Boolean
    Public Sub AutoSetTimeAxis()
        If mTimes.Count = 0 Then Exit Sub
        Dim startValue As Integer = Math.Floor(mTimes(0).Minute / 15)
        With mTimes(0)
            mStartTime = .AddMinutes(-.Minute + 15 * startValue)
        End With

        Dim endValue As Integer = Math.Ceiling(mTimes(mTimes.Count - 1).Minute / 15)
        With mTimes(mTimes.Count - 1)
            If endValue > 3 Then
                mEndTime = .AddHours(1).AddMinutes(-.Minute)
            Else
                mEndTime = .AddMinutes(-.Minute + 15 * endValue)
            End If
        End With
    End Sub

    Private Function GetLineScript() As String
        Dim script As String = "&chd=t:"
        Dim xRange As Double = mMax - mMin
        Dim timeRange As TimeSpan = mEndTime.Subtract(mStartTime)

        Dim timeScrpt As String = ""
        Dim pointScript As String = ""
        Dim prePoint As String = ""
        Dim i As Integer = mTimes.Count - 1
        Dim span As Integer

        Dim gap As Integer = Math.Floor(mTimes.Count / 100)
        If gap = 0 Then gap = 1
        For i = 0 To mTimes.Count - 1 Step gap
            If Format((mPrices(i) - mMin) / xRange * 100, "0.#") <> prePoint Then
                span = (mTimes(i).Hour - StartTime.Hour) * 3600 + (mTimes(i).Minute - StartTime.Minute) * 60 + mTimes(i).Second - StartTime.Second
                timeScrpt += Format(span / timeRange.TotalSeconds * 100, "0.#") + ","
                pointScript += Format((mPrices(i) - mMin) / xRange * 100, "0.#") + ","
            End If
            prePoint = Format((mPrices(i) - mMin) / xRange * 100, "0.#")
        Next

        '加最後一個點
        i = mTimes.Count - 1
        span = (mTimes(i).Hour - StartTime.Hour) * 3600 + (mTimes(i).Minute - StartTime.Minute) * 60 + mTimes(i).Second - StartTime.Second
        timeScrpt += Format(span / timeRange.TotalSeconds * 100, "0.#") + ","
        pointScript += Format((mPrices(i) - mMin) / xRange * 100, "0.#") + ","

        timeScrpt = timeScrpt.TrimEnd(",")
        script = script + timeScrpt + "|" + pointScript

        'For Each time As DateTime In mTimes
        '    Dim span As Integer = (time.Hour - StartTime.Hour) * 60 + time.Minute - StartTime.Minute
        '    script += Format(span / timeRange.TotalMinutes * 100, "0.##") + ","
        'Next

        'script = script.TrimEnd(",")
        'script += "|"

        'For Each point As Double In mPrices
        '    script += Format((point - mMin) / xRange * 100, "0.##") + ","
        'Next

        script = script.TrimEnd(",")
        script += "&chco="
        script += mColor
        script = script + "&chls=" + Format(mLineWidth, "0.##")

        Return script
    End Function

#End Region

#Region "Axis"

    Private mX As Generic.List(Of String)
    Private mXAxis As New Generic.List(Of String)
    Public Property XAxis() As Generic.List(Of String)
        Get
            Return mXAxis
        End Get
        Set(ByVal value As Generic.List(Of String))
            mXAxis = value
        End Set
    End Property

    Private mYAxis As New Generic.List(Of String)
    Public Property YAxis() As Generic.List(Of String)
        Get
            Return mYAxis
        End Get
        Set(ByVal value As Generic.List(Of String))
            mYAxis = value
        End Set
    End Property

    Private Function GetAxisScription() As String
        '軸刻度標題位置 chxp=
        '<axis_1_index>,<label_1_position>,...,<label_n_position>
        '  |...|
        '<axis_m_index>,<label_1_position>,...,<label_n_position>

        Dim script As String = "&chxt=x,y,r&chxl="
        Dim position As String = "&chxp="

        Dim totalMinutes As Integer = mEndTime.Subtract(mStartTime).TotalMinutes

        '畫時間刻度，若有成交量則不畫在這，畫在成交量圖上
        If mhasVolume Then
            script = "&chxt=y,r&chxl="
        Else
            script += "0:|" + Format(mStartTime, "HH:mm")
            position += "0,3"
            Dim tempTime As Date = mStartTime.AddHours(1) '.AddMinutes(-mStartTime.Minute)

            'mEndTime = mStartTime.AddHours(4).AddMinutes(-mStartTime.Minute).AddMinutes(30)

            While tempTime <= mEndTime
                If mWidth <= 400 Then
                    script = script + "|" + Format(tempTime, "HH")
                Else
                    script = script + "|" + Format(tempTime, "HH:mm")
                End If
                position = position + "," + Format(tempTime.Subtract(mStartTime).TotalMinutes / totalMinutes * 100, "0,##")
                tempTime = tempTime.AddHours(1)
            End While
            'script += "|" + Format(mEndTime, "HH:mm")
            'position += ",97"
        End If

        If mIsMaxMinAuto Then

            If mhasVolume Then
                script += "0:"
            Else
                script += "|1:"
            End If

            script += "|" + Format(mMin, "0.##")
            script += "|" + Format(mOpen, "0.##")
            script += "|" + Format(mMax, "0.##")

            If mhasVolume Then
                script += "|1:"
            Else
                script += "|2:"
            End If

            script += "|" + Format((mMin - mOpen) / mOpen, "0.##%")
            script += "|0.00%"
            script += "|" + Format((mMax - mOpen) / mOpen, "0.##%")
        Else

            Dim gap As Double = (mMax - mMin) / 10
            If mhasVolume Then
                script += "0:"
            Else
                script += "|1:"
            End If
            script += "|" + Format(mMin, "0.00")
            script += "|" + Format(mMin + gap, "0.00")
            script += "|" + Format(mMin + gap * 2, "0.00")
            script += "|" + Format(mMin + gap * 3, "0.00")
            script += "|" + Format(mMin + gap * 4, "0.00")
            script += "|" + Format(mOpen, "0.00")
            script += "|" + Format(mMax - gap * 4, "0.00")
            script += "|" + Format(mMax - gap * 3, "0.00")
            script += "|" + Format(mMax - gap * 2, "0.00")
            script += "|" + Format(mMax - gap, "0.00")
            script += "|" + Format(mMax, "0.00")

            If mhasVolume Then
                script += "|1:"
            Else
                script += "|2:"
            End If
            script += "|" + Format((mMin - mOpen) / mOpen, "0.00%")
            script += "|" + Format(-gap * 4 / mOpen, "0.00%")
            script += "|" + Format(-gap * 3 / mOpen, "0.00%")
            script += "|" + Format(-gap * 2 / mOpen, "0.00%")
            script += "|" + Format(-gap / mOpen, "0.00%")
            script += "|0.00%"
            script += "|" + Format(gap / mOpen, "0.00%")
            script += "|" + Format(gap * 2 / mOpen, "0.00%")
            script += "|" + Format(gap * 3 / mOpen, "0.00%")
            script += "|" + Format(gap * 4 / mOpen, "0.00%")
            script += "|" + Format((mMax - mOpen) / mOpen, "0.00%")
        End If

        Return script + position
    End Function


    Private Function GetAxisStyleScription() As String
        'chxs=
        '<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>
        '  |...|
        '<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>

        Dim script As String
        If mhasVolume Then
            script = "&chxs=0,,12|1,,12"
        Else
            script = "&chxs=0,,12|1,,12|2,,12"
        End If
        Return script
    End Function

    Private Function GetGridScription() As String
        '語法 chg= 
        '<x_axis_step_size>,<y_axis_step_size>,<dash_length>,<space_length>,<x_offset>,<y_offset>
        Dim script As String = "&chg="

        Dim xStep As Double = 60 / mEndTime.Subtract(mStartTime).TotalMinutes * 100
        Dim yStep As Double = 10
        If mIsMaxMinAuto Then yStep = 50
        Dim dashLength As Integer = 3
        Dim spaceLength As Integer = 3
        Dim xOffset As Double = 0 'xStep * (60 - mStartTime.Minute) / 60
        'If mStartTime.Minute = 0 Then xOffset = 0
        Dim yOffset As Double = 0

        script = script + Format(xStep, "0.##") + "," + yStep.ToString + "," + _
        dashLength.ToString + "," + _
        spaceLength.ToString + "," + _
        Format(xOffset, "0.##") + "," + _
        yOffset.ToString

        Return script
    End Function
#End Region

    Public Function GetChart() As String
        Dim url As String = "http://chart.apis.google.com/chart?cht=lxy"
        url += GetSize()
        'url += GetTitleScript()
        url += GetLineScript()
        url += GetGridScription()
        url += GetAxisScription()
        url += GetAxisStyleScription()
        Return url
    End Function

    Public Function GetVolumeChart() As String
        Dim url As String = "http://chart.apis.google.com/chart?cht=lxy"
        url += GetSize()
        url += GetTitleScript()
        url += GetLineScript()
        url += GetGridScription()
        url += GetAxisScription()
        Return url
    End Function

End Class

