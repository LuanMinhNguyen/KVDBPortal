using EDMs.Business.Services;
using EDMs.Data.Dto;
using EDMs.Web.Utilities.Sessions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using EDMs.Data.Dto.PPM;

namespace EDMs.Web.Controls.PPM
{
    public partial class WOList : System.Web.UI.Page
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
            Session.Add("SelectedMainMenu", "Quản lý công việc");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
            }
        }

        protected void grdData_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
            DataSet ds;
            var dataList = new List<WODto>();
            ds = this.eamService.GetDataSet("get_WO_r5", new[] { userParam });
            if (ds != null)
            {
                dataList = this.eamService.CreateListFromTable<WODto>(ds.Tables[0]);
            }

            grdData.DataSource = dataList;
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdData.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdData.Height = Unit.Percentage(((CheckBox)sender).Checked ? 75 : 80);
            this.grdData.Rebind();
        }

        protected void grdData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                string douutien = dataItem["DoUuTien"].Text;
                Label lbDoUuTien = dataItem["lbDoUuTien"].FindControl("lbDoUuTien") as Label;
                lbDoUuTien.Text = !string.IsNullOrEmpty(douutien) ? douutien.Split('@')[0] : "";
            }
        }
    }
}