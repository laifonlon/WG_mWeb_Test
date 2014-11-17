<%@ Page Language="VB" AutoEventWireup="false" CodeFile="test.aspx.vb" Inherits="Services_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="../ckfinder/ckfinder.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="ckdata" runat="server" TextMode="MultiLine" Rows="10" Columns="80" ClientIDMode="Static" Font-Size="32px"></asp:TextBox>
        <script type="text/javascript">
            var editor = CKEDITOR.replace('<%= ckdata.ClientID %>');
            CKFinder.setupCKEditor(editor, '/ckfinder/');
        </script>
    </div>
    </form>
</body>
</html>
