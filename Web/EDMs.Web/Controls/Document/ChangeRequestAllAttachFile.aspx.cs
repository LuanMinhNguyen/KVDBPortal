// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Linq;
using Aspose.Cells;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using iTextSharp.text.pdf;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.UI;
    using EDMs.Business.Services.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using EDMs.Business.Services.Library;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestAllAttachFile : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly ChangeRequestService changeRequestervice;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        private readonly ChangeRequestTypeService changeRequestTypeService;



        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestAllAttachFile()
        {
            this.changeRequestervice = new  ChangeRequestService();
            this.changeRequestAttachFileService = new   ChangeRequestAttachFileService();
            this.changeRequestDocFileService = new ChangeRequestDocFileService();
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
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocumentAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                var docFileList = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestId).OrderBy(t => t.FileName);

                this.grdDocumentAttachFile.DataSource = docFileList;
            }
        }

        protected void grdChangeRequestAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var attachList = this.changeRequestAttachFileService.GetByChangeRequest(new Guid(this.Request.QueryString["objId"])).OrderBy(t => t.CreatedDate);
                this.grdChangeRequestAttachFile.DataSource = attachList.Where(t => !t.IsOnlyMarkupPage.GetValueOrDefault());
            }
            else
            {
                this.grdChangeRequestAttachFile.DataSource = new List<ChangeRequestAttachFile>();
            }
        }
    }
}