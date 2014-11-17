<%@ Page Language="VB" AutoEventWireup="false" CodeFile="kcharts.aspx.vb" Inherits="kcharts" %>

<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <link rel="apple-touch-icon" href="/custom_icon.png"/>
    <%--<meta name="apple-touch-fullscreen" content="YES" />--%>
    <link href="http://www.wantgoo.com/favicon.ico" type="image/x-icon" rel="shortcut icon" />
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
    <%--<meta name="apple-mobile-web-app-capable" content="yes" />--%>
    <%--<meta content="black" name="apple-mobile-web-app-status-bar-style" />--%>
    <link href="http://www.wantgoo.com/favicon.ico" type="image/x-icon" rel="shortcut icon" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
    <link href="styles/style.css?20120723" media="all" rel="stylesheet" type="text/css" />
    <link href="styles/responsive.css" media="all" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/styles/hp.css" />
<%--    <link href="m.css" media="all" rel="stylesheet" type="text/css" />--%>

    <script type="text/javascript">
        window.onload = function () { setTimeout(function () { window.scrollTo(0, 1); }, 100); }
        document.addEventListener("touchmove", function (event) {
            event.preventDefault();
        }, false);
        window.addEventListener('load', setOrientation, false);
        window.addEventListener('orientationchange', setOrientation, false);

        // 判斷螢幕旋轉方向
        function setOrientation() {
            var orient;
            if (window.orientation) {
                orient = Math.abs(window.orientation) === 90 ? 'landscape' : 'portrait';
            }
            else if (window.screen) {
                var width = screen.width;
                var height = screen.height;
                orient = (width > height) ? 'landscape' : 'portrait';
            }
            else {
                orient = 'portrait';
            }
            if (orient != 'landscape')
                window.location = "kchart.aspx?no=" + '<%=Request("no")%>';
        }
</script>

<script type="text/javascript" src="Scripts/Highstock-1.1.6/highstock.js"></script>
<script type="text/javascript" src="Scripts/Highstock-1.1.6/modules/exporting.js"></script>
<link href="Styles/kchart.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
<script type="text/javascript">
var chart;
$(function() {		
	// create the chart
	chart = new Highcharts.StockChart({
		chart: {
			renderTo: 'jChart1',
            plotBorderWidth: 2,
            alignTicks: false,
            animation: false
		},
        colors: [
            'red',
	        'blue', 
	        'Green', 
	        '#FF00FF', 
	        '#996600', 
	        'red', 
	        'blue', 
	        'blue', 
	        'red'
        ],
        legend: {
            enabled: false
        },
        scrollbar: {
            enabled: false
        },
		rangeSelector: {
			inputEnabled : false,
            enabled: false
		},
        navigator: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        plotOptions: {
            candlestick: { 
                upColor : 'red',
                color : 'green'
            },
            column: { 
                color : '#FF9900'
            },
            line: {
                lineWidth: 1
            }
		},
 		xAxis: {
		    labels : { 
                formatter : function() {return Highcharts.dateFormat('%Y/%m', this.value);}
            },
            gridLineWidth: 1
		},

        tooltip: {  
            yDecimals : 2,
            crosshairs: [{
                width: 1,
                color: 'blue'
            }, {
                width: 1,
                color: 'blue'
            }],
            formatter: function() {
                var s = '<table width="478px" class="charttable" cellpadding="0" cellspacing="0"><tr><td  colspan="2">' + Highcharts.dateFormat('%m/%d', this.x);
                var p = this.points[5];
                var volume ='量';
                if (p.series.name == "摩台指 (S2TWZ1)") { volume='OI';}
                 var value = parseFloat(this.points[6].y);
                 var tValue= value;
                 if (value>1000){
                    tValue=Highcharts.numberFormat(value/1000,1) + '(千)';
                 }
                 if (value>1000000){
                    tValue=Highcharts.numberFormat(value/1000000,1) + '(百萬)';
                 }
                  if (value>100000000){
                    tValue=Highcharts.numberFormat(value/100000000,1) + '(億)';
                 }
                 var open=p.point.open;
                 var high=p.point.high;
                 var low=p.point.low;
                 var close=p.point.close;
                 var d5=Highcharts.numberFormat(this.points[0].y,2);
                 var d10=Highcharts.numberFormat(this.points[1].y,2);
                 var d20=Highcharts.numberFormat(this.points[2].y,2);
                 var d60=Highcharts.numberFormat(this.points[3].y,2);
                 if (p.point.open>1000) {open=Highcharts.numberFormat(p.point.open,1);}
                 if (p.point.high>1000) {high=Highcharts.numberFormat(p.point.high,1);}
                 if (p.point.low>1000) {low=Highcharts.numberFormat(p.point.low,1);}
                 if (p.point.close>1000) {close=Highcharts.numberFormat(p.point.close,1);}
                 if (this.points[0].y>1000) {d5=Highcharts.numberFormat(this.points[0].y,1);}
                 if (this.points[1].y>1000) {d10=Highcharts.numberFormat(this.points[1].y,1);}
                 if (this.points[2].y>1000) {d20=Highcharts.numberFormat(this.points[2].y,1);}
                 if (this.points[3].y>1000) {d60=Highcharts.numberFormat(this.points[3].y,1);}
                s += '</td><td class="name">開</td><td class="val">' +  open +
                        '</td><td class="name">高</td><td class="val">' + high +
                        '</td><td class="name">低</td><td class="val">' + low +
                        '</td><td class="name">收</td><td class="val">' + close +
                        '</td></tr><tr><td class="name"></td><td class="val">' +
                        '</td><td class="name">5日</td><td>' + d5 +
                        '</td><td class="name">10日</td><td>' + d10 +
                        '</td><td class="name">20日</td><td>' + d20 +
                        '</td><td class="name">60日</td><td>' + d60 
                s += '</td></tr></table>'
               
                var s1 = '<table width="300px" class="charttable" cellpadding="0" cellspacing="0"><tr>';
                if ( parseInt(this.points[7].y) > 99999 )
                     s1 = '<table width="400px" class="charttable" cellpadding="0" cellspacing="0"><tr>';
                if ( parseInt(this.points[7].y) > 999999 )
                     s1 = '<table width="450px" class="charttable" cellpadding="0" cellspacing="0"><tr>';
                if ( parseInt(this.points[7].y) > 9999999 )
                     s1 = '<table width="500px" class="charttable" cellpadding="0" cellspacing="0"><tr>';
                s1 += '<td class="volume">' + volume + '</td><td>' + Highcharts.numberFormat(this.points[6].y,0) +
                        '</td><td class="name">MV5 ' + Highcharts.numberFormat(this.points[7].y,0) +
                        '</td><td class="name">MV20 ' + Highcharts.numberFormat(this.points[8].y,0) +
                        '</td></tr></table>';
                var s2 = '<table class="charttable" cellpadding="0" cellspacing="0"><tr>' +
                        '<td class="name">K9 ' + Highcharts.numberFormat(this.points[9].y,2) +
                        '</td><td class="name">D9 ' + Highcharts.numberFormat(this.points[10].y,2) +
                        '</td></tr></table>';
                $('#info').html(s);
                $('#info1').html(s1);
                $('#info2').html(s2);
                return false;
            }
        },
		yAxis: [{
			height: 150,
            opposite: true
		},{
			top: 160,
			height: 30,
            title: { margin: -65, text: '<% =GetVolumeTitle() %>',rotation :0,align: 'high',y: 15  }
		}, {
			top: 200,
			height: 30,
            max: 80,
            min: 20,
            labels:{ enabled: false},
            title: { margin: -20, text: 'KD',rotation: 0,align: 'high',y: 15 }
		},{
			height: 150,
            max: 1,
            min: 0,
            labels:{ enabled: false},
            gridLineWidth : 0
		}],
				    
		series: [{
			name: '5MA',
			data: [<% =GetData("mean5")%>]
		}, {
			name: '10MA',
			data: [<% =GetData("mean10")%>]
		}, {
			name: '20MA',
			data: [<% =GetData("mean20")%>]
		}, {
			name: '60MA',
			data: [<% =GetData("mean60")%>]
		}, {
			name: '120MA',
			data: [<% =GetData("mean120")%>]
		}, {
			type: 'candlestick',
			name: '<% =GetTitle() %>',
			id: 'a',
			data: [<% =GetData("0000")%>]
		},{
            type: 'column',
			name: '<% =GetVolumeTitle() %>',
			data: [<% =GetData("Volume")%>],
            yAxis: 1
		},{
			name: 'MV5',
			data: [<% =GetData("Mean5Volume")%>],
            yAxis: 1
		},{
			name: 'MV20',
			data: [<% =GetData("Mean20Volume")%>],
            yAxis: 1
		}, {
			name: 'K9',
			data: [<% =GetData("K9")%>],
            yAxis: 2
		},{
			name: 'D9',
			data: [<% =GetData("D9")%>],
            yAxis: 2
		},{
			name: '.',
			id: 'b',
			data: [[<% =GetData("Deduct60MA")%>,0],[<% =GetData("Deduct20MA")%>,0]],
            lineWidth : 0,
            yAxis: 3
		},{
		    type: 'flags',
		    name: '均線扣抵',
		    data: [{
				x: <% =GetData("Deduct60MA")%>,
				title: '季線扣抵<% =GetData("Deduct60MAValue")%>'
			},{
				x: <% =GetData("Deduct20MA")%>,
				title: '月線扣抵<% =GetData("Deduct20MAValue")%>'
			}],
		    onSeries: 'b',
		    shape: 'squarepin'
		}]
	});
});
</script>

<div class="ct" style="background-color:#fff;width:496px">
    <div id="jChart1" style="WIDTH: 496px; HEIGHT: 261px;margin:-9px 0px 0px -10px;position:relative;"></div>
    <div id="info" style="position:relative;z-index:1;margin:-250px 0px 0px -1px;"><asp:Literal ID="a" runat="server"></asp:Literal></div>
    <div id="info1" style="position:relative;z-index:1;margin:120px 20px 0px 0px;float:right;"><asp:Literal ID="b" runat="server"></asp:Literal></div>
    <div id="info2" style="position:relative;z-index:1;margin:25px 20px 0px 0px;float:right;"><asp:Literal ID="c" runat="server"></asp:Literal></div>
</div>

    </form>
</body>
</html>
 
 


