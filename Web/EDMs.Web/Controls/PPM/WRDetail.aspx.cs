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
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Security;
using EDMs.Data.Dto;
using EDMs.Data.Dto.PPM;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.PPM
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class WRDetail : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        /// 
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        private readonly EAMWorkRequestAttachFileService attachFileService = new EAMWorkRequestAttachFileService();
        private readonly EAMWorkRequestService wrService = new EAMWorkRequestService();

        private readonly UserService userService = new UserService();

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
                Session.Remove("ItemId");
                Session.Remove("DocList");
                this.LoadComboData();

                //this.GetObjectDataList();

                var objectList = new List<EAMWorkRequest>();
                if (UserSession.Current.User.RoleId == 1)
                {
                    objectList = this.wrService.GetAll();
                }
                else if (UserSession.Current.User.IsLeader.GetValueOrDefault())
                {
                    objectList = this.wrService.GetAll().Where(t =>
                        t.CreatedById == UserSession.Current.User.Id.ToString()
                        || this.userService.GetByID(Convert.ToInt32(t.CreatedById)).ManagerIds.Split('$').Contains(UserSession.Current.User.Id.ToString())).ToList();
                }
                else
                {
                    objectList = this.wrService.GetAll().Where(t => t.CreatedById == UserSession.Current.User.Id.ToString()).ToList();
                }

                this.BindObjectData(objectList);
                //this.lbObject.SelectedIndex = 0;
            }
        }

        private void LoadComboData()
        {
            // Load organization
            var organizationData = this.eamService.GetDataTable("SELECT ORG_CODE, ORG_DESC, ORG_CODE + ' - ' + ORG_DESC as FullName FROM R5ORGANIZATION where ORG_CODE in ('YN' , 'KN' , 'SVC' , 'NT' , 'TH' , 'AS');");
            this.ddlOrganization.DataSource = organizationData;
            this.ddlOrganization.DataTextField = "FullName";
            this.ddlOrganization.DataValueField = "ORG_CODE";
            this.ddlOrganization.DataBind();
            // --------------------------------------------------------------

            // Load equipment list
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var assetList = new List<AssetDto>();
            ds = this.eamService.GetDataSet("get_asset_r2", new[] { userParam });
            if (ds != null)
            {
                assetList = this.eamService.CreateListFromTable<AssetDto>(ds.Tables[0]);
                foreach (var item in assetList)
                {
                    item.FullName = item.ThietBi + " - " + item.TenThietBi;
                }
            }

            Session.Add("AssetList", assetList);

            this.ddlEquipment.DataSource = assetList;
            this.ddlEquipment.DataTextField = "FullName";
            this.ddlEquipment.DataValueField = "ThietBi";
            this.ddlEquipment.DataBind();
            // -----------------------------------------------------------------

            // Load Department
            var departmentData = this.eamService.GetDataTable("SELECT MRC_CODE, MRC_DESC, MRC_CODE + ' - ' + MRC_DESC as FullName FROM R5MRCS;");
            this.ddlDepartment.DataSource = departmentData;
            this.ddlDepartment.DataTextField = "FullName";
            this.ddlDepartment.DataValueField = "MRC_CODE";
            this.ddlDepartment.DataBind();
            // --------------------------------------------------------------
            
            // Load problem code
            var problemData = this.eamService.GetDataTable(@"Select '' as closingcode, '' as FullName
                            UNION
                            SELECT rqm_code closingcode, 
                                   rqm_code + ' - ' + dbo.r5o7_o7get_desc('VN', rqm_type, rqm_code, NULL, NULL) FullName
                            FROM
                            (
                                SELECT rqm_code rqm_code, 
                                       'RECO' rqm_type
                                FROM r5requircodes
                                WHERE COALESCE(rqm_notused, '-') <> '+'
                                UNION
                                SELECT cau_code rqm_code, 
                                       'CAUS' rqm_type
                                FROM r5causes
                                WHERE COALESCE(cau_notused, '-') <> '+'
                                UNION
                                SELECT fal_code rqm_code, 
                                       'FAIL' rqm_type
                                FROM r5failures
                                WHERE COALESCE(fal_notused, '-') <> '+'
                                UNION
                                SELECT acc_code rqm_code, 
                                       'ACCO' rqm_type
                                FROM r5actioncodes
                                WHERE COALESCE(acc_notused, '-') <> '+'
                            ) r;");
            this.ddlMaVanDe.DataSource = problemData;
            this.ddlMaVanDe.DataTextField = "FullName";
            this.ddlMaVanDe.DataValueField = "closingcode";
            this.ddlMaVanDe.DataBind();
            // --------------------------------------------------------------
        }

        private void GetObjectDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var objectList = new List<WODto>();
            ds = this.eamService.GetDataSet("get_wo_list", new[] { userParam });
            if (ds != null)
            {
                objectList = this.eamService.CreateListFromTable<WODto>(ds.Tables[0]);
                foreach (var item in objectList)
                {
                    item.FullName = item.PhieuCongViec + " - " + item.TenCongViec;
                }
            }

            Session.Add("ObjectList", objectList);
        }

        private void BindObjectData(List<EAMWorkRequest> objectList)
        {
            this.lbObject.Items.Clear();
            foreach (var item in objectList)
            {
                var lbItem = new RadListBoxItem(Utilities.Utility.ReturnSequenceString(item.ID, 5) + " - " + item.RequestName, item.ID.ToString());
                lbItem.Attributes.Add("DoUuTien", item.PriorityName);
                lbItem.Attributes.Add("TrangThaiCongViec", item.StatusName);
                lbItem.Attributes.Add("DonVi", item.OrganizationName);
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
            this.EnableControl(true);
            this.ViewToolBar.Items[5].Enabled = true;
            this.ViewToolBar.Items[6].Enabled = true;

            var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
            lblObjectName.Text = this.lbObject.SelectedItem.Text;
            var wrItem = this.wrService.GetById(Convert.ToInt32(lbObject.SelectedValue));
            Session.Add("ItemId", lbObject.SelectedValue);

            this.FillObjectDetail(wrItem);

            if (wrItem.StatusId == "1")
            {
                this.EnableControl(true);
                this.ViewToolBar.Items[5].Enabled = true;
                this.ViewToolBar.Items[6].Enabled = true;

                //if (!UserSession.Current.User.IsLeader.GetValueOrDefault())
                //{
                //    this.ddlTinhTrang.Items.Remove(1);
                //    this.ddlTinhTrang.Items.Remove(1);
                //}
            }
            else
            {
                if (UserSession.Current.User.IsLeader.GetValueOrDefault())
                {
                    this.EnableControl(true);
                    this.ViewToolBar.Items[5].Enabled = true;
                    this.ViewToolBar.Items[6].Enabled = true;
                }
                else
                {
                    this.EnableControl(false);
                    this.ViewToolBar.Items[5].Enabled = false;
                    this.ViewToolBar.Items[6].Enabled = false;
                }
            }

            this.grdAttachFile.Rebind();
        }

        private void LoadObjectDocumentTree(WODto obj)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var objectParam = new SqlParameter("@phieucv", SqlDbType.NVarChar);
            objectParam.Value = obj.PhieuCongViec;
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var documentList = new List<NormMaterialDocDto>();
            ds = this.eamService.GetDataSet("get_doc_WO", new[] { userParam, objectParam });
            if (ds != null)
            {
                documentList = this.eamService.CreateListFromTable<NormMaterialDocDto>(ds.Tables[0]);
                var mainNode = new RadTreeNode("<b>Phiếu công việc: </b>" + obj.FullName, obj.PhieuCongViec);
                mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in documentList)
                {
                    var childNode = new RadTreeNode(item.MaTaiLieu + ": " + item.TenTaiLieu, item.DuongDan);
                    childNode.ImageUrl = @"~/Images/documents.png";

                    mainNode.Nodes.Add(childNode);
                    mainNode.Expanded = true;
                }


            }
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var objectList = new List<EAMWorkRequest>();
            if (UserSession.Current.User.RoleId == 1)
            {
                objectList = this.wrService.GetAll();
            }
            else if (UserSession.Current.User.IsLeader.GetValueOrDefault())
            {
                objectList = this.wrService.GetAll().Where(t =>
                    t.CreatedById == UserSession.Current.User.Id.ToString()
                    || this.userService.GetByID(Convert.ToInt32(t.CreatedById)).ManagerIds.Split('$').Contains(UserSession.Current.User.Id.ToString())).ToList();
            }
            else
            {
                objectList = this.wrService.GetAll().Where(t => t.CreatedById == UserSession.Current.User.Id.ToString()).ToList();
            }

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();

                var filterObjectList = objectList.Where(t => t.ID.ToString().ToUpper().Contains(searchText)
                                                        || (t.OrganizationName != null && t.OrganizationName.ToUpper().Contains(searchText))
                                                        || (t.RequestName != null && t.RequestName.ToUpper().Contains(searchText))
                                                        || (t.EquipmentName != null && t.EquipmentName.ToUpper().Contains(searchText))
                                                        || (t.DepartmentName != null && t.DepartmentName.ToUpper().Contains(searchText))
                                                        || (t.CreatedByName != null && t.CreatedByName.ToUpper().Contains(searchText))
                                                        || (t.RequestTypeName != null && t.RequestTypeName.ToUpper().Contains(searchText))
                                                        || (t.CategoryName != null && t.CategoryName.ToUpper().Contains(searchText))
                                                        || (t.StatusName != null && t.StatusName.ToUpper().Contains(searchText))
                                                        || (t.PriorityName != null && t.PriorityName.ToUpper().Contains(searchText))
                                                        || (t.ProblemCode != null && t.ProblemCode.ToUpper().Contains(searchText))
                                                        || (t.AssignTo != null && t.AssignTo.ToUpper().Contains(searchText))
                                                        || (t.Description != null && t.Description.ToUpper().Contains(searchText))
                                                        || (t.Note != null && t.Note.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindObjectData(filterObjectList);
            }
            else
            {
                this.BindObjectData(objectList);
            }
        }

        private void FillObjectDetail(EAMWorkRequest wrItem)
        {
            this.txtPhieuCV.Text = Utilities.Utility.ReturnSequenceString(wrItem.ID, 5);
            this.txtDienGiai.Text = wrItem.RequestName;
            this.ddlOrganization.SelectedValue = wrItem.OrganizationId;
            this.ddlEquipment.SelectedValue = wrItem.EquipmentId;
            this.ddlDepartment.SelectedValue = wrItem.DepartmentId;

            this.txtNguoiLap.Text = wrItem.CreatedByName;
            this.txtNgayLap.Text = wrItem.CreatedDate.GetValueOrDefault().ToString("dd-MMM-yy");
            this.txtNgayBaoCao.SelectedDate = wrItem.ReportDate;
            
            this.ddlKieu.SelectedValue = wrItem.RequestTypeId;

            this.txtNgayBatDauKH.SelectedDate = wrItem.StartDate;
            this.txtPhanLoai.Text = wrItem.CategoryName;

            this.ddlMaVanDe.SelectedValue = wrItem.ProblemCode;
            this.ddlTinhTrang.SelectedValue = wrItem.StatusId;
            this.txtPhanCongDen.Text = wrItem.AssignTo;
            this.ddlDoUuTien.SelectedValue = wrItem.PriorityId;

            
            this.txtDuKienHoanThanh.Text = wrItem.PlanComplete;
            this.txtChiTiet.Text = wrItem.Description;
            this.txtChiDao.Text = wrItem.Note;
        }


        protected void ViewToolBar_OnButtonClick(object sender, RadToolBarEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "Insert":
                    Session.Add("ItemId", "");
                    this.EnableControl(true);
                    this.InitControlToCreateNew();
                    break;
                case "Update":
                    var validateMess = this.ValidateForm();
                    if (!string.IsNullOrEmpty(validateMess))
                    {
                        this.errorNotification.Show(validateMess);
                    }
                    else
                    {
                        int? itemId;
                        var wrItem = new EAMWorkRequest();
                        if (Session["ItemId"] == null)
                        {
                            this.CollectData(wrItem);

                            itemId = this.wrService.Insert(wrItem);
                            if (itemId != null)
                            {
                                wrItem = this.wrService.GetById(itemId.Value);
                                var lblObjectName = this.ToolBarObject.Items[0].FindControl("lblObjectName") as Label;
                                lblObjectName.Text = Utilities.Utility.ReturnSequenceString(itemId.Value, 5) + " - " +
                                                     wrItem.RequestName;

                                // Insert attach file
                                var docList = (List<EAMWorkRequestAttachFile>) Session["DocList"];
                                if (docList != null)
                                {
                                    foreach (var attachFile in docList)
                                    {
                                        attachFile.DocumentId = wrItem.ID;

                                        this.attachFileService.Insert(attachFile);
                                    }
                                }

                                // -----------------------------------------------------------------------
                            }
                        }
                        else
                        {
                            itemId = Convert.ToInt32(Session["ItemId"].ToString());
                            wrItem = this.wrService.GetById(itemId.Value);
                            if (wrItem != null)
                            {
                                this.CollectData(wrItem);
                                this.wrService.Update(wrItem);

                                // Insert attach file
                                var docList = (List<EAMWorkRequestAttachFile>) Session["DocList"];
                                if (docList != null)
                                {
                                    foreach (var attachFile in docList)
                                    {
                                        attachFile.DocumentId = wrItem.ID;

                                        this.attachFileService.Insert(attachFile);
                                    }
                                }
                                
                                // -----------------------------------------------------------------------
                            }
                        }

                        var objectList = new List<EAMWorkRequest>();
                        if (UserSession.Current.User.RoleId == 1)
                        {
                            objectList = this.wrService.GetAll();
                        }
                        else if (UserSession.Current.User.IsLeader.GetValueOrDefault())
                        {
                            objectList = this.wrService.GetAll().Where(t =>
                                t.CreatedById == UserSession.Current.User.Id.ToString()
                                || this.userService.GetByID(Convert.ToInt32(t.CreatedById)).ManagerIds.Split('$').Contains(UserSession.Current.User.Id.ToString())).ToList();
                        }
                        else
                        {
                            objectList = this.wrService.GetAll().Where(t => t.CreatedById == UserSession.Current.User.Id.ToString()).ToList();
                        }

                        this.BindObjectData(objectList);
                        if (itemId != null)
                        {
                            this.lbObject.SelectedValue = itemId.Value.ToString();
                        }
                        this.completeNotification.Show("Cập nhật thông tin thành công!");
                    }

                    break;
                case "Delete":
                    if (this.IsDelete.Value == "true")
                    {
                        var deleteItemId = Session["ItemId"].ToString();
                        if (!string.IsNullOrEmpty(deleteItemId))
                        {
                            this.wrService.Delete(Convert.ToInt32(deleteItemId));
                            Response.Redirect(Request.RawUrl);
                        }
                    }
                    break;
            }
        }

        private void CollectData(EAMWorkRequest wrItem)
        {
            wrItem.RequestName = this.txtDienGiai.Text.Trim();
            wrItem.OrganizationId = this.ddlOrganization.SelectedItem != null ? this.ddlOrganization.SelectedValue : string.Empty;
            wrItem.OrganizationName = this.ddlOrganization.SelectedItem != null ? this.ddlOrganization.SelectedItem.Text : string.Empty;

            wrItem.EquipmentId = this.ddlEquipment.SelectedItem != null ? this.ddlEquipment.SelectedValue : string.Empty;
            wrItem.EquipmentName = this.ddlEquipment.SelectedItem != null ? this.ddlEquipment.SelectedItem.Text : string.Empty;

            wrItem.DepartmentId = this.ddlDepartment.SelectedItem != null ? this.ddlDepartment.SelectedValue : string.Empty;
            wrItem.DepartmentName = this.ddlDepartment.SelectedItem != null ? this.ddlDepartment.SelectedItem.Text : string.Empty;

            wrItem.CreatedDate = DateTime.Now;
            wrItem.CreatedById = UserSession.Current.User.Id.ToString();
            wrItem.CreatedByName = UserSession.Current.User.FullName;
            wrItem.ReportDate = this.txtNgayBaoCao.SelectedDate;

            wrItem.RequestTypeId = this.ddlKieu.SelectedItem != null ? this.ddlKieu.SelectedValue : string.Empty;
            wrItem.RequestTypeName = this.ddlKieu.SelectedItem != null ? this.ddlKieu.SelectedItem.Text : string.Empty;

            wrItem.StartDate = this.txtNgayBatDauKH.SelectedDate;
            wrItem.CategoryName = this.txtPhanLoai.Text.Trim();

            wrItem.ProblemCode = this.ddlMaVanDe.SelectedItem != null ? this.ddlMaVanDe.SelectedValue : string.Empty;

            wrItem.StatusId = this.ddlTinhTrang.SelectedItem != null ? this.ddlTinhTrang.SelectedValue : string.Empty;
            wrItem.StatusName = this.ddlTinhTrang.SelectedItem != null ? this.ddlTinhTrang.SelectedItem.Text : string.Empty;

            wrItem.AssignTo = this.txtPhanCongDen.Text.Trim();

            wrItem.PriorityId = this.ddlDoUuTien.SelectedItem != null ? this.ddlDoUuTien.SelectedValue : string.Empty;
            wrItem.PriorityName = this.ddlDoUuTien.SelectedItem != null ? this.ddlDoUuTien.SelectedItem.Text : string.Empty;

            wrItem.PlanComplete = this.txtDuKienHoanThanh.Text.Trim();
            wrItem.Description = this.txtChiTiet.Text.Trim();
            wrItem.Note = this.txtChiDao.Text.Trim();

            wrItem.ManagerId = UserSession.Current.User.ManagerId;
            wrItem.ManagerName = UserSession.Current.User.ManagerName;
        }

        private void EnableControl(bool flag)
        {
            this.txtPhieuCV.Enabled = flag;
            this.txtDienGiai.Enabled = flag;
            this.txtNguoiLap.Enabled = flag;
            this.txtNgayLap.Enabled = flag;
            this.txtNgayBaoCao.Enabled = flag;
            this.txtNgayBatDauKH.Enabled = flag;
            this.txtPhanCongDen.Enabled = flag;
            this.txtPhanLoai.Enabled = flag;
            this.txtDuKienHoanThanh.Enabled = flag;

            this.txtChiDao.Enabled = flag;
            this.txtChiTiet.Enabled = flag;

            this.ddlDepartment.Enabled = flag;
            this.ddlEquipment.Enabled = flag;
            this.ddlKieu.Enabled = flag;
            this.ddlMaVanDe.Enabled = flag;
            this.ddlOrganization.Enabled = flag;
            this.ddlTinhTrang.Enabled = flag;
            this.ddlDoUuTien.Enabled = flag;
            this.radUpload.Enabled = flag;
        }

        private void InitControlToCreateNew()
        {
            // Enable Save button
            this.ViewToolBar.Items[5].Enabled = true;
            this.ViewToolBar.Items[6].Enabled = false;

            Session.Remove("ItemId");
            Session.Remove("DocList");

            this.txtPhieuCV.Text = string.Empty;
            this.txtDienGiai.Text = string.Empty;
            this.txtNguoiLap.Text = UserSession.Current.User.FullName;
            this.txtNgayLap.Text = DateTime.Now.ToString("dd-MMM-yy");
            this.txtNgayBaoCao.SelectedDate = DateTime.Now;

            this.txtPhanCongDen.Text = string.Empty;
            this.txtPhanLoai.Text = string.Empty;
            this.txtDuKienHoanThanh.Text = string.Empty;
            this.txtNgayBatDauKH.SelectedDate = null;
            this.txtChiDao.Text = string.Empty;
            this.txtChiTiet.Text = string.Empty;

            this.ddlDepartment.ClearSelection();
            this.ddlEquipment.ClearSelection();
            this.ddlKieu.SelectedValue = "4";
            this.ddlMaVanDe.ClearSelection();
            this.ddlOrganization.ClearSelection();
            this.ddlTinhTrang.SelectedValue = "1";

            if (!UserSession.Current.User.IsLeader.GetValueOrDefault())
            {
                this.ddlTinhTrang.Items.Remove(1);
                this.ddlTinhTrang.Items.Remove(1);
            }

            this.ddlDoUuTien.SelectedValue = "2";
            this.lbObject.ClearSelection();
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            if (Session["ItemId"] != null && !string.IsNullOrEmpty(Session["ItemId"].ToString()))
            {
                this.attachFileService.Delete(docId);
            }
            else
            {
                if (Session["DocList"] != null)
                {
                    var docList = (List<EAMWorkRequestAttachFile>)Session["DocList"];
                    var doc = docList.FirstOrDefault(t => t.ID == docId);
                    docList.Remove(doc);

                    Session.Add("DocList", docList);
                }
            }

            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var docList = new List<EAMWorkRequestAttachFile>();
            if (Session["ItemId"] != null && !string.IsNullOrEmpty(Session["ItemId"].ToString()))
            {
                var wrId = Convert.ToInt32(Session["ItemId"].ToString());
                docList = this.attachFileService.GetAllByDocId(wrId);
            }
            else
            {
                if (Session["DocList"] != null)
                {
                    docList = (List<EAMWorkRequestAttachFile>)Session["DocList"];
                }
            }

            this.grdAttachFile.DataSource = docList;

        }

        private string ValidateForm()
        {
            var flag = true;
            var completeMess = string.Empty; ;
            var message = "Vui lòng nhập đầy đủ thông tin.<br/> <b>Các thông tin bị thiếu:</b><br/>";

            if (string.IsNullOrEmpty(this.txtDienGiai.Text))
            {
                flag = false;
                message += "_ Tên yêu cầu.<br/>";
            }

            if (this.ddlEquipment.SelectedItem == null)
            {
                flag = false;
                message += "_ Thiết bị.<br/>";
            }

            if (this.ddlDepartment.SelectedItem == null)
            {
                flag = false;
                message += "_ Bộ phận.<br/>";
            }

            if (this.txtNgayBaoCao.SelectedDate == null)
            {
                flag = false;
                message += "_ Ngày báo cáo.<br/>";
            }

            if (this.ddlKieu.SelectedItem == null)
            {
                flag = false;
                message += "_ Kiểu.<br/>";
            }
            
            if (this.ddlTinhTrang.SelectedItem == null)
            {
                flag = false;
                message += "_ Tình trạng.<br/>";
            }

            if (Session["ItemId"] != null && !string.IsNullOrEmpty(Session["ItemId"].ToString()))
            {
                var wrId = Convert.ToInt32(Session["ItemId"].ToString());
                // Insert attach file when edit
                var targetFolder = "/DocumentLibrary/YCCV";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/YCCV";

                foreach (UploadedFile file in this.radUpload.UploadedFiles)
                {
                    var docFileName = file.FileName;
                    var serverDocFileName = DateTime.Now.ToBinary() + "_" + docFileName;
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);
                    var serverFilePath = serverFolder + "/" + serverDocFileName;

                    file.SaveAs(saveFilePath, true);

                    var attachFile = new EAMWorkRequestAttachFile()
                    {
                        DocumentId = wrId,
                        Filename = file.GetName(),
                        FilePath = serverFilePath,
                        FileSize = (double?) (file.ContentLength / 1024),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now
                    };

                    this.attachFileService.Insert(attachFile);
                }

                this.grdAttachFile.Rebind();
            }
            else
            {
                var targetFolder = "/DocumentLibrary/YCCV";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/YCCV";
                var docList = new List<EAMWorkRequestAttachFile>();

                if (Session["DocList"] != null)
                {
                    docList = (List<EAMWorkRequestAttachFile>)Session["DocList"];
                }

                foreach (UploadedFile file in this.radUpload.UploadedFiles)
                {
                    var docFileName = file.FileName;
                    var serverDocFileName = DateTime.Now.ToBinary() + "_" + docFileName;
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);
                    var serverFilePath = serverFolder + "/" + serverDocFileName;

                    file.SaveAs(saveFilePath, true);


                    var attachFile = new EAMWorkRequestAttachFile()
                    {
                        Filename = file.GetName(),
                        FilePath = serverFilePath,
                        FileSize = (double?)(file.ContentLength / 1024),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now
                    };

                    docList.Add(attachFile);
                }

                Session.Add("DocList", docList);
                this.grdAttachFile.Rebind();
            }




            if (string.IsNullOrEmpty(this.txtChiTiet.Text))
            {
                flag = false;
                message += "_ Chi tiết sự cố.<br/>";
            }

            if (!flag)
            {
                return message;
            }
            return completeMess;
        }

        protected void ddlEquipment_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (Session["AssetList"] != null)
            {
                var assetList = Session["AssetList"] as List<AssetDto>;
                var assetItem = assetList.FirstOrDefault(t => t.ThietBi == this.ddlEquipment.SelectedValue);

                if (assetItem != null)
                {
                    this.ddlDepartment.SelectedValue = assetItem.PhongBan;
                }
            }
        }
    }
}

