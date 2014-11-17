<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RateInfo.aspx.vb" Inherits="RateInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<meta name="viewport" content="width=device-width, initial-scale=1" />
<style>
body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6 {
padding: 0;
margin: 0;
font-weight: 100;
list-style-type: none;
font-size: 1em;
-webkit-text-size-adjust: none;
font-family: helvetica,'Nokia Sans','Apple LiGothic','Lihei pro','微軟正黑體', Arial,Sans-serif;
}
<%--body{ background-color:Black; color:White;}--%>
.tb{width:100%;border-collapse: collapse;}
.tb tr{ border-bottom:1px solid #888888;line-height:30px;}
.name a{color:white; text-decoration:none;}
 
.subnav {
background: #5A5A5A;
padding: 5px;
clear: left;
font-weight: bold;
font-size:1.1em;
border-bottom: 1px solid #007FFF;
}
</style>
<script type="text/javascript" src="Scripts/Highstock-1.3.1/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="Scripts/Highstock-1.3.1/highstock.js"></script>
<script type="text/javascript" src="Scripts/Highstock-1.3.1/exporting.js"></script>
</head>
<body>
<form id="form1" runat="server">
<asp:Panel ID="s" runat="server"  Visible="False">
    <asp:Label ID="lblUpdateInterval" runat="server" Text="60"></asp:Label>
        <div style="float: right;"><asp:HyperLink ID="Update" runat="server" NavigateUrl="globals.aspx"></asp:HyperLink></div>
        <h3><asp:Literal ID="lblTitle" runat="server"></asp:Literal></h3>
</asp:Panel>
<a class="updatelink" href='<% =Request.Url.ToString %>'><asp:Literal ID="lblTime" runat="server"></asp:Literal></a>
<table cellpadding="0" cellspacing="0" style="width:100%; height:50px; padding:0 10px; background-color:#FEEBD8;border-bottom: 1px solid #EDBE9D;-webkit-box-shadow:0px 3px 8px rgba(228, 196, 213, 0.6);-moz-box-shadow:0px 3px 8px rgba(228, 196, 213, 0.6);box-shadow:0px 3px 8px rgba(228, 196, 213, 0.6);"><tr>
<td><span id="Span1"><asp:Literal ID="b" runat="server" EnableViewState="False"></asp:Literal><span style="text-decoration:underline;color:#55AAFF; float:right;font-size:0.8em;margin-top:3px; "><asp:Label ID="lblElapseTime" runat="server" Text="0"></asp:Label></span>
</tr></table></td></tr></table>
<%--即時走勢圖--%>
<script type="text/javascript">
    var chart;
    $(document).ready(function () {
        window.chart = new Highcharts.StockChart({
            chart: {
                renderTo: 'chart1',
                plotBorderWidth: 1
            },
            scrollbar: { enabled: false },        
            navigator: { enabled: false },
            credits: { enabled: false },
            title: {
                text: '即時走勢',
                floating: true,
                top: 20,
                align: 'left',
                x:5
            },
            subtitle: {
                text: '',
                floating: true,
                x: 130,
                y: 15
            },
            exporting: {
                enabled: false
            },
            rangeSelector : {
                enabled: false,
				inputEnabled : false
			},
            plotOptions: {
                line: {
                    marker:{states:{hover:{enabled:false},select:{enabled:false}}}
                }
		    },
            yAxis: [{
                opposite: true,
                plotLines : [{
					value : <% =RateLast() %>,
					color : 'black',
					dashStyle : 'shortdash',
					width : 1,
					label : {text : '昨收'},
                    zIndex : 2
                }],
                labels: {
                    formatter: function() {
                        var percent = (this.value - <% =RateLast() %>)*100/<% =RateLast() %>
                        if (percent > 0) {
                             return this.value  +'<br /><span style="color:red;">+' + Highcharts.numberFormat(percent, 1, '.') +'%</span>'; 
                        } else if (percent < 0) {
                             return this.value  +'<br /><span style="color:green;">' + Highcharts.numberFormat(percent, 1, '.') +'%</span>'; 
                        } else if (percent = 0) {
                             return this.value  +'<br />0.0%</span>'; 
                        }
                    }
                }
            }],
 		    xAxis: {
                gridLineWidth: 1 
		    },
            tooltip: {  
                backgroundColor: '#2d2d2d',
                crosshairs: [{
                    width: 1,
                    color: 'blue'
                }, {
                    width: 1,
                    color: 'blue'
                }],
                formatter: function() {
                var p = '';
                var unit = '';
                var decimal = 4;
                if(this.point) {
                    p += '<b style="color:#fff;">'+ Highcharts.dateFormat('%m/%d %H:%M', this.point.x) +'</b><br/>';
                    p += this.point.config.text; // This will add the text on the flags
                }
                else {              
                    p += '<b style="color:#fff;">'+ Highcharts.dateFormat('%m/%d %H:%M', this.x) +'</b><br/>';
                    $.each(this.points, function(i, series){
                        p += '<span style="color:' + this.series.color + '">' + this.series.name + 
                        '</span> : <b style="color:#fff;">' +  Highcharts.numberFormat(this.y, decimal) + ' ' + unit + '</b><br/>';
                     });
                }
                return p;
            }
        },
            series: [{
                name: '<% =GetTitle() %>',
                data: [<% =GetRateChart() %>],
                color:'#4572A7'
            }]
        });
    });  
</script>
<div id="chart1" style="WIDTH:98%; HEIGHT: 240px"></div>
<%--技術分析圖--%>
<script type="text/javascript">
var chart;
$(function() {		
	// create the chart
	chart = new Highcharts.StockChart({
		chart: {
			renderTo: 'chart2',
            plotBorderWidth: 1,
            alignTicks: false,
            animation: false
		},
        legend: {
            enabled: false,
            floating : true,
            x: -5, 
            y: -198,
            borderWidth: 0
        },
        scrollbar: {enabled: false},
		rangeSelector: {enabled: false},
        navigator: {enabled: false },
        exporting: {enabled: false },
        credits: { enabled: false },
		title: {
			text: '歷史走勢',
            floating: true,
            top: 15,
            align: 'left',
            x:5
		},
        subtitle: {
            text: '',
            floating: true,
            x: 250,
            y: 15
        },
        plotOptions: {
            line: {
                marker:{states:{hover:{enabled:false},select:{enabled:false}}}
            }
        },
 		xAxis: {
		    labels : { 
                formatter : function() {return Highcharts.dateFormat('%Y/%m', this.value);}
            },
            gridLineWidth: 1
		},
        tooltip: {  
                backgroundColor: '#2d2d2d',
                crosshairs: [{
                    width: 1,
                    color: 'blue'
                }],
                formatter: function() {
                var p = '';
                var unit = '';
                var decimal = 4;
                if(this.point) {
                    p += '<b style="color:#fff;">'+ Highcharts.dateFormat('%Y/%m/%d', this.point.x) +'</b><br/>';
                    p += this.point.config.text; // This will add the text on the flags
                }
                else {              
                    p += '<b style="color:#fff;">'+ Highcharts.dateFormat('%Y/%m/%d', this.x) +'</b><br/>';
                    $.each(this.points, function(i, series){
                        p += '<span style="color:' + this.series.color + '">' + this.series.name + 
                        '</span> : <b style="color:#fff;">' +  Highcharts.numberFormat(this.y, decimal) + ' ' + unit + '</b><br/>';
                     });
                }
                return p;
            }
        },
		yAxis: [{
            opposite: true
		}],
		series: [{
            name: '<% =GetTitle() %>', data: [<% =GetHistoryRate() %>]
        }]
	});
});
</script>
<div id="info" ><asp:Literal ID="a" runat="server"></asp:Literal></div>
<div id="chart2" style="WIDTH:98%; HEIGHT: 350px;"></div>

<asp:SqlDataSource ID="sdsRTRate" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
        SelectCommand="SELECT [Date], Rate FROM RateRealTime WHERE FromID = @FromID And ToID = @ToID  And [Date] > DateAdd(hour,-24,(Select MAX([Date]) From RateRealTime Where  FromID = @FromID And ToID = @ToID  )) " 
        EnableViewState="False">
    <SelectParameters>
        <asp:Parameter Name="FromID" />
        <asp:Parameter Name="ToID" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sdsHistoryRate" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
        SelectCommand="SELECT [Date], Rate FROM RateHistory WHERE FromID = @FromID And ToID = @ToID And  (DATEDIFF(year, Date, GETDATE()) <3)" 
        EnableViewState="False">
    <SelectParameters>
        <asp:Parameter Name="FromID" />
        <asp:Parameter Name="ToID" />
    </SelectParameters>
</asp:SqlDataSource>
    </form>
</body>
</html>
