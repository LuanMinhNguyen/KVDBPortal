// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Site.Master.cs" company="">
//   
// </copyright>
// <summary>
//   The site master.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Security;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Security;
    using EDMs.Web.Utilities.Enums;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The site master.
    /// </summary>
    public partial class SiteMaster : MasterPage
    {
        #region Fields

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        private PermissionService _permissionService;
        private MenuService _menuService;
        private UsersLoginHistoryService userloginservice;
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
            _permissionService = new PermissionService();
            _menuService = new MenuService();
            userloginservice = new UsersLoginHistoryService();
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
                if (Request.RawUrl != "/Controls/Security/Login.aspx")
                {
                    Session.Add("ReturnURL", Request.RawUrl);
                }

                Response.Redirect("~/Controls/Security/Login.aspx");
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
            var menus = new List<Data.Entities.Menu>();

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

        private void SetIcon(RadMenuItemCollection menu, List<EDMs.Data.Entities.Menu> menuData)
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
                    Response.Redirect("~/Controls/Security/Login.aspx");
                    break;
            }
        }
    }
}
