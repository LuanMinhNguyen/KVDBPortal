<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkupControl.aspx.cs" Inherits="EDMs.Web.Controls.MarkupTool.MarkupControl" %>
<%@ Register TagPrefix="MarkupTool" TagName="PDF" Src="~/Controls/MarkupTool/PdfControl.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PEDMS - PDF Markup Control</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <MarkupTool:PDF runat="server" ID="pdfMarkupControl"/>
    </div>
    </form>
</body>
</html>
