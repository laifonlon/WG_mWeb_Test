<%@ Page Title="部落格文章 - 玩股網手機版" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="Article.aspx.vb" Inherits="blog" EnableViewState="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<%--<script type="text/javascript">
    function ReImgWid(img) {
        var width = img.width;
        var scale = 300 / width;
        var height = img.height;
        if (width > 300) {
            img.width = 300;
            img.height = height * scale;
        };
    }
    </script>
<script>    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/zh_TW/all.js#xfbml=1&appId=288694587893836";
        fjs.parentNode.insertBefore(js, fjs);
    } (document, 'script', 'facebook-jssdk'));</script>--%>
<div id="fb-root"></div>
<script type="text/javascript" src="scripts/Blog.min.js"></script>
<script type="text/javascript" src="http://apis.google.com/js/plusone.js">    { lang: 'zh-TW' }</script>
<div id="gs">
<div class="mg5">
<div class="hd1">
    <h3>玩股網 精選財經文章 <span>SPECIAL</span></h3> 
</div>
<div class="ct" style="padding-top:0px;">
<asp:ListView ID="lvBlog" runat="server" DataKeyNames="BlogId" DataSourceID="sdsBlog" EnableViewState="False" GroupItemCount="1">
<ItemTemplate>
    <td class="list"><asp:Panel ID="Panel1" runat="server" Visible="False">
        <div class="postviewerdatasection">
        <asp:Label ID="lblBlogId" runat="server" Text='<%# Eval("BlogId") %>'></asp:Label>
        <asp:Label ID="lblArticleId" runat="server" Text='<%# Eval("ArticleId") %>'></asp:Label>
        <asp:Label ID="lblTopPic" runat="server" Text='<%# Eval("TopPic") %>'></asp:Label>
        <asp:Label ID="lblMemberNo" runat="server" Text='<%# Eval("MemberNo") %>'></asp:Label>
        <asp:Label ID="lblTagId" runat="server" Text='<%# Eval("BlogTagId") %>'></asp:Label>
        <asp:Label ID="lblTagName" runat="server" Text='<%# Eval("TagNoSplit") %>'></asp:Label>
        <asp:Label ID="lblPublishTime" runat="server" Text='<%# Eval("PublishTime") %>'></asp:Label>
        <asp:Label ID="lblIsSell" runat="server" Text='<%# Eval("IsSell") %>'></asp:Label>
        <asp:Label ID="lblIsShareSell" runat="server" Text='<%# Eval("isSellShare") %>'></asp:Label>
        <asp:Label ID="lblPreviewWordCount" runat="server" Text='<%# Eval("PreviewWordCount") %>'></asp:Label>
        <asp:Label ID="lblGood" runat="server" Text='<%# Eval("Good") %>'></asp:Label>
        <asp:Label ID="lblArticleTitle" runat="server" Text='<%# Eval("ArticleTitle") %>'></asp:Label>
        </div></asp:Panel>
    <div><table cellpadding="0" cellspacing="0" class="tbmain">
    <tr>
    <td class="pic" style=" padding:0px 5px 5px 0px"><a href='blog.aspx?bid=<%# Eval("BlogID") %>'><asp:literal ID="lblImg" runat="server"/></a></td>
    <td class="content" style="vertical-align:top; padding-left:5px;">
        <a class="pvmain" href='blog.aspx?bid=<%# Eval("BlogID") %> %>'>
        <div class="pvmain"><h3 style="font-size:1.6em;"><a class="Atitle" href='blog.aspx?bid=<%# Eval("BlogID") %>'><asp:Literal ID="lblArticleTitleText" runat="server"/></a></h3>
            <p style="font-size:1.2em; line-height:1.2em;"><asp:Literal ID="lblText" runat="server"/></p>
        </div></a>
    </td></tr></table></div>
    </td>
</ItemTemplate>
<LayoutTemplate>
<table cellpadding="0" cellspacing="0" class="tbpostview">
<tr ID="groupPlaceholder" runat="server"></tr>
</table>  
</LayoutTemplate>
<GroupTemplate>
<tr><td ID="itemPlaceholder" runat="server"></td></tr>
</GroupTemplate>
</asp:ListView>
</div></div></div>

<asp:SqlDataSource ID="sdsBlog" runat="server" ConnectionString="<%$ ConnectionStrings:WantGooConnection %>" SelectCommand="SELECT Top(25) BlogId,Good,Blog.Tag,BlogTagClass.BlogTagId,BlogTagClass.TagNoSplit,MemberNo,TopPic,ArticleTitle,Substring(Blog.ArticleText,1,2500) as ArticleText,Preview,OutlineText,PublishTime,IsSell,IsSellShare,PreviewWordCount, ArticleID FROM Blog INNER JOIN BlogTagClass ON Blog.Tag = BlogTagClass.BlogTagId WHERE (IsCopy = 0) AND (Show = 1) AND (IsTopIndex > 0) AND (IsDelete = 0 or IsDelete is NuLL) ORDER BY IsTopIndex DESC,PublishTime DESC"  CacheDuration="600" EnableCaching="True" >
<SelectParameters>
    <asp:Parameter Name="Tag" DefaultValue="0" Type="Int32" />
</SelectParameters>
</asp:SqlDataSource>

</asp:Content>

