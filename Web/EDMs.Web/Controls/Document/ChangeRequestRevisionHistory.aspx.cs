// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Telerik.Windows.Zip;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ChangeRequestRevisionHistory : Page
    {
        /// <summary>
        /// The document service.
        /// </summary>
        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;


        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public ChangeRequestRevisionHistory()
        {
            this.changeRequestService = new ChangeRequestService();
            this.changeRequestAttachFileService = new ChangeRequestAttachFileService();
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
           // this.InitGridColumnView(Convert.ToInt32(this.Request.QueryString["categoryId"]));
        }

        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if(Request.QueryString["docId"] != null)
            {
                Guid docId;
                Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);
                var objDoc = this.changeRequestService.GetById(docId);
                if (objDoc != null)
                {
                    var revList = this.changeRequestService.GetAllRevChangeRequest(objDoc.ParentId == null ? objDoc.ID : objDoc.ParentId.Value);
                    this.grdDocument.DataSource = revList;
                }
                else
                {
                    this.grdDocument.DataSource = new List<PECC2Documents>(); 
                }
            }
        }

 

        /// <summary>
        /// Grid KhacHang item created
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (Image)e.Item.FindControl("uploadLink");
                editLink.Attributes["href"] = "#";
                editLink.Attributes["onclick"] = string.Format(
                    "return ShowUploadForm('{0}');",
                    e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                grdDocument.MasterTableView.SortExpressions.Clear();
                grdDocument.MasterTableView.GroupByExpressions.Clear();
                grdDocument.Rebind();
            }
        }

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;

            Guid docId;
            Guid.TryParse(item.GetDataKeyValue("ID").ToString(), out docId);
            var objDocDelete = this.changeRequestService.GetById(docId);
           // var objDocDelete = this.documentService.GetById(documentId);

            if(objDocDelete != null)
            {
                if(!objDocDelete.IsLeaf.GetValueOrDefault())
                {
                    objDocDelete.IsDelete = true;
                    objDocDelete.LastUpdatedBy = UserSession.Current.User.Id;
                    objDocDelete.LastUpdatedDate = DateTime.Now;
                    this.changeRequestService.Update(objDocDelete);
                    ////this.documentService.Delete(documentId);
                }
                else
                {
                    var objDoc = this.changeRequestService.GetAllRevChangeRequest(objDocDelete.ParentId.GetValueOrDefault())[1];
                    objDoc.IsLeaf = true;
                    objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                    objDoc.LastUpdatedDate = DateTime.Now;

                    objDocDelete.IsLeaf = false;
                    objDocDelete.IsDelete = true;
                    objDocDelete.LastUpdatedBy = UserSession.Current.User.Id;
                    objDocDelete.LastUpdatedDate = DateTime.Now;

                    
                    
                    this.changeRequestService.Update(objDocDelete);
                    this.changeRequestService.Update(objDoc);
                }
            }

            this.grdDocument.Rebind();
        }

  

        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!e.Item.IsInEditMode && e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (DataBinder.Eval(item.DataItem, "ParentId") == null)
                {
                    item["DeleteColumn"].Controls[0].Visible = false;
                }
            }
            else if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
            }
        }

     
       
       

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }
                //Mo file de doc
                FileStream fs = new FileStream(strFileName, FileMode.Open);
                int streamLength = Convert.ToInt32(fs.Length);
                byte[] data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected void grdDocument_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "DocDetail":
                    {
                        var changeRequestId = new Guid(dataItem.GetDataKeyValue("ID").ToString());
                        e.DetailTableView.DataSource = this.changeRequestAttachFileService.GetAllByChangeRequest(changeRequestId);
                        break;
                    }
            }
        }

        private void InitGridColumnView(int categoryId)
        {
            switch (categoryId)
            {
                case 3:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Year").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
                case 4:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Year").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
                case 1:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Year").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
                case 2:
                    this.grdDocument.MasterTableView.GetColumn("ConfidentialityName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("AreaCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("UnitCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("SubsystemCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocTypeCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginatingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ReceivingOrganisationName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("GroupCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("OriginalDocumentNumber").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("PlannedDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("ActualDate").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Remarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevisionSchemaName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MinorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("MajorRev").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("Date").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevRemarks").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("RevStatusName").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocActionCode").Display = true;
                    this.grdDocument.MasterTableView.GetColumn("DocReviewStatusCode").Display = true;

                    this.grdDocument.MasterTableView.GetColumn("Year").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseRequiredDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ResponseToName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("CarbonCopyName").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("RelatedCSLNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IsNeedReply").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Description").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("Treatment").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ProposedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReviewedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedBy").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ApprovedDate").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IssuedDateFrom").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateTo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("ReceivedDateCC").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("KKSCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("TrainNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("DisciplineCode").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("SheetNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("IncomingTransNo").Display = false;
                    this.grdDocument.MasterTableView.GetColumn("OutgoingTransNo").Display = false;
                    break;
            }
        }
    }
}