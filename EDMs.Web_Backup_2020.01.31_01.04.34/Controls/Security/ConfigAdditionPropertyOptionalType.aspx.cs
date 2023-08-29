// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ConfigAdditionPropertyOptionalType : Page
    {
        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly RoleService roleService = new RoleService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        /// <summary>
        /// The group data permission service.
        /// </summary>
        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService = new CategoryService();

        /// <summary>
        /// The doc properties service.
        /// </summary>
        private readonly DocPropertiesService docPropertiesService = new DocPropertiesService();

        /// <summary>
        /// The doc properties view service.
        /// </summary>
        private readonly DocPropertiesViewService docPropertiesViewService = new DocPropertiesViewService();

        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        private readonly OptionalTypePropertiesViewService optionalTypePropertiesViewService = new OptionalTypePropertiesViewService();

        private readonly OptionalTypePropertiesService optionalTypePropertiesService = new OptionalTypePropertiesService();
        
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
        /// The search customer.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void SearchDocument(string search, bool isbind = false)
        {
            ////if (this.radTreeFolder.SelectedNode != null)
            ////{
            ////    var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
            ////    if(!string.IsNullOrEmpty(search))
            ////    {
            ////        this.grdDocument.DataSource = this.documentService.QuickSearch(search, folderId);    
            ////    }
            ////    else
            ////    {
            ////        this.grdDocument.DataSource =  this.documentService.GetAllByFolder(folderId);
            ////    }

                
            ////    if (isbind)
            ////    {
            ////        this.grdDocument.DataBind();
            ////    }
            ////}
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
            if (e.Argument == "Save")
            {
                var propertyIds = string.Empty;
                propertyIds = this.ddlProperties.CheckedItems.Aggregate(propertyIds, (current, t) => current + t.Value + ", ");
                var optionalTypeId = Convert.ToInt32(this.ddlOptionalType.SelectedValue);

                var obj =
                    this.optionalTypePropertiesViewService.GetAll().FirstOrDefault(
                        t => t.OptionalTypeId == optionalTypeId);
                if (obj != null)
                {
                    obj.PropertyIndex = propertyIds;
                    this.optionalTypePropertiesViewService.Update(obj);
                }
                else
                {
                    obj = new OptionalTypePropertiesView()
                    {
                        OptionalTypeId = optionalTypeId,
                        PropertyIndex = propertyIds
                    };

                    this.optionalTypePropertiesViewService.Insert(obj);
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

        protected void rtbCommand_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            var propertyIds = string.Empty;
            propertyIds = this.ddlProperties.CheckedItems.Aggregate(propertyIds, (current, t) => current + t.Value + ", ");
            var optionalTypeId = Convert.ToInt32(this.ddlOptionalType.SelectedValue);

            var obj =
                this.optionalTypePropertiesViewService.GetAll().FirstOrDefault(
                    t => t.OptionalTypeId == optionalTypeId);
            if (obj != null)
            {
                obj.PropertyIndex = propertyIds;
                this.optionalTypePropertiesViewService.Update(obj);
            }
            else
            {
                obj = new OptionalTypePropertiesView()
                {
                    OptionalTypeId = optionalTypeId,
                    PropertyIndex = propertyIds
                };

                this.optionalTypePropertiesViewService.Insert(obj);
            }
        }

        private void LoadComboData()
        {
            var listOptionalType = this.optionalTypeService.GetAll();
            listOptionalType.Insert(0, new OptionalType() { ID = 0, Name = string.Empty });
            this.ddlOptionalType.DataSource = listOptionalType;
            this.ddlOptionalType.DataTextField = "Name";
            this.ddlOptionalType.DataValueField = "ID";
            this.ddlOptionalType.DataBind();
            this.ddlOptionalType.SelectedIndex = 0;

            var listProperties = this.optionalTypePropertiesService.GetAll();
            this.ddlProperties.DataSource = listProperties;
            this.ddlProperties.DataSource = listProperties;
            this.ddlProperties.DataTextField = "Name";
            this.ddlProperties.DataValueField = "ID";
            this.ddlProperties.DataBind();

            this.LoadSelectedProperty();
        }

        private void LoadSelectedProperty()
        {
            var selectedProperty = new List<string>();
            var optionalTypeId = Convert.ToInt32(this.ddlOptionalType.SelectedValue);
            this.ddlProperties.ClearCheckedItems();
            foreach (var optionalTypePropertiesView in this.optionalTypePropertiesViewService.GetAllSpecial(optionalTypeId))
            {
                var temp = optionalTypePropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
                selectedProperty.AddRange(temp);
            }

            selectedProperty = selectedProperty.Distinct().ToList();

            foreach (RadComboBoxItem item in this.ddlProperties.Items)
            {
                if (selectedProperty.Contains(item.Value))
                {
                    item.Checked = true;
                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadSelectedProperty();
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadSelectedProperty();
        }
    }
}

