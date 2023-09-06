using EDMs.Business.Services;
using EDMs.Data.Dto.Store;
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
using EDMs.Data.Dto.Part;

namespace EDMs.Web.Controls.Parts
{
    public partial class PartList : System.Web.UI.Page
    {
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("SelectedMainMenu", "Quản lý vật tư");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
            }
        }

        protected void grdData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Id;
            DataSet ds;
            var dataList = new List<PartDto>();
            ds = this.eamService.GetDataSet("get_part_r5", new[] { userParam });
            if (ds != null)
            {
                dataList = this.eamService.CreateListFromTable<PartDto>(ds.Tables[0]);
            }

            grdData.DataSource = dataList;
        }

        protected void grdData_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                string ngungsudung = dataItem["NgungSuDung"].Text;
                RadCheckBox cbNgungSuDung = dataItem["cbNgungSuDung"].FindControl("cbNgungSuDung") as RadCheckBox;
                cbNgungSuDung.Checked = ngungsudung == "+" ? true : false;
            }
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdData.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdData.Height = Unit.Percentage(((CheckBox)sender).Checked ? 75 : 80);
            this.grdData.Rebind();
        }
    }
}