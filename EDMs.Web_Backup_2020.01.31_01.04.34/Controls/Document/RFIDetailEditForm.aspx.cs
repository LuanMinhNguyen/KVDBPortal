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
    using System.Globalization;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class RFIDetailEditForm : Page
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
        public RFIDetailEditForm()
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
               
                ClearData(); LoadComboData();
                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                this.txtIssueDate.SelectedDate = date;
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
                if (Session["EditingId"] != null)
                {
                    var wfStepId = new Guid(Session["EditingId"].ToString());
                   
                    RFIObj = this.rfidetailService.GetById(wfStepId);
                    if (RFIObj != null)
                    {
                        
                        this.CollectData(ref RFIObj);

                        RFIObj.LastUpdatedBy = UserSession.Current.User.Id;
                        RFIObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        RFIObj.LastUpdatedDate = DateTime.Now;
                        this.rfidetailService.Update(RFIObj);
                    }
                    Session.Remove("EditingId");
                }
                else
                {
                    RFIObj = new  RFIDetail()
                    {
                        ID = Guid.NewGuid(),
                        GroupId = Convert.ToInt32(this.ddlGroup.SelectedValue),
                        GroupName = this.ddlGroup.SelectedItem.Text.Split(',')[0],
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                    };

                    this.CollectData(ref RFIObj);
                  
                        this.rfidetailService.Insert(RFIObj);
                  
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
            }
            this.ClearData();
            this.grdDocument.Rebind();
        }
        private void ClearData()
        {
            this.txtContractorcontact.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            //this.txtIssueDate.Clear();
            this.txtLocation.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            this.txtWorkTitler.Text = string.Empty;
            this.txtWorkTitler.Focus();
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
        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {  if (e.CommandName == "EditCmd")
            {
            var item = (GridDataItem)e.Item;
          
                var wfStepId = new Guid(item.GetDataKeyValue("ID").ToString());
                var wfStepObj = this.rfidetailService.GetById(wfStepId);
                if (wfStepObj != null)
                {
                    Session.Add("EditingId", wfStepObj.ID);
                    LoadDocInfo(wfStepObj);
                }
            }
        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }
        protected void LoadDocuments(bool isbind = false, bool isListAll = false)
        {
            var changeRequestId = new Guid(this.Request.QueryString["rfiId"]);
            var RFIlist = this.rfidetailService.GetByRFI(changeRequestId);
            foreach (var changeRequestAttachFile in RFIlist)
            {
                changeRequestAttachFile.IsCanDelete = UserSession.Current.User.Id == changeRequestAttachFile.CreatedBy;
            }
            this.grdDocument.DataSource = RFIlist.OrderBy(t => t.Number);
        }
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            this.LoadDocuments(false, isListAll);
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
           if (e.Argument.Contains("FileDelete"))
            {
                Guid docId = new Guid(e.Argument.Split('_')[1]);
                var detailobj = this.rfidetailService.GetById(docId);
                if (detailobj != null)
                {
                    this.rfidetailService.Delete(detailobj);

                }
                this.grdDocument.Rebind();
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
           
            //this.txtIssueDate.SelectedDate = DateTime.Now;
            var rfiId = new Guid(this.Request.QueryString["rfiId"]);
            var rfiobj = this.rFIService.GetById(rfiId);
          
            this.txtNumber.Text=  rfiobj.Number;

            var groupList = this.groupCodeService.GetAll();
            this.ddlGroup.DataSource = groupList.OrderBy(t => t.Code);
            this.ddlGroup.DataTextField = "FullName";
            this.ddlGroup.DataValueField = "ID";
            this.ddlGroup.DataBind();
            this.ddlGroup.SelectedValue = rfiobj.GroupId.GetValueOrDefault().ToString();
            

            var listuser = this.userService.GetAll().Where(t => !t.Role.IsAdmin.GetValueOrDefault() && t.Role.TypeId == 1).ToList();
            listuser.Insert(0, new User() { Id = 0, FullName = string.Empty });
            this.ddlengineer.DataSource = listuser.OrderBy(t => t.FullName);
            this.ddlengineer.DataValueField = "Id";
            this.ddlengineer.DataTextField = "FullName";
            this.ddlengineer.DataBind();
        }
        private void CollectData(ref RFIDetail obj)
        {

            var rfiId = new Guid(this.Request.QueryString["rfiId"]);
            var rfiobj = this.rFIService.GetById(rfiId);
            obj.Number = obj.Number?? this.rfidetailService.GetByRFI(rfiId).Count + 1;
            obj.RFIID = rfiobj.ID;
            obj.RFINo = rfiobj.Number;

            obj.GroupId = this.ddlGroup.SelectedItem != null ?
                                        Convert.ToInt32(this.ddlGroup.SelectedValue)
                                        : 0;
            obj.GroupName = this.ddlGroup.SelectedItem != null ?
                                        this.ddlGroup.SelectedItem.Text
                                        : string.Empty;
            obj.WorkTitle = this.txtWorkTitler.Text;
            obj.Description = this.txtDescription.Text;
            obj.Location = this.txtLocation.Text;
            
            obj.Time = this.txtIssueDate.SelectedDate;
            obj.InspectionTypeName = this.ddltype.SelectedValue;
            obj.Remark = this.txtRemark.Text;
            obj.ContractorContact = this.txtContractorcontact.Text;
            obj.EngineeringActionID = this.ddlengineer.SelectedValue != null ? Convert.ToInt32(this.ddlengineer.SelectedValue) : 0;
            obj.EngineeringActionName = this.ddlengineer.SelectedItem != null ? this.ddlengineer.SelectedItem.Text : string.Empty;

        }

        private void LoadDocInfo(RFIDetail obj)
        {
            this.txtNumber.Text = obj.RFINo;
           
            this.ddlGroup.SelectedValue = obj.GroupId.ToString();
         
            this.txtIssueDate.SelectedDate = obj.Time;
            this.txtContractorcontact.Text = obj.ContractorContact;
            this.txtDescription.Text = obj.Description;
            this.txtLocation.Text = obj.Location;
            this.txtRemark.Text = obj.Remark;
            this.ddltype.SelectedValue = obj.InspectionTypeName;
            this.txtWorkTitler.Text = obj.WorkTitle;
            this.ddlengineer.SelectedValue = obj.EngineeringActionID.GetValueOrDefault().ToString();
        }
    }
}