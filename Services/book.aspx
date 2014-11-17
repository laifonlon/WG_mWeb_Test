<%@ Page Language="VB" AutoEventWireup="false" CodeFile="book.aspx.vb" Inherits="Services_book" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width; initial-scale=1.0;"/>
    <script type="text/javascript" src="/scripts/Blog.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="">
            <div style="margin-top:5px;text-align:left;"><div style="margin:0 4px; background:#B0BEC7; height:1px; overflow:hidden;"></div><div style="margin:0 2px; border:1px solid #B0BEC7; border-width:0 2px; background:#E1E7E9; height:1px; overflow:hidden;"></div><div style="margin:0 1px; border:1px solid #B0BEC7; border-width:0 1px; background:#E1E7E9; height:1px; overflow:hidden;"></div><div style="margin:0 1px; border:1px solid #B0BEC7; border-width:0 1px; background:#E1E7E9; height:1px; overflow:hidden;"></div><div style="background:#E1E7E9; border:1px solid #B0BEC7; border-width:0 1px; text-align:left;">
        <div style="background:#FFF; margin:0 2px; font-family:Verdana; padding:2px; overflow:hidden;">
<asp:FormView ID="fv" runat="server" DataSourceID="sdsArticle" EnableViewState="False" Width="100%">
<ItemTemplate>
<div id="" style="background-color:White;">
    <h3><%# Eval("ArticleTitle")%></h3>
</div>
<div id="" style="background-color:White;">
<asp:Label ID="lArticleText" runat="server" Text='<%# Server.HtmlDecode(Eval("ArticleText")) %>'></asp:Label>
<br />
</div>
<asp:Label ID="lMemberNo" runat="server" Text='<%# Eval("MemberNo")%>' Visible="False"></asp:Label>
<asp:Label ID="lArticleID" runat="server" Text='<%# Eval("ArticleID")%>' Visible="False"></asp:Label>
</ItemTemplate>
</asp:FormView>
</div></div><div style="margin:0 1px; border:1px solid #B0BEC7; border-width:0 1px; background:#E1E7E9; height:1px; overflow:hidden;"></div><div style="margin:0 1px; border:1px solid #B0BEC7; border-width:0 2px; background:#E1E7E9; height:1px; overflow:hidden;"></div><div style="margin:0 2px; border:1px solid #B0BEC7; border-width:0 2px; background:#E1E7E9; height:1px; overflow:hidden;"></div><div style="margin:0 4px; background:#B0BEC7; height:1px; overflow:hidden;"></div></div>
</div>
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
    </div>
    </form>
</body>
</html>
