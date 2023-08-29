using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Security;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Security
{
    public partial class ChangePasswordForm : Page
    {
        #region Fields
        private readonly UserService _userService;
        #endregion

        public ChangePasswordForm()
        {
            _userService = new UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void OldPasswordValidator_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            UserSession.CheckSessionAndNavigate();

            var userId = UserSession.Current.User.Id;
            var currentUser = _userService.GetByID(userId);
            args.IsValid = currentUser == null || Utility.GetMd5Hash(txtOldPassword.Text) == currentUser.Password;
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            UserSession.CheckSessionAndNavigate();

            if (!this.IsValid)
            {
                return;
            }

            var userTemp = this._userService.GetByID(UserSession.Current.User.Id);
            userTemp.HashCode = Utility.Encrypt(this.txtNewPassword.Text.Trim());
            this._userService.Update(userTemp);

            this._userService.ChangePassword(UserSession.Current.User.Id, Utility.GetMd5Hash(txtNewPassword.Text));
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseWindow();", true);

            UserSession.DestroySession();
        }
    }
}