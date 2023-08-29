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
using System.Web.UI;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MaterialListByContract : Page
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

        private readonly MaterialService materialService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public MaterialListByContract()
        {
            this.documentService = new DocumentService();
            this.contractService = new ContractService();
            this.paymentHistoryService = new PaymentHistoryService();
            this.materialService = new MaterialService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
                {
                    var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);
                    var contractObj = this.contractService.GetById(contractId);
                }
            }
        }

       

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
            {
                var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);

                this.grdDocument.DataSource = this.materialService.GetAllByContract(contractId);
            }
            else
            {
                this.grdDocument.DataSource = new List<Material>();
            }
        }
    }
}