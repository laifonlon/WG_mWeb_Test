<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<%@ Register Src="Footer.ascx" TagName="Footer" TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" runat="Server">
    <div id="div_choice" style="display: none;" class="fixed-ad">
        <asp:Panel ID="pnlShoc" runat="server" EnableViewState="False">
            <div class="org_box">
                <span class="org_bot_cor"></span><span style="color: white;">按下<img width="20px;"
                    src="/image/iosfa.png" />把玩股網加入主畫面螢幕，一鍵看最新指數！</span>
            </div>
        </asp:Panel>
    </div>

    <asp:Login ID="Login1" runat="server" RememberMeSet="True" Width="100%" CssClass="login-bx">
        <LayoutTemplate>

            <div class="login-submit">
                <span class="btn-sub">
                    <%--<input type="submit" value="免 費 註 冊" onclick="location.href='http://www.wantgoo.com/Registration.aspx';" style="cursor:pointer;">--%>
                    <asp:Button ID="btnRegistration" runat="server" Text="免 費 註 冊" style="cursor:pointer;" OnClick="btnRegistration_Click" />
                </span>
            </div>

            <%--facebook登入按鈕--%>
            <dl class="login-inpt">
                <dd>
                    <input id="btnCreate" type="button"
                        style="border-width: 0px; border-style: none; background-image: url(http://www.wantgoo.com/image/facebooklogin.png); width: 300px; height: 100px; cursor: pointer; background-repeat: no-repeat;"
                        value="" onclick='DoAuth()' />
                </dd>
            </dl>
            <%--facebook登入按鈕--%>

            <dl class="login-inpt">
                <dd>
                    </dd>
                </dl>
            
            <dl class="login-inpt">
                <dt>電子郵件</dt>
                <dd>
                    <asp:TextBox ID="UserName" runat="server" Width="150px"></asp:TextBox></dd>
            </dl>
            <dl class="login-inpt">
                <dt>密 碼</dt>
                <dd>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="150px"></asp:TextBox></dd>
            </dl>

            <dl class="login-remb">
                <dt></dt>
                <dd>
                    <asp:CheckBox ID="RememberMe" runat="server" Text="記住我的登入資料" Checked="True" />
                    <br /><br />
                    <span class="error-txt">
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="帳號密碼不正確，請確認。" ToolTip="無此帳號或密碼錯誤，請確認輸入資料！" ValidationGroup="Login1" ForeColor="DarkRed" Font-Bold="true" Visible="false"></asp:RequiredFieldValidator>
                    </span>
                    <asp:Label runat="server" ID="ltMessage" Visible="false" ForeColor="DarkRed" Font-Bold="true"></asp:Label>
                </dd>
            </dl>

            <div class="login-submit">
                <span class="btn-sub">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="會 員 登 入" ValidationGroup="Login1" style="cursor:pointer;" />
                </span>
            </div>

            <script type="text/javascript">

                (function (d) {
                    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
                    if (d.getElementById(id)) { return; }
                    js = d.createElement('script'); js.id = id; js.async = true;
                    js.src = "//connect.facebook.net/zh_TW/all.js";
                    ref.parentNode.insertBefore(js, ref);
                }(document));

                window.permis = {};
                permis.fbPerms = 'email,user_likes,user_birthday,user_photos,publish_stream,read_stream,read_friendlists';
                window.fbAsyncInit = function () {
                    FB.init({
                        appId: '128424793438',
                        status: true, // check login status
                        cookie: true, // enable cookies to allow the server to access the session
                        xfbml: true,  // parse XFBML
                        oauth: true
                    });

                    $.browser.chrome = /chrome/.test(navigator.userAgent.toLowerCase());
                    if ($.browser.chrome || $.browser.msie) {
                        //FB.XD._origin = window.location.protocol + "//" + document.domain + "/" + FB.guid();
                        //FB.XD.Flash.init();
                        //FB.XD._transport = "flash";
                    } else if ($.browser.opera) {
                        FB.XD._transport = "fragment";
                        FB.XD.Fragment._channelUrl = window.location.protocol + "//" + window.location.host + "/";
                    }
                };

                function Loggin(response) {
                    var accessToken = response.authResponse.accessToken;
                    FB.api('/me', function (response) {
                        var id = response.id;
                        var email = response.email;
                        var name = response.name;
                        var fname = response.first_name;
                        var lname = response.last_name;
                        var g = response.gender;
                        var content = accessToken.concat('&m=', email, '&u=', id, '&l=', lname, '&f=', fname, '&s=', g, '&n=', name);
                        var url = '/fblogin.aspx?a=' + content + '&r=' + $GetParam('r') + '&GoBackUrl=' + $GetParam('GoBackUrl');
                        top.location.href = url;
                    });
                }

                function DoAuth() {
                    FB.login(function (response) {
                        Loggin(response);
                    }, { scope: permis.fbPerms });
                }

                function $GetParam() {
                    var Url = top.window.location.href;
                    var u, g, StrBack = '';
                    if (arguments[arguments.length - 1] == "#")
                        u = Url.split("#");
                    else
                        u = Url.split("?");
                    if (u.length == 1) g = '';
                    else g = u[1];

                    if (g != '') {
                        gg = g.split("&");
                        var MaxI = gg.length;
                        str = arguments[0] + "=";
                        for (i = 0; i < MaxI; i++) {
                            if (gg[i].indexOf(str) == 0) {
                                StrBack = gg[i].replace(str, "");
                                break;
                            }
                        }
                    }
                    return StrBack;
                }
            </script>

        </LayoutTemplate>
    </asp:Login>

    <!-- login-bx:end -->
    <uc8:Footer ID="Footer1" runat="server" EnableViewState="False" />
    <div id="fb-root"></div>
</asp:Content>
