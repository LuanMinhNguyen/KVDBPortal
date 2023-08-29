// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// ----------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace EDMs.Web.Controls.Document
{   
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.IO;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;
   // using Model.ReadWrite.Telerik;
    using Aspose.Cells;
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class NCRSIEditForm : Page
    {
        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly ProjectCodeService projectcodeService;
        private readonly GroupCodeService groupCodeService;
        private readonly ChangeRequestService changeRequestService;

        private readonly NCR_SIService ncrSiService;
        private readonly NCR_SIAttachFileService ncrSiAttachFileService;

        private readonly NCR_SIAddPictureService ncrsiAddPictureService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public NCRSIEditForm()
        {
            this.groupCodeService = new GroupCodeService();
            this.userService = new UserService();
            this.projectcodeService = new ProjectCodeService();
            this.changeRequestService = new ChangeRequestService();
            this.ncrSiService = new NCR_SIService();
            this.ncrSiAttachFileService = new NCR_SIAttachFileService();
            this.ncrsiAddPictureService = new NCR_SIAddPictureService();
        }

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
            if (!this.IsPostBack)
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projectObj = this.projectcodeService.GetById(projectId);
                this.LoadComboData(projectObj);
                bool create = false;
                bool showliTaken = false;
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid objId;
                    Guid.TryParse(this.Request.QueryString["objId"],out objId);
                    this.lbNCRSIId.Value = objId.ToString();
                    var obj = this.ncrSiService.GetById(objId);
                    if (obj != null)
                    {
                        Session.Add("NCRSIID", objId);
                        this.LoadDocInfo(obj, projectObj);
                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());
                        create = UserSession.Current.User.Id == obj.CreatedBy;
                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (obj.LastUpdatedBy != null && obj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(obj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + obj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }

                        this.ddlStatus.Enabled = !(this.Request.QueryString["fromToDoList"] == "true");
                        this.txtClosedDate.Enabled =!(this.Request.QueryString["fromToDoList"] == "true");
                        this.liCancel.Visible = UserSession.Current.User.IsAdmin.GetValueOrDefault();
                        this.cbIsCancel.Enabled = !obj.IsCancel.GetValueOrDefault();
                        if (obj.IsCancel.GetValueOrDefault())
                        {
                            this.RadGrid1.Enabled = false;
                            this.grActiontaken.Enabled = false;
                            this.btnSave.Visible = false;
                        }
                        if (!string.IsNullOrEmpty(obj.ActionTake))
                        {
                            showliTaken = true;
                        }
                    }
                }
                else
                {
                    this.ddlStatus.Enabled = false;
                    this.txtClosedDate.DatePopupButton.Visible = false;
                    this.txtClosedDate.EnableTyping = false;
                    Session.Add("NCRSIID", Guid.NewGuid());
                    var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                    this.CreatedInfo.Visible = false;
                    var sequence = Utilities.Utility.ReturnSequenceString(this.ncrSiService.GetNCRSISequence(Convert.ToInt32(this.ddlType.SelectedValue),Convert.ToInt32(this.ddlGroup.SelectedValue) ,year), 4);
                    this.RegenerateNo(sequence, projectObj, year);                  
                }
                
                this.grdDocument.Visible = this.Request.QueryString["fromToDoList"] == "true";
                this.dlAddtachfile.Visible= this.Request.QueryString["fromToDoList"] == "true";
                var Flag = UserSession.Current.User.IsDC.GetValueOrDefault() || UserSession.Current.User.IsAdmin.GetValueOrDefault() ||UserSession.Current.User.IsLeader.GetValueOrDefault() || (create==true);
                 VisibaleProperti(Flag? false:this.Request.QueryString["fromToDoList"] == "true");
               if ((this.Request.QueryString["fromToDoList"] == "true") && !UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
                {
                   //this.LiContractor.Visible = true;
                    this.txtActionTaken.Enabled = true;
                    this.grActiontaken.Enabled = true;
                    VisibaleProperti(true);
                }
                this.liActionTaken.Visible = this.Request.QueryString["fromToDoList"] == "true" || showliTaken;
                this.LiContractor.Visible = this.Request.QueryString["fromToDoList"] == "true";
            }
        }
        private void VisibaleProperti(bool flag)
        {
          //  this.txtNumber.ReadOnly = flag;
            this.txtSubject.ReadOnly = flag;
            this.txtReference.ReadOnly = flag;
           // this.txtProjectCode.ReadOnly = flag;
            this.txtNote.ReadOnly = flag;
            this.txtSequentialNumber.ReadOnly = flag;
          //  this.txtYear.ReadOnly = flag;
            this.txtDescription.ReadOnly = flag;
            this.txtIssueDate.Enabled = !flag;
            this.RadGrid1.Enabled = !flag;
            this.ddlGroup.Enabled = !flag;
            this.ddlType.Enabled = !flag;
            if(flag && (UserSession.Current.User.IsDC.GetValueOrDefault() || UserSession.Current.User.IsAdmin.GetValueOrDefault()) || UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            {
                this.btnSave.Visible = true;
            }
            else if(flag && !UserSession.Current.User.IsDC.GetValueOrDefault() && !UserSession.Current.User.IsAdmin.GetValueOrDefault())
            {
                this.btnSave.Visible =false ;
            }
            else if(flag)
            {
                this.btnSave.Visible = UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
            }      
        }
        private void RegenerateNo(string sequence, ProjectCode projectObj, int year)
        {
            switch (this.ddlType.SelectedValue)
            {
                case "1":
                    this.txtNumber.Text = projectObj.Code + "-NCR-" + this.ddlGroup.SelectedItem.Text.Split(',')[0]+"-"+year + "-" + sequence;
                    break;
                case "2":
                    this.txtNumber.Text = projectObj.Code + "-SI-" + this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-" + year + "-" + sequence;
                    break;
            }
            this.txtSequentialNumber.Text = sequence;
            this.txtYear.Value = year;
        }
        protected string TrimDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 1000)
            {
                return string.Concat(description.Substring(0, 1000), "...");
            }
            return description;
        }
        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid )
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projectObj = this.projectcodeService.GetById(projectId);
                NCR_SI changeRequestObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                    changeRequestObj = this.ncrSiService.GetById(changeRequestId);
                    if (changeRequestObj != null)
                    {
                        
                        this.CollectData(ref changeRequestObj, projectObj);

                        changeRequestObj.LastUpdatedBy = UserSession.Current.User.Id;
                        changeRequestObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        changeRequestObj.LastUpdatedDate = DateTime.Now;
                        this.ncrSiService.Update(changeRequestObj);
                    }
                }
                else
                {
                    changeRequestObj = new NCR_SI()
                    {
                        ID = new Guid(Session["NCRSIID"].ToString()),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        IsDelete = false,
                        IsCompleteFinal = false,
                        IsInWFProcess = false,
                        IsWFComplete = false,
                        Type = Convert.ToInt32(this.Request.QueryString["type"]),
                    };

                    this.CollectData(ref changeRequestObj, projectObj);
                    if (!this.changeRequestService.IsExist(changeRequestObj.Number))
                    {
                        this.ncrSiService.Insert(changeRequestObj);
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "NCR/SI. is already exist. ";
                        return;
                    }
                }

              if(this.Request.QueryString["fromToDoList"] != "true") ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
            }
        }

        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }

        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            //if (this.txtDocNo.Text.Trim().Length == 0)
            //{
            //    this.fileNameValidator.ErrorMessage = "Please enter Document Number.";
            //    this.divDocNo.Style["margin-bottom"] = "-26px;";
            //    args.IsValid = false;
            //}
            //else if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            //{
            //    Guid objId;
            //    Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);

            //    if (this._PECC2DocumentService.IsExistByDocNo(this.txtDocNumber.Text.Trim()) && objId == null)
            //    {
            //        this.fileNameValidator.ErrorMessage = "Document No. is already exist.";
            //        this.divDocNo.Style["margin-bottom"] = "-5px;";
            //        args.IsValid = false;
            //    }
            //}
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("CheckFileName"))
            {
                //var fileName = e.Argument.Split('$')[1];
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData(ProjectCode projectObj)
        {
            this.txtProjectCode.Text = projectObj.FullName;
            this.txtIssueDate.SelectedDate = DateTime.Now;

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();
        }

        private void CollectData(ref NCR_SI obj, ProjectCode projectObj)
        {
            obj.Number = this.txtNumber.Text.Trim();
            obj.Subject = this.txtSubject.Text.Trim();
            obj.Reference = this.txtReference.Text.Trim();

            obj.Type = this.ddlType.SelectedItem != null ?
                Convert.ToInt32(this.ddlType.SelectedValue)
                : 0;
            obj.TypeName = this.ddlType.SelectedItem != null ?
                this.ddlType.SelectedItem.Text.Split(',')[0]
                : string.Empty;
            obj.GroupId = this.ddlGroup.SelectedItem != null ?
                Convert.ToInt32(this.ddlGroup.SelectedValue)
                : 0;
            obj.GroupName = this.ddlGroup.SelectedItem != null ?
                this.ddlGroup.SelectedItem.Text.Split(',')[0]
                : string.Empty;
            obj.Sequence = Convert.ToInt32(this.txtSequentialNumber.Text);
            obj.SequentialNumber = this.txtSequentialNumber.Text;

            obj.IssuedDate = this.txtIssueDate.SelectedDate.Value.Date;
            obj.Description = this.txtDescription.Text.Trim();
            obj.ActionTake = this.txtActionTaken.Text.Trim();
            obj.SignedByPMC = this.txtSignedbyPMC.Text.Trim();
            obj.SignedByPMB = this.txtSignedbyPMB.Text.Trim();
            obj.DateOfSubmission = this.txtDateofsubmission.SelectedDate;
            obj.Status = this.ddlStatus.SelectedValue.Trim();
            obj.ClosedDate = this.txtClosedDate.SelectedDate!= null ?this.txtClosedDate.SelectedDate.Value.Date:(DateTime?)null ;
            obj.ClosedByPMB = this.txtClosedByPMB.Text.Trim();
            obj.ClosedByPMC = this.txtClosedByPMC.Text.Trim();
            obj.Note = this.txtNote.Text.Trim();
            obj.Year = (int?)this.txtYear.Value;
            obj.ConfidentialityId = 0;
            obj.ConfidentialityName = string.Empty;
            obj.ProjectId = projectObj.ID;
            obj.ProjectName = projectObj.Code;
        }

        private void LoadDocInfo(NCR_SI obj, ProjectCode projectObj)
        {
            this.txtNumber.Text = obj.Number;
            this.txtSubject.Text = obj.Subject;
            this.txtReference.Text = obj.Reference;
            this.txtProjectCode.Text = projectObj.FullName;
            this.ddlType.SelectedValue = obj.Type.ToString();
            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
            this.txtSequentialNumber.Text = obj.SequentialNumber;
            this.txtYear.Value = obj.Year;
            this.txtIssueDate.SelectedDate = obj.IssuedDate;
            this.txtDescription.Text = obj.Description;
            this.txtActionTaken.Text = obj.ActionTake;
            this.txtSignedbyPMB.Text = obj.SignedByPMB;
            this.txtSignedbyPMC.Text = obj.SignedByPMC;
            this.txtDateofsubmission.SelectedDate = obj.DateOfSubmission;
            this.ddlStatus.SelectedValue = obj.Status.Trim();
            this.txtClosedDate.SelectedDate = obj.ClosedDate;
            this.txtClosedByPMB.Text = obj.ClosedByPMB;
            this.txtClosedByPMC.Text = obj.ClosedByPMC;
            this.txtNote.Text = obj.Note;
           this.ddlStatus.Enabled = (UserSession.Current.User.IsDC.GetValueOrDefault() || UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()) && UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
            this.txtClosedDate.DatePopupButton.Visible = (UserSession.Current.User.IsDC.GetValueOrDefault() || UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()) && UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
            this.txtClosedDate.EnableTyping = (UserSession.Current.User.IsDC.GetValueOrDefault() || UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()) && UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
        }
        protected void ddlGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
            var sequence = Utilities.Utility.ReturnSequenceString(this.ncrSiService.GetNCRSISequence(Convert.ToInt32(this.ddlType.SelectedValue), Convert.ToInt32(this.ddlGroup.SelectedValue), year), 4);
            this.RegenerateNo(sequence, projectObj,year);
        }
        protected void txtSequentialNumber_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                var project = this.txtNumber.Text.Split('-')[0];
                var sequence = Convert.ToInt32(this.txtSequentialNumber.Text);
                var obj = this.ncrSiService.GetBySequene(sequence);
                if (obj != null && !obj.IsCancel.GetValueOrDefault())
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Message", "alert('Sequence number already exist.')", true);
                    this.txtSequentialNumber.Text = this.txtNumber.Text.Split('-')[4];
                    return;
                }

                switch (this.ddlType.SelectedValue)
                {
                    case "1":
                        this.txtNumber.Text = project + "-NCR-" + this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-" + year + "-" + this.txtSequentialNumber.Text;
                        break;
                    case "2":
                        this.txtNumber.Text = project + "-SI-" + this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-" + year + "-" + this.txtSequentialNumber.Text;
                        break;
                }
            }
            catch { }
        }
        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                Guid objId;
                Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);

                var attachList = this.ncrSiAttachFileService.GetByNCRSI(objId).OrderBy(t => t.CreatedDate);
                this.grdDocument.DataSource = attachList.Where(t => !t.IsOnlyMarkupPage.GetValueOrDefault());
            }
            else
            {
                this.grdDocument.DataSource = new List<NCR_SIAttachFile>();
            }
        }
        protected void btnExportNCR_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {   ////update 
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projectObj = this.projectcodeService.GetById(projectId);
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
               var changeRequestObj = this.ncrSiService.GetById(changeRequestId);
                if (changeRequestObj != null && UserSession.Current.User.Role.ContractorId==3)
                {
                    changeRequestObj.ActionTake = this.txtActionTaken.Text;
                    changeRequestObj.EPCUpdateActionTaken = DateTime.Now.Date;
                    this.ncrSiService.Update(changeRequestObj);
                }
                ///// save file 
                var flag = false;
                var targetFolder = "../../DocumentLibrary/NCRSI";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/NCRSI";
                Guid objId;
                Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);
                var obj = this.ncrSiService.GetById(objId);
                var attachList = this.ncrSiAttachFileService.GetByNCRSI(objId).OrderBy(t => t.CreatedDate);
                var fileObj = attachList.Where(t => t.TypeId==1).FirstOrDefault();
               
                    //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
                    //var workbook = new Workbook();
                    //workbook.Open(Server.MapPath(fileObj.FilePath));

                    //var dataSheet = workbook.Worksheets[0];
                   // var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
                    var filePath = Server.MapPath("~/Exports") + @"\";
                    var workbook = new Workbook();
                    if (obj.Number.Contains("-SI-"))
                    {
                        workbook.Open(filePath + @"Template\PECC2_SI_Template.xlsm");
                    }
                    else
                    {
                        workbook.Open(filePath + @"Template\PECC2_NRCSI_Template_new.xlsm");

                    }

                    var dataSheet = workbook.Worksheets[0];
                    var dtFull = new DataTable();

                    var filename = obj.Number + "_Form"+DateTime.Now.ToString("ddMMyyyyHHmmss")+".xlsm";
                    int count = 0;
                dataSheet.Cells["J10"].PutValue(obj.IssuedDate);
                dataSheet.Cells["C10"].PutValue(obj.Subject);
                    dataSheet.Cells["R10"].PutValue(obj.Subject);
                    dataSheet.Cells["D11"].PutValue(obj.Reference);
                    dataSheet.Cells["B16"].PutValue(obj.Description);
                    dataSheet.Cells["P16"].PutValue(obj.Description);
                   


                    var imagelist = this.ncrsiAddPictureService.GetByNCRSI(obj.ID, 1).OrderBy(t => t.CreatedDate);
                   
                    foreach (var image in imagelist)
                    {

                        if (count > 0)
                        {
                            dataSheet.Cells.UnhideRow((16 + (count * 2)), 15.75);
                            dataSheet.Cells.UnhideRow((17 + (count * 2)), 237);

                            dataSheet.Cells["B" + (17 + (count * 2))].PutValue(image.Description);
                            dataSheet.Cells["P" + (17 + (count * 2))].PutValue(image.Description);
                            dataSheet.Pictures.Add((17 + (count * 2)), 2, Server.MapPath(image.FilePath));

                        }
                        else
                        {
                            dataSheet.Cells["B" + (17 + count)].PutValue(image.Description);
                            dataSheet.Cells["P" + (17 + count)].PutValue(image.Description);
                            dataSheet.Cells.UnhideRow((17 + (count * 2)), 237);
                            dataSheet.Pictures.Add((17 + (count * 2)), 2, Server.MapPath(image.FilePath));

                        }
                        dataSheet.AutoFitRow((16 + (count * 2)));
                        count++;

                    }
                    count = 0;
                    imagelist =  this.ncrsiAddPictureService.GetByNCRSI(obj.ID, 2).OrderBy(t => t.CreatedDate);
                   
                        //if (this.txtActionTaken.Text.Trim().Length == 0)
                        //{
                        //    this.RadWindowManager1.RadAlert("Please update Action Taken and click Generate again.", 300, 50, "Warring", "");
                        //    return;
                        //}
                        dataSheet.Cells["B" + 46].PutValue(obj.ActionTake);
                        dataSheet.Cells["P" + 46].PutValue(obj.ActionTake);
                        dataSheet.AutoFitRow(45);
                        foreach (var image in imagelist)
                        {
                            dataSheet.Cells.UnhideRow((46 + (count * 2)), 15.75);
                            dataSheet.Cells.UnhideRow((47 + (count * 2)), 237);
                            dataSheet.Cells["B" + (47 + (count * 2))].PutValue(image.Description);
                            dataSheet.Cells["P" + (47 + (count * 2))].PutValue(image.Description);
                            dataSheet.Pictures.Add((47 + (count * 2)), 2, Server.MapPath("../.." + image.FilePath));
                            dataSheet.AutoFitRow((46 + (count * 2)));
                            count++;

                        }
                dataSheet.Cells["J9"].PutValue(obj.Number);
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);
                    workbook.Save(saveFilePath);
                    var serverFilePath = serverFolder + "/" + filename;

                if (fileObj != null)
                {
                    try
                    {
                        File.Delete(Server.MapPath("~" + fileObj.FilePath));
                    }
                    catch { }
                    this.ncrSiAttachFileService.Delete(fileObj);
                }
                var attachFile = new NCR_SIAttachFile()
                    {
                        ID = Guid.NewGuid(),
                        NCR_SIId = obj.ID,
                        FileName = filename,
                        Extension = ".xlsm",
                        FilePath = serverFilePath,
                        ExtensionIcon = "~/images/excelfile.png",
                        FileSize = (double)(new FileInfo(saveFilePath).Length) / 1024,
                        TypeId = 1,
                        TypeName = "NCR/SI Form",
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.UserNameWithFullName,
                        CreatedDate = DateTime.Now
                    };

                    this.ncrSiAttachFileService.Insert(attachFile);
               
                this.grdDocument.Rebind();
               // this.Download_File(filePath + filename);
            }
        }
        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }
        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadAsyncUpload upload = ((GridEditableItem)e.Item)["Upload"].FindControl("AsyncUpload1") as RadAsyncUpload;
                TableCell cell = (TableCell)upload.Parent;

                CustomValidator validator = new CustomValidator();
                validator.ErrorMessage = "Please select file to be uploaded";
                validator.ClientValidationFunction = "validateRadUpload";
                validator.Display = ValidatorDisplay.Dynamic;
                cell.Controls.Add(validator);
            }
        }
        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var pictureObj = new List<NCR_SIAddPicture>();
            if (!string.IsNullOrEmpty(Session["NCRSIID"].ToString())){
            Guid objId;
            Guid.TryParse(Session["NCRSIID"].ToString(), out objId);
             pictureObj= this.ncrsiAddPictureService.GetByNCRSI(objId, 1).OrderBy(t=> t.CreatedDate).ToList();
            }
          
            RadGrid1.DataSource = pictureObj;
        }
        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //if (!IsRadAsyncValid.Value)
            //{
            //    e.Canceled = true;
            //    RadAjaxManager1.Alert("The length of the uploaded file must be less than 1 MB");
            //    return;
            //}
            Guid objId;
            Guid.TryParse(Session["NCRSIID"].ToString(), out objId);
            GridEditFormInsertItem insertItem = e.Item as GridEditFormInsertItem;
            //string imageName = (insertItem["ImageName"].FindControl("txbName") as RadTextBox).Text;
            string description = (insertItem["Description"].FindControl("txbDescription") as RadTextBox).Text;
            RadAsyncUpload radAsyncUpload = insertItem["Upload"].FindControl("AsyncUpload1") as RadAsyncUpload;
            var targetFolder = "../../DocumentLibrary/NCRSI/Picture";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/NCRSI/Picture";

            UploadedFile file = radAsyncUpload.UploadedFiles[0];

            var filename = this.txtNumber.Text + "_" + (this.ncrsiAddPictureService.GetByNCRSI(objId).Count + 1) + file.GetExtension();
            var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

            // Path file to download from server
            var serverFilePath = serverFolder + "/" + filename;


            System.Drawing.Image imgInput = System.Drawing.Image.FromStream(file.InputStream);
            Size size = new Size(450, 300);
            System.Drawing.Image NewImage = resizeImage(imgInput, size);
            NewImage.Save(saveFilePath);
            //create new entity
            var image = new NCR_SIAddPicture();
            image.ID = Guid.NewGuid();
            image.NCR_SIId = objId;
            image.Description = description;
            image.FilePath = serverFilePath;
            image.TypeId = 1;
            image.CreatedDate = DateTime.Now;
            this.ncrsiAddPictureService.Insert(image);

        }
        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //if (!IsRadAsyncValid.Value)
            //{
            //    e.Canceled = true;
            //    RadAjaxManager1.Alert("The length of the uploaded file must be less than 1 MB");
            //    return;
            //}

            GridEditableItem editedItem = e.Item as GridEditableItem;
            var ID = new Guid(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString());
            //string imageName = (editedItem["ImageName"].FindControl("txbName") as RadTextBox).Text;
            string description = (editedItem["Description"].FindControl("txbDescription") as RadTextBox).Text;
            RadAsyncUpload radAsyncUpload = editedItem["Upload"].FindControl("AsyncUpload1") as RadAsyncUpload;

            //retrive entity form the Db
            var image = this.ncrsiAddPictureService.GetById( ID);
            image.Description = description;

            if (radAsyncUpload.UploadedFiles.Count > 0)
            {
                var targetFolder = "../../DocumentLibrary/NCRSI/Picture";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/NCRSI/Picture";

                UploadedFile file = radAsyncUpload.UploadedFiles[0];
                FileInfo OldFile= new FileInfo(Server.MapPath( image.FilePath));
               
                var filename = OldFile.Name;
                OldFile.Delete();
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + filename;

                System.Drawing.Image imgInput = System.Drawing.Image.FromStream(file.InputStream);
                Size size = new Size(450, 300);
                System.Drawing.Image NewImage = resizeImage(imgInput, size);
                NewImage.Save(saveFilePath);

                image.FilePath = serverFilePath;
            }

            this.ncrsiAddPictureService.Update(image);

        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            var ID = new Guid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["ID"].ToString());
            this.ncrsiAddPictureService.Delete(ID);
            
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetEditMode", "isEditMode = true;", true);
            }
        }
        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            //if ((totalBytes < MaxTotalBytes) && (e.File.ContentLength < MaxTotalBytes))
            //{
            //    e.IsValid = true;
            //    totalBytes += e.File.ContentLength;
            //    IsRadAsyncValid = true;
            //}
            //else
            //{
            //    e.IsValid = false;
            //    IsRadAsyncValid = false;
            //}
        }

      

        protected void grActiontaken_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                //RadAsyncUpload upload = ((GridEditableItem)e.Item)["Upload"].FindControl("AsyncUpload2") as RadAsyncUpload;
                //TableCell cell = (TableCell)upload.Parent;

                //CustomValidator validator = new CustomValidator();
                //validator.ErrorMessage = "Please select file to be uploaded";
                //validator.ClientValidationFunction = "validateRadUpload";
                //validator.Display = ValidatorDisplay.Dynamic;
                //cell.Controls.Add(validator);
            }
        }

        protected void grActiontaken_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var pictureObj = new List<NCR_SIAddPicture>();
            if (!string.IsNullOrEmpty(Session["NCRSIID"].ToString()))
            {
                Guid objId;
                Guid.TryParse(Session["NCRSIID"].ToString(), out objId);
                pictureObj = this.ncrsiAddPictureService.GetByNCRSI(objId, 2).OrderBy(t => t.CreatedDate).ToList();
            }

            grActiontaken.DataSource = pictureObj;
        }

        protected void grActiontaken_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            var ID = new Guid(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString());
            //string imageName = (editedItem["ImageName"].FindControl("txbName") as RadTextBox).Text;
            string description = (editedItem["Description"].FindControl("txbDescription") as RadTextBox).Text;
            RadAsyncUpload radAsyncUpload = editedItem["Upload"].FindControl("AsyncUpload2") as RadAsyncUpload;

            //retrive entity form the Db
            var image = this.ncrsiAddPictureService.GetById(ID);
            image.Description = description;

            if (radAsyncUpload.UploadedFiles.Count > 0)
            {
                var targetFolder = "../../DocumentLibrary/NCRSI/Picture";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/NCRSI/Picture";

                UploadedFile file = radAsyncUpload.UploadedFiles[0];
                FileInfo OldFile = new FileInfo(Server.MapPath(image.FilePath));

                var filename = OldFile.Name;
                OldFile.Delete();
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + filename;

                System.Drawing.Image imgInput = System.Drawing.Image.FromStream(file.InputStream);
                Size size = new Size(450, 300);
                System.Drawing.Image NewImage = resizeImage(imgInput, size);
                NewImage.Save(saveFilePath);

                image.FilePath = serverFilePath;
            }

            this.ncrsiAddPictureService.Update(image);
        }

        protected void grActiontaken_InsertCommand(object sender, GridCommandEventArgs e)
        {
            Guid objId;
            Guid.TryParse(Session["NCRSIID"].ToString(), out objId);
            GridEditFormInsertItem insertItem = e.Item as GridEditFormInsertItem;
            //string imageName = (insertItem["ImageName"].FindControl("txbName") as RadTextBox).Text;
            string description = (insertItem["Description"].FindControl("txbDescription") as RadTextBox).Text;
            RadAsyncUpload radAsyncUpload = insertItem["Upload"].FindControl("AsyncUpload2") as RadAsyncUpload;
            var targetFolder = "../../DocumentLibrary/NCRSI/Picture";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/NCRSI/Picture";

            UploadedFile file = radAsyncUpload.UploadedFiles[0];

            var filename = this.txtNumber.Text + "_" + (this.ncrsiAddPictureService.GetByNCRSI(objId).Count + 1) + file.GetExtension();
            var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

            // Path file to download from server
            var serverFilePath = serverFolder + "/" + filename;


            System.Drawing.Image imgInput = System.Drawing.Image.FromStream(file.InputStream);
            Size size = new Size(450, 300);
            System.Drawing.Image NewImage = resizeImage(imgInput, size);
            NewImage.Save(saveFilePath);
            //create new entity
            var image = new NCR_SIAddPicture();
            image.ID = Guid.NewGuid();
            image.NCR_SIId = objId;
            image.Description = description;
            image.FilePath = serverFilePath;
            image.TypeId = 2;
            image.CreatedDate = DateTime.Now;
            this.ncrsiAddPictureService.Insert(image);

        }

        protected void grActiontaken_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            var ID = new Guid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["ID"].ToString());
            this.ncrsiAddPictureService.Delete(ID);
        }

        protected void grActiontaken_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetEditMode", "isEditMode = true;", true);
            }
        }
    }
}