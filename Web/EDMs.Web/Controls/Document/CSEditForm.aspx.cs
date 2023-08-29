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

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class CSEditForm : Page
    {
        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly ProjectCodeService projectcodeService;
        private readonly GroupCodeService groupCodeService;
        private readonly ChangeRequestService changeRequestService;

        private readonly NCR_SIService ncrSiService;

        private readonly OrganizationCodeService organizationCodeService;

        private readonly NCR_SIAttachFileService ncrSiAttachFileService;

        private readonly NCR_SIAddPictureService ncrsiAddPictureService;
        private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public CSEditForm()
        {
            this.groupCodeService = new GroupCodeService();
            this.userService = new UserService();
            this.projectcodeService = new ProjectCodeService();
            this.changeRequestService = new ChangeRequestService();
            this.ncrSiService = new NCR_SIService();
            this.organizationCodeService = new OrganizationCodeService();
            this.ncrSiAttachFileService = new NCR_SIAttachFileService();
            this.ncrsiAddPictureService = new NCR_SIAddPictureService();
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
                var projectObj = this.projectcodeService.GetById(projectId);
                this.LoadComboData(projectObj);
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid objId;
                    Guid.TryParse(this.Request.QueryString["objId"],out objId);
                    var obj = this.ncrSiService.GetById(objId);
                    if (obj != null)
                    {
                        Session.Add("NCRSIID", objId);
                        this.LoadDocInfo(obj, projectObj);
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
                        this.liCancel.Visible = UserSession.Current.User.IsAdmin.GetValueOrDefault();
                        this.cbIsCancel.Enabled = !obj.IsCancel.GetValueOrDefault();
                        if (obj.IsCancel.GetValueOrDefault())
                        {
                            this.rgdTreatment.Enabled = false;
                            this.btnSave.Visible = false;
                        }
                    }
                }
                else
                {
                    Session.Add("NCRSIID", Guid.NewGuid());
                    this.CreatedInfo.Visible = false;
                    if(!string.IsNullOrEmpty(this.Request.QueryString["Replate"]) && !string.IsNullOrEmpty(this.Request.QueryString["ReobjId"]))
                    {
                        Guid objId;
                        Guid.TryParse(this.Request.QueryString["ReobjId"], out objId);
                        var obj = this.ncrSiService.GetById(objId);
                        if (obj != null)
                        {
                            this.txtRelatedCSNo.Text = obj.Number;
                            this.txtProjectCode.Text = projectObj.FullName;
                            var lead = obj.DisciplineID.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => Convert.ToInt32(t)).ToList();
                            foreach (RadComboBoxItem item in this.ddlDiscipline.Items)
                            {
                                if (lead.Contains(Convert.ToInt32(item.Value)))
                                {
                                    item.Checked = true;
                                }
                            }
                            if (obj.ReceivingOrganizationId.ToString() != "")
                            {
                                this.ddlOriginatingOrganization.SelectedValue = obj.ReceivingOrganizationId.ToString();
                            }

                            if (obj.OriginatingOrganizationId.ToString() != "")
                            {
                                this.ddlReceivingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString();
                            }
                            foreach (RadTreeNode actionNode in this.rtvCCOrganisation.Nodes)
                            {
                                actionNode.Checked = !string.IsNullOrEmpty(obj.CCId) && obj.CCId.Split(';').ToList().Contains(actionNode.Value);
                            }
                        }
                    }
                   
                this.RegenerateNo(projectObj);}
                this.grdDocument.Visible = this.Request.QueryString["fromToDoList"] == "true";
                VisibaleProperti(this.Request.QueryString["fromToDoList"] == "true");
            }
        }

        private void VisibaleProperti(bool flag)
        {
           // this.txtNumber.ReadOnly = flag;
            this.txtSubject.ReadOnly = flag;
            this.txtRelatedCSNo.ReadOnly = flag;
          //  this.txtProjectCode.ReadOnly = flag;
           this.txtSequentialNumber.ReadOnly = flag;
        //    this.txtYear.ReadOnly = flag;
            this.txtDescription.ReadOnly = flag;
           // this.txtTreatment.ReadOnly = flag;

            this.ddlDiscipline.Enabled = !flag;
            this.ddlNeedReply.Enabled = !flag;
            this.ddlOriginatingOrganization.Enabled = !flag;
            this.ddlReceivingOrganization.Enabled = !flag;
            this.rtvCCOrganisation.Enabled = !flag;
            this.btnSave.Visible = !flag;
        }

        private void RegenerateNo(ProjectCode projectObj)
        {
            var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
            var sequence = Utilities.Utility.ReturnSequenceString(this.ncrSiService.GetCSSequence(year), 4);
            this.txtNumber.Text = projectObj.Code + "-CS-" + this.ddlOriginatingOrganization.SelectedItem.Text.Split(',')[0] + "-" + this.ddlReceivingOrganization.SelectedItem.Text.Split(',')[0] + "-" + year + "-" + sequence;
            this.txtYear.Text = year.ToString();
            this.txtSequentialNumber.Text = sequence;
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
                var projectObj = this.projectcodeService.GetById(projectId);
                NCR_SI changeRequestObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                    changeRequestObj = this.ncrSiService.GetById(changeRequestId);
                    if (changeRequestObj != null)
                    {
                        this.CollectData(ref changeRequestObj, projectObj);

                        changeRequestObj.LastUpdatedBy = UserSession.Current.User.Id;
                        changeRequestObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        changeRequestObj.LastUpdatedDate = DateTime.Now;
                        this.ncrSiService.Update(changeRequestObj);
                        if (changeRequestObj.IsCancel.GetValueOrDefault())
                        {
                            //delete assing user
                            //var listassign = this.objAssignedWfService.GetAllByObj(changeRequestId);
                            //foreach (var item in listassign)
                            //{
                            //    this.objAssignedWfService.Delete(item);
                            //}
                            var listUserAssign = this.objAssignedUserService.GetAllListObjID(changeRequestId);
                            foreach (var item in listUserAssign.Where(t=> !t.IsComplete.GetValueOrDefault()))
                            {
                                item.IsComplete = true;
                                item.IsLeaf = false;
                                item.CommentContent = "Auto Complete by System DMDC! (This CS is Cancel)";
                                this.objAssignedUserService.Update(item);
                            }
                            changeRequestObj.IsInWFProcess = false;
                            changeRequestObj.IsWFComplete = true;
                            this.ncrSiService.Update(changeRequestObj);
                        }
                    }
                }
                else
                {
                    changeRequestObj = new NCR_SI()
                    {
                        ID = new Guid(Session["NCRSIID"].ToString()),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        IssuedDate=DateTime.Now,
                        IsDelete = false,
                        IsCompleteFinal = false,
                        IsInWFProcess = false,
                        IsWFComplete = false,
                        Type = Convert.ToInt32(this.Request.QueryString["type"]),
                    };

                    this.CollectData(ref changeRequestObj, projectObj);
                    if (!this.changeRequestService.IsExist(changeRequestObj.Number))
                    {
                        this.ncrSiService.Insert(changeRequestObj);
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "NCR/SI. is already exist. ";
                        return;
                    }
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
            if (e.Argument.Contains("CheckFileName"))
            {
                //var fileName = e.Argument.Split('$')[1];
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
            }
        }

       /// <summary>
       /// add
       /// </summary>
       /// <param name="projectObj"></param>
        private void LoadComboData(ProjectCode projectObj)
        {
            this.txtProjectCode.Text = projectObj.FullName;

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

            this.ddlOriginatingOrganization.SelectedValue = UserSession.Current.User.Role.ContractorId.ToString();

            this.ddlReceivingOrganization.DataSource = organizationCodeList;
            this.ddlReceivingOrganization.DataTextField = "FullName";
            this.ddlReceivingOrganization.DataValueField = "ID";
            this.ddlReceivingOrganization.DataBind();

            this.rtvCCOrganisation.DataSource = organizationCodeList;
            this.rtvCCOrganisation.DataTextField = "FullName";
            this.rtvCCOrganisation.DataValueField = "Id";
            this.rtvCCOrganisation.DataBind();
        }

        private void CollectData(ref NCR_SI obj, ProjectCode projectObj)
        {
            var listdis = string.Empty;
            listdis = this.ddlDiscipline.CheckedItems.Aggregate(listdis, (current, t) => current + t.Value + ",");
            var listdisName = string.Empty;
            listdisName = this.ddlDiscipline.CheckedItems.Aggregate(listdisName, (current, t) => current + t.Text + ", ");
            if (listdisName.Length > 0)
            {
                listdisName = listdisName.Substring(0, listdisName.Length - 2);
            }
            obj.Number = this.txtNumber.Text.Trim();
            obj.Subject = this.txtSubject.Text.Trim();
            obj.RelatedCSNo = this.txtRelatedCSNo.Text.Trim();
            obj.IsCancel = this.cbIsCancel.Checked;
            obj.Type = 3;
            obj.TypeName = "CS";
            obj.Treatment = this.txtAttachment.Text;
            obj.DisciplineID = listdis;
            obj.DisCiplineName = listdisName;
            obj.Sequence = Convert.ToInt32(this.txtSequentialNumber.Text);
            obj.SequentialNumber = this.txtSequentialNumber.Text;

            obj.Description = this.txtDescription.Text.Trim();
            obj.Year = Convert.ToInt32(this.txtYear.Text);
            obj.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            obj.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
            obj.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            obj.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
            obj.CCId = string.Empty;
            obj.CCName = string.Empty;
            foreach (RadTreeNode actionNode in this.rtvCCOrganisation.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.CCId += actionNode.Value + ";";
                obj.CCName += actionNode.Text + Environment.NewLine;
            }

            obj.NeedReply = this.ddlNeedReply.SelectedValue;
           // obj.Treatment = this.txtTreatment.Text;

            obj.ConfidentialityId = 0;
            obj.ConfidentialityName = string.Empty;
            obj.ProjectId = projectObj.ID;
            obj.ProjectName = projectObj.Code;
        }

        private void LoadDocInfo(NCR_SI obj, ProjectCode projectObj)
        {
            this.txtNumber.Text = obj.Number;
            this.txtSubject.Text = obj.Subject;
            this.txtRelatedCSNo.Text = obj.RelatedCSNo;
            this.txtProjectCode.Text = projectObj.FullName;
            var dislist = obj.DisciplineID?.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
            if (dislist != null && dislist.Count > 0)
            {
                foreach (RadComboBoxItem item in this.ddlDiscipline.Items)
                {
                    if (dislist.Contains(item.Value))
                    {
                        item.Checked = true;
                    }
                }
            }
            this.txtSequentialNumber.Text = obj.SequentialNumber;
            this.txtYear.Text = obj.Year.ToString();
            this.cbIsCancel.Checked = obj.IsCancel.GetValueOrDefault();
            this.txtDescription.Text = obj.Description;
            //this.txtTreatment.Text = obj.Treatment;
            this.txtAttachment.Text = obj.Treatment;
            this.ddlNeedReply.SelectedValue = obj.NeedReply;
            if (obj.OriginatingOrganizationId.ToString() != "")
            {
                this.ddlOriginatingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString();
            }

            if (obj.ReceivingOrganizationId.ToString() != "")
            {
                this.ddlReceivingOrganization.SelectedValue = obj.ReceivingOrganizationId.ToString();
            }
            foreach (RadTreeNode actionNode in this.rtvCCOrganisation.Nodes)
            {
                actionNode.Checked = !string.IsNullOrEmpty(obj.CCId) && obj.CCId.Split(';').ToList().Contains(actionNode.Value);
            }
        }

        protected void txtSequentialNumber_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                var project = this.txtNumber.Text.Split('-')[0];
                var sequence = Convert.ToInt32(this.txtSequentialNumber.Text);
                var obj = this.ncrSiService.GetBySequene(sequence);
                if(obj!= null && !obj.IsCancel.GetValueOrDefault())
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Message", "alert('Sequence number already exist.')", true);
                    this.txtSequentialNumber.Text = this.txtNumber.Text.Split('-')[5];
                    return;
                }
           
            this.txtNumber.Text = project + "-CS-" + this.ddlOriginatingOrganization.SelectedItem.Text.Split(',')[0] + "-" + this.ddlReceivingOrganization.SelectedItem.Text.Split(',')[0] + "-" + year + "-" + txtSequentialNumber.Text;
            }
            catch { }
          
        }

        protected void Organization_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            this.RegenerateNo(projectObj);
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var attachList = this.ncrSiAttachFileService.GetByNCRSI(objId).OrderBy(t => t.CreatedDate);
                this.grdDocument.DataSource = attachList.Where(t => !t.IsOnlyMarkupPage.GetValueOrDefault());
            }
            else
            {
                this.grdDocument.DataSource = new List<NCR_SIAttachFile>();
            }
        }

        protected string TrimDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
            {
                return string.Concat(description.Substring(0, 200), "...");
            }
            return description;
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        protected void rgdTreatment_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadAsyncUpload upload = ((GridEditableItem)e.Item)["Upload"].FindControl("AsyncUpload1") as RadAsyncUpload;
                TableCell cell = (TableCell)upload.Parent;

                CustomValidator validator = new CustomValidator();
                validator.ErrorMessage = "Please select file to be uploaded";
                validator.ClientValidationFunction = "validateRadUpload";
                validator.Display = ValidatorDisplay.Dynamic;
                cell.Controls.Add(validator);
            }
        }

        protected void rgdTreatment_InsertCommand(object sender, GridCommandEventArgs e)
        {
            Guid objId;
            Guid.TryParse(Session["NCRSIID"].ToString(), out objId);
            GridEditFormInsertItem insertItem = e.Item as GridEditFormInsertItem;
            //string imageName = (insertItem["ImageName"].FindControl("txbName") as RadTextBox).Text;
            string description = (insertItem["Description"].FindControl("txbDescription") as RadTextBox).Text;
            RadAsyncUpload radAsyncUpload = insertItem["Upload"].FindControl("AsyncUpload1") as RadAsyncUpload;
            var targetFolder = "../../DocumentLibrary/NCRSI/Picture";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                + "/DocumentLibrary/NCRSI/Picture";

            UploadedFile file = radAsyncUpload.UploadedFiles[0];

            var filename = this.txtNumber.Text + "_" + (this.ncrsiAddPictureService.GetByNCRSI(objId,3).Count + 1) + file.GetExtension();
            var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

            // Path file to download from server
            var serverFilePath = serverFolder + "/" + filename;
            System.Drawing.Image imgInput = System.Drawing.Image.FromStream(file.InputStream);
            Size size = new Size(450, 300);
            System.Drawing.Image NewImage = resizeImage(imgInput, size);
            NewImage.Save(saveFilePath);
            //create new entity
            var image = new NCR_SIAddPicture();
            image.ID = Guid.NewGuid();
            image.NCR_SIId = objId;
            image.Description = description;
            image.FilePath = serverFilePath;
            image.TypeId = 3;
            image.CreatedDate = DateTime.Now;
            this.ncrsiAddPictureService.Insert(image);
        }

        protected void rgdTreatment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var pictureObj = new List<NCR_SIAddPicture>();
            if (!string.IsNullOrEmpty(Session["NCRSIID"].ToString()))
            {
                Guid objId;
                Guid.TryParse(Session["NCRSIID"].ToString(), out objId);
                pictureObj = this.ncrsiAddPictureService.GetByNCRSI(objId,3).OrderBy(t => t.CreatedDate).ToList();
            }

            rgdTreatment.DataSource = pictureObj;
        }

        protected void rgdTreatment_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            var ID = new Guid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["ID"].ToString());
            this.ncrsiAddPictureService.Delete(ID);
        }

        protected void rgdTreatment_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            var ID = new Guid(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString());
            //string imageName = (editedItem["ImageName"].FindControl("txbName") as RadTextBox).Text;
            var description = (editedItem["Description"].FindControl("txbDescription") as RadTextBox).Text;
            RadAsyncUpload radAsyncUpload = editedItem["Upload"].FindControl("AsyncUpload1") as RadAsyncUpload;

            //retrive entity form the Db
            var image = this.ncrsiAddPictureService.GetById(ID);
            image.Description = description;

            if (radAsyncUpload.UploadedFiles.Count > 0)
            {
                var targetFolder = "../../DocumentLibrary/NCRSI/Picture";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/NCRSI/Picture";

                UploadedFile file = radAsyncUpload.UploadedFiles[0];
                FileInfo OldFile = new FileInfo(Server.MapPath(image.FilePath));

                var filename = OldFile.Name;
                OldFile.Delete();
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + filename;

                System.Drawing.Image imgInput = System.Drawing.Image.FromStream(file.InputStream);
                Size size = new Size(450, 300);
                System.Drawing.Image NewImage = resizeImage(imgInput, size);
                NewImage.Save(saveFilePath);

                image.FilePath = serverFilePath;
            }

            this.ncrsiAddPictureService.Update(image);
        }

        protected void rgdTreatment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetEditMode", "isEditMode = true;", true);
            }
        }
    }
}