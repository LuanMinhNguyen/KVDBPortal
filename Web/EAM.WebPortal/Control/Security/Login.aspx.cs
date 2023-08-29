using System;
using System.Configuration;
using System.Net;
using System.Web.Security;
using System.Web.UI;
using EAM.Business.Services.Security;
using EAM.Data.Entities;
using EAM.WebPortal.Resources.Utilities;
using EAM.WebPortal.Resources.Utilities.LDAP;
using UserSession = EAM.WebPortal.Resources.Utilities.Session.UserSession;

namespace EAM.WebPortal.Control.Security
{
    public partial class Login : Page
    {
        #region Fields
        private readonly AA_UsersService userService;
        private readonly AA_UsersLoginHistoryService userLoginHistoryService;
        private readonly AA_MenusService menuService;
        #endregion

        public Login()
        {
            userService = new AA_UsersService();
            userLoginHistoryService = new AA_UsersLoginHistoryService();
            this.menuService = new AA_MenusService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            txtUsername.Focus();
            if (!IsPostBack)
            {
                UserSession.DestroySession();
                this.txtPassword.Text = "Password";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            var user = userService.GetByUsername(username);
            if (ConfigurationManager.AppSettings["EnableLDAP"] == "true")
            {
                var adPath = ConfigurationManager.AppSettings["LDAPPath"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                var adAuth = new LdapAuthentication(adPath);
                if (adAuth.IsAuthenticated(domain, username, password))
                {
                    if (user != null)
                    {
                        UserSession.CreateSession(user);
                        FormsAuthentication.RedirectFromLoginPage(user.Username, false);

                        if (Session["ReturnURL"] != null)
                        {
                            var returnUrl = Session["ReturnURL"].ToString();
                            Session.Remove("ReturnURL");
                            Response.Redirect("~" + returnUrl);
                        }
                        else
                        {
                            Response.Redirect("~/Default.aspx");
                        }
                    }
                    else
                    {
                        var newUser = new AA_Users()
                        {
                            Username = username,
                            Password = Utility.GetMd5Hash(password),
                            FullName = adAuth.FilterAttribute,
                            Email = username + "@" + domain
                        };

                        var isSuccess = this.userService.Insert(newUser);
                        if (isSuccess)
                        {
                            UserSession.CreateSession(newUser);
                            Response.Redirect("~/Default.aspx");
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "Username or password is incorrect. ";
                }
            }
            else
            {
                if (user != null && user.Password == Utility.GetMd5Hash(password))
                {
                    UserSession.CreateSession(user);
                    FormsAuthentication.RedirectFromLoginPage(user.Username, false);

                    if (Session["ReturnURL"] != null)
                    {
                        var returnUrl = Session["ReturnURL"].ToString();
                        Session.Remove("ReturnURL");
                        Response.Redirect("~" + returnUrl);
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                }
                else
                {
                    lblMessage.Text = "Username or password is incorrect. ";
                }
            }
        }
       
        private void GetInformationClient()
        {
            var userlogin = new AA_UsersLoginHistory();

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