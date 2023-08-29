// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.Hosting;
    using System.Web.UI.WebControls;
    using System.IO;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Workflow;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;
    using EDMs.Web.Utilities;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class VariationLogEditForm : Page
    {
        /// <summary>
        /// The service name.
        /// </summary>

        private readonly ProjectCodeService projectcodeService;


        private readonly VariationLogService variationLogService;

        private readonly VariationLogAttachFileService AttachFileService;

        private readonly OrganizationCodeService organizationCodeService;
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public VariationLogEditForm()
        {
            this.variationLogService = new  VariationLogService();
            this.userService = new UserService();
            this.projectcodeService = new ProjectCodeService();
            this.AttachFileService = new  VariationLogAttachFileService();
            this.organizationCodeService = new OrganizationCodeService();
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
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projecObj = this.projectcodeService.GetById(projectId);
                this.LoadComboData(projecObj);
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid objId;
                    Guid.TryParse(this.Request.QueryString["objId"], out objId);
                    var obj = this.variationLogService.GetById(objId);
                    if (obj != null)
                    {
                        Session.Add("LogID", objId);
                        this.LoadDocInfo(obj, projecObj);
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
                    Session.Add("LogID", Guid.NewGuid());
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
            if (this.Page.IsValid )
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
                var projecObj = this.projectcodeService.GetById(projectId);
                VariationLog Obj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var tId = new Guid(this.Request.QueryString["objId"]);
                    Obj = this.variationLogService.GetById(tId);
                    if (Obj != null)
                    {
                        this.CollectData(ref Obj, projecObj);

                        Obj.LastUpdatedBy = UserSession.Current.User.Id;
                        Obj.LastUpdatedByName = UserSession.Current.User.FullName;
                        Obj.LastUpdatedDate = DateTime.Now;
                        this.variationLogService.Update(Obj);
                    }
                }
                else
                {
                    Obj = new VariationLog()
                    {
                        ID = new Guid(Session["LogID"].ToString()),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        IssuedDate=DateTime.Now,
                        IsDelete = false,
                    };

                    this.CollectData(ref Obj, projecObj);

                    this.variationLogService.Insert(Obj);

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
            //if (this.txtDocNo.Text.Trim().Length == 0)
            //{
            //    this.fileNameValidator.ErrorMessage = "Please enter Document Number.";
            //    this.divDocNo.Style["margin-bottom"] = "-26px;";
            //    args.IsValid = false;
            //}
            //else if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            //{
            //    Guid objId;
            //    Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);

            //    if (this._PECC2DocumentService.IsExistByDocNo(this.txtDocNumber.Text.Trim()) && objId == null)
            //    {
            //        this.fileNameValidator.ErrorMessage = "Document No. is already exist.";
            //        this.divDocNo.Style["margin-bottom"] = "-5px;";
            //        args.IsValid = false;
            //    }
            //}
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
            if (e.Argument == "Rebind")
            {
                this.grdDocument.Rebind();
            }
        }

       /// <summary>
       /// add
       /// </summary>
       /// <param name="projecObj"></param>
        private void LoadComboData(ProjectCode projecObj)
        {
            this.txtProjectCode.Text = projecObj.FullName;

            //var groupList = this.groupCodeService.GetAll();
            //this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            //this.ddlGroup.DataTextField = "FullName";
            //this.ddlGroup.DataValueField = "ID";
            //this.ddlGroup.DataBind();

            var organizationCodeList = this.organizationCodeService.GetAll().OrderBy(t => t.Code).ToList();
            this.ddlOriginatingOrganization.DataSource = organizationCodeList;
            this.ddlOriginatingOrganization.DataTextField = "FullName";
            this.ddlOriginatingOrganization.DataValueField = "ID";
            this.ddlOriginatingOrganization.DataBind();
        }

        private void CollectData(ref VariationLog obj, ProjectCode projecObj)
        {

            obj.Title = this.txtTitle.Text;
            obj.InstructionProposal = this.txtinstruction.Text;
            obj.Order = this.txtorder.Text;
            obj.System = this.txtSystem.Text;
            obj.ContractRequirement = this.txtContract.Text;
            obj.CostImpact = this.txtCost.Value;
            obj.scheduleImpact =(int)this.txtSchedule.Value;
            obj.IssuedDate = this.txtIssuedDate.SelectedDate;
            obj.OtherAttachment = this.txtAttachment.Text;
            obj.Remark = this.txtRemark.Text;
            obj.Description = this.txtDescription.Text.Trim();
            obj.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            obj.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
            obj.ProjectId = projecObj.ID;
            obj.ProjectCode = projecObj.Code;
        }

        private void LoadDocInfo(VariationLog obj, ProjectCode projecObj)
        {
            this.txtTitle.Text = obj.Title;
            this.txtinstruction.Text = obj.InstructionProposal;
            this.txtorder.Text = obj.Order;
            this.txtProjectCode.Text = projecObj.FullName;
            this.txtSystem.Text = obj.System;
            this.txtContract.Text = obj.ContractRequirement;
            this.txtDescription.Text = obj.Description;
            this.txtCost.Value = obj.CostImpact;
            this.txtSchedule.Value = obj.scheduleImpact;
            this.txtIssuedDate.SelectedDate = obj.IssuedDate;
            this.txtRemark.Text = obj.Remark;
            this.txtAttachment.Text = obj.OtherAttachment;
            this.ddlOriginatingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString();
           
        }
        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Session["LogID"].ToString()))
            {
                var objId = new Guid(Session["LogID"].ToString());
                var attachList = this.AttachFileService.GetByVariation(objId).OrderBy(t => t.CreatedDate);
                this.grdDocument.DataSource = attachList;
            }
            else
            {
                this.grdDocument.DataSource = new List<VariationLogAttachFile>();
            }
        }
       protected void btUpLoadFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session["LogID"].ToString()))
            {
                Guid objId;
                Guid.TryParse(Session["LogID"].ToString(), out objId);
                var docObj = this.AttachFileService.GetById(objId);

                var targetFolder = "../../DocumentLibrary/VariationLog";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/VariationLog";
                var listUpload = docuploader.UploadedFiles;

                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileName = docFile.FileName;
                        var fileExt = docFile.GetExtension();

                        var serverDocFileName = docFileName;
                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new VariationLogAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            VariationLogId = objId,
                            FileName = serverDocFileName,
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                            FileSize = (double)docFile.ContentLength / 1024,
                            TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
                            TypeName = this.ddlType.SelectedItem.Text,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };

                        this.AttachFileService.Insert(attachFile);

                        var variaObj = this.variationLogService.GetById(objId);
                        if(variaObj != null)
                        {
                            var listattach = this.AttachFileService.GetByVariation(objId);
                            var instruction = string.Empty;
                            var order = string.Empty;
                            var other = string.Empty;
                            foreach(var item in listattach)
                            {
                                switch (item.TypeId)
                                {
                                    case 1:
                                        instruction += instruction + Environment.NewLine + item.FileName ;
                                        break;
                                    case 2:
                                       order+= order + Environment.NewLine + item.FileName;
                                        break;
                                    case 3:
                                       other += other + Environment.NewLine + item.FileName;
                                        break;
                                }
                            }
                            this.txtinstruction.Text = instruction;
                            this.txtorder.Text = order;
                            this.txtAttachment.Text = other;
                            variaObj.InstructionProposal = instruction;
                            variaObj.Order = order;
                            variaObj.OtherAttachment = other;
                            this.variationLogService.Update(variaObj);
                        }
                        else
                        {
                            switch (Convert.ToInt32(this.ddlType.SelectedValue))
                            {
                                case 1:
                                    this.txtinstruction.Text += this.txtinstruction.Text + Environment.NewLine + docFileName;
                                    break;
                                case 2:
                                    this.txtorder.Text += this.txtorder.Text + Environment.NewLine + docFileName;
                                    break;
                                case 3:
                                    this.txtAttachment.Text += this.txtAttachment.Text + Environment.NewLine + docFileName;
                                    break;
                            }
                        }
                    }
                }
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdDocument.Rebind();
        }

        protected void grdDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.grdDocument.Rebind();
            }
        }
    }
}