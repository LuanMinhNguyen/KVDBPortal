using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Library;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// The role edit form.
    /// </summary>
    public partial class RoleEditForm : Page
    {
        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService;

        private readonly GroupDataPermissionService groupDataPermissionService;

        private readonly OrganizationCodeService organizationCodeService;
        /// <summary>
        /// The role parameter key.
        /// </summary>
        private const string RoleParameterKey = "roleid";

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEditForm"/> class.
        /// </summary>
        public RoleEditForm()
        {
            this.roleService = new RoleService();
            this.categoryService = new CategoryService();
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.organizationCodeService = new OrganizationCodeService();
        }

        /// <summary>
        /// Gets the role id.
        /// </summary>
        private int? RoleId
        {
            get
            {
                if (String.IsNullOrEmpty(Request[RoleParameterKey])) return null;

                int outValue;
                if (int.TryParse(Request[RoleParameterKey], out outValue))
                    return outValue;
                return null;
            }
        }

        /// <summary>
        /// The load data.
        /// </summary>
        private void LoadData()
        {
            if (this.RoleId == null)
            {
                return;
            }

            var selectedCategory = this.groupDataPermissionService.GetByRoleId(this.RoleId.Value).Select(t => t.CategoryIdList).ToList();

            foreach (RadComboBoxItem item in this.ddlCategory.Items)
            {
                if (selectedCategory.Contains(item.Value))
                {
                    item.Checked = true;
                }
            }

            var role = this.roleService.GetByID(this.RoleId.Value);
            this.txtRoleName.Text = role.Name;
            this.txtDescription.Text = role.Description;
            this.cbIsInternal.Checked = role.IsInternal.GetValueOrDefault();
            this.cbIsLimitedView.Checked = role.IsLimitedView.GetValueOrDefault();
            if (!string.IsNullOrEmpty(role.Color))
            {
                this.txtColor.SelectedColor = ColorTranslator.FromHtml(role.Color);
            }
            if (role.ContractorId != null && role.ContractorId.ToString() != "")
            { this.ddlContractor.SelectedValue = role.ContractorId.ToString();
                if (role.ContractorId == 2)
                {
                    this.IndexContractor.Visible = true;
                }
            }
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
            if (!IsPostBack)
            {   this.LoadComboData();
                if (RoleId != null)
                {
                    this.LoadData();
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
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            var role = new Role
            {
                Name = txtRoleName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                IsInternal = this.cbIsInternal.Checked,
                Color = ColorTranslator.ToHtml(this.txtColor.SelectedColor),
                IsLimitedView = this.cbIsLimitedView.Checked,
                ContractorId = Convert.ToInt32(this.ddlContractor.SelectedValue),
                ContractorName = this.ddlContractor.SelectedItem.Text
            };

            if (this.RoleId == null)
            {
                this.roleService.Insert(role);
            }
            else
            {
                role.Id = this.RoleId.Value;
                role.Description = this.txtDescription.Text.Trim();
                role.IsInternal = this.cbIsInternal.Checked;
                role.IsLimitedView = this.cbIsLimitedView.Checked;
                role.ContractorId = Convert.ToInt32(this.ddlContractor.SelectedValue);
                role.ContractorName = this.ddlContractor.SelectedItem.Text;
                this.roleService.Update(role);

                var groupDataPermissionList = this.groupDataPermissionService.GetByRoleId(this.RoleId.Value);
                foreach (var groupDataPermission in groupDataPermissionList)
                {
                    this.groupDataPermissionService.Delete(groupDataPermission);
                }
            }

            foreach (var radComboBoxItem in this.ddlCategory.CheckedItems)
            {
                var groupDataPermission = new GroupDataPermission()
                {
                    CategoryIdList = radComboBoxItem.Value,
                    RoleId = this.RoleId.Value
                };

                this.groupDataPermissionService.Insert(groupDataPermission);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
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
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
        }

        /// <summary>
        /// The server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.txtRoleName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Department name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        /// <summary>
        /// The load combo data.
        /// </summary>
        private void LoadComboData()
        {
            var listCategory = this.categoryService.GetAll();
            this.ddlCategory.DataSource = listCategory;
            this.ddlCategory.DataTextField = "Name";
            this.ddlCategory.DataValueField = "ID";
            this.ddlCategory.DataBind();


            var organizationCodeList = this.organizationCodeService.GetAll().OrderBy(t => t.Code).ToList();
            organizationCodeList.Insert(0, new OrganizationCode() {Code = "", Description = "" });
            this.ddlContractor.DataSource = organizationCodeList;
            this.ddlContractor.DataTextField = "FullName";
            this.ddlContractor.DataValueField = "ID";
            this.ddlContractor.DataBind();

        }
    }
}