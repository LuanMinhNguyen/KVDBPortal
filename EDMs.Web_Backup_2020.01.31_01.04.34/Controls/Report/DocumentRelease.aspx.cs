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
    public partial class DocumentRelease : Page
    {
    
        private readonly DQREDocumentService dqreDocumentSevice = new DQREDocumentService();
      
        private readonly UserService userService = new UserService();

        private readonly RoleService roleService = new RoleService();

        private readonly PermissionService permissionService = new PermissionService();
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
            var overduretask = new List<DQREDocument>();
            this.grdDocument.DataSource = this.dqreDocumentSevice.GetAllDocumentReplease().OrderByDescending(t=> t.CreatedDate);
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
            workbook.Open(filePath + @"Template\DQRE_ReportDocumentRelease.xlsm");

            var dataSheet = workbook.Worksheets[0];
            var dtFull = new DataTable();
            var filename = "Documents-Release-Report_ " + DateTime.Now.ToString("ddmmyy") + ".xlsm";

            dataSheet.Cells["N4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("DocNo", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
                new DataColumn("Unit", typeof (String)),
                new DataColumn("Revision", typeof (String)),
                new DataColumn("IssueDate", typeof (String)),
                new DataColumn("Status", typeof (String)),
                new DataColumn("State", typeof (String)),
                new DataColumn("LasteRev", typeof (String)),
                new DataColumn("Remark", typeof (String)),
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
            var docList = this.dqreDocumentSevice.GetAllDocumentReplease().Where(t=> ListId.Contains(t.ID));
            var docGroupByDisciplineList = docList.GroupBy(t => t.M_DisciplineName);
            var disciplineRowCount = 1;

            foreach (var docGroupByDiscipline in docGroupByDisciplineList)
            {

                var docListOfDiscipline = docGroupByDiscipline.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["DocNo"] = docGroupByDiscipline.Key;
                dtFull.Rows.Add(dataRow);


                foreach (var docObj in docListOfDiscipline)
                {
                    dataRow = dtFull.NewRow();

                    dataRow = dtFull.NewRow();
                    dataRow["DocId"] = docObj.ID;
                    dataRow["NoIndex"] = disciplineRowCount;
                    dataRow["DocNo"] = docObj.DocumentNo;
                    dataRow["DocTitle"] = docObj.DocumentTitle;
                    dataRow["DocType"] = docObj.M_DocumentTypeName;
                    dataRow["Discipline"] = docObj.M_DisciplineName;
                    dataRow["Unit"] = docObj.M_UnitName;
                    dataRow["Revision"] = docObj.Revision;
                    dataRow["IssueDate"] = docObj.IsssuedDate != null ? docObj.IsssuedDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                    dataRow["Status"] = docObj.RevisionStatusName;
                    dataRow["State"] = docObj.RevisionState;
                    dataRow["LasteRev"] = docObj.IsLeaf.GetValueOrDefault() ? "Y" : "N"; 
                    dataRow["Remark"] = docObj.Remark;
                    disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
            }
            
            dataSheet.Cells.ImportDataTable(dtFull, false, 6, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
            var validations = dataSheet.Validations;

            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count + 7);

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
                    if (item.Text == "Documents Release")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }
}