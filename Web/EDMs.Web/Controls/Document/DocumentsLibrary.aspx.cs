// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Resources;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Telerik.Windows.Zip;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Label = System.Web.UI.WebControls.Label;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class DocumentsLibrary : Page
    {
        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService = new RevisionService();

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

        /// <summary>
        /// The status service.
        /// </summary>
        private readonly StatusService statusService = new StatusService();

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService = new DisciplineService();

        /// <summary>
        /// The received from.
        /// </summary>
        private readonly ReceivedFromService receivedFromService = new ReceivedFromService();
        private List<int> AdminGroup
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("GroupAdminList").Split(',').Select(t => Convert.ToInt32(t)).ToList();
            }
        }

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService = new FolderService();

        private readonly DocumentService documentService = new DocumentService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly  CategoryService categoryService = new CategoryService();

        private readonly UserService userService = new UserService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

        private const string RegexValidateEmail =
            @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        /// <summary>
        /// The list folder id.
        /// </summary>
        private List<int> listFolderId = new List<int>();

        private readonly UserDataPermissionService userDataPermissionService = new UserDataPermissionService();
        private readonly RoleService roleService = new RoleService();

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
            Session.Add("SelectedMainMenu", "Document Management");
            this.CurrentRoleId.Value = UserSession.Current.RoleId.ToString();
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;
            if (!Page.IsPostBack)
            {
                this.LoadListPanel();
                this.LoadSystemPanel();
                this.LoadTreeFolder();

                if (!UserSession.Current.User.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.IsDC.GetValueOrDefault())
                {
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = false;
                    //this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = false;
                    this.IsUpdatePermission.Value = "false";
                    this.CustomerMenu.Items[0].Visible = false;
                    ((RadToolBarDropDown) this.CustomerMenu.Items[2]).Buttons[1].Visible = false;
                    ((RadToolBarDropDown)this.CustomerMenu.Items[2]).Buttons[2].Visible = false;
                    ((RadToolBarDropDown)this.CustomerMenu.Items[2]).Buttons[3].Visible = false;

                }
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
        protected void radTreeFolder_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            //this.LoadActionPermission(e.Node.Value);

            e.Node.Expanded = true;

            var folder = this.folderService.GetById(Convert.ToInt32(e.Node.Value));
            var temp = (RadToolBarButton)this.CustomerMenu.FindItemByText("View explorer");
            if (temp != null && folder != null)
            {
                temp.NavigateUrl = ConfigurationSettings.AppSettings.Get("ServerName") + folder.DirName.Replace("../../", string.Empty);
            }

            ////if (e.Node.GetAllNodes().Count == 0)
            ////{
            ////    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = true;
            ////    this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = true;
            ////    this.grdDocument.CurrentPageIndex = 0;
            ////    this.LoadDocuments(true, true);
            ////}
            ////else
            ////{
            ////    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = false;
            ////    this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = false;

            ////    var childFolders = e.Node.GetAllNodes().Where(t => t.Level == e.Node.Level + 1).ToList();
            ////    var listFolder = childFolders.Select(t => new Data.Entities.Document()
            ////        {
            ////            ID = Convert.ToInt32(t.Value),
            ////            Name = t.Text,
            ////            FileExtensionIcon = "~/Images/folderdir16.png",
            ////            IsFolder = true
            ////        });

            ////    this.grdDocument.CurrentPageIndex = 0;
            ////    this.grdDocument.DataSource = listFolder;
            ////    this.grdDocument.DataBind();
            ////}

            this.grdDocument.Rebind();

            if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
            {
                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = false;
            }
        }

        /// <summary>
        /// The rad tree folder_ node edit.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void radTreeFolder_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
        {
            e.Node.Text = e.Text;
            var folder = new Folder();
            
            if (!string.IsNullOrEmpty(Session["FolderMenuAction"].ToString()) && Session["FolderMenuAction"].ToString() == "New")
            {
                try
                {
                    var parentFol = this.folderService.GetById(Convert.ToInt32(e.Node.ParentNode.Value));
                    folder = new Folder()
                        {
                            Name = e.Text,
                            Description = e.Text,
                            ParentID = Convert.ToInt32(e.Node.ParentNode.Value),
                            DirName = parentFol.DirName + "/" + Regex.Replace(e.Text, @"[^0-9a-zA-Z]+", string.Empty),
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedDate = DateTime.Now,
                            ProjectId=parentFol.ProjectId,
                            ProjectName=parentFol.ProjectName
                        };

                    Directory.CreateDirectory(Server.MapPath(folder.DirName));
                    var folderId = this.folderService.Insert(folder);
                    var usersInPermissionOfParent =
                        this.userDataPermissionService.GetAllByFolder(Convert.ToInt32(e.Node.ParentNode.Value));
                    foreach (var parentPermission in usersInPermissionOfParent)
                    {
                        var childPermission = new UserDataPermission()
                        {
                            CategoryId = parentPermission.CategoryId,
                            RoleId = parentPermission.RoleId,
                            FolderId = folderId,
                            UserId = parentPermission.UserId,
                            IsFullPermission = parentPermission.IsFullPermission,
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserSession.Current.User.Id
                        };

                        this.userDataPermissionService.Insert(childPermission);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (!string.IsNullOrEmpty(Session["FolderMenuAction"].ToString()) && Session["FolderMenuAction"].ToString() == "Rename")
            {
                try
                {
                    folder = this.folderService.GetById(Convert.ToInt32(e.Node.Value));
                
                    var oldDirName = folder.DirName;
                    var newDirName = folder.DirName.Substring(0, folder.DirName.LastIndexOf('/') + 1) + this.RemoveAllSpecialCharacter(e.Text);
                    var oldPath = Server.MapPath(folder.DirName);
                    var newPath = Server.MapPath(newDirName);

                    folder.Name = e.Text;
                    folder.DirName = newDirName;

                    if (oldPath != newPath && Directory.Exists(oldPath))
                    {
                        Directory.Move(oldPath, newPath);
                    }

                    this.folderService.Update(folder);

                    foreach (var childNode in e.Node.GetAllNodes())
                    {
                        var childFolder = this.folderService.GetById(Convert.ToInt32(childNode.Value));
                        if (childFolder != null)
                        {
                            childFolder.DirName = childFolder.DirName.Replace(oldDirName, newDirName);
                            this.folderService.Update(childFolder);
                        }
                    }

                    var selectedFolder = this.radTreeFolder.FindNodeByValue(e.Node.Value);
                    var tempListFolderId = new List<int>();

                    tempListFolderId.AddRange(selectedFolder.GetAllNodes().Select(t => Convert.ToInt32(t.Value)));

                    tempListFolderId.Add(folder.ID);

                    var listDocuments = this.documentService.GetAllByFolder(tempListFolderId);
                    foreach (var document in listDocuments)
                    {
                        //document.DirName = document.DirName.Replace(oldDirName, newDirName);
                        document.FilePath = document.FilePath.Replace(oldDirName, newDirName);
                        document.LastUpdatedBy = UserSession.Current.User.Id;
                        document.LastUpdatedDate = DateTime.Now;

                        this.documentService.Update(document);
                    }
                }
                catch (Exception ex)
                {
                    var watcherService = new ServiceController("EDMSFolderWatcher");
                    if (Utility.ServiceIsAvailable("EDMSFolderWatcher"))
                    {
                        watcherService.ExecuteCommand(129);
                    }
                }
            }

            Session.Remove("FolderMenuAction");

            this.LoadTreeFolder();
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
        }

        protected void radTreeFolder_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            var clickedNode = e.Node;

            switch (e.MenuItem.Value)
            {
                case "New":
                    Session.Add("FolderMenuAction", "New");
                    var newFolder = new RadTreeNode(string.Format("New Folder {0}", clickedNode.Nodes.Count + 1))
                        {
                            Selected = true, 
                            ImageUrl = clickedNode.ImageUrl
                        };

                    clickedNode.Nodes.Add(newFolder);
                    clickedNode.Expanded = true;
                    
                    // update the number in the brackets
                    if (Regex.IsMatch(clickedNode.Text, UnreadPattern))
                    {
                        clickedNode.Text = Regex.Replace(
                            clickedNode.Text, UnreadPattern, "(" + clickedNode.Nodes.Count.ToString() + ")");
                    }
                    else
                    {
                        clickedNode.Text += string.Format(" ({0})", clickedNode.Nodes.Count);
                    }

                    clickedNode.Font.Bold = true;
                    
                    // set node's value so we can find it in startNodeInEditMode
                    newFolder.Value = newFolder.GetFullPath("/");
                    this.startNodeInEditMode(newFolder.Value);
                    break;
                case "Rename":
                    Session.Add("FolderMenuAction", "Rename");
                    this.startNodeInEditMode(clickedNode.Value);
                    break;
                case "Delete":
                    Session.Add("FolderMenuAction", "Delete");
                    var folDelete = this.folderService.GetById(Convert.ToInt32(clickedNode.Value));
                    var folderIDs = clickedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value)).ToList();
                    folderIDs.Add(Convert.ToInt32(clickedNode.Value));

                    if (folDelete != null)
                    {
                        this.folderService.Delete(folDelete);

                        // Delete all child Folder
                        foreach (var childNode in clickedNode.GetAllNodes())
                        {
                            var childFolder = this.folderService.GetById(Convert.ToInt32(childNode.Value));
                            if (childFolder != null)
                            {
                                this.folderService.Delete(childFolder);
                            }
                        }

                        var docList = this.documentService.GetAllByFolder(folderIDs);
                        foreach (var document in docList)
                        {
                            document.IsDelete = true;
                            this.documentService.Update(document);
                        }

                        if (Directory.Exists(Server.MapPath(folDelete.DirName)))
                        {
                        //    Directory.Delete(Server.MapPath(folDelete.DirName), true); //tạm thoi an de kiem tra
                        }    
                    }

                    this.LoadTreeFolder();

                    this.grdDocument.CurrentPageIndex = 0;
                    this.grdDocument.Rebind();
                    break;
                case "Permission":
                    var allChildFolder = e.Node.GetAllNodes().Select(t => t.Value).ToList();
                    var allChildFolderWithoutNativeFileFolder = e.Node.GetAllNodes().Where(t => !t.Text.ToLower().Contains("native file")).Select(t => t.Value).ToList();
                    Session.Add("allChildFolder", allChildFolder);
                    Session.Add("allChildFolderWithoutNativeFileFolder", allChildFolderWithoutNativeFileFolder);
                    break;
            }
        }

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false, bool isListAll = false)
        {
            if (this.radTreeFolder.SelectedNode != null)
            {
                try
                {
                    var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
                    var tempListFolderId = new List<int>();
                    var selectedFolder = this.radTreeFolder.SelectedNode;
                    var listDocuments = new List<Data.Entities.Document>();

                    var folderPermission = this.userDataPermissionService.GetByUserId(UserSession.Current.User.Id)
                                                            .Select(t => t.FolderId.GetValueOrDefault()).Distinct().ToList();

                    if (selectedFolder.GetAllNodes().Count > 0)
                    {
                        tempListFolderId.AddRange(
                            UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                ? selectedFolder.GetAllNodes().Select(t => Convert.ToInt32(t.Value))
                                : selectedFolder.GetAllNodes()
                                    .Where(t => folderPermission.Contains(Convert.ToInt32(t.Value)))
                                    .Select(t => Convert.ToInt32(t.Value)));
                           

                        tempListFolderId.Add(folderId);
                        listDocuments = this.documentService.GetAllByFolder(tempListFolderId);
                    }
                    else
                    {
                        listDocuments = this.documentService.GetAllByFolder(folderId);
                    }

                    

                    this.grdDocument.DataSource = listDocuments;

                    if (isbind)
                    {
                        this.grdDocument.DataBind();
                    }
                }
                catch (Exception)
                {
                    this.grdDocument.DataSource = new List<Data.Entities.Document>();
                }
            }
            else
            {
                this.grdDocument.DataSource = new List<Data.Entities.Document>();
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
            if (e.Argument.Contains("ShowChildFolder"))
            {
                var folderId = e.Argument.Split('_')[1];
                var selectedFolder = this.radTreeFolder.FindNodeByValue(folderId);
                if (selectedFolder != null)
                {
                    this.LoadActionPermission(selectedFolder.Value);

                    selectedFolder.Selected = true;
                    selectedFolder.Expanded = true;
                    if (selectedFolder.GetAllNodes().Count == 0)
                    {
                        var folder = this.folderService.GetById(Convert.ToInt32(selectedFolder.Value));
                        //var temp = (RadToolBarButton)this.CustomerMenu.FindItemByText("View explorer");
                        //temp.NavigateUrl = ConfigurationSettings.AppSettings.Get("ServerName") + folder.DirName;

                        this.grdDocument.CurrentPageIndex = 0;
                        this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = true;
                        this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = true;

                        this.LoadDocuments(true, true);
                    }
                    else
                    {
                        this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = false;
                        this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = false;

                        var childFolders = selectedFolder.GetAllNodes().Where(t => t.Level == selectedFolder.Level + 1).ToList();
                        var listFolder = childFolders.Select(t => new Data.Entities.Document
                        {
                            ID = Convert.ToInt32(t.Value),
                            Name = t.Text,
                            FileExtensionIcon = "../../Images/folderdir16.png",
                            IsFolder = true
                        });

                        this.grdDocument.CurrentPageIndex = 0;
                        this.grdDocument.DataSource = listFolder;
                        this.grdDocument.DataBind();
                    }
                }
            }
            else if (e.Argument.Contains("Rebind"))
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("DeleteAll"))
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    var docId = Convert.ToInt32(selectedItem.GetDataKeyValue("ID"));
                    var docObj = this.documentService.GetById(docId);

                    if (docObj != null)
                    {
                        var filePathServer = Server.MapPath(docObj.FilePath);
                        if (File.Exists(filePathServer))
                        {
                            File.Delete(filePathServer);
                        }

                        this.documentService.Delete(docObj);
                    }
                }

                this.grdDocument.Rebind();
            }

            else if (e.Argument == "DownloadMulti")
            {
                if (this.radTreeFolder.SelectedNode != null && this.radTreeFolder.SelectedNode.GetAllNodes().Count == 0)
                {
                    var serverTotalDocPackPath = Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_DocPack.rar");
                    var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);

                    foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                    {
                        var docId = Convert.ToInt32(selectedItem.GetDataKeyValue("ID"));

                        var document = this.documentService.GetById(docId);
                        if (File.Exists(Server.MapPath(document.FilePath)))
                        {
                            docPack.Add(Server.MapPath(document.FilePath));
                        }

                        selectedItem.Selected = false;
                    }

                    this.DownloadByWriteByte(serverTotalDocPackPath,
                        this.radTreeFolder.SelectedNode.ParentNode.Text + " - " + this.radTreeFolder.SelectedNode.Text +
                        "_DocPack.rar", true);
                }
            }

            if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
            {
                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = false;
                //this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = false;
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
            ////var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            var pageSize = this.grdDocument.PageSize;
            var currentPage = this.grdDocument.CurrentPageIndex;
            var startingRecordNumber = currentPage * pageSize;

            this.LoadDocuments(false, true);

            ////var expression = new GridGroupByExpression();
            ////var gridGroupByField = new GridGroupByField { FieldAlias = "Folder", FieldName = "DirName" };

            ////expression.GroupByFields.Add(gridGroupByField);
            ////this.grdDocument.MasterTableView.GroupByExpressions.Add(expression);
        }

        private void LoadTreeFolder()
        {
            var folderPermission = this.userDataPermissionService.GetByUserId(UserSession.Current.User.Id)
                                                            .Select(t => t.FolderId.GetValueOrDefault()).Distinct().ToList();

            var listFolder = UserSession.Current.User.Role.Name=="Admin" && UserSession.Current.User.Role.Id==1
                            ? this.folderService.GetAll()
                            : this.folderService.GetSpecificFolderStatic(folderPermission);

            this.radTreeFolder.DataSource = listFolder;
            this.radTreeFolder.DataFieldParentID = "ParentID";
            this.radTreeFolder.DataTextField = "Name";
            this.radTreeFolder.DataValueField = "ID";
            this.radTreeFolder.DataFieldID = "ID";
            this.radTreeFolder.DataBind();
            if (this.radTreeFolder.Nodes.Count > 0)
            {
                this.radTreeFolder.Nodes[0].Expanded = true;
            }

            this.RestoreExpandStateTreeView();

            this.SortNodes(this.radTreeFolder.Nodes);
        }


        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/folderdir16.png";
        }

        /// <summary>
        /// The load action permission.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        private void LoadActionPermission(string folderId)
        {
            //if (this.AdminGroup.Contains(UserSession.Current.RoleId))
            //{
            //    this.CustomerMenu.Items[0].Visible = true;
            //    this.CustomerMenu.Items[1].Visible = true;
            //    this.CustomerMenu.Items[2].Visible = true;
            //    this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = true;
            //    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = true;
            //}
            //else
            //{
            //    var groupPermission = this.groupDataPermissionService.GetByRoleId(UserSession.Current.RoleId, this.lblCategoryId.Value, folderId);
            //    if (groupPermission != null && groupPermission.IsFullPermission.GetValueOrDefault())
            //    {
            //        this.CustomerMenu.Items[0].Visible = true;
            //        this.CustomerMenu.Items[1].Visible = true;
            //        this.CustomerMenu.Items[2].Visible = true;
            //        this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = true;
            //        this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = true;
            //    }
            //    else
            //    {
            //        var userPermission = this.userDataPermissionService.GetByUserId(
            //            UserSession.Current.User.Id,
            //            Convert.ToInt32(this.lblCategoryId.Value),
            //            Convert.ToInt32(folderId));
            //        if (userPermission != null && userPermission.IsFullPermission.GetValueOrDefault())
            //        {
            //            this.CustomerMenu.Items[0].Visible = true;
            //            this.CustomerMenu.Items[1].Visible = true;
            //            this.CustomerMenu.Items[2].Visible = true;
            //            this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = true;
            //            this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = true;
            //        }
            //        else
            //        {
            //            this.CustomerMenu.Items[0].Visible = false;
            //            this.CustomerMenu.Items[1].Visible = false;
            //            this.CustomerMenu.Items[2].Visible = false;
            //            this.grdDocument.MasterTableView.GetColumn("IsSelected").Display = false;
            //            this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Display = false;
            //        }
            //    }
            //}
        }
        
        //SortNodes is a recursive method enumerating and sorting all node levels 
        private void SortNodes(RadTreeNodeCollection collection)
        {
            Sort(collection);
            foreach (RadTreeNode node in collection)
            {
                if (node.Nodes.Count > 0)
                {
                    SortNodes(node.Nodes);
                }
            }
        }

        //The Sort method is called for each node level sorting the child nodes 
        public void Sort(RadTreeNodeCollection collection)
        {
            RadTreeNode[] nodes = new RadTreeNode[collection.Count];
            collection.CopyTo(nodes, 0);
            Array.Sort(nodes, new TreeNodeComparer());
            collection.Clear();
            collection.AddRange(nodes);
        } 

        

        private void startNodeInEditMode(string nodeValue)
        {
            //find the node by its Value and edit it when page loads
            string js = "Sys.Application.add_load(editNode); function editNode(){ ";
            js += "var tree = $find(\"" + radTreeFolder.ClientID + "\");";
            js += "var node = tree.findNodeByValue('" + nodeValue + "');";
            js += "if (node) node.startEdit();";
            js += "Sys.Application.remove_load(editNode);};";

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit", js, true);
        }

        /// <summary>
        /// The restore expand state tree view.
        /// </summary>
        private void RestoreExpandStateTreeView()
        {
            // Restore expand state of tree folder
            HttpCookie cookie = Request.Cookies["expandedNodes"];
            if (cookie != null)
            {
                var expandedNodeValues = cookie.Value.Split('*');
                foreach (var nodeValue in expandedNodeValues)
                {
                    RadTreeNode expandedNode = this.radTreeFolder.FindNodeByValue(HttpUtility.UrlDecode(nodeValue));
                    if (expandedNode != null)
                    {
                        expandedNode.Expanded = true;
                    }
                }
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

                nodetemp.ImageUrl = "~/Images/folderdir16.png";
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

        private void CreateValidation(string formular, ValidationCollection objValidations, int startRow, int endRow, int startColumn, int endColumn)
        {
            // Create a new validation to the validations list.
            Validation validation = objValidations[objValidations.Add()];

            // Set the validation type.
            validation.Type = Aspose.Cells.ValidationType.List;

            // Set the operator.
            validation.Operator = OperatorType.None;

            // Set the in cell drop down.
            validation.InCellDropDown = true;

            // Set the formula1.
            validation.Formula1 = "=" + formular;

            // Enable it to show error.
            validation.ShowError = true;

            // Set the alert type severity level.
            validation.AlertStyle = ValidationAlertType.Stop;

            // Set the error title.
            validation.ErrorTitle = "Error";

            // Set the error message.
            switch (formular)
            {
                case "RevisionList":
                    validation.ErrorMessage = "Please select a Revision from the list";
                    break;
                case "StatusList":
                    validation.ErrorMessage = "Please select a Status from the list";
                    break;
                case "DisciplineList":
                    validation.ErrorMessage = "Please select a Discipline from the list";
                    break;
                case "DocumentTypeList":
                    validation.ErrorMessage = "Please select a Document type from the list";
                    break;
                case "ReceivedFromList":
                    validation.ErrorMessage = "Please select a Received from from the list";
                    break;
            }
            
            // Specify the validation area.
            CellArea area;
            area.StartRow = startRow;
            area.EndRow = endRow;
            area.StartColumn = startColumn;
            area.EndColumn = endColumn;

            // Add the validation area.
            validation.AreaList.Add(area);

            ////return validation;
        }

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }
                //Mo file de doc
                FileStream fs = new FileStream(strFileName, FileMode.Open);
                int streamLength = Convert.ToInt32(fs.Length);
                byte[] data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private string RemoveAllSpecialCharacter(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z]+", string.Empty);
        }
        public class MyTemplateNode : ITemplate
        {
            private ResourceManager _resources;
            private string _culture;
            private string textNode;

            public MyTemplateNode(RadTreeNode node)
            {
                textNode = node.Text;
            }


            public void InstantiateIn(Control container)
            {

                Label label1 = new Label();
                label1.ID = "ItemLabel";
                label1.Text = textNode;
                label1.Font.Size = 10;
                label1.Font.Bold = true;
                label1.DataBinding += new EventHandler(label1_DataBinding);
                container.Controls.Add(label1);

                CustomValidator cv = new CustomValidator();
                cv.ID = "CustomValidator1";

                cv.ErrorMessage = "name already taken";
                cv.ServerValidate += Validate;

                container.Controls.Add(cv);
            }

            private void Validate(object source, ServerValidateEventArgs args)
            {
                bool isUsed = false;

                var node = (RadTreeNode)((Label)source).Parent;
                RadTreeView tv = node.TreeView;

                for (int i = 0; i < tv.GetAllNodes().Count; i++)
                {
                    if (node == editedNode && tv.GetAllNodes()[i].Text == node.Text && tv.GetAllNodes()[i] != node)
                    {
                        isUsed = true;
                        break;
                    }
                }
                args.IsValid = !isUsed;
            }

            private void label1_DataBinding(object sender, EventArgs e)
            {
                Label target = (Label)sender;
                RadTreeNode node = (RadTreeNode)target.BindingContainer;
                string nodeText = (string)DataBinder.Eval(node, node.Text);
                target.Text = nodeText;
            }
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdDocument.Rebind();
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
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var docObj = this.documentService.GetById(docId);
            if (docObj != null)
            {
                 var filePathServer = Server.MapPath(docObj.FilePath);
                if (File.Exists(filePathServer))
                {
                    File.Delete(filePathServer);
                }

                docObj.IsDelete = true;
                this.documentService.Update(docObj);
            }
        }
    }

    //The TreeNodeComparer class defines the sorting criteria 
    class TreeNodeComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            RadTreeNode firstNode = (RadTreeNode)x;
            RadTreeNode secondNode = (RadTreeNode)y;

            return firstNode.Text.CompareTo(secondNode.Text);
        }

        #endregion
    } 
}

