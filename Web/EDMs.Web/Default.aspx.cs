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
using EDMs.Business.Services;
using EDMs.Data.Dto;
using EDMs.Data.Dto.Dashboard;
using Telerik.Web.UI.Calendar;

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
    public partial class Default : Page
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
            Session.Add("SelectedMainMenu", "Dashboard");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");

            if (!Page.IsPostBack)
            {
                this.LoadInitData();

                this.LoadFolderData();
                this.LoadColumnChartData();
                this.LoadGaugeData();
            }
        }

        private void LoadInitData()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = UserSession.Current.User.Username;
            var ds = this.eamService.GetDataSet("get_org_list", new SqlParameter[] { userParam });
            if (ds != null)
            {
                var dataList = this.eamService.CreateListFromTable<OrganizationDto>(ds.Tables[0]).OrderBy(t => t.DonVi);
                foreach (var item in dataList)
                {
                    item.FullName = item.DonVi + " - " + item.TenDonVi;
                }
                this.ddlOrganization.DataSource = dataList;
                this.ddlOrganization.DataTextField = "FullName";
                this.ddlOrganization.DataValueField = "DonVi";
                this.ddlOrganization.DataBind();

                this.ddlKPIOrg.DataSource = dataList;
                this.ddlKPIOrg.DataTextField = "FullName";
                this.ddlKPIOrg.DataValueField = "DonVi";
                this.ddlKPIOrg.DataBind();
            }

            ds = this.eamService.GetDataSet("get_dept_list", new SqlParameter[] { userParam });
            if (ds != null)
            {
                var dataList = this.eamService.CreateListFromTable<DepartmentDto>(ds.Tables[0]).OrderBy(t => t.BoPhan);
                foreach (var item in dataList)
                {
                    item.FullName = item.BoPhan + " - " + item.TenBoPhan;
                }
                this.ddlDepartment.DataSource = dataList;
                this.ddlDepartment.DataTextField = "FullName";
                this.ddlDepartment.DataValueField = "BoPhan";
                this.ddlDepartment.DataBind();

                this.ddlKPIDept.DataSource = dataList;
                this.ddlKPIDept.DataTextField = "FullName";
                this.ddlKPIDept.DataValueField = "BoPhan";
                this.ddlKPIDept.DataBind();
            }

            this.txtTuNgay.SelectedDate = DateTime.Now.AddYears(-1);
            this.txtDenNgay.SelectedDate = DateTime.Now;
        }

        private void LoadGaugeData()
        {
            //var userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            //var orgParam = new SqlParameter("@DonVi", SqlDbType.NVarChar);
            //var deptParam = new SqlParameter("@BoPhan", SqlDbType.NVarChar);
            //var startDateParam = new SqlParameter("@startDate", SqlDbType.NVarChar);
            //var endDateParam = new SqlParameter("@endDate", SqlDbType.NVarChar);

            //userParam.Value = UserSession.Current.User.Username.ToUpper();
            //orgParam.Value = this.ddlKPIOrg.SelectedValue;
            //deptParam.Value = this.ddlKPIDept.SelectedValue;
            //startDateParam.Value = this.txtTuNgay.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
            //endDateParam.Value = this.txtDenNgay.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
            //var value1 = this.eamService.GetDataValue("get_kpi_year_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //var value2 = this.eamService.GetDataValue("get_kpi_quarter_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //var value3 = this.eamService.GetDataValue("get_kpi_month_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //var value4 = this.eamService.GetDataValue("get_kpi_week_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //var value5 = this.eamService.GetDataValue("get_kpi_user_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //var value6 = this.eamService.GetDataValue("get_kpi_range",
            //    new SqlParameter[] {userParam, orgParam, deptParam, startDateParam, endDateParam});

            //this.Gauge1.Pointer.Value = !string.IsNullOrEmpty(value1.ToString()) ? Math.Round(Convert.ToDecimal(value1), 2) : 0;
            //this.Gauge2.Pointer.Value = !string.IsNullOrEmpty(value2.ToString()) ? Math.Round(Convert.ToDecimal(value2), 2) : 0;
            //this.Gauge3.Pointer.Value = !string.IsNullOrEmpty(value3.ToString()) ? Math.Round(Convert.ToDecimal(value3), 2) : 0;
            //this.Gauge4.Pointer.Value = !string.IsNullOrEmpty(value4.ToString()) ? Math.Round(Convert.ToDecimal(value4), 2) : 0;
            //this.Gauge5.Pointer.Value = !string.IsNullOrEmpty(value5.ToString()) ? Math.Round(Convert.ToDecimal(value5), 2) : 0;
            //this.Gauge6.Pointer.Value = !string.IsNullOrEmpty(value6.ToString()) ? Math.Round(Convert.ToDecimal(value6), 2) : 0;

            //this.lbl1.Text = (!string.IsNullOrEmpty(value1.ToString()) ? Math.Round(Convert.ToDecimal(value1), 2).ToString() : "0.00") + "%";
            //this.lbl2.Text = (!string.IsNullOrEmpty(value2.ToString()) ? Math.Round(Convert.ToDecimal(value2), 2).ToString() : "0.00") + "%";
            //this.lbl3.Text = (!string.IsNullOrEmpty(value3.ToString()) ? Math.Round(Convert.ToDecimal(value3), 2).ToString() : "0.00") + "%";
            //this.lbl4.Text = (!string.IsNullOrEmpty(value4.ToString()) ? Math.Round(Convert.ToDecimal(value4), 2).ToString() : "0.00") + "%";
            //this.lbl5.Text = (!string.IsNullOrEmpty(value5.ToString()) ? Math.Round(Convert.ToDecimal(value5), 2).ToString() : "0.00") + "%";
            //this.lbl6.Text = (!string.IsNullOrEmpty(value6.ToString()) ? Math.Round(Convert.ToDecimal(value6), 2).ToString() : "0.00") + "%";
        }

        private void LoadColumnChartData()
        {
            //var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            //var orgParam = new SqlParameter("@org", SqlDbType.NVarChar);
            //var deptParam = new SqlParameter("@dept", SqlDbType.NVarChar);
            //userParam.Value = UserSession.Current.User.Username.ToUpper();
            //orgParam.Value = this.ddlOrganization.SelectedValue;
            //deptParam.Value = this.ddlDepartment.SelectedValue;
            //DataSet ds;
            //List<ColumnChartDto> chartDataList;
            //var dataList = new List<PmPlanDto>();
            //switch (this.ddlColumnChart.SelectedValue)
            //{
            //    case "1":
            //        this.ColumnChart.ChartTitle.Text = "Thống kê kế hoạch bảo trì, kiểm tra theo định kỳ các bộ phận";
            //        ds = this.eamService.GetDataSet("get_pm_plan", new SqlParameter[] {userParam, orgParam, deptParam});
            //        if (ds != null)
            //        {
            //            dataList = this.eamService.CreateListFromTable<PmPlanDto>(ds.Tables[0]);
            //        }
            //        break;
            //    case "2":
            //        this.ColumnChart.ChartTitle.Text = "Thống kê khối lượng hoàn thành bảo trì, kiểm tra định kỳ các bộ phận";
            //        ds = this.eamService.GetDataSet("get_pm_plan_Completed", new SqlParameter[] { userParam });
            //        if (ds != null)
            //        {
            //            dataList = this.eamService.CreateListFromTable<PmPlanDto>(ds.Tables[0]);
            //        }
            //        break;
            //    case "3":
            //        this.ColumnChart.ChartTitle.Text = "Thống kê khối lượng sửa chữa phát sinh ngoài kế hoạch";
            //        ds = this.eamService.GetDataSet("get_cm_plan", new SqlParameter[] { userParam });
            //        if (ds != null)
            //        {
            //            dataList = this.eamService.CreateListFromTable<PmPlanDto>(ds.Tables[0]);
            //        }
            //        break;
            //    case "4":
            //        this.ColumnChart.ChartTitle.Text = "Thống kê khối lượng hoàn thành sửa chữa phát sinh ngoài kế hoạch";
            //        ds = this.eamService.GetDataSet("get_cm_plan_Completed", new SqlParameter[] { userParam });
            //        if (ds != null)
            //        {
            //            dataList = this.eamService.CreateListFromTable<PmPlanDto>(ds.Tables[0]);
            //        }
            //        break;
            //}

            //chartDataList = new List<ColumnChartDto>()
            //{
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 1)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 2)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 3)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 4)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 5)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 6)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 7)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 8)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 9)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 10)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 11)},
            //    new ColumnChartDto(){TotalValue = dataList.Count(t => t.THANG == 12)},
            //};
            //this.ColumnChart.DataSource = chartDataList;
            //this.ColumnChart.DataBind();
        }

        private void LoadFolderData()
        {
            this.lbFolder.Items.Clear();
            //var userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            //var orgParam = new SqlParameter("@DonVi", SqlDbType.NVarChar);
            //var deptParam = new SqlParameter("@BoPhan", SqlDbType.NVarChar);

            //userParam.Value = UserSession.Current.User.Username.ToUpper();
            //orgParam.Value = this.ddlOrganization.SelectedValue;
            //deptParam.Value = this.ddlDepartment.SelectedValue;
            //switch (this.ddlFolder.SelectedValue)
            //{
            //    case "1":
            //        var workOfYear = this.eamService.GetDataValue("get_pm_plan_year_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var workOfYearItem = new RadListBoxItem("Danh mục công việc bảo trì trong năm", workOfYear != null ? workOfYear.ToString() : "0");
            //        workOfYearItem.Attributes.Add("Color", "icon1");
            //        this.lbFolder.Items.Add(workOfYearItem);

            //        var workOfQuater = this.eamService.GetDataValue("get_pm_plan_quater_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var workOfQuaterItem = new RadListBoxItem("Danh mục công việc bảo trì trong Quý", workOfQuater != null ? workOfQuater.ToString() : "0");
            //        workOfQuaterItem.Attributes.Add("Color", "icon2");
            //        this.lbFolder.Items.Add(workOfQuaterItem);

            //        var workOfMonth = this.eamService.GetDataValue("get_pm_plan_month_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var workOfMonthItem = new RadListBoxItem("Danh mục công việc bảo trì trong tháng", workOfMonth != null ? workOfMonth.ToString() : "0");
            //        workOfMonthItem.Attributes.Add("Color", "icon3");
            //        this.lbFolder.Items.Add(workOfMonthItem);

            //        var workOfWeek = this.eamService.GetDataValue("get_pm_plan_week_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var workOfWeekItem = new RadListBoxItem("Danh mục công việc bảo trì trong tuần", workOfWeek != null ? workOfWeek.ToString() : "0");
            //        workOfWeekItem.Attributes.Add("Color", "icon4");
            //        this.lbFolder.Items.Add(workOfWeekItem);

            //        var cmRequest = this.eamService.GetDataValue("get_cm_request_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var cmRequestItem = new RadListBoxItem("Danh mục yêu cầu sửa chữa cần xem xét trước khi thực hiện", cmRequest != null ? cmRequest.ToString() : "0");
            //        cmRequestItem.Attributes.Add("Color", "icon5");
            //        this.lbFolder.Items.Add(cmRequestItem);

            //        var cmReleased = this.eamService.GetDataValue("get_cm_released_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var cmReleasedItem = new RadListBoxItem("Danh mục yêu cầu sửa chữa cần phê duyệt", cmReleased != null ? cmReleased.ToString() : "0");
            //        cmReleasedItem.Attributes.Add("Color", "icon6");
            //        this.lbFolder.Items.Add(cmReleasedItem);

            //        var woProcessing = this.eamService.GetDataValue("get_WO_processing_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var woProcessingItem = new RadListBoxItem("Danh mục công việc đang thực hiện", woProcessing != null ? woProcessing.ToString() : "0");
            //        woProcessingItem.Attributes.Add("Color", "icon7");
            //        this.lbFolder.Items.Add(woProcessingItem);

            //        var woWaiting = this.eamService.GetDataValue("get_WO_waiting_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var woWaitingItem = new RadListBoxItem("Danh mục công việc chờ vật tư, dịch vụ", woWaiting != null ? woWaiting.ToString() : "0");
            //        woWaitingItem.Attributes.Add("Color", "icon8");
            //        this.lbFolder.Items.Add(woWaitingItem);

            //        var woWaitingReview = this.eamService.GetDataValue("get_WO_waiting_Review_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var woWaitingReviewItem = new RadListBoxItem("Danh mục công việc chờ xem xét sau thực hiện", woWaitingReview != null ? woWaitingReview.ToString() : "0");
            //        woWaitingReviewItem.Attributes.Add("Color", "icon9");
            //        this.lbFolder.Items.Add(woWaitingReviewItem);

            //        var woWaitingConfirm = this.eamService.GetDataValue("get_WO_waiting_Confirm_r2", new SqlParameter[] { userParam, orgParam, deptParam });
            //        var woWaitingConfirmItem = new RadListBoxItem("Danh mục công việc cần xác nhận nghiệm thu sau thực hiện", woWaitingConfirm != null ? woWaitingConfirm.ToString() : "0");
            //        woWaitingConfirmItem.Attributes.Add("Color", "icon10");
            //        this.lbFolder.Items.Add(woWaitingConfirmItem);
            //        break;
            //    case "2":
            //        var req = this.eamService.GetDataValue("get_req_r2", new SqlParameter[] { userParam, orgParam });
            //        var reqItem = new RadListBoxItem("Danh mục yêu cầu mua sắm", req != null ? req.ToString() : "0");
            //        reqItem.Attributes.Add("Color", "icon1");
            //        this.lbFolder.Items.Add(reqItem);

            //        var reqRA = this.eamService.GetDataValue("get_req_RA_r2", new SqlParameter[] { userParam, orgParam });
            //        var reqRAItem = new RadListBoxItem("Danh mục các yêu cầu mua sắm chờ phê duyệt", reqRA != null ? reqRA.ToString() : "0");
            //        reqRAItem.Attributes.Add("Color", "icon2");
            //        this.lbFolder.Items.Add(reqRAItem);

            //        var reqA = this.eamService.GetDataValue("get_req_A_r2", new SqlParameter[] { userParam, orgParam });
            //        var reqAItem = new RadListBoxItem("Danh mục yêu cầu mua sắm đã phê duyệt", reqA != null ? reqA.ToString() : "0");
            //        reqAItem.Attributes.Add("Color", "icon3");
            //        this.lbFolder.Items.Add(reqAItem);

            //        var ordRA = this.eamService.GetDataValue("get_ord_ra_r2", new SqlParameter[] { userParam, orgParam });
            //        var ordRAItem = new RadListBoxItem("Danh mục đơn hàng mua sắm chờ phê duyệt", ordRA != null ? ordRA.ToString() : "0");
            //        ordRAItem.Attributes.Add("Color", "icon4");
            //        this.lbFolder.Items.Add(ordRAItem);

            //        var ordA = this.eamService.GetDataValue("get_ord_a_r2", new SqlParameter[] { userParam, orgParam });
            //        var ordAItem = new RadListBoxItem("Danh mục đơn hàng mua sắm đã phê duyệt", ordA != null ? ordA.ToString() : "0");
            //        ordAItem.Attributes.Add("Color", "icon5");
            //        this.lbFolder.Items.Add(ordAItem);

            //        var poReceipt = this.eamService.GetDataValue("get_PO_receipts_r2", new SqlParameter[] { userParam, orgParam });
            //        var poReceiptItem = new RadListBoxItem("Danh mục phiếu nhập kho", poReceipt != null ? poReceipt.ToString() : "0");
            //        poReceiptItem.Attributes.Add("Color", "icon6");
            //        this.lbFolder.Items.Add(poReceiptItem);

            //        var pick = this.eamService.GetDataValue("get_pick_r2", new SqlParameter[] { userParam, orgParam });
            //        var pickItem = new RadListBoxItem("Danh mục phiếu yêu cầu sử dụng", pick != null ? pick.ToString() : "0");
            //        pickItem.Attributes.Add("Color", "icon7");
            //        this.lbFolder.Items.Add(pickItem);

            //        var pickRA = this.eamService.GetDataValue("get_pick_ra_r2", new SqlParameter[] { userParam, orgParam });
            //        var pickRAItem = new RadListBoxItem("Danh mục phiếu yêu cầu sử dụng chờ phê duyệt", pickRA != null ? pickRA.ToString() : "0");
            //        pickRAItem.Attributes.Add("Color", "icon8");
            //        this.lbFolder.Items.Add(pickRAItem);

            //        var pickA = this.eamService.GetDataValue("get_pick_a_r2", new SqlParameter[] { userParam, orgParam });
            //        var pickAItem = new RadListBoxItem("Danh mục phiếu yêu cầu sử dụng đã phê duyệt", pickA != null ? pickA.ToString() : "0");
            //        pickAItem.Attributes.Add("Color", "icon9");
            //        this.lbFolder.Items.Add(pickAItem);

            //        var pickC = this.eamService.GetDataValue("get_pick_c_r2", new SqlParameter[] { userParam, orgParam });
            //        var pickCItem = new RadListBoxItem("Danh mục phiếu yêu cầu sử dụng đã xuất kho", pickC != null ? pickC.ToString() : "0");
            //        pickCItem.Attributes.Add("Color", "icon10");
            //        this.lbFolder.Items.Add(pickCItem);
            //        break;
            //}

            this.lbFolder.DataBind();
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RefreshProgressReport")
            {
            }
            
        }

        protected void ddlFolder_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadFolderData();
        }

        protected void ddlColumnChart_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadColumnChartData();
        }

        protected void btnRefreshChart_OnClick(object sender, ImageClickEventArgs e)
        {
            this.LoadColumnChartData();
        }

        protected void btnRefreshInbox_OnClick(object sender, ImageClickEventArgs e)
        {
            this.LoadFolderData();
        }

        protected void btnRefreshKPI_OnClick(object sender, ImageClickEventArgs e)
        {
            this.LoadGaugeData();
        }

        protected void ddlKPIOrg_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.LoadGaugeData();
        }

        protected void txtDenNgay_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            this.LoadGaugeData();
        }
    }
}

