// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Asset
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class AssetList : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

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
            Session.Add("SelectedMainMenu", "Quản Lý Tài Sản");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
            }
        }

        protected void grdData_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var assetList = Session["AssestList"] as List<AssetDto>;

            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var dataList = new List<AssetDto>();
            ds = this.eamService.GetDataSet("get_asset_r5", new[] { userParam });
            if (ds != null)
            {
                dataList = this.eamService.CreateListFromTable<AssetDto>(ds.Tables[0]);
            }

            grdData.DataSource = dataList;
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdData.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdData.Height = Unit.Percentage(((CheckBox)sender).Checked ? 75 : 80);
            this.grdData.Rebind();
        }

        protected void grdData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                string trangThai = dataItem["TrangThai"].Text;
                Label lbTrangThai = dataItem["lbTrangThai"].FindControl("lbTrangThai") as Label;
                lbTrangThai.Text = trangThai == "I" ? "Đã lắp đặt" : "Đã gỡ bỏ";
            }
        }
    }
}

