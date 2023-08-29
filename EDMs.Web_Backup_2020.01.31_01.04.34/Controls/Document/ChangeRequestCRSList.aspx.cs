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
    public partial class ChangeRequestCRSList : Page
    {
        private readonly PECC2TransmittalService pecc2TransmittalService;

        private readonly PECC2TransmittalAttachFileService pecc2TransmittalAttachFileService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly PECC2DocumentsService pecc2DocumentsService;

        private readonly PECC2DocumentAttachFileService pecc2DocumentAttachFileService;

        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService;

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestCRSList()
        {
            this.pecc2TransmittalService = new PECC2TransmittalService();
            this.pecc2TransmittalAttachFileService = new PECC2TransmittalAttachFileService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.pecc2DocumentsService = new PECC2DocumentsService();
            this.pecc2DocumentAttachFileService = new PECC2DocumentAttachFileService();
            this.contractorTransmittalAttachFileService = new ContractorTransmittalAttachFileService();
            this.changeRequestService = new ChangeRequestService();
            this.changeRequestAttachFileService = new ChangeRequestAttachFileService();
            this.changeRequestDocFileService = new ChangeRequestDocFileService();
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
                this.lblUserId.Value = UserSession.Current.User.Id.ToString();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdAttachCRSFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                var crsList = this.changeRequestAttachFileService.GetByChangeRequest(changeRequestId)
                        .Where(t => t.TypeId == 4)
                        .OrderBy(t => t.CreatedDate);
                this.grdAttachCRSFile.DataSource = crsList;
            }
            else
            {
                this.grdAttachCRSFile.DataSource = new List<ChangeRequestAttachFile>();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var transObj = this.pecc2TransmittalService.GetById(objId);
                var targetFolder = "../.." + transObj.StoreFolderPath + "/eTRM File";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
                    ? string.Empty
                    : HostingEnvironment.ApplicationVirtualPath)
                                   + transObj.StoreFolderPath + "/eTRM File";
                foreach (UploadedFile docFile in docuploader.UploadedFiles)
                {
                    var docFileName = docFile.FileName;

                    var serverDocFileName = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + "_CRS.xlsm";

                    // Path file to save on server disc
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                    // Path file to download from server
                    var serverFilePath = serverFolder + "/" + serverDocFileName;
                    docFile.SaveAs(saveFilePath, true);

                    var attachFile = new PECC2TransmittalAttachFiles()
                    {
                        ID = Guid.NewGuid(),
                        TransId = objId,
                        Filename = docFileName,
                        Extension = "xlsm",
                        FilePath = serverFilePath,
                        ExtensionIcon = "~/images/excelfile.png",
                        FileSize = (double) docFile.ContentLength/1024,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        TypeId = 2,
                        TypeName = "CRS File"
                    };

                    this.pecc2TransmittalAttachFileService.Insert(attachFile);
                }

                this.docuploader.UploadedFiles.Clear();
                this.grdAttachCRSFile.Rebind();
            }

        }

        protected void grdAttachCRSFile_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem) e.Item;
            Guid attachId = new Guid(item.GetDataKeyValue("ID").ToString());
            this.pecc2TransmittalAttachFileService.Delete(attachId);
            this.grdAttachCRSFile.Rebind();
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
                var changeRequestObj = this.changeRequestService.GetById(new Guid(Request.QueryString["objId"]));
                if (changeRequestObj != null)
                {
                    var docOfChangeRequest = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestObj.ID);
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    if (changeRequestObj.IsFirst.GetValueOrDefault())
                    {
                        workbook = new Workbook(filePath + @"Template\PECC2_DesignChange_CRS.xlsm");
                    }
                    else
                    {
                        var responseCRSAttachFile = this.changeRequestAttachFileService.GetByChangeRequest(changeRequestObj.ID).FirstOrDefault(t => t.TypeId == 5);
                        if (responseCRSAttachFile != null)
                        {
                            workbook = new Workbook(Server.MapPath("../.." + responseCRSAttachFile.FilePath));
                        }
                    }
                    
                    var dataControlSheet = workbook.Worksheets[0];
                    var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value);
                    var changeRequestSheet = workbook.Worksheets[currentSheetIndex];
                    changeRequestSheet.Name = changeRequestObj.Number;

                    if (currentSheetIndex != 1)
                    {
                        changeRequestSheet.IsVisible = true;
                    }

                    // Update new current sheet
                    dataControlSheet.Cells["A1"].Value = currentSheetIndex + 1;
                    // -------------------------------------------------------------------------------------------------------

                    changeRequestSheet.Cells["M4"].PutValue(changeRequestObj.Number);
                    changeRequestSheet.Cells["M5"].PutValue(changeRequestObj.PECC2ReviewResultName);

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
                    if (changeRequestObj.IsFirst.GetValueOrDefault())
                    {
                        foreach (var docObj in docOfChangeRequest)
                        {
                            var dataRow = dtFull.NewRow();
                            dataRow["Temp"] = countDocOfTrans;
                            dataRow["NoIndex"] = countDocOfTrans;
                            dataRow["DocNo"] = docObj.DocumentNo;
                            dataRow["Rev"] = docObj.Revision;
                            dataRow["ReviewStatus"] = string.Empty;
                            dataRow["Comment"] = docObj.DocumentTitle;
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
                            foreach (var docObj in docOfChangeRequest)
                            {
                                var previousDataDocRow = previousDataTableRows.FirstOrDefault(t => t["Column3"].ToString() == docObj.DocumentNo);
                                if (previousDataDocRow != null)
                                {
                                    var previousDataDocRowList = previousDataTableRows.Where(t => t["Column1"].ToString() == previousDataDocRow["Column1"]);

                                    var dataRow = dtFull.NewRow();
                                    dataRow["Temp"] = countDocOfTrans;
                                    dataRow["NoIndex"] = countDocOfTrans;
                                    dataRow["DocNo"] = docObj.DocumentNo;
                                    dataRow["Rev"] = docObj.Revision;
                                    dataRow["ReviewStatus"] = string.Empty;
                                    dataRow["Comment"] = docObj.DocumentTitle;
                                    dtFull.Rows.Add(dataRow);

                                    var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                                    if (commentList.Count != 0)
                                    {
                                        for (var i = 0; i < commentList.Count; i++)
                                        {
                                            var commentIndex = commentList[i].Split('$')[0].Split('.')[0];
                                            var previousCommentsByIndex = previousDataDocRowList.Where(t => t["Column6"].ToString().Trim() == commentIndex.Trim());

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
                                    else
                                    {
                                        dtFull.Rows.Add(dataRow);
                                    }

                                    countDocOfTrans += 1;
                                }
                            }
                        }
                    }

                    changeRequestSheet.Cells.ImportDataTable(dtFull, false, 7, 2, dtFull.Rows.Count, dtFull.Columns.Count, true);
                    changeRequestSheet.Cells.DeleteRow(7 + dtFull.Rows.Count);

                    dataControlSheet.Cells["A2"].PutValue(dtFull.Rows.Count);

                    var filename = Utility.RemoveSpecialCharacterFileName(changeRequestObj.Number) + "_CRS.xlsm";
                    var saveFilePath = Server.MapPath("../.."+ changeRequestObj.StoreFolderPath + "/" + filename);
                    workbook.Save(saveFilePath);

                    // Attach CRS to document Obj
                    var fileInfo = new FileInfo(saveFilePath);
                    if (fileInfo.Exists && this.changeRequestAttachFileService.GetByChangeRequest(changeRequestObj.ID).All(t => t.TypeId != 4))
                    {
                        var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" 
                                               ? string.Empty 
                                               : HostingEnvironment.ApplicationVirtualPath) + changeRequestObj.StoreFolderPath;
                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + filename;
                        var attachFile = new ChangeRequestAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ChangeRequestId = changeRequestObj.ID,
                            FileName = filename,
                            Extension = "xlsm",
                            FilePath = serverFilePath,
                            ExtensionIcon = "~/images/excelfile.png",
                            FileSize = (double) fileInfo.Length/1024,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now,
                            TypeId = 4,
                            TypeName = "CRS File"
                        };

                        this.changeRequestAttachFileService.Insert(attachFile);
                    }
                    else
                    {
                        var crsObj = this.changeRequestAttachFileService.GetByChangeRequest(changeRequestObj.ID).FirstOrDefault(t => t.TypeId == 4);
                        if(crsObj!= null)
                        {
                            crsObj.CreatedDate = DateTime.Now;
                            crsObj.CreatedBy = UserSession.Current.User.Id;
                            crsObj.CreatedByName = UserSession.Current.User.UserNameWithFullName;
                            crsObj.FileSize = (double)fileInfo.Length / 1024;
                            this.changeRequestAttachFileService.Update(crsObj);
                        }
                    }
                    // -------------------------------------------------------------------------------------------------

                    this.grdAttachCRSFile.Rebind();
                }
            }
        }

        private List<string> GetCommentInfo(ChangeRequestDocFile docObj)
        {
            var consolidateDocFileList = this.changeRequestAttachFileService.GetByChangeRequestDocFile(docObj.ID);
            var commentList = new List<string>();
            if (consolidateDocFileList.Count != 0)
            {
                var consolidateDocFile = consolidateDocFileList.OrderByDescending(t=> t.CreatedDate).FirstOrDefault(t => t.TypeId == 3);
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
    }
}