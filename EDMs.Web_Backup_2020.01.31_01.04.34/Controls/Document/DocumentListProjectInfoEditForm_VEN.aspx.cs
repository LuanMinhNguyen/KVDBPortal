// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentListProjectInfoEditForm_VEN : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly OrganizationCodeService organizationcodeService;
        private readonly AreaService areaService;
        private readonly UnitService unitService;
        private readonly ProjectCodeService projectcodeService;
        private readonly RevisionSchemaService revisionSchemaService;
        private readonly RevisionStatuService revisionStatusService;
        private readonly ConfidentialityService confidentialityService;
        private readonly SystemCodeService systemCodeService;
        private readonly KKSIdentificationCodeService kksCodeService;
        private readonly DocumentCodeServices documentCodeServices;
        private readonly GroupCodeService groupCodeService;

        /// <summary>
        /// The document package service.
        /// </summary>
        private readonly PECC2DocumentsService pecc2DocumentService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly DocumentSequenceManagementService documentSequenceManagementService;

        private readonly DocumentNumberingService documentNumberingService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DocumentListProjectInfoEditForm_VEN()
        {
            
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.userService = new UserService();
            this.pecc2DocumentService = new PECC2DocumentsService();
            this.areaService = new AreaService();
            this.confidentialityService = new ConfidentialityService();
            this.organizationcodeService = new OrganizationCodeService();
            this.projectcodeService = new ProjectCodeService();
            this.unitService = new UnitService();
            this.revisionStatusService = new RevisionStatuService();
            this.revisionSchemaService = new   RevisionSchemaService();
            this.documentSequenceManagementService = new DocumentSequenceManagementService();
            this.documentNumberingService = new DocumentNumberingService();
            this.systemCodeService = new SystemCodeService();
            this.kksCodeService = new KKSIdentificationCodeService();
            this.documentCodeServices = new DocumentCodeServices();
            this.groupCodeService = new GroupCodeService();
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
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid docId;
                    Guid.TryParse(this.Request.QueryString["docId"].ToString(),out docId);
                    var docObj = this.pecc2DocumentService.GetById(docId);
                    if (docObj != null)
                    {
                        this.LoadDocInfo(docObj, projectObj);

                        var createdUser = this.userService.GetByID(docObj.CreatedBy.GetValueOrDefault());
                        this.lblCreated.Text = "Created at " + docObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (docObj.LastUpdatedBy != null && docObj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(docObj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + docObj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                }
            }
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
                PECC2Documents docObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var docId = new Guid(this.Request.QueryString["docId"]);
                    docObj = this.pecc2DocumentService.GetById(docId);
                    if (docObj != null)
                    {

                        if (!string.IsNullOrEmpty(this.Request.QueryString["Revise"]))
                        {
                            var oldRevision = docObj.Revision;
                            var newRevision = this.txtRevision.Text.Trim();
                            if (newRevision != oldRevision)
                            {
                                var docObjNew = new PECC2Documents();
                                this.CollectData(ref docObjNew, projectObj);

                                // Insert new doc
                                docObjNew.GroupId = docObj.GroupId;
                                docObjNew.GroupCode = docObj.GroupCode;
                                docObjNew.CreatedBy = UserSession.Current.User.Id;
                                docObjNew.CreatedByName = UserSession.Current.User.FullName;
                                docObjNew.CreatedDate = DateTime.Now;
                                docObjNew.IsLeaf = true;
                                docObjNew.IsDelete = false;
                                docObjNew.ParentId = docObj.ParentId ?? docObj.ID;
                                docObjNew.IsHasAttachFile = false;
                                docObjNew.IsCreateOutgoingTrans = false;
                                docObjNew.IsCompleteFinal = false;
                                docObjNew.IsInWFProcess = false;
                                docObjNew.IsWFComplete = false;
                                if (!this.pecc2DocumentService.IsExist(docObjNew.DocNo, docObjNew.Revision))
                                {
                                    this.pecc2DocumentService.Insert(docObjNew);
                                }
                                else
                                {
                                    this.blockError.Visible = true;
                                    this.lblError.Text = "Document No. is already exist. ";
                                    return;
                                }

                                // Upate old doc
                                docObj.IsLeaf = false;
                            }
                        }
                        else
                        {
                            this.CollectData(ref docObj, projectObj);
                        }

                        docObj.LastUpdatedBy = UserSession.Current.User.Id;
                        docObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        docObj.LastUpdatedDate = DateTime.Now;
                        this.pecc2DocumentService.Update(docObj);
                    }
                }
                else
                {
                    docObj = new PECC2Documents()
                    {
                        ID=Guid.NewGuid(),
                        GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue),
                        GroupCode = this.ddlGroup.SelectedItem.Text.Split(',')[0],
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        IsLeaf = true,
                        IsDelete = false,
                        IsHasAttachFile = false,
                        IsCreateOutgoingTrans=false,
                        IsCompleteFinal=false,
                        IsInWFProcess=false,
                        IsWFComplete=false
                    };

                    this.CollectData(ref docObj, projectObj);
                    if (!this.pecc2DocumentService.IsExist(docObj.DocNo, docObj.Revision))
                    {
                        this.pecc2DocumentService.Insert(docObj);
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "Document No. is already exist. ";
                        return;
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
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
            //else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            //{
            //    Guid docId;
            //    Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);

            //    if (this._PECC2DocumentService.IsExistByDocNo(this.txtDocNumber.Text.Trim()) && docId == null)
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

            var areaList = this.areaService.GetAll();
            this.ddlArea.DataSource = areaList.OrderBy(t => t.Code);
            this.ddlArea.DataTextField = "FullName";
            this.ddlArea.DataValueField = "ID";
            this.ddlArea.DataBind();

            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
            }

            //system document number
            var sysytemlist = this.systemCodeService.GetAll().Where(t => t.ParentId == null).ToList();
            sysytemlist.Insert(0, new SystemCode() {ID = 0});
            this.ddlSystem.DataSource = sysytemlist.OrderBy(t=> t.Code);
            this.ddlSystem.DataTextField = "FullName";
            this.ddlSystem.DataValueField = "ID";
            this.ddlSystem.DataBind();

            if (this.ddlSystem.SelectedItem != null)
            {
                var subsystemList = this.systemCodeService.GetAllSubSystem(Convert.ToInt32(this.ddlSystem.SelectedValue));
                subsystemList.Insert(0, new SystemCode() { ID = 0 });
                this.ddlSubSystem.DataSource = subsystemList.OrderBy(t => t.Code);
                this.ddlSubSystem.DataTextField = "FullName";
                this.ddlSubSystem.DataValueField = "ID";
                this.ddlSubSystem.DataBind();
            }

            var docTypeList = this.documentTypeService.GetAllByCategory(Convert.ToInt32(this.Request.QueryString["categoryId"]));
            this.ddlDocType.DataSource = docTypeList.OrderBy(t => t.Code);
            this.ddlDocType.DataTextField = "FullName";
            this.ddlDocType.DataValueField = "ID";
            this.ddlDocType.DataBind();

            //add Revison Schema chu co
            var revisionschemalist = this.revisionSchemaService.GetAll();
            revisionschemalist.Insert(0, new RevisionSchema() {ID = 0});
            this.ddlRevisonSchema.DataSource = revisionschemalist;
            this.ddlRevisonSchema.DataTextField = "FullName";
            this.ddlRevisonSchema.DataValueField = "ID";
            this.ddlRevisonSchema.DataBind();

            var RevisionStatustlist = this.revisionStatusService.GetAll();
            this.ddlRevStatus.DataSource = RevisionStatustlist;
            this.ddlRevStatus.DataTextField = "FullName";
            this.ddlRevStatus.DataValueField = "ID";
            this.ddlRevStatus.DataBind();

            //Confdentiality
            var Confdentialitylist = this.confidentialityService.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault());
            //Confdentialitylist.Insert(0, new Confidentiality() { ID = 0, Code = string.Empty });
            this.ddlConfidentiality.DataSource = Confdentialitylist;
            this.ddlConfidentiality.DataTextField = "Code";
            this.ddlConfidentiality.DataValueField = "ID";
            this.ddlConfidentiality.DataBind();

            var organizationList = this.organizationcodeService.GetAll();
            organizationList.Insert(0, new OrganizationCode() {ID = 0});
            this.ddlOriginatingOrganisation.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlOriginatingOrganisation.DataTextField = "FullName";
            this.ddlOriginatingOrganisation.DataValueField = "ID";
            this.ddlOriginatingOrganisation.DataBind();

            this.ddlReceivingOrganisation.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlReceivingOrganisation.DataTextField = "FullName";
            this.ddlReceivingOrganisation.DataValueField = "ID";
            this.ddlReceivingOrganisation.DataBind();

            var docActionCodeList = this.documentCodeServices.GetAllActionCode();
            this.ddlDocActionCode.DataSource = docActionCodeList.OrderBy(t => t.Code);
            this.ddlDocActionCode.DataTextField = "FullName";
            this.ddlDocActionCode.DataValueField = "ID";
            this.ddlDocActionCode.DataBind();

            var docReviewStatusList = this.documentCodeServices.GetAllReviewStatus();
            this.ddlDocReviewStatus.DataSource = docReviewStatusList.OrderBy(t => t.Code);
            this.ddlDocReviewStatus.DataTextField = "FullName";
            this.ddlDocReviewStatus.DataValueField = "ID";
            this.ddlDocReviewStatus.DataBind();

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();
        }

        private void CollectData(ref PECC2Documents obj, ProjectCode projectObj)
        {
            obj.CategoryId = 2;
            obj.CategoryName = "2. Vendor Document";
            obj.DocNo = this.txtDocNo.Text.Trim();
            obj.DocTitle = this.txtDocTitle.Text.Trim();

            obj.ConfidentialityId = this.ddlConfidentiality.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlConfidentiality.SelectedValue)
                                        : 0;
            obj.ConfidentialityName = this.ddlConfidentiality.SelectedItem != null ?
                                        this.ddlConfidentiality.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.AreaId = this.ddlArea.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlArea.SelectedValue)
                                        : 0;
            obj.AreaCode = this.ddlArea.SelectedItem != null ?
                                        this.ddlArea.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.UnitId = this.ddlUnit.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlUnit.SelectedValue)
                                        : 0;
            obj.UnitCode = this.ddlUnit.SelectedItem != null ?
                                        this.ddlUnit.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.SystemId = Convert.ToInt32(this.ddlSystem.SelectedValue);
            obj.SystemCode = this.ddlSystem.SelectedItem.Text.Split(',')[0];
            obj.SubsystemId = Convert.ToInt32(this.ddlSubSystem.SelectedValue);
            obj.SubsystemCode = this.ddlSubSystem.SelectedItem.Text.Split(',')[0];
            obj.ProjectId = projectObj.ID;
            obj.ProjectName = projectObj.Code;
            obj.DocTypeId = this.ddlDocType.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlDocType.SelectedValue)
                                        : 0;
            obj.DocTypeCode = this.ddlDocType.SelectedItem != null ?
                                        this.ddlDocType.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            
            obj.OriginatingOrganisationId = Convert.ToInt32(this.ddlOriginatingOrganisation.SelectedValue);
            obj.OriginatingOrganisationName = this.ddlOriginatingOrganisation.SelectedItem.Text.Split(',')[0];
            obj.ReceivingOrganisationId = Convert.ToInt32(this.ddlReceivingOrganisation.SelectedValue);
            obj.ReceivingOrganisationName = this.ddlReceivingOrganisation.SelectedItem.Text.Split(',')[0];
            obj.OriginalDocumentNumber = this.txtOriginalDocNo.Text.Trim();
            obj.PlannedDate = this.txtPlannedDate.SelectedDate;
            obj.ActualDate = this.txtActualDate.SelectedDate;
            obj.Remarks = this.txtRemark.Text.Trim();
            obj.RevisionSchemaId = Convert.ToInt32(this.ddlRevisonSchema.SelectedValue);
            obj.RevisionSchemaName = this.ddlRevisonSchema.SelectedItem.Text.Split(',')[0];
            obj.Revision = this.txtRevision.Text.Trim();
            obj.MinorRev = this.txtMinorRev.Text.Trim();
            obj.RevStatusId = Convert.ToInt32(this.ddlRevStatus.SelectedValue);
            obj.RevStatusName = this.ddlRevStatus.SelectedItem.Text.Split(',')[0];
            obj.RevDate = this.txtRevDate.SelectedDate;
            obj.RevRemarks = this.txtRevRemark.Text.Trim();
            obj.DocActionId = Convert.ToInt32(this.ddlDocActionCode.SelectedValue);
            obj.DocActionCode = this.ddlDocActionCode.SelectedItem.Text.Split(',')[0];
            obj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
            obj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
            obj.Year = DateTime.Now.Year.ToString();

            obj.ResponseToId = 0;
            obj.ResponseToName = string.Empty;
            obj.CarbonCopyId = 0;
            obj.CarbonCopyName = string.Empty;
            obj.RelatedCSLNo = string.Empty;
            obj.IsNeedReply = false;
            obj.Description = string.Empty;
            obj.Treatment = string.Empty;
            obj.ProposedBy = string.Empty;
            obj.ReviewedBy = string.Empty;
            obj.ApprovedBy = string.Empty;
            obj.KKSId = 0;
            obj.KKSCode = string.Empty;
            obj.TrainNo = string.Empty;
            obj.DisciplineId = 0;
            obj.DisciplineCode = string.Empty;
            obj.SheetNo = string.Empty;

            obj.ManHour = this.txtManHour.Value;
        }

        private void LoadDocInfo(PECC2Documents obj, ProjectCode projectObj)
        {
            this.txtDocNo.Text = obj.DocNo;
            this.txtDocTitle.Text = obj.DocTitle;
            this.ddlConfidentiality.SelectedValue = obj.ConfidentialityId.ToString();
            this.ddlArea.SelectedValue = obj.AreaId.ToString();
            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
                this.ddlUnit.SelectedValue = obj.UnitId.ToString();
            }

            this.ddlSystem.SelectedValue= obj.SystemId.ToString();
            if (this.ddlSystem.SelectedItem != null)
            {
                var subsystemList = this.systemCodeService.GetAllSubSystem(Convert.ToInt32(this.ddlSystem.SelectedValue));
                subsystemList.Insert(0, new SystemCode() { ID = 0 });
                this.ddlSubSystem.DataSource = subsystemList.OrderBy(t => t.Code);
                this.ddlSubSystem.DataTextField = "FullName";
                this.ddlSubSystem.DataValueField = "ID";
                this.ddlSubSystem.DataBind();

                this.ddlSubSystem.SelectedValue = obj.SubsystemId.ToString();
            }

            this.txtProjectCode.Text = projectObj.FullName;
            this.ddlDocType.SelectedValue = obj.DocTypeId.ToString();
            this.ddlOriginatingOrganisation.SelectedValue = obj.OriginatingOrganisationId.ToString();
            this.ddlReceivingOrganisation.SelectedValue = obj.ReceivingOrganisationId.ToString();
            this.txtOriginalDocNo.Text = obj.OriginalDocumentNumber;
            this.txtPlannedDate.SelectedDate = obj.PlannedDate;
            this.txtActualDate.SelectedDate = obj.ActualDate;
            this.txtRemark.Text = obj.Remarks;
            this.ddlRevisonSchema.SelectedValue = obj.RevisionSchemaId.ToString();
            this.txtRevision.Text= obj.Revision;
            this.txtMinorRev.Text = obj.MinorRev;
            this.ddlRevStatus.SelectedValue = obj.RevStatusId.ToString();
            this.txtRevDate.SelectedDate = obj.RevDate;
            this.txtRevRemark.Text = obj.RevRemarks;
            this.ddlDocActionCode.SelectedValue = obj.DocActionId.ToString();
            this.ddlDocReviewStatus.SelectedValue = obj.DocReviewStatusId.ToString();
            this.txtManHour.Value = obj.ManHour;
        }

        protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
            }
        }

        protected void ddlSystem_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlSystem.SelectedItem != null)
            {
                var subsystemList = this.systemCodeService.GetAllSubSystem(Convert.ToInt32(this.ddlSystem.SelectedValue));
                subsystemList.Insert(0, new SystemCode() { ID = 0 });
                this.ddlSubSystem.DataSource = subsystemList.OrderBy(t => t.Code);
                this.ddlSubSystem.DataTextField = "FullName";
                this.ddlSubSystem.DataValueField = "ID";
                this.ddlSubSystem.DataBind();
            }
        }
    }
}