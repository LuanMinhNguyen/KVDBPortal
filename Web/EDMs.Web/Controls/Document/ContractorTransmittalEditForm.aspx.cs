// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Business.Services.Document;
    using Business.Services.Library;
    using Business.Services.Security;
    using Data.Entities;
    using Utilities.Sessions;
    using System.Configuration;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ContractorTransmittalEditForm : Page
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly RoleService _RoleService;

        /// <summary>
        /// The to list service.
        /// </summary>
        private readonly OrganizationCodeService organizationCodeService;

        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly ContractorTransmittalService contractorTransmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly ProjectCodeService projectService;
        private readonly HashSet<DateTime> holidays = new HashSet<DateTime>();
        private readonly HolidayConfigService holidayConfigService;
        private readonly GroupCodeService groupCodeService;

        private readonly DocumentCodeServices documentCodeServices;

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractorTransmittalEditForm()
        {
            this.userService = new UserService();
            this.contractorTransmittalService = new ContractorTransmittalService();
            this.organizationCodeService = new OrganizationCodeService();
            this.projectService=new ProjectCodeService();
            this.groupCodeService = new GroupCodeService();
            this.documentCodeServices = new DocumentCodeServices();
            this._RoleService = new RoleService();
            this.holidayConfigService = new HolidayConfigService();
            this.changeRequestService = new ChangeRequestService();
            this.changeRequestDocFileService = new ChangeRequestDocFileService();
            this.changeRequestAttachFileService = new ChangeRequestAttachFileService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
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
                this.LoadComboData();
                var holidayList = this.holidayConfigService.GetAll();
                foreach (var holidayConfig in holidayList)
                {
                    for (DateTime i = holidayConfig.FromDate.GetValueOrDefault(); i < holidayConfig.ToDate.GetValueOrDefault(); i = i.AddDays(1))
                    {
                        this.holidays.Add(i);
                    }
                }
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.contractorTransmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {
                        this.ddlProjectCode.SelectedValue = obj.ProjectId.ToString();
                        this.txtTransNo.Text = obj.TransNo;
                        this.txtDayIssued.SelectedDate = obj.TransDate;
                        this.txtDescription.Text = obj.Description;
                        if (obj.OriginatingOrganizationId.ToString() != "")
                        { this.ddlOriginatingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString(); }
                        if (obj.ReceivingOrganizationId.ToString() != "")
                        { this.ddlReceivingOrganization.SelectedValue = obj.ReceivingOrganizationId.ToString(); }
                        if (obj.ProjectId.ToString() != "")
                        { this.ddlProjectCode.SelectedValue = obj.ProjectId.ToString(); }

                        this.ddlGroup.SelectedValue = obj.GroupId.ToString();
                        this.txtDueDate.SelectedDate = obj.DueDate;
                        this.ddlPurpose.SelectedValue = obj.PurposeId.ToString();
                        this.txtFrom.Text = obj.FromValue;
                        this.txtTo.Text = obj.ToValue;
                        this.txtCC.Text = obj.CCValue;
                        this.txtSubject.Text = obj.Subject;
                        this.txtVolumeNumber.Text = obj.VolumeNumber;
                        this.ddlCategory.SelectedValue = obj.CategoryId.GetValueOrDefault().ToString();
                        this.txtConsultantdeadline.SelectedDate = obj.ConsultantDeadline;
                        this.txtOwnerDeadline.SelectedDate = obj.OwnerDeadline;
                        this.ddlTransmittedByName.SelectedValue = obj.TransmittedById.ToString();
                        this.txtTransmittedByDesignation.Text = obj.TransmittedByDesignation;
                        this.ddlAcknowledgedByName.SelectedValue = obj.AcknowledgedId.ToString();
                        this.txtAcknowledgedByDesignation.Text = obj.AcknowledgedByDesignation;
                        this.txtRemark.Text = obj.Remark;
                        this.ddlForSend.SelectedValue = obj.ForSentId.ToString();
                        this.txtRefIncomingTrans.Text = obj.RefTransNo;
                        foreach (RadTreeNode actionNode in this.rtvCCOrganisation.Nodes)
                        {
                            actionNode.Checked = !string.IsNullOrEmpty(obj.CCOrganizationId) && obj.CCOrganizationId.Split(';').ToList().Contains(actionNode.Value);
                        }

                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());

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
                        this.ddlProjectCode.Enabled = false;
                        this.ddlReceivingOrganization.Enabled = false;
                        this.ddlOriginatingOrganization.Enabled = false;

                    }
                    if (obj.IsSend != null && obj.IsSend == true)
                    {
                        this.ddlOriginatingOrganization.Enabled = false;
                        this.ddlProjectCode.Enabled = false;
                        this.ddlReceivingOrganization.Enabled = false;
                        this.ddlGroup.Enabled = false;
                    }
                }
                else
                {
                    
                    this.ddlProjectCode.SelectedValue = this.Request.QueryString["projId"];
                    this.CreatedInfo.Visible = false;
                    if (!string.IsNullOrEmpty(this.Request.QueryString["TransInId"]))
                    {
                        var obj = this.contractorTransmittalService.GetById(new Guid(this.Request.QueryString["TransInId"]));
                        if (obj != null)
                        {
                            this.txtDescription.Text = obj.Description;
                            this.ddlForSend.SelectedValue = obj.ForSentId.ToString();
                            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
                            this.ddlReceivingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString();
                            this.ddlOriginatingOrganization.SelectedValue = obj.ReceivingOrganizationId.ToString();
                            this.ddlProjectCode.SelectedValue = obj.ProjectId.ToString();
                            this.txtVolumeNumber.Text = obj.VolumeNumber;
                            this.ddlCategory.SelectedValue = obj.CategoryId.GetValueOrDefault().ToString();
                        }
                    }
                    this.RegenerateTransNo();
                }
            }
        }
        private DateTime GetDate(int day, DateTime transdate)
        {
            var actualDeadline = transdate;
            for (int i = 1; i <= day; i++)
            {
                actualDeadline = this.GetNextWorkingDay(actualDeadline);
            }
            return actualDeadline;
        }
        private bool IsHoliday(DateTime date)
        {
            return holidays.Contains(date);
        }
        private bool IsWeekEnd(DateTime date)
        { 
            return ConfigurationManager.AppSettings["WeekendWork"] == "false"? date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday:false;
        }
        private DateTime GetNextWorkingDay(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            }
            while (IsHoliday(date) || IsWeekEnd(date));

            return date;
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
            if (this.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var obj = this.contractorTransmittalService.GetById(objId);
                    if (obj != null)
                    {
                        this.CollectData(obj);

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedByName = UserSession.Current.User.FullName;
                        obj.LastUpdatedDate = DateTime.Now;

                        var refTransObj = this.contractorTransmittalService.GetById(obj.RefTransId.GetValueOrDefault());
                        if (refTransObj != null)
                        {
                            refTransObj.RefTransId = obj.ID;
                            refTransObj.RefTransNo = obj.TransNo;
                            this.contractorTransmittalService.Update(refTransObj);
                        }

                        this.contractorTransmittalService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ContractorTransmittal();
                    obj.ID = Guid.NewGuid();
                    this.CollectData(obj);

                    obj.Status = "Missing Doc File";
                    obj.IsValid = false;
                    obj.IsSend = false;
                    obj.ErrorMessage = "Missing Document Attachment.";

                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedByName = UserSession.Current.User.FullName;
                    obj.CreatedDate = DateTime.Now;

                    // Create store folder
                    var physicalStoreFolder = Server.MapPath("../../DocumentLibrary/ContractorTransmittal/" + obj.TransNo + "_" + obj.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
                    Directory.CreateDirectory(physicalStoreFolder);
                    Directory.CreateDirectory(physicalStoreFolder + @"\eTRM File");


                    var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/" 
                        ? string.Empty 
                        : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/ContractorTransmittal/" + obj.TransNo + "_" + obj.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
                    obj.StoreFolderPath = serverStoreFolder;
                    // -----------------------------------------------------------------------------------------------------------------

                    var objId = this.contractorTransmittalService.Insert(obj);
                    if (!string.IsNullOrEmpty(this.Request.QueryString["TransInId"]) && objId!= null)
                    {
                        var TransObj = this.contractorTransmittalService.GetById(new Guid(this.Request.QueryString["TransInId"]));
                        TransObj.IsValid = true;
                        this.contractorTransmittalService.Update(TransObj);
                    }
                        var refTransObj = this.contractorTransmittalService.GetById(obj.RefTransId.GetValueOrDefault());
                    if (refTransObj != null)
                    {
                        refTransObj.RefTransId = obj.ID;
                        refTransObj.RefTransNo = obj.TransNo;
                        this.contractorTransmittalService.Update(refTransObj);
                    }

                    // New logic when object is Change Request
                    //if (this.ddlForSend.SelectedValue == "2")
                    //{
                    //    var changeRequestObj = this.changeRequestService.GetById(new Guid(this.ddlChangeRequest.SelectedValue));
                    //    if (changeRequestObj != null)
                    //    {
                    //        // Update trans info for change request
                    //        changeRequestObj.OutgoingTransId = obj.ID;
                    //        changeRequestObj.OutgoingTransNo = obj.TransNo;
                    //        changeRequestObj.IsCreateOutgoingTrans = true;

                    //        this.changeRequestService.Update(changeRequestObj);
                    //        // ---------------------------------------------------------------------------------------------------------

                    //        // Get attach doc file for Outgoing Trans
                    //        var docFileList = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestObj.ID);
                    //        foreach (var changeRequestDocFile in docFileList)
                    //        {
                    //            var contractorTransDocFile = new ContractorTransmittalDocFile()
                    //            {
                    //                ID = Guid.NewGuid(),
                    //                TransId = obj.ID,
                    //                PurposeId = obj.PurposeId,
                    //                PurposeName = obj.PurposeName,
                    //                Extension = changeRequestDocFile.Extension,
                    //                ExtensionIcon = changeRequestDocFile.ExtensionIcon,
                    //                FilePath = changeRequestDocFile.FilePath,
                    //                FileSize = changeRequestDocFile.FileSize,
                    //                ProjectId = changeRequestDocFile.ProjectId,
                    //                ProjectName = changeRequestDocFile.ProjectName,
                    //                DocumentTypeId = changeRequestDocFile.DocumentTypeId,
                    //                DocumentTypeName = changeRequestDocFile.DocumentTypeName,
                    //                DocumentTypeGroupId = changeRequestDocFile.DocumentTypeGroupId,
                    //                DocumentTypeGroupName = changeRequestDocFile.DocumentTypeGroupName,
                    //                OriginatingOrganizationId = changeRequestDocFile.OriginatingOrganizationId,
                    //                OriginatingOrganizationName = changeRequestDocFile.OriginatingOrganizationName,
                    //                ReceivingOrganizationId = changeRequestDocFile.ReceivingOrganizationId,
                    //                ReceivingOrganizationName = changeRequestDocFile.ReceivingOrganizationName,
                    //                Year = changeRequestDocFile.Year,
                    //                GroupCodeId = changeRequestDocFile.GroupCodeId,
                    //                GroupCodeName = changeRequestDocFile.GroupCodeName,
                    //                Sequence = changeRequestDocFile.Sequence,
                    //                UnitCodeId = changeRequestDocFile.UnitCodeId,
                    //                UnitCodeName = changeRequestDocFile.UnitCodeName,
                    //                AreaId = changeRequestDocFile.AreaId,
                    //                AreaName = changeRequestDocFile.AreaName,
                    //                KKSCodeId = changeRequestDocFile.KKSCodeId,
                    //                KKSCodeName = changeRequestDocFile.KKSCodeName,
                    //                TrainNo = changeRequestDocFile.TrainNo,
                    //                DisciplineCodeId = changeRequestDocFile.DisciplineCodeId,
                    //                DisciplineCodeName = changeRequestDocFile.DisciplineCodeName,
                    //                DocumentNo = changeRequestDocFile.DocumentNo,
                    //                DocumentTitle = changeRequestDocFile.DocumentTitle,
                    //                FileName = changeRequestDocFile.FileName,
                    //                Revision = changeRequestDocFile.Revision,
                    //                ErrorMessage = string.Empty,
                    //                Status = string.Empty,
                    //                IsReject = false,
                    //                RejectReason = string.Empty,
                    //                RevRemark = changeRequestDocFile.RevRemark,
                    //                ContractorRefNo = changeRequestDocFile.ContractorRefNo
                    //            };

                    //            this.contractorTransmittalDocFileService.Insert(contractorTransDocFile);
                    //        }

                    //        obj.Status = string.Empty;
                    //        obj.ErrorMessage = string.Empty;
                    //        obj.IsValid = true;
                    //        this.contractorTransmittalService.Update(obj);
                    //        // ---------------------------------------------------------------------------------------------------------
                    //    }
                    //}
                    // -----------------------------------------------------------------------------------------------------------------
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(ContractorTransmittal obj)
        {
            obj.ProjectId = Convert.ToInt32(this.ddlProjectCode.SelectedValue);
            obj.ProjectName = (this.ddlProjectCode.SelectedItem.Text.Split(','))[0].Trim();
            obj.TransNo = this.txtTransNo.Text;
            obj.TransDate = this.txtDayIssued.SelectedDate;
            obj.DueDate = this.txtDueDate.SelectedDate;
            obj.ConsultantDeadline = this.txtConsultantdeadline.SelectedDate;
            obj.OwnerDeadline = this.txtOwnerDeadline.SelectedDate;
            obj.Priority = "Normal";
            obj.Description = this.txtDescription.Text.Trim();
            obj.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            obj.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
            obj.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            obj.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
            obj.PurposeId = Convert.ToInt32(this.ddlPurpose.SelectedValue);
            obj.PurposeName = this.ddlPurpose.SelectedItem.Text;
            obj.FromValue = this.txtFrom.Text;
            obj.ToValue = this.txtTo.Text;
            obj.CCValue = this.txtCC.Text;
            //obj.IssuedDate = this.txtDayIssued.SelectedDate;
            obj.VolumeNumber = this.txtVolumeNumber.Text;
            obj.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            obj.CategoryName = this.ddlCategory.SelectedItem.Text;
            obj.GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue);
            obj.GroupCode = this.ddlGroup.SelectedItem.Text.Split(',')[0];
            obj.Subject = this.txtSubject.Text;
            obj.TransmittedById = this.ddlTransmittedByName.SelectedItem != null
                                    ? Convert.ToInt32(this.ddlTransmittedByName.SelectedValue)
                                    : 0;
            obj.TransmittedByName = this.ddlTransmittedByName.SelectedItem != null
                                    ? this.ddlTransmittedByName.SelectedItem.Text
                                    : string.Empty;
            obj.TransmittedByDesignation = this.txtTransmittedByDesignation.Text;

            obj.AcknowledgedId = this.ddlAcknowledgedByName.SelectedItem != null
                                    ? Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue)
                                    : 0;
            obj.AcknowledgedByName = this.ddlAcknowledgedByName.SelectedItem != null
                                    ? this.ddlAcknowledgedByName.SelectedItem.Text
                                    : string.Empty;
            obj.AcknowledgedByDesignation = this.txtAcknowledgedByDesignation.Text;
            obj.Remark = this.txtRemark.Text;
            obj.Year = DateTime.Now.Year;
            obj.Sequence = Convert.ToInt32(this.txtTransNo.Text.Trim().Split('-')[6]);
            obj.SequenceString = this.txtTransNo.Text.Trim().Split('-')[6];
            obj.TypeId = 2;
            obj.ForSentId = Convert.ToInt32(this.ddlForSend.SelectedValue);
            obj.ForSentName = this.ddlForSend.SelectedItem.Text;

            obj.CCOrganizationId = string.Empty;
            obj.CCOrganizationName = string.Empty;

            
            if (!string.IsNullOrEmpty(this.Request.QueryString["TransInId"]))
            {

                var incomingTransObj = this.contractorTransmittalService.GetById(new Guid(this.Request.QueryString["TransInId"]));
                if (incomingTransObj != null)
                {
                    obj.RefTransId = incomingTransObj.ID;
                    obj.RefTransNo = incomingTransObj.TransNo;
                }
            }

            foreach (RadTreeNode actionNode in this.rtvCCOrganisation.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.CCOrganizationId += actionNode.Value + ";";
                obj.CCOrganizationName += actionNode.Text + Environment.NewLine;
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
            if(this.txtTransNo.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter transmittal number.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList;
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();

            var projectList = this.projectService.GetAll();
            this.ddlProjectCode.DataSource = projectList.OrderBy(t => t.Code);
            this.ddlProjectCode.DataTextField = "FullName";
            this.ddlProjectCode.DataValueField = "ID";
            this.ddlProjectCode.DataBind();

            var organizationList = this.organizationCodeService.GetAll().OrderBy(t => t.Code);

            this.ddlOriginatingOrganization.DataSource = organizationList;
            this.ddlOriginatingOrganization.DataTextField = "FullName";
            this.ddlOriginatingOrganization.DataValueField = "Id";
            this.ddlOriginatingOrganization.DataBind();
            this.ddlOriginatingOrganization.SelectedValue = UserSession.Current.User.Role.ContractorId.ToString();

            this.ddlReceivingOrganization.DataSource = organizationList;
            this.ddlReceivingOrganization.DataTextField = "FullName";
            this.ddlReceivingOrganization.DataValueField = "Id";
            this.ddlReceivingOrganization.DataBind();

            if (organizationList.Any(t => t.IsDefaultReceiveContractorOutgoingTrans.GetValueOrDefault()))
            {
                this.ddlReceivingOrganization.SelectedValue = organizationList.FirstOrDefault(t => t.IsDefaultReceiveContractorOutgoingTrans.GetValueOrDefault()).ID.ToString();
            }

            this.rtvCCOrganisation.DataSource = organizationList;
            this.rtvCCOrganisation.DataTextField = "FullName";
            this.rtvCCOrganisation.DataValueField = "Id";
            this.rtvCCOrganisation.DataBind();

            var purposeList = this.documentCodeServices.GetAllActionCode();
            this.ddlPurpose.DataSource = purposeList.OrderBy(t => t.Code);
            this.ddlPurpose.DataTextField = "FullName";
            this.ddlPurpose.DataValueField = "ID";
            this.ddlPurpose.DataBind();

            var userList = this.userService.GetAll();
            var trasmittedUser = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            this.ddlTransmittedByName.DataSource = trasmittedUser;
            this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
            this.ddlTransmittedByName.DataValueField = "ID";
            this.ddlTransmittedByName.DataBind();
            this.ddlTransmittedByName.SelectedValue = UserSession.Current.User.Id.ToString();


            if (this.ddlTransmittedByName.SelectedItem != null)
            {
                var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                if (transmittedUser != null)
                {
                    this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                }
            }

            var acknowledgedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            acknowledgedUserList.Insert(0, new User() {Id = 0});
            this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
            this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
            this.ddlAcknowledgedByName.DataValueField = "ID";
            this.ddlAcknowledgedByName.DataBind();

            if (this.ddlAcknowledgedByName.SelectedItem != null)
            {
                var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                if (acknowledgedUser != null)
                {
                    this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                }
            }

            this.txtDayIssued.SelectedDate = DateTime.Now;
            this.txtConsultantdeadline.SelectedDate = GetDate(6, this.txtDayIssued.SelectedDate.Value);
            this.txtOwnerDeadline.SelectedDate = GetDate(7, this.txtDayIssued.SelectedDate.Value);

            this.divRefIncomingTrans.Visible = !string.IsNullOrEmpty(this.Request.QueryString["TransInId"]);
            if (!string.IsNullOrEmpty(this.Request.QueryString["TransInId"]))
            {
                var incomingTransObj = this.contractorTransmittalService.GetById(new Guid(this.Request.QueryString["TransInId"]));
                if (incomingTransObj != null)
                {
                    this.txtRefIncomingTrans.Text = incomingTransObj.TransNo;
                }
            }

            // ---------------------------------------------------------------------------------------------------------

            this.ddlForSend.SelectedValue = this.Request.QueryString["forSent"];
        }
        protected void ddl_RegenerateTransNo(object sender, EventArgs e)
        {
            // Rebind user list of Transmitted and Acknowledged
            var userList = this.userService.GetAll();
            var trasmittedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();

            this.ddlTransmittedByName.DataSource = trasmittedUserList;
            this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
            this.ddlTransmittedByName.DataValueField = "ID";
            this.ddlTransmittedByName.DataBind();

            if (this.ddlTransmittedByName.SelectedItem != null)
            {
                var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                if (transmittedUser != null)
                {
                    this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                }
            }

            var acknowledgedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
            this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
            this.ddlAcknowledgedByName.DataValueField = "ID";
            this.ddlAcknowledgedByName.DataBind();

            if (this.ddlAcknowledgedByName.SelectedItem != null)
            {
                var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                if (acknowledgedUser != null)
                {
                    this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                }
            }

            // --------------------------------------------------------------------------------------
        }
        private void RegenerateTransNo()
        {
            var fromId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            var fromObj = this.organizationCodeService.GetById(fromId);

            var toId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            var toObj = this.organizationCodeService.GetById(toId);

            this.txtTransNo.Text = this.ddlProjectCode.SelectedItem.Text.Split(',')[0] + "-T-";

            if (fromObj != null)
            {
                this.txtTransNo.Text += fromObj.Code + "-";
            }

            if (toObj != null)
            {
                this.txtTransNo.Text += toObj.Code + "-";
            }

            this.txtTransNo.Text += DateTime.Now.Year.ToString().Substring(2, 2) + "-";
            this.txtTransNo.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-";

            var sequence = Utilities.Utility.ReturnSequenceString(this.contractorTransmittalService.GetCurrentSequence(DateTime.Now.Year, Convert.ToInt32(this.ddlGroup.SelectedValue)), 4);
            this.txtTransNo.Text += sequence;
        }
        protected void ddlTransmittedByName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
            if (transmittedUser != null)
            {
                this.txtTransmittedByDesignation.Text = transmittedUser.Position;
            }
        }
        protected void ddlAcknowledgedByName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
            if (acknowledgedUser != null)
            {
                this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
            }
        }
        protected void ddlOriginatingOrganization_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();
            // Rebind user list of Transmitted and Acknowledged
            var userList = this.userService.GetAll();
            var trasmittedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();

            this.ddlTransmittedByName.DataSource = trasmittedUserList;
            this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
            this.ddlTransmittedByName.DataValueField = "ID";
            this.ddlTransmittedByName.DataBind();

            if (this.ddlTransmittedByName.SelectedItem != null)
            {
                var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                if (transmittedUser != null)
                {
                    this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                }
            }
        }
        protected void ddlReceivingOrganization_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();
            var userList = this.userService.GetAll();
            userList.Insert(0, new User() {Id = 0});
            var roleObj = this._RoleService.GetAll(false).Where(t=> t.ContractorId==Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).Select(t=>t.Id);
            var acknowledgedUserList = userList.Where(t => roleObj.Contains(t.RoleId.GetValueOrDefault())).OrderBy(t => t.UserNameWithFullName).ToList();
            this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
            this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
            this.ddlAcknowledgedByName.DataValueField = "ID";
            this.ddlAcknowledgedByName.DataBind();

            if (this.ddlAcknowledgedByName.SelectedItem != null)
            {
                var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                if (acknowledgedUser != null)
                {
                    this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                }
            }
        }
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();
        }
        protected void txtDayIssued_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            this.txtConsultantdeadline.SelectedDate = GetDate(6, this.txtDayIssued.SelectedDate.Value);
            this.txtOwnerDeadline.SelectedDate= GetDate(7, this.txtDayIssued.SelectedDate.Value);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCategory.SelectedValue.Trim()== "2")
            {
                this.LiVolumeNumber.Visible = true;
            }
            else
            {
                this.LiVolumeNumber.Visible = false;
            }
        }
    }
}