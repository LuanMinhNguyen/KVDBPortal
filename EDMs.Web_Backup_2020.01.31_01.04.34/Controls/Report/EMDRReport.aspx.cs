// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;

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
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;


    /// <summary>
    /// Class customer
    /// </summary>
    public partial class EMDRReport : Page
    {
        private readonly UsersLoginHistoryService _UserloginHistory = new UsersLoginHistoryService();

        private readonly PermissionService permissionService = new PermissionService();

        private readonly UserService userService = new UserService();

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
        /// The rad tree view 1_ node click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        ///  protected void rtvTreeNode_NodeClick1(object sender, RadTreeNodeEventArgs e)
        //{

        //}


        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments()
        {

            var docList = new List<UsersLoginHistory>();

            docList = this._UserloginHistory.GetAll();

            this.grdDocument.DataSource = docList.OrderByDescending(t => t.ServerTime);
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
            workbook.Open(filePath + @"Template\DQRE_ReportUserLogIn.xlsm");

            var dataSheet = workbook.Worksheets[0];
            var dtFull = new DataTable();
            var filename = "USER-LOGIN-DETAILS-REPORT_ " + DateTime.Now.ToString("ddmmyy") + ".xlsm";

            dataSheet.Cells["Q4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("UserName", typeof (String)),
                new DataColumn("FullName", typeof (String)),
                new DataColumn("ServerTime", typeof(String)),
                new DataColumn("Locatime", typeof(String)),
                new DataColumn("TimeZone", typeof(String)),
                new DataColumn("LogOutTime", typeof(String)),
                new DataColumn("DurationTime", typeof(String)),
                new DataColumn("IP", typeof (String)),
                new DataColumn("PhysicalMemory", typeof (String)),
                new DataColumn("Domain", typeof (String)),
                new DataColumn("HostName", typeof (String)),
                new DataColumn("Browser", typeof (String)),
                new DataColumn("OsDetails", typeof (String)),
                new DataColumn("Languare", typeof (String)),
            });

            List<int> ListId = new List<int>();

            this.grdDocument.AllowPaging = false;
            this.grdDocument.Rebind();
            foreach (GridDataItem row in this.grdDocument.Items) // loops through each rows in RadGrid
            {
                var docId=Convert.ToInt32(row.GetDataKeyValue("ID").ToString());
               
                ListId.Add(docId);
            }
            this.grdDocument.AllowPaging = true;
            this.grdDocument.Rebind();
            var docList = this._UserloginHistory.GetAllList(ListId);

            var mindate = docList.Min(t => t.ServerTime);
            var maxdate = docList.Max(t => t.ServerTime);

            dataSheet.Cells["F1"].PutValue(" USER LOGIN DETAILS REPORT (From: " + mindate.Value.ToString("dd/MM/yyy") + " To:" + maxdate.Value.ToString("dd/MM/yyy"));

            var disciplineRowCount = 1;
            
                var dataRow = dtFull.NewRow();
                dtFull.Rows.Add(dataRow);


                foreach (var docObj in docList)
                {
                    dataRow = dtFull.NewRow();

                dataRow["DocId"] = docObj.ID;
                dataRow["NoIndex"] = disciplineRowCount;
                dataRow["UserName"]= docObj.UserName ;
                dataRow["FullName"]= docObj.FullName ;
                dataRow["ServerTime"]= docObj.ServerTime ;
                dataRow["Locatime"]= docObj.LocalTime ;
                dataRow["TimeZone"]= docObj.LocalTimeZone ;
                dataRow["LogOutTime"]= docObj.LogoutLocalTime ;
                dataRow["DurationTime"]= docObj.DurationTimeLogin ;
                dataRow["IP"]= docObj.IpAddress ;
                dataRow["PhysicalMemory"]= docObj.PhysicalMemory ;
                dataRow["Domain"]= docObj.WindownDomainUser ;
                dataRow["HostName"]= docObj.HostNameComputer ;
                dataRow["Browser"]= docObj.Browser ;
                dataRow["OsDetails"]= docObj.OSDetail ;
                dataRow["Languare"]= docObj.LanguageFormat ;
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
                    if (item.Text == "User Login Details")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }
}