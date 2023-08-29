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
using EDMs.Data.Dto.PurchaseOrder;

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
    public partial class PORDDetail : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Mua Sắm");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
                this.GetREQDataList();
                this.LoadPartList();
            }
        }

        private void GetREQDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var PartList = new List<PORDListDto>();
            ds = this.eamService.GetDataSet("get_PORD_List", new[] { userParam });
            if (ds != null)
            {
                PartList = this.eamService.CreateListFromTable<PORDListDto>(ds.Tables[0]);
                foreach (var item in PartList)
                {
                    item.FullName = item.DonHang + " - " + item.DienGiai;
                }
            }

            Session.Add("PartList", PartList);
        }

        private void LoadPartList()
        {
            var PartList = Session["PartList"] as List<PORDListDto>;
            this.BindREQData(PartList);
            
        }

        private void BindREQData(List<PORDListDto> PartList)
        {
            this.lbPORD.Items.Clear();
            foreach (var item in PartList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.DonHang);
                lbItem.Attributes.Add("DonVi", item.DonVi);
                lbItem.Attributes.Add("TrangThai", item.TrangThai);
                lbItem.Attributes.Add("Kho", item.Kho);
                this.lbPORD.Items.Add(lbItem);
            }

            this.lbPORD.DataBind();
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

        private void FillREQDetail(PORDListDto item)
        {
            this.txtDonHang.Text = item.DonHang;
            this.txtDienGiai.Text = item.DienGiai;
            this.txtDonVi.Text = item.DonVi;
            this.txtTrangThai.Text = item.TrangThai;
            this.txtKho.Text = item.Kho;
            this.txtNguoiDatHang.Text = item.NguoiDatHang;
            this.txtSoGoiTheoDoi.Text = item.SoGoiTheoDoi;
            this.txtThoiGianCho.Text = item.ThoiGianCho;
            this.txtDiaChiGiaoNhan.Text = item.DiaChiGiaoNhan;
            this.txtTongGiaTri.Text = item.TongGiaTri.ToString();
            this.txtNhaCungCap.Text = item.NhaCungCap;
            this.txtTienTe.Text = item.TienTe;
            this.txtTyGia.Text = Math.Round(item.TiGia, 2).ToString();

        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var PartList = Session["PartList"] as List<PORDListDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterPartList = PartList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.DienGiai != null && t.DienGiai.ToUpper().Contains(searchText))
                                                        || (t.DonHang != null && t.DonHang.ToUpper().Contains(searchText))
                                                        || (t.TrangThai != null && t.TrangThai.ToUpper().Contains(searchText))
                                                        || (t.Kho != null && t.Kho.ToUpper().Contains(searchText))
                                                        || (t.NguoiDatHang != null && t.NguoiDatHang.ToUpper().Contains(searchText))
                                                        || (t.SoGoiTheoDoi != null && t.SoGoiTheoDoi.ToUpper().Contains(searchText))
                                                        || (t.ThoiGianCho != null && t.ThoiGianCho.ToUpper().Contains(searchText))
                                                        || (t.DiaChiGiaoNhan != null && t.DiaChiGiaoNhan.ToUpper().Contains(searchText))
                                                        || (t.NhaCungCap != null && t.NhaCungCap.ToUpper().Contains(searchText))
                                                        || (t.TienTe != null && t.TienTe.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindREQData(filterPartList);
            }
            else
            {
                this.BindREQData(PartList);
            }
        }

        protected void lbPORD_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbPORD.SelectedItem.Text;
            var PartList = Session["PartList"] as List<PORDListDto>;
            var selectedItem = PartList.FirstOrDefault(t => t.DonHang == lbPORD.SelectedValue);

            this.FillREQDetail(selectedItem);
            this.grdPORDLines.Rebind();
        }

        protected void grdPORDLines_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var reqLineList = new List<PORDLineDto>();
            if (this.lbPORD.SelectedItem != null)
            {
                var reqParam = new SqlParameter("@DonHang", SqlDbType.NVarChar);
                reqParam.Value = this.lbPORD.SelectedItem.Value;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_PORD_Lines", new[] { reqParam });
                if (ds != null)
                {
                    reqLineList = this.eamService.CreateListFromTable<PORDLineDto>(ds.Tables[0]);
                }
            }

            this.grdPORDLines.DataSource = reqLineList;
        }
    }
}

