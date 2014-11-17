<%@ Page Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="Chart.aspx.vb" Inherits="Chart" Title="即時走勢 - 玩股網手機版" %>
<%@ OutputCache Duration="60" VaryByParam="no" %>

<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
<script type="text/javascript" src="Scripts/Highstock-1.2.4/highstock.js"></script>
<script type="text/javascript" src="Scripts/Highstock-1.2.4/modules/exporting.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<asp:Panel ID="NoVolume" runat="server" Visible="false">
<script type="text/javascript">
    var chart;
    $(document).ready(function () {
        window.chart = new Highcharts.StockChart({
            chart: {
                renderTo: 'RTchart',
                plotBorderWidth: 1
            },
            scrollbar: {
                enabled: false
            },        
            navigator: {
                enabled: false
            },
            title: {
                text: '<%=GetTitle() %>',
                floating: true,
                x: -50,
                top: 20
            },
            subtitle: {
                text: '玩股網',
                floating: true,
                x: 100,
                y: 15
            },
            exporting: {
                enabled: false
            },
            rangeSelector : {
                enabled: false,
				inputEnabled : false
			},
            yAxis: [{
                opposite: true,
                plotLines : [{
					value : <% =GetLast("Last") %>,
					color : 'black',
					dashStyle : 'shortdash',
					width : 1,
					label : {text : '昨收'},
                    zIndex : 2
                }],
                labels: {
                    formatter: function() {
                        var percent = (this.value - <% =GetLast("Last") %>)*100/<% =GetLast("Last") %>
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
                yDecimals : 2,
                crosshairs: [{
                    width: 1,
                    color: 'blue'
                }, {
                    width: 1,
                    color: 'blue'
                }]
            }, 
            series: [{
                name: '<% =GetTitle() %>',
                data: [<% =GetData("Price") %>]
            }]
        });
    });  
</script> 
</asp:Panel>
<asp:Panel ID="WithVolume" runat="server" Visible="false">
<script type="text/javascript">
    var chart;
    $(document).ready(function () {
        window.chart = new Highcharts.StockChart({
            chart: {
                renderTo: 'RTchart',
                alignTicks: false,
                plotBorderWidth: 1
            },
            scrollbar: {
                enabled: false
            },        
            navigator: {
                enabled: false
            },
            title: {
                text: '<%=GetTitle() %>',
                floating: true,
                x: -50,
                top: 20
            },
            subtitle: {
                text: '玩股網',
                floating: true,
                x: 100,
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
                    lineWidth: 2
                },
                column: { 
                    color : '#FF9900'
                }
		    },
            yAxis: [{
                opposite: true,
                plotLines : [{
					value : <% =GetLast("FallLimit") %>,
					color : 'green',
					dashStyle : 'shortdash',
					width : 2,
					label : {text : '跌停'}
				}, {
					value : <% =GetLast("RaiseLimit") %>,
					color : 'red',
					dashStyle : 'shortdash',
					width : 2,
                    label : {text : '漲停'}
                }, {
					value : <% =GetLast("Last") %>,
					color : 'black',
					dashStyle : 'shortdash',
					width : 1,
                    label : {text : '昨收'},
                    zIndex : 1
				}],
                height: 145,
//                height: 208,
                max: <% =GetLast("max") %>,
                min: <% =GetLast("min") %>,
                labels: {
                    formatter: function() {
                        var percent = (this.value - <% =GetLast("Last") %>)*100/<% =GetLast("Last") %>
                        if (percent > 0) {
                             return this.value  +'<br /><span style="color:red;">+' + Highcharts.numberFormat(percent, 1, '.') +'%</span>'; 
                        } else if (percent < 0) {
                             return this.value  +'<br /><span style="color:green;">' + Highcharts.numberFormat(percent, 1, '.') +'%</span>'; 
                        } else if (percent = 0) {
                             return this.value  +'<br />0.0%</span>'; 
                        }
                    }
                },
                opposite: true
		    },{
			    top: 180,
			    height: 62
		    }],
 		    xAxis: {
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
                }]
            }, 

            series: [{
                name: '<% =GetTitle() %>',
                data: [<% =GetData("Price") %>]
            }, {
                type: 'column',
                name: '成交量(張)',
                data: [<% =GetData("Volume") %>],
                yAxis: 1
            }]
        });
    });  
</script> 
</asp:Panel>
<uc1:DealInfo ID="DealInfo1" runat="server" EnableViewState="False" Head="即時報價" />
<div id="RTchart" style="WIDTH: 310px; HEIGHT: 240px"></div>
<asp:SqlDataSource ID="sdsRealTimeData" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
        SelectCommand="SELECT  [Time] ,[Deal] ,[Volume]
  FROM [RealTimePrice] WHERE (StockNo = @StockNo)   And Time > DateAdd(hour,-16,(Select MAX(Time) From [RealTimePrice] Where (StockNo = @StockNo)) )" 
        EnableViewState="False">
    <SelectParameters>
        <asp:Parameter DefaultValue="0000" Name="StockNo" />
    </SelectParameters>
</asp:SqlDataSource>
</asp:Content>

