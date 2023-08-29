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
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class InternalDocumentInfoEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        /// <summary>
        /// The revision service.
        /// </summary>
        private readonly RevisionService revisionService;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly ScopeProjectService scopeProjectService;

        /// <summary>
        /// The document package service.
        /// </summary>
        private readonly DocumentPackageService documentPackageService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly StatusService statusService;

        private readonly ProcessActualService processActualService;

        private OriginatorService originatorService;

        private readonly DocumentSequenceManagementService documentSequenceManagementService;

        private readonly DocumentNumberingService documentNumberingService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public InternalDocumentInfoEditForm()
        {
            this.revisionService = new RevisionService();
            this.documentService = new DocumentService();
            this.scopeProjectService = new ScopeProjectService();
            this.documentPackageService = new DocumentPackageService();
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.userService = new UserService();
            this.statusService = new StatusService();
            this.processActualService = new ProcessActualService();
            this.originatorService = new OriginatorService();
            this.documentSequenceManagementService = new DocumentSequenceManagementService();
            this.documentNumberingService = new DocumentNumberingService();
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
                ////this.LoadComboData();
                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.Role.IsUpdate.GetValueOrDefault())
                {
                    this.btnSave.Visible = false;
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["projId"]))
                {
                    var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                    var projObj = this.scopeProjectService.GetById(projectId);
                    this.txtProjectName.Text = projObj.FullName;
                    this.LoadComboData(projectId);
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.CreatedInfo.Visible = true;
                    var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
                    var docObj = this.documentPackageService.GetById(docId);
                    if (docObj != null)
                    {
                        this.LoadComboData(docObj.ProjectId.GetValueOrDefault());
                        this.txtProjectName.Text = docObj.ProjectFullName;

                        this.LoadDocInfo(docObj);

                        var createdUser = this.userService.GetByID(docObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + docObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (docObj.UpdatedBy != null && docObj.UpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(docObj.UpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + docObj.UpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
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
            if (this.Page.IsValid && !string.IsNullOrEmpty(this.Request.QueryString["projId"]))
            {
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projObj = this.scopeProjectService.GetById(projectId);
                DocumentPackage docObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    var docId = Convert.ToInt32(this.Request.QueryString["docId"]);
                    docObj = this.documentPackageService.GetById(docId);
                    if (docObj != null)
                    {
                        var oldRevision = docObj.RevisionId;
                        var newRevision = Convert.ToInt32(this.ddlRevision.SelectedValue);
                        if (newRevision > oldRevision)
                        {
                            var docObjNew = new DocumentPackage();
                            this.CollectData(ref docObjNew, projObj);

                            // Insert new doc
                            docObjNew.CreatedBy = UserSession.Current.User.Id;
                            docObjNew.CreatedDate = DateTime.Now;
                            docObjNew.IsLeaf = true;
                            docObjNew.IsDelete = false;
                            docObjNew.IsEMDR = true;
                            docObjNew.ParentId = docObj.ParentId ?? docObj.ID;
                            this.documentPackageService.Insert(docObjNew);

                            // Upate old doc
                            docObj.IsLeaf = false;
                        }
                        else
                        {
                            this.CollectData(ref docObj, projObj);
                        }

                        docObj.UpdatedBy = UserSession.Current.User.Id;
                        docObj.UpdatedDate = DateTime.Now;
                        this.documentPackageService.Update(docObj);

                        if (projObj.IsAutoCalculate.GetValueOrDefault())
                        {
                            this.UpdateActualProgress(projObj);
                        }
                    }
                }
                else
                {
                    docObj = new DocumentPackage()
                    {
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        IsLeaf = true,
                        IsDelete = false
                    };

                    this.CollectData(ref docObj, projObj);
                    this.documentPackageService.Insert(docObj);

                    // Update Sequence of document
                    var sequenceObj = this.documentSequenceManagementService.GetByDisciplineDocType(docObj.DisciplineId, docObj.DocumentTypeId);
                    if (sequenceObj == null)
                    {
                        sequenceObj = new DocumentSequenceManagement()
                        {
                            DisciplineId = docObj.DisciplineId,
                            DocumentTypeId = docObj.DocumentTypeId,
                            CurrentSequence = 1
                        };

                        this.documentSequenceManagementService.Insert(sequenceObj);
                    }
                    else
                    {
                        sequenceObj.CurrentSequence += 1;
                        this.documentSequenceManagementService.Update(sequenceObj);
                    }


                    if (projObj.IsAutoCalculate.GetValueOrDefault())
                    {
                        this.UpdateActualProgress(projObj);
                    }
                    // --------------------------------------------------------------------------------------------------------------------------------
                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
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

        /// <summary>
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            ////if(this.txtName.Text.Trim().Length == 0)
            ////{
            ////    this.fileNameValidator.ErrorMessage = "Please enter file name.";
            ////    this.divFileName.Style["margin-bottom"] = "-26px;";
            ////    args.IsValid = false;
            ////}
            ////else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            ////{
            ////    var docId = Convert.ToInt32(Request.QueryString["docId"]);
            ////    this.fileNameValidator.ErrorMessage = "The specified name is already in use.";
            ////    this.divFileName.Style["margin-bottom"] = "-26px;";
            ////    args.IsValid = true; ////!this.documentService.IsDocumentExistUpdate(folderId, this.txtName.Text.Trim(), docId);
            ////}
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("CheckFileName"))
            {
                var fileName = e.Argument.Split('$')[1];
                var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                
                if(this.documentService.IsDocumentExist(folderId, fileName))
                {
                }
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData(int projectId)
        {
            var listDisciplineInPermission =this.disciplineService.GetAllDisciplineOfProject(projectId).OrderBy(t => t.Name).ToList();

            this.ddlDiscipline.DataSource = listDisciplineInPermission;
            listDisciplineInPermission.Insert(0, new Discipline() {ID = 0});
            this.ddlDiscipline.DataTextField = "FullName";
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataBind();

            var listRevision = this.revisionService.GetAllByProject(projectId);
            this.ddlRevision.DataSource = listRevision;
            this.ddlRevision.DataTextField = "Name";
            this.ddlRevision.DataValueField = "ID";
            this.ddlRevision.DataBind();

            //var docTypeList = this.documentTypeService.GetAllByProject(projectId).OrderBy(t => t.Name).ToList();
            //docTypeList.Insert(0, new DocumentType() {ID = 0});
            //this.ddlDocType.DataSource = docTypeList;
            //this.ddlDocType.DataTextField = "FullName";
            //this.ddlDocType.DataValueField = "ID";
            //this.ddlDocType.DataBind();

            var originatorList = this.originatorService.GetAll().OrderBy(t => t.Name).ToList();
            originatorList.Insert(0, new Originator() { ID = 0 });
            this.ddlOriginator.DataSource = originatorList;
            this.ddlOriginator.DataTextField = "FullName";
            this.ddlOriginator.DataValueField = "ID";
            this.ddlOriginator.DataBind();

            var statusList = this.statusService.GetAllByProject(projectId).OrderBy(t => t.PercentCompleteDefault).ToList();
            statusList.Insert(0, new Status { ID = 0 });
            this.ddlStatus.DataSource = statusList;
            this.ddlStatus.DataTextField = "FullNameWithWeight";
            this.ddlStatus.DataValueField = "ID";
            this.ddlStatus.DataBind();

            var documentNumberingList = this.documentNumberingService.GetAllByProject(projectId).OrderBy(t => t.Name).ToList();
            this.ddlDocumentNumbering.DataSource = documentNumberingList;
            this.ddlDocumentNumbering.DataTextField = "FullName";
            this.ddlDocumentNumbering.DataValueField = "ID";
            this.ddlDocumentNumbering.DataBind();
        }

        private void CollectData(ref DocumentPackage docObj, ScopeProject projObj)
        {
            docObj.ProjectId = projObj.ID;
            docObj.ProjectName = projObj.Name;

            docObj.DocNo = this.txtDocNumber.Text.Trim();
            docObj.DocTitle = this.txtDocumentTitle.Text.Trim();
            docObj.DocumentTypeId = this.ddlDocType.SelectedItem != null
                                        ? Convert.ToInt32(this.ddlDocType.SelectedValue)
                                        : 0;
            docObj.DocTypeFullName = this.ddlDocType.SelectedItem != null
                                          ? this.ddlDocType.SelectedItem.Text
                                          : string.Empty;
            docObj.DocumentTypeName = docObj.DocTypeFullName.Split('-')[0].Trim();

            docObj.StatusID = this.ddlStatus.SelectedItem != null
                                        ? Convert.ToInt32(this.ddlStatus.SelectedValue)
                                        : 0;
            docObj.StatusName = this.ddlStatus.SelectedItem != null
                                          ? this.ddlStatus.SelectedItem.Text
                                          : string.Empty;

            docObj.DisciplineId = this.ddlDiscipline.SelectedItem != null
                                      ? Convert.ToInt32(this.ddlDiscipline.SelectedValue)
                                      : 0;
            docObj.DisciplineFullName = this.ddlDiscipline.SelectedItem != null
                                        ? this.ddlDiscipline.SelectedItem.Text
                                        : string.Empty;
            docObj.DisciplineName = docObj.DisciplineFullName.Split('-')[0].Trim();
            docObj.Complete = this.txtComplete.Value.GetValueOrDefault();
            docObj.Weight = this.txtWeight.Value.GetValueOrDefault();
            docObj.Notes = this.txtNotes.Text.Trim();

            docObj.RevisionId = Convert.ToInt32(this.ddlRevision.SelectedValue);
            docObj.RevisionName = this.ddlRevision.SelectedItem.Text;
            //docObj.RevisionPlanedDate = this.txtRevisionPlaned.SelectedDate;
            //docObj.RevisionActualDate = this.txtRevisionActual.SelectedDate;
            //docObj.RevisionReceiveTransNo = this.txtReceiveTransNo.Text.Trim();
            docObj.CompleteForProject = Math.Round(this.txtComplete.Value.GetValueOrDefault() * docObj.Weight.GetValueOrDefault() / 100, 2);

            docObj.OriginatorId = this.ddlOriginator.SelectedItem != null
                                      ? Convert.ToInt32(this.ddlOriginator.SelectedValue)
                                      : 0;
            docObj.OriginatorFullName = this.ddlOriginator.SelectedItem != null
                                        ? this.ddlOriginator.SelectedItem.Text
                                        : string.Empty;
            docObj.OriginatorName = docObj.OriginatorFullName.Split('-')[0].Trim();

            docObj.SequencetialNumber = this.txtSequentialNumber.Text.Trim();
            docObj.DrawingSheetNumber = this.txtDrawingSheetNumber.Text.Trim();
            //docObj.RevisionPlanReceiveCommentDate = this.txtCommentPlaned.SelectedDate;
            //docObj.RevisionActualReceiveCommentDate = this.txtActualCommentDate.SelectedDate;

            ////docObj.FirstIssuePlanDate = this.txtFirstIssuePlanDate.SelectedDate;
            ////docObj.FirstIssueActualDate = this.txtFirstIssueActualDate.SelectedDate;
            ////docObj.FirstIssueTransNo = this.txtFirstIssueTransNo.Text.Trim();

            ////docObj.FinalIssuePlanDate = this.txtFinalIssuePlanDate.SelectedDate;
            ////docObj.FinalIssueActualDate = this.txtFinalIssueActualDate.SelectedDate;
            ////docObj.FinalIssueTransNo = this.txtFinalIssueTransNo.Text.Trim();
        }

        private void LoadDocInfo(DocumentPackage docObj)
        {
            this.txtDocNumber.Text = docObj.DocNo;
            this.txtDocumentTitle.Text = docObj.DocTitle;
            this.ddlDocType.SelectedValue = docObj.DocumentTypeId.ToString();
            this.ddlOriginator.SelectedValue = docObj.OriginatorId.ToString();
            this.ddlDiscipline.SelectedValue = docObj.DisciplineId.ToString();
            this.ddlStatus.SelectedValue = docObj.StatusID.ToString();
            this.txtComplete.Value = docObj.Complete;
            this.txtWeight.Value = docObj.Weight;
            this.txtNotes.Text = docObj.Notes;

            this.ddlRevision.SelectedValue = docObj.RevisionId.ToString();
            //this.txtRevisionPlaned.SelectedDate = docObj.RevisionPlanedDate;
            //this.txtRevisionActual.SelectedDate = docObj.RevisionActualDate;
            //this.txtReceiveTransNo.Text = docObj.RevisionReceiveTransNo;

            this.txtSequentialNumber.Text = docObj.SequencetialNumber;
            this.txtDrawingSheetNumber.Text = docObj.DrawingSheetNumber;
            //this.txtCommentPlaned.SelectedDate = docObj.RevisionPlanReceiveCommentDate;
            //this.txtActualCommentDate.SelectedDate = docObj.RevisionActualReceiveCommentDate;
            ////this.txtFinalCode.Text = docObj.FinalCodeName;

            ////this.txtFirstIssueActualDate.SelectedDate = docObj.FirstIssueActualDate;
            ////this.txtFirstIssuePlanDate.SelectedDate = docObj.FirstIssuePlanDate;
            ////this.txtFirstIssueTransNo.Text = docObj.FirstIssueTransNo;

            ////this.txtFinalIssuePlanDate.SelectedDate = docObj.FinalIssuePlanDate;
            ////this.txtFinalIssueActualDate.SelectedDate = docObj.FinalIssueActualDate;
            ////this.txtFinalIssueTransNo.Text = docObj.FinalIssueTransNo;
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var statusObj = this.statusService.GetById(Convert.ToInt32(this.ddlStatus.SelectedValue));
            if (statusObj != null)
            {
                this.txtComplete.Value = Math.Round(statusObj.PercentCompleteDefault.GetValueOrDefault(), 2);
            }
        }

        protected void ddlRevision_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txtFinalCode.Text = string.Empty;
        }

        private void UpdateActualProgress(ScopeProject objProject)
        {
            var count = 0;
            for (var j = objProject.StartDate.GetValueOrDefault();
                j < objProject.EndDate.GetValueOrDefault();
                j = j.AddDays(objProject.FrequencyForProgressChart != null && objProject.FrequencyForProgressChart != 0 ? objProject.FrequencyForProgressChart.Value : 7))
            {
                if (DateTime.Now > j)
                {
                    count += 1;
                }
                else
                {
                    break;
                }
            }

            var listDiscipline = this.disciplineService.GetAllDisciplineOfProject(objProject.ID).OrderBy(t => t.ID).ToList();
            foreach (var discipline in listDiscipline)
            {
                var docList = this.documentPackageService.GetAllByDiscipline(discipline.ID).OrderBy(t => t.DocNo).ToList();
                double? complete = 0;
                complete = docList.Aggregate(complete, (current, t) => current + t.CompleteForProject);
                var existProgressActual = this.processActualService.GetByProjectAndWorkgroup(objProject.ID, discipline.ID);
                if (existProgressActual != null)
                {
                    var arrActual = existProgressActual.Actual.Split('$');
                    if (arrActual.Count() > count)
                    {
                        arrActual[count] = Math.Round(complete.GetValueOrDefault(), 2).ToString();

                        var newAtualProgress = string.Empty;
                        newAtualProgress = arrActual.Aggregate(newAtualProgress, (current, t) => current + t + "$");
                        newAtualProgress = newAtualProgress.Substring(0, newAtualProgress.Length - 1);

                        existProgressActual.Actual = newAtualProgress;

                        this.processActualService.Update(existProgressActual);
                    }
                }
            }
        }

        private string GenerateDocumentNumber()
        {
            if (this.ddlDocumentNumbering.SelectedItem != null)
            {
                var docNo = string.Empty;
                var docNumberingObj = this.documentNumberingService.GetById(Convert.ToInt32(this.ddlDocumentNumbering.SelectedValue));
                var docNumneringObjValuePart = docNumberingObj.Format.ToCharArray();

                var countPart = 0;
                var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
                var projObj = this.scopeProjectService.GetById(projectId);

                var originatorObj = this.originatorService.GetById(Convert.ToInt32(this.ddlOriginator.SelectedValue));
                var disciplineObj = this.disciplineService.GetById(Convert.ToInt32(this.ddlDiscipline.SelectedValue));
                var docTypeObj = this.documentTypeService.GetById(Convert.ToInt32(this.ddlDocType.SelectedValue));

                for (int i = 0; i < docNumneringObjValuePart.Length; i++)
                {
                    if (i == 0)
                    {
                        countPart = 1;
                    }

                    if (i != 0 && docNumneringObjValuePart[i] == docNumneringObjValuePart[i - 1])
                    {
                        countPart += 1;
                    }
                    else
                    {
                        countPart = 1;
                    }

                    switch (docNumneringObjValuePart[i])
                    {
                        case '-':
                            docNo += "-";
                            break;
                        case '.':
                            docNo += ".";
                            break;
                        case 'A':
                            if (projObj != null)
                            {
                                docNo += projObj.Name.ToCharArray()[countPart - 1];
                            }
                            
                            break;
                        case 'B':
                            if (originatorObj != null)
                            {
                                docNo += originatorObj.Name.ToCharArray()[countPart - 1];
                            }
                            break;
                        case 'C':
                            if (disciplineObj != null)
                            {
                                docNo += disciplineObj.Name.ToCharArray()[countPart - 1];
                            }
                            break;
                        case 'D':
                            if (docTypeObj != null)
                            {
                                docNo += docTypeObj.Name.ToCharArray()[countPart - 1];
                            }
                            break;
                        case 'X':
                            if (!string.IsNullOrEmpty(this.txtSequentialNumber.Text))
                            {
                                docNo += this.txtSequentialNumber.Text.ToCharArray()[countPart - 1];
                            }
                            break;
                        case 'Y':
                            break;
                    }
                }

                return docNo;
            }

            return string.Empty;
        }

        protected void ddlOriginator_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtDocNumber.Text = this.GenerateDocumentNumber();
        }

        protected void ddlDiscipline_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var sequenceObj = this.documentSequenceManagementService.GetByDisciplineDocType(Convert.ToInt32(this.ddlDiscipline.SelectedValue), Convert.ToInt32(this.ddlDocType.SelectedValue));
            if (sequenceObj == null)
            {
                this.txtSequentialNumber.Text = Utility.ReturnSequenceString(1, 4);
            }
            else
            {
                this.txtSequentialNumber.Text = Utility.ReturnSequenceString(sequenceObj.CurrentSequence.Value + 1, 4);
            }

            this.txtDocNumber.Text = this.GenerateDocumentNumber();
        }

        protected void ddlDocType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var sequenceObj = this.documentSequenceManagementService.GetByDisciplineDocType(Convert.ToInt32(this.ddlDiscipline.SelectedValue), Convert.ToInt32(this.ddlDocType.SelectedValue));
            if (sequenceObj == null)
            {
                this.txtSequentialNumber.Text = Utility.ReturnSequenceString(1, 4);
            }
            else
            {
                this.txtSequentialNumber.Text = Utility.ReturnSequenceString(sequenceObj.CurrentSequence.Value + 1, 4);
            }

            this.txtDocNumber.Text = this.GenerateDocumentNumber();
        }
    }
}