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

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class AdvanceSearch : Page
    {

        private readonly PECC2DocumentsService pecc2documentService = new PECC2DocumentsService();

        private readonly ProjectCodeService _projectcodeService = new ProjectCodeService();

        private readonly PlantService _plantService = new PlantService();

        private readonly AreaService _areaService = new  AreaService();

        private readonly UnitService _unitService = new UnitService();
        private readonly MaterialService _MaterialService= new MaterialService();
        private readonly WorkService _WorkcodeService= new WorkService();
        private readonly DrawingService _DrawingcodeService = new DrawingService();
        private readonly DocumentCodeServices _DocumnetCodeSErvie= new DocumentCodeServices();
        private readonly DocumentClassService _DocumentClassService= new DocumentClassService();
        private readonly RevisionStatuService _RevisionStatusService = new RevisionStatuService();
        private readonly RevisionSchemaService _RevisionSchemaService = new RevisionSchemaService();
        private readonly ConfidentialityService _ConfidentialityService = new ConfidentialityService();
        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();
        private readonly PECC2TransmittalService _PECC2TransmittalService = new PECC2TransmittalService();
        private readonly AttachDocToTransmittalService _AttachDocToTransmittalService = new AttachDocToTransmittalService();
        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();
        private readonly PlantService _PlantService= new PlantService();
        private readonly AreaService _AreaService = new AreaService();
        private readonly UnitService _UnitService= new UnitService();
        private readonly UserService userService = new UserService();

        private readonly AttachFileService attachFileService = new AttachFileService();

        private readonly AttachFilesPackageService attachFilesPackageService = new AttachFilesPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

        private readonly ProjectCodeService _projectCodeService = new ProjectCodeService();

        private readonly OrganizationCodeService _organizationcodeService = new OrganizationCodeService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

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
            Session.Add("SelectedMainMenu", "Document Management");
            this.Form.DefaultButton = this.btnSearch.UniqueID;

            if (!string.IsNullOrEmpty(this.Request.QueryString["TransOut"]))
            {
                this.txtOutgoingTrans.Text = this.Request.QueryString["TransOut"];
            }

            if (!Page.IsPostBack)
            {
                this.ddlCategory.SelectedIndex = 0;
                this.InitGridColumnView(Convert.ToInt32(this.ddlCategory.SelectedValue));
                this.LoadObjectTree();
                this.LoadComboData();
                Session.Add("IsListAll", false);

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    //this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = false;
                    //this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    //this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }

                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.IsFullPermission.Value = "true";
                }

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
            else if (e.Argument == "ExportEMDRReport")
            {
                ExportCMDRReport();
            }
            else if (e.Argument == "DeleteAllDoc")
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    
                    Guid docId;
                    Guid.TryParse(selectedItem.GetDataKeyValue("ID").ToString(), out docId);
                    var docObj = this.pecc2documentService.GetById(docId);
                    if (docObj != null)
                    {
                        if (docObj.ParentId == null)
                        {
                            docObj.IsDelete = true;
                            this.pecc2documentService.Update(docObj);
                        }
                        else
                        {
                            var listRelateDoc =
                                this.pecc2documentService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                            if (listRelateDoc != null)
                            {
                                foreach (var objDoc in listRelateDoc)
                                {
                                    objDoc.IsDelete = true;
                                    this.pecc2documentService.Update(objDoc);
                                }
                            }
                        }
                    }
                }
                this.grdDocument.Rebind();
            }
           
            else if (e.Argument == "ExportMasterList")
            {
                var filePath = Server.MapPath("Exports") + @"\";
                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\PECC2_ProjectDocListTemplate.xls");
                var sheets = workbook.Worksheets;
                var tempSheet = sheets[0];
                var readMeSheet = sheets[1];

             //   var projectId = this.ddlProject.SelectedItem != null ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0;

                var projectcodelist = this._projectcodeService.GetAll();
                var RevisionStatustlist = this._RevisionStatusService.GetAll();
                var documentcodelist = this._DocumnetCodeSErvie.GetAll();
                var documentclasslist = this._DocumentClassService.GetAll();
                var Confdentialitylist = this._ConfidentialityService.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault());
                var RevisionSchemaList = this._RevisionSchemaService.GetAll();

                for (int i = 0; i < projectcodelist.Count; i++)
                {
                    readMeSheet.Cells["B" + (7 + i)].PutValue(projectcodelist[i].Code);
                    readMeSheet.Cells["C" + (7 + i)].PutValue(projectcodelist[i].Description);
                }

               
                for (int i = 0; i < documentclasslist.Count; i++)
                {
                    readMeSheet.Cells["E" + (7 + i)].PutValue(documentclasslist[i].Code);
                    readMeSheet.Cells["F" + (7 + i)].PutValue(documentclasslist[i].Description);
                }
                for (int i = 0; i < documentcodelist.Count; i++)
                {
                    readMeSheet.Cells["H" + (7 + i)].PutValue(documentcodelist[i].Code);
                    readMeSheet.Cells["I" + (7 + i)].PutValue(documentcodelist[i].Description);
                }
                for (int i = 0; i < RevisionStatustlist.Count; i++)
                {
                    readMeSheet.Cells["K" + (7 + i)].PutValue(RevisionStatustlist[i].Code);
                    readMeSheet.Cells["L" + (7 + i)].PutValue(RevisionStatustlist[i].Description);
                }
                for (int i = 0; i < RevisionSchemaList.Count; i++)
                {
                    readMeSheet.Cells["N" + (7 + i)].PutValue(RevisionSchemaList[i].Code);
                    readMeSheet.Cells["O" + (7 + i)].PutValue(RevisionSchemaList[i].Description);
                }
                for (int i = 0; i < Confdentialitylist.Count; i++)
                {
                    readMeSheet.Cells["Q" + (7 + i)].PutValue(Confdentialitylist[i].Code);
                    readMeSheet.Cells["R" + (7 + i)].PutValue(Confdentialitylist[i].Description);
                }
                //"I=8"
                var rangeProjectCodeList = readMeSheet.Cells.CreateRange("B7", "B" + (7 + (projectcodelist.Count == 0 ? 1 : projectcodelist.Count)));
                rangeProjectCodeList.Name = "ProjectCode";
                var rangDocumentClassList = readMeSheet.Cells.CreateRange("E7", "E" + (7 + (documentclasslist.Count == 0 ? 1 : documentclasslist.Count)));
                rangDocumentClassList.Name = "DocumentClass";
                var rangDocumentCodeList = readMeSheet.Cells.CreateRange("H7", "H" + (7 + (documentcodelist.Count == 0 ? 1 : documentcodelist.Count)));
                rangDocumentCodeList.Name = "DocumentCode";
                var rangRevisionStatusList = readMeSheet.Cells.CreateRange("K7", "K" + (7 + (RevisionStatustlist.Count == 0 ? 1 : RevisionStatustlist.Count)));
                rangRevisionStatusList.Name = "RevisionStatus";
                var rangRevisionSchemaList = readMeSheet.Cells.CreateRange("N7", "N" + (7 + (RevisionSchemaList.Count == 0 ? 1 : RevisionSchemaList.Count)));
                  rangRevisionSchemaList.Name = "RevisionSchema";
                var rangConfdentialitylist = readMeSheet.Cells.CreateRange("Q7", "Q" + (7 + (Confdentialitylist.Count == 0 ? 1 : Confdentialitylist.Count)));
                rangConfdentialitylist.Name = "ConfdentialityCode";
                

                var validations = sheets[2].Validations;
                this.CreateValidation(rangeProjectCodeList.Name, validations, 2, 1000, 2, 2);
                this.CreateValidation(rangRevisionSchemaList.Name, validations, 2, 1000, 8, 8);
                this.CreateValidation(rangRevisionStatusList.Name, validations, 2, 1000, 9, 9);
                this.CreateValidation(rangDocumentClassList.Name, validations, 2, 1000, 10, 10);
                this.CreateValidation(rangDocumentCodeList.Name, validations, 2, 1000, 11, 11);
                this.CreateValidation(rangConfdentialitylist.Name, validations, 2, 1000, 12, 12);



                workbook.Worksheets[0].IsVisible = false;
                    //workbook.Worksheets.RemoveAt(2);

                    var filename = DateTime.Now.ToString("ddmmyyyy")+"_" + "$" + "ProjectDocumentListTemplate.xls";
                    workbook.Save(filePath + filename);
                    this.DownloadByWriteByte(filePath + filename, filename, true);

                
            }
            else if (e.Argument.Contains("DeleteRev"))
            {
                string st = e.Argument.ToString();
                int docId = Convert.ToInt32(st.Replace("DeleteRev_", string.Empty));

                //var docObj = this.documentPackageService.GetById(docId);
                //var listRelateDoc =
                //    this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                //if (docObj != null && listRelateDoc.Count > 1)
                //{

                //    docObj.IsDelete = true;
                //    docObj.IsLeaf = false;
                //    this.documentPackageService.Update(docObj);
                //    docId = 0;
                //    listRelateDoc =
                //        this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                //    if (listRelateDoc != null)
                //    {
                //        foreach (var objDoc in listRelateDoc)
                //        {
                //            if (docId < objDoc.ID)
                //            {
                //                docId = objDoc.ID;
                //                docObj = objDoc;
                //            }
                //        }
                //    }
                //    if (docId != 0)
                //    {
                //        docObj.IsLeaf = true;
                //        this.documentPackageService.Update(docObj);
                //        this.grdDocument.Rebind();
                //    }
                //}
                //else
                //{
                //    Response.Write(
                //        "<script>window.alert('Can not be reduced, because this document is only one version.')</script>");
                //}
            }
            else if (e.Argument == "DownloadMulti")
            {
                //var serverTotalDocPackPath =
                //    Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_DocPack.rar");
                //var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);

                //foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                //{
                //    var cboxSelected = (CheckBox) item["IsSelected"].FindControl("IsSelected");
                //    if (cboxSelected.Checked)
                //    {
                //        var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                //        var name = (Label) item["Index1"].FindControl("lblName");
                //        var serverDocPackPath =
                //            Server.MapPath("~/Exports/DocPack/" + name.Text + "_" +
                //                           DateTime.Now.ToString("ddMMyyyhhmmss") + ".rar");

                //        var attachFiles = this.attachFileService.GetAllByDocId(docId);

                //        var temp = ZipPackage.CreateFile(serverDocPackPath);

                //        foreach (var attachFile in attachFiles)
                //        {
                //            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                //            {
                //                temp.Add(Server.MapPath(attachFile.FilePath));
                //            }
                //        }

                //        docPack.Add(serverDocPackPath);
                //    }
                //}

                //this.Download_File(filePath + filename);

            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "SendNotification")
            {
                var listDisciplineId = new List<int>();
                var listSelectedDoc = new List<Document>();
                var count = 0;
                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox)item["IsSelected"].FindControl("IsSelected");
                    if (cboxSelected.Checked)
                    {
                        count += 1;
                        var docItem = new Document();
                        var disciplineId = item["DisciplineID"].Text != @"&nbsp;"
                            ? item["DisciplineID"].Text
                            : string.Empty;
                        if (!string.IsNullOrEmpty(disciplineId) && disciplineId != "0")
                        {
                            listDisciplineId.Add(Convert.ToInt32(disciplineId));

                            docItem.ID = count;
                            docItem.DocumentNumber = item["DocumentNumber"].Text != @"&nbsp;"
                                ? item["DocumentNumber"].Text
                                : string.Empty;
                            docItem.Title = item["Title"].Text != @"&nbsp;"
                                ? item["Title"].Text
                                : string.Empty;
                            docItem.RevisionName = item["Revision"].Text != @"&nbsp;"
                                ? item["Revision"].Text
                                : string.Empty;
                            docItem.FilePath = item["FilePath"].Text != @"&nbsp;"
                                ? item["FilePath"].Text
                                : string.Empty;
                            docItem.DisciplineID = Convert.ToInt32(disciplineId);
                            listSelectedDoc.Add(docItem);
                        }
                    }
                }

                listDisciplineId = listDisciplineId.Distinct().ToList();

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials =
                        new NetworkCredential(UserSession.Current.User.Email,
                            Utility.Decrypt(UserSession.Current.User.HashCode))
                };

                foreach (var disciplineId in listDisciplineId)
                {
                    var notificationRule = this.notificationRuleService.GetAllByDiscipline(disciplineId);

                    if (notificationRule != null)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(UserSession.Current.User.Email, UserSession.Current.User.FullName);
                        message.Subject = "Test send notification from EDMs";
                        message.BodyEncoding = new UTF8Encoding();
                        message.IsBodyHtml = true;
                        message.Body = @"******<br/>
                                        Dear users,<br/><br/>

                                        Please be informed that the following documents are now available on the BDPOC Document Library System for your information.<br/><br/>

                                        <table border='1' cellspacing='0'>
	                                        <tr>
		                                        <th style='text-align:center; width:40px'>No.</th>
		                                        <th style='text-align:center; width:350px'>Document number</th>
		                                        <th style='text-align:center; width:350px'>Document title</th>
		                                        <th style='text-align:center; width:60px'>Revision</th>
	                                        </tr>";

                        if (!string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listUserId =
                                notificationRule.ReceiverListId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            foreach (var userId in listUserId)
                            {
                                var user = this.userService.GetByID(userId);
                                if (user != null)
                                {
                                    message.To.Add(new MailAddress(user.Email));
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(notificationRule.ReceiveGroupId) &&
                                 string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listGroupId =
                                notificationRule.ReceiveGroupId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            var listUser = this.userService.GetSpecialListUser(listGroupId);
                            foreach (var user in listUser)
                            {
                                message.To.Add(new MailAddress(user.Email));
                            }
                        }

                        var subBody = string.Empty;
                        foreach (var document in listSelectedDoc)
                        {
                            var port = ConfigurationSettings.AppSettings.Get("DocLibPort");
                            if (document.DisciplineID == disciplineId)
                            {
                                subBody += @"<tr>
                                <td>" + document.ID + @"</td>
                                <td><a href='http://" + Server.MachineName +
                                           (!string.IsNullOrEmpty(port) ? ":" + port : string.Empty)
                                           + document.FilePath + "' download='" + document.DocumentNumber + "'>"
                                           + document.DocumentNumber + @"</a></td>
                                <td>"
                                           + document.Title + @"</td>
                                <td>"
                                           + document.RevisionName + @"</td>";
                            }
                        }


                        message.Body += subBody + @"</table>
                                        <br/><br/>
                                        Thanks and regards,<br/>
                                        ******";

                        smtpClient.Send(message);
                    }
                }
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
            var docList = this.pecc2documentService.GetAllLatest();
            var searchAllText = this.txtSearchAllText.Text.Trim();
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            var incomingTrans = this.txtIncomingTrans.Text.Trim();
            var outgoingTrans = this.txtOutgoingTrans.Text.Trim();
            var docNo = this.txtDocNo.Text.Trim();
            var docTitle = this.txtDocTitle.Text.Trim();
            var revStatus = this.ddlRevisionStatus.SelectedValue;
            var area = this.ddlArea.SelectedValue;
            var unit = this.ddlUnit.SelectedValue;
            var docType = this.ddlDocType.SelectedValue;//this.ddlDocType.SelectedItem != null ? this.ddlDocType.SelectedItem.Text : string.Empty;
            var discipline = this.ddlDiscipline.SelectedValue;//this.ddlDiscipline.SelectedItem != null ? this.ddlDiscipline.SelectedItem.Text : string.Empty;
            var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            docList = docList.Where(t => (projectId == 0 || t.ProjectId == projectId) && t.CategoryId == categoryId
                            && (string.IsNullOrEmpty(incomingTrans) || (!string.IsNullOrEmpty(t.IncomingTransNo) && t.IncomingTransNo.ToLower().Contains(incomingTrans.ToLower())))
                            && (string.IsNullOrEmpty(outgoingTrans) || (!string.IsNullOrEmpty(t.OutgoingTransNo) && t.OutgoingTransNo.ToLower().Contains(outgoingTrans.ToLower())))
                            && (string.IsNullOrEmpty(docNo) || (!string.IsNullOrEmpty(t.DocNo) && t.DocNo.ToLower().Contains(docNo.ToLower())))
                            && (string.IsNullOrEmpty(docTitle) || (!string.IsNullOrEmpty(t.DocTitle) && t.DocTitle.ToLower().Contains(docTitle.ToLower())))
                            && (t.RevStatusId ==  null|| revStatus == "" || (t.RevStatusId == Convert.ToInt32(revStatus)))
                            
                            && (t.AreaId == null || area == "0" || (t.AreaId == Convert.ToInt32(area)))
                            && (t.UnitId == null || unit == "0" || (t.UnitId == Convert.ToInt32(unit)))
                            && (t.DocTypeId == null || docType == "0" || (t.DocTypeId == Convert.ToInt32(docType)))
                            && (t.DisciplineId == null || discipline == "0" || (t.DisciplineId == Convert.ToInt32(discipline)))

                            && (string.IsNullOrEmpty(searchAllText) 
                                || (!string.IsNullOrEmpty(t.DocNo) && t.DocNo.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.DocTitle) && t.DocTitle.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.OriginatingOrganisationName) && t.OriginatingOrganisationName.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.ReceivingOrganisationName) && t.ReceivingOrganisationName.ToLower().Contains(searchAllText.ToLower()))
                                
                                || (!string.IsNullOrEmpty(t.DocTypeCode) && t.DocTypeCode.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.DisciplineCode) && t.DisciplineCode.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.SheetNo) && t.SheetNo.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.AreaCode) && t.AreaCode.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.ProjectName) && t.ProjectName.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.OriginalDocumentNumber) && t.OriginalDocumentNumber.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.RevisionSchemaName) && t.RevisionSchemaName.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.Revision) && t.Revision.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.RevStatusName) && t.RevStatusName.ToLower().Contains(searchAllText.ToLower()))
                                
                                || (!string.IsNullOrEmpty(t.Remarks) && t.Remarks.ToLower().Contains(searchAllText.ToLower()))
                                || (!string.IsNullOrEmpty(t.ConfidentialityName) && t.ConfidentialityName.ToLower().Contains(searchAllText.ToLower()))
                            )
                ).ToList();

            this.grdDocument.DataSource = docList.OrderByDescending(t => t.DocNo);
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
            var docObj = this.pecc2documentService.GetById(docId);
            if (docObj != null)
            {
                if (docObj.ParentId == null)
                {
                    docObj.IsDelete = true;
                    this.pecc2documentService.Update(docObj);
                }
                else
                {
                    var listRelateDoc = this.pecc2documentService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                    if (listRelateDoc != null)
                    {
                        foreach (var objDoc in listRelateDoc)
                        {
                            objDoc.IsDelete = true;
                            this.pecc2documentService.Update(objDoc);
                        }
                    }
                }
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
        private void LoadComboData()
        {
            var RevisionStatustlist = this._RevisionStatusService.GetAll();
            RevisionStatustlist.Insert(0, new RevisionStatu() { ID = 0, Code = string.Empty });
            this.ddlRevisionStatus.DataSource = RevisionStatustlist;
            this.ddlRevisionStatus.DataTextField = "Code";
            this.ddlRevisionStatus.DataValueField = "ID";
            this.ddlRevisionStatus.DataBind();

            //document code
            var documentcodelist = this._DocumnetCodeSErvie.GetAll();
            documentcodelist.Insert(0, new DocumentCode() { ID = 0, Code = string.Empty });
            this.ddlDocCode.DataSource = documentcodelist;
            this.ddlDocCode.DataTextField = "Code";
            this.ddlDocCode.DataValueField = "ID";
            this.ddlDocCode.DataBind();


            //add document type
            var documentTypeList = this.documentTypeService.GetAll() ;
            documentTypeList.Insert(0, new DocumentType() { ID = 0, Code = string.Empty });
            this.ddlDocType.DataSource = documentTypeList.OrderBy(t=> t.FullName);
            this.ddlDocType.DataTextField = "FullName";
            this.ddlDocType.DataValueField = "ID";
            this.ddlDocType.DataBind();
            //add discipline
            var disciplinelist = this.disciplineService.GetAll();
            disciplinelist.Insert(0, new Discipline() { ID = 0, Code = string.Empty });
            this.ddlDiscipline.DataSource = disciplinelist.OrderBy(t => t.FullName);
            this.ddlDiscipline.DataTextField = "FullName";
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataBind();

            //add Plant
            var plantList = this._PlantService.GetAll();
            plantList.Insert(0, new Plant() { ID = 0, Code = string.Empty });
            this.ddlPlant.DataSource = plantList.OrderBy(t => t.Name);
            this.ddlPlant.DataTextField = "Name";
            this.ddlPlant.DataValueField = "ID";
            this.ddlPlant.DataBind();
            //add Area
            var areaList = this._AreaService.GetAll();
            areaList.Insert(0, new Area() { ID = 0, Code = string.Empty });
            this.ddlArea.DataSource = areaList.OrderBy(t => t.Code);
            this.ddlArea.DataTextField = "Code";
            this.ddlArea.DataValueField = "ID";
            this.ddlArea.DataBind();
            //add Area
            var unitList = this._UnitService.GetAll();
            unitList.Insert(0, new Data.Entities.Unit() { ID = 0, Code = string.Empty });
            this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
            this.ddlUnit.DataTextField = "Code";
            this.ddlUnit.DataValueField = "ID";
            this.ddlUnit.DataBind();
            //add MaterialCode
            var materialcodeList = this._MaterialService.GetAll();
            materialcodeList.Insert(0, new MaterialCode() { ID = 0, Code = string.Empty });
            this.ddlMaterialCode.DataSource = materialcodeList.OrderBy(t => t.FullName);
            this.ddlMaterialCode.DataTextField = "FullName";
            this.ddlMaterialCode.DataValueField = "ID";
            this.ddlMaterialCode.DataBind();


            //add WorkCode
            var workcodeList = this._WorkcodeService.GetAll();
            workcodeList.Insert(0, new WorkCode() { ID = 0, Code = string.Empty });
            this.ddlWorkCode.DataSource = workcodeList.OrderBy(t => t.FullName);
            this.ddlWorkCode.DataTextField = "FullName";
            this.ddlWorkCode.DataValueField = "ID";
            this.ddlWorkCode.DataBind();

            //add Drawing
            var drawingcodelist = this._DrawingcodeService.GetAll();
            drawingcodelist.Insert(0, new DrawingCode() { ID = 0, Code = string.Empty });
            this.ddlDrawingCode.DataSource = drawingcodelist.OrderBy(t => t.FullName);
            this.ddlDrawingCode.DataTextField = "FullName";
            this.ddlDrawingCode.DataValueField = "ID";
            this.ddlDrawingCode.DataBind();

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

        //private void PopulateNodeOnDemand(RadTreeNodeEventArgs e, TreeNodeExpandMode expandMode)
        //{
        //    var categoryId = this.lblCategoryId.Value;
        //    var folderPermission =
        //        this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
        //            t => t.CategoryIdList == categoryId).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();

        //    var listFolChild = this.folderService.GetAllByParentId(Convert.ToInt32(e.Node.Value), folderPermission);
        //    foreach (var folderChild in listFolChild)
        //    {
        //        var nodeFolder = new RadTreeNode();
        //        nodeFolder.Text = folderChild.Name;
        //        nodeFolder.Value = folderChild.ID.ToString();
        //        nodeFolder.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
        //        nodeFolder.ImageUrl = "Images/folderdir16.png";
        //        e.Node.Nodes.Add(nodeFolder);
        //    }

        //    e.Node.Expanded = true;
        //}

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
            // add eoject code
            var projectcode = this._projectcodeService.GetAll().OrderBy(t=> t.Code).ToList();
            projectcode.Insert(0, new ProjectCode() {ID = 0, Code = "All Project"});
            this.ddlProject.DataSource = projectcode;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
            this.ddlProject.SelectedIndex = 0;

            if (this.ddlProject.SelectedItem != null)
            {
                int projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();

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

        protected void grdDocument_DataBound(object sender, EventArgs e)
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


        //private void ExportCMDRDataFile()
        //{
        //    var filePath = Server.MapPath("Exports") + @"\";
        //    var workbook = new Workbook();
        //    workbook.Open(filePath + @"Template\DSR_CMDRDataFileTemplate.xlsm");

        //    var dataSheet = workbook.Worksheets[2];
        //    var duplicateSheet = workbook.Worksheets[3];
        //    var tempSheet = workbook.Worksheets[0];
        //    var countCol = 11;
        //    var totalColAdded = 0;
        //    var dtFull = new DataTable();
        //    var filename ="CMDR Data File_Cut off - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsm";
            
        //   // dataSheet.Cells["E1"].PutValue(this.ddlProject.Text);
        //    dataSheet.Cells["X4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

        //    dtFull.Columns.AddRange(new[]
        //    {
        //        new DataColumn("DocId", typeof (String)),
        //        new DataColumn("NoIndex", typeof (String)),
        //        new DataColumn("System", typeof (String)),
        //        new DataColumn("DocNo", typeof (String)),
        //        new DataColumn("DocTitle", typeof (String)),
        //        new DataColumn("RevName", typeof (String)),
        //        new DataColumn("IssueDate", typeof (String)),
        //        new DataColumn("Contractor", typeof (String)),
        //        new DataColumn("EquipmentTag", typeof(String)),
        //        new DataColumn("DepartmentCode", typeof(String)),
        //        new DataColumn("MRSequenceNo", typeof(String)),
        //        new DataColumn("DocSequenceNo", typeof(String)),
        //        new DataColumn("SheetNo", typeof(String)),
        //        new DataColumn("Originator", typeof (String)),
        //        new DataColumn("Originatoring", typeof (String)),
        //        new DataColumn("Receiving", typeof (String)),
        //        new DataColumn("DocTypeCode", typeof (String)),
        //        new DataColumn("DisciplineCode", typeof (String)),
        //        new DataColumn("Material", typeof (String)),
        //        new DataColumn("Work", typeof (String)),
        //        new DataColumn("Drawing", typeof (String)),
        //        new DataColumn("Plant", typeof (String)),
        //        new DataColumn("Area", typeof (String)),
        //        new DataColumn("Unit", typeof (String)),
        //        new DataColumn("ProjectCode", typeof (String)),
        //        new DataColumn("Schema", typeof (String)),
        //        new DataColumn("RevStatus", typeof (String)),
        //        new DataColumn("DocClass", typeof (String)),
        //        new DataColumn("DocCode", typeof (String)),
        //        new DataColumn("Confidentiality", typeof (String)),
        //        new DataColumn("Remark", typeof (String)),
        //    });

        //    List<Guid> ListId = new List<Guid>();

        //    this.grdDocument.AllowPaging = false;
        //    this.grdDocument.Rebind();
        //    foreach (GridDataItem row in this.grdDocument.Items) // loops through each rows in RadGrid
        //    {
        //        Guid docId;
        //        Guid.TryParse(row.GetDataKeyValue("ID").ToString(), out docId);
        //        ListId.Add(docId);
        //    }
        //    this.grdDocument.AllowPaging = true;
        //    this.grdDocument.Rebind();

        //    var disciplineRowCount = 1;
        //    var docList = this._dqredocumentService.GetAllDocList(ListId);
        //    var docGroupByDisciplineList = docList.GroupBy(t => t.ProjectCodeName);
        //    foreach (var docGroupByDiscipline in docGroupByDisciplineList)
        //    {
              
        //        var docListOfDiscipline = docGroupByDiscipline.ToList();
        //        var dataRow = dtFull.NewRow();
        //        dataRow["System"] = docGroupByDiscipline.Key;
        //        dtFull.Rows.Add(dataRow);


        //        foreach (var docObj in docListOfDiscipline)
        //        {
        //            dataRow = dtFull.NewRow();

        //            dataRow["DocId"] = docObj.ID;
        //            dataRow["NoIndex"] = disciplineRowCount;
        //            dataRow["System"] = docObj.M_SystemDocumentNo;
        //            dataRow["DocNo"] = docObj.DocumentNo;
        //            dataRow["DocTitle"] = docObj.DocumentTitle;
              
        //        dataRow["RevName"] = docObj.Revision ;
        //        dataRow["IssueDate"] = docObj.IsssuedDate != null? docObj.IsssuedDate.Value.ToString("dd/MM/yyyy"):string.Empty ;
        //        dataRow["Contractor"] = docObj.ContractorDocNo ;
        //        dataRow["EquipmentTag"] = docObj.M_EquipmentTagName ;
        //        dataRow["DepartmentCode"] = docObj.M_DepartmentCode ;
        //        dataRow["MRSequenceNo"] = docObj.M_MRSequenceNo ;
        //        dataRow["DocSequenceNo"] = docObj.M_DocumentSequenceNo ;
        //        dataRow["SheetNo"] = docObj.M_SheetNo ;
        //        dataRow["Originator"] = docObj.M_OriginatorName ;
        //        dataRow["Originatoring"] = docObj.M_OriginatingOrganizationName ;
        //        dataRow["Receiving"] = docObj.M_ReceivingOrganizationName ;
        //        dataRow["DocTypeCode"] = docObj.M_DocumentTypeName ;
        //        dataRow["DisciplineCode"] = docObj.M_DisciplineName ;
        //        dataRow["Material"] = docObj.M_MaterialCodeName ;
        //        dataRow["Work"] = docObj.M_WorkCodeName ;
        //        dataRow["Drawing"] = docObj.M_DrawingCodeName ;
        //        dataRow["Plant"] = docObj.M_PlantName ;
        //        dataRow["Area"] = docObj.M_AreaName ;
        //        dataRow["Unit"] = docObj.M_UnitName ;
        //        dataRow["ProjectCode"] = docObj.ProjectCodeName ;
        //        dataRow["Schema"] = docObj.RevisionSchemaName ;
        //        dataRow["RevStatus"] = docObj.RevisionStatusName ;
        //        dataRow["DocClass"] = docObj.DocumentClassName ;
        //        dataRow["DocCode"] = docObj.DocumentCodeName ;
        //        dataRow["Confidentiality"] = docObj.ConfidentialityName ;
        //        dataRow["Remark"] = docObj.Remark ;


        //            disciplineRowCount += 1;
        //            dtFull.Rows.Add(dataRow);
        //        }
        //    }
        //    dataSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
        //    duplicateSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

        //    var validations = dataSheet.Validations;
            
        //    dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);
        //   // dataSheet.Cells["A1"].PutValue(projectId);


        //    workbook.Save(filePath + filename);
        //    this.DownloadByWriteByte(filePath + filename,filename,true);

        //}

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

        private void ExportCMDRReport()
        {
            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\PECC2_CMDRDataFileTemplate.xlsm");

            var dataSheet = workbook.Worksheets[1];

            var tempSheet = workbook.Worksheets[0];

            var projectName = this.ddlProject.SelectedItem != null
                ? this.ddlProject.SelectedItem.Text.Replace(",", "-")
                : string.Empty;
            var dtFull = new DataTable();
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            var projectObj = this._projectCodeService.GetById(projectId);

            var filename = projectName + "_" + "CMDR Data File_Cut off - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsm";
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
            var docList = this.pecc2documentService.GetByProjectCode(projectId, UserSession.Current.User.ConfidentialId.GetValueOrDefault(),true).Where(t=> ListId.Contains(t.ID));

            var ListAttchDoc = this._AttachDocToTransmittalService.GetAll().Where(t => docList.Select(o => o.ID).ToList().Contains(t.DocumentId.GetValueOrDefault())).ToList();


            var ListTransIn = (from A in this._PECC2TransmittalService.GetAll().Where(t => t.TypeId == 2)
                              join B in ListAttchDoc on A.ID equals B.TransmittalId
                              select new
                              {
                                  A.ID,
                                  A.TransmittalNo,
                                  A.IssuedDate,
                                  A.ReceivedDate,
                                  A.DueDate,
                                  B.DocumentId
                              }).ToList();


            var ListTransOut =( from A in this._PECC2TransmittalService.GetAll().Where(t => t.TypeId == 1)
                               join B in ListAttchDoc on A.ID equals B.TransmittalId
                              select new
                              {
                                  A.ID,
                                  A.TransmittalNo,
                                  A.IssuedDate,
                                  A.ReceivedDate,
                                  A.DueDate,
                                  B.DocumentId
                              }).ToList();



            var tamplate = docList.Select(t =>
                            new
                            {
                                t.ID,
                                t.DocNo,
                                t.DocTitle,
                                t.OriginalDocumentNumber,
                                t.DocTypeCode,
                                t.DisciplineCode,
                                t.UnitCode,
                                t.Revision,
                                t.Date,
                                t.IsLeaf,
                                t.Remarks,
                                t.RevStatusName,
                                TransInNo = ListTransIn.Find(k => k.DocumentId==t.ID)?.TransmittalNo,
                                TransInDate= ListTransIn.Find(k=>k.DocumentId==t.ID)?.ReceivedDate,
                                TransInDueDate= ListTransIn.Find(k => k.DocumentId == t.ID)?.DueDate,
                                TransOutNo=ListTransOut.Find(o=> o.DocumentId==t.ID)?.TransmittalNo,
                                TransOutDate= ListTransOut.Find(o => o.DocumentId == t.ID)?.IssuedDate,
                            });
            dataSheet.Cells["E2"].PutValue(this.ddlProject.Text);
            dataSheet.Cells["I2"].PutValue(DateTime.Now.ToString("dd-mmm-yy"));

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("DocNo", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("ContractorDoc", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
                new DataColumn("Unit", typeof (String)),
                new DataColumn("Revision", typeof (String)),
                new DataColumn("IssueDate", typeof (String)),
                new DataColumn("Status", typeof (String)),
                new DataColumn("State", typeof (String)),
                new DataColumn("LasteRev", typeof (String)),
                new DataColumn("TransInNo", typeof (String)),
                new DataColumn("TransInDate", typeof (String)),
                new DataColumn("TransInDueDate", typeof (String)),
                new DataColumn("TransOutNo", typeof (String)),
                new DataColumn("TransOutDate", typeof (String)),
                new DataColumn("DocumentCode", typeof (String)),
                new DataColumn("Remark", typeof (String)),
            });


            var docGroupByDisciplineList = tamplate.GroupBy(t => t.DisciplineCode);
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
                    dataRow["DocId"] = docObj.ID;
                    dataRow["NoIndex"] = disciplineRowCount;
                    dataRow["DocNo"] = docObj.DocNo;
                    dataRow["DocTitle"] = docObj.DocTitle;
                    dataRow["ContractorDoc"] = docObj.OriginalDocumentNumber;
                    dataRow["DocType"] = docObj.DocTypeCode;
                    dataRow["Discipline"] = docObj.DisciplineCode;
                    dataRow["Unit"] = docObj.UnitCode;
                    dataRow["Revision"] = docObj.Revision;
                    dataRow["IssueDate"] = docObj.Date != null ? docObj.Date.Value.ToString("dd/MM/yyyy") : string.Empty;
                    dataRow["Status"] = docObj.RevStatusName;
                    dataRow["State"] = string.Empty;
                    dataRow["LasteRev"] = docObj.IsLeaf.GetValueOrDefault() ? "Y" : "N";
                    dataRow["TransInNo"] = docObj.TransInNo;
                    dataRow["TransInDate"] = docObj.TransInDate != null ? docObj.TransInDate.Value.ToString("dd/MM/yyyy") : string.Empty; ;
                    dataRow["TransInDueDate"] = docObj.TransInDueDate != null ? docObj.TransInDueDate.Value.ToString("dd/MM/yyyy") : string.Empty; ;
                    dataRow["TransOutNo"] = docObj.TransOutNo;
                    dataRow["TransOutDate"] = docObj.TransOutDate != null ? docObj.TransOutDate.Value.ToString("dd/MM/yyyy") : string.Empty; ;
                    dataRow["Remark"] = docObj.Remarks;

                    disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
            }
            dataSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
            var validations = dataSheet.Validations;
            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count + 7);
            dataSheet.Cells["A1"].PutValue(projectId);


            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);

        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        //protected void ddlRevisionStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    this.grdDocument.Rebind();
        //}

        private void InitGridColumnView(int categoryId)
        {
            switch (categoryId)
            {
                case 3:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Year").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
                case 4:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Year").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
                case 1:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Year").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
                case 2:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("Year").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
            }
        }

        protected void ddlCategory_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            
        }

    }
}