// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ShipmentEditForm : Page
    {
        private readonly EquipmentService equimentService;

        private readonly ShipmentService shipmentService;

        private readonly ShipmentDocumentFileService shipmentdocfileService;

        private readonly ProjectCodeService projectcodeService;

        private readonly UserService userService;

        private readonly AttachFilesShipmentService attachfileshipment;
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ShipmentEditForm()
        {
            this.equimentService = new EquipmentService();
            this.shipmentService = new ShipmentService();
            this.shipmentdocfileService = new ShipmentDocumentFileService();
            this.projectcodeService = new ProjectCodeService();
            this.userService = new UserService();
            this.attachfileshipment = new AttachFilesShipmentService();
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
                if (string.IsNullOrEmpty(this.Request.QueryString["fromToDoList"]))
                {
                    this.grdDocument.Visible = false;
                }
                else
                {
                    this.btnSave.Visible = false;
                }
                if (!string.IsNullOrEmpty(this.Request.QueryString["projId"]))
                {
                    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                    var projObj = this.projectcodeService.GetById(projectId);
                    this.txtProjectName.Text = projObj.Code;
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["ShipmentId"]))
                {
                    this.CreatedInfo.Visible = true;
                    var materialId = new Guid(this.Request.QueryString["ShipmentId"]);
                    var materialObj = this.shipmentService.GetById(materialId);
                    if (materialObj != null)
                    {

                        this.txtProjectName.Text = materialObj.ProjectName;

                        this.LoadDocInfo(materialObj);

                        var createdUser = this.userService.GetByID(materialObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + materialObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (materialObj.UpdatedBy != null && materialObj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(materialObj.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + materialObj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }

                        if (materialObj.IsSend.GetValueOrDefault() && !UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
                        {
                            this.grdDocument.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                }

                this.CheckInternal();

                if (UserSession.Current.User.IsEngineer.GetValueOrDefault())
                {
                    this.btnSave.Visible = false;
                }
            }
        }

        private void CheckInternal()
        {
            if (UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            {
                this.txtNumber.Enabled = false;
                this.txtDescription.Enabled = false;
                this.txtDate.Enabled = false;
                this.ddlShipmentType.Enabled = false;
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
            if (this.Page.IsValid && !string.IsNullOrEmpty(this.Request.QueryString["projId"]))
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projObj = this.projectcodeService.GetById(projectId);
                Shipment materialObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["ShipmentId"]))
                {
                    var docId = new Guid(this.Request.QueryString["ShipmentId"]);
                    materialObj = this.shipmentService.GetById(docId);
                    if (materialObj != null)
                    {

                        this.CollectData(ref materialObj);

                        materialObj.UpdatedBy = UserSession.Current.User.Id;
                        materialObj.UpdatedDate = DateTime.Now;
                        materialObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        this.shipmentService.Update(materialObj);
                    }
                }
                else
                {
                    materialObj = new Shipment()
                    {   ID=Guid.NewGuid(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        CreatedByName=UserSession.Current.User.FullName,
                        IsSend=false,
                    };

                    this.CollectData(ref materialObj);
                    materialObj.ProjectID = projObj.ID;
                    materialObj.ProjectName = projObj.Code;

                    this.shipmentService.Insert(materialObj);

                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
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
            ////if(this.txtName.Text.Trim().Length == 0)
            ////{
            ////    this.fileNameValidator.ErrorMessage = "Please enter file name.";
            ////    this.divFileName.Style["margin-bottom"] = "-26px;";
            ////    args.IsValid = false;
            ////}
            ////else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            ////{
            ////    var docId = Convert.ToInt32(Request.QueryString["docId"]);
            ////    this.fileNameValidator.ErrorMessage = "The specified name is already in use.";
            ////    this.divFileName.Style["margin-bottom"] = "-26px;";
            ////    args.IsValid = true; ////!this.documentService.IsDocumentExistUpdate(folderId, this.txtName.Text.Trim(), docId);
            ////}
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

                //    if(this.documentService.IsDocumentExist(folderId, fileName))
                //    {
                //    }
            }
        }

        private void CollectData(ref Shipment materialObj)
        {

            materialObj.Number = this.txtNumber.Text.Trim();
            materialObj.Description = this.txtDescription.Text;
            materialObj.Date = this.txtDate.SelectedDate;
            materialObj.ShipmentTypeId = this.ddlShipmentType.SelectedValue;
            materialObj.ShipmentTypeName = this.ddlShipmentType.SelectedItem.Text;
            materialObj.ShipmentStatusID = this.ddlShipmentStatus.SelectedValue;
            materialObj.ShipmentStatusName = this.ddlShipmentStatus.SelectedItem.Text;

        }

        private void LoadDocInfo(Shipment materialObj)
        {

            this.txtNumber.Text = materialObj.Number;
            this.txtDescription.Text = materialObj.Description;
            this.txtDate.SelectedDate = materialObj.Date;
            this.ddlShipmentStatus.SelectedValue = materialObj.ShipmentStatusID;
            this.ddlShipmentType.SelectedValue = materialObj.ShipmentTypeId;
        }

        protected void grdDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["ShipmentId"]))
            {
                var materialId = new Guid(this.Request.QueryString["ShipmentId"]);
                var listfile = this.attachfileshipment.GetAllByShipment(materialId);
                this.grdDocument.DataSource = listfile;
            }
        }
    }
}