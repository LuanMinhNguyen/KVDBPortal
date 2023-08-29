﻿// --------------------------------------------------------------------------------------------------------------------
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
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto.PurchaseOrder;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Purchase
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class REQList : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Mua Sắm");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
            }
        }

        protected void grdData_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var objectList = new List<REQListDto>();
            ds = this.eamService.GetDataSet("get_REQ_List", new[] { userParam });
            if (ds != null)
            {
                objectList = this.eamService.CreateListFromTable<REQListDto>(ds.Tables[0]);
            }

            grdData.DataSource = objectList;
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdData.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdData.Height = Unit.Percentage(((CheckBox)sender).Checked ? 85 : 90);
            this.grdData.Rebind();
        }
    }
}
