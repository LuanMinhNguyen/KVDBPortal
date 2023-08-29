// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Data;
    using System.Web.Hosting;
    using System.Web.UI;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportData : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        //private readonly DocumentService documentService;

        private readonly DQRETransmittalService transmittalService;

        //private readonly ScopeProjectService scopeProjectService;

        //private readonly DisciplineService disciplineService;

        //private readonly ProcessPlanedService processPlanedService;
        //private readonly ProcessActualService processActualService;
        //private readonly ProcessRecoveryPlanedService processRecoveryPlanedService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ImportData()
        {
            //this.documentService = new DocumentService();
            this.transmittalService = new DQRETransmittalService();
            //this.scopeProjectService = new ScopeProjectService();
            //this.disciplineService = new DisciplineService();
            //this.processPlanedService = new ProcessPlanedService();
            //this.processActualService = new ProcessActualService();
            //this.processRecoveryPlanedService = new ProcessRecoveryPlanedService();
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
                //if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                //{
                //    var objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
                //    if (objDoc != null)
                //    {
                        
                //    }
                //}
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

            try
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
                {
                    var transID = new Guid(this.Request.QueryString["tranId"]);
                    var transObj = this.transmittalService.GetById(transID);

                    if (transObj != null)
                    {
                        foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                        {
                            var filename = DateTime.Now.ToBinary() + "_" + docFile.FileName;
                            var oldfilePath = Server.MapPath(transObj.File);

                            var newServerPath = "../../Transmittals/Generated/" + filename;
                            var newFilePath = Server.MapPath("../../Transmittals/Generated/") + filename;

                            docFile.SaveAs(newFilePath);

                            transObj.File = newServerPath;
                            transObj.HasAttachFile = true;
                            transObj.LastUpdatedBy = UserSession.Current.User.Id;
                            transObj.LastUpdatedDate = DateTime.Now;

                            if (File.Exists(oldfilePath))
                            {
                                File.Delete(oldfilePath);
                            }

                            this.transmittalService.Update(transObj);
                        }
                    }
                }
                #region type
                //else if (!string.IsNullOrEmpty(this.Request.QueryString["type"]))
                //{
                //    var type = this.Request.QueryString["type"];
                //    switch (type)
                //    {
                //        case "progress":
                //            foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                //            {
                //                var extension = docFile.GetExtension();
                //                if (extension == ".xls" || extension == ".xlsx")
                //                {
                //                    var importPath = Server.MapPath("../../Import") + "/" +
                //                                     DateTime.Now.ToString("ddMMyyyyhhmmss") +
                //                                     "_" + docFile.FileName;
                //                    docFile.SaveAs(importPath);
                //                    var workbook = new Workbook();
                //                    workbook.Open(importPath);
                //                    var wsProgress = workbook.Worksheets[0];

                //                    var projectId = Convert.ToInt32(wsProgress.Cells["A5"].Value.ToString());

                //                    var project = this.scopeProjectService.GetById(projectId);//this.scopeProjectService.GetByName(docFile.FileName.Split('$')[0]);
                //                    if (project != null && project.StartDate != null && project.EndDate != null)
                //                    {
                //                        var count = 0;
                //                        var startDate = project.StartDate.GetValueOrDefault();
                //                        for (var j = startDate;
                //                            j <= project.EndDate.GetValueOrDefault();
                //                            j = j.AddDays(project.FrequencyForProgressChart != null && project.FrequencyForProgressChart != 0 ? project.FrequencyForProgressChart.Value : 7))
                //                        {
                //                            count += 1;
                //                        }



                //                        var progressType = wsProgress.Cells["A7"].Value.ToString();
                //                        if (progressType == "RecoveryPlaned" || progressType == "Planed" || progressType == "Actual")
                //                        {
                //                            var dataTable = wsProgress.Cells.ExportDataTable(7, 0, wsProgress.Cells.MaxRow - 7, count + 3);
                //                            var recoveryPlanList = this.processRecoveryPlanedService.GetAllByProject(project.ID);
                //                            var index = recoveryPlanList.Any() ? recoveryPlanList.Max(t => t.IndexNo.GetValueOrDefault()) : 0;

                //                            foreach (DataRow dataRow in dataTable.Rows)
                //                            {
                //                                var disciplineId = dataRow["Column1"].ToString();
                //                                if (!string.IsNullOrEmpty(disciplineId))
                //                                {
                //                                    var discipline = this.disciplineService.GetById(Convert.ToInt32(disciplineId));
                //                                    if (discipline != null)
                //                                    {
                //                                        var progressValue = string.Empty;
                //                                        for (int i = 0; i < count; i++)
                //                                        {
                //                                            if (!string.IsNullOrEmpty(dataRow["Column" + (i + 4)].ToString()))
                //                                            {

                //                                                if (i != 0)
                //                                                {
                //                                                    var previousValue = !string.IsNullOrEmpty(dataRow["Column" + (i + 3)].ToString())
                //                                                            ? Math.Round(Convert.ToDouble(dataRow["Column" + (i + 3)].ToString()), 4) * 100
                //                                                            : 0;
                //                                                    var value = !string.IsNullOrEmpty(dataRow["Column" + (i + 4)].ToString())
                //                                                            ? Math.Round(Convert.ToDouble(dataRow["Column" + (i + 4)].ToString()), 4) * 100
                //                                                            : 0;
                //                                                    if (previousValue != 0.0 && value == 0.0)
                //                                                    {
                //                                                        break;
                //                                                    }
                //                                                    else
                //                                                    {
                //                                                        progressValue += value + "$";
                //                                                    }
                //                                                }
                //                                                else
                //                                                {
                //                                                    var value = !string.IsNullOrEmpty(dataRow["Column" + (i + 4)].ToString())
                //                                                            ? Math.Round(Convert.ToDouble(dataRow["Column" + (i + 4)].ToString()), 4)*100
                //                                                            : 0;
                //                                                    progressValue += value + "$";
                //                                                }
                //                                            }
                //                                        }

                //                                        progressValue = progressValue.Substring(0, progressValue.Length - 1);

                //                                        if (progressType == "Planed")
                //                                        {
                //                                            var existProgressPlaned =
                //                                                this.processPlanedService.GetByProjectAndWorkgroup(
                //                                                    project.ID, discipline.ID);

                //                                            if (existProgressPlaned == null)
                //                                            {
                //                                                var progressPlaned = new ProcessPlaned();
                //                                                progressPlaned.ProjectId = discipline.ProjectId;
                //                                                progressPlaned.WorkgroupId = discipline.ID;
                //                                                progressPlaned.Planed = progressValue;

                //                                                this.processPlanedService.Insert(progressPlaned);
                //                                            }
                //                                            else
                //                                            {
                //                                                existProgressPlaned.Planed = progressValue;

                //                                                this.processPlanedService.Update(existProgressPlaned);
                //                                            }
                //                                        }
                //                                        else if (progressType == "RecoveryPlaned")
                //                                        {


                //                                            var progressRecoveryPlaned = new ProcessRecoveryPlaned();
                //                                            progressRecoveryPlaned.ProjectId = discipline.ProjectId;
                //                                            progressRecoveryPlaned.WorkgroupId = discipline.ID;
                //                                            progressRecoveryPlaned.Planed = progressValue;
                //                                            progressRecoveryPlaned.IndexNo = index + 1;
                //                                            progressRecoveryPlaned.CreatedBy = UserSession.Current.User.Id;
                //                                            progressRecoveryPlaned.CreatedDate = DateTime.Now;

                //                                            this.processRecoveryPlanedService.Insert(progressRecoveryPlaned);
                //                                        }
                //                                        else
                //                                        {
                //                                            var existProgressActual =
                //                                                this.processActualService.GetByProjectAndWorkgroup(
                //                                                    project.ID, discipline.ID);

                //                                            if (existProgressActual == null)
                //                                            {
                //                                                var progressActual = new ProcessActual();
                //                                                progressActual.ProjectId = discipline.ProjectId;
                //                                                progressActual.WorkgroupId = discipline.ID;
                //                                                progressActual.Actual = progressValue;

                //                                                this.processActualService.Insert(progressActual);
                //                                            }
                //                                            else
                //                                            {
                //                                                existProgressActual.Actual = progressValue;

                //                                                this.processActualService.Update(existProgressActual);
                //                                            }
                //                                        }

                //                                    }
                //                                }
                //                            }
                //                        }
                //                        else
                //                        {
                //                            this.blockError.Visible = true;
                //                            this.lblError.Text = "This file is invalid";
                //                        }


                //                    }
                //                }
                //            }

                //            break;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error when import Progress: <br/>'" + ex.Message + "'";
            }

            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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
        /// The save upload file.
        /// </summary>
        /// <param name="uploadDocControl">
        /// The upload doc control.
        /// </param>
        /// <param name="objDoc">
        /// The obj Doc.
        /// </param>
        private void SaveUploadFile(RadAsyncUpload uploadDocControl, ref Document objDoc)
        {
            var listUpload = uploadDocControl.UploadedFiles;
            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {
                    var revisionFilePath = Server.MapPath(objDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));

                    docFile.SaveAs(revisionFilePath, true);
                }
            }
        }
    }
}