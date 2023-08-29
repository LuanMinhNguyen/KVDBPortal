// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Library
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class MRCheckListPage : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        private readonly TitleService titleService = new TitleService();

        private readonly MaterialRequisitionCheckListDefineService mrCheckListDefineService = new MaterialRequisitionCheckListDefineService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

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
            Session.Add("SelectedMainMenu", "System");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.LoadCostContractPanel();
                this.LoadScopePanel();
                this.LoadSystemPanel();

                this.LoadCheckList();
            }
        }

        private void LoadCheckList()
        {
            var checkList = this.mrCheckListDefineService.GetAll();

            var mainCheckList = new MaterialRequisitionCheckListDefine()
            {
                ID = 0,
                Description = "Material Requisition Check List"
            };

            foreach (var item in checkList.Where(t => t.ParentId == null))
            {
                item.ParentId = 0;
            }

            checkList.Add(mainCheckList);

            this.rtvCheckList.DataSource = checkList;
            this.rtvCheckList.DataFieldParentID = "ParentId";
            this.rtvCheckList.DataTextField = "Description";
            this.rtvCheckList.DataValueField = "ID";
            this.rtvCheckList.DataFieldID = "ID";
            this.rtvCheckList.DataBind();

            this.RestoreExpandStateTreeView();

            this.SortNodes(this.rtvCheckList.Nodes);
        }

        private void RestoreExpandStateTreeView()
        {
            // Restore expand state of tree folder
            HttpCookie cookie = Request.Cookies["expandedNodes"];
            if (cookie != null)
            {
                var expandedNodeValues = cookie.Value.Split('*');
                foreach (var nodeValue in expandedNodeValues)
                {
                    RadTreeNode expandedNode = this.rtvCheckList.FindNodeByValue(HttpUtility.UrlDecode(nodeValue));
                    if (expandedNode != null)
                    {
                        expandedNode.Expanded = true;
                    }
                }
            }
        }

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

        public void Sort(RadTreeNodeCollection collection)
        {
            RadTreeNode[] nodes = new RadTreeNode[collection.Count];
            collection.CopyTo(nodes, 0);
            Array.Sort(nodes, new TreeNodeComparer());
            collection.Clear();
            collection.AddRange(nodes);
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
            if (e.Argument == "RebindTreeView")
            {
                this.LoadCheckList();
            }
            else if (e.Argument.Contains("Delete"))
            {
                var objId = Convert.ToInt32(e.Argument.Split('_')[1]);
                this.mrCheckListDefineService.Delete(objId);

                this.LoadCheckList();
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

        private void LoadCostContractPanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("CostContractID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG COST/CONTRACT MANAGE" });

                this.radPbCostContract.DataSource = permissions;
                this.radPbCostContract.DataFieldParentID = "ParentId";
                this.radPbCostContract.DataFieldID = "Id";
                this.radPbCostContract.DataValueField = "Id";
                this.radPbCostContract.DataTextField = "MenuName";
                this.radPbCostContract.DataBind();
                this.radPbCostContract.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbCostContract.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                }
            }
        }

        private void LoadScopePanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ScopeID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId).OrderBy(t => t.Menu.Priority).ToList();
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG MANAGEMENT" });

                this.radPbScope.DataSource = permissions;
                this.radPbScope.DataFieldParentID = "ParentId";
                this.radPbScope.DataFieldID = "Id";
                this.radPbScope.DataValueField = "Id";
                this.radPbScope.DataTextField = "MenuName";
                this.radPbScope.DataBind();
                this.radPbScope.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbScope.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "MR Check List")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void rtvCheckList_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
        {
            e.Node.Text = e.Text;
            var categoryId = Convert.ToInt32(this.lblCategoryId.Value);
            var checkListItem = new MaterialRequisitionCheckListDefine();

            if (!string.IsNullOrEmpty(Session["CheckListMenuAction"].ToString()) && Session["CheckListMenuAction"].ToString() == "New")
            {
                try
                {
                    checkListItem = new MaterialRequisitionCheckListDefine()
                    {
                        Description = e.Text,
                    };

                    this.mrCheckListDefineService.Insert(checkListItem);

                }
                catch (Exception ex)
                {
                }
            }

            Session.Remove("CheckListMenuAction");

            this.LoadCheckList();
        }

        protected void rtvCheckList_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            var clickedNode = e.Node;

            switch (e.MenuItem.Value)
            {
                case "New":
                    Session.Add("CheckListMenuAction", "New");
                    var newFolder = new RadTreeNode(string.Format("New Check List Item {0}", clickedNode.Nodes.Count + 1))
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
            }
        }

        private void startNodeInEditMode(string nodeValue)
        {
            //find the node by its Value and edit it when page loads
            string js = "Sys.Application.add_load(editNode); function editNode(){ ";
            js += "var tree = $find(\"" + this.rtvCheckList.ClientID + "\");";
            js += "var node = tree.findNodeByValue('" + nodeValue + "');";
            js += "if (node) node.startEdit();";
            js += "Sys.Application.remove_load(editNode);};";

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit", js, true);
        }

        protected void rtvCheckList_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            
        }

        protected void rtvCheckList_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/checkList.png";
        }
    }
}

