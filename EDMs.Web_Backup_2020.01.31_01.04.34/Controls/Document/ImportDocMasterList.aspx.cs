// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportDocMasterList : Page
    {
        private readonly DQREDocumentMasterService _dqredocumentService = new  DQREDocumentMasterService();

       

        private readonly PlantService _plantService = new PlantService();

        private readonly AreaService _areaService = new AreaService();

        private readonly UnitService _unitService = new UnitService();
        private readonly MaterialService _MaterialService = new MaterialService();
        private readonly WorkService _WorkcodeService = new WorkService();
        private readonly DrawingService _DrawingcodeService = new DrawingService();
   
        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly UserService userService = new UserService();

        private readonly AttachFileService attachFileService = new AttachFileService();

        private readonly AttachFilesPackageService attachFilesPackageService = new AttachFilesPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

        private readonly ProjectCodeService _projectCodeService = new ProjectCodeService();

        private readonly OrganizationCodeService _organizationcodeService = new OrganizationCodeService();

        

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
            var currentSheetName = string.Empty;
            var currentDocumentNo = string.Empty;
            var documentPackageList = new List<DQREDocumentMaster>();
            var listExistFile = new List<string>();
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

                        for (var i = 2; i < workbook.Worksheets.Count; i++)
                        {
                            var datasheet = workbook.Worksheets[i];

                            currentSheetName = datasheet.Name;
                            var dataTable = datasheet.Cells.ExportDataTable(2, 1,
                            datasheet.Cells.MaxRow, 19);
                            foreach (DataRow dataRow in dataTable.Rows)
                            {
                                if (!string.IsNullOrEmpty(dataRow["Column1"].ToString()))
                                {
                                    currentDocumentNo = dataRow["Column1"].ToString().Trim();


                                    var revision = dataRow["Column3"].ToString();
                                    if (!this._dqredocumentService.IsExistByDocNo(currentDocumentNo))
                                    {
                                        var docObj = new DQREDocumentMaster();
                                        docObj.ID = Guid.NewGuid();
                                        docObj.SystemDocumentNo = dataRow["Column1"].ToString().Trim();
                                        docObj.Title = dataRow["Column2"].ToString().Trim();
                                    
                                        docObj.EquipmentTagName= dataRow["Column3"].ToString();
                                        docObj.DepartmentCode= dataRow["Column4"].ToString();
                                        docObj.MRSequenceNo= dataRow["Column5"].ToString();
                                        docObj.DocumentSequenceNo= dataRow["Column6"].ToString();
                                        docObj.SheetNo= dataRow["Column7"].ToString();
                                        var originator = this._organizationcodeService.GetByCode(dataRow["Column8"].ToString());
                                            docObj.OriginatorId = originator != null? originator.ID:0;
                                            docObj.OriginatorName = originator != null ?originator.FullName:string.Empty;
                                        

                                        var originatoring = this._organizationcodeService.GetByCode(dataRow["Column9"].ToString());
                                       
                                            docObj.OriginatingOrganizationId = originatoring != null?originatoring.ID:0;
                                            docObj.OriginatingOrganizationName = originatoring != null? originatoring.FullName: string.Empty;
                                       
                                        var Revicing = this._organizationcodeService.GetByCode(dataRow["Column10"].ToString());
                                        
                                            docObj.ReceivingOrganizationId = Revicing != null? Revicing.ID:0;
                                            docObj.ReceivingOrganizationName = Revicing != null? Revicing.FullName:string.Empty;
                                       
                                        var documentype = this.documentTypeService.GetByCode(dataRow["Column11"].ToString());
                                      
                                            docObj.DocumentTypeId = documentype != null? documentype.ID:0;
                                            docObj.DocumentTypeName = documentype != null ? documentype.FullName:string.Empty;
                                        
                                        var discipline = this.disciplineService.GetByCode(dataRow["Column12"].ToString());
                                       
                                            docObj.DisciplineId = discipline != null? discipline.ID:0;
                                            docObj.DisciplineName = discipline != null? discipline.FullName:string.Empty;
                                        
                                        var material= this._MaterialService.GetByCode(dataRow["Column13"].ToString());
                                         docObj.MaterialCodeId = material != null? material.ID:0;
                                            docObj.MaterialCodeName = material != null? material.FullName:string.Empty;
                                       
                                         var work= this._WorkcodeService.GetByCode(dataRow["Column14"].ToString());
                                           docObj.WorkCodeId = work != null? work.ID:0;
                                            docObj.WorkCodeName = work != null? work.FullName:string.Empty;
                                        

                                        var drawing= this._DrawingcodeService.GetByCode(dataRow["Column15"].ToString());
                                       
                                            docObj.DrawingCodeId = drawing != null? drawing.ID:0;
                                            docObj.DrawingCodeName = drawing != null? drawing.FullName:string.Empty;
                                       
                                        var unit = this._unitService.GetByCode(dataRow["Column18"].ToString());
                                        if (unit != null)
                                        {
                                            docObj.UnitId = unit.ID;
                                            docObj.UnitName = unit.Code;
                                            var area = this._areaService.GetById(unit.AreaId.GetValueOrDefault());
                                            if(area !=null){
                                                docObj.AreaId = area.ID;
                                                docObj.AreaName = area.Code;
                                                var plant = this._plantService.GetById(area.PlantId.GetValueOrDefault());
                                                docObj.PlantId = plant != null ? plant.ID : 0;
                                                docObj.PlantName = plant != null ? plant.Name : string.Empty;
                                            }
                                            else
                                            {
                                                docObj.AreaId = 0;
                                                docObj.AreaName =string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            docObj.UnitId = 0;
                                            docObj.UnitName = string.Empty;
                                        }

                                      
                                        docObj.IsDelete = false;
                                        docObj.CreatedBy = UserSession.Current.User.Id;
                                        docObj.CreatedDate = DateTime.Now;
                                       
                                        if (!documentPackageList.Exists(t=> t.SystemDocumentNo == docObj.SystemDocumentNo))
                                        {
                                            documentPackageList.Add(docObj);
                                        }
                                       
                                    }
                                    else
                                    {
                                        listExistFile.Add(currentDocumentNo); }
                                }
                              
                            }

                            if (string.IsNullOrEmpty(this.lblError.Text))
                            {
                                foreach (var docObj in documentPackageList)
                                {

                                    this._dqredocumentService.Insert(docObj);
                                }

                                this.blockError.Visible = true;
                                this.lblError.Text =
                                    "Data of document master list file is valid. System import successfull!";
                            }
                            else
                            {
                                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey",
                                    "CloseAndRebind();", true);
                            }

                        }
                    }
                    if (listExistFile.Count > 0)
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text += "Document already exists: <br/>";
                        foreach (var item in listExistFile)
                        {
                            this.lblError.Text += "<span style='color: blue; font-weight: bold'>'" + item + "'</span> <br/>";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.blockError.Visible = true;
                this.lblError.Text = "Have error at sheet: '" + currentSheetName + "', document: '" + currentDocumentNo + "', with error: '" + ex.Message + "'";
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