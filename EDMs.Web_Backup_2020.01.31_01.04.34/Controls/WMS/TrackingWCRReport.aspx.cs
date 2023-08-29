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
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TrackingWCRReport : Page
    {
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly TrackingWCRService pmService;

        private readonly TrackingWCRAttachFileService pmAttachFileService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingWCRReport"/> class.
        /// </summary>
        public TrackingWCRReport()
        {
            this.pmAttachFileService = new TrackingWCRAttachFileService();
            this.pmService = new TrackingWCRService();

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
            if(Request.QueryString["stausName"] != null && Request.QueryString["projId"] != null)
            {
                var statusName = Request.QueryString["stausName"];
                var projId = Convert.ToInt32(Request.QueryString["projId"]);
                var pmListFilter = new List<TrackingWCR>();
                var pmList = this.pmService.GetAllByProject(projId);

                switch (statusName)
                {
                    case "Closed":
                        pmListFilter = pmList.Where(t => t.Status == "Closed").ToList();
                        break;
                    case "Open":
                        pmListFilter = pmList.Where(t => t.Status == "Open").ToList();
                        break;
                    case "Cancel":
                        pmListFilter = pmList.Where(t => t.Status == "Cancel").ToList();
                        break;
                }
                    this.grdDocument.DataSource = pmListFilter;
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
                        e.DetailTableView.DataSource = this.pmAttachFileService.GetAllObjId(objId);
                        break;
                    }
            }
        }
    }
}