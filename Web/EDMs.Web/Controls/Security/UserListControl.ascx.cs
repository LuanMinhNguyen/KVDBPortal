using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// The user list.
    /// </summary>
    public partial class UserListControl : UserControl
    {
        #region Fields

        private readonly UserService _userService;
        private readonly ResourceService _resourceService;

        #endregion

        #region Helpers

        /// <summary>
        /// Loads the data to grid.
        /// </summary>
        private void LoadDataToGrid(int pageSize, int startingRecordNumber)
        {
            var resources = this._resourceService.GetAll(pageSize, startingRecordNumber);
            this.grdUsers.DataSource = resources;
        }

        #endregion

        #region Initializes

        /// <summary>
        /// Initializes a new instance of the <see cref="UserListControl"/> class.
        /// </summary>
        public UserListControl()
        {
            _userService = new UserService();
            _resourceService = new ResourceService();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the OnNeedDataSource event of the grdUsers control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridNeedDataSourceEventArgs"/> instance containing the event data.</param>
        protected void grdUsers_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var pageSize = this.grdUsers.PageSize;
            var currentPage = this.grdUsers.CurrentPageIndex;
            var startingRecordNumber = currentPage * pageSize;

            this.grdUsers.VirtualItemCount = this._resourceService.GetItemCount();

            LoadDataToGrid(pageSize, startingRecordNumber);
        }

        /// <summary>
        /// Handles the ItemCommand event of the grdUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdUsers_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if(e.CommandName == "DeleteUserCommand")
            {
                var currentItem = e.Item as GridDataItem;
                if (currentItem != null)
                {
                    var resourceId = Convert.ToInt32(currentItem.GetDataKeyValue("Id").ToString());
                    int userId;
                    if (!Int32.TryParse(currentItem["colUserId"].Text, out userId))
                    {
                        userId = -1;
                    }

                    if (userId == GlobalConsts.AdminUserId)
                    {
                        return;
                    }
                    _resourceService.Delete(resourceId);
                    _userService.Delete(userId);
                    grdUsers.Rebind();
                }
            }
        }

        /// <summary>
        /// Handles the OnItemDataBound event of the grdUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridItemEventArgs"/> instance containing the event data.</param>
        protected void grdUsers_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            var currentItem = e.Item as GridDataItem;
            if (currentItem != null)
            {
                var editLink = (Image)currentItem.FindControl("EditLink");
                var rowIndex = currentItem.ItemIndex;
                var resourceId = currentItem.GetDataKeyValue("Id");
                var userId = currentItem["colUserId"].Text;

                editLink.Attributes["onclick"] = string.Format("return ShowUserEditForm('{0}','{1}','{2}');", resourceId, userId, rowIndex);
            }
        }

        #endregion
    }
}