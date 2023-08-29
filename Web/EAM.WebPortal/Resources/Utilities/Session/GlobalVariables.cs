using System.Web;

namespace EAM.WebPortal.Resources.Utilities.Session
{
    public class GlobalVariables
    {
        /// <summary>
        /// Initializes the value of global variables
        /// </summary>
        private GlobalVariables()
        {
            DefaultPasswordForNewUser = "123456";
        }

        #region CommonIds
        #endregion

        public string DefaultPasswordForNewUser { get; set; }

        public static GlobalVariables Current
        {
            get
            {
                var session = (GlobalVariables) HttpContext.Current.Session["__GlobalVariables__"];
                if (session == null)
                {
                    session = new GlobalVariables();
                    HttpContext.Current.Session["__GlobalVariables__"] = session;
                }
                return session;
            }
        }
    }
}