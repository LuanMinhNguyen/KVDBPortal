// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto.MaterialInventory;
using EDMs.Data.Dto.PPM;

namespace EDMs.Web
{
    using System;
    using System.Configuration;
    using System.Web.UI;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class EAM_IssuesLineList : Page
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

        protected void grdIssuesline_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var woPartPlanList = new List<IssuesLineDto>();
            ds = this.eamService.GetDataSet("get_Issues_lines", new[] { userParam });
            if (ds != null)
            {
                woPartPlanList = this.eamService.CreateListFromTable<IssuesLineDto>(ds.Tables[0]);
            }

            grdIssuesline.DataSource = woPartPlanList;
        }
    }
}

