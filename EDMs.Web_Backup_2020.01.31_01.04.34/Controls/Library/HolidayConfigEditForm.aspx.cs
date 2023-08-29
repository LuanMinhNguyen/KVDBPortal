// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Library
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class HolidayConfigEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly HolidayConfigService holidayConfigService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public HolidayConfigEditForm()
        {
            this.userService = new UserService();
            this.holidayConfigService = new HolidayConfigService();
            this.waitingSyncDataService = new WaitingSyncDataService();
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
                //this.LoadComboData();
                
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var obj = this.holidayConfigService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = obj != null;

                    if (obj != null)
                    {
                        this.txtFromDate.SelectedDate = obj.FromDate;
                        this.txtToDate.SelectedDate = obj.ToDate;
                        this.txtDescription.Text = obj.Description;
                        var createdUser = this.userService.GetByID(obj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + obj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (obj.UpdatedBy != null && obj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(obj.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + obj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var documentHolidayConfigId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.holidayConfigService.GetById(documentHolidayConfigId);
                    if (obj != null)
                    {
                        obj.Description = this.txtDescription.Text.Trim();
                        obj.FromDate = this.txtFromDate.SelectedDate;
                        obj.ToDate = this.txtToDate.SelectedDate;
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;
                        obj.UpdatedByName = UserSession.Current.User.FullName;
                        this.holidayConfigService.Update(obj);
                    }
                }
                else
                {
                    var obj = new HolidayConfig()
                    {
                        Description = this.txtDescription.Text.Trim(),
                        FromDate = this.txtFromDate.SelectedDate,
                        ToDate = this.txtToDate.SelectedDate,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now
                    };

                    this.holidayConfigService.Insert(obj);
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
            if(this.txtDescription.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Holiday description.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        //private void LoadComboData()
        //{
        //    var projectList = this.projectService.GetAll();
        //    this.ddlProject.DataSource = projectList;
        //    this.ddlProject.DataTextField = "FullName";
        //    this.ddlProject.DataValueField = "ID";
        //    this.ddlProject.DataBind();
        //}
    }
}