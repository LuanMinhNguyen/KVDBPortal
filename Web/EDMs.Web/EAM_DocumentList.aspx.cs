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
using EDMs.Data.Dto.Document;
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
    public partial class EAM_DocumentList : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Tài Liệu");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
                this.GetREQDataList();
                this.LoadDocumentList();
            }
        }

        private void GetREQDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var DocumentList = new List<DocListDto>();
            ds = this.eamService.GetDataSet("get_Doc_List", new[] { userParam });
            if (ds != null)
            {
                DocumentList = this.eamService.CreateListFromTable<DocListDto>(ds.Tables[0]);
                foreach (var item in DocumentList)
                {
                    item.FullName = item.MaTaiLieu + " - " + item.TenTaiLieu;
                }
            }

            Session.Add("DocumentList", DocumentList);
        }

        private void LoadDocumentList()
        {
            var DocumentList = Session["DocumentList"] as List<DocListDto>;
            this.BindREQData(DocumentList);
            
        }

        private void BindREQData(List<DocListDto> DocumentList)
        {
            this.lbDocument.Items.Clear();
            foreach (var item in DocumentList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.MaTaiLieu);
                //lbItem.Attributes.Add("DonVi", item.DonVi);
                //lbItem.Attributes.Add("TrangThai", item.TrangThai);
                //lbItem.Attributes.Add("Kho", item.Kho);
                this.lbDocument.Items.Add(lbItem);
            }

            this.lbDocument.DataBind();
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

        private void FillREQDetail(DocListDto item)
        {
            this.txtTaiLieu.Text = item.MaTaiLieu;
            this.txtDienGiai.Text = item.TenTaiLieu;
            this.txtTieuDe.Text = item.TieuDe;
            this.txtNgayCapNhat.SelectedDate = item.NgayCapNhat.ToString("dd-MM-yyyy") != "01-01-0001" ? item.NgayCapNhat : (DateTime?)null;

        }


        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var documentList = Session["DocumentList"] as List<DocListDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterDocumentList = documentList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.MaTaiLieu != null && t.MaTaiLieu.ToUpper().Contains(searchText))
                                                        || (t.TenTaiLieu != null && t.TenTaiLieu.ToUpper().Contains(searchText))
                                                        || (t.TieuDe != null && t.TieuDe.ToUpper().Contains(searchText))
                                                        || (t.TenTep != null && t.TenTep.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindREQData(filterDocumentList);
            }
            else
            {
                this.BindREQData(documentList);
            }
        }

        protected void lbDocument_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbDocument.SelectedItem.Text;
            var DocumentList = Session["DocumentList"] as List<DocListDto>;
            var selectedItem = DocumentList.FirstOrDefault(t => t.MaTaiLieu == lbDocument.SelectedValue);

            this.FillREQDetail(selectedItem);
            this.grdPORDLines.Rebind();
        }

        protected void grdPORDLines_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var reqLineList = new List<DocEntitiesDto>();
            
            //var reqParam = new SqlParameter("@DonHang", SqlDbType.NVarChar);
            //reqParam.Value = this.lbDocument.SelectedItem.Value;
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            ds = this.eamService.GetDataSet("get_Doc_Entities", new[] { userParam });
            if (ds != null)
            {
                reqLineList = this.eamService.CreateListFromTable<DocEntitiesDto>(ds.Tables[0]);
            }

            this.grdPORDLines.DataSource = reqLineList;
        }
    }
}

