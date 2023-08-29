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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto;
using EDMs.Data.Dto.PPM;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.PPM
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class WODetail : Page
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
                this.GetObjectDataList();
                this.LoadObjectList();
                //this.lbObject.SelectedIndex = 0;
            }
        }

        private void GetObjectDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var objectList = new List<WODto>();
            ds = this.eamService.GetDataSet("get_wo_list", new[] { userParam });
            if (ds != null)
            {
                objectList = this.eamService.CreateListFromTable<WODto>(ds.Tables[0]);
                foreach (var item in objectList)
                {
                    item.FullName = item.PhieuCongViec + " - " + item.TenCongViec;
                }
            }

            Session.Add("ObjectList", objectList);
        }

        private void LoadObjectList()
        {
            var objectList = Session["ObjectList"] as List<WODto>;
            this.BindObjectData(objectList);
            
        }

        private void BindObjectData(List<WODto> objectList)
        {
            this.lbObject.Items.Clear();
            foreach (var item in objectList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.PhieuCongViec);
                lbItem.Attributes.Add("DoUuTien", item.DoUuTien);
                lbItem.Attributes.Add("TrangThaiCongViec", item.TrangThaiCongViec);
                lbItem.Attributes.Add("DonVi", item.DonVi);
                this.lbObject.Items.Add(lbItem);
            }

            this.lbObject.DataBind();
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

        protected void lbObject_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbObject.SelectedItem.Text;
            var WOList = Session["ObjectList"] as List<WODto>;
            var obj = WOList.FirstOrDefault(t => t.PhieuCongViec == lbObject.SelectedValue);

            this.grdPart.Rebind();
            this.grdWOActivities.Rebind();
            this.LoadObjectDocumentTree(obj);
            this.FillObjectDetail(obj);
        }

        private void LoadObjectDocumentTree(WODto obj)
        {
            this.rtvObjectDocument.Nodes.Clear();
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@phieucv", SqlDbType.NVarChar);
            objectParam.Value = obj.PhieuCongViec;
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var documentList = new List<NormMaterialDocDto>();
            ds = this.eamService.GetDataSet("get_doc_WO", new[] { userParam, objectParam });
            if (ds != null)
            {
                documentList = this.eamService.CreateListFromTable<NormMaterialDocDto>(ds.Tables[0]);
                var mainNode = new RadTreeNode("<b>Phiếu công việc: </b>" + obj.FullName, obj.PhieuCongViec);
                mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in documentList)
                {
                    var childNode = new RadTreeNode(item.MaTaiLieu + ": " + item.TenTaiLieu, item.DuongDan);
                    childNode.ImageUrl = @"~/Images/documents.png";

                    mainNode.Nodes.Add(childNode);
                    mainNode.Expanded = true;
                }

                this.rtvObjectDocument.Nodes.Add(mainNode);

            }
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var objectList = Session["ObjectList"] as List<WODto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterObjectList = objectList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.PhanLoai != null && t.PhanLoai.ToUpper().Contains(searchText))
                                                        || (t.ThietBi != null && t.ThietBi.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.DoUuTien != null && t.DoUuTien.ToUpper().Contains(searchText))
                                                        || (t.TrangThaiCongViec != null && t.TrangThaiCongViec.ToUpper().Contains(searchText))
                                                        || (t.NguoiBaoCao != null && t.NguoiBaoCao.ToUpper().Contains(searchText))
                                                        || (t.PhanCongBoi != null && t.PhanCongBoi.ToUpper().Contains(searchText))
                                                        || (t.Kieu != null && t.Kieu.ToUpper().Contains(searchText))
                                                        || (t.GhiChuKetLuan != null && t.GhiChuKetLuan.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindObjectData(filterObjectList);
            }
            else
            {
                this.BindObjectData(objectList);
            }
        }

        protected void grdPart_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var woPartList = new List<WOPartDto>();
            if (this.lbObject.SelectedItem != null)
            {
                var obj = new WODto()
                {
                    PhieuCongViec = this.lbObject.SelectedItem.Value,
                    DonVi = this.lbObject.SelectedItem.Attributes["DonVi"]
                };

                var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
                var objectParam = new SqlParameter("@phieucv", SqlDbType.NVarChar);
                objectParam.Value = obj.PhieuCongViec;
                userParam.Value = UserSession.Current.User.Username.ToUpper();
                DataSet ds;
                ds = this.eamService.GetDataSet("get_WO_Part", new[] { userParam, objectParam });
                if (ds != null)
                {
                    woPartList = this.eamService.CreateListFromTable<WOPartDto>(ds.Tables[0]);
                }
            }

            this.grdPart.DataSource = woPartList;
        }

        private void FillObjectDetail(WODto obj)
        {
            this.txtPhieuCV.Text = obj.PhieuCongViec;
            this.txtDienGiai.Text = obj.TenCongViec;
            this.txtDonVi.Text = obj.DonVi;
            this.txtThietBi.Text = obj.ThietBi;
            this.txtBoPhan.Text = obj.BoPhan;
            this.txtKieu.Text = obj.Kieu;
            this.txtNguoiTao.Text = obj.createdby;
            this.txtTrangThai.Text = obj.TrangThaiCongViec;
            this.txtNgayTao.SelectedDate =
                obj.NgayTao.ToString("dd-MM-yyyy") != "01-01-0001" ? obj.NgayTao : (DateTime?) null;
            this.cbBaoHanh.Checked = obj.BaoHanh == "Có";
            this.cbThueNgoai.Checked = obj.ThueNgoai == "Có";
            this.txtngayTheoLich.SelectedDate = obj.NgayTheoLich.ToString("dd-MM-yyyy") != "01-01-0001"
                ? obj.NgayTheoLich
                : (DateTime?) null;
            this.txtMaQuyTrinh.Text = obj.MaQuyTrinh;
            this.txtPhanLoai.Text = obj.PhanLoai;
            this.txtPhanCongBoi.Text = obj.PhanCongBoi;
            this.txtNgayBaoCao.SelectedDate = obj.NgayBaoCao.ToString("dd-MM-yyyy") != "01-01-0001"
                ? obj.NgayBaoCao
                : (DateTime?) null;
            this.txtPhanCongDen.Text = obj.PhanCongBoi;
            this.txtNguoiBaoCao.Text = obj.NguoiBaoCao;
            this.txtKeHoachBatDau.SelectedDate = obj.KeHoachBatDau.ToString("dd-MM-yyyy") != "01-01-0001"
                ? obj.KeHoachBatDau
                : (DateTime?) null;
            this.txtKeHoachHoanThanh.SelectedDate = obj.KeHoachHoanThanh.ToString("dd-MM-yyyy") != "01-01-0001"
                ? obj.KeHoachHoanThanh
                : (DateTime?) null;
            this.txtNgayBatDau.SelectedDate = obj.NgayBatDau.ToString("dd-MM-yyyy") != "01-01-0001"
                ? obj.NgayBatDau
                : (DateTime?) null;
            this.txtNgayHoanThanh.SelectedDate = obj.NgayHoanThanh.ToString("dd-MM-yyyy") != "01-01-0001"
                ? obj.NgayHoanThanh
                : (DateTime?) null;
            this.cbDatChoPhepSuDung.Checked = obj.DatChoPhepSuDung == "Có";
            this.cbKhongTheSuDung.Checked = obj.KhongTheSuDung == "Có";
            this.cbSuDungTamThoi.Checked = obj.SuDungTamThoi == "Có";
            this.txtKetLuan.Text = obj.GhiChuKetLuan;
        }

        protected void grdWOActivities_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var woActivitiesList = new List<WOActivitiesDto>();
            if (this.lbObject.SelectedItem != null)
            {
                var obj = new WODto()
                {
                    PhieuCongViec = this.lbObject.SelectedItem.Value,
                    DonVi = this.lbObject.SelectedItem.Attributes["DonVi"]
                };

                var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
                var objectParam = new SqlParameter("@phieucv", SqlDbType.NVarChar);
                objectParam.Value = obj.PhieuCongViec;
                userParam.Value = UserSession.Current.User.Username.ToUpper();
                DataSet ds;
                ds = this.eamService.GetDataSet("get_WO_activities", new[] { userParam, objectParam });
                if (ds != null)
                {
                    woActivitiesList = this.eamService.CreateListFromTable<WOActivitiesDto>(ds.Tables[0]);
                }
            }

            this.grdWOActivities.DataSource = woActivitiesList;
        }

        protected void rtvObjectDocument_OnContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            if (File.Exists(e.Node.Value))
            {
                HttpContext.Current.Response.Redirect("../../DownLoadFile.ashx?file=" + e.Node.Value);
            }
        }
    }
}

