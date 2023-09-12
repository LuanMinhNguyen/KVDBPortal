using EDMs.Business.Services;
using EDMs.Data.Dto.Part;
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
using EDMs.Data.Dto.Document;

namespace EDMs.Web.Controls.Document
{
    public partial class DocumentDetail : System.Web.UI.Page
    {
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("SelectedMainMenu", "Quản lý tài liệu");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
                GetPartDataList();
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            var DocList = Session["DocList"] as List<DocListDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterDocList = DocList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.TaiLieu != null && t.TaiLieu.ToUpper().Contains(searchText))
                                                        || (t.TenTaiLieu != null && t.TenTaiLieu.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.TieuDe != null && t.TieuDe.ToUpper().Contains(searchText))
                                                        || (t.NgungSuDung != null && t.NgungSuDung.ToUpper().Contains(searchText))
                                                        || (t.SoPhienBan != null && t.SoPhienBan.ToUpper().Contains(searchText))
                                                        || (t.LoaiTaiLieu != null && t.LoaiTaiLieu.ToUpper().Contains(searchText))
                                                        || (t.PhanNhom != null && t.PhanNhom.ToUpper().Contains(searchText))
                                                        || (t.TenGoc != null && t.TenGoc.ToUpper().Contains(searchText))
                                                        || (t.DuongDan != null && t.DuongDan.ToUpper().Contains(searchText))
                                                        || (t.NguoiTao != null && t.NguoiTao.ToUpper().Contains(searchText))
                                                        || (t.NguoiCapNhat != null && t.NguoiCapNhat.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindDocData(filterDocList);
            }
            else
            {
                this.BindDocData(DocList);
            }
        }

        private void GetPartDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
            DataSet ds;
            var DocList = new List<DocListDto>();
            ds = this.eamService.GetDataSet("get_doc_r5", new[] { userParam });
            if (ds != null)
            {
                DocList = this.eamService.CreateListFromTable<DocListDto>(ds.Tables[0]);
                foreach (var item in DocList)
                {
                    item.FullName = item.TaiLieu + " - " + item.TenTaiLieu;
                }
            }

            Session.Add("DocList", DocList);
        }

        private void BindDocData(List<DocListDto> DocList)
        {
            this.grdDoc.DataSource = null;
            this.grdDoc.DataSource = DocList;
            this.grdDoc.DataBind();
        }

        protected void grdDoc_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var DocList = Session["DocList"] as List<DocListDto>;

            this.grdDoc.DataSource = DocList;
        }

        protected void grdDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            var DOC_CODE = this.grdDoc.SelectedItems[0].FindControl("hfDoc") as HiddenField;
            var hfDocFullName = this.grdDoc.SelectedItems[0].FindControl("hfDocFullName") as HiddenField;
            var lblDocName = this.ToolBarPart.Items[0].FindControl("lblDocName") as Label;
            lblDocName.Text = hfDocFullName.Value;
            var DocList = Session["DocList"] as List<DocListDto>;
            var docObj = DocList.FirstOrDefault(t => t.TaiLieu == DOC_CODE.Value);

            this.FillObjectDetail(docObj);
        }

        protected void ajaxCustomer_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {

        }

        private void FillObjectDetail(DocListDto obj)
        {
            this.txtTaiLieu.Text = obj.TaiLieu;
            this.txtDienGiai.Text = obj.TenTaiLieu;
            this.txtDonVi.Text = obj.DonVi;
            this.txtSoPhienBan.Text = obj.SoPhienBan;
            this.txtLoaiTaiLieu.Text = obj.LoaiTaiLieu;
            this.txtTenGoc.Text = obj.TenGoc;
            this.txtNgayTaiLen.Text = obj.NgayTaiLen;
            this.txtDuongDan.Text = obj.DuongDan;
            this.cbNgungSuDung.Checked = obj.NgungSuDung == "+" ? true : false;
            this.txtNgayTao.Text = obj.NgayTao;
            this.txtNguoiTao.Text = obj.NguoiTao;
            this.txtPhanNhom.Text = obj.PhanNhom;
            this.txtNgayCapNhatPhienBan.Text = obj.NgayCapNhatPhienBan;
            this.txtNgayCoHieuLuc.Text = obj.NgayCoHieuLuc;
            this.txtNgayHetHieuLuc.Text = obj.NgayHetHieuLuc;
            this.txtNgayCapNhat.Text = obj.NgayCapNhat;
            this.txtNguoiCapNhat.Text = obj.NguoiCapNhat;
        }
    }
}
