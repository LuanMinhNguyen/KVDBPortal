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
using System.Web.UI.WebControls;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ProjectEditForm : Page
    {
        private readonly CostContractProjectService projectService;
        
        private readonly UserService userService;

        private readonly ProcessActualService processActualService;

        private readonly DisciplineService disciplineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ProjectEditForm()
        {
            this.userService = new UserService();
            this.projectService = new CostContractProjectService();
            this.processActualService = new ProcessActualService();
            this.disciplineService = new DisciplineService();
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
                    var objScopeProject = this.projectService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = objScopeProject != null;

                    if (objScopeProject != null)
                    {
                        this.cbAutoCalculate.Checked = objScopeProject.IsAutoCalculate.GetValueOrDefault();
                        this.txtName.Text = objScopeProject.Name;
                        this.txtDescription.Text = objScopeProject.Description;
                        this.txtStartDate.SelectedDate = objScopeProject.StartDate;
                        this.txtEndDate.SelectedDate = objScopeProject.EndDate;
                        this.txtFrequency.Value = objScopeProject.FrequencyForProgressChart.GetValueOrDefault();
                        var createdUser = this.userService.GetByID(objScopeProject.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objScopeProject.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objScopeProject.UpdatedBy != null && objScopeProject.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objScopeProject.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objScopeProject.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
                    var ScopeProjectId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var objProject = this.projectService.GetById(ScopeProjectId);
                    if (objProject != null)
                    {
                        objProject.Name = this.txtName.Text.Trim();
                        objProject.Description = this.txtDescription.Text.Trim();
                        objProject.StartDate = this.txtStartDate.SelectedDate;
                        objProject.EndDate = this.txtEndDate.SelectedDate;
                        objProject.FrequencyForProgressChart = (int?) this.txtFrequency.Value;
                        objProject.IsAutoCalculate = this.cbAutoCalculate.Checked;

                        objProject.UpdatedBy = UserSession.Current.User.Id;
                        objProject.UpdatedDate = DateTime.Now;
                        this.projectService.Update(objProject);

                        // Update template actual progress
                        var listDiscipline = this.disciplineService.GetAllDisciplineOfProject(objProject.ID).OrderBy(t => t.ID).ToList();
                        var count = 0;
                        var startDate = objProject.StartDate.GetValueOrDefault();
                        for (var j = startDate;
                            j <= objProject.EndDate.GetValueOrDefault();
                            j = j.AddDays(objProject.FrequencyForProgressChart != null && objProject.FrequencyForProgressChart != 0 ? objProject.FrequencyForProgressChart.Value : 7))
                        {
                            count += 1;
                        }

                        foreach (var discipline in listDiscipline)
                        {
                            var existProgressActual = this.processActualService.GetByProjectAndWorkgroup(objProject.ID, discipline.ID);
                            if (existProgressActual != null)
                            {
                                var currentCount = existProgressActual.Actual.Split('$').Count();

                                if (currentCount < count)
                                {
                                    for (int i = existProgressActual.Actual.Split('$').Count(); i < count; i++)
                                    {
                                        existProgressActual.Actual += existProgressActual.Actual.Split('$')[currentCount - 1] + "$";
                                    }

                                    existProgressActual.Actual = existProgressActual.Actual.Substring(0, existProgressActual.Actual.Length - 1);
                                    this.processActualService.Update(existProgressActual);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var obj = new CostContractProject()
                    {
                        Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        StartDate = this.txtStartDate.SelectedDate,
                        EndDate = this.txtEndDate.SelectedDate,
                        FrequencyForProgressChart = (int?) this.txtFrequency.Value,
                        IsAutoCalculate = this.cbAutoCalculate.Checked,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                    this.projectService.Insert(obj);
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
            if(this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Discipline name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }
    }
}