// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ShareDocument : Page
    {
        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        private readonly PackageService packageService;

        private readonly RoleService roleService;

        private readonly DocumentPackageService documentPackageService;

        private readonly ScopeProjectService scopeProjectService;
        private readonly WorkGroupService workGroupService;

        private readonly UserDataPermissionService userDataPermissionService;

        private readonly FolderService folderService;

        private readonly AttachFilesPackageService attachFilesPackageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ShareDocument()
        {
            this.revisionService = new RevisionService();
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.documentService = new DocumentService();
            this.packageService = new PackageService();
            this.roleService = new RoleService();
            this.documentPackageService = new DocumentPackageService();
            this.scopeProjectService = new ScopeProjectService();
            this.workGroupService = new WorkGroupService();
            this.userDataPermissionService = new UserDataPermissionService();
            this.folderService = new FolderService();
            this.attachFilesPackageService = new AttachFilesPackageService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["listDoc"]))
                {
                    this.LoadTreeFolder();
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
            if (this.radTreeFolder.SelectedNode != null && !string.IsNullOrEmpty(this.Request.QueryString["listDoc"]))
            {
                var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
                var folder = this.folderService.GetById(folderId);

                var fileIcon = new Dictionary<string, string>()
                {
                    {"doc", "~/images/wordfile.png"},
                    {"docx", "~/images/wordfile.png"},
                    {"dotx", "~/images/wordfile.png"},
                    {"xls", "~/images/excelfile.png"},
                    {"xlsx", "~/images/excelfile.png"},
                    {"pdf", "~/images/pdffile.png"},
                    {"7z", "~/images/7z.png"},
                    {"dwg", "~/images/dwg.png"},
                    {"dxf", "~/images/dxf.png"},
                    {"rar", "~/images/rar.png"},
                    {"zip", "~/images/zip.png"},
                    {"txt", "~/images/txt.png"},
                    {"xml", "~/images/xml.png"},
                    {"xlsm", "~/images/excelfile.png"},
                    {"bmp", "~/images/bmp.png"},
                };

                if (folder != null)
                {
                    var listDocPackId = this.Request.QueryString["listDoc"].Split(',').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t));
                    foreach (var docId in listDocPackId)
                    {
                        var listAttachFile = this.attachFilesPackageService.GetAllDocId(docId);
                        foreach (var attachFilesPackage in listAttachFile)
                        {
                            var docFileName = attachFilesPackage.FileName;
                            var serverDocFileName = DateTime.Now.ToBinary() + "_" + docFileName;

                            // Path file to save on server disc
                            var saveFilePath = Path.Combine(Server.MapPath(folder.DirName), serverDocFileName);
                            // Path file to download from server
                            var serverFilePath = folder.DirName + "/" + serverDocFileName;
                            var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1,
                                docFileName.Length - docFileName.LastIndexOf(".") - 1);

                            var document = new Document()
                            {
                                Name = docFileName,
                                FileExtension = fileExt,
                                FileExtensionIcon =
                                    fileIcon.ContainsKey(fileExt.ToLower())
                                        ? fileIcon[fileExt.ToLower()]
                                        : "~/images/otherfile.png",
                                FilePath = serverFilePath,
                                FolderID = folderId,
                                IsLeaf = true,
                                IsDelete = false,
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedDate = DateTime.Now
                            };

                            if (File.Exists(Server.MapPath(@"../.." + attachFilesPackage.FilePath)))
                            {
                                File.Copy(Server.MapPath(@"../.." + attachFilesPackage.FilePath), saveFilePath);
                            }

                            this.documentService.Insert(document); 
                            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
                        }
                    }
                }
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

        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/folderdir16.png";
            
        }

        private void LoadTreeFolder()
        {
            var folderPermission = this.userDataPermissionService.GetByUserId(UserSession.Current.User.Id)
                                                            .Select(t => t.FolderId.GetValueOrDefault()).Distinct().ToList();

            var listFolder = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                            ? this.folderService.GetAll()
                            : this.folderService.GetSpecificFolderStatic(folderPermission);

            this.radTreeFolder.DataSource = listFolder;
            this.radTreeFolder.DataFieldParentID = "ParentID";
            this.radTreeFolder.DataTextField = "Name";
            this.radTreeFolder.DataValueField = "ID";
            this.radTreeFolder.DataFieldID = "ID";
            this.radTreeFolder.DataBind();

            this.radTreeFolder.Nodes[0].Expanded = true;

            this.RestoreExpandStateTreeView();

            this.SortNodes(this.radTreeFolder.Nodes);
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
    }
}