// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using iTextSharp.awt.geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PECC2CheckinCRSFile : Page
    {
        private readonly PECC2TransmittalService pecc2TransmittalService;

        private readonly PECC2TransmittalAttachFileService pecc2TransmittalAttachFileService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly PECC2DocumentsService pecc2DocumentsService;

        private readonly PECC2DocumentAttachFileService pecc2DocumentAttachFileService;

        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService;

        private readonly ProjectCodeService projectCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PECC2CheckinCRSFile()
        {
            this.pecc2TransmittalService = new PECC2TransmittalService();
            this.pecc2TransmittalAttachFileService = new PECC2TransmittalAttachFileService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.pecc2DocumentsService = new PECC2DocumentsService();
            this.pecc2DocumentAttachFileService = new PECC2DocumentAttachFileService();
            this.contractorTransmittalAttachFileService = new ContractorTransmittalAttachFileService();
            this.projectCodeService = new ProjectCodeService();
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
                this.lblmess.Visible = false;
                this.lblUserId.Value = UserSession.Current.User.Id.ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                //    var crsList =
                //this.pecc2TransmittalAttachFileService.GetByTrans(transId)
                //    .FirstOrDefault(t => t.TypeId == 2);

                //    if (crsList != null)
                //    {
                //        this.IsHasCRSFile.Value = "True";
                //    }
                //    else
                //    {
                //        this.IsHasCRSFile.Value = "False";
                //    }
                }
            }
        
        }

       
        private void ReuploadFileCRS()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var crsFile = this.pecc2TransmittalAttachFileService.GetById(objId);
                var transObj = this.pecc2TransmittalService.GetById(crsFile.TransId.GetValueOrDefault());
                var targetFolder = "../.." + transObj.StoreFolderPath + "/eTRM File";
                foreach (UploadedFile docFile in docuploader.UploadedFiles)
                {
                    var serverDocFileName = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + "_CRS.xlsm";

                    // Path file to save on server disc
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                    // Path file to download from server
                    docFile.SaveAs(saveFilePath, true);

                    crsFile.IsCheckOut = false;
                    crsFile.CheckinDate = DateTime.Now;
                    crsFile.CreatedDate = DateTime.Now;
                    crsFile.CreatedBy = UserSession.Current.User.Id;
                    crsFile.CreatedByName = UserSession.Current.User.UserNameWithFullName;
                    crsFile.FileSize = (double)docFile.ContentLength / 1024;
                    this.pecc2TransmittalAttachFileService.Update(crsFile);
                }

                this.docuploader.UploadedFiles.Clear();
                this.lblmess.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.ReuploadFileCRS();
            this.docuploader.UploadedFiles.Clear();
            //  this.lblmess.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "CancelEdit();", true);
        }

        protected void btnExportCRS_OnClick(object sender, EventArgs e)
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "confirm", "Confirm();", true);
            //string confirmValue = Request.Form["confirm_value"];
            //if (confirmValue == "Yes")
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
            //}
            //else
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
            //}

            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var transObj = this.pecc2TransmittalService.GetById(new Guid(Request.QueryString["objId"]));
                if (transObj != null)
                {
                    var projectObj = this.projectCodeService.GetById(transObj.ProjectCodeId.GetValueOrDefault());
                    var docOfTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID)
                        .Select(t => this.pecc2DocumentsService.GetById(t.DocumentId.GetValueOrDefault()))
                        .Where(t => t != null)
                        .OrderBy(t => t.DocNo)
                        .ToList();
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    if (transObj.IsFirstTrans.GetValueOrDefault())
                    {
                        workbook = new Workbook(filePath + @"Template\PECC2_CRS_Template.xlsm");
                    }
                    else
                    {
                        var responseCRSAttachFile = this.contractorTransmittalAttachFileService.GetByTrans(transObj.ContractorTransId.GetValueOrDefault()).FirstOrDefault(t => t.TypeId == 2);
                        if (responseCRSAttachFile != null)
                        {
                            workbook = new Workbook(Server.MapPath("../.." + responseCRSAttachFile.FilePath));
                        }
                    }

                    var dataControlSheet = workbook.Worksheets[0];
                    var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value);
                    var transSheet = workbook.Worksheets[currentSheetIndex];
                    transSheet.Name = transObj.TransmittalNo;

                    if (currentSheetIndex != 1)
                    {
                        transSheet.IsVisible = true;
                    }

                    // Update new current sheet
                    dataControlSheet.Cells["A1"].Value = currentSheetIndex + 1;
                    // -------------------------------------------------------------------------------------------------------

                    transSheet.Cells["B1"].PutValue(projectObj.Description + "\nCOMMENT RESPONSE SHEET");
                    transSheet.Cells["D5"].PutValue(transObj.TransmittalNo);
                    transSheet.Cells["F5"].PutValue(transObj.IssuedDate);
                    transSheet.Cells["G5"].PutValue(transObj.PurposeName.Split(',')[0]);

                    var dtFull = new DataTable();
                    dtFull.Columns.AddRange(new[]
                    {
                        new DataColumn("Temp", typeof(String)),
                        new DataColumn("NoIndex", typeof(String)),
                        new DataColumn("DocNo", typeof(String)),
                        new DataColumn("Rev", typeof(String)),
                        new DataColumn("ReviewStatus", typeof(String)),
                        new DataColumn("CommentIndex", typeof(String)),
                        new DataColumn("Comment", typeof(String)),
                        new DataColumn("Page", typeof(String)),
                        new DataColumn("CommentedBy", typeof(String)),
                        new DataColumn("ApproveBy", typeof(String)),
                        new DataColumn("ContractorResponse", typeof(String)),
                    });

                    var countDocOfTrans = 1;
                    if (transObj.IsFirstTrans.GetValueOrDefault())
                    {
                        foreach (var docObj in docOfTrans)
                        {
                            var dataRow = dtFull.NewRow();
                            dataRow["Temp"] = countDocOfTrans;
                            dataRow["NoIndex"] = countDocOfTrans;
                            dataRow["DocNo"] = docObj.DocNo;
                            dataRow["Rev"] = docObj.Revision;
                            dataRow["ReviewStatus"] = docObj.DocReviewStatusCode;
                            dataRow["Comment"] = docObj.DocTitle;
                            dtFull.Rows.Add(dataRow);

                            var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                            if (commentList.Count != 0)
                            {
                                for (var i = 0; i < commentList.Count; i++)
                                {
                                    var dataRow1 = dtFull.NewRow();
                                    dataRow1["Temp"] = countDocOfTrans;
                                    dataRow1["Rev"] = docObj.Revision;
                                    dataRow1["CommentIndex"] = commentList[i].Split('$')[0].Split('.')[0];
                                    dataRow1["Comment"] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                    dataRow1["Page"] = commentList[i].Split('$')[1];
                                    dataRow1["CommentedBy"] = commentList[i].Split('$')[2];
                                    dtFull.Rows.Add(dataRow1);
                                }
                            }
                            //else
                            //{
                            //    dtFull.Rows.Add(dataRow);
                            //}

                            countDocOfTrans += 1;
                        }
                    }
                    else
                    {
                        var prevRowCount = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);
                        var previousTransSheet = workbook.Worksheets[currentSheetIndex - 1];
                        var previousDataTable = previousTransSheet.Cells.ExportDataTable(7, 2, prevRowCount, 11);
                        if (previousDataTable.Rows.Count > 0)
                        {
                            var previousDataTableRows = previousDataTable.AsEnumerable().ToList();
                            foreach (var docObj in docOfTrans)
                            {
                                var previousDataDocRow = previousDataTableRows.FirstOrDefault(t => t["Column3"].ToString() == docObj.DocNo);
                                if (previousDataDocRow != null)
                                {
                                    var previousDataDocRowList = previousDataTableRows.Where(t => t["Column1"].ToString() == previousDataDocRow["Column1"]);

                                    var dataRow = dtFull.NewRow();
                                    dataRow["Temp"] = countDocOfTrans;
                                    dataRow["NoIndex"] = countDocOfTrans;
                                    dataRow["DocNo"] = docObj.DocNo;
                                    dataRow["Rev"] = docObj.Revision;
                                    dataRow["ReviewStatus"] = docObj.DocReviewStatusCode;
                                    dataRow["Comment"] = docObj.DocTitle;
                                    dtFull.Rows.Add(dataRow);

                                    var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                                    if (commentList.Count != 0)
                                    {
                                        var currentCommentIndex = commentList.Select(t => Convert.ToInt32(t.Split('$')[0].Split('.')[0])).Distinct().ToList();
                                        var commentIndexCompletedList = previousDataDocRowList.Where(t => !string.IsNullOrEmpty(t["Column6"].ToString().Trim()) && !currentCommentIndex.Contains(Convert.ToInt32(t["Column6"].ToString().Trim()))).Select(t => Convert.ToInt32(t["Column6"].ToString().Trim())).Distinct().ToList();

                                        var totalCommentIndex = new List<int>();
                                        totalCommentIndex.AddRange(currentCommentIndex);
                                        totalCommentIndex.AddRange(commentIndexCompletedList);
                                        totalCommentIndex = totalCommentIndex.OrderBy(t => t).ToList();
                                        
                                        // Fill comment still need review
                                        foreach (var commentIndex in totalCommentIndex)
                                        {
                                            var previousCommentsByIndex = previousDataDocRowList.Where(t => t["Column6"].ToString().Trim() == commentIndex.ToString());

                                            foreach (DataRow prevComment in previousCommentsByIndex)
                                            {
                                                var dataRow2 = dtFull.NewRow();
                                                dataRow2["Temp"] = countDocOfTrans;
                                                dataRow2["Rev"] = prevComment["Column4"];
                                                dataRow2["CommentIndex"] = prevComment["Column6"];
                                                dataRow2["Comment"] = prevComment["Column7"];
                                                dataRow2["Page"] = prevComment["Column8"];
                                                dataRow2["ContractorResponse"] = prevComment["Column11"];
                                                dtFull.Rows.Add(dataRow2);
                                            }

                                            var commentListOfCommentIndex = commentList.Where(t =>
                                                t.Split('$')[0].Split('.')[0] == commentIndex.ToString()).ToList();
                                            for (int i = 0; i < commentListOfCommentIndex.Count; i++)
                                            {
                                                var dataRow1 = dtFull.NewRow();
                                                dataRow1["Temp"] = countDocOfTrans;
                                                dataRow1["Rev"] = docObj.Revision;
                                                dataRow1["CommentIndex"] = commentListOfCommentIndex[i].Split('$')[0].Split('.')[0];
                                                dataRow1["Comment"] = commentListOfCommentIndex[i].Split('$')[0].Substring(commentListOfCommentIndex[i].Split('$')[0].IndexOf('.') + 1);
                                                dataRow1["Page"] = commentListOfCommentIndex[i].Split('$')[1];
                                                dataRow1["CommentedBy"] = commentListOfCommentIndex[i].Split('$')[2];
                                                dtFull.Rows.Add(dataRow1);
                                            }
                                        }
                                    }

                                    countDocOfTrans += 1;
                                }
                            }
                        }
                    }

                    transSheet.Cells.ImportDataTable(dtFull, false, 7, 2, dtFull.Rows.Count, dtFull.Columns.Count, true);
                    transSheet.Cells.DeleteRow(7 + dtFull.Rows.Count);

                    dataControlSheet.Cells["A2"].PutValue(dtFull.Rows.Count);

                    var filename = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + "_CRS.xlsm";
                    var saveFilePath = Server.MapPath("../.." + transObj.StoreFolderPath + "/eTRM File/" + filename);
                    workbook.Save(saveFilePath);

                    // Attach CRS to document Obj
                    var fileInfo = new FileInfo(saveFilePath);
                    if (fileInfo.Exists && this.pecc2TransmittalAttachFileService.GetByTrans(transObj.ID).All(t => t.TypeId != 2))
                    {
                        var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + transObj.StoreFolderPath + "/eTRM File";
                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + filename;
                        var attachFile = new PECC2TransmittalAttachFiles()
                        {
                            ID = Guid.NewGuid(),
                            TransId = transObj.ID,
                            Filename = filename,
                            Extension = "xlsm",
                            FilePath = serverFilePath,
                            ExtensionIcon = "~/images/excelfile.png",
                            FileSize = (double)fileInfo.Length / 1024,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now,
                            TypeId = 2,
                            TypeName = "CRS File"
                        };

                        this.pecc2TransmittalAttachFileService.Insert(attachFile);
                    }
                    else
                    {
                        var crsObj = this.pecc2TransmittalAttachFileService.GetByTrans(transObj.ID).FirstOrDefault(t => t.TypeId == 2);
                        if (crsObj != null)
                        {
                            crsObj.CreatedDate = DateTime.Now;
                            crsObj.CreatedBy = UserSession.Current.User.Id;
                            crsObj.CreatedByName = UserSession.Current.User.UserNameWithFullName;
                            crsObj.FileSize = (double)fileInfo.Length / 1024;
                            this.pecc2TransmittalAttachFileService.Update(crsObj);
                        }
                    }
                    // -------------------------------------------------------------------------------------------------

                }
            }
        }

        private List<string> GetCommentInfo(PECC2Documents docObj)
        {
            var consolidateDocFileList = this.pecc2DocumentAttachFileService.GetAllDocId(docObj.ID);
            var commentList = new List<string>();
            if (consolidateDocFileList.Count != 0)
            {
                var consolidateDocFile= consolidateDocFileList.OrderByDescending(t=> t.CreatedDate).FirstOrDefault(t => t.TypeId == 3);
                if (consolidateDocFile == null) return commentList;

                var pdfReader = new PdfReader(Server.MapPath("../.." + consolidateDocFile.FilePath));
                var count = 0;
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    PdfArray array = pdfReader.GetPageN(i).GetAsArray(PdfName.ANNOTS);
                    if (array == null) continue;
                    for (int j = 0; j < array.Size; j++)
                    {
                        var annot = array.GetAsDict(j);
                        var text = annot.GetAsString(PdfName.CONTENTS);
                        if (text != null && !string.IsNullOrEmpty(text.ToString()))
                        {
                            //var username = annot.GetAsString(PdfName.T);
                            count += 1;
                            commentList.Add(text + "$" + i + "$" + consolidateDocFile.CreatedByName.Split('/')[0]);
                        }
                    }
                }
            }

            return commentList;
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "UploadCRSFile")
            {
                this.ReuploadFileCRS();
            }
        }
    }
}