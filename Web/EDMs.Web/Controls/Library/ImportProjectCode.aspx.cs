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
    public partial class ImportProjectCode : Page
    {

        private readonly ProjectCodeService projectCodeService;

        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ImportProjectCode()
        {
            this.projectCodeService = new ProjectCodeService();

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
                        var prDetailTable = dmSheet.Cells.ExportDataTableAsString(3, 1, rowNumber, columnNumber);

                        foreach (DataRow prDetailRow in prDetailTable.Rows)
                        {
                            var ID = (prDetailRow["Column1"].ToString()) != "" ? prDetailRow["Column1"].ToString() : "0";
                            var Code = prDetailRow["Column2"].ToString();
                            var Description = prDetailRow["Column3"].ToString();
                            var projectCodeObj = this.projectCodeService.GetById(int.Parse(ID));
                            if (projectCodeObj != null)
                            {
                                projectCodeObj.Code = Code;
                                projectCodeObj.Description = Description;
                                projectCodeObj.LastUpdatedBy = UserSession.Current.User.Id;
                                projectCodeObj.LastUpdatedDate = DateTime.Now;
                                projectCodeService.Update(projectCodeObj);
                            }
                            else
                            {
                                projectCodeObj = new ProjectCode();
                                projectCodeObj.Code = Code;
                                projectCodeObj.Description = Description;
                                projectCodeObj.CreatedBy = UserSession.Current.User.Id;
                                projectCodeObj.CreatedDate = DateTime.Now;
                                this.projectCodeService.Insert(projectCodeObj);
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