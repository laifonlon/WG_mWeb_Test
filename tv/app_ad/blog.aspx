<%@ Page Title="部落格文章 - 玩股網手機版" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="blog.aspx.vb" Inherits="blog" EnableViewState="False" %>

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
<div id="bg">
<asp:FormView ID="fv" runat="server" DataSourceID="sdsArticle" EnableViewState="False" Width="100%">
<ItemTemplate>
<div id="bgtitle">
    <h3><%# Eval("ArticleTitle")%></h3>
    <h5><span><%# Eval("NickName")%></span> 於 <%# Eval("PublishTime")%> 發布</h5>
    <p><%--<%# Eval("RecommendCount")%> 回應 | --%><%# Eval("ViewCount")%> 人瀏覽 |<%--<asp:Label ID="lblCollectCount" runat="server"></asp:Label> 收藏 | --%><%# Eval("Good")%> 人推薦<%--<asp:Label ID="lblFoucsCount" runat="server"></asp:Label>追蹤</p>--%>
</div>
<div id="bgcont" style=" font-size:1.5em;">
<asp:Label ID="lArticleText" runat="server" Text='<%# Server.HtmlDecode(Eval("ArticleText")) %>'></asp:Label>
<br />
<table cellpadding="0" cellspacing="0" style="height:25px;"><tr>
        <td><div class="fb-like" data-href="https://www.facebook.com/wantgoo.fans" data-send="false" data-layout="button_count" data-width="90" data-show-faces="false"></div></td>
        <td style=" text-align:left;font-family:Microsoft jhenghei; font-weight:bold;"><table cellpadding="0" cellspacing="0" style="padding-left:2px; line-height: 1.5em;"><tr><td>最新財經資訊都在</td><td><a  href="https://www.facebook.com/wantgoo.fans">玩股粉絲團</a></td></tr></table></td>
<%--         <td style=" text-align:left;font-family:Microsoft jhenghei; font-weight:bold; width:105px"><a style=" font-family:Microsoft jhenghei;" href="https://www.facebook.com/wantgoo.fans" rel="nofollow">玩股粉絲團</a></td>
        <td style=" text-align:left;font-family:Microsoft jhenghei; font-weight:bold;">，獲得最新財經訊息</td>--%>
    </tr></table>
</div>
<asp:Label ID="lMemberNo" runat="server" Text='<%# Eval("MemberNo")%>' Visible="False"></asp:Label>
<asp:Label ID="lArticleID" runat="server" Text='<%# Eval("ArticleID")%>' Visible="False"></asp:Label>
</ItemTemplate>
</asp:FormView>
    <div id="bgshare">
        <h3>分享這篇文章 <span>SHARE</span></h3>
         <table cellpadding="0" cellspacing="0" style="height:25px;"><%--<tr>
         <td><div class="fb-like" data-href="https://www.facebook.com/wantgoo.fans" data-send="false" data-layout="button_count" data-width="90" data-show-faces="false"></div></td>
         <td style=" text-align:right;font-family:Microsoft jhenghei; font-weight:bold;">快按讚 加入</td>
         <td style=" text-align:left;font-family:Microsoft jhenghei; font-weight:bold; width:105px"><a style=" font-family:Microsoft jhenghei;" href="https://www.facebook.com/wantgoo.fans" rel="nofollow">玩股網粉絲團</a></td>
         <td style=" text-align:left;font-family:Microsoft jhenghei; font-weight:bold;">，獲得最新財經訊息</td>
        </tr>--%>
        <tr><td style="text-align:left;"><asp:Label ID="lbllike1" runat="server" Text=""></asp:Label></td>
                 <td style="text-align:left;"><asp:Label ID="lblGPlus" runat="server" Text=""></asp:Label></td></tr></table> 
    </div>
<div id="bghot">
<h3>更多優質好文 <span>MORE</span></h3>
<ul>
<asp:gridview ID="gvArticle" runat="server" DataSourceID="sdsArticleList" 
        AutoGenerateColumns="False" ShowHeader="False" BorderWidth="0px" 
        GridLines="None" PageSize="5" CssClass="tbread" EnableViewState="False">
<Columns><asp:TemplateField><ItemTemplate>
<li><a href='blog.aspx?bid=<%# Eval("BlogID") %>'><%# Eval("ArticleTitle")%></a></li>
</ItemTemplate></asp:TemplateField></Columns></asp:gridview>
</ul>
</div></div>
<asp:SqlDataSource ID="sdsArticle" runat="server" 
ConnectionString="<%$ ConnectionStrings:WantGooConnection %>" 
SelectCommand="SELECT TOP (1) Blog.ArticleID,Blog.BlogID,Blog.IsDelete,Blog.IsLimitBuy,Blog.LimitBuyCount,Blog.TalkId,Blog.ArticleTitle, Blog.ViewCount,  convert(varchar, Blog.PublishTime, 120) as PublishTime, Blog.ArticleText, Member.NickName, Member.Situation, Blog.IsSell, Blog.IsSellShare, Blog.Price, Blog.Gold, Blog.IsPreview , Blog.Preview, Blog.PreviewWordCount,Blog.RecommendCount, Blog.Vote, Blog.Donate, Blog.Good, Blog.IsCopy, Blog.Show, BlogTagClass.TagNoSplit, Blog.Tag, Blog.MemberNo FROM Blog INNER JOIN Member ON Blog.MemberNo = Member.MemberNo INNER JOIN BlogTagClass ON Blog.Tag = BlogTagClass.BlogTagId WHERE (Blog.BlogId = @BId)" 
UpdateCommand="UPDATE [Blog] SET [ViewCount] = [ViewCount] +1 WHERE BlogID = @BlogID">
<SelectParameters>
<asp:Parameter DefaultValue="0" Name="BId" Type="String" />
</SelectParameters>
<UpdateParameters>
<asp:Parameter DefaultValue="0" Name="BlogID" Type="String" />
</UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sdsArticleList" runat="server" 
    ConnectionString="<%$ ConnectionStrings:WantGooConnection %>" 
    SelectCommand="SELECT TOP(9) Blog.BlogID, Blog.MemberNo, Blog.ArticleID, substring(Blog.ArticleTitle,1,20) as ArticleTitle , Blog.ArticleText, Blog.PublishTime, Blog.ViewCount, Blog.RecommendCount, Blog.Show, substring(Member.NickName,1,5) as NickName FROM Blog INNER JOIN Member ON Blog.MemberNo = Member.MemberNo WHERE (Blog.Show = 1) AND (Blog.IsCopy = 0) AND (Blog.IsSell = 0) AND Blog.PublishTime > DATEADD(d,-2,GETDATE())  And (Member.IshideBlog <> 1) ORDER BY Blog.ViewCount DESC" 
    CacheDuration="3600" EnableCaching="True">
</asp:SqlDataSource>
</asp:Content>

