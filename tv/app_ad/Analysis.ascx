<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Analysis.ascx.vb" Inherits="HomePage_Analysis" %>
<%-- <script type="text/javascript">
     $(function () {
         $("#analysis img[title]").tooltip({
             offset: [10, 2],
             effect: 'slide'
         });
     }); 
</script>--%>
<div class="mg5">
<div class="hd1">
    <h3>大盤即時力道 <span>ANALYSIS</span></h3>
</div>
<asp:FormView ID="fv" runat="server" DataSourceID="sdsADline" BorderWidth="0px" Width="100%">
<ItemTemplate>
<div class="ct">
<div class="ring">
    <table class="anl">
        <tr><th>今日</th><td><asp:Label ID="d1" runat="server" Text='<%# format(eval("d1"),"0.00%") %>'></asp:Label></td><th>3日平均</th><td><asp:Label ID="a3" runat="server" Text='<%# format(eval("a3"),"0.00%") %>'></asp:Label></td></tr>
        <tr><th>昨日</th><td><asp:Label ID="d2" runat="server" Text='<%# format(eval("d2"),"0.00%") %>'></asp:Label></td><th>5日平均</th><td><asp:Label ID="a5" runat="server" Text='<%# format(eval("a5"),"0.00%") %>'></asp:Label></td></tr>
    </table>
    <table class="anl">
        <tr><th style="width:35%;">即時力道分析</th>
        <td style="width:65%;">漲停/紅K: <asp:Label ID="raiseLimit" runat="server" Text='<%# eval("RaiseLimitCount") %>' ForeColor="Red"></asp:Label><span style="color:#666;">/</span><asp:Label ID="raiseK" runat="server" Text='<%# eval("RaiseKCount") %>' ForeColor="Red"></asp:Label>家<br />
        跌停/黑K: <asp:Label ID="fallLimit" runat="server" Text='<%# eval("FallLimitCount") %>' ForeColor="Green"></asp:Label><span style="color:#666;">/</span><asp:Label ID="fallK" runat="server" Text='<%# eval("FallKCount") %>' ForeColor="Green"></asp:Label>家
        <asp:Label ID="diffK" runat="server" Text='<%# eval("RaiseKCount") - eval("FallKCount") %>' Font-Bold="True" Visible="False"></asp:Label></td></tr>
    </table>
</div>
</div>
</ItemTemplate></asp:FormView>
</div>
<asp:SqlDataSource ID="sdsADline" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="
    declare @Date date
    Set @Date = (Select Max(Date) FROM [Statistics]) 
    SELECT(
Select cast(RaiseCount as real)/(RaiseCount+FallCount) From 
(SELECT Top 1 FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t) as d1,
(SELECT cast(RaiseCount as real)/(RaiseCount+FallCount) From 
(SELECT Top 1 FallCount, RaiseCount From
(SELECT Top 2 Date, FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as d2,
(SELECT cast(RaiseCount as real)/(RaiseCount+FallCount) From 
(SELECT Top 1 FallCount, RaiseCount From
(SELECT Top 3 Date, FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as d3,
(SELECT cast(RaiseCount as real)/(RaiseCount+FallCount) From 
(SELECT Top 1 FallCount, RaiseCount From
(SELECT Top 4 Date, FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as d4,
(SELECT cast(RaiseCount as real)/(RaiseCount+FallCount) From 
(SELECT Top 1 FallCount, RaiseCount From
(SELECT Top 5 Date, FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as d5,
(SELECT cast(RaiseCount as real)/(RaiseCount+FallCount) From 
(SELECT Top 1 FallCount, RaiseCount From
(SELECT Top 6 Date, FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as d6,
(Select cast(Sum(RaiseCount) as real)/(SUM(RaiseCount)+Sum(FallCount)) From 
(SELECT Top 3 FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t) as a3,
(Select cast(Sum(RaiseCount) as real)/(SUM(RaiseCount)+Sum(FallCount)) From 
(SELECT Top 5 FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t) as a5,
(Select cast(Sum(RaiseCount) as real)/(SUM(RaiseCount)+Sum(FallCount)) From 
(SELECT Top 10 FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t) as a10,
(Select cast(Sum(RaiseCount) as real)/(SUM(RaiseCount)+Sum(FallCount)) From 
(SELECT Top 20 FallCount, RaiseCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t) as a20,
(Select cast(RaiseKCount as real)/(RaiseKCount+FallKCount) From 
(SELECT Top 1 FallKCount, RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t) as k1,
(SELECT cast(RaiseKCount as real)/(RaiseKCount+FallKCount) From 
(SELECT Top 1 FallKCount, RaiseKCount From
(SELECT Top 2 Date, FallKCount, RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as k2,
(SELECT cast(RaiseKCount as real)/(RaiseKCount+FallKCount) From 
(SELECT Top 1 FallKCount, RaiseKCount From
(SELECT Top 3 Date, FallKCount, RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as k3,
(SELECT cast(RaiseKCount as real)/(RaiseKCount+FallKCount) From 
(SELECT Top 1 FallKCount, RaiseKCount From
(SELECT Top 4 Date, FallKCount, RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as k4,
(SELECT cast(RaiseKCount as real)/(RaiseKCount+FallKCount) From 
(SELECT Top 1 FallKCount, RaiseKCount From
(SELECT Top 5 Date, FallKCount, RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as k5,
(SELECT cast(RaiseKCount as real)/(RaiseKCount+FallKCount) From 
(SELECT Top 1 FallKCount, RaiseKCount From
(SELECT Top 6 Date, FallKCount, RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) t Order by Date) t1) as k6,
(SELECT Top 1 RaiseLimitCount FROM [Statistics] Where Date <= @Date Order By Date Desc) as RaiseLimitCount,
(SELECT Top 1 FallLimitCount FROM [Statistics] Where Date <= @Date Order By Date Desc) as FallLimitCount,
(SELECT Top 1 RaiseKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) as RaiseKCount,
(SELECT Top 1 FallKCount FROM [Statistics] Where Date <= @Date Order By Date Desc) as FallKCount,
(SELECT Top 1 NoChangeCount FROM [Statistics] Where Date <= @Date Order By Date Desc) as NoChangeCount" 
CacheDuration="300" EnableCaching="True" EnableViewState="true">
</asp:SqlDataSource>