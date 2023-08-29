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
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MREditForm : Page
    {
        private readonly MRTypeService mrTypeService;

        private readonly PriorityLevelService priorityLevelService;

        private readonly RoleService roleService;

        private readonly MaterialRequisitionService mrService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService;

        private readonly FunctionPermissionService fncPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MREditForm"/> class.
        /// </summary>
        public MREditForm()
        {
            this.mrTypeService = new MRTypeService();
            this.priorityLevelService = new PriorityLevelService();
            this.roleService = new RoleService();
            this.mrService = new MaterialRequisitionService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.fncPermissionService = new FunctionPermissionService();
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
                this.GetFuncPermissionConfig();
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var mrObj = this.mrService.GetById(new Guid(this.Request.QueryString["mrId"]));
                    if(mrObj!=null)
                    {
                        this.txtAMOSNo.Text = mrObj.AMOSWorkOrder;
                        this.txtMRNo.Text = mrObj.MRNo;
                        //this.ddlDepartment.SelectedValue = mrObj.DeparmentId.ToString();
                        this.txtJustification.Text = mrObj.Justification;
                        this.txtDepartment.Text = mrObj.DepartmentName;
                        foreach (RadListBoxItem item in this.lbMRType.Items)
                        {
                            item.Checked = !string.IsNullOrEmpty(mrObj.MRTypeIds) && mrObj.MRTypeIds.Split(';').Contains(item.Value);
                        }

                        this.txtDateRequire.Text = mrObj.DateRequire;
                        this.ddlPriorityLvl.SelectedValue = mrObj.PriorityId.ToString();
                        this.txtOriginator.Text = mrObj.OriginatorName;
                        this.txtDateRaised.Text = mrObj.OriginatorDate;
                        this.lblCreated.Text = "Created at " + mrObj.CreatedByDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + mrObj.CreatedByName;
                        if (mrObj.UpdatedBy != null && mrObj.UpdatedByDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            this.lblUpdated.Text = "Last modified at " + mrObj.UpdatedByDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + mrObj.UpdatedByName;
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.txtDateRaised.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                    var projectObj = this.projectService.GetById(projectId);
                    var currentMRIndex = this.numberManagementService.GetByName("MaterialRequisition", projectId);
                    var mrNo = projectObj.Code + "/" + Utility.ReturnSequenceString(currentMRIndex.NextCount.GetValueOrDefault(), 4) + "/" + DateTime.Now.ToString("yy");
                    this.txtMRNo.Text = mrNo;

                    this.CreatedInfo.Visible = false;
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 1);
            if (fncPermission != null)
            {
                this.IsView.Value = fncPermission.IsView.GetValueOrDefault().ToString();
                this.IsCreate.Value = fncPermission.IsCreate.GetValueOrDefault().ToString();
                this.IsUpdate.Value = fncPermission.IsUpdate.GetValueOrDefault().ToString();
                this.IsCancel.Value = fncPermission.IsCancel.GetValueOrDefault().ToString();
                this.IsAttachWF.Value = fncPermission.IsAttachWorkflow.GetValueOrDefault().ToString();
            }
            else
            {
                this.IsView.Value = "False";
                this.IsCreate.Value = "False";
                this.IsUpdate.Value = "False";
                this.IsCancel.Value = "False";
                this.IsAttachWF.Value = "False";
            }
            // ----------------------------------------------------------------------------------------
        }

        private void LoadInitData()
        {
            var mrTypeList = this.mrTypeService.GetAll();
            this.lbMRType.DataSource = mrTypeList;
            this.lbMRType.DataTextField = "Name";
            this.lbMRType.DataValueField = "ID";
            this.lbMRType.DataBind();

            var priorityLvl = this.priorityLevelService.GetAll();
            this.ddlPriorityLvl.DataSource = priorityLvl;
            this.ddlPriorityLvl.DataTextField = "Name";
            this.ddlPriorityLvl.DataValueField = "ID";
            this.ddlPriorityLvl.DataBind();

            //var offShoreDetpList = this.roleService.GetAll(false).Where(t => t.TypeId == 2);
            //this.ddlDepartment.DataSource = offShoreDetpList;
            //this.ddlDepartment.DataTextField = "Name";
            //this.ddlDepartment.DataValueField = "ID";
            //this.ddlDepartment.DataBind();

            if (UserSession.Current.User.RoleId != null)
            {
                this.txtDepartment.Text = UserSession.Current.User.RoleName;
            }

            var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
            var projectObj = this.projectService.GetById(projectId);

            this.txtFacility.Text = projectObj.Name;

            // Show hide function Control
            if (!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
            {
                this.btnSave.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            }
            else
            {
                this.btnSave.Visible = Convert.ToBoolean(this.IsCreate.Value);
            }
            
            // --------------------------------------------------------------------------
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
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projectObj = this.projectService.GetById(projectId);

                if (!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
                {
                    var mrId = new Guid(this.Request.QueryString["mrId"]);
                    var mrObj = this.mrService.GetById(mrId);
                    if (mrObj != null)
                    {
                        mrObj.ProjectId = projectObj.ID;
                        mrObj.ProjectName = projectObj.Name;

                        mrObj.AMOSWorkOrder = this.txtAMOSNo.Text.Trim();
                        mrObj.MRNo = this.txtMRNo.Text.Trim();
                        //mrObj.DeparmentId = Convert.ToInt32(this.ddlDepartment.SelectedValue);
                        mrObj.DepartmentName = this.txtDepartment.Text;
                        mrObj.Justification = this.txtJustification.Text.Trim();

                        foreach (RadListBoxItem item in this.lbMRType.CheckedItems)
                        {
                            mrObj.MRTypeIds += item.Value + ";";
                            mrObj.MRTypeName += item.Text + "; ";
                        }

                        mrObj.DateRequire = this.txtDateRequire.Text.Trim();
                        mrObj.PriorityId = Convert.ToInt32(this.ddlPriorityLvl.SelectedValue);
                        mrObj.PriorityName = this.ddlPriorityLvl.SelectedItem.Text;
                        mrObj.OriginatorName = this.txtOriginator.Text.Trim();
                        mrObj.OriginatorDate = this.txtDateRaised.Text;
                        mrObj.UpdatedBy = UserSession.Current.User.Id;
                        mrObj.UpdatedByName = UserSession.Current.User.FullName;
                        mrObj.UpdatedByDate = DateTime.Now;
                        this.mrService.Update(mrObj);
                    }
                }
                else
                {
                    var mrObj = new MaterialRequisition();
                    mrObj.ID = Guid.NewGuid();
                    mrObj.ProjectId = projectObj.ID;
                    mrObj.ProjectName = projectObj.Name;
                    mrObj.AMOSWorkOrder = this.txtAMOSNo.Text.Trim();
                    mrObj.MRNo = this.txtMRNo.Text.Trim();
                    mrObj.DepartmentName = this.txtDepartment.Text;
                    mrObj.Justification = this.txtJustification.Text.Trim();
                    mrObj.DateRequire = this.txtDateRequire.Text.Trim();
                    mrObj.PriorityId = Convert.ToInt32(this.ddlPriorityLvl.SelectedValue);
                    mrObj.PriorityName = this.ddlPriorityLvl.SelectedItem.Text;
                    mrObj.CreatedBy = UserSession.Current.User.Id;
                    mrObj.CreatedByDate = DateTime.Now;
                    mrObj.CreatedByName = UserSession.Current.User.FullName;
                    mrObj.IsWFComplete = false;
                    mrObj.IsInWFProcess = false;
                    mrObj.IsCompleteFinal = false;
                    mrObj.IsCancel = false;
                    mrObj.OriginatorName = this.txtOriginator.Text.Trim();
                    mrObj.OriginatorDate = this.txtDateRaised.Text;

                    foreach (RadListBoxItem item in this.lbMRType.CheckedItems)
                    {
                        mrObj.MRTypeIds += item.Value + ";";
                        mrObj.MRTypeName += item.Text + "; ";
                    }

                    this.mrService.Insert(mrObj);

                    var currentMRIndex = this.numberManagementService.GetByName("MaterialRequisition", projectId);
                    currentMRIndex.NextCount += 1;
                    this.numberManagementService.Update(currentMRIndex);
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
            if(this.txtMRNo.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter MR Number.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        
    }
}