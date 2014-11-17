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
                    <%--<asp:label id="lblEffect" runat="server" text='<%# eval("Effect") %>' Visible="False"></asp:label>--%>
                    <asp:Literal ID="lttitle" runat="server" Visible="false" text='<%# Eval("Headline")%>'></asp:Literal>
                    <%--<asp:Literal ID="lttext" runat="server" Visible="false" text='<%# eval("Summary") %>'></asp:Literal>--%>
              
                    <li>                        
                        <a class="red" href="http://www.wantgoo.com/news/detail.aspx?id=<%# eval("ID") %>&c=%E7%B8%BD%E8%A6%BD"><asp:label id="lblTitle" runat="server" ForeColor="#3B5998"></asp:label>
                        <p>(<asp:Literal id="lbltime" runat="server" text='<%# eval("DateTime") %>'></asp:Literal>)
                        <span><asp:Literal id="lblText" runat="server"></asp:Literal></span></p></a>
                    </li>
                </ItemTemplate>
                </asp:ListView>
            </ul>
        </div></div>
</div>

<asp:SqlDataSource ID="sdsNews" runat="server"
connectionstring="<%$ ConnectionStrings:NowNewsConnection %>" 
selectcommand="exec GetStockNewsList @StockNo;" EnableCaching="True" CacheDuration="300" EnableViewState="False">
<SelectParameters>
<asp:Parameter Name="StockNo" DefaultValue="0000" />
</SelectParameters>
</asp:SqlDataSource>