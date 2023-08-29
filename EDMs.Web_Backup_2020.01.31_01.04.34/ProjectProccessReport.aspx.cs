// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using Telerik.Web.UI.Calendar;

namespace EDMs.Web
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ProjectProccessReport : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly MaterialRequisitionService mrService = new MaterialRequisitionService();
        private readonly WorkRequestService wrService = new WorkRequestService();
        private readonly TrackingECRService ecrService = new TrackingECRService();
        private readonly TrackingMOCService mocService = new TrackingMOCService();
        private readonly TrackingShutdownReportService srService = new TrackingShutdownReportService();
        private readonly TrackingBreakdownReportService brService = new TrackingBreakdownReportService();

        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();

        private readonly TrackingProductionMeetingService pmService = new TrackingProductionMeetingService();
        private readonly TrackingMorningCallService morningCallService = new TrackingMorningCallService();
        private readonly TrackingWCRService wcrService = new TrackingWCRService();
        private readonly TrackingPunchService plService=  new TrackingPunchService();
        private readonly TrackingProcedureService procedureService = new TrackingProcedureService();
        private readonly TrackingGeneralWorkingService generalWorkingService = new TrackingGeneralWorkingService();

        private DropDownList ddlProject
        {
            get { return (DropDownList)this.CustomerMenu.Items[6].FindControl("ddlProject"); }
        }

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

            Session.Add("SelectedMainMenu", "Home");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;

            if (!Page.IsPostBack)
            {
                var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtFromDate");
                var txtToDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtToDate");

                txtToDate.SelectedDate = DateTime.Now;
                txtFromDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);

                this.lblToDate.Value = txtToDate.SelectedDate != null ? txtToDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
                this.lblFromDate.Value = txtFromDate.SelectedDate != null ? txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
                Session.Add("FromDate", txtFromDate.SelectedDate);
                Session.Add("ToDate", txtToDate.SelectedDate);


                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[1].Visible = false;
                    this.CustomerMenu.Items[2].Visible = false;
                    this.CustomerMenu.Items[3].Visible = false;
                    this.CustomerMenu.Items[4].Visible = false;
                }

                this.InitData();
                this.LoadProcessReport();
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RefreshProgressReport")
            {
                this.LoadProcessReport();
            }
            
        }

        private void LoadProcessReport()
        {
            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);

                var fromDate = (DateTime?)Session["FromDate"];
                var toDate = (DateTime?)Session["ToDate"];

                // Binding data for General Woring
                var gwList = this.generalWorkingService.GetAllByProject(projectId);
                var gwOpenList = gwList.Where(t => t.Status == "").ToList();
                var gwClosedList = gwList.Where(t => t.Status == "").ToList();
                var gwRoutineList = gwList.Where(t => t.Status == "").ToList();
                var gwCancelList = gwList.Where(t => t.Status == "").ToList();

                this.lblgwClosed.Text = gwClosedList.Count.ToString();
                this.lblgwOpen.Text = gwOpenList.Count.ToString();
                this.lblgwRoutine.Text = gwRoutineList.Count.ToString();
                this.lblgwCancel.Text = gwCancelList.Count.ToString();

                ((BarSeries)this.GWChart.PlotArea.Series[0]).SeriesItems[0].Y = gwList.Count > 0 ? (decimal?)Math.Round(((double)gwClosedList.Count / (double)gwList.Count) * 100, 2) : 0;
                ((BarSeries)this.GWChart.PlotArea.Series[1]).SeriesItems[0].Y = gwList.Count > 0 ? (decimal?)Math.Round(((double)gwOpenList.Count / (double)gwList.Count) * 100, 2) : 0;
                ((BarSeries)this.GWChart.PlotArea.Series[2]).SeriesItems[0].Y = gwList.Count > 0 ? (decimal?)Math.Round(((double)gwRoutineList.Count / (double)gwList.Count) * 100, 2) : 0;
                ((BarSeries)this.GWChart.PlotArea.Series[3]).SeriesItems[0].Y = gwList.Count > 0 ? (decimal?)Math.Round(((double)gwCancelList.Count / (double)gwList.Count) * 100, 2) : 0;

                // ---------------------------------------------------------------------------------------------------------------------

                // Binding data for procedure List
                var procedureList = this.procedureService.GetAllByProject(projectId);
                var procedureOpenList = procedureList.Where(t => t.Status == "Open").ToList();
                var procedureClosedList = procedureList.Where(t => t.Status == "Closed").ToList();
                var procedureCancelList = procedureList.Where(t => t.Status == "Cancel").ToList();

                this.lblProcedureClosed.Text = procedureClosedList.Count.ToString();
                this.lblProcedureOpen.Text = procedureOpenList.Count.ToString();
                this.lblProcedureCancel.Text = procedureCancelList.Count.ToString();

                ((BarSeries)this.ProcedureChart.PlotArea.Series[0]).SeriesItems[0].Y = procedureList.Count > 0 ? (decimal?)Math.Round(((double)procedureClosedList.Count / (double)procedureList.Count) * 100, 2) : 0;
                ((BarSeries)this.ProcedureChart.PlotArea.Series[1]).SeriesItems[0].Y = procedureList.Count > 0 ? (decimal?)Math.Round(((double)procedureOpenList.Count / (double)procedureList.Count) * 100, 2) : 0;
                ((BarSeries)this.ProcedureChart.PlotArea.Series[2]).SeriesItems[0].Y = procedureList.Count > 0 ? (decimal?)Math.Round(((double)procedureCancelList.Count / (double)procedureList.Count) * 100, 2) : 0;
                // ---------------------------------------------------------------------------------------------------------------------


                // Binding data for Punch List
                var punchList = this.plService.GetAllByProject(projectId);
                var punchACList = punchList.Where(t => t.Status == "AC").ToList();
                var punchBCList = punchList.Where(t => t.Status == "BC").ToList();
                var punchCancelList = punchList.Where(t => t.Status == "Cancel").ToList();

                this.lblpunchAC.Text = punchACList.Count.ToString();
                this.lblpunchBC.Text = punchBCList.Count.ToString();
                this.lblpunchCancel.Text = punchCancelList.Count.ToString();

                ((BarSeries)this.PunchChart.PlotArea.Series[0]).SeriesItems[0].Y = punchList.Count > 0 ? (decimal?)Math.Round(((double)punchACList.Count / (double)punchList.Count) * 100, 2) : 0;
                ((BarSeries)this.PunchChart.PlotArea.Series[1]).SeriesItems[0].Y = punchList.Count > 0 ? (decimal?)Math.Round(((double)punchBCList.Count / (double)punchList.Count) * 100, 2) : 0;
                ((BarSeries)this.PunchChart.PlotArea.Series[2]).SeriesItems[0].Y = punchList.Count > 0 ? (decimal?)Math.Round(((double)punchCancelList.Count / (double)punchList.Count) * 100, 2) : 0;
                // ---------------------------------------------------------------------------------------------------------------------


                // Binding data for WCR
                var wcrList = this.wcrService.GetAllByProject(projectId);
                var wcrOpenList = wcrList.Where(t => t.Status == "Open").ToList();
                var wcrClosedList = wcrList.Where(t => t.Status == "Closed").ToList();
                var wcrCancelList = wcrList.Where(t => t.Status == "Cancel").ToList();

                this.lblwcrClosed.Text = wcrClosedList.Count.ToString();
                this.lblwcrOpen.Text = wcrOpenList.Count.ToString();
                this.lblwcrCancel.Text = wcrCancelList.Count.ToString();

                ((BarSeries)this.WCRChart.PlotArea.Series[0]).SeriesItems[0].Y = wcrList.Count > 0 ? (decimal?)Math.Round(((double)wcrClosedList.Count / (double)wcrList.Count) * 100, 2) : 0;
                ((BarSeries)this.WCRChart.PlotArea.Series[1]).SeriesItems[0].Y = wcrList.Count > 0 ? (decimal?)Math.Round(((double)wcrOpenList.Count / (double)wcrList.Count) * 100, 2) : 0;
                ((BarSeries)this.WCRChart.PlotArea.Series[2]).SeriesItems[0].Y = wcrList.Count > 0 ? (decimal?)Math.Round(((double)wcrCancelList.Count / (double)wcrList.Count) * 100, 2) : 0;
                // ---------------------------------------------------------------------------------------------------------------------

                // Binding data for Morning Call
                var mcList = this.morningCallService.GetAllByProject(projectId);
                var mcOpenList = mcList.Where(t => t.StatusId == 2).ToList();
                var mcClosedList = mcList.Where(t => t.StatusId == 1).ToList();
                var mcHoldList = mcList.Where(t => t.StatusId == 3).ToList();
                var mcCancelList = mcList.Where(t => t.StatusId == 4).ToList();

                this.lblmcClosed.Text = mcClosedList.Count.ToString();
                this.lblmcOpen.Text = mcOpenList.Count.ToString();
                this.lblmcHold.Text = mcHoldList.Count.ToString();
                this.lblmcCancel.Text = mcCancelList.Count.ToString();

                ((BarSeries)this.MCChart.PlotArea.Series[0]).SeriesItems[0].Y = mcList.Count > 0 ? (decimal?)Math.Round(((double)mcClosedList.Count / (double)mcList.Count) * 100, 2) : 0;
                ((BarSeries)this.MCChart.PlotArea.Series[1]).SeriesItems[0].Y = mcList.Count > 0 ? (decimal?)Math.Round(((double)mcOpenList.Count / (double)mcList.Count) * 100, 2) : 0;
                ((BarSeries)this.MCChart.PlotArea.Series[2]).SeriesItems[0].Y = mcList.Count > 0 ? (decimal?)Math.Round(((double)mcHoldList.Count / (double)mcList.Count) * 100, 2) : 0;
                ((BarSeries)this.MCChart.PlotArea.Series[3]).SeriesItems[0].Y = mcList.Count > 0 ? (decimal?)Math.Round(((double)mcCancelList.Count / (double)mcList.Count) * 100, 2) : 0;
                // ---------------------------------------------------------------------------------------------------------------------

                // Binding data for Production Meeting
                var pmList = this.pmService.GetAllByProject(projectId);
                var pmOpenList = pmList.Where(t => t.StatusId == 2).ToList();
                var pmClosedList = pmList.Where(t => t.StatusId == 1).ToList();
                var pmContinousList = pmList.Where(t => t.StatusId == 3).ToList();
                var pmCancelList = pmList.Where(t => t.StatusId == 4).ToList();

                this.lblPMClosed.Text = pmClosedList.Count.ToString();
                this.lblPMOpen.Text = pmOpenList.Count.ToString();
                this.lblPMContinous.Text = pmContinousList.Count.ToString();
                this.lblPMCancel.Text = pmCancelList.Count.ToString();

                ((BarSeries)this.PMChart.PlotArea.Series[0]).SeriesItems[0].Y = pmList.Count > 0 ? (decimal?)Math.Round(((double)pmClosedList.Count / (double)pmList.Count) * 100, 2) : 0;
                ((BarSeries)this.PMChart.PlotArea.Series[1]).SeriesItems[0].Y = pmList.Count > 0 ? (decimal?)Math.Round(((double)pmOpenList.Count / (double)pmList.Count) * 100, 2) : 0;
                ((BarSeries)this.PMChart.PlotArea.Series[2]).SeriesItems[0].Y = pmList.Count > 0 ? (decimal?)Math.Round(((double)pmContinousList.Count / (double)pmList.Count) * 100, 2) : 0;
                ((BarSeries)this.PMChart.PlotArea.Series[3]).SeriesItems[0].Y = pmList.Count > 0 ? (decimal?)Math.Round(((double)pmCancelList.Count / (double)pmList.Count) * 100, 2) : 0;

                // ---------------------------------------------------------------------------------------------------------------------

                // Binding data for MRChart
                var mrList = this.mrService.GetAll(projectId, string.Empty).Where(t => (fromDate == null || t.CreatedByDate >= fromDate)
                                                                                && (toDate == null || t.CreatedByDate < toDate.Value.AddDays(1))).ToList();
                var mroverDueTask = this.objAssignedUserService.GetAllOverDueTask("Material Requisition").Select(t => t.ObjectID).ToList();
                var mroverdueList = mrList.Where(t => mroverDueTask.Contains(t.ID)).ToList();
                var mrpendingList = mrList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                var mrcompleteList = mrList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                var mrcancelList = mrList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                this.lblMRPending.Text = mrpendingList.Count.ToString();
                this.lblMROverdue.Text = mroverdueList.Count.ToString();
                this.lblMRCancel.Text = mrcancelList.Count.ToString();
                this.lblMRCompleted.Text = mrcompleteList.Count.ToString();

                ((BarSeries)this.MRChart.PlotArea.Series[0]).SeriesItems[0].Y = mrList.Count > 0 ? (decimal?)Math.Round(((double)mrpendingList.Count / (double)mrList.Count) * 100, 2) : 0;
                ((BarSeries)this.MRChart.PlotArea.Series[1]).SeriesItems[0].Y = mrList.Count > 0 ? (decimal?)Math.Round(((double)mroverdueList.Count / (double)mrList.Count) * 100, 2) : 0;
                ((BarSeries)this.MRChart.PlotArea.Series[2]).SeriesItems[0].Y = mrList.Count > 0 ? (decimal?)Math.Round(((double)mrcancelList.Count / (double)mrList.Count) * 100, 2) : 0;
                ((BarSeries)this.MRChart.PlotArea.Series[3]).SeriesItems[0].Y = mrList.Count > 0 ? (decimal?)Math.Round(((double)mrcompleteList.Count / (double)mrList.Count) * 100, 2) : 0;
                // -------------------------------------------------------------------------------------------------------------------------

                // Binding data for WRChart
                var wrList = this.wrService.GetAll(projectId, string.Empty).Where(t => (fromDate == null || t.RaisedDate >= fromDate)
                                                                                && (toDate == null || t.RaisedDate < toDate.Value.AddDays(1))).ToList();
                var wroverDueTask = this.objAssignedUserService.GetAllOverDueTask("Work Request").Select(t => t.ObjectID).ToList();
                var wroverdueList = wrList.Where(t => wroverDueTask.Contains(t.ID)).ToList();
                var wrpendingList = wrList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                var wrcompleteList = wrList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                var wrcancelList = wrList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                this.lblWRPending.Text = wrpendingList.Count.ToString();
                this.lblWROverdue.Text = wroverdueList.Count.ToString();
                this.lblWRCancel.Text = wrcancelList.Count.ToString();
                this.lblWRCompleted.Text = wrcompleteList.Count.ToString();

                ((BarSeries)this.WRChart.PlotArea.Series[0]).SeriesItems[0].Y = wrList.Count > 0 ? (decimal?)Math.Round(((double)wrpendingList.Count / (double)wrList.Count) * 100, 2) : 0;
                ((BarSeries)this.WRChart.PlotArea.Series[1]).SeriesItems[0].Y = wrList.Count > 0 ? (decimal?)Math.Round(((double)wroverdueList.Count / (double)wrList.Count) * 100, 2) : 0;
                ((BarSeries)this.WRChart.PlotArea.Series[2]).SeriesItems[0].Y = wrList.Count > 0 ? (decimal?)Math.Round(((double)wrcancelList.Count / (double)wrList.Count) * 100, 2) : 0;
                ((BarSeries)this.WRChart.PlotArea.Series[3]).SeriesItems[0].Y = wrList.Count > 0 ? (decimal?)Math.Round(((double)wrcompleteList.Count / (double)wrList.Count) * 100, 2) : 0;
                // -------------------------------------------------------------------------------------------------------------------------

                // Binding data for ECRChart
                var ecrList = this.ecrService.GetAll(projectId, string.Empty).Where(t => (fromDate == null || t.CreatedDate >= fromDate)
                                                                                && (toDate == null || t.CreatedDate < toDate.Value.AddDays(1))).ToList();
                var ecroverDueTask = this.objAssignedUserService.GetAllOverDueTask("ECR").Select(t => t.ObjectID).ToList();
                var ecroverdueList = ecrList.Where(t => ecroverDueTask.Contains(t.ID)).ToList();
                var ecrpendingList = ecrList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                var ecrcompleteList = ecrList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                var ecrcancelList = ecrList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                this.lblECRPending.Text = ecrpendingList.Count.ToString();
                this.lblECROverdue.Text = ecroverdueList.Count.ToString();
                this.lblECRCancel.Text = ecrcancelList.Count.ToString();
                this.lblECRCompleted.Text = ecrcompleteList.Count.ToString();

                ((BarSeries)this.ECRChart.PlotArea.Series[0]).SeriesItems[0].Y = ecrList.Count > 0 ? (decimal?)Math.Round(((double)ecrpendingList.Count / (double)ecrList.Count) * 100, 2) : 0;
                ((BarSeries)this.ECRChart.PlotArea.Series[1]).SeriesItems[0].Y = ecrList.Count > 0 ? (decimal?)Math.Round(((double)ecroverdueList.Count / (double)ecrList.Count) * 100, 2) : 0;
                ((BarSeries)this.ECRChart.PlotArea.Series[2]).SeriesItems[0].Y = ecrList.Count > 0 ? (decimal?)Math.Round(((double)ecrcancelList.Count / (double)ecrList.Count) * 100, 2) : 0;
                ((BarSeries)this.ECRChart.PlotArea.Series[3]).SeriesItems[0].Y = ecrList.Count > 0 ? (decimal?)Math.Round(((double)ecrcompleteList.Count / (double)ecrList.Count) * 100, 2) : 0;
                // -------------------------------------------------------------------------------------------------------------------------

                // Binding data for mocChart
                var mocList = this.mocService.GetAll(projectId, string.Empty).Where(t => (fromDate == null || t.CreatedDate >= fromDate)
                                                                                && (toDate == null || t.CreatedDate < toDate.Value.AddDays(1))).ToList();
                var mocoverDueTask = this.objAssignedUserService.GetAllOverDueTask("MOC").Select(t => t.ObjectID).ToList();
                var mocoverdueList = mocList.Where(t => mocoverDueTask.Contains(t.ID)).ToList();
                var mocpendingList = mocList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                var moccompleteList = mocList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                var moccancelList = mocList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                this.lblMOCPending.Text = mocpendingList.Count.ToString();
                this.lblMOCOverdue.Text = mocoverdueList.Count.ToString();
                this.lblMOCCanceled.Text = moccancelList.Count.ToString();
                this.lblMOCCompleted.Text = moccompleteList.Count.ToString();

                ((BarSeries)this.MOCChart.PlotArea.Series[0]).SeriesItems[0].Y = mocList.Count > 0 ? (decimal?)Math.Round(((double)mocpendingList.Count / (double)mocList.Count) * 100, 2) : 0;
                ((BarSeries)this.MOCChart.PlotArea.Series[1]).SeriesItems[0].Y = mocList.Count > 0 ? (decimal?)Math.Round(((double)mocoverdueList.Count / (double)mocList.Count) * 100, 2) : 0;
                ((BarSeries)this.MOCChart.PlotArea.Series[2]).SeriesItems[0].Y = mocList.Count > 0 ? (decimal?)Math.Round(((double)moccancelList.Count / (double)mocList.Count) * 100, 2) : 0;
                ((BarSeries)this.MOCChart.PlotArea.Series[3]).SeriesItems[0].Y = mocList.Count > 0 ? (decimal?)Math.Round(((double)moccompleteList.Count / (double)mocList.Count) * 100, 2) : 0;
                // -------------------------------------------------------------------------------------------------------------------------


                // Binding data for brChart
                var brList = this.brService.GetAll(projectId, string.Empty).Where(t => (fromDate == null || t.CreatedDate >= fromDate)
                                                                                && (toDate == null || t.CreatedDate < toDate.Value.AddDays(1))).ToList();
                var broverDueTask = this.objAssignedUserService.GetAllOverDueTask("Breakdown Report").Select(t => t.ObjectID).ToList();
                var broverdueList = brList.Where(t => broverDueTask.Contains(t.ID)).ToList();
                var brpendingList = brList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                var brcompleteList = brList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                var brcancelList = brList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                this.lblBRPending.Text = brpendingList.Count.ToString();
                this.lblBROverdue.Text = broverdueList.Count.ToString();
                this.lblBRCanceled.Text = brcancelList.Count.ToString();
                this.lblBRCompleted.Text = brcompleteList.Count.ToString();

                ((BarSeries)this.BRChart.PlotArea.Series[0]).SeriesItems[0].Y = brList.Count > 0 ? (decimal?)Math.Round(((double)brpendingList.Count / (double)brList.Count) * 100, 2) : 0;
                ((BarSeries)this.BRChart.PlotArea.Series[1]).SeriesItems[0].Y = brList.Count > 0 ? (decimal?)Math.Round(((double)broverdueList.Count / (double)brList.Count) * 100, 2) : 0;
                ((BarSeries)this.BRChart.PlotArea.Series[2]).SeriesItems[0].Y = brList.Count > 0 ? (decimal?)Math.Round(((double)brcancelList.Count / (double)brList.Count) * 100, 2) : 0;
                ((BarSeries)this.BRChart.PlotArea.Series[3]).SeriesItems[0].Y = brList.Count > 0 ? (decimal?)Math.Round(((double)brcompleteList.Count / (double)brList.Count) * 100, 2) : 0;
                // -------------------------------------------------------------------------------------------------------------------------

                // Binding data for srChart
                var srList = this.srService.GetAll(projectId, string.Empty).Where(t => (fromDate == null || t.CreatedDate >= fromDate)
                                                                                && (toDate == null || t.CreatedDate < toDate.Value.AddDays(1))).ToList();
                var sroverDueTask = this.objAssignedUserService.GetAllOverDueTask("Shutdown Report").Select(t => t.ObjectID).ToList();
                var sroverdueList = srList.Where(t => sroverDueTask.Contains(t.ID)).ToList();
                var srpendingList = srList.Where(t => t.IsInWFProcess.GetValueOrDefault()).ToList();
                var srcompleteList = srList.Where(t => t.IsWFComplete.GetValueOrDefault() || t.IsCompleteFinal.GetValueOrDefault()).ToList();
                var srcancelList = srList.Where(t => t.IsCancel.GetValueOrDefault()).ToList();
                this.lblSRPending.Text = srpendingList.Count.ToString();
                this.lblSROverdue.Text = sroverdueList.Count.ToString();
                this.lblSRCanceled.Text = srcancelList.Count.ToString();
                this.lblSRCompleted.Text = srcompleteList.Count.ToString();

                ((BarSeries)this.SRChart.PlotArea.Series[0]).SeriesItems[0].Y = srList.Count > 0 ? (decimal?)Math.Round(((double)srpendingList.Count / (double)srList.Count) * 100, 2) : 0;
                ((BarSeries)this.SRChart.PlotArea.Series[1]).SeriesItems[0].Y = srList.Count > 0 ? (decimal?)Math.Round(((double)sroverdueList.Count / (double)srList.Count) * 100, 2) : 0;
                ((BarSeries)this.SRChart.PlotArea.Series[2]).SeriesItems[0].Y = srList.Count > 0 ? (decimal?)Math.Round(((double)srcancelList.Count / (double)srList.Count) * 100, 2) : 0;
                ((BarSeries)this.SRChart.PlotArea.Series[3]).SeriesItems[0].Y = srList.Count > 0 ? (decimal?)Math.Round(((double)srcompleteList.Count / (double)srList.Count) * 100, 2) : 0;
                // -------------------------------------------------------------------------------------------------------------------------
            }
        }

        private void InitData()
        {
            this.lblCurrentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            var projectList = this.scopeProjectService.GetAll().OrderBy(t => t.Name).ToList();
            if (UserSession.Current.User.LocationId == 2)
            {
                projectList = projectList.Where(t => t.ID == UserSession.Current.User.ProjectId.GetValueOrDefault()).ToList();
            }

            projectList.Insert(0, new ScopeProject() { ID = 0, Name = "For All Project" });
            var project = projectList.ToList();
            this.ddlProject.DataSource = project;
            this.ddlProject.DataTextField = "Name";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            if (projectList.Count > 1)
            {
                this.ddlProject.SelectedIndex = 1;
            }

            this.ProjectId.Value = this.ddlProject.SelectedValue;
            this.ProjectName.Value = this.ddlProject.SelectedItem.Text;
        }

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }

                //Mo file de doc
                var fs = new FileStream(strFileName, FileMode.Open);
                var streamLength = Convert.ToInt32(fs.Length);
                var data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected void ddlProject_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ProjectId.Value = this.ddlProject.SelectedValue;
            this.ProjectName.Value = this.ddlProject.SelectedItem.Text;
            this.LoadProcessReport();
        }

        protected void ddlRecoveryPlan_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadProcessReport();
        }

        protected void txtFromDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtFromDate");
            var txtToDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtToDate");

            this.lblToDate.Value = txtToDate.SelectedDate != null ? txtToDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
            this.lblFromDate.Value = txtFromDate.SelectedDate != null ? txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
            Session.Add("FromDate", txtFromDate.SelectedDate);
            Session.Add("ToDate", txtToDate.SelectedDate);

            this.LoadProcessReport();
        }
    }
}

