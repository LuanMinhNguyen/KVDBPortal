// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly AreaService areaService;
        private readonly UnitService unitService;
        private readonly ProjectCodeService projectcodeService;
        private readonly ConfidentialityService confidentialityService;
        private readonly GroupCodeService groupCodeService;

        private readonly ChangeRequestTypeService changeRequestTypeService;
        private readonly ChangeGradeCodeService changeGradeCodeService;
        private readonly ChangeRequestReviewResultCodeService changeRequestReviewResultCodeService;
        private readonly ChangeRequestService changeRequestService;

        /// <summary>
        /// The document package service.
        /// </summary>
        private readonly PECC2DocumentsService pecc2DocumentService;

        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestEditForm()
        {
            this.groupCodeService = new GroupCodeService();
            this.userService = new UserService();
            this.pecc2DocumentService = new PECC2DocumentsService();
            this.areaService = new AreaService();
            this.confidentialityService = new ConfidentialityService();
            this.projectcodeService = new ProjectCodeService();
            this.unitService = new UnitService();
            this.changeGradeCodeService = new ChangeGradeCodeService();
            this.changeRequestReviewResultCodeService = new ChangeRequestReviewResultCodeService();
            this.changeRequestTypeService = new ChangeRequestTypeService();
            this.changeRequestService = new ChangeRequestService();
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
            if (!this.IsPostBack)
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projectObj = this.projectcodeService.GetById(projectId);
                this.LoadComboData(projectObj);
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid objId;
                    Guid.TryParse(this.Request.QueryString["objId"].ToString(),out objId);
                    var docObj = this.changeRequestService.GetById(objId);
                    if (docObj != null)
                    {

                        // this.txtProjectName.Text = docObj.ProjectFullName;

                        this.LoadDocInfo(docObj, projectObj);

                        var createdUser = this.userService.GetByID(docObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + docObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (docObj.LastUpdatedBy != null && docObj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(docObj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + docObj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    Session.Remove("RefDocNoChecked");
                    Session.Remove("DocToBeRevisedChecked");

                    var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                    var sequence = Utilities.Utility.ReturnSequenceString(this.changeRequestService.GetCurrentSequence(year), 4);
                    this.RegenerateChangeRequestNo(projectObj, sequence, year);
                    this.CreatedInfo.Visible = false;
                }
            }
        }

        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid )
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projectObj = this.projectcodeService.GetById(projectId);
                ChangeRequest changeRequestObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                    changeRequestObj = this.changeRequestService.GetById(changeRequestId);
                    if (changeRequestObj != null)
                    {
                        
                        this.CollectData(ref changeRequestObj, projectObj);

                        changeRequestObj.LastUpdatedBy = UserSession.Current.User.Id;
                        changeRequestObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        changeRequestObj.LastUpdatedDate = DateTime.Now;
                        this.changeRequestService.Update(changeRequestObj);
                    }
                }
                else
                {
                    changeRequestObj = new ChangeRequest()
                    {
                        ID = Guid.NewGuid(),
                        GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue),
                        GroupName = this.ddlGroup.SelectedItem.Text.Split(',')[0],
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        IsDelete = false,
                        IsCompleteFinal = false,
                        IsInWFProcess = false,
                        IsWFComplete = false,
                        IsAttachWorkflow = false,
                        IsSend = false,
                        IsValid = false,
                        IsReject = false,
                        ErrorMessage = "Missing Attach Referenced Documents Files.",
                        FromOrganizationId = UserSession.Current.User.Role.ContractorId,
                        IsFirst = this.ddlRefChangeRequest.SelectedIndex != 0,
                    };

                    this.CollectData(ref changeRequestObj, projectObj);
                    if (!this.changeRequestService.IsExist(changeRequestObj.Number))
                    {
                        // Create store folder
                        var physicalStoreFolder = Server.MapPath("../../DocumentLibrary/ChangeRequest/" + changeRequestObj.Number + "_" + changeRequestObj.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
                        Directory.CreateDirectory(physicalStoreFolder);


                        var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                            ? string.Empty
                            : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/ChangeRequest/" + changeRequestObj.Number + "_" + changeRequestObj.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
                        changeRequestObj.StoreFolderPath = serverStoreFolder;
                        // --------------------------------------------------------------------------

                        this.changeRequestService.Insert(changeRequestObj);
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "Change Request No. is already exist. ";
                        return;
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
            }
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
            //if (this.txtDocNo.Text.Trim().Length == 0)
            //{
            //    this.fileNameValidator.ErrorMessage = "Please enter Document Number.";
            //    this.divDocNo.Style["margin-bottom"] = "-26px;";
            //    args.IsValid = false;
            //}
            //else if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            //{
            //    Guid objId;
            //    Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);

            //    if (this._PECC2DocumentService.IsExistByDocNo(this.txtDocNumber.Text.Trim()) && objId == null)
            //    {
            //        this.fileNameValidator.ErrorMessage = "Document No. is already exist.";
            //        this.divDocNo.Style["margin-bottom"] = "-5px;";
            //        args.IsValid = false;
            //    }
            //}
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("CheckFileName"))
            {
                //var fileName = e.Argument.Split('$')[1];
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                
                
            }
        }

        private void RegenerateChangeRequestNo(ProjectCode projectObj, string sequence, int year)
        {
            
            this.txtSequentialNumber.Text = sequence;
            this.txtYear.Value = year;

            //var typeId = Convert.ToInt32(this.ddlType.SelectedValue);
            //var typeObj = this.changeRequestTypeService.GetById(typeId);

            //var groupId = Convert.ToInt32(this.ddlGroup.SelectedValue);
            //var groupObj = this.groupCodeService.GetById(groupId);

            this.txtChangeRequestNo.Text = projectObj.Code + "-";

            //if (typeObj != null)
            //{
            //    this.txtChangeRequestNo.Text += typeObj.Code + "-";
            //}

            //if (groupObj != null)
            //{
                
            //    this.txtChangeRequestNo.Text += groupObj.Code + "-";
            //}

            this.txtChangeRequestNo.Text += this.ddlType.SelectedItem.Text.Split(',')[0] + "-";

            this.txtChangeRequestNo.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-";

            this.txtChangeRequestNo.Text += this.txtYear.Value + "-";
            
            this.txtChangeRequestNo.Text += this.txtSequentialNumber.Text;
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData(ProjectCode projectObj)
        {
            this.txtProjectCode.Text = projectObj.FullName;
            this.txtIssueDate.SelectedDate = DateTime.Now;
            var areaList = this.areaService.GetAll();
            this.ddlArea.DataSource = areaList.OrderBy(t => t.Code);
            this.ddlArea.DataTextField = "FullName";
            this.ddlArea.DataValueField = "ID";
            this.ddlArea.DataBind();

            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
            }

            //Confdentiality
            var Confdentialitylist = this.confidentialityService.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault());
            this.ddlConfidentiality.DataSource = Confdentialitylist;
            this.ddlConfidentiality.DataTextField = "Code";
            this.ddlConfidentiality.DataValueField = "ID";
            this.ddlConfidentiality.DataBind();

            var changeRequestType = this.changeRequestTypeService.GetAll();
            this.ddlType.DataSource = changeRequestType;
            this.ddlType.DataTextField = "Code";
            this.ddlType.DataValueField = "ID";
            this.ddlType.DataBind();

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();

            var changeGradeCodeList = this.changeGradeCodeService.GetAll();
            this.ddlChangeGradeCode.DataSource = changeGradeCodeList.OrderBy(t => t.Code);
            this.ddlChangeGradeCode.DataTextField = "FullName";
            this.ddlChangeGradeCode.DataValueField = "ID";
            this.ddlChangeGradeCode.DataBind();

            var changeRequestReviewResult = this.changeRequestReviewResultCodeService.GetAll();
            changeRequestReviewResult.Insert(0, new ChangeRequestReviewResultCode() { ID = 0 });

            this.ddlPECC2ReviewResult.DataSource = changeRequestReviewResult.OrderBy(t => t.Code);
            this.ddlPECC2ReviewResult.DataTextField = "Code";
            this.ddlPECC2ReviewResult.DataValueField = "ID";
            this.ddlPECC2ReviewResult.DataBind();

            this.ddlOwnerReviewResult.DataSource = changeRequestReviewResult.OrderBy(t => t.Code);
            this.ddlOwnerReviewResult.DataTextField = "Code";
            this.ddlOwnerReviewResult.DataValueField = "ID";
            this.ddlOwnerReviewResult.DataBind();
            
            this.ddlReviewResult.DataSource = changeRequestReviewResult.OrderBy(t => t.Code);
            this.ddlReviewResult.DataTextField = "Code";
            this.ddlReviewResult.DataValueField = "ID";
            this.ddlReviewResult.DataBind();

            var docList = this.pecc2DocumentService.GetAllProjectCode(projectObj.ID);
            this.rtvRefDocNo.DataSource = docList.OrderBy(t => t.DocNo);
            this.rtvRefDocNo.DataTextField = "DocNoWithRev";
            this.rtvRefDocNo.DataValueField = "ID";
            this.rtvRefDocNo.DataBind();

            this.rtvDocToBeRevised.DataSource = docList.OrderBy(t => t.DocNo);
            this.rtvDocToBeRevised.DataTextField = "DocNoWithRev";
            this.rtvDocToBeRevised.DataValueField = "ID";
            this.rtvDocToBeRevised.DataBind();

            this.divReviewResult.Visible = UserSession.Current.User.Role.IsInternal.GetValueOrDefault();
            this.divClosedDate.Visible = UserSession.Current.User.Role.IsInternal.GetValueOrDefault();

            var changeRequestList = this.changeRequestService.GetAll();
            changeRequestList.Insert(0, new ChangeRequest(){ID = Guid.Empty,});
            this.ddlRefChangeRequest.DataSource = changeRequestList;
            this.ddlRefChangeRequest.DataTextField = "Number";
            this.ddlRefChangeRequest.DataValueField = "ID";
            this.ddlRefChangeRequest.DataBind();

            if (!string.IsNullOrEmpty(this.Request.QueryString["ref"]))
            {
                this.ddlRefChangeRequest.SelectedValue = this.Request.QueryString["ref"];
            }
        }

        private void CollectData(ref ChangeRequest obj, ProjectCode projectObj)
        {
            obj.Number = this.txtChangeRequestNo.Text.Trim();
            obj.Description = this.txtDescription.Text.Trim();
            obj.ConfidentialityId = this.ddlConfidentiality.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlConfidentiality.SelectedValue)
                                        : 0;
            obj.ConfidentialityName = this.ddlConfidentiality.SelectedItem != null ?
                                        this.ddlConfidentiality.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.AreaId = this.ddlArea.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlArea.SelectedValue)
                                        : 0;
            obj.AreaCode = this.ddlArea.SelectedItem != null ?
                                        this.ddlArea.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.UnitId = this.ddlUnit.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlUnit.SelectedValue)
                                        : 0;
            obj.UnitCode = this.ddlUnit.SelectedItem != null ?
                                        this.ddlUnit.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.TypeId = this.ddlType.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlType.SelectedValue)
                                        : 0;
            obj.TypeName = this.ddlType.SelectedItem != null ?
                                        this.ddlType.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.GroupId = this.ddlGroup.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlGroup.SelectedValue)
                                        : 0;
            obj.GroupName = this.ddlGroup.SelectedItem != null ?
                                        this.ddlGroup.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.Year = (int?) this.txtYear.Value;
            if (!string.IsNullOrEmpty(this.txtSequentialNumber.Text.Trim()))
            {
                obj.Sequence = Convert.ToInt32(this.txtSequentialNumber.Text);
                obj.SequentialNumber = this.txtSequentialNumber.Text.Trim();

            }
            obj.ReasonForChange = this.txtReasonForChange.Text.Trim();
            obj.ExistingCondition = this.txtExistingCondition.Text.Trim();
            obj.ProjectId = projectObj.ID;
            obj.ProjectCode = projectObj.Code;
            obj.IssuedDate = this.txtIssueDate.SelectedDate;
            obj.ClosedDate = this.txtClosedDate.SelectedDate;
            obj.ChangeGradeCodeId = this.ddlChangeGradeCode.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlChangeGradeCode.SelectedValue)
                                        : 0;
            obj.ChangeGradeCodeName = this.ddlChangeGradeCode.SelectedItem != null ?
                                        this.ddlChangeGradeCode.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.Status = this.ddlStatus.SelectedValue;
            obj.RefDocId = string.Empty;
            obj.RefDocNo = string.Empty;
            foreach (RadTreeNode actionNode in this.rtvRefDocNo.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.RefDocId += actionNode.Value + ";";
                obj.RefDocNo += actionNode.Text + Environment.NewLine;
            }

            obj.ReviewResultId = this.ddlReviewResult.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlReviewResult.SelectedValue)
                                        : 0;
            obj.ReviewResultName = this.ddlReviewResult.SelectedItem != null ?
                                        this.ddlReviewResult.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.PECC2ReviewResultId = this.ddlPECC2ReviewResult.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlPECC2ReviewResult.SelectedValue)
                                        : 0;
            obj.PECC2ReviewResultName = this.ddlPECC2ReviewResult.SelectedItem != null ?
                                        this.ddlPECC2ReviewResult.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.OwnerReviewResultId = this.ddlOwnerReviewResult.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlOwnerReviewResult.SelectedValue)
                                        : 0;
            obj.OwnerReviewResultName = this.ddlOwnerReviewResult.SelectedItem != null ?
                                        this.ddlOwnerReviewResult.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.RefChangeRequestId = new Guid(this.ddlRefChangeRequest.SelectedValue);
            obj.RefChangeRequestNumber = this.ddlRefChangeRequest.SelectedItem.Text;
            obj.DocToBeRevisedId = string.Empty;
            obj.DocToBeRevisedNo = string.Empty;
            foreach (RadTreeNode actionNode in this.rtvDocToBeRevised.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.DocToBeRevisedId += actionNode.Value + ";";
                obj.DocToBeRevisedNo += actionNode.Text + Environment.NewLine;
            }
        }

        private void LoadDocInfo(ChangeRequest obj, ProjectCode projectObj)
        {
            this.txtChangeRequestNo.Text = obj.Number;
            this.txtDescription.Text = obj.Description;
            this.ddlConfidentiality.SelectedValue = obj.ConfidentialityId.ToString();
            this.ddlArea.SelectedValue = obj.AreaId.ToString();
            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
                this.ddlUnit.SelectedValue = obj.UnitId.ToString();
            }

            this.ddlRefChangeRequest.SelectedValue = obj.RefChangeRequestId.ToString();
            this.txtProjectCode.Text = projectObj.FullName;
            this.ddlType.SelectedValue = obj.TypeId.ToString();
            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
            this.txtYear.Value = obj.Year;
            this.txtSequentialNumber.Text = obj.SequentialNumber;
            this.txtReasonForChange.Text = obj.ReasonForChange;
            this.txtExistingCondition.Text = obj.ExistingCondition;
            this.txtIssueDate.SelectedDate = obj.IssuedDate;
            this.txtClosedDate.SelectedDate = obj.ClosedDate;
            this.ddlChangeGradeCode.SelectedValue = obj.ChangeGradeCodeId.ToString();
            this.ddlStatus.SelectedValue = obj.Status;
            foreach (RadTreeNode refDocNode in this.rtvRefDocNo.Nodes)
            {
                refDocNode.Checked = !string.IsNullOrEmpty(obj.RefDocId) && obj.RefDocId.Split(';').ToList().Contains(refDocNode.Value);
            }
            this.ddlReviewResult.SelectedValue = obj.ReviewResultId.ToString();
            this.ddlPECC2ReviewResult.SelectedValue = obj.PECC2ReviewResultId.ToString();
            this.ddlOwnerReviewResult.SelectedValue = obj.OwnerReviewResultId.ToString();
            foreach (RadTreeNode reviseDocNode in this.rtvDocToBeRevised.Nodes)
            {
                reviseDocNode.Checked = !string.IsNullOrEmpty(obj.DocToBeRevisedId) && obj.DocToBeRevisedId.Split(';').ToList().Contains(reviseDocNode.Value);
            }

            if (Session["RefDocNoChecked"] == null)
            {
                var docList = new List<string>();
                docList.AddRange(obj.RefDocId.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                Session.Add("RefDocNoChecked", docList);
            } 

            //if (Session["DocToBeRevisedChecked"] == null)
            //{
            //    var docList = new List<string>();
            //    docList.AddRange(obj.DocToBeRevisedId.Split(';').Where(t => !string.IsNullOrEmpty(t)));

            //    Session.Add("DocToBeRevisedChecked", docList);
            //}
        }

        protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
            }
        }

        protected void RegenChangeRequestNo(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            this.RegenerateChangeRequestNo(projectObj, this.txtSequentialNumber.Text, (int) this.txtYear.Value.GetValueOrDefault());
        }

        protected void txtYear_OnTextChanged(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            this.RegenerateChangeRequestNo(projectObj, this.txtSequentialNumber.Text, (int)this.txtYear.Value.GetValueOrDefault());
        }

        protected void txtSequentialNumber_OnTextChanged(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            this.RegenerateChangeRequestNo(projectObj, this.txtSequentialNumber.Text, (int)this.txtYear.Value.GetValueOrDefault());
        }

        protected void rtvDocToBeRevised_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["DocToBeRevisedChecked"] == null)
            {
                var docList = new List<string>();
                if (e.Node.Checked)
                {
                    docList.Add(e.Node.Value);
                }

                Session.Add("DocToBeRevisedChecked", docList);
            }
            else
            {
                var docList = (List<string>)Session["DocToBeRevisedChecked"];
                if (e.Node.Checked)
                {
                    docList.Add(e.Node.Value);
                }
                else
                {
                    docList.Remove(e.Node.Value);
                }

                Session.Add("DocToBeRevisedChecked", docList);
            }
        }

        protected void rtvRefDocNo_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["RefDocNoChecked"] == null)
            {
                var docList = new List<string>();
                if (e.Node.Checked)
                {
                    docList.Add(e.Node.Value);
                }
                
                Session.Add("RefDocNoChecked", docList);
            }
            else
            {
                var docList = (List<string>) Session["RefDocNoChecked"];
                if (e.Node.Checked)
                {
                    docList.Add(e.Node.Value);
                }
                else
                {
                    docList.Remove(e.Node.Value);
                }

                Session.Add("RefDocNoChecked", docList);
            }
        }

        protected void btnSearchDocRevised_OnClick(object sender, ImageClickEventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);

            var docList = this.pecc2DocumentService.GetAllProjectCode(projectObj.ID)
                    .Where(t => string.IsNullOrEmpty(this.txtSearchDocRevised.Text.Trim()) || t.DocNo.ToUpper().Contains(this.txtSearchDocRevised.Text.Trim().ToUpper()));
            this.rtvDocToBeRevised.DataSource = docList.OrderBy(t => t.DocNo);
            this.rtvDocToBeRevised.DataTextField = "DocNoWithRev";
            this.rtvDocToBeRevised.DataValueField = "ID";
            this.rtvDocToBeRevised.DataBind();

            if (Session["DocToBeRevisedChecked"] != null)
            {
                var docIds = (List<string>)Session["DocToBeRevisedChecked"];
                foreach (RadTreeNode docNode in this.rtvDocToBeRevised.Nodes)
                {
                    docNode.Checked = docIds.Contains(docNode.Value);
                }
            }
        }

        protected void btnSearchRefDocNo_OnClick(object sender, ImageClickEventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            var docList = this.pecc2DocumentService.GetAllProjectCode(projectObj.ID).Where(t => string.IsNullOrEmpty(this.txtSearchDocRevised.Text.Trim()) || t.DocNo.ToUpper().Contains(this.txtSearchRefDocNo.Text.Trim().ToUpper()));
            this.rtvRefDocNo.DataSource = docList.OrderBy(t => t.DocNo);
            this.rtvRefDocNo.DataTextField = "DocNoWithRev";
            this.rtvRefDocNo.DataValueField = "ID";
            this.rtvRefDocNo.DataBind();

            if (Session["RefDocNoChecked"] != null)
            {
                var docIds = (List<string>)Session["RefDocNoChecked"];
                foreach (RadTreeNode docNode in this.rtvRefDocNo.Nodes)
                {
                    docNode.Checked = docIds.Contains(docNode.Value);
                }
            }
        }
    }
}