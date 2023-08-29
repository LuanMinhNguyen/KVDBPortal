// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class RecipientPage : Page
    {

        private readonly WorkflowStepService wfStepService = new WorkflowStepService();
        private readonly WorkflowService wfService = new WorkflowService();
        private readonly WorkflowDetailService wfDetailService = new WorkflowDetailService();
        private readonly ScopeProjectService projectService = new ScopeProjectService();

        private readonly RoleService roleService = new RoleService();
        private readonly UserService userService = new UserService();

        private readonly DistributionMatrixTypeService distributionMatrixTypeService =
            new DistributionMatrixTypeService();

        private readonly DistributionMatrixService distributionMatrixService = new DistributionMatrixService();

        private readonly TitleService titleService = new TitleService();
        private readonly TemplateWorkflowDetailService templateWFDetailService = new TemplateWorkflowDetailService();
        private readonly ObjectWorkflowDetailService ObjWFDetailService = new ObjectWorkflowDetailService();
        private readonly CustomizeWorkflowDetailService customizeWorkflowDetailService = new CustomizeWorkflowDetailService();

        private int WorkflowDetailId
        {
            get { return Convert.ToInt32(this.Request.QueryString["wfdId"]); }
        }

        private Data.Entities.WorkflowDetail WorkflowDetailObj
        {
            get { return this.wfDetailService.GetById(this.WorkflowDetailId); }
        }

        private Data.Entities.TemplateWorkflowDetail TemplteWorkflowDetailObj
        {
            get { return this.templateWFDetailService.GetById(this.WorkflowDetailId); }
        }

        private CustomizeWorkflowDetail CustomizeWorkflowDetailObj
        {
            get { return this.customizeWorkflowDetailService.GetById(this.WorkflowDetailId); }
        }

        private Data.Entities.ObjectWorkflowDetail ObjtWFDetail
        {
            get { return this.ObjWFDetailService.GetById(this.WorkflowDetailId); }
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
                // Clear Session of node checked
                Session.Remove("ApproveUserChecked");
                Session.Remove("ReviewUserChecked");
                Session.Remove("ConsolidateUserChecked");
                Session.Remove("InfoUserChecked");
                Session.Remove("MatrixChecked");
                // -------------------------------------------

                this.LoadComboData();
                if (!string.IsNullOrEmpty(Request.QueryString["customize"]))
                {
                    if (!string.IsNullOrEmpty(this.Request.QueryString["wfdId"]))
                    {
                        if (this.CustomizeWorkflowDetailObj != null)
                        {
                            this.LoadListBoxDataCustomize(this.CustomizeWorkflowDetailObj);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["EditObject"]))
                {
                    if (!string.IsNullOrEmpty(this.Request.QueryString["wfdId"]))
                    {
                        if (this.ObjtWFDetail != null)
                        {
                            this.LoadListBoxDataEdit(this.ObjtWFDetail);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.Request.QueryString["wfdId"]))
                    {
                        if (this.WorkflowDetailObj != null)
                        {
                            this.LoadListBoxData(this.WorkflowDetailObj);
                        }
                    }
                }

            }
        }

        private void LoadListBoxDataCustomize(CustomizeWorkflowDetail workflowDetailObj)
        {
            try
            {
                foreach (RadTreeNode deptNode in this.rtvApproveUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ApproveUserIds) &&
                            workflowDetailObj.ApproveUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvReviewUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ReviewUserIds) &&
                            workflowDetailObj.ReviewUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvConsolidateUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ConsolidateUserIds) &&
                            workflowDetailObj.ConsolidateUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvInfoUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.InformationOnlyUserIDs) &&
                            workflowDetailObj.InformationOnlyUserIDs.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvMatrix.Nodes)
                {
                    if (!string.IsNullOrEmpty(workflowDetailObj.DistributionMatrixIDs) &&
                        workflowDetailObj.DistributionMatrixIDs.Split(';').Contains(deptNode.Value))
                    {
                        deptNode.Checked = true;
                    }
                }

                // Build session data of node checked in radtreeview
                if (Session["ApproveUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ApproveUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ApproveUserChecked", userList);
                }

                if (Session["ReviewUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ReviewUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ReviewUserChecked", userList);
                }

                if (Session["ConsolidateUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ConsolidateUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ConsolidateUserChecked", userList);
                }

                if (Session["InfoUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.InformationOnlyUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("InfoUserChecked", userList);
                }

                if (Session["MatrixChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.DistributionMatrixIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("MatrixChecked", userList);
                }
                // ------------------------------------------------------------------------------------------------------
            }
            catch
            {
            }
        }

        private void LoadListBoxData(WorkflowDetail workflowDetailObj)
        {
            try
            {
                foreach (RadTreeNode deptNode in this.rtvApproveUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ApproveUserIds) &&
                            workflowDetailObj.ApproveUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvReviewUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ReviewUserIds) &&
                            workflowDetailObj.ReviewUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvConsolidateUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ConsolidateUserIds) &&
                            workflowDetailObj.ConsolidateUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvInfoUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.InformationOnlyUserIDs) &&
                            workflowDetailObj.InformationOnlyUserIDs.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode matrixNode in this.rtvMatrix.Nodes)
                {
                    if (!string.IsNullOrEmpty(workflowDetailObj.DistributionMatrixIDs) &&
                            workflowDetailObj.DistributionMatrixIDs.Split(';').Contains(matrixNode.Value))
                    {
                        matrixNode.Checked = true;
                    }
                }

                // Build session data of node checked in radtreeview
                if (Session["ApproveUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ApproveUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ApproveUserChecked", userList);
                }

                if (Session["ReviewUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ReviewUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ReviewUserChecked", userList);
                }

                if (Session["ConsolidateUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ConsolidateUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ConsolidateUserChecked", userList);
                }

                if (Session["InfoUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.InformationOnlyUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("InfoUserChecked", userList);
                }

                if (Session["MatrixChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.DistributionMatrixIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("MatrixChecked", userList);
                }
                // ------------------------------------------------------------------------------------------------------
            }
            catch
            {
            }
        }

        private void LoadListBoxDataTempalte(TemplateWorkflowDetail workflowDetailObj)
        {
            try
            {
                foreach (RadTreeNode deptNode in this.rtvApproveUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ApproveUserIds) &&
                            workflowDetailObj.ApproveUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvReviewUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ReviewUserIds) &&
                            workflowDetailObj.ReviewUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvConsolidateUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.CommentUserIds) &&
                            workflowDetailObj.CommentUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvInfoUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.InformationOnlyUserIDs) &&
                            workflowDetailObj.InformationOnlyUserIDs.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                //foreach (RadTreeNode deptNode in this.rtvManagementUser.Nodes)
                //{
                //    foreach (RadTreeNode userNode in deptNode.Nodes)
                //    {
                //        if (!string.IsNullOrEmpty(workflowDetailObj.ManagementUserIds) && workflowDetailObj.ManagementUserIds.Split(';').Contains(userNode.Value))
                //        {
                //            userNode.Checked = true;
                //        }
                //    }
                //}

                foreach (RadTreeNode deptNode in this.rtvMatrix.Nodes)
                {
                    if (!string.IsNullOrEmpty(workflowDetailObj.DistributionMatrixIDs) &&
                        workflowDetailObj.DistributionMatrixIDs.Split(';').Contains(deptNode.Value))
                    {
                        deptNode.Checked = true;
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadListBoxDataEdit(ObjectWorkflowDetail workflowDetailObj)
        {
            try
            {
                foreach (RadTreeNode deptNode in this.rtvApproveUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ApproveUserIds) &&
                            workflowDetailObj.ApproveUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvReviewUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ReviewUserIds) &&
                            workflowDetailObj.ReviewUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvConsolidateUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ConsolidateUserIds) &&
                            workflowDetailObj.ConsolidateUserIds.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode deptNode in this.rtvInfoUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.InformationOnlyUserIDs) &&
                            workflowDetailObj.InformationOnlyUserIDs.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }
                
                foreach (RadTreeNode deptNode in this.rtvMatrix.Nodes)
                {
                    if (!string.IsNullOrEmpty(workflowDetailObj.DistributionMatrixIDs) &&
                        workflowDetailObj.DistributionMatrixIDs.Split(';').Contains(deptNode.Value))
                    {
                        deptNode.Checked = true;
                    }
                }

                // Build session data of node checked in radtreeview
                if (Session["ApproveUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ApproveUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ApproveUserChecked", userList);
                }

                if (Session["ReviewUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ReviewUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ReviewUserChecked", userList);
                }

                if (Session["ConsolidateUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ConsolidateUserIds.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ConsolidateUserChecked", userList);
                }

                if (Session["InfoUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.InformationOnlyUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("InfoUserChecked", userList);
                }

                if (Session["MatrixChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.DistributionMatrixIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("MatrixChecked", userList);
                }
                // ------------------------------------------------------------------------------------------------------
            }
            catch
            {
            }
        }

        private void LoadComboData()
        {
            try
            {

                var userList = this.userService.GetAll();
                var roleList = userList.Select(t => t.RoleId).Distinct().Select(t => this.roleService.GetByID(t.GetValueOrDefault())).OrderBy(t => t.IsInternal).ThenBy(t => t.Name).ToList();
                //roleList.Insert(0, new Role() { Id=0, Name=" "});
                //var roleList = this.roleService.GetAll(false).OrderBy(t => t.IsInternal).ThenBy(t => t.Name).ToList();//.Where(t => t.TypeId == 1);
                //foreach (var role in roleList)
                //{
                //    var detpNode = new RadTreeNode(role.Name);
                //    var userList1 = this.userService.GetAllByRoleId(role.Id).OrderBy(t => t.UserNameWithFullName);
                //    foreach (var user in userList1)
                //    {
                //        detpNode.Nodes.Add(new RadTreeNode(user.UserNameWithFullName, user.Id.ToString()));
                //    }

                //    this.rtvManagementUser.Nodes.Add(detpNode);
                //}

                foreach (var role in roleList)
                {
                    var detpNode = new RadTreeNode(role.Name);
                    var userList1 = this.userService.GetAllByRoleId(role.Id).OrderBy(t => t.UserNameWithFullName);
                    foreach (var user in userList1)
                    {
                        detpNode.Nodes.Add(new RadTreeNode(user.UserNameWithFullName, user.Id.ToString()));
                    }

                    this.rtvApproveUser.Nodes.Add(detpNode);
                }

                foreach (var role in roleList)
                {
                    var detpNode = new RadTreeNode(role.Name);
                    var userList1 = this.userService.GetAllByRoleId(role.Id).OrderBy(t => t.UserNameWithFullName);
                    foreach (var user in userList1)
                    {
                        detpNode.Nodes.Add(new RadTreeNode(user.UserNameWithFullName, user.Id.ToString()));
                    }

                    this.rtvReviewUser.Nodes.Add(detpNode);
                }

                foreach (var role in roleList)
                {
                    var detpNode = new RadTreeNode(role.Name);
                    var userList1 = this.userService.GetAllByRoleId(role.Id).OrderBy(t => t.UserNameWithFullName);
                    foreach (var user in userList1)
                    {
                        detpNode.Nodes.Add(new RadTreeNode(user.UserNameWithFullName, user.Id.ToString()));
                    }

                    this.rtvConsolidateUser.Nodes.Add(detpNode);
                }

                foreach (var role in roleList)
                {
                    var detpNode = new RadTreeNode(role.Name);
                    var userList1 = this.userService.GetAllByRoleId(role.Id).OrderBy(t => t.UserNameWithFullName);
                    foreach (var user in userList1)
                    {
                        detpNode.Nodes.Add(new RadTreeNode(user.UserNameWithFullName, user.Id.ToString()));
                    }

                    this.rtvInfoUser.Nodes.Add(detpNode);
                }

                var matrixTypeList = this.distributionMatrixService.GetAll().OrderBy(t=> t.Name);
                foreach (var matrixType in matrixTypeList)
                {
                    var matrixTypeNode = new RadTreeNode(matrixType.Name, matrixType.ID.ToString());
                    ////var matrixList = this.distributionMatrixService.GetAllByType(matrixType.ID).OrderBy(t => t.Name);
                    ////foreach (var matrix in matrixList)
                    ////{
                    ////    matrixTypeNode.Nodes.Add(new RadTreeNode(matrix.Name, matrix.ID.ToString()));
                    ////}

                    this.rtvMatrix.Nodes.Add(matrixTypeNode);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["customize"]))
                {
                    this.divApproveUser.Visible = this.CustomizeWorkflowDetailObj.ActionApplyCode.Split(';').Contains("A");
                    this.divInformationUser.Visible = this.CustomizeWorkflowDetailObj.ActionApplyCode.Split(';').Contains("I");
                    this.divReviewUser.Visible = this.CustomizeWorkflowDetailObj.ActionApplyCode.Split(';').Contains("R");
                    this.divConsolidateUser.Visible = this.CustomizeWorkflowDetailObj.ActionApplyCode.Split(';').Contains("C");
                }
                else
                {
                    this.divApproveUser.Visible = this.WorkflowDetailObj.ActionApplyCode.Split(';').Contains("A");
                    this.divInformationUser.Visible = this.WorkflowDetailObj.ActionApplyCode.Split(';').Contains("I");
                    this.divReviewUser.Visible = this.WorkflowDetailObj.ActionApplyCode.Split(';').Contains("R");
                    this.divConsolidateUser.Visible = this.WorkflowDetailObj.ActionApplyCode.Split(';').Contains("C");
                }
                
            }
            catch
            {
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["customize"]))
                {
                    this.CustomizeWorkflowDetailObj.ApproveUserIds = string.Empty;
                    this.CustomizeWorkflowDetailObj.ReviewUserIds = string.Empty;
                    this.CustomizeWorkflowDetailObj.ConsolidateUserIds = string.Empty;
                    this.CustomizeWorkflowDetailObj.InformationOnlyUserIDs = string.Empty;
                    this.CustomizeWorkflowDetailObj.ManagementUserIds = string.Empty;
                    this.CustomizeWorkflowDetailObj.DistributionMatrixIDs = string.Empty;
                    this.CustomizeWorkflowDetailObj.Recipients = string.Empty;

                    foreach (
                        RadTreeNode approveUser in
                            this.rtvApproveUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.CustomizeWorkflowDetailObj.ApproveUserIds += approveUser.Value + ";";
                        this.CustomizeWorkflowDetailObj.Recipients += "A - " + approveUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode reviewUser in
                            this.rtvReviewUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.CustomizeWorkflowDetailObj.ReviewUserIds += reviewUser.Value + ";";
                        this.CustomizeWorkflowDetailObj.Recipients += "R - " + reviewUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode consolidateUser in
                            this.rtvConsolidateUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.CustomizeWorkflowDetailObj.ConsolidateUserIds += consolidateUser.Value + ";";
                        this.CustomizeWorkflowDetailObj.Recipients += "C - " + consolidateUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode infoUser in
                            this.rtvInfoUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.CustomizeWorkflowDetailObj.InformationOnlyUserIDs += infoUser.Value + ";";
                        this.CustomizeWorkflowDetailObj.Recipients += "I - " + infoUser.Text + "</br>";
                    }

                    foreach ( RadTreeNode matrix in this.rtvMatrix.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        var distributionMatrixObj = this.distributionMatrixService.GetById(Convert.ToInt32(matrix.Value));
                        this.CustomizeWorkflowDetailObj.DistributionMatrixIDs += matrix.Value + ";";
                        this.CustomizeWorkflowDetailObj.Recipients += "Matrix - " + matrix.Text + "</br>";
                    }

                    this.customizeWorkflowDetailService.Update(this.CustomizeWorkflowDetailObj);
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["EditObject"]) &&
                         ObjtWFDetail.CanEdit.GetValueOrDefault())
                {
                    this.ObjtWFDetail.ApproveUserIds = string.Empty;
                    this.ObjtWFDetail.ReviewUserIds = string.Empty;
                    this.ObjtWFDetail.ConsolidateUserIds = string.Empty;
                    this.ObjtWFDetail.InformationOnlyUserIDs = string.Empty;
                    this.ObjtWFDetail.ManagementUserIds = string.Empty;
                    this.ObjtWFDetail.DistributionMatrixIDs = string.Empty;
                    this.ObjtWFDetail.Recipients = string.Empty;

                    foreach (
                        RadTreeNode approveUser in
                            this.rtvApproveUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.ObjtWFDetail.ApproveUserIds += approveUser.Value + ";";
                        this.ObjtWFDetail.Recipients += "A - " + approveUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode reviewUser in
                            this.rtvReviewUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.ObjtWFDetail.ReviewUserIds += reviewUser.Value + ";";
                        this.ObjtWFDetail.Recipients += "R - " + reviewUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode consolidateUser in
                            this.rtvConsolidateUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.ObjtWFDetail.ConsolidateUserIds += consolidateUser.Value + ";";
                        this.ObjtWFDetail.Recipients += "C - " + consolidateUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode infoUser in
                            this.rtvInfoUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.ObjtWFDetail.InformationOnlyUserIDs += infoUser.Value + ";";
                        this.ObjtWFDetail.Recipients += "I - " + infoUser.Text + "</br>";
                    }

                    //foreach (RadTreeNode managementUser in this.rtvManagementUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    //{
                    //    this.ObjtWFDetail.ManagementUserIds += managementUser.Value + ";";
                    //    this.ObjtWFDetail.Recipients += "M - " + managementUser.Text + "</br>";
                    //}

                    foreach (
                        RadTreeNode matrix in this.rtvMatrix.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value))
                        )
                    {
                        var distributionMatrixObj =
                            this.distributionMatrixService.GetById(Convert.ToInt32(matrix.Value));
                        this.ObjtWFDetail.DistributionMatrixIDs += matrix.Value + ";";
                        this.ObjtWFDetail.Recipients += "Matrix - " + matrix.Text + "</br>";
                    }

                    this.ObjWFDetailService.Update(this.ObjtWFDetail);

                    List<int> ListObjWFDetail = new List<int>();
                    if (Session["ListEdit"] != null)
                    {
                        ListObjWFDetail = (List<int>) Session["ListEdit"];
                        ListObjWFDetail.Add(this.ObjtWFDetail.ID);
                        Session["ListEdit"] = ListObjWFDetail;
                    }
                    else
                    {
                        ListObjWFDetail.Add(this.ObjtWFDetail.ID);
                        Session["ListEdit"] = ListObjWFDetail;
                    }
                }
                else
                {

                    this.WorkflowDetailObj.ApproveUserIds = string.Empty;
                    this.WorkflowDetailObj.ReviewUserIds = string.Empty;
                    this.WorkflowDetailObj.ConsolidateUserIds = string.Empty;
                    this.WorkflowDetailObj.InformationOnlyUserIDs = string.Empty;
                    this.WorkflowDetailObj.ManagementUserIds = string.Empty;
                    this.WorkflowDetailObj.DistributionMatrixIDs = string.Empty;
                    this.WorkflowDetailObj.Recipients = string.Empty;

                    foreach (
                        RadTreeNode approveUser in
                            this.rtvApproveUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.WorkflowDetailObj.ApproveUserIds += approveUser.Value + ";";
                        this.WorkflowDetailObj.Recipients += "A - " + approveUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode reviewUser in
                            this.rtvReviewUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.WorkflowDetailObj.ReviewUserIds += reviewUser.Value + ";";
                        this.WorkflowDetailObj.Recipients += "R - " + reviewUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode consolidateUser in
                            this.rtvConsolidateUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.WorkflowDetailObj.ConsolidateUserIds += consolidateUser.Value + ";";
                        this.WorkflowDetailObj.Recipients += "C - " + consolidateUser.Text + "</br>";
                    }

                    foreach (
                        RadTreeNode infoUser in
                            this.rtvInfoUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        this.WorkflowDetailObj.InformationOnlyUserIDs += infoUser.Value + ";";
                        this.WorkflowDetailObj.Recipients += "I - " + infoUser.Text + "</br>";
                    }

                    //foreach (RadTreeNode managementUser in this.rtvManagementUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    //{
                    //    this.WorkflowDetailObj.ManagementUserIds += managementUser.Value + ";";
                    //    this.WorkflowDetailObj.Recipients += "M - " + managementUser.Text + "</br>";
                    //}

                    foreach (
                        RadTreeNode matrix in this.rtvMatrix.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value))
                        )
                    {
                        var distributionMatrixObj =
                            this.distributionMatrixService.GetById(Convert.ToInt32(matrix.Value));
                        this.WorkflowDetailObj.DistributionMatrixIDs += matrix.Value + ";";
                        this.WorkflowDetailObj.Recipients += "Matrix - " + matrix.Text + "</br>";
                    }

                    this.wfDetailService.Update(this.WorkflowDetailObj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
            catch
            {
            }
        }

        protected void btnSearchApproveUser_OnClick(object sender, ImageClickEventArgs e)
        {
            this.rtvApproveUser.Nodes.Clear();
            var userCheckedIds = new List<string>();
            if (Session["ApproveUserChecked"] != null)
            {
                userCheckedIds = (List<string>)Session["ApproveUserChecked"];
            }

            var userList = this.userService.GetAll().Where(t => string.IsNullOrEmpty(this.txtSearchApproveUser.Text.Trim()) || 
                                                    (t.Username.ToUpper().Contains(this.txtSearchApproveUser.Text.Trim().ToUpper())
                                                    || t.FullName.ToUpper().Contains(this.txtSearchApproveUser.Text.Trim().ToUpper()))).ToList();
            var roleList = userList.Select(t => t.RoleId).Distinct().Select(t => this.roleService.GetByID(t.GetValueOrDefault())).OrderBy(t => t.IsInternal).ThenBy(t => t.Name);
            foreach (var role in roleList)
            {
                var detpNode = new RadTreeNode(role.Name);
                var userOfRole = userList.Where(t => t.RoleId == role.Id).OrderBy(t => t.UserNameWithFullName);
                foreach (var user in userOfRole)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullName, user.Id.ToString());
                    detpNode.Nodes.Add(userNode);
                    userNode.Checked = userCheckedIds.Contains(userNode.Value);
                }

                this.rtvApproveUser.Nodes.Add(detpNode);
            }
        }

        protected void btnSearchReviewUser_OnClick(object sender, ImageClickEventArgs e)
        {
            this.rtvReviewUser.Nodes.Clear();
            var userCheckedIds = new List<string>();
            if (Session["ReviewUserChecked"] != null)
            {
                userCheckedIds = (List<string>)Session["ReviewUserChecked"];
            }

            var userList = this.userService.GetAll().Where(t => string.IsNullOrEmpty(this.txtSearchReviewUser.Text.Trim()) ||
                                                                (t.Username.ToUpper().Contains(this.txtSearchReviewUser.Text.Trim().ToUpper())
                                                                 || t.FullName.ToUpper().Contains(this.txtSearchReviewUser.Text.Trim().ToUpper()))).ToList();
            var roleList = userList.Select(t => t.RoleId).Distinct().Select(t => this.roleService.GetByID(t.GetValueOrDefault())).OrderBy(t => t.IsInternal).ThenBy(t => t.Name);
            foreach (var role in roleList)
            {
                var detpNode = new RadTreeNode(role.Name);
                var userOfRole = userList.Where(t => t.RoleId == role.Id).OrderBy(t => t.UserNameWithFullName);
                foreach (var user in userOfRole)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullName, user.Id.ToString());
                    detpNode.Nodes.Add(userNode);
                    userNode.Checked = userCheckedIds.Contains(userNode.Value);
                }

                this.rtvReviewUser.Nodes.Add(detpNode);
            }
        }

        protected void btnSearchConsolidateUser_OnClick(object sender, ImageClickEventArgs e)
        {
            this.rtvConsolidateUser.Nodes.Clear();
            var userCheckedIds = new List<string>();
            if (Session["ConsolidateUserChecked"] != null)
            {
                userCheckedIds = (List<string>)Session["ConsolidateUserChecked"];
            }

            var userList = this.userService.GetAll().Where(t => string.IsNullOrEmpty(this.txtSearchConsolidateUser.Text.Trim()) ||
                                                                (t.Username.ToUpper().Contains(this.txtSearchConsolidateUser.Text.Trim().ToUpper())
                                                                 || t.FullName.ToUpper().Contains(this.txtSearchConsolidateUser.Text.Trim().ToUpper()))).ToList();
            var roleList = userList.Select(t => t.RoleId).Distinct().Select(t => this.roleService.GetByID(t.GetValueOrDefault())).OrderBy(t => t.IsInternal).ThenBy(t => t.Name);
            foreach (var role in roleList)
            {
                var detpNode = new RadTreeNode(role.Name);
                var userOfRole = userList.Where(t => t.RoleId == role.Id).OrderBy(t => t.UserNameWithFullName);
                foreach (var user in userOfRole)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullName, user.Id.ToString());
                    detpNode.Nodes.Add(userNode);
                    userNode.Checked = userCheckedIds.Contains(userNode.Value);
                }

                this.rtvConsolidateUser.Nodes.Add(detpNode);
            }
        }

        protected void btnSearchInfoUser_OnClick(object sender, ImageClickEventArgs e)
        {
            this.rtvInfoUser.Nodes.Clear();
            var userCheckedIds = new List<string>();
            if (Session["InfoUserChecked"] != null)
            {
                userCheckedIds = (List<string>)Session["InfoUserChecked"];
            }

            var userList = this.userService.GetAll().Where(t => string.IsNullOrEmpty(this.txtSearchInfoUser.Text.Trim()) ||
                                                                (t.Username.ToUpper().Contains(this.txtSearchInfoUser.Text.Trim().ToUpper())
                                                                 || t.FullName.ToUpper().Contains(this.txtSearchInfoUser.Text.Trim().ToUpper()))).ToList();
            var roleList = userList.Select(t => t.RoleId).Distinct().Select(t => this.roleService.GetByID(t.GetValueOrDefault())).OrderBy(t => t.IsInternal).ThenBy(t => t.Name);
            foreach (var role in roleList)
            {
                var detpNode = new RadTreeNode(role.Name);
                var userOfRole = userList.Where(t => t.RoleId == role.Id).OrderBy(t => t.UserNameWithFullName);
                foreach (var user in userOfRole)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullName, user.Id.ToString());
                    detpNode.Nodes.Add(userNode);
                    userNode.Checked = userCheckedIds.Contains(userNode.Value);
                }

                this.rtvInfoUser.Nodes.Add(detpNode);
            }
        }

        protected void btnSearchDM_OnClick(object sender, ImageClickEventArgs e)
        {
            this.rtvMatrix.Nodes.Clear();
            var userCheckedIds = new List<string>();
            if (Session["MatrixChecked"] != null)
            {
                userCheckedIds = (List<string>)Session["MatrixChecked"];
            }

            var userList = this.userService.GetAll().Where(t => string.IsNullOrEmpty(this.txtSearchDM.Text.Trim()) ||
                                                                (t.Username.ToUpper().Contains(this.txtSearchDM.Text.Trim().ToUpper())
                                                                 || t.FullName.ToUpper().Contains(this.txtSearchDM.Text.Trim().ToUpper()))).ToList();
            var roleList = userList.Select(t => t.RoleId).Distinct().Select(t => this.roleService.GetByID(t.GetValueOrDefault())).OrderBy(t => t.IsInternal).ThenBy(t => t.Name);
            foreach (var role in roleList)
            {
                var detpNode = new RadTreeNode(role.Name);
                var userOfRole = userList.Where(t => t.RoleId == role.Id).OrderBy(t => t.UserNameWithFullName);
                foreach (var user in userOfRole)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullName, user.Id.ToString());
                    detpNode.Nodes.Add(userNode);
                    userNode.Checked = userCheckedIds.Contains(userNode.Value);
                }

                this.rtvMatrix.Nodes.Add(detpNode);
            }
        }

        protected void rtvApproveUser_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["ApproveUserChecked"] == null)
            {
                var userList = new List<string>();
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }

                Session.Add("ApproveUserChecked", userList);
            }
            else
            {
                var userList = (List<string>)Session["ApproveUserChecked"];
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }
                else
                {
                    userList.Remove(e.Node.Value);
                }

                Session.Add("ApproveUserChecked", userList);
            }
        }

        protected void rtvReviewUser_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["ReviewUserChecked"] == null)
            {
                var userList = new List<string>();
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }

                Session.Add("ReviewUserChecked", userList);
            }
            else
            {
                var userList = (List<string>)Session["ReviewUserChecked"];
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }
                else
                {
                    userList.Remove(e.Node.Value);
                }

                Session.Add("ReviewUserChecked", userList);
            }
        }

        protected void rtvConsolidateUser_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["ConsolidateUserChecked"] == null)
            {
                var userList = new List<string>();
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }

                Session.Add("ConsolidateUserChecked", userList);
            }
            else
            {
                var userList = (List<string>)Session["ConsolidateUserChecked"];
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }
                else
                {
                    userList.Remove(e.Node.Value);
                }

                Session.Add("ConsolidateUserChecked", userList);
            }
        }

        protected void rtvInfoUser_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["InfoUserChecked"] == null)
            {
                var userList = new List<string>();
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }

                Session.Add("InfoUserChecked", userList);
            }
            else
            {
                var userList = (List<string>)Session["InfoUserChecked"];
                if (e.Node.Checked)
                {
                    userList.Add(e.Node.Value);
                }
                else
                {
                    userList.Remove(e.Node.Value);
                }

                Session.Add("InfoUserChecked", userList);
            }
        }

        protected void rtvMatrix_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (Session["MatrixChecked"] == null)
            {
                var matrixList = new List<string>();
                if (e.Node.Checked)
                {
                    matrixList.Add(e.Node.Value);
                }

                Session.Add("MatrixChecked", matrixList);
            }
            else
            {
                var matrixList = (List<string>)Session["MatrixChecked"];
                if (e.Node.Checked)
                {
                    matrixList.Add(e.Node.Value);
                }
                else
                {
                    matrixList.Remove(e.Node.Value);
                }

                Session.Add("MatrixChecked", matrixList);
            }
        }
    }
}