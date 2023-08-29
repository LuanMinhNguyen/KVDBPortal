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
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class Upload : Page
    {
        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public Upload()
        {
            this.documentService = new DocumentService();
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
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));
                    if (objDoc != null)
                    {
                        
                    }
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
            this.Session.Remove("IsFillData");
            Document objDoc;
            if (this.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    objDoc = this.documentService.GetById(Convert.ToInt32(this.Request.QueryString["docId"]));

                    this.SaveUploadFile(this.docuploader, ref objDoc);

                    objDoc.LastUpdatedBy = UserSession.Current.User.Id;
                    objDoc.LastUpdatedDate = DateTime.Now;

                    this.documentService.Update(objDoc);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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

            var fileIcon = new Dictionary<string, string>()
                {
                    { "doc", "images/wordfile.png" },
                    { "docx", "images/wordfile.png" },
                    { "dotx", "images/wordfile.png" },
                    { "xls", "images/excelfile.png" },
                    { "xlsx", "images/excelfile.png" },
                    { "pdf", "images/pdffile.png" },
                    { "7z", "images/7z.png" },
                    { "dwg", "images/dwg.png" },
                    { "dxf", "images/dxf.png" },
                    { "rar", "images/rar.png" },
                    { "zip", "images/zip.png" },
                    { "txt", "images/txt.png" },
                    { "xml", "images/xml.png" },
                    { "xlsm", "images/excelfile.png" },
                    { "bmp", "images/bmp.png" },
                };

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