// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.IO;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;
    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class TransmittalAttachDocument : Page
    {
        private readonly PECC2TransmittalService transmittalService;

        private readonly ProjectCodeService projectCodeService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly DisciplineService disciplineService;

        private readonly PECC2DocumentsService PECC2DocumentService;

        private readonly PECC2DocumentAttachFileService PECC2DocumentAttachFileService;

        private readonly PECC2TransmittalAttachDocFileService PECC2TransmittalAttachDocFileService;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        private readonly int TransmittalFolderId = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TransFolderId"));

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalAttachDocument"/> class.
        /// </summary>
        public TransmittalAttachDocument()
        {
            this.transmittalService = new PECC2TransmittalService();
            this.projectCodeService = new ProjectCodeService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.disciplineService = new DisciplineService();
            this.PECC2DocumentService = new PECC2DocumentsService();
            this.PECC2DocumentAttachFileService = new PECC2DocumentAttachFileService();
            this.PECC2TransmittalAttachDocFileService = new PECC2TransmittalAttachDocFileService();
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

            if (!this.Page.IsPostBack)
            {
                this.LoadComboData();
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
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.MasterTableView.CurrentPageIndex = this.grdDocument.MasterTableView.PageCount - 1;
                this.grdDocument.Rebind();
            }
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
            if (this.ddlDiscipline.Items.Count > 0)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                var disciplineId = Convert.ToInt32(this.ddlDiscipline.SelectedValue);

                var incomingTransId = new Guid(this.ddlIncomingTransmittal.SelectedValue);
                var incomingTransObj = this.transmittalService.GetById(incomingTransId);
                var docNumber = this.txtDocNumber.Text.Trim();
                var docTitle = this.txtDocTitle.Text.Trim();

                var isGetAllRevision = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("GetAllRevisionInTrans"));

                var pageSize = this.grdDocument.PageSize;
                var currentPage = this.grdDocument.CurrentPageIndex;
                var startingRecordNumber = currentPage * pageSize;

                var listDoc = this.PECC2DocumentService.SearchDocument(
                    projectId,
                    disciplineId,
                    docNumber,
                    docTitle,
                    string.Empty,
                    isGetAllRevision);

                // Filter by Incoming Trans
                if (incomingTransObj != null)
                {
                    listDoc = listDoc.Where(t => t.IncomingTransId == incomingTransObj.ID).ToList();
                }
                // --------------------------------------------------------------------------------------------------------------


                this.grdDocument.VirtualItemCount = listDoc.Count;


                this.grdDocument.DataSource = listDoc.OrderByDescending(t => t.ID).Skip(startingRecordNumber).Take(pageSize);
            }
        }

        /// <summary>
        /// The rad menu_ item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void radMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The btn search_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var listProject = this.projectCodeService.GetAll().OrderBy(t => t.Code).ToList();

            this.ddlProject.DataSource = listProject;
            this.ddlProject.DataTextField = "Fullname";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            var listDiscipline = this.disciplineService.GetAll().OrderBy(t => t.Code).ToList();

            listDiscipline.Insert(0, new Discipline() { ID = 0 });

            this.ddlDiscipline.DataSource = listDiscipline;
            this.ddlDiscipline.DataTextField = "Fullname";
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataBind();

            if (this.ddlProject.SelectedItem != null)
            {
                var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue), 1, string.Empty);
                incomingTransList.Insert(0, new PECC2Transmittal() { ID = Guid.NewGuid() });
                this.ddlIncomingTransmittal.DataSource = incomingTransList;
                this.ddlIncomingTransmittal.DataTextField = "TransmittalNo";
                this.ddlIncomingTransmittal.DataValueField = "ID";
                this.ddlIncomingTransmittal.DataBind();
            }
        }

        /// <summary>
        /// The btn save_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var objTrans = this.transmittalService.GetById(objId);
                var listSelectedDocId = new List<Guid>();
                var haveAttachDoc = false;
                if (objTrans != null)
                {
                    foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                    {
                        var cboxSelected = (CheckBox)item["IsSelected"].FindControl("cboxSelectDocTransmittal");
                        if (cboxSelected.Checked)
                        {
                            haveAttachDoc = true;
                            var docId = new Guid(item.GetDataKeyValue("ID").ToString());
                            var docObj = this.PECC2DocumentService.GetById(docId);

                            docObj.OutgoingTransId = objTrans.ID;
                            docObj.OutgoingTransNo = objTrans.TransmittalNo;
                            this.PECC2DocumentService.Update(docObj);

                            listSelectedDocId.Add(docId);

                            var attachDoc = new AttachDocToTransmittal()
                            {
                                TransmittalId = objId,
                                DocumentId = docId
                            };
                            if (!this.attachDocToTransmittalService.IsExist(objId, docId))
                            {
                                this.attachDocToTransmittalService.Insert(attachDoc);
                            }

                            cboxSelected.Checked = false;

                            // Get document file for outgoing trans
                            var markupCommentAttachDocList = this.PECC2DocumentAttachFileService.GetAllCmtResByDocId(docId);
                            foreach (var attachFile in markupCommentAttachDocList)
                            {
                                File.Copy(Server.MapPath("../.." + attachFile.FilePath), Path.Combine(Server.MapPath("../.." + objTrans.StoreFolderPath), attachFile.FileName), true);

                                var transAttachDocFile = new PECC2TransmittalAttachDocFiles
                                {
                                    ID = Guid.NewGuid(),
                                    DocumentId = docObj.ID,
                                    TransId = objTrans.ID,
                                    DocNo = docObj.DocNo,
                                    DocTitle = docObj.DocTitle,
                                    Revision = docObj.Revision,
                                    FileName = attachFile.FileName,
                                    ExtensionIcon = attachFile.ExtensionIcon,
                                    Extension = attachFile.Extension,
                                    FileSize = attachFile.FileSize,
                                    FilePath = objTrans.StoreFolderPath + "/" + attachFile.FileName
                                };

                                this.PECC2TransmittalAttachDocFileService.Insert(transAttachDocFile);
                            }
                            // -----------------------------------------------------------------------------------------------
                        }
                    }

                    // Update trans info
                    if (haveAttachDoc)
                    {
                        objTrans.IsValid = true;
                        objTrans.Status = string.Empty;
                        objTrans.ErrorMessage = string.Empty;

                        this.transmittalService.Update(objTrans);
                    }
                    // ---------------------------------------------------------
                }
            }
        }

        protected void ddlProject_SelectedIndexChange(object sender, EventArgs e)
        {
            if (this.ddlProject.SelectedItem != null)
            {
                var incomingTransList = this.transmittalService.GetAllByProject(Convert.ToInt32(this.ddlProject.SelectedValue), 1, string.Empty);
                incomingTransList.Insert(0, new PECC2Transmittal() { ID = Guid.NewGuid() });
                this.ddlIncomingTransmittal.DataSource = incomingTransList;
                this.ddlIncomingTransmittal.DataTextField = "TransmittalNo";
                this.ddlIncomingTransmittal.DataValueField = "ID";
                this.ddlIncomingTransmittal.DataBind();
            }

            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }
    }
}