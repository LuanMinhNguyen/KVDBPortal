// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Controls.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class OptionalTypeDetailEditForm : Page
    {
        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly OptionalTypeDetailService OptionalTypeDetailService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The optional type service.
        /// </summary>
        private readonly OptionalTypeService optionalTypeService;

        private readonly OptionalTypePropertiesViewService optionalTypePropertiesViewService;

        private readonly DocumentNewService documentNewService;

        public int SystempOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("System"));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public OptionalTypeDetailEditForm()
        {
            this.userService = new UserService();
            this.OptionalTypeDetailService = new OptionalTypeDetailService();
            this.roleService = new RoleService();
            this.optionalTypeService = new OptionalTypeService();
            this.categoryService = new CategoryService();
            this.optionalTypePropertiesViewService = new OptionalTypePropertiesViewService();
            this.documentNewService = new DocumentNewService();
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
                this.txtName.Focus();
                this.LoadComboData();
                if (!string.IsNullOrEmpty(this.Request.QueryString["action"]) && this.Request.QueryString["action"] == "add")
                {
                    var parentObj = this.OptionalTypeDetailService.GetById(Convert.ToInt32(this.Request.QueryString["parentId"]));
                    if (parentObj != null)
                    {
                        this.ddlOptionalType.SelectedValue = parentObj.OptionalTypeId.GetValueOrDefault().ToString();

                        var rtvOptionalTypeDetail = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvOptionalTypeDetail");
                        if (rtvOptionalTypeDetail != null)
                        {
                            //// rtvOptionalTypeDetail.Nodes.FindNodeByValue(obj.ParentId.ToString()).Selected = true;
                            var selectedNode = rtvOptionalTypeDetail.GetAllNodes().First(t => t.Value == parentObj.ID.ToString());
                            if (selectedNode != null)
                            {
                                rtvOptionalTypeDetail.GetAllNodes().First(t => t.Value == parentObj.ID.ToString()).Selected = true;
                                this.ExpandParentNode(selectedNode);
                                this.ddlParent.Items[0].Text = selectedNode.Text;
                            }
                        }

                        var roleIds = parentObj.RoleId;
                        foreach (RadComboBoxItem item in this.ddlDepartment.Items)
                        {
                            if (roleIds.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }

                        var categoryIds = parentObj.CategoryId;
                        foreach (RadComboBoxItem item in this.ddlCategory.Items)
                        {
                            if (categoryIds.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(this.Request.QueryString["Id"]))
                {
                    this.CreatedInfo.Visible = true;

                    var obj = this.OptionalTypeDetailService.GetById(Convert.ToInt32(this.Request.QueryString["Id"]));
                    if (obj != null)
                    {
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;
                        this.ckbIsRootNode.Checked = obj.IsRoot.GetValueOrDefault();
                        this.txtSerial.Text = obj.Serial;
                        this.txtModel.Text = obj.Model;
                        this.txtTechnicalSpec.Text = obj.TechnicalSpec;
                        this.txtStartDate.SelectedDate = obj.StartDate;
                        this.txtEndDate.SelectedDate = obj.EndDate;
                        this.txtDutyCapacity.Text = obj.DutyCapacity;
                        this.txtDesPress.Text = obj.DesPress;
                        this.txtDesTemp.Text = obj.DesTemp;
                        this.txtDiffPres.Text = obj.DiffPres;
                        this.txtVendor.Text = obj.Vendor;

                        if (obj.ParentId != null && obj.ParentId != 0)
                        {
                            var rtvOptionalTypeDetail = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvOptionalTypeDetail");
                            if (rtvOptionalTypeDetail != null)
                            {
                               //// rtvOptionalTypeDetail.Nodes.FindNodeByValue(obj.ParentId.ToString()).Selected = true;
                                var selectedNode = rtvOptionalTypeDetail.GetAllNodes().First(t => t.Value == obj.ParentId.ToString());
                                if (selectedNode != null)
                                {
                                    rtvOptionalTypeDetail.GetAllNodes().First(t => t.Value == obj.ParentId.ToString()).Selected = true;
                                    this.ExpandParentNode(selectedNode);
                                    this.ddlParent.Items[0].Text = selectedNode.Text;
                                }
                            }
                        }

                        if (obj.SystemId != null && obj.SystemId != 0)
                        {
                            var rtvSystem = (RadTreeView)this.ddlSystem.Items[0].FindControl("rtvSystem");
                            if (rtvSystem != null)
                            {
                                var selectedNode = rtvSystem.GetAllNodes().First(t => t.Value == obj.SystemId.ToString());
                                if (selectedNode != null)
                                {
                                    rtvSystem.GetAllNodes().First(t => t.Value == obj.SystemId.ToString()).Selected = true;
                                    this.ExpandParentNode(selectedNode);
                                    this.ddlSystem.Items[0].Text = selectedNode.Text;
                                }
                            }
                        }

                        var roleIds = obj.RoleId;
                        foreach (RadComboBoxItem item in this.ddlDepartment.Items)
                        {
                            if (roleIds.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }

                        var categoryIds = obj.CategoryId;
                        foreach (RadComboBoxItem item in this.ddlCategory.Items)
                        {
                            if (categoryIds.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }

                        this.ddlOptionalType.SelectedValue = obj.OptionalTypeId.GetValueOrDefault().ToString();

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

                this.LoadPropertiesConfig();
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
                var roleIds = string.Empty;
                var categoryIds = string.Empty;
                roleIds = this.ddlDepartment.CheckedItems.Aggregate(roleIds, (current, t) => current + t.Value + ", ");
                categoryIds = this.ddlCategory.CheckedItems.Aggregate(categoryIds, (current, t) => current + t.Value + ", ");

                if (!string.IsNullOrEmpty(this.Request.QueryString["Id"]))
                {
                    var optTypeDetailId = Convert.ToInt32(this.Request.QueryString["Id"]);
                    var obj = this.OptionalTypeDetailService.GetById(optTypeDetailId);
                    if (obj != null)
                    {
                        obj.Name = this.txtName.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.RoleId = roleIds;
                        var rtvOptionalTypeDetail = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvOptionalTypeDetail");
                        if (rtvOptionalTypeDetail != null && rtvOptionalTypeDetail.SelectedNode != null)
                        {
                            obj.ParentId = Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value);
                        }

                        var rtvSystem = (RadTreeView)this.ddlSystem.Items[0].FindControl("rtvSystem");
                        if (rtvSystem != null && rtvSystem.SelectedNode != null)
                        {
                            obj.SystemId = Convert.ToInt32(rtvSystem.SelectedNode.Value);
                            obj.SystemName = rtvSystem.SelectedNode.Text;
                        }

                        obj.OptionalTypeId = Convert.ToInt32(this.ddlOptionalType.SelectedValue);
                        obj.CategoryId = categoryIds;
                        ////obj.IsRoot = this.ckbIsRootNode.Checked;

                        obj.Serial = this.txtSerial.Text.Trim();
                        obj.Model = this.txtModel.Text.Trim();
                        obj.TechnicalSpec = this.txtTechnicalSpec.Text.Trim();
                        obj.StartDate = this.txtStartDate.SelectedDate;
                        obj.EndDate = this.txtEndDate.SelectedDate;
                        obj.DutyCapacity = this.txtDutyCapacity.Text.Trim();
                        obj.DesPress = this.txtDesPress.Text.Trim();
                        obj.DesTemp = this.txtDesTemp.Text.Trim();
                        obj.DiffPres = this.txtDiffPres.Text.Trim();
                        obj.Vendor = this.txtVendor.Text.Trim();

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.OptionalTypeDetailService.Update(obj);

                        if (obj.OptionalTypeId == 3)
                        {
                            var tempDocList = this.documentNewService.GetAllByTagType(this.Request.QueryString["Id"]);
                            foreach (var documentNew in tempDocList)
                            {
                                var tagtypeNames = string.Empty;
                                foreach (var tagtypeId in documentNew.TagTypeId.Split(',').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()))
                                {
                                    var tagtypeObj = this.OptionalTypeDetailService.GetById(Convert.ToInt32(tagtypeId));
                                    if (tagtypeObj != null)
                                    {
                                        tagtypeNames += tagtypeObj.Name + "<br/>";
                                        if (!string.IsNullOrEmpty(tagtypeObj.Serial) || !string.IsNullOrEmpty(tagtypeObj.Model) || !string.IsNullOrEmpty(tagtypeObj.TechnicalSpec))
                                        {
                                            tagtypeNames += "(";
                                            if (!string.IsNullOrEmpty(tagtypeObj.Serial))
                                            {
                                                tagtypeNames += "<b style='font-style: italic'>Serial: </b>" + tagtypeObj.Serial + (string.IsNullOrEmpty(tagtypeObj.Model) && string.IsNullOrEmpty(tagtypeObj.TechnicalSpec) ? ") <br/>" : "<br/>");
                                            }

                                            if (!string.IsNullOrEmpty(tagtypeObj.Model))
                                            {
                                                tagtypeNames += "<b style='font-style: italic'>Model: </b>" + tagtypeObj.Model + (string.IsNullOrEmpty(tagtypeObj.TechnicalSpec) ? ") <br/>" : "<br/>");
                                            }

                                            if (!string.IsNullOrEmpty(tagtypeObj.TechnicalSpec))
                                            {
                                                tagtypeNames += "<b style='font-style: italic'>Tech spec: </b>" + tagtypeObj.TechnicalSpec + ") <br/>";
                                            }
                                        }
                                    }
                                }

                                documentNew.TagTypeName = tagtypeNames;
                                this.documentNewService.Update(documentNew);
                            }
                        }
                    }
                }
                else
                {
                    var obj = new OptionalTypeDetail()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        OptionalTypeId = Convert.ToInt32(this.ddlOptionalType.SelectedValue),
                        RoleId = roleIds,
                        CategoryId = categoryIds,
                        IsRoot = ckbIsRootNode.Checked,
                        Serial = this.txtSerial.Text.Trim(),
                        Model = this.txtModel.Text.Trim(),
                        TechnicalSpec = this.txtTechnicalSpec.Text.Trim(),
                        StartDate = this.txtStartDate.SelectedDate,
                        EndDate = this.txtEndDate.SelectedDate,
                        DutyCapacity = this.txtDutyCapacity.Text.Trim(),
                        DesTemp = this.txtDesTemp.Text.Trim(),
                        DesPress = this.txtDesPress.Text.Trim(),
                        DiffPres = this.txtDiffPres.Text.Trim(),
                        Vendor = this.txtVendor.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        Active = true
                    };

                    var rtvOptionalTypeDetail = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvOptionalTypeDetail");
                    if (rtvOptionalTypeDetail != null && rtvOptionalTypeDetail.SelectedNode != null)
                    {
                        obj.ParentId = Convert.ToInt32(rtvOptionalTypeDetail.SelectedNode.Value);
                    }

                    var rtvSystem = (RadTreeView)this.ddlSystem.Items[0].FindControl("rtvSystem");
                    if (rtvSystem != null && rtvSystem.SelectedNode != null)
                    {
                        obj.SystemId = Convert.ToInt32(rtvSystem.SelectedNode.Value);
                        obj.SystemName = rtvSystem.SelectedNode.Text;
                    }

                    this.OptionalTypeDetailService.Insert(obj);
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
            if(this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Document type name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            var listCategory = this.categoryService.GetAll();
            this.ddlCategory.DataSource = listCategory;
            this.ddlCategory.DataTextField = "Name";
            this.ddlCategory.DataValueField = "ID";
            this.ddlCategory.DataBind();
            this.ddlCategory.SelectedIndex = 0;

            var listDeparment = this.roleService.GetAllSpecial();
            this.ddlDepartment.DataSource = listDeparment;
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "Id";
            this.ddlDepartment.DataBind();
            this.ddlDepartment.SelectedIndex = 0;

            var listOptionalType = this.optionalTypeService.GetAllActive();
            listOptionalType.Insert(0, new OptionalType() { ID = 0, Name = string.Empty});
            this.ddlOptionalType.DataSource = listOptionalType;
            this.ddlOptionalType.DataTextField = "Name";
            this.ddlOptionalType.DataValueField = "ID";
            this.ddlOptionalType.DataBind();
            this.ddlOptionalType.SelectedIndex = 0;

            var listOptionalTypeDetail = this.OptionalTypeDetailService.GetAll();
            var rtvOptionalTypeDetail = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvOptionalTypeDetail");
            if (rtvOptionalTypeDetail != null)
            {
                rtvOptionalTypeDetail.DataSource = listOptionalTypeDetail;
                rtvOptionalTypeDetail.DataFieldParentID = "ParentId";
                rtvOptionalTypeDetail.DataTextField = "Name";
                rtvOptionalTypeDetail.DataValueField = "ID";
                rtvOptionalTypeDetail.DataFieldID = "ID";
                rtvOptionalTypeDetail.DataBind();
            }

            var rtvSystem = (RadTreeView)this.ddlSystem.Items[0].FindControl("rtvSystem");
            if (rtvSystem != null)
            {
                rtvSystem.DataSource = listOptionalTypeDetail.Where(t => t.OptionalTypeId == this.SystempOptType);
                rtvSystem.DataFieldParentID = "ParentId";
                rtvSystem.DataTextField = "Name";
                rtvSystem.DataValueField = "ID";
                rtvSystem.DataFieldID = "ID";
                rtvSystem.DataBind();
            }

            this.RestoreExpandStateTreeView();
        }

        protected void rtvOptionalTypeDetail_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Images/object.png";
        }

        /// <summary>
        /// The restore expand state tree view.
        /// </summary>
        private void RestoreExpandStateTreeView()
        {
            var rtvOptionalTypeDetail = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvOptionalTypeDetail");
            if (rtvOptionalTypeDetail != null)
            {
                // Restore expand state of tree folder
                HttpCookie cookie = this.Request.Cookies["expandedNodesOptionalTypeDetailEdit"];
                if (cookie != null)
                {
                    var expandedNodeValues = cookie.Value.Split('*');
                    foreach (var nodeValue in expandedNodeValues)
                    {
                        RadTreeNode expandedNode =
                            rtvOptionalTypeDetail.FindNodeByValue(HttpUtility.UrlDecode(nodeValue));
                        if (expandedNode != null)
                        {
                            expandedNode.Expanded = true;
                        }
                    }
                }
            }
        }

        protected void ddlOptionalType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadPropertiesConfig();
        }

        private void LoadPropertiesConfig()
        {
            var obj = this.optionalTypePropertiesViewService.GetByOptionalType(Convert.ToInt32(ddlOptionalType.SelectedValue));
            var totalOptTypeDetailProperty = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TotalOptTypeDetailProperty"));
            for (int i = 1; i <= totalOptTypeDetailProperty; i++)
            {
                if (obj != null && !string.IsNullOrEmpty(obj.PropertyIndex) && obj.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim()).Contains(i.ToString()))
                {
                    this.divContent.FindControl("Pro" + i).Visible = true;
                }
                else
                {
                    this.divContent.FindControl("Pro" + i).Visible = false;
                }
            }
        }
    }
}