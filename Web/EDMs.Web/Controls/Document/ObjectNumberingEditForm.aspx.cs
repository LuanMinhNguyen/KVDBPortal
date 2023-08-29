// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ObjectNumberingEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly PECC2ObjectNumberingService objectNumberingService;

        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// 
        /// </summary>
        public ObjectNumberingEditForm()
        {
            this.userService = new UserService();
            this.objectNumberingService = new PECC2ObjectNumberingService();
            this.documentTypeService = new DocumentTypeService();
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

                    var obj = this.objectNumberingService.GetById(Convert.ToInt32(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {

                        this.txtSegmentLenght.Value = obj.SegmentLenght;
                        this.txtFullFormat.Text = obj.FullFormat;
                        this.txtShortFormat.Text = obj.RealFormat;
                        this.ddlDocumentType.SelectedValue = obj.DocumentTypeId.ToString();

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
                    var obj = this.objectNumberingService.GetById(objId);
                    if (obj != null)
                    {
                        this.CollectData(obj);

                        obj.DocumentTypeId = Convert.ToInt32(this.ddlDocumentType.SelectedValue);
                        obj.DocumentTypeName = this.ddlDocumentType.SelectedItem.Text;

                        obj.LastUpdatedBy = UserSession.Current.User.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        this.objectNumberingService.Update(obj);
                        
                    }
                }
                else
                {
                    var obj = new PECC2ObjectNumbering();
                    this.CollectData(obj);
                    obj.DocumentTypeId = Convert.ToInt32(this.ddlDocumentType.SelectedValue);
                    obj.DocumentTypeName = this.ddlDocumentType.SelectedItem.Text;
                    obj.CreatedBy = UserSession.Current.User.Id;
                    obj.CreatedDate = DateTime.Now;

                    this.objectNumberingService.Insert(obj);

                    // Auto apply document numbering rule for child
                    var docTypeChildList = this.documentTypeService.GetAllByParent(obj.DocumentTypeId.GetValueOrDefault());
                    foreach (var documentType in docTypeChildList)
                    {
                        var objChild = new PECC2ObjectNumbering
                        {
                            DocumentTypeId = documentType.ID,
                            DocumentTypeName = documentType.Code,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedDate = DateTime.Now
                        };

                        this.CollectData(objChild);

                        this.objectNumberingService.Insert(objChild);
                    }
                    // ---------------------------------------------------
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(PECC2ObjectNumbering obj)
        {
            obj.SegmentLenght = this.txtSegmentLenght.Value;
            obj.FullFormat = this.txtFullFormat.Text;
            obj.RealFormat = this.txtShortFormat.Text;
            obj.FullFormatName = string.Empty;
            foreach (var item in obj.FullFormat.Split('-'))
            {
                switch (item)
                {
                    case "1":
                        obj.FullFormatName += "-[Project Code]";
                        break;
                    case "2":
                        obj.FullFormatName += "-[Document Type]";
                        break;
                    case "3":
                        obj.FullFormatName += "-[Originating Organization]";
                        break;
                    case "4":
                        obj.FullFormatName += "-[Receiving Organization]";
                        break;
                    case "5":
                        obj.FullFormatName += "-[Year]";
                        break;
                    case "6":
                        obj.FullFormatName += "-[Group]";
                        break;
                    case "7":
                        obj.FullFormatName += "-[Sequential Number]";
                        break;
                    case "8":
                        obj.FullFormatName += "-[Content Code]";
                        break;
                    case "9":
                        obj.FullFormatName += "-[Discipline Code]";
                        break;
                    case "10":
                        obj.FullFormatName += "-[Numbering System]";
                        break;
                }
            }
            obj.FullFormatName = obj.FullFormatName.Remove(0, 1);

            obj.RealFormatName = string.Empty;
            
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

        private void LoadComboData()
        {
            var docTypeList = this.documentTypeService.GetAll().OrderBy(t => t.ParentId).ThenBy(t => t.Code.Length).ThenBy(t => t.Code).ToList();
            this.ddlDocumentType.DataSource = docTypeList;
            this.ddlDocumentType.DataTextField = "FullName";
            this.ddlDocumentType.DataValueField = "ID";
            this.ddlDocumentType.DataBind();
        }
    }
}