// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using EDMs.Business.Services.Scope;
using EDMs.Web.Utilities;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportEMDR : Page
    {
        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        private readonly PackageService packageService;

        private readonly RoleService roleService;

        private readonly DocumentPackageService documentPackageService;

        private readonly ScopeProjectService scopeProjectService;
        private readonly WorkGroupService workGroupService;

        private readonly StatusService statusService;

        private readonly TransmittalService transmittalService;
        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>

        private readonly DepartmentService departmentService;
        private readonly PlatformService platformService;

        private readonly ProcessActualService processActualService;
        public ImportEMDR()
        {
            this.revisionService = new RevisionService();
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.documentService = new DocumentService();
            this.packageService = new PackageService();
            this.roleService = new RoleService();
            this.documentPackageService = new DocumentPackageService();
            this.scopeProjectService = new ScopeProjectService();
            this.workGroupService = new WorkGroupService();
            this.departmentService = new DepartmentService();
            this.platformService = new PlatformService();
            this.statusService = new StatusService();
            this.transmittalService = new TransmittalService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.processActualService = new ProcessActualService();
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
            if (!this.IsPostBack)
            {
            }
        }


        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var currentFileName = string.Empty;
            var currentDocumentNo = string.Empty;

            var documentPackageList = new List<DocumentPackage>();
            var newdocumentPackageList = new List<DocumentPackage>();
            var projectObj=new ScopeProject();;
            try
            {
                foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                {
                    currentFileName = docFile.FileName;
                    var extension = docFile.GetExtension();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {
                        var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") +
                                         "_" + docFile.FileName;
                        docFile.SaveAs(importPath);

                        // Instantiate a new workbook
                        var workbook = new Workbook();
                        workbook.Open(importPath);
                        var wsData = workbook.Worksheets[2];

                        var projectId = Convert.ToInt32(wsData.Cells["A1"].Value);

                         projectObj = this.scopeProjectService.GetById(projectId);
                       

                        // Create a datatable
                        var dataTable = new DataTable();

                        // Export worksheet data to a DataTable object by calling either ExportDataTable or ExportDataTableAsString method of the Cells class	
                        var rowCount = Convert.ToInt32(wsData.Cells["A7"].Value);
                        dataTable = wsData.Cells.ExportDataTable(7, 0, rowCount, 24);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            if (!string.IsNullOrEmpty(dataRow["Column1"].ToString()) && !string.IsNullOrEmpty(dataRow["Column2"].ToString()))
                            {
                                currentDocumentNo = dataRow["Column4"].ToString();
                                var docId = Convert.ToInt32(dataRow["Column2"].ToString());
                                var docObj = this.documentPackageService.GetById(docId);
                                if (docObj != null)
                                {
                                    var revisionName = dataRow["Column6"].ToString();
                                    var revisionObj = this.revisionService.GetByName(revisionName, projectId);
                                    var statusName = dataRow["Column7"].ToString();
                                    var statusObj = this.statusService.GetByName(statusName, projectId);
                                    if (revisionObj != null && revisionObj.ID > docObj.RevisionId)
                                    {
                                        var docNewRev = new DocumentPackage()
                                        {
                                            ProjectId = docObj.ProjectId,
                                            ProjectName = docObj.ProjectName,
                                            ProjectFullName = docObj.ProjectFullName,
                                            DocNo = docObj.DocNo,
                                            DocTitle = docObj.DocTitle,

                                            RevisionId = revisionObj.ID,
                                            RevisionName = revisionObj.Name,
                                            
                                            OriginatorId = docObj.OriginatorId,
                                            OriginatorFullName = docObj.OriginatorFullName,
                                            OriginatorName = docObj.OriginatorName,

                                            DisciplineId = docObj.DisciplineId,
                                            DisciplineFullName = docObj.DisciplineFullName,
                                            DisciplineName = docObj.DisciplineName,

                                            DocTypeFullName = docObj.DocTypeFullName,
                                            DocumentTypeId = docObj.DocumentTypeId,
                                            DocumentTypeName = docObj.DocumentTypeName,

                                            SequencetialNumber = docObj.SequencetialNumber,
                                            DrawingSheetNumber = docObj.DrawingSheetNumber,

                                            Complete = docObj.Complete,
                                            RevisionReceiveTransNo = dataRow["Column15"].ToString(),
                                            FirstIssueTransNo = dataRow["Column18"].ToString(),
                                            FinalIssueTransNo = dataRow["Column21"].ToString(),

                                            StatusID = statusObj != null? statusObj.ID : 0,
                                            StatusFullName = statusObj != null?  statusObj.FullNameWithWeight : string.Empty,
                                            StatusName = statusObj != null ?statusObj.Name : string.Empty,

                                            IsDelete = false,
                                            IsLeaf = true,
                                            IsEMDR = true,
                                            ParentId = docObj.ParentId ?? docObj.ID,
                                            CreatedBy = UserSession.Current.User.Id,
                                            CreatedDate = DateTime.Now,
                                        };

                                        if (!string.IsNullOrEmpty(dataRow["Column23"].ToString()))
                                        {
                                            docNewRev.Weight = Math.Round( Convert.ToDouble(dataRow["Column23"])*100,2);
                                        }

                                        var strRevReceivedDate = dataRow["Column14"].ToString();
                                        var revReceivedDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strRevReceivedDate, ref revReceivedDate))
                                        {
                                            docNewRev.RevisionActualDate = revReceivedDate;
                                        }

                                        var strFirstIssuePlanDate = dataRow["Column16"].ToString();
                                        var firstIssuePlanDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFirstIssuePlanDate, ref firstIssuePlanDate))
                                        {
                                            docNewRev.FirstIssuePlanDate = firstIssuePlanDate;
                                        }

                                        var strFirstIssueActualDate = dataRow["Column17"].ToString();
                                        var firstIssueActualDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFirstIssueActualDate, ref firstIssueActualDate))
                                        {
                                            docNewRev.FirstIssueActualDate = firstIssueActualDate;
                                        }

                                        var strFinalIssuePlanDate = dataRow["Column19"].ToString();
                                        var finalIssuePlanDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFinalIssuePlanDate, ref finalIssuePlanDate))
                                        {
                                            docNewRev.FinalIssuePlanDate = finalIssuePlanDate;
                                        }

                                        var strFinalIssueActualDate = dataRow["Column20"].ToString();
                                        var finalIssueActualDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFinalIssueActualDate, ref finalIssueActualDate))
                                        {
                                            docNewRev.FinalIssueActualDate = finalIssueActualDate;
                                        }

                                        if (statusObj != null)
                                        {
                                            docNewRev.Complete = statusObj.PercentCompleteDefault;
                                        }
                                        docNewRev.CompleteForProject = Math.Round(docNewRev.Complete.GetValueOrDefault() * docNewRev.Weight.GetValueOrDefault() / 100, 2);
                                        newdocumentPackageList.Add(docNewRev);

                                        docObj.IsLeaf = false;
                                        docObj.UpdatedBy = UserSession.Current.User.Id;
                                        docObj.UpdatedDate = DateTime.Now;
                                        documentPackageList.Add(docObj);
                                    }
                                    else
                                    {
                                        var strRevReceivedDate = dataRow["Column14"].ToString();
                                        var revReceivedDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strRevReceivedDate, ref revReceivedDate))
                                        {
                                            docObj.RevisionActualDate = revReceivedDate;
                                        }

                                        var strFirstIssuePlanDate = dataRow["Column16"].ToString();
                                        var firstIssuePlanDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFirstIssuePlanDate, ref firstIssuePlanDate))
                                        {
                                            docObj.FirstIssuePlanDate = firstIssuePlanDate;
                                        }

                                        var strFirstIssueActualDate = dataRow["Column17"].ToString();
                                        var firstIssueActualDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFirstIssueActualDate, ref firstIssueActualDate))
                                        {
                                            docObj.FirstIssueActualDate = firstIssueActualDate;
                                        }

                                        var strFinalIssuePlanDate = dataRow["Column19"].ToString();
                                        var finalIssuePlanDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFinalIssuePlanDate, ref finalIssuePlanDate))
                                        {
                                            docObj.FinalIssuePlanDate = finalIssuePlanDate;
                                        }

                                        var strFinalIssueActualDate = dataRow["Column20"].ToString();
                                        var finalIssueActualDate = new DateTime();
                                        if (Utility.ConvertStringToDateTime(strFinalIssueActualDate, ref finalIssueActualDate))
                                        {
                                            docObj.FinalIssueActualDate = finalIssueActualDate;
                                        }

                                        docObj.RevisionReceiveTransNo = dataRow["Column15"].ToString();
                                        docObj.FirstIssueTransNo = dataRow["Column18"].ToString();
                                        docObj.FinalIssueTransNo = dataRow["Column21"].ToString();
                                        docObj.UpdatedBy = UserSession.Current.User.Id;
                                        docObj.UpdatedDate = DateTime.Now;
                                        docObj.StatusID = statusObj != null ? statusObj.ID : 0;
                                        docObj.StatusFullName = statusObj != null ?statusObj.FullNameWithWeight : string.Empty;
                                        docObj.StatusName =statusObj != null ? statusObj.Name : string.Empty;

                                        if (!string.IsNullOrEmpty(dataRow["Column23"].ToString()))
                                        {
                                            docObj.Weight = Math.Round(Convert.ToDouble(dataRow["Column23"]) * 100, 2);
                                        }

                                        if (statusObj != null)
                                        {
                                            docObj.Complete = statusObj.PercentCompleteDefault;
                                        }

                                        docObj.CompleteForProject = Math.Round(docObj.Complete.GetValueOrDefault() * docObj.Weight.GetValueOrDefault() / 100, 2);



                                        documentPackageList.Add(docObj);
                                    }
                                }
                            }
                        }
                    }
                }

                if (this.cbCheckValidFile.Checked)
                {
                    foreach (var documentPackage in documentPackageList)
                    {
                        this.documentPackageService.Update(documentPackage);
                    }

                    foreach (var documentPackage in newdocumentPackageList)
                    {
                        this.documentPackageService.Insert(documentPackage);
                    }

                    if (projectObj != null && projectObj.IsAutoCalculate.GetValueOrDefault())
                    {
                        UpdateActualProgress(projectObj);
                    }


                    this.blockError.Visible = true;
                    this.lblError.Text = "All CMDR data file is valid. Data import successfull.";
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);    
                }
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error at CMDR data file: '" + currentFileName + "', document: '" + currentDocumentNo + "', with error: '" + ex.Message + "'";
            }
        }

        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }

        /// <summary>
        /// Auto calculater
        /// </summary>
        /// <param name="objProject"></param>

        private void UpdateActualProgress(ScopeProject objProject)
        {
            var count = 0;
            for (var j = objProject.StartDate.GetValueOrDefault();
                j < objProject.EndDate.GetValueOrDefault();
                j = j.AddDays(objProject.FrequencyForProgressChart != null && objProject.FrequencyForProgressChart != 0 ? objProject.FrequencyForProgressChart.Value : 7))
            {
                if (DateTime.Now > j)
                {
                    count += 1;
                }
                else
                {
                    break;
                }
            }

            var listDiscipline = this.disciplineService.GetAllDisciplineOfProject(objProject.ID).OrderBy(t => t.ID).ToList();
            foreach (var discipline in listDiscipline)
            {
                var docList = this.documentPackageService.GetAllByDiscipline(discipline.ID).OrderBy(t => t.DocNo).ToList();
                double? complete = 0;
                complete = docList.Aggregate(complete, (current, t) => current + t.CompleteForProject.GetValueOrDefault());
                discipline.Complete =  complete ;
                var existProgressActual = this.processActualService.GetByProjectAndWorkgroup(objProject.ID, discipline.ID);
                if (existProgressActual != null)
                {
                    var arrActual = existProgressActual.Actual.Split('$');
                    if (arrActual.Count() >= count)
                    {
                        arrActual[count] = Math.Round(complete.GetValueOrDefault(), 2).ToString();

                        var newAtualProgress = string.Empty;
                        newAtualProgress = arrActual.Aggregate(newAtualProgress, (current, t) => current + t + "$");
                        newAtualProgress = newAtualProgress.Substring(0, newAtualProgress.Length - 1);

                        existProgressActual.Actual = newAtualProgress;

                        this.processActualService.Update(existProgressActual);
                    }
                  

                }
                else
                {
                    var progressActual = new ProcessActual();
                    progressActual.ProjectId = discipline.ProjectId;
                    progressActual.WorkgroupId = discipline.ID;
                    progressActual.Actual = Math.Round(complete.GetValueOrDefault(), 2).ToString();
                    this.processActualService.Insert(progressActual);
                }
                this.disciplineService.Update(discipline);
            }
        }


    }
}