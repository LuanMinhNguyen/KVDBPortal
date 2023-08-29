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
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Data.Entities;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.Text.RegularExpressions;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PECC2TransETRMAttach : Page
    {
        private readonly PECC2TransmittalAttachFileService transAttachFileService;

        private readonly PECC2TransmittalService transService;

        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2TransETRMAttach"/> class.
        /// </summary>
        public PECC2TransETRMAttach()
        {
            this.transAttachFileService = new PECC2TransmittalAttachFileService();
            this.transService = new PECC2TransmittalService();
            this.contractorTransmittalAttachFileService = new ContractorTransmittalAttachFileService();
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
                var pecc2TransId = new Guid(this.Request.QueryString["objId"]);
                var pecc2TransObj = this.transService.GetById(pecc2TransId);
                this.divContractorTransAttachFile.Visible = pecc2TransObj != null && pecc2TransObj.TypeId == 1;
                this.divUploadControl.Visible = string.IsNullOrEmpty(this.Request.QueryString["Onlyshow"]);

                if (this.transAttachFileService.GetByTrans(new Guid(this.Request.QueryString["objId"])).Where(t => t.TypeId == 1).Any())
                {

                    this.IsHasAttachFileeTRM.Value = "true";
                }
                else
                {
                    this.IsHasAttachFileeTRM.Value = "false";
                }
                this.eTRMChecked.Value = this.cbETRM.Checked.ToString();
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

        protected void btnSaveAttachFile_Click(object sender, EventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var transObj = this.transService.GetById(objId);
            var fileIcon = new Dictionary<string, string>()
                    {
                        { "doc", "~/images/wordfile.png" },
                        { "docx", "~/images/wordfile.png" },
                        { "dotx", "~/images/wordfile.png" },
                        { "xls", "~/images/excelfile.png" },
                        { "xlsx", "~/images/excelfile.png" },
                        { "pdf", "~/images/pdffile.png" },
                        { "7z", "~/images/7z.png" },
                        { "dwg", "~/images/dwg.png" },
                        { "dxf", "~/images/dxf.png" },
                        { "rar", "~/images/rar.png" },
                        { "zip", "~/images/zip.png" },
                        { "txt", "~/images/txt.png" },
                        { "xml", "~/images/xml.png" },
                        { "xlsm", "~/images/excelfile.png" },
                        { "bmp", "~/images/bmp.png" },
                    };

            var targetFolder = "../.." + transObj.StoreFolderPath + "/eTRM File";
            var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + transObj.StoreFolderPath + "/eTRM File";
            foreach (UploadedFile docFile in docuploader.UploadedFiles)
            {
                var fileExt = docFile.FileName.Substring(docFile.FileName.LastIndexOf(".") + 1,
                    docFile.FileName.Length - docFile.FileName.LastIndexOf(".") - 1);

                var filename = string.Empty;
                if (this.cbETRM.Checked || this.cbCRS.Checked)
                {
                    filename = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + (cbETRM.Checked ? "_Transmittal Cover Sheet." : "_CRS.") + fileExt;
                }
                else
                {
                    filename = docFile.FileName;
                }

                // Path file to save on server disc
                var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

                // Path file to download from server
                var serverFilePath = serverFolder + "/" + filename;

                var typeId = cbETRM.Checked
                                ? 1
                                : cbCRS.Checked
                                    ? 2
                                    : 3;
                var typeName = cbETRM.Checked
                                ? "Transmittal Cover Sheet"
                                : cbCRS.Checked
                                    ? "Comment Response Sheet (CRS)"
                                    : "Other File";
                docFile.SaveAs(saveFilePath, true);
                if (typeId == 3 || this.transAttachFileService.GetByTrans(transObj.ID).All(t => t.TypeId != typeId))
                {
                    var attachFile = new PECC2TransmittalAttachFiles()
                    {
                        ID = Guid.NewGuid(),
                        TransId = objId,
                        Filename = filename,
                        Extension = fileExt,
                        FilePath = serverFilePath,
                        ExtensionIcon =
                            fileIcon.ContainsKey(fileExt.ToLower())
                                ? fileIcon[fileExt.ToLower()]
                                : "~/images/otherfile.png",
                        FileSize = (double)docFile.ContentLength / 1024,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                        TypeId = typeId,
                        TypeName = typeName
                    };
                    this.transAttachFileService.Insert(attachFile);
                }
            }

            this.docuploader.UploadedFiles.Clear();
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());

            this.transAttachFileService.Delete(objId);
            this.grdAttachFile.Rebind();
        }

        protected void grdAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var attachList = this.transAttachFileService.GetByTrans(objId);

            this.grdAttachFile.DataSource = attachList;
        }

        protected void grdContractorTransAttachFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var pecc2TransId = new Guid(this.Request.QueryString["objId"]);
            var pecc2TransObj = this.transService.GetById(pecc2TransId);
            if (pecc2TransObj != null && pecc2TransObj.ContractorTransId != null)
            {
                var attachList =
                    this.contractorTransmittalAttachFileService.GetByTrans(
                        pecc2TransObj.ContractorTransId.GetValueOrDefault());

                this.grdContractorTransAttachFile.DataSource = attachList;
            }
            else
            {
                this.grdContractorTransAttachFile.DataSource = new List<ContractorTransmittalAttachFile>();

            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "UploadFile")
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var transObj = this.transService.GetById(objId);
                var fileIcon = new Dictionary<string, string>()
                    {
                        { "doc", "~/images/wordfile.png" },
                        { "docx", "~/images/wordfile.png" },
                        { "dotx", "~/images/wordfile.png" },
                        { "xls", "~/images/excelfile.png" },
                        { "xlsx", "~/images/excelfile.png" },
                        { "pdf", "~/images/pdffile.png" },
                        { "7z", "~/images/7z.png" },
                        { "dwg", "~/images/dwg.png" },
                        { "dxf", "~/images/dxf.png" },
                        { "rar", "~/images/rar.png" },
                        { "zip", "~/images/zip.png" },
                        { "txt", "~/images/txt.png" },
                        { "xml", "~/images/xml.png" },
                        { "xlsm", "~/images/excelfile.png" },
                        { "bmp", "~/images/bmp.png" },
                    };

                var targetFolder = "../.." + transObj.StoreFolderPath + "/eTRM File";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                        + transObj.StoreFolderPath + "/eTRM File";
                foreach (UploadedFile docFile in docuploader.UploadedFiles)
                {
                    var fileExt = docFile.FileName.Substring(docFile.FileName.LastIndexOf(".") + 1,
                        docFile.FileName.Length - docFile.FileName.LastIndexOf(".") - 1);
                    var filename = string.Empty;
                    if (this.cbETRM.Checked)
                    {
                        filename = Regex.Replace(transObj.TransmittalNo + "_Transmittal Cover Sheet", @"[^0-9a-zA-Z _-]+", string.Empty) + "." + fileExt;
                        var objETRM = this.transAttachFileService.GetByTrans(new Guid(this.Request.QueryString["objId"])).FirstOrDefault(t => t.TypeId == 1);
                        if (objETRM != null)
                        {

                            try
                            {
                                if (File.Exists(Server.MapPath(@"../.." + objETRM.FilePath)))
                                {
                                    // File.SetAttributes(objETRM.FilePath, FileAttributes.Normal);
                                    File.Delete(Server.MapPath(@"../.." + objETRM.FilePath));
                                }
                                this.transAttachFileService.Delete(objETRM);
                               
                            }
                            catch { }

                        }
                       

                    }
                    else if (this.cbCRS.Checked)
                    {
                        filename = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + "_CRS." + fileExt;
                    }
                    else
                    {
                        filename = docFile.FileName;
                    }



                    // Path file to save on server disc
                    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

                    // Path file to download from server
                    var serverFilePath = serverFolder + "/" + filename;

                    var typeId = cbETRM.Checked
                                    ? 1
                                    : cbCRS.Checked
                                        ? 2
                                        : 3;
                    var typeName = cbETRM.Checked
                                    ? "eTRM File"
                                    : cbCRS.Checked
                                        ? "CRS File"
                                        : "Other File";
                    docFile.SaveAs(saveFilePath, true);
                    if (typeId == 3 || this.transAttachFileService.GetByTrans(transObj.ID).All(t => t.TypeId != typeId))
                    {
                        var attachFile = new PECC2TransmittalAttachFiles()
                        {
                            ID = Guid.NewGuid(),
                            TransId = objId,
                            Filename = filename,
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon =
                             fileIcon.ContainsKey(fileExt.ToLower())
                                 ? fileIcon[fileExt.ToLower()]
                                 : "~/images/otherfile.png",
                            FileSize = (double)docFile.ContentLength / 1024,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.FullName,
                            CreatedDate = DateTime.Now,
                            TypeId = typeId,
                            TypeName = typeName
                        };
                        this.transAttachFileService.Insert(attachFile);
                        this.IsHasAttachFileeTRM.Value = "true";
                    }


                }

                this.docuploader.UploadedFiles.Clear();
                this.grdAttachFile.Rebind();
            }

           
           
        }
        //protected void cbETRM_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.eTRMChecked.Value = this.cbETRM.Checked.ToString();
        //}

        //protected void cbCRS_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.eTRMChecked.Value = this.cbETRM.Checked.ToString();
        //}

        //protected void cbOther_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.eTRMChecked.Value = this.cbETRM.Checked.ToString();
        //}
    }
}