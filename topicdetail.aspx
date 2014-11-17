<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="topicdetail.aspx.vb" Inherits="topicdetail" %>

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
<div class="hd-tit"><h3><asp:Literal ID="Literal_ClubName" runat="server"></asp:Literal></h3></div>

<div class="pgs-bx">
    <asp:Literal ID="Literal_pagelink" runat="server"></asp:Literal>
</div><!-- pgs-bx:end -->

<h1 class="hdtit-art"><asp:Literal ID="LiteralTopicTitle" runat="server"></asp:Literal></h1>
<ul class="art-list">
    <asp:Literal ID="LiteralTopicContent" runat="server"></asp:Literal>
</ul>

<div class="pgs-bx">
<asp:Literal ID="Literal_pagelink2" runat="server"></asp:Literal>
</div><!-- pgs-bx:end -->

<div class="reply">
<dl class="reply-fb">
<dt>回覆留言</dt>
<dd><asp:TextBox ID="msgTxt" runat="server" MaxLength="20" TextMode="MultiLine"></asp:TextBox></dd>
<dd><asp:Button ID="btnPost" runat="server" Text="送出"  CssClass="btn-rpy" /></dd>
</dl>

</div><!-- reply:end -->

</div><!-- wgclub:end -->

</div><!-- gs:end -->


<script type="text/javascript">

    function changepage(cid,tid,pid) {
        location.href = '/topicdetail.aspx?tid=' + tid + '&id=' + cid + '&p=' + pid;
    }
    $(function () { $(".art-inner img").click(function () { var $this = $(this); var url = $this.attr("src"); window.open(url); }); });
</script>
</asp:Content>

