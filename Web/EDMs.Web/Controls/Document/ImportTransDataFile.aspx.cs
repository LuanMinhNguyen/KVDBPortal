// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using EDMs.Business.Services.Scope;
using EDMs.Web.Utilities;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
    public partial class ImportTransDataFile : Page
    {
        private readonly DocumentPackageService documentPackageService;

        private readonly ScopeProjectService scopeProjectService;


        private readonly ToListService toListService;

        private readonly TransmittalService transmittalService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly RevisionService revisionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ImportTransDataFile()
        {
            this.documentPackageService = new DocumentPackageService();
            this.scopeProjectService = new ScopeProjectService();
            this.toListService = new ToListService();
            this.transmittalService = new TransmittalService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.revisionService = new RevisionService();
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
                this.LoadComboData();
            }
        }

        private void LoadComboData()
        {
            var toList = this.toListService.GetAll();

            this.ddlFromList.DataSource = toList;
            this.ddlFromList.DataTextField = "FullName";
            this.ddlFromList.DataValueField = "Id";
            this.ddlFromList.DataBind();

            var projectList = this.scopeProjectService.GetAll();
            this.ddlProject.DataSource = projectList;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
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
                var docNonExistList = new List<string>();
                foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                {
                    var extension = docFile.GetExtension();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {
                        var transFileName = docFile.FileName;
                        var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + docFile.FileName;
                        docFile.SaveAs(importPath);

                        var owner = this.toListService.GetById(1);

                        var workbook = new Workbook();
                        workbook.Open(importPath);
                        var tranSheet = workbook.Worksheets[0];
                        var projectID = this.ddlProject.SelectedItem != null
                                ? Convert.ToInt32(this.ddlProject.SelectedValue)
                                : 0;
                        var projectObj = this.scopeProjectService.GetById(projectID);
                        if (projectObj != null)
                        {
                            File.Copy(importPath, Server.MapPath("../../" + projectObj.TransFolderPath + "/01.IncomingTrans/" + transFileName), true);

                            // Create incoming Trans Info

                            var objTran = new Transmittal()
                            {
                                Name = tranSheet.Cells["F15"].Value?.ToString() ?? string.Empty,
                                ProjectName = projectObj.FullName,
                                ProjectId = projectObj.ID,
                                //ReasonForIssue = tranSheet.Cells["E10"].Value?.ToString() ?? string.Empty,
                                ToId = owner.ID,
                                ToList = owner.FullName,
                                FromId = Convert.ToInt32(this.ddlFromList.SelectedValue),
                                FromList = this.ddlFromList.SelectedItem.Text,

                                TransType = 1,
                                //IssuseDate = (DateTime?) tranSheet.Cells["F10"].Value,
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedDate = DateTime.Now,
                                IsGenerate = true,
                                GeneratePath = projectObj.TransFolderPath + "/01.IncomingTrans/" + transFileName
                            };

                            var transId = this.transmittalService.Insert(objTran);

                            // -------------------------------------------

                            // Create attach doc to trans info
                            if (transId != null)
                            {
                                var dataTable = tranSheet.Cells.ExportDataTable(28, 1, tranSheet.Cells.MaxDataRow - 37, 5);

                                foreach (DataRow dataRow in dataTable.Rows)
                                {
                                    if (!string.IsNullOrEmpty(dataRow["Column1"].ToString()) && dataRow["Column1"].ToString() != "(*) STATUS according to Coordination Procedure.")
                                    {
                                        var revName = dataRow["Column2"].ToString();
                                        var docNo = dataRow["Column1"].ToString();
                                        var docObj = this.documentPackageService.GetOneByDocNo(docNo, revName, projectID);

                                        if (docObj != null)
                                        {
                                            var attachDoc = new AttachDocToTransmittal()
                                            {
                                                //TransmittalId = transId,
                                                //DocumentId = docObj.ID
                                            };
                                            //if (!this.attachDocToTransmittalService.IsExist(transId.GetValueOrDefault(), docObj.ID))
                                            {
                                                this.attachDocToTransmittalService.Insert(attachDoc);
                                            }

                                            docObj.RevisionActualDate = (DateTime?) dataRow["Column5"];
                                            docObj.RevisionReceiveTransNo = objTran.Name;

                                            var docRevObj = this.revisionService.GetById(docObj.RevisionId.GetValueOrDefault());
                                            if (docRevObj.IsFirst.GetValueOrDefault())
                                            {
                                                docObj.FirstIssueActualDate = (DateTime?)dataRow["Column5"];
                                                docObj.FirstIssueTransNo = objTran.Name;
                                            }
                                            
                                            this.documentPackageService.Update(docObj);
                                        }
                                        else
                                        {
                                            docNonExistList.Add(docNo + "_" + revName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (docNonExistList.Count > 0)
                {
                    this.blockError.Visible = true;
                    this.lblError.Text = "Transmittal object created Successfull.</br>" +
                                         "But can't find documents:</br>";
                    foreach (var docNo in docNonExistList)
                    {
                        this.lblError.Text += docNo + "</br>";
                    }

                    this.lblError.Text += "to attach to Transmittal. Please create Document first and Attach to transmittal again.";
                }
                else
                {
                    this.blockError.Visible = true;
                    this.lblError.Text = "Import transmittal file Successfull.";
                }
                
            }
           catch (Exception ex)
           {
               this.blockError.Visible = true;
               this.lblError.Text = "Have error at Transmittal Form: '" + ex.Message + "'";
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
    }
}