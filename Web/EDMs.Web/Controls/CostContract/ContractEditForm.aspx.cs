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
    public partial class ContractEditForm : Page
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
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractEditForm()
        {
            this.documentService = new DocumentService();
            this.projectService = new CostContractProjectService();
            this.contractService = new ContractService();
            this.userService = new UserService();
            this.prService = new ProcurementRequirementService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
                {
                    this.PaymentDiv.Visible = true;
                    this.CreatedInfo.Visible = true;
                    var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);
                    var contractObj = this.contractService.GetById(contractId);
                    if (contractObj != null)
                    {
                        this.LoadComboData(contractObj.ProjectID.GetValueOrDefault());
                        this.txtProjectName.Text = contractObj.ProjectName;

                        this.LoadDocInfo(contractObj);

                        var createdUser = this.userService.GetByID(contractObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + contractObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (contractObj.UpdatedBy != null && contractObj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(contractObj.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + contractObj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.PaymentDiv.Visible = false;
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
            if (this.Page.IsValid && !string.IsNullOrEmpty(this.Request.QueryString["projId"]))
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projObj = this.projectService.GetById(projectId);
                Contract contractObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
                    contractObj = this.contractService.GetById(docId);
                    if (contractObj != null)
                    {
                        
                        this.CollectData(ref contractObj, projObj);

                        contractObj.UpdatedBy = UserSession.Current.User.Id;
                        contractObj.UpdatedDate = DateTime.Now;
                        this.contractService.Update(contractObj);
                    }
                }
                else
                {
                    contractObj = new Contract()
                    {
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                    };

                    this.CollectData(ref contractObj, projObj);
                    this.contractService.Insert(contractObj);

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
            var listPRInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                ? this.prService.GetAllByProject(projectId)
                    .OrderBy(t => t.ID)
                    .ToList()
                : this.prService.GetAllPRHaveFullPermission(UserSession.Current.User.Id, projectId)
                    .OrderBy(t => t.ID).ToList();
            this.ddlPR.DataSource = listPRInPermission;
            this.ddlPR.DataTextField = "Number";
            this.ddlPR.DataValueField = "ID";
            this.ddlPR.DataBind();
        }

        private void CollectData(ref Contract contractObj, CostContractProject projObj)
        {
            contractObj.ProjectID = projObj.ID;
            contractObj.ProjectName = projObj.Name;
            contractObj.ProcurementRequirementID = this.ddlPR.SelectedItem != null
                                        ? Convert.ToInt32(this.ddlPR.SelectedValue)
                                        : 0;
            contractObj.ProcurementRequirementNumber = this.ddlPR.SelectedItem != null
                                          ? this.ddlPR.SelectedItem.Text
                                          : string.Empty;

            contractObj.Number = this.txtContractNumber.Text.Trim();
            contractObj.ContractorSelectedName = this.txtContractor.Text;
            contractObj.ContractContent = this.txtContractContent.Text;
            contractObj.DeliveryDate = this.txtDeliveryDate.SelectedDate;
            contractObj.DeliveryStatus = this.txtDeliveryStatus.Text;
            contractObj.EffectedDate = this.txtEffectedDate.SelectedDate;
            contractObj.EndDate = this.txtEndDate.SelectedDate;
            contractObj.ContractTypeName = this.txtContractType.Text;
            contractObj.ContractStausName = this.txtContractStatus.Text;
            contractObj.Note = this.txtNote.Text;

            contractObj.ExchangeRate = this.txtExchangeRate.Value;
            contractObj.ContractValueVND = this.txtContractValueVND.Value;
            contractObj.ContractValueUSD = this.txtContractValueUSD.Value;
            contractObj.ArisingTotalValue = this.txtArisingValue.Value;
            contractObj.ContractTotalValue = this.txtContractTotalValue.Value;

            contractObj.PaymentedValueVND = this.txtPaymentVND.Value;
            contractObj.PaymentedValueUSD = this.txtPaymentUSD.Value;
            contractObj.PaymentValueUSDExchange = this.txtPaymentUSDExchange.Value;
            contractObj.RemainPaymentVND = this.txtRemainVND.Value;
            contractObj.RemainPaymentUSD = this.txtRemainUSD.Value;
            contractObj.RemainPaymentUSDExchange = this.txtRemainUSDExchange.Value;
            contractObj.Complete = this.txtComplete.Value;
            contractObj.Weight = this.txtWeight.Value;
        }

        private void LoadDocInfo(Contract contractObj)
        {
            this.ddlPR.SelectedValue = contractObj.ProcurementRequirementID.ToString();
            this.txtContractNumber.Text = contractObj.Number;
            this.txtContractor.Text = contractObj.ContractorSelectedName;
            this.txtContractContent.Text = contractObj.ContractContent;
            this.txtDeliveryDate.SelectedDate = contractObj.DeliveryDate;
            this.txtDeliveryStatus.Text = contractObj.DeliveryStatus;
            this.txtEffectedDate.SelectedDate = contractObj.EffectedDate;
            this.txtEndDate.SelectedDate = contractObj.EndDate;
            this.txtContractType.Text = contractObj.ContractTypeName;
            this.txtContractStatus.Text = contractObj.ContractStausName;
            this.txtNote.Text = contractObj.Note;

            this.txtExchangeRate.Value = contractObj.ExchangeRate;
            this.txtContractValueVND.Value = contractObj.ContractValueVND;
            this.txtContractValueUSD.Value = contractObj.ContractValueUSD;
            this.txtArisingValue.Value = contractObj.ArisingTotalValue;
            this.txtContractTotalValue.Value = contractObj.ContractTotalValue;

            this.txtPaymentVND.Value = contractObj.PaymentedValueVND;
            this.txtPaymentUSD.Value = contractObj.PaymentedValueUSD;
            this.txtPaymentUSDExchange.Value = contractObj.PaymentValueUSDExchange;
            this.txtRemainVND.Value = contractObj.RemainPaymentVND;
            this.txtRemainUSD.Value = contractObj.RemainPaymentUSD;
            this.txtRemainUSDExchange.Value = contractObj.RemainPaymentUSDExchange;
            this.txtComplete.Value = contractObj.Complete;
            this.txtWeight.Value = contractObj.Weight;
        }

        protected void txtExchangeRate_OnTextChanged(object sender, EventArgs e)
        {
            this.txtContractTotalValue.Value = this.txtContractValueVND.Value.GetValueOrDefault() > 0
                ? this.txtArisingValue.Value.GetValueOrDefault() +
                  this.txtContractValueVND.Value.GetValueOrDefault()/this.txtExchangeRate.Value.GetValueOrDefault()
                : this.txtArisingValue.Value.GetValueOrDefault() + this.txtContractValueUSD.Value.GetValueOrDefault();
        }
    }
}