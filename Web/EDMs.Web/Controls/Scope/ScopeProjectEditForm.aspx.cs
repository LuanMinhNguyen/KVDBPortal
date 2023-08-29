// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Web.Hosting;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.WMS;

namespace EDMs.Web.Controls.Scope
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ScopeProjectEditForm : Page
    {
        private readonly ScopeProjectService scopeProjectService;
        
        private readonly UserService userService;

        private readonly NumberManagementService numberManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeProjectEditForm"/> class.
        /// </summary>
        public ScopeProjectEditForm()
        {
            this.userService = new UserService();
            this.scopeProjectService = new ScopeProjectService();
            this.numberManagementService = new NumberManagementService();
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
                this.LoadComboData();
                if (!string.IsNullOrEmpty(this.Request.QueryString["disId"]))
                {
                    var obj = this.scopeProjectService.GetById(Convert.ToInt32(this.Request.QueryString["disId"]));
                    this.CreatedInfo.Visible = obj != null;
                    this.LoadComboData();
                    if (obj != null)
                    {
                        ////this.cbAutoCalculate.Checked = objScopeProject.IsAutoCalculate.GetValueOrDefault();
                        this.txtName.Text = obj.Name;
                        this.txtDescription.Text = obj.Description;
                        this.txtCode.Text = obj.Code;
                        this.txtEmail.Text = obj.EmailAddress;
                        this.txtEmailName.Text = obj.EmailName;
                        this.txtPassword.Text = obj.EmailPwd;
                        this.txtMailServer.Text = obj.MailServer;
                        this.txtPort.Text = obj.Port;
                        this.cbUseDefaultCredentials.Checked = obj.UseDefaultCredentials.GetValueOrDefault();
                        this.cbEnableSsl.Checked = obj.EnableSsl.GetValueOrDefault();
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

                    this.CopyFromProjectControl.Visible = false;
                    this.CopyFromProjectControl1.Visible = false;
                }
                else
                {
                    this.CopyFromProjectControl.Visible = false;
                    this.CopyFromProjectControl1.Visible = false;
                }
            }
        }

        private void LoadComboData()
        {
            //var projectList = this.scopeProjectService.GetAll();
            //projectList.Insert(0, new ScopeProject() {ID = 0});
            //this.ddlProject.DataTextField = "FullName";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataSource = projectList;
            //this.ddlProject.DataBind();

            var userList = this.userService.GetAll().Where(t => t.Id != 1);
            this.ddlDC.DataTextField = "FullNameWithPosition";
            this.ddlDC.DataValueField = "Id";
            this.ddlDC.DataSource = userList;
            this.ddlDC.DataBind();
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
                    var objId = Convert.ToInt32(this.Request.QueryString["disId"]);
                    var obj = this.scopeProjectService.GetById(objId);
                    if (obj != null)
                    {
                        //obj.Name = this.txtName.Text.Trim();
                        obj.Code = this.txtCode.Text.Trim();
                        obj.Description = this.txtDescription.Text.Trim();
                        
                        obj.UpdatedBy = UserSession.Current.User.Id;
                        obj.UpdatedDate = DateTime.Now;

                        //obj.EmailAddress = this.txtEmail.Text;
                        //obj.EmailName = this.txtEmailName.Text;
                        //obj.EmailPwd = this.txtPassword.Text;
                        //obj.MailServer = this.txtMailServer.Text;
                        //obj.Port  = this.txtPort.Text;
                        //obj.UseDefaultCredentials = this.cbUseDefaultCredentials.Checked;
                        //obj.EnableSsl = this.cbEnableSsl.Checked;

                        this.scopeProjectService.Update(obj);
                    }
                }
                else
                {
                    var obj = new ScopeProject()
                    {
                        //Name = this.txtName.Text.Trim(),
                        Description = this.txtDescription.Text.Trim(),
                        Code = this.txtCode.Text.Trim(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,

                        //EmailAddress = this.txtEmail.Text,
                        //EmailName = this.txtEmailName.Text,
                        //EmailPwd = this.txtPassword.Text,
                        //MailServer = this.txtMailServer.Text,
                        //Port = this.txtPort.Text,
                        //UseDefaultCredentials = this.cbUseDefaultCredentials.Checked,
                        //EnableSsl = this.cbEnableSsl.Checked,
                };

                    var projectId = this.scopeProjectService.Insert(obj);

                    if (projectId != null)
                    {
                        this.CreateDefaultNumberManagement(obj);
                    }
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        private void CreateDefaultNumberManagement(ScopeProject projectObj)
        {
            var numberManagementObj = new NumberManagement()
            {
                ObjectName = "MaterialRequisition",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "WorkRequest",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingBreakdownReport",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingECR",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingMOC",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingMorningCall",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingProcedure",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingPunch",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingSail",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingShutdownReport",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingWCR",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);

            numberManagementObj = new NumberManagement()
            {
                ObjectName = "TrackingGeneralWorking",
                NextCount = 1,
                ProjectId = projectObj.ID,
                ProjectName = projectObj.Name
            };

            this.numberManagementService.Insert(numberManagementObj);
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
                this.fileNameValidator.ErrorMessage = "Please enter Project Code.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }
    }
}