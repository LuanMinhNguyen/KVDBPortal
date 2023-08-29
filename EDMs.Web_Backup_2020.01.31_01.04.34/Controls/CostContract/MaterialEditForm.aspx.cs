// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MaterialEditForm : Page
    {
        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly CostContractProjectService projectService;

        /// <summary>
        /// The document package service.
        /// </summary>
        private readonly ContractService contractService;

        private readonly UserService userService;

        private readonly ProcurementRequirementService prService;

        private readonly MaterialService materialService;
        private readonly PermissionContractService permissionContractService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public MaterialEditForm()
        {
            this.documentService = new DocumentService();
            this.projectService = new CostContractProjectService();
            this.contractService = new ContractService();
            this.userService = new UserService();
            this.prService = new ProcurementRequirementService();
            this.materialService = new MaterialService();
            this.permissionContractService = new PermissionContractService();
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
                ////this.LoadComboData();
                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    this.btnSave.Visible = false;
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["projId"]))
                {
                    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                    var projObj = this.projectService.GetById(projectId);
                    this.txtProjectName.Text = projObj.Name;
                    this.LoadComboData(projectId);
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["materialId"]))
                {
                    this.CreatedInfo.Visible = true;
                    var materialId = Convert.ToInt32(this.Request.QueryString["materialId"]);
                    var materialObj = this.materialService.GetById(materialId);
                    if (materialObj != null)
                    {
                        this.LoadComboData(materialObj.ProjectID.GetValueOrDefault());
                        this.txtProjectName.Text = materialObj.ProjectName;

                        this.LoadDocInfo(materialObj);

                        var createdUser = this.userService.GetByID(materialObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + materialObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (materialObj.UpdatedBy != null && materialObj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(materialObj.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + materialObj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
            //if (this.Page.IsValid && !string.IsNullOrEmpty(this.Request.QueryString["projId"]))
            //{
            //    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
            //    var projObj = this.projectService.GetById(projectId);
            //    Material materialObj;
            //    if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
            //    {
            //        var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
            //        materialObj = this.materialService.GetById(docId);
            //        if (materialObj != null)
            //        {

            //            this.CollectData(ref materialObj, projObj);

            //            materialObj.UpdatedBy = UserSession.Current.User.Id;
            //            materialObj.UpdatedDate = DateTime.Now;
            //            this.contractService.Update(materialObj);
            //        }
            //    }
            //    else
            //    {
            //        materialObj = new Contract()
            //        {
            //            CreatedBy = UserSession.Current.User.Id,
            //            CreatedDate = DateTime.Now,
            //        };

            //        this.CollectData(ref materialObj, projObj);
            //        this.contractService.Insert(materialObj);

            //    }

            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
            //}
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
            ////if(this.txtName.Text.Trim().Length == 0)
            ////{
            ////    this.fileNameValidator.ErrorMessage = "Please enter file name.";
            ////    this.divFileName.Style["margin-bottom"] = "-26px;";
            ////    args.IsValid = false;
            ////}
            ////else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            ////{
            ////    var docId = Convert.ToInt32(Request.QueryString["docId"]);
            ////    this.fileNameValidator.ErrorMessage = "The specified name is already in use.";
            ////    this.divFileName.Style["margin-bottom"] = "-26px;";
            ////    args.IsValid = true; ////!this.documentService.IsDocumentExistUpdate(folderId, this.txtName.Text.Trim(), docId);
            ////}
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
                var fileName = e.Argument.Split('$')[1];
                var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                
                if(this.documentService.IsDocumentExist(folderId, fileName))
                {
                }
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData(int projectId)
        {
            var contractList = new List<Contract>();
            if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
            {
                var listPRId =
                    this.prService.GetAllByProject(projectId)
                        .Select(t => t.ID)
                        .ToList();
                contractList = this.contractService.GetAllByPR(listPRId).OrderBy(t => t.Number).ToList();
            }
            else
            {
                var listPRId = this.prService.GetAllPRInPermission(UserSession.Current.User.Id, projectId).Select(t => t.ID).ToList();
                contractList = this.contractService.GetAllByPR(listPRId).OrderBy(t => t.Number).ToList();

                var materialIdListInPermission =
                    this.permissionContractService.GetAllByUser(UserSession.Current.User.Id)
                        .Select(t => t.ContractID)
                        .ToList();
                contractList = contractList.Where(t => materialIdListInPermission.Contains(t.ID)).ToList();
            }

            this.ddlContract.DataSource = contractList;
            this.ddlContract.DataTextField = "Number";
            this.ddlContract.DataValueField = "ID";
            this.ddlContract.DataBind();
        }

        private void CollectData(ref Material materialObj, Contract contractObj)
        {
            materialObj.ContractID = contractObj.ID;
            materialObj.ContractNumber = contractObj.Number;
            materialObj.Number = this.txtNumber.Text.Trim();
            materialObj.Quality = this.txtQuality.Value;
            materialObj.DeliveryPlan = this.txtDeliveryDate.SelectedDate;
            materialObj.DeliveryActual = this.txtDeliveryActual.SelectedDate;
            materialObj.DeliveryStatus = this.txtDeliveryStatus.Text.Trim();
            materialObj.Note = this.txtNote.Text.Trim();
            materialObj.Complete = this.txtComplete.Value;
            materialObj.Weight = this.txtWeight.Value;
        }

        private void LoadDocInfo(Material materialObj)
        {
            this.ddlContract.SelectedValue = materialObj.ContractID.ToString();
            this.txtNumber.Text = materialObj.Number;
            this.txtQuality.Value = materialObj.Quality;
            this.txtDeliveryDate.SelectedDate = materialObj.DeliveryPlan;
            this.txtDeliveryActual.SelectedDate = materialObj.DeliveryActual;
            this.txtDeliveryStatus.Text = materialObj.DeliveryStatus;
            this.txtNote.Text = materialObj.Note;
            this.txtComplete.Value = materialObj.Complete;
            this.txtWeight.Value = materialObj.Weight;
        }
    }
}