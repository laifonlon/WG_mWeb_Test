<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MainIndex.ascx.vb" Inherits="Stock_MainIndex" %>
<div class="mg5"><div class="hd1">
<ul class="tab_tip">
    <li><a href="javascript:tabSwitch(1, 3, 'tabS_stat_', 'ctstat_');" id="tabS_stat_1" class="active">一週</a></li>
    <li><a href="javascript:tabSwitch(2, 3, 'tabS_stat_', 'ctstat_');" id="tabS_stat_2">一月</a></li>
    <li><a href="javascript:tabSwitch(3, 3, 'tabS_stat_', 'ctstat_');" id="tabS_stat_3">一季</a></li>
</ul>
<h3>績效統計 <span>STAT</span></h3>
</div><div class="ct">
<div class="ring">
    <div id='ctstat_1' style="display:block;"><asp:Literal ID="lMain1" runat="server"></asp:Literal></div>
    <div id='ctstat_2' style="display:none;"><asp:Literal ID="lMain2" runat="server"></asp:Literal></div>
    <div id='ctstat_3' style="display:none;"><asp:Literal ID="lMain3" runat="server"></asp:Literal></div>
</div>
</div></div>
<%--<script type="text/javascript">
    function tabSwitch(active, number, tab_prefix, content_prefix) {
        for (var i = 1; i < number + 1; i++) {
            document.getElementById(content_prefix + i).style.display = 'none';
            document.getElementById(tab_prefix + i).className = '';
        }
        document.getElementById(content_prefix + active).style.display = 'block';
        document.getElementById(tab_prefix + active).className = 'active';
    }
</script>--%>