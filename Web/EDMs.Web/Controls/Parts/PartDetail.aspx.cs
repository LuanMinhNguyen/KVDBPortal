using EDMs.Data.Dto.Store;
using EDMs.Web.Utilities.Sessions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using EDMs.Business.Services;
using EDMs.Data.Dto.Part;

namespace EDMs.Web.Controls.Parts
{
    public partial class PartDetail : System.Web.UI.Page
    {
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("SelectedMainMenu", "Quản lý vật tư");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
                GetPartDataList();
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            var partList = Session["PartList"] as List<PartDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterPartList = partList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.VatTu != null && t.VatTu.ToUpper().Contains(searchText))
                                                        || (t.TenVatTu != null && t.TenVatTu.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.QuyCach != null && t.QuyCach.ToUpper().Contains(searchText))
                                                        || (t.NgungSuDung != null && t.NgungSuDung.ToUpper().Contains(searchText))
                                                        || (t.PhanLoai != null && t.PhanLoai.ToUpper().Contains(searchText))
                                                        || (t.NhaCungCapUuTien != null && t.NhaCungCapUuTien.ToUpper().Contains(searchText))
                                                        || (t.PhanNhom != null && t.PhanNhom.ToUpper().Contains(searchText))
                                                        || (t.DonViTinh != null && t.DonViTinh.ToUpper().Contains(searchText)) 
                                                        || (t.Nguon != null && t.Nguon.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindPartData(filterPartList);
            }
            else
            {
                this.BindPartData(partList);
            }
        }

        private void GetPartDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
            DataSet ds;
            var partList = new List<PartDto>();
            ds = this.eamService.GetDataSet("get_part_r5", new[] { userParam });
            if (ds != null)
            {
                partList = this.eamService.CreateListFromTable<PartDto>(ds.Tables[0]);
                foreach (var item in partList)
                {
                    item.FullName = item.VatTu + " - " + item.TenVatTu;
                }
            }

            Session.Add("PartList", partList);
        }

        private void BindPartData(List<PartDto> partList)
        {
            this.grdPart.DataSource = null;
            this.grdPart.DataSource = partList;
            this.grdPart.DataBind();
        }

        protected void grdPart_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var partList = Session["PartList"] as List<PartDto>;

            this.grdPart.DataSource = partList;
        }

        protected void grdPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            var PART_CODE = this.grdPart.SelectedItems[0].FindControl("hfPart") as HiddenField;
            var hfPartFullName = this.grdPart.SelectedItems[0].FindControl("hfPartFullName") as HiddenField;
            var lblPartName = this.ToolBarPart.Items[0].FindControl("lblPartName") as Label;
            lblPartName.Text = hfPartFullName.Value;
            var partList = Session["PartList"] as List<PartDto>;
            var partObj = partList.FirstOrDefault(t => t.VatTu == PART_CODE.Value);

            this.FillObjectDetail(partObj);
        }

        protected void ajaxCustomer_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {

        }

        private void FillObjectDetail(PartDto obj)
        {
            this.txtVatTu.Text = obj.VatTu;
            this.txtDienGiai.Text = obj.TenVatTu;
            this.txtDonVi.Text = obj.DonVi;
            this.txtQuyCach.Text = obj.QuyCach;
            this.cbNgungSuDung.Checked = obj.NgungSuDung == "+" ? true : false;
            this.txtPhanLoai.Text = obj.PhanLoai;
            this.txtNhaCCUuTien.Text = obj.NhaCungCapUuTien;
            this.txtPhanNhom.Text = obj.PhanNhom;
            this.txtDVT.Text = obj.DonViTinh;
            this.txtNguon.Text = obj.Nguon;
        }
    }
}