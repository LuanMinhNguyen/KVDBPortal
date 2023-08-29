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

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Data;
    using System.Web.UI;

    using Aspose.Cells;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ImportDistributionMatrix : Page
    {

        private readonly DocumentTypeService documentTypeService;

        private readonly DisciplineService disciplineService;

        private readonly UnitService unitService;

        private readonly DocumentService documentService;

        private readonly DistributionMatrixDetailService dmDetailService;

        private readonly DistributionMatrixService dmService;

        private readonly UserService userService;

        private readonly MaterialService materialService;

        private readonly GroupCodeService groupCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ImportDistributionMatrix()
        {
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.documentService = new DocumentService();

            this.dmDetailService = new DistributionMatrixDetailService();
            this.dmService = new DistributionMatrixService();
            this.userService = new UserService();
            this.unitService = new UnitService();
            this.materialService = new MaterialService();
            this.groupCodeService = new GroupCodeService();
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
                        var dmSheet = workbook.Worksheets[1];
                        var rowNumber = Convert.ToInt32(dmSheet.Cells["A3"].Value);
                        var userNumber = Convert.ToInt32(dmSheet.Cells["A4"].Value);

                        if (!string.IsNullOrEmpty(this.Request.QueryString["dmId"]))
                        {
                            var dmId = Convert.ToInt32(this.Request.QueryString["dmId"]);
                            var dmObj = this.dmService.GetById(dmId);
                            if (dmObj != null)
                            {
                                this.ImportMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);

                                //switch (dmObj.TypeId)
                                //{
                                //    // Document have Material/Work Code
                                //    case 1:
                                //        this.ImportMaterialWorkCodeMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //    // Document have Drawing Code (00) Matrix
                                //    case 2:
                                //        this.ImportDrawingCode00MatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //    // Document have Drawing Code Matrix
                                //    case 3:
                                //        this.ImportDrawingCodeMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //    // AU, CO, PLG, QIR, GTC, PO Matrix
                                //    case 4:
                                //        this.ImportAU_CO_PLGMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //    // EL, ML Matrix
                                //    case 5:
                                //        this.ImportEL_MLMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //    // PP Matrix
                                //    case 6:
                                //        this.ImportPPMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //    // Vendor Document Matrix
                                //    case 7:
                                //        this.ImportVendorDocumentMatrixDetail(dmSheet, rowNumber, userNumber, dmObj);
                                //        break;
                                //}
                            }
                        }

                        if (string.IsNullOrEmpty(this.lblError.Text))
                        {
                            this.blockError.Visible = true;
                            this.lblMessTitle.Text = "Information: ";
                            this.lblError.Text =
                                "Data of Document Distribution Matrix file is valid. System import successfull!";
                        }
                        else
                        {
                            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey",
                                "CloseAndRebind();", true);
                        }
                    }
                }
           }
           catch (Exception ex)
           {
               this.blockError.Visible = true;
                this.lblMessTitle.Text = "Warning: ";
                this.lblError.Text = "Have error: '" + ex.Message + "'";
           }
        }

        private void ImportMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var groupCode = dmDetailRow["Column1"].ToString();
                var groupCodeObj = this.groupCodeService.GetByCode(groupCode);
                if (groupCodeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString().Trim();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var listUserDm = dmDetailList.Where(t => t.GroupCodeId == groupCodeObj.ID
                                                                        && t.UserId == userObj.Id);
                            foreach(var item in listUserDm)
                            {
                                this.dmDetailService.Delete(item);
                            }
                            //if (dmDetailObj != null)
                            //{
                            //    if (!string.IsNullOrEmpty(actionType))
                            //    {
                            //        dmDetailObj.GroupCodeId = groupCodeObj.ID;
                            //        dmDetailObj.GroupCodeName = groupCodeObj.Code;
                            //        dmDetailObj.GroupCodeFullName = groupCodeObj.FullName;
                            //        dmDetailObj.UserId = userObj.Id;
                            //        dmDetailObj.UserName = userObj.UserNameWithFullName;


                            //        switch (actionType.ToUpper())
                            //        {
                            //            case "I":
                            //                dmDetailObj.ActionTypeId = 1;
                            //                dmDetailObj.ActionTypeFullName = "I - For Information";
                            //                dmDetailObj.ActionTypeName = "I";
                            //                break;
                            //            case "R":
                            //                dmDetailObj.ActionTypeId = 2;
                            //                dmDetailObj.ActionTypeFullName = "R - Review";
                            //                dmDetailObj.ActionTypeName = "R";
                            //                break;
                            //            case "C":
                            //                dmDetailObj.ActionTypeId = 3;
                            //                dmDetailObj.ActionTypeFullName = "C - Consolidate";
                            //                dmDetailObj.ActionTypeName = "C";
                            //                break;
                            //            case "A":
                            //                dmDetailObj.ActionTypeId = 4;
                            //                dmDetailObj.ActionTypeFullName = "A - Approve";
                            //                dmDetailObj.ActionTypeName = "A";
                            //                break;
                            //        }

                            //        this.dmDetailService.Update(dmDetailObj);
                            //    }
                            //    else
                            //    {
                            //        this.dmDetailService.Delete(dmDetailObj);
                            //    }
                            //}
                            //else
                            //{
                            DistributionMatrixDetail dmDetailObj;
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                var listAction = actionType.Split(';').ToList();
                                foreach(var action in listAction)
                                {

                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        GroupCodeId = groupCodeObj.ID,
                                        GroupCodeName = groupCodeObj.Code,
                                        GroupCodeFullName = groupCodeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (action.Trim().ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "C - Consolidate";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportVendorDocumentMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var materialCode = dmDetailRow["Column2"].ToString();
                var materialCodeObj = this.materialService.GetByCode(materialCode);

                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (materialCodeObj != null && docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.MaterialCodeId == materialCodeObj.ID
                                                                        && t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.MaterialCodeId = materialCodeObj.ID;
                                    dmDetailObj.MaterialCodeName = materialCodeObj.Code;
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        MaterialCodeId = materialCodeObj.ID,
                                        MaterialCodeName = materialCodeObj.Code,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportPPMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var serialNo = dmDetailRow["Column2"].ToString();

                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (!string.IsNullOrEmpty(serialNo) && docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.SerialNo == serialNo
                                                                        && t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.SerialNo = serialNo;
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        SerialNo = serialNo,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportEL_MLMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var unitCode = dmDetailRow["Column2"].ToString();
                var unitObj = this.unitService.GetByCode(unitCode);

                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (unitObj != null && docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.UnitCodeId == unitObj.ID
                                                                        && t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.UnitCodeId = unitObj.ID;
                                    dmDetailObj.UnitCodeName = unitObj.Code;
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        UnitCodeId = unitObj.ID,
                                        UnitCodeName = unitObj.Code,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportAU_CO_PLGMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportDrawingCodeMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var disciplineCode = dmDetailRow["Column2"].ToString();
                var disciplineObj = this.disciplineService.GetByCode(disciplineCode);

                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (disciplineObj != null && docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.DisciplineId == disciplineObj.ID
                                                                        && t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.DisciplineId = disciplineObj.ID;
                                    dmDetailObj.DisciplineName = disciplineObj.Code;
                                    dmDetailObj.DisciplineFullName = disciplineObj.FullName;
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        DisciplineId = disciplineObj.ID,
                                        DisciplineName = disciplineObj.Code,
                                        DisciplineFullName = disciplineObj.FullName,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportDrawingCode00MatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var unitCode = dmDetailRow["Column2"].ToString();
                var unitObj = this.unitService.GetByCode(unitCode);

                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (unitObj != null && docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.UnitCodeId == unitObj.ID
                                                                        && t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.UnitCodeId = unitObj.ID;
                                    dmDetailObj.UnitCodeName = unitObj.Code;
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        UnitCodeId = unitObj.ID,
                                        UnitCodeName = unitObj.Code,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportMaterialWorkCodeMatrixDetail(Worksheet dmSheet, int rowNumber, int userNumber, DistributionMatrix dmObj)
        {
            // Get data from Distribution Matrix File
            var dmDetailTable = dmSheet.Cells.ExportDataTable(6, 2, rowNumber, 2 + userNumber);
            var userTable = dmSheet.Cells.ExportDataTable(4, 4, 1, userNumber);
            // ---------------------------------------------------------------------------------

            var dmDetailList = this.dmDetailService.GetAllByDM(dmObj.ID);

            foreach (DataRow dmDetailRow in dmDetailTable.Rows)
            {
                var disciplineCode = dmDetailRow["Column2"].ToString();
                var disciplineObj = this.disciplineService.GetByCode(disciplineCode);

                var docTypeCode = dmDetailRow["Column1"].ToString();
                var docTypeObj = this.documentTypeService.GetByCode(docTypeCode);
                if (disciplineObj != null && docTypeObj != null)
                {
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        var actionType = dmDetailRow["Column" + (3 + i)].ToString();
                        var userName = userTable.Rows[0][i].ToString();
                        var userObj = this.userService.GetUserByUsername(userName);
                        if (userObj != null)
                        {
                            var dmDetailObj = dmDetailList.FirstOrDefault(t => t.DisciplineId == disciplineObj.ID
                                                                        && t.DocTypeId == docTypeObj.ID
                                                                        && t.UserId == userObj.Id);
                            if (dmDetailObj != null)
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj.DisciplineId = disciplineObj.ID;
                                    dmDetailObj.DisciplineName = disciplineObj.Code;
                                    dmDetailObj.DisciplineFullName = disciplineObj.FullName;
                                    dmDetailObj.DocTypeId = docTypeObj.ID;
                                    dmDetailObj.DocTypeName = docTypeObj.Code;
                                    dmDetailObj.DocTypeFullName = docTypeObj.FullName;
                                    dmDetailObj.UserId = userObj.Id;
                                    dmDetailObj.UserName = userObj.UserNameWithFullName;
                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Update(dmDetailObj);
                                }
                                else
                                {
                                    this.dmDetailService.Delete(dmDetailObj);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(actionType))
                                {
                                    dmDetailObj = new DistributionMatrixDetail()
                                    {
                                        DistributionMatrixId = dmObj.ID,
                                        DistributionMatrixName = dmObj.FullName,
                                        DisciplineId = disciplineObj.ID,
                                        DisciplineName = disciplineObj.Code,
                                        DisciplineFullName = disciplineObj.FullName,
                                        DocTypeId = docTypeObj.ID,
                                        DocTypeName = docTypeObj.Code,
                                        DocTypeFullName = docTypeObj.FullName,
                                        UserId = userObj.Id,
                                        UserName = userObj.UserNameWithFullName,
                                    };

                                    switch (actionType.ToUpper())
                                    {
                                        case "I":
                                            dmDetailObj.ActionTypeId = 1;
                                            dmDetailObj.ActionTypeFullName = "I - For Information";
                                            dmDetailObj.ActionTypeName = "I";
                                            break;
                                        case "C":
                                            dmDetailObj.ActionTypeId = 2;
                                            dmDetailObj.ActionTypeFullName = "C - Comment";
                                            dmDetailObj.ActionTypeName = "C";
                                            break;
                                        case "R":
                                            dmDetailObj.ActionTypeId = 3;
                                            dmDetailObj.ActionTypeFullName = "R - Review";
                                            dmDetailObj.ActionTypeName = "R";
                                            break;
                                        case "A":
                                            dmDetailObj.ActionTypeId = 4;
                                            dmDetailObj.ActionTypeFullName = "A - Approve";
                                            dmDetailObj.ActionTypeName = "A";
                                            break;
                                        case "M":
                                            dmDetailObj.ActionTypeId = 5;
                                            dmDetailObj.ActionTypeFullName = "M - Management";
                                            dmDetailObj.ActionTypeName = "M";
                                            break;
                                    }

                                    this.dmDetailService.Insert(dmDetailObj);
                                }
                            }
                        }
                    }
                }
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