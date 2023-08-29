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
using EDMs.Data.Dto;

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
    public partial class AssetManagement : Page
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
                this.lbAsset.SelectedIndex = 0;
            }
        }

        private void GetAssetDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var assetList = new List<AssetDto>();
            ds = this.eamService.GetDataSet("get_asset", new[] { userParam });
            if (ds != null)
            {
                assetList = this.eamService.CreateListFromTable<AssetDto>(ds.Tables[0]);
                foreach (var item in assetList)
                {
                    item.FullName = item.MA_THIET_BI + " - " + item.TEN_THIET_BI;
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
                var lbItem = new RadListBoxItem(item.FullName, item.MA_THIET_BI);
                lbItem.Attributes.Add("DON_VI", item.DON_VI);
                lbItem.Attributes.Add("BO_PHAN", item.BO_PHAN);
                lbItem.Attributes.Add("TRANG_THAI", item.TRANG_THAI);
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
            var assetObj = new AssetDto()
            {
                MA_THIET_BI = this.lbAsset.SelectedItem.Value,
                FullName = this.lbAsset.SelectedItem.Text,
                DON_VI = this.lbAsset.SelectedItem.Attributes["DON_VI"]
            };

            this.LoadAssetStructure(assetObj);
            this.grdAssetPMSchedule.Rebind();
            this.grdAssetPartUsage.Rebind();
            this.grdAssetPartAssociated.Rebind();
            this.LoadAssetDocumentTree(assetObj);
            this.grdAssetDepreciation.Rebind();
            this.grdAssetEvent.Rebind();
        }

        private void LoadAssetDocumentTree(AssetDto assetObj)
        {
            this.rtvAssetDocument.Nodes.Clear();
            var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            objectParam.Value = assetObj.MA_THIET_BI;
            organizationParam.Value = assetObj.DON_VI;
            DataSet ds;
            var documentList = new List<AssetDocumentDto>();
            ds = this.eamService.GetDataSet("get_asset_documents", new[] { objectParam, organizationParam });
            if (ds != null)
            {
                documentList = this.eamService.CreateListFromTable<AssetDocumentDto>(ds.Tables[0]);
                var mainNode = new RadTreeNode("<b>Thiết bị: </b>" + assetObj.FullName, assetObj.MA_THIET_BI);
                mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in documentList)
                {
                    var childNode = new RadTreeNode(item.MA_TAI_LIEU + ": " + item.TEN_TAI_LIEU, item.MA_TAI_LIEU);
                    childNode.ImageUrl = @"~/Images/documents.png";

                    mainNode.Nodes.Add(childNode);
                    mainNode.Expanded = true;
                }

                this.rtvAssetDocument.Nodes.Add(mainNode);

            }
        }

        private void LoadAssetStructure(AssetDto assetObj)
        {
            this.rtvAssetLvl.Nodes.Clear();
            var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
            var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            objectParam.Value = assetObj.MA_THIET_BI;
            organizationParam.Value = assetObj.DON_VI;
            DataSet ds;
            var assetList = new List<AssetDto>();
            ds = this.eamService.GetDataSet("get_asset_structure", new[] { objectParam, organizationParam });
            if (ds != null)
            {
                assetList = this.eamService.CreateListFromTable<AssetDto>(ds.Tables[0]);
                var mainNode = new RadTreeNode(assetObj.FullName, assetObj.MA_THIET_BI);
                mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in assetList)
                {
                    var childNode = new RadTreeNode(item.MA_THIET_BI + " - " + item.TEN_THIET_BI, item.MA_THIET_BI);
                    childNode.ImageUrl = @"~/Images/folderdir16.png";

                    mainNode.Nodes.Add(childNode);
                    mainNode.Expanded = true;
                }

                this.rtvAssetLvl.Nodes.Add(mainNode);

            }
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var assetList = Session["AssestList"] as List<AssetDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterAssestList = assetList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.BO_PHAN != null && t.BO_PHAN.ToUpper().Contains(searchText))
                                                        || (t.DON_VI != null && t.DON_VI.ToUpper().Contains(searchText))
                                                        || (t.TRANG_THAI != null && t.TRANG_THAI.ToUpper().Contains(searchText))
                                                        || (t.TINH_TRANG != null && t.TINH_TRANG.ToUpper().Contains(searchText))
                                                        || (t.NGUOI_PHU_TRACH != null && t.NGUOI_PHU_TRACH.ToUpper().Contains(searchText))
                                                        || (t.THONG_SO_KY_THUAT != null && t.THONG_SO_KY_THUAT.ToUpper().Contains(searchText))
                                                        || (t.NHA_SAN_XUAT != null && t.NHA_SAN_XUAT.ToUpper().Contains(searchText))
                                                        || (t.NHA_CUNG_CAP != null && t.NHA_CUNG_CAP.ToUpper().Contains(searchText))
                                                        || (t.MODEL != null && t.MODEL.ToUpper().Contains(searchText))
                                                        || (t.SERIAL != null && t.SERIAL.ToUpper().Contains(searchText))
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
                    MA_THIET_BI = this.lbAsset.SelectedItem.Value,
                    DON_VI = this.lbAsset.SelectedItem.Attributes["DON_VI"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.MA_THIET_BI;
                organizationParam.Value = assetObj.DON_VI;
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
            var assetPartUsageList = new List<AssetPartUsageDto>();
            if (this.lbAsset.SelectedItem != null)
            {
                var assetObj = new AssetDto()
                {
                    MA_THIET_BI = this.lbAsset.SelectedItem.Value,
                    DON_VI = this.lbAsset.SelectedItem.Attributes["DON_VI"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.MA_THIET_BI;
                organizationParam.Value = assetObj.DON_VI;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_asset_part_usage", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetPartUsageList = this.eamService.CreateListFromTable<AssetPartUsageDto>(ds.Tables[0]);
                }
            }

            this.grdAssetPartUsage.DataSource = assetPartUsageList;
        }

        protected void grdAssetPartAssociated_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var assetPartAssociatedList = new List<AssetPartAssociatedDto>();
            if (this.lbAsset.SelectedItem != null)
            {
                var assetObj = new AssetDto()
                {
                    MA_THIET_BI = this.lbAsset.SelectedItem.Value,
                    DON_VI = this.lbAsset.SelectedItem.Attributes["DON_VI"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.MA_THIET_BI;
                organizationParam.Value = assetObj.DON_VI;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_asset_part_associated", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetPartAssociatedList = this.eamService.CreateListFromTable<AssetPartAssociatedDto>(ds.Tables[0]);
                }
            }

            this.grdAssetPartAssociated.DataSource = assetPartAssociatedList;
        }

        protected void grdAssetDepreciation_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var assetDepreciationList = new List<AssetDepreciationDto>();
            if (this.lbAsset.SelectedItem != null)
            {
                var assetObj = new AssetDto()
                {
                    MA_THIET_BI = this.lbAsset.SelectedItem.Value,
                    DON_VI = this.lbAsset.SelectedItem.Attributes["DON_VI"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.MA_THIET_BI;
                organizationParam.Value = assetObj.DON_VI;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_asset_depreciations", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetDepreciationList = this.eamService.CreateListFromTable<AssetDepreciationDto>(ds.Tables[0]);
                }
            }

            this.grdAssetDepreciation.DataSource = assetDepreciationList;
        }

        protected void grdAssetEvent_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var assetEventList = new List<AssetEventDto>();
            if (this.lbAsset.SelectedItem != null)
            {
                var assetObj = new AssetDto()
                {
                    MA_THIET_BI = this.lbAsset.SelectedItem.Value,
                    DON_VI = this.lbAsset.SelectedItem.Attributes["DON_VI"]
                };
                var objectParam = new SqlParameter("@object", SqlDbType.NVarChar);
                var organizationParam = new SqlParameter("@organization", SqlDbType.NVarChar);
                objectParam.Value = assetObj.MA_THIET_BI;
                organizationParam.Value = assetObj.DON_VI;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_asset_events", new[] { objectParam, organizationParam });
                if (ds != null)
                {
                    assetEventList = this.eamService.CreateListFromTable<AssetEventDto>(ds.Tables[0]);
                }
            }

            this.grdAssetEvent.DataSource = assetEventList;
        }
    }
}

