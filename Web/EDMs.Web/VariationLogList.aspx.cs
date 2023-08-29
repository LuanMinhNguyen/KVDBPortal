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
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Telerik.Windows.Zip;

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class VariationLogList : Page
    {

        private readonly PECC2DocumentsService _PECC2documentService = new PECC2DocumentsService();

        private readonly ProjectCodeService _projectcodeService = new ProjectCodeService();
        
        private readonly UserService userService = new UserService();

        private readonly AttachFileService attachFileService = new AttachFileService();

        private readonly RoleService roleService = new RoleService();

        private readonly OrganizationCodeService _organizationcodeService = new OrganizationCodeService();

        private readonly VariationLogService variationLogService = new VariationLogService();


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
                var temp = (RadPane)this.Master.FindControl("leftPane");
                temp.Collapsed = true;
                if (!UserSession.Current.User.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.IsDC.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[0].Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false; 
                         //this.grdDocument.MasterTableView.GetColumn("ReviseColumn").Visible = false;
                }

                if (UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault())
                {
                    this.IsFullPermission.Value = "true";
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
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            var projectId = Convert.ToInt32(ddlProject.SelectedValue);
            var variationList = this.variationLogService.GetAllByProject(projectId);
            this.grdDocument.DataSource = variationList.OrderBy(t =>t.CreatedDate);
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
            }
            else if (e.Argument == "DeleteAllDoc")
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    
                    Guid docId;
                    Guid.TryParse(selectedItem.GetDataKeyValue("ID").ToString(), out docId);
                    var docObj = this._PECC2documentService.GetById(docId);
                    if (docObj != null)
                    {
                        if (docObj.ParentId == null)
                        {
                            docObj.IsDelete = true;
                            this._PECC2documentService.Update(docObj);
                        }
                        else
                        {
                            var listRelateDoc =
                                this._PECC2documentService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                            if (listRelateDoc != null)
                            {
                                foreach (var objDoc in listRelateDoc)
                                {
                                    objDoc.IsDelete = true;
                                    this._PECC2documentService.Update(objDoc);
                                }
                            }
                        }
                    }
                }
                this.grdDocument.Rebind();
            }
           
            ////else if (e.Argument == "ExportMasterList")
            ////{
            ////    var filePath = Server.MapPath("Exports") + @"\";
            ////    var workbook = new Workbook();
            ////    workbook.Open(filePath + @"Template\PECC2_ProjectDocListTemplate.xls");
            ////    var sheets = workbook.Worksheets;
            ////    var tempSheet = sheets[0];
            ////    var readMeSheet = sheets[1];

            //// //   var projectId = this.ddlProject.SelectedItem != null ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0;

            ////    var projectcodelist = this._projectcodeService.GetAll();
            ////    var RevisionStatustlist = this._RevisionStatusService.GetAll();
            ////    var documentcodelist = this._DocumnetCodeSErvie.GetAll();
            ////    var documentclasslist = this._DocumentClassService.GetAll();
            ////    var Confdentialitylist = this._ConfidentialityService.GetAll();
            ////    var RevisionSchemaList = this._RevisionSchemaService.GetAll();

            ////    var systemMasterlist = this._PECC2DocumentMasterService.GetAll();

            ////    for (int i = 0; i < projectcodelist.Count; i++)
            ////    {
            ////        readMeSheet.Cells["B" + (7 + i)].PutValue(projectcodelist[i].Code);
            ////        readMeSheet.Cells["C" + (7 + i)].PutValue(projectcodelist[i].Description);
            ////    }

               
            ////    for (int i = 0; i < documentclasslist.Count; i++)
            ////    {
            ////        readMeSheet.Cells["E" + (7 + i)].PutValue(documentclasslist[i].Code);
            ////        readMeSheet.Cells["F" + (7 + i)].PutValue(documentclasslist[i].Description);
            ////    }
            ////    for (int i = 0; i < documentcodelist.Count; i++)
            ////    {
            ////        readMeSheet.Cells["H" + (7 + i)].PutValue(documentcodelist[i].Code);
            ////        readMeSheet.Cells["I" + (7 + i)].PutValue(documentcodelist[i].Description);
            ////    }
            ////    for (int i = 0; i < RevisionStatustlist.Count; i++)
            ////    {
            ////        readMeSheet.Cells["K" + (7 + i)].PutValue(RevisionStatustlist[i].Code);
            ////        readMeSheet.Cells["L" + (7 + i)].PutValue(RevisionStatustlist[i].Description);
            ////    }
            ////    for (int i = 0; i < RevisionSchemaList.Count; i++)
            ////    {
            ////        readMeSheet.Cells["N" + (7 + i)].PutValue(RevisionSchemaList[i].Code);
            ////        readMeSheet.Cells["O" + (7 + i)].PutValue(RevisionSchemaList[i].Description);
            ////    }
            ////    for (int i = 0; i < Confdentialitylist.Count; i++)
            ////    {
            ////        readMeSheet.Cells["Q" + (7 + i)].PutValue(Confdentialitylist[i].Code);
            ////        readMeSheet.Cells["R" + (7 + i)].PutValue(Confdentialitylist[i].Description);
            ////    }
            ////    for (int i = 0; i < systemMasterlist.Count; i++)
            ////    {
            ////        readMeSheet.Cells["Y" + (7 + i)].PutValue(systemMasterlist[i].SystemDocumentNo);
            ////        readMeSheet.Cells["Z" + (7 + i)].PutValue(systemMasterlist[i].Title);
            ////    }
            ////    //"I=8"
            ////    var rangeProjectCodeList = readMeSheet.Cells.CreateRange("B7", "B" + (7 + (projectcodelist.Count == 0 ? 1 : projectcodelist.Count)));
            ////    rangeProjectCodeList.Name = "ProjectCode";
            ////    var rangDocumentClassList = readMeSheet.Cells.CreateRange("E7", "E" + (7 + (documentclasslist.Count == 0 ? 1 : documentclasslist.Count)));
            ////    rangDocumentClassList.Name = "DocumentClass";
            ////    var rangDocumentCodeList = readMeSheet.Cells.CreateRange("H7", "H" + (7 + (documentcodelist.Count == 0 ? 1 : documentcodelist.Count)));
            ////    rangDocumentCodeList.Name = "DocumentCode";
            ////    var rangRevisionStatusList = readMeSheet.Cells.CreateRange("K7", "K" + (7 + (RevisionStatustlist.Count == 0 ? 1 : RevisionStatustlist.Count)));
            ////    rangRevisionStatusList.Name = "RevisionStatus";
            ////    var rangRevisionSchemaList = readMeSheet.Cells.CreateRange("N7", "N" + (7 + (RevisionSchemaList.Count == 0 ? 1 : RevisionSchemaList.Count)));
            ////      rangRevisionSchemaList.Name = "RevisionSchema";
            ////    var rangConfdentialitylist = readMeSheet.Cells.CreateRange("Q7", "Q" + (7 + (Confdentialitylist.Count == 0 ? 1 : Confdentialitylist.Count)));
            ////    rangConfdentialitylist.Name = "ConfdentialityCode";
            ////    var rangesystemlist = readMeSheet.Cells.CreateRange("Y7", "Y" + (7 + (systemMasterlist.Count == 0 ? 1 : systemMasterlist.Count)));
            ////    rangesystemlist.Name = "SystemList";

            ////    var validations = sheets[2].Validations;
            ////    this.CreateValidation(rangesystemlist.Name, validations, 2, 1000, 1, 1);
            ////    this.CreateValidation(rangeProjectCodeList.Name, validations, 2, 1000, 2, 2);
            ////    this.CreateValidation(rangRevisionSchemaList.Name, validations, 2, 1000, 8, 8);
            ////    this.CreateValidation(rangRevisionStatusList.Name, validations, 2, 1000, 9, 9);
            ////    this.CreateValidation(rangDocumentClassList.Name, validations, 2, 1000, 10, 10);
            ////    this.CreateValidation(rangDocumentCodeList.Name, validations, 2, 1000, 11, 11);
            ////    this.CreateValidation(rangConfdentialitylist.Name, validations, 2, 1000, 12, 12);



            ////    workbook.Worksheets[0].IsVisible = false;
            ////        //workbook.Worksheets.RemoveAt(2);

            ////        var filename = DateTime.Now.ToString("ddmmyyyy")+"_" + "$" + "ProjectDocumentListTemplate.xls";
            ////        workbook.Save(filePath + filename);
            ////        this.DownloadByWriteByte(filePath + filename, filename, true);

                
            ////}
        
 

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
            var docObj = this.variationLogService.GetById(docId);
            if (docObj != null)
            {
               
                    docObj.IsDelete = true;
                    this.variationLogService.Update(docObj);

            }
        }

        protected void ckbEnableFilter_OnCheckedChanged(object sender, EventArgs e)
        {
            this.grdDocument.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdDocument.Rebind();
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
            var projectList = this._projectcodeService.GetAll().OrderBy(t => t.Code);

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
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            int projectId = Convert.ToInt32(ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
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
        protected void ddlCategory_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/category1.png";
        }
    }
}