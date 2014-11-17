Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Web

Public Class ClubClassForMobile

    Shared Function ClubHomeList() As DataTable

        Dim dt As New DataTable()



        Try
            dt = DirectCast(System.Web.HttpContext.Current.Cache.Get("dt_ClubHomeList"), DataTable)
        Catch ex As Exception
            dt = Nothing
        End Try
        ' dt = Nothing
        If dt Is Nothing OrElse dt.Rows.Count < 5 Then
            Dim sqlcmd As String = "ClubHomeListForMobile"

            dt = KevinDataAccess.getSqlDataReturnDataTable(KevinDataAccess.str_Connstr_Club, sqlcmd, CommandType.StoredProcedure)

            System.Web.HttpContext.Current.Cache.Insert("dt_ClubHomeList", dt, Nothing, DateTime.UtcNow.AddMinutes(5), Cache.NoSlidingExpiration)

        End If


        Return dt

    End Function
End Class
