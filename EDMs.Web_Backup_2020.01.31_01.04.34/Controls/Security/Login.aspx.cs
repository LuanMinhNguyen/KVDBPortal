using System;
using System.Net;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.Configuration;
using EDMs.Business.Services.Security;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using EDMs.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using EDMs.Web.Utilities.Enums;

namespace EDMs.Web.Controls.Security
{
    public partial class Login : Page
    {
        #region Fields
        private readonly UserService userService;
        private readonly UsersLoginHistoryService userLoginHistoryService;
        private readonly MenuService menuService;
        #endregion

        public Login()
        {
            userService = new UserService();
            userLoginHistoryService = new UsersLoginHistoryService();
            this.menuService = new MenuService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            txtUsername.Focus();
            if (!IsPostBack)
            {
                this.txtPassword.Text = "Password";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
            string username = txtUsername.Text;
           // string password = Utility.GetMd5Hash(txtPassword.Text);
            string password = txtPassword.Text;
            var user = userService.GetUserByUsername(username);
            if (ConfigurationManager.AppSettings["EnableLDAP"].ToLower() == "true")
            {
                var adPath = ConfigurationManager.AppSettings["LDAPPath"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                var adAuth = new LdapAuthentication(adPath);
                if (adAuth.GetUser(domain, username, password) != null)
                {
                    if (user != null)
                    {
                        var menus = this.menuService.GetAllRelatedPermittedMenuItems(user.RoleId.GetValueOrDefault(), (int)MenuType.TopMenu).Where(t => t.Active == true).OrderBy(t => t.Priority).ThenBy(t => t.Id).ToList();

                        UserSession.CreateSession(user);
                        FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                        GetInformationClient();
                        Session["UserName"] = user.Username;
                        if (UserSession.Current.User.Role.ContractorId == 3)
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["TransNoContractor"]))
                            {
                                Response.Redirect("~/Controls/Document/ContractorTransmittalList.aspx?TransNoContractor=" + Request.QueryString["TransNoContractor"].ToString());
                            }
                            else if(!string.IsNullOrEmpty(Request.QueryString["TransNoPecc2"]))
                            {
                                Response.Redirect("~/Controls/Document/ContractorTransmittalList.aspx?TransNoPecc2=" + Request.QueryString["TransNoPecc2"].ToString());
                            }
                            else
                            {
                                Response.Redirect("~/Controls/Document/ContractorTransmittalList.aspx");
                            }
                            
                        }
                        else
                        {
                                if (Session["ReturnURL"] != null)
                                {
                                    var returnUrl = Session["ReturnURL"].ToString();
                                    Session.Remove("ReturnURL");
                                    Response.Redirect("~" + returnUrl);
                                }
                                else
                                {
                                    var temp = menus.FirstOrDefault(t => !string.IsNullOrEmpty(t.Url));
                                    if (temp != null)
                                    {
                                    //Response.Redirect(temp.Url.Contains("~/") ? temp.Url : "~/" + temp.Url);
                                    Response.Redirect("~//ToDoListPage.aspx");
                                }
                                    else
                                    {
                                        Response.Redirect("~/Dashboard.aspx");
                                    }
                                }
                        }
                        
                    }
                    else
                    {
                        //var newUser = new User()
                        //{
                        //    Username = username,
                        //    Password = Utility.GetMd5Hash(password),
                        //    FullName = adAuth.FilterAttribute,
                        //    Email = username + "@" + domain
                        //};

                        //var isSuccess = this._userService.Insert(newUser);
                        //if (isSuccess)
                        //{
                        //    UserSession.CreateSession(newUser);
                        //    Response.Redirect("~/Default.aspx");
                        //}
                        Session.Clear();
                        FormsAuthentication.SignOut();
                        lblMessage.Text = "Please. Contact admin to register your account on PEDMS System.";
                    }
                }
                else
                {
                   
                        Session.Clear();
                        FormsAuthentication.SignOut();
                        lblMessage.Text = "The user name or password is incorrect.";
                    
                    
                }
            }
            else
            {
                if (user != null && user.Password == Utility.GetMd5Hash(password))
                {
                    var menus = this.menuService.GetAllRelatedPermittedMenuItems(user.RoleId.GetValueOrDefault(), (int)MenuType.TopMenu).Where(t => t.Active == true).OrderBy(t => t.Priority).ThenBy(t => t.Id).ToList();
                    UserSession.CreateSession(user);
                    FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                    GetInformationClient();
                    Session["UserName"] = user.Username;
                    if (UserSession.Current.User.Role.ContractorId == 3)
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["TransNoContractor"]))
                        {
                            Response.Redirect("~/Controls/Document/ContractorTransmittalList.aspx?TransNoContractor=" + Request.QueryString["TransNoContractor"].ToString());
                        }
                        else if (!string.IsNullOrEmpty(Request.QueryString["TransNoPecc2"]))
                        {
                            Response.Redirect("~/Controls/Document/ContractorTransmittalList.aspx?TransNoPecc2=" + Request.QueryString["TransNoPecc2"].ToString());
                        }
                        else
                        {
                            Response.Redirect("~/Controls/Document/ContractorTransmittalList.aspx");
                        }
                    }
                    else
                    {

                        if (Session["ReturnURL"] != null)
                        {
                            var returnUrl = Session["ReturnURL"].ToString();
                            Session.Remove("ReturnURL");
                            Response.Redirect("~" + returnUrl);
                        }
                        else
                        {
                            var temp = menus.FirstOrDefault(t => !string.IsNullOrEmpty(t.Url));
                            if (temp != null)
                            {
                                //Response.Redirect(temp.Url.Contains("~/") ? temp.Url : "~/" + temp.Url);
                                Response.Redirect("~//ToDoListPage.aspx");
                            }
                            else
                            {
                                Response.Redirect("~/Dashboard.aspx");
                            }
                        }
                    }
                }
                else
                {
                    Session.Clear();
                    FormsAuthentication.SignOut();
                    lblMessage.Text = "Username or password is incorrect.";
                }
            }
        }
       
        private void GetInformationClient()
        {
            var userlogin = new UsersLoginHistory();

            var ip = Request.ServerVariables["REMOTE_HOST"];
            userlogin.IpAddress = this.IpAddress.Value.ToString().Contains(ip)? this.IpAddress.Value: ip+", "+this.IpAddress.Value;

            var hostname = "";var domain = "";
            try {  IPHostEntry hostinfor = new IPHostEntry();
            hostinfor= Dns.Resolve(ip);
            hostname = hostinfor.HostName;
            if (hostname == ip) hostname = Request.UserHostName;
            domain = hostname;}
            catch { }
           

            var language = Request.UserLanguages[0].ToLowerInvariant().Trim();
            var culture= System.Globalization.CultureInfo.CreateSpecificCulture(language);
            

            var os = this.Os.Value;
            var getBrowser = this.Browser.Value;
            var localtime = this.LocalTime.Value.Split(':');
            var timeZone = this.TimeZone.Value;
            var localdate = this.LocalDate.Value.Split('/');
           
           // var sizeRam = this.Memory.Value;
            userlogin.UserName = UserSession.Current.User.Username;
            userlogin.FullName = UserSession.Current.User.FullName;
            userlogin.ServerTime = DateTime.Now;
          
            userlogin.LocalTime = new DateTime(Convert.ToInt32( localdate[2]), Convert.ToInt32(localdate[1]), Convert.ToInt32(localdate[0]), Convert.ToInt32(localtime[0]), Convert.ToInt32(localtime[1]), Convert.ToInt32(localtime[2]), Convert.ToInt32(localtime[3]));
            userlogin.LocalTimeZone = timeZone;

          
          
           
            userlogin.WindownDomainUser = domain;
            userlogin.HostNameComputer = hostname;
            userlogin.Browser = getBrowser;
            userlogin.OSDetail = os;
            userlogin.LanguageFormat = culture.NativeName;
            userlogin.IsOn = true;

            
            this.userLoginHistoryService.Insert(userlogin);
            UserSession.Current.LogId = userlogin.ID;
            Session["LogID"] = userlogin.ID;
        }
       
    }
}