// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Business.Services.Document;
    using Business.Services.Library;
    using Data.Entities;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ContractorTransmittalAttachChangeRequestFileEditForm : Page
    {
        private readonly ContractorTransmittalService transmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly DQREDocumentService documentService;

        private readonly DocumentTypeService documentTypeService;

        private readonly DQREDocumentNumberingService documentNumberingService;

        private readonly ProjectCodeService projectService;

        private readonly UnitService unitService;

        private readonly DrawingService drawingCodeService;

        private readonly MaterialService materialCodeService;

        private readonly WorkService workCodeService;

        private readonly DisciplineService disciplineService;

        private readonly DocumentCodeServices documentCodeServices;

        private readonly DocumentClassService documentClassService;

        private readonly KKSIdentificationCodeService kksService;

        private readonly OrganizationCodeService organizationCodeService;

        private readonly GroupCodeService groupCodeService;

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestTypeService changeRequestTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractorTransmittalAttachChangeRequestFileEditForm()
        {
            this.transmittalService = new ContractorTransmittalService();
            this.projectService = new ProjectCodeService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
            this.documentService = new DQREDocumentService();
            this.documentTypeService = new DocumentTypeService();
            this.documentNumberingService = new DQREDocumentNumberingService();
            this.drawingCodeService = new DrawingService();
            this.unitService = new UnitService();
            this.workCodeService = new WorkService();
            this.disciplineService = new DisciplineService();
            this.documentCodeServices = new DocumentCodeServices();
            this.materialCodeService = new MaterialService();
            this.documentClassService = new DocumentClassService();
            this.kksService = new KKSIdentificationCodeService();
            this.groupCodeService = new GroupCodeService();
            this.organizationCodeService = new OrganizationCodeService();
            this.changeRequestService = new ChangeRequestService();
            this.changeRequestTypeService = new ChangeRequestTypeService();
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
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var obj = this.contractorTransmittalDocFileService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {
                        this.txtFileName.Text = obj.FileName;
                        this.txtDOcumentNo.Text = obj.DocumentNo;
                        this.ddlProjectCode.SelectedValue = obj.ProjectId.ToString();
                        this.txtDocumentTitle.Text = obj.DocumentTitle;
                        this.txtSequence.Text = obj.Sequence;
                        this.txtYear.Text = obj.Year;
                        this.ddlChangeRequestType.SelectedValue = obj.ChangeRequestTypeId.ToString();
                        this.ddlGroup.SelectedValue = obj.GroupCodeId.ToString();  


                        if (!string.IsNullOrEmpty(obj.ErrorPosition))
                        {
                            foreach (var position in obj.ErrorPosition.Split('$').Where(t => !string.IsNullOrEmpty(t)))
                            {
                                switch (position)
                                {
                                    case "0":
                                        this.txtDOcumentNo.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "1":
                                        this.ddlProjectCode.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "2":
                                        this.ddlChangeRequestType.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "3":
                                        this.ddlGroup.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                }
                            }
                        }
                    }
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
            if (this.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var obj = this.contractorTransmittalDocFileService.GetById(objId);
                    if (obj != null)
                    {
                        this.CollectData(obj);
                        this.contractorTransmittalDocFileService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ContractorTransmittalDocFile();
                    obj.ID = Guid.NewGuid();
                    this.CollectData(obj);
                    this.contractorTransmittalDocFileService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(ContractorTransmittalDocFile obj)
        {
            obj.DocumentTitle = this.txtDocumentTitle.Text;
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
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var projectList = this.projectService.GetAll().OrderBy(t => t.Code).ToList();
            projectList.Insert(0, new ProjectCode() {ID = 0});
            this.ddlProjectCode.DataSource = projectList;
            this.ddlProjectCode.DataTextField = "Code";
            this.ddlProjectCode.DataValueField = "ID";
            this.ddlProjectCode.DataBind();

            var typeList = this.changeRequestTypeService.GetAll().OrderBy(t => t.Code).ToList();
            typeList.Insert(0, new ChangeRequestType() { ID = 0 });
            this.ddlChangeRequestType.DataSource = typeList;
            this.ddlChangeRequestType.DataTextField = "Code";
            this.ddlChangeRequestType.DataValueField = "ID";
            this.ddlChangeRequestType.DataBind();

            var groupList = this.groupCodeService.GetAll();
            groupList.Insert(0, new GroupCode() {ID = 0});
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "Code";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();
        }
    }
}