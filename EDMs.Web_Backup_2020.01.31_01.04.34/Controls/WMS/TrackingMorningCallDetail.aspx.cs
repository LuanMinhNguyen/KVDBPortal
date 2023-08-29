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
    public partial class TrackingMorningCallDetail : Page
    {
        private readonly TrackingMorningCallService MorningCallService;

        private readonly TrackingMorningCallAttachFileService MorningCallAttachFileService;

        private readonly TrackingMorningCallCommentService MorningCallCommentService;

        private readonly ObjectAssignedUserService objAssignedUserService;

        private readonly WorkflowDetailService wfSetailService;

        private readonly UserService userService;



        /// <summary>
        /// Initializes a new instance of the <see cref="MRDetail"/> class.
        /// </summary>
        public TrackingMorningCallDetail()
        {
            this.MorningCallService = new TrackingMorningCallService();
            this.MorningCallAttachFileService = new TrackingMorningCallAttachFileService();
            this.MorningCallCommentService = new TrackingMorningCallCommentService();
            this.objAssignedUserService = new ObjectAssignedUserService();
            this.wfSetailService = new WorkflowDetailService();
            this.userService = new UserService();
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
            //this.RadScriptManager2.RegisterPostBackControl(this.btnExportMorningCall);

            if (!this.IsPostBack)
            {
                this.LoadInitData();

                if(!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var obj = this.MorningCallService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if(obj!=null)
                    {
                        this.txtFacility.Text = obj.ProjectName;
                        this.txtDateRaised.SelectedDate = obj.DateRaised;
                        this.txtName.Text = obj.EquipmentName;
                        this.txtDescription.Text = obj.IssueDescription;
                        this.txtInitRisk.Text = obj.InitRiskAssessment;
                        this.ddlInitRiskLvl.SelectedValue = obj.InitRiskLvlId.ToString();
                        this.txtActionPlan.Text = obj.ActionPlanDescription;
                        this.ddlActionPlanRiskLvl.SelectedValue = obj.ActionPlanRiskLvlId.ToString();

                        foreach (RadListBoxItem item in this.lbPIC.Items)
                        {
                            item.Checked = !string.IsNullOrEmpty(obj.PICId) && obj.PICId.Split(';').Contains(item.Value);
                        }

                        //this.txtDeadline.SelectedDate = obj.Deadline;
                        //this.txtDeadlineHistory.SelectedDate = obj.DeadlineHistory;
                        this.txtRelativeDoc.Text = obj.RelativeDoc;
                        this.txtCurrentUpdate.Text = obj.CurrentUpdate;
                        this.txtOffshoreComment.Text = obj.OffshoreComment;
                        this.ddlStatus.SelectedValue = obj.StatusId.ToString();

                        this.txtReasonDeadlineChange.Text = obj.DeadlineReasonChange;
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

        private void LoadInitData()
        {
            var userList = this.userService.GetAll().Where(t => t.Id != 1).ToList();
            this.lbPIC.DataSource = userList;
            this.lbPIC.DataTextField = "FullNameWithPosition";
            this.lbPIC.DataValueField = "Id";
            this.lbPIC.DataBind();

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

            this.MorningCallAttachFileService.Delete(docId);
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var mrAttachFile = this.MorningCallAttachFileService.GetByMorningCall(objId);

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

            var targetFolder = "../../DocumentLibrary/MorningCall";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/MorningCall";
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

                var attachFile = new TrackingMorningCallAttachFile()
                {
                    ID = Guid.NewGuid(),
                    MorningCallId = objId,
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
                var obj = this.MorningCallService.GetById(objId);
                if (obj != null && obj.IsWFComplete.GetValueOrDefault() && !obj.IsCompleteFinal.GetValueOrDefault())
                {
                    obj.IsCompleteFinal = true;
                    this.MorningCallService.Update(obj);
                }
                //-----------------------------------------------------
                this.MorningCallAttachFileService.Insert(attachFile);
            }

            this.grdAttachFile.Rebind();
        }

        protected void btnExportWR_Click(object sender, EventArgs e)
        {
            ////if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            ////{
            ////    var priorityLvlList = this.priorityLevelService.GetAll();
            ////    var objId = new Guid(this.Request.QueryString["objId"]);
            ////    var wrObj = this.MorningCallService.GetById(objId);
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
            ////        var commentList = this.MorningCallCommentService.GetByWR(wrObj.ID);
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
            ////        var imageAttachList = this.MorningCallAttachFileService.GetByWR(wrObj.ID).Where(t => t.Extension == "png" || t.Extension == "jpg").ToList();
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
                var MorningCallObj = this.MorningCallService.GetById(objId);
                if (MorningCallObj != null)
                {
                    var commentObj = new TrackingMorningCallComment();

                    if (Session["EditingCommentId"] != null)
                    {
                        var commentId = new Guid(Session["EditingCommentId"].ToString());
                        commentObj = this.MorningCallCommentService.GetById(commentId);
                        if (commentObj != null)
                        {
                            commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                            commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                            commentObj.Comment = this.txtComment.Text;
                            commentObj.CommentBy = UserSession.Current.User.Id;
                            commentObj.CommentByName = UserSession.Current.User.FullName;
                            commentObj.CommentDate = DateTime.Now;
                            
                            this.MorningCallCommentService.Update(commentObj);
                        }

                        Session.Remove("EditingCommentId");
                    }
                    else
                    {
                        commentObj.ID = Guid.NewGuid();
                        commentObj.MorningCallId = MorningCallObj.ID;
                        commentObj.MorningCallNo = MorningCallObj.Code;
                        commentObj.CommentTypeId = Convert.ToInt32(this.ddlCommentFrom.SelectedValue);
                        commentObj.CommentTypeName = this.ddlCommentFrom.SelectedItem.Text;
                        commentObj.Comment = this.txtComment.Text;
                        commentObj.CommentBy = UserSession.Current.User.Id;
                        commentObj.CommentByName = UserSession.Current.User.FullName;
                        commentObj.CommentDate = DateTime.Now;

                        this.MorningCallCommentService.Insert(commentObj);
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
                var commentObj = this.MorningCallCommentService.GetById(commentId);
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

            this.MorningCallCommentService.Delete(commentId);
            this.grdComment.Rebind();
        }

        protected void grdComment_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var commentList = this.MorningCallCommentService.GetByMorningCall(objId).OrderByDescending(t => t.CommentDate);
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
                var MorningCallObj = MorningCallService.GetById(new Guid(objId));
                if (MorningCallObj != null)
                {
                    MorningCallObj.UpdatedByName = UserSession.Current.User.FullName;
                    MorningCallObj.UpdatedBy = UserSession.Current.User.Id;
                    MorningCallObj.UpdatedDate = DateTime.Now;
                    this.CollectData(MorningCallObj);

                    this.MorningCallService.Update(MorningCallObj);

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

        private void CollectData(TrackingMorningCall obj)
        {
            obj.DateRaised = this.txtDateRaised.SelectedDate;
            obj.EquipmentName = this.txtName.Text;
            obj.IssueDescription = this.txtDescription.Text;
            obj.InitRiskAssessment = this.txtInitRisk.Text;
            obj.InitRiskLvlId = Convert.ToInt32(this.ddlInitRiskLvl.SelectedValue);
            obj.InitRiskLvlName = this.ddlInitRiskLvl.SelectedItem.Text;
            obj.ActionPlanDescription = this.txtActionPlan.Text;
            obj.ActionPlanRiskLvlId = Convert.ToInt32(this.ddlActionPlanRiskLvl.SelectedValue);
            obj.ActionPlanRiskLvlName = this.ddlActionPlanRiskLvl.SelectedItem.Text;

            obj.PICId = string.Empty;
            obj.PICName = string.Empty;
            foreach (RadListBoxItem item in this.lbPIC.CheckedItems)
            {
                obj.PICId += item.Value + ";";
                obj.PICName += item.Text + Environment.NewLine;
            }

            //obj.Deadline = this.txtDeadline.SelectedDate;
            //obj.DeadlineHistory = this.txtDeadlineHistory.SelectedDate;
            obj.RelativeDoc = this.txtRelativeDoc.Text;
            obj.CurrentUpdate = this.txtCurrentUpdate.Text;
            obj.OffshoreComment = this.txtOffshoreComment.Text;
            obj.StatusId = Convert.ToInt32(this.ddlStatus.SelectedValue);
            obj.StatusName = this.ddlStatus.SelectedItem.Text;

            obj.DeadlineReasonChange = this.txtReasonDeadlineChange.Text;
            obj.Code = this.txtCode.Text;
        }

        ////protected void btnExportMorningCall_Click(object sender, EventArgs e)
        ////{
        ////    if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
        ////    {

        ////        var objId = new Guid(this.Request.QueryString["objId"]);
        ////        var MorningCallObj = this.MorningCallService.GetById(objId);
        ////        if (MorningCallObj != null)
        ////        {
        ////            var filePath = Server.MapPath("../../Exports") + @"\";
        ////            var workbook = new Workbook();
        ////            workbook.Open(filePath + @"Template\MorningCallFormTemplate.xlsm");

        ////            var dataSheet = workbook.Worksheets[1];
        ////            dataSheet.Cells["D1"].PutValue("MorningCall No: " + MorningCallObj.Code
        ////                + Environment.NewLine + "Date: " + (MorningCallObj.DateIssued != null ? MorningCallObj.DateIssued.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty));

        ////            dataSheet.Cells["B4"].PutValue(MorningCallObj.ProjectName);
        ////            dataSheet.Cells["B5"].PutValue(MorningCallObj.DescriptionOfChange);
        ////            dataSheet.Cells["B7"].PutValue(MorningCallObj.ReasonOfChange);

        ////            var filename = Utility.RemoveSpecialCharacterFileName(MorningCallObj.Code) + "_MorningCall Form.xlsm";
        ////            workbook.Save(filePath + filename);
        ////            this.Download_File(filePath + filename);
        ////        }
        ////    }
        ////}
    }
}