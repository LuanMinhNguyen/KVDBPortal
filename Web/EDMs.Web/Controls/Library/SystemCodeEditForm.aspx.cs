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
    public partial class SystemCodeEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly SystemCodeService systemCodeService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly KKSIdentificationCodeService kksIdentificationCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemCodeEditForm"/> class.
        /// </summary>
        public SystemCodeEditForm()
        {
            this.userService = new UserService();
            this.systemCodeService = new SystemCodeService();
            this.kksIdentificationCodeService = new KKSIdentificationCodeService();
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
             
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.systemCodeService.GetById(Convert.ToInt32(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {
                        this.txtName.Text = obj.Code;
                        this.txtDescription.Text = obj.Description;
                        this.txtPackageNo.Text = obj.PackageNo;
                        this.ddlKKS.SelectedValue = obj.KKSId.ToString();
                        if (obj.ParentId != null && obj.ParentId != 0)
                        {
                            var rtvSystemCode = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvSystemCode");
                            if (rtvSystemCode != null)
                            {
                                //// rtvSystemCode.Nodes.FindNodeByValue(obj.ParentId.ToString()).Selected = true;
                                var selectedNode = rtvSystemCode.GetAllNodes().First(t => t.Value == obj.ParentId.ToString());
                                if (selectedNode != null)
                                {
                                    rtvSystemCode.GetAllNodes().First(t => t.Value == obj.ParentId.ToString()).Selected = true;
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = Convert.ToInt32(this.Request.QueryString["objId"]);
                    var obj = this.systemCodeService.GetById(objId);
                    if (obj != null)
                    {
                        obj.Code = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        var rtvSystemCode = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvSystemCode");
                        if (rtvSystemCode != null)
                        {
                            obj.ParentId = Convert.ToInt32(rtvSystemCode.SelectedNode.Value);

                            var sysObj = this.systemCodeService.GetById(obj.ParentId.GetValueOrDefault());
                            if (sysObj != null)
                            {
                                obj.ParentCode = sysObj.Code;
                                obj.ParentName = sysObj.FullName;
                            }
                        }

                        obj.PackageNo = this.txtPackageNo.Text.Trim();
                        obj.KKSId = Convert.ToInt32(this.ddlKKS.SelectedValue);
                        obj.KKSCode = this.ddlKKS.SelectedItem.Text.Split('-')[0].Trim();
                        obj.KKSFullName = this.ddlKKS.SelectedItem.Text;

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.systemCodeService.Update(obj);
                    }
                }
                else
                {
                    var obj = new SystemCode()
                    {
                        Code = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        PackageNo = this.txtPackageNo.Text.Trim(),
                        KKSId = Convert.ToInt32(this.ddlKKS.SelectedValue),
                        KKSCode = this.ddlKKS.SelectedItem.Text.Split('-')[0].Trim(),
                        KKSFullName = this.ddlKKS.SelectedItem.Text,
                        ParentId = !string.IsNullOrEmpty(this.ddlParent.SelectedValue) ? Convert.ToInt32(this.ddlParent.SelectedValue) : (int?)null,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };

                    var sysObj = this.systemCodeService.GetById(obj.ParentId.GetValueOrDefault());
                    if (sysObj != null)
                    {
                        obj.ParentCode = sysObj.Code;
                        obj.ParentName = sysObj.FullName;
                    }

                    this.systemCodeService.Insert(obj);
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
            var rtvSystemCode = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvSystemCode");
            if (rtvSystemCode != null)
            {
                var listSystemCode = this.systemCodeService.GetAll().Where(t=> t.ParentId== null).ToList();

                listSystemCode.Insert(0, new SystemCode() { ParentId = null, Name = "(None)" });

                rtvSystemCode.DataSource = listSystemCode;
                rtvSystemCode.DataFieldParentID = "ParentId";
                rtvSystemCode.DataTextField = "FullName";
                rtvSystemCode.DataValueField = "ID";
                rtvSystemCode.DataFieldID = "ID";
                rtvSystemCode.DataBind();
            }

            var kksList = this.kksIdentificationCodeService.GetAll();
            kksList.Insert(0, new KKSIdentificationCode() {ID = 0});
            this.ddlKKS.DataSource = kksList;
            this.ddlKKS.DataTextField = "FullName";
            this.ddlKKS.DataValueField = "ID";
            this.ddlKKS.DataBind();
        }
    }
}