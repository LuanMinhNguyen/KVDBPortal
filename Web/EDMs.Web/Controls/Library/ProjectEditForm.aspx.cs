﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Controls.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ProjectEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly ProjectService ProjectService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ProjectEditForm()
        {
            this.userService = new UserService();
            this.ProjectService = new ProjectService();
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
                this.LoadComboData();

                if (!string.IsNullOrEmpty(this.Request.QueryString["doctypeId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.ProjectService.GetById(Convert.ToInt32(this.Request.QueryString["doctypeId"]));
                    if (obj != null)
                    {

                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;


                        if (obj.ParentId != null && obj.ParentId != 0)
                        {
                            var rtvProject = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvProject");
                            if (rtvProject != null)
                            {
                               //// rtvProject.Nodes.FindNodeByValue(obj.ParentId.ToString()).Selected = true;
                                var selectedNode = rtvProject.GetAllNodes().First(t => t.Value == obj.ParentId.ToString());
                                if (selectedNode != null)
                                {
                                    rtvProject.GetAllNodes().First(t => t.Value == obj.ParentId.ToString()).Selected = true;
                                    this.ExpandParentNode(selectedNode);
                                    this.ddlParent.Items[0].Text = selectedNode.Text;
                                }
                            }
                        }

                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (obj.LastUpdatedBy != null && obj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(obj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + obj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                }
            }
        }

        private void ExpandParentNode(RadTreeNode node)
        {
            if (node.ParentNode != null)
            {
                node.ParentNode.Expanded = true;
                this.ExpandParentNode(node.ParentNode);
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
            if (this.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["doctypeId"]))
                {
                    var doctypeId = Convert.ToInt32(this.Request.QueryString["doctypeId"]);
                    var obj = this.ProjectService.GetById(doctypeId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();

                        var rtvProject = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvProject");
                        if (rtvProject != null)
                        {
                            obj.ParentId = Convert.ToInt32(rtvProject.SelectedNode.Value);
                        }

                        ////obj.ParentId = !string.IsNullOrEmpty(this.ddlParent.SelectedValue) ? Convert.ToInt32(this.ddlParent.SelectedValue) : (int?)null;

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.ProjectService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Project()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        ParentId = !string.IsNullOrEmpty(this.ddlParent.SelectedValue) ? Convert.ToInt32(this.ddlParent.SelectedValue) : (int?)null,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };

                    this.ProjectService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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
            if(this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Document type name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var rtvProject = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvProject");
            if (rtvProject != null)
            {
                var listProject = this.ProjectService.GetAll();

                listProject.Insert(0, new Project() { ParentId = null, Name = "(None)" });

                rtvProject.DataSource = listProject;
                rtvProject.DataFieldParentID = "ParentId";
                rtvProject.DataTextField = "Name";
                rtvProject.DataValueField = "ID";
                rtvProject.DataFieldID = "ID";
                rtvProject.DataBind();
            }
        }
    }
}