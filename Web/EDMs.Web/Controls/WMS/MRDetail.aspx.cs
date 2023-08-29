// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Image = System.Web.UI.WebControls.Image;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MRDetail : Page
    {
        private readonly MRTypeService mrTypeService;

        private readonly PriorityLevelService priorityLevelService;

        private readonly RoleService roleService;

        private readonly MaterialRequisitionService mrService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService;

        private readonly MaterialRequisitionDetailService mrDetailService;

        private readonly MaterialRequisitionCheckListDefineService mrCheckListDefineService;

        private readonly MaterialRequisitionCheckListDetailService mrCheckListDetailService;

        private readonly MaterialRequisitionAttachFileService mrAttachFileService;

        private readonly MaterialRequisitionCommentService mrCommentService;

        private readonly ObjectAssignedUserService objAssignedUserService;

        private readonly AMOSExService amosExService;

        private readonly IntergrateParamConfigService paramConfigService;

        private readonly WorkflowDetailService wfDetailService;

        private readonly FunctionPermissionService fncPermissionService;

        private readonly UserService userService = new UserService();

        /// <summary>
        /// Initializes a new instance of the <see cref="MRDetail"/> class.
        /// </summary>
        public MRDetail()
        {
            this.mrTypeService = new MRTypeService();
            this.priorityLevelService = new PriorityLevelService();
            this.roleService = new RoleService();
            this.mrService = new MaterialRequisitionService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.mrDetailService = new MaterialRequisitionDetailService();
            this.mrCheckListDefineService = new MaterialRequisitionCheckListDefineService();
            this.mrCheckListDetailService = new MaterialRequisitionCheckListDetailService();
            this.mrAttachFileService = new MaterialRequisitionAttachFileService();
            this.mrCommentService = new MaterialRequisitionCommentService();
            this.amosExService = new AMOSExService();
            this.objAssignedUserService = new ObjectAssignedUserService();
            this.paramConfigService = new IntergrateParamConfigService();
            this.wfDetailService = new WorkflowDetailService();
            this.fncPermissionService = new FunctionPermissionService();
        }

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
            this.RadScriptManager2.RegisterPostBackControl(this.btnExportMR);
            this.RadScriptManager2.RegisterPostBackControl(this.btnExportComment);

            if (!this.IsPostBack)
            {
                this.GetFuncPermissionConfig();
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
                {
                    var mrObj = this.mrService.GetById(new Guid(this.Request.QueryString["mrId"]));
                    if(mrObj!=null)
                    {
                        this.lblAMOSNo.Text = mrObj.AMOSWorkOrder;
                        this.lblMRNo.Text = mrObj.MRNo;
                        this.lblDepartment.Text = mrObj.DepartmentName;
                        this.lblJustification.Text = mrObj.Justification;
                        this.lblProjectName.Text = mrObj.ProjectName;

                        foreach (RadListBoxItem item in this.lbMRType.Items)
                        {
                            item.Checked = !string.IsNullOrEmpty(mrObj.MRTypeIds) && mrObj.MRTypeIds.Split(';').Contains(item.Value);
                        }

                        this.lblDateRequire.Text = mrObj.DateRequire;

                        foreach (RadListBoxItem item in this.lbPriority.Items)
                        {
                            item.Checked = mrObj.PriorityId.GetValueOrDefault().ToString() == item.Value;
                        }
                    }
                }

                //this.rtlMRCheckListDefine.ExpandAllItems();
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 1);
            if (fncPermission != null)
            {
                this.IsView.Value = fncPermission.IsView.GetValueOrDefault().ToString();
                this.IsCreate.Value = fncPermission.IsCreate.GetValueOrDefault().ToString();
                this.IsUpdate.Value = fncPermission.IsUpdate.GetValueOrDefault().ToString();
                this.IsCancel.Value = fncPermission.IsCancel.GetValueOrDefault().ToString();
                this.IsAttachWF.Value = fncPermission.IsAttachWorkflow.GetValueOrDefault().ToString();
            }
            else
            {
                this.IsView.Value = "False";
                this.IsCreate.Value = "False";
                this.IsUpdate.Value = "False";
                this.IsCancel.Value = "False";
                this.IsAttachWF.Value = "False";
            }
            // ----------------------------------------------------------------------------------------
        }

        private void LoadInitData()
        {
            var deptList = this.roleService.GetAll(false).OrderBy(t => t.NameWithLocation);
            foreach (var role in deptList)
            {
                var detpNode = new RadTreeNode(role.FullNameWithLocation);
                var userList1 = this.userService.GetAllByRoleId(role.Id).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);
                foreach (var user in userList1)
                {
                    detpNode.Nodes.Add(new RadTreeNode(user.FullNameWithPosition, user.Id.ToString()));
                }

                this.rtvPIC.Nodes.Add(detpNode);
            }

            var mrTypeList = this.mrTypeService.GetAll();
            this.lbMRType.DataSource = mrTypeList;
            this.lbMRType.DataTextField = "Name";
            this.lbMRType.DataValueField = "ID";
            this.lbMRType.DataBind();

            var priorityLvl = this.priorityLevelService.GetAll();
            this.lbPriority.DataSource = priorityLvl;
            this.lbPriority.DataTextField = "Name";
            this.lbPriority.DataValueField = "ID";
            this.lbPriority.DataBind();

            var mrId = new Guid(this.Request.QueryString["mrId"]);
            var mrObj = this.mrService.GetById(mrId);

            if (mrObj != null)
            {
                //this.txtPurchasingGroup.Text = mrObj.Comment_PurchasingGroup;
                //this.txtReceivedMRFromFacility.SelectedDate = mrObj.Comment_ReceivedMRFromFacility;
                //this.txtMrProcessComplete.SelectedDate = mrObj.Comment_MRProcessingCompleted;
                //this.txtForwardMRToTech.SelectedDate = mrObj.Comment_ForwardMRToTechDept;
                //this.txtReceiveMRFromTech.SelectedDate = mrObj.Comment_ReceivedMRFromTech;
                //this.txtForwardMRToPurchasing.SelectedDate = mrObj.Comment_ForwardMRToPurchasingGroup;

                this.txtOriginator.Text = mrObj.OriginatorName;
                this.txtOriginatorDate.Text = mrObj.OriginatorDate;

                this.txtStoremanName.Text = mrObj.StoreManName;
                this.txtStoremanDate.Text = mrObj.StoreManDate;

                this.txtSupervisorName.Text = mrObj.SupervisorName;
                this.txtSupervisorDate.Text = mrObj.SupervisorDate;

                this.txtOIMName.Text = mrObj.OIMName;
                this.txtOIMDate.Text = mrObj.OIMDate;
            }

            //this.btnComplete.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);
            //this.btnReject.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);
            //this.btnCompleteFinal.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);
            
            if (!string.IsNullOrEmpty(this.Request.QueryString["objAssignUserId"]))
            {
                this.ObjAssignUserId.Value = this.Request.QueryString["objAssignUserId"];
                this.ObjectType.Value = this.Request.QueryString["objType"];
                this.ObjectId.Value = this.Request.QueryString["mrId"];

                var objAssignUserId = new Guid(this.ObjAssignUserId.Value);
                var objAssignUser = this.objAssignedUserService.GetById(objAssignUserId);
                if (objAssignUser != null && objAssignUser.CanReject.GetValueOrDefault())
                {
                    this.btnReject.Visible = true;

                    var currentWFStep = this.wfDetailService.GetByCurrentStep(objAssignUser.CurrentWorkflowStepId.GetValueOrDefault());
                    if (currentWFStep != null && currentWFStep.NextWorkflowStepID == 0)
                    {
                        this.btnCompleteFinal.Visible = true;
                        this.btnComplete.Visible = false;
                    }
                    else
                    {
                        this.btnCompleteFinal.Visible = false;
                        this.btnComplete.Visible = true;
                    }
                }
                else
                {
                    this.btnReject.Visible = false;
                    this.btnCompleteFinal.Visible = false;
                }

                if (objAssignUser != null && objAssignUser.ActionTypeId == 2)
                {
                    this.btnReject.Visible = false;
                    this.btnCompleteFinal.Visible = false;
                    this.btnComplete.Visible = false;
                }
            }
            else
            {
                this.btnReject.Visible = false;
                this.btnCompleteFinal.Visible = false;
                this.btnComplete.Visible = false;
            }

            //if (mrObj != null && mrObj.IsCompleteFinal.GetValueOrDefault())
            //{
            //    this.btnReject.Visible = false;
            //    this.btnCompleteFinal.Visible = false;
            //    this.btnComplete.Visible = false;
            //}

            this.ddlCommentFrom.SelectedValue = UserSession.Current.User.CommentGroupId.ToString();
            if (this.ddlCommentFrom.SelectedIndex != 0)
            {
                this.txtComment.Text = @"_ Tổ:
_ Mục đích:
_ Kỹ thuật:
_ Tần xuất sử dụng:
_ Số lượng yêu cầu:
_ Chi phí:
_ Kính đề xuất:";
            }
            // Show hide function Control
            this.btnSaveMRSignInfo.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            //this.btnSaveReceivedInfo.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.btnSaveMRSignInfo.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.divMRDetail.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdMaterials.MasterTableView.GetColumn("EditColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdMaterials.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            this.grdCheckList.MasterTableView.CommandItemSettings.ShowCancelChangesButton = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdCheckList.MasterTableView.CommandItemSettings.ShowSaveChangesButton = Convert.ToBoolean(this.IsUpdate.Value);
            

            this.divMRAttachFile.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdAttachFile.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            this.divMRComment.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdComment.MasterTableView.GetColumn("EditColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdComment.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            this.divMRDetailTracking.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdMRDetailTracking.MasterTableView.GetColumn("EditColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            // --------------------------------------------------------------------------------------
        }


        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }

        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
        }

        protected void grdMaterials_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            this.grdMaterials.DataSource = this.mrDetailService.GetByMR(mrId).OrderBy(t => t.SFICode);
        }

        protected void grdMaterials_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            if (e.CommandName == "EditCmd")
            {
                var mrDetailId = new Guid(item.GetDataKeyValue("ID").ToString());
                var mrDetailObj = this.mrDetailService.GetById(mrDetailId);
                if (mrDetailObj != null && !mrDetailObj.IsCancel.GetValueOrDefault())
                {
                    Session.Add("EditingMRId", mrDetailObj.ID);
                    this.FillMRDetailData(mrDetailObj);
                }
            }
            else if (e.CommandName == "Cancle")
            {
                var mrDetailId = new Guid(item.GetDataKeyValue("ID").ToString());
                var mrDetailObj = this.mrDetailService.GetById(mrDetailId);
                if (mrDetailObj != null && !mrDetailObj.IsCancel.GetValueOrDefault())
                {
                    mrDetailObj.IsLeaf = false;
                    this.mrDetailService.Update(mrDetailObj);

                    var newMRDetailRevObj = new MaterialRequisitionDetail
                    {
                        ID = Guid.NewGuid(),
                        MRId = mrDetailObj.MRId,
                        MRNo = mrDetailObj.MRNo,
                        ParentId = mrDetailObj.ParentId ?? mrDetailObj.ID,
                        IsLeaf = true,
                        IsCancel = true,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullNameWithDeptPosition,
                        CreatedDate = DateTime.Now,

                        SFICode = mrDetailObj.SFICode,
                        ReqROBMin = mrDetailObj.ReqROBMin,
                        ReqROBMax = mrDetailObj.ReqROBMax,
                        ROB = mrDetailObj.ROB,
                        QtyRemarkForSpare = mrDetailObj.QtyRemarkForSpare,
                        QtyRemarkUseForJob = mrDetailObj.QtyRemarkUseForJob,
                        Units = mrDetailObj.Units,
                        Description = mrDetailObj.Description,
                        MakerName = mrDetailObj.MakerName,
                        CertificateRequired = mrDetailObj.CertificateRequired,
                        NormalUsingFrequency = mrDetailObj.NormalUsingFrequency,
                        Alternative = mrDetailObj.Alternative,
                        Remarks = mrDetailObj.Remarks,
                        QtyReq = mrDetailObj.QtyReq
                    };
                    this.mrDetailService.Insert(newMRDetailRevObj);

                    this.grdMaterials.Rebind();
                }
            }
        }

        protected void grdMaterials_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var mrDetailId = new Guid(item.GetDataKeyValue("ID").ToString());
            var mrDetailObj = this.mrDetailService.GetById(mrDetailId);
            if (mrDetailObj != null)
            {
                mrDetailObj.IsCancel = true;
                this.mrDetailService.Update(mrDetailObj);
                this.grdMaterials.Rebind();
            }

            
        }

        //protected void rtlMRCheckListDefine_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        //{
        //    var mrId = new Guid(this.Request.QueryString["mrId"]);
        //    var mrCheckListDefine = this.mrCheckListDefineService.GetAll();
        //    var mrCheckListDetail = this.mrCheckListDetailService.GetByMR(mrId);

        //    foreach (var item in mrCheckListDefine)
        //    {
        //        if (mrCheckListDetail.Select(t => t.MRCheckListId).Contains(item.ID))
        //        {
        //            item.IsYes = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsYes.GetValueOrDefault();
        //            item.IsNo = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsNo.GetValueOrDefault();
        //            item.IsNA = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsNA.GetValueOrDefault();
        //            item.Remark = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).Remark;
        //        }
        //    }

        //    this.rtlMRCheckListDefine.DataSource = mrCheckListDefine;
            
        //}

        protected void grdAttachFile_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = new Guid(item.GetDataKeyValue("ID").ToString());

            this.mrAttachFileService.Delete(docId);
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            var mrAttachFile = this.mrAttachFileService.GetByMR(mrId);

            this.grdAttachFile.DataSource = mrAttachFile;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var configObj = this.paramConfigService.GetById(1);
            var searchText = this.txtSFICode.Text.Replace(".", string.Empty);
            if (configObj != null)
            {
                // DeptID of RUBY II: 1000001
                // DeptID of BIENDONG : 1000003
                // Connect and search on AMOS data
                var dns = configObj.Amos_Dns;
                var uid = configObj.Amos_Uid;
                var pwd = configObj.Amos_Pwd;
                

                var connStr = "DSN=" + dns + ";UID=" + uid + ";pwd=" + pwd;

                var conn = new OdbcConnection(connStr);
                var da = new OdbcDataAdapter();
                var cmd = new OdbcCommand();
                cmd.CommandText = "SELECT " +
                                  "u.PartID as PartID," +
                                  "u.PartTypeID," +
                                  "u.StockMax," +
                                  "u.StockMin," +
                                  "spLocation.InStock as ROB," +
                                  "unit.Name as UnitName," +
                                  "t.PartTypeNo as PartNo," +
                                  "t.PartName as Name," +
                                  "t.MakerRef as MakerRef " +
                                  "FROM amos.SpareUnit u, amos.SpareLocation spLocation, amos.SpareType t " +
                                  //"left outer join amos.SpareTypeFinancial f ON t.PartTypeID = f.PartTypeID AND f.DeptID = amos.GetSharing(0, 'SpareTypeFinancial') " +
                                  "left join amos.Unit unit ON unit.UnitID = t.StockUnitID " +
                                  "WHERE u.PartTypeID = t.PartTypeID " +
                                  "AND u.PartID = spLocation.PartID " +
                                  "AND t.PartTypeNo like '%" + searchText + "%' " +
                                  "AND u.DeptID=" + configObj.DefaultDeptId;
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                var ds = new DataSet();
                try
                {
                    conn.Open();
                    
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.txtROBMin.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["StockMin"]);
                    this.txtROBMax.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["StockMax"]);
                    this.txtROB.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["ROB"]);
                    this.txtUnit.Text = ds.Tables[0].Rows[0]["UnitName"].ToString();
                    this.txtSFICode.Text = ds.Tables[0].Rows[0]["PartNo"].ToString();
                    this.txtMaterialName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    this.txtMakerRef.Text = ds.Tables[0].Rows[0]["MakerRef"].ToString();
                }

                // --------------------------------------------------------------------------------
            }

            // Get ref MR related with SFI code
            var objIdList = this.mrDetailService.GetAll().Where(t => !string.IsNullOrEmpty(this.txtSFICode.TextWithPromptAndLiterals) && t.SFICode.Contains(this.txtSFICode.TextWithPromptAndLiterals)).Select(t => t.MRId.GetValueOrDefault());
            var refMRList = this.mrService.GetAll().Where(t => objIdList.Contains(t.ID)).OrderByDescending(t => t.CreatedByDate);
            this.ddlRefMR.DataSource = refMRList;
            this.ddlRefMR.DataTextField = "MRNo";
            this.ddlRefMR.DataValueField = "ID";
            this.ddlRefMR.DataBind();
            // -------------------------------------------------------------------------------------

        }

        protected void btnMaterialDetailSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
            {
                var mrId = new Guid(this.Request.QueryString["mrId"]);
                var mrObj = this.mrService.GetById(mrId);
                if (mrObj != null)
                {
                    if (Session["EditingMRId"] != null)
                    {
                        var mrDetailId = new Guid(Session["EditingMRId"].ToString());
                        var mrDetailObj = this.mrDetailService.GetById(mrDetailId);
                        if (mrDetailObj != null)
                        {
                            // Create new  Rev of MR Detail
                            var newMRDetailRevObj = new MaterialRequisitionDetail
                            {
                                ID = Guid.NewGuid(),
                                MRId = mrObj.ID,
                                MRNo = mrObj.MRNo,
                                DateReq = mrObj.DateRequire,
                                PriorityName = mrObj.PriorityName,
                                ParentId = mrDetailObj.ParentId ?? mrDetailObj.ID,
                                IsLeaf = true,
                                IsCancel = false,
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.FullNameWithDeptPosition,
                                CreatedDate = DateTime.Now
                            };

                            this.CollectMRDetailData(newMRDetailRevObj);
                            this.mrDetailService.Insert(newMRDetailRevObj);


                            // Update old MR Detail
                            mrDetailObj.IsLeaf = false;
                            this.mrDetailService.Update(mrDetailObj);
                            // ---------------------------------------------
                        }

                        Session.Remove("EditingMRId");
                    }
                    else
                    {
                        var mrDetailObj = new MaterialRequisitionDetail
                        {
                            ID = Guid.NewGuid(),
                            MRId = mrObj.ID,
                            MRNo = mrObj.MRNo,
                            DateReq = mrObj.DateRequire,
                            PriorityName = mrObj.PriorityName,
                            IsLeaf = true,
                            IsCancel = false,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.FullNameWithDeptPosition,
                            CreatedDate = DateTime.Now,
                        };

                        this.CollectMRDetailData(mrDetailObj);
                        this.mrDetailService.Insert(mrDetailObj);
                    }
                }

                this.ClearMRDetailForm();
                this.grdMaterials.Rebind();
                this.grdMRDetailTracking.Rebind();
            }
        }

        private void CollectMRDetailData(MaterialRequisitionDetail mrDetailObj)
        {
            mrDetailObj.SFICode = this.txtSFICode.TextWithPromptAndLiterals;
            mrDetailObj.ReqROBMin = this.txtROBMin.Value;
            mrDetailObj.ReqROBMax = this.txtROBMax.Value;
            mrDetailObj.ROB = this.txtROB.Value;
            mrDetailObj.QtyRemarkUseForJob = this.txtUseForJob.Value;
            mrDetailObj.QtyRemarkForSpare = this.txtForSpare.Value;
            mrDetailObj.Units = this.txtUnit.Text;
            mrDetailObj.Description = this.txtMaterialName.Text;
            mrDetailObj.MakerName = this.txtMakerRef.Text;
            mrDetailObj.CertificateRequired = this.ddlCertReq.SelectedValue;
            mrDetailObj.NormalUsingFrequency = this.txtFrequency.Text.Trim();
            mrDetailObj.Alternative = this.ddlAlter.SelectedValue;
            mrDetailObj.Remarks = this.txtRemark.Text;
            mrDetailObj.QtyReq = this.txtQuantityReq.Value;
        }

        private void FillMRDetailData(MaterialRequisitionDetail mrDetailObj)
        {
            this.txtSFICode.Text = mrDetailObj.SFICode;
            this.txtROBMin.Value = mrDetailObj.ReqROBMin;
            this.txtROBMax.Value = mrDetailObj.ReqROBMax;
            this.txtROB.Value = mrDetailObj.ROB;
            this.txtUseForJob.Value = mrDetailObj.QtyRemarkUseForJob;
            this.txtForSpare.Value = mrDetailObj.QtyRemarkForSpare;
            this.txtUnit.Text = mrDetailObj.Units;
            this.txtMaterialName.Text = mrDetailObj.Description;
            this.txtMakerRef.Text = mrDetailObj.MakerName;
            this.ddlCertReq.SelectedValue = mrDetailObj.CertificateRequired;
            this.txtFrequency.Text = mrDetailObj.NormalUsingFrequency;
            this.ddlAlter.SelectedValue = mrDetailObj.Alternative;
            this.txtRemark.Text = mrDetailObj.Remarks;
            this.txtQuantityReq.Value = mrDetailObj.QtyReq;
        }

        private void ClearMRDetailForm()
        {
            this.txtSFICode.Text = string.Empty;
            this.txtROBMin.Value = null;
            this.txtROBMax.Value = null;
            this.txtROB.Value = null;
            this.txtUseForJob.Value = null;
            this.txtForSpare.Value = null;
            this.txtUnit.Text = string.Empty;
            this.txtMaterialName.Text = string.Empty;
            this.txtMakerRef.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            this.txtQuantityReq.Value = null;
            this.ddlAlter.SelectedIndex = 0;
            this.txtFrequency.Text = string.Empty;
            this.ddlCertReq.SelectedIndex = 0;
        }

        protected void btnMaterialDetailClear_Click(object sender, EventArgs e)
        {
            Session.Remove("EditingMRId");
            this.ClearMRDetailForm();

        }

        //protected void rtlMRCheckListDefine_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var temp = rtlMRCheckListDefine.SelectedItems[0].GetDataKeyValue("ID");
        //}

        //protected void btnCheckListSave_Click(object sender, EventArgs e)
        //{
        //    if (this.rtlMRCheckListDefine.SelectedItems.Count > 0)
        //    {
        //        var mrId = new Guid(this.Request.QueryString["mrId"]);
        //        var mrCheckListDefineId = Convert.ToInt32(this.rtlMRCheckListDefine.SelectedItems[0].GetDataKeyValue("ID"));
        //        var mrCheckListDetailObj = this.mrCheckListDetailService.GetByMR(mrId, mrCheckListDefineId);
        //        if (mrCheckListDetailObj != null)
        //        {
        //            mrCheckListDetailObj.IsNA = this.rbtnNA.Checked;
        //            mrCheckListDetailObj.IsYes = this.rbtnYes.Checked;
        //            mrCheckListDetailObj.IsNo = this.rbtnNo.Checked;
        //            mrCheckListDetailObj.Remark = this.txtCheckListRemark.Text;

        //            this.mrCheckListDetailService.Update(mrCheckListDetailObj);
        //        }
        //        else
        //        {
        //            mrCheckListDetailObj = new MaterialRequisitionCheckListDetail
        //            {
        //                ID = Guid.NewGuid(),
        //                MRId = mrId,
        //                MRCheckListId = mrCheckListDefineId,
        //                IsNA = this.rbtnNA.Checked,
        //                IsYes = this.rbtnYes.Checked,
        //                IsNo = this.rbtnNo.Checked,
        //                Remark = this.txtCheckListRemark.Text
        //            };

        //            this.mrCheckListDetailService.Insert(mrCheckListDetailObj);
        //        }

        //        this.ClearCheckListForm();
        //        this.rtlMRCheckListDefine.Rebind();
        //    }
        //}

        //protected void btnCheckListClear_Click(object sender, EventArgs e)
        //{
        //    this.ClearCheckListForm();
        //}

        //private void ClearCheckListForm()
        //{
        //    this.rbtnYes.Checked = false;
        //    this.rbtnNo.Checked = false;
        //    this.rbtnNA.Checked = false;
        //    this.txtCheckListRemark.Text = string.Empty;
        //    this.txtCheckListDescription.Text = String.Empty;
        //    this.rtlMRCheckListDefine.ClearSelectedItems();
        //}
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (this.rtlMRCheckListDefine.SelectedItems.Count > 0)
            //{
            //    var mrId = new Guid(this.Request.QueryString["mrId"]);
            //    var mrCheckListDefineId = Convert.ToInt32(this.rtlMRCheckListDefine.SelectedItems[0].GetDataKeyValue("ID"));
            //    this.txtCheckListDescription.Text = this.rtlMRCheckListDefine.SelectedItems[0]["Description"].Text;

            //    var mrCheckListDetailObj = this.mrCheckListDetailService.GetByMR(mrId, mrCheckListDefineId);
            //    if (mrCheckListDetailObj != null)
            //    {
            //        this.rbtnYes.Checked = mrCheckListDetailObj.IsYes.GetValueOrDefault();
            //        this.rbtnNo.Checked = mrCheckListDetailObj.IsNo.GetValueOrDefault();
            //        this.rbtnNA.Checked = mrCheckListDetailObj.IsNA.GetValueOrDefault();
            //        this.txtCheckListRemark.Text = mrCheckListDetailObj.Remark;
            //    }
            //    else
            //    {
            //        this.rbtnYes.Checked = false;
            //        this.rbtnNo.Checked = false;
            //        this.rbtnNA.Checked = false;
            //        this.txtCheckListRemark.Text = string.Empty;
            //    }
            //}
        }

        protected void btnSaveAttachFile_Click(object sender, EventArgs e)
        {
            var mrId =  new Guid(this.Request.QueryString["mrId"]);
            var fileIcon = new Dictionary<string, string>()
                    {
                        { "doc", "~/images/wordfile.png" },
                        { "docx", "~/images/wordfile.png" },
                        { "dotx", "~/images/wordfile.png" },
                        { "xls", "~/images/excelfile.png" },
                        { "xlsx", "~/images/excelfile.png" },
                        { "pdf", "~/images/pdffile.png" },
                        { "7z", "~/images/7z.png" },
                        { "dwg", "~/images/dwg.png" },
                        { "dxf", "~/images/dxf.png" },
                        { "rar", "~/images/rar.png" },
                        { "zip", "~/images/zip.png" },
                        { "txt", "~/images/txt.png" },
                        { "xml", "~/images/xml.png" },
                        { "xlsm", "~/images/excelfile.png" },
                        { "bmp", "~/images/bmp.png" },
                    };

            var targetFolder = "../../DocumentLibrary/MaterialRequisition";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/MaterialRequisition";
            foreach (UploadedFile docFile in docuploader.UploadedFiles)
            {
                var docFileName = docFile.FileName;

                var serverDocFileName = docFileName;

                // Path file to save on server disc
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + serverDocFileName;
                var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1, docFileName.Length - docFileName.LastIndexOf(".") - 1);

                docFile.SaveAs(saveFilePath, true);

                var attachFile = new MaterialRequisitionAttachFile()
                {
                    ID = Guid.NewGuid(),
                    MRId = mrId,
                    Filename = docFileName,
                    Extension = fileExt,
                    FilePath = serverFilePath,
                    ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                    FileSize = (double)docFile.ContentLength / 1024,
                    CreatedBy = UserSession.Current.User.Id,
                    CreatedByName = UserSession.Current.User.FullNameWithDeptPosition,
                    CreatedDate = DateTime.Now,
                    Description = this.txtAttachDescription.Text
                };

                this.mrAttachFileService.Insert(attachFile);
            }

            // Update final complete for MR
            var obj = this.mrService.GetById(mrId);
            if (obj != null && obj.IsWFComplete.GetValueOrDefault() && !obj.IsCompleteFinal.GetValueOrDefault())
            {
                obj.IsCompleteFinal = true;
                this.mrService.Update(obj);
            }
            //-----------------------------------------------------

            this.grdAttachFile.Rebind();
        }

        protected void btnExportMR_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
            {
                var mrTypeList = this.mrTypeService.GetAll();
                var priorityLvlList = this.priorityLevelService.GetAll();

                var mrId = new Guid(this.Request.QueryString["mrId"]);
                var mrObj = this.mrService.GetById(mrId);

                var mrCheckListDefine = this.mrCheckListDefineService.GetAll();
                var mrCheckListDetail = this.mrCheckListDetailService.GetByMR(mrId);

                foreach (var item in mrCheckListDefine)
                {
                    if (mrCheckListDetail.Select(t => t.MRCheckListId).Contains(item.ID))
                    {
                        item.IsYes = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsYes.GetValueOrDefault();
                        item.IsNo = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsNo.GetValueOrDefault();
                        item.IsNA = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsNA.GetValueOrDefault();
                        item.Remark = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).Remark;
                    }
                }

                if (mrObj != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\MRTemplate.xlsm");

                    var dataSheet = workbook.Worksheets[0];
                    var checklist = workbook.Worksheets[1];
                    var attachImage = workbook.Worksheets[2];

                    dataSheet.Cells["A1"].PutValue(dataSheet.Cells["A1"].Value.ToString().Replace("<FacilityName>", mrObj.ProjectName)); 
                    dataSheet.Cells["A2"].PutValue(dataSheet.Cells["A2"].Value.ToString().Replace("<AMOSNo>", mrObj.AMOSWorkOrder)); 
                    dataSheet.Cells["J2"].PutValue(dataSheet.Cells["J2"].Value.ToString().Replace("<MRNo>", mrObj.MRNo)); 
                    dataSheet.Cells["M2"].PutValue(dataSheet.Cells["M2"].Value.ToString().Replace("<Department>", mrObj.DepartmentName));
                    dataSheet.Cells["F3"].PutValue(mrObj.Justification);
                    dataSheet.Cells["F3"].PutValue(mrObj.Justification);



                    checklist.Cells["B2"].PutValue(checklist.Cells["B2"].Value.ToString().Replace("<FacilityName>", mrObj.ProjectName));
                    checklist.Cells["B3"].PutValue(checklist.Cells["B3"].Value.ToString().Replace("<MRNo>", mrObj.MRNo));

                    // Fill mrtype checkbox
                    for (int i = 0; i < mrTypeList.Count; i++)
                    {
                        var index = dataSheet.CheckBoxes.Add(3, 9 + i, 30, 120);
                        Aspose.Cells.Drawing.CheckBox checkBox = dataSheet.CheckBoxes[index];
                        checkBox.Text = mrTypeList[i].Name;
                        if (mrObj.MRTypeIds.Split(';').Contains(mrTypeList[i].ID.ToString()))
                        {
                            checkBox.CheckedValue = CheckValueType.Checked;
                        }
                    }
                    // -----------------------------------------------------------------------

                    var dtFull = new DataTable();
                    dtFull.Columns.AddRange(new[]
                    {
                        new DataColumn("NoIndex", typeof (String)),
                        new DataColumn("Min", typeof (String)),
                        new DataColumn("Max", typeof (String)),
                        new DataColumn("ROB", typeof (String)),
                        new DataColumn("QtyReq", typeof (String)),
                        new DataColumn("Use", typeof (String)),
                        new DataColumn("Spare", typeof (String)),
                        new DataColumn("Unit", typeof (String)),
                        new DataColumn("SFICode", typeof (String)),
                        new DataColumn("Description", typeof (String)),
                        new DataColumn("Maker", typeof (String)),
                        new DataColumn("CertReq", typeof (String)),
                        new DataColumn("Alter", typeof (String)),
                        new DataColumn("Frequency", typeof (String)),
                        new DataColumn("Remark", typeof (String)),
                    });

                    var mrDetailList = this.mrDetailService.GetLatestByMR(mrObj.ID);
                    for (int i = 0; i < mrDetailList.Count; i++)
                    {
                        var dataRow = dtFull.NewRow();
                        dataRow["NoIndex"] = i + 1;
                        dataRow["Min"] = mrDetailList[i].ReqROBMin;
                        dataRow["Max"] = mrDetailList[i].ReqROBMax;
                        dataRow["ROB"] = mrDetailList[i].ROB;
                        dataRow["QtyReq"] = mrDetailList[i].QtyReq;
                        dataRow["Use"] = mrDetailList[i].QtyRemarkUseForJob;
                        dataRow["Spare"] = mrDetailList[i].QtyRemarkForSpare;
                        dataRow["Unit"] = mrDetailList[i].Units;
                        dataRow["SFICode"] = mrDetailList[i].SFICode;
                        dataRow["Description"] = mrDetailList[i].Description;
                        dataRow["Maker"] = mrDetailList[i].MakerName;
                        dataRow["CertReq"] = mrDetailList[i].CertificateRequired;
                        dataRow["Alter"] = mrDetailList[i].Alternative;
                        dataRow["Frequency"] = mrDetailList[i].NormalUsingFrequency;
                        dataRow["Remark"] = mrDetailList[i].Remarks;

                        dtFull.Rows.Add(dataRow);

                    }

                    dataSheet.Cells.ImportDataTable(dtFull, false, 6, 0, dtFull.Rows.Count, dtFull.Columns.Count, true);
                    dataSheet.Cells.DeleteRow(6 + dtFull.Rows.Count);

                    dataSheet.Cells[6 + dtFull.Rows.Count, 8].PutValue(mrObj.DateRequire);

                    // Fill priority checkbox
                    for (int i = 0; i < priorityLvlList.Count; i++)
                    {
                        var index = dataSheet.CheckBoxes.Add(6 + dtFull.Rows.Count, 10 + (i * 2), 30, 120);
                        Aspose.Cells.Drawing.CheckBox checkBox = dataSheet.CheckBoxes[index];
                        checkBox.Text = priorityLvlList[i].Name;
                        if (mrObj.PriorityId.GetValueOrDefault() == priorityLvlList[i].ID)
                        {
                            checkBox.CheckedValue = CheckValueType.Checked;
                        }
                    }
                    // -----------------------------------------------------------------------


                    // Fill Checklist data
                    var dtCheckListFull = new DataTable();
                    dtCheckListFull.Columns.AddRange(new[]
                    {
                        new DataColumn("Description", typeof (String)),
                        new DataColumn("Yes", typeof (String)),
                        new DataColumn("No", typeof (String)),
                        new DataColumn("NA", typeof (String)),
                        new DataColumn("Remark", typeof (String)),
                    });
                    for (int i = 0; i < mrCheckListDefine.Count; i++)
                    {
                        var dataRow = dtCheckListFull.NewRow();
                        dataRow["Description"] = mrCheckListDefine[i].Description;
                        dataRow["Yes"] = mrCheckListDefine[i].IsYes ? "X" : string.Empty;
                        dataRow["No"] = mrCheckListDefine[i].IsNo ? "X" : string.Empty;
                        dataRow["NA"] = mrCheckListDefine[i].IsNA ? "X" : string.Empty;
                        dataRow["Remark"] = mrCheckListDefine[i].Remark;
                        dtCheckListFull.Rows.Add(dataRow);
                    }

                    checklist.Cells.ImportDataTable(dtCheckListFull, false, 4, 1, dtCheckListFull.Rows.Count, dtCheckListFull.Columns.Count, true);
                    checklist.Cells.DeleteRow(4 + dtCheckListFull.Rows.Count);
                    // -------------------------------------------------------------------------

                    // Add image
                    var imageAttachList = this.mrAttachFileService.GetByMR(mrObj.ID).Where(t => t.Extension == "png" || t.Extension == "jpg").ToList();
                    for (int i = 0; i < imageAttachList.Count; i++)
                    {
                        attachImage.Cells[4, 2 + (i * 5)].PutValue(imageAttachList[i].Description);
                        attachImage.Pictures.Add(5, 2 + (i * 5), Server.MapPath(imageAttachList[i].FilePath));
                    }
                    // --------------------------------------------------------------------------

                    dataSheet.Cells["A" + (9 + mrDetailList.Count)].PutValue(mrObj.OriginatorName);
                    dataSheet.Cells["E" + (10 + mrDetailList.Count)].PutValue(mrObj.OriginatorDate);

                    dataSheet.Cells["I" + (9 + mrDetailList.Count)].PutValue(mrObj.StoreManName);
                    dataSheet.Cells["J" + (10 + mrDetailList.Count)].PutValue(mrObj.StoreManDate);

                    dataSheet.Cells["K" + (9 + mrDetailList.Count)].PutValue(mrObj.SupervisorName);
                    dataSheet.Cells["L" + (10 + mrDetailList.Count)].PutValue(mrObj.SupervisorDate);

                    dataSheet.Cells["N" + (9 + mrDetailList.Count)].PutValue(mrObj.OIMName);
                    dataSheet.Cells["O" + (10 + mrDetailList.Count)].PutValue(mrObj.OIMDate);


                    var filename = Utility.RemoveSpecialCharacterFileName(mrObj.MRNo) + "_MR Form.xlsm";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);

                }
                
            }
        }

        protected void btnExportComment_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
            {
                var mrId = new Guid(this.Request.QueryString["mrId"]);
                var mrObj = this.mrService.GetById(mrId);
                if (mrObj != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\MRCommentTemplate.xlsm");

                    var dataSheet = workbook.Worksheets[0];

                    dataSheet.Cells["A1"].PutValue(dataSheet.Cells["A1"].Value.ToString().Replace("<FacilityName>", mrObj.ProjectName));
                    dataSheet.Cells["B3"].PutValue(dataSheet.Cells["B3"].Value.ToString().Replace("<FacilityName>", mrObj.ProjectName));
                    dataSheet.Cells["B2"].PutValue(mrObj.AMOSWorkOrder);
                    dataSheet.Cells["F2"].PutValue(mrObj.MRNo);
                    dataSheet.Cells["F3"].PutValue(dataSheet.Cells["F3"].Value.ToString().Replace("<Purchasing>", mrObj.Comment_PurchasingGroup));


                    dataSheet.Cells["B5"].PutValue(mrObj.Comment_ReceivedMRFromFacility);
                    dataSheet.Cells["C5"].PutValue(mrObj.Comment_MRProcessingCompleted);
                    dataSheet.Cells["D5"].PutValue(mrObj.Comment_ForwardMRToTechDept);
                    dataSheet.Cells["E5"].PutValue(mrObj.Comment_ReceivedMRFromTech);
                    dataSheet.Cells["F5"].PutValue(mrObj.Comment_ForwardMRToPurchasingGroup);

                    var operationComment = this.mrCommentService.GetByMR(mrId).Where(t => t.CommentTypeId == 1).ToList();
                    var techComment = this.mrCommentService.GetByMR(mrId).Where(t => t.CommentTypeId == 2).ToList();
                    var bodComment = this.mrCommentService.GetByMR(mrId).Where(t => t.CommentTypeId == 3).ToList();

                    for (int i = 0; i < operationComment.Count; i++)
                    {
                        dataSheet.Cells[6 + i, 0].PutValue(operationComment[i].Comment + " (Commented by: " + operationComment[i].CommentByName + ")");
                    }

                    for (int i = 0; i < techComment.Count; i++)
                    {
                        dataSheet.Cells[6 + i, 2].PutValue(techComment[i].Comment + " (Commented by: " + techComment[i].CommentByName + ")");
                    }

                    for (int i = 0; i < bodComment.Count; i++)
                    {
                        dataSheet.Cells[6 + i, 4].PutValue(bodComment[i].Comment + " (Commented by: " + bodComment[i].CommentByName + ")");
                    }

                    var filename = Utility.RemoveSpecialCharacterFileName(mrObj.MRNo) + "_MR Check-Comment-Approve Form.xlsm";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);

                }
            }
        }
        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void btnSaveComment_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["mrId"]))
            {
                var mrId = new Guid(this.Request.QueryString["mrId"]);
                var mrObj = this.mrService.GetById(mrId);
                if (mrObj != null)
                {
                    var commentObj = new MaterialRequisitionComment();
                    if (Session["EditingCommentId"] != null)
                    {
                        var commentId = new Guid(Session["EditingCommentId"].ToString());
                        commentObj = this.mrCommentService.GetById(commentId);
                        if (commentObj != null)
                        {
                            commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                            commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                            commentObj.Comment = this.txtComment.Text;
                            commentObj.CommentBy = UserSession.Current.User.Id;
                            commentObj.CommentByName = UserSession.Current.User.FullNameWithDeptPosition;
                            commentObj.CommentDate = DateTime.Now;

                            this.mrCommentService.Update(commentObj);
                        }

                        Session.Remove("EditingCommentId");
                    }
                    else
                    {
                        commentObj.ID = Guid.NewGuid();
                        commentObj.MRId = mrObj.ID;
                        commentObj.MRNo = mrObj.MRNo;
                        commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                        commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                        commentObj.Comment = this.txtComment.Text;
                        commentObj.CommentBy = UserSession.Current.User.Id;
                        commentObj.CommentByName = UserSession.Current.User.FullNameWithDeptPosition;
                        commentObj.CommentDate = DateTime.Now;

                        this.mrCommentService.Insert(commentObj);
                    }

                    if (!string.IsNullOrEmpty(this.Request.QueryString["objAssignUserId"]) && commentObj != null)
                    {
                        var objAssignUserId = new Guid(this.Request.QueryString["objAssignUserId"]);
                        var objAssignedUser = this.objAssignedUserService.GetById(objAssignUserId);
                        if (objAssignedUser != null)
                        {
                            objAssignedUser.CommentContent = commentObj.Comment;

                            this.objAssignedUserService.Update(objAssignedUser);
                        }
                    }
                }

                this.txtComment.Text = string.Empty;
                this.grdComment.Rebind();
            }
        }

        protected void grdComment_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            if (e.CommandName == "EditCmd")
            {
                var commentId = new Guid(item.GetDataKeyValue("ID").ToString());
                var commentObj = this.mrCommentService.GetById(commentId);
                if (commentObj != null)
                {
                    Session.Add("EditingCommentId", commentObj.ID);
                    this.ddlCommentFrom.SelectedValue = commentObj.CommentTypeId.ToString();
                    this.txtComment.Text = commentObj.Comment;
                }
            }
        }

        protected void grdComment_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var commentId = new Guid(item.GetDataKeyValue("ID").ToString());

            this.mrCommentService.Delete(commentId);
            this.grdComment.Rebind();
        }

        protected void grdComment_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            var commentList = this.mrCommentService.GetByMR(mrId).OrderByDescending(t => t.CommentDate);
            this.grdComment.DataSource = commentList;
        }

        protected void grdComment_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem ditem = (GridDataItem)e.Item;
                ImageButton linkbutton1 = (ImageButton)ditem["EditColumn"].Controls[0];
                ImageButton linkbutton2 = (ImageButton)ditem["DeleteColumn"].Controls[0];

                var commentBy = Convert.ToInt32(ditem["CommentBy"].Text);
                
                linkbutton1.Visible = commentBy == UserSession.Current.User.Id;
                linkbutton2.Visible = commentBy == UserSession.Current.User.Id;
            }
        }

        protected void btnSaveReceivedInfo_Click(object sender, EventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            var mrObj = this.mrService.GetById(mrId);

            if (mrObj != null)
            {
                //mrObj.Comment_PurchasingGroup = this.txtPurchasingGroup.Text.Trim();
                //mrObj.Comment_ReceivedMRFromFacility = this.txtReceivedMRFromFacility.SelectedDate;
                //mrObj.Comment_MRProcessingCompleted = this.txtMrProcessComplete.SelectedDate;
                //mrObj.Comment_ForwardMRToTechDept = this.txtForwardMRToTech.SelectedDate;
                //mrObj.Comment_ReceivedMRFromTech = this.txtReceiveMRFromTech.SelectedDate;
                //mrObj.Comment_ForwardMRToPurchasingGroup = this.txtForwardMRToPurchasing.SelectedDate;

                this.mrService.Update(mrObj);
            }

            this.RadScriptManager2.RegisterPostBackControl(this.btnExportMR);
            this.RadScriptManager2.RegisterPostBackControl(this.btnExportComment);
        }


        protected void btnSaveMRSignInfo_Click(object sender, EventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            var mrObj = this.mrService.GetById(mrId);

            if (mrObj != null)
            {
                mrObj.OriginatorName = this.txtOriginator.Text;
                mrObj.OriginatorDate = this.txtOriginatorDate.Text;

                mrObj.StoreManName = this.txtStoremanName.Text ;
                mrObj.StoreManDate = this.txtStoremanDate.Text;

                mrObj.SupervisorName = this.txtSupervisorName.Text ;
                mrObj.SupervisorDate = this.txtSupervisorDate.Text;

                mrObj.OIMName = this.txtOIMName.Text ;
                mrObj.OIMDate = this.txtOIMDate.Text;
                this.mrService.Update(mrObj);
            }
        }

        protected void grdMaterials_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["IsCancel"].Text == "True")
                {
                    item["Status"].BackColor = Color.Red;
                    item["Status"].BorderColor = Color.Red;
                }
            }
        }

        protected void grdMaterials_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var historyLink = (Image) e.Item.FindControl("HistoryLink");
                historyLink.Attributes["href"] = "#";
                historyLink.Attributes["onclick"] = string.Format(
                    "return ShowHistory('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
        }

        protected void grdCheckList_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            var mrCheckListDefine = this.mrCheckListDefineService.GetAll().Where(t => t.ParentId != null);
            var mrCheckListDetail = this.mrCheckListDetailService.GetByMR(mrId);

            foreach (var item in mrCheckListDefine)
            {
                if (mrCheckListDetail.Select(t => t.MRCheckListId).Contains(item.ID))
                {
                    item.IsYes = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsYes.GetValueOrDefault();
                    item.IsNo = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsNo.GetValueOrDefault();
                    item.IsNA = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).IsNA.GetValueOrDefault();
                    item.Remark = mrCheckListDetail.FirstOrDefault(t => t.MRCheckListId == item.ID).Remark;
                }
            }

            this.grdCheckList.DataSource = mrCheckListDefine;
        }

        protected void grdCheckList_OnBatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                var mrCheckListDefineId = Convert.ToInt32(newValues["ID"].ToString());
                var mrId = new Guid(this.Request.QueryString["mrId"]);

                var isYes = Convert.ToBoolean(newValues["IsYes"].ToString());
                var isNo = Convert.ToBoolean(newValues["IsNo"].ToString());
                var isNa = Convert.ToBoolean(newValues["IsNA"].ToString());
                var remark = newValues["Remark"]?.ToString() ?? string.Empty;

                var mrCheckListDetailObj = this.mrCheckListDetailService.GetByMR(mrId, mrCheckListDefineId);
                if (mrCheckListDetailObj != null)
                {
                    mrCheckListDetailObj.IsNA = isNa;
                    mrCheckListDetailObj.IsYes = isYes;
                    mrCheckListDetailObj.IsNo = isNo;
                    mrCheckListDetailObj.Remark = remark;

                    this.mrCheckListDetailService.Update(mrCheckListDetailObj);
                }
                else
                {
                    mrCheckListDetailObj = new MaterialRequisitionCheckListDetail
                    {
                        ID = Guid.NewGuid(),
                        MRId = mrId,
                        MRCheckListId = mrCheckListDefineId,
                        IsNA = isNa,
                        IsYes = isYes,
                        IsNo = isNo,
                        Remark = remark
                    };

                    this.mrCheckListDetailService.Insert(mrCheckListDetailObj);
                }
            }
        }

        protected void grdMRDetailTracking_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            if (e.CommandName == "EditCmd")
            {
                var mrDetailId = new Guid(item.GetDataKeyValue("ID").ToString());
                var mrDetailObj = this.mrDetailService.GetById(mrDetailId);
                if (mrDetailObj != null)
                {
                    Session.Add("EditingMRDetailTrackingId", mrDetailObj.ID);
                    this.lblSFICode.Text = mrDetailObj.SFICode;
                    this.lblMRDetailDescription.Text = mrDetailObj.Description;

                    this.txtMRTrackingRecieveDate.SelectedDate = mrDetailObj.MRRecieveDate;

                    foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
                    {
                        foreach (RadTreeNode userNode in deptNode.Nodes)
                        {
                            if (!string.IsNullOrEmpty(mrDetailObj.PICIds) && mrDetailObj.PICIds.Split(';').Contains(userNode.Value))
                            {
                                userNode.Checked = true;
                            }
                        }
                    }

                    this.txtContractNo.Text = mrDetailObj.ContractNumber;
                    this.txtRequestQuotationDate.SelectedDate = mrDetailObj.ReqQuotationDate;
                    this.txtDeliveryDateRevisedSupplier.SelectedDate = mrDetailObj.DeliveryDateRevisedBySupplier;
                    this.txtReceiveQuotationDate.SelectedDate = mrDetailObj.ReceiveQuotationDate;
                    this.txtPONumber.Text = mrDetailObj.PONumber;
                    this.txtPOIssueDate.SelectedDate = mrDetailObj.POIssueDate;
                    this.txtUnit.Text = mrDetailObj.UnitPrice;
                    this.txtTotalPrice.Text = mrDetailObj.TotalPrice;
                    this.txtExpectedDeliveryDate.SelectedDate = mrDetailObj.ExpectedDeliveryDate;
                    this.txtActualDeliveryDate.SelectedDate = mrDetailObj.ActualDeliveryDate;
                    this.txtSupplier.Text = mrDetailObj.SupplierName;
                    this.ddlMSRPurchasingStatus.SelectedValue = mrDetailObj.MRPurchasingStatus;
                    this.ddlMSRItemPurchasingStatus.SelectedValue = mrDetailObj.MRDetailPurchasingStatus;
                }
            }
        }

        protected void grdMRDetailTracking_ItemDataBound(object sender, GridItemEventArgs e)
        {
        }

        protected void grdMRDetailTracking_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var mrId = new Guid(this.Request.QueryString["mrId"]);
            this.grdMRDetailTracking.DataSource = this.mrDetailService.GetByMR(mrId).Where(t => !t.IsCancel.GetValueOrDefault()).OrderBy(t => t.SFICode);
        }

        protected void grdMRDetailTracking_OnItemCreated(object sender, GridItemEventArgs e)
        {
        }

        protected void btnMRDetailTrackingSave_OnClick(object sender, EventArgs e)
        {
            if (Session["EditingMRDetailTrackingId"] != null)
            {
                var mrDetailId = new Guid(Session["EditingMRDetailTrackingId"].ToString());
                var mrDetailObj = this.mrDetailService.GetById(mrDetailId);
                if (mrDetailObj != null)
                {
                    mrDetailObj.MRRecieveDate = this.txtMRTrackingRecieveDate.SelectedDate;

                    mrDetailObj.PICIds = string.Empty;
                    mrDetailObj.PICName = string.Empty;
                    foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        mrDetailObj.PICIds += pic.Value + ";";
                        mrDetailObj.PICName += pic.Text + Environment.NewLine;
                    }

                    mrDetailObj.ContractNumber = this.txtContractNo.Text;
                    mrDetailObj.ReqQuotationDate = this.txtRequestQuotationDate.SelectedDate;
                    mrDetailObj.DeliveryDateRevisedBySupplier = this.txtDeliveryDateRevisedSupplier.SelectedDate;
                    mrDetailObj.ReceiveQuotationDate = this.txtReceiveQuotationDate.SelectedDate;
                    mrDetailObj.PONumber = this.txtPONumber.Text;
                    mrDetailObj.POIssueDate = this.txtPOIssueDate.SelectedDate;
                    mrDetailObj.UnitPrice = this.txtUnit.Text;
                    mrDetailObj.TotalPrice = this.txtTotalPrice.Text;
                    mrDetailObj.ExpectedDeliveryDate = this.txtExpectedDeliveryDate.SelectedDate;
                    mrDetailObj.ActualDeliveryDate = this.txtActualDeliveryDate.SelectedDate;
                    mrDetailObj.SupplierName = this.txtSupplier.Text;
                    mrDetailObj.MRPurchasingStatus = this.ddlMSRPurchasingStatus.SelectedValue;
                    mrDetailObj.MRDetailPurchasingStatus = this.ddlMSRItemPurchasingStatus.SelectedValue;

                    this.mrDetailService.Update(mrDetailObj);
                }

                Session.Remove("EditingMRDetailTrackingId");
            }

            this.ClearMRDetailTrackingInfo();
            this.grdMRDetailTracking.Rebind();

        }

        protected void btnMRDetailTrackingClear_OnClick(object sender, EventArgs e)
        {
            Session.Remove("EditingMRDetailTrackingId");
            this.ClearMRDetailTrackingInfo();
        }

        private void ClearMRDetailTrackingInfo()
        {
            this.txtMRTrackingRecieveDate.SelectedDate = null;

            foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
            {
                foreach (RadTreeNode userNode in deptNode.Nodes)
                {
                    userNode.Checked = false;
                }
            }

            this.txtContractNo.Text = string.Empty;
            this.txtRequestQuotationDate.SelectedDate = null;
            this.txtDeliveryDateRevisedSupplier.SelectedDate = null;
            this.txtReceiveQuotationDate.SelectedDate = null;
            this.txtPONumber.Text = string.Empty;
            this.txtPOIssueDate.SelectedDate = null;
            this.txtUnit.Text = string.Empty;
            this.txtTotalPrice.Text = string.Empty;
            this.txtExpectedDeliveryDate.SelectedDate = null;
            this.txtActualDeliveryDate.SelectedDate = null;
            this.txtSupplier.Text = string.Empty;
            this.lblSFICode.Text = string.Empty;
            this.lblMRDetailDescription.Text = string.Empty;
        }

        protected void ddlCommentFrom_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCommentFrom.SelectedIndex != 0)
            {
                this.txtComment.Text = @"_ Tổ:
_ Mục đích:
_ Kỹ thuật:
_ Tần xuất sử dụng:
_ Số lượng yêu cầu:
_ Chi phí:
_ Kính đề xuất:";
            }
        }
    }
}