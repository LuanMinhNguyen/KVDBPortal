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
using EDMs.Business.Services.Scope;
using Telerik.Web.UI.Calendar;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
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
    public partial class ContractorIssueReport : Page
    {
        private readonly OptionalTypeService optionalTypeService = new OptionalTypeService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();


        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();

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
            Session.Add("SelectedMainMenu", "Report Management");

            if (!Page.IsPostBack)
            {
                var txtToDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtToDate");
                txtToDate.SelectedDate = DateTime.Now;
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
            if (e.Argument == "Rebind")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ExportReport")
            {
                this.ExportContractorIssueStatus();
            }
        }

        private void ExportContractorIssueStatus()
        {
            var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtFromDate");
            var txtToDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtToDate");

            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\NPK_ContractorIssueReport.xlsm");

            var dataSheet = workbook.Worksheets[0];
            
            var projectName = this.ddlProject.SelectedItem != null
                ? this.ddlProject.SelectedItem.Text.Replace("&", "-")
                : string.Empty;
            var dtFull = new DataTable();
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);

            var filename = projectName.Split('-')[0].Trim() + "_" + "Contractor Over Due Issue_FromDate " + (txtFromDate.SelectedDate?.ToString("dd-MM-yy") ?? string.Empty) + " - ToDate " + (txtToDate.SelectedDate?.ToString("dd-MM-yy") ?? string.Empty) + ".xlsm";
            var docList = this.documentPackageService.GetAllByProject(projectId, false).Where(t => !t.IsInternalDocument.GetValueOrDefault()
                                && !t.HasAttachFile
                                && (
                                    ((txtFromDate.SelectedDate == null || (t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value >= txtFromDate.SelectedDate.Value))
                                    && (txtToDate.SelectedDate == null || (t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value < txtToDate.SelectedDate.Value.AddDays(1))))
                                    ||
                                    ((txtFromDate.SelectedDate == null || (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value >= txtFromDate.SelectedDate.Value))
                                    && (txtToDate.SelectedDate == null || (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value < txtToDate.SelectedDate.Value.AddDays(1))))
                                    )
                                ).ToList();

            if (this.rtvDiscipline.SelectedNode != null)
            {
                docList = docList.Where(t => t.DisciplineId == Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value)).ToList();
                filename = Utility.RemoveSpecialCharacterFileName(this.rtvDiscipline.SelectedNode.Text) + "(" + projectName.Split('-')[0].Trim() + ")" + "_" + "Contractor Over Due Issue_FromDate " + (txtFromDate.SelectedDate?.ToString("dd-MM-yy") ?? string.Empty) + " - ToDate " + (txtToDate.SelectedDate?.ToString("dd-MM-yy") ?? string.Empty) + ".xlsm";
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
                    if (docObj.Complete != null)
                    {
                        dataRow["Complete"] = docObj.Complete;
                    }

                    if (docObj.Weight != null)
                    {
                        dataRow["Weight"] = docObj.Weight;
                    }

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
            var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtFromDate");
            var txtToDate = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtToDate");

            var projectId = this.ddlProject.SelectedItem != null ? Convert.ToInt32(this.ddlProject.SelectedValue) : 0;
            var docList = new List<DocumentPackage>();
            if (this.rtvDiscipline.SelectedNode != null)
            {
                docList = this.documentPackageService.GetAllByDiscipline(Convert.ToInt32(this.rtvDiscipline.SelectedNode.Value), false)
                        .Where(t => !t.IsInternalDocument.GetValueOrDefault()
                                && !t.HasAttachFile 
                                && (
                                    ((txtFromDate.SelectedDate == null || (t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value >= txtFromDate.SelectedDate.Value))
                                    && (txtToDate.SelectedDate == null || (t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value < txtToDate.SelectedDate.Value.AddDays(1))))
                                    ||
                                    ((txtFromDate.SelectedDate == null || (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value >= txtFromDate.SelectedDate.Value))
                                    && (txtToDate.SelectedDate == null || (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value < txtToDate.SelectedDate.Value.AddDays(1))))
                                    )
                                )
                        .OrderBy(t => t.FirstIssuePlanDate)
                        .ToList();
            }
            else
            {
                docList = this.documentPackageService.GetAllByProject(projectId, false)
                    .Where(t => !t.IsInternalDocument.GetValueOrDefault()
                                && !t.HasAttachFile
                                && (
                                    ((txtFromDate.SelectedDate == null || (t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value >= txtFromDate.SelectedDate.Value))
                                    && (txtToDate.SelectedDate == null || (t.FirstIssuePlanDate != null && t.FirstIssuePlanDate.Value < txtToDate.SelectedDate.Value.AddDays(1))))
                                    ||
                                    ((txtFromDate.SelectedDate == null || (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value >= txtFromDate.SelectedDate.Value))
                                    && (txtToDate.SelectedDate == null || (t.FinalIssuePlanDate != null && t.FinalIssuePlanDate.Value < txtToDate.SelectedDate.Value.AddDays(1))))
                                    )
                                )
                    .OrderBy(t => t.FirstIssuePlanDate)
                    .ToList();

            }

            this.grdDocument.DataSource = docList;
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
            var docId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var docObj = this.documentPackageService.GetById(docId);
            if (docObj != null)
            {
                if (docObj.ParentId == null)
                {
                    docObj.IsDelete = true;
                    this.documentPackageService.Update(docObj);
                }
                else
                {
                    var listRelateDoc = this.documentPackageService.GetAllRelatedDocument(docObj.ParentId.GetValueOrDefault());
                    if (listRelateDoc != null)
                    {
                        foreach (var objDoc in listRelateDoc)
                        {
                            objDoc.IsDelete = true;
                            this.documentPackageService.Update(objDoc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The grd document_ item command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {

            }
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                this.CustomerMenu.Items[3].Visible = false;
                this.rtvDiscipline.UnselectAllNodes();
                this.grdDocument.Rebind();
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {

            }
        }

        /// <summary>
        /// The grd document_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["HasAttachFile"].Text == "True")
                {
                    item.BackColor = Color.Aqua;
                    item.BorderColor = Color.Aqua;
                }
            }
        }

        protected void grdDocument_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                var item = e.Item as GridEditableItem;
                var lbldocNo = item.FindControl("lbldocNo") as Label;
                var txtDocTitle = item.FindControl("txtDocTitle") as TextBox;
                var ddlDepartment = item.FindControl("ddlDepartment") as RadComboBox;
                var txtStartDate = item.FindControl("txtStartDate") as RadDatePicker;
                var txtPlanedDate = item.FindControl("txtPlanedDate") as RadDatePicker;

                var ddlRevision = item.FindControl("ddlRevision") as RadComboBox;
                var txtRevisionPlanedDate = item.FindControl("txtRevisionPlanedDate") as RadDatePicker;
                var txtRevisionActualDate = item.FindControl("txtRevisionActualDate") as RadDatePicker;
                var txtRevisionCommentCode = item.FindControl("txtRevisionCommentCode") as TextBox;

                var txtComplete = item.FindControl("txtComplete") as RadNumericTextBox;
                var txtWeight = item.FindControl("txtWeight") as RadNumericTextBox;

                var txtOutgoingTransNo = item.FindControl("txtOutgoingTransNo") as TextBox;
                var txtOutgoingTransDate = item.FindControl("txtOutgoingTransDate") as RadDatePicker;

                var txtIncomingTransNo = item.FindControl("txtIncomingTransNo") as TextBox;
                var txtIncomingTransDate = item.FindControl("txtIncomingTransDate") as RadDatePicker;

                var txtICAReviewOutTransNo = item.FindControl("txtICAReviewOutTransNo") as TextBox;
                var txtICAReviewReceivedDate = item.FindControl("txtICAReviewReceivedDate") as RadDatePicker;
                var txtICAReviewCode = item.FindControl("txtICAReviewCode") as TextBox;

                var cbIsEMDR = item.FindControl("cbIsEMDR") as CheckBox;

                var docId = Convert.ToInt32(item.GetDataKeyValue("ID"));

                var objDoc = this.documentPackageService.GetById(docId);

                var currentRevision = objDoc.RevisionId;
                var newRevision = Convert.ToInt32(ddlRevision.SelectedValue);
                var department = this.roleService.GetByID(Convert.ToInt32(ddlDepartment.SelectedValue));

                var projectid = Convert.ToInt32(this.ddlProject.SelectedValue);
                var projectname = this.ddlProject.SelectedItem.Text;

                if (newRevision > currentRevision)
                {
                    var docObjNew = new DocumentPackage();
                    docObjNew.ProjectId = projectid;
                    docObjNew.ProjectName = projectname;
                    docObjNew.DisciplineId = objDoc.DisciplineId;
                    docObjNew.DisciplineName = objDoc.DisciplineName;
                    docObjNew.DocNo = lbldocNo.Text;
                    docObjNew.DocTitle = txtDocTitle.Text.Trim();
                    docObjNew.DeparmentId = objDoc.DeparmentId;
                    docObjNew.DeparmentName = objDoc.DeparmentName;
                    docObjNew.StartDate = txtStartDate.SelectedDate;

                    docObjNew.PlanedDate = txtPlanedDate.SelectedDate;
                    docObjNew.RevisionId = newRevision;
                    docObjNew.RevisionName = ddlRevision.SelectedItem.Text;
                    docObjNew.RevisionActualDate = txtRevisionActualDate.SelectedDate;
                    docObjNew.RevisionCommentCode = txtRevisionCommentCode.Text.Trim();
                    docObjNew.RevisionPlanedDate = txtRevisionPlanedDate.SelectedDate;
                    docObjNew.Complete = txtComplete.Value.GetValueOrDefault();
                    docObjNew.Weight = txtWeight.Value.GetValueOrDefault();
                    docObjNew.OutgoingTransNo = txtOutgoingTransNo.Text.Trim();
                    docObjNew.OutgoingTransDate = txtOutgoingTransDate.SelectedDate;
                    docObjNew.IncomingTransNo = txtIncomingTransNo.Text.Trim();
                    docObjNew.IncomingTransDate = txtIncomingTransDate.SelectedDate;
                    docObjNew.ICAReviewOutTransNo = txtICAReviewOutTransNo.Text.Trim();
                    docObjNew.ICAReviewCode = txtICAReviewCode.Text.Trim();
                    docObjNew.ICAReviewReceivedDate = txtICAReviewReceivedDate.SelectedDate;

                    docObjNew.DocumentTypeId = objDoc.DocumentTypeId;
                    docObjNew.DocumentTypeName = objDoc.DocumentTypeName;
                    docObjNew.DisciplineId = objDoc.DisciplineId;
                    docObjNew.DisciplineName = objDoc.DisciplineName;
                    docObjNew.PackageId = objDoc.PackageId;
                    docObjNew.PackageName = objDoc.PackageName;
                    docObjNew.Notes = string.Empty;
                    docObjNew.PlatformId = objDoc.PlatformId;
                    docObjNew.PlatformName = objDoc.PlatformName;

                    docObjNew.IsLeaf = true;
                    docObjNew.IsEMDR = cbIsEMDR.Checked;
                    docObjNew.ParentId = objDoc.ParentId ?? objDoc.ID;
                    docObjNew.CreatedBy = UserSession.Current.User.Id;
                    docObjNew.CreatedDate = DateTime.Now;

                    this.documentPackageService.Insert(docObjNew);

                    objDoc.IsLeaf = false;
                }
                else
                {
                    ////objDoc.DocNo = lbldocNo.Text;
                    objDoc.DocTitle = txtDocTitle.Text.Trim();
                    ////objDoc.DeparmentName = department != null ? department.FullName : string.Empty;
                    ////objDoc.DeparmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                    objDoc.StartDate = txtStartDate.SelectedDate;
                    objDoc.PlanedDate = txtPlanedDate.SelectedDate;
                    objDoc.RevisionId = newRevision;
                    objDoc.RevisionName = ddlRevision.SelectedItem.Text;
                    objDoc.RevisionActualDate = txtRevisionActualDate.SelectedDate;
                    objDoc.RevisionCommentCode = txtRevisionCommentCode.Text.Trim();
                    objDoc.RevisionPlanedDate = txtRevisionPlanedDate.SelectedDate;
                    objDoc.Complete = txtComplete.Value.GetValueOrDefault();
                    objDoc.Weight = txtWeight.Value.GetValueOrDefault();
                    ////objDoc.OutgoingTransNo = txtOutgoingTransNo.Text.Trim();
                    ////objDoc.OutgoingTransDate = txtOutgoingTransDate.SelectedDate;
                    ////objDoc.IncomingTransNo = txtIncomingTransNo.Text.Trim();
                    ////objDoc.IncomingTransDate = txtIncomingTransDate.SelectedDate;
                    ////objDoc.ICAReviewOutTransNo = txtICAReviewOutTransNo.Text.Trim();
                    ////objDoc.ICAReviewCode = txtICAReviewCode.Text.Trim();
                    ////objDoc.ICAReviewReceivedDate = txtICAReviewReceivedDate.SelectedDate;

                    objDoc.IsEMDR = cbIsEMDR.Checked;
                }

                objDoc.UpdatedBy = UserSession.Current.User.Id;
                objDoc.UpdatedDate = DateTime.Now;

                this.documentPackageService.Update(objDoc);
            }
        }

        protected void ckbEnableFilter_OnCheckedChanged(object sender, EventArgs e)
        {
            this.grdDocument.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdDocument.Rebind();
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
            this.grdDocument.Rebind();


        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
        }

        protected void rtvDiscipline_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
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

        protected void txtToDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void txtFromDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            this.grdDocument.Rebind();
        }
    }
}