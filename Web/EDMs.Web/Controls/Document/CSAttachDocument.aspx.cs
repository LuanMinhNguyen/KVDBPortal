// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Linq;
using Aspose.Cells;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using iTextSharp.text.pdf;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.UI;
    using EDMs.Business.Services.Document;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class CSAttachDocument : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly NCR_SIService _NCR_SIervice;

        private readonly NCR_SIAttachFileService attachFileService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public CSAttachDocument()
        {
            this._NCR_SIervice = new  NCR_SIService();
            this.attachFileService = new   NCR_SIAttachFileService();
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
                if (UserSession.Current.User.IsEngineer.GetValueOrDefault() || UserSession.Current.User.IsLeader.GetValueOrDefault()|| 
                    !UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
                {
                   
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }
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
            this.Session.Remove("IsFillData");
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                Guid objId;
                Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);
                var docObj = this._NCR_SIervice.GetById(objId);
              
                var flag = false;
                var targetFolder =  "../../DocumentLibrary/NCRSI" ;
                var serverFolder =  (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) 
                    +  "/DocumentLibrary/NCRSI";


                var listUpload = docuploader.UploadedFiles;

                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileName = docFile.FileName;
                        var fileExt = docFile.GetExtension();

                        var serverDocFileName = docFileName;

                        //if (this.ddlType.SelectedValue == "2")
                        //{
                        //    serverDocFileName = docFile.GetNameWithoutExtension() + "_MarkupConsolidate_" + UserSession.Current.User.Username + fileExt;
                        //}
                        //else if (this.ddlType.SelectedValue == "3")
                        //{
                        //    serverDocFileName = docFile.GetNameWithoutExtension() + "_Markup_" + UserSession.Current.User.Username + fileExt;
                        //}

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new NCR_SIAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            NCR_SIId = objId,
                            FileName = serverDocFileName,
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                            FileSize = (double)docFile.ContentLength / 1024,
                            TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
                            TypeName = this.ddlType.SelectedItem.Text,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now
                        };

                        this.attachFileService.Insert(attachFile);

                        // Extract only comment page
                        if (this.ddlType.SelectedValue == "3" && fileExt.ToLower() == ".pdf")
                        {
                            var outputOnlyMarkupPageFileName = docFile.GetNameWithoutExtension() + "_OnlyMarkupPages_" + UserSession.Current.User.Username + fileExt;
                            var outputOnlyMarkupPageFilePath = Server.MapPath("../../DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName);
                            PdfImportedPage importedPage = null;
                            var pdfReader = new PdfReader(saveFilePath);
                            
                            var sourceDocument = new iTextSharp.text.Document(pdfReader.GetPageSizeWithRotation(1));
                            var pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outputOnlyMarkupPageFilePath, FileMode.Create));
                            sourceDocument.Open();
                            for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                            {
                                PdfArray array = pdfReader.GetPageN(i).GetAsArray(PdfName.ANNOTS);
                                if (array != null)
                                {
                                    importedPage = pdfCopyProvider.GetImportedPage(pdfReader, i);
                                    pdfCopyProvider.AddPage(importedPage);
                                }
                            }

                            sourceDocument.Close();
                            pdfReader.Close();

                            // Add to attach file
                            var fileInfo = new FileInfo(outputOnlyMarkupPageFilePath);
                            if (fileInfo.Exists)
                            {
                                var onlyMarkupPageAttachFile = new NCR_SIAttachFile()
                                {
                                    ID = Guid.NewGuid(),
                                    NCR_SIId = objId,
                                    FileName = outputOnlyMarkupPageFileName,
                                    Extension = fileExt,
                                    FilePath = "/DocumentLibrary/MarkupFile/" + outputOnlyMarkupPageFileName,
                                    ExtensionIcon = Utility.FileIcon.ContainsKey(fileExt.ToLower()) ? Utility.FileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                    FileSize = (double)fileInfo.Length / 1024,
                                    TypeId = Convert.ToInt32(this.ddlType.SelectedValue),
                                    TypeName = this.ddlType.SelectedItem.Text,
                                    CreatedBy = UserSession.Current.User.Id,
                                    CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                    CreatedDate = DateTime.Now,
                                    IsOnlyMarkupPage = true
                                };

                                this.attachFileService.Insert(onlyMarkupPageAttachFile);
                            }
                            // -------------------------------------------------------------------------------------------------------------------
                        }
                        
                        // -------------------------------------------------------------------------------------------------


                    }

                    docObj.IsHasAttachFile = this.attachFileService.GetByNCRSI(objId).Any();
                    this._NCR_SIervice.Update(docObj);
                }
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdDocument.Rebind();
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            Guid objId;
            Guid.TryParse(item.GetDataKeyValue("ID").ToString(), out objId);
            this.attachFileService.Delete(objId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var attachList = this.attachFileService.GetByNCRSI(objId).OrderBy(t => t.CreatedDate);
                this.btnExportCRS.Visible = attachList.Any(t => t.TypeId == 3);
                this.grdDocument.DataSource = attachList.Where(t=> !t.IsOnlyMarkupPage.GetValueOrDefault());
            }
            else
            {
                this.grdDocument.DataSource = new List<NCR_SIAttachFile>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void btnExportCRS_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var consolidateDocFile = this.attachFileService.GetByNCRSI(new Guid(Request.QueryString["objId"])).FirstOrDefault(t => t.TypeId == 3);
                if (consolidateDocFile != null)
                {
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook(filePath + @"Template\PECC2_CRS_Template.xlsm");
                    var dataSheet = workbook.Worksheets[0];

                    var docObj = this._NCR_SIervice.GetById(consolidateDocFile.NCR_SIId.GetValueOrDefault());
                    if (docObj != null)
                    {
                        dataSheet.Cells["B4"].PutValue("Doc. No.: " + docObj.Number);
                        dataSheet.Cells["B5"].PutValue("Rev. No.: " + docObj.Subject);
                        dataSheet.Cells["B6"].PutValue("Transmittal Contractor: " + Environment.NewLine + string.Empty);
                        dataSheet.Cells["E4"].PutValue(docObj.Subject);
                        dataSheet.Cells["H4"].PutValue(string.Empty);
                        dataSheet.Cells["H5"].PutValue(string.Empty);
                        dataSheet.Cells["G6"].PutValue("Transmittal No.: " + Environment.NewLine + string.Empty);
                        var dtFull = new DataTable();
                        dtFull.Columns.AddRange(new[]
                        {
                        new DataColumn("NoIndex", typeof (String)),
                        new DataColumn("FileName", typeof (String)),
                        new DataColumn("Page", typeof (String)),
                        new DataColumn("Comment", typeof (String)),
                        new DataColumn("Review", typeof (String)),
                        new DataColumn("User", typeof (String)),
                        new DataColumn("Confirmation", typeof (String)),
                        });

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
                                    var username = annot.GetAsString(PdfName.T);
                                    count += 1;
                                    var dataRow = dtFull.NewRow();
                                    dataRow["NoIndex"] = count;
                                    dataRow["FileName"] = consolidateDocFile.FileName.Replace("_Consolidate", string.Empty);
                                    dataRow["Page"] = i;
                                    dataRow["Comment"] = text;
                                    dataRow["Review"] = username;
                                    dataRow["User"] = consolidateDocFile.CreatedByName.Split('/')[0];
                                    dataRow["Confirmation"] = string.Empty;

                                    dtFull.Rows.Add(dataRow);
                                }
                            }
                        }
                        dataSheet.Cells.ImportDataTable(dtFull, false, 8, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
                        dataSheet.Cells.DeleteRow(8 + dtFull.Rows.Count);

                        var filename = Utility.RemoveSpecialCharacterFileName(docObj.Number) + "_CRS.xlsm";
                        var saveFilePath = Server.MapPath("../../DocumentLibrary/MarkupFile/" + filename);
                        workbook.Save(saveFilePath);
                        // Attach CRS to document Obj
                        var fileInfo = new FileInfo(saveFilePath);
                        if (fileInfo.Exists && this.attachFileService.GetByNCRSI(docObj.ID).All(t => t.TypeId != 3))
                        {
                            var attachFile = new NCR_SIAttachFile()
                            {
                                ID = Guid.NewGuid(),
                                NCR_SIId = docObj.ID,
                                FileName = filename,
                                Extension = ".xlsm",
                                FilePath = "/DocumentLibrary/MarkupFile/" + filename,
                                ExtensionIcon = "~/images/excelfile.png",
                                FileSize = (double)fileInfo.Length / 1024,
                                TypeId = 4,
                                TypeName = "Comment Sheet",
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedByName = UserSession.Current.User.UserNameWithFullName,
                                CreatedDate = DateTime.Now
                            };

                            this.attachFileService.Insert(attachFile);
                        }
                        // -------------------------------------------------------------------------------------------------

                        this.Download_File(saveFilePath);
                    }
                }
            }
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }
    }
}