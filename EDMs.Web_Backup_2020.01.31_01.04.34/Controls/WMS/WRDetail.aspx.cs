﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class WRDetail : Page
    {
        private readonly MRTypeService mrTypeService;

        private readonly PriorityLevelService priorityLevelService;

        private readonly RoleService roleService;

        private readonly WorkRequestService wrService;

        private readonly ScopeProjectService projectService;

        private readonly NumberManagementService numberManagementService;

        private readonly WorkRequestAttachFileService wrAttachFileService;

        private readonly WorkRequestCommentService wrCommentService;

        private readonly AMOSExService amosExService;

        private readonly ObjectAssignedUserService objAssignedUserService;

        private readonly WorkflowDetailService wfSetailService;

        private readonly FunctionPermissionService fncPermissionService;


        /// <summary>
        /// Initializes a new instance of the <see cref="MRDetail"/> class.
        /// </summary>
        public WRDetail()
        {
            this.mrTypeService = new MRTypeService();
            this.priorityLevelService = new PriorityLevelService();
            this.roleService = new RoleService();
            this.wrService = new WorkRequestService();
            this.projectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
            this.wrAttachFileService = new WorkRequestAttachFileService();
            this.wrCommentService = new WorkRequestCommentService();
            this.amosExService = new AMOSExService();
            this.objAssignedUserService = new ObjectAssignedUserService();
            this.wfSetailService = new WorkflowDetailService();
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
            this.RadScriptManager2.RegisterPostBackControl(this.btnExportWR);

            if (!this.IsPostBack)
            {
                this.GetFuncPermissionConfig();
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["wrId"]))
                {
                    var wrObj = this.wrService.GetById(new Guid(this.Request.QueryString["wrId"]));
                    if(wrObj!=null)
                    {
                        this.lblWRNo.Text = wrObj.WRNo;
                        this.lblWRTitle.Text = wrObj.WRTitle;
                        this.lblOriginator.Text = wrObj.OriginatorName;
                        this.lblJobTitle.Text = wrObj.OriginatorJobTitle;
                        this.lblRaiseDate.Text = wrObj.RaisedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                        this.lblDateRequire.Text = wrObj.RequriedDate;
                        this.lblDescription.Text = wrObj.Description.Replace(Environment.NewLine, "<br/>");
                        this.lblScope.Text = wrObj.ScopeOfService.Replace(Environment.NewLine, "<br/>");
                        this.lblReason.Text = wrObj.Reason.Replace(Environment.NewLine, "<br/>");

                       this.txtRiskAssessment.Text = wrObj.RiskAssessment;
                        this.txtReceiveDateFromBOD.SelectedDate = wrObj.DateReceivedFromBOD;
                        this.txtPIC.Text = wrObj.PICName;
                        this.txtHODIC.Text = wrObj.HODName;
                        this.txtDatePassFncDept.SelectedDate = wrObj.DatePassFuncDept;
                        this.txtActionPlan.Text = wrObj.ActionPlan;
                        this.txtFncDeptUpdate.Text = wrObj.ActionPlan;
                        this.txtDeadline.SelectedDate = wrObj.DeadlineToComplete;
                        this.txtOverdueReason.Text = wrObj.OverdueReason;
                        this.txtRemarks.Text = wrObj.Remark;
                        this.ddlStatus.SelectedValue = wrObj.Status;

                        foreach (RadListBoxItem item in this.lbPriority.Items)
                        {
                            item.Checked = wrObj.PriotiyLevelId.GetValueOrDefault().ToString() == item.Value;
                        }
                        
                    }
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 2);
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
            var priorityLvl = this.priorityLevelService.GetAll();
            this.lbPriority.DataSource = priorityLvl;
            this.lbPriority.DataTextField = "Name";
            this.lbPriority.DataValueField = "ID";
            this.lbPriority.DataBind();

            this.btnComplete.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);
            this.btnReject.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);

            if (!string.IsNullOrEmpty(this.Request.QueryString["objAssignUserId"]))
            {
                this.ObjAssignUserId.Value = this.Request.QueryString["objAssignUserId"];
                this.ObjectType.Value = this.Request.QueryString["objType"];
                this.ObjectId.Value = this.Request.QueryString["wrId"];

                var objAssignUserId = new Guid(this.ObjAssignUserId.Value);
                var objAssignUser = this.objAssignedUserService.GetById(objAssignUserId);
                if (objAssignUser != null && objAssignUser.CanReject.GetValueOrDefault())
                {
                    this.btnReject.Visible = true;

                    var currentWFStep = this.wfSetailService.GetByCurrentStep(objAssignUser.CurrentWorkflowStepId.GetValueOrDefault());
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
            }
            else
            {
                this.btnReject.Visible = false;
                this.btnCompleteFinal.Visible = false;
                this.btnComplete.Visible = false;
            }

            this.ddlCommentFrom.SelectedValue = UserSession.Current.User.CommentGroupId.ToString();
            
            // Show hide function Control
            this.divWRAttachFile.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdAttachFile.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            this.divWRComment.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdComment.MasterTableView.GetColumn("EditColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdComment.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
            // -----------------------------------------------------------------------------------------------------------
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

        protected void grdAttachFile_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = new Guid(item.GetDataKeyValue("ID").ToString());

            this.wrAttachFileService.Delete(docId);
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var wrId = new Guid(this.Request.QueryString["wrId"]);
            var mrAttachFile = this.wrAttachFileService.GetByWR(wrId);

            this.grdAttachFile.DataSource = mrAttachFile;
        }

        protected void btnSaveAttachFile_Click(object sender, EventArgs e)
        {
            var wrId = new Guid(this.Request.QueryString["wrId"]);
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

            var targetFolder = "../../DocumentLibrary/WorkRequest";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/WorkRequest";
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

                var attachFile = new WorkRequestAttachFile()
                {
                    ID = Guid.NewGuid(),
                    WRId = wrId,
                    Filename = docFileName,
                    Extension = fileExt,
                    FilePath = serverFilePath,
                    ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                    FileSize = (double)docFile.ContentLength / 1024,
                    CreatedBy = UserSession.Current.User.Id,
                    CreatedByName = UserSession.Current.User.FullName,
                    CreatedDate = DateTime.Now,
                    Description = this.txtAttachDescription.Text
                };


                // Update final complete for MR
                var obj = this.wrService.GetById(wrId);
                if (obj != null && obj.IsWFComplete.GetValueOrDefault() && !obj.IsCompleteFinal.GetValueOrDefault())
                {
                    obj.IsCompleteFinal = true;
                    this.wrService.Update(obj);
                }
                //-----------------------------------------------------
                this.wrAttachFileService.Insert(attachFile);
            }

            this.grdAttachFile.Rebind();
        }

        protected void btnExportWR_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["wrId"]))
            {
                var priorityLvlList = this.priorityLevelService.GetAll();
                var wrId = new Guid(this.Request.QueryString["wrId"]);
                var wrObj = this.wrService.GetById(wrId);
                if (wrObj != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\WRTemplate.xlsm");

                    var dataSheet = workbook.Worksheets[0];
                    var attachImage = workbook.Worksheets[1];

                    dataSheet.Cells["D1"].PutValue(wrObj.ProjectName);
                    dataSheet.Cells["D7"].PutValue(wrObj.WRNo);
                    dataSheet.Cells["D9"].PutValue(wrObj.WRTitle);
                    dataSheet.Cells["D11"].PutValue(wrObj.OriginatorName);
                    dataSheet.Cells["I11"].PutValue(wrObj.OriginatorJobTitle);
                    dataSheet.Cells["D13"].PutValue(wrObj.RaisedDate.GetValueOrDefault());
                    dataSheet.Cells["I13"].PutValue(wrObj.RequriedDate);
                    dataSheet.Cells["B16"].PutValue(wrObj.Description);
                    dataSheet.Cells["B18"].PutValue(wrObj.ScopeOfService);
                    dataSheet.Cells["B20"].PutValue(wrObj.Reason);

                    // Fill priority checkbox
                    for (int i = 0; i < priorityLvlList.Count; i++)
                    {
                        var index = dataSheet.CheckBoxes.Add(20, 3 + (i*3), 30, 120);
                        Aspose.Cells.Drawing.CheckBox checkBox = dataSheet.CheckBoxes[index];
                        checkBox.Text = priorityLvlList[i].Name;
                        if (wrObj.PriotiyLevelId.GetValueOrDefault() == priorityLvlList[i].ID)
                        {
                            checkBox.CheckedValue = CheckValueType.Checked;
                        }
                    }
                    // -----------------------------------------------------------------------

                    // Fill Comment
                    var commentList = this.wrCommentService.GetByWR(wrObj.ID);
                    var offshoreComment = string.Empty;
                    var operationComment = string.Empty;
                    var techComment = string.Empty;

                    foreach (var comment in commentList.Where(t => t.CommentTypeId == 0).ToList())
                    {
                        offshoreComment += comment.Comment + "(Comment By: " + comment.CommentByName + ")" + Environment.NewLine;
                    }

                    foreach (var comment in commentList.Where(t => t.CommentTypeId == 1).ToList())
                    {
                        operationComment += comment.Comment + "(Comment By: " + comment.CommentByName + ")" + Environment.NewLine;
                    }

                    foreach (var comment in commentList.Where(t => t.CommentTypeId == 2).ToList())
                    {
                        techComment += comment.Comment + "(Comment By: " + comment.CommentByName + ")" + Environment.NewLine;
                    }

                    dataSheet.Cells["B25"].PutValue(offshoreComment);
                    dataSheet.Cells["G25"].PutValue(operationComment);
                    dataSheet.Cells["G27"].PutValue(techComment);

                    // ------------------------------------------------------------------------

                    // Add image
                    var imageAttachList = this.wrAttachFileService.GetByWR(wrObj.ID).Where(t => t.Extension == "png" || t.Extension == "jpg").ToList();
                    for (int i = 0; i < imageAttachList.Count; i++)
                    {
                        attachImage.Cells[4, 2 + (i * 5)].PutValue(imageAttachList[i].Description);
                        attachImage.Pictures.Add(5, 2 + (i * 5), Server.MapPath(imageAttachList[i].FilePath));
                    }
                    // --------------------------------------------------------------------------

                    var filename = Utility.RemoveSpecialCharacterFileName(wrObj.WRNo) + "_WR Form.xlsm";
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["wrId"]))
            {
                var wrId = new Guid(this.Request.QueryString["wrId"]);
                var wrObj = this.wrService.GetById(wrId);
                if (wrObj != null)
                {
                    var commentObj = new WorkRequestComment();

                    if (Session["EditingCommentId"] != null)
                    {
                        var commentId = new Guid(Session["EditingCommentId"].ToString());
                        commentObj = this.wrCommentService.GetById(commentId);
                        if (commentObj != null)
                        {
                            commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                            commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                            commentObj.Comment = this.txtComment.Text;
                            commentObj.CommentBy = UserSession.Current.User.Id;
                            commentObj.CommentByName = UserSession.Current.User.FullName;
                            commentObj.CommentDate = DateTime.Now;
                            
                            this.wrCommentService.Update(commentObj);
                        }

                        Session.Remove("EditingCommentId");
                    }
                    else
                    {
                        commentObj.ID = Guid.NewGuid();
                        commentObj.WRId = wrObj.ID;
                        commentObj.WRNo = wrObj.WRNo;
                        commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                        commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                        commentObj.Comment = this.txtComment.Text;
                        commentObj.CommentBy = UserSession.Current.User.Id;
                        commentObj.CommentByName = UserSession.Current.User.FullName;
                        commentObj.CommentDate = DateTime.Now;

                        this.wrCommentService.Insert(commentObj);
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
                var commentObj = this.wrCommentService.GetById(commentId);
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

            this.wrCommentService.Delete(commentId);
            this.grdComment.Rebind();
        }

        protected void grdComment_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var wrId = new Guid(this.Request.QueryString["wrId"]);
            var commentList = this.wrCommentService.GetByWR(wrId).OrderByDescending(t => t.CommentDate);
            this.grdComment.DataSource = commentList;
        }

        protected void btnClearComment_OnClick(object sender, EventArgs e)
        {
            
        }

        protected void btnTrackingSave_Click(object sender, EventArgs e)
        {
            var wrId = new Guid(this.Request.QueryString["wrId"]);
            var wrObj = this.wrService.GetById(wrId);
            if (wrObj != null)
            {
                wrObj.RiskAssessment = this.txtRiskAssessment.Text;
                wrObj.DateReceivedFromBOD = this.txtReceiveDateFromBOD.SelectedDate;
                wrObj.PICName = this.txtPIC.Text;
                wrObj.HODName = this.txtHODIC.Text;
                wrObj.DatePassFuncDept = this.txtDatePassFncDept.SelectedDate;
                wrObj.ActionPlan = this.txtActionPlan.Text;
                wrObj.FunctionDeptUpdate = this.txtFncDeptUpdate.Text;
                wrObj.DeadlineToComplete = this.txtDeadline.SelectedDate;
                wrObj.OverdueReason = this.txtOverdueReason.Text;
                wrObj.Remark = this.txtRemarks.Text;
                wrObj.Status = this.ddlStatus.SelectedValue;

                this.wrService.Update(wrObj);
            }
        }
    }
}