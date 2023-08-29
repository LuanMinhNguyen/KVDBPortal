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
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.CostContract;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ShipmentAttachFile : Page
    {

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly ShipmentService shipmentService;

        private readonly AttachFilesShipmentService attachFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ShipmentAttachFile()
        {
            this.shipmentService = new  ShipmentService();
            this.attachFileService = new  AttachFilesShipmentService();
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


                if (UserSession.Current.User.IsEngineer.GetValueOrDefault() || UserSession.Current.User.IsLeader.GetValueOrDefault())
                {
                    this.btnSave.Visible = false;
                    this.UploadControl.Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }
                    //if (this.Request.QueryString["isFullPermission"] != "true")
                    //{
                    //    this.btnSave.Visible = false;
                    //    this.UploadControl.Visible = false;
                    //    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                    //}
                    //if (!string.IsNullOrEmpty(this.Request.QueryString["contractId"]))
                    //{
                    //    var objDoc = this.prService.GetById(Convert.ToInt32(this.Request.QueryString["contractId"]));
                    //    if (objDoc != null)
                    //    {

                    //    }
                    //}
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
            if (!string.IsNullOrEmpty(Request.QueryString["ShipmentId"]))
            {
                var contractId = new Guid(this.Request.QueryString["ShipmentId"]);
                var flag = false;
                const string targetFolder = "../../DocumentLibrary/Shipment";
                var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/Shipment";
                var listUpload = docuploader.UploadedFiles;

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

                if (listUpload.Count > 0)
                {
                    foreach (UploadedFile docFile in listUpload)
                    {
                        var docFileName = docFile.FileName;

                        var serverDocFileName = DateTime.Now.ToBinary() + "_" + docFileName;

                        // Path file to save on server disc
                        var saveFilePath = Path.Combine(Server.MapPath(targetFolder), serverDocFileName);

                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + serverDocFileName;
                        var fileExt = docFileName.Substring(docFileName.LastIndexOf(".") + 1, docFileName.Length - docFileName.LastIndexOf(".") - 1);

                        docFile.SaveAs(saveFilePath, true);

                        var attachFile = new AttachFilesShipment()
                            {
                                ShipmentId = contractId,
                                FileName = docFileName,
                                Extension = fileExt,
                                FilePath = serverFilePath,
                                //FilePathEncrypt = CryptorEngine.Encrypt(serverFilePath, true),
                                ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                                FileSize = (double)docFile.ContentLength / 1024,
                                CreatedBy = UserSession.Current.User.Id,
                                CreatedDate = DateTime.Now
                            };

                        this.attachFileService.Insert(attachFile);
                    }
                }
            }

            this.docuploader.UploadedFiles.Clear();

            this.grdDocument.Rebind();
        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var contractId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            this.attachFileService.Delete(contractId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ShipmentId"]))
            {
                var contractId = new Guid(Request.QueryString["ShipmentId"]);
                var attachList = this.attachFileService.GetAllByShipment(contractId);

                this.grdDocument.DataSource = attachList;
            }
            else
            {
                this.grdDocument.DataSource = new List<AttachFilesPackage>();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void setGrdRadioButtonOnClick()
        {
            int i;
            RadioButton radioButton;
            for (i = 0; i < grdDocument.Items.Count; i++)
            {

                radioButton = (RadioButton)grdDocument.Items[i].FindControl("rdSelect");

                radioButton.Attributes.Add("OnClick", "SelectMeOnly(" + radioButton.ClientID + ", " + "'grdDocument'" + ")");
            }
        }

        protected void rbtnDefaultDoc_CheckedChanged(object sender, EventArgs e)
        {
            ((GridItem)((RadioButton)sender).Parent.Parent).Selected = ((RadioButton)sender).Checked;

            var item = ((RadioButton)sender).Parent.Parent as GridDataItem;
            var attachFileId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var attachFileObj = this.attachFileService.GetById(attachFileId);
            if (attachFileObj != null)
            {
                var attachFiles = this.attachFileService.GetAllByShipment(attachFileObj.ShipmentId.GetValueOrDefault());
                foreach (var attachFile in attachFiles)
                {
                    attachFile.IsDefault = attachFile.ID == attachFileId;
                    this.attachFileService.Update(attachFile);
                }
            }
        }
    }
}