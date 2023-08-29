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
using System.Net;
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
    public partial class MatDetail : Page
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
            var objectList = new List<NormMaterialDto>();
            ds = this.eamService.GetDataSet("get_matlist", new[] { userParam });
            if (ds != null)
            {
                objectList = this.eamService.CreateListFromTable<NormMaterialDto>(ds.Tables[0]);
                foreach (var item in objectList)
                {
                    item.FullName = item.DinhMuc + " - " + item.MoTa;
                }
            }

            Session.Add("ObjectList", objectList);
        }

        private void LoadObjectList()
        {
            var objectList = Session["ObjectList"] as List<NormMaterialDto>;
            this.BindObjectData(objectList);
            
        }

        private void BindObjectData(List<NormMaterialDto> objectList)
        {
            this.lbObject.Items.Clear();
            foreach (var item in objectList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.DinhMuc);
                lbItem.Attributes.Add("PhanLoai", item.PhanLoai);
                lbItem.Attributes.Add("NguoiYeuCau", item.NguoiYeuCau);
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
            var obj = new NormMaterialDto()
            {
                DinhMuc = this.lbObject.SelectedItem.Value,
                FullName = this.lbObject.SelectedItem.Text,
                DonVi = this.lbObject.SelectedItem.Attributes["DonVi"]
            };

            this.grdPart.Rebind();
            this.LoadObjectDocumentTree(obj);
            this.FillObjectDetail(obj);
        }

        private void LoadObjectDocumentTree(NormMaterialDto assetObj)
        {
            this.rtvObjectDocument.Nodes.Clear();
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@DinhMuc", SqlDbType.NVarChar);
            var donViParam = new SqlParameter("@DonVi", SqlDbType.NVarChar);
            objectParam.Value = assetObj.DinhMuc;
            donViParam.Value = assetObj.DonVi;
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var documentList = new List<NormMaterialDocDto>();
            ds = this.eamService.GetDataSet("get_doc_matlist", new[] { userParam, objectParam, donViParam });
            if (ds != null)
            {
                documentList = this.eamService.CreateListFromTable<NormMaterialDocDto>(ds.Tables[0]);
                var mainNode = new RadTreeNode("<b>Định mức vật tư: </b>" + assetObj.FullName, assetObj.DinhMuc);
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
            var objectList = Session["ObjectList"] as List<NormMaterialDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterObjectList = objectList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.PhanLoai != null && t.PhanLoai.ToUpper().Contains(searchText))
                                                        || (t.NguoiYeuCau != null && t.NguoiYeuCau.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
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
            var assetPMScheduleList = new List<NormMaterialPartDto>();
            if (this.lbObject.SelectedItem != null)
            {
                var assetObj = new NormMaterialDto()
                {
                    DinhMuc = this.lbObject.SelectedItem.Value,
                    DonVi = this.lbObject.SelectedItem.Attributes["DonVi"]
                };

                var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
                var objectParam = new SqlParameter("@DinhMuc", SqlDbType.NVarChar);
                var donViParam = new SqlParameter("@DonVi", SqlDbType.NVarChar);
                objectParam.Value = assetObj.DinhMuc;
                donViParam.Value = assetObj.DonVi;
                userParam.Value = UserSession.Current.User.Username;

                objectParam.Value = assetObj.DinhMuc;
                donViParam.Value = assetObj.DonVi;
                userParam.Value = UserSession.Current.User.Username.ToUpper();
                DataSet ds;
                ds = this.eamService.GetDataSet("get_part_matlist", new[] { userParam, objectParam, donViParam });
                if (ds != null)
                {
                    assetPMScheduleList = this.eamService.CreateListFromTable<NormMaterialPartDto>(ds.Tables[0]);
                }
            }

            this.grdPart.DataSource = assetPMScheduleList;
        }

        private void FillObjectDetail(NormMaterialDto item)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@DinhMuc", SqlDbType.NVarChar);
            var donViParam = new SqlParameter("@DonVi", SqlDbType.NVarChar);
            objectParam.Value = item.DinhMuc;
            donViParam.Value = item.DonVi;
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var objList = new List<NormMaterialDto>();
            ds = this.eamService.GetDataSet("get_detail_matlist", new[] { userParam, objectParam, donViParam });
            if (ds != null)
            {
                objList = this.eamService.CreateListFromTable<NormMaterialDto>(ds.Tables[0]);
                var obj = objList[0];

                this.txtDinhMuc.Text = obj.DinhMuc;
                this.txtDienGiai.Text = obj.MoTa;
                this.txtPhanLoai.Text = obj.PhanLoai;
                this.txtSoMuc.Text = obj.SoMuc.ToString();
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

