<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BillBoard_T.ascx.vb" Inherits="HomePage_BillBoard_T" %>
<%-- <script type="text/javascript">
     function tabSwitch2(active, number, tab_prefix, content_prefix) {
         for (var i = 1; i < number + 1; i++) {
             document.getElementById(content_prefix + i).style.display = 'none';
             document.getElementById(tab_prefix + i).className = '';
         }
         document.getElementById(content_prefix + active).style.display = 'block';
         document.getElementById(tab_prefix + active).className = 'active';
     } 
</script>--%>
 <div class="mg5">
        <div class="hd1">
            <ul class="tab_tip">
                <li><a href="javascript:tabSwitch(1, 2, 'tab2_', 'content2_');" id="tab2_1" class="active">漲幅</a></li>
                <li><a href="javascript:tabSwitch(2, 2, 'tab2_', 'content2_');" id="tab2_2">跌幅</a></li>
            </ul>
            <h3>台股分類 <span>CLASS</span></h3>
        </div>
        <div class="ct"><div class="ring">
            <table cellpadding="0" cellspacing="0" class="cls"><tr class="ev"><th></th><th>分類</th><th class="hl">比例</th><th></th><th>分類</th><th class="hl">比例</th></tr></table>
            <div id="content2_1">
<asp:ListView ID="lv1_1" runat="server" EnableViewState="False" GroupItemCount="2" >
    <ItemTemplate>
<td>
    <table cellpadding="0" cellspacing="0" class="cls"><tr>
        <td class="w10"><asp:Literal ID="lIndex" runat="server" Text='<%#Eval("Index")%>'/></td>
        <td style="width:25%;"><asp:HyperLink ID="hN" runat="server" Text='<%#Eval("ShortName")%>' NavigateUrl='<%# "class.aspx?id=" & Eval("ClassId") %>' ></asp:HyperLink></td>
        <td style="width:20%;"><asp:Label ID="lC" runat="server" Text='<%#Eval("ChangeDay")%>'></asp:Label></td></tr></table>
</td></ItemTemplate>
    <LayoutTemplate><table cellpadding="0" cellspacing="0" style="width:100%;" ><tr ID="groupPlaceholder" runat="server"></tr></table></LayoutTemplate>
    <GroupTemplate><tr><td ID="itemPlaceholder" runat="server"></td></tr></GroupTemplate></asp:ListView>
            </div>
            <div id="content2_2">
<asp:ListView ID="lv1_2" runat="server" EnableViewState="False" GroupItemCount="2" >
    <ItemTemplate>
<td>
    <table cellpadding="0" cellspacing="0" class="cls"><tr>
        <td class="w10"><asp:Literal ID="lIndex" runat="server" Text='<%#Eval("Index")%>'/></td>
        <td style="width:25%;"><asp:HyperLink ID="hN" runat="server" Text='<%#Eval("ShortName")%>' NavigateUrl='<%# "class.aspx?id=" & Eval("ClassId") %>' ></asp:HyperLink></td>
        <td style="width:20%;"><asp:Label ID="lC" runat="server" Text='<%#Eval("ChangeDay")%>'></asp:Label></td></tr></table>
</td></ItemTemplate>
    <LayoutTemplate><table runat="server" cellpadding="0" cellspacing="0" style="width:100%;"><tr ID="groupPlaceholder" runat="server"></tr></table></LayoutTemplate>
    <GroupTemplate><tr><td ID="itemPlaceholder" runat="server"></td></tr></GroupTemplate></asp:ListView>      
            </div></div>
        </div>
    </div>
<asp:sqldatasource id="sdsStock" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top (5) Name, StockNo, Deal, Last, [Open], Change, TotalVolume, CASE (Last) WHEN 0 THEN 0 ELSE (Change /Last) END AS percentage FROM Stock WHERE TotalVolume> 0 and StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1)  Order by percentage DESC" 
    EnableCaching="True" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter DefaultValue="0" Type="Int32" />
    </SelectParameters>
</asp:sqldatasource>
<asp:SqlDataSource ID="sdsStock3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top(5) Name, StockNo, Deal, Last, [Open], Change, TotalVolume, CASE (Last) WHEN 0 THEN 0 ELSE (Change /Last) END AS percentage FROM Stock WHERE TotalVolume> 0 and (StockNo <> '0000') AND (StockNo <> '0081') AND (StockNo <> '0080') AND (Last <> '0') AND (Last <> '') AND (Market <=1) ORDER BY percentage" 
    EnableCaching="True" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter DefaultValue="0" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sdsStock2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top(5) Name, StockNo, Deal, Last, [Open], Change, TotalVolume, CASE (Last) WHEN 0 THEN 0 ELSE (Change /Last) END AS percentage FROM Stock WHERE TotalVolume> 0 and (StockNo <> '0000') AND (StockNo <> '0081') AND (StockNo <> '0080') AND (Last <> '0') AND (Last <> '') AND (Market <=1) ORDER BY TotalVolume DESC" 
    EnableCaching="True" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter DefaultValue="0" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>