// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Business.Services.Document;
    using Business.Services.Library;
    using Business.Services.Security;
    using Data.Entities;
    using Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ContractorTransmittalAttachDocFileList : Page
    {
        private readonly ContractorTransmittalService transmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly PECC2TransmittalService pecc2TransmittalService;
        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ContractorTransmittalAttachDocFileList()
        {
            this.transmittalService = new ContractorTransmittalService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
            this.pecc2TransmittalService = new PECC2TransmittalService();
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
                this.lblProjectIncomingId.Value = this.Request.QueryString["projId"];
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var contractorTransObj = this.transmittalService.GetById(transId);
                    var PECC2TransObj = this.pecc2TransmittalService.GetById(contractorTransObj.PECC2TransId.GetValueOrDefault());
                    if (PECC2TransObj != null)
                    {
                        PECC2TransObj.IsOpen = true;
                        this.pecc2TransmittalService.Update(PECC2TransObj);
                    }
                }
                this.PECC2TransID.Value= this.Request.QueryString["PECC2TransID"]; }
                
        }

        protected void grdDocumentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var transId = new Guid(this.Request.QueryString["objId"]);
                var docFileList = this.contractorTransmittalDocFileService.GetAllByTrans(transId).OrderBy(t => t.FileName);

                this.grdDocumentFile.DataSource = docFileList;
            }
        }
        protected void grdAttachCRSFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var transId = new Guid(this.Request.QueryString["objId"]);
                var attachList =
                    this.contractorTransmittalAttachFileService.GetByTrans(transId);
                this.grdAttachCRSFile.DataSource = attachList;
            }
            else
            {
                this.grdAttachCRSFile.DataSource = new List<ContractorTransmittalAttachFile>();
            }

        }
        protected void grdDocumentFile_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                var errorPosition = item["ErrorPosition"].Text;
                if (!string.IsNullOrEmpty(errorPosition))
                {
                    foreach (var position in errorPosition.Split('$').Where(t => !string.IsNullOrEmpty(t)))
                    {
                        switch (position)
                        {
                            case "0":
                                item["DocumentNo"].BackColor = Color.Red;
                                item["DocumentNo"].BorderColor = Color.Red;
                                break;
                            case "1":
                                item["ProjectName"].BackColor = Color.Red;
                                item["ProjectName"].BorderColor = Color.Red;
                                break;
                            case "2":
                                item["UnitCodeName"].BackColor = Color.Red;
                                item["UnitCodeName"].BorderColor = Color.Red;
                                break;
                            case "4":
                                item["DrawingCodeName"].BackColor = Color.Red;
                                item["DrawingCodeName"].BorderColor = Color.Red;
                                break;
                            case "5":
                                item["MaterialCodeName"].BackColor = Color.Red;
                                item["MaterialCodeName"].BorderColor = Color.Red;
                                break;
                            case "7":
                                item["WorkCodeName"].BackColor = Color.Red;
                                item["WorkCodeName"].BorderColor = Color.Red;
                                break;
                            case "14":
                                item["DisciplineCodeName"].BackColor = Color.Red;
                                item["DisciplineCodeName"].BorderColor = Color.Red;
                                break;
                        }
                    }
                }
            }
        }

        protected void grdDocumentFile_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            var obj = this.contractorTransmittalDocFileService.GetById(objId);
            var transObj = this.transmittalService.GetById(obj.TransId.GetValueOrDefault());

            var physicalPath = Server.MapPath("../.." + obj.FilePath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            this.contractorTransmittalDocFileService.Delete(objId);

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

            this.transmittalService.Update(transObj);
            this.grdDocumentFile.Rebind();
        }

        protected void ajaxDocument_OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "DeleteAll")
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var transObj = this.transmittalService.GetById(transId);

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

                    this.transmittalService.Update(transObj);
                    // ----------------------------------------------------------------------------------------------------------

                    this.grdDocumentFile.Rebind();
                }
            }
        }
    }
}