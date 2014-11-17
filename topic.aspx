<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="topic.aspx.vb" Inherits="topic" %>
<%@ Register src="Footer.ascx" tagname="Footer" tagprefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">


<div id="div_choice" style="display:none;" class="fixed-ad">
<div id="CPH_pnlShoc">
<div class="org_box">
<span class="org_bot_cor"></span>
<span style="color:white;">按下<img width="20px;" src="/image/iosfa.png" />把玩股網加入主畫面螢幕，一鍵看最新指數！</span>
</div>
</div>
</div>



<div id="gs">

<div class="wgclub">
<div class="hd-tit"><h3><asp:Literal ID="Literal_title" runat="server"></asp:Literal></h3></div>

<div class="pgs-bx">
    <asp:Literal ID="Literal_pagelink" runat="server"></asp:Literal>
</div><!-- pgs-bx:end -->

<ul class="cb-list">
<asp:Literal ID="Literal_main" runat="server"></asp:Literal>
</ul>

<div class="pgs-bx">
<asp:Literal ID="Literal_pagelink2" runat="server"></asp:Literal>
</div><!-- pgs-bx:end -->
</div><!-- wgclub:end -->

</div><!-- gs:end -->


<uc8:Footer ID="Footer1" runat="server" EnableViewState="False" />

<script type="text/javascript">

    function changepage(cid, pid) {
        location.href = '/topic.aspx?id=' + cid + '&p=' + pid;

    }
</script>

</asp:Content>

