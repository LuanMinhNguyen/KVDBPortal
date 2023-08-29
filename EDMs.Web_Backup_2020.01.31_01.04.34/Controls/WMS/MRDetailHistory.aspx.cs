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
using System.Drawing;
using System.Web.UI;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using Telerik.Web.UI;
using Image = System.Web.UI.WebControls.Image;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MRDetailHistory : Page
    {
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly MaterialRequisitionDetailService mrDetailService;



        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingPunchRevisionHistory"/> class.
        /// </summary>
        public MRDetailHistory()
        {
            this.mrDetailService = new MaterialRequisitionDetailService();

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
            //    this.grdMaterials.MasterTableView.GetColumn("DeleteColumn").Visible = false;
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
        protected void grdMaterials_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if(Request.QueryString["objId"] != null)
            {
                var objId = new Guid(Request.QueryString["objId"]);
                var obj = this.mrDetailService.GetById(objId);
                if (obj != null)
                {
                    var revList = this.mrDetailService.GetAllMRDetailRev(obj.ParentId == null ? obj.ID : obj.ParentId.Value);
                    this.grdMaterials.DataSource = revList;
                }
                else
                {
                    this.grdMaterials.DataSource = new List<TrackingPunch>(); 
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
        protected void grdMaterials_ItemCreated(object sender, GridItemEventArgs e)
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
                grdMaterials.MasterTableView.SortExpressions.Clear();
                grdMaterials.MasterTableView.GroupByExpressions.Clear();
                grdMaterials.Rebind();
            }
        }

        protected void grdMaterials_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["IsCancel"].Text == "True")
                {
                    item["Status"].BackColor = Color.Red;
                    item["Status"].BorderColor = Color.Red;
                }
            }
        }
    }
}