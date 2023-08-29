// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class DistributionMatrixList : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DistributionMatrixService distributionMatrixService = new DistributionMatrixService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly DocumentTypeService docTypeService = new DocumentTypeService();

        private readonly DistributionMatrixDetailService dmDetailService = new DistributionMatrixDetailService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly UserService userService = new UserService();

        private readonly UnitService unitService = new UnitService();

        private readonly MaterialService materialService = new MaterialService();

        private readonly GroupCodeService groupCodeService = new GroupCodeService();
        
        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

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
            Session.Add("SelectedMainMenu", "System");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.LoadScopePanel();
                this.LoadSystemPanel();
            }
        }

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false)
        {
            this.grdDocument.DataSource = this.distributionMatrixService.GetAll().OrderBy(t=> t.Name);

            if (isbind)
            {
                this.grdDocument.DataBind();
            }
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.MasterTableView.CurrentPageIndex = this.grdDocument.MasterTableView.PageCount - 1;
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("ExportDMDetail_"))
            {
                this.ExportDMDetail(e.Argument);
            }
        }

        private void ExportDMDetail(string strCommand)
        {
            var dmId = Convert.ToInt32(strCommand.Split('_')[1]);
            //var typeId = Convert.ToInt32(strCommand.Split('_')[2]);
            var dmObj = this.distributionMatrixService.GetById(dmId);
            if (dmObj != null)
            {
                this.ExportDistributionMatrixDetail(dmObj);
                //switch (typeId)
                //{
                //    // Document have Material/Work Code
                //    case 1:
                //        this.ExportMaterialWorkCodeMatrixDetail(dmObj);
                //        break;
                //    // Document have Drawing Code (00) Matrix
                //    case 2:
                //        this.ExportDrawingCode00MatrixDetail(dmObj);
                //        break;
                //    // Document have Drawing Code Matrix
                //    case 3:
                //        this.ExportDrawingCodeMatrixDetail(dmObj);
                //        break;
                //    // AU, CO, PLG, QIR, GTC, PO Matrix
                //    case 4:
                //        this.ExportAU_CO_PLGMatrixDetail(dmObj);
                //        break;
                //    // EL, ML Matrix
                //    case 5:
                //        this.ExportEL_MLMatrixDetail(dmObj);
                //        break;
                //    // PP Matrix
                //    case 6:
                //        this.ExportPPMatrixDetail(dmObj);
                //        break;
                //    // Vendor Document Matrix
                //    case 7:
                //        this.ExportVendorDocumentMatrixDetail(dmObj);
                //        break;
                //}
            }
        }

        private void ExportDistributionMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\PECC2_DistributionMatrix_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            
            var groupCodeList = this.groupCodeService.GetAll().Where(t => !string.IsNullOrEmpty(t.Code)).OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + groupCodeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            //matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["F3"].PutValue(dmObj.Name);


            for (int i = 0; i < groupCodeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(groupCodeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(groupCodeList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 4].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 5].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + groupCodeList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("E4", "E" + (4 + userList.Count));
            rangeDocTypeList.Name = "GroupCodeList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + groupCodeList.Count, 2, 2);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("GroupCode", typeof (String)),
                new DataColumn("GroupDiscription", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByGroupCodeList = dmDetailList.GroupBy(t => t.GroupCodeName);
            foreach (var dmDetailGroupByGroupCode in dmDetailGroupByGroupCodeList)
            {
                var dmDetailOfDisAndDocType = dmDetailGroupByGroupCode.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["GroupCode"] = dmDetailGroupByGroupCode.Key;
                dataRow["GroupDiscription"] = string.Empty;
                foreach (var dmDetail in dmDetailOfDisAndDocType)
                {
                    //dataRow["ID"] = dmDetail.ID;
                    if(dataRow["User_" + dmDetail.UserId.GetValueOrDefault()]!= null && !string.IsNullOrEmpty(dataRow["User_" + dmDetail.UserId.GetValueOrDefault()].ToString())){
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] +";"+ dmDetail.ActionTypeName;
                    }
                    else
                    {
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                    }
                   
                }

                dtFull.Rows.Add(dataRow);
            }

            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename =  dmObj.Name + "_Distribution Matrix.xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportVendorDocumentMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_DrawingCode00_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var materialCodeList = this.materialService.GetAll().OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + materialCodeList.Count * docTypeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["E3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < materialCodeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 4].PutValue(materialCodeList[i].Code);
                indexDataSheet.Cells[3 + i, 5].PutValue(materialCodeList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeMaterialCodeList = indexDataSheet.Cells.CreateRange("E4", "E" + (4 + materialCodeList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeMaterialCodeList.Name = "MaterialCodeList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + materialCodeList.Count * docTypeList.Count, 2, 2);
            this.CreateValidation(rangeMaterialCodeList.Name, validations, 6, 5 + materialCodeList.Count * docTypeList.Count, 3, 3);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("MaterialCode", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByMaterialCodeList = dmDetailList.GroupBy(t => t.MaterialCodeName);
            foreach (var dmDetailGroupByMaterialCode in dmDetailGroupByMaterialCodeList)
            {
                var dmDetailOfMaterialCodeList = dmDetailGroupByMaterialCode.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfMaterialCodeList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["MaterialCode"] = dmDetailGroupByMaterialCode.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }

            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Vendor Document" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportPPMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_EL-MLMatrix_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C10007" + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["E3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 10006, 2, 2);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("SerialNo", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDocTypeList = dmDetailList.GroupBy(t => t.DocTypeName);
            foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
            {
                var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["DocType"] = dmDetailGroupByDocType.Key;
                foreach (var dmDetail in dmDetailOfDisAndDocType)
                {
                    //dataRow["ID"] = dmDetail.ID;
                    dataRow["SerialNo"] = dmDetail.SerialNo;
                    dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                }

                dtFull.Rows.Add(dataRow);
            }


            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Document PP" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportEL_MLMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_EL-MLMatrix_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var unitList = this.unitService.GetAll().OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + unitList.Count * docTypeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["E3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < unitList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 4].PutValue(unitList[i].Code);
                indexDataSheet.Cells[3 + i, 5].PutValue(unitList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeUnitList = indexDataSheet.Cells.CreateRange("E4", "E" + (4 + unitList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeUnitList.Name = "UnitList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + unitList.Count * docTypeList.Count, 2, 2);
            this.CreateValidation(rangeUnitList.Name, validations, 6, 5 + unitList.Count * docTypeList.Count, 3, 3);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Unit", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByUnitList = dmDetailList.GroupBy(t => t.UnitCodeName);
            foreach (var dmDetailGroupByUnit in dmDetailGroupByUnitList)
            {
                var dmDetailOfUnitList = dmDetailGroupByUnit.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfUnitList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Unit"] = dmDetailGroupByUnit.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }

            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Document EL-ML" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportAU_CO_PLGMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_AU-CO-PLG-QIR-GTC-POMatrix_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + docTypeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["G2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["G3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + docTypeList.Count, 2, 2);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Empty", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDocTypeList = dmDetailList.GroupBy(t => t.DocTypeName);
            foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
            {
                var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["Empty"] = string.Empty;
                dataRow["DocType"] = dmDetailGroupByDocType.Key;
                foreach (var dmDetail in dmDetailOfDisAndDocType)
                {
                    //dataRow["ID"] = dmDetail.ID;
                    dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                }

                dtFull.Rows.Add(dataRow);
            }


            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Document AU-CO-PLG-QIR-GTC-PO" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportDrawingCodeMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_DrawingCode_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var disciplineList = this.disciplineService.GetAll().OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + disciplineList.Count * docTypeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["E3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < disciplineList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 4].PutValue(disciplineList[i].Code);
                indexDataSheet.Cells[3 + i, 5].PutValue(disciplineList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeDisciplineList = indexDataSheet.Cells.CreateRange("E4", "E" + (4 + disciplineList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeDisciplineList.Name = "DisciplineList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + disciplineList.Count * docTypeList.Count, 2, 2);
            this.CreateValidation(rangeDisciplineList.Name, validations, 6, 5 + disciplineList.Count * docTypeList.Count, 3, 3);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDisciplineList = dmDetailList.GroupBy(t => t.DisciplineName);
            foreach (var dmDetailGroupByDiscipline in dmDetailGroupByDisciplineList)
            {
                var dmDetailOfDisciplineList = dmDetailGroupByDiscipline.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfDisciplineList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Discipline"] = dmDetailGroupByDiscipline.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }

            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Document have Drawing Code" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportMaterialWorkCodeMatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_MaterialWorkCodeMatrix_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var disciplineList = this.disciplineService.GetAll().OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + disciplineList.Count * docTypeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["E3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < disciplineList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 4].PutValue(disciplineList[i].Code);
                indexDataSheet.Cells[3 + i, 5].PutValue(disciplineList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeDisciplineList = indexDataSheet.Cells.CreateRange("E4", "E" + (4 + disciplineList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeDisciplineList.Name = "DisciplineList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + disciplineList.Count * docTypeList.Count, 2, 2);
            this.CreateValidation(rangeDisciplineList.Name, validations, 6, 5 + disciplineList.Count * docTypeList.Count, 3, 3);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
            });

            
            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDisciplineList = dmDetailList.GroupBy(t => t.DisciplineName);
            foreach (var dmDetailGroupByDiscipline in dmDetailGroupByDisciplineList)
            {
                var dmDetailOfDisciplineList = dmDetailGroupByDiscipline.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfDisciplineList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Discipline"] = dmDetailGroupByDiscipline.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }

            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Document have Material-Work Code" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }

        private void ExportDrawingCode00MatrixDetail(DistributionMatrix dmObj)
        {
            var filePath = Server.MapPath("../../Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_DrawingCode00_Template.xlsm");

            var indexDataSheet = workbook.Worksheets[0];
            var matrixDataSheet = workbook.Worksheets[1];
            var docTypeList = this.docTypeService.GetAll().Where(t => !string.IsNullOrEmpty(t.DistributionMatrixTypeIds) && t.DistributionMatrixTypeIds.Split(';').Contains(dmObj.TypeId.ToString())).OrderBy(t => t.Code).ToList();
            var unitList = this.unitService.GetAll().OrderBy(t => t.Code).ToList();
            var userList = this.userService.GetAll().Where(t => t.Id != 1).OrderBy(t => t.Username).ToList();

            matrixDataSheet.Cells["A1"].PutValue(dmObj.ID);
            matrixDataSheet.Cells["A2"].PutValue(dmObj.TypeId);

            matrixDataSheet.Cells["A3"].Formula = "=COUNTIF(C7:C" + (7 + unitList.Count * docTypeList.Count) + ",\"*\")";
            matrixDataSheet.Cells["A4"].Formula = "=COUNTIF(E5:" + matrixDataSheet.Cells[4, 4 + userList.Count].Name + ",\"*\")";

            matrixDataSheet.Cells["E2"].PutValue(dmObj.TypeName);
            matrixDataSheet.Cells["E3"].PutValue(dmObj.Name);


            for (int i = 0; i < docTypeList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 1].PutValue(docTypeList[i].Code);
                indexDataSheet.Cells[3 + i, 2].PutValue(docTypeList[i].Description);
            }

            for (int i = 0; i < unitList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 4].PutValue(unitList[i].Code);
                indexDataSheet.Cells[3 + i, 5].PutValue(unitList[i].Description);
            }

            for (int i = 0; i < userList.Count; i++)
            {
                indexDataSheet.Cells[3 + i, 7].PutValue(userList[i].Username);
                indexDataSheet.Cells[3 + i, 8].PutValue(userList[i].FullName);
            }

            // Create range info
            var rangeDocTypeList = indexDataSheet.Cells.CreateRange("B4", "B" + (4 + docTypeList.Count));
            var rangeUnitList = indexDataSheet.Cells.CreateRange("E4", "E" + (4 + unitList.Count));
            var rangeUserList = indexDataSheet.Cells.CreateRange("H4", "H" + (4 + userList.Count));
            rangeDocTypeList.Name = "DocTypeList";
            rangeUnitList.Name = "UnitList";
            rangeUserList.Name = "UserList";

            var validations = matrixDataSheet.Validations;
            this.CreateValidation(rangeDocTypeList.Name, validations, 6, 5 + unitList.Count * docTypeList.Count, 2, 2);
            this.CreateValidation(rangeUnitList.Name, validations, 6, 5 + unitList.Count * docTypeList.Count, 3, 3);
            this.CreateValidation(rangeUserList.Name, validations, 4, 4, 4, 3 + userList.Count);
            // -------------------------------------------------------------------------------------------------------------------------------

            // Export exist data
            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Unit", typeof (String)),
            });


            for (int i = 0; i < dmUserList.Count; i++)
            {
                //matrixDataSheet.Cells[3, 4 + i].PutValue(userList[i].FullNameWithPosition);
                matrixDataSheet.Cells[4, 4 + i].PutValue(dmUserList[i].Username);

                var userColumn = new DataColumn("User_" + dmUserList[i].Id, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByUnitList = dmDetailList.GroupBy(t => t.UnitCodeName);
            foreach (var dmDetailGroupByUnit in dmDetailGroupByUnitList)
            {
                var dmDetailOfUnitList = dmDetailGroupByUnit.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfUnitList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Unit"] = dmDetailGroupByUnit.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow["User_" + dmDetail.UserId.GetValueOrDefault()] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }

            matrixDataSheet.Cells.ImportDataTable(dtFull, false, 6, 2, dtFull.Rows.Count, dtFull.Columns.Count, false);
            workbook.CalculateFormula();
            // -------------------------------------------------------------------------------------------------------------------------------

            // Download File
            var filename = "Document have Drawing Code-00" + "_" + dmObj.Name + ".xlsm";
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
            // -------------------------------------------------------------------------------------------------------------------------------
        }


        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        private void CreateValidation(string formular, ValidationCollection objValidations, int startRow, int endRow, int startColumn, int endColumn)
        {
            // Create a new validation to the validations list.
            Validation validation = objValidations[objValidations.Add()];

            // Set the validation type.
            validation.Type = Aspose.Cells.ValidationType.List;

            // Set the operator.
            validation.Operator = OperatorType.None;

            // Set the in cell drop down.
            validation.InCellDropDown = true;

            // Set the formula1.
            validation.Formula1 = "=" + formular;

            // Enable it to show error.
            validation.ShowError = true;

            // Set the alert type severity level.
            validation.AlertStyle = ValidationAlertType.Stop;

            // Set the error title.
            validation.ErrorTitle = "Error";

            // Set the error message.
            validation.ErrorMessage = "Please select item from the list";

            // Specify the validation area.
            CellArea area;
            area.StartRow = startRow;
            area.EndRow = endRow;
            area.StartColumn = startColumn;
            area.EndColumn = endColumn;

            // Add the validation area.
            validation.AreaList.Add(area);

            ////return validation;
        }

        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Grid KhacHang item created
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (Image)e.Item.FindControl("EditLink");
                editLink.Attributes["href"] = "#";
                editLink.Attributes["onclick"] = string.Format(
                    "return ShowEditForm('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
        }

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var disId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            this.distributionMatrixService.Delete(disId);

            this.grdDocument.Rebind();
        }

        private void LoadSystemPanel()
        {
            var systemId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("SystemID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), systemId);
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "SYSTEM" });

                this.radPbSystem.DataSource = permissions;
                this.radPbSystem.DataFieldParentID = "ParentId";
                this.radPbSystem.DataFieldID = "Id";
                this.radPbSystem.DataValueField = "Id";
                this.radPbSystem.DataTextField = "MenuName";
                this.radPbSystem.DataBind();
                this.radPbSystem.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbSystem.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                }
            }
        }

        private void LoadScopePanel()
        {
            var listId = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("ScopeID"));
            var permissions = this.permissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault(), listId).OrderBy(t => t.Menu.Priority).ToList();
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    permission.ParentId = -1;
                    permission.MenuName = permission.Menu.Description;
                }

                permissions.Insert(0, new Permission() { Id = -1, MenuName = "CONFIG MANAGEMENT" });

                this.radPbScope.DataSource = permissions;
                this.radPbScope.DataFieldParentID = "ParentId";
                this.radPbScope.DataFieldID = "Id";
                this.radPbScope.DataValueField = "Id";
                this.radPbScope.DataTextField = "MenuName";
                this.radPbScope.DataBind();
                this.radPbScope.Items[0].Expanded = true;

                foreach (RadPanelItem item in this.radPbScope.Items[0].Items)
                {
                    item.ImageUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Icon;
                    item.NavigateUrl = permissions.FirstOrDefault(t => t.Id == Convert.ToInt32(item.Value)).Menu.Url;
                    if (item.Text == "Distribution Matrix")
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ExportDMDetail")
            {
                
            }
        }
    }
}

