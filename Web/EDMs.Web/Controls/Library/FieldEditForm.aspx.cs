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
    public partial class FieldEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly FieldService FieldService;



        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly BlockService blockService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public FieldEditForm()
        {
            this.userService = new UserService();
            this.FieldService = new FieldService();
            this.roleService = new RoleService();
            this.blockService = new BlockService();
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

                    var obj = this.FieldService.GetById(Convert.ToInt32(this.Request.QueryString["id"]));
                    if (obj != null)
                    {
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;

                        var roleIds = obj.RoleId;
                        foreach (RadComboBoxItem item in this.ddlDepartment.Items)
                        {
                            if (roleIds.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }
                            
                        this.ddlBlock.SelectedValue = obj.BlockId.GetValueOrDefault().ToString();

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
                roleIds = this.ddlDepartment.CheckedItems.Aggregate(roleIds, (current, t) => current + t.Value + ", ");
                roleNames = this.ddlDepartment.CheckedItems.Aggregate(roleNames, (current, t) => current + t.Text + ", ");
                if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
                {
                    var id = Convert.ToInt32(this.Request.QueryString["id"]);
                    var obj = this.FieldService.GetById(id);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.BlockId = Convert.ToInt32(this.ddlBlock.SelectedValue);
                        obj.RoleId = roleIds;
                        obj.RoleName = roleNames;
                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.FieldService.Update(obj);
                    }
                }
                else
                {
                    var obj = new Field()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        BlockId = Convert.ToInt32(this.ddlBlock.SelectedValue),
                        RoleId = roleIds,
                        RoleName = roleNames,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };

                    this.FieldService.Insert(obj);
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
                this.fileNameValidator.ErrorMessage = "Please enter Field name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var listDeparment = this.roleService.GetAllSpecial();
            this.ddlDepartment.DataSource = listDeparment;
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "Id";
            this.ddlDepartment.DataBind();
            this.ddlDepartment.SelectedIndex = 0;

            var listBlock = this.blockService.GetAll();
            listBlock.Insert(0, new Block() { ID = 0, Name = string.Empty });
            this.ddlBlock.DataSource = listBlock;
            this.ddlBlock.DataTextField = "Name";
            this.ddlBlock.DataValueField = "Id";
            this.ddlBlock.DataBind();
            this.ddlBlock.SelectedIndex = 0;
        }
    }
}