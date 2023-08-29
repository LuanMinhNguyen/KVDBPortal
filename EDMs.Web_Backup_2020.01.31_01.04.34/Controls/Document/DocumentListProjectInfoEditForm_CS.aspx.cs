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
    public partial class DocumentListProjectInfoEditForm_CS : Page
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
        public DocumentListProjectInfoEditForm_CS()
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
                        this.CollectData(ref docObj, projectObj);

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
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData(ProjectCode projectObj)
        {
            this.txtProjectCode.Text = projectObj.FullName;
            var docTypeList = this.documentTypeService.GetAllByCategory(Convert.ToInt32(this.Request.QueryString["categoryId"]));
            this.ddlDocType.DataSource = docTypeList.OrderBy(t => t.Code);
            this.ddlDocType.DataTextField = "FullName";
            this.ddlDocType.DataValueField = "ID";
            this.ddlDocType.DataBind();

            //Confdentiality
            var confdentialitylist = this.confidentialityService.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault());
            this.ddlConfidentiality.DataSource = confdentialitylist;
            this.ddlConfidentiality.DataTextField = "Code";
            this.ddlConfidentiality.DataValueField = "ID";
            this.ddlConfidentiality.DataBind();

            var organizationList = this.organizationcodeService.GetAll();
            
            this.ddlOriginatingOrganisation.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlOriginatingOrganisation.DataTextField = "FullName";
            this.ddlOriginatingOrganisation.DataValueField = "ID";
            this.ddlOriginatingOrganisation.DataBind();

            this.ddlReceivingOrganisation.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlReceivingOrganisation.DataTextField = "FullName";
            this.ddlReceivingOrganisation.DataValueField = "ID";
            this.ddlReceivingOrganisation.DataBind();

            organizationList.Insert(0, new OrganizationCode() { ID = 0 });
            this.ddlCarbonCopy.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlCarbonCopy.DataTextField = "FullName";
            this.ddlCarbonCopy.DataValueField = "ID";
            this.ddlCarbonCopy.DataBind();

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();
        }

        private void CollectData(ref PECC2Documents obj, ProjectCode projectObj)
        {
            obj.CategoryId = 4;
            obj.CategoryName = "4. Correspondence Document  (CS)";
            obj.DocNo = this.txtDocNo.Text.Trim();
            obj.DocTitle = this.txtDocTitle.Text.Trim();

            obj.ConfidentialityId = this.ddlConfidentiality.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlConfidentiality.SelectedValue)
                                        : 0;
            obj.ConfidentialityName = this.ddlConfidentiality.SelectedItem != null ?
                                        this.ddlConfidentiality.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
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
            obj.CarbonCopyId = Convert.ToInt32(this.ddlCarbonCopy.SelectedValue);
            obj.CarbonCopyName = this.ddlCarbonCopy.SelectedItem.Text.Split(',')[0];
            obj.Year = this.txtYear.Text.Trim();
            obj.GroupId = this.ddlGroup.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlGroup.SelectedValue)
                                        : 0;
            obj.GroupCode = this.ddlGroup.SelectedItem != null ?
                                        this.ddlGroup.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.RelatedCSLId = new Guid(this.ddlRelatedCS.SelectedValue);
            obj.RelatedCSLNo = this.ddlRelatedCS.SelectedItem.Text;
            obj.IsNeedReply = this.cbNeedReply.Checked;
            obj.Description = this.txtDescription.Text;
            obj.Treatment = this.txtTreatment.Text;
            obj.ProposedBy = this.txtProposedBy.Text;
            obj.ProposedDate = this.txtProposedDate.SelectedDate;
            obj.ReviewedBy = this.txtReviewedBy.Text;
            obj.ReviewedDate = this.txtReviewedDate.SelectedDate;
            obj.ApprovedBy = this.txtApprovedBy.Text;
            obj.ApprovedDate = this.txtApprovedDate.SelectedDate;
            obj.IssuedDateFrom = this.txtIssuedDateFROM.SelectedDate;
            obj.ReceivedDateTo = this.txtReceivedDateTO.SelectedDate;
            obj.ReceivedDateCC = this.txtReceivedDateCC.SelectedDate;

            obj.AreaId = 0;
            obj.AreaCode = string.Empty;
            obj.UnitId = 0;
            obj.UnitCode = string.Empty;
            obj.SystemId = 0;
            obj.SystemCode = string.Empty;
            obj.SubsystemId = 0;
            obj.SubsystemCode = string.Empty;
            obj.OriginalDocumentNumber = string.Empty;
            obj.ResponseToId = 0;
            obj.ResponseToName = string.Empty;
            obj.Remarks = string.Empty;
            obj.RevisionSchemaId = 0;
            obj.RevisionSchemaName = string.Empty;
            obj.Revision = string.Empty;
            obj.MinorRev = string.Empty;
            obj.RevStatusId = 0;
            obj.RevStatusName = string.Empty;
            obj.RevRemarks = string.Empty;
            obj.DocActionId = 0;
            obj.DocActionCode = string.Empty;
            obj.DocReviewStatusId = 0;
            obj.DocReviewStatusCode = string.Empty;
            obj.KKSId = 0;
            obj.KKSCode = string.Empty;
            obj.TrainNo = string.Empty;
            obj.DisciplineId = 0;
            obj.DisciplineCode = string.Empty;
            obj.SheetNo = string.Empty;
        }

        private void LoadDocInfo(PECC2Documents obj, ProjectCode projectObj)
        {
            this.txtDocNo.Text = obj.DocNo;
            this.txtDocTitle.Text = obj.DocTitle;
            this.ddlConfidentiality.SelectedValue = obj.ConfidentialityId.ToString();
            this.txtProjectCode.Text = projectObj.FullName;
            this.ddlDocType.SelectedValue = obj.DocTypeId.ToString();
            this.ddlOriginatingOrganisation.SelectedValue = obj.OriginatingOrganisationId.ToString();
            this.ddlReceivingOrganisation.SelectedValue = obj.ReceivingOrganisationId.ToString();
            this.txtYear.Text = obj.Year;
            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
        }
    }
}