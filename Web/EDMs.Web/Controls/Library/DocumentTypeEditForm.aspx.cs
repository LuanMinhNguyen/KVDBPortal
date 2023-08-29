// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Document;

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentTypeEditForm : Page
    {
      

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly DistributionMatrixTypeService distributionMatrixTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeEditForm"/> class.
        /// </summary>
        public DocumentTypeEditForm()
        {
            this.userService = new UserService();
            this.documentTypeService = new DocumentTypeService();
            this.distributionMatrixTypeService = new DistributionMatrixTypeService();;
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
                this.LoadComboData();
             
                if (!string.IsNullOrEmpty(this.Request.QueryString["doctypeId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.documentTypeService.GetById(Convert.ToInt32(this.Request.QueryString["doctypeId"]));
                    if (obj != null)
                    {

                        this.txtName.Text = obj.Code;
                        this.txtDescription.Text = obj.Description;
                        this.ddlCategory.SelectedValue = obj.CategoryIds;
                        //foreach (RadTreeNode matrixTypeNode in this.rtvMatrixType.Nodes)
                        //{
                        //    matrixTypeNode.Checked = !string.IsNullOrEmpty(obj.CategoryIds) && obj.CategoryIds.Split(';').Contains(matrixTypeNode.Value);
                        //}

                        if (obj.ParentId != null && obj.ParentId != 0)
                        {
                            var rtvDocumentType = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvDocumentType");
                            if (rtvDocumentType != null)
                            {
                                //// rtvDocumentType.Nodes.FindNodeByValue(obj.ParentId.ToString()).Selected = true;
                                var selectedNode = rtvDocumentType.GetAllNodes().First(t => t.Value == obj.ParentId.ToString());
                                if (selectedNode != null)
                                {
                                    rtvDocumentType.GetAllNodes().First(t => t.Value == obj.ParentId.ToString()).Selected = true;
                                    this.ExpandParentNode(selectedNode);
                                    this.ddlParent.Items[0].Text = selectedNode.Text;
                                }
                            }
                        }

                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (obj.LastUpdatedBy != null && obj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(obj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + obj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                }
            }
        }

        private void ExpandParentNode(RadTreeNode node)
        {
            if (node.ParentNode != null)
            {
                node.ParentNode.Expanded = true;
                this.ExpandParentNode(node.ParentNode);
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
            if (this.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["doctypeId"]))
                {
                    var doctypeId = Convert.ToInt32(this.Request.QueryString["doctypeId"]);
                    var obj = this.documentTypeService.GetById(doctypeId);
                    if (obj != null)
                    {
                        obj.Code = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        var rtvDocumentType = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvDocumentType");
                        if (rtvDocumentType != null && rtvDocumentType.SelectedNode != null)
                        {
                            obj.ParentId = Convert.ToInt32(rtvDocumentType.SelectedNode.Value);
                            obj.ParentCode = rtvDocumentType.SelectedNode.Text.Split(',')[0];
                            obj.ParentFullName = rtvDocumentType.SelectedNode.Text;
                        }

                        obj.CategoryIds = this.ddlCategory.SelectedValue;
                        obj.CategoryName = this.ddlCategory.SelectedItem.Text;
                        //obj.CategoryIds = string.Empty;
                        //obj.CategoryName = string.Empty;
                        //foreach (RadTreeNode matrixTypeNode in this.rtvMatrixType.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                        //{
                        //    obj.CategoryIds += matrixTypeNode.Value + ";";
                        //    obj.CategoryName += matrixTypeNode.Text + Environment.NewLine;
                        //}

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.documentTypeService.Update(obj);
                    }
                }
                else
                {
                    var obj = new DocumentType()
                    {
                        Code = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                      
                        ParentId = !string.IsNullOrEmpty(this.ddlParent.SelectedValue) ? Convert.ToInt32(this.ddlParent.SelectedValue) : (int?)null,
                        ParentCode = !string.IsNullOrEmpty(this.ddlParent.SelectedValue) ? this.ddlParent.Text.Split(',')[0] : string.Empty,
                        ParentFullName = !string.IsNullOrEmpty(this.ddlParent.SelectedValue) ? this.ddlParent.Text : string.Empty,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };
                    obj.CategoryIds = this.ddlCategory.SelectedValue;
                    obj.CategoryName = this.ddlCategory.SelectedItem.Text;
                    //obj.CategoryIds = string.Empty;
                    //obj.CategoryName = string.Empty;
                    //foreach (RadTreeNode matrixTypeNode in this.rtvMatrixType.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
                    //{
                    //    obj.CategoryIds += matrixTypeNode.Value + ";";
                    //    obj.CategoryName += matrixTypeNode.Text + Environment.NewLine;
                    //}

                    this.documentTypeService.Insert(obj);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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

        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            if (this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Document type name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
           
            var rtvDocumentType = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvDocumentType");
            if (rtvDocumentType != null)
            {
                var listDocumentType = this.documentTypeService.GetAll().Where(t=> t.ParentId== null).ToList();

                listDocumentType.Insert(0, new DocumentType() { ParentId = null, Name = "(None)" });

                rtvDocumentType.DataSource = listDocumentType;
                rtvDocumentType.DataFieldParentID = "ParentId";
                rtvDocumentType.DataTextField = "FullName";
                rtvDocumentType.DataValueField = "ID";
                rtvDocumentType.DataFieldID = "ID";
                rtvDocumentType.DataBind();

            }

            //var matrixTypeList = this.distributionMatrixTypeService.GetAll();
            //foreach (var matrixType in matrixTypeList)
            //{
            //    var matrixTypeNode = new RadTreeNode(matrixType.Name, matrixType.ID.ToString());
            //    this.rtvMatrixType.Nodes.Add(matrixTypeNode);
            //}
        }

       
      
    }
}