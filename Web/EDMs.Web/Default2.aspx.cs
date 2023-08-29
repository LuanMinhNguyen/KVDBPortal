// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class Default2 : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService = new FolderService();

        private readonly DocumentService documentService = new DocumentService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly  CategoryService categoryService = new CategoryService();

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
                    }    
                }
                

                if (!string.IsNullOrEmpty(Request.QueryString["doctype"]))
                {
                    var doctype = Convert.ToInt32(Request.QueryString["doctype"]);
                    var listFolder = this.folderService.GetAllByCategory(doctype);
                    this.radTreeFolder.DataSource = listFolder;
                    this.radTreeFolder.DataFieldParentID = "ParentID";
                    this.radTreeFolder.DataTextField = "Name";
                    this.radTreeFolder.DataValueField = "ID";
                    this.radTreeFolder.DataFieldID = "ID";
                    this.radTreeFolder.DataBind();

                    foreach (var node in this.radTreeFolder.Nodes)
                    {
                        ((RadTreeNode)node).ImageUrl = "Images/folderdir16.png";
                        ((RadTreeNode)node).Expanded = true;
                        this.CustomFolderTree((RadTreeNode)node);
                    }

                    this.radPbCategories.Items[0].Items[doctype - 2].Selected = true;
                    this.RestoreExpandStateTreeView();
                }
            }
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

                nodetemp.ImageUrl = "Images/folderdir16.png";
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
            var folder = this.folderService.GetById(Convert.ToInt32(e.Node.Value));
            var temp = (RadToolBarButton)this.CustomerMenu.FindItemByText("View explorer");
            temp.NavigateUrl = ConfigurationSettings.AppSettings.Get("ServerName") + folder.DirName;
            this.LoadDocuments(true);
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
            var categoryId = Convert.ToInt32(this.lblCategoryId.Value);
            var folderPermission = this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();
            var folder = new Folder();

            if (!string.IsNullOrEmpty(Session["FolderMenuAction"].ToString()) && Session["FolderMenuAction"].ToString() == "New")
            {
                var parentFol = this.folderService.GetById(Convert.ToInt32(e.Node.ParentNode.Value));
                folder = new Folder()
                    {
                        Name = e.Text,
                        Description = e.Text,
                        CategoryID = categoryId,
                        ParentID = Convert.ToInt32(e.Node.ParentNode.Value),
                        DirName = parentFol.DirName + "/" + e.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };
                var folId = this.folderService.Insert(folder);
                Directory.CreateDirectory(Server.MapPath(folder.DirName));

                var dataPermission = new GroupDataPermission()
                    {
                        RoleId = UserSession.Current.User.RoleId.GetValueOrDefault(),
                        FolderIdList = folId.ToString(),
                        CategoryIdList = categoryId.ToString()
                    };
                this.groupDataPermissionService.Insert(dataPermission);

            }
            else if (!string.IsNullOrEmpty(Session["FolderMenuAction"].ToString()) && Session["FolderMenuAction"].ToString() == "Rename")
            {
                folder = this.folderService.GetById(Convert.ToInt32(e.Node.Value));
                folder.Name = e.Text;
                this.folderService.Update(folder);
            }

            Session.Remove("FolderMenuAction");

            var listFolder = this.folderService.GetAllSpecificFolder(folderPermission);
            this.radTreeFolder.DataSource = listFolder;
            this.radTreeFolder.DataFieldParentID = "ParentID";
            this.radTreeFolder.DataTextField = "Name";
            this.radTreeFolder.DataValueField = "ID";
            this.radTreeFolder.DataFieldID = "ID";
            
            this.radTreeFolder.DataBind();

            foreach (var node in this.radTreeFolder.Nodes)
            {
                var treeNode = (RadTreeNode)node;
                treeNode.ImageUrl = "Images/folderdir16.png";
                treeNode.Expanded = true;
                this.CustomFolderTree(treeNode);
            }

            this.RestoreExpandStateTreeView();
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
                    if (Regex.IsMatch(clickedNode.Text, unreadPattern))
                    {
                        clickedNode.Text = Regex.Replace(
                            clickedNode.Text, unreadPattern, "(" + clickedNode.Nodes.Count.ToString() + ")");
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
                    var categoryId = Convert.ToInt32(this.lblCategoryId.Value);
                    var folDelete = this.folderService.GetById(Convert.ToInt32(clickedNode.Value));
                    var dataPermission =
                        this.groupDataPermissionService.GetByRoleId(
                            UserSession.Current.User.RoleId.GetValueOrDefault(),
                            categoryId.ToString(),
                            folDelete.ID.ToString());
                    this.groupDataPermissionService.Delete(dataPermission);
                    this.folderService.Delete(folDelete);

                    Directory.Delete(Server.MapPath(folDelete.DirName));

                    var folderPermission =
                    this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
                            t => t.CategoryIdList == categoryId.ToString()).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();

                    var listFolder = this.folderService.GetAllSpecificFolder(folderPermission);
                    this.radTreeFolder.DataSource = listFolder;
                    this.radTreeFolder.DataFieldParentID = "ParentID";
                    this.radTreeFolder.DataTextField = "Name";
                    this.radTreeFolder.DataValueField = "ID";
                    this.radTreeFolder.DataFieldID = "ID";
                    this.radTreeFolder.DataBind();

                    foreach (var node in this.radTreeFolder.Nodes)
                    {
                        var treeNode = (RadTreeNode)node;
                        treeNode.ImageUrl = "Images/folderdir16.png";
                        treeNode.Expanded = true;
                        this.CustomFolderTree(treeNode);
                    }

                    this.RestoreExpandStateTreeView();
                    break;
            }
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
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false)
        {
            if (this.radTreeFolder.SelectedNode != null)
            {
                var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
                var listDocuments = this.documentService.GetAllByFolder(folderId);

                this.grdDocument.DataSource = listDocuments;

                if (isbind)
                {
                    this.grdDocument.DataBind();
                }
            }
            else
            {
                this.grdDocument.DataSource = new List<Document>();
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
            if (this.radTreeFolder.SelectedNode != null)
            {
                var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
                if(!string.IsNullOrEmpty(search))
                {
                    this.grdDocument.DataSource = this.documentService.QuickSearch(search, folderId);    
                }
                else
                {
                    this.grdDocument.DataSource =  this.documentService.GetAllByFolder(folderId);
                }
                
                if (isbind)
                {
                    this.grdDocument.DataBind();
                }
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
            if (e.Argument == "Rebind")
            {
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                grdDocument.MasterTableView.CurrentPageIndex = grdDocument.MasterTableView.PageCount - 1;
                grdDocument.Rebind();
            }
            else if (e.Argument == "SendNotification")
            {
                int x = 10;
                var listDisciplineId = new List<int>();
                var listSelectedDoc = new List<Document>();
                var count = 0;
                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox)item["IsSelected"].FindControl("IsSelected");
                    if (cboxSelected.Checked)
                    {
                        count += 1;
                        var docItem = new Document();
                        var disciplineId = item["DisciplineID"].Text != @"&nbsp;"
                                                     ? item["DisciplineID"].Text
                                                     : string.Empty;
                        if (!string.IsNullOrEmpty(disciplineId) && disciplineId != "0")
                        {
                            listDisciplineId.Add(Convert.ToInt32(disciplineId));

                            docItem.ID = count;
                            docItem.DocumentNumber = item["DocumentNumber"].Text != @"&nbsp;"
                                                     ? item["DocumentNumber"].Text
                                                     : string.Empty;
                            docItem.Title = item["Title"].Text != @"&nbsp;"
                                                         ? item["Title"].Text
                                                         : string.Empty;
                            docItem.RevisionName = item["Revision"].Text != @"&nbsp;"
                                                         ? item["Revision"].Text
                                                         : string.Empty;
                            docItem.FilePath = item["FilePath"].Text != @"&nbsp;"
                                                         ? item["FilePath"].Text
                                                         : string.Empty;
                            docItem.DisciplineID = Convert.ToInt32(disciplineId);
                            listSelectedDoc.Add(docItem);
                        }
                    }
                }

                listDisciplineId = listDisciplineId.Distinct().ToList();

                var smtpClient = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        Host = ConfigurationManager.AppSettings["Host"],
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                        Credentials = new NetworkCredential(UserSession.Current.User.Email, Utility.Decrypt(UserSession.Current.User.HashCode))
                    };

                foreach (var disciplineId in listDisciplineId)
                {
                    var notificationRule = this.notificationRuleService.GetAllByDiscipline(disciplineId);
                    
                    if (notificationRule != null)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(UserSession.Current.User.Email, UserSession.Current.User.FullName);
                        message.Subject = "Test send notification from EDMs";
                        message.BodyEncoding = new UTF8Encoding();
                        message.IsBodyHtml = true;
                        message.Body = @"******<br/>
                                        Dear users,<br/><br/>

                                        Please be informed that the following documents are now available on the BDPOC Document Library System for your information.<br/><br/>

                                        <table border='1' cellspacing='0'>
	                                        <tr>
		                                        <th style='text-align:center; width:40px'>No.</th>
		                                        <th style='text-align:center; width:350px'>Document number</th>
		                                        <th style='text-align:center; width:350px'>Document title</th>
		                                        <th style='text-align:center; width:60px'>Revision</th>
	                                        </tr>";

                        if (!string.IsNullOrEmpty(notificationRule.ReceiverList))
                        {
                            message.To.Add(notificationRule.ReceiverList);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(notificationRule.ReceiveGroup) && string.IsNullOrEmpty(notificationRule.ReceiverList))
                            {
                                var listGroupId = notificationRule.ReceiveGroupId.Split(',').ToList();
                            }
                            // Implement for send notification for all user in group
                        }

                        var subBody = string.Empty;
                        foreach (var document in listSelectedDoc)
                        {
                            if (document.DisciplineID == disciplineId)
                            {
                                subBody += @"<tr>
                                <td>" + document.ID + @"</td>
                                <td><a href='http://localhost"
                                           + document.FilePath + "' download='" + document.DocumentNumber + "'>"
                                           + document.DocumentNumber + @"</a></td>
                                <td>"
                                           + document.Title + @"</td>
                                <td>"
                                           + document.RevisionName + @"</td>";
                            }
                        }
                        

                        message.Body += subBody + @"</table>
                                        <br/><br/>
                                        Thanks and regards,<br/>
                                        ******";
                        
                        smtpClient.Send(message);
                    }
                }
            }
            else
            {
                SearchDocument(e.Argument, true);
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
                editLink.Attributes["onclick"] = string.Format("return ShowEditForm('{0}','{1}');",
                                                               e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex][
                                                                   "ID"], e.Item.ItemIndex);
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
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            var listRelateDoc = this.documentService.GetAllRelateDocument(docId);
            if (listRelateDoc != null)
            {
                foreach (var objDoc in listRelateDoc)
                {
                    objDoc.IsDelete = true;
                    objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                    objDoc.LastUpdatedDate = DateTime.Now;
                    this.documentService.Update(objDoc);
                }
            }


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
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.grdDocument.Rebind();
            }
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

        protected void radPbCategories_ItemClick(object sender, RadPanelBarEventArgs e)
        {
            this.radTreeFolder.Nodes.Clear();

            var folderPermission =
                this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
                    t => t.CategoryIdList == e.Item.Value).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();

            var listFolder = this.folderService.GetSpecificFolder(folderPermission);
            foreach (var folder in listFolder)
            {
                var nodeFolder = new RadTreeNode();
                nodeFolder.Text = folder.Name;
                nodeFolder.Value = folder.ID.ToString();
                nodeFolder.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                nodeFolder.ImageUrl = "Images/folderdir16.png";
                this.radTreeFolder.Nodes.Add(nodeFolder);
            }

            ////this.radTreeFolder.DataSource = listFolder;
            ////this.radTreeFolder.DataFieldParentID = "ParentID";
            ////this.radTreeFolder.DataTextField = "Name";
            ////this.radTreeFolder.DataValueField = "ID";
            ////this.radTreeFolder.DataFieldID = "ID";
            ////this.radTreeFolder.DataBind();

            ////foreach (var node in this.radTreeFolder.Nodes)
            ////{
            ////    ((RadTreeNode)node).ImageUrl = "Images/folderdir16.png";
            ////    ((RadTreeNode)node).Expanded = true;
            ////    this.CustomFolderTree((RadTreeNode)node);
            ////}

            this.RestoreExpandStateTreeView();

            this.grdDocument.Rebind();
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

        protected void radTreeFolder_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSideCallBack);
        }

        private void PopulateNodeOnDemand(RadTreeNodeEventArgs e, TreeNodeExpandMode expandMode)
        {
            var categoryId = this.lblCategoryId.Value;
            var folderPermission =
                this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
                    t => t.CategoryIdList == categoryId).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();

            var listFolChild = this.folderService.GetAllByParentId(Convert.ToInt32(e.Node.Value), folderPermission);
            foreach (var folderChild in listFolChild)
            {
                var nodeFolder = new RadTreeNode();
                nodeFolder.Text = folderChild.Name;
                nodeFolder.Value = folderChild.ID.ToString();
                nodeFolder.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                nodeFolder.ImageUrl = "Images/folderdir16.png";
                e.Node.Nodes.Add(nodeFolder);
            }

            e.Node.Expanded = true;
        }
    }
}

