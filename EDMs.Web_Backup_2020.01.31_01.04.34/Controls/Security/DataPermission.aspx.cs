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
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class DataPermission : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService = new FolderService();

        private readonly RoleService roleService = new RoleService();

        private readonly CategoryService categoryService = new CategoryService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly PermissionWorkgroupService PermissionWorkgroupService = new PermissionWorkgroupService();

        private readonly MenuService menuService = new MenuService();
        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";
        
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
            
            if (!Page.IsPostBack)
            {
                var listRole = new List<Role>();
                listRole = this.roleService.GetAll(UserSession.Current.RoleId == 1);

                ////if (UserSession.Current.User.IsAdmin.GetValueOrDefault())
                ////{
                ////    listRole = this.roleService.GetAll();
                ////}
                ////else
                ////{
                ////    var roleObj = this.roleService.GetByID(UserSession.Current.User.RoleId.GetValueOrDefault());
                ////    if (roleObj != null)
                ////    {
                ////        listRole.Add(roleObj);
                ////    }
                ////}

                foreach (var role in listRole)
                {
                    role.ParentId = -1;
                }

                listRole.Insert(0, new Role { Id = -1, Name = "GROUP USER" });

                this.radPbGroup.DataSource = listRole;
                this.radPbGroup.DataFieldParentID = "ParentId";
                this.radPbGroup.DataFieldID = "Id";
                this.radPbGroup.DataValueField = "Id";
                this.radPbGroup.DataTextField = "Name";
                this.radPbGroup.DataBind();
                this.radPbGroup.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbGroup.Items[0].Items)
                {
                    item.ImageUrl = @"~/Images/group.png";
                }

                ////this.radTreeCategories.DataSource = this.categoryService.GetAll();
                ////this.radTreeCategories.DataFieldID = "ID";
                ////this.radTreeCategories.DataValueField = "ID";
                ////this.radTreeCategories.DataTextField = "Name";
                ////this.radTreeCategories.DataBind();

                ////foreach (RadTreeNode categoryNode in this.radTreeCategories.Nodes)
                ////{
                ////    categoryNode.ImageUrl = "~/Images/Document.png";
                ////}


                ////if (!string.IsNullOrEmpty(Request.QueryString["doctype"]))
                ////{
                ////    var doctype = Convert.ToInt32(Request.QueryString["doctype"]);
                ////    var listFolder = this.folderService.GetAllByCategory(doctype);
                ////    this.radTreeFolder.DataSource = listFolder;
                ////    this.radTreeFolder.DataFieldParentID = "ParentID";
                ////    this.radTreeFolder.DataTextField = "Name";
                ////    this.radTreeFolder.DataValueField = "ID";
                ////    this.radTreeFolder.DataFieldID = "ID";
                ////    this.radTreeFolder.DataBind();

                ////    foreach (var node in this.radTreeFolder.Nodes)
                ////    {
                ////        ((RadTreeNode)node).ImageUrl = "~/Images/folderdir16.png";
                ////        ((RadTreeNode)node).Expanded = true;
                ////        this.CustomFolderTree((RadTreeNode)node);
                ////    }

                ////    this.radPbCategories.Items[0].Items[doctype - 2].Selected = true;
                ////    this.RestoreExpandStateTreeView();
                ////}
            }
        }

        /// <summary>
        /// The rad tree view 1_ node click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void radTreeCategories_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var doctype = Convert.ToInt32(e.Node.Value);
            var listFolder = this.folderService.GetAllByCategory(doctype);
            var selectedFolder = new List<int>();
            if (string.IsNullOrEmpty(this.Session[e.Node.Value].ToString()))
            {
                Session.Add(e.Node.Value, new List<string>());
            }
            else
            {
                selectedFolder = ((List<string>)Session[e.Node.Value]).Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).ToList();
            }
            
            ////// Get all parent of selectec folder
            ////var selectedFolderFull = this.folderService.GetSpecificFolder(selectedFolder).Select(t => t.ID);

            var folderPermissions = listFolder.Select(folder => new FolderPermissionWrapper
            {
                Folder = folder,
                IsPermitted = selectedFolder.Count == 0 ? false : selectedFolder.Any(folderId => folderId == folder.ID)
            });
            this.radTreeFolder.DataSource = folderPermissions;
            this.radTreeFolder.DataFieldParentID = "ParentID";
            this.radTreeFolder.DataTextField = "Name";
            this.radTreeFolder.DataValueField = "ID";
            this.radTreeFolder.DataFieldID = "ID";
            this.radTreeFolder.DataBind();
            this.radTreeFolder.Nodes[0].Expanded = true;
            ////this.RestoreExpandStateTreeView();
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
            if (e.Argument == "CreateCategorySession")
            {
                if (this.Session[this.radTreeCategories.SelectedNode.Value] == null)
                {
                    Session.Add(this.radTreeCategories.SelectedNode.Value, string.Empty);
                }
            }
        }
        
        /// <summary>
        /// The rad pb group_ item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void radPbGroup_ItemClick(object sender, RadPanelBarEventArgs e)
        {
            Session.Add("SelectedGroup", e.Item.Value);
            var groupDatapermission = this.groupDataPermissionService.GetByRoleId(Convert.ToInt32(e.Item.Value));
            List<Category> listCategory;
            if (UserSession.Current.User.IsAdmin.GetValueOrDefault())
            {
                listCategory = this.categoryService.GetAll();
            }
            else
            {
                var categoryPermission = this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault())
                                                                        .Select(t => t.CategoryIdList).ToList();
                listCategory = this.categoryService.GetAll().Where(t => categoryPermission.Any(x => x == t.ID.ToString(CultureInfo.InvariantCulture))).ToList();
            }

            foreach (var category in listCategory)
            {
                Session.Remove(category.ID.ToString(CultureInfo.InvariantCulture));
            }

            if (groupDatapermission != null)
            {
                var selectedCategory = groupDatapermission.Select(t => t.CategoryIdList).Distinct().ToList();
                
                foreach (string category in selectedCategory)
                {
                    Session.Add(category, groupDatapermission.Where(t => t.CategoryIdList == category).Select(t => t.FolderIdList).ToList());
                }


                var catePermissions = listCategory.Select(cate => new CategoryPermissionWrapper()
                {
                    Category = cate,
                    IsPermitted = selectedCategory.Any(cateId => cateId == cate.ID.ToString())
                });

                this.radTreeCategories.DataSource = catePermissions;
            }
            else
            {
                this.radTreeCategories.DataSource = this.categoryService.GetAll();
            }

            this.radTreeCategories.DataFieldID = "ID";
            this.radTreeCategories.DataValueField = "ID";
            this.radTreeCategories.DataTextField = "Name";
            this.radTreeCategories.DataBind();

            foreach (RadTreeNode categoryNode in this.radTreeCategories.Nodes)
            {
                categoryNode.ImageUrl = "~/Images/category2.png";
            }

            this.radTreeFolder.Nodes.Clear();
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

        /// <summary>
        /// The btn save_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var menuObj = this.menuService.GetByID(1);
            if (menuObj != null)
            {
                menuObj.Description = "Home";
                menuObj.Url = "ProjectProccessReport.aspx";
                this.menuService.Update(menuObj);
            }

            menuObj = this.menuService.GetByID(84);
            if (menuObj != null)
            {
                menuObj.Active = false;
                this.menuService.Update(menuObj);
            }

            ////if (this.Session["SelectedGroup"] != null)
            ////{
            ////    var selectedRole = Convert.ToInt32(Session["SelectedGroup"]);
            ////    var checkedCategoryNodes = this.radTreeCategories.CheckedNodes.Where(t => t.Nodes.Count == 0);

            ////    var groupDataPermissionList = this.groupDataPermissionService.GetByRoleId(selectedRole);
            ////    foreach (var groupDataPermission in groupDataPermissionList)
            ////    {
            ////        this.groupDataPermissionService.Delete(groupDataPermission);
            ////    }

            ////    foreach (var checkedCategoryNode in checkedCategoryNodes)
            ////    {
            ////        var groupDataPermission = new GroupDataPermission()
            ////            {
            ////                CategoryIdList = checkedCategoryNode.Value,
            ////                RoleId = selectedRole
            ////            };

            ////        this.groupDataPermissionService.Insert(groupDataPermission);
            ////    }
            ////}
        }

        protected void radTreeFolder_NodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            var selectedFolderNodes = this.radTreeFolder.CheckedNodes.ToList();
            ////var selectedFolder = string.Empty;
            var selectedFolder = new List<string>();
            foreach (RadTreeNode folderNode in selectedFolderNodes)
            {
                ////selectedFolder += folderNode.Value + ",";
                selectedFolder.Add(folderNode.Value);
            }

            this.Session[this.radTreeCategories.SelectedNode.Value] = selectedFolder;
        }

        protected void radTreeCategories_NodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Checked)
            {
                if (this.Session[e.Node.Value] == null)
                {
                    Session.Add(e.Node.Value, string.Empty);
                }    
            }
            else
            {
                Session.Remove(e.Node.Value);

                var listFolder = this.folderService.GetAllByCategory(Convert.ToInt32(e.Node.Value));

                if (this.Session[e.Node.Value] == null)
                {
                    Session.Add(e.Node.Value, string.Empty);
                }

                var selectedFolder = Session[e.Node.Value].ToString().Split(',').Where(t => !string.IsNullOrEmpty(t)).ToList();

                var folderPermissions = listFolder.Select(folder => new FolderPermissionWrapper
                {
                    Folder = folder,
                    IsPermitted = selectedFolder.Any(folderId => folderId == folder.ID.ToString())
                });
                this.radTreeFolder.DataSource = folderPermissions;
                this.radTreeFolder.DataFieldParentID = "ParentID";
                this.radTreeFolder.DataTextField = "Name";
                this.radTreeFolder.DataValueField = "ID";
                this.radTreeFolder.DataFieldID = "ID";
                this.radTreeFolder.DataBind();
                
            }
        }
    }

    public class FolderPermissionWrapper
    {
        #region Properties

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public Data.Entities.Folder Folder { get; set; }

        public int ID
        {
            get { return this.Folder.ID; }
        }

        public int? ParentID
        {
            get { return this.Folder.ParentID; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is permitted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is permitted; otherwise, <c>false</c>.
        /// </value>
        public bool IsPermitted { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Name
        {
            get { return Folder.Name; }
        }

        #endregion

    }

    public class CategoryPermissionWrapper
    {
        #region Properties

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public Category Category { get; set; }

        public int ID
        {
            get { return this.Category.ID; }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is permitted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is permitted; otherwise, <c>false</c>.
        /// </value>
        public bool IsPermitted { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Name
        {
            get { return this.Category.Name; }
        }

        #endregion

    }
}

