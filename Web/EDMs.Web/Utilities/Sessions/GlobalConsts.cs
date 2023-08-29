using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDMs.Web.Utilities.Sessions
{
    public static class GlobalConsts
    {
        #region CommonIds

        public const int AdminRoleId = 1;
        public const int AdminUserId = 1;
        
        #endregion

        #region Paths

        public const string LoginFormPath = "~/Controls/Security/Login.aspx";
        public const string SystemUserControlPath = "~/Controls/Security/UserListControl.ascx";
        public const string SystemRoleControlPath = "~/Controls/Security/RoleListControl.ascx";
        public const string SystemPermissionControlPath = "~/Controls/Security/PermissionControl.ascx";
        public const string SystemPermissionDataControlPath = "~/Controls/Security/PermissionDataControl.ascx";

        #endregion
    }
}