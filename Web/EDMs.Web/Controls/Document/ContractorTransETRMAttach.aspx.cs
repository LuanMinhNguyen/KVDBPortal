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
    public partial class ContractorTransETRMAttach : Page
    {
        private readonly ContractorTransmittalAttachFileService transAttachFileService;

        private readonly ContractorTransmittalService transService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractorTransETRMAttach"/> class.
        /// </summary>
        public ContractorTransETRMAttach()
        {
            this.transAttachFileService = new ContractorTransmittalAttachFileService();
            this.transService = new ContractorTransmittalService();
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
                var objId = new Guid(this.Request.QueryString["objId"]);
                var transObj = this.transService.GetById(objId);
                this.divUploadControl.Visible = transObj != null && !transObj.IsSend.GetValueOrDefault();
           
            if (this.transAttachFileService.GetByTrans(new Guid(this.Request.QueryString["objId"])).Where(t => t.TypeId == 1).Any())
            {
                
                this.IsHasAttachFileeTRM.Value = "true";
            }
            else
            {
                this.IsHasAttachFileeTRM.Value = "false";
            }
            this.eTRMChecked.Value = this.cbETRM.Checked.ToString();
        } }

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
          //if(this.IsHasAttachFileeTRM.Value=="false" || !this.cbETRM.Checked)
          //  {
          //      SaveAttachFile();
          //  }

            //var objId = new Guid(this.Request.QueryString["objId"]);
            //var transObj = this.transService.GetById(objId);
            //var fileIcon = new Dictionary<string, string>()
            //        {
            //            { "doc", "~/images/wordfile.png" },
            //            { "docx", "~/images/wordfile.png" },
            //            { "dotx", "~/images/wordfile.png" },
            //            { "xls", "~/images/excelfile.png" },
            //            { "xlsx", "~/images/excelfile.png" },
            //            { "pdf", "~/images/pdffile.png" },
            //            { "7z", "~/images/7z.png" },
            //            { "dwg", "~/images/dwg.png" },
            //            { "dxf", "~/images/dxf.png" },
            //            { "rar", "~/images/rar.png" },
            //            { "zip", "~/images/zip.png" },
            //            { "txt", "~/images/txt.png" },
            //            { "xml", "~/images/xml.png" },
            //            { "xlsm", "~/images/excelfile.png" },
            //            { "bmp", "~/images/bmp.png" },
            //        };

            //var targetFolder = "../.." + transObj.StoreFolderPath + "/eTRM File";
            //var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
            //        + transObj.StoreFolderPath + "/eTRM File";
            //foreach (UploadedFile docFile in docuploader.UploadedFiles)
            //{
            //    var fileExt = docFile.FileName.Substring(docFile.FileName.LastIndexOf(".") + 1,
            //        docFile.FileName.Length - docFile.FileName.LastIndexOf(".") - 1);
            //    var filename = string.Empty;
            //    if (this.cbETRM.Checked)
            //    {
            //        filename = Regex.Replace(transObj.TransNo+"_"+transObj.Description, @"[^0-9a-zA-Z _-]+", string.Empty)+"."+ fileExt;
                  

            //    } else if(this.cbCRS.Checked)
            //    {
            //        filename = Utility.RemoveSpecialCharacterFileName(transObj.TransNo) +  "_CRS." + fileExt;
            //    }
            //    else
            //    {
            //        filename = docFile.FileName;
            //    }
                


            //    // Path file to save on server disc
            //    var saveFilePath = Path.Combine(Server.MapPath(targetFolder), filename);

            //    // Path file to download from server
            //    var serverFilePath = serverFolder + "/" + filename;

            //    var typeId = cbETRM.Checked
            //                    ? 1
            //                    : cbCRS.Checked
            //                        ? 2
            //                        : 3;
            //    var typeName = cbETRM.Checked
            //                    ? "eTRM File"
            //                    : cbCRS.Checked
            //                        ? "CRS File"
            //                        : "Other File";
            //    docFile.SaveAs(saveFilePath, true);
            //    if (typeId == 3 || this.transAttachFileService.GetByTrans(transObj.ID).All(t => t.TypeId != typeId))
            //    {
            //        var attachFile = new ContractorTransmittalAttachFile()
            //        {
            //            ID = Guid.NewGuid(),
            //            ContractorTransId = objId,
            //            Filename = filename,
            //            Extension = fileExt,
            //            FilePath = serverFilePath,
            //            ExtensionIcon =
            //                fileIcon.ContainsKey(fileExt.ToLower())
            //                    ? fileIcon[fileExt.ToLower()]
            //                    : "~/images/otherfile.png",
            //            FileSize = (double) docFile.ContentLength/1024,
            //            CreatedBy = UserSession.Current.User.Id,
            //            CreatedByName = UserSession.Current.User.FullName,
            //            CreatedDate = DateTime.Now,
            //            TypeId = typeId,
            //            TypeName = typeName
            //        };

            //        this.transAttachFileService.Insert(attachFile);
            //    }

                
            //}

            //this.docuploader.UploadedFiles.Clear();
             
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
                        filename = Regex.Replace(transObj.TransNo + "_Transmittal Cover Sheet", @"[^0-9a-zA-Z _-]+", string.Empty) + "." + fileExt;
                        var objETRM = this.transAttachFileService.GetByTrans(new Guid(this.Request.QueryString["objId"])).FirstOrDefault(t => t.TypeId == 1);
                        if (objETRM != null)
                        {
                           
                            try
                            {   this.transAttachFileService.Delete(objETRM);
                                if (File.Exists(Server.MapPath(@"../.." + objETRM.FilePath)))
                                {
                                    File.Delete(Server.MapPath(@"../.." + objETRM.FilePath));
                                }
                                
                                this.IsHasAttachFileeTRM.Value = "true";
                            }
                            catch { }
                            
                        }
                       

                    }
                    else if (this.cbCRS.Checked)
                    {
                        filename = Utility.RemoveSpecialCharacterFileName(transObj.TransNo) + "_CRS." + fileExt;
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
                        var attachFile = new ContractorTransmittalAttachFile()
                        {
                            ID = Guid.NewGuid(),
                            ContractorTransId = objId,
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
        }

        //protected void cbETRM_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.eTRMChecked.Value = "true";
        //}

        //protected void cbCRS_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.eTRMChecked.Value = "false";
        //}

        //protected void cbOther_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.eTRMChecked.Value = "false";
        //}
    }
}