// --------------------------------------------------------------------------------------------------------------------
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

    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Controls.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class BlockEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly BlockService blockService;

        /// <summary>
        /// The project service.
        /// </summary>
        private readonly ProjectService projectService;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public BlockEditForm()
        {
            this.userService = new UserService();
            this.blockService = new BlockService();
            this.projectService = new ProjectService();
            this.roleService = new RoleService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.blockService.GetById(Convert.ToInt32(this.Request.QueryString["id"]));
                    if (obj != null)
                    {

                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;


                        if (obj.ProjectId != null && obj.ProjectId != 0)
                        {
                            var rtvProject = (RadTreeView)this.ddlProject.Items[0].FindControl("rtvProject");
                            if (rtvProject != null)
                            {
                               //// rtvProject.Nodes.FindNodeByValue(obj.ParentId.ToString()).Selected = true;
                                var selectedNode = rtvProject.GetAllNodes().First(t => t.Value == obj.ParentId.ToString());
                                if (selectedNode != null)
                                {
                                    rtvProject.GetAllNodes().First(t => t.Value == obj.ParentId.ToString()).Selected = true;
                                    this.ExpandParentNode(selectedNode);
                                    this.ddlProject.Items[0].Text = selectedNode.Text;
                                }
                            }
                        }

                        var roleIds = obj.RoleId;
                        foreach (RadComboBoxItem item in this.ddlDeparment.Items)
                        {
                            if (roleIds.Contains(item.Value))
                            {
                                item.Checked = true;
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
                var roleIds = string.Empty;
                var roleNames = string.Empty;
                roleIds = this.ddlDeparment.CheckedItems.Aggregate(roleIds, (current, t) => current + t.Value + ", ");
                roleNames = this.ddlDeparment.CheckedItems.Aggregate(roleNames, (current, t) => current + t.Text + ", ");
                if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
                {
                    var id = Convert.ToInt32(this.Request.QueryString["id"]);
                    var obj = this.blockService.GetById(id);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.RoleId = roleIds;
                        obj.RoleName = roleNames;
                        var rtvProject = (RadTreeView)this.ddlProject.Items[0].FindControl("rtvProject");
                        if (rtvProject != null && rtvProject.SelectedNode != null)
                        {
                            obj.ProjectId = Convert.ToInt32(rtvProject.SelectedNode.Value);
                        }

                        ////obj.ParentId = !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? Convert.ToInt32(this.ddlProject.SelectedValue) : (int?)null;

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.blockService.Update(obj);
                    }
                }
                else
                {
                    
                    var obj = new Block()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        ProjectId = !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? Convert.ToInt32(this.ddlProject.SelectedValue) : (int?)null,
                        RoleId = roleIds,
                        RoleName = roleNames,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };

                    this.blockService.Insert(obj);
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
                this.fileNameValidator.ErrorMessage = "Please enter Block name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var listDeparment = this.roleService.GetAllSpecial();
            this.ddlDeparment.DataSource = listDeparment;
            this.ddlDeparment.DataTextField = "Name";
            this.ddlDeparment.DataValueField = "Id";
            this.ddlDeparment.DataBind();
            this.ddlDeparment.SelectedIndex = 0;

            var rtvProject = (RadTreeView)this.ddlProject.Items[0].FindControl("rtvProject");
            if (rtvProject != null)
            {
                var listBlock = this.projectService.GetAll();

                listBlock.Insert(0, new Project() { ParentId = null, Name = "(None)" });

                rtvProject.DataSource = listBlock;
                rtvProject.DataFieldParentID = "ParentId";
                rtvProject.DataTextField = "Name";
                rtvProject.DataValueField = "ID";
                rtvProject.DataFieldID = "ID";
                rtvProject.DataBind();
            }
        }
    }
}