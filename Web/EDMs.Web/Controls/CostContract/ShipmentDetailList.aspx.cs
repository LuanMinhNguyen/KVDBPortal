// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Drawing;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.CostContract
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.ServiceProcess;
    using System.Text;
    using System.Collections;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.CostContract;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;


    /// <summary>
    /// Class customer
    /// </summary>
    public partial class ShipmentDetailList : Page
    {


        private readonly ProjectCodeService projectcodeService;

        private readonly EquipmentService equimentService;

        private readonly ShipmentService shipmentService;

        private readonly ShipmentDetailService shipmentdetailService;

        private readonly ShipmentDocumentFileService shipmentdocumentService;

        private readonly KKSIdentificationCodeService kkscodeService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ShipmentDetailList()
        {
            this.equimentService = new EquipmentService();
            this.shipmentService = new ShipmentService();
            this.userService = new UserService();
            this.projectcodeService = new ProjectCodeService();
            this.shipmentdetailService = new ShipmentDetailService();
            this.shipmentdocumentService = new ShipmentDocumentFileService();
            this.kkscodeService = new KKSIdentificationCodeService();
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

                ClearData(); LoadComboData();
                if ((!string.IsNullOrEmpty(this.Request.QueryString["Fromtodolist"]) && this.Request.QueryString["Fromtodolist"] == "true") || UserSession.Current.User.Role.IsInternal.GetValueOrDefault() || UserSession.Current.User.IsEngineer.GetValueOrDefault())
                {
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                    this.EditContent.Visible = false;
                    this.dIVUploadControl.Visible = false;
                    this.grdDocShipment.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                    this.grdDocShipment.MasterTableView.EditMode = GridEditMode.InPlace;

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
            if (!string.IsNullOrEmpty(Request.QueryString["shipmentId"]))
            {

                ShipmentDetail detailObj;
                if (Session["EditingId"] != null)
                {
                    var wfStepId = Convert.ToInt32(Session["EditingId"].ToString());

                    detailObj = this.shipmentdetailService.GetById(wfStepId);
                    if (detailObj != null)
                    {

                        this.CollectData(ref detailObj);

                        detailObj.UpdatedBy = UserSession.Current.User.Id;
                        detailObj.LastUpdatedByName = UserSession.Current.User.FullName;
                        detailObj.UpdatedDate = DateTime.Now;
                        this.shipmentdetailService.Update(detailObj);
                    }
                    Session.Remove("EditingId");
                }
                else
                {
                    detailObj = new ShipmentDetail()
                    {

                        CreatedBy = UserSession.Current.User.Id,
                        CreatedByName = UserSession.Current.User.FullName,
                        CreatedDate = DateTime.Now,
                    };

                    this.CollectData(ref detailObj);

                    this.shipmentdetailService.Insert(detailObj);

                }
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
            }
            this.ClearData();
            this.grdDocument.Rebind();
        }
        private void ClearData()
        {
            this.txtQuality.Value = 0;
            this.ddlEquipment.ClearSelection();
           // this.ddlEquipment.SelectedItem.Text = string.Empty;
            this.ddlKKS.ClearSelection();
            //this.ddlKKS.SelectedItem.Text = string.Empty;
            Session.Remove("EditingId");
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
        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditCmd")
            {
                var item = (GridDataItem)e.Item;

                var wfStepId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                var wfStepObj = this.shipmentdetailService.GetById(wfStepId);
                if (wfStepObj != null)
                {
                    Session.Add("EditingId", wfStepObj.ID);
                    LoadDocInfo(wfStepObj);
                }
            }
        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }
        protected void LoadDocuments(bool isbind = false, bool isListAll = false)
        {
            var changeRequestId = new Guid(this.Request.QueryString["shipmentId"]);
            var RFIlist = this.shipmentdetailService.GetByShipmentId(changeRequestId);

            this.grdDocument.DataSource = RFIlist.OrderBy(t => t.CreatedDate);
        }
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            this.LoadDocuments(false, isListAll);
        }
        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            //if (this.txtDocNo.Text.Trim().Length == 0)
            //{
            //    this.fileNameValidator.ErrorMessage = "Please enter Document Number.";
            //    this.divDocNo.Style["margin-bottom"] = "-26px;";
            //    args.IsValid = false;
            //}
            //else if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            //{
            //    Guid objId;
            //    Guid.TryParse(this.Request.QueryString["objId"].ToString(), out objId);

            //    if (this._PECC2DocumentService.IsExistByDocNo(this.txtDocNumber.Text.Trim()) && objId == null)
            //    {
            //        this.fileNameValidator.ErrorMessage = "Document No. is already exist.";
            //        this.divDocNo.Style["margin-bottom"] = "-5px;";
            //        args.IsValid = false;
            //    }
            //}
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("FileDelete"))
            {
                int docId = Convert.ToInt32(e.Argument.Split('_')[1]);
                var detailobj = this.shipmentdetailService.GetById(docId);
                if (detailobj != null)
                {
                    this.shipmentdetailService.Delete(detailobj);

                }
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("Deletedocument"))
            {
                var objId = Convert.ToInt32(e.Argument.Split('_')[1]);
                var obj = this.shipmentdocumentService.GetById(objId);
                if (!string.IsNullOrEmpty(obj?.FilePath))
                {
                    if (File.Exists(Server.MapPath( obj.FilePath)))
                    {
                        File.Delete(Server.MapPath(obj.FilePath));
                    }
                }

               this.shipmentdocumentService.Delete(obj);
                this.grdDocShipment.Rebind();
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {

            var kkscodelist = this.kkscodeService.GetAll().OrderBy(t => t.Code).ToList();
            kkscodelist.Insert(0, new KKSIdentificationCode() { ID = 0, Code = string.Empty });
            this.ddlKKS.DataSource = kkscodelist;
            this.ddlKKS.DataTextField = "Code";
            this.ddlKKS.DataValueField = "ID";
            this.ddlKKS.DataBind();
            var wfStepId = new Guid(this.Request.QueryString["shipmentId"]);

            var shipmentOBJ = this.shipmentService.GetById(wfStepId);
            this.txtNumber.Text = shipmentOBJ.Number;
            var equitListObj = this.equimentService.GetAllByProject(shipmentOBJ.ProjectID.GetValueOrDefault());
            var equitListID = equitListObj.Select(t => t.ParentId).Distinct().ToList();
            var equitBind = equitListObj.Where(t => !equitListID.Contains(t.ID)).ToList();
            equitBind.Insert(0, new Equipment() { ID = 0, Number = string.Empty });
            this.ddlEquipment.DataSource = equitBind;
            this.ddlEquipment.DataTextField = "Number";
            this.ddlEquipment.DataValueField = "ID";
            this.ddlEquipment.DataBind();


        }

        private void CollectData(ref ShipmentDetail obj)
        {

            var shipmentId = new Guid(this.Request.QueryString["shipmentId"]);
            var detailObj = this.shipmentService.GetById(shipmentId);
            obj.ShipmentID = detailObj.ID;
            obj.ShipmentNumber = detailObj.Number;
            obj.Quantity = (int)this.txtQuality.Value;
            obj.KKSId = Convert.ToInt32(this.ddlKKS.SelectedValue);
            obj.KKSCode = this.ddlKKS.SelectedItem.Text;
            obj.EquipmentID = this.ddlEquipment.SelectedValue != null ? Convert.ToInt32(this.ddlEquipment.SelectedValue) : 0;
            obj.EquipmentNumber = this.ddlEquipment.SelectedItem != null ? this.ddlEquipment.SelectedItem.Text : string.Empty;

        }

        private void LoadDocInfo(ShipmentDetail obj)
        {
            this.txtNumber.Text = obj.ShipmentNumber;

            this.ddlEquipment.SelectedValue = obj.EquipmentID.ToString();
            this.ddlKKS.SelectedValue = obj.KKSId.ToString();
            this.txtQuality.Value = obj.Quantity;

        }

        protected void grdDocShipment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var shipmentId = new Guid(this.Request.QueryString["shipmentId"]);
            var docshipment = this.shipmentdocumentService.GetOfShipment(shipmentId);
            this.grdDocShipment.DataSource = docshipment;

        }

        protected void grdDocShipment_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;

                if (command.Type== GridBatchEditingCommandType.Delete)
                {
                    var docID = Convert.ToInt32(newValues["ID"].ToString());
                    this.shipmentdocumentService.Delete(docID);
                }
                else
                {

                
               
                var docID = Convert.ToInt32(newValues["ID"].ToString());
                var docObj = this.shipmentdocumentService.GetById(docID);

                    if (docObj != null)
                    {
                        docObj.Description = newValues["Description"].ToString();
                        var type = newValues["TypeId"].ToString();
                        switch (type)
                        {
                            case "0":
                                docObj.TypeId = "0";
                                docObj.TypeName = "Introduction Letter";
                                break;
                            case "1":
                                docObj.TypeId = "1";
                                docObj.TypeName = "Authorization Letter";
                                break;
                            case "2":
                                docObj.TypeId = "2";
                                docObj.TypeName = "Attached cargo list";
                                break;
                            case "3":
                                docObj.TypeId = "3";
                                docObj.TypeName = "Invoice";
                                break;
                            case "4":
                                docObj.TypeId = "4";
                                docObj.TypeName = "Packing List/ General Packing List/ Detail Packing List";
                                break;
                            case "5":
                                docObj.TypeId = "5";
                                docObj.TypeName = "Bill of Lading";
                                break;
                            case "6":
                                docObj.TypeId = "6";
                                docObj.TypeName = "Certificate of Origin (CO)";
                                break;
                            case "7":
                                docObj.TypeId = "7";
                                docObj.TypeName = "Certificate of Quality (CQ)";
                                break;
                            case "8":
                                docObj.TypeId = "8";
                                docObj.TypeName = "Certificate of Insurance (optional)";
                                break;
                            case "9":
                                docObj.TypeId = "9";
                                docObj.TypeName = "Others";
                                break;

                        }
                        this.shipmentdocumentService.Update(docObj);
                    }
                }
            }
        }

        protected void btnUploadfile_Click(object sender, EventArgs e)
        {
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

                        var attachFile = new ShipmentDocumentFile()
                        {
                            ShipmentID = contractId,
                            FileName = docFileName,
                            Number = docFile.GetNameWithoutExtension(),
                            Extension = fileExt,
                            FilePath = serverFilePath,
                            ExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower()) ? fileIcon[fileExt.ToLower()] : "~/images/otherfile.png",
                            FileSize = (double)docFile.ContentLength / 1024,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedDate = DateTime.Now
                        };

                        this.shipmentdocumentService.Insert(attachFile);
                    }
                }
            }

            this.docuploader.UploadedFiles.Clear();

            this.grdDocShipment.Rebind();

        }

        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var contractId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            this.shipmentdetailService.Delete(contractId);
            this.grdDocument.Rebind();
        }

        protected void grdDocShipment_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var contractId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            this.shipmentdocumentService.Delete(contractId);
            this.grdDocShipment.Rebind();
        }

        protected void grdDocShipment_ItemDeleted(object sender, GridDeletedEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            var contractId = Convert.ToInt32(dataItem.GetDataKeyValue("ID").ToString());

            this.shipmentdocumentService.Delete(contractId);
            this.grdDocShipment.Rebind();
        }
    }


}
