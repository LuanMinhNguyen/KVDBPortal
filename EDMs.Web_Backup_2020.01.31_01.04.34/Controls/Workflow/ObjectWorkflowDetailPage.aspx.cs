// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using System.Configuration;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ObjectWorkflowDetailPage : Page
    {

        private readonly WorkflowService wfService= new WorkflowService();
        private readonly UserService userService= new UserService();
        private readonly ObjectAssignedWorkflowService objAssignedWfService= new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService= new ObjectAssignedUserService();
        private readonly WorkflowStepService wfStepService= new WorkflowStepService();
        private readonly WorkflowDetailService wfDetailService= new WorkflowDetailService();
        private readonly HolidayConfigService holidayConfigService= new HolidayConfigService();
        private readonly PECC2DocumentsService documentService= new PECC2DocumentsService();
        private readonly DistributionMatrixService matrixService= new DistributionMatrixService();
        private readonly DistributionMatrixDetailService matrixDetailService= new DistributionMatrixDetailService();
        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService= new ContractorTransmittalDocFileService();
        private readonly PECC2TransmittalService pecc2TransmittalService= new PECC2TransmittalService();
        private readonly AttachDocToTransmittalService attachDocToTransmittalService= new AttachDocToTransmittalService();
        private readonly HashSet<DateTime> holidays = new HashSet<DateTime>();
        private readonly ChangeRequestService changeRequestService= new ChangeRequestService();
        private readonly NCR_SIService ncrSiService= new NCR_SIService();
        private readonly CustomizeWorkflowDetailService customizeWorkflowDetailService= new CustomizeWorkflowDetailService();

        private readonly PECC2TransmittalService _pecc2transmittal= new PECC2TransmittalService();

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();
        private readonly RFIService rfiService = new RFIService();
        private readonly ChangeGradeCodeService changeGradeCodeService = new ChangeGradeCodeService();
        private readonly NCR_SIService ncrsiService = new NCR_SIService();
        private readonly RFIDetailService rfidetailService= new RFIDetailService();
        private readonly ShipmentService shipmentService = new ShipmentService();

        private int WorkflowId
        {
            get
            {
                return Convert.ToInt32(this.Request.QueryString["wfId"]);
            }
        }
        private new Guid ObjId
        {
            get
            {
                return new Guid(Request.QueryString["objId"]);
            }
        }
        private Data.Entities.Workflow WorkflowObj
        {
            get { return this.wfService.GetById(this.WorkflowId); }
        }
        //private Data.Entities.DQREDocument ObjectDocument
        //{
        //    get { return this.dqreDocumentService.GetById(this.ObjId); }
        //}
        //private ScopeProject ProjectObj
        //{
        //    get { return this.projectService.GetById(this.WorkflowObj.ProjectID.GetValueOrDefault()); }
        //}

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
            // this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.IsPostBack)
            {
                var wfObj = this.wfService.GetById(this.WorkflowId);
                if (wfObj != null)
                {
                    this.txtWorkflowName.Text = wfObj.Name;
                }
                if (!this.customizeWorkflowDetailService.GetAllByObjId(this.ObjId).Where(t => t.WorkflowID == wfObj.ID).Any() && !this.customizeWorkflowDetailService.GetAllByTransId(this.ObjId).Where(t => t.WorkflowID == wfObj.ID).Any())
                {
                    var customizeWfFrom = this.Request.QueryString["customizeWfFrom"];
                    var originalWorkflowDetails = this.wfDetailService.GetAllByWorkflow(this.WorkflowId);
                    foreach (var wfDetail in originalWorkflowDetails)
                    {
                        var customizeWfDetail = new CustomizeWorkflowDetail()
                        {
                            WorkflowID = wfDetail.WorkflowID,
                            WorkflowName = wfDetail.WorkflowName,
                            CurrentWorkflowStepID = wfDetail.CurrentWorkflowStepID,
                            CurrentWorkflowStepName = wfDetail.CurrentWorkflowStepName,
                            StepDefinitionID = wfDetail.StepDefinitionID,
                            StepDefinitionName = wfDetail.StepDefinitionName,
                            Duration = wfDetail.Duration,
                            AssignTitleIds = wfDetail.AssignTitleIds,
                            AssignUserIDs = wfDetail.AssignUserIDs,
                            ReviewUserIds = wfDetail.ReviewUserIds,
                            ConsolidateUserIds = wfDetail.ConsolidateUserIds,
                            CommentUserIds = wfDetail.CommentUserIds,
                            ApproveUserIds = wfDetail.ApproveUserIds,
                            InformationOnlyUserIDs = wfDetail.InformationOnlyUserIDs,
                            ManagementUserIds = wfDetail.ManagementUserIds,
                            AssignRoleIDs = wfDetail.AssignRoleIDs,
                            InformationOnlyRoleIDs = wfDetail.InformationOnlyRoleIDs,
                            DistributionMatrixIDs = wfDetail.DistributionMatrixIDs,
                            Recipients = wfDetail.Recipients,
                            NextWorkflowStepID = wfDetail.NextWorkflowStepID,
                            NextWorkflowStepName = wfDetail.NextWorkflowStepName,
                            RejectWorkflowStepID = wfDetail.RejectWorkflowStepID,
                            RejectWorkflowStepName = wfDetail.RejectWorkflowStepName,
                            CreatedBy = wfDetail.CreatedBy,
                            CreatedDate = wfDetail.CreatedDate,
                            UpdatedBy = wfDetail.UpdatedBy,
                            UpdatedDate = wfDetail.UpdatedDate,
                            ProjectID = wfDetail.ProjectID,
                            ProjectName = wfDetail.ProjectName,
                            IsFirst = wfDetail.IsFirst,
                            CanReject = wfDetail.CanReject,
                            IsOnlyWorkingDay = wfDetail.IsOnlyWorkingDay,
                            IsCanCreateOutgoingTrans = wfDetail.IsCanCreateOutgoingTrans,
                            ActionApplyCode = wfDetail.ActionApplyCode,
                            ActionApplyName = wfDetail.ActionApplyName,

                        };

                        if (customizeWfFrom == "Trans")
                        {
                            customizeWfDetail.IncomingTransId = this.ObjId;
                        }
                        else
                        {
                            customizeWfDetail.ObjectId = this.ObjId;
                        }

                        this.customizeWorkflowDetailService.Insert(customizeWfDetail);
                    }
                }
            
                //else if (!this.customizeWorkflowDetailService.GetAllByObjId(this.ObjId).Where(t => t.WorkflowID != wfObj.ID).Any() && !this.customizeWorkflowDetailService.GetAllByTransId(this.ObjId).Where(t => t.WorkflowID != wfObj.ID).Any())
                //{
                   

                //}

                //this.divMessage.Visible = false;
                //var firstStep = this.wfStepService.GetAllByWorkflow(WorkflowId).FirstOrDefault(t => t.IsFirst.GetValueOrDefault());
                //if (firstStep != null)
                //{
                //    this.txtFirstStep.Text = firstStep.WorkflowName;
                //}
                //DeleteTemplateWFDetail(this.templateWFDetailService.GetAllByWorkflow(this.WorkflowId, UserSession.Current.User.Id));
                //InsertTemplateWFDetail(this.wfDetailService.GetAllByWorkflow(this.WorkflowId));
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var customizeWfFrom = this.Request.QueryString["customizeWfFrom"];
            if (customizeWfFrom == "Trans")
            {
                this.grdDocument.DataSource = this.customizeWorkflowDetailService.GetAllByWorkflowAndTrans(this.WorkflowId,
                    this.ObjId);
            }
            else
            {
                this.grdDocument.DataSource = this.customizeWorkflowDetailService.GetAllByWorkflowAndObj(this.WorkflowId,
                    this.ObjId);
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnBatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                var customizeWFDetailID = Convert.ToInt32(newValues["ID"].ToString());
                var customizeWFDetailObj = this.customizeWorkflowDetailService.GetById(customizeWFDetailID);

                if (customizeWFDetailObj != null)
                {
                    //var wfStepDefineID = Convert.ToInt32(newValues["StepDefinitionID"].ToString());
                    //var isOnlyWorkingDay = Convert.ToBoolean(newValues["IsOnlyWorkingDay"].ToString());
                    //var nextWorkflowStepId = Convert.ToInt32(newValues["NextWorkflowStepID"].ToString());
                    //var nextWorkflowStepObj = this.wfStepService.GetById(nextWorkflowStepId);
                    //var rejectWorkflowStepId = Convert.ToInt32(newValues["RejectWorkflowStepID"].ToString());
                    //var rejectWorkflowStepObj = this.wfStepService.GetById(rejectWorkflowStepId);

                    //wfDetailObj.StepDefinitionID = wfStepDefineID;
                    //wfDetailObj.StepDefinitionName = Utility.WorkflowStepDefine[wfStepDefineID];

                    //customizeWFDetailObj.NextWorkflowStepID = nextWorkflowStepObj != null
                    //    ? nextWorkflowStepObj.ID
                    //    : 0;
                    //customizeWFDetailObj.NextWorkflowStepName = nextWorkflowStepObj != null
                    //    ? nextWorkflowStepObj.Name
                    //    : string.Empty;

                    //customizeWFDetailObj.RejectWorkflowStepID = rejectWorkflowStepObj != null
                    //    ? rejectWorkflowStepObj.ID
                    //    : 0;
                    //customizeWFDetailObj.RejectWorkflowStepName = rejectWorkflowStepObj != null
                    //    ? rejectWorkflowStepObj.Name
                    //    : string.Empty;

                    customizeWFDetailObj.Duration = !string.IsNullOrEmpty(newValues["Duration"].ToString()) 
                                        ? Convert.ToDouble(newValues["Duration"].ToString())
                                        : (double?) null;
                    customizeWFDetailObj.IsOnlyWorkingDay = true;

                    ////wfDetailObj.AssignRoleIDs = string.Empty;
                    ////wfDetailObj.AssignUserIDs = string.Empty;
                    ////wfDetailObj.InformationOnlyRoleIDs = string.Empty;
                    ////wfDetailObj.InformationOnlyUserIDs = string.Empty;
                    ////wfDetailObj.DistributionMatrixIDs = string.Empty;

                    this.customizeWorkflowDetailService.Update(customizeWFDetailObj);
                    
                }
            }
           // this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
          // this.ClientScript.RegisterStartupScript(Page.GetType(), "SaveClose", "<script>alert('hey');</script>");
        }

        protected void grdDocument_OnPreRender(object sender, EventArgs e)
        {
            //var wfStepList = this.wfStepService.GetAllByWorkflow(this.WorkflowId);

            //var ddlAcceptStep = (this.grdDocument.MasterTableView.GetBatchEditorContainer("NextWorkflowStepID") as Panel).FindControl("ddlAcceptStep") as RadComboBox;
            //var ddlRejectStep = (this.grdDocument.MasterTableView.GetBatchEditorContainer("RejectWorkflowStepID") as Panel).FindControl("ddlRejectStep") as RadComboBox;

            //if (ddlAcceptStep != null && ddlRejectStep != null)
            //{
            //    wfStepList.Insert(0, new WorkflowStep() {ID = 0});
            //    ddlAcceptStep.DataSource = wfStepList;
            //    ddlAcceptStep.DataTextField = "Name";
            //    ddlAcceptStep.DataValueField = "ID";
            //    ddlAcceptStep.DataBind();

            //    ddlRejectStep.DataSource = wfStepList;
            //    ddlRejectStep.DataTextField = "Name";
            //    ddlRejectStep.DataValueField = "ID";
            //    ddlRejectStep.DataBind();
            //}
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var projectId = Convert.ToInt32(this.Request.QueryString["projId"]);
            var objType = Convert.ToInt32(this.Request.QueryString["ObjectType"]);
            var attachFrom = Request.QueryString["attachFrom"];

            var customizeWfFrom = Request.QueryString["customizeWfFrom"];
            var isHaveCustomizeWf = false;
           
            var wfObj = this.wfService.GetById(this.WorkflowId);
            if (wfObj != null)
            {
                switch (objType)
                {
                    // Document
                    case 1:
                        switch (attachFrom)
                        {
                            case "ProjectDocList":
                                var docObj = this.documentService.GetById(this.ObjId);
                                if (docObj != null)
                                {
                                    var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                                    var previousstep = new ObjectAssignedWorkflow();
                                    // Update docobj workflow status
                                    docObj.IsInWFProcess = true;
                                    docObj.IsWFComplete = false;
                                    docObj.CurrentWorkflowName = wfObj.Name;
                                    docObj.CurrentWorkflowStepName = wfFirstStepObj.Name;

                                        if (customizeWfFrom == "Trans")
                                        {
                                            docObj.IsUseCustomWfFromTrans = true;
                                    
                                        }
                                        else
                                        {
                                        docObj.IsUseIsUseCustomWfFromObj = true;
                                        }


                                this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, docObj, previousstep, DateTime.Now, objType, customizeWfFrom);
                                    this.documentService.Update(docObj);
                                }
                                break;
                            case "AttachByDocTrans":
                                var contractorTransDocFileObj = contractorTransmittalDocFileService.GetById(this.ObjId);
                                //var TranObjId= new Guid(Request.QueryString["TransPECC2"]);
                                if (contractorTransDocFileObj.PECC2ProjectDocId != null)
                                {
                                    var docObj2 = this.documentService.GetById(contractorTransDocFileObj.PECC2ProjectDocId.GetValueOrDefault());
                                    if (docObj2 != null)
                                    {
                                        var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                                        var previousstep = new ObjectAssignedWorkflow();
                                        //this.ProcessOriginalWorkflow(wfFirstStepObj, wfObj, docObj2, previousstep, DateTime.Now, objType);

                                        // Update docobj workflow status
                                        docObj2.IsInWFProcess = true;
                                        docObj2.IsWFComplete = false;
                                        docObj2.CurrentWorkflowName = wfObj.Name;
                                        docObj2.CurrentWorkflowStepName = wfFirstStepObj.Name;

                                       
                                            if (customizeWfFrom == "Trans")
                                            {
                                                docObj2.IsUseCustomWfFromTrans = true;
                                            }
                                            else
                                            {
                                                docObj2.IsUseIsUseCustomWfFromObj = true;
                                            }

                                            this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, docObj2, previousstep, DateTime.Now, objType, customizeWfFrom);
                                        
                                        this.documentService.Update(docObj2);
                                    }
                                }
                                break;
                            case "AttachByTrans":
                                var pecc2IncomingTrans = this.pecc2TransmittalService.GetById(this.ObjId);
                                var contractorTransDocFileList = this.contractorTransmittalDocFileService.GetAllByTrans(pecc2IncomingTrans.ContractorTransId.GetValueOrDefault());
                                var doclistInTrans = this.attachDocToTransmittalService.GetAllByTransId(this.ObjId);
                                var docIds = doclistInTrans.Select(t => t.DocumentId).Distinct().ToList(); //contractorTransDocFileList.Select(t => t.PECC2ProjectDocId).Distinct().ToList();
                                foreach (var docId in docIds)
                                {
                                    var docObj1 = this.documentService.GetById(docId.GetValueOrDefault());
                                    if (docObj1 != null)
                                    {
                                        var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                                        var previousstep = new ObjectAssignedWorkflow();
                                        //this.ProcessOriginalWorkflow(wfFirstStepObj, wfObj, docObj1, previousstep, DateTime.Now, objType);

                                        // Update docobj workflow status
                                        docObj1.IsInWFProcess = true;
                                        docObj1.IsWFComplete = false;
                                        docObj1.CurrentWorkflowName = wfObj.Name;
                                        docObj1.CurrentWorkflowStepName = wfFirstStepObj.Name;

                                        
                                            if (customizeWfFrom == "Trans")
                                            {
                                                docObj1.IsUseCustomWfFromTrans = true;
                                            }
                                            else
                                            {
                                                docObj1.IsUseIsUseCustomWfFromObj = true;
                                            }

                                            this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, docObj1, previousstep, DateTime.Now, objType, customizeWfFrom);
                                       

                                        this.documentService.Update(docObj1);
                                    }
                                }

                                // Update attach workflow status for contractor doc file, and PECC2 incoming trans
                                foreach (var contractorTransDocFile in contractorTransDocFileList)
                                {
                                    contractorTransDocFile.IsAttachWorkflow = true;
                                    this.contractorTransmittalDocFileService.Update(contractorTransDocFile);
                                }

                                pecc2IncomingTrans.IsAttachWorkflow = true;
                                this.pecc2TransmittalService.Update(pecc2IncomingTrans);
                                // -------------------------------------------------------------------------------------------------
                                break;
                        }
                        break;
                    // Change request
                    case 2:
                        var pecc2TransObj1 = this.pecc2TransmittalService.GetById(this.ObjId);
                        if (pecc2TransObj1 != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            var previousstep = new ObjectAssignedWorkflow();
                            //this.ProcessOriginalWorkflow(wfFirstStepObj, wfObj, ncrsiObj, previousstep, DateTime.Now, objType);

                            // Update docobj workflow status
                            pecc2TransObj1.IsAttachWorkflow = true;
                            pecc2TransObj1.IsInWFProcess = true;
                            pecc2TransObj1.IsWFComplete = false;
                            pecc2TransObj1.CurrentWorkflowName = wfObj.Name;
                            pecc2TransObj1.CurrentWorkflowStepName = wfFirstStepObj.Name;

                            // Use customize workflow

                            if (customizeWfFrom == "Trans")
                            {
                                pecc2TransObj1.IsUseCustomWf = true;
                            }
                            else
                            {
                                pecc2TransObj1.IsUseCustomWf = true;
                            }

                            this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, pecc2TransObj1, previousstep, DateTime.Now, objType, customizeWfFrom);


                            this.pecc2TransmittalService.Update(pecc2TransObj1);
                        }

                        break;
                    // NCR/SI/CS
                    case 3:
                        var ncrsiObj = this.ncrSiService.GetById(this.ObjId);
                        if (ncrsiObj != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            var previousstep = new ObjectAssignedWorkflow();
                            //this.ProcessOriginalWorkflow(wfFirstStepObj, wfObj, ncrsiObj, previousstep, DateTime.Now, objType);

                            // Update docobj workflow status
                            ncrsiObj.IsInWFProcess = true;
                            ncrsiObj.IsWFComplete = false;
                            ncrsiObj.IsAttachWorkflow = true;
                            ncrsiObj.CurrentWorkflowName = wfObj.Name;
                            ncrsiObj.CurrentWorkflowStepName = wfFirstStepObj.Name;

                            // Use customize workflow
                           
                                if (customizeWfFrom == "Trans")
                                {
                                    ncrsiObj.IsUseCustomWfFromTrans = true;
                                }
                                else
                                {
                                    ncrsiObj.IsUseIsUseCustomWfFromObj = true;
                                }

                                this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, ncrsiObj, previousstep, DateTime.Now, objType, customizeWfFrom);
                            this.ncrSiService.Update(ncrsiObj);

                            var wfDetailObj =
                                    this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfFirstStepObj.ID,
                                        ncrsiObj.ID);
                            if (wfDetailObj != null && wfDetailObj.RejectWorkflowStepID != null && !ncrsiObj.Number.Contains("-CS-"))
                            {
                                foreach (var PrwfDetailObj in
                                   this.customizeWorkflowDetailService.GetAllByStepWorkflowAndObj(wfObj.ID,
                                       ncrsiObj.ID))
                                {
                                    var step = this.wfStepService.GetById(PrwfDetailObj.CurrentWorkflowStepID.GetValueOrDefault());
                                    if (step != null && step.IsCreated.GetValueOrDefault())
                                    {
                                        PrwfDetailObj.ReviewUserIds = UserSession.Current.User.Id + ";";
                                        this.customizeWorkflowDetailService.Update(PrwfDetailObj);
                                    }
                                }
                            }

                           
                        }
                        break;
                    // Attach trans
                    case 4:
                        var pecc2TransObj = this.pecc2TransmittalService.GetById(this.ObjId);
                        if (pecc2TransObj != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            var previousstep = new ObjectAssignedWorkflow();
                            //this.ProcessOriginalWorkflow(wfFirstStepObj, wfObj, ncrsiObj, previousstep, DateTime.Now, objType);

                            // Update docobj workflow status
                            pecc2TransObj.IsAttachWorkflow = true;
                            pecc2TransObj.IsInWFProcess = true;
                            pecc2TransObj.IsWFComplete = false;
                            pecc2TransObj.CurrentWorkflowName = wfObj.Name;
                            pecc2TransObj.CurrentWorkflowStepName = wfFirstStepObj.Name;

                            // Use customize workflow
                          
                                if (customizeWfFrom == "Trans")
                                {
                                    pecc2TransObj.IsUseCustomWf = true;
                                }
                                else
                                {
                                    pecc2TransObj.IsUseCustomWf = true;
                                }

                                this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, pecc2TransObj, previousstep, DateTime.Now, objType, customizeWfFrom);
                           

                            this.pecc2TransmittalService.Update(pecc2TransObj);
                        }
                        break;
                    case 5:
                        var RFIObj = this.rfiService.GetById(this.ObjId);
                        if (RFIObj != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            var previousstep = new ObjectAssignedWorkflow();
                            //this.ProcessOriginalWorkflow(wfFirstStepObj, wfObj, ncrsiObj, previousstep, DateTime.Now, objType);

                            // Update docobj workflow status
                            RFIObj.IsInWFProcess = true;
                            RFIObj.IsWFComplete = false;
                            RFIObj.IsAttachWorkflow = true;
                            RFIObj.CurrentWorkflowName = wfObj.Name;
                            RFIObj.CurrentWorkflowStepName = wfFirstStepObj.Name;

                            // Use customize workflow

                            if (customizeWfFrom == "Trans")
                            {
                                RFIObj.IsUseCustomWfFromTrans = true;
                            }
                            else
                            {
                                RFIObj.IsUseIsUseCustomWfFromObj = true;
                            }

                            this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, RFIObj, previousstep, DateTime.Now, objType, customizeWfFrom);


                            this.rfiService.Update(RFIObj);
                        }
                        break;
                    case 6:
                        var ShipObj = this.shipmentService.GetById(this.ObjId);
                        if (ShipObj != null)
                        {
                            var wfFirstStepObj = this.wfStepService.GetFirstStep(wfObj.ID);
                            var previousstep = new ObjectAssignedWorkflow();
                            ShipObj.IsInWFProcess = true;
                            ShipObj.IsWFComplete = false;
                            ShipObj.IsAttachWorkflow = true;
                            ShipObj.CurrentWorkflowName = wfObj.Name;
                            ShipObj.CurrentWorkflowStepName = wfFirstStepObj.Name;

                            // Use customize workflow

                            ShipObj.IsUseCustomWfFromTrans = false;
                            ShipObj.IsUseIsUseCustomWfFromObj = true;
                          

                            this.ProcessCustomizeWorkflow(wfFirstStepObj, wfObj, ShipObj, previousstep, DateTime.Now, objType, customizeWfFrom);


                            this.shipmentService.Update(ShipObj);
                        }
                        break;
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "CloseModal();", true);
        }


        private void ProcessCustomizeWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj,
            ObjectAssignedWorkflow ObjAssignWF, DateTime CurrenDate, int objType, string customizeWfFrom)
        {
            var groupId = 0;
            CustomizeWorkflowDetail wfDetailObj = null;
            if (customizeWfFrom == "Trans")
            {
                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        if (docObj.IncomingTransId != null)
                        {
                            wfDetailObj = this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                    docObj.IncomingTransId.GetValueOrDefault());
                        }
                        break;
                    case 2:
                    case 4:
                        var pecc2TransObj = (PECC2Transmittal)obj;
                        wfDetailObj = this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                pecc2TransObj.ID);
                        break;
                }
            }
            else
            {
                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        if (docObj.IncomingTransId != null)
                        {
                            wfDetailObj =
                                this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                    docObj.ID);
                        }
                        break;

                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        wfDetailObj = this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                ncrsiObj.ID);
                        break;
                    case 2:
                    case 4:
                        var pecc2TransObj = (PECC2Transmittal)obj;
                        wfDetailObj = this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                pecc2TransObj.ID);
                        break;
                    case 5:
                        var rfiObj = (RFI)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                rfiObj.ID);
                        break;
                    case 6:
                        var shipmentObj = (Shipment)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                shipmentObj.ID);
                        break;
                }
            }

            if (wfDetailObj != null)
            {
                var assignWorkFlow = new ObjectAssignedWorkflow
                {
                    ID = Guid.NewGuid(),
                    WorkflowID = wfObj.ID,
                    WorkflowName = wfObj.Name,
                    ObjectWFDwtailId = wfDetailObj.ID,

                    CurrentWorkflowStepID = wfDetailObj.CurrentWorkflowStepID,
                    CurrentWorkflowStepName = wfDetailObj.CurrentWorkflowStepName,
                    NextWorkflowStepID = wfDetailObj.NextWorkflowStepID,
                    NextWorkflowStepName = wfDetailObj.NextWorkflowStepName,
                    RejectWorkflowStepID = wfDetailObj.RejectWorkflowStepID,
                    RejectWorkflowStepName = wfDetailObj.RejectWorkflowStepName,
                    IsComplete = false,
                    IsReject = false,
                    IsLeaf = wfStepObj.IsFirst.GetValueOrDefault(),
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,


                };

                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        assignWorkFlow.ObjectID = docObj.ID;
                        assignWorkFlow.ObjectNumber = docObj.DocNo;
                        assignWorkFlow.ObjectTitle = docObj.DocTitle;
                        assignWorkFlow.ObjectProject = docObj.ProjectName;
                        assignWorkFlow.ObjectType = "Project's Document";
                        groupId = docObj.GroupId.GetValueOrDefault();
                        break;
                    case 2:
                        var pecc2TransObj1 = (PECC2Transmittal)obj;
                        assignWorkFlow.ObjectID = pecc2TransObj1.ID;
                        assignWorkFlow.ObjectNumber = pecc2TransObj1.TransmittalNo;
                        assignWorkFlow.ObjectTitle = pecc2TransObj1.Description;
                        assignWorkFlow.ObjectProject = pecc2TransObj1.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Change Request";
                        groupId = pecc2TransObj1.GroupId.GetValueOrDefault();

                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        assignWorkFlow.ObjectID = ncrsiObj.ID;
                        assignWorkFlow.ObjectNumber = ncrsiObj.Number;
                        assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
                        assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
                        assignWorkFlow.ObjectType = ncrsiObj.Number.Contains("-CS-") ? "CS" : "NCR/SI";
                        groupId = ncrsiObj.GroupId.GetValueOrDefault();

                        break;
                    case 4:
                        var pecc2TransObj = (PECC2Transmittal)obj;
                        assignWorkFlow.ObjectID = pecc2TransObj.ID;
                        assignWorkFlow.ObjectNumber = pecc2TransObj.TransmittalNo;
                        assignWorkFlow.ObjectTitle = pecc2TransObj.Description;
                        assignWorkFlow.ObjectProject = pecc2TransObj.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Document Transmittal";
                        groupId = pecc2TransObj.GroupId.GetValueOrDefault();
                        break;
                    case 5:
                        var rfiObj = (RFI)obj;
                        assignWorkFlow.ObjectID = rfiObj.ID;
                        assignWorkFlow.ObjectNumber = rfiObj.Number;
                        assignWorkFlow.ObjectTitle = rfiObj.Description;
                        assignWorkFlow.ObjectProject = rfiObj.ProjectCode;
                        assignWorkFlow.ObjectType = "RFI";
                        groupId = rfiObj.GroupId.GetValueOrDefault();

                        break;
                    case 6:
                        var ShipObj = (Shipment)obj;
                        assignWorkFlow.ObjectID = ShipObj.ID;
                        assignWorkFlow.ObjectNumber = ShipObj.Number;
                        assignWorkFlow.ObjectTitle = ShipObj.Description;
                        assignWorkFlow.ObjectProject = ShipObj.ProjectName;
                        assignWorkFlow.ObjectType = "Shipment";
                        // groupId = ShipObj.GroupId.GetValueOrDefault();

                        break;
                }


                if (ObjAssignWF != null)
                {
                    assignWorkFlow.PreviousStepId = ObjAssignWF.ID;
                }

                var assignWorkflowId = this.objAssignedWfService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    // Get actual deadline if workflow step detail use only working day
                    var actualDeadline = CurrenDate;
                    if (wfDetailObj.Duration.GetValueOrDefault() < 1)
                    {
                        actualDeadline = actualDeadline.AddDays((double)wfDetailObj.Duration.GetValueOrDefault());
                    }
                    else
                    {
                        if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                        {
                            for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                            {
                                actualDeadline = this.GetNextWorkingDay(actualDeadline);
                            }
                        }
                    }
                    // -------------------------------------------------------------------------
                    var listUserAction = new List<User>();
                    // Get assign User List
                    var wfStepWorkingAssignUser = new List<User>();
                    var infoUserIds = wfDetailObj.InformationOnlyUserIDs != null
                        ? wfDetailObj.InformationOnlyUserIDs.Split(';').ToList()
                        : new List<string>();
                    var reviewUserIds = wfDetailObj.ReviewUserIds != null
                        ? wfDetailObj.ReviewUserIds.Split(';').ToList()
                        : new List<string>();
                    var consolidateUserIds = wfDetailObj.ConsolidateUserIds != null
                        ? wfDetailObj.ConsolidateUserIds.Split(';').ToList()
                        : new List<string>();
                    var approveUserIds = wfDetailObj.ApproveUserIds != null
                        ? wfDetailObj.ApproveUserIds.Split(';').ToList()
                        : new List<string>();

                    var matrixList =
                       this.matrixService.GetAllByList(wfDetailObj.DistributionMatrixIDs.Split(';')
                            .Where(t => !string.IsNullOrEmpty(t))
                            .Select(t => Convert.ToInt32(t)).ToList());
                    foreach (var matrix in matrixList)
                    {
                        var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID).Where(t => t.GroupCodeId == groupId);
                        var acceptAction = wfStepObj.ActionApplyCode.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList();
                        var matrixDetailValid = matrixDetailList.Where(t => acceptAction.Contains(t.ActionTypeName)).ToList();

                        reviewUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 2).Select(t => t.UserId.ToString()));
                        consolidateUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 3).Select(t => t.UserId.ToString()));
                        approveUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 4).Select(t => t.UserId.ToString()));
                        infoUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 1).Select(t => t.UserId.ToString()));
                    }

                    foreach (
                       var userId in
                           infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null && !wfStepWorkingAssignUser.Any(t => t.Id == userObj.Id && t.ActionTypeId == 1))
                        {
                            userObj.ActionTypeId = 1;
                            userObj.ActionTypeName = "I - For Information";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (
                        var userId in
                            reviewUserIds.Distinct()
                                .Where(t => !string.IsNullOrEmpty(t))
                                .Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null && !wfStepWorkingAssignUser.Any(t => t.Id == userObj.Id && t.ActionTypeId == 2))
                        {
                            userObj.ActionTypeId = 2;
                            userObj.ActionTypeName = "R - Review";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (
                        var userId in
                            consolidateUserIds.Distinct()
                                .Where(t => !string.IsNullOrEmpty(t))
                                .Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null && !wfStepWorkingAssignUser.Any(t => t.Id == userObj.Id && t.ActionTypeId == 3))
                        {
                            userObj.ActionTypeId = 3;
                            userObj.ActionTypeName = "C - Consolidate";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }



                    foreach (
                        var userId in
                            approveUserIds.Distinct()
                                .Where(t => !string.IsNullOrEmpty(t))
                                .Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null && !wfStepWorkingAssignUser.Any(t => t.Id == userObj.Id && t.ActionTypeId == 4))
                        {
                            userObj.ActionTypeId = 4;
                            userObj.ActionTypeName = "A - Approve";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }


                    // ---------------------------------------------------------------------------
                    var assignWorkingUserInfor = new ObjectAssignedUser();
                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = user.Id,
                            UserFullName = user.FullName,
                            ReceivedDate = CurrenDate,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = user.ActionTypeId == 1,// && wfDetailObj.IsFirst.GetValueOrDefault(),
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = user.ActionTypeId,
                            ActionTypeName = user.ActionTypeName,
                            WorkingStatus = string.Empty,
                            //Status = (user.ActionTypeId == 1 && wfDetailObj.IsFirst.GetValueOrDefault()) ? "SO" : wfDetailObj.IsFirst.GetValueOrDefault() ? "RS" : "NR",

                            IsMainWorkflow = true,
                            IsReassign = false,
                            IsLeaf = wfStepObj.IsFirst.GetValueOrDefault()
                        };

                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                assignWorkingUser.ObjectID = docObj.ID;
                                assignWorkingUser.ObjectNumber = docObj.DocNo;
                                assignWorkingUser.ObjectTitle = docObj.DocTitle;
                                assignWorkingUser.ObjectProject = docObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = docObj.ProjectId;
                                assignWorkingUser.Revision = docObj.Revision;
                                assignWorkingUser.Categoryid = docObj.CategoryId;
                                assignWorkingUser.ObjectType = "Project's Document";
                                assignWorkingUser.ObjectTypeId = 1;
                                break;
                            case 2:
                                var pecc2TransObj1 = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = pecc2TransObj1.ID;
                                assignWorkingUser.ObjectNumber = pecc2TransObj1.TransmittalNo;
                                assignWorkingUser.ObjectTitle = pecc2TransObj1.Description;
                                assignWorkingUser.ObjectProject = pecc2TransObj1.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = pecc2TransObj1.ProjectCodeId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Change Request";
                                assignWorkingUser.ObjectTypeId = 2;
                                break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Subject;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = ncrsiObj.ProjectId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = ncrsiObj.Number.Contains("-CS-") ? "CS" : "NCR/SI";
                                assignWorkingUser.ObjectTypeId = 3;
                                break;
                            case 4:
                                var pecc2TransObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = pecc2TransObj.ID;
                                assignWorkingUser.ObjectNumber = pecc2TransObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = pecc2TransObj.Description;
                                assignWorkingUser.ObjectProject = pecc2TransObj.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = pecc2TransObj.ProjectCodeId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Document Transmittal";
                                assignWorkingUser.ObjectTypeId = 4;
                                break;
                            case 5:
                                var rfiObj = (RFI)obj;
                                assignWorkingUser.ObjectID = rfiObj.ID;
                                assignWorkingUser.ObjectNumber = rfiObj.Number;
                                assignWorkingUser.ObjectTitle = rfiObj.Description;
                                assignWorkingUser.ObjectProject = rfiObj.ProjectCode;
                                assignWorkingUser.ObjectProjectId = rfiObj.ProjectId;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "RFI";
                                assignWorkingUser.ObjectTypeId = 5;
                                break;
                            case 6:
                                var shipObj = (Shipment)obj;
                                assignWorkingUser.ObjectID = shipObj.ID;
                                assignWorkingUser.ObjectNumber = shipObj.Number;
                                assignWorkingUser.ObjectTitle = shipObj.Description;
                                assignWorkingUser.ObjectProject = shipObj.ProjectName;
                                assignWorkingUser.ObjectProjectId = shipObj.ProjectID;
                                assignWorkingUser.Categoryid = 0;
                                assignWorkingUser.ObjectType = "Shipment";
                                assignWorkingUser.ObjectTypeId = 6;
                                break;
                        }
                        if (objAssignedUserService.GetCurrentInCompleteByDocUseraction(assignWorkingUser.UserID.GetValueOrDefault(), assignWorkingUser.ObjectID.GetValueOrDefault(), assignWorkingUser.ObjectTypeId.GetValueOrDefault(), assignWorkingUser.ActionTypeId.GetValueOrDefault()) == null)
                        {
                            objAssignedUserService.Insert(assignWorkingUser);
                            assignWorkingUserInfor = assignWorkingUser;
                            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
                                 user.ActionTypeId != 1)
                            {
                                switch (objType)
                                {
                                    case 4:
                                    //this.SendNotification(assignWorkingUser, user);
                                    //break;
                                    case 2:
                                    //  this.SendNotificationFCRDCN(assignWorkingUser, user);
                                    // break;
                                    case 3:
                                        // this.SendNotificatioNCRSI(assignWorkingUser, user);
                                       
                                       // break;
                                    case 5:
                                        //this.SendNotificationRFI(assignWorkingUser, user);
                                        listUserAction.Add(user);
                                        break;
                                    case 6:
                                        this.SendNotificationShipment(assignWorkingUser, user);
                                        break;
                                }
                            }
                        }
                    }

                    // Assign to re-assign user of workflow when can't find working user
                    if (wfStepWorkingAssignUser.Count == 0 && wfObj.Re_assignUserId != null)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = wfObj.Re_assignUserId,
                            UserFullName = wfObj.Re_assignUserName,
                            ReceivedDate = CurrenDate,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = false,
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            WorkflowId = wfObj.ID,
                            WorkflowName = wfObj.Name,
                            CurrentWorkflowStepName = wfStepObj.Name,
                            CurrentWorkflowStepId = wfStepObj.ID,
                            CanReject = wfStepObj.CanReject,
                            IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                            IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                            ActionTypeId = 5,
                            ActionTypeName = "Can't find working user. Re-assign to another user.",
                            WorkingStatus = string.Empty,
                            //Status = "RS",
                            IsMainWorkflow = true,
                            IsReassign = true
                        };

                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                assignWorkingUser.ObjectID = docObj.ID;
                                assignWorkingUser.ObjectNumber = docObj.DocNo;
                                assignWorkingUser.ObjectTitle = docObj.DocTitle;
                                assignWorkingUser.ObjectProject = docObj.ProjectName;
                                assignWorkingUser.Revision = docObj.Revision;
                                break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Description;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                break;
                            case 2:
                            case 4:
                                var pecc2TransObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = pecc2TransObj.ID;
                                assignWorkingUser.ObjectNumber = pecc2TransObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = pecc2TransObj.Description;
                                assignWorkingUser.ObjectProject = pecc2TransObj.ProjectCodeName;
                                break;
                            case 5:
                                var rfiObj = (RFI)obj;
                                assignWorkingUser.ObjectID = rfiObj.ID;
                                assignWorkingUser.ObjectNumber = rfiObj.Number;
                                assignWorkingUser.ObjectTitle = rfiObj.Description;
                                assignWorkingUser.ObjectProject = rfiObj.ProjectCode;
                                break;
                            case 6:
                                var ShipObj = (Shipment)obj;
                                assignWorkingUser.ObjectID = ShipObj.ID;
                                assignWorkingUser.ObjectNumber = ShipObj.Number;
                                assignWorkingUser.ObjectTitle = ShipObj.Description;
                                assignWorkingUser.ObjectProject = ShipObj.ProjectName;
                                break;
                        }

                        objAssignedUserService.Insert(assignWorkingUser);
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                            switch (objType)
                            {
                                case 4:
                                //    this.SendNotification(assignWorkingUser,
                                //this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()));
                                //    break;
                                case 2:
                                //this.SendNotificationFCRDCN(assignWorkingUser, this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()));

                                // break;
                                case 3:
                                    // this.SendNotificatioNCRSI(assignWorkingUser, this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()));
                                   
                                    //break;
                                case 5:
                                    // this.SendNotificationRFI(assignWorkingUser, this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()));
                                    listUserAction.Add(this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()));
                                    break;
                                case 6:
                                    this.SendNotificationShipment(assignWorkingUser, this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()));
                                    break;
                            }
                    }
                    // ----------------------------------------------------------------------------------
                    if (wfDetailObj.NextWorkflowStepID == null || wfDetailObj.NextWorkflowStepID == 0)
                    {
                        switch (objType)
                        {
                            case 1:
                                var docObj = (PECC2Documents)obj;
                                docObj.IsInWFProcess = false;
                                docObj.IsWFComplete = true;
                                this.documentService.Update(docObj);
                                break;
                            //case 2:
                            //    var changeRequestObj = (ChangeRequest)obj;
                            //    changeRequestObj.IsInWFProcess = true;
                            //    changeRequestObj.IsWFComplete = false;
                            //    changeRequestObj.CurrentWorkflowName = wfObj.Name;
                            //    changeRequestObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            //    changeRequestObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            //    this.changeRequestService.Update(changeRequestObj);
                            //    break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                ncrsiObj.IsInWFProcess = false;
                                ncrsiObj.IsWFComplete = true;
                                this.ncrSiService.Update(ncrsiObj);
                                break;
                            case 2:
                            case 4:
                                var incomingTransObj = (PECC2Transmittal)obj;
                                incomingTransObj.IsInWFProcess = false;
                                incomingTransObj.IsWFComplete = true;
                                this.pecc2TransmittalService.Update(incomingTransObj);
                                break;
                            case 5:
                                var rfiObj = (RFI)obj;
                                rfiObj.IsInWFProcess = false;
                                rfiObj.IsWFComplete = true;
                                this.rfiService.Update(rfiObj);
                                break;
                            case 6:
                                var ShipObj = (Shipment)obj;
                                ShipObj.IsInWFProcess = false;
                                ShipObj.IsWFComplete = true;
                                this.shipmentService.Update(ShipObj);
                                break;

                        }
                    }
                    // Send notification for Info & Management user
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && listUserAction.Count > 0)
                    {
                        switch (objType)
                        {
                            case 3:
                                this.SendNotificatioNCRSI(assignWorkingUserInfor, listUserAction, infoUserIds);

                                break;
                            case 2:
                            case 4:
                                this.SendNotification(assignWorkingUserInfor, listUserAction, infoUserIds);

                                break;
                            case 5:
                                this.SendNotificationRFI(assignWorkingUserInfor, listUserAction, infoUserIds);
                                break;
                        }
                    }
                    else if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && listUserAction.Count == 0 && infoUserIds.Count > 0)
                    {
                        switch (objType)
                        {
                            case 4:
                            case 2:
                                this.SendNotificationInfor(assignWorkingUserInfor, infoUserIds);
                                break;

                            //    this.SendNotificationInforFCRDCN(assignWorkingUserInfor,
                            //this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()), infoUserIds);
                            //    break;
                            case 3:
                                this.SendNotificationInforNCRSI(assignWorkingUserInfor, infoUserIds);
                                break;
                            case 5:
                                this.SendNotificationInforRFI(assignWorkingUserInfor, infoUserIds);
                                break;
                            case 6:
                                this.SendNotificationInforShipment(assignWorkingUserInfor,
                            this.userService.GetByID(wfObj.Re_assignUserId.GetValueOrDefault()), infoUserIds);
                                break;
                        }
                    }

                    //var wfNextStepObj = this.wfStepService.GetById(wfDetailObj.NextWorkflowStepID.GetValueOrDefault());
                    //if (wfNextStepObj != null)
                    //{
                    //    this.ProcessWorkflow(wfNextStepObj, wfObj, obj, assignWorkFlow, actualDeadline, objType);
                    //}
                }
            }
        }

        private void SendNotification(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj, List<string> infoUser)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var transObj = this.pecc2TransmittalService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var projctobj = this.projectCodeService.GetById(assignWorkingUser.ObjectProjectId.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + ", " + assignWorkingUser.ObjectTitle;
                // Generate email body
                // var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                // var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                // var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var emailto = string.Empty;
                var Userlist = assignUserObj.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                foreach (var user in Userlist)
                {
                    try
                    {
                        if (user.Email.Contains(";"))
                        {
                            foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                            {
                                message.To.Add(new MailAddress(stemail.Trim()));
                            }
                        }
                        else
                        {
                            message.To.Add(new MailAddress(user.Email.Trim()));
                        }
                        emailto += user.Email + "; ";
                    }
                    catch { }
                }

                var emailCC = string.Empty;
                foreach (var userId in infoUser.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            if (userObj.Email.Contains(";"))
                            {
                                foreach (string stemail in userObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.CC.Add(new MailAddress(stemail.Trim()));
                                }
                            }
                            else
                            {
                                message.CC.Add(new MailAddress(userObj.Email.Trim()));
                            }
                            emailCC += userObj.Email + "; ";
                        }
                    }
                }
                var bodyContent = @"<head><title></title><style>
					body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
					</style></head>
				<body>
                <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
									<br><b>New To-do-list :</b> Transmittal <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>action</b>
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:red'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal No.</span></td><td class='font_s' style='color:red' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Priority</span></td><td class='font_s' style='color:#003399'>" + transObj.Priority + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Subject</span></td><td class='font_s' colspan='3'>" + assignWorkingUser.ObjectTitle + @"</td>
                   
                  </tr>
                  </table>
                  </div>";

                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx?TransNoContractor=" + assignWorkingUser.ObjectNumber;
                bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this transmittal</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(emailto))
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }
        private void SendNotificationInfor(ObjectAssignedUser assignWorkingUser, List<string> infoUserIds)
        {
            try
            {
                // Implement send mail function
                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var transObj = this.pecc2TransmittalService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var projctobj = this.projectCodeService.GetById(assignWorkingUser.ObjectProjectId.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYI:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + ", " + assignWorkingUser.ObjectTitle;
                // Generate email body
                // var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                //  var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                // var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var emailto = string.Empty;

                foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            if (userObj.Email.Contains(";"))
                            {
                                foreach (string stemail in userObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.To.Add(new MailAddress(stemail.Trim()));
                                }
                            }
                            else
                            {
                                message.To.Add(new MailAddress(userObj.Email.Trim()));
                            }
                            emailto += userObj.Email + "; ";
                        }
                    }
                }
                var bodyContent = @"<head><title></title><style>
					body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
					</style></head>
				<body>
                <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
									<br><b>New To-do-list :</b> Transmittal <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>information</b>
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:#003399'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal No.</span></td><td class='font_s' style='color:red' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Priority</span></td><td class='font_s' style='color:#003399'>" + transObj.Priority + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Subject</span></td><td class='font_s' colspan='3'>" + assignWorkingUser.ObjectTitle + @"</td>
                   
                  </tr>
                  </table>
                  </div>";

                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx?TransNoContractor=" + assignWorkingUser.ObjectNumber;
                bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this transmittal</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(emailto))
                {
                    smtpClient.Send(message);
                }
            }
            catch { }
        }
        private void SendNotificationFCRDCN(ObjectAssignedUser assignWorkingUser, User assignUserObj)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var transObj = this.pecc2TransmittalService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + assignWorkingUser.ObjectTitle;
                // Generate email body
                var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var changerequestObj = this.changeRequestService.GetAllByIncomingTrans(assignWorkingUser.ObjectID.GetValueOrDefault()).FirstOrDefault();
                var bodyContent = @"<head><title></title><style>
                        body {font-family:Calibri;font-size:10px;}
                        hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                        .msg {font-size:16px;}
                        table {border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                        td {border:1px solid #ACCEF5;}
                        .span1 {font-size:16px;}
                        .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                        .ch2 {background-color:#F7FAFF;padding:5px;}
                        a {color:mediumblue;}
                        .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                        .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                        .link {font-size:16px;margin-left:30px;}
                        .footer {color:darkgray; font-size:12px;}
                        /*TYPE OF NOTIFICATION PURPOSE*/
                        .action {background-color:#fffda5;}
                        .info {background-color:#d1fcbd;}
                        .overdue {background-color:#f00;color:white;font-weight:bold;}
                        </style></head>
                        <body>
                        <h2 style='font-family:'Arial Rounded MT Bold';'><u>Workflow Notification</u></h2>
                        <span class='msg'>Dear " + (assignUserObj.FullName.Contains(" - ") ? assignUserObj.FullName.Split('-')[0] : assignUserObj.FullName) + @",
                        <br /><br />Please be informed that the following workflow notification details from DMDC System:
                        </span>
                        <br /><br />
                        <table border='1'>
                        <tr><td colspan='6' class='ch1'>Workflow Step Details</td></tr>
                        <tr>
                            <td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='action'>For Your Action</td>
                            <td class='ch2'>Notification Description</td><td class='ch2'>:</td><td>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                        </tr>
                        <tr>
                            <td class='ch2'>Started Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                            <td class='ch2'>Target Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"(" + wfdetail.Duration + @" Days Duration)</td>
                            </tr>
                            <tr>
                                <td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>This is first step</td>
                            <td class='ch2'>Next Step</td><td class='ch2'>:</td><td>" + nextStep?.Name + @"</td>
                        </tr>
                        <tr><td colspan = '6' class='ch1'>DCN/FCR Details</td></tr>
                        <tr>
                            <td class='ch2'>DCN/FCR Number</td><td class='ch2'>:</td><td>" + changerequestObj?.Number + @"</td>
                            <td class='ch2'>Remarks/ Title</td><td class='ch2'>:</td><td>" + changerequestObj?.Description + @"</td>
                        </tr>
                        <tr>
                            <td class='ch2'>Rev</td><td class='ch2'>:</td><td>" + changerequestObj?.Revision + @"</td>
                            <td class='ch2'>Grade</td><td class='ch2'>:</td><td>" +(changerequestObj.ChangeGradeCodeId!= null? this.changeGradeCodeService.GetById(changerequestObj.ChangeGradeCodeId.GetValueOrDefault()).FullName:"") + @"</td>
                        </tr>
                        <tr><td colspan = '6' class='ch1'>Reference Documents</td></tr>
                        <tr>
                            <td colspan = '2' class='ch2'>Document No.</td>
                            <td class='ch2'>Rev.</td>
                            <td colspan = '3' class='ch2'>Remarks/ Title</td>
                        </tr>";
                var Listdocrefer = this.documentService.GetAllByReferChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

                List<string> ListDoc = new List<string>();
                foreach (var document in Listdocrefer)
                {
                    if (!ListDoc.Where(t => t == document.DocNo).Any())
                    {
                        ListDoc.Add(document.DocNo);
                        bodyContent += @"<tr>
                               <td colspan='2'>" + document.DocNo + @"</td>

                               <td>"
                                       + document.Revision + @"</td>
                               <td colspan='3'>"
                                       + document.DocTitle + @"</td></tr>";
                    }
                }
                bodyContent += @"<tr><td colspan='6' class='ch1'>Documents To Be Revised</td></tr>
                                <tr>
                                    <td colspan='2' class='ch2'>Document No.</td>
                                    <td class='ch2'>Rev.</td>
                                    <td colspan='3' class='ch2'>Remarks/ Title</td>
                                </tr>";
                var ListdocRevised = this.documentService.GetAllByRevisedChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

                List<string> ListDoc1 = new List<string>();
                foreach (var document in ListdocRevised)
                {
                    if (!ListDoc1.Where(t => t == document.DocNo).Any())
                    {
                        ListDoc1.Add(document.DocNo);
                        bodyContent += @"<tr>
                               <td colspan='2'>" + document.DocNo + @"</td>

                               <td>"
                                       + document.Revision + @"</td>
                               <td colspan='3'>"
                                       + document.DocTitle + @"</td></tr>";
                    }
                }
                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/ChangeRequestNewList.aspx?FCRDCNNo=" + changerequestObj.Number;
                bodyContent += @"</table>
                            <div class='link'>
                            <br />
                            <u><b>Useful Links:</b></u>
                            <ul>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this DCN/FCR</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
                            </div>
                            <br />
                            <h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
                            <hr />
                            <h3 class='system'>DMDC System</h3>
                            <br />
                            <span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span>
                            </body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(assignUserObj.Email))
                {
                    message.To.Add(assignUserObj.Email);

                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }
        private void SendNotificationInforFCRDCN(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<string> infoUserIds)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var transObj = this.pecc2TransmittalService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYI: " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + assignWorkingUser.ObjectTitle;
                // Generate email body
                var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var changerequestObj = this.changeRequestService.GetAllByIncomingTrans(assignWorkingUser.ObjectID.GetValueOrDefault()).FirstOrDefault();
                var bodyContent = @"<head><title></title><style>
                        body {font-family:Calibri;font-size:10px;}
                        hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                        .msg {font-size:16px;}
                        table {border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                        td {border:1px solid #ACCEF5;}
                        .span1 {font-size:16px;}
                        .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                        .ch2 {background-color:#F7FAFF;padding:5px;}
                        a {color:mediumblue;}
                        .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                        .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                        .link {font-size:16px;margin-left:30px;}
                        .footer {color:darkgray; font-size:12px;}
                        /*TYPE OF NOTIFICATION PURPOSE*/
                        .action {background-color:#fffda5;}
                        .info {background-color:#d1fcbd;}
                        .overdue {background-color:#f00;color:white;font-weight:bold;}
                        </style></head>
                        <body>
                        <h2 style='font-family:'Arial Rounded MT Bold';'><u>Workflow Notification</u></h2>
                        <span class='msg'>Dear All,
                        <br /><br />Please be informed that the following workflow notification details from DMDC System:
                        </span>
                        <br /><br />
                        <table border='1'>
                        <tr><td colspan='6' class='ch1'>Workflow Step Details</td></tr>
                        <tr>
                            <td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='info'>For Your Information</td>
                            <td class='ch2'>Notification Description</td><td class='ch2'>:</td><td>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                        </tr>
                        <tr>
                            <td class='ch2'>Started Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                            <td class='ch2'>Target Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"(" + wfdetail.Duration + @" Days Duration)</td>
                            </tr>
                            <tr>
                                <td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>This is first step</td>
                            <td class='ch2'>Next Step</td><td class='ch2'>:</td><td>" + nextStep?.Name + @"</td>
                        </tr>
                        <tr><td colspan = '6' class='ch1'>DCN/FCR Details</td></tr>
                        <tr>
                            <td class='ch2'>DCN/FCR Number</td><td class='ch2'>:</td><td>" + changerequestObj?.Number + @"</td>
                            <td class='ch2'>Remarks/ Title</td><td class='ch2'>:</td><td>" + changerequestObj?.Description + @"</td>
                        </tr>
                        <tr>
                            <td class='ch2'>Rev</td><td class='ch2'>:</td><td>" + changerequestObj?.Revision + @"</td>
                            <td class='ch2'>Grade</td><td class='ch2'>:</td><td>" + (changerequestObj.ChangeGradeCodeId != null ? this.changeGradeCodeService.GetById(changerequestObj.ChangeGradeCodeId.GetValueOrDefault()).FullName : "") + @"</td>
                        </tr>
                        <tr><td colspan = '6' class='ch1'>Reference Documents</td></tr>
                        <tr>
                            <td colspan = '2' class='ch2'>Document No.</td>
                            <td class='ch2'>Rev.</td>
                            <td colspan = '3' class='ch2'>Remarks/ Title</td>
                        </tr>";
                var Listdocrefer = this.documentService.GetAllByReferChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

                List<string> ListDoc = new List<string>();
                foreach (var document in Listdocrefer)
                {
                    if (!ListDoc.Where(t => t == document.DocNo).Any())
                    {
                        ListDoc.Add(document.DocNo);
                        bodyContent += @"<tr>
                               <td colspan='2'>" + document.DocNo + @"</td>

                               <td>"
                                       + document.Revision + @"</td>
                               <td colspan='3'>"
                                       + document.DocTitle + @"</td></tr>";
                    }
                }
                bodyContent += @"<tr><td colspan='6' class='ch1'>Documents To Be Revised</td></tr>
                                <tr>
                                    <td colspan='2' class='ch2'>Document No.</td>
                                    <td class='ch2'>Rev.</td>
                                    <td colspan='3' class='ch2'>Remarks/ Title</td>
                                </tr>";
                var ListdocRevised = this.documentService.GetAllByRevisedChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

                List<string> ListDoc1 = new List<string>();
                foreach (var document in ListdocRevised)
                {
                    if (!ListDoc1.Where(t => t == document.DocNo).Any())
                    {
                        ListDoc1.Add(document.DocNo);
                        bodyContent += @"<tr>
                               <td colspan='2'>" + document.DocNo + @"</td>

                               <td>"
                                       + document.Revision + @"</td>
                               <td colspan='3'>"
                                       + document.DocTitle + @"</td></tr>";
                    }
                }
                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/ChangeRequestNewList.aspx?FCRDCNNo=" + changerequestObj.Number;
                bodyContent += @"</table>
                            <div class='link'>
                            <br />
                            <u><b>Useful Links:</b></u>
                            <ul>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this DCN/FCR</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
                            </div>
                            <br />
                            <h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
                            <hr />
                            <h3 class='system'>DMDC System</h3>
                            <br />
                            <span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span>
                            </body>";
                message.Body = bodyContent;
                foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email)) message.To.Add(userObj.Email);
                    }
                }
                smtpClient.Send(message);
            }
            catch (Exception ex) { }
        }
        private void SendNotificatioNCRSI(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj, List<string> infoUser)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                // var transObj = this.pecc2TransmittalService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var projctobj = this.projectCodeService.GetById(assignWorkingUser.ObjectProjectId.GetValueOrDefault());
                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + ", " + assignWorkingUser.ObjectTitle;
                // Generate email body
                //var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                // var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                // var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());

                var emailto = string.Empty;
                var Userlist = assignUserObj.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                foreach (var user in Userlist)
                {
                    try
                    {
                        if (user.Email.Contains(";"))
                        {
                            foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                            {
                                message.To.Add(new MailAddress(stemail.Trim()));
                            }
                        }
                        else
                        {
                            message.To.Add(new MailAddress(user.Email.Trim()));
                        }
                        emailto += user.Email + "; ";
                    }
                    catch { }
                }

                var emailCC = string.Empty;
                foreach (var userId in infoUser.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            if (userObj.Email.Contains(";"))
                            {
                                foreach (string stemail in userObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.CC.Add(new MailAddress(stemail.Trim()));
                                }
                            }
                            else
                            {
                                message.CC.Add(new MailAddress(userObj.Email.Trim()));
                            }
                            emailCC += userObj.Email + "; ";
                        }
                    }
                }
                var ncrsiObj = this.ncrsiService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var bodyContent = @"<head><title></title><style>
					body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
					</style></head>
				<body>
                <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
									<br><b>New To-do-list :</b> <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>action</b>
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:red'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>NCR/SI No.</span></td><td class='font_s' style='color:red' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s' style='color:#003399'>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Subject</span></td><td class='font_s' colspan='3'>" + assignWorkingUser.ObjectTitle + @"</td>
                   
                  </tr>
                  </table>
                  </div>";


                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/NCRSINewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
                if (ncrsiObj != null && ncrsiObj.Number.Contains("-CS-"))
                {
                    st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/CSNewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
                }
                bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>" + (ncrsiObj.Number.Contains("-CS-") ? "this CS" : "this NCR/SI") + @"</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(emailto))
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }
        private void SendNotificationInforNCRSI(ObjectAssignedUser assignWorkingUser, List<string> infoUserIds)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                // var transObj = this.pecc2TransmittalService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var projctobj = this.projectCodeService.GetById(assignWorkingUser.ObjectProjectId.GetValueOrDefault());
                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYI:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + ", " + assignWorkingUser.ObjectTitle;
                // Generate email body
                //var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                // var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                // var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());

                var emailto = string.Empty;

                foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            if (userObj.Email.Contains(";"))
                            {
                                foreach (string stemail in userObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.To.Add(new MailAddress(stemail.Trim()));
                                }
                            }
                            else
                            {
                                message.To.Add(new MailAddress(userObj.Email.Trim()));
                            }
                            emailto += userObj.Email + "; "; }
                    }
                }
                var ncrsiObj = this.ncrsiService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var bodyContent = @"<head><title></title><style>
					body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
					</style></head>
				<body>
                <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
									<br><b>New To-do-list :</b> <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>information</b>
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:#003399'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>NCR/SI No.</span></td><td class='font_s' style='color:red' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s' style='color:#003399'>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Subject</span></td><td class='font_s' colspan='3'>" + assignWorkingUser.ObjectTitle + @"</td>
                   
                  </tr>
                  </table>
                  </div>";


                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/NCRSINewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
                if (ncrsiObj != null && ncrsiObj.Number.Contains("-CS-"))
                {
                    st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/CSNewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
                }
                bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>" + (ncrsiObj.Number.Contains("-CS-") ? "this CS" : "this NCR/SI") + @"</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(emailto))
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }

        private void SendNotificationRFI(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj, List<string> infoUser)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var RFIObj = this.rfiService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var projctobj = this.projectCodeService.GetById(assignWorkingUser.ObjectProjectId.GetValueOrDefault());
                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                // Generate email body
                var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
               // var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
               // var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var emailto = string.Empty;
                var Userlist = assignUserObj.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                foreach (var user in Userlist)
                {
                    try
                    {
                        if (user.Email.Contains(";"))
                        {
                            foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                            {
                                message.To.Add(new MailAddress(stemail.Trim()));
                            }
                        }
                        else
                        {
                            message.To.Add(new MailAddress(user.Email.Trim()));
                        }
                        emailto += user.Email + "; ";
                    }
                    catch { }
                }

                var emailCC = string.Empty;
                foreach (var userId in infoUser.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            if (userObj.Email.Contains(";"))
                            {
                                foreach (string stemail in userObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.CC.Add(new MailAddress(stemail.Trim()));
                                }
                            }
                            else
                            {
                                message.CC.Add(new MailAddress(userObj.Email.Trim()));
                            }
                            emailCC += userObj.Email + "; ";
                        }
                    }
                }
                var bodyContent = @"<head><title></title><style>
					body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
					</style></head>
				<body>
                <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
									<br><b>New To-do-list :</b> RFI <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>action</b>
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:red'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>RFI No.</span></td><td class='font_s' style='color:red' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s' style='color:#003399'>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
               
                                <tr><td colspan='4' class='ch1'>RFI Details</td></tr>
                                <tr>
                                    <td class='ch2'>Group</td>
                                    <td colspan='3' class='ch2'>Work Title</td>
                                </tr>";
                var Listdoc = this.rfidetailService.GetByRFI(RFIObj.ID).OrderBy(t => t.Number);

            
                foreach (var document in Listdoc)
                {
                       bodyContent += @"<tr>
                               <td'>" + document.GroupName + @"</td>
                               <td colspan='3'>"
                                       + document.WorkTitle + @"</td></tr>";
                }
                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/RFIList.aspx?RFINo=" + assignWorkingUser.ObjectNumber;
                bodyContent += @"</table>
			                    </div>
                            <p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this RFI</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(emailto))
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }
        private void SendNotificationInforRFI(ObjectAssignedUser assignWorkingUser, List<string> infoUserIds)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var RFIObj = this.rfiService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
                var projctobj = this.projectCodeService.GetById(assignWorkingUser.ObjectProjectId.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYI: " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                // Generate email body
                var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                //  var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                //  var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var emailto = string.Empty;

                foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email))
                        {
                            if (userObj.Email.Contains(";"))
                            {
                                foreach (string stemail in userObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.To.Add(new MailAddress(stemail.Trim()));
                                }
                            }
                            else
                            {
                                message.To.Add(new MailAddress(userObj.Email.Trim()));
                            }
                            emailto += userObj.Email + "; ";
                        }
                    }
                }
                var bodyContent = @"<head><title></title><style>
					body {font-family:Calibri;font-size:10px;}
                hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                .msg {font-size:16px;}                        
                table {width:98.0%;border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                td {border:1px solid #ACCEF5;}
                .span1 {font-size:16px;}
                .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                .ch2 {background-color:#F7FAFF;padding:5px;}
                a {color:mediumblue;}
                .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                .link {font-size:16px;margin-left:30px;}
                .footer {color:darkgray; font-size:12px;}
                /*TYPE OF NOTIFICATION PURPOSE*/
                .action {background-color:#fffda5;}
                .info {background-color:#d1fcbd;}
                .overdue {background-color:#f00;color:white;font-weight:bold;}
                  .header_ {width:50.0%;border:none;border-bottom:solid #98C6EA 1.0pt;mso-border-bottom-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:3.75pt 3.75pt 3.75pt 3.75pt}
                  .footer_ {border:none;border-top:solid #98C6EA 1.0pt;mso-border-top-alt:solid #98C6EA .75pt;background:#D4EFFC;padding:6.0pt 6.0pt 6.0pt 6.0pt}
                  .font_l {font-size:13.5pt;font-family:'Verdana',sans-serif}
                .font_m {font-size:10.0pt;font-family:'Verdana',sans-serif}
                .font_s {font-size:9.0pt;font-family:'Verdana',sans-serif}
                .font_xs {font-size:7.5pt;font-family:'Verdana',sans-serif}
					</style></head>
				<body>
                <table border='1'>
                  <tr>
                                <td width='50%' class='header_'>
								<b><span class='font_m'>" + projctobj.Description + @"</span></b><br>				
								<b><span class='font_xs' style='color:red'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</span></b>
							</td>
							<td width='50%' class='header_'>
								<p class='MsoNormal' align='right' style='text-align:right'>
									<b><span class='font_l' style='color:#000066'>EVN</span></b>
									<em><b><span class='font_l' style='color:red'>PECC2</span></b></em>
								</p>
							</td>
                    </tr>
                  
                  <tr><td colspan='2' > 
                    <p align='center' style='margin-bottom:12.0pt;text-align:center'>
							<span class='font_m'>
									<br><b>New To-do-list :</b> RFI <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>action</b>
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:red'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>RFI No.</span></td><td class='font_s' style='color:red' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s' style='color:#003399'>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
               
                                <tr><td colspan='4' class='ch1'>RFI Details</td></tr>
                                <tr>
                                    <td class='ch2'>Group</td>
                                    <td colspan='3' class='ch2'>Work Title</td>
                                </tr>";
                var Listdoc = this.rfidetailService.GetByRFI(RFIObj.ID).OrderBy(t => t.Number);


                foreach (var document in Listdoc)
                {
                    bodyContent += @"<tr>
                               <td'>" + document.GroupName + @"</td>
                               <td colspan='5'>"
                                    + document.WorkTitle + @"</td></tr>";
                }
                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/RFIList.aspx?RFINo=" + assignWorkingUser.ObjectNumber;
                bodyContent += @"</table>
			                    </div>
                            <p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this RFI</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
					   </span>
						</p>			
						<p  align='center' style='margin-bottom:12.0pt'>
						<span class='font_m'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION. PLEASE DO NOT REPLY.]
						</span>
						</p>
						</td>
						</tr>
						<tr>
							<td class='footer_'>
								<b><span class='font_xs'>Power Engineering Consulting Joint Stock Company 2 (PECC2)</span></b>
							</td>
							<td class='footer_'>
								<p  align=right style='text-align:right'>
									<b><span class='font_xs'>32 Ngo Thoi Nhiem,Ward 7, District 3, Ho Chi Minh City<br>Tel: (84 8) 22.211.057 - Fax: (84 8) 22.210.408 - Email: <a href='mailto:info@pecc2.com'>info@pecc2.com</a> 
									</span></b>
								</p>
							</td>
						</tr>
					</table></body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(emailto))
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }

        private void SendNotificationShipment(ObjectAssignedUser assignWorkingUser, User assignUserObj)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var transmittal = this.shipmentService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                // Generate email body
                var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var bodyContent = @"<head><title></title><style>
                            body {font-family:Calibri;font-size:10px;}
                            hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                            .msg {font-size:16px;}
                            table {border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                            td {border:1px solid #ACCEF5;}
                            .span1 {font-size:16px;}
                            .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                            .ch2 {background-color:#F7FAFF;padding:5px;}
                            a {color:mediumblue;}
                            .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                            .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                            .link {font-size:16px;margin-left:30px;}
                            .footer {color:darkgray; font-size:12px;}
                            /*TYPE OF NOTIFICATION PURPOSE*/
                            .action {background-color:#fffda5;}
                            .info {background-color:#d1fcbd;}
                            .overdue {background-color:#f00;color:white;font-weight:bold;}
                            </style></head>
                            <body>
                         <h2 style='font-family:'Arial Rounded MT Bold';'><u>Workflow Notification</u></h2>
                <span class='msg'>Dear " + (assignUserObj.FullName.Contains(" - ") ? assignUserObj.FullName.Split('-')[0] : assignUserObj.FullName) + @",
                <br /><br />Please be informed that the following workflow notification details from DMDC System:
                </span>
                <br /><br />
                <table border='1'>
                <tr><td colspan='6' class='ch1'>Shipment Details</td></tr>
                 <tr>
                                <td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='action'>For Your Action</td>
                               
                            </tr>

                <tr>

                    <td class='ch2'>Shipment No.</td><td class='ch2'>:</td><td>" + transmittal.Number + @"</td></tr>
                   
                <tr>
                    <td class='ch2'>Description</td><td class='ch2'>:</td><td>" + transmittal.Description + @"</td></tr>
                   
                <tr>
                    <td class='ch2'>Issued Date</td><td class='ch2'>:</td><td>" + transmittal.Date.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td></tr>
                   
                <tr>
                    <td class='ch2'>Type</td><td class='ch2'>:</td><td>" + transmittal.ShipmentTypeName + @"</td>
                </tr>";                
                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/CostContract/ShipmentList.aspx?ShipmentNo=" + transmittal.Number;
                bodyContent += @"</table>
                            <div class='link'>
                            <br />
                            <u><b>Useful Links:</b></u>
                            <ul>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this Shipment</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
                            </div>
                            <br />
                            <h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
                            <hr />
                            <h3 class='system'>DMDC System</h3>
                            <br />
                            <span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span>
                            </body>";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(assignUserObj.Email))
                {
                    message.To.Add(assignUserObj.Email);

                    smtpClient.Send(message);
                }
            }
            catch (Exception ex) { }
        }
        private void SendNotificationInforShipment(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<string> infoUserIds)
        {
            try
            {
                // Implement send mail function

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };

                var transmittal = this.shipmentService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());

                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;
                int count = 0;
                message.Subject = "FYI: " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                // Generate email body
                var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
                var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
                var bodyContent = @"<head><title></title><style>
                            body {font-family:Calibri;font-size:10px;}
                            hr {color:#2C4E9C;background-color:#2C4E9C; height:3px;}
                            .msg {font-size:16px;}
                            table {border-collapse:collapse;margin-left:20px;color:black;background-color:white;border:1px solid #ACCEF5;padding:3px;font-size:16px;}
                            td {border:1px solid #ACCEF5;}
                            .span1 {font-size:16px;}
                            .ch1 {background-color:#F7FAFF;padding:10px;font-weight:bold;color:#2C4E9C;}
                            .ch2 {background-color:#F7FAFF;padding:5px;}
                            a {color:mediumblue;}
                            .system {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                            .company {font-weight:bolder; font-family:'Bookman Old Style'; color:#2C4E9C;}
                            .link {font-size:16px;margin-left:30px;}
                            .footer {color:darkgray; font-size:12px;}
                            /*TYPE OF NOTIFICATION PURPOSE*/
                            .action {background-color:#fffda5;}
                            .info {background-color:#d1fcbd;}
                            .overdue {background-color:#f00;color:white;font-weight:bold;}
                            </style></head>
                            <body>
                           <h2 style='font-family:'Arial Rounded MT Bold';'><u>Workflow Notification</u></h2>
                <span class='msg'>Dear All,
                <br /><br />Please be informed that the following workflow notification details from DMDC System:
                </span>
                <br /><br />
                <table border='1'>
                 <tr><td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='info'>For Your Information</td>
                               
                            </tr>

                <tr>

                    <td class='ch2'>Shipment No.</td><td class='ch2'>:</td><td>" + transmittal.Number + @"</td></tr>
                   
                <tr>
                    <td class='ch2'>Description</td><td class='ch2'>:</td><td>" + transmittal.Description + @"</td></tr>
                   
                <tr>
                    <td class='ch2'>Issued Date</td><td class='ch2'>:</td><td>" + transmittal.Date.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td></tr>
                   
                <tr>
                    <td class='ch2'>Type</td><td class='ch2'>:</td><td>" + transmittal.ShipmentTypeName + @"</td>
                </tr>";
                var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
                var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/CostContract/ShipmentList.aspx?ShipmentNo=" + transmittal.Number;
                bodyContent += @"</table>
                            <div class='link'>
                            <br />
                            <u><b>Useful Links:</b></u>
                            <ul>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>this Shipment</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st + @"'>here</a> to show <u>this task</u> in DMDC System
                                </li>
                                <li>
                                    Click <a href='" + st1 + @"'>here</a> to show <u>all your tasks</u> in DMDC System
                                </li>
                            </ul>
                            </div>
                            <br />
                            <h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
                            <hr />
                            <h3 class='system'>DMDC System</h3>
                            <br />
                            <span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span>
                            </body>";
                message.Body = bodyContent;
                foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                {
                    var userObj = this.userService.GetByID(userId);
                    if (userObj != null)
                    {
                        if (!string.IsNullOrEmpty(userObj.Email)) message.To.Add(userObj.Email);
                    }
                }
            }
            catch (Exception ex) { }
        }
        private bool IsHoliday(DateTime date)
        {
            return holidays.Contains(date);
        }
        private bool IsWeekEnd(DateTime date)
        {
            return ConfigurationManager.AppSettings["WeekendWork"] == "false" ? date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday : false;
        }

        private DateTime GetNextWorkingDay(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            }
            while (IsHoliday(date) || IsWeekEnd(date));

            return date;
        }
        protected void btnReload_Click(object sender, EventArgs e)
        {
            if (this.customizeWorkflowDetailService.GetAllByObjId(this.ObjId).Any() || this.customizeWorkflowDetailService.GetAllByTransId(this.ObjId).Any())
            {
                var customizeWfFrom = this.Request.QueryString["customizeWfFrom"];
                var ObjectList = new List<CustomizeWorkflowDetail>();
                if (customizeWfFrom == "Trans")
                {
                    ObjectList = this.customizeWorkflowDetailService.GetAllByTransId(this.ObjId);
                }
                else
                {
                    ObjectList = this.customizeWorkflowDetailService.GetAllByObjId(this.ObjId);
                }

                foreach (var item in ObjectList)
                {
                    this.customizeWorkflowDetailService.Delete(item);
                }

                var originalWorkflowDetails = this.wfDetailService.GetAllByWorkflow(this.WorkflowId);
                foreach (var wfDetail in originalWorkflowDetails)
                {
                    var customizeWfDetail = new CustomizeWorkflowDetail()
                    {
                        WorkflowID = wfDetail.WorkflowID,
                        WorkflowName = wfDetail.WorkflowName,
                        CurrentWorkflowStepID = wfDetail.CurrentWorkflowStepID,
                        CurrentWorkflowStepName = wfDetail.CurrentWorkflowStepName,
                        StepDefinitionID = wfDetail.StepDefinitionID,
                        StepDefinitionName = wfDetail.StepDefinitionName,
                        Duration = wfDetail.Duration,
                        AssignTitleIds = wfDetail.AssignTitleIds,
                        AssignUserIDs = wfDetail.AssignUserIDs,
                        ReviewUserIds = wfDetail.ReviewUserIds,
                        ConsolidateUserIds = wfDetail.ConsolidateUserIds,
                        CommentUserIds = wfDetail.CommentUserIds,
                        ApproveUserIds = wfDetail.ApproveUserIds,
                        InformationOnlyUserIDs = wfDetail.InformationOnlyUserIDs,
                        ManagementUserIds = wfDetail.ManagementUserIds,
                        AssignRoleIDs = wfDetail.AssignRoleIDs,
                        InformationOnlyRoleIDs = wfDetail.InformationOnlyRoleIDs,
                        DistributionMatrixIDs = wfDetail.DistributionMatrixIDs,
                        Recipients = wfDetail.Recipients,
                        NextWorkflowStepID = wfDetail.NextWorkflowStepID,
                        NextWorkflowStepName = wfDetail.NextWorkflowStepName,
                        RejectWorkflowStepID = wfDetail.RejectWorkflowStepID,
                        RejectWorkflowStepName = wfDetail.RejectWorkflowStepName,
                        CreatedBy = wfDetail.CreatedBy,
                        CreatedDate = wfDetail.CreatedDate,
                        UpdatedBy = wfDetail.UpdatedBy,
                        UpdatedDate = wfDetail.UpdatedDate,
                        ProjectID = wfDetail.ProjectID,
                        ProjectName = wfDetail.ProjectName,
                        IsFirst = wfDetail.IsFirst,
                        CanReject = wfDetail.CanReject,
                        IsOnlyWorkingDay = wfDetail.IsOnlyWorkingDay,
                        IsCanCreateOutgoingTrans = wfDetail.IsCanCreateOutgoingTrans,
                        ActionApplyCode = wfDetail.ActionApplyCode,
                        ActionApplyName = wfDetail.ActionApplyName,

                    };

                    if (customizeWfFrom == "Trans")
                    {
                        customizeWfDetail.IncomingTransId = this.ObjId;
                    }
                    else
                    {
                        customizeWfDetail.ObjectId = this.ObjId;
                    }

                    this.customizeWorkflowDetailService.Insert(customizeWfDetail);
                }
                this.grdDocument.Rebind();
            }
        }
    }
}