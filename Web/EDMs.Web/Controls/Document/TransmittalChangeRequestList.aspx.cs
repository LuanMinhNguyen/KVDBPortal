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

    using EDMs.Business.Services.Document;
    using EDMs.Data.Entities;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TransmittalChangeRequestList : Page
    {
        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly PECC2TransmittalService transmittalService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly ChangeRequestService changeRequestService;


        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public TransmittalChangeRequestList()
        {
            this.transmittalService = new PECC2TransmittalService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.changeRequestService = new ChangeRequestService();
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
            if (Request.QueryString["objId"] != null)
            {
                var objId = new Guid(Request.QueryString["objId"]);
                var changeRequestList = new List<ChangeRequest>();

                var attachDocList = this.attachDocToTransmittalService.GetAllByTransId(objId);
                foreach (var item in attachDocList)
                {
                    var obj = this.changeRequestService.GetById(item.DocumentId.GetValueOrDefault());
                    if (obj != null)
                    {
                        changeRequestList.Add(obj);
                    }
                }

                this.grdDocument.DataSource = changeRequestList;
            }
        }

        protected void grdDocument_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            //GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            //switch (e.DetailTableView.Name)
            //{
            //    case "DocDetail":
            //        {
            //            var docId = Convert.ToInt32(dataItem.GetDataKeyValue("ID").ToString());
            //            e.DetailTableView.DataSource = this.attachFilesPackageService.GetAllDocumentFileByDocId(docId);
            //            break;
            //        }
            //}
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