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
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Library
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class RecipientPageEmail : Page
    {

         private readonly ScopeProjectService projectService = new ScopeProjectService();

        private readonly RoleService roleService = new RoleService();
        private readonly UserService userService = new UserService();

        private readonly DistributionMatrixTypeService distributionMatrixTypeService =
            new DistributionMatrixTypeService();

        private readonly DistributionMatrixService distributionMatrixService = new DistributionMatrixService();

        private readonly TitleService titleService = new TitleService();
        private readonly CustomizeReceivedEmailService ReceivedEmailService = new CustomizeReceivedEmailService();
        private int DetailId
        {
            get { return Convert.ToInt32(this.Request.QueryString["wfdId"]); }
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
                
                Session.Remove("ReviewUserChecked");
                Session.Remove("InfoUserChecked");
                Session.Remove("MatrixChecked");
                // -------------------------------------------

                this.LoadComboData(this.Request.QueryString["type"]);
               
                    if (!string.IsNullOrEmpty(this.Request.QueryString["wfdId"]))
                    {
                    var WorkflowDetailObj = this.ReceivedEmailService.GetById(Convert.ToInt32(this.Request.QueryString["wfdId"]));


                            this.LoadListBoxData(WorkflowDetailObj);
                        
                    }
                

            }
        }

        private void LoadListBoxData(CustomizeReceivedEmail workflowDetailObj)
        {
            try
            {
               

                foreach (RadTreeNode deptNode in this.rtvReviewUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.ToUserIDs) &&
                            workflowDetailObj.ToUserIDs.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

               
                foreach (RadTreeNode deptNode in this.rtvInfoUser.Nodes)
                {
                    foreach (RadTreeNode userNode in deptNode.Nodes)
                    {
                        if (!string.IsNullOrEmpty(workflowDetailObj.CCUserIDs) &&
                            workflowDetailObj.CCUserIDs.Split(';').Contains(userNode.Value))
                        {
                            userNode.Checked = true;
                        }
                    }
                }

                foreach (RadTreeNode matrixNode in this.rtvMatrix.Nodes)
                {
                    if (!string.IsNullOrEmpty(workflowDetailObj.DistributionMatrixCCIDs) &&
                            workflowDetailObj.DistributionMatrixCCIDs.Split(';').Contains(matrixNode.Value))
                    {
                        matrixNode.Checked = true;
                    }
                }

                // Build session data of node checked in radtreeview
               

                if (Session["ReviewUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.ToUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("ReviewUserChecked", userList);
                }

                

                if (Session["InfoUserChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.CCUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("InfoUserChecked", userList);
                }

                if (Session["MatrixChecked"] == null)
                {
                    var userList = new List<string>();
                    userList.AddRange(workflowDetailObj.DistributionMatrixCCIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)));

                    Session.Add("MatrixChecked", userList);
                }
                // ------------------------------------------------------------------------------------------------------
            }
            catch
            {
            }
        }
        private void LoadComboData(string from)
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

                if (from=="To")
                {

                    this.divInformationUser.Visible = false;
                    this.divReviewUser.Visible = true;
                    this.divMatrix.Visible = false;



                }
                else
                {
                    this.divInformationUser.Visible = true;
                    this.divReviewUser.Visible = false;
                    this.divMatrix.Visible = true;
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
                var WorkflowDetailObj = this.ReceivedEmailService.GetById(Convert.ToInt32(this.Request.QueryString["wfdId"]));




                if (this.Request.QueryString["type"] == "To")
                {
                    WorkflowDetailObj.ToUserIDs = string.Empty;
                    WorkflowDetailObj.RecipientsTo = string.Empty;
                    foreach (
                      RadTreeNode reviewUser in
                          this.rtvReviewUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        WorkflowDetailObj.ToUserIDs += reviewUser.Value + ";";
                        WorkflowDetailObj.RecipientsTo += reviewUser.Text + "</br>";
                    }
                }
                else
                {
                    WorkflowDetailObj.CCUserIDs = string.Empty;
                    WorkflowDetailObj.DistributionMatrixCCIDs = string.Empty;
                    WorkflowDetailObj.RecipientsCC = string.Empty;
                    foreach (
                       RadTreeNode infoUser in
                           this.rtvInfoUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    {
                        WorkflowDetailObj.CCUserIDs += infoUser.Value + ";";
                        WorkflowDetailObj.RecipientsCC += infoUser.Text + "</br>";
                    }



                    foreach (
                        RadTreeNode matrix in this.rtvMatrix.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value))
                        )
                    {
                        var distributionMatrixObj =
                            this.distributionMatrixService.GetById(Convert.ToInt32(matrix.Value));
                        WorkflowDetailObj.DistributionMatrixCCIDs += matrix.Value + ";";
                        WorkflowDetailObj.RecipientsCC += "Matrix - " + matrix.Text + "</br>";
                    }
                }
                this.ReceivedEmailService.Update(WorkflowDetailObj);


                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
            catch
            {
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