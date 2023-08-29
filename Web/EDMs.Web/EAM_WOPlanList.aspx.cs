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
using EDMs.Data.Dto.PPM;

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
    public partial class EAM_WOPlanList : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Công Việc");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
                this.GetWODataList();
                this.LoadWOList();
            }
        }

        private void GetWODataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var WOList = new List<WOPlanDto>();
            ds = this.eamService.GetDataSet("get_WO_Plan_List", new[] { userParam });
            if (ds != null)
            {
                WOList = this.eamService.CreateListFromTable<WOPlanDto>(ds.Tables[0]);
                foreach (var item in WOList)
                {
                    item.FullName = item.PhieuCV + " - " + item.DienGiai;
                }
            }

            Session.Add("WOList", WOList);
        }

        private void LoadWOList()
        {
            var WOList = Session["WOList"] as List<WOPlanDto>;
            this.BindWOData(WOList);
            
        }

        private void BindWOData(List<WOPlanDto> WOList)
        {
            this.lbWO.Items.Clear();
            foreach (var item in WOList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.PhieuCV);
                lbItem.Attributes.Add("DoUuTien", item.DoUuTien);
                lbItem.Attributes.Add("TrangThai", item.TrangThai);
                lbItem.Attributes.Add("DonVi", item.DonVi);
                this.lbWO.Items.Add(lbItem);
            }

            this.lbWO.DataBind();
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

        protected void lbWO_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblWOName = this.ToolBarWO.Items[0].FindControl("lblWOName") as Label;
            lblWOName.Text = this.lbWO.SelectedItem.Text;
            var WOList = Session["WOList"] as List<WOPlanDto>;
            var selectedItem = WOList.FirstOrDefault(t => t.PhieuCV == lbWO.SelectedValue);

            this.FillWODetail(selectedItem);
        }

        private void FillWODetail(WOPlanDto item)
        {
            this.txtPhieuCV.Text = item.PhieuCV;
            this.txtDienGiai.Text = item.DienGiai;
            this.txtDonVi.Text = item.DonVi;
            this.txtThietBi.Text = item.ThietBi;
            this.txtBoPhan.Text = item.BoPhan;
            this.txtKieu.Text = item.KieuCV;
            this.txtNguoiTao.Text = item.NguoiTao;
            this.txtNgayTao.SelectedDate = item.NgayTao.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayTao : (DateTime?)null;
            this.cbBaoHanh.Checked = item.BaoHanh == "+";
            this.cbThueNgoai.Checked = item.ThueNgoai == "+";
            this.txtNoiDungYeuCauSuaChua.Text = item.NoiDungYeuCauSuaChua;
            this.txtKeHoachBatDau.SelectedDate = item.NgayBatDauKH.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayBatDauKH : (DateTime?)null;
            this.txtKeHoachHoanThanh.SelectedDate = item.NgayKetThucKH.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayKetThucKH : (DateTime?)null;
            this.txtNgayBatDau.SelectedDate = item.NgayBatDauTT.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayBatDauTT : (DateTime?) null;
            this.txtNgayHoanThanh.SelectedDate = item.NgayHoanThanhTT.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayHoanThanhTT : (DateTime?)null;
            this.cbDatChoPhepSuDung.Checked = item.DatChoPhepVanHanh == "+";
            this.cbKhongTheSuDung.Checked = item.KhongTheDuaVaoSuDung == "+";
            this.cbSuDungTamThoi.Checked = item.ChophepSuDungTamThoi == "+";
            this.txtKetLuanSuaChua.Text = item.KetLuanSauSuaChua;
        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var WOList = Session["WOList"] as List<WOPlanDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterWOList = WOList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.PhieuCV != null && t.PhieuCV.ToUpper().Contains(searchText))
                                                        || (t.DienGiai != null && t.DienGiai.ToUpper().Contains(searchText))
                                                        || (t.DoUuTien != null && t.DoUuTien.ToUpper().Contains(searchText))
                                                        || (t.ThietBi != null && t.ThietBi.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.BoPhan != null && t.BoPhan.ToUpper().Contains(searchText))
                                                        || (t.KieuCV != null && t.KieuCV.ToUpper().Contains(searchText))
                                                        || (t.TrangThai != null && t.TrangThai.ToUpper().Contains(searchText))
                                                        || (t.NguoiTao != null && t.NguoiTao.ToUpper().Contains(searchText))
                                                        || (t.KetLuanSauSuaChua != null && t.KetLuanSauSuaChua.ToUpper().Contains(searchText))
                                                        || (t.NoiDungYeuCauSuaChua != null && t.NoiDungYeuCauSuaChua.ToUpper().Contains(searchText))
                                                        || (t.BaoHanh != null && t.BaoHanh.ToUpper().Contains(searchText))
                                                        || (t.ThueNgoai != null && t.ThueNgoai.ToUpper().Contains(searchText))
                                                        || (t.DatChoPhepVanHanh != null && t.DatChoPhepVanHanh.ToUpper().Contains(searchText))
                                                        || (t.ChophepSuDungTamThoi != null && t.ChophepSuDungTamThoi.ToUpper().Contains(searchText))
                                                        || (t.KhongTheDuaVaoSuDung != null && t.KhongTheDuaVaoSuDung.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindWOData(filterWOList);
            }
            else
            {
                this.BindWOData(WOList);
            }
        }
    }
}

