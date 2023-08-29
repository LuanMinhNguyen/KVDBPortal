// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto.MaterialInventory;

namespace EDMs.Web
{
    using System;
    using System.Configuration;
    using System.Web.UI;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class EAM_StoresManagement : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Vật Tư");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
                this.GetStoreDataList();
                this.LoadStoreList();
            }
        }

        private void GetStoreDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var StoreList = new List<StoresDto>();
            ds = this.eamService.GetDataSet("get_Stores", new[] { userParam });
            if (ds != null)
            {
                StoreList = this.eamService.CreateListFromTable<StoresDto>(ds.Tables[0]);
                foreach (var item in StoreList)
                {
                    item.FullName = item.MaKho + " - " + item.DienGiai;
                }
            }

            Session.Add("StoreList", StoreList);
        }

        private void LoadStoreList()
        {
            var StoreList = Session["StoreList"] as List<StoresDto>;
            this.BindStoreData(StoreList);
            
        }

        private void BindStoreData(List<StoresDto> StoreList)
        {
            this.lbStore.Items.Clear();
            foreach (var item in StoreList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.MaKho);
                lbItem.Attributes.Add("DonVi", item.DonVi);
                this.lbStore.Items.Add(lbItem);
            }

            this.lbStore.DataBind();
        }

        private static int[] GetData()
        {
            var itemsCount = 5000;
            var arr = new int[itemsCount];

            for (var i = 0; i < itemsCount; i++)
            {
                arr[i] = i;
            }
            return arr;
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RefreshProgressReport")
            {
            }
            
        }

        private void FillStoreDetail(StoresDto item)
        {
            this.txtMaKho.Text = item.MaKho;
            this.txtDienGiai.Text = item.DienGiai;
            this.txtDonVi.Text = item.DonVi;
        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var StoreList = Session["StoreList"] as List<StoresDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterStoreList = StoreList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.MaKho != null && t.MaKho.ToUpper().Contains(searchText))
                                                        || (t.DienGiai != null && t.DienGiai.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindStoreData(filterStoreList);
            }
            else
            {
                this.BindStoreData(StoreList);
            }
        }

        protected void lbStore_OnSelectedIndexChangeddIndexChanged(object sender, EventArgs e)
        {
            var lblStoreName = this.ToolBarStore.Items[0].FindControl("lblStoreName") as Label;
            lblStoreName.Text = this.lbStore.SelectedItem.Text;
            var StoreList = Session["StoreList"] as List<StoresDto>;
            var selectedItem = StoreList.FirstOrDefault(t => t.MaKho == lbStore.SelectedValue);

            this.grdStock.Rebind();
            this.FillStoreDetail(selectedItem);
        }

        protected void grdStock_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            var storeParam = new SqlParameter("@MaKho", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            storeParam.Value = this.lbStore.SelectedValue;
            DataSet ds;
            var stockList = new List<StockDto>();
            ds = this.eamService.GetDataSet("get_Stock_r2", new[] { userParam, storeParam });
            if (ds != null)
            {
                stockList = this.eamService.CreateListFromTable<StockDto>(ds.Tables[0]);
            }

            this.grdStock.DataSource = stockList;
        }
    }
}

