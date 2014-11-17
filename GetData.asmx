<%@ WebService Language="VB" Class="GetData" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization

' 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace := "http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _  
Public Class GetData
    Inherits System.Web.Services.WebService 
    
    Private twStockConnectString As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString

    <WebMethod()> _
    Public Sub GlobalData()
        
        '設定輸出格式為json格式
        Me.Context.Response.ContentType = "application/json"
        
        Dim timeKey As String = "GlobalData_LastTime"
        Dim key As String = "GlobalData"
        If Application(timeKey) IsNot Nothing AndAlso _
            Application(timeKey) > Now Then
            Me.Context.Response.Write(Application(key))
            Exit Sub
        End If

        Dim sdsStock As New SqlDataSource
        sdsStock.ConnectionString = twStockConnectString
        sdsStock.SelectCommand = "SELECT StockNo, Deal, Change, Time, Market, showorder FROM Stock " + _
            "WHERE Market = 10 or Market = 11 or Market =20 or Market = 21 or Market = 22  or  " + _
            "Market = 5 Or Market = 3 Or " + _
            "StockNo = 'DJI' or StockNo ='NAS' or StockNo ='SP5' or StockNo ='SOX' or StockNo ='IBO' or  " + _
            "StockNo='NBI' or StockNo ='DJC' or StockNo ='YDX' or StockNo = '0000' "
        Dim en As IEnumerator = sdsStock.Select(New DataSourceSelectArguments).GetEnumerator
     
        Dim dic As New Generic.Dictionary(Of String, String)
        Dim deal As String = ""
        Dim change As String = ""
        Dim stockNo As String = ""
        Dim percent As String = ""
        Dim color As String = "black"
        While en.MoveNext
            
            stockNo = en.Current("StockNo").ToString.Trim
            If Application(stockNo + "_gTime") Is Nothing OrElse _
                Application(stockNo + "_gTime") <> en.Current("Time") OrElse _
                stockNo.ToLower.Trim = "s2twz1" OrElse _
                stockNo.ToLower.Trim = "b1ym&" OrElse _
                (Now.Minute Mod 3 = 0 AndAlso Now.Second <= 10) Then
                
                '顯示位數
                If en.Current("Deal") > 99.9 Then
                    deal = Format(en.Current("Deal"), "0.0")
                ElseIf en.Current("Deal") < 1 Then
                    deal = Format(en.Current("Deal"), "0.0000")
                Else
                    deal = Format(en.Current("Deal"), "0.00")
                End If

                If Math.Abs(en.Current("Change")) > 0.5 Then
                    change = Format(en.Current("Change"), "0.0")
                ElseIf Math.Abs(en.Current("Change")) > 0.01 Then
                    change = Format(en.Current("Change"), "0.00")
                ElseIf en.Current("Change") = 0 Then
                    change = "0"
                Else
                    change = en.Current("Change").ToString
                End If
                
                percent = Format(en.Current("Change") / (en.Current("Deal") - en.Current("Change")), "0.00%")
                
                If en.Current("Change") > 0 Then
                    percent = "+" + percent
                    change = "▲" + change
                    color = "red"
                ElseIf en.Current("Change") < 0 Then
                    change = "▼" + change.Trim("-")
                    color = "green"
                End If

                Application(en.Current("StockNo").ToString.Trim + "_gTime") = en.Current("Time")
                
                stockNo = stockNo.Replace("&", "")
                dic.Add(stockNo + "_Price", deal)
                dic.Add(stockNo + "_Change", change)
                dic.Add(stockNo + "_Percent", percent)
                dic.Add(stockNo + "_Time", Format(en.Current("Time"), "HH:mm"))
                dic.Add(stockNo, color)
                
            End If
        End While
        
        '新版的可以用
        'System.Runtime.Serialization.Json.DataContractJsonSerializer

        Dim serializer As New JavaScriptSerializer()

        '輸出json格式
        Application(key) = serializer.Serialize(dic)
        Application(timeKey) = Now.AddSeconds(10)
        
        Me.Context.Response.Write(Application(key))
        'Me.Context.Response.Write(serializer.Serialize(dic))
    End Sub
    
    '<WebMethod(CacheDuration:=30)> _
    'Public Sub GlobalAllData()
        
    '    '設定輸出格式為json格式
    '    Me.Context.Response.ContentType = "application/json"

    '    Dim sdsStock As New SqlDataSource
    '    sdsStock.ConnectionString = twStockConnectString
    '    sdsStock.SelectCommand = "SELECT StockNo, Deal, Change, Time, Market, showorder FROM Stock " + _
    '        "WHERE Market = 10 or Market = 11 or Market =20 or Market = 21 or Market = 22  or  " + _
    '        "Market = 5 Or Market = 3 Or " + _
    '        "StockNo = 'DJI' or StockNo ='NAS' or StockNo ='SP5' or StockNo ='SOX' or StockNo ='IBO' or  " + _
    '        "StockNo='NBI' or StockNo ='DJC' or StockNo ='YDX' or StockNo = '0000' "
    '    Dim en As IEnumerator = sdsStock.Select(New DataSourceSelectArguments).GetEnumerator
     
    '    Dim dic As New Generic.Dictionary(Of String, String)
    '    Dim deal As String = ""
    '    Dim change As String = ""
    '    Dim stockNo As String = ""
    '    Dim percent As String = ""
    '    Dim color As String = "black"
    '    While en.MoveNext
            
    '        stockNo = en.Current("StockNo").ToString.Trim
  
    '            '顯示位數
    '            If en.Current("Deal") > 99.9 Then
    '                deal = Format(en.Current("Deal"), "0.0")
    '            ElseIf en.Current("Deal") < 1 Then
    '                deal = Format(en.Current("Deal"), "0.0000")
    '            Else
    '                deal = Format(en.Current("Deal"), "0.00")
    '            End If

    '            If Math.Abs(en.Current("Change")) > 0.5 Then
    '                change = Format(en.Current("Change"), "0.0")
    '            ElseIf Math.Abs(en.Current("Change")) > 0.01 Then
    '                change = Format(en.Current("Change"), "0.00")
    '            ElseIf en.Current("Change") = 0 Then
    '                change = "0"
    '            Else
    '                change = en.Current("Change").ToString
    '            End If
                
    '            percent = Format(en.Current("Change") / (en.Current("Deal") - en.Current("Change")), "0.00%")
                
    '            If en.Current("Change") > 0 Then
    '                percent = "+" + percent
    '                change = "▲" + change
    '                color = "red"
    '            ElseIf en.Current("Change") < 0 Then
    '                change = "▼" + change.Trim("-")
    '                color = "green"
    '            End If

    '            Application(en.Current("StockNo").ToString.Trim + "_gTime") = en.Current("Time")
                
    '            stockNo = stockNo.Replace("&", "")
    '            dic.Add(stockNo + "_Price", deal)
    '            dic.Add(stockNo + "_Change", change)
    '            dic.Add(stockNo + "_Percent", percent)
    '            dic.Add(stockNo + "_Time", Format(en.Current("Time"), "HH:mm"))
    '            dic.Add(stockNo, color)
                
    '    End While
        
    '    '新版的可以用
    '    'System.Runtime.Serialization.Json.DataContractJsonSerializer

    '    Dim serializer As New JavaScriptSerializer()

    '    '輸出json格式
    '    Me.Context.Response.Write(serializer.Serialize(dic))

    'End Sub
    
End Class