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
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Asset
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class AssetDetail : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Tài Sản");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
                this.GetAssetDataList();
                this.LoadAssetList();
                //this.lbAsset.SelectedIndex = 0;
            }
        }

        private void GetAssetDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var assetList = new List<AssetDto>();
            ds = this.eamService.GetDataSet("get_asset_r5", new[] { userParam });
            if (ds != null)
            {
                assetList = this.eamService.CreateListFromTable<AssetDto>(ds.Tables[0]);
                foreach (var item in assetList)
                {
                    item.FullName = item.ThietBi + " - " + item.TenThietBi;
                }
            }

            Session.Add("AssestList", assetList);
        }

        private void LoadAssetList()
        {
            var assetList = Session["AssestList"] as List<AssetDto>;
            this.BindAssestData(assetList);
            
        }

        private void BindAssestData(List<AssetDto> assetList)
        {
            this.lbAsset.Items.Clear();
            foreach (var item in assetList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.ThietBi);
                lbItem.Attributes.Add("ToChuc", item.ToChuc);
                lbItem.Attributes.Add("TrangThai", item.TrangThai == "I" ? "Đã lắp đặp" : "Đã gỡ bỏ");
                lbItem.Attributes.Add("PhongBan", item.PhongBan);
                this.lbAsset.Items.Add(lbItem);
            }

            this.lbAsset.DataBind();
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

        protected void lbAsset_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblAssetName = this.ToolBarAsset.Items[0].FindControl("lblAssetName") as Label;
            lblAssetName.Text = this.lbAsset.SelectedItem.Text;

            var assestList = Session["AssestList"] as List<AssetDto>;
            var assetObj = assestList.FirstOrDefault(t => t.ThietBi == lbAsset.SelectedValue);

            //this.LoadAssetStructure(assetObj);
            this.grdAssetPMSchedule.Rebind();
            //this.grdAssetPartUsage.Rebind();
            //this.grdAssetPartAssociated.Rebind();
            //this.LoadAssetDocumentTree(assetObj);
            //this.grdAssetDepreciation.Rebind();
            //this.grdAssetEvent.Rebind();
            this.grdAssetHistory.Rebind();
            this.FillObjectDetail(assetObj);
        }

        private void LoadAssetDocumentTree(AssetDto assetObj)
        {
            //this.rtvAssetDocument.Nodes.Clear();
            //var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            //var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            //objectParam.Value = assetObj.ThietBi;
            //organizationParam.Value = assetObj.DonVi;
            //DataSet ds;
            //var documentList = new List<AssetDocumentDto>();
            //ds = this.eamService.GetDataSet("get_asset_documents", new[] { objectParam, organizationParam });
            //if (ds != null)
            //{
            //    documentList = this.eamService.CreateListFromTable<AssetDocumentDto>(ds.Tables[0]);
            //    var mainNode = new RadTreeNode("<b>Thiết bị: </b>" + assetObj.FullName, assetObj.ThietBi);
            //    mainNode.ImageUrl = @"~/Images/folderdir16.png";
            //    foreach (var item in documentList)
            //    {
            //        var childNode = new RadTreeNode(item.MA_TAI_LIEU + ": " + item.TEN_TAI_LIEU, item.DuongDan);
            //        childNode.ImageUrl = @"~/Images/documents.png";

            //        mainNode.Nodes.Add(childNode);
            //        mainNode.Expanded = true;
            //    }

            //    this.rtvAssetDocument.Nodes.Add(mainNode);

            //}
        }

        private void LoadAssetStructure(AssetDto assetObj)
        {
            //this.rtvAssetLvl.Nodes.Clear();
            //var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            //var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            //objectParam.Value = assetObj.ThietBi;
            //organizationParam.Value = assetObj.DonVi;
            //DataSet ds;
            //var assetList = new List<AssetStructureDto>();
            //ds = this.eamService.GetDataSet("get_asset_structure", new[] { objectParam, organizationParam });
            //if (ds != null)
            //{
            //    assetList = this.eamService.CreateListFromTable<AssetStructureDto>(ds.Tables[0]);
            //    var mainNode = new RadTreeNode(assetObj.FullName, assetObj.ThietBi);
            //    mainNode.ImageUrl = @"~/Images/folderdir16.png";
            //    foreach (var item in assetList)
            //    {
            //        var childNode = new RadTreeNode(item.MA_THIET_BI + " - " + item.TEN_THIET_BI, item.MA_THIET_BI);
            //        childNode.ImageUrl = @"~/Images/folderdir16.png";

            //        mainNode.Nodes.Add(childNode);
            //        mainNode.Expanded = true;
            //    }

            //    this.rtvAssetLvl.Nodes.Add(mainNode);

            //}
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var assetList = Session["AssestList"] as List<AssetDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterAssestList = assetList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.ToChuc != null && t.ToChuc.ToUpper().Contains(searchText))
                                                        || (t.PhongBan != null && t.PhongBan.ToUpper().Contains(searchText))
                                                        || (t.TrangThai != null && t.TrangThai.ToUpper().Contains(searchText))
                                                        || (t.NguoiQuanLy != null && t.NguoiQuanLy.ToUpper().Contains(searchText))
                                                        || (t.ThongSoKyThuat != null && t.ThongSoKyThuat.ToUpper().Contains(searchText))
                                                        || (t.NhaSanXuat != null && t.NhaSanXuat.ToUpper().Contains(searchText))
                                                        || (t.Model != null && t.Model.ToUpper().Contains(searchText))
                                                        || (t.SerialNumber != null && t.SerialNumber.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindAssestData(filterAssestList);
            }
            else
            {
                this.BindAssestData(assetList);
            }
        }

        protected void grdAssetPMSchedule_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var assetPMScheduleList = new List<AssetPMScheduleDto>();
            if (this.lbAsset.SelectedItem != null)
            {
                var assetObj = new AssetDto()
                {
                    ThietBi = this.lbAsset.SelectedItem.Value,
                    ToChuc = this.lbAsset.SelectedItem.Attributes["ToChuc"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.ThietBi;
                organizationParam.Value = assetObj.ToChuc;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_asset_pmschedule", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetPMScheduleList = this.eamService.CreateListFromTable<AssetPMScheduleDto>(ds.Tables[0]);
                }
            }

            this.grdAssetPMSchedule.DataSource = assetPMScheduleList;
        }

        protected void grdAssetPartUsage_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var assetPartUsageList = new List<AssetPartUsageDto>();
            //if (this.lbAsset.SelectedItem != null)
            //{
            //    var assetObj = new AssetDto()
            //    {
            //        ThietBi = this.lbAsset.SelectedItem.Value,
            //        DonVi = this.lbAsset.SelectedItem.Attributes["DonVi"]
            //    };
            //    var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            //    var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            //    objectParam.Value = assetObj.ThietBi;
            //    organizationParam.Value = assetObj.DonVi;
            //    DataSet ds;
            //    ds = this.eamService.GetDataSet("get_asset_part_usage", new[] { objectParam, organizationParam });
            //    if (ds != null)
            //    {
            //        assetPartUsageList = this.eamService.CreateListFromTable<AssetPartUsageDto>(ds.Tables[0]);
            //    }
            //}

            //this.grdAssetPartUsage.DataSource = assetPartUsageList;
        }

        protected void grdAssetPartAssociated_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var assetPartAssociatedList = new List<AssetPartAssociatedDto>();
            //if (this.lbAsset.SelectedItem != null)
            //{
            //    var assetObj = new AssetDto()
            //    {
            //        ThietBi = this.lbAsset.SelectedItem.Value,
            //        DonVi = this.lbAsset.SelectedItem.Attributes["DonVi"]
            //    };
            //    var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            //    var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            //    objectParam.Value = assetObj.ThietBi;
            //    organizationParam.Value = assetObj.DonVi;
            //    DataSet ds;
            //    ds = this.eamService.GetDataSet("get_asset_part_associated", new[] { objectParam, organizationParam });
            //    if (ds != null)
            //    {
            //        assetPartAssociatedList = this.eamService.CreateListFromTable<AssetPartAssociatedDto>(ds.Tables[0]);
            //    }
            //}

            //this.grdAssetPartAssociated.DataSource = assetPartAssociatedList;
        }

        protected void grdAssetDepreciation_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var assetDepreciationList = new List<AssetDepreciationDto>();
            //if (this.lbAsset.SelectedItem != null)
            //{
            //    var assetObj = new AssetDto()
            //    {
            //        ThietBi = this.lbAsset.SelectedItem.Value,
            //        DonVi = this.lbAsset.SelectedItem.Attributes["DonVi"]
            //    };
            //    var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            //    var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            //    objectParam.Value = assetObj.ThietBi;
            //    organizationParam.Value = assetObj.DonVi;
            //    DataSet ds;
            //    ds = this.eamService.GetDataSet("get_asset_depreciations", new[] { objectParam, organizationParam });
            //    if (ds != null)
            //    {
            //        assetDepreciationList = this.eamService.CreateListFromTable<AssetDepreciationDto>(ds.Tables[0]);
            //    }
            //}

            //this.grdAssetDepreciation.DataSource = assetDepreciationList;
        }

        protected void grdAssetEvent_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var assetEventList = new List<AssetEventDto>();
            //if (this.lbAsset.SelectedItem != null)
            //{
            //    var assetObj = new AssetDto()
            //    {
            //        ThietBi = this.lbAsset.SelectedItem.Value,
            //        DonVi = this.lbAsset.SelectedItem.Attributes["DonVi"]
            //    };
            //    var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            //    var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            //    objectParam.Value = assetObj.ThietBi;
            //    organizationParam.Value = assetObj.DonVi;
            //    DataSet ds;
            //    ds = this.eamService.GetDataSet("get_asset_events", new[] { objectParam, organizationParam });
            //    if (ds != null)
            //    {
            //        assetEventList = this.eamService.CreateListFromTable<AssetEventDto>(ds.Tables[0]);
            //    }
            //}

            //this.grdAssetEvent.DataSource = assetEventList;
        }

        private void FillObjectDetail(AssetDto obj)
        {
            this.txtThietBi.Text = obj.ThietBi;
            this.txtDienGiai.Text = obj.TenThietBi;
            this.txtToChuc.Text = obj.ToChuc;

            this.txtThongSoKyThuat.Text = obj.ThongSoKyThuat;
            this.txtPhongBan.Text = obj.PhongBan;
            this.txtTrangThai.Text = obj.TrangThai == "I" ? "Đã lắp đặt" : "Đã gỡ bỏ";

            this.txtPhanNhom.Text = obj.PhanNhom;
            this.txtNgayBDTheoDoi.SelectedDate = obj.NgayBDTheoDoi;
            this.txtKhuVuc.Text = obj.KhuVuc;

            this.txtNhaSanXuat.Text = obj.NhaSanXuat;
            this.txtDonViDo.Text = obj.DonViDo;
            this.txtThietBiCha.Text = obj.ThietBiCha;

            this.txtModel.Text = obj.Model;
            this.txtGiaTri.Text = obj.GiaTri;
            this.txtPhanLoai.Text = obj.PhanLoai;

            this.txtSerialNumber.Text = obj.SerialNumber;   
            this.txtSoDangKy.Text = obj.SoDangKy;
            
            this.txtXepLoai.Text = obj.XepLoai;
            this.txtNguoiQuanLy.Text = obj.NguoiQuanLy;
        }

        protected void rtvObjectDocument_OnContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            if (File.Exists(e.Node.Value))
            {
                HttpContext.Current.Response.Redirect("../../DownLoadFile.ashx?file=" + e.Node.Value);
            }
        }

        protected void grdAssetHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var assetEventList = new List<AssetEventDto>();

            //if (this.lbAsset.SelectedItem != null)
            //{
            //    var lblAssetName = this.ToolBarAsset.Items[0].FindControl("lblAssetName") as Label;
            //    lblAssetName.Text = this.lbAsset.SelectedItem.Text;
            //    var assestList = Session["AssestList"] as List<AssetDto>;
            //    var assetObj = assestList.FirstOrDefault(t => t.ThietBi == lbAsset.SelectedValue);
                

            //    if (assetObj != null)
            //    {
            //        var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            //        userParam.Value = UserSession.Current.User.Username;
            //        DataSet ds;
            //        ds = this.eamService.GetDataSet("get_assetEvent_r5", new[] { userParam });
            //        if (ds != null)
            //        {
            //            assetEventList = this.eamService.CreateListFromTable<AssetEventDto>(ds.Tables[0]);
            //        }
            //    }
            //}

            //this.grdAssetHistory.DataSource = assetEventList;

            var assetEventList = new List<AssetEventDto>();
            if (this.lbAsset.SelectedItem != null)
            {
                var assetObj = new AssetDto()
                {
                    ThietBi = this.lbAsset.SelectedItem.Value,
                    ToChuc = this.lbAsset.SelectedItem.Attributes["ToChuc"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.ThietBi;
                organizationParam.Value = assetObj.ToChuc;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_assetEvent_r5", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetEventList = this.eamService.CreateListFromTable<AssetEventDto>(ds.Tables[0]);
                }
            }

            this.grdAssetHistory.DataSource = assetEventList;
        }
    }
}

