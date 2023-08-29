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
    using System.Linq;
    using System.Web.UI;
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
    public partial class RFIEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly ProjectCodeService projectcodeService;

        private readonly GroupCodeService groupCodeService;

        private readonly RFIService rFIService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public RFIEditForm()
        {
            this.groupCodeService = new GroupCodeService();
            this.userService = new UserService();
            this.projectcodeService = new ProjectCodeService();
            this.rFIService = new RFIService();
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
                    var docObj = this.rFIService.GetById(new Guid(this.Request.QueryString["objId"]));
                    if (docObj != null)
                    {
                        this.LoadDocInfo(docObj, projectObj);
                        var createdUser = this.userService.GetByID(docObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + docObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (docObj.LastUpdatedBy != null && docObj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(docObj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + docObj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                    var sequence = Utilities.Utility.ReturnSequenceString(this.rFIService.GetCurrentSequence(year,Convert.ToInt32(ddlGroup.SelectedValue)), 4);
                    this.RegenerateChangeRequestNo(projectObj, sequence, year);
                    this.CreatedInfo.Visible = false;
                }
            }
        }

        private void RegenerateChangeRequestNo(ProjectCode projectObj, string sequence, int year)
        {

            this.txtSequentialNumber.Text = sequence;
            this.txtYear.Value = year;

            this.txtNumber.Text = projectObj.Code + "-RFI-";

            this.txtNumber.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-"+ this.txtYear.Value + "-" ;

            this.txtNumber.Text += this.txtSequentialNumber.Text;
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
                RFI RFIObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                    RFIObj = this.rFIService.GetById(changeRequestId);
                    if (RFIObj != null)
                    {
                        
                        this.CollectData(ref RFIObj, projectObj);

                        RFIObj.LastUpdatedBy = UserSession.Current.User.Id;
                        RFIObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        RFIObj.LastUpdatedDate = DateTime.Now;
                        this.rFIService.Update(RFIObj);
                    }
                }
                else
                {
                    RFIObj = new RFI()
                    {
                        ID = Guid.NewGuid(),
                        GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue),
                        GroupName = this.ddlGroup.SelectedItem.Text.Split(',')[0],
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        IsDelete = false,
                        IsCompleteFinal = false,
                        IsInWFProcess = false,
                        IsWFComplete = false,
                    };

                    this.CollectData(ref RFIObj, projectObj);
                   // if (!this.rFIService.IsExist(RFIObj.Number))
                   // {
                        this.rFIService.Insert(RFIObj);
                  //  }
                  //  else
                  //  {
                  //      this.blockError.Visible = true;
                   //     this.lblError.Text = "RFI. is already exist. ";
                   //     return;
                   // }
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
        /// Load all combo data
        /// </summary>
        private void LoadComboData(ProjectCode projectObj)
        {
            this.txtProjectCode.Text = projectObj.Code;
            this.txtIssueDate.SelectedDate = DateTime.Now;
            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();

         
        }

        private void CollectData(ref RFI obj, ProjectCode projectObj)
        {
            obj.Number = this.txtNumber.Text.Trim();
          

            obj.GroupId = this.ddlGroup.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlGroup.SelectedValue)
                                        : 0;
            obj.GroupName = this.ddlGroup.SelectedItem != null ?
                                        this.ddlGroup.SelectedItem.Text
                                        : string.Empty;
            obj.ProjectId = projectObj.ID;
            obj.ProjectCode = projectObj.Code;

            obj.Year = (int?)this.txtYear.Value;
            obj.IssuedDate = this.txtIssueDate.SelectedDate;
            obj.SequentialNumber = this.txtNumber.Text.Split('-')[4];
            obj.Sequence = Convert.ToInt32(this.txtNumber.Text.Split('-')[4]);
            obj.SiteManager = this.txtSiteManager.Text;
            obj.QAQCManager = this.txtQAQCManager.Text;
        }

        private void LoadDocInfo(RFI obj, ProjectCode projectObj)
        {
            this.txtNumber.Text = obj.Number;
            this.txtYear.Value = obj.Year;

            this.txtProjectCode.Text = projectObj.FullName;
            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
         
            this.txtIssueDate.SelectedDate = obj.IssuedDate;
            this.txtQAQCManager.Text = obj.QAQCManager;
            this.txtSiteManager.Text = obj.SiteManager;
            this.txtSequentialNumber.Text = obj.SequentialNumber;
        
            
        }

        protected void ddlGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projectId"]);
            var projectObj = this.projectcodeService.GetById(projectId);
            var year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
            var sequence = Utilities.Utility.ReturnSequenceString(this.rFIService.GetCurrentSequence(year, Convert.ToInt32(ddlGroup.SelectedValue)), 4);
            this.RegenerateChangeRequestNo(projectObj, sequence, year);


            //this.txtNumber.Text = this.txtProjectCode.Text.Split(',')[0] + "-RFI-";

            //this.txtNumber.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-" + this.txtYear.Value + "-";

            //this.txtNumber.Text += this.txtSequentialNumber.Text;
        }

        protected void txtSequentialNumber_TextChanged(object sender, EventArgs e)
        {

            this.txtNumber.Text = this.txtProjectCode.Text.Split(',')[0] + "-RFI-";

            this.txtNumber.Text += this.ddlGroup.SelectedItem.Text.Split(',')[0] + "-" + this.txtYear.Value + "-";

            this.txtNumber.Text += this.txtSequentialNumber.Text;
        }
    }
}