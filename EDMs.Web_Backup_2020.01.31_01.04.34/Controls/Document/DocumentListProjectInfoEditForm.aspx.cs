// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Scope;
using EDMs.Web.Utilities;

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

    using System.Collections.Generic;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentListProjectInfoEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly OrganizationCodeService _OrganizationcodeService;
        private readonly DQREDocumentMasterService _DQREDocumentMasterServce;
        private readonly MaterialService _MaterialService;
        private readonly WorkService _WorkcodeService;
        private readonly DrawingService _DrawingcodeService;
        private readonly PlantService _PlantService;
        private readonly AreaService _AreaService;
        private readonly UnitService _UnitService;
        private readonly ProjectCodeService _ProjectcodeService;
        private readonly RevisionSchemaService _RevisionSchemaService;
        private readonly DocumentCodeServices _DocumnetCodeSErvie;
        private readonly DocumentClassService _DocumentClassService;
        private readonly RevisionStatuService _RevisionStatusService;
        private readonly ConfidentialityService _ConfidentialityService;

        /// <summary>
        /// The document package service.
        /// </summary>
        private readonly DQREDocumentService _DQREDocumentService;

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
        public DocumentListProjectInfoEditForm()
        {
            
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.userService = new UserService();
            this._DQREDocumentService = new DQREDocumentService();
            this._AreaService = new AreaService();
            this._ConfidentialityService = new ConfidentialityService();
            this._DocumentClassService = new DocumentClassService();
            this._DocumnetCodeSErvie = new DocumentCodeServices();
            this._DrawingcodeService = new DrawingService();
            this._MaterialService = new MaterialService();
            this._OrganizationcodeService = new OrganizationCodeService();
            this._PlantService = new PlantService();
            this._ProjectcodeService = new ProjectCodeService();
            this._UnitService = new UnitService();
            this._RevisionStatusService = new RevisionStatuService();
            this._WorkcodeService = new WorkService();
            this._RevisionSchemaService = new   RevisionSchemaService();
            this._DQREDocumentMasterServce = new DQREDocumentMasterService();
            this.documentSequenceManagementService = new DocumentSequenceManagementService();
            this.documentNumberingService = new DocumentNumberingService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid docId;
                    Guid.TryParse(this.Request.QueryString["docId"].ToString(),out docId);
                    var docObj = this._DQREDocumentService.GetById(docId);
                    if (docObj != null)
                    {

                        // this.txtProjectName.Text = docObj.ProjectFullName;

                        this.LoadDocInfo(docObj);

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
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
              
                DQREDocument docObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    Guid docId;
                    Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);
                    docObj = this._DQREDocumentService.GetById(docId);
                    if (docObj != null)
                    {

                        if (!string.IsNullOrEmpty(this.Request.QueryString["Revise"]))
                        {

                            var oldRevision = docObj.Revision;
                            var newRevision = this.txtRevision.Text.Trim();
                            if (newRevision != oldRevision)
                            {
                                var docObjNew = new DQREDocument();
                                this.CollectData(ref docObjNew);

                                // Insert new doc
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
                                if (!this._DQREDocumentService.IsExist(docObjNew.DocumentNo, docObjNew.Revision))
                                {
                                    this._DQREDocumentService.Insert(docObjNew);
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
                            this.CollectData(ref docObj);
                        }

                        docObj.LastUpdatedBy = UserSession.Current.User.Id;
                        docObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        docObj.LastUpdatedDate = DateTime.Now;
                        this._DQREDocumentService.Update(docObj);
                    }
                }
                else
                {
                    docObj = new DQREDocument()
                    {
                        ID=Guid.NewGuid(),
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

                    this.CollectData(ref docObj);
                    if (!this._DQREDocumentService.IsExist(docObj.DocumentNo, docObj.Revision))
                    {
                        this._DQREDocumentService.Insert(docObj);
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
            if (this.txtDocNumber.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Document Number.";
                this.divDocNo.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            //else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            //{
            //    Guid docId;
            //    Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);

            //    if (this._DQREDocumentService.IsExistByDocNo(this.txtDocNumber.Text.Trim()) && docId == null)
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
        private void LoadComboData()
        {
            //system document number
            var sysytemlist = this._DQREDocumentMasterServce.GetAll();

            sysytemlist.Insert(0, new DQREDocumentMaster() { SystemDocumentNo = string.Empty });
            this.ddlSystem.DataSource = sysytemlist.OrderBy(t=> t.SystemDocumentNo);
            this.ddlSystem.DataTextField = "SystemDocumentNo";
            this.ddlSystem.DataValueField = "ID";
            this.ddlSystem.DataBind();

            //add projectcode
            var projectcodelist = this._ProjectcodeService.GetAll();
            projectcodelist.Insert(0, new ProjectCode() { ID = 0, Code = string.Empty });
            this.ddlProjectCode.DataSource = projectcodelist;
            this.ddlProjectCode.DataTextField = "FullName";
            this.ddlProjectCode.DataValueField ="ID";
            this.ddlProjectCode.DataBind();
            

            //add Revison Schema chu co
            var revisionschemalist = this._RevisionSchemaService.GetAll();
            revisionschemalist.Insert(0, new RevisionSchema() { ID = 0, Code = string.Empty });
            this.ddlRevisionScheme.DataSource = revisionschemalist;
            this.ddlRevisionScheme.DataTextField = "Code";
            this.ddlRevisionScheme.DataValueField = "ID";
            this.ddlRevisionScheme.DataBind();

            var RevisionStatustlist = this._RevisionStatusService.GetAll();
            RevisionStatustlist.Insert(0, new RevisionStatu() { ID = 0, Code = string.Empty });
            this.ddlRevisionStartus.DataSource = RevisionStatustlist;
            this.ddlRevisionStartus.DataTextField = "Code";
            this.ddlRevisionStartus.DataValueField = "ID";
            this.ddlRevisionStartus.DataBind();

            //document code
            var documentcodelist = this._DocumnetCodeSErvie.GetAll();
            documentcodelist.Insert(0, new DocumentCode() { ID = 0, Code = string.Empty });
            this.ddlDocumentCode.DataSource = documentcodelist;
            this.ddlDocumentCode.DataTextField = "Code";
            this.ddlDocumentCode.DataValueField = "ID";
            this.ddlDocumentCode.DataBind();
            //document class
            var documentclasslist = this._DocumentClassService.GetAll();
            documentclasslist.Insert(0, new DocumentClass() { ID = 0, Code = string.Empty });
            this.ddlDocumentclass.DataSource = documentclasslist;
            this.ddlDocumentclass.DataTextField = "Code";
            this.ddlDocumentclass.DataValueField = "ID";
            this.ddlDocumentclass.DataBind();

            //Confdentiality
            var Confdentialitylist = this._ConfidentialityService.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault());
            Confdentialitylist.Insert(0, new Confidentiality() { ID = 0, Code = string.Empty });
            this.ddlConfidentiality.DataSource = Confdentialitylist;
            this.ddlConfidentiality.DataTextField = "Code";
            this.ddlConfidentiality.DataValueField = "ID";
            this.ddlConfidentiality.DataBind();

            
        }

        private void CollectData(ref DQREDocument docObj)
        {
            docObj.DocumentNo = this.txtDocNumber.Text;
            docObj.DocumentTitle = this.txtDocumentTitle.Text ;
            docObj.ProjectCodeId = this.ddlProjectCode.SelectedItem != null ?
                 Convert.ToInt32(this.ddlProjectCode.SelectedValue.ToString()): 0;
            docObj.ProjectCodeName = this.ddlProjectCode.SelectedItem != null ?
                this.ddlProjectCode.SelectedItem.Text
                : string.Empty;
             docObj.ContractorDocNo= this.txtContractornumber.Text ;
             docObj.RevisionSchemaId= this.ddlRevisionScheme.SelectedItem!= null?
                Convert.ToInt32(this.ddlRevisionScheme.SelectedValue)
                :0;
            docObj.RevisionSchemaName = this.ddlRevisionScheme.SelectedItem != null ?
                this.ddlRevisionScheme.SelectedItem.Text
                : string.Empty;

             docObj.Revision= this.txtRevision.Text ;
             docObj.IsssuedDate= this.txtIssueDate.SelectedDate ;
             docObj.DocumentClassId=this.ddlDocumentclass.SelectedItem!= null?
                Convert.ToInt32( this.ddlDocumentclass.SelectedValue)
                :0;
            docObj.DocumentClassName = this.ddlDocumentclass.SelectedItem != null ?
                this.ddlDocumentclass.SelectedItem.Text
                : string.Empty;

             docObj.DocumentCodeId=this.ddlDocumentCode.SelectedItem!= null?
                Convert.ToInt32(this.ddlDocumentCode.SelectedValue )
                :0;
            docObj.DocumentCodeName = this.ddlDocumentCode.SelectedItem != null ?
                this.ddlDocumentCode.SelectedItem.Text
                : string.Empty;

             docObj.RevisionStatusId=this.ddlRevisionStartus.SelectedItem!= null?
                Convert.ToInt32(this.ddlRevisionStartus.SelectedValue)
                :0;
            docObj.RevisionStatusName = this.ddlRevisionStartus.SelectedItem != null ?
                this.ddlRevisionStartus.SelectedItem.Text
                : string.Empty;

             docObj.Remark= this.txtNotes.Text ;
             docObj.ConfidentialityId= this.ddlConfidentiality.SelectedItem!= null?
                Convert.ToInt32(this.ddlConfidentiality.SelectedValue )
                :0;
            docObj.ConfidentialityName = this.ddlConfidentiality.SelectedItem != null ?
                this.ddlConfidentiality.SelectedItem.Text
                : string.Empty;

            Guid docId;
            Guid.TryParse(this.ddlSystem.SelectedValue.ToString(), out docId);
            var MasterObj = this._DQREDocumentMasterServce.GetById(docId);
            if (MasterObj != null)
            {
                docObj.M_SystemDocumentNo = MasterObj.SystemDocumentNo;
                docObj.DocumentMasterId = MasterObj.ID;
                docObj.M_EquipmentTagName = MasterObj.EquipmentTagName;
                docObj.M_DepartmentCode = MasterObj.DepartmentCode;
                docObj.M_MRSequenceNo = MasterObj.MRSequenceNo;
                docObj.M_DocumentSequenceNo = MasterObj.DocumentSequenceNo;
                docObj.M_SheetNo = MasterObj.SheetNo;
                docObj.M_OriginatorId = MasterObj.OriginatorId;
                docObj.M_OriginatorName = MasterObj.OriginatorName;
                docObj.M_OriginatingOrganizationId = MasterObj.OriginatingOrganizationId;
                docObj.M_OriginatingOrganizationName = MasterObj.OriginatingOrganizationName;
                docObj.M_ReceivingOrganizationId = MasterObj.ReceivingOrganizationId;
                docObj.M_ReceivingOrganizationName = MasterObj.ReceivingOrganizationName;
                docObj.M_DocumentTypeId = MasterObj.DocumentTypeId;
                docObj.M_DocumentTypeName = MasterObj.DocumentTypeName;
                docObj.M_DisciplineId = MasterObj.DisciplineId;
                docObj.M_DisciplineName = MasterObj.DisciplineName;
                docObj.M_MaterialCodeId = MasterObj.MaterialCodeId;
                docObj.M_MaterialCodeName = MasterObj.MaterialCodeName;
                docObj.M_WorkCodeId = MasterObj.WorkCodeId;
                docObj.M_WorkCodeName = MasterObj.WorkCodeName;
                docObj.M_DrawingCodeId = MasterObj.DrawingCodeId;
                docObj.M_DrawingCodeName = MasterObj.DrawingCodeName;
                docObj.M_UnitId = MasterObj.UnitId;
                docObj.M_UnitName = MasterObj.UnitName;
                docObj.M_AreaId = MasterObj.AreaId;
                docObj.M_AreaName = MasterObj.AreaName;
                docObj.M_PlantId = MasterObj.PlantId;
                docObj.M_PlantName = MasterObj.PlantName;
            }
        }

        private void LoadDocInfo(DQREDocument docObj)
        {
            this.txtDocNumber.Text = docObj.DocumentNo;
            this.txtDocumentTitle.Text = docObj.DocumentTitle;
            this.txtOriginator.Text = docObj.M_OriginatorName;
            this.txtOriginating.Text = docObj.M_OriginatingOrganizationName;
            this.txtReceiving.Text= docObj.M_ReceivingOrganizationName;
            this.txtDocumenType.Text = docObj.M_DocumentTypeName;
            this.txtDiscipline.Text = docObj.M_DisciplineName;
            this.txtMaterial.Text = docObj.M_MaterialCodeName;
            this.txtWork.Text = docObj.M_WorkCodeName;
            this.txtDrawing.Text = docObj.M_DrawingCodeName;
            this.txtEquipmentTagNumber.Text = docObj.M_EquipmentTagName;
            this.txtDepartmentcode.Text = docObj.M_DepartmentCode;
            this.txtMRSequenceNo.Text = docObj.M_MRSequenceNo;
            this.txtDocumentSequenceNo.Text = docObj.M_DocumentSequenceNo;
            this.txtsheetno.Text = docObj.M_SheetNo;
            this.txtPlant.Text = docObj.M_PlantName;
            this.txtArea.Text= docObj.M_AreaName;
            this.txtUnit.Text = docObj.M_UnitName;
            this.ddlProjectCode.SelectedValue = docObj.ProjectCodeId.ToString();
            this.txtContractornumber.Text = docObj.ContractorDocNo;
            this.ddlRevisionScheme.SelectedValue = docObj.RevisionSchemaId.ToString();
            this.txtRevision.Text = docObj.Revision;
            this.txtIssueDate.SelectedDate = docObj.IsssuedDate;
            this.ddlDocumentclass.SelectedValue = docObj.DocumentClassId.ToString();
            this.ddlDocumentCode.SelectedValue = docObj.DocumentCodeId.ToString();
            this.ddlRevisionStartus.SelectedValue = docObj.RevisionStatusId.ToString();
            this.txtNotes.Text = docObj.Remark;
            this.ddlConfidentiality.SelectedValue = docObj.ConfidentialityId.ToString();
            this.ddlSystem.SelectedValue = docObj.DocumentMasterId.ToString();
        }

        protected void ddlSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid docId;
            Guid.TryParse(this.ddlSystem.SelectedValue.ToString(), out docId);
            var docObj = this._DQREDocumentMasterServce.GetById(docId);
            if (docObj != null)
            {
                this.txtOriginator.Text = docObj.OriginatorName;
                this.txtOriginating.Text = docObj.OriginatingOrganizationName;
                this.txtReceiving.Text = docObj.ReceivingOrganizationName;
                this.txtDocumenType.Text = docObj.DocumentTypeName;
                this.txtDiscipline.Text = docObj.DisciplineName;
                this.txtMaterial.Text = docObj.MaterialCodeName;
                this.txtWork.Text = docObj.WorkCodeName;
                this.txtDrawing.Text = docObj.DrawingCodeName;
                this.txtEquipmentTagNumber.Text = docObj.EquipmentTagName;
                this.txtDepartmentcode.Text = docObj.DepartmentCode;
                this.txtMRSequenceNo.Text = docObj.MRSequenceNo;
                this.txtDocumentSequenceNo.Text = docObj.DocumentSequenceNo;
                this.txtsheetno.Text = docObj.SheetNo;
                this.txtPlant.Text = docObj.PlantName;
                this.txtArea.Text = docObj.AreaName;
                this.txtUnit.Text = docObj.UnitName;
                if (string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.txtDocumentTitle.Text = docObj.Title;
                }

            }
        }

        protected void ddlProjectCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.ddlSystem.SelectedItem != null)
            {
                this.txtDocNumber.Text = this.ddlProjectCode.SelectedItem.Text.Split(',')[0] + "-" + this.ddlSystem.SelectedItem.Text;
            }
        }
    }
}