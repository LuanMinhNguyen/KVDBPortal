using System;
using System.Reflection;
using System.Web;

//using System.Web.Routing;

namespace EAM.WebPortal
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //AuthConfig.RegisterOpenAuth();
                PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                object o = p.GetValue(null, null);
                FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                object monitor = f.GetValue(o);
                MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
                m.Invoke(monitor, new object[] { });

                // Start tracking the number of active sessions when the application starts.
                Application.Add("userLoginCount", 0);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            //Response.Redirect("~/Control/Security/Login.aspx");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Increase the count of active sessions as they come on.
            int userLoginCount = Convert.ToInt32(Application.Get("userLoginCount").ToString());
            userLoginCount++;

            Application.Set("userLoginCount", userLoginCount);
        }

        void Session_End(object sender, EventArgs e)
        {
            if(Session["UserName"]!= null && Session["LogID"] != null)
            {
                object objInt = Session["UserName"];
                string username = objInt.ToString();
                Session.Remove("UserName");

                //object LogID = Session["LogID"];
                //int LogId =Convert.ToInt32( LogID.ToString());
                //Session.Remove("LogID");
                //var userloginservice = new UsersLoginHistoryService();
                //var userlogin = userloginservice.GetByID(LogId);
                //if(userlogin != null) { 
                //var curenttime = DateTime.Now;
                //var duretime = curenttime.TimeOfDay - userlogin.ServerTime.Value.TimeOfDay;
                //userlogin.LogoutLocalTime = userlogin.LocalTime.Value.Add(duretime);
                //userlogin.DurationTimeLogin = duretime.ToString();
                //userlogin.IsOn = false;
                //userloginservice.Update(userlogin);}
            }
              

            // Decrease the number of active sessions as they end.
            int userLoginCount = Convert.ToInt32(Application.Get("userLoginCount").ToString());
            userLoginCount--;

            Application.Set("userLoginCount", userLoginCount);

            // OnlineActiveUsers.OnlineUsersInstance.OnlineUsers.UpdateForUserLeave();
            //Response.Redirect("~/Controls/Security/Login.aspx");
        }
    }
}
