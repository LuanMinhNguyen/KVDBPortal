// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentInfoEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";
        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The document new service.
        /// </summary>
        private readonly DocumentNewService documentNewService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly FolderService folderService;

        /// <summary>
        /// The doc properties view service.
        /// </summary>
        private readonly DocPropertiesViewService docPropertiesViewService;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService categoryService;

        /// <summary>
        /// The optional type detail service.
        /// </summary>
        private readonly OptionalTypeDetailService optionalTypeDetailService;

        /// <summary>
        /// The originator service.
        /// </summary>
        private readonly OriginatorService originatorService;

        private readonly GroupDataPermissionService groupDataPermissionService;

        public int PlantOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Plant"));
            }
        }

        public int SystempOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("System"));
            }
        }

        public int TagOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Tagtype"));
            }
        }

        public int DisciplineOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Discipline"));
            }
        }

        public int DocumentTypeOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("DocumentType"));
            }
        }

        public int ProjectOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Project"));
            }
        }

        public int BlockOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Block"));
            }
        }

        public int FieldOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Field"));
            }
        }

        public int PlatformOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Platform"));
            }
        }

        public int WellOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("Well"));
            }
        }

        public int RIGOptType
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("RIG"));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DocumentInfoEditForm()
        {
            this.categoryService = new CategoryService();
            this.revisionService = new RevisionService();
            this.documentService = new DocumentService();
            this.userService = new UserService();
            this.folderService = new FolderService();
            this.docPropertiesViewService = new DocPropertiesViewService();
            this.optionalTypeDetailService = new OptionalTypeDetailService();
            this.originatorService = new OriginatorService();
            this.documentNewService = new DocumentNewService();
            this.groupDataPermissionService = new GroupDataPermissionService();
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
                Session.Remove("ExistDoc");

                var selectedCategory = this.groupDataPermissionService.GetByRoleId(UserSession.Current.RoleId).Select(t => Convert.ToInt32(t.CategoryIdList)).ToList();
                var listCategory = this.categoryService.GetAll().Where(t => selectedCategory.Contains(t.ID));
                this.ddlCategory.DataSource = listCategory;
                this.ddlCategory.DataTextField = "Name";
                this.ddlCategory.DataValueField = "ID";
                this.ddlCategory.DataBind();
                this.ddlCategory.SelectedIndex = 0;

                this.LoadComboData();
                if ((!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault()) || this.ddlCategory.Items.Count == 0)
                {
                    this.btnSave.Visible = false;
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.CreatedInfo.Visible = true;
                    this.btnGetInfo.Visible = false;
                    var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
                    var docObj = this.documentNewService.GetById(docId);
                    if (docObj != null)
                    {
                        this.LoadDocInfo(docObj);

                        var revList = this.documentNewService.GetAllRevDoc(docObj.ParentId == null ? docObj.ID : docObj.ParentId.Value);
                        if (revList != null && revList.Count > 1)
                        {
                            this.RevisionDoc.Visible = true;
                            this.ddlRevFullDoc.DataSource = revList;
                            this.ddlRevFullDoc.DataValueField = "ID";
                            this.ddlRevFullDoc.DataTextField = "RevisionFullName";
                            this.ddlRevFullDoc.DataBind();
                            this.ddlRevFullDoc.SelectedValue = docObj.ID.ToString();
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                }

                this.LoadViewPropertiesConfig();
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
            this.Session.Remove("IsFillData");
            DocumentNew docObjNew;

            if (this.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var docId = this.RevisionDoc.Visible ? Convert.ToInt32(this.ddlRevFullDoc.SelectedValue) : Convert.ToInt32(this.Request.QueryString["docId"]);
                    docObjNew = this.documentNewService.GetById(docId);
                    if (docObjNew != null)
                    {
                        this.CollectData(ref docObjNew);
                        docObjNew.LastUpdatedBy = UserSession.Current.User.Id;
                        docObjNew.LastUpdatedDate = DateTime.Now;
                        
                        this.documentNewService.Update(docObjNew);
                    }
                }
                else
                {
                    var isExistDoc = Session["ExistDoc"] != null;
                    docObjNew = new DocumentNew()
                    {
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        IsLeaf = true,
                        DocIndex = 1,
                        RoleId = UserSession.Current.RoleId
                    };

                    if (!string.IsNullOrEmpty(this.txtName.Text.Trim()) && isExistDoc)
                    {
                        var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
                        var docObj = this.documentNewService.GetByName(this.txtName.Text.Trim(), categoryId);
                        if (docObj != null)
                        {
                            docObj.IsLeaf = false;

                            docObjNew.DocIndex = docObj.DocIndex + 1;

                            docObjNew.ParentId = docObj.ParentId ?? docObj.ID;

                            this.documentNewService.Update(docObj);
                        }
                    }

                    this.CollectData(ref docObjNew);
                    this.documentNewService.Insert(docObjNew);
                }


                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(ref DocumentNew docObj)
        {
            var rtvPlant = (RadTreeView)this.ddlPlant.Items[0].FindControl("rtvPlant");
            var rtvSystem = (RadTreeView)this.ddlSystem.Items[0].FindControl("rtvSystem");
            var rtvDiscipline = (RadTreeView)this.ddlDiscipline.Items[0].FindControl("rtvDiscipline");
            var rtvDocumentType = (RadTreeView)this.ddlDocumentType.Items[0].FindControl("rtvDocumentType");
            var rtvTagType = (RadTreeView)this.ddlTagType.Items[0].FindControl("rtvTagType");
            var rtvProject = (RadTreeView)this.ddlProject.Items[0].FindControl("rtvProject");
            var rtvBlock = (RadTreeView)this.ddlBlock.Items[0].FindControl("rtvBlock");
            var rtvField = (RadTreeView)this.ddlField.Items[0].FindControl("rtvField");
            var rtvPlatform = (RadTreeView)this.ddlPlatform.Items[0].FindControl("rtvPlatform");
            var rtvWell = (RadTreeView)this.ddlWell.Items[0].FindControl("rtvWell");
            var rtvRIG = (RadTreeView)this.ddlRIG.Items[0].FindControl("rtvRIG");

            docObj.Name = this.txtName.Text.Trim();
            docObj.Description = this.txtDescription.Text.Trim();

            docObj.RevId = Convert.ToInt32(this.ddlRevision.SelectedValue);
            docObj.RevName = this.ddlRevision.SelectedItem.Text;

            docObj.VendorName = this.txtVendorName.Text.Trim();
            docObj.DrawingNumber = this.txtDrawingNumber.Text.Trim();
            docObj.Year = Convert.ToInt32(this.ddlYear.SelectedValue);

            if (rtvPlant != null && rtvPlant.SelectedNode != null)
            {
                docObj.PlantId = Convert.ToInt32(rtvPlant.SelectedNode.Value);
                docObj.PlantName = rtvPlant.SelectedNode.Text;
            }
            else
            {
                docObj.PlantName = string.Empty;
            }

            if (rtvSystem != null && rtvSystem.SelectedNode != null)
            {
                docObj.SystemId = Convert.ToInt32(rtvSystem.SelectedNode.Value);
                docObj.SystemName = rtvSystem.SelectedNode.Text;
            }
            else
            {
                docObj.SystemName = string.Empty;
            }

            if (rtvDiscipline != null && rtvDiscipline.SelectedNode != null)
            {
                docObj.DisciplineId = Convert.ToInt32(rtvDiscipline.SelectedNode.Value);
                docObj.DisciplineName = rtvDiscipline.SelectedNode.Text;
            }
            else
            {
                docObj.DisciplineName = string.Empty;
            }

            if (rtvDocumentType != null && rtvDocumentType.SelectedNode != null)
            {
                docObj.DocumentTypeId = Convert.ToInt32(rtvDocumentType.SelectedNode.Value);
                docObj.DocumentTypeName = rtvDocumentType.SelectedNode.Text;
            }
            else
            {
                docObj.DocumentTypeName = string.Empty;
            }

            var tagtypeIds = string.Empty;
            var tagtypeNames = string.Empty;
            if (rtvTagType != null && rtvTagType.CheckedNodes.Count > 0)
            {
                tagtypeIds = rtvTagType.CheckedNodes.Aggregate(tagtypeIds, (current, t) => current + t.Value + ",");

                foreach (var tagTypeNode in rtvTagType.CheckedNodes)
                {
                    var tagtypeId = Convert.ToInt32(tagTypeNode.Value);
                    var tagtypeObj = this.optionalTypeDetailService.GetById(tagtypeId);
                    if (tagtypeObj != null)
                    {
                        tagtypeNames += tagtypeObj.Name + ", ";
                        ////tagtypeNames += tagtypeObj.Name + "<br/>";
                        ////if (!string.IsNullOrEmpty(tagtypeObj.Serial) || !string.IsNullOrEmpty(tagtypeObj.Model) || !string.IsNullOrEmpty(tagtypeObj.TechnicalSpec))
                        ////{
                        ////    tagtypeNames += "(";
                        ////    if (!string.IsNullOrEmpty(tagtypeObj.Serial))
                        ////    {
                        ////        tagtypeNames += "<b style='font-style: italic'>Serial: </b>" + tagtypeObj.Serial + (string.IsNullOrEmpty(tagtypeObj.Model) && string.IsNullOrEmpty(tagtypeObj.TechnicalSpec) ? ") <br/>" : "<br/>");
                        ////    }

                        ////    if (!string.IsNullOrEmpty(tagtypeObj.Model))
                        ////    {
                        ////        tagtypeNames += "<b style='font-style: italic'>Model: </b>" + tagtypeObj.Model + (string.IsNullOrEmpty(tagtypeObj.TechnicalSpec) ? ") <br/>" : "<br/>");
                        ////    }

                        ////    if (!string.IsNullOrEmpty(tagtypeObj.TechnicalSpec))
                        ////    {
                        ////        tagtypeNames += "<b style='font-style: italic'>Tech spec: </b>" + tagtypeObj.TechnicalSpec + ") <br/>";
                        ////    }
                        ////}
                    }
                }

                ////if (rtvTagType.CheckedNodes.Count > 1)
                ////{
                ////    tagtypeNames = rtvTagType.CheckedNodes.Aggregate(tagtypeNames, (current, t) => current + "_ " + t.Text + "<br/>");
                ////}
                ////else
                ////{
                ////    tagtypeNames = rtvTagType.CheckedNodes.Aggregate(tagtypeNames, (current, t) => current + t.Text + "<br/>");
                ////}

                docObj.TagTypeId = tagtypeIds;
            }

            docObj.TagTypeName = tagtypeNames;

            if (rtvProject != null && rtvProject.SelectedNode != null)
            {
                docObj.ProjectId = Convert.ToInt32(rtvProject.SelectedNode.Value);
                docObj.ProjectName = rtvProject.SelectedNode.Text;
            }
            else
            {
                docObj.ProjectName = string.Empty;
            }

            if (rtvBlock != null && rtvBlock.SelectedNode != null)
            {
                docObj.BlockId = Convert.ToInt32(rtvBlock.SelectedNode.Value);
                docObj.BlockName = rtvBlock.SelectedNode.Text;
            }
            else
            {
                docObj.BlockName = string.Empty;
            }

            if (rtvField != null && rtvField.SelectedNode != null)
            {
                docObj.FieldId = Convert.ToInt32(rtvField.SelectedNode.Value);
                docObj.FieldName = rtvField.SelectedNode.Text;
            }
            else
            {
                docObj.FieldName = string.Empty;
            }

            if (rtvPlatform != null && rtvPlatform.SelectedNode != null)
            {
                docObj.PlatformId = Convert.ToInt32(rtvPlatform.SelectedNode.Value);
                docObj.PlatformName = rtvPlatform.SelectedNode.Text;
            }
            else
            {
                docObj.PlatformName = string.Empty;
            }

            if (rtvWell != null && rtvWell.SelectedNode != null)
            {

                var wellId = Convert.ToInt32(rtvWell.SelectedNode.Value);
                var wellName = string.Empty;
                docObj.WellId = wellId;
                var wellObj = this.optionalTypeDetailService.GetById(wellId);
                if (wellObj != null)
                {
                    wellName = wellObj.Name + "<br/>";
                    if (wellObj.StartDate != null && wellObj.EndDate != null)
                    {
                        wellName = "<b style='color: blue'>" + wellObj.Name + "</b><br/>";
                        wellName += "(";
                        if (wellObj.StartDate != null)
                        {
                            wellName += "<b style='font-style: italic'>Start date: </b>" + wellObj.StartDate.Value.ToString("dd/MM/yyyy") + (wellObj.EndDate != null ? ") <br/>" : "<br/>");
                        }

                        if (wellObj.EndDate != null)
                        {
                            wellName += "<b style='font-style: italic'>End date: </b>" + wellObj.EndDate.Value.ToString("dd/MM/yyyy") + ") <br/>";
                        }
                    }
                }


                docObj.WellName = wellName;
            }
            else
            {
                docObj.WellName = string.Empty;
            }

            docObj.StartDate = this.txtStartDate.SelectedDate;
            docObj.EndDate = this.txtEndDate.SelectedDate;
            docObj.NumberOfWork = Convert.ToInt32(this.txtNumberOfWork.Value);
            docObj.TagNo = this.txtTagNo.Text.Trim();
            docObj.TagDes = this.txtTagDes.Text.Trim();
            docObj.Manufacturers = this.txtManufacturers.Text.Trim();
            docObj.SerialNo = this.txtSerialNo.Text.Trim();
            docObj.ModelNo = this.txtModelNo.Text.Trim();
            docObj.AssetNo = this.txtAssetNo.Text.Trim();
            docObj.TableOfContents = this.txtTableOfContents.Text.Trim();
            docObj.PublishDate = this.txtPublishDate.SelectedDate;

            docObj.FromId = Convert.ToInt32(this.ddlFrom.SelectedValue);
            docObj.FromName = this.ddlFrom.SelectedItem.Text;

            docObj.ToId = Convert.ToInt32(this.ddlTo.SelectedValue);
            docObj.ToName = this.ddlTo.SelectedItem.Text;

            docObj.Signer = this.txtSigner.Text.Trim();
            docObj.Other = this.txtOther.Text.Trim();

            if (rtvRIG != null && rtvRIG.SelectedNode != null)
            {
                docObj.RIGId = Convert.ToInt32(rtvRIG.SelectedNode.Value);
                docObj.RIGName = rtvRIG.SelectedNode.Text;
            }
            else
            {
                docObj.RIGName = string.Empty;
            }

            docObj.KindOfRepair = this.txtKindOfRepair.Text.Trim();

            docObj.IsPrivate = this.rbtnPrivate.Checked;
            docObj.IsPublish = this.rbtnPublish.Checked;
            docObj.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
        }

        private void LoadDocInfo(DocumentNew docObj)
        {

            var rtvPlant = (RadTreeView)this.ddlPlant.Items[0].FindControl("rtvPlant");
            var rtvSystem = (RadTreeView)this.ddlSystem.Items[0].FindControl("rtvSystem");
            var rtvDiscipline = (RadTreeView)this.ddlDiscipline.Items[0].FindControl("rtvDiscipline");
            var rtvDocumentType = (RadTreeView)this.ddlDocumentType.Items[0].FindControl("rtvDocumentType");
            var rtvTagType = (RadTreeView)this.ddlTagType.Items[0].FindControl("rtvTagType");
            var rtvProject = (RadTreeView)this.ddlProject.Items[0].FindControl("rtvProject");
            var rtvBlock = (RadTreeView)this.ddlBlock.Items[0].FindControl("rtvBlock");
            var rtvField = (RadTreeView)this.ddlField.Items[0].FindControl("rtvField");
            var rtvPlatform = (RadTreeView)this.ddlPlatform.Items[0].FindControl("rtvPlatform");
            var rtvWell = (RadTreeView)this.ddlWell.Items[0].FindControl("rtvWell");
            var rtvRIG = (RadTreeView)this.ddlRIG.Items[0].FindControl("rtvRIG");


            this.ddlCategory.SelectedValue = docObj.CategoryId.GetValueOrDefault().ToString();

            this.txtName.Text = docObj.Name;
            this.txtDescription.Text = docObj.Description;

            this.ddlRevision.SelectedValue = docObj.RevId.GetValueOrDefault().ToString();

            this.txtVendorName.Text = docObj.VendorName;
            this.txtDrawingNumber.Text = docObj.DrawingNumber;
            this.ddlYear.SelectedValue = docObj.Year.GetValueOrDefault().ToString();

            if (rtvPlant != null && docObj.PlantId != 0 && docObj.PlantId != null)
            {
                var nodeObj = rtvPlant.FindNodeByValue(docObj.PlantId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlPlant.Items[0].Text = nodeObj.Text;
                    this.ddlPlant.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvSystem != null && docObj.SystemId != 0 && docObj.SystemId != null)
            {
                var nodeObj = rtvSystem.FindNodeByValue(docObj.SystemId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlSystem.Items[0].Text = nodeObj.Text;
                    this.ddlSystem.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvDiscipline != null && docObj.DisciplineId != 0 && docObj.DisciplineId != null)
            {
                var nodeObj = rtvDiscipline.FindNodeByValue(docObj.DisciplineId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlDiscipline.Items[0].Text = nodeObj.Text;
                    this.ddlDiscipline.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvDocumentType != null && docObj.DocumentTypeId != 0 && docObj.DocumentTypeId != null)
            {
                var nodeObj = rtvDocumentType.FindNodeByValue(docObj.DocumentTypeId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlDocumentType.Items[0].Text = nodeObj.Text;
                    this.ddlDocumentType.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvTagType != null && !string.IsNullOrEmpty(docObj.TagTypeId))
            {
                ////var nodeObj = rtvTagType.FindNodeByValue(docObj.TagTypeId.GetValueOrDefault().ToString());
                ////if (nodeObj != null)
                ////{
                ////    nodeObj.Selected = true;
                ////    this.ddlTagType.Items[0].Text = nodeObj.Text;
                ////    this.ddlTagType.Items[0].Value = nodeObj.Value;
                ////}

                var tagtypeNames = string.Empty;

                foreach (var tagtypeId in docObj.TagTypeId.Split(',').Where(t => !string.IsNullOrEmpty(t)).ToList())
                {
                    var nodeObj = rtvTagType.FindNodeByValue(tagtypeId);
                    if (nodeObj != null)
                    {
                        nodeObj.Checked = true;
                        tagtypeNames += nodeObj.Text + ",";
                    }
                }

                this.ddlTagType.Items[0].Text = tagtypeNames;
            }

            if (rtvProject != null && docObj.ProjectId != 0 && docObj.ProjectId != null)
            {
                var nodeObj = rtvProject.FindNodeByValue(docObj.ProjectId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlProject.Items[0].Text = nodeObj.Text;
                    this.ddlProject.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvBlock != null && docObj.BlockId != 0 && docObj.BlockId != null)
            {
                var nodeObj = rtvBlock.FindNodeByValue(docObj.BlockId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlBlock.Items[0].Text = nodeObj.Text;
                    this.ddlBlock.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvField != null && docObj.FieldId != 0 && docObj.FieldId != null)
            {
                var nodeObj = rtvField.FindNodeByValue(docObj.FieldId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlField.Items[0].Text = nodeObj.Text;
                    this.ddlField.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvPlatform != null && docObj.PlatformId != 0 && docObj.PlatformId != null)
            {
                var nodeObj = rtvPlatform.FindNodeByValue(docObj.PlatformId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlPlatform.Items[0].Text = nodeObj.Text;
                    this.ddlPlatform.Items[0].Value = nodeObj.Value;
                }
            }

            if (rtvWell != null && docObj.WellId != 0 && docObj.WellId != null)
            {
                var nodeObj = rtvWell.FindNodeByValue(docObj.WellId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlWell.Items[0].Text = nodeObj.Text;
                    this.ddlWell.Items[0].Value = nodeObj.Value;
                }
            }

            this.txtStartDate.SelectedDate = docObj.StartDate ?? null;
            this.txtEndDate.SelectedDate = docObj.EndDate ?? null;
            this.txtNumberOfWork.Value = docObj.NumberOfWork;
            this.txtTagNo.Text = docObj.TagNo;
            this.txtTagDes.Text = docObj.TagDes;
            this.txtManufacturers.Text = docObj.Manufacturers;
            this.txtSerialNo.Text = docObj.SerialNo;
            this.txtModelNo.Text = docObj.ModelNo;
            this.txtAssetNo.Text = docObj.AssetNo;
            this.txtTableOfContents.Text = docObj.TableOfContents;
            this.txtPublishDate.SelectedDate = docObj.PublishDate ?? null;

            this.ddlFrom.SelectedValue = docObj.FromId.GetValueOrDefault().ToString();

            this.ddlTo.SelectedValue = docObj.ToId.GetValueOrDefault().ToString();

            this.txtSigner.Text = docObj.Signer;
            this.txtOther.Text = docObj.Other;

            if (rtvRIG != null && docObj.RIGId != 0 && docObj.RIGId != null)
            {
                var nodeObj = rtvRIG.FindNodeByValue(docObj.RIGId.GetValueOrDefault().ToString());
                if (nodeObj != null)
                {
                    nodeObj.Selected = true;
                    this.ddlRIG.Items[0].Text = nodeObj.Text;
                    this.ddlRIG.Items[0].Value = nodeObj.Value;
                }
            }

            this.txtKindOfRepair.Text = docObj.KindOfRepair;

            this.rbtnPrivate.Checked = docObj.IsPrivate.GetValueOrDefault();
            this.rbtnPublish.Checked = docObj.IsPublish.GetValueOrDefault();
            this.rbtnInGroup.Checked = !docObj.IsPrivate.GetValueOrDefault() && !docObj.IsPublish.GetValueOrDefault();

            var createdUser = this.userService.GetByID(docObj.CreatedBy.GetValueOrDefault());
            this.lblCreated.Text = "Created at "
                                   + docObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by "
                                   + (createdUser != null ? createdUser.FullName : string.Empty);

            if (docObj.LastUpdatedBy != null && docObj.LastUpdatedDate != null)
            {
                this.lblCreated.Text += "<br/>";
                var lastUpdatedUser = this.userService.GetByID(docObj.LastUpdatedBy.GetValueOrDefault());
                this.lblUpdated.Text = "Last modified at "
                                       + docObj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt")
                                       + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
            }
            else
            {
                this.lblUpdated.Visible = false;
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
                this.fileNameValidator.ErrorMessage = "Please enter file name.";
                //this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                var docId = Convert.ToInt32(Request.QueryString["docId"]);
                this.fileNameValidator.ErrorMessage = "The specified name is already in use.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = true; ////!this.documentService.IsDocumentExistUpdate(folderId, this.txtName.Text.Trim(), docId);
            }
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("CheckFileName"))
            {
                var fileName = e.Argument.Split('$')[1];
                var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                
                if(this.documentService.IsDocumentExist(folderId, fileName))
                {
                    var docObjLeaf = this.documentService.GetSpecificDocument(folderId, fileName);
                    if (docObjLeaf != null)
                    {
                        ////this.txtDocumentNumber.Text = docObjLeaf.DocumentNumber;
                        ////this.txtTitle.Text = docObjLeaf.Title;
                        this.ddlDocumentType.SelectedValue = docObjLeaf.DocumentTypeID.GetValueOrDefault().ToString();
                        ////this.ddlStatus.SelectedValue = docObjLeaf.StatusID.GetValueOrDefault().ToString();
                        this.ddlDiscipline.SelectedValue = docObjLeaf.DisciplineID.GetValueOrDefault().ToString();
                        ////this.ddlReceivedFrom.SelectedValue = docObjLeaf.ReceivedFromID.GetValueOrDefault().ToString();
                        ////this.txtReceivedDate.SelectedDate = docObjLeaf.ReceivedDate.GetValueOrDefault();
                        ////this.ddlLanguage.SelectedValue = docObjLeaf.LanguageID.GetValueOrDefault().ToString();
                        ////this.txtWell.Text = docObjLeaf.Well;
                        ////this.txtKeywords.Text = docObjLeaf.KeyWords;

                        this.docUploadedIsExist.Value = "true";
                        this.docIdUpdateUnIsLeaf.Value = docObjLeaf.ID.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// The docuploader_ file uploaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void docuploader_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            var folderId = Convert.ToInt32(Request.QueryString["folId"]);
            var fileName = e.File.FileName;

            this.txtName.Text = fileName;
            if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
            {
                this.txtName.ReadOnly = false;
            }
            else
            {
                this.txtName.ReadOnly = true;
            }

            if (this.documentService.IsDocumentExist(folderId, fileName))
            {
                var docObjLeaf = this.documentService.GetSpecificDocument(folderId, fileName);
                if (docObjLeaf != null)
                {
                    if (this.Session["IsFillData"] == null || this.Session["IsFillData"].ToString() != "false")
                    {
                        ////this.txtDocumentNumber.Text = docObjLeaf.DocumentNumber;
                        ////this.txtTitle.Text = docObjLeaf.Title;
                        this.ddlDocumentType.SelectedValue = docObjLeaf.DocumentTypeID.GetValueOrDefault().ToString();
                        ////this.ddlStatus.SelectedValue = docObjLeaf.StatusID.GetValueOrDefault().ToString();
                        this.ddlDiscipline.SelectedValue = docObjLeaf.DisciplineID.GetValueOrDefault().ToString();
                        ////this.ddlReceivedFrom.SelectedValue = docObjLeaf.ReceivedFromID.GetValueOrDefault().ToString();
                        ////this.txtReceivedDate.SelectedDate = docObjLeaf.ReceivedDate;
                        ////this.ddlLanguage.SelectedValue = docObjLeaf.LanguageID.GetValueOrDefault().ToString();
                        ////this.txtWell.Text = docObjLeaf.Well;
                        ////this.txtKeywords.Text = docObjLeaf.KeyWords;
                        ////this.txtRemark.Text = docObjLeaf.Remark;
                        ////this.txtTransmittalNumber.Text = docObjLeaf.TransmittalNumber;
                    }
                    
                    this.Session.Add("IsFillData", "false");

                    this.docUploadedIsExist.Value = "true";
                    this.docIdUpdateUnIsLeaf.Value = docObjLeaf.ID.ToString();
                }
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            

            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            ////var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId).OrderBy(t => t.Name);

            var revisionList = this.revisionService.GetAll();
            revisionList.Insert(0, new Revision() { Name = string.Empty });
            this.ddlRevision.DataSource = revisionList;
            this.ddlRevision.DataValueField = "ID";
            this.ddlRevision.DataTextField = "Name";
            this.ddlRevision.DataBind();

            this.rtvOptionalTypeDetailTemp.DataSource = listOptionalTypeDetail;
            this.rtvOptionalTypeDetailTemp.DataFieldParentID = "ParentId";
            this.rtvOptionalTypeDetailTemp.DataTextField = "Name";
            this.rtvOptionalTypeDetailTemp.DataValueField = "ID";
            this.rtvOptionalTypeDetailTemp.DataFieldID = "ID";
            this.rtvOptionalTypeDetailTemp.DataBind();

            for (var i = DateTime.Now.Year; i >= 1975; i--)
            {
                this.ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            this.ddlYear.Items.Insert(0, new ListItem(string.Empty, "0"));
            this.ddlYear.SelectedIndex = 0;

            // Bind Plant object
            this.BindTreeViewCombobox(this.PlantOptType, this.ddlPlant, "rtvPlant", listOptionalTypeDetail);

            // Bind System object
            this.BindTreeViewCombobox(this.SystempOptType, this.ddlSystem, "rtvSystem", listOptionalTypeDetail);

            // Bind TagType object
            this.BindTreeViewCombobox(this.TagOptType, this.ddlTagType, "rtvTagType", listOptionalTypeDetail);

            // Bind Discipline object
            this.BindTreeViewCombobox(this.DisciplineOptType, this.ddlDiscipline, "rtvDiscipline", listOptionalTypeDetail);

            // Bind Document type object
            this.BindTreeViewCombobox(this.DocumentTypeOptType, this.ddlDocumentType, "rtvDocumentType", listOptionalTypeDetail);

            // Bind Project object
            this.BindTreeViewCombobox(this.ProjectOptType, this.ddlProject, "rtvProject", listOptionalTypeDetail);

            // Bind Block object
            this.BindTreeViewCombobox(this.BlockOptType, this.ddlBlock, "rtvBlock", listOptionalTypeDetail);

            // Bind Field object
            this.BindTreeViewCombobox(this.FieldOptType, this.ddlField, "rtvField", listOptionalTypeDetail);

            // Bind Platform object
            this.BindTreeViewCombobox(this.PlatformOptType, this.ddlPlatform, "rtvPlatform", listOptionalTypeDetail);

            // Bind Well object
            this.BindTreeViewCombobox(this.WellOptType, this.ddlWell, "rtvWell", listOptionalTypeDetail);

            // Bind RIG object
            this.BindTreeViewCombobox(this.RIGOptType, this.ddlRIG, "rtvRIG", listOptionalTypeDetail);

            var listOriginator = this.originatorService.GetAll();
            listOriginator.Insert(0, new Originator() { ID = 0, Name = string.Empty });
            this.ddlFrom.DataSource = listOriginator;
            this.ddlFrom.DataValueField = "ID";
            this.ddlFrom.DataTextField = "Name";
            this.ddlFrom.DataBind();
            this.ddlFrom.SelectedIndex = 0;

            this.ddlTo.DataSource = listOriginator;
            this.ddlTo.DataValueField = "ID";
            this.ddlTo.DataTextField = "Name";
            this.ddlTo.DataBind();
            this.ddlTo.SelectedIndex = 0;
        }

        /// <summary>
        /// The repair list.
        /// </summary>
        /// <param name="listOptionalTypeDetail">
        /// The list optional type detail.
        /// </param>
        private void RepairList(ref List<OptionalTypeDetail> listOptionalTypeDetail)
        {
            var temp = listOptionalTypeDetail.Where(t => t.ParentId != null).Select(t => t.ParentId).Distinct().ToList();
            var temp2 = listOptionalTypeDetail.Select(t => t.ID).ToList();

            foreach (var x in temp)
            {
                if (!temp2.Contains(x.Value))
                {
                    var tempList = listOptionalTypeDetail.Where(t => t.ParentId == x.Value).ToList();
                    foreach (var optionalTypeDetail in tempList)
                    {
                        optionalTypeDetail.ParentId = null;
                    }
                }
            }
        }

        /// <summary>
        /// The bind tree view combobox.
        /// </summary>
        /// <param name="optionalType">
        /// The optional type.
        /// </param>
        /// <param name="ddlObj">
        /// The ddl obj.
        /// </param>
        /// <param name="rtvName">
        /// The rtv name.
        /// </param>
        /// <param name="listOptionalTypeDetailFull">
        /// The list optional type detail full.
        /// </param>
        private void BindTreeViewCombobox(int optionalType, RadComboBox ddlObj, string rtvName, IEnumerable<OptionalTypeDetail> listOptionalTypeDetailFull)
        {
            var rtvobj = (RadTreeView)ddlObj.Items[0].FindControl(rtvName);
            if (rtvobj != null)
            {
                var listOptionalTypeDetail = listOptionalTypeDetailFull.Where(t => t.OptionalTypeId == optionalType).ToList();
                this.RepairList(ref listOptionalTypeDetail);

                rtvobj.DataSource = listOptionalTypeDetail;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();

                if (rtvobj.Nodes.Count > 0)
                {
                    rtvobj.Nodes[0].Expanded = true;
                }
            }
        }

        private void LoadViewPropertiesConfig()
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var selectedProperty = new List<string>();
            if (this.ddlCategory.Items.Count > 0)
            {
                var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
                var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                                      ? UserSession.Current.RoleId
                                      : 0;
                foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(categoryId, deparmentId))
                {
                    var temp =
                        docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(
                            t => t.Trim()).ToList();
                    selectedProperty.AddRange(temp);
                }

                selectedProperty = selectedProperty.Distinct().ToList();
                this.btnGetInfo.Visible = selectedProperty.Contains("3");
                var totalProperty = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TotalProperty"));
                for (int i = 1; i <= totalProperty; i++)
                {
                    var property = (HtmlGenericControl)this.divContent.FindControl("Index" + i);
                    if (property != null)
                    {
                        property.Visible = selectedProperty.Contains(i.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// The save upload file.
        /// </summary>
        /// <param name="uploadDocControl">
        /// The upload doc control.
        /// </param>
        /// <param name="objDoc">
        /// The obj Doc.
        /// </param>
        private void SaveUploadFile(RadAsyncUpload uploadDocControl, ref Document objDoc, bool isUpdateOldRev)
        {
            var listUpload = uploadDocControl.UploadedFiles;
            var folder = this.folderService.GetById(objDoc.FolderID.GetValueOrDefault());
            var targetFolder = "../../" + folder.DirName;
            var revisionPath = "../../DocumentLibrary/RevisionHistory/";
            var serverFolder = HostingEnvironment.ApplicationVirtualPath + "/" + folder.DirName;
            var serverRevisionFolder = HostingEnvironment.ApplicationVirtualPath + "/DocumentLibrary/RevisionHistory/";


            var fileIcon = new Dictionary<string, string>()
                {
                    { "doc", "images/wordfile.png" },
                    { "docx", "images/wordfile.png" },
                    { "dotx", "images/wordfile.png" },
                    { "xls", "images/excelfile.png" },
                    { "xlsx", "images/excelfile.png" },
                    { "pdf", "images/pdffile.png" },
                    { "7z", "images/7z.png" },
                    { "dwg", "images/dwg.png" },
                    { "dxf", "images/dxf.png" },
                    { "rar", "images/rar.png" },
                    { "zip", "images/zip.png" },
                    { "txt", "images/txt.png" },
                    { "xml", "images/xml.png" },
                    { "xlsm", "images/excelfile.png" },
                    { "bmp", "images/bmp.png" },
                };

            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {
                    var docRevisionFileName = string.Empty;
                    var docFileNameOrignal = docFile.FileName;
                    if (!string.IsNullOrEmpty(objDoc.RevisionName))
                    {
                        docRevisionFileName = objDoc.RevisionName + "_" + docFile.FileName;
                    }
                    else
                    {
                        docRevisionFileName = docFile.FileName;
                    }

                    var revisionServerFileName = DateTime.Now.ToString("ddMMyyhhmmss") + "_" + docRevisionFileName;

                    // Path file to save on server disc
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileNameOrignal);
                    var saveFileRevisionPath = Path.Combine(Server.MapPath(revisionPath), revisionServerFileName);

                    // Path file to download from server
                    var serverFilePath = serverFolder + "/" + docFileNameOrignal;
                    var revisionFilePath = serverRevisionFolder + revisionServerFileName;

                    var fileExt = docFileNameOrignal.Substring(docFileNameOrignal.LastIndexOf(".") + 1, docFileNameOrignal.Length - docFileNameOrignal.LastIndexOf(".") - 1);

                    objDoc.RevisionFileName = docRevisionFileName;
                    objDoc.FilePath = serverFilePath;
                    objDoc.RevisionFilePath = revisionFilePath;
                    objDoc.FileExtension = fileExt;
                    objDoc.FileExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "images/otherfile.png";
                    objDoc.FileNameOriginal = docFileNameOrignal;
                    objDoc.DirName = folder.DirName;
                    ////EDMSFolderWatcher
                    var watcherService = new ServiceController(ServiceName);
                    if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                    {
                        watcherService.ExecuteCommand(128);
                    }

                    if (isUpdateOldRev)
                    {
                        docFile.SaveAs(saveFileRevisionPath, true);
                    }
                    else
                    {
                        docFile.SaveAs(saveFilePath, true);
                        var fileinfo = new FileInfo(saveFilePath);
                        fileinfo.CopyTo(saveFileRevisionPath, true);    
                    }
                    
                    if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                    {
                        watcherService.ExecuteCommand(129);
                    }
                }
            }
        }

        /// <summary>
        /// The ddl category_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadViewPropertiesConfig();
            this.LoadComboData();
        }

        protected void rtvProject_Nodeclick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 7).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlBlock.Items[0].FindControl("rtvBlock");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvBlock_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 8).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlField.Items[0].FindControl("rtvField");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvField_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 9).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlPlatform.Items[0].FindControl("rtvPlatform");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvPlatform_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var deparmentId = isViewByGroup && !UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() ? UserSession.Current.RoleId.ToString() : "0";
            var listOptionalTypeDetail = this.optionalTypeDetailService.GetAllSpecial(this.ddlCategory.SelectedValue, 0, deparmentId);

            var selectedNode = this.rtvOptionalTypeDetailTemp.FindNodeByValue(e.Node.Value);
            var childNodeId = selectedNode.GetAllNodes().Select(t => Convert.ToInt32(t.Value));

            var listTemp = listOptionalTypeDetail.Where(t => childNodeId.Contains(t.ID) && t.OptionalTypeId == 10).ToList();
            this.RepairList(ref listTemp);

            var rtvobj = (RadTreeView)this.ddlWell.Items[0].FindControl("rtvWell");
            if (rtvobj != null)
            {
                rtvobj.DataSource = listTemp;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void btnGetInfo_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                var categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
                var docObj = this.documentNewService.GetByName(this.txtName.Text.Trim(), categoryId);
                if (docObj != null)
                {
                    Session.Add("ExistDoc", true);
                    this.LoadDocInfo(docObj);
                }
            }
        }

        protected void ddlRevFullDoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var docId = Convert.ToInt32(this.ddlRevFullDoc.SelectedValue);
            var docObj = this.documentNewService.GetById(docId);
            if (docObj != null)
            {
                this.LoadDocInfo(docObj);
            }
        }
    }
}