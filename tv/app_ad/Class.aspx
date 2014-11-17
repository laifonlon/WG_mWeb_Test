<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="Class.aspx.vb" Inherits="_Class" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">

<div class="mg5"><div style="background-color:#fff;">
    <div class="hd1"><h3><asp:Literal ID="lTitle" runat="server"></asp:Literal></h3></div>
    <div class="ct"><div class="ring">
<asp:GridView ID="gv1" runat="server" AutoGenerateColumns="False" CssClass="oul" GridLines="None" DataSourceID="sdsCollect" BorderStyle="None" RowStyle-HorizontalAlign="Center" Width="100%" EnableViewState="false" >
<Columns>
    <asp:templatefield headertext="代號" SortExpression="StockNo" Visible="False">
        <itemtemplate><asp:Label ID="lblNumber" runat="server" text='<%# Eval("StockNo") %>'></asp:Label></itemtemplate></asp:templatefield>
    <asp:templatefield headertext="股票" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%" HeaderStyle-BackColor="#CCA3A3">
        <itemtemplate><asp:HyperLink ID="hyStock" runat="server"  NavigateUrl='<%# "/stock.aspx?no=" + Eval("StockNo").Replace("&","$") %>' text='<%# Eval("Name") %>'></asp:HyperLink>
        </itemtemplate></asp:templatefield>
    <asp:templatefield headertext="股價" HeaderStyle-HorizontalAlign="Center" SortExpression="Deal" ItemStyle-Width="20%" HeaderStyle-BackColor="#CC7A7A"><itemtemplate><asp:Label ID="lblPrice" runat="server" text='<%# Format(Eval("Deal"),"0.00") %>'></asp:Label></itemtemplate></asp:templatefield>
    <asp:templatefield headertext="漲跌" SortExpression="Change" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%" HeaderStyle-BackColor="#CCA3A3">
        <itemtemplate><asp:Label ID="lblDiffence" runat="server" text='<%# Format(Eval("Change"),"0.00") %>'></asp:Label></itemtemplate></asp:templatefield>
    <asp:templatefield headertext="%" SortExpression="Ratio" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%" HeaderStyle-BackColor="#CC7A7A"><itemtemplate><asp:Label ID="lblPercent" runat="server"  text='<%# Eval("Ratio") %>'></asp:Label></itemtemplate> 
                    </asp:templatefield>
    <asp:templatefield headertext="成交量" SortExpression="TotalVolume" ItemStyle-Width="20%" HeaderStyle-BackColor="#CCA3A3"><itemtemplate><asp:Label ID="lblVolume" runat="server" text='<%# Format(Eval("TotalVolume")/1000,"#,###,###,##0") %>' ToolTip='<%# Format(Eval("TotalVolume"),"#,###,###,##0 股") %>'></asp:Label></itemtemplate>
                    </asp:templatefield>
    <asp:templatefield headertext="昨收" Visible="False"><itemtemplate><asp:Label ID="lblLast" runat="server" text='<%# Format(Eval("Deal")-Eval("Change"),"0.00") %>'></asp:Label></itemtemplate>
                    </asp:templatefield>
            </Columns></asp:GridView>
    </div></div></div></div>

<asp:SqlDataSource ID="sdsClass" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="SELECT [Name] FROM [Class] WHERE ([ClassId] = @ClassId)">
    <SelectParameters><asp:Parameter DefaultValue="0" Name="ClassId" Type="Int32" /></SelectParameters></asp:SqlDataSource>

<asp:sqldatasource id="sdsCollect" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Stock.Name, Stock.StockNo, Stock.Deal, Stock.Last, Stock.[Open], Stock.Buy, Stock.Sell, Stock.High, Stock.Low, Stock.Change, Stock.SingleVolume, Stock.TotalVolume, Stock.NewHighLow, Stock.NewHighLowDate, Stock.NoNewHighLowDates, case Stock.Deal when 0 then 0 else (ROUND((Stock.Change / (Stock.Deal-Stock.Change)), 4) *100 )end as Ratio FROM Stock INNER JOIN StockClass ON Stock.StockNo = StockClass.StockNo WHERE (StockClass.ClassId = @ClassId) ORDER BY Stock.StockNo" 
    InsertCommand="IF Not Exists (Select * From Collection Where MemberNo = @MemberNo And StockNo = @StockNo)
    BEGIN
        Update Collection Set [Order] = [Order] +1 Where MemberNo = @MemberNo 
        INSERT INTO Collection(MemberNo, StockNo) VALUES (@MemberNo,@StockNo)
    END" >
    <SelectParameters>
        <asp:Parameter DefaultValue="0" Name="ClassId" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="MemberNo" />
        <asp:Parameter Name="StockNo" />
    </InsertParameters>
</asp:sqldatasource>
    
</asp:Content>

