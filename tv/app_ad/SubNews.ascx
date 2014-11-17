<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubNews.ascx.vb" Inherits="SubNews" %>
<div class="mg5">
    <div class="hd1">
        <%--<div style="float:right;"><asp:Label ID="lblStockNews" runat="server"></asp:Label></div>--%>
        <h3>即時新聞 <span>NEWS</span></h3></div>
    <div class="nws">
    <asp:Label ID="lblnews_note" runat="server" Visible="false"></asp:Label>

        <div>
            <ul>
                <asp:ListView ID="lv" runat="server" DataSourceID="sdsNews" EnableViewState="False" >
                <ItemTemplate>
                    <asp:label id="lblEffect" runat="server" text='<%# eval("Effect") %>' Visible="False"></asp:label>
                    <asp:Literal ID="lttitle" runat="server" Visible="false" text='<%# eval("Title") %>'></asp:Literal>
                    <asp:Literal ID="lttext" runat="server" Visible="false" text='<%# eval("Text") %>'></asp:Literal>
              
                    <li>
                        <%--<a class="red" href='http://www.wantgoo.com/stock/newsdetail.aspx?no=<%# eval("StockNo") %>&id=<%# eval("NewsId") %>' ><asp:literal id="lbTitle" runat="server" text='<%# eval("title") %>'></asp:literal></a>--%>
                        <a class="red" href="<%# Eval("Link") %>"><asp:label id="lblTitle" runat="server" ForeColor="#3B5998"></asp:label>
                        <p>(<asp:Literal id="lbltime" runat="server" text='<%# eval("Time") %>'></asp:Literal>)
                        <span><asp:Literal id="lblText" runat="server"></asp:Literal></span></p></a>
                    </li>
                </ItemTemplate>
                </asp:ListView>
            </ul>
        </div></div>
</div>

<asp:SqlDataSource ID="sdsNews" runat="server"
connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
selectcommand="SELECT TOP(10) News.Time , News.Title, News.Text , News.Link, News.Effect,StockNews.NewsId,StockNews.StockNo FROM News INNER JOIN StockNews ON News.NewsId = StockNews.NewsId WHERE (StockNews.StockNo = @StockNo) ORDER BY News.Time DESC" EnableCaching="True" CacheDuration="900" EnableViewState="False">
<SelectParameters>
<asp:Parameter Name="StockNo" DefaultValue="0000" />
</SelectParameters>
</asp:SqlDataSource>