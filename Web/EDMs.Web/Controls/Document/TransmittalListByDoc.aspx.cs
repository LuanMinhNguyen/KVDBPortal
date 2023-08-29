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
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Telerik.Windows.Zip;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TransmittalListByDoc : Page
    {
        /// <summary>
        /// The document service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The status service.
        /// </summary>
        private readonly StatusService statusService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The received from.
        /// </summary>
        private readonly ReceivedFromService receivedFromService;

        private readonly DocPropertiesViewService docPropertiesViewService;

        private readonly DocumentNewService documentNewService;

        private readonly AttachFileService attachFileService;

        private readonly DocumentPackageService documentPackageService;

        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly AttachFilesPackageService attachFilePackageService;

        private readonly TransmittalService transmittalService;


        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public TransmittalListByDoc()
        {
            this.documentService = new DocumentService();
            this.revisionService = new RevisionService();
            this.documentTypeService = new DocumentTypeService();
            this.statusService = new StatusService();
            this.disciplineService = new DisciplineService();
            this.receivedFromService = new ReceivedFromService();
            this.docPropertiesViewService = new DocPropertiesViewService();
            this.documentNewService = new DocumentNewService();
            this.attachFileService = new AttachFileService();
            this.documentPackageService = new DocumentPackageService();
            this.attachFilePackageService = new AttachFilesPackageService();
            this.transmittalService = new TransmittalService();
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
            if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
            {
                ////this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                //this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                ////this.grdDocument.MasterTableView.GetColumn("ReUpload").Visible = false;
            }
        }

        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if(Request.QueryString["docId"] != null)
            {
                var docId = Convert.ToInt32(Request.QueryString["docId"]);
                //this.grdDocument.DataSource = this.transmittalService.GetAllByDocumentId(docId);
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                grdDocument.Rebind();
            }
        }
    }
}