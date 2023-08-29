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
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Data.Dto;
using EDMs.Data.Dto.PPM;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

namespace EDMs.Web.Controls.PPM
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class WOPlanList : Page
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
            Session.Add("SelectedMainMenu", "Quản Lý Công Việc");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
                var txtStartDate = this.ViewToolBar.Items[6].FindControl("txtStartDate") as RadDatePicker;
                var txtEndDate = this.ViewToolBar.Items[6].FindControl("txtEndDate") as RadDatePicker;

                txtStartDate.SelectedDate = DateTime.Now.AddYears(-1);
                txtEndDate.SelectedDate = DateTime.Now;
            }
        }

        protected void grdData_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            var startDateParam = new SqlParameter("@startDate", SqlDbType.NVarChar);
            var endDateParam = new SqlParameter("@endDate", SqlDbType.NVarChar);

            var txtStartDate = this.ViewToolBar.Items[6].FindControl("txtStartDate") as RadDatePicker;
            var txtEndDate = this.ViewToolBar.Items[6].FindControl("txtEndDate") as RadDatePicker;

            startDateParam.Value = txtStartDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
            endDateParam.Value = txtEndDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
            userParam.Value = UserSession.Current.User.Username.ToUpper();
            DataSet ds;
            var objectList = new List<WODto>();
            ds = this.eamService.GetDataSet("get_wo_list_R1", new[] { userParam, startDateParam, endDateParam});
            if (ds != null)
            {
                objectList = this.eamService.CreateListFromTable<WODto>(ds.Tables[0]);
            }

            grdData.DataSource = objectList;
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdData.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdData.Height = Unit.Percentage(((CheckBox)sender).Checked ? 85 : 90);
            this.grdData.Rebind();
        }

        protected void txtStartDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            this.grdData.Rebind();
        }
    }
}

