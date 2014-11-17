<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HottipSelectTest.aspx.vb" Inherits="Hottip_HottipSelectTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">


        

<link rel="stylesheet" type="text/css" href='../styles/HottipSelect.css'/> 
 <script type="text/javascript">
     $(function () {
         $("img[title]").tooltip({ offset: [10, 2], effect: 'slide' });
         $("span[title]").tooltip({ offset: [10, 2], effect: "slide" });
         $("a[title]").tooltip({ offset: [10, 2], tipClass: 'csstip', effect: "slide" });
     });
</script>
<span id="AD" runat="server" visible="false" enableviewstate ="false">
<script type="text/javascript"><!--
    google_ad_client = "pub-6240822095534788";
    google_ad_host = "pub-7475939466736018";
    google_ad_width = 468;
    google_ad_height = 60;
    google_ad_format = "468x60_as";
    google_ad_type = "text_image";
    google_ad_channel = "3585104308";
    google_ad_host_channel = "5906667350";
    google_color_border = "FFFFFF";
    google_color_bg = "FFFFFF";
    google_color_link = "FFFFFF";
    google_color_url = "FFFFFF";
    google_color_text = "FFFFFF";
    //--></script>
<script type="text/javascript"
  src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>
</span>
<div class="backfringe" style="padding:5px; width:970px; margin:5px auto 0px; background-color:#fff; text-align:left; font-size:15px; font-weight:bold;">
<img alt="itemicon" src="/image/icons/item.png" width="6px" height="8px" /> 盤後寄送俏秘書智慧選股：
<iframe id="ifr" runat="server" src="/Hottip/HottipMailSetting.aspx" scrolling="no" height="17" width="200px" marginwidth="0" marginheight="0" frameborder="0"></iframe>
</div>
<div style="background-color: #FFE0E1; border: 1px solid #d92b34;border-radius: 5px;text-align: left; width: 980px; margin:7px 0 0px 10px; float:left; font-size:13px;">
<div style="float:left;background-color:#da464b; line-height:26px"><h2 style="padding:5px; color:#fff; font-family:Microsoft jhenghei; width:60px; font-size:14px; ">選股設定</h2></div>
<div style="float:left;background-color:#FFE0E1;">
<div style="cursor:pointer; padding:4px 0px 3px 10px;">
<%--<a id="aTab0" runat="server" href="/hottipselect.aspx?m=0" class="sexybutton sexysimple sexyred">俏秘書選股</a>
<a id="aTab1" runat="server" href="/hottipselect.aspx?m=1" class="sexybutton sexysimple sexyred">熱門選股</a>
<a id="aTab2" runat="server" href="/hottipmyselect.aspx?m=2" class="sexybutton sexysimple sexyred">我的飆股</a>--%>

<span style=" font-weight:bold;font-size:13px;padding-left:5px">日期 : </span>
<asp:TextBox ID="tbxDate" runat="server" Font-Size="13px" Width="120px" />
<asp:Button ID="btnQuery" runat="server" Text="重新查詢" Font-Size="14px" Width="80px" Height="25px" />

<span style="padding-left:10px">(<a target="_blank" href ="/mall/richinfo.aspx">好野會員專屬功能</a>，
<asp:Literal ID="lNote" runat="server" Text="盤中9:30至13:30，每5分鐘即時更新。"></asp:Literal>) </span>
</div></div></div>

<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
ErrorMessage="請輸入正確的日期" ValidationExpression="[0-9]{4}[/]{1}[0-9](2)[/]{1}[0-9](2)" 
ControlToValidate="tbxDate" Display="Dynamic" ValidationGroup="Date">請輸入正確的日期，請不要包含空白字元。</asp:RegularExpressionValidator>
<%--俏秘書選股--%>
<div id="tab0" style="display:none" runat="server">
<div class="backfringe topic_intro" style="clear:left;">
<div class="outline"><h1>多方強勢股</h1></div>
<div class="cont"><dl><asp:literal ID="Buy" runat="server"></asp:literal></dl></div></div>
<div class="backfringe topic_g" style="clear:left;">
<div class="outline"><h1>空方強勢股</h1></div>
<div class="cont"><dl><asp:literal ID="Sell" runat="server"></asp:literal></dl></div></div></div>
<%--熱門選股--%>
<div id="tab1" style="display:none" runat="server">
<div class="backfringe topic_intro" style="clear:left;">
<div class="outline"><h1>多方強勢股</h1></div>
<div class="cont"><dl><asp:literal ID="Buy1" runat="server"></asp:literal></dl></div></div>
<div class="backfringe topic_g" style="clear:left;">
<div class="outline"><h1>空方強勢股</h1></div>
<div class="cont"><dl><asp:literal ID="Sell1" runat="server"></asp:literal></dl></div></div></div>
<%--我的飆股--%>
<div id="tab2" style="display:none" runat="server">
<div class="backfringe topic_intro" style="clear:left;">
<div class="outline"><h1>選股條件</h1></div>
<div class="cont"><dl><asp:literal ID="Buy2" runat="server"></asp:literal></dl></div></div></div>
<asp:SqlDataSource ID="sdsNewestDate" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT MAX(Date) AS LastDate FROM Selection1" 
    CacheDuration="3600" EnableCaching="True">
</asp:SqlDataSource>
<asp:SqlDataSource ID="sdsLastDate" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 Order by Date Desc) t Order by Date" 
    CacheDuration="3600" EnableCaching="True">
</asp:SqlDataSource>
<link rel="stylesheet" href="http://code.jquery.com/ui/1.8.18/themes/base/jquery-ui.css" type="text/css" media="all" />
<script src="http://code.jquery.com/ui/1.8.18/jquery-ui.min.js" type="text/javascript"></script>
<script src="http://jquery-ui.googlecode.com/svn/tags/latest/ui/minified/i18n/jquery-ui-i18n.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#<%= Me.tbxDate.ClientID %>").datepicker();
    });
</script>



    </form>
</body>
</html>
