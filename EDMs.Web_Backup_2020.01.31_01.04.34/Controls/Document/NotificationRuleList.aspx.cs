﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class NotificationRuleList : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The notification rule service.
        /// </summary>
        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

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
                        item.ImageUrl = @"Images/category2.png";
                        item.NavigateUrl = "Default.aspx?doctype=" + item.Value;
                    }
                }

                this.LoadListPanel();
                this.LoadSystemPanel();
            }
        }

        /// <summary>
        /// The custom folder tree.
        /// </summary>
        /// <param name="radTreeView">
        /// The rad tree view.
        /// </param>
        private void CustomFolderTree(RadTreeNode radTreeView)
        {
            foreach (var node in radTreeView.Nodes)
            {
                var nodetemp = (RadTreeNode)node;
                if (nodetemp.Nodes.Count > 0)
                {
                    this.CustomFolderTree(nodetemp);
                }

                nodetemp.ImageUrl = "Images/folderdir16.png";
            }
        }

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false)
        {
            this.grdDocument.DataSource = this.notificationRuleService.GetAll();

            if (isbind)
            {
                this.grdDocument.DataBind();
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
            if (e.Argument == "Rebind")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.MasterTableView.CurrentPageIndex = this.grdDocument.MasterTableView.PageCount - 1;
                this.grdDocument.Rebind();
            }
            else
            {
                this.SearchDocument(e.Argument, true);
            }
        }
        
        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Grid KhacHang item created
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (Image)e.Item.FindControl("EditLink");
                editLink.Attributes["href"] = "#";
                editLink.Attributes["onclick"] = string.Format(
                    "return ShowEditForm('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
        }

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var notificationId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            this.notificationRuleService.Delete(notificationId);

            this.grdDocument.Rebind();
        }

        /// <summary>
        /// The grd document_ item command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string abc = e.CommandName;
        }

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            //AddPageView(e.Tab);
            e.Tab.PageView.Selected = true;
        }

        protected void radMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The grd document_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                var newIcon = (Image)e.Item.FindControl("newicon");

                if ((DateTime.Now - Convert.ToDateTime(DataBinder.Eval(item.DataItem, "CreatedDate"))).TotalHours < 24)
                {
                    newIcon.Visible = true;
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
                    item.ImageUrl = @"Images/listmenu.png";
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Notification Rules")
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
    }
}

