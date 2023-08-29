using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using EAM.Business.Services;
using EAM.Business.Services.Material;
using EAM.Data.Dto;
using EAM.WebPortal.Resources.Utilities;
using Telerik.Web.UI;

namespace EAM.WebPortal.Control.Material
{
    public partial class DetailImexSearching : System.Web.UI.Page
    {
        private readonly AA_MaterialRequestService mrService = new AA_MaterialRequestService();
        private readonly AA_MaterialRequestDetailService mrDetailService = new AA_MaterialRequestDetailService();
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.txtFromDate.SelectedDate = DateTime.Now.AddDays(-30);
                this.txtToDate.SelectedDate = DateTime.Now;

                Session.Remove("CheckedNodes");
                var partList = this.GetPartList();
                this.LoadPartList(partList);
            }
        }
        protected void grdPartInStock_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var partStockList = this.GetPartStock();
            this.grdPartInStock.DataSource = partStockList;
        }

        private List<DetailImexDto> GetPartStock()
        {
            var dsTotal = new DataSet();
            List<DetailImexDto> partStockList = new List<DetailImexDto>();
            if (Session["CheckedNodes"] != null && this.txtToDate.SelectedDate != null && this.txtFromDate.SelectedDate != null && this.ddlStore.CheckedItems.Count > 0)
            {
                foreach (RadComboBoxItem store in this.ddlStore.CheckedItems)
                {
                    DataSet ds;
                    var storeParam = new SqlParameter("@storeCode", SqlDbType.NVarChar, 30);
                    storeParam.Value = store.Value;
                    var fromDateParam = new SqlParameter("@startDate", SqlDbType.DateTime);
                    fromDateParam.Value = this.txtFromDate.SelectedDate;
                    var toDateParam = new SqlParameter("@endDate", SqlDbType.DateTime);
                    toDateParam.Value = this.txtToDate.SelectedDate;

                    ds = this.eamService.GetDataSet("AA_SoChiTietNhapXuat", new[] { storeParam, fromDateParam, toDateParam });
                    if (ds != null)
                    {
                        dsTotal.Merge(ds);

                    }
                }

                partStockList = this.eamService.CreateListFromTable<DetailImexDto>(dsTotal.Tables[0]).OrderBy(t => t.TRA_DATE).ToList();

                var checkedPart = ((List<PartListDto>)Session["CheckedNodes"]).Select(t => t.VatTu);
                partStockList = partStockList.Where(t => checkedPart.Contains(t.par_code)).ToList();
            }

            return partStockList;
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            if (Session["PartList"] is List<PartListSYTDto> partList)
            {
                var searchText = this.txtSearch.Text.ToUpper().Trim();
                partList = partList.Where(t => (!string.IsNullOrEmpty(t.PHANLOAI) && t.PHANLOAI.ToUpper().Contains(searchText))
                                               || t.FullName.ToUpper().Contains(searchText)
                                               || t.DONVI.ToUpper().Contains(searchText)
                                               || t.DVT.ToUpper().Contains(searchText)
                ).ToList();

                this.LoadPartList(partList);
            }
        }

        protected void rtvPart_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            var checkedNodes = new List<PartListDto>();
            if (Session["CheckedNodes"] != null)
            {
                checkedNodes = (List<PartListDto>)Session["CheckedNodes"];
                var checkedNode = checkedNodes.FirstOrDefault(t => t.VatTu == e.Node.Value);
                if (e.Node.Checked && !e.Node.Value.Contains("ROOT$"))
                {
                    checkedNodes.Add(new PartListDto()
                    {
                        FullName = e.Node.Text,
                        VatTu = e.Node.Value
                    });
                }
                else
                {
                    checkedNodes.Remove(checkedNode);
                }

                Session.Add("CheckedNodes", checkedNodes);
            }
            else
            {
                if (e.Node.Checked && !e.Node.Value.Contains("ROOT$"))
                {
                    checkedNodes.Add(new PartListDto()
                    {
                        FullName = e.Node.Text,
                        VatTu = e.Node.Value
                    });
                }

                Session.Add("CheckedNodes", checkedNodes);
            }
        }

        private void LoadPartList(List<PartListSYTDto> partList)
        {
            this.rtvPart.Nodes.Clear();
            var checkedNodes = new List<PartListDto>();
            if (Session["CheckedNodes"] != null)
            {
                checkedNodes = (List<PartListDto>)Session["CheckedNodes"];
            }

            foreach (var phanLoaiGroup in partList.GroupBy(t => t.PHANLOAI))
            {
                var mainNode = new RadTreeNode("<b>Nhóm: </b>" + phanLoaiGroup.Key, "ROOT$" + phanLoaiGroup.Key);
                //mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in phanLoaiGroup)
                {
                    var childNode = new RadTreeNode(item.FullName, item.MAVT);
                    childNode.Checked = checkedNodes.Any(t => t.VatTu == childNode.Value);

                    //childNode.ImageUrl = @"~/Images/equiment16.png";
                    mainNode.Nodes.Add(childNode);
                    mainNode.Expanded = true;
                }

                this.rtvPart.Nodes.Add(mainNode);
            }
        }

        private List<PartListSYTDto> GetPartList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = "ADMIN";
            DataSet ds;
            var partList = new List<PartListSYTDto>();
            ds = this.eamService.GetDataSet("getSpareParts", new[] { userParam });
            if (ds != null)
            {
                partList = this.eamService.CreateListFromTable<PartListSYTDto>(ds.Tables[0]);
                foreach (var item in partList)
                {
                    item.FullName = item.MAVT + " - " + item.DIENGIAI;
                }

                Session.Add("PartList", partList);
            }

            return partList;
        }

        protected void btnSearchFull_OnClick(object sender, EventArgs e)
        {
            if (this.txtFromDate.SelectedDate != null && this.txtToDate.SelectedDate != null && this.rtvPart.CheckedNodes.Count > 0 && this.ddlStore.CheckedItems.Count > 0)
            {
                this.grdPartInStock.Rebind();
            }
            else
            {
                this.errorNotification.Show("Vui lòng chọn đầy đủ điều kiện lọc<br/>" +
                                            " _ Từ ngày - Đến ngày.<br/>" +
                                            "_ Kho.<br/>" +
                                            "_ Vật tư.");
            }
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            this.txtSearch.Text = string.Empty;
            Session.Remove("CheckedNodes");
            this.rtvPart.UncheckAllNodes();
            this.ddlStore.ClearCheckedItems();
            this.grdPartInStock.Rebind();
        }
    }
}