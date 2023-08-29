// --------------------------------------------------------------------------------------------------------------------
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
    public partial class TrackingBreakdownReportDetail : Page
    {
        private readonly TrackingBreakdownReportService BreakdownReportService;

        private readonly TrackingBreakdownReportAttachFileService BreakdownReportAttachFileService;

        private readonly TrackingBreakdownReportCommentService BreakdownReportCommentService;

        private readonly ObjectAssignedUserService objAssignedUserService;

        private readonly WorkflowDetailService wfSetailService;

        private readonly UserService userService;

        private readonly RoleService roleService;

        private readonly FunctionPermissionService fncPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MRDetail"/> class.
        /// </summary>
        public TrackingBreakdownReportDetail()
        {
            this.BreakdownReportService = new TrackingBreakdownReportService();
            this.BreakdownReportAttachFileService = new TrackingBreakdownReportAttachFileService();
            this.BreakdownReportCommentService = new TrackingBreakdownReportCommentService();
            this.objAssignedUserService = new ObjectAssignedUserService();
            this.wfSetailService = new WorkflowDetailService();
            this.userService = new UserService();
            this.roleService = new RoleService();
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
            this.RadScriptManager2.RegisterPostBackControl(this.btnExportBR);

            if (!this.IsPostBack)
            {
                this.GetFuncPermissionConfig();
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var obj = this.BreakdownReportService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;

                        this.txtBreakdownDate.SelectedDate = obj.BrekdownDate;
                        this.txtBreakdownSystemName.Text = obj.BreakdownSystemName;
                        this.txtTagno.Text = obj.TagNo;
                        this.txtSystemName.Text = obj.SystemName;
                        this.ddlPriority.SelectedValue = obj.Priority;
                        this.txtCauseGroup.Text = obj.CauseGroup;
                        this.txtDescription.Text = obj.Description;
                        this.txtFalureDuplication.Text = obj.FailureDuplication;
                        this.txtRootCause.Text = obj.RootCause;
                        this.txtProposedAction.Text = obj.ProposedAction;
                        this.txtLesson.Text = obj.Lesson;
                        this.txtUnplannedWoNo.Text = obj.UnplannedWoNo;
                        foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.PICIds) && obj.PICIds.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        this.txtPICDeadline.Text = obj.PICDeadline;
                        this.txtPicStatus.Text = obj.PICStatus;
                        this.txtCurrentStatus.Text = obj.CurrentStatus;
                        this.txtMRWRItem.Text = obj.MRWRItem;
                        this.txtCommentDetail.Text = obj.Comment;
                        this.ddlOpen.SelectedValue = obj.Status;
                        this.txtCost.Value = obj.Cost;

                        this.txtReasonDeadlineChange.Text = obj.DeadlineReasonChange;
                        this.txtCode.Text = obj.Code;

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + obj.CreatedByName;
                        if (obj.UpdatedBy != null && obj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            this.lblUpdated.Text = "Last modified at " + obj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + obj.UpdatedByName;
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }

                    }
                }
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 5);
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

            this.btnComplete.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);
            this.btnReject.Visible = !string.IsNullOrEmpty(this.Request.QueryString["todolist"]);

            if (!string.IsNullOrEmpty(this.Request.QueryString["objAssignUserId"]))
            {
                this.ObjAssignUserId.Value = this.Request.QueryString["objAssignUserId"];
                this.ObjectType.Value = this.Request.QueryString["objType"];
                this.ObjectId.Value = this.Request.QueryString["objId"];

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

            this.ddlCommentFrom.SelectedValue = UserSession.Current.User.CommentGroupId.ToString();

            // Show hide function Control
            this.divBRAttachFile.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdAttachFile.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            this.divBRComment.Visible = Convert.ToBoolean(this.IsUpdate.Value);
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

            this.BreakdownReportAttachFileService.Delete(docId);
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var mrAttachFile = this.BreakdownReportAttachFileService.GetByBreakdownReport(objId);

            this.grdAttachFile.DataSource = mrAttachFile;
        }

        protected void btnSaveAttachFile_Click(object sender, EventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
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

            var targetFolder = "../../DocumentLibrary/BreakdownReport";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/BreakdownReport";
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

                var attachFile = new TrackingBreakdownReportAttachFile()
                {
                    ID = Guid.NewGuid(),
                    BreakdownReportId = objId,
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
                var obj = this.BreakdownReportService.GetById(objId);
                if (obj != null && obj.IsWFComplete.GetValueOrDefault() && !obj.IsCompleteFinal.GetValueOrDefault())
                {
                    obj.IsCompleteFinal = true;
                    this.BreakdownReportService.Update(obj);
                }
                //-----------------------------------------------------
                this.BreakdownReportAttachFileService.Insert(attachFile);
            }

            this.grdAttachFile.Rebind();
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var BreakdownReportObj = this.BreakdownReportService.GetById(objId);
                if (BreakdownReportObj != null)
                {
                    var commentObj = new TrackingBreakdownReportComment();

                    if (Session["EditingCommentId"] != null)
                    {
                        var commentId = new Guid(Session["EditingCommentId"].ToString());
                        commentObj = this.BreakdownReportCommentService.GetById(commentId);
                        if (commentObj != null)
                        {
                            commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                            commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                            commentObj.Comment = this.txtComment.Text;
                            commentObj.CommentBy = UserSession.Current.User.Id;
                            commentObj.CommentByName = UserSession.Current.User.FullName;
                            commentObj.CommentDate = DateTime.Now;
                            
                            this.BreakdownReportCommentService.Update(commentObj);
                        }

                        Session.Remove("EditingCommentId");
                    }
                    else
                    {
                        commentObj.ID = Guid.NewGuid();
                        commentObj.BreakdownReportId = BreakdownReportObj.ID;
                        commentObj.BreakdownReportNo = BreakdownReportObj.Code;
                        commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                        commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                        commentObj.Comment = this.txtComment.Text;
                        commentObj.CommentBy = UserSession.Current.User.Id;
                        commentObj.CommentByName = UserSession.Current.User.FullName;
                        commentObj.CommentDate = DateTime.Now;

                        this.BreakdownReportCommentService.Insert(commentObj);
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
                var commentObj = this.BreakdownReportCommentService.GetById(commentId);
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

            this.BreakdownReportCommentService.Delete(commentId);
            this.grdComment.Rebind();
        }

        protected void grdComment_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var commentList = this.BreakdownReportCommentService.GetByBreakdownReport(objId).OrderByDescending(t => t.CommentDate);
            this.grdComment.DataSource = commentList;
        }

        protected void btnClearComment_OnClick(object sender, EventArgs e)
        {
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var objId = this.Request.QueryString["objId"];
                var BreakdownReportObj = BreakdownReportService.GetById(new Guid(objId));
                if (BreakdownReportObj != null)
                {
                    BreakdownReportObj.UpdatedByName = UserSession.Current.User.FullName;
                    BreakdownReportObj.UpdatedBy = UserSession.Current.User.Id;
                    BreakdownReportObj.UpdatedDate = DateTime.Now;
                    this.CollectData(BreakdownReportObj);

                    this.BreakdownReportService.Update(BreakdownReportObj);

                    ////try
                    ////{
                    ////    this.SendNotification(obj);
                    ////}
                    ////catch (Exception)
                    ////{
                    ////    // ignored
                    ////}
                }
            }
        }

        private void CollectData(TrackingBreakdownReport obj)
        {
            obj.BrekdownDate = this.txtBreakdownDate.SelectedDate;
            obj.BreakdownSystemName = this.txtBreakdownSystemName.Text;
            obj.TagNo = this.txtTagno.Text;
            obj.SystemName = this.txtSystemName.Text;
            obj.Priority = this.ddlPriority.SelectedValue;
            obj.CauseGroup = this.txtCauseGroup.Text;
            obj.Description = this.txtDescription.Text;
            obj.FailureDuplication = this.txtFalureDuplication.Text;
            obj.RootCause = this.txtRootCause.Text;
            obj.ProposedAction = this.txtProposedAction.Text;
            obj.Lesson = this.txtLesson.Text;
            obj.UnplannedWoNo = this.txtUnplannedWoNo.Text;

            obj.PICIds = string.Empty;
            obj.PICName = string.Empty;
            foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.PICIds += pic.Value + ";";
                obj.PICName += pic.Text + Environment.NewLine;
            }

            obj.PICDeadline = this.txtPICDeadline.Text;
            obj.PICStatus = this.txtPicStatus.Text;
            obj.CurrentStatus = this.txtCurrentStatus.Text;
            obj.MRWRItem = this.txtMRWRItem.Text;
            obj.Comment = this.txtCommentDetail.Text;
            obj.Status = this.ddlOpen.SelectedValue;
            obj.Cost = this.txtCost.Value;

            obj.DeadlineReasonChange = this.txtReasonDeadlineChange.Text;
            obj.Code = this.txtCode.Text;
        }

        protected void btnExportBR_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                
                var objId = new Guid(this.Request.QueryString["objId"]);
                var brObj = this.BreakdownReportService.GetById(objId);
                if (brObj != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\BRFormTemplate.xlsm");

                    var dataSheet = workbook.Worksheets[0];
                    var createdUser = this.userService.GetByID(brObj.CreatedBy.GetValueOrDefault());
                    dataSheet.Cells["C1"].PutValue(dataSheet.Cells["C1"].Value.ToString().Replace("<FacilityName>", brObj.ProjectName));
                    dataSheet.Cells["B4"].PutValue(brObj.ProjectName);

                    dataSheet.Cells["H4"].PutValue(brObj.Code);
                    dataSheet.Cells["B5"].PutValue(brObj.BrekdownDate.GetValueOrDefault().ToString("HH:mm"));
                    dataSheet.Cells["D5"].PutValue(brObj.BrekdownDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    dataSheet.Cells["F5"].PutValue(createdUser.FullName);
                    dataSheet.Cells["H5"].PutValue(createdUser.Role.NameWithLocation);
                    dataSheet.Cells["C6"].PutValue(brObj.TagNo);
                    dataSheet.Cells["G6"].PutValue(brObj.BreakdownSystemName);
                    dataSheet.Cells["A12"].PutValue(brObj.Description);

                    var filename = Utility.RemoveSpecialCharacterFileName(brObj.Code) + "_Breakdown Report Form.xlsm";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);
                }
            }
        }

        
    }
}