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
    using System.Web.UI;
    using System.Linq;
    using EDMs.Business.Services.Document;
    using EDMs.Data.Entities;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TransmittalDocumentList : Page
    {
        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly PECC2TransmittalService transmittalService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly PECC2DocumentsService pecc2documentService;

        private readonly PECC2DocumentAttachFileService attachFilesPackageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public TransmittalDocumentList()
        {
            this.transmittalService = new PECC2TransmittalService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.pecc2documentService = new PECC2DocumentsService();
            this.attachFilesPackageService = new PECC2DocumentAttachFileService();
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
            if (!string.IsNullOrEmpty( Request.QueryString["objId"]))
            {
                var objId = new Guid(Request.QueryString["objId"]);
                var listDocument = new List<PECC2Documents>();

                var attachDocList = this.attachDocToTransmittalService.GetAllByTransId(objId);
                foreach (var item in attachDocList)
                {
                    var docObj = this.pecc2documentService.GetById(item.DocumentId.GetValueOrDefault());
                    if (docObj != null)
                    {
                        listDocument.Add(docObj);
                    }
                }

                this.grdDocument.DataSource = listDocument;
            }else if(!string.IsNullOrEmpty(Request.QueryString["RefDocNo"]))
            {
                var objId = new Guid(Request.QueryString["RefDocNo"]);
                
                 var listDocument = this.pecc2documentService.GetAllByReferChangeRequest(objId);

                this.grdDocument.DataSource = listDocument.OrderBy(t=> t.DocNo);
            }
        }

        protected void grdDocument_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "DocDetail":
                    {
                        Guid docId;
                        Guid.TryParse(dataItem.GetDataKeyValue("ID").ToString(), out docId);
                        e.DetailTableView.DataSource = this.attachFilesPackageService.GetAllDocumentFileByDocId(docId);
                        break;
                    }
            }
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = new Guid(item.GetDataKeyValue("ID").ToString());
            if (Request.QueryString["objId"] != null)
            {
                var objId = new Guid(Request.QueryString["objId"]);
                var transObj = this.transmittalService.GetById(objId);
                if (transObj != null)
                {
                    var temp = this.attachDocToTransmittalService.GetByDoc(objId, docId);
                    if (temp != null)
                    {
                        this.attachDocToTransmittalService.Delete(temp);
                    }
                }
            }

            this.grdDocument.Rebind();
        }
    }
}