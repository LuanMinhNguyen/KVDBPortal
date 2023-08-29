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
    public partial class ContractorTransmittalChangeRequestEditForm : Page
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

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;
        private readonly ContractorTransmittalService contractorTransmittalService = new ContractorTransmittalService();

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
        public ContractorTransmittalChangeRequestEditForm()
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
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var obj = this.contractorTransmittalDocFileService.GetById(objId);
                    if (obj != null)
                    {
                        var projectObj = this.projectcodeService.GetById(obj.ProjectId.GetValueOrDefault());
                        this.LoadComboData(obj, projectObj);
                        this.LoadDocInfo(obj, projectObj);

                        //this.RefDoc.Visible = this.txtChangeRequestNo.Text.Contains("-FCR-");
                    }
                }
                else
                {
                    Session.Remove("RefDocNoChecked");

                    //var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                    //var sequence = Utilities.Utility.ReturnSequenceString(this.changeRequestService.GetCurrentSequence(year), 4);
                    //this.RegenerateChangeRequestNo(projectObj, sequence, year);
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
                var objId = new Guid(this.Request.QueryString["objId"]);
                var obj = this.contractorTransmittalDocFileService.GetById(objId);
                if (obj != null)
                {
                    var projectObj = this.projectcodeService.GetById(obj.ProjectId.GetValueOrDefault());
                    this.CollectData(ref obj, projectObj);

                    this.contractorTransmittalDocFileService.Update(obj);
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
      ///  load combo data
      /// </summary>
      /// <param name="obj"></param>
      /// <param name="projectObj"></param>
        private void LoadComboData(ContractorTransmittalDocFile obj,ProjectCode projectObj)
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
            var contractortrans = this.contractorTransmittalService.GetById(obj.TransId.GetValueOrDefault());
            var docList = this.pecc2DocumentService.GetAllProjectCode(projectObj.ID);
            this.rtvRefDocNo.DataSource = docList.Where(t=> t.GroupId== contractortrans.GroupId).OrderBy(t => t.DocNo);
            this.rtvRefDocNo.DataTextField = "DocNoWithRev";
            this.rtvRefDocNo.DataValueField = "ID";
            this.rtvRefDocNo.DataBind();
        }

        private void CollectData(ref ContractorTransmittalDocFile obj, ProjectCode projectObj)
        {
            obj.DocumentNo = this.txtChangeRequestNo.Text.Trim();
            obj.Description = this.txtDescription.Text.Trim();
            obj.AreaId = this.ddlArea.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlArea.SelectedValue)
                                        : 0;
            obj.AreaName = this.ddlArea.SelectedItem != null ?
                                        this.ddlArea.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.UnitCodeId = this.ddlUnit.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlUnit.SelectedValue)
                                        : 0;
            obj.UnitCodeName = this.ddlUnit.SelectedItem != null ?
                                        this.ddlUnit.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.ChangeRequestTypeId = this.ddlType.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlType.SelectedValue)
                                        : 0;
            obj.ChangeRequestTypeName = this.ddlType.SelectedItem != null ?
                                        this.ddlType.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;

            obj.GroupCodeId = this.ddlGroup.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlGroup.SelectedValue)
                                        : 0;
            obj.GroupCodeName = this.ddlGroup.SelectedItem != null ?
                                        this.ddlGroup.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.DocumentTitle = this.txtTitle.Text;
            obj.Year = this.txtYear.Value.ToString();
            if (!string.IsNullOrEmpty(this.txtSequentialNumber.Text.Trim()))
            {
                obj.Sequence = this.txtSequentialNumber.Text;
            }
            obj.ReasonForChange = this.txtReasonForChange.Text.Trim();
            obj.ExistingCondition = this.txtExistingCondition.Text.Trim();
            obj.ProjectId = projectObj.ID;
            obj.ProjectName = projectObj.Code;
            obj.IssuedDate = this.txtIssueDate.SelectedDate;
            obj.ChangeGradeCodeId = this.ddlChangeGradeCode.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlChangeGradeCode.SelectedValue)
                                        : 0;
            obj.ChangeGradeCode = this.ddlChangeGradeCode.SelectedItem != null ?
                                        this.ddlChangeGradeCode.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
            obj.Status = this.ddlStatus.SelectedValue;
            obj.RefDocId = string.Empty;
            obj.RefDocNo = string.Empty;

            var docListss = (List<string>)Session["RefDocNoChecked"];
            var docList = this.pecc2DocumentService.GetAllProjectCode(projectObj.ID).Where(t => docListss.Any(k => t.ID.ToString().Contains(k))).OrderBy(t => t.DocNo);
            foreach(var actionNode in docList) /*(RadTreeNode actionNode in this.rtvRefDocNo.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))*/
            {
                obj.RefDocId += actionNode.ID + ";";
                obj.RefDocNo += actionNode.DocNoWithRev + Environment.NewLine;
            }
            Session.Remove("RefDocNoChecked");
        }

        private void LoadDocInfo(ContractorTransmittalDocFile obj, ProjectCode projectObj)
        {
            this.txtChangeRequestNo.Text = obj.DocumentNo;
            this.txtTitle.Text = obj.DocumentTitle;
            this.txtDescription.Text = obj.Description;
            this.ddlArea.SelectedValue = obj.AreaId.ToString();
            if (this.ddlArea.SelectedItem != null)
            {
                var unitList = this.unitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
                this.ddlUnit.DataSource = unitList.OrderBy(t => t.Code);
                this.ddlUnit.DataTextField = "FullName";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
                this.ddlUnit.SelectedValue = obj.UnitCodeId.ToString();
            }

            this.txtProjectCode.Text = projectObj.FullName;
            this.ddlType.SelectedValue = obj.TypeId.ToString();
            this.ddlGroup.SelectedValue = obj.GroupCodeId.ToString();
            this.txtYear.Value = Convert.ToInt16(obj.Year);
            this.txtSequentialNumber.Text = obj.Sequence;
            this.txtReasonForChange.Text = obj.ReasonForChange;
            this.txtExistingCondition.Text = obj.ExistingCondition;
            this.txtIssueDate.SelectedDate = obj.IssuedDate;
            this.ddlChangeGradeCode.SelectedValue = obj.ChangeGradeCodeId.ToString();
            this.ddlStatus.SelectedValue = obj.Status;
            foreach (RadTreeNode refDocNode in this.rtvRefDocNo.Nodes)
            {
                refDocNode.Checked = !string.IsNullOrEmpty(obj.RefDocId) && obj.RefDocId.Split(';').ToList().Contains(refDocNode.Value);
            }

            if (Session["RefDocNoChecked"] == null)
            {
                var docList = new List<string>();
                docList.AddRange(obj.RefDocId.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                Session.Add("RefDocNoChecked", docList);
            }
            var docFileList = this.contractorTransmittalDocFileService.GetAllByTrans(obj.TransId.GetValueOrDefault(), 1).Select(t => t.DocumentNo).ToList();
            if (docFileList.Count > 0 && string.IsNullOrEmpty(obj.RefDocId))
            {
                var docList = new List<string>();
                foreach (RadTreeNode refDocNode in this.rtvRefDocNo.Nodes)
                {if(docFileList.Any(t => refDocNode.Text.Contains(t)))
                    {
                        refDocNode.Checked =true ;
                        docList.Add(refDocNode.Value);
                    }
                }
                Session.Add("RefDocNoChecked", docList);
            }
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

        protected void btnSearchRefDocNo_OnClick(object sender, ImageClickEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var obj = this.contractorTransmittalDocFileService.GetById(objId);
            if (obj != null)
            {
                var projectObj = this.projectcodeService.GetById(obj.ProjectId.GetValueOrDefault());
                var docList = this.pecc2DocumentService.GetAllProjectCode(projectObj.ID).Where(t =>
                    string.IsNullOrEmpty(this.txtSearchRefDocNo.Text.Trim()) ||
                    t.DocNo.IndexOf(this.txtSearchRefDocNo.Text.Trim(), StringComparison.CurrentCultureIgnoreCase) >= 0);
                this.rtvRefDocNo.DataSource = docList.OrderBy(t => t.DocNo);
                this.rtvRefDocNo.DataTextField = "DocNoWithRev";
                this.rtvRefDocNo.DataValueField = "ID";
                this.rtvRefDocNo.DataBind();

                if (Session["RefDocNoChecked"] != null)
                {
                    var docIds = (List<string>) Session["RefDocNoChecked"];
                    foreach (RadTreeNode docNode in this.rtvRefDocNo.Nodes)
                    {
                        docNode.Checked = docIds.Contains(docNode.Value);
                    }
                }
            }
        }
    }
}