// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Document;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DisciplineEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        private readonly ScopeProjectService scopeProjectService;
        private readonly ProcessActualService processActualService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisciplineEditForm"/> class.
        /// </summary>
        public DisciplineEditForm()
        {
            this.userService = new UserService();
            this.disciplineService = new DisciplineService();
            this.scopeProjectService = new ScopeProjectService();
            this.processActualService = new ProcessActualService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objDis = this.disciplineService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    if (objDis != null)
                    {

                        this.txtCode.Text = objDis.Code;
                        this.txtDescription.Text = objDis.Description;
                        var createdUser = this.userService.GetByID(objDis.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objDis.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objDis.LastUpdatedBy != null && objDis.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objDis.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objDis.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var disId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var objDis = this.disciplineService.GetById(disId);
                    if (objDis != null)
                    {
                        objDis.Code = this.txtCode.Text.Trim();
                        objDis.Description = this.txtDescription.Text.Trim();
                        objDis.LastUpdatedBy = UserSession.Current.User.Id;
                        objDis.LastUpdatedDate = DateTime.Now;

                        this.disciplineService.Update(objDis);
                        
                    }
                }
                else
                {
                    var objDis = new Discipline()
                    {
                        Code = this.txtCode.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.disciplineService.Insert(objDis);
                    // Insert temp actual
                    var projectObj = this.scopeProjectService.GetById(objDis.ProjectId.GetValueOrDefault());
                    if (projectObj != null
                        && projectObj.IsAutoCalculate.GetValueOrDefault()
                        && projectObj.StartDate != null
                        && projectObj.EndDate != null)
                    {
                        var newActualProgess = string.Empty;
                        var startDate = projectObj.StartDate.GetValueOrDefault();
                        for (var j = startDate;
                            j <= projectObj.EndDate.GetValueOrDefault();
                            j = j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart != 0 ? projectObj.FrequencyForProgressChart.Value : 7))
                        {
                            newActualProgess += "0$";
                        }

                        newActualProgess = newActualProgess.Substring(0, newActualProgess.Length - 1);
                        var progressActual = new ProcessActual();
                        progressActual.ProjectId = projectObj.ID;
                        progressActual.WorkgroupId = objDis.ID;
                        progressActual.Actual = newActualProgess;

                        this.processActualService.Insert(progressActual);
                    }
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
            if(this.txtCode.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Discipline Code.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        private void LoadComboData()
        {
            //var projectInPermission = this.scopeProjectService.GetAll();
            //this.ddlProject.DataSource = projectInPermission;
            //this.ddlProject.DataTextField = "FullName";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataBind();
        }
    }
}