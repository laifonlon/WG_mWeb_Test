<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="KChart.aspx.vb" Inherits="KChart" %>
<%@ OutputCache Duration="1800" VaryByParam="no" %>

<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
<script type="text/javascript">
    window.onload = function () { setTimeout(function () { window.scrollTo(0, 1); }, 100); }
//    document.addEventListener("touchmove", function (event) {
//        event.preventDefault();
//    }, false);
    function checkserAgent() {
        var userAgentInfo = navigator.userAgent;
        var userAgentKeywords = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod", "MQQBrowser");
        var flag = false;
        //排除windows系统
        if (userAgentInfo.indexOf("Windows NT") == -1) {
            flag = true;
        }
        return flag;
    }
    //Mobile
    if (checkserAgent()) {
        window.addEventListener('load', setOrientation, false);
        window.addEventListener('orientationchange', setOrientation, false);
    }

    // 判斷螢幕旋轉方向
    function setOrientation() {
        var orient= 'portrait';
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
        if (orient != 'portrait')
            window.location = "kchartl.aspx?no=" + '<%=Request("no")%>';
    }
</script>

<script type="text/javascript" src="Scripts/Highstock-1.2.4/highstock.js"></script>
<script type="text/javascript" src="Scripts/Highstock-1.2.4/modules/exporting.js"></script>
<link href="Styles/kchart.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
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
		title: {
			text: '<% =GetTitle() %>',
            floating: true,
            x: 10,
            top: 20
		},
        subtitle: {
            text: '玩股網',
            floating: true,
            x: 120,
            y: 15
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
                var s = '<table width="317px" class="charttable" cellpadding="0" cellspacing="0"><tr><td colspan="2">' + Highcharts.dateFormat('%m/%d', this.x);
                var p = this.points[5];
                var volume ='量';
                if (p.series.name == "摩台指 (S2TWZ1)")
                 {   volume='OI';}
                 var value = parseFloat(this.points[6].y);
                 var tValue= value;
                 if (value>1000){
                    tValue=Highcharts.numberFormat(value/1000,1) + '(千)';
                 }else if (value>1000000){
                    tValue=Highcharts.numberFormat(value/1000000,1) + '(百萬)';
                 }else if (value>100000000){
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
                s += '</td><td class="name">收</td><td class="val">' + close +
                        '</td><td class="name">' + volume + '</td><td class="val">' +tValue +
                        '</td></tr><tr><td class="name">開</td><td class="val">' + open +
                        '</td><td class="name">高</td><td class="val">' + high +
                        '</td><td class="name">低</td><td class="val">' + low +
                        '</td></tr><tr><td class="name">5日</td><td class="val">' + d5 +
                        '</td><td class="name">20日</td><td class="val">' + d20 +
                        '</td><td class="name">60日</td><td class="val">' + d60 +
                        '</td></tr></table>'
                $('#info').html(s);
                return false;
            }
        },
		yAxis: [{
			height: 220,
            opposite: true
		},{
			top: 230,
			height: 40,
            title: { margin: -65, text: '<% =GetVolumeTitle() %>',rotation :0,align: 'high',y: 15  }
		}, {
			top: 280,
			height: 40,
            max: 80,
            min: 20,
            labels:{ enabled: false},
            title: { margin: -20, text: 'KD',rotation: 0,align: 'high',y: 15 }
		},{
			height: 220,
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

<div class="mg5" style=" text-align:left; margin:-6px 0px 0px -6px"><div style="background-color:#fff;">
<div class="ct">
<div id="info"style="position:relative;z-index:1; margin-left:-1px"><asp:Literal ID="a" runat="server"></asp:Literal></div>
<div id="jChart1" style="WIDTH: 336px; HEIGHT: 350px;margin:-9px 0px 0px -10px;position:relative;"></div>
</div></div></div>
</asp:Content>

