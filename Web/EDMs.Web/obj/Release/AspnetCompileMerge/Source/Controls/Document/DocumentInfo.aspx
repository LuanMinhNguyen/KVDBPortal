<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentInfo.aspx.cs" Inherits="EDMs.Web.Controls.Document.DocumentInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Liên hệ</title>
    <link href="~/Content/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
html, body, form {
	overflow:visible;
}
        
.RadComboBoxDropDown_Default .rcbHovered {
       background-color: #46A3D3;
       color: #fff;
   }
   .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered {
       margin: 0 0px;
   }
   .RadComboBox .rcbInputCell .rcbInput{
        border-left-color: #FF0000 !important;
   border-color: #8E8E8E #B8B8B8 #B8B8B8 #46A3D3;
    border-style: solid;
    border-width: 1px 1px 1px 5px;
    color: #000000;
    float: left;
    font: 12px "segoe ui";
    margin: 0;
    padding: 2px 5px 3px;
    vertical-align: middle;
      width: 224px;
   }
   .RadComboBox table td.rcbInputCell, .RadComboBox .rcbInputCell .rcbInput {
       padding-left: 0px !important;
   }
      div.rgEditForm label {
       float: right;
    text-align: right;
    width: 72px;
   }
   .rgEditForm {
       text-align: right;
   }
   .RadComboBox {
       width: 253px !important;
       border-bottom: none !important;
   }
   .RadUpload .ruFileWrap {
       overflow: visible !important;
   }

        /*#grdContactHistory
        {
            height: 300px !important;
        }

        #grdContactHistory_GridData
        {
            height: 250px !important;
        }*/
    </style>
    <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function RowDblClick(sender, eventArgs) {
            window.parent.radopen("Controls/Customers/Details/Detail_Contact_Edit.aspx?ID=" + eventArgs.getDataKeyValue("ID"), "CustomerDialog");
        }
        $(document).ready(function () {
            $(".content").fadeIn("slow");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 500px" class="">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            
            <div class="qlcbFormItem">
                <div class="dnnFormMessage dnnFormInfo">
                    <div class="dnnFormItem dnnFormHelp dnnClear">
                        <p class="dnnFormRequired" style="float: left;">
                            <span style="text-decoration: underline;">Notes</span>: All fields marked with a red are required.
                        </p>
                        <br />
                    </div>
                </div>
            </div>
            

        </div>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
