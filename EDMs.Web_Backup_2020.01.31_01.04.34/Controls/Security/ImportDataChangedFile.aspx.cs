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
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Security
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportDataChangedFile : Page
    {
        private readonly ImportDataChangeHistoryService importHistoryService = new ImportDataChangeHistoryService();
        private readonly IntergrateParamConfigService configService = new IntergrateParamConfigService();
        private SqlConnection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportDataChangedFile"/> class.
        /// </summary>
        public ImportDataChangedFile()
        {
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
            var flag = false;
            var configObj = configService.GetById(1);
            var sqlList = new List<String>();

            conn = new SqlConnection(configObj.Sync_ImportConnStr);
            foreach (UploadedFile dataFile in this.docuploader.UploadedFiles)
            {
                var extension = dataFile.GetExtension();
                if (extension == ".sql")
                {
                    var importPath = Server.MapPath("../../Import") + "/" + dataFile.FileName;
                    dataFile.SaveAs(importPath);
                    var dirInfo = new DirectoryInfo(configObj.Sync_ImportFolder);
                    File.Copy(importPath, dirInfo.FullName + "/" + dataFile.FileName, true);



                    var line = string.Empty;
                    // Read the file and display it line by line.
                    var file = new StreamReader(importPath);
                    conn.Open();
                    var trans = conn.BeginTransaction();
                    try
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                var cmd = new SqlCommand(line, conn, trans);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                        // Tracking import file
                        var importItem = new ImportDataChangeHistory()
                        {
                            FilePath = dirInfo.FullName + "/" + dataFile.FileName,
                            FileName = dataFile.FileName,
                            CreatedTime = DateTime.Now,
                            IsComplete = false,
                            ErrorMess = string.Empty

                        };

                        this.importHistoryService.Insert(importItem);
                        // ------------------------------------------------------
                        importItem.IsComplete = true;
                        importHistoryService.Update(importItem);

                        this.blockError.Visible = true;
                        this.lblError.Text = "Import data changed file Completed.";
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        this.blockError.Visible = true;
                        this.lblError.Text = "Have error: " + ex.Message;
                        break;
                    }

                    file.Close();
                }
            }
        }
    }
}