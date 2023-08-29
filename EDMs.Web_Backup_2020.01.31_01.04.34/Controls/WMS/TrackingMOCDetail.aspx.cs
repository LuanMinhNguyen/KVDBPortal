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
    public partial class TrackingMOCDetail : Page
    {
        private readonly TrackingMOCService mocService;

        private readonly TrackingMOCAttachFileService mocAttachFileService;

        private readonly TrackingMOCCommentService mocCommentService;

        private readonly ObjectAssignedUserService objAssignedUserService;

        private readonly WorkflowDetailService wfSetailService;

        private readonly UserService userService;
        private readonly RoleService roleService;

        private readonly FunctionPermissionService fncPermissionService;




        /// <summary>
        /// Initializes a new instance of the <see cref="MRDetail"/> class.
        /// </summary>
        public TrackingMOCDetail()
        {
            this.mocService = new TrackingMOCService();
            this.mocAttachFileService = new TrackingMOCAttachFileService();
            this.mocCommentService = new TrackingMOCCommentService();
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
            this.RadScriptManager2.RegisterPostBackControl(this.btnExportMOC);

            if (!this.IsPostBack)
            {
                this.GetFuncPermissionConfig();
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var obj = this.mocService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;
                        this.txtName.Text = obj.SystemName;
                        this.txtDescription.Text = obj.DescriptionOfChange;
                        this.txtDateIssue.SelectedDate = obj.DateIssued;
                        this.txtReason.Text = obj.ReasonOfChange;
                        this.txtInitRisk.Text = obj.InitRisk;
                        this.ddlInitRiskLvl.SelectedValue = obj.InitRiskLvlId.ToString();
                        this.txtMitigationAction.Text = obj.MigrationAction;
                        this.ddlMitigationRiskLvl.SelectedValue = obj.MigrationRiskLvlId.ToString();
                        this.txtAcceptableDate.Text = obj.DateAccepted;
                        this.txtTechAuth.Text = obj.TechAuthority;

                        foreach (RadTreeNode deptNode in this.rtvPIC.Nodes)
                        {
                            foreach (RadTreeNode userNode in deptNode.Nodes)
                            {
                                if (!string.IsNullOrEmpty(obj.PICId) && obj.PICId.Split(';').Contains(userNode.Value))
                                {
                                    userNode.Checked = true;
                                }
                            }
                        }

                        this.txtRemark.Text = obj.Remark;
                        this.txtClosedClari.Text = obj.ClosedClarification;
                        this.ddlStatus.SelectedValue = obj.StatusId.ToString();
                        this.txtCode.Text = obj.Code;
                        this.cbComplete.Checked = obj.IsComplete.GetValueOrDefault();
                        this.lblCreated.Text = "Updated at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + obj.CreatedByName;
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
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 4);
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
            this.divMOCAttachFile.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdAttachFile.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);

            this.divMRComment.Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdComment.MasterTableView.GetColumn("EditColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
            this.grdComment.MasterTableView.GetColumn("DeleteColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
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

        protected void grdAttachFile_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var docId = new Guid(item.GetDataKeyValue("ID").ToString());

            this.mocAttachFileService.Delete(docId);
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var mrAttachFile = this.mocAttachFileService.GetByMOC(objId);

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

            var targetFolder = "../../DocumentLibrary/MOC";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/MOC";
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

                var attachFile = new TrackingMOCAttachFile()
                {
                    ID = Guid.NewGuid(),
                    MOCId = objId,
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
                var obj = this.mocService.GetById(objId);
                if (obj != null && obj.IsWFComplete.GetValueOrDefault() && !obj.IsCompleteFinal.GetValueOrDefault())
                {
                    obj.IsCompleteFinal = true;
                    this.mocService.Update(obj);
                }
                //-----------------------------------------------------
                this.mocAttachFileService.Insert(attachFile);
            }

            this.grdAttachFile.Rebind();
        }

        protected void btnExportWR_Click(object sender, EventArgs e)
        {
            ////if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            ////{
            ////    var priorityLvlList = this.priorityLevelService.GetAll();
            ////    var objId = new Guid(this.Request.QueryString["objId"]);
            ////    var wrObj = this.mocService.GetById(objId);
            ////    if (wrObj != null)
            ////    {
            ////        var filePath = Server.MapPath("../../Exports") + @"\";
            ////        var workbook = new Workbook();
            ////        workbook.Open(filePath + @"Template\WRTemplate.xlsm");

            ////        var dataSheet = workbook.Worksheets[0];
            ////        var attachImage = workbook.Worksheets[1];

            ////        dataSheet.Cells["D1"].PutValue(wrObj.ProjectName);
            ////        dataSheet.Cells["D7"].PutValue(wrObj.WRNo);
            ////        dataSheet.Cells["D9"].PutValue(wrObj.WRTitle);
            ////        dataSheet.Cells["D11"].PutValue(wrObj.OriginatorName);
            ////        dataSheet.Cells["I11"].PutValue(wrObj.OriginatorJobTitle);
            ////        dataSheet.Cells["D13"].PutValue(wrObj.RaisedDate.GetValueOrDefault());
            ////        dataSheet.Cells["I13"].PutValue(wrObj.RequriedDate);
            ////        dataSheet.Cells["B16"].PutValue(wrObj.Description);
            ////        dataSheet.Cells["B18"].PutValue(wrObj.ScopeOfService);
            ////        dataSheet.Cells["B20"].PutValue(wrObj.Reason);

            ////        // Fill priority checkbox
            ////        for (int i = 0; i < priorityLvlList.Count; i++)
            ////        {
            ////            var index = dataSheet.CheckBoxes.Add(20, 3 + (i*3), 30, 120);
            ////            Aspose.Cells.Drawing.CheckBox checkBox = dataSheet.CheckBoxes[index];
            ////            checkBox.Text = priorityLvlList[i].Name;
            ////            if (wrObj.PriotiyLevelId.GetValueOrDefault() == priorityLvlList[i].ID)
            ////            {
            ////                checkBox.CheckedValue = CheckValueType.Checked;
            ////            }
            ////        }
            ////        // -----------------------------------------------------------------------

            ////        // Fill Comment
            ////        var commentList = this.mocCommentService.GetByWR(wrObj.ID);
            ////        var offshoreComment = string.Empty;
            ////        var operationComment = string.Empty;
            ////        var techComment = string.Empty;

            ////        foreach (var comment in commentList.Where(t => t.CommentTypeId == 0).ToList())
            ////        {
            ////            offshoreComment += comment.Comment + "(Comment By: " + comment.CommentByName + ")" + Environment.NewLine;
            ////        }

            ////        foreach (var comment in commentList.Where(t => t.CommentTypeId == 1).ToList())
            ////        {
            ////            operationComment += comment.Comment + "(Comment By: " + comment.CommentByName + ")" + Environment.NewLine;
            ////        }

            ////        foreach (var comment in commentList.Where(t => t.CommentTypeId == 2).ToList())
            ////        {
            ////            techComment += comment.Comment + "(Comment By: " + comment.CommentByName + ")" + Environment.NewLine;
            ////        }

            ////        dataSheet.Cells["B25"].PutValue(offshoreComment);
            ////        dataSheet.Cells["G25"].PutValue(operationComment);
            ////        dataSheet.Cells["G27"].PutValue(techComment);

            ////        // ------------------------------------------------------------------------

            ////        // Add image
            ////        var imageAttachList = this.mocAttachFileService.GetByWR(wrObj.ID).Where(t => t.Extension == "png" || t.Extension == "jpg").ToList();
            ////        for (int i = 0; i < imageAttachList.Count; i++)
            ////        {
            ////            attachImage.Cells[4, 2 + (i * 5)].PutValue(imageAttachList[i].Description);
            ////            attachImage.Pictures.Add(5, 2 + (i * 5), Server.MapPath(imageAttachList[i].FilePath));
            ////        }
            ////        // --------------------------------------------------------------------------

            ////        var filename = Utility.RemoveSpecialCharacterFileName(wrObj.WRNo) + "_WR Form.xlsm";
            ////        workbook.Save(filePath + filename);
            ////        this.Download_File(filePath + filename);
            ////    }
            ////}
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
                var mocObj = this.mocService.GetById(objId);
                if (mocObj != null)
                {
                    var commentObj = new TrackingMOCComment();

                    if (Session["EditingCommentId"] != null)
                    {
                        var commentId = new Guid(Session["EditingCommentId"].ToString());
                        commentObj = this.mocCommentService.GetById(commentId);
                        if (commentObj != null)
                        {
                            commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                            commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                            commentObj.Comment = this.txtComment.Text;
                            commentObj.CommentBy = UserSession.Current.User.Id;
                            commentObj.CommentByName = UserSession.Current.User.FullName;
                            commentObj.CommentDate = DateTime.Now;
                            
                            this.mocCommentService.Update(commentObj);
                        }

                        Session.Remove("EditingCommentId");
                    }
                    else
                    {
                        commentObj.ID = Guid.NewGuid();
                        commentObj.MOCId = mocObj.ID;
                        commentObj.MOCNo = mocObj.Code;
                        commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                        commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                        commentObj.Comment = this.txtComment.Text;
                        commentObj.CommentBy = UserSession.Current.User.Id;
                        commentObj.CommentByName = UserSession.Current.User.FullName;
                        commentObj.CommentDate = DateTime.Now;

                        this.mocCommentService.Insert(commentObj);
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
                var commentObj = this.mocCommentService.GetById(commentId);
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

            this.mocCommentService.Delete(commentId);
            this.grdComment.Rebind();
        }

        protected void grdComment_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var commentList = this.mocCommentService.GetByMOC(objId).OrderByDescending(t => t.CommentDate);
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
                var mocObj = mocService.GetById(new Guid(objId));
                if (mocObj != null)
                {
                    mocObj.UpdatedByName = UserSession.Current.User.FullName;
                    mocObj.UpdatedBy = UserSession.Current.User.Id;
                    mocObj.UpdatedDate = DateTime.Now;
                    this.CollectData(mocObj);

                    this.mocService.Update(mocObj);

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

        private void CollectData(TrackingMOC obj)
        {
            obj.SystemName = this.txtName.Text;
            obj.DescriptionOfChange = this.txtDescription.Text;
            obj.DateIssued = this.txtDateIssue.SelectedDate;
            obj.ReasonOfChange = this.txtReason.Text;
            obj.InitRisk = this.txtInitRisk.Text;
            obj.InitRiskLvlId = Convert.ToInt32(this.ddlInitRiskLvl.SelectedValue);
            obj.InitRiskLvlName = this.ddlInitRiskLvl.SelectedItem.Text;
            obj.MigrationAction = this.txtMitigationAction.Text;
            obj.MigrationRiskLvlId = Convert.ToInt32(this.ddlMitigationRiskLvl.SelectedValue);
            obj.MigrationRiskLvlName = this.ddlMitigationRiskLvl.SelectedItem.Text;
            obj.DateAccepted = this.txtAcceptableDate.Text;
            obj.TechAuthority = this.txtTechAuth.Text;
            obj.Remark = this.txtRemark.Text;
            obj.ClosedClarification = this.txtClosedClari.Text;
            obj.StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
            obj.StatusName = this.ddlStatus.SelectedItem.Text;

            obj.PICId = string.Empty;
            obj.PICName = string.Empty;
            foreach (RadTreeNode pic in this.rtvPIC.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.PICId += pic.Value + ";";
                obj.PICName += pic.Text + Environment.NewLine;
            }

            //obj.IsComplete = this.cbComplete.Checked;
            //obj.IsLeaf = true;
            obj.Code = this.txtCode.Text;
        }

        protected void btnExportMOC_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {

                var objId = new Guid(this.Request.QueryString["objId"]);
                var mocObj = this.mocService.GetById(objId);
                if (mocObj != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\MOCFormTemplate.xlsm");

                    var dataSheet = workbook.Worksheets[1];
                    dataSheet.Cells["D1"].PutValue("MOC No: " + mocObj.Code
                        + Environment.NewLine + "Date: " + (mocObj.DateIssued != null ? mocObj.DateIssued.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty));

                    dataSheet.Cells["B4"].PutValue(mocObj.ProjectName);
                    dataSheet.Cells["B5"].PutValue(mocObj.DescriptionOfChange);
                    dataSheet.Cells["B7"].PutValue(mocObj.ReasonOfChange);

                    var filename = Utility.RemoveSpecialCharacterFileName(mocObj.Code) + "_MOC Form.xlsm";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);
                }
            }
        }
    }
}