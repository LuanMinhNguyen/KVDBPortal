// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Business.Services.Document;
    using Business.Services.Library;
    using Data.Entities;
    using Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestAttachDocFileEditForm : Page
    {
        private readonly ChangeRequestDocFileService ChangeRequestDocFileService;

        private readonly DocumentTypeService documentTypeService;

        private readonly ProjectCodeService projectService;

        private readonly UnitService unitService;

        private readonly DisciplineService disciplineService;

        private readonly DocumentCodeServices documentCodeServices;

        private readonly KKSIdentificationCodeService kksService;

        private readonly OrganizationCodeService organizationCodeService;

        private readonly GroupCodeService groupCodeService; 

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestAttachDocFileEditForm()
        {
            this.projectService = new ProjectCodeService();
            this.ChangeRequestDocFileService = new ChangeRequestDocFileService();
            this.documentTypeService = new DocumentTypeService();
            this.unitService = new UnitService();
            this.disciplineService = new DisciplineService();
            this.documentCodeServices = new DocumentCodeServices();
            this.kksService = new KKSIdentificationCodeService();
            this.groupCodeService = new GroupCodeService();
            this.organizationCodeService = new OrganizationCodeService();
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
                    var obj = this.ChangeRequestDocFileService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {
                        this.txtFileName.Text = obj.FileName;
                        this.txtDOcumentNo.Text = obj.DocumentNo;
                        this.ddlProjectCode.SelectedValue = obj.ProjectId.ToString();
                        this.txtRevision.Text = obj.Revision;
                        this.txtDocumentTitle.Text = obj.DocumentTitle;
                        this.ddlUnitCode.SelectedValue = obj.UnitCodeId.ToString();
                        this.ddlDocumentType.SelectedValue = obj.DocumentTypeId.ToString();
                        this.ddlDiscipline.SelectedValue = obj.DisciplineCodeId.ToString();
                        this.txtSequence.Text = obj.Sequence;
                        this.txtContractorRefNo.Text = obj.ContractorRefNo;
                        this.ddlKKSCode.SelectedValue = obj.KKSCodeId.ToString();
                        this.txtTrainNo.Text = obj.TrainNo;
                        this.ddlOriginatingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString();
                        this.ddlReceivingOrganization.SelectedValue = obj.ReceivingOrganizationId.ToString();
                        this.ddlGroup.SelectedValue = obj.GroupCodeId.ToString();
                        this.txtRevRemark.Text = obj.RevRemark;
                        this.ddlPurpose.SelectedValue = obj.PurposeId.ToString();

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
                                        this.ddlOriginatingOrganization.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "3":
                                        this.ddlReceivingOrganization.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "4":
                                        this.ddlGroup.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "5":
                                        this.ddlUnitCode.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "6":
                                        this.ddlKKSCode.CssClass = "min25Percent qlcbFormRequired";
                                        break;
                                    case "7":
                                        this.ddlDiscipline.CssClass = "min25Percent qlcbFormRequired";
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
                    var obj = this.ChangeRequestDocFileService.GetById(objId);
                    if (obj != null)
                    {
                        this.CollectData(obj);
                        this.ChangeRequestDocFileService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ChangeRequestDocFile();
                    obj.ID = Guid.NewGuid();
                    this.CollectData(obj);
                    this.ChangeRequestDocFileService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(ChangeRequestDocFile obj)
        {
            obj.Revision = this.txtRevision.Text;
            obj.DocumentTitle = this.txtDocumentTitle.Text;
            obj.ContractorRefNo = this.txtContractorRefNo.Text;
            obj.PurposeId = Convert.ToInt32(this.ddlPurpose.SelectedValue);
            obj.PurposeName = this.ddlPurpose.SelectedItem.Text;
            obj.RevRemark = this.txtRevRemark.Text;
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

            var unitCodeList = this.unitService.GetAll().OrderBy(t => t.Code).ToList();
            unitCodeList.Insert(0, new Unit() { ID = 0 });
            this.ddlUnitCode.DataSource = unitCodeList;
            this.ddlUnitCode.DataTextField = "Code";
            this.ddlUnitCode.DataValueField = "ID";
            this.ddlUnitCode.DataBind();

            var doctypeList = this.documentTypeService.GetAll().OrderBy(t => t.Code).ToList();
            doctypeList.Insert(0, new DocumentType() { ID = 0 });
            this.ddlDocumentType.DataSource = doctypeList;
            this.ddlDocumentType.DataTextField = "Code";
            this.ddlDocumentType.DataValueField = "ID";
            this.ddlDocumentType.DataBind();

            var disciplineList = this.disciplineService.GetAll().OrderBy(t => t.Code).ToList();
            disciplineList.Insert(0, new Discipline() { ID = 0 });
            this.ddlDiscipline.DataSource = disciplineList;
            this.ddlDiscipline.DataTextField = "Code";
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataBind();

            var kksList = this.kksService.GetAll();
            kksList.Insert(0, new KKSIdentificationCode() {ID = 0});
            this.ddlKKSCode.DataSource = kksList.OrderBy(t => t.Code);
            this.ddlKKSCode.DataTextField = "Code";
            this.ddlKKSCode.DataValueField = "ID";
            this.ddlKKSCode.DataBind();

            var organizationList = this.organizationCodeService.GetAll();
            organizationList.Insert(0, new OrganizationCode() {ID = 0});
            this.ddlOriginatingOrganization.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlOriginatingOrganization.DataTextField = "Code";
            this.ddlOriginatingOrganization.DataValueField = "ID";
            this.ddlOriginatingOrganization.DataBind();

            this.ddlReceivingOrganization.DataSource = organizationList.OrderBy(t => t.Code);
            this.ddlReceivingOrganization.DataTextField = "Code";
            this.ddlReceivingOrganization.DataValueField = "ID";
            this.ddlReceivingOrganization.DataBind();

            var groupList = this.groupCodeService.GetAll();
            groupList.Insert(0, new GroupCode() {ID = 0});
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "Code";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();

            var purposeList = this.documentCodeServices.GetAllActionCode();
            purposeList.Insert(0, new DocumentCode() { ID = 0 });
            this.ddlPurpose.DataSource = purposeList.OrderBy(t => t.Code);
            this.ddlPurpose.DataTextField = "FullName";
            this.ddlPurpose.DataValueField = "ID";
            this.ddlPurpose.DataBind();
        }
    }
}