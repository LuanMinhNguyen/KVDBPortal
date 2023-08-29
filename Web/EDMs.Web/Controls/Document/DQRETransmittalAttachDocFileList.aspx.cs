// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.IO;
using System.Linq;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Business.Services.Document;
    using Data.Entities;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System.Configuration;
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DQRETransmittalAttachDocFileList : Page
    {
        private readonly ContractorTransmittalService contractorTransmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly DQRETransmittalService dqreTransmittalService;

        private readonly DQRETransmittalAttachDocFileService dqreAttachDocFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DQRETransmittalAttachDocFileList()
        {
            this.contractorTransmittalService = new ContractorTransmittalService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
            this.dqreTransmittalService = new DQRETransmittalService();
            this.dqreAttachDocFileService = new DQRETransmittalAttachDocFileService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var dqreTrans = this.dqreTransmittalService.GetById(transId);
                    if (dqreTrans.ContractorTransId != null)
                    {
                        var contractorTransObj = this.contractorTransmittalService.GetById(dqreTrans.ContractorTransId.GetValueOrDefault());
                        if (contractorTransObj != null)
                        {
                            contractorTransObj.IsOpen = true;
                            this.contractorTransmittalService.Update(contractorTransObj);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["Upload"]) && this.Request.QueryString["Upload"] == "False")
                {
                    this.DivUpload.Visible = false;
                    this.grdDocumentFile.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }
            }
        }

        protected void grdDocumentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var transId = new Guid(this.Request.QueryString["objId"]);
                var docFileList = this.dqreAttachDocFileService.GetByTrans(transId).OrderBy(t => t.DocNo);

                this.grdDocumentFile.DataSource = docFileList;
            }
        }

        protected void ajaxDocument_OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "DeleteAll")
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var transObj = this.contractorTransmittalService.GetById(transId);

                    var errorAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(transId).Where(t => !string.IsNullOrEmpty(t.ErrorMessage));
                    foreach (var errorItem in errorAttachDocFile)
                    {
                        var physicalPath = Server.MapPath("../.." + errorItem.FilePath);
                        if (File.Exists(physicalPath))
                        {
                            File.Delete(physicalPath);
                        }

                        this.contractorTransmittalDocFileService.Delete(errorItem);
                    }

                    // Update Trans status info
                    var currentTransAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);
                    if (!currentTransAttachDocFile.Any())
                    {
                        transObj.Status = "Missing Doc File";
                        transObj.IsValid = false;
                        transObj.ErrorMessage = "Missing Attach Document File.";
                    }
                    else if (currentTransAttachDocFile.Any(t => !string.IsNullOrEmpty(t.Status)))
                    {
                        transObj.Status = "Attach Doc File Invalid";
                        transObj.ErrorMessage = "Some attach document files are invalid format.";
                        transObj.IsValid = false;
                    }
                    else
                    {
                        transObj.Status = string.Empty;
                        transObj.ErrorMessage = string.Empty;
                        transObj.IsValid = true;
                    }

                    this.contractorTransmittalService.Update(transObj);
                    // ----------------------------------------------------------------------------------------------------------

                    this.grdDocumentFile.Rebind();
                }
            }
            else if (e.Argument.Contains("DeleteFile_"))
            {
                var objId = new Guid(e.Argument.Split('_')[1]);
                var transObj = this.dqreAttachDocFileService.GetById(objId);
                if (!string.IsNullOrEmpty(transObj?.FilePath))
                {
                    var folderPath = Server.MapPath("../.." + transObj.FilePath);
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                    }
                }

                this.dqreAttachDocFileService.Delete(objId);
                this.grdDocumentFile.Rebind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            var listUpload = docuploader.UploadedFiles;

            var listExistFile = new List<string>();

            var fileIcon = new Dictionary<string, string>()
                {
                    {"doc", "~/images/wordfile.png"},
                    {"docx", "~/images/wordfile.png"},
                    {"dotx", "~/images/wordfile.png"},
                    {"xls", "~/images/excelfile.png"},
                    {"xlsx", "~/images/excelfile.png"},
                    {"pdf", "~/images/pdffile.png"},
                    {"7z", "~/images/7z.png"},
                    {"dwg", "~/images/dwg.png"},
                    {"dxf", "~/images/dxf.png"},
                    {"rar", "~/images/rar.png"},
                    {"zip", "~/images/zip.png"},
                    {"txt", "~/images/txt.png"},
                    {"xml", "~/images/xml.png"},
                    {"xlsm", "~/images/excelfile.png"},
                    {"bmp", "~/images/bmp.png"},
                };

            if (listUpload.Count > 0)
            {
              
                foreach (UploadedFile docFile in listUpload)
                {
                    var docFileName = docFile.FileName;
                    var serverDocFileName = docFileName;

                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var dqreTrans = this.dqreTransmittalService.GetById(transId);
                    if (dqreTrans != null)
                    {
                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(dqreTrans.StoreFolderPath), Regex.Replace(serverDocFileName, @"[^0-9a-zA-Z_.-]+", string.Empty) );
                        // Path file to download from server
                        var serverFilePath = dqreTrans.StoreFolderPath + "/" + Regex.Replace(serverDocFileName, @"[^0-9a-zA-Z_.-]+", string.Empty);
                        var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1,
                            docFileName.Length - docFileName.LastIndexOf(".") - 1);

                        if (!File.Exists(saveFilePath))
                        {
                           
                            docFile.SaveAs(saveFilePath, true);

                            var document = new DQRETransmittalAttachDocFile()
                            {
                                 ID = Guid.NewGuid(),
                                TransId = dqreTrans.ID,
                                FileName = docFile.FileName,
                                ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower())
                                        ? fileIcon[fileExt.ToLower()]
                                        : "~/images/otherfile.png",
                                Extension = fileExt,
                               
                                FileSize = (double)docFile.ContentLength / 1024,
                                FilePath = serverFilePath
                            };

                            var idFile = this.dqreAttachDocFileService.Insert(document);
                        }
                    }
                }
                this.docuploader.UploadedFiles.Clear();
                this.grdDocumentFile.Rebind();
            }
        }

    }
}