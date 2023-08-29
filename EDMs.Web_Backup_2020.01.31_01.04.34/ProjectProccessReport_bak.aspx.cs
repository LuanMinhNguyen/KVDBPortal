// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Scope;
using EDMs.Data.Entities;

namespace EDMs.Web
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;


    using EDMs.Business.Services.Library;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ProjectProccessReport_bak : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly ProcessActualService processActualService = new ProcessActualService();
        private readonly ProcessPlanedService processPlanedService = new ProcessPlanedService();
        private readonly ProcessRecoveryPlanedService processRecoveryPlanedService = new ProcessRecoveryPlanedService();

        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();


        private DropDownList ddlProject
        {
            get { return (DropDownList)this.CustomerMenu.Items[6].FindControl("ddlProject"); }
        }

        private DropDownList ddlRecoveryPlan
        {
            get { return (DropDownList)this.CustomerMenu.Items[8].FindControl("ddlRecoveryPlan"); }
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
            else if (e.Argument == "PrintProgress")
            {
                var projectId = this.ddlProject.SelectedItem != null
                    ? Convert.ToInt32(this.ddlProject.SelectedValue)
                    : 0;
                var processReportList = new List<ProcessReport>();

                var projectObj = this.scopeProjectService.GetById(projectId);
                if (projectObj != null && projectObj.StartDate != null && projectObj.EndDate != null)
                {
                    var indexNo = this.ddlRecoveryPlan.SelectedItem != null
                                        ? Convert.ToInt32(this.ddlRecoveryPlan.SelectedValue)
                                        : 0;
                    var processRecoveryPlanedList = this.processRecoveryPlanedService.GetAllByProject(projectId, indexNo);

                    var listDiscipline = this.disciplineService.GetAllDisciplineOfProject(projectObj.ID).OrderBy(t => t.ID).ToList();
                    var dateList = new List<DateTime>();

                    var filePath = Server.MapPath("Exports") + @"\";
                    var workbook = new Workbook();
                    if (processRecoveryPlanedList.Any())
                    {
                        workbook.Open(filePath + @"Template\ProgressRecoveryTemplate_New.xls");
                    }
                    else
                    {
                        workbook.Open(filePath + @"Template\ProgressTemplate_New.xls");
                    }
                    
                    var wsProgress = workbook.Worksheets[0];

                    wsProgress.Cells["C1"].PutValue("DETAIL ENGINEERING SERVICE FOR " + projectObj.Description);
                    wsProgress.Cells["C2"].PutValue("PROJECT PROGRESS (CUT OFF: " + DateTime.Now.ToString("dd/MM/yyyy") + ")");

                    var workgroupCount = 0;
                    var startDate = projectObj.StartDate.GetValueOrDefault();
                    //while (startDate.DayOfWeek != DayOfWeek.Monday)
                    //{
                    //    startDate = startDate.AddDays(1);
                    //}

                    var currentMonth = 0;
                    var countMerge = 0;
                    var count = 3;
                    var countNumberCurrentActual = 0;
                    for (var j = startDate;
                            j <= projectObj.EndDate.GetValueOrDefault();
                            j = j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart !=0 ?projectObj.FrequencyForProgressChart.Value : 7))
                    {
                        var processReport = new ProcessReport();
                        processReport.WeekDate = j;
                        processReportList.Add(processReport);

                        if (DateTime.Now > j)
                        {
                            countNumberCurrentActual += 1;
                        }

                        if (currentMonth != j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart !=0 ?projectObj.FrequencyForProgressChart.Value : 7).Month)
                        {
                            wsProgress.Cells[2, count].PutValue(j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart !=0 ?projectObj.FrequencyForProgressChart.Value : 7));
                            currentMonth = j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart !=0 ?projectObj.FrequencyForProgressChart.Value : 7).Month;

                            if (countMerge != 0)
                            {
                                wsProgress.Cells.Merge(2, count - countMerge, 1, countMerge);
                            }

                            countMerge = 1;
                        }
                        else
                        {
                            countMerge += 1;
                        }

                        wsProgress.Cells[3, count].PutValue(count - 2);
                        wsProgress.Cells[4, count].PutValue(j);
                        //wsProgress.Cells[5, count].PutValue(j.AddDays(5));

                        count += 1;
                    }

                    if (listDiscipline.Count > 0)
                    {
                        
                        var wgCount = listDiscipline.Count;
                        // Case have recoverty plan
                        if (processRecoveryPlanedList.Any())
                        {
                            wsProgress.Cells.InsertRows(8, wgCount * 4);
                            for (int i = 0; i < listDiscipline.Count; i++)
                            {
                                var discipline = listDiscipline[i];

                                wsProgress.Cells["B" + (8 + (i * 4))].PutValue(discipline.FullName);
                                wsProgress.Cells["C" + (8 + (i * 4))].PutValue("Planed (%)");
                                wsProgress.Cells["C" + (8 + (i * 4) + 1)].PutValue("Recovery Plan (%)");
                                wsProgress.Cells["C" + (8 + (i * 4) + 2)].PutValue("Actual (%)");
                                wsProgress.Cells["C" + (8 + (i * 4) + 3)].PutValue("+/-");

                                wsProgress.Cells.Merge(7 + (i * 4), 1, 4, 1);

                                var progressPlaned = this.processPlanedService.GetByProjectAndWorkgroup(projectObj.ID, discipline.ID);
                                var progressActual = this.processActualService.GetByProjectAndWorkgroup(projectObj.ID, discipline.ID);
                                var progressRecoverPlan = processRecoveryPlanedList.FirstOrDefault(t => t.WorkgroupId == discipline.ID);

                                if (progressPlaned != null && progressActual != null && progressRecoverPlan != null)
                                {
                                    var planedList = progressPlaned.Planed.Split('$');
                                    var actualList = progressActual.Actual.Split('$');
                                    var recoveryPlanList = progressRecoverPlan.Planed.Split('$');


                                    for (int j = 0; j < planedList.Count(); j++)
                                    {
                                        var planed = !string.IsNullOrEmpty(planedList[j]) ? Convert.ToDouble(planedList[j]) / 100 : 0;
                                        var actual = 0.0;

                                        wsProgress.Cells[7 + (i * 4), j + 3].PutValue(planed);

                                        if (j < processReportList.Count)
                                        {
                                            //processReportList[j].Planed += (planed * workgroup.Weight.GetValueOrDefault()) / 100;
                                            processReportList[j].Planed += planed;
                                        }
                                    }

                                    for (int j = 0; j < actualList.Count(); j++)
                                    {
                                        var actual = !string.IsNullOrEmpty(actualList[j]) ? Convert.ToDouble(actualList[j]) / 100 : 0;
                                        var recoveryPlan = 0.0;

                                        if (j < actualList.Count() && j < countNumberCurrentActual)
                                        {
                                            recoveryPlan = !string.IsNullOrEmpty(recoveryPlanList[j]) ? Convert.ToDouble(recoveryPlanList[j]) / 100 : 0;
                                            if (recoveryPlan != 0.0)
                                            {
                                                wsProgress.Cells[8 + (i * 4), j + 3].PutValue(recoveryPlan);
                                                wsProgress.Cells[9 + (i * 4), j + 3].PutValue(actual);

                                                wsProgress.Cells[10 + (i * 4), j + 3].PutValue(actual - recoveryPlan);
                                            }

                                        }

                                        if (j < processReportList.Count)
                                        {
                                            //processReportList[j].Planed += (planed * workgroup.Weight.GetValueOrDefault()) / 100;
                                            processReportList[j].RecoveryPlan += recoveryPlan;
                                        }

                                        if (j < actualList.Count() && j < processReportList.Count)
                                        {
                                            //processReportList[j].Actual += (Convert.ToDouble(actualList[j])/100 * workgroup.Weight.GetValueOrDefault())/100;
                                            processReportList[j].Actual += Convert.ToDouble(actualList[j]) / 100;
                                        }
                                    }
                                }
                            }

                            wsProgress.Cells.Merge(7 + (wgCount * 4), 1, 3, 1);

                            wsProgress.Cells["B" + (8 + (wgCount * 4))].PutValue("Overall Project");
                            wsProgress.Cells["C" + (8 + (wgCount * 4))].PutValue("Planed (%)");
                            wsProgress.Cells["C" + (8 + (wgCount * 4) + 1)].PutValue("Recovery Plan (%)");
                            wsProgress.Cells["C" + (8 + (wgCount * 4) + 2)].PutValue("Actual (%)");
                            wsProgress.Cells["C" + (8 + (wgCount * 4) + 3)].PutValue("+/-");

                            for (int i = 0; i < processReportList.Count; i++)
                            {
                                if (processReportList[i].Planed != 0.0)
                                {
                                    wsProgress.Cells[7 + (wgCount * 4), i + 3].PutValue(processReportList[i].Planed);
                                }
                                
                                if (processReportList[i].RecoveryPlan != 0.0)
                                {
                                    wsProgress.Cells[8 + (wgCount * 4), i + 3].PutValue(processReportList[i].RecoveryPlan);
                                }

                                if (processReportList[i].Actual != 0.0)
                                {
                                    wsProgress.Cells[9 + (wgCount * 4), i + 3].PutValue(processReportList[i].Actual);
                                }

                                if (i < countNumberCurrentActual)
                                {
                                    wsProgress.Cells[10 + (wgCount * 4), i + 3].PutValue(
                                        processReportList[i].Actual - processReportList[i].RecoveryPlan);
                                }
                            }

                            wsProgress.Cells["A1"].PutValue(0);
                            wsProgress.Cells["A2"].PutValue(wgCount);
                            wsProgress.Cells["A3"].PutValue(processReportList.Count);

                            wsProgress.Cells.Merge(0, 2, 1, count - 2);
                            wsProgress.Cells.Merge(1, 2, 1, count - 2);
                        }
                        // Case don't have Recovery plan
                        else
                        {
                            wsProgress.Cells.InsertRows(8, wgCount * 3);
                            for (int i = 0; i < listDiscipline.Count; i++)
                            {
                                var workgroup = listDiscipline[i];

                                wsProgress.Cells["B" + (8 + (i * 3))].PutValue(workgroup.Name);
                                wsProgress.Cells["C" + (8 + (i * 3))].PutValue("Planed (%)");
                                wsProgress.Cells["C" + (8 + (i * 3) + 1)].PutValue("Actual (%)");
                                wsProgress.Cells["C" + (8 + (i * 3) + 2)].PutValue("+/-");

                                wsProgress.Cells.Merge(7 + (i * 3), 1, 3, 1);

                                var progressPlaned = this.processPlanedService.GetByProjectAndWorkgroup(projectObj.ID, workgroup.ID);
                                var progressActual = this.processActualService.GetByProjectAndWorkgroup(projectObj.ID, workgroup.ID);

                                if (progressPlaned != null && progressActual != null)
                                {
                                    var planedList = progressPlaned.Planed.Split('$');
                                    var actualList = progressActual.Actual.Split('$');
                                    for (int j = 0; j < planedList.Count(); j++)
                                    {
                                        var planed = !string.IsNullOrEmpty(planedList[j]) ? Convert.ToDouble(planedList[j]) / 100 : 0;
                                        var actual = 0.0;

                                        wsProgress.Cells[7 + (i * 3), j + 3].PutValue(planed);
                                        if (j < actualList.Count() && j < countNumberCurrentActual)
                                        {
                                            actual = !string.IsNullOrEmpty(actualList[j]) ? Convert.ToDouble(actualList[j]) / 100 : 0;
                                            if (actual != 0.0)
                                            {
                                                wsProgress.Cells[8 + (i * 3), j + 3].PutValue(actual);
                                                wsProgress.Cells[9 + (i * 3), j + 3].PutValue(actual - planed);
                                            }

                                        }

                                        if (j < processReportList.Count)
                                        {
                                            //processReportList[j].Planed += (planed * workgroup.Weight.GetValueOrDefault()) / 100;
                                            processReportList[j].Planed += planed;
                                        }

                                        if (j < actualList.Count() && j < processReportList.Count)
                                        {
                                            //processReportList[j].Actual += (Convert.ToDouble(actualList[j])/100 * workgroup.Weight.GetValueOrDefault())/100;
                                            processReportList[j].Actual += Convert.ToDouble(actualList[j]) / 100;
                                        }
                                    }
                                }
                            }

                            wsProgress.Cells.Merge(7 + (wgCount * 3), 1, 3, 1);

                            wsProgress.Cells["B" + (8 + (wgCount * 3))].PutValue("Overall Project");
                            wsProgress.Cells["C" + (8 + (wgCount * 3))].PutValue("Planed (%)");
                            wsProgress.Cells["C" + (8 + (wgCount * 3) + 1)].PutValue("Actual (%)");
                            wsProgress.Cells["C" + (8 + (wgCount * 3) + 2)].PutValue("+/-");

                            for (int i = 0; i < processReportList.Count; i++)
                            {
                                wsProgress.Cells[7 + (wgCount * 3), i + 3].PutValue(processReportList[i].Planed);
                                if (processReportList[i].Actual != 0.0)
                                {
                                    wsProgress.Cells[8 + (wgCount * 3), i + 3].PutValue(processReportList[i].Actual);
                                    if (i < countNumberCurrentActual)
                                    {
                                        wsProgress.Cells[9 + (wgCount * 3), i + 3].PutValue(processReportList[i].Actual - processReportList[i].Planed);
                                    }
                                }
                            }

                            wsProgress.Cells["A1"].PutValue(0);
                            wsProgress.Cells["A2"].PutValue(wgCount);
                            wsProgress.Cells["A3"].PutValue(processReportList.Count);

                            wsProgress.Cells.Merge(0, 2, 1, count - 2);
                            wsProgress.Cells.Merge(1, 2, 1, count - 2);
                        }
                        

                    }

                    // Save and export file
                    var filename = (this.ddlProject.SelectedItem != null
                            ? this.ddlProject.SelectedItem.Text + "_"
                            : string.Empty) + "Progress report - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xls";
                    workbook.Save(filePath + filename);
                    this.DownloadByWriteByte(filePath + filename, filename, true);
                }
            }
            else if (e.Argument == "GetLatestData")
            {
                var listDate = new List<DateTime>();
                var projectId = this.ddlProject.SelectedItem != null
                    ? Convert.ToInt32(this.ddlProject.SelectedValue)
                    : 0;
                var projectObj = this.scopeProjectService.GetById(projectId);
                if (projectObj != null && projectObj.StartDate != null && projectObj.EndDate != null)
                {
                    var count = 0;
                    for (var j = projectObj.StartDate.GetValueOrDefault();
                        j < projectObj.EndDate.GetValueOrDefault();
                        j =  j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart !=0 ?projectObj.FrequencyForProgressChart.Value : 7))
                    {
                        if (DateTime.Now > j)
                        {
                            count += 1;
                        }
                        
                        //listDate.Add(j);
                    }

                    var listDiscipline = this.disciplineService.GetAllDisciplineOfProject(projectObj.ID).OrderBy(t => t.ID).ToList();
                    
                    foreach (var workgroup in listDiscipline)
                    {
                        var docList = this.documentPackageService.GetAllByWorkgroup(workgroup.ID)
                                            .OrderBy(t => t.DocNo)
                                            .ToList();

                        double? complete = 0;
                        complete = docList.Aggregate(complete, (current, t) => current + (t.Complete * t.Weight) / 100);

                        var existProgressActual = this.processActualService.GetByProjectAndWorkgroup(projectObj.ID, workgroup.ID);
                        if (existProgressActual != null)
                        {
                            if (existProgressActual.Actual.Split('$').Count() > count)
                            {
                                var actualList = existProgressActual.Actual.Split('$');
                                actualList[count - 1] = Math.Round(complete.GetValueOrDefault(), 2).ToString();
                                var newActual = string.Empty;
                                newActual = actualList.Aggregate(newActual, (current, t) => current + t + "$");

                                newActual = newActual.Substring(0, newActual.Length - 1);
                                existProgressActual.Actual = newActual;

                                this.processActualService.Update(existProgressActual);
                            }
                        }
                    }

                    this.LoadProcessReport();
                }
            }
            else if (e.Argument.Contains("ExportProgress"))
            {
                var type = e.Argument.Split('_')[1];

                var projectId = this.ddlProject.SelectedItem != null
                    ? Convert.ToInt32(this.ddlProject.SelectedValue)
                    : 0;
                var projectObj = this.scopeProjectService.GetById(projectId);
                if (projectObj != null && projectObj.StartDate != null && projectObj.EndDate != null)
                {
                    var listDiscipline = this.disciplineService.GetAllDisciplineOfProject(projectObj.ID).OrderBy(t => t.ID).ToList();

                    var dateList = new List<DateTime>();

                    var filePath = Server.MapPath("Exports") + @"\";
                    var workbook = new Workbook();
                    if (type == "Planed" || type == "RecoveryPlaned")
                    {
                        workbook.Open(filePath + @"Template\ProgressPlanedTemplate.xls");
                    }
                    else
                    {
                        workbook.Open(filePath + @"Template\ProgressActualTemplate.xls");
                    }
                    var wsProgress = workbook.Worksheets[0];
                    wsProgress.Cells["A5"].PutValue(projectId);
                    wsProgress.Cells["A7"].PutValue(type);
                    wsProgress.Cells["C1"].PutValue("DETAIL ENGINEERING SERVICE FOR " + projectObj.Description);
                    var workgroupCount = 0;
                    var startDate = projectObj.StartDate.GetValueOrDefault();
                    //while (startDate.DayOfWeek != DayOfWeek.Monday)
                    //{
                    //    startDate = startDate.AddDays(1);
                    //}

                    //var currentMonth = 0;
                    //var countMerge = 0;
                    var count = 3;
                    for (var j = startDate;
                            j <= projectObj.EndDate.GetValueOrDefault();
                            j = j.AddDays(projectObj.FrequencyForProgressChart != null && projectObj.FrequencyForProgressChart !=0 ?projectObj.FrequencyForProgressChart.Value : 7))
                    {
                        //if (currentMonth != j.AddDays(5).Month)
                        //{
                        //    wsProgress.Cells[2, count].PutValue(j.AddDays(5));
                        //    currentMonth = j.AddDays(5).Month;

                        //    if (countMerge != 0)
                        //    {
                        //        wsProgress.Cells.Merge(2, count - countMerge, 1, countMerge);
                        //    }

                        //    countMerge = 1;
                        //}
                        //else
                        //{
                        //    countMerge += 1;
                        //}

                        wsProgress.Cells[3, count].PutValue("W" + (count - 2));
                        wsProgress.Cells[4, count].PutValue(j);
                        //wsProgress.Cells[5, count].PutValue(j.AddDays(5));

                        count += 1;
                    }

                    if (listDiscipline.Count > 0)
                    {
                        var indexNo = this.processRecoveryPlanedService.GetAllByProject(projectObj.ID).Max(t => t.IndexNo).GetValueOrDefault();

                        wsProgress.Cells.InsertRows(8, listDiscipline.Count);
                        for (int i = 0; i < listDiscipline.Count; i++)
                        {
                            var text = string.Empty;
                            if (type == "Planed")
                            {
                                text = "Planed (%)";
                            }
                            else if (type == "RecoveryPlaned")
                            {
                                text = "Recovery Planed (%)";
                            }
                            else
                            {
                                text = "Actual (%)";
                            }
                            var discipline = listDiscipline[i];

                            wsProgress.Cells["A" + (8 + i)].PutValue(discipline.ID);
                            wsProgress.Cells["B" + (8 + i)].PutValue(discipline.FullName);
                            wsProgress.Cells["C" + (8 + i)].PutValue(text);

                            if (type == "Planed")
                            {
                                var progressPlaned = this.processPlanedService.GetByProjectAndWorkgroup(projectObj.ID, discipline.ID);
                                if (progressPlaned != null)
                                {
                                    var planedList = progressPlaned.Planed.Split('$');
                                    for (int j = 0; j < planedList.Count(); j++)
                                    {
                                        wsProgress.Cells[workgroupCount + 7, j + 3].PutValue(!string.IsNullOrEmpty(planedList[j]) ? Convert.ToDouble(planedList[j]) / 100 : 0);
                                    }
                                }
                            }
                            else if (type =="RecoveryPlaned")
                            {
                                var progressRecoveryPlaned = this.processRecoveryPlanedService.GetByProjectAndWorkgroup(projectObj.ID, discipline.ID, indexNo);
                                if (progressRecoveryPlaned != null)
                                {
                                    var recoveryPlanedList = progressRecoveryPlaned.Planed.Split('$');
                                    for (int j = 0; j < recoveryPlanedList.Count(); j++)
                                    {
                                        wsProgress.Cells[workgroupCount + 7, j + 3].PutValue(!string.IsNullOrEmpty(recoveryPlanedList[j]) ? Convert.ToDouble(recoveryPlanedList[j]) / 100 : 0);
                                    }
                                }
                            }
                            else if (type == "Actual")
                            {
                                var progressActual = this.processActualService.GetByProjectAndWorkgroup(projectObj.ID, discipline.ID);
                                if (progressActual != null)
                                {
                                    var actualList = progressActual.Actual.Split('$');
                                    for (int j = 0; j < actualList.Count(); j++)
                                    {
                                        wsProgress.Cells[workgroupCount + 7, j + 3].PutValue(!string.IsNullOrEmpty(actualList[j]) ? Convert.ToDouble(actualList[j]) / 100 : 0);
                                    }
                                }
                            }

                            workgroupCount += 1;
                        }

                        wsProgress.Cells.Merge(0, 2, 1, count - 2);
                        wsProgress.Cells.Merge(1, 2, 1, count - 2);
                    }

                    
                    // Save and export file
                    var lastFileName = string.Empty;
                    if (type == "Planed")
                    {
                        lastFileName = "ProgressPlaned.xls";
                    }
                    else if (type == "RecoveryPlaned")
                    {
                        lastFileName = "ProgressRecoveryPlaned.xls";
                    }
                    else
                    {
                        lastFileName = "ProgressActual.xls";
                    }

                    var filename = (this.ddlProject.SelectedItem != null
                            ? this.ddlProject.SelectedItem.Text + "$"
                            : string.Empty)
                            + lastFileName;
                    workbook.Save(filePath + filename);
                    this.DownloadByWriteByte(filePath + filename, filename, true);
                }
            }
        }

        private void LoadProcessReport()
        {
            if (this.ddlProject.SelectedItem != null)
            {


                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                var disciplineId = 0;
                var indexNo = this.ddlRecoveryPlan.SelectedItem != null
                    ? Convert.ToInt32(this.ddlRecoveryPlan.SelectedValue)
                    : 0;
                var lineSeriesActual = this.LineChart.PlotArea.Series[1] as LineSeries;
                var lineSeriesPlaned = this.LineChart.PlotArea.Series[0] as LineSeries;
                var lineSeriesRecoveryPlaned = this.LineChart.PlotArea.Series[2] as LineSeries;

                var processReportList = new List<ProcessReport>();
                var temp = new List<ProcessReport>();

                var projectObj = this.scopeProjectService.GetById(projectId);
                var processPlanedList = this.processPlanedService.GetAllByProject(projectId, disciplineId);
                var processActualList = this.processActualService.GetAllByProject(projectId, disciplineId);
                var processRecoveryPlanedList = this.processRecoveryPlanedService.GetAllByProject(projectId, indexNo);

                if (projectObj != null && projectObj.StartDate != null && projectObj.EndDate != null)
                {
                    var countWeekName = 1;
                    for (var i = projectObj.StartDate.GetValueOrDefault();
                        i <= projectObj.EndDate.GetValueOrDefault();
                        i =
                            i.AddDays(projectObj.FrequencyForProgressChart != null &&
                                      projectObj.FrequencyForProgressChart != 0
                                ? projectObj.FrequencyForProgressChart.Value
                                : 7))
                    {
                        var processReport = new ProcessReport();
                        processReport.WeekDate = i;
                        processReport.WeekName = "W" + countWeekName;
                        processReportList.Add(processReport);
                        temp.Add(processReport);

                        countWeekName += 1;
                    }

                    foreach (var processPlaned in processPlanedList)
                    {
                        var count = 0;
                        var disciplineObj = this.disciplineService.GetById(processPlaned.WorkgroupId.GetValueOrDefault());
                        if (disciplineObj != null)
                        {
                            foreach (var planed in processPlaned.Planed.Split('$').Select(Convert.ToDouble))
                            {
                                if (count < processReportList.Count)
                                {
                                    processReportList[count].Planed += disciplineId == 0
                                        ? (planed * disciplineObj.Weight.GetValueOrDefault()) / 100
                                        : planed;

                                   // processReportList[count].Planed += planed;
                                }

                                count += 1;
                            }

                        }
                    }

                    foreach (var processPlaned in processRecoveryPlanedList)
                    {
                        var count = 0;
                        var disciplineObj = this.disciplineService.GetById(processPlaned.WorkgroupId.GetValueOrDefault());
                        if (disciplineObj != null)
                        {
                            foreach (var planed in processPlaned.Planed.Split('$').Select(Convert.ToDouble))
                            {
                                if (count < processReportList.Count)
                                {
                                    processReportList[count].Planed += disciplineId == 0
                                        ? (planed * disciplineObj.Weight.GetValueOrDefault()) / 100
                                        : planed;

                                   // processReportList[count].RecoveryPlan += planed;
                                }

                                count += 1;
                            }

                        }
                    }

                    foreach (var processActual in processActualList)
                    {
                        var count = 0;
                        var actualList = new List<double>();
                        actualList = processActual.Actual.Split('$').Select(Convert.ToDouble).ToList();

                        var disciplineObj = this.disciplineService.GetById(processActual.WorkgroupId.GetValueOrDefault());
                        if (disciplineObj != null)
                        {
                            foreach (var planed in processActual.Actual.Split('$').Select(Convert.ToDouble))
                            {
                                if (count < actualList.Count && count < processReportList.Count)
                                {
                                    temp[count].Actual += (disciplineId == 0
                                        ? (actualList[count] * disciplineObj.Weight.GetValueOrDefault()) / 100
                                        : actualList[count]);

                                   // temp[count].Actual += actualList[count];
                                }

                                count += 1;
                            }

                        }
                    }

                    this.LineChart.ChartTitle.Text = "DETAIL ENGINEERING SERVICE FOR " + projectObj.Description +
                                                     " | Cut-off: " +
                                                     DateTime.Now.ToString("dd/MM/yyyy");


                    this.LineChart.DataSource = processReportList;
                    this.LineChart.DataBind();
                    if (lineSeriesRecoveryPlaned != null)
                    {
                        lineSeriesRecoveryPlaned.Items.Clear();
                    }

                    if (lineSeriesActual != null)
                    {
                        lineSeriesActual.Items.Clear();
                    }

                    if (lineSeriesPlaned != null)
                    {
                        lineSeriesPlaned.Items.Clear();
                    }


                    foreach (var progressNode in temp)
                    {
                        if (lineSeriesActual != null)
                        {
                            if (progressNode.Actual == 0)
                            {
                                lineSeriesActual.Items.Add((decimal?) null);
                            }
                            else
                            {
                                lineSeriesActual.Items.Add((decimal?) progressNode.Actual);
                            }
                        }
                        if (lineSeriesPlaned != null)
                        {
                            if (progressNode.Planed == 0)
                            {
                                lineSeriesPlaned.Items.Add((decimal?) null);
                            }
                            else
                            {
                                lineSeriesPlaned.Items.Add((decimal?) progressNode.Planed);
                            }
                        }
                        if (lineSeriesRecoveryPlaned != null)
                        {
                            if (progressNode.RecoveryPlan == 0)
                            {
                                if (lineSeriesRecoveryPlaned != null)
                                    lineSeriesRecoveryPlaned.Items.Add((decimal?) null);
                            }
                            else
                            {
                                if (lineSeriesRecoveryPlaned != null)
                                    lineSeriesRecoveryPlaned.Items.Add((decimal?) progressNode.RecoveryPlan);
                            }
                        }
                    }
                }
                else
                {
                    if (projectObj != null)
                    {
                        this.LineChart.ChartTitle.Text = "DETAIL ENGINEERING SERVICE FOR " + projectObj.Description +
                                                         " | Cut-off: " +
                                                         DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    this.LineChart.DataSource = processReportList;
                    this.LineChart.DataBind();
                }

            }
        }

        private void InitData()
        {
            var projectInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                ? this.scopeProjectService.GetAll().OrderBy(t => t.Name)
                : this.scopeProjectService.GetAllInPermission(UserSession.Current.User.Id).OrderBy(t => t.Name);
            var project = projectInPermission.ToList();
            this.ddlProject.DataSource = project;
            this.ddlProject.DataTextField = "Name";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
            
            if (ddlProject.Items.Count > 0)
            {
                this.ddlProject.SelectedIndex = 0;
                var revPlaned = new List<ProcessRecoveryPlaned>();

                var recoveryPlanList = this.processRecoveryPlanedService.GetAllByProject(Convert.ToInt32(ddlProject.SelectedValue));
                var timeList = recoveryPlanList.Select(t => t.IndexNo).Distinct().OrderByDescending(t => t);
                foreach (var index in timeList)
                {
                    var revPlanAtTime = recoveryPlanList.Where(t => t.IndexNo == index).ToList();
                    if (revPlanAtTime.Any())
                    {
                        revPlaned.Add(revPlanAtTime[0]);
                    }
                }

                this.ddlRecoveryPlan.DataSource = revPlaned;
                this.ddlRecoveryPlan.DataTextField = "RevcoveryName";
                this.ddlRecoveryPlan.DataValueField = "IndexNo";
                this.ddlRecoveryPlan.DataBind();
            }
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
            this.ddlRecoveryPlan.Items.Clear();
            var revPlaned = new List<ProcessRecoveryPlaned>();

            var recoveryPlanList = this.processRecoveryPlanedService.GetAllByProject(Convert.ToInt32(ddlProject.SelectedValue));
            var timeList = recoveryPlanList.Select(t => t.IndexNo).Distinct().OrderByDescending(t => t);
            foreach (var index in timeList)
            {
                var revPlanAtTime = recoveryPlanList.Where(t => t.IndexNo == index).ToList();
                if (revPlanAtTime.Any())
                {
                    revPlaned.Add(revPlanAtTime[0]);
                }
            }

            this.ddlRecoveryPlan.DataSource = revPlaned;
            this.ddlRecoveryPlan.DataTextField = "RevcoveryName";
            this.ddlRecoveryPlan.DataValueField = "IndexNo";
            this.ddlRecoveryPlan.DataBind();

            this.LoadProcessReport();
        }

        protected void ddlRecoveryPlan_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadProcessReport();
        }
    }
}

