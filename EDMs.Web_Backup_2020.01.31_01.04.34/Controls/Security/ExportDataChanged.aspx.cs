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
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Menu = EDMs.Data.Entities.Menu;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ExportDataChanged : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        private readonly WaitingSyncDataService syncWaitingService = new WaitingSyncDataService();

        private readonly TitleService titleService = new TitleService();

        private readonly ScopeProjectService projectService = new ScopeProjectService();

        private readonly MenuService menuService = new MenuService();

        private MRTypeService mrTypeService = new MRTypeService();

        private readonly UserService userService = new UserService();

        private readonly PriorityLevelService priorityLevelService = new PriorityLevelService();

        private readonly MaterialRequisitionDetailService mrDetailService = new MaterialRequisitionDetailService();

        private readonly MaterialRequisitionCheckListDetailService mrChecklistDetailService = new MaterialRequisitionCheckListDetailService();

        private readonly MaterialRequisitionService mrService = new MaterialRequisitionService();

        private readonly MaterialRequisitionAttachFileService mrAttachFileService = new MaterialRequisitionAttachFileService();

        private readonly MaterialRequisitionCheckListDefineService mrCheckListDefineService = new MaterialRequisitionCheckListDefineService();

        private readonly NumberManagementService numberManagementService = new NumberManagementService();

        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();

        private readonly ObjectAssignedWorkflowService objAssignedWorkflowService = new ObjectAssignedWorkflowService();

        private readonly RoleService roleService = new RoleService();

        private readonly IntergrateParamConfigService configService = new IntergrateParamConfigService();

        private readonly ExportDataChangeHistoryService exportHistoryService = new ExportDataChangeHistoryService();

        private readonly MaterialRequisitionCommentService mrCommentService = new MaterialRequisitionCommentService();

        private readonly TrackingBreakdownReportService trackingBreakdownReportService = new TrackingBreakdownReportService();

        private TrackingMorningCallService trackingMorningCallService = new TrackingMorningCallService();

        private TrackingMorningCallAttachFileService trackingMorningCallAttachFileService = new TrackingMorningCallAttachFileService();

        private TrackingMorningCallCommentService trackingMorningCallCommentService = new TrackingMorningCallCommentService();

        private TrackingBreakdownReportAttachFileService trackingBreakdownReportAttachFileService = new TrackingBreakdownReportAttachFileService();

        private TrackingBreakdownReportCommentService trackingBreakdownReportCommentService = new TrackingBreakdownReportCommentService();

        private TrackingECRService trackingECRService = new TrackingECRService();

        private TrackingECRAttachFileService trackingECRAttachFileService = new TrackingECRAttachFileService();

        private TrackingECRCommentService trackingEcrCommentService = new TrackingECRCommentService();

        private TrackingMOCService trackingMOCService = new TrackingMOCService();

        private TrackingMOCAttachFileService trackingMOCAttachFileService = new TrackingMOCAttachFileService();

        private TrackingMOCCommentService trackingMocCommentService = new TrackingMOCCommentService();

        private TrackingProcedureService trackingProcedureService = new TrackingProcedureService();

        private TrackingProcedureAttachFileService trackingProcedureAttachFileService = new TrackingProcedureAttachFileService();

        private TrackingPunchService trackingPunchService = new TrackingPunchService();

        private TrackingPunchAttachFileService trackingPunchAttachFileService = new TrackingPunchAttachFileService();

        private TrackingSailService trackingSailService = new TrackingSailService();

        private TrackingSailAttachFileService trackingSailAttachFileService = new TrackingSailAttachFileService();

        private TrackingShutdownReportService trackingShutdownReportService = new TrackingShutdownReportService();

        private TrackingShutdownReportAttachFileService trackingShutdownReportAttachFileService = new  TrackingShutdownReportAttachFileService();

        private TrackingShutdownReportCommentService trackingShutdownReportCommentService = new TrackingShutdownReportCommentService();

        private TrackingWCRService trackingWCRService = new TrackingWCRService();

        private TrackingWCRAttachFileService trackingWCRAttachFileService = new TrackingWCRAttachFileService();

        private WorkflowService wfService = new WorkflowService();

        private WorkflowDetailService wfDetailService = new WorkflowDetailService();

        private WorkflowStepService wfStepService = new WorkflowStepService();

        private WorkRequestService workRequestService = new WorkRequestService();

        private WorkRequestAttachFileService workRequestAttachFileService = new WorkRequestAttachFileService();

        private WorkRequestCommentService workRequestCommentService = new WorkRequestCommentService();

        private TrackingGeneralWorkingService trackingGeneralWorkingService = new TrackingGeneralWorkingService();

        private TrackingGeneralWorkingAttachFileService trackingGeneralWorkingAttachFileService = new TrackingGeneralWorkingAttachFileService();

        private HolidayConfigService holidayConfigService = new HolidayConfigService();

        private ProcedureStageService procedureStageService = new ProcedureStageService();

        private ProcedureTypeService procedureTypeService = new ProcedureTypeService();

        private PerformanceReviewAttachFileService prAttachFileService = new PerformanceReviewAttachFileService();
        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

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
            Session.Add("SelectedMainMenu", "System");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.LoadScopePanel();
                this.LoadSystemPanel();
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.MasterTableView.CurrentPageIndex = this.grdDocument.MasterTableView.PageCount - 1;
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ExportDataChange")
            {
                var configObj = configService.GetById(1);

                var dataChangedList = this.syncWaitingService.GetAllActive();
                var filePath = Server.MapPath("../../Exports/DataChanged") + @"\";
                var newFileName = configObj.Sync_EmailName + "_Data Changed_" +
                                                   DateTime.Now.ToString("dd-MM-yyyy HHmmss_") + (this.exportHistoryService.GetCount() + 1) + ".sql";
                File.Copy(filePath + "DataChanged.sql", filePath + newFileName);

                var exportHistory = new ExportDataChangeHistory()
                {
                    FileName = newFileName,
                    FilePath = filePath + newFileName,
                    CreatedTime = DateTime.Now,
                    IsComplete = false,
                    ErrorMess = string.Empty,
                    SendMailComplete = false
                };
                this.exportHistoryService.Insert(exportHistory);

                try
                {
                    this.ExportDataFile(filePath + newFileName, dataChangedList);

                    exportHistory.IsComplete = true;
                    this.exportHistoryService.Update(exportHistory);
                }
                catch (Exception ex)
                {
                    exportHistory.ErrorMess += "Export data file error: " + ex.Message + Environment.NewLine;
                    this.exportHistoryService.Update(exportHistory);
                }

                this.Download_File(filePath + newFileName);
                this.grdDocument.Rebind();
            }
        }

        private void ExportDataFile(string filePath, List<WaitingSyncData> dataChangedList)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var waitingSyncData in dataChangedList)
                    {
                        switch (waitingSyncData.ObjectName)
                        {
                            case "[WMS].[PerformanceReviewAttachFiles]":
                                var prAttachFileObj = this.prAttachFileService.GetById(waitingSyncData.ObjectID.GetValueOrDefault());
                                if (prAttachFileObj != null)
                                {
                                    sw.WriteLine(this.GetPerformanceReviewAttachFileChanged(waitingSyncData, prAttachFileObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Library].[ProcedureStage]":
                                var procedureStageObj = this.procedureStageService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (procedureStageObj != null)
                                {
                                    sw.WriteLine(this.GetProcedureStageChanged(waitingSyncData, procedureStageObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;

                            case "[Library].[ProcedureType]":
                                var procedureTypegObj = this.procedureTypeService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (procedureTypegObj != null)
                                {
                                    sw.WriteLine(this.GetProcedureTypeChanged(waitingSyncData, procedureTypegObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;

                            case "[Library].[HolidayConfig]":
                                var holidayConfigObj = this.holidayConfigService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (holidayConfigObj != null)
                                {
                                    sw.WriteLine(this.GetHolidayConfigChanged(waitingSyncData, holidayConfigObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;

                            case "[Library].[MRType]":
                                var mrTypeObj = this.mrTypeService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (mrTypeObj != null)
                                {
                                    sw.WriteLine(this.GetMRTypeChanged(waitingSyncData, mrTypeObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }

                                break;
                            case "[Library].[PriorityLevel]":
                                var priorityLvlObj = this.priorityLevelService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (priorityLvlObj != null)
                                {
                                    sw.WriteLine(this.GetPriorityLevelChanged(waitingSyncData, priorityLvlObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Library].[Title]":
                                var titleObj = this.titleService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (titleObj != null)
                                {
                                    sw.WriteLine(this.GetTitleChanged(waitingSyncData, titleObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Scope].[ScopeProjects]":
                                var projectObj = this.projectService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (projectObj != null)
                                {
                                    sw.WriteLine(this.GetProjectChanged(waitingSyncData, projectObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Security].[Menus]":
                                var menuObj = this.menuService.GetByID(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (menuObj != null)
                                {
                                    sw.WriteLine(this.GetMenuChanged(waitingSyncData, menuObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Security].[Permissions]":
                                var permissionObj = this.permissionService.GetByID(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (permissionObj != null)
                                {
                                    sw.WriteLine(this.GetPermissionChanged(waitingSyncData, permissionObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Security].[Roles]":
                                var roleObj = this.roleService.GetByID(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (roleObj != null)
                                {
                                    sw.WriteLine(this.GetRoleChanged(waitingSyncData, roleObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[Security].[Users]":
                                var userObj = this.userService.GetByID(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (userObj != null)
                                {
                                    sw.WriteLine(this.GetUserChanged(waitingSyncData, userObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[MaterialRequisition]":
                                var mrObj = this.mrService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (mrObj != null)
                                {
                                    sw.WriteLine(this.GetMRChanged(waitingSyncData, mrObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[MaterialRequisitionAttachFiles]":
                                var mrAttachFileObj = this.mrAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (mrAttachFileObj != null)
                                {
                                    sw.WriteLine(this.GetMRAttachFileChanged(waitingSyncData, mrAttachFileObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[MaterialRequisitionCheckListDefine]":
                                var mrCheckListDefineObj = this.mrCheckListDefineService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (mrCheckListDefineObj != null)
                                {
                                    sw.WriteLine(this.GetMRCheckListDefineChanged(waitingSyncData, mrCheckListDefineObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[MaterialRequisitionCheckListDetail]":
                                var mrCheckListDetailObj = this.mrChecklistDetailService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (mrCheckListDetailObj != null)
                                {
                                    sw.WriteLine(this.GetMRCheckListDetailChanged(waitingSyncData, mrCheckListDetailObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }

                                break;
                            case "[WMS].[MaterialRequisitionComment]":
                                var mrCommentObj = this.mrCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (mrCommentObj != null)
                                {
                                    sw.WriteLine(this.GetMRCommentChanged(waitingSyncData, mrCommentObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[MaterialRequisitionDetail]":
                                var mrDetailObj = this.mrDetailService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (mrDetailObj != null)
                                {
                                    sw.WriteLine(this.GetMRDetailChanged(waitingSyncData, mrDetailObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[NumberManagement]":
                                var numberObj = this.numberManagementService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (numberObj != null)
                                {
                                    sw.WriteLine(this.GetNumberManagementChanged(waitingSyncData, numberObj));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[ObjectAssignedUser]":
                                var objAssignUser = this.objAssignedUserService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objAssignUser != null)
                                {
                                    sw.WriteLine(this.GetObjAssignUserChanged(waitingSyncData, objAssignUser));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[ObjectAssignedWorkflow]":
                                var objAssignWf = this.objAssignedWorkflowService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objAssignWf != null)
                                {
                                    sw.WriteLine(this.GetObjAssignWorkFlowChanged(waitingSyncData, objAssignWf));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingBreakdownReport]":
                                var objbreakdownReport = this.trackingBreakdownReportService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objbreakdownReport != null)
                                {
                                    sw.WriteLine(this.GetObjBreakdownReportChanged(waitingSyncData, objbreakdownReport));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingBreakdownReportAttachFiles]":
                                var objbreakdownReportAttach = this.trackingBreakdownReportAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objbreakdownReportAttach != null)
                                {
                                    sw.WriteLine(this.GetObjBreakdownReportAttachChanged(waitingSyncData, objbreakdownReportAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingBreakdownReportComment]":
                                var objbreakdownReportComment = this.trackingBreakdownReportCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objbreakdownReportComment != null)
                                {
                                    sw.WriteLine(this.GetObjBreakdownReportCommentChanged(waitingSyncData, objbreakdownReportComment));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingECR]":
                                var objECR = this.trackingECRService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objECR != null)
                                {
                                    sw.WriteLine(this.GetObjECRChanged(waitingSyncData, objECR));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }

                                break;
                            case "[WMS].[TrackingECRAttachFiles]":
                                var objECRAttach = this.trackingECRAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objECRAttach != null)
                                {
                                    sw.WriteLine(this.GetObjECRAttachChanged(waitingSyncData, objECRAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingECRComment]":
                                var objECRComment = this.trackingEcrCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objECRComment != null)
                                {
                                    sw.WriteLine(this.GetObjECRCommentChanged(waitingSyncData, objECRComment));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingMOC]":
                                var objMOC = this.trackingMOCService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objMOC != null)
                                {
                                    sw.WriteLine(this.GetObjMOCChanged(waitingSyncData, objMOC));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingMOCAttachFiles]":
                                var objMOCAttach = this.trackingMOCAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objMOCAttach != null)
                                {
                                    sw.WriteLine(this.GetObjMOCAttachChanged(waitingSyncData, objMOCAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingMOCComment]":
                                var objMOCComment = this.trackingMocCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objMOCComment != null)
                                {
                                    sw.WriteLine(this.GetObjMOCCommentChanged(waitingSyncData, objMOCComment));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingMorningCall]":
                                var objMorningCall = this.trackingMorningCallService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objMorningCall != null)
                                {
                                    sw.WriteLine(this.GetObjMorningCallChanged(waitingSyncData, objMorningCall));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingMorningCallAttachFiles]":
                                var objMorningCallAttach = this.trackingMorningCallAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objMorningCallAttach != null)
                                {
                                    sw.WriteLine(this.GetObjMorningCallAttachChanged(waitingSyncData, objMorningCallAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingMorningCallComment]":
                                var objMorningCallComment = this.trackingMorningCallCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objMorningCallComment != null)
                                {
                                    sw.WriteLine(this.GetObjMorningCallCommentChanged(waitingSyncData, objMorningCallComment));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingProcedure]":
                                var objProcedure = this.trackingProcedureService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objProcedure != null)
                                {
                                    sw.WriteLine(this.GetObjProcedureChanged(waitingSyncData, objProcedure));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingProcedureAttachFiles]":
                                var objProcedureAttach = this.trackingProcedureAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objProcedureAttach != null)
                                {
                                    sw.WriteLine(this.GetObjProcedureAttachChanged(waitingSyncData, objProcedureAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingPunch]":
                                var objPunch = this.trackingPunchService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objPunch != null)
                                {
                                    sw.WriteLine(this.GetObjPunchChanged(waitingSyncData, objPunch));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingPunchAttachFiles]":
                                var objPunchAttach = this.trackingPunchAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objPunchAttach != null)
                                {
                                    sw.WriteLine(this.GetObjPunchAttachChanged(waitingSyncData, objPunchAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingSail]":
                                var objSail = this.trackingSailService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objSail != null)
                                {
                                    sw.WriteLine(this.GetObjSailChanged(waitingSyncData, objSail));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingSailAttachFiles]":
                                var objSailAttach = this.trackingSailAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objSailAttach != null)
                                {
                                    sw.WriteLine(this.GetObjSailAttachChanged(waitingSyncData, objSailAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingShutdownReport]":
                                var objShutdownReport = this.trackingShutdownReportService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objShutdownReport != null)
                                {
                                    sw.WriteLine(this.GetObjShutdownReportChanged(waitingSyncData, objShutdownReport));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingShutdownReportAttachFiles]":
                                var objShutdownReportAttach = this.trackingShutdownReportAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objShutdownReportAttach != null)
                                {
                                    sw.WriteLine(this.GetObjShutdownReportAttachChanged(waitingSyncData, objShutdownReportAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingShutdownReportComment]":
                                var objShutdownReportComment = this.trackingShutdownReportCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objShutdownReportComment != null)
                                {
                                    sw.WriteLine(this.GetObjShutdownReportCommentChanged(waitingSyncData, objShutdownReportComment));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingWCR]":
                                var objWCR = this.trackingWCRService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objWCR != null)
                                {
                                    sw.WriteLine(this.GetObjWCRChanged(waitingSyncData, objWCR));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingWCRAttachFiles]":
                                var objWCRAttach = this.trackingWCRAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objWCRAttach != null)
                                {
                                    sw.WriteLine(this.GetObjWCRAttachChanged(waitingSyncData, objWCRAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingGeneralWorking]":
                                var objGeneralWorking = this.trackingGeneralWorkingService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objGeneralWorking != null)
                                {
                                    sw.WriteLine(this.GetObjGeneralWorkingChanged(waitingSyncData, objGeneralWorking));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[TrackingGeneralWorkingAttachFiles]":
                                var objGeneralWorkingAttach = this.trackingGeneralWorkingAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objGeneralWorkingAttach != null)
                                {
                                    sw.WriteLine(this.GetObjGeneralWorkingAttachChanged(waitingSyncData, objGeneralWorkingAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[Workflow]":
                                var objWorkflow = this.wfService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (objWorkflow != null)
                                {
                                    sw.WriteLine(this.GetObjWorkflowChanged(waitingSyncData, objWorkflow));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[WorkflowDetails]":
                                var objWFDetail = this.wfDetailService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (objWFDetail != null)
                                {
                                    sw.WriteLine(this.GetObjWFDetailChanged(waitingSyncData, objWFDetail));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[WorkflowStep]":
                                var objWFStep = this.wfStepService.GetById(waitingSyncData.ObjectID2.GetValueOrDefault());
                                if (objWFStep != null)
                                {
                                    sw.WriteLine(this.GetObjWFStepChanged(waitingSyncData, objWFStep));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[WorkRequest]":
                                var objwr = this.workRequestService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objwr != null)
                                {
                                    sw.WriteLine(this.GetObjWRChanged(waitingSyncData, objwr));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[WorkRequestAttachFiles]":
                                var objWRAttach = this.workRequestAttachFileService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objWRAttach != null)
                                {
                                    sw.WriteLine(this.GetObjWRAttachChanged(waitingSyncData, objWRAttach));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                            case "[WMS].[WorkRequestComment]":
                                var objWRComment = this.workRequestCommentService.GetById(new Guid(waitingSyncData.ObjectID.ToString()));
                                if (objWRComment != null)
                                {
                                    sw.WriteLine(this.GetObjWRCommentChanged(waitingSyncData, objWRComment));
                                    waitingSyncData.IsSynced = true;
                                    this.syncWaitingService.Update(waitingSyncData);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        private string GetProcedureTypeChanged(WaitingSyncData changeInfo, ProcedureType obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Name, Description, ProjectId, ProjectName, CreatedBy, CreatedDate, CreatedName) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "N'" + obj.ProjectName + "', " +
                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +

                            "N'" + obj.CreatedName + "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Name=" + "N'" + obj.Name + "', " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "ProjectName=" + "N'" + obj.ProjectName + "', " +
                            "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                            "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                            "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetProcedureStageChanged(WaitingSyncData changeInfo, ProcedureStage obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Name, Description, ProjectId, ProjectName, CreatedBy, CreatedDate, CreatedName) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "N'" + obj.ProjectName + "', " +
                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +

                            "N'" + obj.CreatedName + "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Name=" + "N'" + obj.Name + "', " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "ProjectName=" + "N'" + obj.ProjectName + "', " +
                            "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                            "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                            "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetPerformanceReviewAttachFileChanged(WaitingSyncData changeInfo, PerformanceReviewAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, UserId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          +obj.UserId + ", " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjShutdownReportCommentChanged(WaitingSyncData changeInfo, TrackingShutdownReportComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ShutdownReportId, ShutdownReportNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ShutdownReportId.GetValueOrDefault() + "', " +
                          "N'" + obj.ShutdownReportNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "ShutdownReportId=" + "N'" + obj.ShutdownReportId + "', " +
                              "ShutdownReportNo=" + "N'" + obj.ShutdownReportNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjMorningCallCommentChanged(WaitingSyncData changeInfo, TrackingMorningCallComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MorningCallId, MOCNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MorningCallId.GetValueOrDefault() + "', " +
                          "N'" + obj.MorningCallNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "MorningCallId=" + "N'" + obj.MorningCallId + "', " +
                              "MorningCallNo=" + "N'" + obj.MorningCallNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjMOCCommentChanged(WaitingSyncData changeInfo, TrackingMOCComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MOCId, MOCNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MOCId.GetValueOrDefault() + "', " +
                          "N'" + obj.MOCNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "MOCId=" + "N'" + obj.MOCId + "', " +
                              "MOCNo=" + "N'" + obj.MOCNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjECRCommentChanged(WaitingSyncData changeInfo, TrackingECRComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ECRId, ECRNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ECRId.GetValueOrDefault() + "', " +
                          "N'" + obj.ECRNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "ECRId=" + "N'" + obj.ECRId + "', " +
                              "ECRNo=" + "N'" + obj.ECRNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjBreakdownReportCommentChanged(WaitingSyncData changeInfo, TrackingBreakdownReportComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, BreakdownReportId, BreakdownReportNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.BreakdownReportId.GetValueOrDefault() + "', " +
                          "N'" + obj.BreakdownReportNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "BreakdownReportId=" + "N'" + obj.BreakdownReportId + "', " +
                              "BreakdownReportNo=" + "N'" + obj.BreakdownReportNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetHolidayConfigChanged(WaitingSyncData changeInfo, HolidayConfig obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Description, FromDate, ToDate, CreatedBy, CreatedByName, CreatedDate) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Description + "', " +
                            (obj.FromDate != null ? "CASt('" + obj.FromDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                            (obj.ToDate != null ? "CASt('" + obj.ToDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +

                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            "N'" + obj.Description + "', " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                            "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "FromDate=" + (obj.FromDate != null ? "CASt('" + obj.FromDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                            "ToDate=" + (obj.ToDate != null ? "CASt('" + obj.ToDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                            "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                            "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                            "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +

                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjWRCommentChanged(WaitingSyncData changeInfo, WorkRequestComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, WRId, WRNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.WRId.GetValueOrDefault() + "', " +
                          "N'" + obj.WRNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "WRId=" + "N'" + obj.WRId + "', " +
                              "WRNo=" + "N'" + obj.WRNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjWRAttachChanged(WaitingSyncData changeInfo, WorkRequestAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, WRId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.WRId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjWRChanged(WaitingSyncData changeInfo, WorkRequest obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, WRNo, WRTitle, OriginatorId, OriginatorName, OriginatorJobTitle, RaisedDate, RequriedDate, Description, ScopeOfService, Reason, PriotiyLevelId, PriorityLevelName, OIMReviewId, OIMReviewName, OIMReviewDate, OperationManagerReviewId, OperationManagerReviewName, OperationManagerReviewDate, TechnicalDeptReviewId, TechnicalDeptReviewName, TechnicalDeptReviewDate, DirectorReviewId, DirectorReviewName, DirectorReviewDate, DepartmentId, DepartmentName, ProjectId, ProjectName, IsWFComplete, IsInWFProcess, IsCancel, IsCompleteFinal, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.WRNo + "', " +
                          "N'" + obj.WRTitle + "', " +
                          (obj.OriginatorId?.ToString() ?? "null") + ", " +
                          "N'" + obj.OriginatorName + "', " +
                          "N'" + obj.OriginatorJobTitle + "', " +
                          (obj.RaisedDate != null ? "CASt('" + obj.RaisedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.RequriedDate + "', " +
                          "N'" + obj.Description + "', " +
                          "N'" + obj.ScopeOfService + "', " +
                          "N'" + obj.Reason + "', " +
                          (obj.PriotiyLevelId?.ToString() ?? "null") + ", " +
                          "N'" + obj.PriorityLevelName + "', " +

                          (obj.OIMReviewId?.ToString() ?? "null") + ", " +
                          "N'" + obj.OIMReviewName + "', " +
                          (obj.OIMReviewDate != null ? "CASt('" + obj.OIMReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +

                          (obj.OperationManagerReviewId?.ToString() ?? "null") + ", " +
                          "N'" + obj.OperationManagerReviewName + "', " +
                          (obj.OperationManagerReviewDate != null ? "CASt('" + obj.OperationManagerReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +

                          (obj.TechnicalDeptReviewId?.ToString() ?? "null") + ", " +
                          "N'" + obj.TechnicalDeptReviewName + "', " +
                          (obj.TechnicalDeptReviewDate != null ? "CASt('" + obj.TechnicalDeptReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +

                          (obj.DirectorReviewId?.ToString() ?? "null") + ", " +
                          "N'" + obj.DirectorReviewName + "', " +
                          (obj.DirectorReviewDate != null ? "CASt('" + obj.DirectorReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +

                          (obj.DepartmentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.DepartmentName + "', " +

                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "WRNo=" + "N'" + obj.WRNo + "', " +
                        "WRTitle=" + "N'" + obj.WRTitle + "', " +
                        "OriginatorId=" + (obj.OriginatorId?.ToString() ?? "null") + ", " +

                        "OriginatorName=" + "N'" + obj.OriginatorName + "', " +
                        "OriginatorJobTitle=" + "N'" + obj.OriginatorJobTitle + "', " +
                        "RaisedDate=" + (obj.RaisedDate != null ? "CASt('" + obj.RaisedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +


                        "RequriedDate=" + "N'" + obj.RequriedDate + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "ScopeOfService=" + "N'" + obj.ScopeOfService + "', " +
                        "Reason=" + "N'" + obj.Reason + "', " +

                        "PriotiyLevelId=" + (obj.PriotiyLevelId?.ToString() ?? "null") + ", " +
                        "PriorityLevelName=" + "N'" + obj.PriorityLevelName + "', " +

                        "OIMReviewId=" + (obj.OIMReviewId?.ToString() ?? "null") + ", " +
                        "OIMReviewName=" + "N'" + obj.OIMReviewName + "', " +
                        "OIMReviewDate=" + (obj.OIMReviewDate != null ? "CASt('" + obj.OIMReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                        "OperationManagerReviewId=" + (obj.OperationManagerReviewId?.ToString() ?? "null") + ", " +
                        "OperationManagerReviewName=" + "N'" + obj.OperationManagerReviewName + "', " +
                        "OperationManagerReviewDate=" + (obj.OperationManagerReviewDate != null ? "CASt('" + obj.OperationManagerReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                        "TechnicalDeptReviewId=" + (obj.TechnicalDeptReviewId?.ToString() ?? "null") + ", " +
                        "TechnicalDeptReviewName=" + "N'" + obj.TechnicalDeptReviewName + "', " +
                        "TechnicalDeptReviewDate=" + (obj.TechnicalDeptReviewDate != null ? "CASt('" + obj.TechnicalDeptReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                        "DirectorReviewId=" + (obj.DirectorReviewId?.ToString() ?? "null") + ", " +
                        "DirectorReviewName=" + "N'" + obj.DirectorReviewName + "', " +
                        "DirectorReviewDate=" + (obj.DirectorReviewDate != null ? "CASt('" + obj.DirectorReviewDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                        "DepartmentId=" + (obj.DepartmentId?.ToString() ?? "null") + ", " +
                        "DepartmentName=" + "N'" + obj.DepartmentName + "', " +

                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +

                        "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                        "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +

                        "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                        "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "', " +

                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +

                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjWFStepChanged(WaitingSyncData changeInfo, WorkflowStep obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                        "(ID, Name, Description, WorkflowID, WorkflowName, CreatedBy, CreatedDate, ProjectID, ProjectName, IsFirst, CanReject, LocationId, LocationName) VALUES(" +
                        obj.ID + ", " +
                        "N'" + obj.Name + "', " +
                        "N'" + obj.Description + "', " +
                        (obj.WorkflowID?.ToString() ?? "null") + ", " +
                        "N'" + obj.WorkflowName + "', " +
                        (obj.CreatedBy?.ToString() ?? "null") + ", " +
                        (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        (obj.ProjectID?.ToString() ?? "null") + ", " +
                        "N'" + obj.ProjectName + "', " +
                        (obj.IsFirst.GetValueOrDefault() ? 1 : 0) + ", " +
                        (obj.CanReject.GetValueOrDefault() ? 1 : 0) + ", " +
                        (obj.LocationId?.ToString() ?? "null") + ", " +
                        "N'" + obj.LocationName + "' " +
                        "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "Name=" + "N'" + obj.Name + "', " +
                        "Description=" + "N'" + obj.Description + "', " +

                        "WorkflowID=" + (obj.WorkflowID?.ToString() ?? "null") + ", " +
                        "WorkflowName=" + "N'" + obj.WorkflowName + "', " +

                        "ProjectID=" + (obj.ProjectID?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +

                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "IsFirst=" + (obj.IsFirst.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CanReject=" + (obj.CanReject.GetValueOrDefault() ? 1 : 0) + ", " +
                        "LocationId=" + (obj.LocationId?.ToString() ?? "null") + ", " +
                        "LocationName=" + "N'" + obj.LocationName + "' " +


                        "WHERE ID=" + obj.ID +
                        ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetObjWFDetailChanged(WaitingSyncData changeInfo, WorkflowDetail obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                        "(ID, WorkflowID, WorkflowName, CurrentWorkflowStepID, CurrentWorkflowStepName, StepDefinitionID, StepDefinitionName, Duration, AssignTitleIds, AssignUserIDs, InformationOnlyUserIDs, AssignRoleIDs, InformationOnlyRoleIDs, DistributionMatrixIDs, NextWorkflowStepID, NextWorkflowStepName, RejectWorkflowStepID, RejectWorkflowStepName, CreatedBy, CreatedDate, ProjectID, ProjectName, Recipients, IsFirst, IsOnlyWorkingDay, ManagementUserIds, CanReject) VALUES(" +
                        obj.ID + ", " +
                        (obj.WorkflowID?.ToString() ?? "null") + ", " +
                        "N'" + obj.WorkflowName + "', " +
                        (obj.CurrentWorkflowStepID?.ToString() ?? "null") + ", " +
                        "N'" + obj.CurrentWorkflowStepName + "', " +
                        (obj.StepDefinitionID?.ToString() ?? "null") + ", " +
                        "N'" + obj.StepDefinitionName + "', " +
                        (obj.Duration?.ToString() ?? "null") + ", " +
                        "N'" + obj.AssignTitleIds + "', " +
                        "N'" + obj.AssignUserIDs + "', " +
                        "N'" + obj.InformationOnlyUserIDs + "', " +
                        "N'" + obj.AssignRoleIDs + "', " +
                        "N'" + obj.InformationOnlyRoleIDs + "', " +
                        "N'" + obj.DistributionMatrixIDs + "', " +
                        (obj.NextWorkflowStepID?.ToString() ?? "null") + ", " +
                        "N'" + obj.NextWorkflowStepName + "', " +
                        (obj.RejectWorkflowStepID?.ToString() ?? "null") + ", " +
                        "N'" + obj.RejectWorkflowStepName + "', " +
                        (obj.CreatedBy?.ToString() ?? "null") + ", " +
                        (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        (obj.ProjectID?.ToString() ?? "null") + ", " +
                        "N'" + obj.ProjectName + "', " +
                        "N'" + obj.Recipients + "', " +
                        (obj.IsFirst.GetValueOrDefault() ? 1 : 0) + ", " +
                        (obj.IsOnlyWorkingDay.GetValueOrDefault() ? 1 : 0) + ", " +
                        "N'" + obj.ManagementUserIds + "', " +
                        (obj.CanReject.GetValueOrDefault() ? 1 : 0) + " " +
                        "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +

                        "WorkflowID=" + (obj.WorkflowID?.ToString() ?? "null") + ", " +
                        "WorkflowName=" + "N'" + obj.WorkflowName + "', " +

                        "CurrentWorkflowStepID=" + (obj.CurrentWorkflowStepID?.ToString() ?? "null") + ", " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +

                        "StepDefinitionID=" + (obj.StepDefinitionID?.ToString() ?? "null") + ", " +
                        "StepDefinitionName=" + "N'" + obj.StepDefinitionName + "', " +

                        "Duration=" + (obj.Duration?.ToString() ?? "null") + ", " +
                        "ManagementUserIds=" + "N'" + obj.ManagementUserIds + "', " +
                        "AssignTitleIds=" + "N'" + obj.AssignTitleIds + "', " +
                        "AssignUserIDs=" + "N'" + obj.AssignUserIDs + "', " +
                        "InformationOnlyUserIDs=" + "N'" + obj.InformationOnlyUserIDs + "', " +
                        "AssignRoleIDs=" + "N'" + obj.AssignRoleIDs + "', " +
                        "InformationOnlyRoleIDs=" + "N'" + obj.InformationOnlyRoleIDs + "', " +
                        "DistributionMatrixIDs=" + "N'" + obj.DistributionMatrixIDs + "', " +

                        "NextWorkflowStepID=" + (obj.NextWorkflowStepID?.ToString() ?? "null") + ", " +
                        "NextWorkflowStepName=" + "N'" + obj.NextWorkflowStepName + "', " +

                        "RejectWorkflowStepID=" + (obj.RejectWorkflowStepID?.ToString() ?? "null") + ", " +
                        "RejectWorkflowStepName=" + "N'" + obj.RejectWorkflowStepName + "', " +

                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectID=" + (obj.ProjectID?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "Recipients=" + "N'" + obj.Recipients + "', " +
                        "IsFirst=" + (obj.IsFirst.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsOnlyWorkingDay=" + (obj.IsOnlyWorkingDay.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CanReject=" + (obj.CanReject.GetValueOrDefault() ? 1 : 0) + " " +


                        "WHERE ID=" + obj.ID +
                        ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetObjWorkflowChanged(WaitingSyncData changeInfo, Data.Entities.Workflow obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                        "(ID, Name, ProjectID, ProjectName, Description, ObjectTypeId, ObjectTypeName, CreatedBy, CreatedDate) VALUES(" +
                        obj.ID + ", " +
                        "N'" + obj.Name + "', " +
                        (obj.ProjectID?.ToString() ?? "null") + ", " +
                        "N'" + obj.ProjectName + "', " +
                        "N'" + obj.Description + "', " +
                        (obj.ObjectTypeId?.ToString() ?? "null") + ", " +
                        "N'" + obj.ObjectTypeName + "', " +
                        (obj.CreatedBy?.ToString() ?? "null") + ", " +
                        (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                        "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "Name=" + "N'" + obj.Name + "', " +
                        "ProjectID=" + (obj.ProjectID?.ToString() ?? "null") + ", " +
                        "ObjectTypeId=" + (obj.ObjectTypeId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "ObjectTypeName=" + "N'" + obj.ObjectTypeName + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                        "WHERE ID=" + obj.ID +
                        ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetObjGeneralWorkingAttachChanged(WaitingSyncData changeInfo, TrackingGeneralWorkingAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, GeneralWorkingId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.GeneralWorkingId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjGeneralWorkingChanged(WaitingSyncData changeInfo, TrackingGeneralWorking obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, WorkCategoryId, WorkCategoryName, WorkContent, AssignUserIds, AssignUserName, BackupUserIds, BackupUserName, VerifyUserIds, VerifyUserName, StartDate, StartDate1, Deadline, Deadline1, DeadlineReasonChange, Status, Description, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, Code) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          (obj.WorkCategoryId?.ToString() ?? "null") + ", " +
                          "N'" + obj.WorkCategoryName + "', " +
                          "N'" + obj.WorkContent + "', " +
                          "N'" + obj.AssignUserIds + "', " +
                          "N'" + obj.AssignUserName + "', " +
                          "N'" + obj.BackupUserIds + "', " +
                          "N'" + obj.BackupUserName + "', " +
                          "N'" + obj.VerifyUserIds + "', " +
                          "N'" + obj.VerifyUserName + "', " +
                          (obj.StartDate != null ? "CASt('" + obj.StartDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.StartDate1 + "', " +
                          (obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.Deadline1 + "', " +
                          "N'" + obj.DeadlineReasonChange + "', " +
                          "N'" + obj.Status + "', " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Code + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "WorkCategoryId=" + (obj.WorkCategoryId?.ToString() ?? "null") + ", " +
                        "WorkCategoryName=" + "N'" + obj.WorkCategoryName + "', " +
                        "WorkContent=" + "N'" + obj.WorkContent + "', " +
                        "AssignUserIds=" + "N'" + obj.AssignUserIds + "', " +
                        "AssignUserName=" + "N'" + obj.AssignUserName + "', " +
                        "BackupUserIds=" + "N'" + obj.BackupUserIds + "', " +
                        "BackupUserName=" + "N'" + obj.BackupUserName + "', " +
                        "VerifyUserIds=" + "N'" + obj.VerifyUserIds + "', " +
                        "VerifyUserName=" + "N'" + obj.VerifyUserName + "', " +
                        "StartDate1=" + "N'" + obj.StartDate1 + "', " +
                        "StartDate=" + (obj.StartDate != null ? "CASt('" + obj.StartDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "Deadline=" + (obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "Deadline1=" + "N'" + obj.Deadline1 + "', " +
                        "DeadlineReasonChange=" + "N'" + obj.DeadlineReasonChange + "', " +
                        "Status=" + "N'" + obj.Status + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "Code=" + "N'" + obj.Code + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjWCRAttachChanged(WaitingSyncData changeInfo, TrackingWCRAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, WCRId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.WCRId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjWCRChanged(WaitingSyncData changeInfo, TrackingWCR obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, Name, Description, Reason, Action, OffshoreUpdate, Priority, OfficeComment, OffshoreComment, ShipyardUpdate, OpenDate, CloseDate, Status, Remark, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, PICIds,PICName,Code) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.Name + "', " +
                          "N'" + obj.Description + "', " +
                          "N'" + obj.Reason + "', " +
                          "N'" + obj.Action + "', " +
                          "N'" + obj.OffshoreUpdate + "', " +
                          "N'" + obj.Priority + "', " +
                          "N'" + obj.OfficeComment + "', " +
                          "N'" + obj.OffshoreComment + "', " +
                          "N'" + obj.ShipyardUpdate + "', " +
                          "N'" + obj.OpenDate + "', " +
                          "N'" + obj.CloseDate + "', " +
                          //(obj.OpenDate != null ? "CASt('" + obj.OpenDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          //(obj.CloseDate != null ? "CASt('" + obj.CloseDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.Status + "', " +
                          "N'" + obj.Remark + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.PICIds + "', " +
                        "N'" + obj.PICName + "', " +
                          "N'" + obj.Code + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "Name=" + "N'" + obj.Name + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "Reason=" + "N'" + obj.Reason + "', " +
                        "Action=" + "N'" + obj.Action + "', " +
                        "OffshoreUpdate=" + "N'" + obj.OffshoreUpdate + "', " +
                        "Priority=" + "N'" + obj.Priority + "', " +
                        "OfficeComment=" + "N'" + obj.OfficeComment + "', " +
                        "OffshoreComment=" + "N'" + obj.OffshoreComment + "', " +
                        "ShipyardUpdate=" + "N'" + obj.ShipyardUpdate + "', " +
                        "OpenDate=" + "N'" + obj.OpenDate + "', " +
                        "CloseDate=" + "N'" + obj.CloseDate + "', " +
                        //"OpenDate=" + (obj.OpenDate != null ? "CASt('" + obj.OpenDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        //"CloseDate=" + (obj.CloseDate != null ? "CASt('" + obj.CloseDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "Name=" + "N'" + obj.Status + "', " +
                        "Remark=" + "N'" + obj.Remark + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "PICIds=" + "N'" + obj.PICIds + "', " +
                        "PICName=" + "N'" + obj.PICName + "', " +
                        "Code=" + "N'" + obj.Code + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjShutdownReportAttachChanged(WaitingSyncData changeInfo, TrackingShutdownReportAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ShutdownReportId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ShutdownReportId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjShutdownReportChanged(WaitingSyncData changeInfo, TrackingShutdownReport obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, DateOfShutdown, TimeOfShutdown, DateResume, TimeResume, DownTime, EstimatedProduction, CauseShutdown, CauseClarificationProcess, CauseClarificationPowerloss, CauseClarificationFireGas, RootCause, AreaConcern, WayForward, PICIds, PICName, Deadline, DeadlineReasonChange, Status, Lesson, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, Code, IsWFComplete, IsInWFProcess, IsCancel, IsCompleteFinal, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          (obj.DateOfShutdown != null ? "CASt('" + obj.DateOfShutdown.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.TimeOfShutdown != null ? "CASt('" + obj.TimeOfShutdown.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.DateResume != null ? "CASt('" + obj.DateResume.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.TimeResume != null ? "CASt('" + obj.TimeResume.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.DownTime?.ToString() ?? "null") + ", " +
                          (obj.EstimatedProduction?.ToString() ?? "null") + ", " +
                          "N'" + obj.CauseShutdown + "', " +
                          "N'" + obj.CauseClarificationProcess + "', " +
                          "N'" + obj.CauseClarificationPowerloss + "', " +
                          "N'" + obj.CauseClarificationFireGas + "', " +
                          "N'" + obj.RootCause + "', " +
                          "N'" + obj.AreaConcern + "', " +
                          "N'" + obj.WayForward + "', " +
                          "N'" + obj.PICIds + "', " +
                          "N'" + obj.PICName + "', " +
                          (obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.DeadlineReasonChange + "', " +
                          "N'" + obj.Status + "', " +
                          "N'" + obj.Lesson + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Code + "', " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +

                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "DateOfShutdown=" + (obj.DateOfShutdown != null ? "CASt('" + obj.DateOfShutdown.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "TimeOfShutdown=" + (obj.TimeOfShutdown != null ? "CASt('" + obj.TimeOfShutdown.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "DateResume=" + (obj.DateResume != null ? "CASt('" + obj.DateResume.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "TimeResume=" + (obj.TimeResume != null ? "CASt('" + obj.TimeResume.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "DownTime=" + (obj.DownTime?.ToString() ?? "null") + ", " +
                        "EstimatedProduction=" + (obj.EstimatedProduction?.ToString() ?? "null") + ", " +
                        "CauseShutdown=" + "N'" + obj.CauseShutdown + "', " +
                        "CauseClarificationProcess=" + "N'" + obj.CauseClarificationProcess + "', " +
                        "CauseClarificationPowerloss=" + "N'" + obj.CauseClarificationPowerloss + "', " +
                        "CauseClarificationFireGas=" + "N'" + obj.CauseClarificationFireGas + "', " +
                        "RootCause=" + "N'" + obj.RootCause + "', " +
                        "AreaConcern=" + "N'" + obj.AreaConcern + "', " +
                        "WayForward=" + "N'" + obj.WayForward + "', " +
                        "PICIds=" + "N'" + obj.PICIds + "', " +
                        "PICName=" + "N'" + obj.PICName + "', " +
                        "Deadline=" + (obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "DeadlineReasonChange=" + "N'" + obj.DeadlineReasonChange + "', " +
                        "Status=" + "N'" + obj.Status + "', " +
                        "Lesson=" + "N'" + obj.Lesson + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "Code=" + "N'" + obj.Code + "', " +

                        "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                        "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +

                        "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                        "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjSailAttachChanged(WaitingSyncData changeInfo, TrackingSailAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, SailId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.SailId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjSailChanged(WaitingSyncData changeInfo, TrackingSail obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, DateRaised, SourceId, SourceName, NameObserver, Location, Description, Action, ProposedAction, Priority, TargetClose, ActionTakeClose, ClosedDate, HOCTrackingNo, MSRStatus, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId,PICIds, PICName, WRNo, MOCNo, ECRNo, Code) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          (obj.DateRaised != null ? "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.SourceId?.ToString() ?? "null") + ", " +
                          "N'" + obj.SourceName + "', " +
                          "N'" + obj.NameObserver + "', " +
                          "N'" + obj.Location + "', " +
                          "N'" + obj.Description + "', " +
                          "N'" + obj.Action + "', " +
                          "N'" + obj.ProposedAction + "', " +
                          "N'" + obj.Priority + "', " +
                          "N'" + obj.TargetClose + "', " +
                          "N'" + obj.ActionTakeClose + "', " +
                          "N'" + obj.ClosedDate + "', " +

                          //(obj.ClosedDate != null ? "CASt('" + obj.ClosedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.HOCTrackingNo + "', " +
                          "N'" + obj.MSRStatus + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.PICIds + "', " +
                          "N'" + obj.PICName + "', " +

                          "N'" + obj.WRNo + "', " +
                          "N'" + obj.MOCNo + "', " +
                          "N'" + obj.ECRNo + "', " +

                          "N'" + obj.Code + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        (obj.DateRaised != null ? "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "SourceId=" + (obj.SourceId?.ToString() ?? "null") + ", " +

                        "SourceName=" + "N'" + obj.SourceName + "', " +
                        "NameObserver=" + "N'" + obj.NameObserver + "', " +
                        "Location=" + "N'" + obj.Location + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "Action=" + "N'" + obj.Action + "', " +
                        "ProposedAction=" + "N'" + obj.ProposedAction + "', " +
                        "Priority=" + "N'" + obj.Priority + "', " +
                        "TargetClose=" + "N'" + obj.TargetClose + "', " +
                        "ActionTakeClose=" + "N'" + obj.ActionTakeClose + "', " +
                        "ClosedDate=" + "N'" + obj.ClosedDate + "', " +

                        //(obj.ClosedDate != null ? "CASt('" + obj.ClosedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "HOCTrackingNo=" + "N'" + obj.HOCTrackingNo + "', " +
                        "MSRStatus=" + "N'" + obj.MSRStatus + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "PICIds=" + "N'" + obj.PICIds + "', " +
                        "PICName=" + "N'" + obj.PICName + "', " +
                        "WRNo=" + "N'" + obj.WRNo + "', " +
                        "MOCNo=" + "N'" + obj.MOCNo + "', " +
                        "ECRNo=" + "N'" + obj.ECRNo + "', " +

                        "Code=" + "N'" + obj.Code + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjPunchAttachChanged(WaitingSyncData changeInfo, TrackingPunchAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, PunchId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.PunchId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjPunchChanged(WaitingSyncData changeInfo, TrackingPunch obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, CatAB, Description, Reason, DrawingNo, Location, SystemNo, DateRaised, Name, RaisedBy, PPSApproval, ShipOwnerApproval, ShipOwnerAction, MaterialRequire, PONo, TargetDate, Priority, CloseOutDate, Deadline, DeadlineReasonChange, WayForward, VerifyBy, Status, Remark, Impact, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, Code) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.CatAB + "', " +
                          "N'" + obj.Description + "', " +
                          "N'" + obj.Reason + "', " +
                          "N'" + obj.DrawingNo + "', " +
                          "N'" + obj.Location + "', " +
                          "N'" + obj.SystemNo + "', " +
                          (obj.DateRaised != null ? "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.Name + "', " +
                          "N'" + obj.RaisedBy + "', " +
                          "N'" + obj.PPSApproval + "', " +
                          "N'" + obj.ShipOwnerApproval + "', " +
                          "N'" + obj.ShipOwnerAction + "', " +
                          "N'" + obj.MaterialRequire + "', " +
                          "N'" + obj.PONo + "', " +
                          "N'" + obj.TargetDate + "', " +

                          //(obj.TargetDate != null ? "CASt('" + obj.TargetDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.Priority + "', " +
                          "N'" + obj.CloseOutDate + "', " +
                          "N'" + obj.Deadline + "', " +

                          //(obj.CloseOutDate != null ? "CASt('" + obj.CloseOutDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          //(obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.DeadlineReasonChange + "', " +
                          "N'" + obj.WayForward + "', " +
                          "N'" + obj.VerifyBy + "', " +
                          "N'" + obj.Status + "', " +
                          "N'" + obj.Remark + "', " +
                          "N'" + obj.Impact + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Code + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "CatAB=" + "N'" + obj.CatAB + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "Reason=" + "N'" + obj.Reason + "', " +
                        "DrawingNo=" + "N'" + obj.DrawingNo + "', " +
                        "Location=" + "N'" + obj.Location + "', " +
                        "SystemNo=" + "N'" + obj.SystemNo + "', " +
                        "DateRaised=" + (obj.DateRaised != null ? "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "Name=" + "N'" + obj.Name + "', " +
                        "RaisedBy=" + "N'" + obj.RaisedBy + "', " +
                        "PPSApproval=" + "N'" + obj.PPSApproval + "', " +
                        "ShipOwnerApproval=" + "N'" + obj.ShipOwnerApproval + "', " +
                        "ShipOwnerAction=" + "N'" + obj.ShipOwnerAction + "', " +
                        "MaterialRequire=" + "N'" + obj.MaterialRequire + "', " +
                        "PONo=" + "N'" + obj.PONo + "', " +
                        "TargetDate=" + "N'" + obj.TargetDate + "', " +

                        //"TargetDate=" + (obj.TargetDate != null ? "CASt('" + obj.TargetDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "Priority=" + "N'" + obj.Priority + "', " +
                        "CloseOutDate=" + "N'" + obj.CloseOutDate + "', " +
                        "Deadline=" + "N'" + obj.Deadline + "', " +

                        //"CloseOutDate=" + (obj.CloseOutDate != null ? "CASt('" + obj.CloseOutDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        //"Deadline=" + (obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "DeadlineReasonChange=" + "N'" + obj.DeadlineReasonChange + "', " +
                        "WayForward=" + "N'" + obj.WayForward + "', " +
                        "VerifyBy=" + "N'" + obj.VerifyBy + "', " +
                        "Status=" + "N'" + obj.Status + "', " +
                        "Remark=" + "N'" + obj.Remark + "', " +
                        "Impact=" + "N'" + obj.Impact + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "Code=" + "N'" + obj.Code + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjProcedureAttachChanged(WaitingSyncData changeInfo, TrackingProcedureAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ProcedureId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ProcedureId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjProcedureChanged(WaitingSyncData changeInfo, TrackingProcedure obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, SystemName, OldCode, NewCode, ProcedureName, PICIds, PICName, CheckerIds, Checker, TargerStage, StartDate, CompleteDate, TotalPage, DifficultLvl, OfficeManday, OffshoreManDay, CreateType, PercentComplete, Status, Deadline, DeadlineReasonChange, UpdatedInAMOS, Remark, LevelName, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, TargerStageId, TypeId, TypeName, Code) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.SystemName + "', " +
                          "N'" + obj.OldCode + "', " +
                          "N'" + obj.NewCode + "', " +
                          "N'" + obj.ProcedureName + "', " +
                          "N'" + obj.PICIds + "', " +
                          "N'" + obj.PICName + "', " +
                          "N'" + obj.CheckerIds + "', " +
                          "N'" + obj.Checker + "', " +
                          "N'" + obj.TargerStage + "', " +
                          "N'" + obj.StartDate + "', " +
                          "N'" + obj.CompleteDate + "', " +

                          //(obj.StartDate != null ? "CASt('" + obj.StartDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          //(obj.CompleteDate != null ? "CASt('" + obj.CompleteDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.TotalPage?.ToString() ?? "null") + ", " +
                          "N'" + obj.DifficultLvl + "', " +
                          (obj.OfficeManday?.ToString() ?? "null") + ", " +
                          (obj.OffshoreManDay?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreateType + "', " +
                          (obj.PercentComplete?.ToString() ?? "null") + ", " +
                          "N'" + obj.Status + "', " +
                          "N'" + obj.Deadline + "', " +

                          //(obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.DeadlineReasonChange + "', " +
                          "N'" + obj.UpdatedInAMOS + "', " +
                          "N'" + obj.Remark + "', " +
                          "N'" + obj.LevelName + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +

                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          (obj.TargerStageId?.ToString() ?? "null") + ", " +
                          (obj.TypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.TypeName + "', " +

                          "N'" + obj.Code + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "SystemName=" + "N'" + obj.SystemName + "', " +
                        "OldCode=" + "N'" + obj.OldCode + "', " +
                        "NewCode=" + "N'" + obj.NewCode + "', " +
                        "ProcedureName=" + "N'" + obj.ProcedureName + "', " +
                        "PICIds=" + "N'" + obj.PICIds + "', " +
                        "PICName=" + "N'" + obj.PICName + "', " +
                        "CheckerIds=" + "N'" + obj.CheckerIds + "', " +
                        "Checker=" + "N'" + obj.Checker + "', " +
                        "TargerStage=" + "N'" + obj.TargerStage + "', " +
                        "StartDate=" + "N'" + obj.StartDate + "', " +
                        "CompleteDate=" + "N'" + obj.CompleteDate + "', " +

                        //(obj.StartDate != null ? "CASt('" + obj.StartDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        //(obj.CompleteDate != null ? "CASt('" + obj.CompleteDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "TotalPage=" + (obj.TotalPage?.ToString() ?? "null") + ", " +
                        "DifficultLvl=" + "N'" + obj.DifficultLvl + "', " +
                        "OfficeManday=" + (obj.OfficeManday?.ToString() ?? "null") + ", " +
                        "OffshoreManDay=" + (obj.OffshoreManDay?.ToString() ?? "null") + ", " +
                        "CreateType=" + "N'" + obj.CreateType + "', " +
                        "PercentComplete=" + (obj.PercentComplete?.ToString() ?? "null") + ", " +
                        "Status=" + "N'" + obj.Status + "', " +
                        "Deadline=" + "N'" + obj.Deadline + "', " +

                        //(obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "DeadlineReasonChange=" + "N'" + obj.DeadlineReasonChange + "', " +
                        "UpdatedInAMOS=" + "N'" + obj.UpdatedInAMOS + "', " +
                        "Remark=" + "N'" + obj.Remark + "', " +
                        "Remark=" + "N'" + obj.LevelName + "', " +

                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "TargerStageId=" + (obj.TargerStageId?.ToString() ?? "null") + ", " +
                        "TypeId=" + (obj.TypeId?.ToString() ?? "null") + ", " +
                        "TypeName=" + "N'" + obj.TypeName + "', " +

                        "Code=" + "N'" + obj.Code + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjMorningCallAttachChanged(WaitingSyncData changeInfo, TrackingMorningCallAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MorningCallId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MorningCallId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetObjMorningCallChanged(WaitingSyncData changeInfo, TrackingMorningCall obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, Code, DateRaised, EquipmentName, IssueDescription, InitRiskAssessment, InitRiskLvlId, InitRiskLvlName, ActionPlanDescription, ActionPlanRiskLvlId, ActionPlanRiskLvlName, PICId, PICName, Deadline, DeadlineReasonChange, DeadlineHistory, RelativeDoc, CurrentUpdate, StatusId, StatusName, OffshoreComment, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, IsWFComplete, IsInWFProcess, IsCancel, IsCompleteFinal, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.Code + "', " +
                          (obj.DateRaised != null ? "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.EquipmentName + "', " +
                          "N'" + obj.IssueDescription + "', " +
                          "N'" + obj.InitRiskAssessment + "', " +
                          (obj.InitRiskLvlId?.ToString() ?? "null") + ", " +
                          "N'" + obj.InitRiskLvlName + "', " +
                          "N'" + obj.ActionPlanDescription + "', " +
                          (obj.ActionPlanRiskLvlId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ActionPlanRiskLvlName + "', " +
                          (obj.PICId?.ToString() ?? "null") + ", " +
                          "N'" + obj.PICName + "', " +
                          "N'" + obj.Deadline + "', " +
                          //(obj.Deadline != null ? "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.DeadlineReasonChange + "', " +
                          "N'" + obj.DeadlineHistory + "', " +
                          "N'" + obj.RelativeDoc + "', " +
                          "N'" + obj.CurrentUpdate + "', " +
                          (obj.StatusId?.ToString() ?? "null") + ", " +
                          "N'" + obj.StatusName + "', " +
                          "N'" + obj.OffshoreComment + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "Code=" + "N'" + obj.Code + "', " +
                        "DateRaised=" + "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "EquipmentName=" + "N'" + obj.EquipmentName + "', " +
                        "IssueDescription=" + "N'" + obj.IssueDescription + "', " +
                        "InitRiskAssessment=" + "N'" + obj.InitRiskAssessment + "', " +
                        "InitRiskLvlId=" + (obj.InitRiskLvlId?.ToString() ?? "null") + ", " +
                        "InitRiskLvlName=" + "N'" + obj.InitRiskLvlName + "', " +
                        "ActionPlanDescription=" + "N'" + obj.ActionPlanDescription + "', " +
                        "ActionPlanRiskLvlId=" + (obj.ActionPlanRiskLvlId?.ToString() ?? "null") + ", " +
                        "ActionPlanRiskLvlName=" + "N'" + obj.ActionPlanRiskLvlName + "', " +
                        "PICId=" + (obj.PICId?.ToString() ?? "null") + ", " +
                        "PICName=" + "N'" + obj.PICName + "', " +
                        "Deadline=" + "N'" + obj.Deadline + "', " +
                        //"Deadline=" + "CASt('" + obj.Deadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "DeadlineReasonChange=" + "N'" + obj.DeadlineReasonChange + "', " +
                        "DeadlineHistory=" + "N'" + obj.DeadlineHistory + "', " +
                        "RelativeDoc=" + "N'" + obj.RelativeDoc + "', " +
                        "CurrentUpdate=" + "N'" + obj.CurrentUpdate + "', " +
                        "StatusId=" + (obj.StatusId?.ToString() ?? "null") + ", " +
                        "StatusName=" + "N'" + obj.StatusName + "', " +
                        "OffshoreComment=" + "N'" + obj.OffshoreComment + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +

                        "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                        "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +

                        "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                        "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +

                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjMOCAttachChanged(WaitingSyncData changeInfo, TrackingMOCAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MOCId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MOCId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjMOCChanged(WaitingSyncData changeInfo, TrackingMOC obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, SystemName, DescriptionOfChange, DateIssued, ReasonOfChange, InitRisk, InitRiskLvlId, InitRiskLvlName, MigrationAction, MigrationRiskLvlId, MigrationRiskLvlName, DateAccepted, TechAuthority, StatusId, StatusName, Remark, ClosedClarification, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, Code, IsWFComplete, IsInWFProcess, IsCancel, IsCompleteFinal, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.SystemName + "', " +
                          "N'" + obj.DescriptionOfChange + "', " +
                          (obj.DateIssued != null ? "CASt('" + obj.DateIssued.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.ReasonOfChange + "', " +
                          "N'" + obj.InitRisk + "', " +
                          (obj.InitRiskLvlId?.ToString() ?? "null") + ", " +
                          "N'" + obj.InitRiskLvlName + "', " +
                          "N'" + obj.MigrationAction + "', " +
                          (obj.MigrationRiskLvlId?.ToString() ?? "null") + ", " +
                          "N'" + obj.MigrationRiskLvlName + "', " +
                          "N'" + obj.DateAccepted + "', " +
                          //(obj.DateAccepted != null ? "CASt('" + obj.DateAccepted.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.TechAuthority + "', " +
                          (obj.StatusId?.ToString() ?? "null") + ", " +
                          "N'" + obj.StatusName + "', " +
                          "N'" + obj.Remark + "', " +
                          "N'" + obj.ClosedClarification + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Code + "', " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +

                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +

                        "SystemName=" + "N'" + obj.SystemName + "', " +
                        "DescriptionOfChange=" + "N'" + obj.DescriptionOfChange + "', " +
                        "DateIssued=" + "CASt('" + obj.DateIssued.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "ReasonOfChange=" + "N'" + obj.ReasonOfChange + "', " +
                        "InitRisk=" + "N'" + obj.InitRisk + "', " +
                        "InitRiskLvlId=" + (obj.InitRiskLvlId?.ToString() ?? "null") + ", " +
                        "InitRiskLvlName=" + "N'" + obj.InitRiskLvlName + "', " +
                        "MigrationAction=" + "N'" + obj.MigrationAction + "', " +
                        "MigrationRiskLvlId=" + (obj.MigrationRiskLvlId?.ToString() ?? "null") + ", " +
                        "MigrationRiskLvlName=" + "N'" + obj.MigrationRiskLvlName + "', " +
                        "DateAccepted=" + "N'" + obj.DateAccepted + "', " +
                        //"DateAccepted=" + "CASt('" + obj.DateAccepted.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "TechAuthority=" + "N'" + obj.TechAuthority + "', " +
                        "StatusId=" + (obj.StatusId?.ToString() ?? "null") + ", " +
                        "StatusName=" + "N'" + obj.StatusName + "', " +
                        "Remark=" + "N'" + obj.Remark + "', " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "Code=" + "N'" + obj.Code + "', " +

                        "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                        "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +

                        "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                        "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjECRAttachChanged(WaitingSyncData changeInfo, TrackingECRAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ECRId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ECRId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjECRChanged(WaitingSyncData changeInfo, TrackingECR obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, Title, Description, DateRaised, Section1Id, Section1Name, Section2Id, Section2Name, Section3Id, Section3Name, Section4Id, Section4Name, Section5Id, Section5Name, ApSection3Id, ApSection3Name, ApRequirementId, ApRequirementName, ExecutionStatus, PersonInChargeIds, PersonInCharge, StatusId, StatusName, Cost, Remark, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, Code, IsWFComplete, IsInWFProcess, IsCancel, IsCompleteFinal, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.Title + "', " +
                          "N'" + obj.Description + "', " +
                          (obj.DateRaised != null ? "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.Section1Id?.ToString() ?? "null") + ", " +
                          "N'" + obj.Section1Name + "', " +
                          (obj.Section2Id?.ToString() ?? "null") + ", " +
                          "N'" + obj.Section2Name + "', " +
                          (obj.Section3Id?.ToString() ?? "null") + ", " +
                          "N'" + obj.Section3Name + "', " +
                          (obj.Section4Id?.ToString() ?? "null") + ", " +
                          "N'" + obj.Section4Name + "', " +
                          (obj.Section5Id?.ToString() ?? "null") + ", " +
                          "N'" + obj.Section5Name + "', " +
                          (obj.ApSection3Id?.ToString() ?? "null") + ", " +
                          "N'" + obj.ApSection3Name + "', " +
                          (obj.ApRequirementId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ApRequirementName + "', " +
                          "N'" + obj.ExecutionStatus + "', " +
                          "N'" + obj.PersonInChargeIds + "', " +
                          "N'" + obj.PersonInCharge + "', " +
                          (obj.StatusId?.ToString() ?? "null") + ", " +
                          "N'" + obj.StatusName + "', " +
                          (obj.Cost?.ToString() ?? "null") + ", " +
                          "N'" + obj.Remark + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Code + "', " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "Title=" + "N'" + obj.Title + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "DateRaised=" + "CASt('" + obj.DateRaised.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "Section1Id=" + (obj.Section1Id?.ToString() ?? "null") + ", " +
                        "Section1Name=" + "N'" + obj.Section1Name + "', " +

                        "Section2Id=" + (obj.Section2Id?.ToString() ?? "null") + ", " +
                        "Section2Name=" + "N'" + obj.Section2Name + "', " +

                        "Section3Id=" + (obj.Section3Id?.ToString() ?? "null") + ", " +
                        "Section3Name=" + "N'" + obj.Section3Name + "', " +

                        "Section4Id=" + (obj.Section4Id?.ToString() ?? "null") + ", " +
                        "Section4Name=" + "N'" + obj.Section4Name + "', " +

                        "Section5Id=" + (obj.Section5Id?.ToString() ?? "null") + ", " +
                        "Section5Name=" + "N'" + obj.Section5Name + "', " +

                        "ApSection3Id=" + (obj.ApSection3Id?.ToString() ?? "null") + ", " +
                        "ApSection3Name=" + "N'" + obj.ApSection3Name + "', " +

                        "ApRequirementId=" + (obj.ApRequirementId?.ToString() ?? "null") + ", " +
                        "ApRequirementName=" + "N'" + obj.ApRequirementName + "', " +

                        "ExecutionStatus=" + "N'" + obj.ExecutionStatus + "', " +
                        "PersonInChargeIds=" + "N'" + obj.PersonInChargeIds + "', " +
                        "PersonInCharge=" + "N'" + obj.PersonInCharge + "', " +
                        "StatusId=" + (obj.StatusId?.ToString() ?? "null") + ", " +
                        "StatusName=" + "N'" + obj.StatusName + "', " +
                        "Cost=" + (obj.Cost?.ToString() ?? "null") + ", " +
                        "Remark=" + "N'" + obj.Remark + "', " +

                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "Code=" + "N'" + obj.Code + "', " +

                        "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                        "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +

                        "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                        "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +
                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjBreakdownReportAttachChanged(WaitingSyncData changeInfo, TrackingBreakdownReportAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, BreakdownReportId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.BreakdownReportId + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FileSize?.ToString() ?? "null") + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjBreakdownReportChanged(WaitingSyncData changeInfo, TrackingBreakdownReport obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, BrekdownDate, BreakdownSystemName, TagNo, SystemName, Priority, CauseGroup, Description, FailureDuplication, RootCause, ProposedAction, Lesson, UnplannedWoNo, PICIds,PICName, PICDeadline, DeadlineReasonChange, PICStatus, CurrentStatus, MRWRItem, Status, Comment, Cost, CreatedBy, CreatedByName, CreatedDate, ProjectId, ProjectName, IsLeaf, IsComplete, ParentId, Code, IsWFComplete, IsInWFProcess, IsCancel, IsCompleteFinal, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          (obj.BrekdownDate != null ? "CASt('" + obj.BrekdownDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.BreakdownSystemName + "', " +
                          "N'" + obj.TagNo + "', " +
                          "N'" + obj.SystemName + "', " +
                          "N'" + obj.Priority + "', " +
                          "N'" + obj.CauseGroup + "', " +
                          "N'" + obj.Description + "', " +
                          "N'" + obj.FailureDuplication + "', " +
                          //(obj.FailureDuplication?.ToString() ?? "null") + ", " +
                          "N'" + obj.RootCause + "', " +
                          "N'" + obj.ProposedAction + "', " +
                          "N'" + obj.Lesson + "', " +
                          "N'" + obj.UnplannedWoNo + "', " +
                          "N'" + obj.PICIds + "', " +
                          "N'" + obj.PICName + "', " +
                          "N'" + obj.PICDeadline + "', " +
                          //(obj.PICDeadline != null ? "CASt('" + obj.PICDeadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          "N'" + obj.DeadlineReasonChange + "', " +
                          "N'" + obj.PICStatus + "', " +
                          "N'" + obj.CurrentStatus + "', " +
                          "N'" + obj.MRWRItem + "', " +
                          "N'" + obj.Status + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.Cost?.ToString() ?? "null") + ", " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.ParentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Code + "', " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                        "BrekdownDate=" + "CASt('" + obj.BrekdownDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "BreakdownSystemName=" + "N'" + obj.BreakdownSystemName + "', " +
                        "TagNo=" + "N'" + obj.TagNo + "', " +
                        "SystemName=" + "N'" + obj.SystemName + "', " +
                        "Priority=" + "N'" + obj.Priority + "', " +
                        "CauseGroup=" + "N'" + obj.CauseGroup + "', " +
                        "Description=" + "N'" + obj.Description + "', " +
                        "FailureDuplication=" + "N'" + obj.FailureDuplication + "', " +
                        //"FailureDuplication=" + (obj.FailureDuplication?.ToString() ?? "null") + ", " +
                        "RootCause=" + "N'" + obj.RootCause + "', " +
                        "ProposedAction=" + "N'" + obj.ProposedAction + "', " +
                        "Lesson=" + "N'" + obj.Lesson + "', " +
                        "UnplannedWoNo=" + "N'" + obj.UnplannedWoNo + "', " +
                        "PICIds=" + "N'" + obj.PICIds + "', " +
                        "PICName=" + "N'" + obj.PICName + "', " +
                        "PICDeadline=" + "N'" + obj.PICDeadline + "', " +
                        //"PICDeadline=" + "CASt('" + obj.PICDeadline.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                        "DeadlineReasonChange=" + "N'" + obj.DeadlineReasonChange + "', " +
                        "PICStatus=" + "N'" + obj.PICStatus + "', " +
                        "CurrentStatus=" + "N'" + obj.CurrentStatus + "', " +
                        "MRWRItem=" + "N'" + obj.MRWRItem + "', " +
                        "Status=" + "N'" + obj.Status + "', " +
                        "Comment=" + "N'" + obj.Comment + "', " +
                        "Cost=" + (obj.Cost?.ToString() ?? "null") + ", " +
                        "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                        "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                        "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                        "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                        "ProjectName=" + "N'" + obj.ProjectName + "', " +
                        "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                        "Code=" + "N'" + obj.Code + "', " +

                        "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                        "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                        "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                        "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                        "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +

                        "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                        "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +

                        "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetObjAssignWorkFlowChanged(WaitingSyncData changeInfo, ObjectAssignedWorkflow obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ObjectID, ObjectNumber, ObjectTitle, ObjectType, WorkflowID, WorkflowName, CurrentWorkflowStepID, CurrentWorkflowStepName, NextWorkflowStepID, NextWorkflowStepName, RejectWorkflowStepID, RejectWorkflowStepName, IsComplete, IsReject, IsLeaf, AssignedBy, CanReject, ObjectProject, RejectFromId) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ObjectID + "', " +
                          "N'" + obj.ObjectNumber + "', " +
                          "N'" + obj.ObjectTitle + "', " +
                          "N'" + obj.ObjectType + "', " +
                          (obj.WorkflowID?.ToString() ?? "null") + ", " +
                          "N'" + obj.WorkflowName + "', " +
                          (obj.CurrentWorkflowStepID?.ToString() ?? "null") + ", " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          (obj.NextWorkflowStepID?.ToString() ?? "null") + ", " +
                          "N'" + obj.NextWorkflowStepName + "', " +
                          (obj.RejectWorkflowStepID?.ToString() ?? "null") + ", " +
                          "N'" + obj.RejectWorkflowStepName + "', " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsReject.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.AssignedBy?.ToString() ?? "null") + ", " +
                          (obj.CanReject.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.ObjectProject + "', " +
                          (obj.RejectFromId?.ToString() ?? "null") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                          "ObjectID=" + "N'" + obj.ObjectID + "', " +
                          "ObjectNumber=" + "N'" + obj.ObjectNumber + "', " +
                          "ObjectTitle=" + "N'" + obj.ObjectTitle + "', " +
                          "ObjectType=" + "N'" + obj.ObjectType + "', " +
                          "WorkflowID=" + (obj.WorkflowID?.ToString() ?? "null") + ", " +
                          "WorkflowName=" + "N'" + obj.WorkflowName + "', " +
                          "CurrentWorkflowStepID=" + (obj.CurrentWorkflowStepID?.ToString() ?? "null") + ", " +
                          "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                          "NextWorkflowStepID=" + (obj.NextWorkflowStepID?.ToString() ?? "null") + ", " +
                          "NextWorkflowStepName=" + "N'" + obj.NextWorkflowStepName + "', " +
                          "RejectWorkflowStepID=" + (obj.RejectWorkflowStepID?.ToString() ?? "null") + ", " +
                          "RejectWorkflowStepName=" + "N'" + obj.RejectWorkflowStepName + "', " +
                          "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "IsReject=" + (obj.IsReject.GetValueOrDefault() ? 1 : 0) + ", " +
                          "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          "AssignedBy=" + (obj.AssignedBy?.ToString() ?? "null") + ", " +
                          "CanReject=" + (obj.CanReject.GetValueOrDefault() ? 1 : 0) + ", " +
                          "ObjectProject=" + "N'" + obj.ObjectProject + "', " +
                          "RejectFromId=" + (obj.RejectFromId?.ToString() ?? "null") + " " +
                          "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetObjAssignUserChanged(WaitingSyncData changeInfo, ObjectAssignedUser obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ObjectAssignedWorkflowID, ObjectID, ObjectNumber, ObjectTitle, ObjectType, UserID, ReceivedDate, PlanCompleteDate, IsOverDue, IsComplete, AssignedBy, CurrentWorkflowStepId, CurrentWorkflowStepName, WorkflowId, WorkflowName, IsReject, CanReject, ObjectProject, RejectFromId, IsOnshoreComment, IsDistributeOnshore, RefId, CommentContent,  ActionTypeId, ActionTypeName, WorkingStatus, IsFinal, FinalAssignDeptId, FinalAssignDeptName) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.ObjectAssignedWorkflowID.GetValueOrDefault() + "', " +
                          "N'" + obj.ObjectID + "', " +
                          "N'" + obj.ObjectNumber + "', " +
                          "N'" + obj.ObjectTitle + "', " +
                          "N'" + obj.ObjectType + "', " +
                          (obj.UserID?.ToString() ?? "null") + ", " +
                          (obj.ReceivedDate != null ? "CASt('" + obj.ReceivedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                          (obj.PlanCompleteDate != null ? "CASt('" + obj.PlanCompleteDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.IsOverDue.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.AssignedBy?.ToString() ?? "null") + ", " +
                          (obj.CurrentWorkflowStepId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          (obj.WorkflowId?.ToString() ?? "null") + ", " +
                          "N'" + obj.WorkflowName + "', " +
                          (obj.IsReject.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.CanReject.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.ObjectProject + "', " +
                          (obj.RejectFromId?.ToString() ?? "null") + ", " +
                          (obj.IsOnshoreComment.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.IsDistributeOnshore.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.RefId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentContent + "', " +

                          (obj.ActionTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ActionTypeName + "', " +
                          "N'" + obj.WorkingStatus + "', " +

                          (obj.IsFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "ObjectAssignedWorkflowID=" + "N'" + obj.ObjectAssignedWorkflowID + "', " +
                              "ObjectID=" + "N'" + obj.ObjectID + "', " +
                              "ObjectNumber=" + "N'" + obj.ObjectNumber + "', " +
                              "ObjectTitle=" + "N'" + obj.ObjectTitle + "', " +
                              "ObjectType=" + "N'" + obj.ObjectType + "', " +
                              "UserID=" + (obj.UserID?.ToString() ?? "null") + ", " +
                              "ReceivedDate=" + "CASt('" + obj.ReceivedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                              "PlanCompleteDate=" + "CASt('" + obj.PlanCompleteDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                              "IsOverDue=" + (obj.IsOverDue.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsComplete=" + (obj.IsComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                              "AssignedBy=" + (obj.AssignedBy?.ToString() ?? "null") + ", " +
                              "CurrentWorkflowStepId=" + (obj.CurrentWorkflowStepId?.ToString() ?? "null") + ", " +

                              "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                              "WorkflowId=" + (obj.WorkflowId?.ToString() ?? "null") + ", " +
                              "WorkflowName=" + "N'" + obj.WorkflowName + "', " +
                              "IsReject=" + (obj.IsReject.GetValueOrDefault() ? 1 : 0) + ", " +
                              "CanReject=" + (obj.CanReject.GetValueOrDefault() ? 1 : 0) + ", " +
                              "ObjectProject=" + "N'" + obj.ObjectProject + "', " +
                              "RejectFromId=" + (obj.RejectFromId?.ToString() ?? "null") + ", " +
                              "IsOnshoreComment=" + (obj.IsOnshoreComment.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsDistributeOnshore=" + (obj.IsDistributeOnshore.GetValueOrDefault() ? 1 : 0) + ", " +
                              "RefId=" + (obj.RefId?.ToString() ?? "null") + ", " +
                              "CommentContent=" + "N'" + obj.CommentContent + "', " +

                              "ActionTypeId=" + (obj.ActionTypeId?.ToString() ?? "null") + ", " +
                              "ActionTypeName=" + "N'" + obj.ActionTypeName + "', " +
                              "WorkingStatus=" + "N'" + obj.WorkingStatus + "', " +

                              "IsFinal=" + (obj.IsFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                              "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                              "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +
                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetNumberManagementChanged(WaitingSyncData changeInfo, NumberManagement obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, ObjectName, NextCount, ProjectId, ProjectName) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.ObjectName + "', " +
                            (obj.NextCount?.ToString() ?? "null") + ", " +
                            (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "N'" + obj.ProjectName + "' " +
                            "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "ObjectName=" + "N'" + obj.ObjectName + "', " +
                            "NextCount=" + (obj.NextCount?.ToString() ?? "null") + ", " +
                            "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "ProjectName=" + "N'" + obj.ProjectName + "' " +
                            "WHERE ID" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }

            return cmd;

        }

        private string GetMRCheckListDefineChanged(WaitingSyncData changeInfo, MaterialRequisitionCheckListDefine obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Description, ParentId) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Description + "', " +
                            (obj.ParentId?.ToString() ?? "null") + " " +
                            "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "ParentId=" + (obj.ParentId?.ToString() ?? "null") + " " +
                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }

            return cmd;

        }

        private string GetMRCommentChanged(WaitingSyncData changeInfo, MaterialRequisitionComment obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MRId, MRNo, CommentTypeId, CommentTypeName, Comment, CommentBy, CommentByName, CommentDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MRId.GetValueOrDefault() + "', " +
                          "N'" + obj.MRNo + "', " +
                          (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentTypeName + "', " +
                          "N'" + obj.Comment + "', " +
                          (obj.CommentBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CommentByName + "', " +
                          "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "MRId=" + "N'" + obj.MRId + "', " +
                              "MRNo=" + "N'" + obj.MRNo + "', " +
                              "CommentTypeId=" + (obj.CommentTypeId?.ToString() ?? "null") + ", " +
                              "CommentTypeName=" + "N'" + obj.CommentTypeName + "', " +
                              "Comment=" + "N'" + obj.Comment + "', " +
                              "CommentBy=" + (obj.CommentBy?.ToString() ?? "null") + ", " +
                              "CommentByName=" + "N'" + obj.CommentByName + "', " +
                              "CommentDate=" + "CASt('" + obj.CommentDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetMRAttachFileChanged(WaitingSyncData changeInfo, MaterialRequisitionAttachFile obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MRId, Filename, Extension, FilePath, ExtensionIcon, IsDefault, FileSize, Description, CreatedBy, CreatedByName, CreatedDate) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MRId.GetValueOrDefault() + "', " +
                          "N'" + obj.Filename + "', " +
                          "N'" + obj.Extension + "', " +
                          "N'" + obj.FilePath + "', " +
                          "N'" + obj.ExtensionIcon + "', " +
                          (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                          obj.FileSize + ", " +
                          "N'" + obj.Description + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "MRId=" + "N'" + obj.MRId + "', " +
                              "Filename=" + "N'" + obj.Filename + "', " +
                              "Extension=" + "N'" + obj.Extension + "', " +
                              "FilePath=" + "N'" + obj.FilePath + "', " +
                              "ExtensionIcon=" + "N'" + obj.ExtensionIcon + "', " +
                              "IsDefault=" + (obj.IsDefault.GetValueOrDefault() ? 1 : 0) + ", " +
                              "FileSize=" + obj.FileSize + ", " +
                              "Description=" + "N'" + obj.Description + "' " +
                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetUserChanged(WaitingSyncData changeInfo, User obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(Id, RoleId, Username, Password, FullName, Email, CreatedBy, CreatedDate, IsActive, TitleId, TitleName, CommentGroupId, LocationId, LocationName, ProjectId, ProjectName, IsDeptManager,CommentGroupName) VALUES(" +
                          obj.Id + ", " +
                          (obj.RoleId?.ToString() ?? "null") + ", " +
                          "N'" + obj.Username + "', " +
                          "N'" + obj.Password + "', " +
                          "N'" + obj.FullName + "', " +
                          "N'" + obj.Email + "', " +
                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime), " +
                          (obj.IsActive.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.TitleId?.ToString() ?? "null") + ", " +
                          "N'" + obj.TitleName + "', " +
                          (obj.CommentGroupId?.ToString() ?? "null") + ", " +
                          (obj.LocationId?.ToString() ?? "null") + ", " +
                          "N'" + obj.LocationName + "', " +
                            (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          (obj.IsDeptManager.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CommentGroupName + "' " +
                          "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "TypeId=" + (obj.RoleId?.ToString() ?? "null") + ", " +
                            "Username=" + "N'" + obj.Username + "', " +
                            "Password=" + "N'" + obj.Password + "', " +
                            "FullName=" + "N'" + obj.FullName + "', " +
                            "Email=" + "N'" + obj.Email + "', " +
                            "IsActive=" + (obj.IsActive.GetValueOrDefault() ? 1 : 0) + ", " +
                            "IsDeptManager=" + (obj.IsDeptManager.GetValueOrDefault() ? 1 : 0) + ", " +
                            "TitleId=" + (obj.TitleId?.ToString() ?? "null") + ", " +
                            "CommentGroupId=" + (obj.CommentGroupId?.ToString() ?? "null") + ", " +
                            "TitleName=" + "N'" + obj.TitleName + "', " +

                            "LocationId=" + (obj.LocationId?.ToString() ?? "null") + ", " +
                            "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "LocationName=" + "N'" + obj.LocationName + "', " +
                            "ProjectName=" + "N'" + obj.ProjectName + "', " +


                            "CommentGroupName=" + "N'" + obj.CommentGroupName + "', " +
                            "LastUpdatedBy=" + (obj.LastUpdatedBy?.ToString() ?? "null") + ", " +
                            "LasrUpdatedDate=" + (obj.LasrUpdatedDate != null ? "CASt('" + obj.LasrUpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                            "WHERE Id=" + obj.Id +
                              ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE Id=" + obj.Id + ";" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetRoleChanged(WaitingSyncData changeInfo, Role obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(Id, Name, Description, IsAdmin, IsUpdate, TypeId, TypeName, Color) VALUES(" +
                            obj.Id + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.TypeId?.ToString() ?? "null") + ", " +
                            (obj.IsUpdate.GetValueOrDefault() ? 1 : 0) + ", " +
                            (obj.IsAdmin.GetValueOrDefault() ? 1 : 0) + ", " +
                            "N'" + obj.TypeName + "', " +
                            "N'" + obj.Color + "' " +
                            "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Name=" + "N'" + obj.Name + "', " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "IsUpdate=" + (obj.IsUpdate.GetValueOrDefault() ? 1 : 0) + ", " +
                            "IsAdmin=" + (obj.IsAdmin.GetValueOrDefault() ? 1 : 0) + ", " +
                            "TypeId=" + (obj.TypeId?.ToString() ?? "null") + ", " +
                            "TypeName=" + "N'" + obj.TypeName + "', " +
                            "Color=" + "N'" + obj.Color + "' " +
                            "WHERE Id=" + obj.Id +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE Id=" + obj.Id + ";" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetPermissionChanged(WaitingSyncData changeInfo, Permission obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(Id, RoleId, MenuId) VALUES(" +
                          obj.Id + ", " +
                          obj.RoleId + ", " +
                          obj.MenuId + "); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "RoleId=" + obj.RoleId + ", " +
                              "MenuId=" + obj.MenuId + "" +
                              "WHERE Id=" + obj.Id +
                              ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE Id=" + obj.Id + ";" + Environment.NewLine;
                    break;
            }

            return cmd;
        }

        private string GetMenuChanged(WaitingSyncData changeInfo, Menu obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(Id, Name, Description, ParentId, Url, Priority, Type, Active, Icon) VALUES(" +
                            obj.Id + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.ParentId?.ToString() ?? "null") + ", " +
                            "N'" + obj.Url + "', " +
                            (obj.Priority?.ToString() ?? "null") + ", " +
                            (obj.Type?.ToString() ?? "null") + ", " +
                            (obj.Active.GetValueOrDefault() ? 1 : 0) + ", " +
                            "N'" + obj.Icon + "'); " + Environment.NewLine;
                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "Name=" + "N'" + obj.Name + "', " +
                              "Description=" + "N'" + obj.Description + "', " +
                              "ParentId=" + (obj.ParentId?.ToString() ?? "null") + ", " +
                              "Type=" + (obj.Type?.ToString() ?? "null") + ", " +
                              "Active=" + (obj.Active.GetValueOrDefault() ? 1 : 0) + ", " +
                              "Priority=" + (obj.Priority?.ToString() ?? "null") + ", " +
                              "Url=" + "N'" + obj.Url + "', " +
                              "Icon=" + "N'" + obj.Icon + "' " +
                              "WHERE Id=" + obj.Id +
                              ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE Id=" + obj.Id + ";" + Environment.NewLine;
                    break;
            }


            return cmd;

        }

        private string GetProjectChanged(WaitingSyncData changeInfo, ScopeProject obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Code, Name, Description,  CreatedBy, CreatedDate) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Code + "', " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                            ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Name=" + "N'" + obj.Name + "', " +
                            "Code=" + "N'" + obj.Code + "', " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                            "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetTitleChanged(WaitingSyncData changeInfo, Title obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Name, Description, LocationId, LocationName, CreatedBy, CreatedDate, CreatedName) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.LocationId?.ToString() ?? "null") + ", " +
                            "N'" + obj.LocationName + "', " +
                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + ", " +
                            "N'" + obj.CreatedName + "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Name=" + "N'" + obj.Name + "', " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "LocationId=" + (obj.LocationId?.ToString() ?? "null") + ", " +
                            "LocationName=" + "N'" + obj.LocationName + "', " +
                            "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                            "UpdatedName=" + "N'" + obj.UpdatedName + "', " +
                            "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetPriorityLevelChanged(WaitingSyncData changeInfo, PriorityLevel obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Name, Description, ProjectId, ProjectName, CreatedBy, CreatedDate, CreatedName) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.ProjectId?.ToString() ?? "null") + ", " +

                            "N'" + obj.ProjectName + "', " +
                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +
                            "N'" + obj.CreatedName + "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "Name=" + "N'" + obj.Name + "', " +
                              "Description=" + "N'" + obj.Description + "', " +
                              "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                              "ProjectName=" + "N'" + obj.ProjectName + "', " +
                              "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                              "UpdateByName=" + "N'" + obj.UpdateByName + "', " +
                              "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                              "WHERE ID=" + obj.ID +
                              ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetMRChanged(WaitingSyncData changeInfo, MaterialRequisition obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, ProjectId, ProjectName, AMOSWorkOrder, MRNo, DeparmentId, DepartmentName, Justification, MRTypeIds, MRTypeName, DateRequire, PriorityId, PriorityName, OriginatorId, OriginatorName, OriginatorDate, StoreManId, StoreManName, StoreManDate, SupervisorId, SupervisorName, SupervisorDate, OIMId, OIMName, OIMDate, Comment_PurchasingGroup, Comment_ReceivedMRFromFacility, Comment_MRProcessingCompleted, Comment_ForwardMRToTechDept, Comment_ReceivedMRFromTech, Comment_ForwardMRToPurchasingGroup, Comment_OperationSign, Comment_OperationSignName, Comment_OperationSignDate, Comment_TechSign, Comment_TechSignName, Comment_TechSignDate, Comment_DirectorSign, Comment_DirectorSignName, Comment_DirectorSignDate, CreatedBy, CreatedByName, CreatedByDate, IsWFComplete, IsInWFProcess, IsCancel, CurrentWorkflowName, CurrentWorkflowStepName, CurrentAssignUserName, IsCompleteFinal,FinalAssignDeptId, FinalAssignDeptName ) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          (obj.ProjectId?.ToString() ?? "null") + ", " +
                          "N'" + obj.ProjectName + "', " +
                          "N'" + obj.AMOSWorkOrder + "', " +
                          "N'" + obj.MRNo + "', " +
                          (obj.DeparmentId?.ToString() ?? "null") + ", " +
                          "N'" + obj.DepartmentName + "', " +
                          "N'" + obj.Justification + "', " +
                          "N'" + obj.MRTypeIds + "', " +
                          "N'" + obj.MRTypeName + "', " +
                          "N'" + obj.DateRequire + "', " +
                          (obj.PriorityId?.ToString() ?? "null") + ", " +
                          "N'" + obj.PriorityName + "', " +

                          (obj.OriginatorId?.ToString() ?? "null") + ", " +
                          "N'" + obj.OriginatorName + "', " +
                          "N'" + obj.OriginatorDate + "', " +
                          //(obj.OriginatorDate != null ? "CASt('" + obj.OriginatorDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.StoreManId?.ToString() ?? "null") + ", " +
                          "N'" + obj.StoreManName + "', " +
                          "N'" + obj.StoreManDate + "', " +
                           //(obj.StoreManDate != null ? "CASt('" + obj.StoreManDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                           (obj.SupervisorId?.ToString() ?? "null") + ", " +
                          "N'" + obj.SupervisorName + "', " +
                          "N'" + obj.SupervisorDate + "', " +
                          //(obj.SupervisorDate != null ? "CASt('" + obj.SupervisorDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.OIMId?.ToString() ?? "null") + ", " +
                          "N'" + obj.OIMName + "', " +
                          "N'" + obj.OIMDate + "', " +
                          //(obj.OIMDate != null ? "CASt('" + obj.OIMDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          "N'" + obj.Comment_PurchasingGroup + "', " +
                          (obj.Comment_ReceivedMRFromFacility != null ? "CASt('" + obj.Comment_ReceivedMRFromFacility.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.Comment_MRProcessingCompleted != null ? "CASt('" + obj.Comment_MRProcessingCompleted.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.Comment_ForwardMRToTechDept != null ? "CASt('" + obj.Comment_ForwardMRToTechDept.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.Comment_ReceivedMRFromTech != null ? "CASt('" + obj.Comment_ReceivedMRFromTech.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.Comment_ForwardMRToPurchasingGroup != null ? "CASt('" + obj.Comment_ForwardMRToPurchasingGroup.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.Comment_OperationSign?.ToString() ?? "null") + ", " +
                          "N'" + obj.Comment_OperationSignName + "', " +
                          (obj.Comment_OperationSignDate != null ? "CASt('" + obj.Comment_OperationSignDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.Comment_TechSign?.ToString() ?? "null") + ", " +
                          "N'" + obj.Comment_TechSignName + "', " +
                          (obj.Comment_TechSignDate != null ? "CASt('" + obj.Comment_TechSignDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.Comment_DirectorSign?.ToString() ?? "null") + ", " +
                          "N'" + obj.Comment_DirectorSignName + "', " +
                          (obj.Comment_DirectorSignDate != null ? "CASt('" + obj.Comment_DirectorSignDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          (obj.CreatedBy?.ToString() ?? "null") + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedByDate != null ? "CASt('" + obj.CreatedByDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                          "" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.CurrentWorkflowName + "', " +
                          "N'" + obj.CurrentWorkflowStepName + "', " +
                          "N'" + obj.CurrentAssignUserName + "', " +

                          "" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                          (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                          "N'" + obj.FinalAssignDeptName + "' " +

                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "ProjectName=" + "N'" + obj.ProjectName + "', " +
                              "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                              "AMOSWorkOrder=" + "N'" + obj.AMOSWorkOrder + "', " +
                              "MRNo=" + "N'" + obj.MRNo + "', " +
                              "DeparmentId=" + (obj.DeparmentId?.ToString() ?? "null") + ", " +
                              "DepartmentName=" + "N'" + obj.DepartmentName + "', " +
                              "Justification=" + "N'" + obj.Justification + "', " +
                              "MRTypeIds=" + "N'" + obj.MRTypeIds + "', " +
                              "MRTypeName=" + "N'" + obj.MRTypeName + "', " +
                              "DateRequire=" + "N'" + obj.DateRequire + "', " +
                              "PriorityName=" + "N'" + obj.PriorityName + "', " +
                              "PriorityId=" + (obj.PriorityId?.ToString() ?? "null") + ", " +

                              "OriginatorId=" + (obj.OriginatorId?.ToString() ?? "null") + ", " +
                              "OriginatorName=" + "N'" + obj.OriginatorName + "', " +
                              "OriginatorDate=" + "N'" + obj.OriginatorDate + "', " +
                              //"OriginatorDate=" + (obj.OriginatorDate != null ? "CASt('" + obj.OriginatorDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "StoreManId=" + (obj.StoreManId?.ToString() ?? "null") + ", " +
                              "StoreManName=" + "N'" + obj.StoreManName + "', " +
                              "StoreManDate=" + "N'" + obj.StoreManDate + "', " +
                              //"StoreManDate=" + (obj.StoreManDate != null ? "CASt('" + obj.StoreManDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "SupervisorId=" + (obj.SupervisorId?.ToString() ?? "null") + ", " +
                              "SupervisorName=" + "N'" + obj.SupervisorName + "', " +
                              "SupervisorDate=" + "N'" + obj.SupervisorDate + "', " +
                              //"SupervisorDate=" + (obj.SupervisorDate != null ? "CASt('" + obj.SupervisorDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "OIMId=" + (obj.OIMId?.ToString() ?? "null") + ", " +
                              "OIMName=" + "N'" + obj.OIMName + "', " +
                              "OIMDate=" + "N'" + obj.OIMDate + "', " +
                              //"OIMDate=" + (obj.OIMDate != null ? "CASt('" + obj.OIMDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_PurchasingGroup=" + "N'" + obj.Comment_PurchasingGroup + "', " +
                              "Comment_ReceivedMRFromFacility=" + (obj.Comment_ReceivedMRFromFacility != null ? "CASt('" + obj.Comment_ReceivedMRFromFacility.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_MRProcessingCompleted=" + (obj.Comment_MRProcessingCompleted != null ? "CASt('" + obj.Comment_MRProcessingCompleted.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_ForwardMRToTechDept=" + (obj.Comment_ForwardMRToTechDept != null ? "CASt('" + obj.Comment_ForwardMRToTechDept.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_ReceivedMRFromTech=" + (obj.Comment_ReceivedMRFromTech != null ? "CASt('" + obj.Comment_ReceivedMRFromTech.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_ForwardMRToPurchasingGroup=" + (obj.Comment_ForwardMRToPurchasingGroup != null ? "CASt('" + obj.Comment_ForwardMRToPurchasingGroup.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_OperationSign=" + (obj.Comment_OperationSign?.ToString() ?? "null") + ", " +
                              "Comment_OperationSignName=" + "N'" + obj.Comment_OperationSignName + "', " +
                              "Comment_OperationSignDate=" + (obj.Comment_OperationSignDate != null ? "CASt('" + obj.Comment_OperationSignDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_TechSign=" + (obj.Comment_TechSign?.ToString() ?? "null") + ", " +
                              "Comment_TechSignName=" + "N'" + obj.Comment_TechSignName + "', " +
                              "Comment_TechSignDate=" + (obj.Comment_TechSignDate != null ? "CASt('" + obj.Comment_TechSignDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "Comment_DirectorSign=" + (obj.Comment_DirectorSign?.ToString() ?? "null") + ", " +
                              "Comment_DirectorSignName=" + "N'" + obj.Comment_DirectorSignName + "', " +
                              "Comment_DirectorSignDate=" + (obj.Comment_DirectorSignDate != null ? "CASt('" + obj.Comment_DirectorSignDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                              "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                              "UpdatedByDate=" + (obj.UpdatedByDate != null ? "CASt('" + obj.UpdatedByDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +

                              "IsWFComplete=" + (obj.IsWFComplete.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsInWFProcess=" + (obj.IsInWFProcess.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                              "CurrentWorkflowName=" + "N'" + obj.CurrentWorkflowName + "', " +
                              "CurrentWorkflowStepName=" + "N'" + obj.CurrentWorkflowStepName + "', " +
                              "CurrentAssignUserName=" + "N'" + obj.CurrentAssignUserName + "', " +
                              "IsCompleteFinal=" + (obj.IsCompleteFinal.GetValueOrDefault() ? 1 : 0) + ", " +
                              "FinalAssignDeptId=" + (obj.FinalAssignDeptId?.ToString() ?? "null") + ", " +
                              "FinalAssignDeptName=" + "N'" + obj.FinalAssignDeptName + "' " +

                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetMRCheckListDetailChanged(WaitingSyncData changeInfo, MaterialRequisitionCheckListDetail obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MRId, MRCheckListId, IsYes, IsNo, IsNA, Remark) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MRId + "', " +
                          "" + obj.MRCheckListId + ", " +
                          "" + (obj.IsYes.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsNo.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsNA.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.Remark + "' " +
                          ");" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "MRId=" + "N'" + obj.MRId + "', " +
                              "MRCheckListId=" + obj.MRCheckListId + ", " +
                              "IsYes=" + (obj.IsYes.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsNo=" + (obj.IsYes.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsNA=" + (obj.IsYes.GetValueOrDefault() ? 1 : 0) + ", " +
                              "Remark=" + "N'" + obj.Remark + "' " +
                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetMRDetailChanged(WaitingSyncData changeInfo, MaterialRequisitionDetail obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                          "(ID, MRId, MRNo, ReqROBMin, ReqROBMax, ROB, QtyReq, QtyRemarkUseForJob, QtyRemarkForSpare, Units, SFICode, Description, MakerName, CertificateRequired, Alternative, NormalUsingFrequency, IsLeaf, IsCancel, ParentId, CreatedBy, CreatedByName, CreatedDate, PriorityName, MRRecieveDate, DepartmentId, DepartmentName, PICIds, PICName, ContractNumber, ReqQuotationDate, ReceiveQuotationDate, PONumber, POIssueDate, UnitPrice, TotalPrice, ExpectedDeliveryDate, DeliveryDateRevisedBySupplier, ActualDeliveryDate, SupplierName, MRPurchasingStatus, MRDetailPurchasingStatus, DateReq, Remarks) " +
                          "VALUES(" +
                          "N'" + obj.ID + "', " +
                          "N'" + obj.MRId.GetValueOrDefault() + "', " +
                          "N'" + obj.MRNo + "', " +
                          obj.ReqROBMin + ", " +
                          obj.ReqROBMax + ", " +
                          obj.ROB + ", " +
                          obj.QtyReq + ", " +
                          obj.QtyRemarkUseForJob + ", " +
                          obj.QtyRemarkForSpare + ", " +
                          "N'" + obj.Units + "', " +
                          "N'" + obj.SFICode + "', " +
                          "N'" + obj.Description + "', " +
                          "N'" + obj.MakerName + "', " +
                          "N'" + obj.CertificateRequired + "', " +
                          "N'" + obj.Alternative + "', " +
                          "N'" + obj.NormalUsingFrequency + "', " +

                          "" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                          "" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                          "N'" + obj.ParentId + "', " +
                          obj.CreatedBy + ", " +
                          "N'" + obj.CreatedByName + "', " +
                          (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          "N'" + obj.PriorityName + "', " +
                          (obj.MRRecieveDate != null ? "CASt('" + obj.MRRecieveDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          obj.DepartmentId + ", " +
                          "N'" + obj.DepartmentName + "', " +
                          "N'" + obj.PICIds + "', " +
                          "N'" + obj.PICName + "', " +
                          "N'" + obj.ContractNumber + "', " +
                          (obj.ReqQuotationDate != null ? "CASt('" + obj.ReqQuotationDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.ReceiveQuotationDate != null ? "CASt('" + obj.ReceiveQuotationDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          "N'" + obj.PONumber + "', " +
                          (obj.POIssueDate != null ? "CASt('" + obj.POIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          "N'" + obj.UnitPrice + "', " +
                          "N'" + obj.TotalPrice + "', " +
                          (obj.ExpectedDeliveryDate != null ? "CASt('" + obj.ExpectedDeliveryDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.DeliveryDateRevisedBySupplier != null ? "CASt('" + obj.DeliveryDateRevisedBySupplier.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          (obj.ActualDeliveryDate != null ? "CASt('" + obj.ActualDeliveryDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                          "N'" + obj.SupplierName + "', " +
                          "N'" + obj.MRPurchasingStatus + "', " +
                          "N'" + obj.MRDetailPurchasingStatus + "', " +
                          "N'" + obj.DateReq + "', " +

                          "N'" + obj.Remarks + "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                              "MRId=" + "N'" + obj.MRId + "', " +
                              "MRNo=" + "N'" + obj.MRNo + "', " +
                              "ReqROBMin=" + (obj.ReqROBMin?.ToString() ?? "null") + ", " +
                              "ReqROBMax=" + (obj.ReqROBMax?.ToString() ?? "null") + ", " +
                              "ROB=" + (obj.ROB?.ToString() ?? "null") + ", " +
                              "QtyReq=" + (obj.QtyReq?.ToString() ?? "null") + ", " +
                              "QtyRemarkUseForJob=" + (obj.QtyRemarkUseForJob?.ToString() ?? "null") + ", " +
                              "QtyRemarkForSpare=" + (obj.QtyRemarkForSpare?.ToString() ?? "null") + ", " +
                              "Units=" + "N'" + obj.Units + "', " +
                              "SFICode=" + "N'" + obj.SFICode + "', " +
                              "Description=" + "N'" + obj.Description + "', " +
                              "MakerName=" + "N'" + obj.MakerName + "', " +
                              "CertificateRequired=" + "N'" + obj.CertificateRequired + "', " +
                              "Alternative=" + "N'" + obj.Alternative + "', " +
                              "NormalUsingFrequency=" + "N'" + obj.NormalUsingFrequency + "', " +
                              "IsLeaf=" + (obj.IsLeaf.GetValueOrDefault() ? 1 : 0) + ", " +
                              "IsCancel=" + (obj.IsCancel.GetValueOrDefault() ? 1 : 0) + ", " +
                              "ParentId=" + "N'" + obj.ParentId + "', " +
                              "CreatedBy=" + (obj.CreatedBy?.ToString() ?? "null") + ", " +
                              "CreatedByName=" + "N'" + obj.CreatedByName + "', " +
                              "CreatedDate=" + (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "PriorityName=" + "N'" + obj.PriorityName + "', " +
                              "MRRecieveDate=" + (obj.MRRecieveDate != null ? "CASt('" + obj.MRRecieveDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "DepartmentId=" + (obj.DepartmentId?.ToString() ?? "null") + ", " +
                              "DepartmentName=" + "N'" + obj.DepartmentName + "', " +
                              "PICIds=" + "N'" + obj.PICIds + "', " +
                              "PICName=" + "N'" + obj.PICName + "', " +
                              "ContractNumber=" + "N'" + obj.ContractNumber + "', " +
                              "ReqQuotationDate=" + (obj.ReqQuotationDate != null ? "CASt('" + obj.ReqQuotationDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "ReceiveQuotationDate=" + (obj.ReceiveQuotationDate != null ? "CASt('" + obj.ReceiveQuotationDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "PONumber=" + "N'" + obj.PONumber + "', " +
                              "POIssueDate=" + (obj.POIssueDate != null ? "CASt('" + obj.POIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "UnitPrice=" + "N'" + obj.UnitPrice + "', " +
                              "TotalPrice=" + "N'" + obj.TotalPrice + "', " +
                              "ExpectedDeliveryDate=" + (obj.ExpectedDeliveryDate != null ? "CASt('" + obj.ExpectedDeliveryDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "DeliveryDateRevisedBySupplier=" + (obj.DeliveryDateRevisedBySupplier != null ? "CASt('" + obj.DeliveryDateRevisedBySupplier.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "ActualDeliveryDate=" + (obj.ActualDeliveryDate != null ? "CASt('" + obj.ActualDeliveryDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + ", " +
                              "SupplierName=" + "N'" + obj.SupplierName + "', " +
                              "MRPurchasingStatus=" + "N'" + obj.MRPurchasingStatus + "', " +
                              "MRDetailPurchasingStatus=" + "N'" + obj.MRDetailPurchasingStatus + "', " +
                              "DateReq=" + "N'" + obj.DateReq + "', " +

                              "Remarks=" + "N'" + obj.Remarks + "' " +
                              "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID='" + obj.ID + "';" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private string GetMRTypeChanged(WaitingSyncData changeInfo, MRType obj)
        {
            var cmd = string.Empty;
            switch (changeInfo.ActionTypeID)
            {
                case 1:
                    cmd = "INSERT INTO " + changeInfo.ObjectName +
                            "(ID, Name, Description, ProjectId, ProjectName, CreatedBy, CreatedDate, CreatedName) VALUES(" +
                            obj.ID + ", " +
                            "N'" + obj.Name + "', " +
                            "N'" + obj.Description + "', " +
                            (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "N'" + obj.ProjectName + "', " +
                            (obj.CreatedBy?.ToString() ?? "null") + ", " +
                            (obj.CreatedDate != null ? "CASt('" + obj.CreatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime) " : "null ") + " " +

                            "N'" + obj.CreatedName + "');" + Environment.NewLine;

                    break;
                case 2:
                    cmd = "Update " + changeInfo.ObjectName + " SET " +
                            "Name=" + "N'" + obj.Name + "', " +
                            "Description=" + "N'" + obj.Description + "', " +
                            "ProjectId=" + (obj.ProjectId?.ToString() ?? "null") + ", " +
                            "ProjectName=" + "N'" + obj.ProjectName + "', " +
                            "UpdatedBy=" + (obj.UpdatedBy?.ToString() ?? "null") + ", " +
                            "UpdatedByName=" + "N'" + obj.UpdatedByName + "', " +
                            "UpdatedDate=" + (obj.UpdatedDate != null ? "CASt('" + obj.UpdatedDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)" : "null") + " " +
                            "WHERE ID=" + obj.ID +
                            ";" + Environment.NewLine;
                    break;
                case 3:
                    cmd = "DELETE " + changeInfo.ObjectName + "WHERE ID=" + obj.ID + ";" + Environment.NewLine;
                    break;
            }


            return cmd;
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var dataList = this.syncWaitingService.GetAll();
            this.grdDocument.DataSource = dataList;
        }

        private void LoadSystemPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("SystemID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "SYSTEM" });

                this.radPbSystem.DataSource = permissions;
                this.radPbSystem.DataFieldParentID = "ParentId";
                this.radPbSystem.DataFieldID = "Id";
                this.radPbSystem.DataValueField = "Id";
                this.radPbSystem.DataTextField = "MenuName";
                this.radPbSystem.DataBind();
                this.radPbSystem.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbSystem.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Data Changed History")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void LoadScopePanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ScopeID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId).OrderBy(t => t.Menu.Priority).ToList();
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG MANAGEMENT" });

                this.radPbScope.DataSource = permissions;
                this.radPbScope.DataFieldParentID = "ParentId";
                this.radPbScope.DataFieldID = "Id";
                this.radPbScope.DataValueField = "Id";
                this.radPbScope.DataTextField = "MenuName";
                this.radPbScope.DataBind();
                this.radPbScope.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbScope.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    
                }
            }
        }
    }
}

