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

namespace EDMs.Web.Controls.Report
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Workflow;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;


    


    /// <summary>
    /// Class customer
    /// </summary>
    public partial class OverdueDocument : Page
    {
        private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
        private readonly DQREDocumentService dqreDocumentSevice = new DQREDocumentService();
        private readonly DistributionMatrixService matrixService = new DistributionMatrixService();
        private readonly DistributionMatrixDetailService matrixDetailService = new DistributionMatrixDetailService();
        private readonly WorkflowDetailService wfDetailService = new WorkflowDetailService();
        private readonly DQRETransmittalService dqreTransmittalService = new DQRETransmittalService();
        private readonly WorkflowService wfService = new WorkflowService();
        private readonly UserService userService = new UserService();
        private readonly PermissionService permissionService = new PermissionService();
        private readonly RoleService roleService = new RoleService();

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
            Session.Add("SelectedMainMenu", "Report Management");

            if (!Page.IsPostBack)
            {
                this.LoadReportPanel();
                this.LoadDocuments();
            }
        }
        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments()
        {


            DateTime? datenull = null;
        

               var overduretask = new List<TaskProcessing>();

            overduretask = this.dqreDocumentSevice.GetAllOverdueDocument().Select(t => new TaskProcessing
            {
                ID=t.ID,
                TransmittalNumber=t.IncomingTransNo,
                DocumentNumber =t.DocumentNo,
                DocRev=t.Revision,
                CLassCode=t.DocumentClassName,
                StatusCode=t.RevisionStatusName,
                Title=t.DocumentTitle,
                UnitCode=t.M_UnitName,
                DisciplineCode=t.M_DisciplineName,
                LeadReview= GetLeadReview(t),
                TransInDate = t.IncomingTransId != null? this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).ReceivedDate.GetValueOrDefault(): datenull,
                TrasInDueDate= t.IncomingTransId != null ? (this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).DueDate != null? this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).DueDate.GetValueOrDefault() : datenull) : datenull,
               DaysOverDue= t.IncomingTransId != null ?(this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).DueDate!= null ?(DateTime.Now - this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).DueDate.GetValueOrDefault()).Days :0) :0,
            }).ToList();

            this.grdDocument.DataSource = overduretask.Where(t=> t.TrasInDueDate!= null).ToList();
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
               // this.ExportCMDRDataFile();
            }



        }

        private string GetLeadReview(DQREDocument docObj)
        {
            var WFofObject = this.objAssignedWfService.GetAll().Where(t=>t.ObjectID==docObj.ID).Select(t=> t.WorkflowID).Distinct();
            var listuserlead = this.userService.GetAll().Where(t => t.IsLeader.GetValueOrDefault());
            var wfDetailObj = this.wfDetailService.GetAll().Where(t=> WFofObject.Contains(t.WorkflowID)).ToList();
            if (wfDetailObj.Count >0)
            {
                var reviewUserIds = ""; 
                var matrixList = ""; 
                foreach(var item in wfDetailObj)
                {
                    reviewUserIds += item.ReviewUserIds + ";";
                    matrixList += item.DistributionMatrixIDs + ";";
                }
                foreach (var matrix in matrixList.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => this.matrixService.GetById(Convert.ToInt32(t))))
                {
                    var matrixDetailValid = new List<DistributionMatrixDetail>();
                    var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID);
                    // Filter follow matrix rule
                    switch (matrix.TypeId)
                    {
                        // Document have Material/Work Code
                        case 1:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
                            break;
                        // Document have Drawing Code (00) Matrix
                        case 2:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
                            break;
                        // Document have Drawing Code Matrix
                        case 3:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
                            break;
                        // AU, CO, PLG, QIR, GTC, PO Matrix
                        case 4:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId).ToList();
                            break;
                        // EL, ML Matrix
                        case 5:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
                            break;
                        // PP Matrix
                        case 6:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.SerialNo == docObj.SerialNo).ToList();
                            break;
                        // Vendor Document Matrix
                        case 7:
                            matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.MaterialCodeId == docObj.M_MaterialCodeId).ToList();
                            break;
                    }
                    if (matrixDetailList.Where(t=> t.ActionTypeId==3 ).Any())
                    {     
                            reviewUserIds = reviewUserIds + string.Join(";",matrixDetailValid.Where(t => t.ActionTypeId == 3&& t.DocTypeId==docObj.M_DocumentTypeId).Select(t => t.UserId.ToString())) + ";";      
                    }
                }
                
                listuserlead = listuserlead.Where(k => reviewUserIds.Split(';').Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).Contains(k.Id));
                return string.Join(", ", listuserlead.Select(t => t.Username));
            }
            return "";
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

            this.LoadDocuments();
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

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }


        private void ExportCMDRDataFile()
        {
            var filePath = Server.MapPath("~/Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_ReportOverDueDocument.xlsm");

            var dataSheet = workbook.Worksheets[0];
            var dtFull = new DataTable();
            var filename = "OverdueDocuments-Report_ " + DateTime.Now.ToString("ddmmyy") + ".xlsm";

            dataSheet.Cells["O4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));
            dataSheet.Cells["E1"].PutValue("OVERDUE DOCUMENTS REPORT");
            dataSheet.Cells["O5"].PutValue("Days overdue");

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                  new DataColumn("TransmittalNumber", typeof (String)),
                 new DataColumn("DocumentNumber", typeof (String)),
                 new DataColumn("DocRev", typeof (String)),
                 new DataColumn("CLassCode", typeof (String)),
                 new DataColumn("StatusCode", typeof (String)),
                 new DataColumn("Title", typeof (String)),
                 new DataColumn("UnitCode", typeof (String)),
                 new DataColumn("DisciplineCode", typeof (String)),
                 new DataColumn("LeadReview", typeof (String)),
                 new DataColumn("TransInDate", typeof (String)),
                 new DataColumn("TrasInDueDate", typeof (String)),
                new DataColumn("days", typeof (String)),
    });

            List<Guid> ListId = new List<Guid>();

            this.grdDocument.AllowPaging = false;
            this.grdDocument.Rebind();
            foreach (GridDataItem row in this.grdDocument.Items) // loops through each rows in RadGrid
            {
                Guid docId;
                Guid.TryParse(row.GetDataKeyValue("ID").ToString(), out docId);
                ListId.Add(docId);
            }
            this.grdDocument.AllowPaging = true;
            this.grdDocument.Rebind();

            DateTime? datenull = null;
            var docList = this.dqreDocumentSevice.GetAllOverdueDocument().Where(t=>ListId.Contains(t.ID)).Select(t => new {
                ID = t.ID,
                TransmittalNumber = t.IncomingTransNo,
                DocumentNumber = t.DocumentNo,
                DocRev = t.Revision,
                CLassCode = t.DocumentClassName,
                StatusCode = t.RevisionStatusName,
                Title = t.DocumentTitle,
                UnitCode = t.M_UnitName,
                DisciplineCode = t.M_DisciplineName,
                LeadReview = GetLeadReview(t),
                TransInDate = t.IncomingTransId != null ? this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).ReceivedDate.GetValueOrDefault() : datenull,
                TrasInDueDate = t.IncomingTransId != null ? (this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).DueDate != null ? this.dqreTransmittalService.GetById(t.IncomingTransId.GetValueOrDefault()).DueDate.GetValueOrDefault() : datenull) : datenull,
            }).ToList(); 
            var disciplineRowCount = 1;
            
                var dataRow = dtFull.NewRow();
                dtFull.Rows.Add(dataRow);


                foreach (var docObj in docList)
                {
                    dataRow = dtFull.NewRow();

                dataRow["DocId"] = docObj.ID;
                dataRow["NoIndex"] = disciplineRowCount;
                dataRow["TransmittalNumber"] = docObj.TransmittalNumber;
                dataRow["DocumentNumber"]= docObj.DocumentNumber ;
                 dataRow["DocRev"]= docObj.DocRev ;
                 dataRow["CLassCode"]= docObj.CLassCode ;
                 dataRow["StatusCode"]= docObj.StatusCode ;
                 dataRow["Title"]= docObj.Title ;
                 dataRow["UnitCode"]= docObj.UnitCode ;
                 dataRow["DisciplineCode"]= docObj.DisciplineCode ;
                 dataRow["LeadReview"]= docObj.LeadReview ;
                dataRow["TransInDate"]= docObj.TransInDate!= null ? docObj.TransInDate.GetValueOrDefault().ToString("dd/MM/yyyy"):string.Empty ;
                dataRow["TrasInDueDate"]= docObj.TrasInDueDate != null ? docObj.TrasInDueDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                dataRow["days"] = (DateTime.Now- docObj.TrasInDueDate.Value).Days;
                disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
            
            dataSheet.Cells.ImportDataTable(dtFull, false, 6, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
            var validations = dataSheet.Validations;

            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);

            dataSheet.AutoFitRows(true);

            workbook.Save(filePath + filename);
            this.DownloadByWriteByte(filePath + filename, filename, true);

        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

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
                ExportCMDRDataFile();
            }
        }

        private void LoadReportPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ReportId"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "REPORT TYPE" });

                this.radPbReport.DataSource = permissions;
                this.radPbReport.DataFieldParentID = "ParentId";
                this.radPbReport.DataFieldID = "Id";
                this.radPbReport.DataValueField = "Id";
                this.radPbReport.DataTextField = "MenuName";
                this.radPbReport.DataBind();
                this.radPbReport.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbReport.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Overdue Documents")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }
}