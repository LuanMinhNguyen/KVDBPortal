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
using EDMs.Data.Dto;
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
    public partial class EAM_PPMManagement : Page
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
                this.GetPPMDataList();
                this.LoadPPMList();
            }
        }

        private void GetPPMDataList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            DataSet ds;
            var PPMList = new List<PPMsDto>();
            ds = this.eamService.GetDataSet("get_PPMs", new[] { userParam });
            if (ds != null)
            {
                PPMList = this.eamService.CreateListFromTable<PPMsDto>(ds.Tables[0]);
                foreach (var item in PPMList)
                {
                    item.FullName = item.QUY_TRINH + " - " + item.MO_TA;
                }
            }

            Session.Add("PPMList", PPMList);
        }

        private void LoadPPMList()
        {
            var PPMList = Session["PPMList"] as List<PPMsDto>;
            this.BindPPMData(PPMList);
            
        }

        private void BindPPMData(List<PPMsDto> PPMList)
        {
            this.lbPPM.Items.Clear();
            foreach (var item in PPMList)
            {
                var lbItem = new RadListBoxItem(item.FullName, item.QUY_TRINH);
                lbItem.Attributes.Add("THOI_LUONG", item.THOI_LUONG.ToString());
                lbItem.Attributes.Add("KIEU_SINH_PHIEU", item.KIEU_SINH_PHIEU);
                lbItem.Attributes.Add("KIEU_CONG_VIEC", item.KIEU_CONG_VIEC);
                this.lbPPM.Items.Add(lbItem);
            }

            this.lbPPM.DataBind();
        }

        private static int[] GetData()
        {
            var itemsCount = 5000;
            var arr = new int[itemsCount];

            for (var i = 0; i < itemsCount; i++)
            {
                arr[i] = i;
            }
            return arr;
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RefreshProgressReport")
            {
            }
            
        }

        protected void lbPPM_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lblPPMName = this.ToolBarPPM.Items[0].FindControl("lblPPMName") as Label;
            lblPPMName.Text = this.lbPPM.SelectedItem.Text;
            var PPMObj = new PPMsDto()
            {
                QUY_TRINH = this.lbPPM.SelectedItem.Value,
                FullName = this.lbPPM.SelectedItem.Text,
            };

            this.grdPPMActivities.Rebind();
            this.grdPPMLaborPlan.Rebind();
            this.grdPPMPartPlan.Rebind();
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            var PPMList = Session["PPMList"] as List<PPMsDto>;

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                var searchText = this.txtSearch.Text.Trim().ToUpper();
                var filterPPMList = PPMList.Where(t => t.FullName.ToUpper().Contains(searchText)
                                                        || (t.QUY_TRINH != null && t.QUY_TRINH.ToUpper().Contains(searchText))
                                                        || (t.MO_TA != null && t.MO_TA.ToUpper().Contains(searchText))
                                                        || (t.KIEU_SINH_PHIEU != null && t.KIEU_SINH_PHIEU.ToUpper().Contains(searchText))
                                                        || (t.KIEU_CONG_VIEC != null && t.KIEU_CONG_VIEC.ToUpper().Contains(searchText))
                                                        || (t.DON_VI_THOI_GIAN != null && t.DON_VI_THOI_GIAN.ToUpper().Contains(searchText))
                                                        ).ToList();

                this.BindPPMData(filterPPMList);
            }
            else
            {
                this.BindPPMData(PPMList);
            }
        }
        
        protected void grdPPMActivities_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ppmActivitiesList = new List<PPMActivitiesDto>();
            if (this.lbPPM.SelectedItem != null)
            {
                var ppmParam = new SqlParameter("@QuyTrinh", SqlDbType.NVarChar);
                ppmParam.Value = this.lbPPM.SelectedItem.Value;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_PPM_Activities", new[] { ppmParam });
                if (ds != null)
                {
                    ppmActivitiesList = this.eamService.CreateListFromTable<PPMActivitiesDto>(ds.Tables[0]);
                }
            }

            this.grdPPMActivities.DataSource = ppmActivitiesList;
        }

        protected void grdPPMPartPlan_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ppmPartPlanList = new List<PPMPartPlanDto>();
            if (this.lbPPM.SelectedItem != null)
            {
                var ppmParam = new SqlParameter("@QuyTrinh", SqlDbType.NVarChar);
                ppmParam.Value = this.lbPPM.SelectedItem.Value;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_PPM_Plan_Parts", new[] { ppmParam });
                if (ds != null)
                {
                    ppmPartPlanList = this.eamService.CreateListFromTable<PPMPartPlanDto>(ds.Tables[0]);
                }
            }

            this.grdPPMPartPlan.DataSource = ppmPartPlanList;
        }

        protected void grdPPMLaborPlan_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var ppmLaborPlanList = new List<PPMLaborPlanDto>();
            if (this.lbPPM.SelectedItem != null)
            {
                var ppmParam = new SqlParameter("@QuyTrinh", SqlDbType.NVarChar);
                ppmParam.Value = this.lbPPM.SelectedItem.Value;
                DataSet ds;
                ds = this.eamService.GetDataSet("get_PPM_Plan_Labors", new[] { ppmParam });
                if (ds != null)
                {
                    ppmLaborPlanList = this.eamService.CreateListFromTable<PPMLaborPlanDto>(ds.Tables[0]);
                }
            }

            this.grdPPMLaborPlan.DataSource = ppmLaborPlanList;
        }
    }
}

