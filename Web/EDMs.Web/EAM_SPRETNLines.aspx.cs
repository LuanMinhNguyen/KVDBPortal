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
    public partial class EAM_SPRETNLines : Page
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
                this.LoadPartList();
            }
        }

        private void GetStoreDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var PartList = new List<SPRETNLineDto>();
            ds = this.eamService.GetDataSet("get_SPRETN_lines", new[] { userParam });
            if (ds != null)
            {
                PartList = this.eamService.CreateListFromTable<SPRETNLineDto>(ds.Tables[0]);
                foreach (var item in PartList)
                {
                    item.FullName = item.SoGiaoDich + " - " + item.DienGiai;
                }
            }

            Session.Add("PartList", PartList);
        }

        private void LoadPartList()
        {
            var PartList = Session["PartList"] as List<SPRETNLineDto>;
            this.BindStoreData(PartList);
            
        }

        private void BindStoreData(List<SPRETNLineDto> PartList)
        {
            this.lbSPRETNLine.Items.Clear();
            foreach (var item in PartList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.VatTu);
                lbItem.Attributes.Add("DonVi", item.DonVi);
                lbItem.Attributes.Add("TrangThai", item.TrangThai);
                lbItem.Attributes.Add("DonHang", item.DonHang);
                this.lbSPRETNLine.Items.Add(lbItem);
            }

            this.lbSPRETNLine.DataBind();
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

        private void FillStoreDetail(SPRETNLineDto item)
        {
            this.txtPhieuTra.Text = item.SoGiaoDich;
            this.txtDienGiai.Text = item.DienGiai;
            this.txtDonVi.Text = item.DonVi;
            this.txtTrangThai.Text = item.TrangThai;
            this.txtDonhang.Text = item.DonHang;
            this.txtNgayTra.SelectedDate = item.NgayGiaoDich.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayGiaoDich : (DateTime?)null;
        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var PartList = Session["PartList"] as List<SPRETNLineDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterPartList = PartList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.SoGiaoDich != null && t.SoGiaoDich.ToUpper().Contains(searchText))
                                                        || (t.DienGiai != null && t.DienGiai.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.TrangThai != null && t.TrangThai.ToUpper().Contains(searchText))
                                                        || (t.DonHang != null && t.DonHang.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindStoreData(filterPartList);
            }
            else
            {
                this.BindStoreData(PartList);
            }
        }

        protected void lbSPRETNLine_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbSPRETNLine.SelectedItem.Text;
            var PartList = Session["PartList"] as List<SPRETNLineDto>;
            var selectedItem = PartList.FirstOrDefault(t => t.VatTu == lbSPRETNLine.SelectedValue);

            this.FillStoreDetail(selectedItem);
        }
    }
}

