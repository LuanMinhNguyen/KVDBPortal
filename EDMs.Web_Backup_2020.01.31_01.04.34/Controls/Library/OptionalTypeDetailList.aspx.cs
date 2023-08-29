// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.UI;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class OptionalTypeDetailList : Page
    {
        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly OptionalTypeDetailService OptionalTypeDetailService = new OptionalTypeDetailService();

        /// <summary>
        /// The plant service.
        /// </summary>
        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        /// <summary>
        /// The group data permission service.
        /// </summary>
        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService = new CategoryService();

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
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.LoadComboData();
                this.LoadOptionalTypeDetailTree();
                
                var categoryPermission = this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Select(t => t.CategoryIdList).ToList();
                var listCategory = this.categoryService.GetAll().Where(t => categoryPermission.Any(x => x == t.ID.ToString())).ToList();
                if (listCategory.Any())
                {
                    foreach (var category in listCategory)
                    {
                        category.ParentId = -1;
                    }

                    listCategory.Insert(0, new Category() { ID = -1, Name = "BDPOC DOCUMENTS" });

                    this.radPbCategories.DataSource = listCategory;
                    this.radPbCategories.DataFieldParentID = "ParentId";
                    this.radPbCategories.DataFieldID = "Id";
                    this.radPbCategories.DataValueField = "Id";
                    this.radPbCategories.DataTextField = "Name";
                    this.radPbCategories.DataBind();
                    this.radPbCategories.Items[0].Expanded = true;

                    foreach (RadPanelItem item in this.radPbCategories.Items[0].Items)
                    {
                        item.ImageUrl = @"~/Images/category2.png";
                        item.NavigateUrl = "Default.aspx?doctype=" + item.Value;
                    }
                }

                this.LoadListPanel();
                this.LoadSystemPanel();
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("Delete_"))
            {
                var docTypeId = Convert.ToInt32(e.Argument.Replace("Delete_", ""));
                var tempNode = new RadTreeNode();

                foreach (RadTreeNode node in this.rtvOptionalTypeDetail.GetAllNodes())
                {
                    if (node.Value == docTypeId.ToString())
                    {
                        tempNode = node;
                        break;
                    }
                }

                foreach (var node in tempNode.GetAllNodes())
                {
                    this.OptionalTypeDetailService.Delete(Convert.ToInt32(node.Value));
                }

                this.OptionalTypeDetailService.Delete(Convert.ToInt32(tempNode.Value));
                this.LoadOptionalTypeDetailTree();
                this.RestoreExpandStateTreeView();
            }
            else if (e.Argument == "RebindTreeView")
            {
                this.LoadOptionalTypeDetailTree();
            }
        }

        /// <summary>
        /// The rtv system doc_ on node data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void rtvOptionalTypeDetail_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/doctype.png";
        }

        /// <summary>
        /// The restore expand state tree view.
        /// </summary>
        private void RestoreExpandStateTreeView()
        {
            // Restore expand state of tree folder
            HttpCookie cookie = this.Request.Cookies["expandedNodesOptionalTypeDetail"];
            if (cookie != null)
            {
                var expandedNodeValues = cookie.Value.Split('*');
                foreach (var nodeValue in expandedNodeValues)
                {
                    RadTreeNode expandedNode = this.rtvOptionalTypeDetail.FindNodeByValue(HttpUtility.UrlDecode(nodeValue));
                    if (expandedNode != null)
                    {
                        expandedNode.Expanded = true;
                    }
                }
            }
        }

        private void LoadListPanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ListID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "LIST" });

                this.radPbList.DataSource = permissions;
                this.radPbList.DataFieldParentID = "ParentId";
                this.radPbList.DataFieldID = "Id";
                this.radPbList.DataValueField = "Id";
                this.radPbList.DataTextField = "MenuName";
                this.radPbList.DataBind();
                this.radPbList.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbList.Items[0].Items)
                {
                    item.ImageUrl = @"~/Images/listmenu.png";
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Optional type detail")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void LoadSystemPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("SystemID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "SYSTEM" });

                this.radPbSystem.DataSource = permissions;
                this.radPbSystem.DataFieldParentID = "ParentId";
                this.radPbSystem.DataFieldID = "Id";
                this.radPbSystem.DataValueField = "Id";
                this.radPbSystem.DataTextField = "MenuName";
                this.radPbSystem.DataBind();
                this.radPbSystem.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbSystem.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                }
            }
        }

        private void LoadOptionalTypeDetailTree()
        {
            var listOptionalTypeDetail = this.OptionalTypeDetailService.GetAll().OrderBy(t => t.FullName);
            var textField = ConfigurationManager.AppSettings.Get("TextFieldOptionalTypeDetail");

            this.rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
            this.rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
            this.rtvOptionalTypeDetail.DataTextField = !string.IsNullOrEmpty(textField) ? textField : "Name";
            this.rtvOptionalTypeDetail.DataValueField = "ID";
            this.rtvOptionalTypeDetail.DataFieldID = "ID";
            this.rtvOptionalTypeDetail.DataBind();

            this.RestoreExpandStateTreeView();
        }

        private  void LoadComboData()
        {
            var filterConditionItem = this.CustomerMenu.FindItemByValue("FilterCondition");
            var ddlCategory = (RadComboBox)filterConditionItem.FindControl("ddlCategory");
            var ddlOptionalType = (RadComboBox)filterConditionItem.FindControl("ddlOptionalType");
            if (ddlCategory != null)
            {
                var listCategory = this.categoryService.GetAll();
                listCategory.Insert(0, new Category { ID = 0, Name = string.Empty });
                ddlCategory.DataSource = listCategory;
                ddlCategory.DataTextField = "Name";
                ddlCategory.DataValueField = "ID";
                ddlCategory.DataBind();
                ddlCategory.SelectedIndex = 0;
            }

            if (ddlOptionalType != null)
            {
                var listOptionalType = this.optionalTypeService.GetAllActive();
                listOptionalType.Insert(0, new OptionalType() { ID = 0, Name = string.Empty });
                ddlOptionalType.DataSource = listOptionalType;
                ddlOptionalType.DataTextField = "Name";
                ddlOptionalType.DataValueField = "ID";
                ddlOptionalType.DataBind();
                ddlOptionalType.SelectedIndex = 0;
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var filterConditionItem = this.CustomerMenu.FindItemByValue("FilterCondition");
            var ddlCategory = (RadComboBox)filterConditionItem.FindControl("ddlCategory");
            var ddlOptionalType = (RadComboBox)filterConditionItem.FindControl("ddlOptionalType");
            var categoryId = ddlCategory.SelectedValue;
            var optionalTypeId = Convert.ToInt32(ddlOptionalType.SelectedValue);
            var listOptionalTypeDetail = this.OptionalTypeDetailService.GetAllSpecial(categoryId, optionalTypeId, "0").OrderBy(t => t.FullName);

            this.rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
            this.rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
            this.rtvOptionalTypeDetail.DataTextField = "Name";
            this.rtvOptionalTypeDetail.DataValueField = "ID";
            this.rtvOptionalTypeDetail.DataFieldID = "ID";
            this.rtvOptionalTypeDetail.DataBind();

            this.RestoreExpandStateTreeView();
        }

        protected void ddlOptionalType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var filterConditionItem = this.CustomerMenu.FindItemByValue("FilterCondition");
            var ddlCategory = (RadComboBox)filterConditionItem.FindControl("ddlCategory");
            var ddlOptionalType = (RadComboBox)filterConditionItem.FindControl("ddlOptionalType");
            var categoryId = ddlCategory.SelectedValue;
            var optionalTypeId = Convert.ToInt32(ddlOptionalType.SelectedValue);
            var listOptionalTypeDetail = this.OptionalTypeDetailService.GetAllSpecial(categoryId, optionalTypeId, "0").OrderBy(t => t.FullName);

            var temp = listOptionalTypeDetail.Where(t => t.ParentId != null).Select(t => t.ParentId).Distinct().ToList();
            var temp2 = listOptionalTypeDetail.Select(t => t.ID).ToList();

            foreach (var x in temp)
            {
                if (!temp2.Contains(x.Value))
                {
                    var tempList = listOptionalTypeDetail.Where(t => t.ParentId == x.Value).ToList();
                    foreach (var optionalTypeDetail in tempList)
                    {
                        optionalTypeDetail.ParentId = null;
                    }
                }
            }

            this.rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
            this.rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
            this.rtvOptionalTypeDetail.DataTextField = "Name";
            this.rtvOptionalTypeDetail.DataValueField = "ID";
            this.rtvOptionalTypeDetail.DataFieldID = "ID";
            this.rtvOptionalTypeDetail.DataBind();

            this.RestoreExpandStateTreeView();
        }
    }
}

