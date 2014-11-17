Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Web


Public Class KevinDataAccess


    'Public Shared str_Connstr_twstock As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString1").ToString()
    'Public Shared str_Connstr_news As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("NowNewsConnectionString").ToString()
    Public Shared str_Connstr_Club As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("ClubConnection").ToString()
    'Public Shared str_Connstr_Wantgoo As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("WantGooConnection").ToString()


    ''' <summary>
    ''' sql injection 檢查函式
    ''' </summary>
    ''' <param name="str_sourcestr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function checkSQL(ByVal str_sourcestr As String) As String

        Dim tempstr As String = str_sourcestr
        tempstr = tempstr.ToLower()

        '取代不必要的符號
        Try
            tempstr = tempstr.Replace(ControlChars.NewLine, "")
            'tempstr = tempstr.Replace(ControlChars.Tab, "")
            'tempstr = tempstr.Replace(ControlChars.CrLf, "")
            'tempstr = tempstr.Replace(ControlChars.Cr, "")
            'tempstr = tempstr.Replace(ControlChars.Lf, "")
            'tempstr = tempstr.Replace(ControlChars.VerticalTab, "")
        Catch ex As Exception

        End Try



        '有些字串忘記了 或是非必要 
        '暫且檢查這些
        Dim checklist(22) As String

        checklist(0) = "'sa'"
        checklist(1) = "select "
        checklist(2) = "insert "
        checklist(3) = "update "
        checklist(4) = "delete "
        checklist(5) = "exec "
        checklist(6) = "truncate "
        checklist(7) = "cmdshell " 'xp_cmdshell
        checklist(8) = "script "
        checklist(9) = "iframe "
        checklist(10) = "javascript:"
        checklist(11) = "location.href"
        checklist(12) = "location.replace"
        checklist(13) = "location.asset"
        checklist(14) = "document.execcommand"
        checklist(15) = "--"
        checklist(16) = "/*"
        checklist(17) = "waitfor "
        checklist(18) = "delay "
        checklist(19) = "expression("
        checklist(20) = "grant "
        checklist(21) = "deny "
        checklist(22) = "revoke "


        '簡單的檢查一下
        For i As Integer = 0 To checklist.Length - 1
            Try
                If System.Web.HttpUtility.HtmlDecode(tempstr).IndexOf(checklist(i)) >= 0 Then
                    tempstr = ""
                    Exit For
                End If

                If System.Web.HttpUtility.UrlDecode(tempstr).IndexOf(checklist(i)) >= 0 Then
                    tempstr = ""
                    Exit For
                End If
            Catch ex As Exception

            End Try


        Next

        '準備回傳
        If tempstr <> "" Then
            Return str_sourcestr
        Else
            Return ""
        End If

        Return ""

    End Function


    ''' <summary>
    ''' 取得網址列參數 ( url querystring ) 且型態為 Integer
    ''' </summary>
    ''' <param name="querystringName">querystring name</param>
    ''' <param name="upOrDownSomeInt">是否要比特定的數值大或小  up / down</param>
    ''' <param name="theInt">前一個參數的基礎比較值</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getQueryStringForInt(ByVal querystringName As String, _
                                                Optional ByVal upOrDownSomeInt As String = "", _
                                                Optional ByVal theInt As Integer = 0) As Integer
        Dim tempstr As String
        Dim returnInt As Integer = 0
        If System.Web.HttpContext.Current.Request.QueryString(querystringName) IsNot Nothing Then
            tempstr = System.Web.HttpContext.Current.Request.QueryString(querystringName).ToString()



            Try
                returnInt = Integer.Parse(tempstr)
                If upOrDownSomeInt = "up" Or upOrDownSomeInt = "down" Then
                    If upOrDownSomeInt = "up" AndAlso returnInt < theInt Then
                        returnInt = theInt
                    End If
                    If upOrDownSomeInt = "down" AndAlso returnInt > theInt Then
                        returnInt = theInt
                    End If
                End If
            Catch ex As Exception

            End Try
        End If


        Return returnInt

    End Function


    ''' <summary>
    ''' 取得網址列參數 ( url querystring ) 且型態為 Double
    ''' </summary>
    ''' <param name="querystringName">querystring name</param>
    ''' <param name="upOrDownSomeDouble">是否要比特定的數值大或小  up / down</param>
    ''' <param name="theDouble">前一個參數的基礎比較值</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getQueryStringForDouble(ByVal querystringName As String, _
                                                Optional ByVal upOrDownSomeDouble As String = "", _
                                                Optional ByVal theDouble As Double = 0.0) As Double

        Dim tempstr As String = System.Web.HttpContext.Current.Request.QueryString(querystringName)

        Dim returnDouble As Double = 0.0
        Integer.TryParse(tempstr, returnDouble)
        Try
            If upOrDownSomeDouble = "up" Or upOrDownSomeDouble = "down" Then
                If upOrDownSomeDouble = "up" AndAlso returnDouble < theDouble Then
                    returnDouble = theDouble
                End If
                If upOrDownSomeDouble = "down" AndAlso returnDouble > theDouble Then
                    returnDouble = theDouble
                End If
            End If
        Catch ex As Exception

        End Try

        Return returnDouble

    End Function


    ''' <summary>
    ''' 取得網址列參數 ( url querystring ) 且型態為 String
    ''' </summary>
    ''' <param name="querystringName">querystring name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getQueryStringForString(ByVal querystringName As String) As String

        Dim tempstr As String = checkSQL(System.Web.HttpContext.Current.Request.QueryString(querystringName))
        Return tempstr

    End Function


    ''' <summary>
    ''' 取得SQL資料 回傳 DataTable
    ''' </summary>
    ''' <param name="connstr"></param>
    ''' <param name="sqlcmd"></param>
    ''' <param name="commandType"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getSqlDataReturnDataTable(ByVal connstr As String, _
                                                    ByVal sqlcmd As String, _
                                                    Optional ByVal commandType As Integer = CommandType.Text, _
                                                    Optional ByVal params() As SqlParameter = Nothing) As DataTable

        Dim dt As New DataTable

        Dim conn As New SqlConnection(connstr)
        Dim cmd As New SqlCommand(sqlcmd, conn)
        cmd.CommandType = commandType


        If params IsNot Nothing AndAlso params.Length > 0 Then
            For i As Integer = 0 To params.Length - 1
                cmd.Parameters.Add(params(i))
            Next
        End If


        conn.Open()

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader
            dt.Load(dr)
            cmd.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            conn.Close()
            conn.Dispose()
        End Try

        Return dt


    End Function


    ''' <summary>
    ''' 執行SQL 但是只需確認是否執行成功
    ''' </summary>
    ''' <param name="connstr"></param>
    ''' <param name="sqlcmd"></param>
    ''' <param name="commandType"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function executeSqlAndNoReturn(ByVal connstr As String, _
                                                  ByVal sqlcmd As String, _
                                                  Optional ByVal commandType As Integer = CommandType.Text, _
                                                  Optional ByVal params() As SqlParameter = Nothing) As Boolean

        Dim bool_return As Boolean = True

        Dim conn As New SqlConnection(connstr)
        Dim cmd As New SqlCommand(sqlcmd, conn)
        cmd.CommandType = commandType

        If params IsNot Nothing Then
            For i As Integer = 0 To params.Length - 1
                cmd.Parameters.Add(params(i))
            Next
        End If

        conn.Open()

        Try
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception

            bool_return = False

            conn.Close()
            conn.Dispose()
        End Try

        Return bool_return

    End Function




End Class
