// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;

namespace EDMs.Web.Controls.Report
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;


    /// <summary>
    /// Class customer
    /// </summary>
    public partial class MonthlyReport : Page
    {

        private readonly UsersLoginHistoryService _UserloginHistory = new UsersLoginHistoryService();

        private readonly PermissionService permissionService = new PermissionService();

        private readonly UserService userService = new UserService();

        private readonly RoleService roleService = new RoleService();

        private readonly PECC2DocumentsService pecc2DocumentsService = new PECC2DocumentsService();

        private readonly ChangeRequestService changeRequestService = new ChangeRequestService();

        private readonly NCR_SIService ncrSiService = new NCR_SIService();

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

        private readonly PECC2TransmittalService pecc2TransmittalService = new PECC2TransmittalService();

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
                this.txtFromDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                this.txtTodate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                var projectCodeList = this.projectCodeService.GetAll();
                this.ddlProject.DataSource = projectCodeList;
                this.ddlProject.DataTextField = "FullName";
                this.ddlProject.DataValueField = "ID";
                this.ddlProject.DataBind();

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
        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "Images/folderdir16.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }


        //private void ExportCMDRDataFile()
        //{
        //    var filePath = Server.MapPath("~/Exports") + @"\";
        //    var workbook = new Workbook();
        //    workbook.Open(filePath + @"Template\DQRE_ReportUserLogIn.xlsm");

        //    var dataSheet = workbook.Worksheets[0];
        //    var dtFull = new DataTable();
        //    var filename = "USER-LOGIN-DETAILS-REPORT_ " + DateTime.Now.ToString("ddmmyy") + ".xlsm";

        //    dataSheet.Cells["Q4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

        //    dtFull.Columns.AddRange(new[]
        //    {
        //        new DataColumn("DocId", typeof (String)),
        //        new DataColumn("NoIndex", typeof (String)),
        //        new DataColumn("UserName", typeof (String)),
        //        new DataColumn("FullName", typeof (String)),
        //        new DataColumn("ServerTime", typeof(String)),
        //        new DataColumn("Locatime", typeof(String)),
        //        new DataColumn("TimeZone", typeof(String)),
        //        new DataColumn("LogOutTime", typeof(String)),
        //        new DataColumn("DurationTime", typeof(String)),
        //        new DataColumn("IP", typeof (String)),
        //        new DataColumn("PhysicalMemory", typeof (String)),
        //        new DataColumn("Domain", typeof (String)),
        //        new DataColumn("HostName", typeof (String)),
        //        new DataColumn("Browser", typeof (String)),
        //        new DataColumn("OsDetails", typeof (String)),
        //        new DataColumn("Languare", typeof (String)),
        //    });

        //    List<int> ListId = new List<int>();

        //    this.grdDocument.AllowPaging = false;
        //    this.grdDocument.Rebind();
        //    foreach (GridDataItem row in this.grdDocument.Items) // loops through each rows in RadGrid
        //    {
        //        var docId=Convert.ToInt32(row.GetDataKeyValue("ID").ToString());
               
        //        ListId.Add(docId);
        //    }
        //    this.grdDocument.AllowPaging = true;
        //    this.grdDocument.Rebind();
        //    var docList = this._UserloginHistory.GetAllList(ListId);

        //    var mindate = docList.Min(t => t.ServerTime);
        //    var maxdate = docList.Max(t => t.ServerTime);

        //    dataSheet.Cells["F1"].PutValue(" USER LOGIN DETAILS REPORT (From: " + mindate.Value.ToString("dd/MM/yyy") + " To:" + maxdate.Value.ToString("dd/MM/yyy"));

        //    var disciplineRowCount = 1;
            
        //        var dataRow = dtFull.NewRow();
        //        dtFull.Rows.Add(dataRow);


        //        foreach (var docObj in docList)
        //        {
        //            dataRow = dtFull.NewRow();

        //        dataRow["DocId"] = docObj.ID;
        //        dataRow["NoIndex"] = disciplineRowCount;
        //        dataRow["UserName"]= docObj.UserName ;
        //        dataRow["FullName"]= docObj.FullName ;
        //        dataRow["ServerTime"]= docObj.ServerTime ;
        //        dataRow["Locatime"]= docObj.LocalTime ;
        //        dataRow["TimeZone"]= docObj.LocalTimeZone ;
        //        dataRow["LogOutTime"]= docObj.LogoutLocalTime ;
        //        dataRow["DurationTime"]= docObj.DurationTimeLogin ;
        //        dataRow["IP"]= docObj.IpAddress ;
        //        dataRow["PhysicalMemory"]= docObj.PhysicalMemory ;
        //        dataRow["Domain"]= docObj.WindownDomainUser ;
        //        dataRow["HostName"]= docObj.HostNameComputer ;
        //        dataRow["Browser"]= docObj.Browser ;
        //        dataRow["OsDetails"]= docObj.OSDetail ;
        //        dataRow["Languare"]= docObj.LanguageFormat ;
        //            disciplineRowCount += 1;
        //            dtFull.Rows.Add(dataRow);
        //        }
            
        //    dataSheet.Cells.ImportDataTable(dtFull, false, 6, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
        //    var validations = dataSheet.Validations;

        //    dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);

        //    dataSheet.AutoFitRows(true);

        //    workbook.Save(filePath + filename);
        //    this.DownloadByWriteByte(filePath + filename, filename, true);

        //}

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
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
                    if (item.Text == "Monthly Report")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            if (this.ddlProject.SelectedItem != null)
            {
                var projectObj = this.projectCodeService.GetById(Convert.ToInt32(this.ddlProject.SelectedValue));
                var filePath = Server.MapPath("~/Exports") + @"\";
                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\PECC2_MDR.xlsm");

                var docListSheet = workbook.Worksheets[0];
                var overalProgessSheet = workbook.Worksheets[1];
                var hseManagementSheet = workbook.Worksheets[2];
                var qualityManagementSheet = workbook.Worksheets[3];
                var documentManagementSheet = workbook.Worksheets[4];
                var documentResponedSheet = workbook.Worksheets[5];
                var dtDocIncomingList = new DataTable();
               // var dtDocOutgoingList = new DataTable();

                // Fill Overal Progess
                var engDocList = this.pecc2DocumentsService.GetAllProjectCode(Convert.ToInt32(this.ddlProject.SelectedValue)).OrderBy(t => t.DocNo).ToList();
                var count = 0;

                for (int i = 0; i < 31; i++)
                {
                    if (i == 0)
                    {
                        docListSheet.Cells[0, 52 + i].PutValue(projectObj.StartDate);
                        docListSheet.Cells[0, 84 + i].PutValue(projectObj.StartDate);
                    }
                    else
                    {
                        docListSheet.Cells[0, 52 + i].PutValue(projectObj.StartDate.GetValueOrDefault().AddMonths(i));
                        docListSheet.Cells[0, 84 + i].PutValue(projectObj.StartDate.GetValueOrDefault().AddMonths(i));
                    }

                    docListSheet.Cells[1, 52 + i].Formula = "=" + docListSheet.Cells[2, 52 + i].Name + "-" + docListSheet.Cells[2, 52 + i - 1].Name;
                    docListSheet.Cells[1, 84 + i].Formula = "=" + docListSheet.Cells[2, 84 + i].Name + "-" + docListSheet.Cells[2, 84 + i - 1].Name;

                    docListSheet.Cells[2, 52 + i].Formula = "=SUMPRODUCT($C$4:$C$" + (3 + engDocList.Count) + ","+ docListSheet.Cells[3, 52 + i].Name + ":"+ docListSheet.Cells[2 + engDocList.Count, 52 + i].Name + ")";
                    docListSheet.Cells[2, 84 + i].Formula = "=SUMPRODUCT($C$4:$C$" + (3 + engDocList.Count) + ","+ docListSheet.Cells[3, 84 + i].Name + ":"+ docListSheet.Cells[2 + engDocList.Count, 84 + i].Name + ")";

                }

                docListSheet.Cells["B3"].Formula = "=SUM(B4:B"+ (3 + engDocList.Count) +")";
                foreach (var docObj in engDocList)
                {
                    docListSheet.Cells["A" + (4 + count)].PutValue(docObj.DocNo);
                    docListSheet.Cells["B" + (4 + count)].PutValue(docObj.ManHour.GetValueOrDefault());
                    docListSheet.Cells["C" + (4 + count)].Formula = "=B" + (4 + count) + "/$B$3";
                    docListSheet.Cells["D" + (4 + count)].PutValue(docObj.PlannedDate);
                    docListSheet.Cells["E" + (4 + count)].PutValue(docObj.ActualDate);

                    for (int i = 0; i < 31; i++)
                    {
                        docListSheet.Cells[3 + count, 52 + i].Formula = "=IF($"+ docListSheet.Cells[3 + count, 3].Name + "<="+ docListSheet.Cells[0, 52 + i].Name + ",1,0)";

                        docListSheet.Cells[3 + count, 84 + i].Formula = "=IF($" + docListSheet.Cells[3 + count, 4].Name + "<=" + docListSheet.Cells[0, 84 + i].Name + ",1,0)";
                    }

                    count += 1;
                }

                // -----------------------------------------------------------------------------------------------------------

                // Fill HSE
                var hseList = this.ncrSiService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue))
                    .Where(t => (t.Type == 1 || t.Type == 2)
                                && t.GroupName == "HSE"
                                && ((this.txtFromDate.SelectedDate == null || t.IssuedDate >= this.txtFromDate.SelectedDate)
                                 && (this.txtTodate.SelectedDate == null || t.IssuedDate < this.txtTodate.SelectedDate.GetValueOrDefault().AddDays(1))));
                var hseCount = 0;
                foreach (var hse in hseList)
                {
                    hseManagementSheet.Cells["B" + (4 + hseCount)].PutValue(hse.Number);
                    hseManagementSheet.Cells["C" + (4 + hseCount)].PutValue(hse.IssuedDate);
                    hseManagementSheet.Cells["D" + (4 + hseCount)].PutValue(hse.Subject);
                    hseManagementSheet.Cells["E" + (4 + hseCount)].PutValue(hse.Status);

                    hseCount += 1;
                }
                // ----------------------------------------------------------------------------------------------------------

                // Fill Quality
                var qualityList = this.ncrSiService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue))
                    .Where(t => (t.Type == 1 || t.Type == 2)
                                && t.GroupName == "C"
                                && ((this.txtFromDate.SelectedDate == null || t.IssuedDate >= this.txtFromDate.SelectedDate)
                                 && (this.txtTodate.SelectedDate == null || t.IssuedDate < this.txtTodate.SelectedDate.GetValueOrDefault().AddDays(1))));
                var qualityCount = 0;
                foreach (var hse in qualityList)
                {
                    qualityManagementSheet.Cells["B" + (4 + qualityCount)].PutValue(hse.Number);
                    qualityManagementSheet.Cells["C" + (4 + qualityCount)].PutValue(hse.IssuedDate);
                    qualityManagementSheet.Cells["D" + (4 + qualityCount)].PutValue(hse.Subject);
                    qualityManagementSheet.Cells["E" + (4 + qualityCount)].PutValue(hse.Status);

                    qualityCount += 1;
                }
                // ----------------------------------------------------------------------------------------------------------

                // Fill Imcoming Trans
                var incomingTransList = this.pecc2TransmittalService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue), 1, string.Empty)
                    .Where(t => 
                        (this.txtFromDate.SelectedDate == null || t.IssuedDate >= this.txtFromDate.SelectedDate)
                        && (this.txtTodate.SelectedDate == null || t.IssuedDate < this.txtTodate.SelectedDate.GetValueOrDefault().AddDays(1))).ToList();

                var incomingTransCount = 0;
                foreach (var trans in incomingTransList)
                {
                    documentManagementSheet.Cells["B" + (4 + incomingTransCount)].PutValue(incomingTransCount + 1);
                    documentManagementSheet.Cells["C" + (4 + incomingTransCount)].PutValue(trans.TransmittalNo);
                    documentManagementSheet.Cells["D" + (4 + incomingTransCount)].PutValue(trans.ReceivedDate);
                    documentManagementSheet.Cells["E" + (4 + incomingTransCount)].PutValue(trans.DueDate);
                    documentManagementSheet.Cells["F" + (4 + incomingTransCount)].PutValue(trans.Description);
                    if (trans.IsCreateOutGoingTrans.GetValueOrDefault())
                    {
                        var objtransout = this.pecc2TransmittalService.GetByRefId(trans.ID);
                        if(objtransout!= null)
                        {
                            documentManagementSheet.Cells["G" + (4 + incomingTransCount)].PutValue(objtransout.TransmittalNo);
                            documentManagementSheet.Cells["H" + (4 + incomingTransCount)].PutValue(objtransout.IssuedDate);
                        }
                    }
                    else if(trans.DueDate.GetValueOrDefault().Date < DateTime.Now.Date)
                    {
                        documentManagementSheet.Cells["I" + (4 + incomingTransCount)].PutValue("X");
                        documentManagementSheet.Cells["J" + (4 + incomingTransCount)].PutValue((trans.DueDate.GetValueOrDefault().Date - DateTime.Now.Date).Days);
                    }
                    incomingTransCount += 1;
                }
                // ----------------------------------------------------------------------------------------------------------

             //   var outgoingTransList = this.pecc2TransmittalService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue), 2, string.Empty).Where(t =>                       (this.txtFromDate.SelectedDate == null || t.IssuedDate >= this.txtFromDate.SelectedDate)                       && (this.txtTodate.SelectedDate == null || t.IssuedDate < this.txtTodate.SelectedDate.GetValueOrDefault().AddDays(1))).ToList();

                dtDocIncomingList.Columns.AddRange(new[]
                {
                    new DataColumn("NoIndex", typeof (String)),
                    new DataColumn("DocNo", typeof (String)),
                    new DataColumn("DocTitle", typeof (String)),
                    new DataColumn("Rev", typeof(String)),
                    new DataColumn("IncomingTrans", typeof(String)),
                    new DataColumn("IssuedDate", typeof(DateTime)),
                    new DataColumn("ResponseDate", typeof(DateTime)),
                    new DataColumn("Deadline", typeof(DateTime)),
                    new DataColumn("Late", typeof(String)),
                    new DataColumn("OutgoingTrans", typeof(String)),
                    new DataColumn("ReviewCode", typeof(String)),  
                });

                //dtDocOutgoingList.Columns.AddRange(new[]
                //{
                //    new DataColumn("NoIndex", typeof (String)),
                //    new DataColumn("DocNo", typeof (String)),
                //    new DataColumn("DocTitle", typeof (String)),
                //    new DataColumn("Rev", typeof(String)),
                //    new DataColumn("ActionCode", typeof(String)),
                //    new DataColumn("ReviewCode", typeof(String)),
                //    new DataColumn("OutgoingTrans", typeof(String)),
                //    new DataColumn("IssuedDate", typeof(String)),
                //});

                var countDocIncomingTrans = 1;
                foreach (var incomingTrans in incomingTransList)
                {
                    var docIncomingTransList = this.pecc2DocumentsService.GetAllByIncomingTrans(incomingTrans.ID);
                    foreach (var docObj in docIncomingTransList)
                    {
                        var dataRow = dtDocIncomingList.NewRow();
                        dataRow["NoIndex"] = countDocIncomingTrans;
                        dataRow["DocNo"] = docObj.DocNo;
                        dataRow["DocTitle"] = docObj.DocTitle;
                        dataRow["Rev"] = docObj.Revision;
                        dataRow["IncomingTrans"] = docObj.IncomingTransNo;
                        dataRow["IssuedDate"] = incomingTrans.IssuedDate.GetValueOrDefault();
                        dataRow["DeadLine"] = incomingTrans.DueDate.GetValueOrDefault();
                        dataRow["ReviewCode"] = docObj.DocReviewStatusCode;
                        if (!string.IsNullOrEmpty(docObj.OutgoingTransNo))
                        {
                            var objtransout = this.pecc2TransmittalService.GetById(docObj.OutgoingTransId.GetValueOrDefault());
                            if (objtransout != null)
                            {
                                dataRow["OutgoingTrans"] = objtransout.TransmittalNo;
                                dataRow["ResponseDate"] = objtransout.IssuedDate.GetValueOrDefault();
                                dataRow["Late"] = (incomingTrans.DueDate.GetValueOrDefault()- objtransout.IssuedDate.GetValueOrDefault()).Days;
                            }
                        }
                        else
                        {
                            dataRow["Late"] = (incomingTrans.DueDate.GetValueOrDefault().Date - DateTime.Now.Date).Days;
                        }
                        countDocIncomingTrans += 1;
                        dtDocIncomingList.Rows.Add(dataRow);
                    }
                }

                documentResponedSheet.Cells.ImportDataTable(dtDocIncomingList, false, 4, 1, dtDocIncomingList.Rows.Count, dtDocIncomingList.Columns.Count, true);
                documentResponedSheet.Cells["D1"].PutValue(this.txtFromDate.SelectedDate);
                documentResponedSheet.Cells["D2"].PutValue(this.txtTodate.SelectedDate);

                //var countDocOutgoingTrans = 1;
                //foreach (var outgoingTrans in outgoingTransList)
                //{
                //    var docOutgoingTransList = this.pecc2DocumentsService.GetAllByOutgoingTrans(outgoingTrans.ID);
                //    foreach (var docObj in docOutgoingTransList)
                //    {
                //        var dataRow = dtDocOutgoingList.NewRow();
                //        dataRow["NoIndex"] = countDocOutgoingTrans;
                //        dataRow["DocNo"] = docObj.DocNo;
                //        dataRow["DocTitle"] = docObj.DocTitle;
                //        dataRow["Rev"] = docObj.Revision;
                //        dataRow["ActionCode"] = docObj.DocActionCode;
                //        dataRow["ReviewCode"] = docObj.DocReviewStatusCode;
                //        dataRow["OutgoingTrans"] = docObj.OutgoingTransNo;
                //        dataRow["IssuedDate"] = outgoingTrans.IssuedDate;

                //        countDocOutgoingTrans += 1;
                //        dtDocOutgoingList.Rows.Add(dataRow);
                //    }
                //}

                //  documentResponedSheet.Cells.ImportDataTable(dtDocOutgoingList, false, 10 + dtDocIncomingList.Rows.Count, 1, dtDocOutgoingList.Rows.Count, dtDocOutgoingList.Columns.Count, true);

                var filename = projectObj.Code +  " Project Monthly Report_" + DateTime.Now.ToString("dd-MM-yy") + ".xlsm";
                workbook.Save(filePath + filename);
                this.DownloadByWriteByte(filePath + filename, filename, true);
            }
        }
    }
}