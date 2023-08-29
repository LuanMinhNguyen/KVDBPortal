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
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TrackingBreakdownReportOverview : Page
    {
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly TrackingBreakdownReportService brService;

        private readonly TrackingBreakdownReportAttachFileService brAttachFileService;

        private readonly ObjectAssignedUserService objAssignedUserService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingProductionMeetingReport"/> class.
        /// </summary>
        public TrackingBreakdownReportOverview()
        {
            this.brAttachFileService = new TrackingBreakdownReportAttachFileService();
            this.brService = new TrackingBreakdownReportService();
            this.objAssignedUserService = new ObjectAssignedUserService();

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
            if (Request.QueryString["stausName"] != null && Request.QueryString["projId"] != null)
            {
                var fromDate = (DateTime?)Session["FromDate"];
                var toDate = (DateTime?)Session["ToDate"];

                var statusName = Request.QueryString["stausName"];
                var projId = Convert.ToInt32(Request.QueryString["projId"]);
                var brListFilter = new List<TrackingBreakdownReport>();

                var brList = this.brService.GetAll(projId, string.Empty).Where(t => (fromDate == null || t.CreatedDate >= fromDate)
                                                                                && (toDate == null || t.CreatedDate < toDate.Value.AddDays(1))).ToList();
                var mroverDueTask = this.objAssignedUserService.GetAllOverDueTask("Breakdown Report").Select(t => t.ObjectID).ToList();
                switch (statusName)
                {
                    case "Pending":
                        brListFilter = brList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                        break;
                    case "Overdue":
                        brListFilter = brList.Where(t => mroverDueTask.Contains(t.ID)).ToList();
                        break;
                    case "Canceled":
                        brListFilter = brList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                        break;
                    case "Completed":
                        brListFilter = brList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                        break;
                }
                    this.grdDocument.DataSource = brListFilter;
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
                        e.DetailTableView.DataSource = this.brAttachFileService.GetByBreakdownReport(objId);
                        break;
                    }
            }
        }
    }
}