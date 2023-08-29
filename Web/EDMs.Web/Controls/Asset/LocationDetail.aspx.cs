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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto.Asset;
using EDMs.Data.Dto.PPM;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Asset
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class LocationDetail : Page
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
            var objectList = new List<LocationDto>();
            ds = this.eamService.GetDataSet("get_location_list", new[] { userParam });
            if (ds != null)
            {
                objectList = this.eamService.CreateListFromTable<LocationDto>(ds.Tables[0]);
                foreach (var item in objectList)
                {
                    item.FullName = item.ViTri + " - " + item.MoTa;
                }
            }

            Session.Add("ObjectList", objectList);
        }

        private void LoadObjectList()
        {
            var objectList = Session["ObjectList"] as List<LocationDto>;
            this.BindObjectData(objectList);
            
        }

        private void BindObjectData(List<LocationDto> objectList)
        {
            this.lbObject.Items.Clear();
            foreach (var item in objectList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.ViTri);
                lbItem.Attributes.Add("PhanLoai", item.PhanLoai);
                lbItem.Attributes.Add("BoPhan", item.BoPhan);
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
            var obj = new LocationDto()
            {
                ViTri = this.lbObject.SelectedItem.Value,
                FullName = this.lbObject.SelectedItem.Text,
                DonVi = this.lbObject.SelectedItem.Attributes["DonVi"]
            };

            this.LoadObjectDocumentTree(obj);
            this.FillObjectDetail(obj);
        }

        private void LoadObjectDocumentTree(LocationDto assetObj)
        {
            this.rtvLocationStructure.Nodes.Clear();
            var objectParam = new SqlParameter("@ViTri", SqlDbType.NVarChar);
            var donViParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            objectParam.Value = assetObj.ViTri;
            donViParam.Value = assetObj.DonVi;
            DataSet lvl1ds;
            DataSet lvl2ds;
            var lvl1Structure = new List<LocationStructureDto>();
            var lvl2Structure = new List<LocationStructureDto>();
            lvl1ds = this.eamService.GetDataSet("get_location_structure", new[] { objectParam, donViParam });
            
            if (lvl1ds != null)
            {
                lvl1Structure = this.eamService.CreateListFromTable<LocationStructureDto>(lvl1ds.Tables[0]);
                var mainNode = new RadTreeNode(assetObj.FullName, assetObj.ViTri);
                mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in lvl1Structure)
                {
                    var lvl1ChildNode = new RadTreeNode(item.ViTri + " - " + item.TenMoTa, item.ViTri + "$" + item.DON_VI);

                    lvl1ChildNode.ImageUrl = item.Loai == "Vị trí" ? @"~/Images/folderdir16.png" : @"~/Images/no-cost-rollup.png";
                    lvl1ChildNode.ExpandMode = TreeNodeExpandMode.ServerSide;
                    mainNode.Nodes.Add(lvl1ChildNode);
                    mainNode.Expanded = true;
                }

                this.rtvLocationStructure.Nodes.Add(mainNode);
            }
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var objectList = Session["ObjectList"] as List<LocationDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterObjectList = objectList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.PhanLoai != null && t.PhanLoai.ToUpper().Contains(searchText))
                                                        || (t.BoPhan != null && t.BoPhan.ToUpper().Contains(searchText))
                                                        || (t.DonVi != null && t.DonVi.ToUpper().Contains(searchText))
                                                        || (t.ViTriCha != null && t.ViTriCha.ToUpper().Contains(searchText))
                                                        || (t.NgungSuDung != null && t.NgungSuDung.ToUpper().Contains(searchText))
                                                        || (t.TienTe != null && t.TienTe.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindObjectData(filterObjectList);
            }
            else
            {
                this.BindObjectData(objectList);
            }
        }

        private void FillObjectDetail(LocationDto item)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@ViTri", SqlDbType.NVarChar);
            objectParam.Value = item.ViTri;
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var objList = new List<LocationDto>();
            ds = this.eamService.GetDataSet("get_location_detail", new[] { userParam, objectParam });
            if (ds != null)
            {
                objList = this.eamService.CreateListFromTable<LocationDto>(ds.Tables[0]);
                var obj = objList[0];

                this.txtViTri.Text = obj.ViTri;
                this.txtDienGiai.Text = obj.MoTa;
                this.txtDonVi.Text = obj.DonVi;
                this.txtViTriCha.Text = obj.ViTriCha;
            }
        }

        protected void rtvLocationStructure_OnNodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
                PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSide);
        }

        private void PopulateNodeOnDemand(RadTreeNodeEventArgs e, TreeNodeExpandMode expandMode)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@ViTri", SqlDbType.NVarChar);
            var donViParam = new SqlParameter("@organization", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            objectParam.Value = e.Node.Value.Split('$')[0];
            donViParam.Value = e.Node.Value.Split('$')[1];
            DataSet ds;
            var objStructureList = new List<LocationStructureDto>();

            ds = this.eamService.GetDataSet("get_location_structure", new[] {objectParam, donViParam});
            if (ds != null)
            {
                objStructureList = this.eamService.CreateListFromTable<LocationStructureDto>(ds.Tables[0]);
                foreach (var item in objStructureList)
                {
                    var lvl2ChildNode = new RadTreeNode(item.ViTri + " - " + item.TenMoTa, item.ViTri + "$" + item.DON_VI);
                    lvl2ChildNode.ImageUrl = item.Loai == "Vị trí" ? @"~/Images/folderdir16.png" : @"~/Images/no-cost-rollup.png";
                    lvl2ChildNode.ExpandMode = expandMode;
                    e.Node.Nodes.Add(lvl2ChildNode);
                }
            }

            e.Node.Expanded = true;
        }
    }
}

