using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EAM.Business.Services;
using EAM.Data.Dto;
using EAM.Data.Entities;

namespace EAM.WebPortal.Resources.Utilities.Session
{
    public class UserSession
    {
        private static readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        /// <summary>
        /// Creates the session.
        /// </summary>
        /// <param name="user">The user.</param>
        public static void CreateSession(AA_Users user)
        {

            var session = new UserSession
                              {
                                  User = user,
                                  RoleId = user.RoleId != null ? user.RoleId.Value : -1,
                                  IsAvailable = true,
                                  AuthGroup = ConfigurationManager.AppSettings.Get("AuthGroup"),
                                  UserInfor = GetUserInfor(user)
            };
            HttpContext.Current.Session["__UserSession__"] = session;
            HttpContext.Current.Session.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TimeOut"));
        }

        public static UserInforDto GetUserInfor(AA_Users userObj)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = "ADMIN";
            var orgList = new List<OrganizationDto>();
            var ds = eamService.GetDataSet("getOrgbyU", new[] { userParam });
            if (ds != null)
            {
                orgList = eamService.CreateListFromTable<OrganizationDto>(ds.Tables[0]);
                foreach (var item in orgList)
                {
                    item.FullName = item.MADONVI + " - " + item.TENDONVI;
                }
            }

            var userInfor = new UserInforDto()
            {
                TENNGUOIDUNG = userObj.FullName,
                MANGUOIDUNG = userObj.Username,
                TENNHOM = userObj.Role.Name,
                MANHOM = userObj.Role.Id.ToString(),
                Organizations = new List<OrganizationDto>()
            };

            if (!string.IsNullOrEmpty(userObj.OrgCode))
            {
                foreach (var orgCode in userObj.OrgCode.Split(';'))
                {
                    var orgobj = orgList.FirstOrDefault(t => t.MADONVI == orgCode);
                    if (orgobj != null)
                    {
                        userInfor.Organizations.Add(orgobj);
                    }
                }
            }
            

            return userInfor;
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

        public UserInforDto UserInfor { get; set; }
        public string AuthGroup { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public AA_Users User { get; set; }

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