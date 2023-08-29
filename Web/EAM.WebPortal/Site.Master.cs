// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Site.Master.cs" company="">
//   
// </copyright>
// <summary>
//   The site master.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EAM.Business.Services.Security;
using EAM.Data.Entities;
using EAM.WebPortal.Resources.Utilities.Session;
using Telerik.Web.UI;
using MenuType = EAM.WebPortal.Resources.Utilities.MenuType;

namespace EAM.WebPortal
{
    /// <summary>
    /// The site master.
    /// </summary>
    public partial class SiteMaster : MasterPage
    {
        #region Fields

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        private AA_PermissionsService _permissionService;
        private AA_MenusService _menuService;
        private AA_UsersLoginHistoryService userloginservice;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the role id on session.
        /// </summary>
        /// <value>
        /// The role id.
        /// </value>
        public int RoleId
        {
            get { return UserSession.Current.User.RoleId != null ? UserSession.Current.User.RoleId.Value : -1; }
        }

        #endregion

        #region Methods

        public SiteMaster()
        {
            _permissionService = new AA_PermissionsService();
            _menuService = new AA_MenusService();
            userloginservice = new AA_UsersLoginHistoryService();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!UserSession.Current.IsAvailable)
            {
                if (Request.RawUrl != "/Control/Security/Login.aspx")
                {
                    Session.Add("ReturnURL", Request.RawUrl);
                }

                Response.Redirect("~/Control/Security/Login.aspx");
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        /// <summary>
        /// Handles the PreLoad event of the master_Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.InvalidOperationException">Validation of Anti-XSRF token failed.</exception>
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? string.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? string.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserSession.Current.IsAvailable)
                {
                    var lblFullName = this.menu.Items[0].FindControl("lblFullName") as Label;
                    var lblCurrentUserOnline = this.menu.Items[0].FindControl("lblCurrentUserOnline") as Label;

                    if (lblFullName != null)
                    {
                        lblFullName.Text = "Welcome, " + UserSession.Current.User.FullName + ".";
                    }

                    if (lblCurrentUserOnline != null)
                    {
                        lblCurrentUserOnline.Text = "Current license online: " + Application.Get("userLoginCount");
                    }
                    
                    LoadLeftMenu();
                    LoadMainMenu();

                    this.menu.Items[0].Items[0].Visible = ConfigurationManager.AppSettings.Get("EnableLDAP") == "False";
                }
            }
        }

       
        private void UserLogOut(int logId)
        {
            var userlogin = userloginservice.GetByID(logId);
            if(userlogin != null) { 
            var curenttime = DateTime.Now;
            var duretime = curenttime.TimeOfDay - userlogin.ServerTime.Value.TimeOfDay;
            userlogin.LogoutLocalTime = userlogin.LocalTime.Value.Add(duretime);
            userlogin.DurationTimeLogin = duretime.ToString();
            userlogin.IsOn = false;
            userloginservice.Update(userlogin);}
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Loads the main menu.
        /// </summary>
        private void LoadMainMenu()
        {
            var menus = new List<AA_Menus>();

            if (UserSession.Current.User.RoleId == 1)
            {
                menus = this._menuService.GetAll().Where(t => t.Active == true).OrderBy(t => t.Priority).ToList();
            }
            else
            {
                menus = this._menuService.GetAllRelatedPermittedMenuItems(RoleId, (int)MenuType.TopMenu).Where(t => t.Active == true).OrderBy(t => t.Priority).ToList();

                if (menus.Count == 0)
                {
                    var dashBoardMenu = _menuService.GetByID(1);
                    menus.Add(dashBoardMenu);
                }
            }

            this.MainMenu.DataSource = menus;
            this.MainMenu.DataTextField = "Description";
            this.MainMenu.DataNavigateUrlField = "Url";
            this.MainMenu.DataFieldID = "Id";
            this.MainMenu.DataValueField = "Id";
            this.MainMenu.DataFieldParentID = "ParentId";
            
            this.MainMenu.DataBind();

            if (Session["SelectedMainMenu"] != null)
            {
                var selectedMenu = this.MainMenu.Items.FirstOrDefault(t => t.Text == Session["SelectedMainMenu"].ToString());
                if (selectedMenu != null)
                {
                    selectedMenu.Selected = true;
                }

                Session.Remove("SelectedMainMenu");
            }

            this.SetIcon(this.MainMenu.Items, menus);

            ////foreach (RadMenuItem menuItem in MainMenu.Items)
            ////{
            ////    if (!string.IsNullOrEmpty(menuItem.Value))
            ////    {
            ////        var tempMenu = menus.FirstOrDefault(t => t.Id == Convert.ToInt32(menuItem.Value));
            ////        if (tempMenu != null)
            ////        {
            ////            menuItem.ImageUrl = tempMenu.Icon;
            ////        }
            ////    }
            ////}
        }

        private void SetIcon(RadMenuItemCollection menu, List<AA_Menus> menuData)
        {
            foreach (RadMenuItem menuItem in menu)
            {
                if (!string.IsNullOrEmpty(menuItem.Value))
                {
                    var tempMenu = menuData.FirstOrDefault(t => t.Id == Convert.ToInt32(menuItem.Value));
                    if (tempMenu != null)
                    {
                        menuItem.ImageUrl = tempMenu.Icon;
                    }
                }

                if (menuItem.Items.Count > 0)
                {
                    this.SetIcon(menuItem.Items, menuData);
                }
            }
        }

        /// <summary>
        /// Loads the left menu.
        /// </summary>
        private void LoadLeftMenu()
        {
            var menus = _menuService.GetAllRelatedPermittedMenuItems(RoleId, (int)MenuType.LeftMenu);
            if (menus == null)
            {
                //bottomLeftPane.Collapsed = true; 
                return;
            }
            
            //Gets root parents only
            menus = menus.Where(x => x.ParentId == null).ToList();
            
            if (menus.Count == 0)
            {
                //bottomLeftPane.Collapsed = true;
                return;
            }
            //LeftMenu.DataSource = menus;
            //LeftMenu.DataTextField = "Description";
            //LeftMenu.DataNavigateUrlField = "Url";
            //LeftMenu.DataFieldID = "Id";
            //LeftMenu.DataBind();
        }

        #endregion

        protected void menu_OnItemClick(object sender, RadMenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "LogoutCommand":
                    UserLogOut(UserSession.Current.LogId);
                    Session.Clear();
                    Session.Abandon();
                    FormsAuthentication.SignOut();

                    UserSession.DestroySession();
                    Session.Remove("ReturnURL");
                    Response.Redirect("~/Control/Security/Login.aspx");
                    break;
            }
        }
    }
}
