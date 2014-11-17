<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="Club.aspx.vb" Inherits="ClubM" %>
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
    <asp:Literal ID="Literal_ClubHome" runat="server"></asp:Literal>
<div class="wgclub">
<ul class="wgclub-list">




</ul>
</div><!-- wgclub:end -->


</div><!-- gs:end -->




<uc8:Footer ID="Footer1" runat="server" EnableViewState="False" />




</asp:Content>


<%--主內容區 加載到 Footer--%>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer_ContentPlaceHolder" runat="Server" EnableViewState="False">
</asp:Content>