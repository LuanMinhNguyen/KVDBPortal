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
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.UI;
    using Business.Services.Document;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestToDoListAttachRefDocFile : Page
    {
        private readonly ContractorTransmittalService contractorTransmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService;

        private readonly PECC2TransmittalService pecc2TransmittalService;

        private readonly PECC2DocumentsService _Pecc2DocumentService;

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly ChangeRequestDocFileService changeRequestDocFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ChangeRequestToDoListAttachRefDocFile()
        {
            this.contractorTransmittalService = new ContractorTransmittalService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
            this.pecc2TransmittalService = new PECC2TransmittalService();
            this._Pecc2DocumentService = new PECC2DocumentsService();
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
                this.ActionType.Value = this.Request.QueryString["actionType"];
                this.lbobjId.Value= this.Request.QueryString["objId"];

                this.DivCRS.Visible = this.Request.QueryString["actionType"] == "2"? false: true;
                //this.lblProjectIncomingId.Value = this.Request.QueryString["projId"];
                //if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                //{
                //    var transId = new Guid(this.Request.QueryString["objId"]);
                //    var contractorTransObj = this.transmittalService.GetById(transId);
                //    var pecc2TransObj = this.pecc2TransmittalService.GetById(transId);
                //    if (pecc2TransObj != null)
                //    {
                //        pecc2TransObj.IsOpen = true;
                //        this.pecc2TransmittalService.Update(pecc2TransObj);
                //    }
                //}
                this.LbcurrentAssignId.Value= this.Request.QueryString["currentAssignId"];
            }
        }

        protected void grdDocumentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var changeRequestId = new Guid(this.Request.QueryString["objId"]);
                this.grdDocumentFile.DataSource = this.changeRequestDocFileService.GetAllByChangeRequest(changeRequestId);
            }
        }

        protected void grdDocumentFile_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    var item = e.Item as GridDataItem;
            //    var errorPosition = item["ErrorPosition"].Text;
            //    if (!string.IsNullOrEmpty(errorPosition))
            //    {
            //        foreach (var position in errorPosition.Split('$').Where(t => !string.IsNullOrEmpty(t)))
            //        {
            //            switch (position)
            //            {
            //                case "0":
            //                    item["DocumentNo"].BackColor = Color.Red;
            //                    item["DocumentNo"].BorderColor = Color.Red;
            //                    break;
            //                case "1":
            //                    item["ProjectName"].BackColor = Color.Red;
            //                    item["ProjectName"].BorderColor = Color.Red;
            //                    break;
            //                case "2":
            //                    item["UnitCodeName"].BackColor = Color.Red;
            //                    item["UnitCodeName"].BorderColor = Color.Red;
            //                    break;
            //                case "4":
            //                    item["DrawingCodeName"].BackColor = Color.Red;
            //                    item["DrawingCodeName"].BorderColor = Color.Red;
            //                    break;
            //                case "5":
            //                    item["MaterialCodeName"].BackColor = Color.Red;
            //                    item["MaterialCodeName"].BorderColor = Color.Red;
            //                    break;
            //                case "7":
            //                    item["WorkCodeName"].BackColor = Color.Red;
            //                    item["WorkCodeName"].BorderColor = Color.Red;
            //                    break;
            //                case "14":
            //                    item["DisciplineCodeName"].BackColor = Color.Red;
            //                    item["DisciplineCodeName"].BorderColor = Color.Red;
            //                    break;
            //            }
            //        }
            //    }
            //}
        }

        protected void grdDocumentFile_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            //var item = (GridDataItem)e.Item;
            //var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            //var obj = this.contractorTransmittalDocFileService.GetById(objId);
            //var transObj = this.transmittalService.GetById(obj.TransId.GetValueOrDefault());

            //var physicalPath = Server.MapPath("../.." + obj.FilePath);
            //if (File.Exists(physicalPath))
            //{
            //    File.Delete(physicalPath);
            //}

            //this.contractorTransmittalDocFileService.Delete(objId);

            //var currentTransAttachDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);
            //if (!currentTransAttachDocFile.Any())
            //{
            //    transObj.Status = "Missing Doc File";
            //    transObj.IsValid = false;
            //    transObj.ErrorMessage = "Missing Attach Document File.";
            //}
            //else if (currentTransAttachDocFile.Any(t => !string.IsNullOrEmpty(t.Status)))
            //{
            //    transObj.Status = "Attach Doc File Invalid";
            //    transObj.ErrorMessage = "Some attach document files are invalid format.";
            //    transObj.IsValid = false;
            //}
            //else
            //{
            //    transObj.Status = string.Empty;
            //    transObj.ErrorMessage = string.Empty;
            //    transObj.IsValid = true;
            //}

            //this.transmittalService.Update(transObj);
            //this.grdDocumentFile.Rebind();
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
        }

        protected void grdAttachCRSFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var changeRequestObj = this.changeRequestService.GetById(objId);
            if (changeRequestObj != null)
            {
                var attachList = this.changeRequestAttachFileService.GetByChangeRequest(objId);
                this.grdAttachCRSFile.DataSource = attachList.Where(t => t.TypeId == 5);
            }
            else
            {
                this.grdAttachCRSFile.DataSource = new List<ChangeRequestAttachFile>();
            }
        }
    }
}