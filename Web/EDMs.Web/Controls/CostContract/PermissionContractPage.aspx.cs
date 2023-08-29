// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PermissionContractPage : Page
    {
        private readonly PermissionContractService permissionContractService = new PermissionContractService();
        private readonly UserService userService = new UserService();
        private readonly ContractService contractService = new ContractService();

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
                {
                    this.LoadUserList();
                }
            }
        }

        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["contractId"]))
            {
                var contractObj = this.contractService.GetById(Convert.ToInt32(Request.QueryString["contractId"]));
                if (contractObj != null)
                {
                    foreach (RadListBoxItem user in this.ddlUser.SelectedItems)
                    {

                        var permissionObj = new PermissionContract()
                        {
                            UserId = Convert.ToInt32(user.Value),
                            ContractID = contractObj.ID,
                            ProjectId = contractObj.ProjectID,
                            ProcurementRequirementID = contractObj.ProcurementRequirementID,
                            IsFullPermission = this.rbtnFull.Checked,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedDate = DateTime.Now
                        };
                        this.permissionContractService.Insert(permissionObj);
                    }
                }

                this.LoadUserList();
                this.grdDocument.Rebind();
            }

            this.grdDocument.Rebind();
        }

        private void LoadUserList()
        {
            var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);

            var userIdsInPermission =
                        this.permissionContractService.GetAllByContract(contractId).Select(t => t.UserId);
            var userList = this.userService.GetAll().Where(t => !userIdsInPermission.Contains(t.Id));
            this.ddlUser.DataSource = userList;
            this.ddlUser.DataTextField = "UserNameWithFullName";
            this.ddlUser.DataValueField = "Id";
            this.ddlUser.DataBind();
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            this.permissionContractService.Delete(objId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var contractId = Convert.ToInt32(this.Request.QueryString["contractId"]);
            var permissionList = this.permissionContractService.GetAllByContract(contractId);
            this.grdDocument.DataSource = permissionList;
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }
    }
}