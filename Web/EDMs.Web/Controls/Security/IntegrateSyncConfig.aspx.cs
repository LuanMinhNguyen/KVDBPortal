// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class IntegrateSyncConfig : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        private readonly ImportScheduleService importScheduleService = new ImportScheduleService();

        private readonly ExportScheduleService exportScheduleService = new ExportScheduleService();

        private readonly IntergrateParamConfigService paramConfigService = new IntergrateParamConfigService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

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
            Session.Add("SelectedMainMenu", "System");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.LoadScopePanel();
                this.LoadSystemPanel();

                var configObj = this.paramConfigService.GetById(1);
                if (configObj != null)
                {
                    this.txtDataSource.Text = configObj.Amos_Dns;
                    this.txtAmosUid.Text = configObj.Amos_Uid;
                    this.txtAmosPwd.Text = configObj.Amos_Pwd;
                    this.txtAmosDataFile.Text = configObj.Amos_DataFile;
                    this.txtAmosDeptId.Text = configObj.DefaultDeptId;
                    this.txtExportFolder.Text = configObj.Sync_ExportFolder;
                    this.txtImportFolder.Text = configObj.Sync_ImportFolder;
                    this.txtEmail.Text = configObj.Sync_DefaultEmail;
                    this.txtEmailName.Text = configObj.Sync_EmailName;
                    this.txtPass.Text = configObj.Sync_EmailPwd;
                    this.txtMailServer.Text = configObj.Sync_MailServer;
                    this.txtPort.Text = configObj.Sync_Port;
                    this.cbUseDefaultCredentials.Checked = configObj.Sync_UseDefaultCredentials.GetValueOrDefault();
                    this.cbEnableSsl.Checked = configObj.Sync_EnableSsl.GetValueOrDefault();
                    this.txtEmailSendExport.Text = configObj.Sync_EmailSendExport;
                    this.txtConnStr.Text = configObj.Sync_ImportConnStr;
                    this.txtPopMailServer.Text = configObj.Sync_PopServer;
                    this.txtPopPort.Text = configObj.Sync_PopPort;
                    this.cbAuto.Checked = configObj.IsAutoGetSend.GetValueOrDefault();
                    this.cbEnableNotification.Checked = configObj.IsEnableSendEmailNotification.GetValueOrDefault();
                }
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        private void LoadSystemPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("SystemID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "SYSTEM" });

                this.radPbSystem.DataSource = permissions;
                this.radPbSystem.DataFieldParentID = "ParentId";
                this.radPbSystem.DataFieldID = "Id";
                this.radPbSystem.DataValueField = "Id";
                this.radPbSystem.DataTextField = "MenuName";
                this.radPbSystem.DataBind();
                this.radPbSystem.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbSystem.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Integrate/Sync Parameter Config")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void LoadScopePanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ScopeID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId).OrderBy(t => t.Menu.Priority).ToList();
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG MANAGEMENT" });

                this.radPbScope.DataSource = permissions;
                this.radPbScope.DataFieldParentID = "ParentId";
                this.radPbScope.DataFieldID = "Id";
                this.radPbScope.DataValueField = "Id";
                this.radPbScope.DataTextField = "MenuName";
                this.radPbScope.DataBind();
                this.radPbScope.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbScope.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    
                }
            }
        }

        protected void btnTestConnect_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.MessControl.Visible = true;
                var dns = this.txtDataSource.Text.Trim();
                var uid = this.txtAmosUid.Text.Trim();
                var pwd = this.txtAmosPwd.Text.Trim();
                var dataFile = this.txtAmosDataFile.Text.Trim();

                //var connStr = "Dsn=" + dns + ";userid=" + uid + ";databasefile='" + dataFile + "';servername=" + dns +
                //              ";autostop=YES;integrated=NO;debug=NO;disablemultirowfetch=NO;compress=NO;uid=" + uid +
                //              ";pwd=" + pwd;

                var connStr = "DSN=" + dns + ";UID=" + uid + ";pwd=" + pwd;

                //var connStr = "Dsn=amos;uid=admin";

                var conn = new OdbcConnection(connStr);
                conn.Open();
                this.lblMess.Text = "Connect to AMOS data successfull.";
                     
                conn.Close();
            }
            catch (Exception ex)
            {
                this.lblMess.Text = "Can't connect to AMOS data. Have error: " + ex.Message;

            }
        }

        protected void btnSaveAmosConfig_OnClick(object sender, EventArgs e)
        {
            var configObj = this.paramConfigService.GetById(1);
            if (configObj != null)
            {
                configObj.Amos_Dns = this.txtDataSource.Text.Trim();
                configObj.Amos_Uid = this.txtAmosUid.Text.Trim();
                configObj.Amos_Pwd = this.txtAmosPwd.Text.Trim();
                configObj.Amos_DataFile = this.txtAmosDataFile.Text.Trim();
                configObj.DefaultDeptId = this.txtAmosDeptId.Text.Trim();
                this.paramConfigService.Update(configObj);

                this.MessControl.Visible = false;
                this.lblMess.Text = string.Empty;
            }
        }

        protected void btnSaveSyncConfig_OnClick(object sender, EventArgs e)
        {
            var configObj = this.paramConfigService.GetById(1);
            if (configObj != null)
            {
                configObj.Sync_ExportFolder = this.txtExportFolder.Text.Trim();
                configObj.Sync_ImportFolder = this.txtImportFolder.Text.Trim();
                configObj.Sync_DefaultEmail = this.txtEmail.Text.Trim();
                configObj.Sync_EmailName = this.txtEmailName.Text.Trim();
                configObj.Sync_EmailPwd = this.txtPass.Text.Trim();
                configObj.Sync_MailServer = this.txtMailServer.Text.Trim();
                configObj.Sync_Port = this.txtPort.Text.Trim();
                configObj.Sync_UseDefaultCredentials = this.cbUseDefaultCredentials.Checked;
                configObj.Sync_EnableSsl = this.cbEnableSsl.Checked;
                configObj.Sync_EmailSendExport = this.txtEmailSendExport.Text.Trim();
                configObj.Sync_ImportConnStr = this.txtConnStr.Text.Trim();
                configObj.Sync_PopPort = this.txtPopPort.Text.Trim();
                configObj.Sync_PopServer = this.txtPopMailServer.Text.Trim();
                configObj.IsAutoGetSend = this.cbAuto.Checked;
                configObj.IsEnableSendEmailNotification = this.cbEnableNotification.Checked;
                this.paramConfigService.Update(configObj);
            }
        }

        protected void btnAddTimeExport_OnClick(object sender, EventArgs e)
        {
            if (this.txtTimeExport.SelectedDate != null)
            {
                var item = new ExportSchedule()
                {
                    ExportTime = this.txtTimeExport.SelectedDate
                };

                this.exportScheduleService.Insert(item);

                this.grdTimeExport.Rebind();
            }
        }

        protected void grdTimeExport_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.grdTimeExport.DataSource = this.exportScheduleService.GetAll();
        }

        protected void btnAddImportTime_OnClick(object sender, EventArgs e)
        {
            if (this.txtImportTime.SelectedDate != null)
            {
                var item = new ImportSchedule()
                {
                    ImportTime = this.txtImportTime.SelectedDate
                };

                this.importScheduleService.Insert(item);

                this.grdImportTime.Rebind();
            }
        }

        protected void grdImportTime_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.grdImportTime.DataSource = this.importScheduleService.GetAll();
        }

        protected void grdImportTime_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            this.importScheduleService.Delete(objId);

            this.grdImportTime.Rebind();
        }

        protected void grdTimeExport_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            this.exportScheduleService.Delete(objId);

            this.grdTimeExport.Rebind();
        }
    }
}

