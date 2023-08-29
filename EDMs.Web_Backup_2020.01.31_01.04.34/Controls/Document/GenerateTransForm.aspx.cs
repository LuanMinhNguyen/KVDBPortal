// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using EDMs.Business.Services.Scope;
using EDMs.Web.Utilities;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using System.Linq;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class GenerateTransForm : Page
    {
        private readonly DQREDocumentService documentService = new DQREDocumentService();




        private readonly DQRETransmittalService transmittalService = new DQRETransmittalService();

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        private readonly RevisionService revisionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public GenerateTransForm()
        {
            this.documentService = new DQREDocumentService();
            this.transmittalService = new DQRETransmittalService();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
            this.revisionService = new RevisionService();
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
                this.LoadComboData();
            }
        }

        private void LoadComboData()
        {
            //var toList = this.toListService.GetAll();

            //this.ddlFromList.DataSource = toList;
            //this.ddlFromList.DataTextField = "FullName";
            //this.ddlFromList.DataValueField = "Id";
            //this.ddlFromList.DataBind();

            //var projectList = this.scopeProjectService.GetAll();
            //this.ddlProject.DataSource = projectList;
            //this.ddlProject.DataTextField = "FullName";
            //this.ddlProject.DataValueField = "ID";
            //this.ddlProject.DataBind();
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
            {
                var transID = new Guid(this.Request.QueryString["tranId"]);
                var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transID);
                if (attachDocToTrans != null)
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
                    //var filePath = Server.MapPath("Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\TransTemplate.xlsx");

                    var dataSheet = workbook.Worksheets[0];

                    var dtFull = new DataTable();

                    dtFull.Columns.AddRange(new[]
                    {
                        new DataColumn("DocumentNo", typeof (String)),
                        new DataColumn("Revision", typeof (String)),
                        new DataColumn("IsssuedDate", typeof (String)),
                        new DataColumn("DocumentTitle", typeof (String)),
                        new DataColumn("DocumentClassName", typeof (String)),
                        new DataColumn("DocumentCodeName", typeof (String))
                    });

                    foreach (var docobj in attachDocToTrans)
                    {
                        var dataRow = dtFull.NewRow();
                        var documentObj = this.documentService.GetById(docobj.DocumentId.GetValueOrDefault());
                        dataRow["DocumentNo"] = documentObj.DocumentNo;
                        dataRow["Revision"] = documentObj.Revision;
                        dataRow["IsssuedDate"] = Convert.ToDateTime(documentObj.IsssuedDate).ToString("dd-MMM-yy");
                        dataRow["DocumentTitle"] = documentObj.DocumentTitle;
                        dataRow["DocumentClassName"] = documentObj.DocumentClassName;
                        dataRow["DocumentCodeName"] = documentObj.DocumentCodeName;
                        dtFull.Rows.Add(dataRow);
                    }
                    //dataSheet.Cells.ImportDataTable(dtFull, false, 12, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
                    int firstrow = 12;
                    for (int i = 0; i < dtFull.Rows.Count; i++)
                    {
                        firstrow++;
                        dataSheet.Cells["C" + firstrow].PutValue(dtFull.Rows[i]["DocumentNo"]);
                        dataSheet.Cells["E" + firstrow].PutValue(dtFull.Rows[i]["Revision"]);
                        dataSheet.Cells["F" + firstrow].PutValue(dtFull.Rows[i]["IsssuedDate"]);
                        dataSheet.Cells["G" + firstrow].PutValue(dtFull.Rows[i]["DocumentTitle"]);
                        dataSheet.Cells["J" + firstrow].PutValue(dtFull.Rows[i]["DocumentClassName"]);
                        dataSheet.Cells["K" + firstrow].PutValue(dtFull.Rows[i]["DocumentCodeName"]);
                    }

                    var transObj = this.transmittalService.GetById(transID);
                    var filename = transObj.TransmittalNo + "-" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);
                }
            }
        }
        private void Download_File(string FilePath)
        {
            Response.Clear();
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
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
    }
}