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
using EDMs.Data.Dto.MaterialInventory;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Material
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class PartDetail : Page
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
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var PartList = new List<PartListDto>();
            ds = this.eamService.GetDataSet("get_Parts_List", new[] { userParam });
            if (ds != null)
            {
                PartList = this.eamService.CreateListFromTable<PartListDto>(ds.Tables[0]);
                foreach (var item in PartList)
                {
                    item.FullName = item.VatTu + " - " + item.DienGiai;
                }
            }

            Session.Add("PartList", PartList);
        }

        private void LoadPartList()
        {
            var PartList = Session["PartList"] as List<PartListDto>;
            this.BindStoreData(PartList);
            
        }

        private void BindStoreData(List<PartListDto> PartList)
        {
            this.lbPart.Items.Clear();
            foreach (var item in PartList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.VatTu);
                lbItem.Attributes.Add("PhanLoai", item.PhanLoai);
                lbItem.Attributes.Add("PhanNhom", item.PhanNhom);
                lbItem.Attributes.Add("NhaCungCapThamKhao", item.NhaCungCapThamKhao);
                this.lbPart.Items.Add(lbItem);
            }

            this.lbPart.DataBind();
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

        private void FillStoreDetail(PartListDto item)
        {
            this.txtVatTu.Text = item.VatTu;
            this.txtDienGiai.Text = item.DienGiai;
            this.cbThietBi.Checked = item.ThietBi == "+";
            this.txtPhanLoai.Text = item.PhanLoai;
            this.txtDonViTinh.Text = item.DVT;
            this.txtPhanNhom.Text = item.PhanNhom;
            this.txtNhaSanXuat.Text = item.NhaSanXuat;
            this.txtCongCu.Text = item.CongCu;
            this.txtPartNumber.Text = item.PartNumber;
            this.cbNgungSuDung.Checked = item.NgungSuDung == "+";
            this.txtMaThuehangHoa.Text = item.MaThueHangHoa;
            this.txtPhuongThucTheoDoi.Text = item.TheoDoi;
            this.cbKiemTraNhap.Checked = item.KiemTraTruocKhiNhapKho == "+";
            this.txtSoNgayBaoHanh.Text = item.SoNgayBaoHanh;
            this.txtDonGiaTieuChuan.Text = Math.Round(item.GiaTieuChuan, 2).ToString();
            this.txtDonGiaCoSo.Text = Math.Round(item.GiaCoSo, 2).ToString();
            this.txtDonGiaMuaCuoi.Text = Math.Round(item.GiaMuaCuoi, 2).ToString();
            this.txtDonGiaTrungBinh.Text = Math.Round(item.GiaTrungBinh, 2).ToString();

        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var PartList = Session["PartList"] as List<PartListDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterPartList = PartList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.VatTu != null && t.VatTu.ToUpper().Contains(searchText))
                                                        || (t.DienGiai != null && t.DienGiai.ToUpper().Contains(searchText))
                                                        || (t.PhanLoai != null && t.PhanLoai.ToUpper().Contains(searchText))
                                                        || (t.PhanNhom != null && t.PhanNhom.ToUpper().Contains(searchText))
                                                        || (t.NhaCungCapThamKhao != null && t.NhaCungCapThamKhao.ToUpper().Contains(searchText))
                                                        || (t.CongCu != null && t.CongCu.ToUpper().Contains(searchText))
                                                        || (t.DVT != null && t.DVT.ToUpper().Contains(searchText))
                                                        || (t.NhaSanXuat != null && t.NhaSanXuat.ToUpper().Contains(searchText))
                                                        || (t.PartNumber != null && t.PartNumber.ToUpper().Contains(searchText))
                                                        || (t.MaThueHangHoa != null && t.MaThueHangHoa.ToUpper().Contains(searchText))
                                                        || (t.TheoDoi != null && t.TheoDoi.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindStoreData(filterPartList);
            }
            else
            {
                this.BindStoreData(PartList);
            }
        }

        protected void lbPart_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbPart.SelectedItem.Text;
            var PartList = Session["PartList"] as List<PartListDto>;
            var selectedItem = PartList.FirstOrDefault(t => t.VatTu == lbPart.SelectedValue);

            this.FillStoreDetail(selectedItem);
            this.LoadObjectDocumentTree(selectedItem);

        }

        private void LoadObjectDocumentTree(PartListDto obj)
        {
            this.rtvObjectDocument.Nodes.Clear();
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@VatTu", SqlDbType.NVarChar);
            var orgParam = new SqlParameter("@DonVi", SqlDbType.NVarChar);
            objectParam.Value = obj.VatTu;
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            orgParam.Value = "*";
            DataSet ds;
            var documentList = new List<DocListDto>();
            ds = this.eamService.GetDataSet("get_doc_part", new[] { userParam, objectParam , orgParam });
            if (ds != null)
            {
                documentList = this.eamService.CreateListFromTable<DocListDto>(ds.Tables[0]);
                var mainNode = new RadTreeNode("<b>Vật tư: </b>" + obj.FullName, obj.VatTu);
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

        protected void rtvObjectDocument_OnContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            if (File.Exists(e.Node.Value))
            {
                HttpContext.Current.Response.Redirect("../../DownLoadFile.ashx?file=" + e.Node.Value);
            }
        }
    }
}

