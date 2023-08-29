using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.ApplicationServices;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells.Charts;
using EAM.Business.Services;
using EAM.Business.Services.Security;
using EAM.Data.Dto;
using EAM.Data.Entities;
using EAM.WebPortal.Resources.Utilities;
using EAM.WebPortal.Resources.Utilities.Session;
using Telerik.Web.UI;
using Telerik.Web.UI.Chat;

namespace EAM.WebPortal.Control.Security
{
    public partial class UserEditForm : Page
    {

        #region Fields
        private readonly AA_UsersService userService;

        private readonly AA_RolesService roleService;

        private const string ResourceParameterKey = "id";
        private const string UserParameterKey = "userid";

        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        #endregion

        #region Properties
        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        private int? ResourceId
        {
            get
            {
                if (String.IsNullOrEmpty(Request[ResourceParameterKey])) return null;

                int outValue;
                if (int.TryParse(Request[ResourceParameterKey], out outValue))
                    return outValue;
                return null;
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        private int? UserId
        {
            get
            {
                if (String.IsNullOrEmpty(Request[UserParameterKey])) return null;

                int outValue;
                if (int.TryParse(Request[UserParameterKey], out outValue))
                    return outValue;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has user account.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has user account; otherwise, <c>false</c>.
        /// </value>
        private bool HasUserAccount
        {
            get { return !string.IsNullOrEmpty(txtUsername.Text.Trim()); }
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Loads the data to combo.
        /// </summary>
        private void LoadDataToCombo()
        {
            var roles = this.roleService.GetAll(UserSession.Current.RoleId == 1).OrderBy(t => t.Name).ToList();
            this.ddlRoles.DataSource = roles;
            this.ddlRoles.DataValueField = "Id";
            this.ddlRoles.DataTextField = "Name";
            this.ddlRoles.DataBind();

            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = "ADMIN";
            var orgList = new List<OrganizationDto>();
            var ds = this.eamService.GetDataSet("getOrgbyU", new[] { userParam });
            if (ds != null)
            {
                orgList = this.eamService.CreateListFromTable<OrganizationDto>(ds.Tables[0]).OrderBy(t => t.THUTUSAPXEP).ThenBy(t => t.MADONVI).ToList();
                foreach (var item in orgList)
                {
                    item.FullName = item.MADONVI + " - " + item.TENDONVI;
                }

                this.lbOrg.DataSource = orgList.OrderBy(t => t.FullName);
                this.lbOrg.DataValueField = "MADONVI";
                this.lbOrg.DataTextField = "FullName";
                this.lbOrg.DataBind();

            }
        }

        #endregion

        #region Initializes
        
        public UserEditForm()
        {
            this.userService =new AA_UsersService();
            this.roleService = new AA_RolesService();
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsername.Focus();
                LoadDataToCombo();
                if (!string.IsNullOrEmpty(this.Request.QueryString["userId"]))
                {
                    var userId = Convert.ToInt32(this.Request.QueryString["userId"]);
                    var user = this.userService.GetByID(userId);
                    if (user != null)
                    {
                        this.txtUsername.Text = user.Username;
                        this.txtEmail.Text = user.Email;
                        this.txtCellPhone.Text = user.CellPhone;
                        this.txtHomePhone.Text = user.Phone;
                        this.txtFullName.Text = user.FullName;
                        this.txtPosition.Text = user.Position;
                        this.ddlRoles.SelectedValue = user.RoleId.GetValueOrDefault().ToString();
                        this.ddlDiscipline.SelectedValue = user.DisciplineId.ToString();
                        this.IsPD.Checked = user.IsPD.GetValueOrDefault();
                        this.cbLeader.Checked = user.IsLeader.GetValueOrDefault();
                        this.cbReceivedEmail.Checked = user.IsSendMail.GetValueOrDefault();
                        this.cbEngineer.Checked = user.IsEngineer.GetValueOrDefault();
                        this.ddlConfidentiality.SelectedValue = user.ConfidentialId.ToString();

                        this.lblUploadedSigned.Visible = !string.IsNullOrEmpty(user.SignImageUrl);
                        this.imgUserSigned.Visible = !string.IsNullOrEmpty(user.SignImageUrl);
                        this.imgUserSigned.ImageUrl = user.SignImageUrl;
                        this.cbActive.Checked = user.IsActive.GetValueOrDefault();
                        this.IsDC.Checked = user.IsDC.GetValueOrDefault();

                        if (!string.IsNullOrEmpty(user.OrgCode))
                        {
                            foreach (RadListBoxItem orgItem in this.lbOrg.Items)
                            {
                                if (user.OrgCode.Split(';').Contains(orgItem.Value))
                                {
                                    orgItem.Checked = true;
                                }
                            }
                        }

                        this.cbIsTTYT.Checked = user.IsTTYT.GetValueOrDefault();
                        this.txtSoTYT.Text = user.SoTYT;

                    }
                }
                else
                {
                    this.imgUserSigned.Visible = false;
                    this.lblUploadedSigned.Visible = false;
                    if (this.ddlRoles.SelectedItem != null)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCapNhat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var departmentObj = this.roleService.GetByID(Convert.ToInt32(this.ddlRoles.SelectedValue));
                if (departmentObj != null)
                {
                    if (!string.IsNullOrEmpty(this.Request.QueryString["userId"]))
                    {
                        var userId = Convert.ToInt32(this.Request.QueryString["userId"]);
                        var user = this.userService.GetByID(userId);
                        if (user != null)
                        {
                            user.Username = this.txtUsername.Text.Trim();
                            user.FullName = this.txtFullName.Text.Trim();
                            user.RoleId = Convert.ToInt32(this.ddlRoles.SelectedValue);
                            user.RoleName = this.ddlRoles.SelectedItem.Text;
                            user.Position = this.txtPosition.Text.Trim();
                            user.LocationId = departmentObj.TypeId;
                            user.LocationName = departmentObj.TypeName;
                            user.Email = this.txtEmail.Text.Trim();
                            user.Phone = this.txtHomePhone.Text.Trim();
                            user.CellPhone = this.txtCellPhone.Text.Trim();
                            //user.DisciplineId = Convert.ToInt32(this.ddlDiscipline.SelectedValue);
                            //user.DisciplineName = this.ddlDiscipline.SelectedItem.Text;
                            //user.IsDeptManager = this.cbManager.Checked;
                            user.IsLeader = this.cbLeader.Checked;
                            user.IsEngineer = this.cbEngineer.Checked;
                            //user.ConfidentialId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue);
                            //user.ConfidentialName = this.ddlConfidentiality.SelectedItem.Text;
                            //user.TitleName = this.ddlTitle.SelectedItem.Text;
                            //user.TitleId = Convert.ToInt32(this.ddlTitle.SelectedValue);
                            user.IsActive = this.cbActive.Checked;
                            user.IsDC = this.IsDC.Checked;
                            user.ProjectId = 0;
                            user.ProjectName = string.Empty;
                            user.OrgCode = string.Empty;
                            foreach (var orgItem in this.lbOrg.CheckedItems)
                            {
                                user.OrgCode += orgItem.Value + ";";
                            }
                            //this.AttachSigned(user);

                            user.IsTTYT = this.cbIsTTYT.Checked;
                            user.SoTYT = this.txtSoTYT.Text;
                            this.userService.Update(user);
                        }
                    }
                    else
                    {
                        var user = new AA_Users
                        {
                            Username = this.txtUsername.Text.Trim(),
                            Password = Utility.GetMd5Hash(GlobalVariables.Current.DefaultPasswordForNewUser),
                            FullName = this.txtFullName.Text.Trim(),
                            Position = this.txtPosition.Text.Trim(),
                            RoleId = Convert.ToInt32(this.ddlRoles.SelectedValue),
                            RoleName = this.ddlRoles.SelectedItem.Text,
                            LocationId = departmentObj.TypeId,
                            LocationName = departmentObj.TypeName,
                            Email = this.txtEmail.Text.Trim(),
                            Phone = this.txtHomePhone.Text.Trim(),
                            CellPhone = this.txtCellPhone.Text.Trim(),
                            //DisciplineId = Convert.ToInt32(this.ddlDiscipline.SelectedValue),
                            //DisciplineName = this.ddlDiscipline.SelectedItem.Text,
                            IsDeptManager = false,
                            IsLeader = this.cbLeader.Checked,
                            IsEngineer = this.cbEngineer.Checked,
                            IsActive = this.cbActive.Checked,
                            IsDC = this.IsDC.Checked,
                            //ConfidentialId = Convert.ToInt32(this.ddlConfidentiality.SelectedValue),
                            //ConfidentialName = this.ddlConfidentiality.SelectedItem.Text
                        };

                        
                        user.ProjectId = 0;
                        user.ProjectName = string.Empty;
                        user.OrgCode = string.Empty;
                        foreach (var orgItem in this.lbOrg.CheckedItems)
                        {
                            user.OrgCode += orgItem.Value + ";";
                        }
                        //this.AttachSigned(user);
                        this.userService.Insert(user);
                    }

                    departmentObj.IsAllowDelete = false;
                    this.roleService.Update(departmentObj);

                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                }
                
            }
        }

        private void AttachSigned(AA_Users userObj)
        {
            var targetFolder = "../../DocumentLibrary/UserSigned";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + "/DocumentLibrary/UserSigned";
            if (docuploader.UploadedFiles.Count > 0)
            {
                var docFile = docuploader.UploadedFiles[0];
                var docFileName = docFile.FileName;

                var serverDocFileName = docFileName;

                // Path file to save on server disc
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + serverDocFileName;

                docFile.SaveAs(saveFilePath, true);
                userObj.SignImageUrl = serverFilePath;
                this.docuploader.UploadedFiles.Clear();
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
        }

        #endregion

        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            
        }

        protected void ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.txtUsername.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter User name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            else if (this.HasUserAccount)
            {
                this.fileNameValidator.ErrorMessage = "The user name is already in use.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = !this.userService.CheckExists(this.UserId, this.txtUsername.Text.Trim());
            }
        }

        protected void ddlRoles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //var departmentObj = this.roleService.GetByID(Convert.ToInt32(this.ddlRoles.SelectedItem.Value));
            //if (departmentObj != null)
            //{
            //    var titleList = this.titleService.GetAll().Where(t => t.LocationId == departmentObj.TypeId).OrderBy(t => t.Name).ToList();
            //    titleList.Insert(0, new Title() { ID = 0 });
            //    this.ddlTitle.DataSource = titleList;
            //    this.ddlTitle.DataValueField = "Id";
            //    this.ddlTitle.DataTextField = "Name";
            //    this.ddlTitle.DataBind();
            //}
        }
    }
}