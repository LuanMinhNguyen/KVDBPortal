// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PREditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly ProcurementRequirementService prService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ProcurementRequirementTypeService prTypeService;

        private readonly CostContractProjectService projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PREditForm()
        {
            this.userService = new UserService();
            this.prService = new ProcurementRequirementService();
            this.projectService = new CostContractProjectService();
            this.prTypeService = new ProcurementRequirementTypeService();
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
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["prId"]))
                {
                    var objPR = this.prService.GetById(Convert.ToInt32(this.Request.QueryString["prId"]));
                    this.CreatedInfo.Visible = objPR != null;

                    if (objPR != null)
                    {
                        this.txtNumber.Text = objPR.Number;
                        this.txtDescription.Text = objPR.Description;
                        this.ddlProject.SelectedValue = objPR.ProjectID.GetValueOrDefault().ToString();
                        this.txtCode.Text = objPR.Code;
                        this.txtMainPerform.Text = objPR.MainOwnerName;
                        this.txtContractorSelectionType.Text = objPR.ContractorChoiceTypeName;
                        this.txtProcurementPlan.Value = objPR.ProcurementPlanValue;
                        this.txtProcurementRequirement.Value = objPR.ProcurementRequirementValue;
                        this.txtUSDExchange.Value = objPR.USDExchangeValue;
                        this.ddlType.SelectedValue = objPR.TypeID.GetValueOrDefault().ToString();


                        var createdUser = this.userService.GetByID(objPR.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objPR.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objPR.UpdatedBy != null && objPR.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objPR.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objPR.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["prId"]))
                {
                    var contractorId = Convert.ToInt32(this.Request.QueryString["prId"]);
                    var obj = this.prService.GetById(contractorId);
                    if (obj != null)
                    {
                        this.CollectDate(ref obj);
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        this.prService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ProcurementRequirement();
                    this.CollectDate(ref obj);
                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedDate = DateTime.Now;
                    this.prService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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
            if(this.txtNumber.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Procurement requirement Number.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var projectList = this.projectService.GetAll();
            this.ddlProject.DataSource = projectList;
            this.ddlProject.DataTextField = "Name";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            if (projectList.Any())
            {
                var projectID = this.ddlProject.SelectedItem != null ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0;
                var prTypeList = this.prTypeService.GetAllByProject(projectID);
                this.ddlType.DataSource = prTypeList;
                this.ddlType.DataTextField = "FullName";
                this.ddlType.DataValueField = "ID";
                this.ddlType.DataBind();
            }
        }

        private void CollectDate(ref ProcurementRequirement objPR)
        {
            objPR.Number = this.txtNumber.Text.Trim();
            objPR.Description = this.txtDescription.Text.Trim();
            objPR.ProjectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            objPR.ProjectName = this.ddlProject.SelectedItem.Text;
            objPR.Code = this.txtCode.Text.Trim();
            objPR.MainOwnerName = this.txtMainPerform.Text.Trim();
            objPR.ContractorChoiceTypeName = this.txtContractorSelectionType.Text.Trim();
            objPR.ProcurementPlanValue = this.txtProcurementPlan.Value;
            objPR.ProcurementRequirementValue = this.txtProcurementRequirement.Value;
            objPR.USDExchangeValue = this.txtUSDExchange.Value;

            objPR.TypeID = this.ddlType.SelectedItem != null ? Convert.ToInt32(this.ddlType.SelectedValue) : 0;
            objPR.TypeName = this.ddlType.SelectedItem != null ? this.ddlType.SelectedItem.Text.Trim() : string.Empty;
        }
    }
}