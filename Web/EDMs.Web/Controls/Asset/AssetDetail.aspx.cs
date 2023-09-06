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
using EDMs.Data.Dto.Asset;
using EDMs.Data.Dto.MRC;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;

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
            Session.Add("SelectedMainMenu", "Quản lý thiết bị");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
                this.GetAssetDataList();
            }
        }

        private void GetAssetDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
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

        private void BindAssestData(List<AssetDto> assetList)
        {
            this.grdAsset.DataSource = null;
            this.grdAsset.DataSource = assetList;
            this.grdAsset.DataBind();
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
            if (this.grdAsset.SelectedItems.Count > 0)
            {
                var OBJ_CODE = this.grdAsset.SelectedItems[0].FindControl("hfAsset") as HiddenField;
                var ORG = this.grdAsset.SelectedItems[0].FindControl("lbOrg") as Label;
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = OBJ_CODE.Value;
                organizationParam.Value = ORG.Text;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_asset_pmschedule", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetPMScheduleList = this.eamService.CreateListFromTable<AssetPMScheduleDto>(ds.Tables[0]);
                }
            }

            this.grdAssetPMSchedule.DataSource = assetPMScheduleList;
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

        public class TreeNodeData
        {
            public int ThietBi { get; set; }
            public string TenThietBi { get; set; }
            public int ThietBiCha { get; set; }
        }


        private void LoadTreeView(AssetDto assetDto)
        {
            List<TreeNodeData> treeData = new List<TreeNodeData>
            {
                new TreeNodeData { ThietBi = 1, TenThietBi = assetDto.TenThietBi, ThietBiCha = 0 },
            };

            var assestList = Session["AssestList"] as List<AssetDto>;
            var childList = assestList.Where(t => t.ThietBiCha == assetDto.ThietBi).ToList();
            if (childList.Count > 0)
            {
                int i = 2;
                foreach (var item in childList)
                {
                    treeData.Add(new TreeNodeData()
                    {
                        ThietBi = i,
                        TenThietBi = item.TenThietBi,
                        ThietBiCha = 1
                    });
                    i++;
                }
            }

            RadTreeView1.DataTextField = "TenThietBi";
            RadTreeView1.DataFieldID = "ThietBi";
            RadTreeView1.DataFieldParentID = "ThietBiCha";
            RadTreeView1.DataSource = treeData;
            RadTreeView1.DataBind();

            foreach (RadTreeNode rtn in RadTreeView1.Nodes)
            {
                rtn.ExpandChildNodes();
                rtn.Expanded = true;
            }
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
            var assetEventList = new List<AssetEventDto>();
            if (this.grdAsset.SelectedItems.Count > 0)
            {
                var OBJ_CODE = this.grdAsset.SelectedItems[0].FindControl("hfAsset") as HiddenField;
                var ORG = this.grdAsset.SelectedItems[0].FindControl("lbOrg") as Label;
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = OBJ_CODE.Value;
                organizationParam.Value = ORG.Text;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_assetEvent_r5", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetEventList = this.eamService.CreateListFromTable<AssetEventDto>(ds.Tables[0]);
                }
            }

            this.grdAssetHistory.DataSource = assetEventList;
        }

        protected void grdAsset_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var assestList = Session["AssestList"] as List<AssetDto>;

            this.grdAsset.DataSource = assestList;
        }

        protected void grdAsset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var OBJ_CODE = this.grdAsset.SelectedItems[0].FindControl("hfAsset") as HiddenField;
            var hfAssetFullName = this.grdAsset.SelectedItems[0].FindControl("hfAssetFullName") as HiddenField;
            var lblAssetName = this.ToolBarAsset.Items[0].FindControl("lblAssetName") as Label;
            lblAssetName.Text = hfAssetFullName.Value;
            var assestList = Session["AssestList"] as List<AssetDto>;
            var assetObj = assestList.FirstOrDefault(t => t.ThietBi == OBJ_CODE.Value);

            this.grdAssetPMSchedule.Rebind();
            this.grdAssetHistory.Rebind();
            this.FillObjectDetail(assetObj);
            this.LoadTreeView(assetObj);
            this.grdAssetParameters.Rebind();
        }

        protected void grdAsset_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                var lbStatus = item.FindControl("lbStatus") as Label;
                var hfStatus = item.FindControl("hfStatus") as HiddenField;
                lbStatus.Text = hfStatus.Value == "I" ? "Đã lắp đặt" : "Đã gỡ bỏ";
            }
        }

        protected void RadTreeView1_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "../../Images/folderdir16.png";
        }

        protected void grdAssetParameters_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var assetParameterList = new List<AssetParameterDto>();
            if (this.grdAsset.SelectedItems.Count > 0)
            {
                var OBJ_CODE = this.grdAsset.SelectedItems[0].FindControl("hfAsset") as HiddenField;
                var ORG = this.grdAsset.SelectedItems[0].FindControl("lbOrg") as Label;
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = OBJ_CODE.Value;
                organizationParam.Value = ORG.Text;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_assetParam_r5", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetParameterList = this.eamService.CreateListFromTable<AssetParameterDto>(ds.Tables[0]);
                }
            }
            this.grdAssetParameters.DataSource = assetParameterList;
        }

        protected void grdAssetParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy chỉ mục (index) của dòng được chọn
            var selectedItem = grdAssetParameters.SelectedItems;

            if (selectedItem.Count > 0)
            {
                // Lấy dòng được chọn từ chỉ mục
                GridDataItem selectedRow = grdAssetParameters.SelectedItems[0] as GridDataItem;

                if (selectedRow != null)
                {
                    // Lấy giá trị của cột "MyColumn" từ dòng được chọn
                    string donvido = selectedRow["DonViDo"].Text;
                    string mota = selectedRow["MoTa"].Text;
                    string totalusage = selectedRow["TotalUsage"].Text;
                    string usagesinceinstall = selectedRow["UsageSinceInstall"].Text;
                    string usagesincelastwo = selectedRow["UsageSinceLastWO"].Text;
                    string typeofmeter = selectedRow["TypeOfMeter"].Text;
                    string physicalmeter = selectedRow["PhysicalMeter"].Text;
                    string meterrollover = selectedRow["MeterRollover"].Text;
                    string lastreading = selectedRow["LastReading"].Text;
                    string lastreadingdate = selectedRow["LastReadingDate"].Text;
                    string updownmeter = selectedRow["UpDownMeter"].Text;

                    // Bây giờ bạn có thể làm gì đó với giá trị của cột, ví dụ:
                    // Hiển thị giá trị trong một Label
                    this.txtDonViDoParam.Text = !string.IsNullOrEmpty(donvido) ? donvido : "";
                    this.txtMoTaParam.Text = !string.IsNullOrEmpty(mota) ? mota : "";
                    this.txtTotalUsage.Text = !string.IsNullOrEmpty(totalusage) ? totalusage : "";
                    this.txtUsageSinceInstall.Text = !string.IsNullOrEmpty(usagesinceinstall) ? usagesinceinstall : "";
                    this.txtUsageSinceLastWO.Text = !string.IsNullOrEmpty(usagesincelastwo) ? usagesincelastwo : "";
                    this.txtTypeOfMeter.Text = !string.IsNullOrEmpty(typeofmeter) ? typeofmeter : "";
                    this.txtPhysicalMeter.Text = !string.IsNullOrEmpty(physicalmeter) && physicalmeter != "&nbsp;" ? physicalmeter : "";
                    this.txtMeterRollover.Text = !string.IsNullOrEmpty(meterrollover) && meterrollover != "&nbsp;" ? meterrollover : "";
                    this.txtLastReading.Text = !string.IsNullOrEmpty(lastreading) ? lastreading : "";
                    this.txtLastReadingDate1.Text = !string.IsNullOrEmpty(lastreadingdate) && lastreadingdate != "&nbsp;" ? lastreadingdate : "";
                    this.cbUpDownMeter.Checked = updownmeter == "+" ? true : false;
                }
            }
        }
    }
}

