<%@ Page Title="理財課程 - 玩股網手機版" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="LessonInfo.aspx.vb" Inherits="LessonInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
<style type="text/css"> .name{color:#7D7D7D; padding-top:5px;}</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<div id="gs">
<div style="background-color:#fff; margin-top:5px;line-height: 1.5em;padding: 0px 3px;">
 
<div class="name">課程名稱：</div>
<div style="font-weight:bold; color:#eb6100;"><asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal></div>
<div class="name">活動日期：</div>
<div><asp:Literal ID="lblActionTime" runat="server" Text="ActionTime"></asp:Literal></div>
<div class="name">活動地點：</div>
<div style="font-weight:bold; color:#eb6100; font-size:15px;"><asp:Literal ID="lblAddressName" runat="server" Text="AddressName"></asp:Literal></div>
<div class="name">費用：<span style="color:Black;"><asp:Literal ID="lblPrice" runat="server"></asp:Literal>
<asp:HyperLink ID="hlPrice" runat="server"></asp:HyperLink></span></div>
<div class="name">講師：<span style="font-weight:bold; color:#3d3d3d;"><asp:Literal ID="lblTeacher" runat="server" Text="Teacher"></asp:Literal></span></div>
<asp:Panel ID="pnlBook" runat="server">
    <div class="name"><asp:Literal ID="lbBook" runat="server" Text="著作："></asp:Literal></div>
<asp:Literal ID="lblTeacherBook" runat="server" Text=""></asp:Literal>
</asp:Panel>
<div class="name">簡介：</div>
<div><asp:Literal ID="lblTeacherNote" runat="server" Text=""></asp:Literal></div>
<div class="name">活動大綱：</div>
<div><ul style="letter-spacing:1px; color:#000; font-size:15px; list-style-type:none;">
    <asp:Literal ID="lblActionList" runat="server" Text="ActionList"></asp:Literal>
</ul></div>
<div class="name">活動地點：</div>
<div style="font-weight:bold; color:#eb6100;font-size:15px;">
    <asp:Literal ID="lblMapAddress" runat="server" Text="AddressName"></asp:Literal>
    <br /><asp:Literal ID="lblMap" runat="server" Text="Map"></asp:Literal>
</div>
<div class="name">交通：</div>
<div><asp:Literal ID="lblTraffic" runat="server" Text="Traffic"></asp:Literal></div>
<div class="name">備註：</div>
<div><asp:Literal ID="lblNote" runat="server" Text="Note"></asp:Literal></div>

<div style="text-decoration:none; padding:20px 0px;">
    <asp:LinkButton ID="btnJoinTop" runat="server">點我到玩股網報名付費課程</asp:LinkButton>
</div>
</div></div>
 
</asp:Content>

