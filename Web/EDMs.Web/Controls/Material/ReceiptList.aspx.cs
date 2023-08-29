// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto.MaterialInventory;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Material
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ReceiptList : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

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
            Session.Add("SelectedMainMenu", "Quản Lý Vật Tư");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
            }
        }

        protected void grdReceiptline_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var objList = new List<ReceiptLineDto>();
            ds = this.eamService.GetDataSet("get_Translines_RECV", new[] { userParam });
            if (ds != null)
            {
                objList = this.eamService.CreateListFromTable<ReceiptLineDto>(ds.Tables[0]);
                var ddlKieuGiaoDich = this.ViewToolBar.Items[4].FindControl("ddlKieuGiaoDich") as RadDropDownList;
                switch (ddlKieuGiaoDich.SelectedValue)
                {
                    case "1":
                        objList = objList.Where(t => t.kieuGiaoDich == "PO").ToList();
                        break;
                    case "2":
                        objList = objList.Where(t => t.kieuGiaoDich == "Non-PO").ToList();
                        break;
                }
            }

            grdReceiptline.DataSource = objList;
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdReceiptline.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdReceiptline.Height = Unit.Percentage(((CheckBox)sender).Checked ? 85 : 90);
            this.grdReceiptline.Rebind();
        }

        protected void ddlKieuGiaoDich_OnSelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            this.grdReceiptline.Rebind();
        }
    }
}

