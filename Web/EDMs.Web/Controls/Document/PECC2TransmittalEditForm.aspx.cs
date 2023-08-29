// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web.Hosting;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using System.Linq;

    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PECC2TransmittalEditForm : Page
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly PECC2TransmittalService transmittalService;
        /// <summary>
        /// The scope project services
        /// </summary>
        private readonly ProjectCodeService projectCodeService;

        /// <summary>
        /// The confidentiality services
        /// </summary>
        private readonly ConfidentialityService confidentialityServices;

        private readonly OrganizationCodeService organizationCodeService;

        private readonly GroupCodeService groupCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PECC2TransmittalEditForm()
        {
            this.userService = new UserService();
            this.transmittalService = new PECC2TransmittalService();
            this.projectCodeService = new ProjectCodeService();
            this.confidentialityServices = new ConfidentialityService();
            this.organizationCodeService = new OrganizationCodeService();
            this.groupCodeService = new GroupCodeService();
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
                this.LoadInitData();

                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid id;
                    var obj = this.transmittalService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (obj != null)
                    {
                        this.txtTransmittalNo.Text = obj.TransmittalNo;
                        this.txtDescription.Text = obj.Description;
                        if (obj.OriginatingOrganizationId.ToString() != "")
                        { this.ddlOriginatingOrganization.SelectedValue = obj.OriginatingOrganizationId.ToString(); }
                        if (obj.ReceivingOrganizationId.ToString() != "")
                        { this.ddlReceivingOrganization.SelectedValue = obj.ReceivingOrganizationId.ToString(); }
                        if (obj.ProjectCodeId.ToString() != "")
                        { this.ddlProjectCode.SelectedValue = obj.ProjectCodeId.ToString(); }
                        if (obj.ConfidentialityId.ToString() != "")
                        { this.ddlConfidentiality.SelectedValue = obj.ConfidentialityId.ToString(); }
                        this.txtDueDate.SelectedDate = obj.DueDate;
                        this.ddlIncomingTrans.SelectedValue = obj.RefTransId.ToString();
                        this.ddlCategory.SelectedValue = obj.CategoryId.ToString();
                        this.txtVolumeNumber.Text = obj.VolumeNumber;
                        this.ddlGroup.SelectedValue = obj.GroupId.ToString();
                        this.txtOtherTransNo.Text = obj.OtherTransNo;
                        this.txtFrom.Text = obj.FromValue;
                        this.txtTo.Text = obj.ToValue;
                        this.txtCC.Text = obj.CCValue;
                        this.txtDayIssued.SelectedDate = obj.IssuedDate;
                        this.txtSubject.Text = obj.Subject;
                        this.DropDownPriority.SelectedValue = obj.Priority;
                        this.ddlTransmittedByName.SelectedValue = obj.TransmittedById.ToString();
                        this.txtTransmittedByDesignation.Text = obj.TransmittedByDesignation;
                        this.ddlAcknowledgedByName.SelectedValue = obj.AcknowledgedId.ToString();
                        this.txtAcknowledgedByDesignation.Text = obj.AcknowledgedByDesignation;
                        this.txtRemark.Text = obj.Remark;
                        this.ddlForSend.SelectedValue = obj.ForSentId.ToString();
                        foreach (RadTreeNode actionNode in this.rtvCCOrganisation.Nodes)
                        {
                            actionNode.Checked = !string.IsNullOrEmpty(obj.CCOrganizationId) && obj.CCOrganizationId.Split(';').ToList().Contains(actionNode.Value);
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
                    this.ddlOriginatingOrganization.Enabled = false;
                    this.ddlProjectCode.Enabled = false;
                    this.ddlReceivingOrganization.Enabled = false;
                    this.ddlGroup.Enabled = false;
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                    this.RegenerateTransNo();
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
                this.SetHiden();
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var objTran = this.transmittalService.GetById(objId);
                    if (objTran != null)
                    {
                        this.CollectData(objTran);

                        objTran.IssuedDate = this.txtDayIssued.SelectedDate;
                        objTran.DueDate = this.txtDueDate.SelectedDate;
                        objTran.LastUpdatedBy = UserSession.Current.User.Id;
                        objTran.LastUpdatedDate = DateTime.Now;

                        this.transmittalService.Update(objTran);
                    }
                }
                else
                {
                    var objTran = new PECC2Transmittal();
                    objTran.ID = Guid.NewGuid();

                    this.CollectData(objTran);

                    objTran.Status = "Missing Attach Document";
                    objTran.IsValid = false;
                    objTran.IsSend = false;
                    objTran.IsOpen = false;
                    objTran.ErrorMessage = "Missing Attach Document";
                    objTran.TypeId = 2;
                    objTran.IssuedDate = this.txtDayIssued.SelectedDate;
                    objTran.DueDate = this.txtDueDate.SelectedDate;
                    objTran.CreatedBy = UserSession.Current.User.Id;
                    objTran.CreatedDate = DateTime.Now;

                    // Create store folder
                    var physicalStoreFolder = Server.MapPath("../../DocumentLibrary/PECC2Transmittal/" + objTran.TransmittalNo + "_" + objTran.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
                    Directory.CreateDirectory(physicalStoreFolder);
                    Directory.CreateDirectory(physicalStoreFolder + @"\eTRM File");


                    var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                        ? string.Empty
                        : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/PECC2Transmittal/" + objTran.TransmittalNo + "_" + objTran.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
                    objTran.StoreFolderPath = serverStoreFolder;
                    // --------------------------------------------------------------------------

                    this.transmittalService.Insert(objTran);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CollectData(PECC2Transmittal obj)
        {
            obj.TransmittalNo = this.txtTransmittalNo.Text.Trim();
            obj.Description = this.txtDescription.Text.Trim();
            obj.OriginatingOrganizationId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            obj.OriginatingOrganizationName = (this.ddlOriginatingOrganization.SelectedItem.Text.Trim());
            obj.ReceivingOrganizationId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            obj.ReceivingOrganizationName = (this.ddlReceivingOrganization.SelectedItem.Text.Trim());
            obj.ProjectCodeId = Convert.ToInt32(this.ddlProjectCode.SelectedValue);
            obj.ProjectCodeName = (this.ddlProjectCode.SelectedItem.Text.Split(','))[0].Trim();
            obj.ConfidentialityId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue);
            obj.ConfidentialityName = this.ddlConfidentiality.SelectedItem.Text;
            //obj.TypeId = 2;
            obj.RefTransId = new Guid(this.ddlIncomingTrans.SelectedValue);
            obj.RefTransNo = this.ddlIncomingTrans.SelectedItem.Text;
            obj.Priority = this.DropDownPriority.SelectedValue;
            obj.GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue);
            obj.GroupCode = this.ddlGroup.SelectedItem.Text.Split(',')[0];
            obj.OtherTransNo = this.txtOtherTransNo.Text;
            obj.FromValue = this.txtFrom.Text;
            obj.ToValue = this.txtTo.Text;
            obj.CCValue = this.txtCC.Text;
            //objTran.IssuedDate = this.txtDayIssued.SelectedDate;
            obj.Subject = this.txtSubject.Text;
            obj.TransmittedById = this.ddlTransmittedByName.SelectedItem != null
                                    ? Convert.ToInt32(this.ddlTransmittedByName.SelectedValue)
                                    : 0;
            obj.TransmittedByName = this.ddlTransmittedByName.SelectedItem != null
                                    ? this.ddlTransmittedByName.SelectedItem.Text
                                    : string.Empty;
            obj.TransmittedByDesignation = this.txtTransmittedByDesignation.Text;

            obj.AcknowledgedId = this.ddlAcknowledgedByName.SelectedItem != null
                                    ? Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue)
                                    : 0;
            obj.AcknowledgedByName = this.ddlAcknowledgedByName.SelectedItem != null
                                    ? this.ddlAcknowledgedByName.SelectedItem.Text
                                    : string.Empty;
            obj.AcknowledgedByDesignation = this.txtAcknowledgedByDesignation.Text;
            obj.Remark = this.txtRemark.Text;
            obj.Year = DateTime.Now.Year;
            obj.Sequence = Convert.ToInt32(this.txtTransmittalNo.Text.Trim().Split('-')[6]);
            obj.SequenceString = this.txtTransmittalNo.Text.Trim().Split('-')[6];

            obj.ForSentId = Convert.ToInt32(this.ddlForSend.SelectedValue);
            obj.ForSentName = this.ddlForSend.SelectedItem.Text;
            obj.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            obj.CategoryName = this.ddlCategory.SelectedItem.Text;
            obj.CCOrganizationId = string.Empty;
            obj.CCOrganizationName = string.Empty;
            foreach (RadTreeNode actionNode in this.rtvCCOrganisation.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value)))
            {
                obj.CCOrganizationId += actionNode.Value + ";";
                obj.CCOrganizationName += actionNode.Text + Environment.NewLine;
            }
        }

        private void SetHiden()
        {
            if (this.Request.QueryString["Type"] == "In")
            {
                this.ddlForSend.Enabled = false;
                this.ddlProjectCode.Enabled = false;
                this.ddlOriginatingOrganization.Enabled = false;
                this.ddlReceivingOrganization.Enabled = false;
                this.rtvCCOrganisation.AllowNodeEditing = false;
                this.txtDayIssued.EnableTyping = false;
                this.txtDayIssued.DatePopupButton.Enabled = false;
                // this.ddlGroup.Enabled = false;
                this.ddlCategory.Enabled = true;
                this.txtVolumeNumber.Enabled = true;
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
            if (this.txtTransmittalNo.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter transmittal number.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            //else if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            //{
                //var objId = Convert.ToInt32(Request.QueryString["objId"]);
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                //this.fileNameValidator.ErrorMessage = "The specified number is already in use.";
                //this.divFileName.Style["margin-bottom"] = "-26px;";
                //args.IsValid = !this.documentService.IsDocumentExistUpdate(folderId, this.txtTransmittalNo.Text.Trim(), objId);
            //}
        }

        //protected void ServerValidationDescription(object source, ServerValidateEventArgs args)
        //{
        //    if (this.txtDescription.Text.Trim().Length == 0)
        //    {
        //        this.descriptionValidator.ErrorMessage = "Please enter description.";
        //        this.divDescription.Style["margin-bottom"] = "-26px;";
        //        args.IsValid = false;
        //    }
        //}

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadInitData()
        {
            this.txtDayIssued.SelectedDate = DateTime.Now;
            
            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList;
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();

            var confidentialityList = this.confidentialityServices.GetAll(UserSession.Current.User.ConfidentialId.GetValueOrDefault()).OrderBy(t => t.ID).ToList();
            this.ddlConfidentiality.DataSource = confidentialityList;
            this.ddlConfidentiality.DataTextField = "Code";
            this.ddlConfidentiality.DataValueField = "ID";
            this.ddlConfidentiality.DataBind();

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

            var projectCodeList = this.projectCodeService.GetAll().OrderBy(t=>t.Code).ToList();
            this.ddlProjectCode.DataSource = projectCodeList;
            this.ddlProjectCode.DataTextField = "FullName";
            this.ddlProjectCode.DataValueField = "ID";
            this.ddlProjectCode.DataBind();

            if (this.ddlProjectCode.SelectedItem != null)
            {
                var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
                incomingTransList.Insert(0, new PECC2Transmittal() { ID = Guid.NewGuid() });
                this.ddlIncomingTrans.DataSource = incomingTransList;
                this.ddlIncomingTrans.DataTextField = "TransmittalNo";
                this.ddlIncomingTrans.DataValueField = "ID";
                this.ddlIncomingTrans.DataBind();
            }

            var userList = this.userService.GetAll();
            var trasmittedUser = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            this.ddlTransmittedByName.DataSource = trasmittedUser;
            this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
            this.ddlTransmittedByName.DataValueField = "ID";
            this.ddlTransmittedByName.DataBind();
            this.ddlTransmittedByName.SelectedValue = UserSession.Current.User.Id.ToString();


            if (this.ddlTransmittedByName.SelectedItem != null)
            {
                var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                if (transmittedUser != null)
                {
                    this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                }
            }

            var acknowledgedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            acknowledgedUserList.Insert(0, new User() { Id = 0 });
            this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
            this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
            this.ddlAcknowledgedByName.DataValueField = "ID";
            this.ddlAcknowledgedByName.DataBind();

            if (this.ddlAcknowledgedByName.SelectedItem != null)
            {
                var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                if (acknowledgedUser != null)
                {
                    this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                }
            }

        }

        protected void Organization_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();
            var userList = this.userService.GetAll();
            var trasmittedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            
            this.ddlTransmittedByName.DataSource = trasmittedUserList;
            this.ddlTransmittedByName.DataTextField = "UserNameWithFullName";
            this.ddlTransmittedByName.DataValueField = "ID";
            this.ddlTransmittedByName.DataBind();

            if (this.ddlTransmittedByName.SelectedItem != null)
            {
                var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
                if (transmittedUser != null)
                {
                    this.txtTransmittedByDesignation.Text = transmittedUser.Position;
                }
            }

            var acknowledgedUserList = userList.Where(t => t.Role.ContractorId == Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue)).OrderBy(t => t.UserNameWithFullName).ToList();
            this.ddlAcknowledgedByName.DataSource = acknowledgedUserList;
            this.ddlAcknowledgedByName.DataTextField = "UserNameWithFullName";
            this.ddlAcknowledgedByName.DataValueField = "ID";
            this.ddlAcknowledgedByName.DataBind();

            if (this.ddlAcknowledgedByName.SelectedItem != null)
            {
                var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
                if (acknowledgedUser != null)
                {
                    this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
                }
            }
        }

        protected void ddlProjectCode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlProjectCode.SelectedItem != null)
            {
                ////var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProjectCode.SelectedValue), 1, string.Empty);
                ////incomingTransList.Insert(0, new PECC2Transmittal() {ID = Guid.NewGuid()});
                ////this.ddlIncomingTrans.DataSource = incomingTransList;
                ////this.ddlIncomingTrans.DataTextField = "TransmittalNo";
                ////this.ddlIncomingTrans.DataValueField = "ID";
                ////this.ddlIncomingTrans.DataBind();

                this.RegenerateTransNo();
            }
        }

        protected void ddlGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RegenerateTransNo();
        }

        private void RegenerateTransNo()
        {
            var fromId = Convert.ToInt32(this.ddlOriginatingOrganization.SelectedValue);
            var fromObj = this.organizationCodeService.GetById(fromId);

            var toId = Convert.ToInt32(this.ddlReceivingOrganization.SelectedValue);
            var toObj = this.organizationCodeService.GetById(toId);

            this.txtTransmittalNo.Text = this.ddlProjectCode.SelectedItem.Text.Split(',')[0] + "-T-";

            if (fromObj != null)
            {
                this.txtTransmittalNo.Text += fromObj.Code + "-";
            }

            if (toObj != null)
            {
                this.txtTransmittalNo.Text += toObj.Code + "-";
            }

            this.txtTransmittalNo.Text += DateTime.Now.Year.ToString().Substring(2, 2) + "-";
            this.txtTransmittalNo.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-";

            var sequence = Utilities.Utility.ReturnSequenceString(this.transmittalService.GetCurrentSequence(DateTime.Now.Year, Convert.ToInt32(this.ddlGroup.SelectedValue)), 4);
            this.txtTransmittalNo.Text += sequence;
        }

        protected void ddlTransmittedByName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var transmittedUser = this.userService.GetByID(Convert.ToInt32(this.ddlTransmittedByName.SelectedValue));
            if (transmittedUser != null)
            {
                this.txtTransmittedByDesignation.Text = transmittedUser.Position;
            }
        }

        protected void ddlAcknowledgedByName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var acknowledgedUser = this.userService.GetByID(Convert.ToInt32(this.ddlAcknowledgedByName.SelectedValue));
            if (acknowledgedUser != null)
            {
                this.txtAcknowledgedByDesignation.Text = acknowledgedUser.Position;
            }
        }
    }
}