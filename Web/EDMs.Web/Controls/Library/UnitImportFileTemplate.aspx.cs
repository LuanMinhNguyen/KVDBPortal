
namespace EDMs.Web.Controls.Library
{
    using System;
    using System.Web.Hosting;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;


    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using Aspose.Cells;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class UnitImportFileTemplate : Page
    {

        private readonly AreaService areaService_ = new AreaService();

        private readonly PlantService plantService_ = new PlantService();

        private readonly UnitService UnitService_ = new UnitService();




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
            try
            {
                var error = "";
                var docNonExistList = new List<string>();
                var splitAttachFileNameCharacter =
                                       ConfigurationManager.AppSettings.Get("SplitAttachFileNameCharacter");
                var fileIcon = new Dictionary<string, string>()
                {
                    {"doc", "~/images/wordfile.png"},
                    {"docx", "~/images/wordfile.png"},
                    {"dotx", "~/images/wordfile.png"},
                    {"xls", "~/images/excelfile.png"},
                    {"xlsx", "~/images/excelfile.png"},
                    {"pdf", "~/images/pdffile.png"},
                    {"7z", "~/images/7z.png"},
                    {"dwg", "~/images/dwg.png"},
                    {"dxf", "~/images/dxf.png"},
                    {"rar", "~/images/rar.png"},
                    {"zip", "~/images/zip.png"},
                    {"txt", "~/images/txt.png"},
                    {"xml", "~/images/xml.png"},
                    {"xlsm", "~/images/excelfile.png"},
                    {"bmp", "~/images/bmp.png"},
                };

                foreach (UploadedFile docFile in this.docuploader.UploadedFiles)
                {
                    var extension = docFile.GetExtension();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {
                        var transFileName = docFile.FileName;
                        var importPath = Server.MapPath("../../Import") + "/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + docFile.FileName;
                        docFile.SaveAs(importPath);

                        //  var owner = this.toListService.GetAll().FirstOrDefault(t => t.Name == "XNXL");

                        var workbook = new Workbook();
                        workbook.Open(importPath);
                        var tranSheet = workbook.Worksheets[0];
                        var datatable = new DataTable();
                        datatable = tranSheet.Cells.ExportDataTable(2, 1,
                                               tranSheet.Cells.MaxRow-1, 4);

                        foreach (DataRow dataRow in datatable.Rows)
                        {
                            var AreaText = dataRow["Column1"].ToString();
                            var UnitCode = dataRow["Column2"].ToString();
                            var description = dataRow["Column3"].ToString();
                            var Shortform = dataRow["Column4"].ToString();

                            var Obj = this.areaService_.GetByCode(AreaText);
                            if (Obj != null && !string.IsNullOrEmpty(UnitCode))
                            {
                                var areaobj = new Unit()
                                {
                                    Code = UnitCode,
                                    Name=Shortform,
                                    AreaId = Obj.ID,
                                    Description = description,
                                    CreatedBy = UserSession.Current.User.Id,
                                    CreatedDate = DateTime.Now,
                                    Active = true,
                                };
                                this.UnitService_.Insert(areaobj);
                            }
                            else
                            {
                                error = "Have Error";
                            }

                        }
                    }


                }

                if (error.Length != 0)
                {
                    this.blockError.Visible = true;
                    this.lblError.Text = "AreaCode does not exist</br>";

                }
                else
                {
                    this.blockError.Visible = true;
                    this.lblError.Text = "Import template file Successfull.";
                }

            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error at template Form: '" + ex.Message + "'";
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


        private string RemoveAllSpecialCharacter(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z]+", string.Empty);
        }

    }
}