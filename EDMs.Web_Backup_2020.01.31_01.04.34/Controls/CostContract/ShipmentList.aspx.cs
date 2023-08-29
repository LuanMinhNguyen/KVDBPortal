// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Telerik.Web.Zip;


namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ShipmentList : Page
    {

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

        private readonly EquipmentService equimentService = new EquipmentService();

        private readonly ShipmentService shipmentService = new ShipmentService();

        private readonly ShipmentDetailService shipmentdetailService = new ShipmentDetailService();

        private readonly ShipmentDocumentFileService shipmentdocumentService = new ShipmentDocumentFileService();

        private readonly UserService userService = new UserService();

        private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var leftPane = this.Master.FindControl("leftPane") as RadPane;
            if (leftPane != null)
            {
                leftPane.Collapsed = true;
            }
            if (!Page.IsPostBack)
            {
                this.LoadComboData();
                Session.Add("IsListAll", false);

                if (UserSession.Current.User.Role.IsInternal.GetValueOrDefault() && !UserSession.Current.User.IsAdmin.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[0].Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("AttachWorkflow").Visible = true;
                }
                else if (!UserSession.Current.User.IsAdmin.GetValueOrDefault())
                {
                    this.grdDocument.MasterTableView.GetColumn("AttachWorkflow").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteWorkflow").Visible = false;
                }
                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.IsFullPermission.Value = "true";
                }
                if (!string.IsNullOrEmpty(this.Request.QueryString["ShipmentNo"]))
                {
                    var txtSearch = (System.Web.UI.WebControls.TextBox)this.CustomerMenu.Items[2].FindControl("txtSearch");
                    txtSearch.Text = this.Request.QueryString["ShipmentNo"];
                }

                if (UserSession.Current.User.IsEngineer.GetValueOrDefault() || UserSession.Current.User.IsLeader.GetValueOrDefault())
                {
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                    this.CustomerMenu.Items[0].Visible = false;
                }
            }
        }

        private void LoadComboData()
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);

            if (ddlProject != null)
            {
                ddlProject.DataSource = projectList;
                ddlProject.DataTextField = "FullName";
                ddlProject.DataValueField = "ID";
                ddlProject.DataBind();

                int projectId = Convert.ToInt32(ddlProject.SelectedValue);
                this.lblProjectId.Value = ddlProject.SelectedValue;
                Session.Add("SelectedProject", projectId);
            }
        }

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false, bool isListAll = false)
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            var txtSearch = (System.Web.UI.WebControls.TextBox)this.CustomerMenu.Items[2].FindControl("txtSearch");
            if (ddlProject != null)
            {
                var projectId = ddlProject.SelectedItem != null ? Convert.ToInt32(ddlProject.SelectedValue) : 0;
                var projectList = this.shipmentService.GetAllByProject(projectId);
                if(UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
                {
                    projectList = projectList.Where(t=> t.IsSend.GetValueOrDefault()).ToList();
                }
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    var listkey = txtSearch.Text.ToLower().Split(' ').ToArray();
                    projectList = projectList.Where(t => listkey.All(k => (t.Number.ToLower() + " " + t.Description.ToLower()).Contains(k))).ToList();

                }

                this.grdDocument.DataSource = projectList;
            }

        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("SendShipment"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var shipmentObj = this.shipmentService.GetById(objId);
                if (shipmentObj != null && !shipmentObj.IsSend.GetValueOrDefault())
                {
                    if (!this.shipmentdetailService.GetByShipmentId(shipmentObj.ID).Any() && !this.shipmentdocumentService.GetOfShipment(shipmentObj.ID).Any())
                    {
                        this.RadWindowManager1.RadAlert("Please create shipment detail and documents!", 350, 70, "Warring", "");
                    }
                    else
                    {
                        shipmentObj.IsSend = true;
                        this.shipmentService.Update(shipmentObj);
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                        {
                            // this.NotifiNewTransmittal(transObj);
                            this.ContrctorSendemail(shipmentObj);
                        }
                        this.grdDocument.Rebind();
                    }
                }
            }
            else if(e.Argument.Contains("DeleteShipment"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var shipmentObj = this.shipmentService.GetById(objId);
                if (!this.shipmentdetailService.GetByShipmentId(shipmentObj.ID).Any() && !this.shipmentdocumentService.GetOfShipment(shipmentObj.ID).Any())
                {
                    this.RadWindowManager1.RadAlert("Please delete shipment detail and documents befor delete shipment!", 350, 70, "Warring", "");
                }
                else
                {
                    this.shipmentService.Delete(objId);
                    this.grdDocument.Rebind();
                }


            }
            else if (e.Argument.Contains("DeleteWorkflow_"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var Obj = this.shipmentService.GetById(objId);
                if (Obj != null)
                {
                    //delete assing user
                    var listassign = this.objAssignedWfService.GetAllByObj(Obj.ID);
                    foreach (var item in listassign)
                    {
                        this.objAssignedWfService.Delete(item);
                    }
                    var listUserAssign = this.objAssignedUserService.GetAllListObjID(Obj.ID);
                    foreach (var item in listUserAssign)
                    {
                        this.objAssignedUserService.Delete(item);
                    }
                    Obj.IsAttachWorkflow = false;
                    Obj.IsInWFProcess = false;
                    Obj.IsWFComplete = false;
                    this.shipmentService.Update(Obj);
                }
                this.grdDocument.Rebind();
            }

        }

        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            this.LoadDocuments(false, isListAll);
        }

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId =new Guid (item.GetDataKeyValue("ID").ToString());
            var docObj = this.shipmentService.GetById(docId);
            if (docObj != null)
            {
                        this.shipmentService.Delete(docObj);
               
                    //var listRelateDoc = this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                    //if (listRelateDoc != null)
                    //{
                    //    foreach (var objDoc in listRelateDoc)
                    //    {
                    //        objDoc.IsDelete = true;
                    //        this.documentPackageService.Update(objDoc);
                    //    }
                    //}
                
            }
        }

        /// <summary>
        /// The grd document_ item command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {

            }
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
               
                this.grdDocument.Rebind();
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {

            }
        }

       
        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/folderdir16.png";
        }

      
        private void CreateValidation(string formular, ValidationCollection objValidations, int startRow, int endRow, int startColumn, int endColumn)
        {
            // Create a new validation to the validations list.
            Validation validation = objValidations[objValidations.Add()];

            // Set the validation type.
            validation.Type = Aspose.Cells.ValidationType.List;

            // Set the operator.
            validation.Operator = OperatorType.None;

            // Set the in cell drop down.
            validation.InCellDropDown = true;

            // Set the formula1.
            validation.Formula1 = "=" + formular;

            // Enable it to show error.
            validation.ShowError = true;

            // Set the alert type severity level.
            validation.AlertStyle = ValidationAlertType.Stop;

            // Set the error title.
            validation.ErrorTitle = "Error";

            // Set the error message.
            validation.ErrorMessage = "Please select item from the list";

            // Specify the validation area.
            CellArea area;
            area.StartRow = startRow;
            area.EndRow = endRow;
            area.StartColumn = startColumn;
            area.EndColumn = endColumn;

            // Add the validation area.
            validation.AreaList.Add(area);

            ////return validation;
        }

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }
                //Mo file de doc
                FileStream fs = new FileStream(strFileName, FileMode.Open);
                int streamLength = Convert.ToInt32(fs.Length);
                byte[] data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

     
        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
        }

        protected void rtvPR_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
        }

      
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {
                var filterItem = (GridFilteringItem)e.Item;
                var selectedProperty = new List<string>();

                var ddlFilterRev = (RadComboBox)filterItem.FindControl("ddlFilterRev");
            }
        }

        protected DateTime? SetPublishDate(GridItem item)
        {
            if (item.OwnerTableView.GetColumn("Index27").CurrentFilterValue == string.Empty)
            {
                return new DateTime?();
            }
            else
            {
                return DateTime.Parse(item.OwnerTableView.GetColumn("Index27").CurrentFilterValue);
            }
        }

        protected void rtvPR_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            //e.Node.ImageUrl = @"~/Images/shopping.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        private void ContrctorSendemail(Shipment transmittal)
        {
            try
            {
                if (transmittal != null)
                {

                    var userListid = this.userService.GetAllByDC().Where(t => t.Role.IsInternal.GetValueOrDefault());
                    var projctobj = this.projectCodeService.GetById(transmittal.ProjectID.GetValueOrDefault());

                    var smtpClient = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        Host = ConfigurationManager.AppSettings["Host"],
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                    };
                    int count = 0;
                    var containtable = string.Empty;

                    var subject = "FYI: New shipment submitted, " + transmittal.Number + ", " + transmittal.Date.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + transmittal.Description;

                    var message = new MailMessage();
                    message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                    message.Subject = subject;
                    message.BodyEncoding = new UTF8Encoding();
                    message.IsBodyHtml = true;
                    var bodyContent = @"<head><title></title><style>
                body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                </style></head>
                <body>
               <h2 style='font-family:'Arial Rounded MT Bold';'><u>Shipment Notification</u></h2>
                <span class='msg'>Dear All,
                <br /><br />Please be informed about new shipment submitted:
                </span>
                <br /><br />
                <table border='1'>
                <tr><td colspan='6' class='ch1'>Shipment Details</td></tr>
                <tr>
                    <td class='ch2'>Shipment No.</td><td class='ch2'>:</td><td>" + transmittal.Number + @"</td>
                   
                <tr>
                    <td class='ch2'>Description</td><td class='ch2'>:</td><td>" + transmittal.Description + @"</td>
                   
                <tr>
                    <td class='ch2'>Issued Date</td><td class='ch2'>:</td><td>" + transmittal.Date.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                   
                <tr>
                    <td class='ch2'>Type</td><td class='ch2'>:</td><td>" + transmittal.ShipmentTypeName + @"</td>
                </tr>";

                    var st = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/CostContract/ShipmentList.aspx?ShipmentNo=" + transmittal.Number;
                    
                    bodyContent += @"</table>
                        <div class='link'>
                        <br />
                        <u><b>Useful Links:</b></u>
                        <ul>
                            <li>
                                Click <a href='" + st + @"'>here</a> to show <u>this shipment</u> in DMDC System
                            </li>
                        </ul>
                        </div>
                        <br />
                        <h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
                        <hr />
                        <h3 class='system'>DMDC System</h3>
                        <br />
                        <span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span></body>";
                    message.Body = bodyContent;
                    var Userlist = userListid.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                    foreach (var user in Userlist)
                    {
                        try
                        {
                            message.To.Add(new MailAddress(user.Email));
                        }
                        catch { }

                    }
                    smtpClient.Send(message);
                }
            }
            catch { }
        }
        protected void ckbShowAll_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }
    }
}