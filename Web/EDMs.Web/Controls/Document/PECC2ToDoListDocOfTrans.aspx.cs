// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using EDMs.Data.Entities;
using Telerik.Web.UI;
using System.Configuration;
using Telerik.Windows.Zip;
using System;
 using System.Web;
using System.Data;
using System.Drawing;
using System.Web.Hosting;
using System.Web.UI;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Workflow;
using EDMs.Web.Utilities;
using EDMs.Web.Utilities.Sessions;
using iTextSharp.text.pdf;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class PECC2ToDoListDocOfTrans : Page
    {
        private readonly ContractorTransmittalService contractorTransmittalService;

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService;

        private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService;

        private readonly PECC2TransmittalService pecc2TransmittalService;


        private readonly PECC2TransmittalAttachFileService pecc2TransmittalAttachFileService;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly PECC2DocumentsService pecc2DocumentsService;

        private readonly PECC2DocumentAttachFileService pecc2DocumentAttachFileService;

        private readonly ObjectAssignedUserService todolistService;

        private readonly ProjectCodeService projectCodeService;

        private readonly ChangeRequestService changeRequestService;

        private readonly ChangeRequestAttachFileService changeRequestAttachFileService;

        private readonly DocumentCodeServices documnetCodeSErvie;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public PECC2ToDoListDocOfTrans()
        {
            this.contractorTransmittalService = new ContractorTransmittalService();
            this.contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();
            this.pecc2TransmittalService = new PECC2TransmittalService();
            this.contractorTransmittalAttachFileService = new ContractorTransmittalAttachFileService();
            this.pecc2TransmittalAttachFileService = new PECC2TransmittalAttachFileService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.pecc2DocumentsService = new PECC2DocumentsService();
            this.pecc2DocumentAttachFileService = new PECC2DocumentAttachFileService();
            this.projectCodeService = new ProjectCodeService();
            this.todolistService = new ObjectAssignedUserService();
            this.changeRequestService = new ChangeRequestService();
            this.changeRequestAttachFileService = new ChangeRequestAttachFileService();
            this.documnetCodeSErvie = new DocumentCodeServices();
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
                this.LbcurrentAssignId.Value = this.Request.QueryString["currentAssignId"];
                this.ServerName.Value = ConfigurationManager.AppSettings.Get("ServerName");
                var todolistObj = this.todolistService.GetById(new Guid(this.Request.QueryString["currentAssignId"]));
                this.DivCRS.Visible = this.Request.QueryString["actionType"] == "2" && !todolistObj.IsCanCreateOutgoingTrans.GetValueOrDefault() ? false: true;
                this.LbIsCancreateOutTrans.Value = todolistObj.IsCanCreateOutgoingTrans.GetValueOrDefault().ToString();
                //this.lblProjectIncomingId.Value = this.Request.QueryString["projId"];
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var transId = new Guid(this.Request.QueryString["objId"]);
                    var pecc2TransObj = this.pecc2TransmittalService.GetById(transId);
                    if (pecc2TransObj != null)
                    {
                        //pecc2TransObj.IsOpen = true;
                        //this.pecc2TransmittalService.Update(pecc2TransObj);
                        this.lbStorePath.Value = pecc2TransObj.StoreFolderPathContractor;
                    }

                    var crsList =
                 this.pecc2TransmittalAttachFileService.GetByTrans(transId)
                     .FirstOrDefault(t => t.TypeId == 2);
                     
                    if (crsList != null)
                    {
                        this.IsHasCRSFile.Value = "True";
                    }
                    else
                    {
                        this.IsHasCRSFile.Value = "False";
                    }
                }
               var documentcodelist= this.documnetCodeSErvie.GetAllReviewStatus().ToList();
                documentcodelist.Insert(0, new DocumentCode() { ID = 0, Code = string.Empty });
                this.ddlDocReviewStatus.DataSource = documentcodelist.Where(t => !t.FullName.Contains("ReFC")); ;
                this.ddlDocReviewStatus.DataTextField = "FullName";
                this.ddlDocReviewStatus.DataValueField = "ID";
                this.ddlDocReviewStatus.DataBind();
               // this.ddlDocreviewStatus2.Visible = false;
                this.ddlDocreviewStatus2.DataSource = documentcodelist.Where(t => t.ID == 0 || t.FullName.Contains("ReFC"));
                this.ddlDocreviewStatus2.DataTextField = "FullName";
                this.ddlDocreviewStatus2.DataValueField = "ID";
                this.ddlDocreviewStatus2.DataBind();
            }
        }

        protected void grdDocumentFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var pecc2TransId = new Guid(this.Request.QueryString["objId"]);
                var pecc2TransObj = this.pecc2TransmittalService.GetById(pecc2TransId);
                if (pecc2TransObj != null)
                {
                    //var contractorTransObj = this.contractorTransmittalService.GetById(pecc2TransObj.ContractorTransId.GetValueOrDefault());
                    //var docFileList = this.contractorTransmittalDocFileService.GetAllByTrans(contractorTransObj.ID).OrderBy(t => t.FileName);
                    //this.grdDocumentFile.DataSource = docFileList;

                    var pecc2ListDoc = this.pecc2DocumentsService.GetAllByIncomingTrans(pecc2TransObj.ID).OrderBy(t => t.DocNo).ToList();
                    if (pecc2TransObj.ForSentId == 2)
                    {
                        var contractorTransObj = this.contractorTransmittalService.GetById(pecc2TransObj.ContractorTransId.GetValueOrDefault());
                        var changeRequestDocFile = this.contractorTransmittalDocFileService.GetAllByTrans(contractorTransObj.ID).FirstOrDefault(t => t.TypeId == 2);
                        if (changeRequestDocFile != null)
                        {
                            var changeRequestObj =
                                this.changeRequestService.GetById(changeRequestDocFile.PECC2ProjectDocId
                                    .GetValueOrDefault());
                            if (changeRequestObj != null)
                            {
                                pecc2ListDoc.Insert(0, new PECC2Documents()
                                {
                                    ID = changeRequestObj.ID,
                                    IsChangeRequest = true,
                                    DocNo = changeRequestObj.Number,
                                    DocActionCode = changeRequestObj.ActionCodeName,
                                    Revision = changeRequestObj.Revision,
                                    DocReviewStatusCode = changeRequestObj.ReviewResultName,
                                    DocTitle = changeRequestDocFile.DocumentTitle,
                                    IncomingTransNo = changeRequestObj.IncomingTransNo,
                                });
                            }
                        }
                    }
                    this.grdDocumentFile.DataSource = pecc2ListDoc;
                }
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
            else if (e.Argument == "DownloadMulti")
            {
                try
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                    var pecc2TransObj = this.pecc2TransmittalService.GetById(objId);
                    var filename = "Transmittal_" + pecc2TransObj.TransmittalNo + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".rar";
                    if (pecc2TransObj != null)
                    {
                        var serverTotalDocPackPath = Server.MapPath("~/Exports/DocPack/" + filename);
                        var docPack = ZipPackage.CreateFile(serverTotalDocPackPath);
                        var attachFiles = this.contractorTransmittalDocFileService.GetAllByTrans(pecc2TransObj.ContractorTransId.GetValueOrDefault());

                        foreach (var attachFile in attachFiles)
                        {
                            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                            {
                                docPack.Add(Server.MapPath(attachFile.FilePath));
                            }
                        }

                        var attachFilesCRS = this.contractorTransmittalAttachFileService.GetByTrans(pecc2TransObj.ContractorTransId.GetValueOrDefault());
                        foreach (var attachFile in attachFilesCRS)
                        {
                            if (File.Exists(Server.MapPath(attachFile.FilePath)))
                            {
                                docPack.Add(Server.MapPath(attachFile.FilePath));
                            }
                        }

                        // this.DownloadByWriteByte(serverTotalDocPackPath, "Transmittal_" + pecc2TransObj.TransmittalNo + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".rar", true);
                       
                        this.Download_File(serverTotalDocPackPath);
                    }
                }
                catch (Exception ex)
                {  }
            }
            else if(e.Argument== "GenerateCRSFile")
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var pecc2TransObj = this.pecc2TransmittalService.GetById(objId);
                var pecc2ListDoc = this.pecc2DocumentsService.GetAllByIncomingTrans(pecc2TransObj.ID).Where(t=> t.DocReviewStatusId==null || t.DocReviewStatusId==0).ToList();
                if (pecc2ListDoc.Count() > 0)
                {
                    var st = string.Empty;
                    foreach(var item in pecc2ListDoc)
                    {
                        st += item.DocNo + ";" + Environment.NewLine;
                    }
                    this.RadWindowManager1.RadAlert("Please review and apply code for documents:"+ Environment.NewLine+ st, 300, 100, "Warring", "");

                }
                else
                { // this.btnExportCRSk();
                this.GenerateCRS();
                this.grdCRSFilePECC2.Rebind();

                }
               
            }
            else if(e.Argument.Contains("Checkout"))
            {
                var crsFileId = new Guid(e.Argument.Split('_')[1]);
                var crsFile = this.pecc2TransmittalAttachFileService.GetById(crsFileId);
                if (crsFile != null && !crsFile.IsCheckOut.GetValueOrDefault())
                {
                    crsFile.IsCheckOut = true;
                    crsFile.CheckoutBy = UserSession.Current.User.Id;
                    crsFile.CheckoutByName = UserSession.Current.User.FullName;
                    crsFile.CheckinDate = DateTime.Now;
                    this.pecc2TransmittalAttachFileService.Update(crsFile);
                }

                this.grdCRSFilePECC2.Rebind();
            }
        }
        private void Download_File(string FilePath)
        {
            try
            {
                Response.Clear();
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
                Response.WriteFile(FilePath);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
               // Response.End();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                File.SetAttributes(FilePath, FileAttributes.Normal);
                System.IO.File.Delete(FilePath);
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

                if (Response.IsClientConnected)
                {
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    System.IO.File.Delete(strFileName);
                }
            }
            return true;
        }
        protected void grdAttachCRSFile_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objId = new Guid(this.Request.QueryString["objId"]);
            var pecc2TransObj = this.pecc2TransmittalService.GetById(objId);
            if (pecc2TransObj != null)
            {
                var attachList =
                    this.contractorTransmittalAttachFileService.GetByTrans(
                        pecc2TransObj.ContractorTransId.GetValueOrDefault());
                this.grdAttachCRSFile.DataSource = attachList.Where(t=> t.TypeId==2);
            }
            else
            {
                this.grdAttachCRSFile.DataSource = new List<ContractorTransmittalAttachFile>();
            }
            
        }

        protected void grdCRSFilePECC2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var transId = new Guid(this.Request.QueryString["objId"]);
                var crsList = this.pecc2TransmittalAttachFileService.GetByTrans(transId)
                                    .Where(t => t.TypeId == 2)
                                    .OrderBy(t => t.CreatedDate);
                foreach (var pecc2TransmittalAttachFile in crsList)
                {
                    pecc2TransmittalAttachFile.IsCanCheckin = UserSession.Current.User.Id == pecc2TransmittalAttachFile.CheckoutBy;
                }

                this.grdCRSFilePECC2.DataSource = crsList;
            }
            else
            {
                this.grdCRSFilePECC2.DataSource = new List<PECC2TransmittalAttachFileService>();
            }
        }

        private void GenerateCRS()
        {
            var transObj = this.pecc2TransmittalService.GetById(new Guid(Request.QueryString["objId"]));
            if (transObj != null)
            {
                var docOfTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID)
                    .Select(t => this.pecc2DocumentsService.GetById(t.DocumentId.GetValueOrDefault()))
                    .Where(t => t != null)
                    .OrderBy(t => t.DocNo)
                    .ToList();

                var crsFile = this.pecc2TransmittalAttachFileService.GetByTrans(transObj.ID).FirstOrDefault(t => t.TypeId == 2);
                if (crsFile != null)
                {
                    var workbook = new Workbook();
                    workbook = new Workbook(Server.MapPath("../.." + crsFile.FilePath));

                    var dataControlSheet = workbook.Worksheets[0];
                    var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value) - 1;
                    var currentDocListCount = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);
                    var transSheet = workbook.Worksheets[currentSheetIndex];

                    if (currentSheetIndex != 1)
                    {
                        transSheet.IsVisible = true;
                    }

                    // Update new current sheet
                    //dataControlSheet.Cells["A1"].Value = currentSheetIndex + 1;
                    // -------------------------------------------------------------------------------------------------------

                    if (transObj.IsFirstTrans.GetValueOrDefault())
                    {
                        transSheet.Cells["E2"].PutValue(transObj.TransmittalNo);
                        transSheet.Cells["J2"].PutValue(transObj.IssuedDate);
                        transSheet.Cells["E3"].PutValue(transObj.Description);
                    }
                    else
                    {
                        transSheet.Cells[1, 4 + 7 * (currentSheetIndex - 1)].PutValue(transObj.TransmittalNo);
                        transSheet.Cells[1, 9 + 7 * (currentSheetIndex - 1)].PutValue(transObj.IssuedDate);
                        transSheet.Cells[2, 4 + 7 * (currentSheetIndex - 1)].PutValue(transObj.Description);
                    }

                    var dtDocListFull = new DataTable();
                    dtDocListFull.Columns.AddRange(new[]
                    {
                    new DataColumn("DocNo", typeof(String)),
                    new DataColumn("Empty1", typeof(String)),
                    new DataColumn("Rev1", typeof(String)),
                    new DataColumn("ReviewStatus1", typeof(String)),
                    new DataColumn("Empty2", typeof(String)),
                    new DataColumn("Title1", typeof(String)),
                    new DataColumn("Empty3", typeof(String)),
                    new DataColumn("Empty4", typeof(String)),
                    new DataColumn("Empty35", typeof(String)),

                    new DataColumn("Rev2", typeof(String)),
                    new DataColumn("ReviewStatus2", typeof(String)),
                    new DataColumn("Empty5", typeof(String)),
                    new DataColumn("Title2", typeof(String)),
                    new DataColumn("Empty6", typeof(String)),
                    new DataColumn("Empty7", typeof(String)),
                    new DataColumn("Empty36", typeof(String)),

                    new DataColumn("Rev3", typeof(String)),
                    new DataColumn("ReviewStatus3", typeof(String)),
                    new DataColumn("Empty8", typeof(String)),
                    new DataColumn("Title3", typeof(String)),
                    new DataColumn("Empty9", typeof(String)),
                    new DataColumn("Empty10", typeof(String)),
                    new DataColumn("Empty37", typeof(String)),

                    new DataColumn("Rev4", typeof(String)),
                    new DataColumn("ReviewStatus4", typeof(String)),
                    new DataColumn("Empty11", typeof(String)),
                    new DataColumn("Title4", typeof(String)),
                    new DataColumn("Empty13", typeof(String)),
                    new DataColumn("Empty14", typeof(String)),
                    new DataColumn("Empty38", typeof(String)),

                    new DataColumn("Rev5", typeof(String)),
                    new DataColumn("ReviewStatus5", typeof(String)),
                    new DataColumn("Empty15", typeof(String)),
                    new DataColumn("Title5", typeof(String)),
                    new DataColumn("Empty17", typeof(String)),
                    new DataColumn("Empty18", typeof(String)),
                    new DataColumn("Empty39", typeof(String)),

                    new DataColumn("Rev6", typeof(String)),
                    new DataColumn("ReviewStatus6", typeof(String)),
                    new DataColumn("Empty19", typeof(String)),
                    new DataColumn("Title6", typeof(String)),
                    new DataColumn("Empty21", typeof(String)),
                    new DataColumn("Empty22", typeof(String)),
                    new DataColumn("Empty40", typeof(String)),

                    new DataColumn("Rev7", typeof(String)),
                    new DataColumn("ReviewStatus7", typeof(String)),
                    new DataColumn("Empty23", typeof(String)),
                    new DataColumn("Title7", typeof(String)),
                    new DataColumn("Empty24", typeof(String)),
                    new DataColumn("Empty25", typeof(String)),
                    new DataColumn("Empty41", typeof(String)),

                    new DataColumn("Rev8", typeof(String)),
                    new DataColumn("ReviewStatus8", typeof(String)),
                    new DataColumn("Empty26", typeof(String)),
                    new DataColumn("Title8", typeof(String)),
                    new DataColumn("Empty27", typeof(String)),
                    new DataColumn("Empty28", typeof(String)),
                    new DataColumn("Empty42", typeof(String)),

                    new DataColumn("Rev9", typeof(String)),
                    new DataColumn("ReviewStatus9", typeof(String)),
                    new DataColumn("Empty29", typeof(String)),
                    new DataColumn("Title9", typeof(String)),
                    new DataColumn("Empty30", typeof(String)),
                    new DataColumn("Empty31", typeof(String)),
                    new DataColumn("Empty43", typeof(String)),

                    new DataColumn("Rev10", typeof(String)),
                    new DataColumn("ReviewStatus10", typeof(String)),
                    new DataColumn("Empty32", typeof(String)),
                    new DataColumn("Title10", typeof(String)),
                    new DataColumn("Empty33", typeof(String)),
                    new DataColumn("Empty34", typeof(String)),
                    new DataColumn("Empty44", typeof(String)),
                });

                    var dtCommentList = new DataTable();
                    dtCommentList.Columns.AddRange(new[]
                    {
                    new DataColumn("Index", typeof(String)),
                    new DataColumn("DocNo", typeof(String)),

                    new DataColumn("Comment1", typeof(String)),
                    new DataColumn("Empty1", typeof(String)),
                    new DataColumn("Empty2", typeof(String)),
                    new DataColumn("Empty3", typeof(String)),
                    new DataColumn("Response1", typeof(String)),
                    new DataColumn("Empty4", typeof(String)),
                    new DataColumn("Empty5", typeof(String)),

                    new DataColumn("Comment2", typeof(String)),
                    new DataColumn("Empty6", typeof(String)),
                    new DataColumn("Empty7", typeof(String)),
                    new DataColumn("Empty8", typeof(String)),
                    new DataColumn("Response2", typeof(String)),
                    new DataColumn("Empty9", typeof(String)),
                    new DataColumn("Empty10", typeof(String)),

                    new DataColumn("Comment3", typeof(String)),
                    new DataColumn("Empty11", typeof(String)),
                    new DataColumn("Empty12", typeof(String)),
                    new DataColumn("Empty13", typeof(String)),
                    new DataColumn("Response3", typeof(String)),
                    new DataColumn("Empty14", typeof(String)),
                    new DataColumn("Empty15", typeof(String)),

                    new DataColumn("Comment4", typeof(String)),
                    new DataColumn("Empty16", typeof(String)),
                    new DataColumn("Empty17", typeof(String)),
                    new DataColumn("Empty18", typeof(String)),
                    new DataColumn("Response4", typeof(String)),
                    new DataColumn("Empty19", typeof(String)),
                    new DataColumn("Empty20", typeof(String)),

                    new DataColumn("Comment5", typeof(String)),
                    new DataColumn("Empty21", typeof(String)),
                    new DataColumn("Empty22", typeof(String)),
                    new DataColumn("Empty23", typeof(String)),
                    new DataColumn("Response5", typeof(String)),
                    new DataColumn("Empty24", typeof(String)),
                    new DataColumn("Empty25", typeof(String)),

                    new DataColumn("Comment6", typeof(String)),
                    new DataColumn("Empty26", typeof(String)),
                    new DataColumn("Empty27", typeof(String)),
                    new DataColumn("Empty28", typeof(String)),
                    new DataColumn("Response6", typeof(String)),
                    new DataColumn("Empty29", typeof(String)),
                    new DataColumn("Empty30", typeof(String)),

                    new DataColumn("Comment7", typeof(String)),
                    new DataColumn("Empty31", typeof(String)),
                    new DataColumn("Empty32", typeof(String)),
                    new DataColumn("Empty33", typeof(String)),
                    new DataColumn("Response7", typeof(String)),
                    new DataColumn("Empty34", typeof(String)),
                    new DataColumn("Empty35", typeof(String)),

                    new DataColumn("Comment8", typeof(String)),
                    new DataColumn("Empty36", typeof(String)),
                    new DataColumn("Empty37", typeof(String)),
                    new DataColumn("Empty38", typeof(String)),
                    new DataColumn("Response8", typeof(String)),
                    new DataColumn("Empty39", typeof(String)),
                    new DataColumn("Empty40", typeof(String)),

                    new DataColumn("Comment9", typeof(String)),
                    new DataColumn("Empty41", typeof(String)),
                    new DataColumn("Empty42", typeof(String)),
                    new DataColumn("Empty43", typeof(String)),
                    new DataColumn("Response9", typeof(String)),
                    new DataColumn("Empty44", typeof(String)),
                    new DataColumn("Empty45", typeof(String)),

                    new DataColumn("Comment10", typeof(String)),
                    new DataColumn("Empty46", typeof(String)),
                    new DataColumn("Empty47", typeof(String)),
                    new DataColumn("Empty48", typeof(String)),
                    new DataColumn("Response10", typeof(String)),
                    new DataColumn("Empty49", typeof(String)),
                    new DataColumn("Empty50", typeof(String)),
                });

                    var dtGeneralCommentList = new DataTable();
                    dtGeneralCommentList.Columns.AddRange(new[]
                    {
                    new DataColumn("Index", typeof(String)),
                    new DataColumn("DocNo", typeof(String)),

                    new DataColumn("Comment1", typeof(String)),
                    new DataColumn("Empty1", typeof(String)),
                    new DataColumn("Empty2", typeof(String)),
                    new DataColumn("Empty3", typeof(String)),
                    new DataColumn("Response1", typeof(String)),
                    new DataColumn("Empty4", typeof(String)),
                    new DataColumn("Empty5", typeof(String)),

                    new DataColumn("Comment2", typeof(String)),
                    new DataColumn("Empty6", typeof(String)),
                    new DataColumn("Empty7", typeof(String)),
                    new DataColumn("Empty8", typeof(String)),
                    new DataColumn("Response2", typeof(String)),
                    new DataColumn("Empty9", typeof(String)),
                    new DataColumn("Empty10", typeof(String)),

                    new DataColumn("Comment3", typeof(String)),
                    new DataColumn("Empty11", typeof(String)),
                    new DataColumn("Empty12", typeof(String)),
                    new DataColumn("Empty13", typeof(String)),
                    new DataColumn("Response3", typeof(String)),
                    new DataColumn("Empty14", typeof(String)),
                    new DataColumn("Empty15", typeof(String)),

                    new DataColumn("Comment4", typeof(String)),
                    new DataColumn("Empty16", typeof(String)),
                    new DataColumn("Empty17", typeof(String)),
                    new DataColumn("Empty18", typeof(String)),
                    new DataColumn("Response4", typeof(String)),
                    new DataColumn("Empty19", typeof(String)),
                    new DataColumn("Empty20", typeof(String)),

                    new DataColumn("Comment5", typeof(String)),
                    new DataColumn("Empty21", typeof(String)),
                    new DataColumn("Empty22", typeof(String)),
                    new DataColumn("Empty23", typeof(String)),
                    new DataColumn("Response5", typeof(String)),
                    new DataColumn("Empty24", typeof(String)),
                    new DataColumn("Empty25", typeof(String)),

                    new DataColumn("Comment6", typeof(String)),
                    new DataColumn("Empty26", typeof(String)),
                    new DataColumn("Empty27", typeof(String)),
                    new DataColumn("Empty28", typeof(String)),
                    new DataColumn("Response6", typeof(String)),
                    new DataColumn("Empty29", typeof(String)),
                    new DataColumn("Empty30", typeof(String)),

                    new DataColumn("Comment7", typeof(String)),
                    new DataColumn("Empty31", typeof(String)),
                    new DataColumn("Empty32", typeof(String)),
                    new DataColumn("Empty33", typeof(String)),
                    new DataColumn("Response7", typeof(String)),
                    new DataColumn("Empty34", typeof(String)),
                    new DataColumn("Empty35", typeof(String)),

                    new DataColumn("Comment8", typeof(String)),
                    new DataColumn("Empty36", typeof(String)),
                    new DataColumn("Empty37", typeof(String)),
                    new DataColumn("Empty38", typeof(String)),
                    new DataColumn("Response8", typeof(String)),
                    new DataColumn("Empty39", typeof(String)),
                    new DataColumn("Empty40", typeof(String)),

                    new DataColumn("Comment9", typeof(String)),
                    new DataColumn("Empty41", typeof(String)),
                    new DataColumn("Empty42", typeof(String)),
                    new DataColumn("Empty43", typeof(String)),
                    new DataColumn("Response9", typeof(String)),
                    new DataColumn("Empty44", typeof(String)),
                    new DataColumn("Empty45", typeof(String)),

                    new DataColumn("Comment10", typeof(String)),
                    new DataColumn("Empty46", typeof(String)),
                    new DataColumn("Empty47", typeof(String)),
                    new DataColumn("Empty48", typeof(String)),
                    new DataColumn("Response10", typeof(String)),
                    new DataColumn("Empty49", typeof(String)),
                    new DataColumn("Empty50", typeof(String)),
                });

                    var prevDocCount = 0;
                    var prevGeneralCommentCount = dataControlSheet.Cells["A4"].Value.ToString() == "#VALUE!" || dataControlSheet.Cells["A4"].Value.ToString() == "#REF!" ? 0 : Convert.ToInt32(dataControlSheet.Cells["A4"].Value);
                    var prevCommentCount = dataControlSheet.Cells["A3"].Value.ToString() == "#VALUE!" ? 0 : Convert.ToInt32(dataControlSheet.Cells["A3"].Value);


                    if (transObj.IsFirstTrans.GetValueOrDefault() && prevCommentCount == 0)
                    {
                        prevDocCount = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);
                        var countComment = prevCommentCount + 1;

                        // Get info of change request doc first
                        if (transObj.ForSentId == 2)
                        {
                            var changeRequestObj = this.changeRequestService.GetAllByIncomingTrans(transObj.ID).FirstOrDefault();
                            if (changeRequestObj != null)
                            {
                                //var docRow = dtDocListFull.NewRow();
                                //docRow["DocNo"] = changeRequestObj.Number;
                                //docRow["Rev" + currentSheetIndex] = changeRequestObj.Revision;
                                //docRow["ReviewStatus" + currentSheetIndex] = changeRequestObj.ReviewResultName;
                                //docRow["Title" + currentSheetIndex] = changeRequestObj.Title;
                                //dtDocListFull.Rows.Add(docRow);

                                // Update review status for doclist
                                for (int i = 0; i < prevDocCount; i++)
                                {
                                    if (changeRequestObj.Number == transSheet.Cells[5 + i, 2].Value.ToString())
                                    {
                                        transSheet.Cells[5 + i, 5].PutValue(changeRequestObj.ReviewResultName);
                                    }
                                }
                                // --------------------------------------------------------------------------

                                var commentList = this.GetCommentInfoChangeRequest(changeRequestObj).OrderBy(t => t).ToList();
                                if (commentList.Count != 0)
                                {
                                    for (var i = 0; i < commentList.Count; i++)
                                    {
                                        var page = commentList[i].Split('$')[1];
                                        var commentIndex = commentList[i].Split('$')[0].Split('.')[0];
                                        var commentRow = dtCommentList.NewRow();
                                        commentRow["Index"] = countComment;
                                        commentRow["DocNo"] = changeRequestObj.Number + " / " + page + " / " + commentIndex;
                                        commentRow["Comment" + currentSheetIndex] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                        dtCommentList.Rows.Add(commentRow);
                                        countComment += 1;
                                    }
                                }
                            }
                        }
                        // ------------------------------------------------------------------------------------------------------------


                        foreach (var docObj in docOfTrans)
                        {
                            //var docRow = dtDocListFull.NewRow();
                            //docRow["DocNo"] = docObj.DocNo;
                            //docRow["Rev" + currentSheetIndex] = docObj.Revision;
                            //docRow["ReviewStatus" + currentSheetIndex] = docObj.DocReviewStatusCode;
                            //docRow["Title" + currentSheetIndex] = docObj.DocTitle;
                            //dtDocListFull.Rows.Add(docRow);

                            var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                            if (commentList.Count != 0)
                            {
                                for (var i = 0; i < commentList.Count; i++)
                                {
                                    var page = commentList[i].Split('$')[1];
                                    var commentIndex = commentList[i].Split('$')[0].Split('.')[0];
                                    var commentRow = dtCommentList.NewRow();
                                    commentRow["Index"] = countComment;
                                    commentRow["DocNo"] = docObj.DocNo + " / PAGE " + page + " / " + commentIndex;
                                    commentRow["Comment" + currentSheetIndex] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                    dtCommentList.Rows.Add(commentRow);
                                    countComment += 1;
                                }
                            }
                           
                        }
                        if (dtCommentList.Rows.Count == 0)
                        {
                            var commentRow = dtCommentList.NewRow();
                            commentRow["Index"] = 1;
                            dtCommentList.Rows.Add(commentRow);
                        }
                        // Update review status for doclist
                        for (int i = 0; i < prevDocCount; i++)
                        {
                            var docObj = docOfTrans.FirstOrDefault(t => t.DocNo == transSheet.Cells[5 + i, 2].Value.ToString());
                            if (docObj != null)
                            {
                                transSheet.Cells[5 + i, 5].PutValue(docObj.DocReviewStatusCode);
                                transSheet.Cells[5 + i, 6].PutValue(docObj.DocReviewStatusCode2);
                            }
                        }
                        // --------------------------------------------------------------------------

                        if (dtCommentList.Rows.Count > 0)
                        {
                            dtCommentList = dtCommentList.AsEnumerable().OrderBy(t => t["DocNo"]).CopyToDataTable();
                        }

                        for (int i = 0; i < dtCommentList.Rows.Count; i++)
                        {
                            dtCommentList.Rows[i]["Index"] = i + 1;
                        }

                        transSheet.Cells.ImportDataTable(dtCommentList, false, 9 + currentDocListCount + 2 + prevGeneralCommentCount + prevCommentCount, 2, dtCommentList.Rows.Count, dtCommentList.Columns.Count, false);

                        for (int i = 0; i <= dtCommentList.Rows.Count + prevCommentCount - 1; i++)
                        {
                            for (int j = 0; j < currentSheetIndex; j++)
                            {
                                transSheet.Cells.Merge(9 + i + currentDocListCount + 2 + prevGeneralCommentCount, 4 + (7 * j), 1, 4);
                                transSheet.Cells.Merge(9 + i + currentDocListCount + 2 + prevGeneralCommentCount, 8 + (7 * j), 1, 2);

                            }
                        }
                        // Apply border 
                        var rb = transSheet.Cells.CreateRange(9 + currentDocListCount + 2 + prevGeneralCommentCount + prevCommentCount, 2, dtCommentList.Rows.Count + prevCommentCount, 2 + 6 * currentSheetIndex + currentSheetIndex - 1);
                        var style = workbook.CreateStyle();
                        style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.LeftBorder].Color = Color.Black;
                        style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.RightBorder].Color = Color.Black;
                        style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.TopBorder].Color = Color.Black;
                        style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.BottomBorder].Color = Color.Black;
                        var flag = new StyleFlag();

                        flag.Borders = true;
                        rb.ApplyStyle(style, flag);
                    }
                    else
                    {
                        prevDocCount = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);
                        prevCommentCount = Convert.ToInt32(dataControlSheet.Cells["A3"].Value);
                        prevGeneralCommentCount = Convert.ToInt32(dataControlSheet.Cells["A4"].Value);
                        var prevTransSheet = workbook.Worksheets[currentSheetIndex];
                        //var prevDtDocList = prevTransSheet.Cells.ExportDataTable(5, 2, prevDocCount, 2 + 7 * currentSheetIndex).AsEnumerable().ToList();
                        //var prevGeneralDtCommentList = prevTransSheet.Cells.ExportDataTable(9 + prevDocCount, 2, prevGeneralCommentCount, 2 + 7 * currentSheetIndex).AsEnumerable().ToList();
                        if (prevCommentCount != 0)
                        {
                            var prevDtCommentList = prevTransSheet.Cells.ExportDataTable(9 + prevDocCount + 3 + prevGeneralCommentCount, 2, prevCommentCount, 2 + 7 * currentSheetIndex).AsEnumerable().ToList();
                            foreach (DataRow commentItem in prevDtCommentList)
                            {
                                var commentRow = dtCommentList.NewRow();
                                commentRow["Index"] = commentItem[0];
                                commentRow["DocNo"] = commentItem[1];
                                for (int i = 0; i < currentSheetIndex; i++)
                                {
                                    commentRow["Comment" + (i + 1)] = commentItem[2 + (7 * i)];
                                    commentRow["Response" + (i + 1)] = commentItem[6 + (7 * i)];
                                }
                                dtCommentList.Rows.Add(commentRow);
                            }
                        }

                        //foreach (DataRow docItem in prevDtDocList)
                        //{
                        //    var docRow = dtDocListFull.NewRow();
                        //    docRow["DocNo"] = docItem[0];
                        //    for (int i = 0; i < currentSheetIndex - 1; i++)
                        //    {
                        //        docRow["Rev" + (i + 1)] = docItem[2 + (7 * i)];
                        //        docRow["ReviewStatus" + (i + 1)] = docItem[3 + (7 * i)];
                        //        docRow["Title" + (i + 1)] = docItem[5 + (7 * i)];
                        //    }
                        //    dtDocListFull.Rows.Add(docRow);
                        //}


                        // Fill new rev info for exist doc of trans
                        var countComment = prevCommentCount + 1;

                        if (transObj.ForSentId == 2)
                        {
                            var changeRequestObj = this.changeRequestService.GetAllByIncomingTrans(transObj.ID).FirstOrDefault();
                            if (changeRequestObj != null)
                            {
                                //var existRow = dtDocListFull.AsEnumerable().FirstOrDefault(t => t[0].ToString() == changeRequestObj.Number);
                                //if (existRow != null)
                                //{
                                //    existRow["Rev" + currentSheetIndex] = changeRequestObj.Revision;
                                //    existRow["ReviewStatus" + currentSheetIndex] = changeRequestObj.ReviewResultName;
                                //    existRow["Title" + currentSheetIndex] = changeRequestObj.Title;
                                //}

                                // Update review status for change request
                                for (int i = 0; i < prevDocCount; i++)
                                {
                                    if (changeRequestObj.Number == transSheet.Cells[5 + i, 2].Value.ToString())
                                    {
                                        transSheet.Cells[5 + i, 5].PutValue(changeRequestObj.ReviewResultName);
                                    }
                                }
                                // --------------------------------------------------------------------------

                                var commentList = this.GetCommentInfoChangeRequest(changeRequestObj).OrderBy(t => t).ToList();
                                if (commentList.Count != 0)
                                {
                                    for (var i = 0; i < commentList.Count; i++)
                                    {
                                        var page = commentList[i].Split('$')[1];
                                        var commentIndex = commentList[i].Split('$')[0].Split('.')[0];
                                        var existComment = dtCommentList.AsEnumerable()
                                            .FirstOrDefault(t => t[1].ToString().Split('/')[0].Trim() == changeRequestObj.Number.Trim()
                                                                 && t[1].ToString().Split('/')[2].Trim() == commentIndex.Trim());
                                        if (existComment != null)
                                        {
                                            existComment["Comment" + currentSheetIndex] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                        }
                                        else
                                        {
                                            var commentRow = dtCommentList.NewRow();
                                            commentRow["Index"] = countComment;
                                            commentRow["DocNo"] = changeRequestObj.Number + " / " + page + " / " + commentIndex;
                                            commentRow["Comment" + currentSheetIndex] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                            dtCommentList.Rows.Add(commentRow);
                                            countComment += 1;
                                        }
                                    }
                                }
                                
                            }
                        }


                        foreach (var docObj in docOfTrans)
                        {
                            //var existRow = dtDocListFull.AsEnumerable().FirstOrDefault(t => t[0].ToString() == docObj.DocNo);
                            //if (existRow != null)
                            //{
                            //    existRow["Rev" + currentSheetIndex] = docObj.Revision;
                            //    existRow["ReviewStatus" + currentSheetIndex] = docObj.DocReviewStatusCode;
                            //    existRow["Title" + currentSheetIndex] = docObj.DocTitle;
                            //}

                            var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                            if (commentList.Count != 0)
                            {
                                for (var i = 0; i < commentList.Count; i++)
                                {
                                    var page = commentList[i].Split('$')[1];
                                    var commentIndex = commentList[i].Split('$')[0].Split('.')[0];
                                    var existComment = dtCommentList.AsEnumerable()
                                        .FirstOrDefault(t => t[1].ToString().Split('/')[0].Trim() == docObj.DocNo.Trim()
                                                             && t[1].ToString().Split('/')[2].Trim() == commentIndex.Trim());
                                    if (existComment != null)
                                    {
                                        existComment["Comment" + currentSheetIndex] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                    }
                                    else
                                    {
                                        var commentRow = dtCommentList.NewRow();
                                        commentRow["Index"] = countComment;
                                        commentRow["DocNo"] = docObj.DocNo + " / PAGE " + page + " / " + commentIndex;
                                        commentRow["Comment" + currentSheetIndex] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                        dtCommentList.Rows.Add(commentRow);
                                        countComment += 1;
                                    }


                                }
                            }
                        }
                        if (dtCommentList.Rows.Count == 0)
                        {
                            var commentRow = dtCommentList.NewRow();
                            commentRow["Index"] = 1;
                            dtCommentList.Rows.Add(commentRow);
                        }
                        // Update review status for doclist
                        for (int i = 0; i < prevDocCount; i++)
                        {
                            var docObj = docOfTrans.FirstOrDefault(t => t.DocNo == transSheet.Cells[5 + i, 2].Value.ToString());
                            if (docObj != null)
                            {
                                transSheet.Cells[5 + i, 12+(7*(currentSheetIndex-2)) ].PutValue(docObj.DocReviewStatusCode);
                                transSheet.Cells[5 + i, 13 + (7 * (currentSheetIndex - 2))].PutValue(docObj.DocReviewStatusCode2);
                            }
                        }
                        // --------------------------------------------------------------------------

                        if (dtCommentList.Rows.Count > 0)
                        {
                            dtCommentList = dtCommentList.AsEnumerable().OrderBy(t => t["DocNo"]).CopyToDataTable();
                        }

                        for (int i = 0; i < dtCommentList.Rows.Count; i++)
                        {
                            dtCommentList.Rows[i]["Index"] = i + 1;
                        }

                        transSheet.Cells.ImportDataTable(dtCommentList, false, 9 + currentDocListCount + 2 + prevGeneralCommentCount, 2, dtCommentList.Rows.Count, dtCommentList.Columns.Count, false);
                        //transSheet.Cells.DeleteRows(9 + currentDocListCount + dtCommentList.Rows.Count, prevCommentCount);

                        for (int i = 1; i <= dtCommentList.Rows.Count - 1; i++)
                        {
                            for (int j = 0; j < currentSheetIndex; j++)
                            {
                                transSheet.Cells.Merge(9 + i + currentDocListCount + 2 + prevGeneralCommentCount, 4 + (7 * j), 1, 4);
                                transSheet.Cells.Merge(9 + i + currentDocListCount + 2 + prevGeneralCommentCount, 8 + (7 * j), 1, 2);
                            }
                        }
                        // Apply border 
                        var rb = transSheet.Cells.CreateRange(9 + currentDocListCount + 2 + prevGeneralCommentCount, 2, dtCommentList.Rows.Count, 2 + 6 * currentSheetIndex + currentSheetIndex - 1);
                        var style = workbook.CreateStyle();
                        style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.LeftBorder].Color = Color.Black;
                        style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.RightBorder].Color = Color.Black;
                        style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.TopBorder].Color = Color.Black;
                        style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.BottomBorder].Color = Color.Black;
                        var flag = new StyleFlag();

                        flag.Borders = true;
                        rb.ApplyStyle(style, flag);
                        // --------------------------------------------------------------------------




                        // Filter new doc of trans
                        //var newDoc = docOfTrans.Where(t => !prevDtDocList.Select(x => x[0]).Contains(t.DocNo)).ToList();
                        //foreach (var docObj in newDoc)
                        //{
                        //    var docRow = dtDocListFull.NewRow();
                        //    docRow["DocNo"] = docObj.DocNo;
                        //    docRow["Rev" + currentSheetIndex] = docObj.Revision;
                        //    docRow["ReviewStatus" + currentSheetIndex] = docObj.DocReviewStatusCode;
                        //    docRow["Title" + currentSheetIndex] = docObj.DocTitle;
                        //    dtDocListFull.Rows.Add(docRow);
                        //}
                        // --------------------------------------------------------------------------
                    }

                    //transSheet.Cells.ImportDataTable(dtDocListFull, false, 5, 2, dtDocListFull.Rows.Count, dtDocListFull.Columns.Count, true);

                    //transSheet.Cells.DeleteRow(7 + dtFull.Rows.Count);

                    // Merge Cell
                    //for (int i = 1; i <= dtDocListFull.Rows.Count; i++)
                    //{
                    //    transSheet.Cells.Merge(5 + i, 2, 1, 2);
                    //    for (int j = 0; j < currentSheetIndex; j++)
                    //    {
                    //        transSheet.Cells.Merge(5 + i, 7 + (7 * j), 1, 3);
                    //    }
                    //}


                    // ------------------------------------------------------------------------

                    //dataControlSheet.Cells["A2"].PutValue(dtDocListFull.Rows.Count);
                    //dataControlSheet.Cells["A3"].PutValue(dtCommentList.Rows.Count);
                    var options = new AutoFitterOptions();
                    options.AutoFitMergedCells = true;
                    transSheet.AutoFitRows(options);

                    transSheet.Cells.SetRowHeight(1, 23.25);
                    transSheet.Cells.SetRowHeight(2, 23.25);
                    transSheet.Cells.SetRowHeight(3, 23.25);
                    transSheet.Cells.SetRowHeight(4, 23.25);
                    transSheet.Cells.SetRowHeight(6 + currentDocListCount, 23.25);
                    transSheet.Cells.SetRowHeight(7 + currentDocListCount, 23.25);
                    transSheet.Cells.SetRowHeight(8 + currentDocListCount, 23.25);

                    if (!transObj.IsFirstTrans.GetValueOrDefault())
                    {
                        var prevTransSheet = workbook.Worksheets[currentSheetIndex - 1];
                        for (int i = 0; i < prevCommentCount; i++)
                        {
                            transSheet.Cells.SetRowHeight(5 + dtDocListFull.Rows.Count + 4 + i, prevTransSheet.Cells.GetRowHeight(5 + prevDocCount + 4 + i));
                        }
                    }

                    var filename = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + "_CRS.xlsm";
                    var saveFilePath = Server.MapPath("../.." + transObj.StoreFolderPath + "/eTRM File/" + filename);
                    workbook.Save(saveFilePath);


                    var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + transObj.StoreFolderPath + "/eTRM File";
                    // Path file to download from server
                    var serverFilePath = serverFolder + "/" + filename;
                    // Attach CRS to document Obj
                    var fileInfo = new FileInfo(saveFilePath);
                    crsFile.Filename = filename;
                    crsFile.FilePath = serverFilePath;
                    crsFile.CreatedDate = DateTime.Now;
                    crsFile.CreatedBy = UserSession.Current.User.Id;
                    crsFile.CreatedByName = UserSession.Current.User.UserNameWithFullName;
                    crsFile.FileSize = (double)fileInfo.Length / 1024;
                    this.pecc2TransmittalAttachFileService.Update(crsFile);
                    // -------------------------------------------------------------------------------------------------

                    this.IsHasCRSFile.Value = "True";
                    this.grdCRSFilePECC2.Rebind();
                }
            }
        }


        private void btnExportCRSk()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["objId"]))
            {
                var transObj = this.pecc2TransmittalService.GetById(new Guid(Request.QueryString["objId"]));
                if (transObj != null)
                {
                    var projectObj = this.projectCodeService.GetById(transObj.ProjectCodeId.GetValueOrDefault());
                    var docOfTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID)
                        .Select(t => this.pecc2DocumentsService.GetById(t.DocumentId.GetValueOrDefault()))
                        .Where(t => t != null)
                        .OrderBy(t => t.DocNo)
                        .ToList();
                    var filePath = Server.MapPath("../../Exports") + @"\";
                    var workbook = new Workbook();
                    if (transObj.IsFirstTrans.GetValueOrDefault())
                    {
                        workbook = new Workbook(filePath + @"Template\PECC2_CRS_Template.xlsm");
                    }
                    else
                    {
                        var responseCRSAttachFile = this.contractorTransmittalAttachFileService.GetByTrans(transObj.ContractorTransId.GetValueOrDefault()).FirstOrDefault(t => t.TypeId == 2);
                        if (responseCRSAttachFile != null)
                        {
                            workbook = new Workbook(Server.MapPath("../.." + responseCRSAttachFile.FilePath));
                        }
                    }

                    var dataControlSheet = workbook.Worksheets[0];
                    var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value);
                    var transSheet = workbook.Worksheets[currentSheetIndex];
                    transSheet.Name = transObj.TransmittalNo;

                    if (currentSheetIndex != 1)
                    {
                        transSheet.IsVisible = true;
                    }

                    // Update new current sheet
                    dataControlSheet.Cells["A1"].Value = currentSheetIndex + 1;
                    // -------------------------------------------------------------------------------------------------------

                    transSheet.Cells["B1"].PutValue(projectObj.Description + "\nCOMMENT RESPONSE SHEET");
                    transSheet.Cells["D5"].PutValue(transObj.TransmittalNo);
                    transSheet.Cells["F5"].PutValue(transObj.IssuedDate);
                    transSheet.Cells["G5"].PutValue(transObj.PurposeName.Split(',')[0]);

                    var dtFull = new DataTable();
                    dtFull.Columns.AddRange(new[]
                    {
                        new DataColumn("Temp", typeof(String)),
                        new DataColumn("NoIndex", typeof(String)),
                        new DataColumn("DocNo", typeof(String)),
                        new DataColumn("Rev", typeof(String)),
                        new DataColumn("ReviewStatus", typeof(String)),
                        new DataColumn("CommentIndex", typeof(String)),
                        new DataColumn("Comment", typeof(String)),
                        new DataColumn("Page", typeof(String)),
                        new DataColumn("CommentedBy", typeof(String)),
                        new DataColumn("ApproveBy", typeof(String)),
                        new DataColumn("ContractorResponse", typeof(String)),
                    });

                    var countDocOfTrans = 1;
                    if (transObj.IsFirstTrans.GetValueOrDefault())
                    {
                        foreach (var docObj in docOfTrans)
                        {
                            var dataRow = dtFull.NewRow();
                            dataRow["Temp"] = countDocOfTrans;
                            dataRow["NoIndex"] = countDocOfTrans;
                            dataRow["DocNo"] = docObj.DocNo;
                            dataRow["Rev"] = docObj.Revision;
                            dataRow["ReviewStatus"] = docObj.DocReviewStatusCode;
                            dataRow["Comment"] = docObj.DocTitle;
                            dtFull.Rows.Add(dataRow);

                            var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                            if (commentList.Count != 0)
                            {
                                for (var i = 0; i < commentList.Count; i++)
                                {
                                    var dataRow1 = dtFull.NewRow();
                                    dataRow1["Temp"] = countDocOfTrans;
                                    dataRow1["Rev"] = docObj.Revision;
                                    dataRow1["CommentIndex"] = commentList[i].Split('$')[0].Split('.')[0];
                                    dataRow1["Comment"] = commentList[i].Split('$')[0].Substring(commentList[i].Split('$')[0].IndexOf('.') + 1);
                                    dataRow1["Page"] = commentList[i].Split('$')[1];
                                    dataRow1["CommentedBy"] = commentList[i].Split('$')[2];
                                    dtFull.Rows.Add(dataRow1);
                                }
                            }
                            //else
                            //{
                            //    dtFull.Rows.Add(dataRow);
                            //}

                            countDocOfTrans += 1;
                        }
                    }
                    else
                    {
                        var prevRowCount = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);
                        var previousTransSheet = workbook.Worksheets[currentSheetIndex - 1];
                        var previousDataTable = previousTransSheet.Cells.ExportDataTable(7, 2, prevRowCount, 11);
                        if (previousDataTable.Rows.Count > 0)
                        {
                            var previousDataTableRows = previousDataTable.AsEnumerable().ToList();
                            foreach (var docObj in docOfTrans)
                            {
                                var previousDataDocRow = previousDataTableRows.FirstOrDefault(t => t["Column3"].ToString() == docObj.DocNo);
                                if (previousDataDocRow != null)
                                {
                                    var previousDataDocRowList = previousDataTableRows.Where(t => t["Column1"].ToString() == previousDataDocRow["Column1"]);

                                    var dataRow = dtFull.NewRow();
                                    dataRow["Temp"] = countDocOfTrans;
                                    dataRow["NoIndex"] = countDocOfTrans;
                                    dataRow["DocNo"] = docObj.DocNo;
                                    dataRow["Rev"] = docObj.Revision;
                                    dataRow["ReviewStatus"] = docObj.DocReviewStatusCode;
                                    dataRow["Comment"] = docObj.DocTitle;
                                    dtFull.Rows.Add(dataRow);

                                    var commentList = this.GetCommentInfo(docObj).OrderBy(t => t).ToList();
                                    if (commentList.Count != 0)
                                    {
                                        var currentCommentIndex = commentList.Select(t => Convert.ToInt32(t.Split('$')[0].Split('.')[0])).Distinct().ToList();
                                        var commentIndexCompletedList = previousDataDocRowList.Where(t => !string.IsNullOrEmpty(t["Column6"].ToString().Trim()) && !currentCommentIndex.Contains(Convert.ToInt32(t["Column6"].ToString().Trim()))).Select(t => Convert.ToInt32(t["Column6"].ToString().Trim())).Distinct().ToList();

                                        var totalCommentIndex = new List<int>();
                                        totalCommentIndex.AddRange(currentCommentIndex);
                                        totalCommentIndex.AddRange(commentIndexCompletedList);
                                        totalCommentIndex = totalCommentIndex.OrderBy(t => t).ToList();

                                        // Fill comment still need review
                                        foreach (var commentIndex in totalCommentIndex)
                                        {
                                            var previousCommentsByIndex = previousDataDocRowList.Where(t => t["Column6"].ToString().Trim() == commentIndex.ToString());

                                            foreach (DataRow prevComment in previousCommentsByIndex)
                                            {
                                                var dataRow2 = dtFull.NewRow();
                                                dataRow2["Temp"] = countDocOfTrans;
                                                dataRow2["Rev"] = prevComment["Column4"];
                                                dataRow2["CommentIndex"] = prevComment["Column6"];
                                                dataRow2["Comment"] = prevComment["Column7"];
                                                dataRow2["Page"] = prevComment["Column8"];
                                                dataRow2["ContractorResponse"] = prevComment["Column11"];
                                                dtFull.Rows.Add(dataRow2);
                                            }

                                            var commentListOfCommentIndex = commentList.Where(t =>
                                                t.Split('$')[0].Split('.')[0] == commentIndex.ToString()).ToList();
                                            for (int i = 0; i < commentListOfCommentIndex.Count; i++)
                                            {
                                                var dataRow1 = dtFull.NewRow();
                                                dataRow1["Temp"] = countDocOfTrans;
                                                dataRow1["Rev"] = docObj.Revision;
                                                dataRow1["CommentIndex"] = commentListOfCommentIndex[i].Split('$')[0].Split('.')[0];
                                                dataRow1["Comment"] = commentListOfCommentIndex[i].Split('$')[0].Substring(commentListOfCommentIndex[i].Split('$')[0].IndexOf('.') + 1);
                                                dataRow1["Page"] = commentListOfCommentIndex[i].Split('$')[1];
                                                dataRow1["CommentedBy"] = commentListOfCommentIndex[i].Split('$')[2];
                                                dtFull.Rows.Add(dataRow1);
                                            }
                                        }
                                    }

                                    countDocOfTrans += 1;
                                }
                            }
                        }
                    }

                    transSheet.Cells.ImportDataTable(dtFull, false, 7, 2, dtFull.Rows.Count, dtFull.Columns.Count, true);
                    transSheet.Cells.DeleteRow(7 + dtFull.Rows.Count);

                   // dataControlSheet.Cells["A2"].PutValue(dtFull.Rows.Count);

                    var filename = Utility.RemoveSpecialCharacterFileName(transObj.TransmittalNo) + "_CRS.xlsm";
                    var saveFilePath = Server.MapPath("../.." + transObj.StoreFolderPath + "/eTRM File/" + filename);
                    workbook.Save(saveFilePath);

                    // Attach CRS to document Obj
                    var fileInfo = new FileInfo(saveFilePath);
                    if (fileInfo.Exists && this.pecc2TransmittalAttachFileService.GetByTrans(transObj.ID).All(t => t.TypeId != 2))
                    {
                        var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
                    + transObj.StoreFolderPath + "/eTRM File";
                        // Path file to download from server
                        var serverFilePath = serverFolder + "/" + filename;
                        var attachFile = new PECC2TransmittalAttachFiles()
                        {
                            ID = Guid.NewGuid(),
                            TransId = transObj.ID,
                            Filename = filename,
                            Extension = "xlsm",
                            FilePath = serverFilePath,
                            ExtensionIcon = "~/images/excelfile.png",
                            FileSize = (double)fileInfo.Length / 1024,
                            CreatedBy = UserSession.Current.User.Id,
                            CreatedByName = UserSession.Current.User.UserNameWithFullName,
                            CreatedDate = DateTime.Now,
                            TypeId = 2,
                            TypeName = "CRS File"
                        };

                        this.pecc2TransmittalAttachFileService.Insert(attachFile);
                    }
                    else
                    {
                        var crsObj = this.pecc2TransmittalAttachFileService.GetByTrans(transObj.ID).FirstOrDefault(t => t.TypeId == 2);
                        if (crsObj != null)
                        {
                            crsObj.CreatedDate = DateTime.Now;
                            crsObj.CreatedBy = UserSession.Current.User.Id;
                            crsObj.CreatedByName = UserSession.Current.User.UserNameWithFullName;
                            crsObj.FileSize = (double)fileInfo.Length / 1024;
                            this.pecc2TransmittalAttachFileService.Update(crsObj);
                        }
                    }
                    // -------------------------------------------------------------------------------------------------
                    this.IsHasCRSFile.Value = "True";
                    this.grdCRSFilePECC2.Rebind();
                }
            }
        }

        private List<string> GetCommentInfo(PECC2Documents docObj)
        {
            var consolidateDocFileList = this.pecc2DocumentAttachFileService.GetAllDocId(docObj.ID);
            var commentList = new List<string>();
            if (consolidateDocFileList.Count != 0)
            {
                var consolidateDocFile = consolidateDocFileList.OrderByDescending(t => t.CreatedDate).FirstOrDefault(t => t.TypeId == 3);
                if (consolidateDocFile == null) return commentList;

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
                            //var username = annot.GetAsString(PdfName.T);
                            count += 1;
                            commentList.Add(text + "$" + i + "$" + consolidateDocFile.CreatedByName.Split('/')[0]);
                        }
                    }
                }
            }

            return commentList;
        }

        private List<string> GetCommentInfoChangeRequest(ChangeRequest changeRequestObj)
        {
            var consolidateDocFileList = this.changeRequestAttachFileService.GetAllByChangeRequest(changeRequestObj.ID);
            var commentList = new List<string>();
            if (consolidateDocFileList.Count != 0)
            {
                var consolidateDocFile = consolidateDocFileList.OrderByDescending(t => t.CreatedDate).FirstOrDefault(t => t.TypeId == 3);
                if (consolidateDocFile == null) return commentList;

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
                            //var username = annot.GetAsString(PdfName.T);
                            count += 1;
                            commentList.Add(text + "$" + i + "$" + consolidateDocFile.CreatedByName.Split('/')[0]);
                        }
                    }
                }
            }

            return commentList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var pecc2TransId = new Guid(this.Request.QueryString["objId"]);
                var pecc2TransObj = this.pecc2TransmittalService.GetById(pecc2TransId);
                if (pecc2TransObj != null)
                {
                    var pecc2ListDoc = this.pecc2DocumentsService.GetAllByIncomingTrans(pecc2TransObj.ID);
                    foreach ( var doc in pecc2ListDoc)
                    {
                        try
                        {
                            if (this.ddlDocreviewStatus2.SelectedItem != null && this.ddlDocreviewStatus2.SelectedValue != "0")
                            {
                                var convertoint = Convert.ToInt32(doc.Revision);
                                doc.DocReviewStatusId2 = this.ddlDocreviewStatus2.SelectedValue != null
                                                           ? Convert.ToInt32(this.ddlDocreviewStatus2.SelectedValue)
                                                           : 0;
                                doc.DocReviewStatusCode2 = this.ddlDocreviewStatus2.SelectedItem != null
                                        ? this.ddlDocreviewStatus2.SelectedItem.Text.Split(',')[0]
                                        : string.Empty;
                            }
                        }
                        catch { }
                        if (this.ddlDocReviewStatus.SelectedItem != null && this.ddlDocReviewStatus.SelectedValue != "0")
                        {
                            doc.DocReviewStatusId = this.ddlDocReviewStatus.SelectedValue != null
                            ? Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue)
                            : 0;
                            doc.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem != null
                                    ? this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0]
                                    : string.Empty;
                            if (UserSession.Current.User.Role.ContractorId == 2)
                            {
                                doc.IsOwnerComment = DateTime.Now;
                            }
                            if (UserSession.Current.User.Role.ContractorId == 1)
                            {
                                doc.IsConsultantComment = DateTime.Now;
                            }
                        }
                        this.pecc2DocumentsService.Update(doc);
                    }

                    this.grdDocumentFile.Rebind();
                }
            }
        }
    }
}