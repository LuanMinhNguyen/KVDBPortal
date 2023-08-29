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
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PaymentHistoryPage : Page
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
        /// The document package service.
        /// </summary>
        private readonly ContractService contractService;

        private readonly PaymentHistoryService paymentHistoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PaymentHistoryPage()
        {
            this.documentService = new DocumentService();
            this.contractService = new ContractService();
            this.paymentHistoryService = new PaymentHistoryService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
                {
                    var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);
                    var contractObj = this.contractService.GetById(contractId);
                    if (contractObj != null)
                    {
                        this.txtContractNumber.Text = contractObj.Number;
                        this.txtContractContent.Text = contractObj.ContractContent;
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
            if (this.Page.IsValid && !string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
            {
                var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);
                var contractObj = this.contractService.GetById(contractId);

                if (Session["EditingId"] != null)
                {
                    var paymentHistoryId = Convert.ToInt32(Session["EditingId"]);
                    var paymentHistoryObj = this.paymentHistoryService.GetById(paymentHistoryId);
                    if (paymentHistoryObj != null)
                    {
                        this.CollectData(ref paymentHistoryObj, contractObj);

                        paymentHistoryObj.UpdatedBy = UserSession.Current.User.Id;
                        paymentHistoryObj.UpdatedDate = DateTime.Now;

                        this.paymentHistoryService.Update(paymentHistoryObj);

                        this.UpdatePaymentForContract(contractObj);
                    }

                    Session.Remove("EditingId");
                }
                else
                {
                    var paymentHistoryObj = new PaymentHistory();

                    paymentHistoryObj.CreatedBy = UserSession.Current.User.Id;
                    paymentHistoryObj.CreatedDate = DateTime.Now;

                    this.CollectData(ref paymentHistoryObj, contractObj);
                    this.paymentHistoryService.Insert(paymentHistoryObj);

                    this.UpdatePaymentForContract(contractObj);
                }

                this.ClearData();
                this.grdDocument.Rebind();
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

        private void CollectData(ref PaymentHistory paymentHistoryObj, Contract contractObj)
        {
            paymentHistoryObj.ContractID = contractObj.ID;
            paymentHistoryObj.ContractName = contractObj.Number;
            paymentHistoryObj.Name = this.txtPaymentName.Text.Trim();
            paymentHistoryObj.PlanDate = this.txtPaymentPlanDate.SelectedDate;
            paymentHistoryObj.PlanValueVND = this.txtPaymentPlanVND.Value;
            paymentHistoryObj.PlanValueUSD = this.txtPaymentPlanUSD.Value;
            paymentHistoryObj.ActualDate = this.txtPaymentActualDate.SelectedDate;
            paymentHistoryObj.ActualValueVND = this.txtPaymentActualVND.Value;
            paymentHistoryObj.ActualValueUSD = this.txtPaymentActualUSD.Value;
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var paymentHistoryId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            this.paymentHistoryService.Delete(paymentHistoryId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            if (e.CommandName == "EditCmd")
            {
                var paymentHistoryId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                var paymentHistoryObj = this.paymentHistoryService.GetById(paymentHistoryId);
                if (paymentHistoryObj != null)
                {
                    Session.Add("EditingId", paymentHistoryObj.ID);

                    this.txtPaymentName.Text = paymentHistoryObj.Name;
                    this.txtPaymentPlanDate.SelectedDate = paymentHistoryObj.PlanDate;
                    this.txtPaymentPlanVND.Value = paymentHistoryObj.PlanValueVND;
                    this.txtPaymentPlanUSD.Value = paymentHistoryObj.PlanValueUSD;

                    this.txtPaymentActualDate.SelectedDate = paymentHistoryObj.ActualDate;
                    this.txtPaymentActualVND.Value = paymentHistoryObj.ActualValueVND;
                    this.txtPaymentActualUSD.Value = paymentHistoryObj.ActualValueUSD;
                }
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
            {
                this.grdDocument.DataSource =
                    this.paymentHistoryService.GetAllByContract(Convert.ToInt32(this.Request.QueryString["contractId"]));
            }
            else
            {
                this.grdDocument.DataSource = new List<PaymentHistory>();
            }
        }

        private void ClearData()
        {
            this.txtPaymentName.Text = string.Empty;
            this.txtPaymentPlanDate.SelectedDate = null;
            this.txtPaymentPlanVND.Value = null;
            this.txtPaymentPlanUSD.Value = null;

            this.txtPaymentActualDate.SelectedDate = null;
            this.txtPaymentActualVND.Value = null;
            this.txtPaymentActualUSD.Value = null;
            Session.Remove("EditingId");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearData();
        }

        private void UpdatePaymentForContract(Contract contractObj)
        {
            var paymentList = this.paymentHistoryService.GetAllByContract(contractObj.ID);
            var paymentVND = 0.0;
            var paymentUSD = 0.0;

            paymentVND = paymentList.Aggregate(paymentVND, (current, t) => current + t.ActualValueVND.GetValueOrDefault());
            paymentUSD = paymentList.Aggregate(paymentUSD, (current, t) => current + t.ActualValueUSD.GetValueOrDefault());

            contractObj.PaymentedValueUSD = paymentUSD;
            contractObj.PaymentedValueVND = paymentVND;
            contractObj.PaymentValueUSDExchange = contractObj.PaymentedValueUSD +
                                                  contractObj.PaymentedValueVND/contractObj.ExchangeRate;

            contractObj.RemainPaymentUSDExchange = contractObj.ContractTotalValue.GetValueOrDefault() -
                                                   contractObj.PaymentValueUSDExchange;

            this.contractService.Update(contractObj);
        }
    }
}