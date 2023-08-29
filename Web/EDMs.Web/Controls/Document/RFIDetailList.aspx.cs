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

namespace EDMs.Web.Controls.Document
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
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;


    /// <summary>
    /// Class customer
    /// </summary>
    public partial class RFIDetailList : Page
    {

      

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();
        private readonly RFIDetailService rfidetailservice = new RFIDetailService();
        private readonly RFIService rfiService = new RFIService();

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
                this.CustomerMenu.Items[3].Visible = false;
                var temp = (RadPane)this.Master.FindControl("leftPane");
                temp.Collapsed = true;

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var rfiobj = this.rfiService.GetById(new Guid(this.Request.QueryString["objId"]));
                    GridColumn rowStateColumn = grdDocument.MasterTableView.GetColumnSafe("RFINo");
                    rowStateColumn.CurrentFilterValue = rfiobj.Number ;
                    this.Request.QueryString.Remove("objId");
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
            var RFIlist = this.rfidetailservice.GetAll();
            

            foreach (var changeRequestAttachFile in RFIlist)
            {
                changeRequestAttachFile.IsCanDelete = UserSession.Current.User.Id == changeRequestAttachFile.CreatedBy;
            }
            this.grdDocument.DataSource = RFIlist.OrderByDescending(t=> t.Time).ThenByDescending(t =>t.RFINo).ThenBy(t=> t.Number);
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
            else if (e.Argument == "ExportEMDRReport")
            {
               // ExportCMDRReport();
            }
            else if(e.Argument.Contains("FileDelete"))
            {
                Guid docId = new Guid(e.Argument.Split('_')[1]);
                var detailobj = this.rfidetailservice.GetById(docId);
                if (detailobj != null)
                {
                    this.rfidetailservice.Delete(detailobj);

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
            var docObj = this.rfidetailservice.GetById(docId);
            if (docObj != null)
            {
             this.rfidetailservice.Delete(docObj);
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
              //  this.CustomerMenu.Items[3].Visible = false;
                
              //  this.grdDocument.Rebind();
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

                ////Populate Filters by binding the combo to datasource
                //var filteringItem = (GridFilteringItem)e.Item;
                //var myRadComboBox = (RadComboBox)filteringItem.FindControl("RadComboBoxCustomerProgramDescription");

                //myRadComboBox.DataSource = myDataSet;
                //myRadComboBox.DataTextField = "CustomerProgramDescription";
                //myRadComboBox.DataValueField = "CustomerProgramDescription";
                //myRadComboBox.ClearSelection();
                //myRadComboBox.DataBind();
            }
            if (e.Item is GridDataItem)
            {
                //var item = e.Item as GridDataItem;
                //if (item["IsHasAttachFile"].Text == "True")
                //{
                //    item.BackColor = Color.Aqua;
                //    item.BorderColor = Color.Aqua;
                //}
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
            //var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            //var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);

            //if (ddlProject != null)
            //{
            //    ddlProject.DataSource = projectList;
            //    ddlProject.DataTextField = "FullName";
            //    ddlProject.DataValueField = "ID";
            //    ddlProject.DataBind();

            //    int projectId = Convert.ToInt32(ddlProject.SelectedValue);
            //    this.lblProjectId.Value = ddlProject.SelectedValue;
            //    Session.Add("SelectedProject", projectId);
            //}

         
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

      

        }

       
    }
