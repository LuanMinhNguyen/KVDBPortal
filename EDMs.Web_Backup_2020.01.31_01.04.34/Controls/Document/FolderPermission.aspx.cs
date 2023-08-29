// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Document;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using System.Web.UI.HtmlControls;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class FolderPermission : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly GroupDataPermissionService groupDataPermissionService;

        /// <summary>
        /// The user data permission.
        /// </summary>
        private readonly UserDataPermissionService userDataPermissionService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService;

        /// <summary>
        /// The document service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ScopeProjectService scopeProjectService;
        private bool ProjectRoot;
        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private List<int> AdminGroup
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("GroupAdminList").Split(',').Select(t => Convert.ToInt32(t)).ToList();
            }
        } 


        /// <summary>
        /// Initializes a new instance of the <see cref="FolderPermission"/> class.
        /// </summary>
        public FolderPermission()
        {
            this.groupDataPermissionService = new GroupDataPermissionService();
            this.folderService = new FolderService();
            this.documentService = new DocumentService();
            this.roleService = new RoleService();
            this.userService = new UserService();
            this.userDataPermissionService = new UserDataPermissionService();
            this.scopeProjectService = new ScopeProjectService();
           
           
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
                //this.ProjectRoot = true;
                if (!string.IsNullOrEmpty(Request.QueryString["folderId"]))
                {
                    this.lblFolderDirName.Text = "<font style='text-decoration: underline;'>Folder</font>: ";
                    var folderId = Request.QueryString["folderId"];
                    var folderNeedToMove = this.folderService.GetById(Convert.ToInt32(folderId));
                    if (folderNeedToMove != null)
                    {
                        this.lblFolderDirName.Text += folderNeedToMove.DirName;
                        this.LoadComboData();
                        //this.LoadPermissionData(folderId.Trim());

                      
                        var listFolder = this.folderService.GetAll().Where(t => t.ParentID == 1276).Select(t => t.ID).ToList();                    
                        if (!listFolder.Contains(Convert.ToInt32(folderId)) )
                        {
                            var IDProject = (HtmlGenericControl)this.divContent.FindControl("IDProject");
                            if (IDProject != null)
                            {
                                //IDProject.Visible = false;
                                this.ProjectRoot = false;
                            }
                        }
                        if (folderNeedToMove.ProjectId != null && folderNeedToMove.ProjectId != 0)
                        {
                            //IDProject.Visible = false;
                           // this.ProjectRoot = false;
                        }
                    }
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["folderId"]))
            {
                var listFolderId = new List<int>();
                var selectedFolderId = Request.QueryString["folderId"];
                var selectedFolder = this.folderService.GetById(Convert.ToInt32(selectedFolderId));

                var selectedGroup = this.ddlGroup.SelectedValue;
                var selectedUser = this.ddlUser.SelectedValue;
                var alluser=this.cbApplyAllUser.Checked;

                if (selectedFolder != null)
                {
                    var categoryId = selectedFolder.CategoryID;
                    var listParentFolderId = this.GetAllParentID(Convert.ToInt32(selectedFolderId), listFolderId);
                    listParentFolderId.Add(Convert.ToInt32(selectedFolderId));
                    ////var cookie = Request.Cookies["allchildfolder"];
                    var allChildFolderSession = Session["allChildFolder"];
                    var allChildFolderWithoutNativeFileFolderSession = Session["allChildFolderWithoutNativeFileFolder"];

                    //if (selectedUser == "0")
                    //{
                    //}
                    //else 
                    if (selectedUser != "0" && !alluser)
                    {
                        var userDatapermission =
                            this.userDataPermissionService.GetByUserId(Convert.ToInt32(selectedUser));
                        var addingPermission =
                            listParentFolderId.Where(
                                t => !userDatapermission.Select(x => x.FolderId.ToString()).Contains(t.ToString())).Select(
                                    t =>
                                    new UserDataPermission()
                                    {
                                        CategoryId = categoryId,
                                        RoleId = Convert.ToInt32(selectedGroup),
                                        FolderId = t,
                                        UserId = Convert.ToInt32(selectedUser),
                                        IsFullPermission = this.rdbFullPermission.Checked,
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = UserSession.Current.User.Id
                                       
                                    }).ToList();

                        if (this.cbApplyAll.Checked)
                        {
                            if (allChildFolderSession != null)
                            {
                                var allChildNodes = this.rdbFullPermission.Checked
                                                    ? (List<string>)allChildFolderSession
                                                    : (List<string>)allChildFolderWithoutNativeFileFolderSession;
                                  addingPermission.AddRange(
                                      allChildNodes.Select(
                                          t =>
                                          new UserDataPermission()
                                          {
                                              CategoryId = categoryId,
                                              RoleId = Convert.ToInt32(selectedGroup),
                                              FolderId = Convert.ToInt32(t),
                                              UserId = Convert.ToInt32(selectedUser),
                                              IsFullPermission = this.rdbFullPermission.Checked,
                                              CreatedDate = DateTime.Now,
                                              CreatedBy = UserSession.Current.User.Id
                                          }));
                               
                               
                            }
                        }

                        this.userDataPermissionService.AddUserDataPermissions(addingPermission.ToList());
                    }
                    else if(alluser==true)
                    {

                        var usersInPermission_ = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"])).Select(t => t.UserId).Distinct().ToList();
                        var listUser_ = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlGroup.SelectedValue)).Where(t => !usersInPermission_.Contains(t.Id)).Select(t=> t.Id).ToList();

                        foreach (var id in listUser_)
                        {
                            var userDatapermission =
                                                        this.userDataPermissionService.GetByUserId(Convert.ToInt32(selectedUser));
                            var addingPermission =
                                listParentFolderId.Where(
                                    t => !userDatapermission.Select(x => x.FolderId.ToString()).Contains(t.ToString())).Select(
                                        t =>
                                        new UserDataPermission()
                                        {
                                            CategoryId = categoryId,
                                            RoleId = Convert.ToInt32(selectedGroup),
                                            FolderId = t,
                                            UserId = id,
                                            IsFullPermission = this.rdbFullPermission.Checked,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = UserSession.Current.User.Id
                                        }).ToList();

                            if (this.cbApplyAll.Checked)
                            {
                                if (allChildFolderSession != null)
                                {
                                    var allChildNodes = this.rdbFullPermission.Checked
                                                        ? (List<string>)allChildFolderSession
                                                        : (List<string>)allChildFolderWithoutNativeFileFolderSession;

                                    addingPermission.AddRange(
                                        allChildNodes.Select(
                                            t =>
                                            new UserDataPermission()
                                            {
                                                CategoryId = categoryId,
                                                RoleId = Convert.ToInt32(selectedGroup),
                                                FolderId = Convert.ToInt32(t),
                                                UserId = id,
                                                IsFullPermission = this.rdbFullPermission.Checked,
                                                CreatedDate = DateTime.Now,
                                                CreatedBy = UserSession.Current.User.Id
                                            }));
                                }
                            }

                            this.userDataPermissionService.AddUserDataPermissions(addingPermission.ToList());

                        }


                    }
                  

                   
                    

                    // Reload combobox User
                    var usersInPermission = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"])).Select(t => t.UserId).Distinct().ToList();
                    var listUser = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlGroup.SelectedValue)).Where(t => !usersInPermission.Contains(t.Id)).ToList();

                    listUser.Insert(0, new User { Id = 0, FullName = string.Empty });

                    this.ddlUser.DataSource = listUser;
                    this.ddlUser.DataTextField = "FullName";
                    this.ddlUser.DataValueField = "Id";
                    this.ddlUser.DataBind();

                    this.grdPermission.Rebind();

                    //Session.Remove("allChildFolder");
                    // Session.Remove("allChildFolderWithoutNativeFileFolder");
                }
            }
        }

        private List<int> GetAllParentID(int folderId, List<int> listFolderId)
        {
            var folder = this.folderService.GetById(folderId);
            if (folder.ParentID != null)
            {
                listFolderId.Add(folder.ParentID.Value);
                this.GetAllParentID(folder.ParentID.Value, listFolderId);
            }

            return listFolderId;
        }

        private void LoadComboData()
        {
            //var groupsInPermission = this.groupDataPermissionService.GetAllByFolder(Request.QueryString["folderId"]).Select(t => t.RoleId).Distinct().ToList();
            var listGroup = this.roleService.GetAll(false).Where(t => !this.AdminGroup.Contains(t.Id)).ToList();
            listGroup.Insert(0, new Role { Id = 0 });
            this.ddlGroup.DataSource = listGroup;
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "Id";
            this.ddlGroup.DataBind();

            var usersInPermission = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"])).Select(t => t.UserId).Distinct().ToList();
            var listUser = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlGroup.SelectedValue)).Where(t => !usersInPermission.Contains(t.Id)).ToList();

            listUser.Insert(0, new User { Id = 0, FullName = string.Empty });

            this.ddlUser.DataSource = listUser;
            this.ddlUser.DataTextField = "FullName";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();

        }

        protected void grdPermission_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["folderId"]))
            {
                
                var usersInPermission = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"]));

                var listDataPermission = usersInPermission
                                        .Select(t => new Data.Entities.FolderPermissionItem
                                                { 
                                                    ID = t.ID, 
                                                    IsFullPermission = t.IsFullPermission.GetValueOrDefault(), 
                                                    Name = t.UserFullName, 
                                                    IsGroup = false ,
                                                    roleId = t.RoleId.GetValueOrDefault()
                                                }).ToList();

                this.grdPermission.DataSource = this.ddlGroup.SelectedValue != null && this.ddlGroup.SelectedValue != "0" ? listDataPermission.Where(t => t.roleId == Convert.ToInt32(this.ddlGroup.SelectedValue)) : listDataPermission;
            }
            else
            {
                this.grdPermission.DataSource = new List<UserDataPermission>();    
            }
        }

        protected void ddlGroup_SelectedIndexChange(object sender, EventArgs e)
        {
            var usersInPermission = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"])).Select(t => t.UserId).Distinct().ToList();
            var listUser = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlGroup.SelectedValue)).Where(t => !usersInPermission.Contains(t.Id)).ToList();

            listUser.Insert(0, new User { Id = 0, FullName = string.Empty });

            this.ddlUser.DataSource = listUser;
            this.ddlUser.DataTextField = "FullName";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();

            this.grdPermission.Rebind();
        }

        protected void grdPermission_OnDeteleCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var permissionId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var isGroup = Convert.ToBoolean(item["IsGroup"].Text);
            var selectedFolderId = Request.QueryString["folderId"];
            var allChildFolderSession = Session["allChildFolder"];

            ////var cookie = Request.Cookies["allchildfolder"];

            if (isGroup)
            {
                var groupPermission = this.groupDataPermissionService.GetById(permissionId);
                if (groupPermission != null)
                {
                    var groupDatapermission = this.groupDataPermissionService.GetByRoleId(groupPermission.RoleId.GetValueOrDefault());
                    if (allChildFolderSession != null)
                    {
                        var allChildNodes = (List<string>)allChildFolderSession;
                        ////allChildNodes.Add(selectedFolderId);
                        
                        var deletePermission = groupDatapermission.Where(t => allChildNodes.Contains(t.FolderIdList) || t.FolderIdList == selectedFolderId);
                        this.groupDataPermissionService.DeleteGroupDataPermission(deletePermission.ToList());
                    }
                }
            }
            else
            {
                var userPermission = this.userDataPermissionService.GetById(permissionId);
                if (userPermission != null)
                {
                    var userDatapermission = this.userDataPermissionService.GetByUserId(userPermission.UserId.GetValueOrDefault());
                    if (allChildFolderSession != null)
                    {
                        var allChildNodes = (List<string>)allChildFolderSession;
                        ////allChildNodes.Add(selectedFolderId);

                        var deletePermission = userDatapermission.Where(t => allChildNodes.Contains(t.FolderId.GetValueOrDefault().ToString()) || t.FolderId.ToString() == selectedFolderId);
                        this.userDataPermissionService.DeleteUserDataPermission(deletePermission.ToList());
                    }
                }
            }

            // Reload combobox User
            var usersInPermission = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"])).Select(t => t.UserId).Distinct().ToList();
            var listUser = this.userService.GetAllByRoleId(Convert.ToInt32(this.ddlGroup.SelectedValue)).Where(t => !usersInPermission.Contains(t.Id)).ToList();

            listUser.Insert(0, new User { Id = 0, FullName = string.Empty });

            this.ddlUser.DataSource = listUser;
            this.ddlUser.DataTextField = "FullName";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();
        }

        protected void cbApplyAllUser_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbApplyAllUser.Checked == true)
            {this.ddlUser.Enabled = false;

            }
            else
            {
                  this.ddlUser.SelectedIndex = 0;
                this.ddlUser.Enabled = true;
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            //string confirmValue = Request.Form["confirm_value"];
            //if (confirmValue == "Yes")
            //{
                if (!string.IsNullOrEmpty(Request.QueryString["folderId"]))
                {

                    var usersInPermission = this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(Request.QueryString["folderId"]));

                    var listDataPermission = usersInPermission
                                            .Select(t => new Data.Entities.FolderPermissionItem
                                            {
                                                ID = t.ID,
                                                IsFullPermission = t.IsFullPermission.GetValueOrDefault(),
                                                Name = t.UserFullName,
                                                IsGroup = false,
                                                roleId = (int)t.RoleId
                                            }).ToList();

                    var listuser = this.ddlGroup.SelectedValue != null && this.ddlGroup.SelectedValue != "0" ? listDataPermission.Where(t => t.roleId == Convert.ToInt32(this.ddlGroup.SelectedValue)) : listDataPermission;
                    var selectedFolderId = Request.QueryString["folderId"];
                    var allChildFolderSession = Session["allChildFolder"];
                    foreach (var item in listuser)
                    {
                        var userPermission = this.userDataPermissionService.GetById(item.ID);
                        if (userPermission != null)
                        {
                            var userDatapermission = this.userDataPermissionService.GetByUserId(userPermission.UserId.GetValueOrDefault());
                            if (allChildFolderSession != null)
                            {
                                var allChildNodes = (List<string>)allChildFolderSession;

                                var deletePermission = userDatapermission.Where(t => allChildNodes.Contains(t.FolderId.GetValueOrDefault().ToString()) || t.FolderId.ToString() == selectedFolderId);
                                this.userDataPermissionService.DeleteUserDataPermission(deletePermission.ToList());
                            }
                        }
                    }
                    this.grdPermission.Rebind();
                }
           // }
        }
    }
}