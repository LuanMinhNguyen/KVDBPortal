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
using System.Web.UI.WebControls;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TrackingECRRevisionHistory : Page
    {
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly TrackingECRService ecrService;

        private readonly TrackingECRAttachFileService mcAttachFileService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingECRRevisionHistory"/> class.
        /// </summary>
        public TrackingECRRevisionHistory()
        {
            this.mcAttachFileService = new TrackingECRAttachFileService();
            this.ecrService = new TrackingECRService();

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
            //if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
            //{
            //    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
            //}
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
            if(Request.QueryString["objId"] != null)
            {
                var objId = new Guid(Request.QueryString["objId"]);
                var obj = this.ecrService.GetById(objId);
                if (obj != null)
                {
                    var revList = this.ecrService.GetAllECRRev(obj.ParentId == null ? obj.ID : obj.ParentId.Value);
                    this.grdDocument.DataSource = revList;
                }
                else
                {
                    this.grdDocument.DataSource = new List<TrackingECR>(); 
                }
            }
        }

        /// <summary>
        /// Grid KhacHang item created
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (Image)e.Item.FindControl("uploadLink");
                editLink.Attributes["href"] = "#";
                editLink.Attributes["onclick"] = string.Format(
                    "return ShowUploadForm('{0}');",
                    e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                grdDocument.Rebind();
            }
        }

        protected void grdDocument_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "DocDetail":
                    {
                        var objId = new Guid(dataItem.GetDataKeyValue("ID").ToString());
                        e.DetailTableView.DataSource = this.mcAttachFileService.GetAllObjId(objId);
                        break;
                    }
            }
        }
    }
}