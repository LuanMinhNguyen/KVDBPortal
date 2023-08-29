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
    public partial class EAM_REQList : Page
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
            var PartList = new List<REQListDto>();
            ds = this.eamService.GetDataSet("get_REQ_List", new[] { userParam });
            if (ds != null)
            {
                PartList = this.eamService.CreateListFromTable<REQListDto>(ds.Tables[0]);
                foreach (var item in PartList)
                {
                    item.FullName = item.PhieuYC + " - " + item.DienGiai;
                }
            }

            Session.Add("PartList", PartList);
        }

        private void LoadPartList()
        {
            var PartList = Session["PartList"] as List<REQListDto>;
            this.BindREQData(PartList);
            
        }

        private void BindREQData(List<REQListDto> PartList)
        {
            this.lbREQ.Items.Clear();
            foreach (var item in PartList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.PhieuYC);
                lbItem.Attributes.Add("DonVi", item.DonVi);
                lbItem.Attributes.Add("TrangThai", item.TrangThai);
                lbItem.Attributes.Add("Kho", item.Kho);
                this.lbREQ.Items.Add(lbItem);
            }

            this.lbREQ.DataBind();
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

        private void FillREQDetail(REQListDto item)
        {
            this.txtYeuCauMuaHang.Text = item.PhieuYC;
            this.txtDienGiai.Text = item.DienGiai;
            this.txtDonVi.Text = item.DonVi;
            this.txtTrangThai.Text = item.TrangThai;
            this.txtKho.Text = item.Kho;
            this.txtNguoiYeuCau.Text = item.NguoiYC;
            this.txtNgayYeuCau.SelectedDate = item.NgayYC.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayYC : (DateTime?)null;
            this.txtPhieuCV.Text = item.PhieuCV;
            this.txtMucCV.Text = item.MucCV.ToString();
            this.txtPheDuyetBoi.Text = item.NguoiPheDuyet;
            this.txtNgayPheDuyet.SelectedDate = item.NgayPheDuyet.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayPheDuyet : (DateTime?)null;

        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var PartList = Session["PartList"] as List<REQListDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterPartList = PartList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.DienGiai != null && t.DienGiai.ToUpper().Contains(searchText))
                                                        || (t.PhieuYC != null && t.PhieuYC.ToUpper().Contains(searchText))
                                                        || (t.TrangThai != null && t.TrangThai.ToUpper().Contains(searchText))
                                                        || (t.Kho != null && t.Kho.ToUpper().Contains(searchText))
                                                        || (t.NguoiYC != null && t.NguoiYC.ToUpper().Contains(searchText))
                                                        || (t.PhieuCV != null && t.PhieuCV.ToUpper().Contains(searchText))
                                                        || (t.NguoiPheDuyet != null && t.NguoiPheDuyet.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindREQData(filterPartList);
            }
            else
            {
                this.BindREQData(PartList);
            }
        }

        protected void lbREQ_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbREQ.SelectedItem.Text;
            var PartList = Session["PartList"] as List<REQListDto>;
            var selectedItem = PartList.FirstOrDefault(t => t.PhieuYC == lbREQ.SelectedValue);

            this.FillREQDetail(selectedItem);
            this.grdREQLines.Rebind();
        }

        protected void grdREQLines_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var reqLineList = new List<REQLineDto>();
            if (this.lbREQ.SelectedItem != null)
            {
                var reqParam = new SqlParameter("@PhieuYC", SqlDbType.NVarChar);
                reqParam.Value = this.lbREQ.SelectedItem.Value;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_REQ_lines", new[] { reqParam });
                if (ds != null)
                {
                    reqLineList = this.eamService.CreateListFromTable<REQLineDto>(ds.Tables[0]);
                }
            }

            this.grdREQLines.DataSource = reqLineList;
        }
    }
}

