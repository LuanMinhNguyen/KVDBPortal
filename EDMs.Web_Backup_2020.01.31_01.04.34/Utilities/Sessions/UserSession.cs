using System;
using System.Configuration;

namespace EDMs.Web.Utilities.Sessions
{
    using System.Web;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;

    /// <summary>
    /// The user session.
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// Creates the session.
        /// </summary>
        /// <param name="user">The user.</param>
        public static void CreateSession(User user)
        {
            var resourceService = new ResourceService();
            var resource = user.ResourceId != null ? resourceService.GetByID(user.ResourceId.Value) : null;

            var session = new UserSession
                              {
                                  User = user,
                                  IsResource = (resource != null && resource.IsResource != null) && resource.IsResource.Value,
                                  ResourceId = resource != null ? resource.Id : -1,
                                  RoleId = user.RoleId != null ? user.RoleId.Value : -1,
                                  IsAvailable = true,
                              };
            HttpContext.Current.Session["__UserSession__"] = session;
            HttpContext.Current.Session.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TimeOut"));
        }

        /// <summary>
        /// Destroys the session.
        /// </summary>
        public static void DestroySession()
        {
            HttpContext.Current.Session["__UserSession__"] = null;
        }

        /// <summary>
        /// Checks the session and navigate.
        /// </summary>
        public static void CheckSessionAndNavigate()
        {
            if(HttpContext.Current.Session["__UserSession__"] == null)
            {
                HttpContext.Current.Response.Redirect(GlobalConsts.LoginFormPath);
            }
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static UserSession Current
        {
            get
            {
                if (HttpContext.Current.Session != null)
                {
                    var session = (UserSession)HttpContext.Current.Session["__UserSession__"] ??
                                 new UserSession { IsAvailable = false };
                    return session;   
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets the role id.
        /// </summary>
        public int RoleId { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether is resource.
        /// </summary>
        public bool IsResource { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is available.
        /// </summary>
        public bool IsAvailable { get; set; }
        /// <summary>
        /// Set ID login history
        /// </summary>
        public int LogId { get; set; }
    }
}