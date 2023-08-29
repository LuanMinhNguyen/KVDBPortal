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
    public partial class RFIDetailRemarkEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly ProjectCodeService projectcodeService;

        private readonly GroupCodeService groupCodeService;

        private readonly RFIService rFIService;

        private readonly RFIDetailService rfidetailService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public RFIDetailRemarkEditForm()
        {
            this.groupCodeService = new GroupCodeService();
            this.userService = new UserService();
            this.projectcodeService = new ProjectCodeService();
            this.rFIService = new RFIService();
            this.rfidetailService = new RFIDetailService();
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
               
                 LoadComboData();
               
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
            if (!string.IsNullOrEmpty(Request.QueryString["rfiId"]))
            {

                RFIDetail RFIObj;
                
                    var wfStepId = new Guid(Request.QueryString["rfiId"]);
                   
                    RFIObj = this.rfidetailService.GetById(wfStepId);
                    if (RFIObj != null)
                    {
                        
                        this.CollectData(ref RFIObj);

                        RFIObj.LastUpdatedBy = UserSession.Current.User.Id;
                        RFIObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        RFIObj.LastUpdatedDate = DateTime.Now;
                        this.rfidetailService.Update(RFIObj);
                    }
                  
                
                
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CancelEdit();", true);
            }
      
          
        }
        private void ClearData()
        {
            this.txtContractorcontact.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtIssueDate.Clear();
            this.txtLocation.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            this.txtWorkTitler.Text = string.Empty;
            Session.Remove("EditingId");
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
            ClearData();
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
          
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
           
            this.txtIssueDate.SelectedDate = DateTime.Now;
            var rfiId = new Guid(this.Request.QueryString["rfiId"]);
            var rfiobj = this.rfidetailService.GetById(rfiId);
            LoadDocInfo(rfiobj);
         // this.txtNumber.Text=  rfiobj.Number;

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();
        }

        private void CollectData(ref RFIDetail obj)
        {

       
            obj.CommentContent = this.txtcomment.Text;
          

        }

        private void LoadDocInfo(RFIDetail obj)
        {
            this.txtNumber.Text = obj.RFINo;
           
            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
         
            this.txtIssueDate.SelectedDate = obj.Time;
            this.txtContractorcontact.Text = obj.ContractorContact;
            this.txtDescription.Text = obj.Description;
            this.txtLocation.Text = obj.Location;
            this.txtcomment.Text = obj.CommentContent;
            this.txtRemark.Text = obj.Remark;
            this.ddltype.SelectedValue = obj.InspectionTypeName;
            this.txtIssueDate.SelectedDate = obj.Time;
            this.txtWorkTitler.Text = obj.WorkTitle;
          this.txtEngineer.Text= obj.EngineeringActionName;
        }
    }
}