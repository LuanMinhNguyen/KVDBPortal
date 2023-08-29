using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EAM.Business.Services;
using EAM.Business.Services.Material;
using EAM.Data.Dto;
using EAM.Data.Entities;
using EAM.WebPortal.Resources.Utilities;
using Telerik.Web.UI;

namespace EAM.WebPortal.Control.Material
{
    public partial class MaterialRequestDetail : System.Web.UI.Page
    {
        private readonly AA_MaterialRequestService mrService = new AA_MaterialRequestService();
        private readonly AA_MaterialRequestDetailService mrDetailService = new AA_MaterialRequestDetailService();
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();
        private readonly AA_MaterialRequestAttachFileService mrAttachFileService = new AA_MaterialRequestAttachFileService();
        private readonly AA_PartLibraryForRequestService partLibraryForRequestService = new AA_PartLibraryForRequestService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["userId"]))
                {
                    Session.Remove("ItemId");
                    var userid = Request.QueryString["userId"].ToUpper();
                    var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
                    var userInfor = Utility.GetUserInfor(userid);
                    if (userInfor != null)
                    {
                        Session.Add("UserInfor", userInfor);

                        var isDinhKy = this.ddlRequestType.SelectedValue == "1";
                        this.LoadMaterialRequest(userInfor, isDinhKy);

                        var ds = eamService.GetDataSet("getEAMPAvailable");
                        if (ds != null)
                        {
                            var partList = eamService.CreateListFromTable<PartListSYTAvailableDto>(ds.Tables[0]);
                            Session.Add("AvailablePart", partList);
                        }

                        this.ExportToolBar.Visible = userInfor.MANHOM == authGroup || userInfor.MANHOM == "1" || userInfor.MANHOM == "ADMIN";
                    }
                }
            }
        }

        private void LoadInitData(UserInforDto userInfor)
        {
            Session.Add("ItemId", "");
            this.lbMR.ClearSelection();

            this.ViewToolBar.Items[5].Enabled = true;
            this.AllowSave.Value = "true";
            this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
            this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = false;
            this.grdPart.MasterTableView.GetColumn("FromStoreCode").Display = false;
            this.grdPart.MasterTableView.GetColumn("ApprovedQty").Display = false;
            this.grdPart.Rebind();
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = string.Empty;
            this.txtDescription.Focus();
            this.txtDescription.ReadOnly = false;
            this.txtCode.Text = string.Empty;
            this.txtNote.Text = string.Empty;
            this.txtNote.ReadOnly = false;
            this.txtRequestBy.Text = string.Empty;
            this.txtRequestBy.ReadOnly = false;
            this.txtRequestDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            this.txtSoTYT.Text = String.Empty;
            this.txtSoTYT.ReadOnly = false;

            this.txtSoCachLyTaiNha.Text = String.Empty;
            this.txtSoCachLyTaiNha.ReadOnly = false;

            this.txtSoCachLyTapTrung.Text = String.Empty;
            this.txtSoCachLyTapTrung.ReadOnly = false;

            this.txtSoLayMauXN.Text = String.Empty;
            this.txtSoLayMauXN.ReadOnly = false;


            this.btnProcess.Visible = false;
            this.cbIsGenMR.Visible = false;

            this.ddlStatus.Items.Clear();
            this.ddlStatus.Enabled = true;
            var statusList = new List<DropDownListItem>()
            {
                new DropDownListItem("Đang lập phiếu", "1"),
                new DropDownListItem("Chờ CDC xét duyệt", "2")
            };
            foreach (var item in statusList)
            {
                this.ddlStatus.Items.Add(item);
            }

            this.ddlOrg.Items.Clear();
            this.ddlOrg.Enabled = true;

            foreach (var org in userInfor.Organizations)
            {
                var orgItem = new DropDownListItem(org.TENDONVI, org.MADONVI);
                this.ddlOrg.Items.Add(orgItem);
            }

            if (this.ddlOrg.SelectedItem != null)
            {
                this.txtDescription.Text = "YC cấp vật tư";

                this.ddlStore.Items.Clear();
                this.ddlStore.Enabled = true;

                DataSet ds;
                var orgParam = new SqlParameter("@Org", SqlDbType.NVarChar, 15);
                orgParam.Value = this.ddlOrg.SelectedValue;
                ds = eamService.GetDataSet("getStorebyOrg", new[] { orgParam });
                if (ds != null)
                {
                    var storeList = eamService.CreateListFromTable<StoreDto>(ds.Tables[0]);
                    foreach (var store in storeList)
                    {
                        var storeItem = new DropDownListItem(store.TENKHO, store.MAKHO);
                        this.ddlStore.Items.Add(storeItem);
                    }
                }
            }

        }

        private void LoadMaterialRequest(UserInforDto userInfor, bool isDinhKy)
        {
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var searchText = this.txtSearch.Text.Trim().ToUpper();
            var mrList = this.mrService.Search(userInfor, authGroup, searchText, isDinhKy);
            this.lbMR.Items.Clear();
            foreach (var item in mrList)
            {
                var lbItem = new RadListBoxItem(item.Code + "-" + item.Description, item.ID.ToString());
                lbItem.Attributes.Add("OrganizationName", item.OrganizationName);
                lbItem.Attributes.Add("RequestBy", item.RequestBy);
                lbItem.Attributes.Add("StatusName", item.StatusName);
                this.lbMR.Items.Add(lbItem);
            }

            this.lbMR.DataBind();
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var userInfor = Session["UserInfor"] as UserInforDto;
            if (userInfor != null)
            {
                var isDinhKy = this.ddlRequestType.SelectedValue == "1";
                this.LoadMaterialRequest(userInfor, isDinhKy);
            }
        }

       private List<StoreToStoreReqDto> PartReqObj(DataTable dataTable, int partCount, Worksheet worksheet)
        {
            var storeToStoreReqList = new List<StoreToStoreReqDto>();
            var regex = new Regex("^[0-9]+$", RegexOptions.Compiled);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var storeToStoreReq = new StoreToStoreReqDto()
                {
                    FromCode = dataRow["Column4"].ToString(),
                    ToCode = dataRow["Column2"].ToString(),
                    ToCodeName = dataRow["Column1"].ToString(),
                    UserName = Request.QueryString["userid"],
                    LineList = new List<StoreToStoreReqLineDto>()
                };

                for (int i = 1; i <= partCount; i++)
                {
                    if (!string.IsNullOrEmpty(worksheet.Cells[0, 5 + i].Value.ToString()))
                    {
                        var partItem = new StoreToStoreReqLineDto()
                        {
                            PartNumber = worksheet.Cells[0, 5 + i].Value.ToString().Split('-')[0].Trim(),
                            Quality = !string.IsNullOrEmpty(dataRow["Column" + (4 + i)].ToString()) && regex.Match(dataRow["Column" + (4 + i)].ToString()).Success 
                                ? Convert.ToDouble(dataRow["Column" + (4 + i)].ToString()) 
                                : 0
                        };

                        if (partItem.Quality != 0)
                        {
                            storeToStoreReq.LineList.Add(partItem);
                        }
                    }
                }

                if (storeToStoreReq.LineList.Count > 0)
                {
                    storeToStoreReqList.Add(storeToStoreReq);
                }
            }

            return storeToStoreReqList;
        }
        
        protected void lbMR_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbMR.SelectedItem.Text;
            var mrItem = this.mrService.GetById(new Guid(this.lbMR.SelectedValue));
            if (mrItem != null)
            {
                Session.Add("ItemId", mrItem.ID.ToString());
                this.FillData(mrItem);
            }
        }

        private void FillData(AA_MaterialRequest item)
        {
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var userInfor = Session["UserInfor"] as UserInforDto;
            this.divItemDescription.Visible = true;
            this.lblItemDescription.Text = item.Code + " - " + item.Description;

            this.cbMonthly.Checked = item.IsMonthlyRequest.GetValueOrDefault();
            this.cbUnexpected.Checked = !item.IsMonthlyRequest.GetValueOrDefault();
            this.lblDuTru.Text = cbMonthly.Checked ? "Dự trù cho tháng:" : "Dự trù cho sử dụng từ ngày - đến ngày:";
            //this.divTitleCV.Visible = !this.cbMonthly.Checked;
            //this.divDetailCV1.Visible = !this.cbMonthly.Checked;
            //this.divDetailCV2.Visible = !this.cbMonthly.Checked;

            
            this.txtRequestForMonth.Text = item.RequestForMonth;

            this.txtCode.Text = item.Code.ToString();
            this.txtDescription.Text = item.Description;
            this.txtNote.Text = item.Note;
            this.txtRequestBy.Text = item.RequestBy;
            this.txtRequestDate.Text = item.RequestDate.GetValueOrDefault().ToString("dd-MMM-yyyy");
            this.txtSoCachLyTapTrung.Text = item.SoNguoiCachLyTapTrung;
            this.txtSoCachLyTaiNha.Text = item.SoNguoiCachLyTaiNha;
            this.txtSoTYT.Text = item.SoTYT;
            this.txtSoLayMauXN.Text = item.SoLayMauXN;
            this.BuildStatus(userInfor, authGroup, item.StatusId.GetValueOrDefault());
            this.ddlStatus.SelectedValue = item.StatusId.ToString();
            this.btnProcess.Visible = userInfor.MANHOM == authGroup && item.StatusId == 3 && !item.IsMonthlyRequest.GetValueOrDefault() && !item.IsGenEAMMR.GetValueOrDefault();
            this.cbIsGenMR.Visible = userInfor.MANHOM == authGroup && item.StatusId == 3 && !item.IsMonthlyRequest.GetValueOrDefault() && item.IsGenEAMMR.GetValueOrDefault();

            this.ddlOrg.Items.Clear();
            foreach (var org in userInfor.Organizations)
            {
                var orgItem = new DropDownListItem(org.TENDONVI, org.MADONVI);
                this.ddlOrg.Items.Add(orgItem);
            }
            this.ddlOrg.SelectedValue = item.OrganizationCode;

            if (this.ddlOrg.SelectedItem != null)
            {
                this.ddlStore.Items.Clear();
                DataSet ds;
                var orgParam = new SqlParameter("@Org", SqlDbType.NVarChar, 15);
                orgParam.Value = this.ddlOrg.SelectedValue;
                ds = eamService.GetDataSet("getStorebyOrg", new[] { orgParam });
                if (ds != null)
                {
                    var storeList = eamService.CreateListFromTable<StoreDto>(ds.Tables[0]);
                    foreach (var store in storeList)
                    {
                        var storeItem = new DropDownListItem(store.TENKHO, store.MAKHO);
                        this.ddlStore.Items.Add(storeItem);
                    }
                }
            }

            this.ddlStore.SelectedValue = item.StoreCode;

            if (item.StatusId > 1)
            {
                this.ViewToolBar.Items[6].Enabled = false;
                this.AllowDelete.Value = "false";
                
                this.txtDescription.ReadOnly = true;
                this.txtRequestBy.ReadOnly = true;
                this.txtNote.ReadOnly = true;
                this.txtSoCV.ReadOnly = true;
                this.txtNgayCV.Enabled = false;
                this.txtNoiDungCV.ReadOnly = true;

                this.ddlOrg.Enabled = false;
                this.ddlStore.Enabled = false;
                this.grdPart.MasterTableView.GetColumn("FromStoreCode").Display = true;
                this.grdPart.MasterTableView.GetColumn("ApprovedQty").Display = true;
                //this.BuildStatus(userInfor,authGroup, item.StatusId.GetValueOrDefault());
                if (userInfor.MANHOM == authGroup)
                {
                    this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
                    this.grdPart.MasterTableView.EditMode = GridEditMode.Batch;
                    this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = true;
                    this.grdPart.MasterTableView.GetColumn("KHO_PCD").Display = true;
                    this.grdPart.MasterTableView.GetColumn("KHOTAITRO").Display = true;
                    this.grdPart.MasterTableView.GetColumn("KHO_DUOC").Display = true;
                    this.ddlStatus.Enabled = true;
                    this.txtSoTYT.ReadOnly = false;
                    this.txtSoCachLyTaiNha.ReadOnly = false;
                    this.txtSoCachLyTapTrung.ReadOnly = false;
                    this.txtSoLayMauXN.ReadOnly = false;
                    this.ViewToolBar.Items[5].Enabled = true;
                    this.AllowSave.Value = "true";
                    //this.TabObjectDetail.Tabs[2].Visible = true;
                }
                else
                {
                    this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    this.grdPart.MasterTableView.EditMode = GridEditMode.EditForms;
                    this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = false;
                    this.grdPart.MasterTableView.GetColumn("KHO_PCD").Display = false;
                    this.grdPart.MasterTableView.GetColumn("KHOTAITRO").Display = false;
                    this.grdPart.MasterTableView.GetColumn("KHO_DUOC").Display = false;
                    this.ddlStatus.Enabled = false;
                    this.txtSoTYT.ReadOnly = true;
                    this.txtSoCachLyTaiNha.ReadOnly = true;
                    this.txtSoCachLyTapTrung.ReadOnly = true;
                    this.txtSoLayMauXN.ReadOnly = true;
                    this.ViewToolBar.Items[5].Enabled = false;
                    this.AllowSave.Value = "false";

                    //this.TabObjectDetail.Tabs[2].Visible = false;
                }
            }
            else
            {
                this.ViewToolBar.Items[5].Enabled = true;
                this.ViewToolBar.Items[6].Enabled = true;
                this.AllowDelete.Value = "true";
                this.AllowSave.Value = "true";
                this.txtDescription.ReadOnly = false;
                this.txtRequestBy.ReadOnly = false;
                this.txtNote.ReadOnly = false;
                this.ddlOrg.Enabled = true;
                this.ddlStore.Enabled = true;
                this.ddlStatus.Enabled = true;
                this.txtSoTYT.ReadOnly = false;
                this.txtSoCachLyTaiNha.ReadOnly = false;
                this.txtSoCachLyTapTrung.ReadOnly = false;
                this.txtSoLayMauXN.ReadOnly = false;
                this.grdPart.MasterTableView.GetColumn("FromStoreCode").Display = false;
                this.grdPart.MasterTableView.GetColumn("ApprovedQty").Display = false;
                this.grdPart.MasterTableView.GetColumn("KHO_PCD").Display = false;
                this.grdPart.MasterTableView.GetColumn("KHOTAITRO").Display = false;
                this.grdPart.MasterTableView.GetColumn("KHO_DUOC").Display = false;
                this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
                this.grdPart.MasterTableView.EditMode = GridEditMode.Batch;
                this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = true;
            }

            this.grdPart.Rebind();
            this.grdPartInStock.Rebind();
            this.grdAttachFile.Rebind();
        }

        private void BuildStatus(UserInforDto userInfor, string authGroup, int currentStatus)
        {
            this.ddlStatus.Items.Clear();
            var statusList = new List<DropDownListItem>();
            switch (currentStatus)
            {
                case 1:
                    statusList = new List<DropDownListItem>()
                    {
                        new DropDownListItem("Đang lập phiếu", "1"),
                        new DropDownListItem("Gửi HCDC phê duyệt", "2")
                    };
                    break;
                case 2:
                    if (userInfor.MANHOM == authGroup)
                    {
                        statusList = new List<DropDownListItem>()
                        {
                            new DropDownListItem("Chờ HCDC phê duyệt", "2"),
                            new DropDownListItem("HCDC không duyệt", "4"),
                            new DropDownListItem("Đã phê duyệt", "3")
                        };
                    }
                    else
                    {
                        statusList = new List<DropDownListItem>()
                        {
                            new DropDownListItem("Đang lập phiếu", "1"),
                            new DropDownListItem("Gửi HCDC phê duyệt", "2")
                        };
                    }
                    break;
                case 3:
                case 4:
                    statusList = new List<DropDownListItem>()
                    {
                        new DropDownListItem("Chờ HCDC phê duyệt", "2"),
                        new DropDownListItem("HCDC không duyệt", "4"),
                        new DropDownListItem("Đã phê duyệt", "3")
                    };
                    break;

            }

            foreach (var statusItem in statusList)
            {
                this.ddlStatus.Items.Add(statusItem);
            }
        }

        protected void ddlOrg_OnSelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (this.ddlOrg.SelectedItem != null)
            {
                this.ddlStore.Items.Clear();
                DataSet ds;
                var orgParam = new SqlParameter("@Org", SqlDbType.NVarChar, 15);
                orgParam.Value = this.ddlOrg.SelectedValue;
                ds = eamService.GetDataSet("getStorebyOrg", new[] { orgParam });
                if (ds != null)
                {
                    var storeList = eamService.CreateListFromTable<StoreDto>(ds.Tables[0]);
                    foreach (var store in storeList)
                    {
                        //store.FullName = store.MAKHO + "-" + store.TENKHO;
                        var storeItem = new DropDownListItem(store.TENKHO, store.MAKHO);
                        this.ddlStore.Items.Add(storeItem);
                    }
                }
            }
        }

        protected void ViewToolBar_OnButtonClick(object sender, RadToolBarEventArgs e)
        {
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var userInfor = Session["UserInfor"] as UserInforDto;
            switch (e.Item.Value)
            {
                case "Insert":
                    Session.Add("ItemId", "");
                    this.LoadInitData(userInfor);
                    break;
                case "Update":
                    //var validation = this.ValidateForm();
                    //if (!string.IsNullOrEmpty(validation))
                    //{
                    //    this.errorNotification.Show(validation);
                    //}
                    //else
                    //{
                    //    this.errorNotification.Show("Test notification");
                    //}

                    var validateMess = this.ValidateForm();
                    if (!string.IsNullOrEmpty(validateMess))
                    {
                        this.errorNotification.Show(validateMess);
                    }
                    else
                    {
                        var itemId = Session["ItemId"].ToString();
                        var mrItem = new AA_MaterialRequest();
                        if (string.IsNullOrEmpty(itemId))
                        {
                            // Insert new 
                            mrItem = new AA_MaterialRequest()
                            {
                                ID = Guid.NewGuid(),
                                Description = this.txtDescription.Text.Trim(),
                                OrganizationCode = this.ddlOrg.SelectedValue,
                                OrganizationName = this.ddlOrg.SelectedItem.Text,
                                StoreCode = this.ddlStore.SelectedValue,
                                StoreName = this.ddlStore.SelectedItem.Text,
                                RequestDate = DateTime.Now,
                                RequestBy = this.txtRequestBy.Text.Trim(),
                                Note = this.txtNote.Text.Trim(),
                                StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue),
                                StatusName = this.ddlStatus.SelectedItem.Text,
                                SoNguoiCachLyTapTrung = this.txtSoCachLyTapTrung.Text.Trim(),
                                SoNguoiCachLyTaiNha = this.txtSoCachLyTaiNha.Text.Trim(),
                                SoLayMauXN = this.txtSoLayMauXN.Text.Trim(),
                                SoTYT = this.txtSoTYT.Text.Trim(),


                                CreatedBy = userInfor.TENNGUOIDUNG,
                                CreatedById = userInfor.MANGUOIDUNG,
                                CreatedDate = DateTime.Now
                            };

                            var mrItemId = this.mrService.Insert(mrItem);
                            if (mrItemId != null)
                            {
                                mrItem = this.mrService.GetById(mrItemId.Value);
                                this.txtCode.Text = mrItem.Code.ToString();
                                var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
                                lblObjectName.Text = mrItem.Code + " - " + mrItem.Description;
                                Session.Add("ItemId", mrItemId.ToString());
                            }
                        }
                        else
                        {
                            // Edit
                            mrItem = this.mrService.GetById(new Guid(itemId));
                            if (mrItem != null)
                            {
                                mrItem.Description = this.txtDescription.Text.Trim();
                                if (mrItem.StatusId == 1)
                                {
                                    mrItem.OrganizationCode = this.ddlOrg.SelectedValue;
                                    mrItem.OrganizationName = this.ddlOrg.SelectedItem.Text;
                                    mrItem.StoreCode = this.ddlStore.SelectedValue;
                                    mrItem.StoreName = this.ddlStore.SelectedItem.Text;
                                }
                                
                                mrItem.Note = this.txtNote.Text.Trim();
                                mrItem.StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
                                mrItem.StatusName = this.ddlStatus.SelectedItem.Text;
                                mrItem.SoNguoiCachLyTapTrung = this.txtSoCachLyTapTrung.Text.Trim();
                                mrItem.SoNguoiCachLyTaiNha = this.txtSoCachLyTaiNha.Text.Trim();
                                mrItem.SoLayMauXN = this.txtSoLayMauXN.Text.Trim();
                                mrItem.SoTYT = this.txtSoTYT.Text.Trim();
                                mrItem.UpdatedBy = userInfor.TENNGUOIDUNG;
                                mrItem.UpdatedById = userInfor.MANGUOIDUNG;
                                mrItem.UpdatedDate = DateTime.Now;
                                mrItem.RequestBy = this.txtRequestBy.Text.Trim();
                                this.mrService.Update(mrItem);
                            }
                        }
                        this.btnProcess.Visible = userInfor.MANHOM == authGroup && mrItem.StatusId == 3 && !mrItem.IsGenEAMMR.GetValueOrDefault();
                        this.cbIsGenMR.Visible = userInfor.MANHOM == authGroup && mrItem.StatusId == 3 && mrItem.IsGenEAMMR.GetValueOrDefault();

                        var isDinhKy = this.ddlRequestType.SelectedValue == "1";
                        this.LoadMaterialRequest(userInfor, isDinhKy);
                        if (mrItem.StatusId > 1)
                        {
                            this.ViewToolBar.Items[6].Enabled = false;
                            this.AllowDelete.Value = "false";
                            this.txtDescription.ReadOnly = true;
                            this.txtNote.ReadOnly = true;
                            this.ddlOrg.Enabled = false;
                            this.ddlStore.Enabled = false;
                            this.grdPart.MasterTableView.GetColumn("FromStoreCode").Display = true;
                            this.grdPart.MasterTableView.GetColumn("ApprovedQty").Display = true;
                            if (userInfor.MANHOM == authGroup)
                            {
                                this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
                                this.grdPart.MasterTableView.EditMode = GridEditMode.Batch;
                                this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = true;
                                this.grdPart.MasterTableView.GetColumn("KHO_PCD").Display = true;
                                this.grdPart.MasterTableView.GetColumn("KHOTAITRO").Display = true;
                                this.grdPart.MasterTableView.GetColumn("KHO_DUOC").Display = true;
                                this.ddlStatus.Enabled = true;
                                this.txtSoTYT.ReadOnly = false;
                                this.txtSoCachLyTaiNha.ReadOnly = false;
                                this.txtSoCachLyTapTrung.ReadOnly = false;
                                this.txtSoLayMauXN.ReadOnly = false;
                            }
                            else
                            {
                                this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                                this.grdPart.MasterTableView.EditMode = GridEditMode.EditForms;
                                this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                this.grdPart.MasterTableView.GetColumn("KHO_PCD").Display = false;
                                this.grdPart.MasterTableView.GetColumn("KHOTAITRO").Display = false;
                                this.grdPart.MasterTableView.GetColumn("KHO_DUOC").Display = false;
                                this.ddlStatus.Enabled = false;
                                this.txtSoTYT.ReadOnly = true;
                                this.txtSoCachLyTaiNha.ReadOnly = true;
                                this.txtSoCachLyTapTrung.ReadOnly = true;
                                this.txtSoLayMauXN.ReadOnly = true;
                            }
                        }
                        else
                        {
                            this.ViewToolBar.Items[6].Enabled = true;
                            this.AllowDelete.Value = "true";
                            this.txtDescription.ReadOnly = false;
                            this.txtNote.ReadOnly = false;
                            this.ddlOrg.Enabled = true;
                            this.ddlStore.Enabled = true;
                            this.ddlStatus.Enabled = true;
                            this.txtSoTYT.ReadOnly = false;
                            this.txtSoCachLyTaiNha.ReadOnly = false;
                            this.txtSoCachLyTapTrung.ReadOnly = false;
                            this.txtSoLayMauXN.ReadOnly = false;

                            this.grdPart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
                            this.grdPart.MasterTableView.EditMode = GridEditMode.Batch;
                            this.grdPart.MasterTableView.GetColumn("DeleteColumn").Display = true;
                            this.grdPart.MasterTableView.GetColumn("FromStoreCode").Display = false;
                            this.grdPart.MasterTableView.GetColumn("ApprovedQty").Display = false;
                            this.grdPart.MasterTableView.GetColumn("KHO_PCD").Display = false;
                            this.grdPart.MasterTableView.GetColumn("KHOTAITRO").Display = false;
                            this.grdPart.MasterTableView.GetColumn("KHO_DUOC").Display = false;
                        }

                        this.grdPart.Rebind();
                        this.completeNotification.Show("Cập nhật thông tin thành công!");
                    }

                    

                    break;
                case "Delete":
                    if (this.IsDelete.Value == "true")
                    {
                        var deleteItemId = Session["ItemId"].ToString();
                        if (!string.IsNullOrEmpty(deleteItemId))
                        {
                            this.mrService.Delete(new Guid(deleteItemId));
                            Response.Redirect(Request.RawUrl);
                        }
                    }

                    

                    break;
            }
        }

        private string ValidateForm()
        {
            var flag = true;
            var completeMess = string.Empty;;
            var message = "Vui lòng nhập đầy đủ thông tin.<br/> <b>Các thông tin bị thiếu:</b><br/>";

            if (this.ddlStore.SelectedItem == null)
            {
                flag = false;
                message += "_ Kho đơn vị.<br/>";
            }

            if (string.IsNullOrEmpty(this.txtRequestBy.Text))
            {
                flag = false;
                message += "_ Người yêu cầu.<br/>";
            }

            //if (string.IsNullOrEmpty(this.txtSoTYT.Text))
            //{
            //    flag = false;
            //    message += "_ Số TYT.<br/>";
            //}

            if (string.IsNullOrEmpty(this.txtSoCachLyTaiNha.Text))
            {
                flag = false;
                message += "_ Số người phục vụ trong khu cách ly/Ngày. (Điền 0 nếu không có)<br/>";
            }

            if (string.IsNullOrEmpty(this.txtSoCachLyTapTrung.Text))
            {
                flag = false;
                message += "_ Số người cách ly tập trung. (Điền 0 nếu không có)<br/>";
            }

            if (string.IsNullOrEmpty(this.txtSoLayMauXN.Text))
            {
                flag = false;
                message += "_ Số đội lấy mẫu/ngày (mỗi đội 2 người). (Điền 0 nếu không có)<br/>";
            }

            if (!flag)
            {
                return message;
            }
            return completeMess;
        }

        protected void grdPart_OnBatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            var mrId = Session["ItemId"].ToString();
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var userInfor = Session["UserInfor"] as UserInforDto;
            if (!string.IsNullOrEmpty(mrId))
            {
                var mrObj = this.mrService.GetById(new Guid(mrId));
                var partList = Session["AvailablePart"] as List<PartListSYTAvailableDto>;

                foreach (var command in e.Commands)
                {
                    var newValue = command.NewValues;
                    var oldValue = command.OldValues;
                    var obj = new AA_MaterialRequestDetail();
                    var partLibrary = this.partLibraryForRequestService.GetByName(newValue["PartCode"].ToString());
                    if (partLibrary != null)
                    {
                        obj.MaterialRequestID = new Guid(mrId);
                        obj.PartCode = partLibrary.ID.ToString();
                        obj.PartDescription = partLibrary.PartName;
                        obj.EAMPartCode = newValue["EAMPartCode"].ToString().Split('-')[0].Trim();
                        obj.EAMPartDescription = newValue["EAMPartCode"].ToString().Split('-')[1].Trim();
                        var partItem = partList.FirstOrDefault(t => t.MAVATTU == obj.EAMPartCode);
                        if (partItem != null)
                        {
                            obj.PartUMO = partItem.DVT;
                        }
                        
                        obj.CurrentStock = (decimal?)newValue["CurrentStock"];
                        obj.RequestQty = (decimal?)newValue["RequestQty"];

                        obj.ToStoreCode = this.ddlStore.SelectedValue;
                        obj.ToStoreName = this.ddlStore.SelectedItem.Text;

                        if (!string.IsNullOrEmpty(newValue["FromStoreCode"].ToString()))
                        {
                            obj.FromStoreCode = newValue["FromStoreCode"].ToString().Split('-')[0].Trim();
                            obj.FromStoreName = newValue["FromStoreCode"].ToString().Split('-')[1].Trim();
                        }

                        obj.ApprovedQty = !string.IsNullOrEmpty(newValue["ApprovedQty"].ToString())
                            ? (decimal?)newValue["ApprovedQty"]
                            : 0;

                        if (newValue["ID"] != null)
                        {
                            obj.ID = new Guid(newValue["ID"].ToString());
                            var oldObj = this.mrDetailService.GetById(obj.ID);
                            obj.RequestPartName = oldObj.RequestPartName;

                            if (partItem == null)
                            {
                                obj.PartUMO = oldObj.PartUMO;
                            }

                            obj.CreatedBy = oldObj.CreatedBy;
                            obj.CreatedById = oldObj.CreatedById;
                            obj.CreatedDate = oldObj.CreatedDate;
                            obj.UpdatedBy = userInfor.TENNGUOIDUNG;
                            obj.UpdatedById = userInfor.MANGUOIDUNG;
                            obj.UpdatedDate = DateTime.Now;
                            this.mrDetailService.Update(obj);
                        }
                        else
                        {
                            obj.ID = Guid.NewGuid();
                            obj.CreatedBy = userInfor.TENNGUOIDUNG;
                            obj.CreatedById = userInfor.MANGUOIDUNG;
                            obj.CreatedDate = DateTime.Now;
                            this.mrDetailService.Insert(obj);
                        }
                    }
                }

                this.grdPartInStock.Rebind();
            }
        }

        protected void grdPart_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var partList = new List<AA_MaterialRequestDetail>();
            if (Session["ItemId"] != null)
            {
                var mrId = Session["ItemId"].ToString();
                if (!string.IsNullOrEmpty(mrId))
                {
                    partList = this.mrDetailService.GetAllByMR(new Guid(mrId)); //.Where(t => t.CurrentStock != 0 || t.RequestQty != 0).ToList();
                    foreach (var item in partList)
                    {
                        if (!string.IsNullOrEmpty(item.EAMPartCode))
                        {
                            var partInStore = this.GetPartInStore(item.EAMPartCode);
                            if (partInStore != null)
                            {
                                item.KHOTAITRO = partInStore.KHOTAITRO;
                                item.KHO_PCD = partInStore.KHO_PCD;
                                item.KHO_DUOC = partInStore.KHO_DUOC;
                            }
                        }
                    }
                }
            }

            this.grdPart.DataSource = partList;
        }

        protected void grdPart_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                var item = (GridDataItem)e.Item;
                var objId = new Guid(item.GetDataKeyValue("ID").ToString());
                this.mrDetailService.Delete(objId);
                this.grdPart.Rebind();
            }
        }

        protected void btnProcess_OnClick(object sender, EventArgs e)
        {
            if (Session["ItemId"] != null)
            {
                var mrId = Session["ItemId"].ToString();
                var totalmess = string.Empty;
                if (!string.IsNullOrEmpty(mrId))
                {
                    var mrObj = this.mrService.GetById(new Guid(mrId));
                    if (mrObj != null)
                    {
                        try
                        {
                            var partList = this.mrDetailService.GetAllByMR(new Guid(mrId)).Where(t => t.ApprovedQty > 0).ToList();
                            foreach (var partItemGroupByFromStore in partList.GroupBy(t => t.FromStoreCode))
                            {
                                if (!string.IsNullOrEmpty(partItemGroupByFromStore.Key))
                                {
                                    var outputParam = new SqlParameter("@outputVal", SqlDbType.VarChar, 30);
                                    outputParam.Direction = ParameterDirection.Output;
                                    var reqSeq = this.eamService.GetOutputValue("get_seqREQ", new[] { outputParam })[0];

                                    // Create store to store REQ
                                    var reqSeqParam = new SqlParameter("@REQSEQ", SqlDbType.NVarChar, 30);
                                    reqSeqParam.Value = reqSeq;
                                    var fromCodeParam = new SqlParameter("@FROMCODE", SqlDbType.NVarChar, 30);
                                    fromCodeParam.Value = partItemGroupByFromStore.Key;
                                    var toCodeParam = new SqlParameter("@TOCODE", SqlDbType.NVarChar, 30);
                                    toCodeParam.Value = mrObj.StoreCode;
                                    var userParam = new SqlParameter("@USER", SqlDbType.NVarChar, 30);
                                    userParam.Value = Request.QueryString["userid"].ToUpper();
                                    var result1 = this.eamService.ExcuteQuery("INS_REQ", new[] { reqSeqParam, fromCodeParam, toCodeParam, userParam });
                                    // --------------------------------------------------------------------------------

                                    // Create REQ Line
                                    var totalPartLine = 0;
                                    if (string.IsNullOrEmpty(result1))
                                    {
                                        totalmess += "Tạo phiếu yêu cầu vật tư cho <b>'" + mrObj.OrganizationName + "'</b> thành công với Số phiếu '" + reqSeq + "'. </br>";
                                        foreach (var itemLine in partItemGroupByFromStore)
                                        {
                                            if (!string.IsNullOrEmpty(itemLine.EAMPartCode))
                                            {
                                                totalPartLine += 1;
                                                var reqParam = new SqlParameter("@REQ", SqlDbType.NVarChar, 30);
                                                reqParam.Value = reqSeq;
                                                var partNumberParam = new SqlParameter("@PART", SqlDbType.NVarChar, 30);
                                                partNumberParam.Value = itemLine.EAMPartCode;
                                                var qtyParam = new SqlParameter("@QTY", SqlDbType.Decimal);
                                                qtyParam.Value = itemLine.ApprovedQty;

                                                var result2 = this.eamService.ExcuteQuery("INS_REQLINES", new[] { reqParam, partNumberParam, qtyParam });
                                                if (string.IsNullOrEmpty(result2))
                                                {
                                                    totalmess += "---Tạo yêu cầu vật tư <b>'" + itemLine.EAMPartCode + "-" + itemLine.PartDescription + "'</b> cho số phiếu '" + reqSeq + "' thành công. </br>";
                                                }
                                                else
                                                {
                                                    totalmess += result2 + "</br>";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalmess += result1 + "</br>";
                                    }
                                    // --------------------------------------------------------------------------------

                                    // Approve REQ
                                    if (totalPartLine > 0)
                                    {
                                        var approveReqParam = new SqlParameter("@REQ", SqlDbType.NVarChar, 30);
                                        approveReqParam.Value = reqSeq;
                                        var approveUserParam = new SqlParameter("@USER", SqlDbType.NVarChar, 30);
                                        approveUserParam.Value = Request.QueryString["userid"].ToUpper();
                                        var result3 = this.eamService.ExcuteQuery("A_REQ", new[] { approveReqParam, approveUserParam });
                                        if (string.IsNullOrEmpty(result3))
                                        {
                                            totalmess += "Duyệt phiếu yêu cầu vật tư số '" + reqSeq + "' thành công. </br>";
                                        }
                                        else
                                        {
                                            totalmess += result3 + "</br>";
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    totalmess +=
                                        "Error: Yêu cầu vật tư thiếu thông tin Nguồn Cấp, Vui lòng kiểm tra lại.";
                                }
                                
                                // ----------------------------------------------------------------
                            }
                        }
                        catch (Exception ex)
                        {
                            totalmess += "Error: " + ex.Message + "</br></br>";
                        }
                        
                        mrObj.IsGenEAMMR = true;
                        this.mrService.Update(mrObj);

                        if (totalmess.Contains("Error"))
                        {
                            this.errorNotification.Show(totalmess);
                        }
                        else
                        {
                            this.completeNotification.Show(totalmess);
                        }

                        this.btnProcess.Visible = false;
                        this.cbIsGenMR.Visible = true;
                    }
                }
            }
        }

        protected void ExportToolBar_OnButtonClick(object sender, RadToolBarEventArgs e)
        {
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var userInfor = Session["UserInfor"] as UserInforDto;
            switch (e.Item.Value)
            {
                case "ExportExcel":
                    var workbook = new Workbook();
                    var filePath = Server.MapPath(@"~\Resources\DataTemplate") + @"\";
                    workbook.Open(filePath + "SummaryMRTemplate.xlsx");
                    var wsData = workbook.Worksheets[0];

                    var mrWaitingList = this.mrService.GetAllMRWaitingApprove().OrderBy(t => t.OrganizationOrder).ToList();



                    var totalPartRequestList = new List<AA_MaterialRequestDetail>();
                    
                    var count = 1;
                    foreach (var mrItem in mrWaitingList)
                    {
                        totalPartRequestList.AddRange(this.mrDetailService.GetAllByMR(mrItem.ID));

                        wsData.Cells[count + 4,1].PutValue(count);
                        wsData.Cells[count + 4, 2].PutValue(mrItem.OrganizationName);
                        wsData.Cells[count + 4, 3].PutValue(mrItem.SoTYT);
                        wsData.Cells[count + 4, 4].PutValue(mrItem.SoNguoiCachLyTaiNha);
                        wsData.Cells[count + 4, 5].PutValue(mrItem.SoNguoiCachLyTapTrung);
                        wsData.Cells[count + 4, 6].PutValue(mrItem.SoLayMauXN);

                        count += 1;
                    }

                    //var totalPartList = totalPartRequestList.Select(t => t.PartDescription + "\n" + "(" +t.PartUMO + ")").Distinct().OrderBy(t => t).ToList();
                    
                    var totalPartList = totalPartRequestList.Select(t => partLibraryForRequestService.GetById(new Guid(t.PartCode))).Distinct().OrderBy(t => t.OrderNumber).ToList();

                    for (int i = 0; i < totalPartList.Count; i++)
                    {
                        wsData.Cells[3, 7 + (i * 4)].PutValue(totalPartList[i].PartName + "\n(" + totalPartList[i].UMO + ")");
                        wsData.Cells[4, 7 + (i * 4)].PutValue("Hiện tồn");
                        wsData.Cells[4, 8 + (i * 4)].PutValue("Dự trù");
                        wsData.Cells[4, 9 + (i * 4)].PutValue("Cơ số");
                        wsData.Cells[4, 10 + (i * 4)].PutValue("Duyệt cấp");

                        wsData.Cells[5, 9 + (i * 4)].Formula = totalPartList[i].Formula;

                    }

                    count = 1;
                    foreach (var mrItem in mrWaitingList)
                    {
                        for (int i = 0; i < totalPartList.Count; i++)
                        {
                            var mrDetail = totalPartRequestList.FirstOrDefault(t => t.MaterialRequestID == mrItem.ID && t.PartCode == totalPartList[i].ID.ToString());
                            if (mrDetail != null)
                            {
                                wsData.Cells[count + 4, 7 + (i * 4)].PutValue(mrDetail.CurrentStock);
                                wsData.Cells[count + 4, 8 + (i * 4)].PutValue(mrDetail.RequestQty);
                                wsData.Cells[count + 4, 10 + (i * 4)].PutValue(mrDetail.ApprovedQty);
                            }
                        }

                        count += 1;
                    }

                    var saveFileName = "Tong hop yeu cau cap vat tu_" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".xlsx";
                    var savePath = Server.MapPath(@"~\Resources\ExportData") + @"\";
                    workbook.Save(savePath + saveFileName);

                    HttpContext.Current.Response.Redirect("../../DownLoadFile.ashx?file=" + savePath + saveFileName);
                    //this.DownloadByWriteByte(savePath + saveFileName, saveFileName, false);
                    break;
                case "ExportExcel2":
                    if (Session["ItemId"] != null)
                    {
                        var obj = this.mrService.GetById(new Guid(Session["ItemId"].ToString()));
                        if (obj != null)
                        {
                            if (!obj.IsMonthlyRequest.GetValueOrDefault())
                            {
                                var workbook2 = new Workbook();
                                var filePath2 = Server.MapPath(@"~\Resources\DataTemplate") + @"\";
                                workbook2.Open(filePath2 + "ReqReport.xlsx");
                                var wsData2 = workbook2.Worksheets[0];

                                wsData2.Cells["D2"].PutValue("TP. Hồ Chí Minh, ngày " + DateTime.Now.Day + " tháng "+DateTime.Now.Month+" năm " + DateTime.Now.Year);

                                wsData2.Cells["B5"].PutValue("Căn cứ công văn số:……………………………, về việc............................................................, ngày………tháng…… năm ……… của " + obj.OrganizationName);
                                wsData2.Cells["B6"].PutValue("Khoa PCBTN đề nghị cấp hóa chất vật tư cho "+ obj.OrganizationName +" với số lượng như sau:");
                                var dt = new DataTable();
                                var listColumn = new DataColumn[]
                                {
                                    new DataColumn("STT", Type.GetType("System.String")),
                                    new DataColumn("TenVatTu", Type.GetType("System.String")),
                                    new DataColumn("DonVi", Type.GetType("System.String")),
                                    new DataColumn("SoLuong", Type.GetType("System.Decimal")),
                                    new DataColumn("GhiChu", Type.GetType("System.String")),
                                };
                                dt.Columns.AddRange(listColumn);

                                var requestPartList = this.mrDetailService.GetAllByMR(obj.ID).Where(t => t.ApprovedQty > 0).ToList();
                                var count2 = 0;
                                foreach (var item in requestPartList)
                                {
                                    var dataItem = dt.NewRow();
                                    count2 += 1;
                                    dataItem["STT"] = count2;
                                    dataItem["TenVatTu"] = item.PartDescription;
                                    dataItem["DonVi"] = item.PartUMO;
                                    dataItem["SoLuong"] = item.ApprovedQty != null ? item.ApprovedQty.Value : 0;
                                    dataItem["GhiChu"] = string.Empty;

                                    dt.Rows.Add(dataItem);
                                }

                                wsData2.Cells.ImportDataTable(dt, false, 8, 1, dt.Rows.Count, 5, true);

                                var saveFileName2 = "Trinh ky yeu cau dot xuat_" + obj.OrganizationCode + "_" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".xlsx";
                                var savePath2 = Server.MapPath(@"~\Resources\ExportData") + @"\";
                                workbook2.Save(savePath2 + saveFileName2);

                                HttpContext.Current.Response.Redirect("../../DownLoadFile.ashx?file=" + savePath2 + saveFileName2);
                            }
                            else
                            {
                                this.errorNotification.Show("Vui lòng chọn một phiếu yêu cầu đột xuất để Tải file trình ký.");
                            }
                        }
                        else
                        {
                            this.errorNotification.Show("Vui lòng chọn một phiếu yêu cầu đột xuất để Tải file trình ký.");
                        }
                    }
                    else
                    {
                        this.errorNotification.Show("Vui lòng chọn một phiếu yêu cầu đột xuất để Tải file trình ký.");
                    }
                    


                    break;
            }
        }

        protected void grdPartInStock_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var partList = new List<AA_MaterialRequestDetail>();
            //var partInStoreList = new List<PartInStoreDto>();
            //if (Session["ItemId"] != null)
            //{
            //    var mrId = Session["ItemId"].ToString();
            //    if (!string.IsNullOrEmpty(mrId))
            //    {
            //        partList = this.mrDetailService.GetAllByMR(new Guid(mrId));
            //        foreach (var item in partList)
            //        {
            //            if (!string.IsNullOrEmpty(item.PartCode))
            //            {
            //                var partInStore = this.GetPartInStore(item.PartCode);
            //                if (partInStore != null)
            //                {
            //                    partInStoreList.Add(partInStore);
            //                }
            //            }
            //        }
            //    }
            //}

            //this.grdPartInStock.DataSource = partInStoreList;
        }
        
        private PartInStoreDto GetPartInStore(string partNumber)
        {
            DataSet ds;
            PartInStoreDto partInStore = null;
            var partParam = new SqlParameter("@PART", SqlDbType.NVarChar, 30);
            partParam.Value = partNumber;
            ds = this.eamService.GetDataSet("getStock", new[] { partParam });
            if (ds != null)
            {
                var partInStoreList = this.eamService.CreateListFromTable<PartInStoreDto>(ds.Tables[0]);
                partInStore = partInStoreList[0];
            }

            return partInStore;
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var docList = new List<AA_MaterialRequestAttachFile>();
            if (Session["ItemId"] != null && !string.IsNullOrEmpty(Session["ItemId"].ToString()))
            {
                var mrId = new Guid(Session["ItemId"].ToString());
                docList = this.mrAttachFileService.GetByMR(mrId);
            }
            else
            {
                if (Session["DocList"] != null)
                {
                    docList = (List<AA_MaterialRequestAttachFile>)Session["DocList"];
                }
            }

            this.grdAttachFile.DataSource = docList;
        }

        protected void cbMonthly_OnCheckedChanged(object sender, EventArgs e)
        {
            this.lblDuTru.Text = cbMonthly.Checked ? "Dự trù cho tháng:" : "Dự trù cho sử dụng từ ngày - đến ngày:";
            //this.divTitleCV.Visible = !this.cbMonthly.Checked;
            //this.divDetailCV1.Visible = !this.cbMonthly.Checked;
            //this.divDetailCV2.Visible = !this.cbMonthly.Checked;
        }

        protected void cbRequestTypeDinhKy_OnCheckedChanged(object sender, EventArgs e)
        {
            Session.Remove("ItemId");
            var userid = Request.QueryString["userId"].ToUpper();
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var userInfor = Utility.GetUserInfor(userid);
            if (userInfor != null)
            {
                Session.Add("UserInfor", userInfor);
                var isDinhKy = this.ddlRequestType.SelectedValue == "1";

                this.LoadMaterialRequest(userInfor, isDinhKy);
            }

        }

        protected void ddlRequestType_OnSelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            Session.Remove("ItemId");
            var userid = Request.QueryString["userId"].ToUpper();
            var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
            var userInfor = Utility.GetUserInfor(userid);
            if (userInfor != null)
            {
                Session.Add("UserInfor", userInfor);
                var isDinhKy = this.ddlRequestType.SelectedValue == "1";

                this.LoadMaterialRequest(userInfor, isDinhKy);
            }
        }
    }
}