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
    using Telerik.Web.Zip;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocListByTag : Page
    {
        /// <summary>
        /// The document service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The status service.
        /// </summary>
        private readonly StatusService statusService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The received from.
        /// </summary>
        private readonly ReceivedFromService receivedFromService;

        private readonly DocPropertiesViewService docPropertiesViewService;

        private readonly DocumentNewService documentNewService;

        private readonly AttachFileService attachFileService;

        protected const string ServiceName = "EDMSFolderWatcher";

        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public DocListByTag()
        {
            this.documentService = new DocumentService();
            this.revisionService = new RevisionService();
            this.documentTypeService = new DocumentTypeService();
            this.statusService = new StatusService();
            this.disciplineService = new DisciplineService();
            this.receivedFromService = new ReceivedFromService();
            this.docPropertiesViewService = new DocPropertiesViewService();
            this.documentNewService = new DocumentNewService();
            this.attachFileService = new AttachFileService();
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
            
            if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
            {
                ////this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                ////this.grdDocument.MasterTableView.GetColumn("ReUpload").Visible = false;
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
            if (Request.QueryString["tagId"] != null)
            {
                var tagId = Request.QueryString["tagId"];
                var docList = this.documentNewService.GetAllCurrentDoc().Where(t => !string.IsNullOrEmpty(t.TagTypeId) && t.TagTypeId.Split(',').Contains(tagId));
                this.grdDocument.DataSource = docList;
            }
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
            var isViewByGroup = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ViewSettingByGroup"));
            var selectedProperty = new List<string>();

            if (Request.QueryString["tagId"] != null && Session["SelectedCategory"] != null)
            {
                var categoryId = Convert.ToInt32(Session["SelectedCategory"]);
                var deparmentId = isViewByGroup ? UserSession.Current.RoleId : 0;
                foreach (var docPropertiesView in this.docPropertiesViewService.GetAllSpecial(categoryId, deparmentId))
                {
                    var temp = docPropertiesView.PropertyIndex.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())).Select(t => t.Trim()).ToList();
                    selectedProperty.AddRange(temp);
                }

                selectedProperty = selectedProperty.Distinct().ToList();

                for (int i = 1; i <= 30; i++)
                {
                    var column = this.grdDocument.MasterTableView.GetColumn("Index" + i);
                    if (column != null)
                    {
                        column.Visible = selectedProperty.Contains(i.ToString());
                    }
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
            var documentId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var objDocDelete = this.documentService.GetById(documentId);

            if(objDocDelete != null)
            {
                if(!objDocDelete.IsLeaf.GetValueOrDefault())
                {
                    objDocDelete.IsDelete = true;
                    objDocDelete.LastUpdatedBy = UserSession.Current.User.Id;
                    objDocDelete.LastUpdatedDate = DateTime.Now;
                    this.documentService.Update(objDocDelete);
                    ////this.documentService.Delete(documentId);
                }
                else
                {
                    var objDoc = this.documentService.GetAllDocRevision(objDocDelete.ParentID.GetValueOrDefault())[1];
                    objDoc.IsLeaf = true;
                    objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                    objDoc.LastUpdatedDate = DateTime.Now;

                    objDocDelete.IsLeaf = false;
                    objDocDelete.IsDelete = true;
                    objDocDelete.LastUpdatedBy = UserSession.Current.User.Id;
                    objDocDelete.LastUpdatedDate = DateTime.Now;

                    var revisionPath = Server.MapPath(objDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                    var filePath = Server.MapPath(objDoc.FilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));
                    var fileInfo = new FileInfo(revisionPath);

                    var watcherService = new ServiceController(ServiceName);

                    if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                    {
                        watcherService.ExecuteCommand(128);
                    }

                    if (fileInfo.Exists)
                    {
                        fileInfo.CopyTo(filePath, true);    
                    }

                    if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                    {
                        watcherService.ExecuteCommand(129);
                    }
                    
                    this.documentService.Update(objDocDelete);
                    this.documentService.Update(objDoc);
                }
            }

            this.grdDocument.Rebind();
        }

        /// <summary>
        /// The grd document_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridDataItem item = (GridDataItem)grdDocument.SelectedItems[0];
            if (item != null)
            {
                int docId = int.Parse(item["ID"].Text);
                ////Patient kh = patientService.GetByID(customerID);

                //Bind to controls
                ////lblCustomerID.Value = customerID.ToString();
                ////lblTenKhachHang.Text = kh.FullName;
                ////lblMaKH.Text = kh.SSN;
                ////lblNgheNghiep.Text = kh.Occupation;
                ////lblDiaChi.Text = kh.Address1;
                ////lblDienThoai.Text = kh.CellPhone;
                ////lblEmail.Text = kh.Email;
                ////lblCMND.Text = kh.IdentityCard;
                ////lblNgayCap.Text = "N/A";
                ////lblNoiCap.Text = "N/A";
                ////lblTinhTrang.Text = kh.PatientStatus.Name;
                //////Ghi chu cuoi cung cua Customer
                ////Patient_DescriptionService descriptionService = new Patient_DescriptionService();
                ////Description ds = descriptionService.Find(x => x.CustomerID.Value == customerID).OrderByDescending(x => x.ID).FirstOrDefault();
                ////lblGhiChu.Text = ds==null?"N/A":ds.Content;
                ////if (lblGhiChu.Text != "N/A")
                ////{
                ////    lblGhiChu.ForeColor = System.Drawing.Color.Red;
                ////    lblGhiChu.Font.Bold = true;
                ////}
                ////else
                ////{
                ////    lblGhiChu.ForeColor = System.Drawing.Color.Black;
                ////    lblGhiChu.Font.Bold = false;
                ////}
                //End Ghi chu
            }
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
                var item = e.Item as GridEditableItem;
                var txtRevisionName = item.FindControl("txtRevisionName") as Label;
                var ddlStatus = item.FindControl("ddlStatus") as RadComboBox;
                var ddlDiscipline = item.FindControl("ddlDiscipline") as RadComboBox;
                var ddlDocumentType = item.FindControl("ddlDocumentType") as RadComboBox;
                var ddlReceivedFrom = item.FindControl("ddlReceivedFrom") as RadComboBox;
                var txtReceivedDate = item.FindControl("txtReceivedDate") as RadDatePicker;
                var txtRevisionFileName = item.FindControl("txtRevisionFileName") as TextBox;
                var txtDocumentNumber = item.FindControl("txtDocumentNumber") as TextBox;
                var txtTitle = item.FindControl("txtTitle") as TextBox;
                var txtRemark = item.FindControl("txtRemark") as TextBox;
                var txtWell = item.FindControl("txtWell") as TextBox;
                var txtTransmittalNumber = item.FindControl("txtTransmittalNumber") as TextBox;

                txtReceivedDate.DatePopupButton.Visible = false;

                var revisionFileName = (item.FindControl("RevisionFileName") as HiddenField).Value;
                var revisionName = (item.FindControl("RevisionName") as HiddenField).Value;
                var statusId = (item.FindControl("StatusID") as HiddenField).Value;
                var disciplineId = (item.FindControl("DisciplineID") as HiddenField).Value;
                var documentTypeId = (item.FindControl("DocumentTypeID") as HiddenField).Value;
                var receivedFromId = (item.FindControl("ReceivedFromID") as HiddenField).Value;
                var receivedDate = (item.FindControl("ReceivedDate") as HiddenField).Value;
                var remark = (item.FindControl("Remark") as HiddenField).Value;
                var well = (item.FindControl("Well") as HiddenField).Value;
                var title = (item.FindControl("Title") as HiddenField).Value;
                var documentNumber = (item.FindControl("DocumentNumber") as HiddenField).Value;
                var transmittalNumber = (item.FindControl("TransmittalNumber") as HiddenField).Value;

                if (!string.IsNullOrEmpty(receivedDate))
                {
                    txtReceivedDate.SelectedDate = Convert.ToDateTime(receivedDate);
                }

                txtRevisionFileName.Text = revisionFileName;
                txtTitle.Text = title;
                txtRemark.Text = remark;
                txtWell.Text = well;
                txtDocumentNumber.Text = documentNumber;
                txtTransmittalNumber.Text = transmittalNumber;

                txtRevisionName.Text = revisionName;

                ////var revisionList = this.revisionService.GetAll();
                ////revisionList.Insert(0, new Revision() { Name = string.Empty });
                ////ddlRevision.DataSource = revisionList;
                ////ddlRevision.DataValueField = "ID";
                ////ddlRevision.DataTextField = "Name";
                ////ddlRevision.DataBind();
                ////ddlRevision.SelectedValue = revisionId;

                var categoryId = 0;

                if (!string.IsNullOrEmpty(this.Request.QueryString["categoryId"]))
                {
                    categoryId = Convert.ToInt32(this.Request.QueryString["categoryId"]);
                }

                var documentTypeList = this.documentTypeService.GetAll();
                documentTypeList.Insert(0, new DocumentType() { Name = string.Empty });
                ddlDocumentType.DataSource = documentTypeList;
                ddlDocumentType.DataValueField = "ID";
                ddlDocumentType.DataTextField = "Name";
                ddlDocumentType.DataBind();
                ddlDocumentType.SelectedValue = documentTypeId;

                var statusList = this.statusService.GetAllByCategory(categoryId);
                statusList.Insert(0, new Status { Name = string.Empty });
                ddlStatus.DataSource = statusList;
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataBind();
                ddlStatus.SelectedValue = statusId;

                var receivedFromList = this.receivedFromService.GetAllByCategory(categoryId);
                receivedFromList.Insert(0, new ReceivedFrom() { Name = string.Empty });
                ddlReceivedFrom.DataSource = receivedFromList;
                ddlReceivedFrom.DataValueField = "ID";
                ddlReceivedFrom.DataTextField = "Name";
                ddlReceivedFrom.DataBind();
                ddlReceivedFrom.SelectedValue = receivedFromId;

                var disciplineList = this.disciplineService.GetAllByCategory(categoryId);
                disciplineList.Insert(0, new Discipline() { Name = string.Empty });
                ddlDiscipline.DataSource = disciplineList;
                ddlDiscipline.DataValueField = "ID";
                ddlDiscipline.DataTextField = "Name";
                ddlDiscipline.DataBind();
                ddlDiscipline.SelectedValue = disciplineId;
            }
        }

        protected void grdDocument_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                var item = e.Item as GridEditableItem;
                var ddlStatus = item.FindControl("ddlStatus") as RadComboBox;
                var ddlDiscipline = item.FindControl("ddlDiscipline") as RadComboBox;
                var ddlDocumentType = item.FindControl("ddlDocumentType") as RadComboBox;
                var ddlReceivedFrom = item.FindControl("ddlReceivedFrom") as RadComboBox;
                var txtReceivedDate = item.FindControl("txtReceivedDate") as RadDatePicker;
                var txtRevisionFileName = item.FindControl("txtRevisionFileName") as TextBox;
                var txtDocumentNumber = item.FindControl("txtDocumentNumber") as TextBox;
                var txtTitle = item.FindControl("txtTitle") as TextBox;
                var txtRemark = item.FindControl("txtRemark") as TextBox;
                var txtWell = item.FindControl("txtWell") as TextBox;
                var txtTransmittalNumber = item.FindControl("txtTransmittalNumber") as TextBox;

                var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));
                var objDoc = this.documentService.GetById(docId);
                if (objDoc != null)
                {
                    objDoc.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objDoc.DisciplineID = Convert.ToInt32(ddlDiscipline.SelectedValue);
                    objDoc.DocumentTypeID = Convert.ToInt32(ddlDocumentType.SelectedValue);
                    objDoc.ReceivedFromID = Convert.ToInt32(ddlReceivedFrom.SelectedValue);
                    objDoc.ReceivedDate = txtReceivedDate.SelectedDate;
                    objDoc.RevisionFileName = txtRevisionFileName.Text.Trim();
                    objDoc.DocumentNumber = txtDocumentNumber.Text.Trim();
                    objDoc.Title = txtTitle.Text.Trim();
                    objDoc.Remark = txtRemark.Text.Trim();
                    objDoc.Well = txtWell.Text.Trim();
                    objDoc.TransmittalNumber = txtTransmittalNumber.Text.Trim();

                    this.documentService.Update(objDoc);    
                }
            }
        }

        /// <summary>
        /// The btn download_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnDownload_Click(object sender, ImageClickEventArgs e)
        {
            var item = ((ImageButton)sender).Parent.Parent as GridDataItem;
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var docObj = this.documentNewService.GetById(docId);
            var docPackName = string.Empty;
            if (docObj != null)
            {
                docPackName = docObj.Name;
                var serverDocPackPath = Server.MapPath("~/Exports/DocPack/" + DateTime.Now.ToBinary() + "_" + docObj.Name + "_Pack.rar");

                var attachFiles = this.attachFileService.GetAllByDocId(docId);

                var temp = ZipPackage.CreateFile(serverDocPackPath);

                foreach (var attachFile in attachFiles)
                {
                    if (File.Exists(Server.MapPath(attachFile.FilePath)))
                    {
                        temp.Add(Server.MapPath(attachFile.FilePath));    
                    }
                }

                this.DownloadByWriteByte(serverDocPackPath, docPackName + ".rar", true);
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
    }
}