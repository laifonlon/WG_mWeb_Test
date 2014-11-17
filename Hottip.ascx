<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Hottip.ascx.vb" Inherits="HomePage_Hottip" EnableViewState="False" %> 
<script type="text/javascript">
    function tabSwitch(active, number, tab_prefix, content_prefix) {
        for (var i = 1; i < number + 1; i++) {
            document.getElementById(content_prefix + i).style.display = 'none';
            document.getElementById(tab_prefix + i).className = '';
        }
        document.getElementById(content_prefix + active).style.display = 'block';
        document.getElementById(tab_prefix + active).className = 'active';
    }
</script>
<div class="mg5">
    <div class="hd1">
        <ul class="tab_tip">
            <li><a href="javascript:tabSwitch(1, 2, 'tab_', 'content_');" id="tab_1" class="active">多方</a></li>
            <li><a href="javascript:tabSwitch(2, 2, 'tab_', 'content_');" id="tab_2">空方</a></li>
        </ul>   
        <h3>俏秘書選股 <span>SELECTION</span><div style="float:right; padding:2px 5px;"><asp:Label ID="lblDate" runat="server"></asp:Label></div> </h3>
    </div>
    <div class="ct">
        <div class="ring" id="content_1">
            <table class="oul"><asp:Literal ID="lblContent" runat="server"/></table>  
        </div>
        <div class="ring" id="content_2">
            <table class="oul"><asp:Literal ID="lblContent2" runat="server"/></table> 
        </div>         
    </div>
</div>
<asp:SqlDataSource ID="sdsNewestDate" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT MAX(Date) AS LastDate FROM Selection1" 
    CacheDuration="3600" EnableCaching="True">
</asp:SqlDataSource>
<asp:SqlDataSource ID="sdsLastDate" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 Order by Date Desc) t Order by Date" 
    CacheDuration="3600" EnableCaching="True">
</asp:SqlDataSource>