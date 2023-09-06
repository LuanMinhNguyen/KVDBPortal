using EDMs.Data.Dto;
using EDMs.Web.Utilities.Sessions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Web.Controls.Material;
using EDMs.Data.Dto.Store;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Store
{
    public partial class StoreDetail : System.Web.UI.Page
    {
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("SelectedMainMenu", "Quản lý vật tư");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
                GetStoreDataList();
            }
        }

        private void GetStoreDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
            DataSet ds;
            var storeList = new List<StoreDto>();
            ds = this.eamService.GetDataSet("get_stores_r5", new[] { userParam });
            if (ds != null)
            {
                storeList = this.eamService.CreateListFromTable<StoreDto>(ds.Tables[0]);
                foreach (var item in storeList)
                {
                    item.FullName = item.Kho + " - " + item.TenKho;
                }
            }

            Session.Add("StoreList", storeList);
        }

        private void BindStoreData(List<StoreDto> storeList)
        {
            this.grdStore.DataSource = null;
            this.grdStore.DataSource = storeList;
            this.grdStore.DataBind();
        }

        protected void grdStore_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var storeList = Session["StoreList"] as List<StoreDto>;

            this.grdStore.DataSource = storeList;
        }

        protected void grdStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            var STORE_CODE = this.grdStore.SelectedItems[0].FindControl("hfStore") as HiddenField;
            var hfStoreFullName = this.grdStore.SelectedItems[0].FindControl("hfStoreFullName") as HiddenField;
            var lblStoreName = this.ToolBarAsset.Items[0].FindControl("lblStoreName") as Label;
            lblStoreName.Text = hfStoreFullName.Value;
            var storeList = Session["StoreList"] as List<StoreDto>;
            var storeObj = storeList.FirstOrDefault(t => t.Kho == STORE_CODE.Value);

            this.FillObjectDetail(storeObj);
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            var storeList = Session["StoreList"] as List<StoreDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterStoreList = storeList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.TenKho != null && t.TenKho.ToUpper().Contains(searchText))
                                                        || (t.KhuVuc != null && t.KhuVuc.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.PPTinhGia != null && t.PPTinhGia.ToUpper().Contains(searchText))
                                                        || (t.Kho != null && t.Kho.ToUpper().Contains(searchText))
                                                        || (t.ViTri != null && t.ViTri.ToUpper().Contains(searchText))
                                                        || (t.Nhom != null && t.Nhom.ToUpper().Contains(searchText))
                                                        || (t.KhoCha != null && t.KhoCha.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindStoreData(filterStoreList);
            }
            else
            {
                this.BindStoreData(storeList);
            }
        }

        protected void ajaxCustomer_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {

        }

        private void FillObjectDetail(StoreDto obj)
        {
            this.txtKho.Text = obj.Kho;
            this.txtDienGiai.Text = obj.TenKho;
            this.txtDonVi.Text = obj.DonVi;
            this.txtViTri.Text = obj.ViTri;
            this.txtKhuVuc.Text = obj.KhuVuc;
            this.txtNhom.Text = obj.Nhom;
            this.txtKhoCha.Text = obj.KhoCha;
            this.txtPPTinhGia.Text = obj.PPTinhGia;
            this.cbNgungSuDung.Checked = obj.NgungSuDung == "+" ? true : false;
        }
    }
}