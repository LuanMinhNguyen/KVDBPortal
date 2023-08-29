// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using EDMs.Business.Services.Scope;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Data;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Globalization;
    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class RFIImportDetail : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly RFIService rfiService;
        private readonly RFIDetailService rfiDetailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public RFIImportDetail()
        {
            this.rfiDetailService = new RFIDetailService();
            this.rfiService = new RFIService();
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
                //if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                //{
                //    var objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
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

            try
            {
                var currentSheetName = string.Empty;
                var currentDocumentNo = string.Empty;

                foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                {
                    var extension = docFile.GetExtension();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {
                        var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + docFile.FileName;
                        docFile.SaveAs(importPath);

                        var workbook = new Workbook();
                        workbook.Open(importPath);
                        var datasheet = workbook.Worksheets[0];

                        currentSheetName = datasheet.Name;
                        var dataTable = datasheet.Cells.ExportDataTable(4, 1,
                        datasheet.Cells.MaxRow, 10);
                        var rfiId = new Guid(this.Request.QueryString["objId"]);
                        var rfiobj = this.rfiService.GetById(rfiId);
                        bool continues= true;
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            if (( dataRow["Column1"].ToString().Trim().Contains("Comments of Owner's PMC:")||string.IsNullOrEmpty(dataRow["Column3"].ToString()))&& continues)
                            {
                                continues = false;
                            }
                            if (!string.IsNullOrEmpty(dataRow["Column3"].ToString()) && continues)
                            {
                                var obj = new RFIDetail();
                                obj.ID = Guid.NewGuid();
                                obj.WorkTitle = dataRow["Column3"].ToString().Trim();
                                obj.RFIID = rfiobj.ID;
                                obj.RFINo = rfiobj.Number;
                                obj.Number= this.rfiDetailService.GetByRFI(rfiId).Count + 1;
                                obj.GroupId = rfiobj.GroupId;
                                obj.GroupName = rfiobj.GroupName;
                                obj.Description= dataRow["Column4"].ToString().Trim();
                                obj.Location= dataRow["Column5"].ToString().Trim();
                                obj.InspectionTypeName= dataRow["Column7"].ToString().Trim();
                                obj.ContractorContact= dataRow["Column8"].ToString().Trim();
                                obj.Remark= dataRow["Column9"].ToString().Trim();
                                DateTime date;
                                if (DateTime.TryParseExact(dataRow["Column6"].ToString().Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                {
                                    obj.Time = date;
                                }
                                obj.CreatedBy = UserSession.Current.User.Id;
                                obj.CreatedByName = UserSession.Current.User.FullName;
                                obj.CreatedDate = DateTime.Now;
                                this.rfiDetailService.Insert(obj);
                                
                            }
                        }
                    }
                }
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error when import Progress: <br/>'" + ex.Message + "'";
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

        /// <summary>
        /// The save upload file.
        /// </summary>
        /// <param name="uploadDocControl">
        /// The upload doc control.
        /// </param>
        /// <param name="objDoc">
        /// The obj Doc.
        /// </param>
        private void SaveUploadFile(RadAsyncUpload uploadDocControl, ref Document objDoc)
        {
            var listUpload = uploadDocControl.UploadedFiles;
            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {
                    var revisionFilePath = Server.MapPath(objDoc.RevisionFilePath.Replace("/" + HostingEnvironment.ApplicationVirtualPath, "../.."));

                    docFile.SaveAs(revisionFilePath, true);
                }
            }
        }
    }
}