// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Drawing;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Scope;
using Telerik.Web.UI.Calendar;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using Aspose.Cells;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;


    using Telerik.Web.UI;
    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class GeneralReport : Page
    {
        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();


        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

        private readonly AttachFilesPackageService attachfilePackagesevice= new AttachFilesPackageService();

        private readonly ToDoListService toDoListService = new ToDoListService();

        private readonly RevisionService revisionService = new RevisionService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

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
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            Session.Add("SelectedMainMenu", 3);

            if (!Page.IsPostBack)
            {
                var monday = DateTime.Today;
                int valueday = monday.DayOfWeek - DayOfWeek.Monday;
                monday = monday.AddDays(-valueday);
                var sunday = monday.AddDays(6);
                this.txtFromDate.SelectedDate = monday;
                this.txtToDate.SelectedDate = sunday;

                this.txtday.Value = 2;

                this.LoadObjectTree();
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
         
           if (e.Argument == "ExportReport")
            {
               
            }
        }


      
        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "Images/folderdir16.png";
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

        private void LoadObjectTree()
        {
            var projectInPermission = this.scopeProjectService.GetAll().OrderBy(t => t.Name).ToList();

            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();
            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();
                var listDisciplineInPermission = this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList();

                this.rtvDiscipline.DataSource = listDisciplineInPermission;
                this.rtvDiscipline.DataTextField = "FullName";
                this.rtvDiscipline.DataValueField = "ID";
                this.rtvDiscipline.DataFieldID = "ID";
                this.rtvDiscipline.DataBind();

                //this.InitGridContextMenu(projectId);
            }
        }

        /// <summary>
        /// The repair list.
        /// </summary>
        /// <param name="listOptionalTypeDetail">
        /// The list optional type detail.
        /// </param>
        private void RepairList(ref List<OptionalTypeDetail> listOptionalTypeDetail)
        {
            var temp = listOptionalTypeDetail.Where(t => t.ParentId != null).Select(t => t.ParentId).Distinct().ToList();
            var temp2 = listOptionalTypeDetail.Select(t => t.ID).ToList();
            var tempList = new List<OptionalTypeDetail>();
            foreach (var x in temp)
            {
                if (!temp2.Contains(x.Value))
                {
                    tempList.AddRange(listOptionalTypeDetail.Where(t => t.ParentId == x.Value).ToList());
                }
            }

            var listOptionalType = tempList.Where(t => t.OptionalTypeId != null).Select(t => t.OptionalTypeId).Distinct().ToList();

            foreach (var optionalTypeId in listOptionalType)
            {
                var optionalType = this.optionalTypeService.GetById(optionalTypeId.Value);
                var tempOptTypeDetail = new OptionalTypeDetail() { ID = optionalType.ID * 9898, Name = optionalType.Name + "s" };
                listOptionalTypeDetail.Add(tempOptTypeDetail);
                ////tempList.Add(tempOptTypeDetail);
                OptionalType type = optionalType;
                foreach (var optionalTypeDetail in tempList.Where(t => t.OptionalTypeId == type.ID).ToList())
                {
                    optionalTypeDetail.ParentId = tempOptTypeDetail.ID;
                }
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ////this.CustomerMenu.Items[2].Visible = false;
            int projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.CustomerMenu.Items[3].Visible = false;

            var listDisciplineInPermission = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()
                ? this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList()
                : this.disciplineService.GetAllDisciplineInPermission(UserSession.Current.User.Id, !string.IsNullOrEmpty(this.ddlProject.SelectedValue) ? projectId : 0)
                .OrderBy(t => t.Name).ToList();

            this.rtvDiscipline.DataSource = listDisciplineInPermission;
            this.rtvDiscipline.DataTextField = "Name";
            this.rtvDiscipline.DataValueField = "ID";
            this.rtvDiscipline.DataFieldID = "ID";
            this.rtvDiscipline.DataBind();
           


        }

       

        protected void rtvDiscipline_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
           
        }

        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {
                var filterItem = (GridFilteringItem)e.Item;
                var selectedProperty = new List<string>();

                var ddlFilterRev = (RadComboBox)filterItem.FindControl("ddlFilterRev");
            }
        }

        protected DateTime? SetPublishDate(GridItem item)
        {
            if (item.OwnerTableView.GetColumn("Index27").CurrentFilterValue == string.Empty)
            {
                return new DateTime?();
            }
            else
            {
                return DateTime.Parse(item.OwnerTableView.GetColumn("Index27").CurrentFilterValue);
            }
        }

        /// <summary>
        /// The bind tree view combobox.
        /// </summary>
        /// <param name="optionalType">
        /// The optional type.
        /// </param>
        /// <param name="ddlObj">
        /// The ddl obj.
        /// </param>
        /// <param name="rtvName">
        /// The rtv name.
        /// </param>
        /// <param name="listOptionalTypeDetailFull">
        /// The list optional type detail full.
        /// </param>
        private void BindTreeViewCombobox(int optionalType, RadComboBox ddlObj, string rtvName, IEnumerable<OptionalTypeDetail> listOptionalTypeDetailFull)
        {
            var rtvobj = (RadTreeView)ddlObj.Items[0].FindControl(rtvName);
            if (rtvobj != null)
            {
                var listOptionalTypeDetail = listOptionalTypeDetailFull.Where(t => t.OptionalTypeId == optionalType).ToList();
                this.RepairList(ref listOptionalTypeDetail);

                rtvobj.DataSource = listOptionalTypeDetail;
                rtvobj.DataFieldParentID = "ParentId";
                rtvobj.DataTextField = "Name";
                rtvobj.DataValueField = "ID";
                rtvobj.DataFieldID = "ID";
                rtvobj.DataBind();
            }
        }

        protected void rtvDiscipline_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = @"Images/discipline.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
          //  var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtFromDate");
           // var txtToDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtToDate");

            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\NPK_DocumentsIssueReport.xls");

            var SummerSheet = workbook.Worksheets[0];
            var dataSheet = workbook.Worksheets[1];
            
            var projectName = this.ddlProject.SelectedItem != null
                ? this.ddlProject.SelectedItem.Text.Replace("&", "-")
                : string.Empty;
            var dtFull = new DataTable();
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);

            var filename = projectName.Split('-')[0].Trim() + "_" + "Documents Issued_FromDate " + (txtFromDate.SelectedDate!= null ? txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd-MM-yy") : string.Empty) + " - ToDate " + (txtToDate.SelectedDate != null?txtToDate.SelectedDate.GetValueOrDefault().ToString("dd-MM-yy"):string.Empty) + ".xls";

            var fileDocID=this.attachfilePackagesevice.GetAll().Where(t=> (txtFromDate.SelectedDate != null && t.CreatedDate.Value >= txtFromDate.SelectedDate.Value)
                                    && (txtToDate.SelectedDate != null && t.CreatedDate.Value < txtToDate.SelectedDate.Value.AddDays(1)) ).Select(t=> t.DocumentPackageID).Distinct().ToList();

            var docList = this.documentPackageService.GetAllByProject(projectId, false).Where(t => !t.IsInternalDocument.GetValueOrDefault()
                                && t.HasAttachFile    && fileDocID.Contains(t.ID) ).ToList();

            var AllDocumentOfProject=this.documentPackageService.GetAllByProject(projectId,false);

            if (this.rtvDiscipline.SelectedNode != null)
            {
                docList = docList.Where(t => t.DisciplineId == Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value)).ToList();
                filename = Utility.RemoveSpecialCharacterFileName(this.rtvDiscipline.SelectedNode.Text) + "(" + projectName.Split('-')[0].Trim() + ")" + "_" + "Documents Issued_FromDate " + (txtFromDate.SelectedDate != null ? txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd-MM-yy") : string.Empty) + " - ToDate " + (txtToDate.SelectedDate != null ? txtToDate.SelectedDate.GetValueOrDefault().ToString("dd-MM-yy") : string.Empty) + ".xls";
            }

            var rev=this.revisionService.GetAllByProject(projectId).OrderBy(t=>t.Name).ToList();

            SummerSheet.Cells["C1"].PutValue(projectName);
            SummerSheet.Cells["G4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            SummerSheet.Cells.InsertColumns(4, rev.Count() > 0 ? rev.Count() - 1 : 0);
            var count=0;
            foreach (var columns in rev)
            {
                SummerSheet.Cells[5, 3 + count].PutValue("Rev." + columns.Name);
                count++;
            }

            dataSheet.Cells["E1"].PutValue(this.ddlProject.Text);
            dataSheet.Cells["U4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            if (txtFromDate.SelectedDate != null)
            {
                dataSheet.Cells["G3"].PutValue(txtFromDate.SelectedDate.Value);
            }

            if (txtToDate.SelectedDate != null)
            {
                dataSheet.Cells["G4"].PutValue(txtToDate.SelectedDate.Value);
            }

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("DocNo", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("RevName", typeof (String)),
                new DataColumn("StatusName", typeof (String)),
                new DataColumn("ProjectCode", typeof (String)),
                new DataColumn("Originator", typeof (String)),
                new DataColumn("DisciplineCode", typeof (String)),
                new DataColumn("DocTypeCode", typeof (String)),
                new DataColumn("SequentialNo", typeof (String)),
                new DataColumn("DrawingSheetNo", typeof (String)),
                new DataColumn("FirstIssuePlan", typeof (DateTime)),
                new DataColumn("FirstIssueActual", typeof (DateTime)),
                new DataColumn("FirstIssueTransNo", typeof (String)),
                new DataColumn("FinalIssuePlan", typeof (DateTime)),
                new DataColumn("FinalIssueActual", typeof (DateTime)),
                new DataColumn("FinalIssueTransNo", typeof (String)),
                new DataColumn("Complete", typeof (double)),
                new DataColumn("Weight", typeof (double)),
            });

            int i = 1;
            var docGroupByDisciplineList = docList.GroupBy(t => t.DisciplineFullName);
            
            SummerSheet.Cells.InsertRows(7, docGroupByDisciplineList.Count()-1);

            foreach (var docGroupByDiscipline in docGroupByDisciplineList)
            {
                var disciplineRowCount = 1;
                var docListOfDiscipline = docGroupByDiscipline.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["DocNo"] = docGroupByDiscipline.Key;
                dtFull.Rows.Add(dataRow);

                SummerSheet.Cells["A" + (6 + i)].PutValue(i);
                SummerSheet.Cells["B" + (6 + i)].PutValue(docGroupByDiscipline.Key);

                foreach (var docObj in docListOfDiscipline)
                {
                    dataRow = dtFull.NewRow();
                    dataRow["DocId"] = docObj.ID;
                    dataRow["NoIndex"] = disciplineRowCount;
                    dataRow["DocNo"] = docObj.DocNo;
                    dataRow["DocTitle"] = docObj.DocTitle;
                    dataRow["RevName"] = docObj.RevisionName;
                    dataRow["StatusName"] = docObj.StatusName;
                    dataRow["ProjectCode"] = docObj.ProjectName;
                    dataRow["Originator"] = docObj.OriginatorName;
                    dataRow["DisciplineCode"] = docObj.DisciplineName;
                    dataRow["DocTypeCode"] = docObj.DocumentTypeName;
                    dataRow["SequentialNo"] = docObj.SequencetialNumber;
                    dataRow["DrawingSheetNo"] = docObj.DrawingSheetNumber;
                    if (docObj.FirstIssuePlanDate != null)
                    {
                        dataRow["FirstIssuePlan"] = docObj.FirstIssuePlanDate;
                    }

                    if (docObj.FirstIssueActualDate != null)
                    {
                        dataRow["FirstIssueActual"] = docObj.FirstIssueActualDate;
                    }
                    dataRow["FirstIssueTransNo"] = docObj.FirstIssueTransNo;
                    if (docObj.FinalIssuePlanDate != null)
                    {
                        dataRow["FinalIssuePlan"] = docObj.FinalIssuePlanDate;
                    }
                    if (docObj.FinalIssueActualDate != null)
                    {
                        dataRow["FinalIssueActual"] = docObj.FinalIssueActualDate;
                    }
                    dataRow["FinalIssueTransNo"] = docObj.FinalIssueTransNo;

                    dataRow["Complete"] = docObj.Complete != null ? docObj.Complete / 100 : 0;

                    dataRow["Weight"] = docObj.Weight != null ? docObj.Weight / 100 : 0;

                    disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
                count = 0;
                var tong=0; var hieu = 0;
                tong = docListOfDiscipline.Count();
                foreach (var column in rev)
                {
                    if (column.IsFirst.GetValueOrDefault())
                    {
                        SummerSheet.Cells[5 + i, 3 + count].PutValue(tong);
                        hieu = docListOfDiscipline.Count(t => t.RevisionId == column.ID);
                    }
                    else{
                        if (docListOfDiscipline.Count(t => t.RevisionId == column.ID) != 0)
                        {
                            SummerSheet.Cells[5 + i, 3 + count].PutValue(tong - hieu);
                            tong -= hieu;
                            hieu = docListOfDiscipline.Count(t => t.RevisionId == column.ID);
                        }
                        else
                        {
                            SummerSheet.Cells[5 + i, 3 + count].PutValue(0);
                        }
                    }
                  
                    count++;
                }
                SummerSheet.Cells[5 + i, 2].PutValue(AllDocumentOfProject.Where(t => t.DisciplineFullName == docGroupByDiscipline.Key).Count());

                i++;
            }

            count = 0;
            foreach (var column in rev)
            {
                var sum = 0;
                for (int j = 1; j < i; j++)
                {
                    sum += Convert.ToInt32(SummerSheet.Cells[5 + j, 3 + count].Value);
                }
                SummerSheet.Cells[5 + i, 3 + count].PutValue(sum);
                count++;
            }
            SummerSheet.Cells[5 + i, 2].PutValue(AllDocumentOfProject.Count);

            dataSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);
            dataSheet.Cells["A1"].PutValue(projectId);
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);
        }

        protected void btnExportDocReleased_Click(object sender, EventArgs e)
        {
            var days = DateTime.Now.AddDays(this.txtday.Value != null ? (int)this.txtday.Value : 2);

            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\NPK_DocumentsReparedtoReleased.xls");

            var dataSheet = workbook.Worksheets[1];

            var projectName = this.ddlProject.SelectedItem != null
                ? this.ddlProject.SelectedItem.Text.Replace("&", "-")
                : string.Empty;
            var dtFull = new DataTable();
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);

            var filename = projectName.Split('-')[0].Trim() + "_" + "Documents Reparing to Release " +  days.ToString("dd-MM-yy") + ".xls";
            var docList = this.documentPackageService.GetAllByProject(projectId, false).Where(t => !t.IsInternalDocument.GetValueOrDefault()
                                && !t.HasAttachFile
                                && ( (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value.Date == days.Date)||(t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value.Date==days.Date))
                                ).ToList();

            if (this.rtvDiscipline.SelectedNode != null)
            {
                docList = docList.Where(t => t.DisciplineId == Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value)).ToList();
                filename = Utility.RemoveSpecialCharacterFileName(this.rtvDiscipline.SelectedNode.Text) + "(" + projectName.Split('-')[0].Trim() + ")" + "_" + "Documents Reparing to Release " + days.ToString("dd-MM-yy") + ".xls";
            }

            dataSheet.Cells["E1"].PutValue(this.ddlProject.Text);
            dataSheet.Cells["U4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            //if (txtFromDate.SelectedDate != null)
            //{
            //    dataSheet.Cells["G3"].PutValue(txtFromDate.SelectedDate.Value);
            //}

           // if (txtToDate.SelectedDate != null)
           // {
                dataSheet.Cells["F4"].PutValue(days);
           // }

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("DocNo", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("RevName", typeof (String)),
                new DataColumn("StatusName", typeof (String)),
                new DataColumn("ProjectCode", typeof (String)),
                new DataColumn("Originator", typeof (String)),
                new DataColumn("DisciplineCode", typeof (String)),
                new DataColumn("DocTypeCode", typeof (String)),
                new DataColumn("SequentialNo", typeof (String)),
                new DataColumn("DrawingSheetNo", typeof (String)),
                new DataColumn("FirstIssuePlan", typeof (DateTime)),
                new DataColumn("FirstIssueActual", typeof (DateTime)),
                new DataColumn("FirstIssueTransNo", typeof (String)),
                new DataColumn("FinalIssuePlan", typeof (DateTime)),
                new DataColumn("FinalIssueActual", typeof (DateTime)),
                new DataColumn("FinalIssueTransNo", typeof (String)),
                new DataColumn("Complete", typeof (double)),
                new DataColumn("Weight", typeof (double)),
            });


            var docGroupByDisciplineList = docList.GroupBy(t => t.DisciplineFullName);
            foreach (var docGroupByDiscipline in docGroupByDisciplineList)
            {
                var disciplineRowCount = 1;
                var docListOfDiscipline = docGroupByDiscipline.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["DocNo"] = docGroupByDiscipline.Key;
                dtFull.Rows.Add(dataRow);


                foreach (var docObj in docListOfDiscipline)
                {
                    dataRow = dtFull.NewRow();
                    dataRow["DocId"] = docObj.ID;
                    dataRow["NoIndex"] = disciplineRowCount;
                    dataRow["DocNo"] = docObj.DocNo;
                    dataRow["DocTitle"] = docObj.DocTitle;
                    dataRow["RevName"] = docObj.RevisionName;
                    dataRow["StatusName"] = docObj.StatusName;
                    dataRow["ProjectCode"] = docObj.ProjectName;
                    dataRow["Originator"] = docObj.OriginatorName;
                    dataRow["DisciplineCode"] = docObj.DisciplineName;
                    dataRow["DocTypeCode"] = docObj.DocumentTypeName;
                    dataRow["SequentialNo"] = docObj.SequencetialNumber;
                    dataRow["DrawingSheetNo"] = docObj.DrawingSheetNumber;
                    if (docObj.FirstIssuePlanDate != null)
                    {
                        dataRow["FirstIssuePlan"] = docObj.FirstIssuePlanDate;
                    }

                    if (docObj.FirstIssueActualDate != null)
                    {
                        dataRow["FirstIssueActual"] = docObj.FirstIssueActualDate;
                    }
                    dataRow["FirstIssueTransNo"] = docObj.FirstIssueTransNo;
                    if (docObj.FinalIssuePlanDate != null)
                    {
                        dataRow["FinalIssuePlan"] = docObj.FinalIssuePlanDate;
                    }
                    if (docObj.FinalIssueActualDate != null)
                    {
                        dataRow["FinalIssueActual"] = docObj.FinalIssueActualDate;
                    }
                    dataRow["FinalIssueTransNo"] = docObj.FinalIssueTransNo;

                    dataRow["Complete"] = docObj.Complete != null ? docObj.Complete / 100 : 0;

                    dataRow["Weight"] = docObj.Weight != null ? docObj.Weight / 100 : 0;

                    disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
            }
            dataSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);
            dataSheet.Cells["A1"].PutValue(projectId);
            workbook.Save(filePath + filename);
            this.Download_File(filePath + filename);

        }

      
    }
}