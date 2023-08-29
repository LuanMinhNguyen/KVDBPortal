// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using EDMs.Business.Services.Security;

namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;

    using Aspose.Cells;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;

    using Telerik.Web.UI;
    using Utilities.Sessions;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportMaterialCode : Page
    {

        private readonly MaterialService materialService;

        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ImportMaterialCode()
        {
            this.materialService = new MaterialService();

            this.userService = new UserService();
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
            this.lblError.Text = string.Empty;
            try
            {
                foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                {
                    var extension = docFile.GetExtension();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {
                        var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + docFile.FileName;
                        docFile.SaveAs(importPath);

                        var workbook = new Workbook();
                        workbook.Open(importPath);
                        var dmSheet = workbook.Worksheets[0];
                        var rowNumber = Convert.ToInt32(dmSheet.Cells["A1"].Value);
                        var columnNumber = Convert.ToInt32(dmSheet.Cells["A2"].Value);

                        // Get data from Project Code File
                        var prDetailTable = dmSheet.Cells.ExportDataTable(3, 1, rowNumber, columnNumber);

                        foreach (DataRow prDetailRow in prDetailTable.Rows)
                        {
                            var ID = prDetailRow["Column1"].ToString();
                            var Code = prDetailRow["Column2"].ToString();
                            var Description = prDetailRow["Column3"].ToString();
                            var materialCodeObj = this.materialService.GetById(int.Parse(ID));
                            if (materialCodeObj != null)
                            {
                                materialCodeObj.Code = Code;
                                materialCodeObj.Description = Description;
                                materialCodeObj.LastUpdatedBy = UserSession.Current.User.Id;
                                materialCodeObj.LastUpdatedDate = DateTime.Now;
                                materialService.Update(materialCodeObj);
                            }
                            else
                            {
                                materialCodeObj = new MaterialCode();
                                materialCodeObj.Code = Code;
                                materialCodeObj.Description = Description;
                                materialCodeObj.CreatedBy = UserSession.Current.User.Id;
                                materialCodeObj.CreatedDate = DateTime.Now;
                                this.materialService.Insert(materialCodeObj);
                            }
                        }

                        if (string.IsNullOrEmpty(this.lblError.Text))
                        {
                            this.blockError.Visible = true;
                            this.lblError.Text = "System import successfull!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error: '" + ex.Message + "'";
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
    }
}