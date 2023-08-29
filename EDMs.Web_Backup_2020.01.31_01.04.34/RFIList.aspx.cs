// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Drawing;
using EDMs.Business.Services.Scope;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.ServiceProcess;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Workflow;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Telerik.Web.Zip;

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class RFIList : Page
    {

        private readonly RFIService rfiService = new RFIService();

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();
        private readonly RFIDetailService rfidetailservice = new RFIDetailService();
        private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();

        /// <summary>
        /// The list folder id.
        /// </summary>
        private List<int> listFolderId = new List<int>();

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
            Session.Add("SelectedMainMenu", "Project Execution");

            if (!Page.IsPostBack)
            {
                this.LoadObjectTree();
                Session.Add("IsListAll", false);
                this.grdDocument.MasterTableView.GetColumn("DeleteWorkflow").Visible = (UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault()) && UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
                var temp = (RadPane)this.Master.FindControl("leftPane");
                temp.Collapsed = true;
                if (!string.IsNullOrEmpty(this.Request.QueryString["RFINo"]))
                {
                    var txtSearch = (System.Web.UI.WebControls.TextBox)this.CustomerMenu.Items[2].FindControl("txtSearch");
                    txtSearch.Text= this.Request.QueryString["RFINo"];
                }

                if ((UserSession.Current.User.IsEngineer.GetValueOrDefault() || UserSession.Current.User.IsLeader.GetValueOrDefault())&& UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[0].Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("AttachWorkflow").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }
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
            var RFIlist = this.rfiService.GetAll().Where(t=> !t.IsDelete.GetValueOrDefault());

            foreach (var changeRequestAttachFile in RFIlist)
            {
                changeRequestAttachFile.IsCanDelete = UserSession.Current.User.Id == changeRequestAttachFile.CreatedBy || UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault();
                changeRequestAttachFile.IsHasImport = this.rfidetailservice.GetByRFI(changeRequestAttachFile.ID).Any();
            }
            var txtSearch = (System.Web.UI.WebControls.TextBox)this.CustomerMenu.Items[2].FindControl("txtSearch");
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var listkey = txtSearch.Text.ToLower().Split(' ').ToArray();
                RFIlist = RFIlist.Where(t => listkey.All(k => t.Number.ToLower().Contains(k))).ToList();

            }

            this.grdDocument.DataSource = RFIlist.OrderByDescending(t=> t.CreatedDate).ThenBy(t =>t.Number);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
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
            else if (e.Argument.Contains("DeleteWorkflow_"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var Obj = this.rfiService.GetById(objId);
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
                    this.rfiService.Update(Obj);
                }
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains( "ExportRFI"))
            {
                var rfiId = new Guid(e.Argument.Split('_')[1]);
                var rfiObj = this.rfiService.GetById(rfiId);
                ExportCMDRReport(rfiObj);
            }
            else if (e.Argument == "DeleteAllDoc")
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    
                    Guid docId;
                    Guid.TryParse(selectedItem.GetDataKeyValue("ID").ToString(), out docId);
                  
                }
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("FileDelete"))
            {
                Guid docId = new Guid(e.Argument.Split('_')[1]);
                var docObj = this.rfiService.GetById(docId);
                if (docObj != null)
                {
                    docObj.IsDelete = true;
                    this.rfiService.Update(docObj);

                    //var listRelateDoc = this.rfidetailservice.GetByRFI(docObj.ID);
                    //if (listRelateDoc != null)
                    //{
                    //    foreach (var objDoc in listRelateDoc)
                    //    {

                    //        this.rfidetailservice.Delete(objDoc);
                    //    }
                    //}

                }
                this.grdDocument.Rebind();
            }


            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "SendNotification")
            {
               
            }

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
           
            Guid docId;
            Guid.TryParse(item.GetDataKeyValue("ID").ToString(), out docId);
            var docObj = this.rfiService.GetById(docId);
            if (docObj != null)
            {
               
                    docObj.IsDelete = true;
                    this.rfiService.Update(docObj);
               
                    //var listRelateDoc = this.rfidetailservice.GetByRFI(docObj.ID);
                    //if (listRelateDoc != null)
                    //{
                    //    foreach (var objDoc in listRelateDoc)
                    //    {
                            
                    //        this.rfidetailservice.Delete(objDoc);
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
                this.CustomerMenu.Items[3].Visible = false;
                
                this.grdDocument.Rebind();
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {

            }
        }

        /// <summary>
        /// The grd document_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {

         
            }
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["IsHasAttachFile"].Text == "True")
                {
                    item.BackColor = Color.Aqua;
                    item.BorderColor = Color.Aqua;
                }
            }

            
        }

        protected void radTreeFolder_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
          //  PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSideCallBack);
        }

        protected void grdDocument_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
            }
        }

        protected void ckbEnableFilter_OnCheckedChanged(object sender, EventArgs e)
        {
            this.grdDocument.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdDocument.Rebind();
        }

        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "Images/folderdir16.png";
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

        private void LoadObjectTree()
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

        

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        

        protected void rtvTreeNode_NodeClick(object sender, RadTreeNodeEventArgs e)
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

      
        
        protected void rtvTreeNode_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = @"Images/discipline.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }
        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void ckbShowAll_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        private void ExportCMDRReport( RFI rfiObj)
        {
            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\Pecc2_Form_RFI.xlsm");

            var dataSheet = workbook.Worksheets[0];
            var dtFull = new DataTable();
            var projectObj = this.projectCodeService.GetById(rfiObj.ProjectId.GetValueOrDefault());

            dataSheet.Cells["E1"].PutValue(projectObj.FullName);
            dataSheet.Cells["C3"].PutValue(rfiObj.IssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            dataSheet.Cells["I3"].PutValue(rfiObj.Number);
            dataSheet.Cells["D10"].PutValue(rfiObj.SiteManager);
            dataSheet.Cells["F10"].PutValue(rfiObj.QAQCManager);

            var filename = projectObj.Code + "_" + "RFI Cover_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsm";
            dtFull.Columns.AddRange(new[]
            {
             
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
                new DataColumn("WorkTitle", typeof (String)),
                new DataColumn("Description", typeof (String)),
                new DataColumn("Localtion", typeof (String)),
                new DataColumn("Time", typeof (String)),
                new DataColumn("Type", typeof (String)),
                new DataColumn("Contractor", typeof (String)),
                new DataColumn("Remark", typeof (String)),
            });


            var rfidetailList = this.rfidetailservice.GetByRFI(rfiObj.ID);
            var disciplineRowCount = 1;
            foreach (var detailObj in rfidetailList)
            {

                var dataRow = dtFull.NewRow();
                dataRow["NoIndex"] = disciplineRowCount;
                dataRow["Discipline"] = detailObj.GroupName;
                dataRow["WorkTitle"] = detailObj.WorkTitle;
                dataRow["Description"] = detailObj.Description;
                dataRow["Localtion"] = detailObj.Location;
                dataRow["Time"] = detailObj.Time.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm");
                dataRow["Type"] = detailObj.InspectionTypeName;
                dataRow["Contractor"] = detailObj.ContractorContact;
                dataRow["Remark"] = detailObj.Remark;
                disciplineRowCount += 1;
                dtFull.Rows.Add(dataRow);
            
                }
        dataSheet.Cells.ImportDataTable(dtFull, false, 4, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
            var validations = dataSheet.Validations;
            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count + 7);
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
        }

        }

       
    }
