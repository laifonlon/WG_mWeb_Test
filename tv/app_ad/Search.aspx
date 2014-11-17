<%@ Page Title="股票搜尋 - 玩股網手機版" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="Search.aspx.vb" Inherits="Search" %>
<%@ Register src="footer.ascx" tagname="footer" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">

<script type="text/javascript" src='Scripts/Autocomplete/jquery.autocomplete.js'></script>
<script type="text/javascript" src='Scripts/Autocomplete/jquery.autocomplete.pack.js'></script>
<link rel="stylesheet" type="text/css" href='Scripts/Autocomplete/jquery.autocomplete.css'/>  
<script type="text/javascript" src='Scripts/Autocomplete/TWStocks.js'></script>
<script type="text/javascript">
    var stocks = GetStocks();
    $(function () {
        $("#<%=tbxKeyWord.ClientID %>").autocomplete(stocks,
        {
            max: 20,
            delay: 10,
            minChars: 1,
            width: 130,
            matchContains: true,
            autoFill: false,
            formatItem: function (row) {
                return "<div style='height:12px;float:left;padding-left:5px;'>" + row.No + "&nbsp;&nbsp;" + row.Name + "</div>";
            },
            formatMatch: function (row) {
                return row.No + row.Name;
            },
            formatResult: function (row) {
                return row.No;
            }
        })
        $("#<%=tbxKeyWord.ClientID %>").keyup(function (event) {
            if (event.keyCode == '13') {
                __doPostBack('<%=btnsearch.UniqueID%>', '');
            }
        });
    });
</script>
<style type="text/css">

/*sexy button style*/
.sexybutton {display: inline-block;vertical-align: middle;margin: 0;padding: 0;	font: normal 13px "microsoft jhenghei" !important;	text-decoration: none !important;
	text-shadow: 1px 1px 2px rgba(0,0,0,0.20);background: none;border: none;white-space: nowrap;cursor: pointer;user-select: none;-moz-user-select: none;
	/* Fix extra width padding in IE */
	_width: 0;overflow: visible;}
.sexybutton[disabled],.sexybutton[disabled]:hover,.sexybutton[disabled]:focus,.sexybutton[disabled]:active,.sexybutton.disabled,.sexybutton.disabled:hover,
.sexybutton.disabled:focus,.sexybutton.disabled:active {color: #333 !important;cursor: inherit;text-shadow: none; opacity: 0.33;}
.sexybutton img {margin-right: 2px;	vertical-align: middle;	/* IE6 Hack */
	_margin-top: 0px;_vertical-align: text-bottom;
	/* IE6 still requires a PNG transparency fix */ 
	/* _display: none;		Or just hide icons from the undeserving IE6 */}
.sexybutton img.after {	margin-right: 0;margin-left: 5px;
	/* IE6 still requires a PNG transparency fix */ 
	/* _margin-left: 0;		Or just hide icons from the undeserving IE6 */}
.sexybutton,.sexybutton.sexysilver {color: #666 !important;}
.sexybutton:hover,.sexybutton:focus,.sexybutton.sexysilver:hover,.sexybutton.sexysilver:focus {	color: #333 !important;}
.sexybutton.sexysimple { position: relative; padding: 3px 6px;font: inherit;font-size: 17px !important;	font-style: normal !important; font-weight: bold !important; 
	color: #fff !important;	line-height: 1; background-image: url(/image/simple/awesome-overlay-sprite.png);background-repeat: repeat-x;background-position: 0 0;margin-top:0px;
	/* Special effects */
	text-shadow: 0 -1px 1px rgba(0,0,0,0.25), -2px 0 1px rgba(0,0,0,0.25); 	border-radius: 2px; -moz-border-radius: 2px; -webkit-border-radius: 2px; -moz-box-shadow: 0 1px 2px rgba(0,0,0,0.5); -webkit-box-shadow: 0 1px 2px rgba(0,0,0,0.5);
	/* IE only stuff */
	border-bottom: 1px solid transparent\9;	_background-image: none;
	/* Cross browser inline block hack - http://blog.mozilla.com/webdev/2009/02/20/cross-browser-inline-block/ */
	display: -moz-inline-stack;	display: inline-block;	vertical-align: middle;	*display: inline !important;position: relative;	
	/* Force hasLayout in IE */
	zoom: 1;
	/* Disable text selection (Firefox only)*/
	-moz-user-select: none;}
.sexybutton.sexysimple::selection {	background: transparent;}
.sexybutton.sexysimple:hover,.sexybutton.sexysimple:focus {	background-position: 0 -50px; color: #fff !important;}
.sexybutton.sexysimple:active { background-position: 0 -100px; -moz-box-shadow: inset 0 1px 2px rgba(0,0,0,0.7); /* Unfortunately, Safari doesn't support inset yet */
	-webkit-box-shadow: none;
	/* IE only stuff */
	border-bottom: 0\9;	border-top: 1px solid #666\9;}
.sexybutton.sexysimple[disabled], .sexybutton.sexysimple.disabled { background-position: 0 -150px; color: #333 !important;text-shadow: none;}
.sexybutton.sexysimple[disabled]:hover,.sexybutton.sexysimple[disabled]:focus,.sexybutton.sexysimple[disabled]:active,.sexybutton.sexysimple.disabled:hover,
.sexybutton.sexysimple.disabled:focus,.sexybutton.sexysimple.disabled:active {-moz-box-shadow: 0 1px 2px rgba(0,0,0,0.5); -webkit-box-shadow: 0 1px 2px rgba(0,0,0,0.5);} 
/* Simple button colors */
.sexybutton.sexysimple.sexymagenta	{ background-color: #a9014b; }
.sexybutton.sexysimple.sexyred		{ background-color: #A90118;}
</style>

<div id="gs">
<div class="mg5"><div style="background-color:#fff;">
    <div class="hd1"><h3>股票搜尋 <span>SEARCH</span></h3></div>

    <div class="ct"><br />
<div id="search" style=" font-size:1.5em;">
<div class="title"><span class="header"></span></div>
<asp:Panel ID="p" runat="server" DefaultButton="btnsearch">
<table cellpadding="0" cellspacing="0"><tr><td style="padding-left:5px;"><asp:textbox id="tbxKeyWord" runat="server" text="輸入股票代碼/名稱" ForeColor="#999999"  Height="30px"
       onfocus="if (this.value == '輸入股票代碼/名稱') this.value = ''" onblur="if(this.value=='')this.value='輸入股票代碼/名稱'" Width="220px" ></asp:textbox></td>
        <td style="padding-left:3px;"><asp:LinkButton ID="btnsearch" runat="server" CssClass="sexybutton sexysimple sexyred">搜尋</asp:LinkButton></td></tr></table>
</asp:Panel><br />
<p>熱門搜尋: <asp:Label ID="lblresult" runat="server"></asp:Label></p>
</div>  
    </div></div></div></div>
 
<%--<asp:Button ID="btnSearch" runat="server" Text="搜尋" />--%>
<asp:SqlDataSource ID="sdsSearchStockNo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="SELECT StockNo FROM Stock WHERE (StockNo = @KeyWord1) OR ((Name LIKE @KeyWord2) And Not (Name is Null))">
    <SelectParameters>
        <asp:Parameter Name="KeyWord1" DefaultValue="-1" />
        <asp:Parameter Name="KeyWord2" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:sqldatasource id="sdsStocksearch" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top (7) Name, StockNo, Deal, Last, [Open], Change, TotalVolume FROM Stock WHERE StockNo <> '0000' AND StockNo <> '0081' AND StockNo <> '0080' AND (Market <=1) AND ((change/last)>0.02) Order by [Deal] Desc" 
    EnableCaching="True" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter DefaultValue="0" Type="Int32" />
    </SelectParameters>
</asp:sqldatasource>

<uc1:footer ID="footer1" runat="server" />
</asp:Content>