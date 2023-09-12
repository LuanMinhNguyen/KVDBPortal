using EDMs.Business.Services;
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
using EDMs.Data.Dto.PPM;

namespace EDMs.Web.Controls.WO
{
    public partial class WODetail : System.Web.UI.Page
    {
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("SelectedMainMenu", "Quản lý công việc");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
                GetWODataList();
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            var WOList = Session["WOList"] as List<WODto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterWOList = WOList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.PhieuCongViec != null && t.PhieuCongViec.ToUpper().Contains(searchText))
                                                        || (t.TenCongViec != null && t.TenCongViec.ToUpper().Contains(searchText))
                                                        || (t.ThietBi != null && t.ThietBi.ToUpper().Contains(searchText))
                                                        || (t.TenThietBi != null && t.TenThietBi.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindWOData(filterWOList);
            }
            else
            {
                this.BindWOData(WOList);
            }
        }

        private void GetWODataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
            DataSet ds;
            var WOList = new List<WODto>();
            ds = this.eamService.GetDataSet("get_WO_r5", new[] { userParam });
            if (ds != null)
            {
                WOList = this.eamService.CreateListFromTable<WODto>(ds.Tables[0]);
                foreach (var item in WOList)
                {
                    item.FullName = item.PhieuCongViec + " - " + item.TenCongViec;
                }
            }

            Session.Add("WOList", WOList);
        }

        private void BindWOData(List<WODto> WOList)
        {
            this.grdWO.DataSource = null;
            this.grdWO.DataSource = WOList;
            this.grdWO.DataBind();
        }

        protected void grdWO_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var WOList = Session["WOList"] as List<WODto>;

            this.grdWO.DataSource = WOList;
        }

        protected void grdWO_SelectedIndexChanged(object sender, EventArgs e)
        {
            var WO_CODE = this.grdWO.SelectedItems[0].FindControl("hfWO") as HiddenField;
            var hfWOFullName = this.grdWO.SelectedItems[0].FindControl("hfWOFullName") as HiddenField;
            var lblWOName = this.ToolBarPart.Items[0].FindControl("lblWOName") as Label;
            lblWOName.Text = hfWOFullName.Value;
            var WOList = Session["WOList"] as List<WODto>;
            var docObj = WOList.FirstOrDefault(t => t.PhieuCongViec == WO_CODE.Value);

            this.FillObjectDetail(docObj);
        }

        protected void ajaxCustomer_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {

        }

        private void FillObjectDetail(WODto obj)
        {
            this.txtPhieuCongViec.Text = obj.PhieuCongViec;
            this.txtDienGiai.Text = obj.TenCongViec;
            this.txtPhongBan.Text = obj.PhongBan;
            this.txtThietBi.Text = obj.ThietBi;
            this.txtTenThietBi.Text = obj.TenThietBi;
            this.txtDonVi.Text = obj.DonVi;
            this.txtLoaiBaoDuong.Text = obj.LoaiBaoDuong;
            this.txtMaQuyTrinh.Text = obj.MaQuyTrinh;
            this.txtTrangThai.Text = obj.TrangThai;
            this.txtDoUuTien.Text = !string.IsNullOrEmpty(obj.DoUuTien) ? obj.DoUuTien.Split('@')[0] : "";
            this.txtNguoiTao.Text = obj.NguoiTao;
            this.txtNgayTao.Text = obj.NgayTao;
            this.txtNgayKeHoachGoc.Text = obj.NgayKeHoachGoc;
            this.txtLenhSanXuat.Text = obj.LenhSanXuat;
            this.txtBaoCaoTinhTrangThietBi.Text = obj.BaoCaoTinhTrangThietBi;
            this.txtBBGiamDinhKiThuat.Text = obj.BBGiamDinhKyThuat;
            this.txtNguoiBaoCao.Text = obj.NguoiBaoCao;
            this.txtBBKiemTraSuCoTB.Text = obj.BBKiemTraSuCoTB;
            this.txtBienPhapSuaChua.Text = obj.BienPhapSuaChua;
            this.txtNgayBaoCao.Text = obj.NgayBaoCao;
            this.txtBBHienTruong.Text = obj.BBHienTruong;
            this.txtBBNghiemThu.Text = obj.BBNghiemThu;
            this.txtNguoiGiaoViec.Text = obj.NguoiGiaoViec;
            this.txtPhieuBatToaXe.Text = obj.PhieuBatToaXe;
            this.txtPhieuTraToaXe.Text = obj.PhieuTraToaXe;
            this.txtNguoiThucHien.Text = obj.NguoiThucHien;
            this.txtPhieuBatDauMay.Text = obj.PhieuBatDauMay;
            this.txtPhieuTraDauMay.Text = obj.PhieuTraDauMay;
            this.txtNgayBatDauKeHoach.Text = obj.NgayBatDauKeHoach;
            this.txtBBGiamDinhB2.Text = obj.BBGiamDinhB2;
            this.txtNgayKetThucKeHoach.Text = obj.NgayKetThucKeHoach;
            this.txtNgayBatDauThucTe.Text = obj.NgayBatDauThucTe;
            this.txtNgayKetThucThucTe.Text = obj.NgayKetThucTe;
            this.txtCaKip.Text = obj.CaKip;
        }
    }
}