using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using EDMs.Data.Entities;
using Aspose.Cells;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data;

namespace InitDataTools
{
    class Program
    {
        private static readonly TrackingSailService SailListService = new TrackingSailService();
        private static readonly WorkRequestService WrService = new WorkRequestService();
        private static readonly ScopeProjectService ProjectService = new ScopeProjectService();
        private static readonly TrackingProductionMeetingService OperationMeetingService = new TrackingProductionMeetingService();
        private static readonly UserService UserService = new UserService();
        private static readonly TrackingMorningCallService MorningCallService = new TrackingMorningCallService();
        private static readonly TrackingProcedureService ProcedureService = new TrackingProcedureService();
        private static readonly TrackingProcedureAttachFileService ProcedureAttachService = new TrackingProcedureAttachFileService();
        private static readonly TrackingBreakdownReportService BrService = new TrackingBreakdownReportService();
        private static readonly TrackingECRService EcrService = new TrackingECRService();
        private static readonly TrackingMOCService MocService = new TrackingMOCService();
        private static readonly MaterialRequisitionService MrService = new MaterialRequisitionService();
        private static readonly MaterialRequisitionDetailService MrDetailService = new MaterialRequisitionDetailService();
        private static readonly MaterialRequisitionCheckListDetailService MrChecklistDetailService = new MaterialRequisitionCheckListDetailService();

            
        static void Main(string[] args)
        {
            // Input WR Data
            //InputWRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\WR\Tracking List Of WR FSO BD01.xlsm", ProjectService.GetById(23));
            //InputWRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\WR\Tracking List Of WR Lam Son.xlsm", ProjectService.GetById(21));
            //InputWRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\WR\Tracking List Of WR Ruby  (Data updated 26.04.17).xlsm", ProjectService.GetById(22));
            // ---------------------------------------------------------


            // Input Sail List Data
            //InputSailListData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking Sailist\Bien Dong.xlsm", ProjectService.GetById(23));
            //InputSailListData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking Sailist\Lam Son.xlsx", ProjectService.GetById(21));
            //InputSailListData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking Sailist\RuBy II.xlsx", ProjectService.GetById(22));
            // ---------------------------------------------------------

            // Input Operation Meeting Data
            //InputOperationMeetingData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking operation Meeting\Lam Son.xlsm", ProjectService.GetById(21));
            //InputOperationMeetingData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking operation Meeting\Ruby II.xlsm", ProjectService.GetById(22));
            //InputOperationMeetingData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking operation Meeting\Bien Dong.xlsm", ProjectService.GetById(23));
            //InputOperationMeetingData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking operation Meeting\General.xlsm", new ScopeProject() { ID = 0, Name = "For All Project", Code = "General" });
            // ---------------------------------------------------------


            // Input Morning Call Data
            //InputMorningCallData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking Morning Call\Tracking Morning Call FSO BD01.xlsm", ProjectService.GetById(23));
            //InputMorningCallData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking Morning Call\Tracking Morning Call Lam Son.xlsm", ProjectService.GetById(21));
            //InputMorningCallData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\Tracking Morning Call\Tracking Morning Call (Data updated for RBII 05.05.17).xlsm", ProjectService.GetById(22));
            // ---------------------------------------------------------

            // Input Procedure Data
            //InputProcedureData(@"D:\WMSProcedure\Bien Dong.xlsm", ProjectService.GetById(23));
            //InputProcedureData(@"D:\WMSProcedure\Lam Son.xlsm", ProjectService.GetById(21));
            //InputProcedureData(@"D:\WMSProcedure\Ruby II.xlsm", ProjectService.GetById(22));
            // ---------------------------------------------------------

            // Input BreakdownReport Data
            //InputBreakdownReportData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\BR\BD\Copy of Tracking Breakdown Report  (Update 26.4.2017).xlsm", ProjectService.GetById(23));
            //InputBreakdownReportData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\BR\LS\Tracking Breakdown Report  LS (Update 26 4 2017).xlsm", ProjectService.GetById(21));
            //InputBreakdownReportData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\BR\RB\FPSO RUBY II - TRACKING LIST OF BREAKDOWN REPORTS UPDATED 12 MAY 2017 - Cuong MH - Copy.xlsx", ProjectService.GetById(22));
            // ---------------------------------------------------------

            // Input ECR Data
            //InputECRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\ECR\BD\Tracking List Of ECR.xlsm", ProjectService.GetById(23));
            //InputECRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\ECR\LS\Tracking List Of ECR (LS).xlsm", ProjectService.GetById(21));
            //InputECRData(@"D:\WMSProcedure\Ruby II.xlsm", ProjectService.GetById(22));
            // ---------------------------------------------------------

            // Input MR Data
            //InputMRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\MR\Tracking List Of MR.xlsm", ProjectService.GetById(23));//DONE
            //InputMRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\MR\Copy of Tracking List Of MR  Lam Son (2).xlsm", ProjectService.GetById(21));//Done
            InputMRData(@"D:\01. Desktop\003. PPS - WMS\Init Data Form\WMS Data-09052017\MR\Tracking List Of MR RUBY II 2017.xlsm", ProjectService.GetById(22));
            // ---------------------------------------------------------

            Console.WriteLine("Completed!");
            Console.ReadLine();
        }

        static void InputMRData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable = dataSheet.Cells.ExportDataTable(3, 1, dataSheet.Cells.MaxRow, 24)
                    .AsEnumerable()
                    .Where(t => !string.IsNullOrEmpty(t["Column1"].ToString()))
                    .CopyToDataTable();
            var dsMRDetail = workbook.Worksheets[2];
            var dtMRDetail = dsMRDetail.Cells.ExportDataTable(3, 1, dsMRDetail.Cells.MaxRow, 15)
                                            .AsEnumerable()
                                            .Where(t => !string.IsNullOrEmpty(t["Column1"].ToString()));
            var dsCheckList = workbook.Worksheets[3];
            var dtCheckList = dsCheckList.Cells.ExportDataTable(4, 4, 21, 701).AsEnumerable().ToList();
            var mrList = new List<MaterialRequisition>();
            var mrDetailList = new List<MaterialRequisitionDetail>();
            var mrCheckListList = new List<MaterialRequisitionCheckListDetail>();
            var currentMRNo = string.Empty;
            var flag = false;
            var countObj = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var obj = new MaterialRequisition();
                    obj.ID = Guid.NewGuid();
                    obj.ProjectId = project.ID;
                    obj.ProjectName = project.Name;

                    obj.AMOSWorkOrder = dataRow["Column2"].ToString();
                    obj.MRNo = dataRow["Column3"].ToString();
                    obj.DepartmentName = dataRow["Column4"].ToString();
                    obj.Justification = dataRow["Column5"].ToString();
                    

                    if (!string.IsNullOrEmpty(dataRow["Column6"].ToString()))
                    {
                        obj.MRTypeIds += "1;";
                        obj.MRTypeName += "Planned Maintenance; ";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column7"].ToString()))
                    {
                        obj.MRTypeIds += "2;";
                        obj.MRTypeName += "Consumables; ";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column8"].ToString()))
                    {
                        obj.MRTypeIds += "3;";
                        obj.MRTypeName += "Tools; ";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column9"].ToString()))
                    {
                        obj.MRTypeIds += "4;";
                        obj.MRTypeName += "WR; ";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column10"].ToString()))
                    {
                        obj.MRTypeIds += "5;";
                        obj.MRTypeName += "ECR; ";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column11"].ToString()))
                    {
                        obj.MRTypeIds += "6;";
                        obj.MRTypeName += "Equipment Breakdown; ";
                    }

                    obj.DateRequire = dataRow["Column12"].ToString();

                    if (!string.IsNullOrEmpty(dataRow["Column13"].ToString()))
                    {
                        obj.PriorityId = 3;
                        obj.PriorityName = "Super Urgent";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column14"].ToString()))
                    {
                        obj.PriorityId = 2;
                        obj.PriorityName = "Urgent";
                    }

                    if (!string.IsNullOrEmpty(dataRow["Column15"].ToString()))
                    {
                        obj.PriorityId = 1;
                        obj.PriorityName = "Normal";
                    }

                    obj.OriginatorName = dataRow["Column16"].ToString();
                    obj.OriginatorDate = dataRow["Column17"].ToString();

                    obj.StoreManName = dataRow["Column18"].ToString();
                    obj.StoreManDate = dataRow["Column19"].ToString();

                    obj.SupervisorName = dataRow["Column20"].ToString();
                    obj.SupervisorDate = dataRow["Column21"].ToString();

                    obj.OIMName = dataRow["Column22"].ToString();
                    obj.OIMDate = dataRow["Column23"].ToString();

                    obj.IsWFComplete = false;
                    obj.IsInWFProcess = false;
                    obj.IsCompleteFinal = true;
                    obj.IsCancel = false;

                    obj.CreatedByName = "System admin";
                    obj.CreatedBy = 1;
                    obj.CreatedByDate = DateTime.Now;

                    mrList.Add(obj);
                    
                    // Get mrdetail info
                    var mrDetails = dtMRDetail.Where(t => t["Column1"].ToString().Trim() == dataRow["Column3"].ToString().Trim()).ToList();
                    currentMRNo = obj.MRNo;
                    foreach (var drMRDetail in mrDetails)
                    {
                        var mrDetailObj = new MaterialRequisitionDetail
                        {
                            ID = Guid.NewGuid(),
                            MRId = obj.ID,
                            MRNo = obj.MRNo,
                            DateReq = obj.DateRequire,
                            PriorityName = obj.PriorityName,
                            IsLeaf = true,
                            IsCancel = false,
                            CreatedBy = 1,
                            CreatedByName = "System admin",
                            CreatedDate = DateTime.Now,
                        };

                        mrDetailObj.ReqROBMin = Convert.ToDouble(drMRDetail["Column2"].ToString().Replace("'", string.Empty));
                        mrDetailObj.ReqROBMax = Convert.ToDouble(drMRDetail["Column3"].ToString().Replace("'", string.Empty));
                        mrDetailObj.ROB = Convert.ToDouble(drMRDetail["Column4"].ToString().Replace("'", string.Empty));
                        mrDetailObj.QtyReq = Convert.ToDouble(drMRDetail["Column5"].ToString().Replace("'", string.Empty));
                        mrDetailObj.QtyRemarkUseForJob = Convert.ToDouble(drMRDetail["Column6"].ToString().Replace("'", string.Empty));
                        mrDetailObj.QtyRemarkForSpare = Convert.ToDouble(drMRDetail["Column7"].ToString().Replace("'", string.Empty));
                        mrDetailObj.Units = drMRDetail["Column8"].ToString();
                        mrDetailObj.SFICode = drMRDetail["Column9"].ToString();

                        mrDetailObj.Description = drMRDetail["Column10"].ToString();
                        mrDetailObj.MakerName = drMRDetail["Column11"].ToString();
                        mrDetailObj.CertificateRequired = drMRDetail["Column12"].ToString();
                        mrDetailObj.Alternative = drMRDetail["Column13"].ToString();
                        mrDetailObj.NormalUsingFrequency = drMRDetail["Column14"].ToString();
                        mrDetailObj.Remarks = drMRDetail["Column15"].ToString();

                        mrDetailList.Add(mrDetailObj);
                    }
                    // -------------------------------------------------------------------------------------

                    // Get checklist info
                    for (int i = 0; i < 21; i++)
                    {
                        if (i != 0 && i != 9 && i != 14)
                        {
                            var mrCheckListDetailObj = new MaterialRequisitionCheckListDetail
                            {
                                ID = Guid.NewGuid(),
                                MRId = obj.ID,
                                MRCheckListId = i + 1,
                                IsYes = !string.IsNullOrEmpty(dtCheckList[i]["Column" + ((4 * countObj) + 1)].ToString()),
                                IsNo = !string.IsNullOrEmpty(dtCheckList[i]["Column" + ((4 * countObj) + 2)].ToString()),
                                IsNA = !string.IsNullOrEmpty(dtCheckList[i]["Column" + ((4 * countObj) + 3)].ToString()),
                                Remark = dtCheckList[i]["Column" + ((4 * countObj) + 4)].ToString()
                            };

                            mrCheckListList.Add(mrCheckListDetailObj);
                            
                        }
                    }
                    // -------------------------------------------------------------------------------------

                    countObj += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(currentMRNo + ": " + ex.Message);
                    throw;
                }
            }

            var temp = 0;
            foreach (var obj in mrList)
            {
                MrService.Insert(obj);
            }

            foreach (var obj in mrDetailList)
            {
                MrDetailService.Insert(obj);
            }

            foreach (var obj in mrCheckListList)
            {
                MrChecklistDetailService.Insert(obj);
            }
        }

        static void InputMOCData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable =
                dataSheet.Cells.ExportDataTable(2, 1, dataSheet.Cells.MaxRow, 15)
                    .AsEnumerable()
                    .Where(t => !string.IsNullOrEmpty(t["Column1"].ToString()))
                    .CopyToDataTable();
            var mocList = new List<TrackingMOC>();
            var flag = false;
            var count = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var obj = new TrackingMOC();
                    obj.ID = Guid.NewGuid();
                    obj.ProjectId = project.ID;
                    obj.ProjectName = project.Name;

                    obj.Code = string.Empty;

                    obj.SystemName = dataRow["Column2"].ToString();
                    obj.DescriptionOfChange = dataRow["Column3"].ToString();
                    obj.DateIssued = !string.IsNullOrEmpty(dataRow["Column4"].ToString()) ? (DateTime?)dataRow["Column4"] : null;
                    obj.ReasonOfChange = dataRow["Column5"].ToString();
                    obj.InitRisk = dataRow["Column6"].ToString();

                    switch (dataRow["Column7"].ToString())
                    {
                        case "High":
                        case "H":
                            obj.InitRiskLvlId = 1;
                            obj.InitRiskLvlName = "High";
                            break;
                        case "Low":
                        case "L":
                            obj.InitRiskLvlId = 2;
                            obj.InitRiskLvlName = "Low";

                            break;
                        case "Medium":
                        case "M":
                            obj.InitRiskLvlId = 3;
                            obj.InitRiskLvlName = "Medium";

                            break;
                        case "Very low":
                        case "VL":
                            obj.InitRiskLvlId = 4;
                            obj.InitRiskLvlName = "Very Low";

                            break;
                        case "Normal":
                            obj.InitRiskLvlId = 5;
                            obj.InitRiskLvlName = "Normal";

                            break;
                        default:
                            obj.InitRiskLvlId = 0;
                            obj.InitRiskLvlName = "";

                            break;
                    }

                    obj.MigrationAction = dataRow["Column8"].ToString();
                    switch (dataRow["Column9"].ToString())
                    {
                        case "High":
                        case "H":
                            obj.MigrationRiskLvlId = 1;
                            obj.MigrationRiskLvlName = "High";
                            break;
                        case "Low":
                        case "L":
                            obj.MigrationRiskLvlId = 2;
                            obj.MigrationRiskLvlName = "Low";

                            break;
                        case "Medium":
                        case "M":
                            obj.MigrationRiskLvlId = 3;
                            obj.MigrationRiskLvlName = "Medium";

                            break;
                        case "Very low":
                        case "VL":
                            obj.MigrationRiskLvlId = 4;
                            obj.MigrationRiskLvlName = "Very Low";

                            break;
                        case "Normal":
                            obj.MigrationRiskLvlId = 5;
                            obj.MigrationRiskLvlName = "Normal";

                            break;
                        default:
                            obj.MigrationRiskLvlId = 0;
                            obj.MigrationRiskLvlName = "";

                            break;
                    }
                    
                    obj.DateAccepted = dataRow["Column10"].ToString();
                    obj.TechAuthority = dataRow["Column11"].ToString();
                    obj.PICId = string.Empty;
                    obj.PICName = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column12"].ToString()))
                    {
                        foreach (var pic in dataRow["Column12"].ToString().Split('/'))
                        {
                            var picObj = UserService.GetUserByUsername(pic.ToLower().Trim());
                            if (picObj != null)
                            {
                                obj.PICId += picObj.Id + ";";
                                obj.PICName += picObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    switch (dataRow["Column13"].ToString())
                    {
                        case "Approved":
                            obj.StatusId = 1;
                            break;
                        case "Cancelled":
                            obj.StatusId = 2;
                            break;
                        case "Closed":
                            obj.StatusId = 3;
                            break;
                        case "Hold":
                            obj.StatusId = 4;
                            break;
                        case "Implementing":
                            obj.StatusId = 5;
                            break;
                        default:
                            obj.StatusId = 0;
                            break;
                    }

                    obj.StatusName = dataRow["Column13"].ToString();
                    obj.ClosedClarification = dataRow["Column14"].ToString();
                    obj.Remark = dataRow["Column15"].ToString();
                    
                    obj.IsWFComplete = false;
                    obj.IsInWFProcess = false;
                    obj.IsCompleteFinal = obj.StatusName == "Closed";
                    obj.IsCancel = obj.StatusName == "Cancelled";

                    obj.CreatedByName = "System admin";
                    obj.CreatedBy = 1;
                    obj.CreatedDate = obj.DateIssued;

                    mocList.Add(obj);
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var obj in mocList)
            {
                MocService.Insert(obj);
            }
        }

        static void InputECRData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable =
                dataSheet.Cells.ExportDataTable(3, 1, dataSheet.Cells.MaxRow, 18)
                    .AsEnumerable()
                    .Where(t => !string.IsNullOrEmpty(t["Column1"].ToString()))
                    .CopyToDataTable();
            var ecrList = new List<TrackingECR>();
            var flag = false;
            var count = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var obj = new TrackingECR();
                    obj.ID = Guid.NewGuid();
                    obj.ProjectId = project.ID;
                    obj.ProjectName = project.Name;

                    obj.Code = dataRow["Column18"].ToString();

                    obj.Title = dataRow["Column2"].ToString();
                    obj.Description = dataRow["Column3"].ToString();
                    obj.DateRaised = !string.IsNullOrEmpty(dataRow["Column4"].ToString()) ? (DateTime?)dataRow["Column4"] : null;
                    obj.PriorityId = 0;
                    obj.PriorityName = string.Empty;

                    obj.Section1Id = dataRow["Column6"].ToString() == "Yes" ? 1 : (dataRow["Column6"].ToString() == "No" ? 2 : 0);
                    obj.Section1Name = dataRow["Column6"].ToString();
                    obj.Section2Id = dataRow["Column7"].ToString() == "Yes" ? 1 : (dataRow["Column7"].ToString() == "No" ? 2 : 0);
                    obj.Section2Name = dataRow["Column7"].ToString();
                    obj.Section3Id = dataRow["Column8"].ToString() == "Yes" ? 1 : (dataRow["Column8"].ToString() == "No" ? 2 : 0);
                    obj.Section3Name = dataRow["Column8"].ToString();
                    obj.Section4Id = dataRow["Column9"].ToString() == "Yes" ? 1 : (dataRow["Column9"].ToString() == "No" ? 2 : 0);
                    obj.Section4Name = dataRow["Column9"].ToString();
                    obj.Section5Id = dataRow["Column10"].ToString() == "Yes" ? 1 : (dataRow["Column10"].ToString() == "No" ? 2 : 0);
                    obj.Section5Name = dataRow["Column10"].ToString();

                    obj.ApSection3Id = dataRow["Column11"].ToString() == "Yes" ? 1 : (dataRow["Column11"].ToString() == "No" ? 2 : 0);
                    obj.ApSection3Name = dataRow["Column11"].ToString();
                    obj.ApRequirementId = dataRow["Column12"].ToString() == "Yes" ? 1 : (dataRow["Column12"].ToString() == "No" ? 2 : 0);
                    obj.ApRequirementName = dataRow["Column12"].ToString();

                    obj.ExecutionStatus = dataRow["Column13"].ToString();

                    obj.PersonInChargeIds = string.Empty;
                    obj.PersonInCharge = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column14"].ToString()))
                    {
                        foreach (var pic in dataRow["Column14"].ToString().Split('/'))
                        {
                            var picObj = UserService.GetUserByUsername(pic.ToLower().Trim());
                            if (picObj != null)
                            {
                                obj.PersonInChargeIds += picObj.Id + ";";
                                obj.PersonInCharge += picObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    switch (dataRow["Column15"].ToString())
                    {
                        case "Open":
                            obj.StatusId = 1;
                            break;
                        case "Closed":
                            obj.StatusId = 2;
                            break;
                        case "Hold":
                            obj.StatusId = 3;
                            break;
                        case "Cancelled":
                            obj.StatusId = 4;
                            break;
                        default:
                            obj.StatusId = 0;
                            break;
                    }

                    obj.StatusName = dataRow["Column15"].ToString();
                    obj.Remark = dataRow["Column17"].ToString();

                    obj.IsWFComplete = false;
                    obj.IsInWFProcess = false;
                    obj.IsCompleteFinal = obj.StatusName == "Closed";
                    obj.IsCancel = obj.StatusName == "Cancelled";

                    obj.CreatedByName = "System admin";
                    obj.CreatedBy = 1;
                    obj.CreatedDate = obj.DateRaised;

                    ecrList.Add(obj);
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var obj in ecrList)
            {
                EcrService.Insert(obj);
            }
        }

        static void InputBreakdownReportData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable = dataSheet.Cells.ExportDataTable(3, 1, dataSheet.Cells.MaxRow, 22).AsEnumerable().Where(t => !string.IsNullOrEmpty(t["Column1"].ToString())).CopyToDataTable();
            var brList = new List<TrackingBreakdownReport>();
            var attachFileList = new List<string>();
            var flag = false;
            var count = 1;
            var currentCode = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var brObj = new TrackingBreakdownReport();
                    currentCode = dataRow["Column2"].ToString();
                    brObj.ID = Guid.NewGuid();
                    brObj.ProjectId = project.ID;
                    brObj.ProjectName = project.Name;

                    brObj.BrekdownDate = !string.IsNullOrEmpty(dataRow["Column3"].ToString()) ? (DateTime?)dataRow["Column3"] : null;
                    brObj.BreakdownSystemName = dataRow["Column4"].ToString();
                    brObj.TagNo = dataRow["Column5"].ToString();
                    brObj.SystemName = dataRow["Column6"].ToString();

                    switch (dataRow["Column7"].ToString())
                    {
                        case "1":
                            brObj.Priority = "1 - Super Critical";
                            break;
                        case "2":
                            brObj.Priority = "2 - Critical";
                            break;
                        case "3":
                            brObj.Priority = "3 - Normal";
                            break;
                    }

                    brObj.CauseGroup = dataRow["Column8"].ToString();
                    brObj.Description = dataRow["Column9"].ToString();
                    brObj.FailureDuplication = dataRow["Column10"].ToString();
                    brObj.RootCause = dataRow["Column11"].ToString();
                    brObj.ProposedAction = dataRow["Column12"].ToString();
                    brObj.Lesson = dataRow["Column13"].ToString();
                    brObj.UnplannedWoNo = dataRow["Column14"].ToString();

                    brObj.PICIds = string.Empty;
                    brObj.PICName = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column15"].ToString()))
                    {
                        foreach (var pic in dataRow["Column15"].ToString().Split('/'))
                        {
                            var picObj = UserService.GetUserByUsername(pic.Trim());
                            if (picObj != null)
                            {
                                brObj.PICIds += picObj.Id + ";";
                                brObj.PICName += picObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    brObj.PICDeadline = dataRow["Column16"].ToString();
                    brObj.PICStatus = dataRow["Column17"].ToString();
                    brObj.CurrentStatus = dataRow["Column18"].ToString();
                    brObj.Status = dataRow["Column19"].ToString() == "C" ? "Closed" : "Open";
                    brObj.MRWRItem = dataRow["Column20"].ToString();
                    brObj.Comment = dataRow["Column21"].ToString();
                    brObj.Cost = 0;
                    brObj.DeadlineReasonChange = string.Empty;
                    brObj.Code = dataRow["Column2"].ToString();

                    brObj.IsWFComplete = false;
                    brObj.IsInWFProcess = false;
                    brObj.IsCompleteFinal = brObj.Status == "Closed";
                    brObj.IsCancel = false;

                    brObj.CreatedByName = "System admin";
                    brObj.CreatedBy = 1;
                    brObj.CreatedDate = DateTime.Now;
                    attachFileList.Add(dataRow["Column22"].ToString());
                    brList.Add(brObj);
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(currentCode + ": " + ex.Message);
                    throw;
                }
            }

            foreach (var brObj in brList)
            {
                BrService.Insert(brObj);
            }
        }

        static void InputProcedureData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[0];
            var dataTable = dataSheet.Cells.ExportDataTable(3, 1, dataSheet.Cells.MaxRow, 23).AsEnumerable().Where(t => !string.IsNullOrEmpty(t["Column1"].ToString())).CopyToDataTable();
            var procedureList = new List<TrackingProcedure>();
            var attachList = new List<string>();
            var flag = false;
            var count = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var procedureObj = new TrackingProcedure();
                    procedureObj.ID = Guid.NewGuid();
                    procedureObj.ProjectId = project.ID;
                    procedureObj.ProjectName = project.Name;

                    procedureObj.SystemName = dataRow["Column2"].ToString();
                    procedureObj.OldCode = dataRow["Column3"].ToString();
                    procedureObj.NewCode = dataRow["Column4"].ToString();
                    procedureObj.TypeId = 0;
                    procedureObj.TypeName = String.Empty;
                    procedureObj.ProcedureName = dataRow["Column6"].ToString();

                    
                    

                    procedureObj.PICIds = string.Empty;
                    procedureObj.PICName = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column7"].ToString()))
                    {
                        foreach (var pic in dataRow["Column7"].ToString().Split('/'))
                        {
                            var picObj = UserService.GetUserByUsername(pic.Trim());
                            if (picObj != null)
                            {
                                procedureObj.PICIds += picObj.Id + ";";
                                procedureObj.PICName += picObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    procedureObj.CheckerIds = string.Empty;
                    procedureObj.Checker = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column8"].ToString()))
                    {
                        foreach (var manager in dataRow["Column8"].ToString().Split('/'))
                        {
                            var managerObj = UserService.GetUserByUsername(manager.Trim());
                            if (managerObj != null)
                            {
                                procedureObj.CheckerIds += managerObj.Id + ";";
                                procedureObj.Checker += managerObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    procedureObj.TargerStageId = 0;
                    procedureObj.TargerStage = String.Empty;

                    procedureObj.StartDate = dataRow["Column10"].ToString();
                    procedureObj.CompleteDate = dataRow["Column11"].ToString();
                    procedureObj.TotalPage = !string.IsNullOrEmpty(dataRow["Column12"].ToString()) ? Convert.ToInt32(dataRow["Column12"].ToString()) : 0;
                    procedureObj.DifficultLvl = dataRow["Column13"].ToString();
                    //procedureObj.OfficeManday = dataRow["Column14"].ToString();
                    //procedureObj.OffshoreManDay = dataRow["Column15"].ToString();
                    procedureObj.CreateType = dataRow["Column16"].ToString();
                    //procedureObj.PercentComplete = dataRow["Column17"].ToString();
                    procedureObj.Status = dataRow["Column18"].ToString();
                    procedureObj.Deadline = dataRow["Column19"].ToString();
                    procedureObj.UpdatedInAMOS = dataRow["Column20"].ToString();
                    procedureObj.Remark = dataRow["Column21"].ToString();
                    procedureObj.LevelName = dataRow["Column22"].ToString();

                    procedureObj.IsComplete = false;
                    procedureObj.IsLeaf = true;
                    procedureObj.Code = project.Code + "/" + ReturnSequenceString(count, 4) + "/" + DateTime.Now.ToString("yy");

                    procedureObj.CreatedByName = "System admin";
                    procedureObj.CreatedBy = 1;
                    procedureObj.CreatedDate = DateTime.Now;

                    procedureList.Add(procedureObj);
                    attachList.Add(dataRow["Column23"].ToString());
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var procedureObj in procedureList)
            {
                ProcedureService.Insert(procedureObj);
            }
        }

        static void InputMorningCallData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable = dataSheet.Cells.ExportDataTable(3, 1, dataSheet.Cells.MaxRow, 16).AsEnumerable().Where(t => !string.IsNullOrEmpty(t["Column1"].ToString())).CopyToDataTable();
            var morningCallList = new List<TrackingMorningCall>();
            var flag = false;
            var count = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var morningCallObj = new TrackingMorningCall();
                    morningCallObj.ID = Guid.NewGuid();
                    morningCallObj.ProjectId = project.ID;
                    morningCallObj.ProjectName = project.Name;

                    morningCallObj.DateRaised = !string.IsNullOrEmpty(dataRow["Column2"].ToString()) ? (DateTime?)dataRow["Column2"] : null; ;
                    morningCallObj.EquipmentName = dataRow["Column3"].ToString();
                    morningCallObj.IssueDescription = dataRow["Column4"].ToString();
                    morningCallObj.InitRiskAssessment = dataRow["Column5"].ToString();

                    switch (dataRow["Column6"].ToString())
                    {
                        case "H":
                            morningCallObj.InitRiskLvlId = 1;
                            morningCallObj.InitRiskLvlName = "High";
                            break;
                        case "L":
                            morningCallObj.InitRiskLvlId = 2;
                            morningCallObj.InitRiskLvlName = "Low";
                            break;
                        case "M":
                            morningCallObj.InitRiskLvlId = 3;
                            morningCallObj.InitRiskLvlName = "Medium";
                            break;
                    }

                    
                    morningCallObj.ActionPlanDescription = dataRow["Column7"].ToString();
                    switch (dataRow["Column8"].ToString())
                    {
                        case "H":
                            morningCallObj.ActionPlanRiskLvlId = 1;
                            morningCallObj.ActionPlanRiskLvlName = "High";
                            break;
                        case "L":
                            morningCallObj.ActionPlanRiskLvlId = 2;
                            morningCallObj.ActionPlanRiskLvlName = "Low";
                            break;
                        case "M":
                            morningCallObj.ActionPlanRiskLvlId = 3;
                            morningCallObj.ActionPlanRiskLvlName = "Medium";
                            break;
                    }

                    morningCallObj.Deadline = dataRow["Column10"].ToString();
                    morningCallObj.DeadlineHistory = dataRow["Column11"].ToString();
                    morningCallObj.RelativeDoc = dataRow["Column12"].ToString();
                    morningCallObj.CurrentUpdate = dataRow["Column13"].ToString();
                    morningCallObj.OffshoreComment = dataRow["Column15"].ToString();
                    switch (dataRow["Column14"].ToString())
                    {
                        case "Closed":
                            morningCallObj.StatusId = 1;
                            break;
                        case "Open":
                            morningCallObj.StatusId = 2;
                            break;
                        case "Continous":
                            morningCallObj.StatusId = 3;
                            break;
                        case "Cancel":
                            morningCallObj.StatusId = 4;
                            break;
                        default:
                            morningCallObj.StatusId = 0;
                            break;
                    }

                    morningCallObj.StatusName = dataRow["Column14"].ToString();


                    morningCallObj.PICId = string.Empty;
                    morningCallObj.PICName = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column9"].ToString()))
                    {
                        foreach (var pic in dataRow["Column9"].ToString().Split('/').Where(t => !string.IsNullOrEmpty(t)))
                        {
                            var picObj = UserService.GetUserByUsername(pic.Trim());
                            if (picObj != null)
                            {
                                morningCallObj.PICId += picObj.Id + ";";
                                morningCallObj.PICName += picObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    morningCallObj.IsComplete = morningCallObj.StatusName.ToLower().Contains("closed");
                    morningCallObj.IsLeaf = true;
                    morningCallObj.Code = project.Code + "/" + dataRow["Column16"].ToString() + "/" + DateTime.Now.ToString("yy");

                    morningCallObj.CreatedByName = "System admin";
                    morningCallObj.CreatedBy = 1;
                    morningCallObj.CreatedDate = morningCallObj.DateRaised;

                    morningCallList.Add(morningCallObj);
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var operationMeetingObj in morningCallList)
            {
                MorningCallService.Insert(operationMeetingObj);
            }
        }

        static void InputOperationMeetingData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[0];
            var dataTable = dataSheet.Cells.ExportDataTable(2, 1, dataSheet.Cells.MaxRow, 10).AsEnumerable().Where(t => !string.IsNullOrEmpty(t["Column1"].ToString())).CopyToDataTable();
            var operationMeetingList = new List<TrackingProductionMeeting>();
            var flag = false;
            var count = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var operationMeetingObj = new TrackingProductionMeeting();
                    operationMeetingObj.ID = Guid.NewGuid();
                    operationMeetingObj.ProjectId = project.ID;
                    operationMeetingObj.ProjectName = project.Name;

                    operationMeetingObj.WorkGroup = dataRow["Column1"].ToString();
                    operationMeetingObj.MainProblem = dataRow["Column2"].ToString();
                    operationMeetingObj.BODCommand = dataRow["Column3"].ToString();


                    operationMeetingObj.DeparmentName = dataRow["Column4"].ToString();
                    operationMeetingObj.Deadline = dataRow["Column7"].ToString();
                    operationMeetingObj.UpdateComment = dataRow["Column8"].ToString();
                    operationMeetingObj.Note = dataRow["Column9"].ToString();
                    operationMeetingObj.ManagerIds = string.Empty;
                    operationMeetingObj.ManagerName = string.Empty;

                    if (!string.IsNullOrEmpty(dataRow["Column5"].ToString()))
                    {
                        foreach (var manager in dataRow["Column5"].ToString().Split('/'))
                        {
                            var managerObj = UserService.GetUserByUsername(manager);
                            if (managerObj != null)
                            {
                                operationMeetingObj.ManagerIds += managerObj.Id + ";";
                                operationMeetingObj.ManagerName += managerObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }

                    operationMeetingObj.PICIds = string.Empty;
                    operationMeetingObj.PICName = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow["Column6"].ToString()))
                    {
                        foreach (var pic in dataRow["Column6"].ToString().Split('/'))
                        {
                            var picObj = UserService.GetUserByUsername(pic);
                            if (picObj != null)
                            {
                                operationMeetingObj.PICIds += picObj.Id + ";";
                                operationMeetingObj.PICName += picObj.FullNameWithDeptPosition + Environment.NewLine;
                            }
                        }
                    }
                    
                    switch (dataRow["Column10"].ToString())
                    {
                        case "Closed":
                            operationMeetingObj.StatusId = 1;
                            break;
                        case "Open":
                            operationMeetingObj.StatusId = 2;
                            break;
                        case "Continous":
                            operationMeetingObj.StatusId = 3;
                            break;
                        case "Cancel":
                            operationMeetingObj.StatusId = 4;
                            break;
                        default:
                            operationMeetingObj.StatusId = 0;
                            break;
                    }

                    operationMeetingObj.StatusName = dataRow["Column10"].ToString();


                    operationMeetingObj.IsComplete = operationMeetingObj.StatusName.ToLower().Contains("closed");
                    operationMeetingObj.IsLeaf = true;
                    operationMeetingObj.Code = project.Code + "/" + ReturnSequenceString(count, 4) + "/" + DateTime.Now.ToString("yy");

                    operationMeetingObj.CreatedByName = "System admin";
                    operationMeetingObj.CreatedBy = 1;
                    operationMeetingObj.CreatedDate = DateTime.Now;

                    operationMeetingList.Add(operationMeetingObj);
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var operationMeetingObj in operationMeetingList)
            {
                OperationMeetingService.Insert(operationMeetingObj);
            }
        }

        static void InputSailListData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable = dataSheet.Cells.ExportDataTable(2, 1, dataSheet.Cells.MaxRow, 18).AsEnumerable().Where(t => !string.IsNullOrEmpty(t["Column1"].ToString())).CopyToDataTable();
            var sailList = new List<TrackingSail>();
            var flag = false;
            var count = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var sailObj = new TrackingSail();
                    sailObj.ID = Guid.NewGuid();
                    sailObj.ProjectId = project.ID;
                    sailObj.ProjectName = project.Name;

                    sailObj.DateRaised = !string.IsNullOrEmpty(dataRow["Column2"].ToString()) ? (DateTime?)dataRow["Column2"] : null;
                    sailObj.SourceName = dataRow["Column3"].ToString();
                    sailObj.NameObserver = dataRow["Column4"].ToString();
                    sailObj.Location = dataRow["Column5"].ToString();
                    sailObj.Description = dataRow["Column6"].ToString();
                    sailObj.Action = dataRow["Column7"].ToString();
                    sailObj.ProposedAction = dataRow["Column8"].ToString();
                    sailObj.Priority = dataRow["Column9"].ToString();
                    sailObj.TargetClose = dataRow["Column10"].ToString();
                    sailObj.ActionTakeClose = dataRow["Column11"].ToString();
                    sailObj.ClosedDate = dataRow["Column12"].ToString();
                    sailObj.HOCTrackingNo = dataRow["Column14"].ToString();
                    sailObj.MSRStatus = dataRow["Column15"].ToString();

                    sailObj.WRNo = dataRow["Column16"].ToString();
                    sailObj.MOCNo = dataRow["Column17"].ToString();
                    sailObj.ECRNo = dataRow["Column18"].ToString();

                    sailObj.PICIds = string.Empty;
                    sailObj.PICName = string.Empty;

                    sailObj.IsComplete = sailObj.ClosedDate.ToLower().Contains("closed");
                    sailObj.IsLeaf = true;
                    sailObj.Code = project.Code + "/" + ReturnSequenceString(count, 4) + "/" + DateTime.Now.ToString("yy");

                    sailObj.CreatedByName = "System admin";
                    sailObj.CreatedBy = 1;
                    sailObj.CreatedDate = sailObj.DateRaised;

                    sailList.Add(sailObj);
                    count += 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var sailObj in sailList)
            {
                SailListService.Insert(sailObj);
            }
        }

        static void InputWRData(string fileName, ScopeProject project)
        {
            var workbook = new Workbook();
            workbook.Open(fileName);

            var dataSheet = workbook.Worksheets[1];
            var dataTable = dataSheet.Cells.ExportDataTable(2, 1, dataSheet.Cells.MaxRow, 11).AsEnumerable().Where(t => !string.IsNullOrEmpty(t["Column1"].ToString())).CopyToDataTable();
            var wrList = new List<WorkRequest>();
            var flag = false;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    var wrObj = new WorkRequest();
                    wrObj.ID = Guid.NewGuid();
                    wrObj.ProjectId = project.ID;
                    wrObj.ProjectName = project.Name;
                    wrObj.WRNo = project.Code + "/" + ReturnSequenceString(Convert.ToInt32(dataRow["Column2"].ToString()), 4) + "/" + DateTime.Now.ToString("yy");
                    wrObj.WRTitle = dataRow["Column3"].ToString();
                    wrObj.DepartmentId = 0;
                    wrObj.DepartmentName = "";
                    wrObj.OriginatorName = dataRow["Column4"].ToString();
                    wrObj.OriginatorJobTitle = dataRow["Column5"].ToString();
                    wrObj.RaisedDate = !string.IsNullOrEmpty(dataRow["Column6"].ToString()) ? (DateTime?)dataRow["Column6"] : null;
                    wrObj.RequriedDate = dataRow["Column7"].ToString();
                    wrObj.Description = dataRow["Column8"].ToString();
                    wrObj.ScopeOfService = dataRow["Column9"].ToString();
                    wrObj.Reason = dataRow["Column10"].ToString();
                    wrObj.PriotiyLevelId = 0;
                    wrObj.PriorityLevelName = "";

                    wrObj.IsWFComplete = false;
                    wrObj.IsInWFProcess = false;
                    wrObj.IsCompleteFinal = dataRow["Column11"].ToString().ToLower() != "cancelled";
                    wrObj.IsCancel = dataRow["Column11"].ToString().ToLower() == "cancelled";

                    wrList.Add(wrObj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            foreach (var wrObj in wrList)
            {
                WrService.Insert(wrObj);
            }
        }

        public static string ReturnSequenceString(int currentSequence, int lenght)
        {
            var strSequence = string.Empty;
            for (int i = currentSequence.ToString().Length; i < lenght; i++)
            {
                strSequence += "0";
            }

            strSequence += currentSequence.ToString();
            return strSequence;
        }
    }
}
