using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web
{
    using EDMs.Business.Services.Security;

    /// <summary>
    /// The role list.
    /// </summary>
    public partial class RoleListControl : UserControl
    {
        #region Fields
        
        private readonly RoleService _roleService;

        #endregion

        #region Initializes

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleListControl"/> class.
        /// </summary>
        public RoleListControl()
        {
            _roleService = new RoleService();
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
            if (!IsPostBack)
            {
            }
        }

        /// <summary>
        /// Handles the OnNeedDataSource event of the grdRoles control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridNeedDataSourceEventArgs"/> instance containing the event data.</param>
        protected void grdRoles_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            grdRoles.DataSource = _roleService.GetAll(false);
        }

        /// <summary>
        /// Handles the ItemCommand event of the grdRoles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdRoles_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRoleCommand")
            {
                var item = (GridDataItem)e.Item;
                var roleId = Convert.ToInt32(item.GetDataKeyValue("Id").ToString());
                if (roleId == GlobalConsts.AdminRoleId)
                {
                    return;
                }
                _roleService.Delete(roleId);
                grdRoles.Rebind();
            }
        }

        /// <summary>
        /// Handles the OnItemDataBound event of the grdRoles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridItemEventArgs"/> instance containing the event data.</param>
        protected void grdRoles_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            var currentItem = e.Item as GridDataItem;
            if (currentItem != null)
            {
                var rowIndex = currentItem.ItemIndex;
                var dataKey = currentItem.GetDataKeyValue("Id");

                //Edit link
                var editLink = (Image)e.Item.FindControl("EditLink");
                editLink.Attributes["onclick"] = string.Format("return ShowRoleEditForm('{0}','{1}');", dataKey, rowIndex);
            }
        }

        #endregion

        #region Helpers

        #endregion
    }
}