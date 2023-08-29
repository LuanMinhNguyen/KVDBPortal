// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Workflow
{
	/// <summary>
	/// The customer edit form.
	/// </summary>
	public partial class CompleteMoveNext : Page
	{
		private readonly WorkflowService wfService;
		private readonly UserService userService;
		private readonly ObjectAssignedWorkflowService objAssignedWfService;
		private readonly ObjectAssignedUserService objAssignedUserService;
		private readonly WorkflowStepService wfStepService;
		private readonly WorkflowDetailService wfDetailService;
		private readonly HolidayConfigService holidayConfigService;
		private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();
		private readonly DistributionMatrixService matrixService;
		private readonly DistributionMatrixDetailService matrixDetailService;
		private readonly PECC2DocumentsService pecc2DocumentService;
		private readonly DocumentCodeServices documnetCodeSErvie;
		private readonly ChangeRequestService changeRequestService;
		private readonly NCR_SIService ncrSiService;
		private readonly PECC2TransmittalService pecc2TransmittalService;
		private readonly PECC2TransmittalAttachFileService pecc2TransmittalAttachFileService;

		private readonly CustomizeWorkflowDetailService customizeWorkflowDetailService;
		private readonly ChangeRequestReviewResultCodeService changeRequestReviewResultCodeService;
		private readonly ProjectCodeService projectCodeService;

		private readonly ChangeRequestDocFileService changeRequestDocFileService;
		private readonly DocumentTypeService documentTypeService;
		private readonly RevisionStatuService revisionStatuService;
		private readonly PECC2DocumentAttachFileService pecc2DocumentAttachFileService;
		private readonly PECC2TransmittalService _pecc2transmittal;
		private readonly RFIService rfiService;
		private readonly ChangeGradeCodeService changeGradeCodeService;
		private readonly NCR_SIService ncrsiService;
		private readonly RFIDetailService rfideatilService;
		private readonly ShipmentService shipmentService = new ShipmentService();
		private int ObjId
		{
			get
			{
				return Convert.ToInt32(Request.QueryString["objId"]);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompleteMoveNext"/> class.
		/// </summary>
		public CompleteMoveNext()
		{
			this.userService = new UserService();
			this.wfService = new WorkflowService();
			this.objAssignedUserService = new ObjectAssignedUserService();
			this.objAssignedWfService = new ObjectAssignedWorkflowService();
			this.wfStepService = new WorkflowStepService();
			this.wfDetailService = new WorkflowDetailService();
			this.holidayConfigService = new HolidayConfigService();
			this.documnetCodeSErvie = new DocumentCodeServices();
			this.matrixService = new DistributionMatrixService();
			this.matrixDetailService = new DistributionMatrixDetailService();
			this.pecc2DocumentService = new PECC2DocumentsService();
			this.changeRequestService = new ChangeRequestService();
			this.ncrSiService = new NCR_SIService();
			this.pecc2TransmittalService = new PECC2TransmittalService();
			this.customizeWorkflowDetailService = new CustomizeWorkflowDetailService();
			this.pecc2TransmittalAttachFileService = new PECC2TransmittalAttachFileService();
			this.changeRequestReviewResultCodeService = new ChangeRequestReviewResultCodeService();
			this.changeRequestDocFileService = new ChangeRequestDocFileService();
			this.documentTypeService = new DocumentTypeService();
			this.revisionStatuService = new RevisionStatuService();
			this.pecc2DocumentAttachFileService = new PECC2DocumentAttachFileService();
			this._pecc2transmittal = new PECC2TransmittalService();
			this.rfiService = new RFIService();
			this.changeGradeCodeService = new ChangeGradeCodeService();
			this.ncrsiService = new NCR_SIService();
			this.rfideatilService = new RFIDetailService();
			this.projectCodeService = new ProjectCodeService();

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
			if (!IsPostBack)
			{
				var holidayList = this.holidayConfigService.GetAll();
				foreach (var holidayConfig in holidayList)
				{
					for (DateTime i = holidayConfig.FromDate.GetValueOrDefault(); i < holidayConfig.ToDate.GetValueOrDefault(); i = i.AddDays(1))
					{
						this.Holidays.Add(i);
					}
				}

				LoadComboData();

				var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
				var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
				if (currentWorkAssignedUser != null)
				{
					if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"])
						&& this.Request.QueryString["actionType"] != "1"
						&& currentWorkAssignedUser.ObjectTypeId == 1)
					{
						this.DevCommentCode.Visible = true;
						var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
						var docObj = this.pecc2DocumentService.GetById(objId);
						if (docObj != null)
						{
							this.ddlDocReviewStatus.SelectedValue = docObj.DocReviewStatusId.GetValueOrDefault().ToString();
						}
					}
					else
					{
						this.DevCommentCode.Visible = false;
					}


					//if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"])
					//    && this.Request.QueryString["actionType"] != "1"
					//    && currentWorkAssignedUser.ObjectTypeId == 2)
					//{
					//    this.divChangeRequestReviewResult.Visible = true;
					//    var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
					//    var changeRequestObj = this.changeRequestService.GetById(objId);
					//    if (changeRequestObj != null)
					//    {
					//        this.ddlChangeRequestReviewCode.SelectedValue = changeRequestObj.ReviewResultId.GetValueOrDefault().ToString();
					//    }
					//}
					//else
					//{
					//    this.divChangeRequestReviewResult.Visible = false;
					//}
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
			var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
			var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
			
			if (currentWorkAssignedUser != null)
			{
				var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
				var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
				var objType = currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault();
				// Update Current work assign
				currentWorkAssignedUser.CommentContent = this.txtMessage.Text.Trim();
				//currentWorkAssignedUser.IsReject = false;
				currentWorkAssignedUser.IsComplete = true;
				//currentWorkAssignedUser.Status = "SO";
				currentWorkAssignedUser.IsLeaf = false;
				if(currentWorkAssignedUser.ObjectTypeId == 3)
				{
					currentWorkAssignedUser.Status = this.ddlNCRSICSReviewCode.SelectedValue;

				}
				currentWorkAssignedUser.ActualDate = DateTime.Now;
				currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
				if (objType == 3 && UserSession.Current.User.Role.ContractorId == 3 && !currentWorkAssignedUser.ObjectNumber.Contains("-CS-"))
				{
					var ncrsiObj = this.ncrSiService.GetById(objId);
					if (ncrsiObj != null && string.IsNullOrEmpty(ncrsiObj.ActionTake))
					{
						this.blockError.Visible = true;
						this.lblError.Text = "Please update Action Taken of " + ncrsiObj.Number+"  befor click 'Complete task'.";
						return;
					}
					else if(ncrsiObj != null)
					{
						ncrsiObj.Status = "Closing";
						this.ncrSiService.Update(ncrsiObj);
					}
				}

				if (currentWorkAssignedUser.ObjectTypeId == 5 && currentWorkAssignedUser.ActionTypeId==3)
				{
					if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"])){
						var rfiobj = this.rfiService.GetById(objId);
						var usercreate = this.userService.GetByID(rfiobj.CreatedBy.GetValueOrDefault());
						SendNotificationRFIForEPC(rfiobj, usercreate);
							}
				}
				// ---------------------------------------------------------------------------------------------

				// Update "Approved By" info for CRS file
				//if (currentWorkAssignedUser.ActionTypeId == 4 && objType == 4)
				//{
				//    var incomingTransObj = this.pecc2TransmittalService.GetById(objId);
				//    if (incomingTransObj != null)
				//    {
				//        var crsAttachFile = this.pecc2TransmittalAttachFileService.GetByTrans(incomingTransObj.ID).FirstOrDefault(t => t.TypeId == 2);
				//        if (crsAttachFile != null)
				//        {
				//            var workbook = new Workbook();
				//            workbook = new Workbook(Server.MapPath("../.." + crsAttachFile.FilePath));
				//            var dataControlSheet = workbook.Worksheets[0];
				//            var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value) - 1;
				//            var totalRow = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);

				//            var transSheet = workbook.Worksheets[currentSheetIndex];
				//            for (int i = 0; i < totalRow; i++)
				//            {
				//                transSheet.Cells[7 + i, 11].PutValue(UserSession.Current.User.Username);
				//            }

				//            var filename = Utility.RemoveSpecialCharacterFileName(incomingTransObj.TransmittalNo) + "_CRS.xlsm";
				//            var saveFilePath = Server.MapPath("../.." + incomingTransObj.StoreFolderPath + "/eTRM File/" + filename);
				//            workbook.Save(saveFilePath);
				//        }
				//    }
				//}
				// -----------------------------------------------------------------------------------------------------------------

				var pendingTaskList = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
				if (!pendingTaskList.Any())
				{
					// Reject Case
					if (this.objAssignedUserService.IsHaveRejectTask(
							currentWorkAssignedUser.ObjectID.GetValueOrDefault(),
							currentWorkAssignedUser.CurrentWorkflowStepId.GetValueOrDefault()))
					{
						// Process for reject
						var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
						if (currentWorkAssignedWf != null)
						{
							var prevStep = this.wfStepService.GetById(currentWorkAssignedWf.RejectWorkflowStepID.GetValueOrDefault());
							if (prevStep != null)
							{
								switch (objType)
								{
									case 1:
										var docObj = this.pecc2DocumentService.GetById(objId);
										if (docObj != null)
										{
											docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
											docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
											this.pecc2DocumentService.Update(docObj);

											if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(prevStep, wfObj, docObj, objType, false);
											}
											else
											{
												var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
													? "Trans"
													: "Obj";
												this.ProcessCustomizeWorkflow(prevStep, wfObj, docObj, objType, false, customizeWfFrom);
											}
										}
										break;
									//case 2:
									//    var changeRequestObj = this.changeRequestService.GetById(objId);
									//    if (changeRequestObj != null)
									//    {
									//        changeRequestObj.PECC2ReviewResultName = this.ddlChangeRequestReviewCode.SelectedItem.Text;
									//        changeRequestObj.PECC2ReviewResultId = Convert.ToInt16(this.ddlChangeRequestReviewCode.SelectedValue);
									//        this.changeRequestService.Update(changeRequestObj);

									//        if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
									//            && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
									//        {
									//            this.ProcessOriginalWorkflow(prevStep, wfObj, changeRequestObj, objType,
									//                false);
									//        }
									//        else
									//        {
									//            var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
									//                ? "Trans"
									//                : "Obj";
									//            this.ProcessCustomizeWorkflow(prevStep, wfObj, changeRequestObj, objType, false, customizeWfFrom);
									//        }
									//    }
									//    break;
									case 3:
										var ncrsiObj = this.ncrSiService.GetById(objId);
										if (ncrsiObj != null)
										{
											ncrsiObj.ReviewResult = this.ddlNCRSICSReviewCode.SelectedValue;
											this.ncrSiService.Update(ncrsiObj);

											if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(prevStep, wfObj, ncrsiObj, objType, false);
											}
											else
											{
												var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
													? "Trans"
													: "Obj";
												this.ProcessCustomizeWorkflow(prevStep, wfObj, ncrsiObj, objType, false, customizeWfFrom);
											}
										}
										break;
									case 2:
									case 4:
										var incomingTransObj = this.pecc2TransmittalService.GetById(objId);
										if (incomingTransObj != null)
										{
											if (!incomingTransObj.IsUseCustomWf.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(prevStep, wfObj, incomingTransObj, objType, false);
											}
											else
											{
												this.ProcessCustomizeWorkflow(prevStep, wfObj, incomingTransObj, objType, false, "Trans");
											}
										}
										break;
									case 5:
										var rfiObj = this.rfiService.GetById(objId);
										if (rfiObj != null)
										{
											if (!rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !rfiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(prevStep, wfObj, rfiObj, objType, false);
											}
											else
											{
												var customizeWfFrom = rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
													? "Trans"
													: "Obj";
												this.ProcessCustomizeWorkflow(prevStep, wfObj, rfiObj, objType, false, customizeWfFrom);
											}
										}
										break;
									case 6:
										var shipObj = this.shipmentService.GetById(objId);
										if (shipObj != null)
										{
											if (!shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !shipObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(prevStep, wfObj, shipObj, objType, false);
											}
											else
											{
												var customizeWfFrom = shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
													? "Trans"
													: "Obj";
												this.ProcessCustomizeWorkflow(prevStep, wfObj, shipObj, objType, false, customizeWfFrom);
											}
										}
										break;
								}

							}
						}
					}
					// Move next case
					else
					{
						var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
						if (currentWorkAssignedWf != null)
						{
							var nextStep = this.wfStepService.GetById(currentWorkAssignedWf.NextWorkflowStepID.GetValueOrDefault());
							if (nextStep != null)
							{
								switch (objType)
								{
									case 1:
										var docObj = this.pecc2DocumentService.GetById(objId);
										if (docObj != null)
										{
											docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
											docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
											this.pecc2DocumentService.Update(docObj);

											if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(nextStep, wfObj, docObj, objType, false);
											}
											else
											{
												var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												  ? "Trans"
												  : "Obj";
												this.ProcessCustomizeWorkflow(nextStep, wfObj, docObj, objType, false, customizeWfFrom);
											}
											
										}
										break;
									//case 2:
									//    var changeRequestObj = this.changeRequestService.GetById(objId);
									//    if (changeRequestObj != null)
									//    {
									//        changeRequestObj.PECC2ReviewResultName = this.ddlChangeRequestReviewCode.SelectedItem.Text;
									//        changeRequestObj.PECC2ReviewResultId = Convert.ToInt16(this.ddlChangeRequestReviewCode.SelectedValue);
									//        this.changeRequestService.Update(changeRequestObj);

									//        if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
									//            && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
									//        {
									//            this.ProcessOriginalWorkflow(nextStep, wfObj, changeRequestObj, objType,
									//                false);
									//        }
									//        else
									//        {
									//            var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
									//              ? "Trans"
									//              : "Obj";
									//            this.ProcessCustomizeWorkflow(nextStep, wfObj, changeRequestObj, objType, false, customizeWfFrom);
									//        }
									//    }
									//    break;
									case 3:
										var ncrsiObj = this.ncrSiService.GetById(objId);
										if (ncrsiObj != null)
										{
											ncrsiObj.ReviewResult = this.ddlNCRSICSReviewCode.SelectedValue;
											this.ncrSiService.Update(ncrsiObj);
											if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(nextStep, wfObj, ncrsiObj, objType, false);
											}
											else
											{
												var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												  ? "Trans"
												  : "Obj";
												this.ProcessCustomizeWorkflow(nextStep, wfObj, ncrsiObj, objType, false, customizeWfFrom);
											}
										}
										break;

									case 2:
									case 4:
										var incomingTransObj = this.pecc2TransmittalService.GetById(objId);
										if (incomingTransObj != null)
										{
											if (!incomingTransObj.IsUseCustomWf.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(nextStep, wfObj, incomingTransObj, objType, false);
											}
											else
											{
												this.ProcessCustomizeWorkflow(nextStep, wfObj, incomingTransObj, objType, false, "Trans");
											}
										}
										break;
									case 5:
										var rfiObj = this.rfiService.GetById(objId);
										if (rfiObj != null)
										{
											if (!rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !rfiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(nextStep, wfObj, rfiObj, objType, false);
											}
											else
											{
												var customizeWfFrom = rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
													? "Trans"
													: "Obj";
												this.ProcessCustomizeWorkflow(nextStep, wfObj, rfiObj, objType, false, customizeWfFrom);
											}
										}
										break;
									case 6:
										var shipObj = this.shipmentService.GetById(objId);
										if (shipObj != null)
										{
											if (!shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
												&& !shipObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
											{
												this.ProcessOriginalWorkflow(nextStep, wfObj, shipObj, objType, false);
											}
											else
											{
												var customizeWfFrom = shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
													? "Trans"
													: "Obj";
												this.ProcessCustomizeWorkflow(nextStep, wfObj, shipObj, objType, false, customizeWfFrom);
											}
										}
										break;
								}
							}
							else
							{
								// Case: Document completed workflow process => Re-check & Update info for incoming trans
								switch (objType)
								{
									case 1:
										var docObj = this.pecc2DocumentService.GetById(objId);
										if (docObj != null)
										{
											docObj.IsInWFProcess = false;
											docObj.IsWFComplete = true;
											docObj.IsUseIsUseCustomWfFromObj = false;
											docObj.IsUseCustomWfFromTrans = false;
											this.pecc2DocumentService.Update(docObj);

											var docInTrans =
												this.pecc2DocumentService.GetAllByIncomingTrans(
													docObj.IncomingTransId.GetValueOrDefault());
											if (docInTrans.All(t => t.IsWFComplete.GetValueOrDefault()))
											{
												var incomingTransObj =
													this.pecc2TransmittalService.GetById(
														docObj.IncomingTransId.GetValueOrDefault());
												if (incomingTransObj != null)
												{
													incomingTransObj.IsAllDocCompleteWorkflow = true;
													this.pecc2TransmittalService.Update(incomingTransObj);
												}
											}
										}
										break;
									case 2:
										var incomingTransObj2 = this.pecc2TransmittalService.GetById(objId);
										if (incomingTransObj2 != null)
										{
											incomingTransObj2.IsInWFProcess = false;
											incomingTransObj2.IsWFComplete = true;
											incomingTransObj2.IsUseCustomWf = false;
											this.pecc2TransmittalService.Update(incomingTransObj2);

											// Update review result code for document
											var changeRequestObj = this.changeRequestService.GetAllByIncomingTrans(incomingTransObj2.ID).First();
											if (changeRequestObj != null)
											{
												var docList = this.pecc2DocumentService.GetAllByIncomingTrans(incomingTransObj2.ID);
												foreach (var pecc2Document in docList)
												{
													pecc2Document.ChangeRequestReviewResultCode = changeRequestObj.ReviewResultName;
													pecc2Document.ChangeRequestReviewResultId = changeRequestObj.ReviewResultId;

													this.pecc2DocumentService.Update(pecc2Document);
												}
											}
											// -----------------------------------------------------------------------------------
										}
										break;
									//case 2:
									//    var changeRequestObj = this.changeRequestService.GetById(objId);
									//    if (changeRequestObj != null)
									//    {
									//        changeRequestObj.ReviewResultId = changeRequestObj.PECC2ReviewResultId;
									//        changeRequestObj.ReviewResultName = changeRequestObj.PECC2ReviewResultName;
									//        changeRequestObj.IsReject = changeRequestObj.ReviewResultName == "Disapproved";
									//        changeRequestObj.IsInWFProcess = false;
									//        changeRequestObj.IsWFComplete = true;
									//        changeRequestObj.IsUseIsUseCustomWfFromObj = false;
									//        changeRequestObj.IsUseCustomWfFromTrans = false;
									//        this.changeRequestService.Update(changeRequestObj);

									//        // Import Document File
									//        if (changeRequestObj.ReviewResultName == "Approved")
									//        {
									//            var revisionStatus = this.revisionStatuService.GetByCode("FA");
									//            var docList = this.changeRequestDocFileService.GetAllByChangeRequest( changeRequestObj.ID);
									//            foreach (var changeRequestDocFile in docList)
									//            {
									//                var currentProjectDocList = this.PECC2DocumentService.GetAllByProjectDocNo(changeRequestDocFile.DocumentNo);
									//                if (currentProjectDocList.Count > 0)
									//                {
									//                    var currentLeafProjectDoc = currentProjectDocList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
									//                    if (currentLeafProjectDoc != null)
									//                    {
									//                        var projectDoc = new PECC2Documents();
									//                        this.CollectProjectDocData(changeRequestDocFile, projectDoc);
									//                        projectDoc.DocNo = currentLeafProjectDoc.DocNo;
									//                        projectDoc.DocTitle = currentLeafProjectDoc.DocTitle;
									//                        projectDoc.ProjectId = currentLeafProjectDoc.ProjectId;
									//                        projectDoc.ProjectName = currentLeafProjectDoc.ProjectName;
									//                        projectDoc.RevisionSchemaId = currentLeafProjectDoc.RevisionSchemaId;
									//                        projectDoc.RevisionSchemaName = currentLeafProjectDoc.RevisionSchemaName;
									//                        projectDoc.ConfidentialityId = currentLeafProjectDoc.ConfidentialityId;
									//                        projectDoc.ConfidentialityName = currentLeafProjectDoc.ConfidentialityName;
									//                        if (revisionStatus != null)
									//                        {
									//                            projectDoc.RevStatusId = revisionStatus.ID;
									//                            projectDoc.RevStatusName = revisionStatus.FullName;
									//                            if (revisionStatus.Code == "FA")
									//                            {
									//                                projectDoc.ActualDate = changeRequestObj.IssuedDate;
									//                            }
									//                        }

									//                        projectDoc.CreatedBy = UserSession.Current.User.Id;
									//                        projectDoc.CreatedDate = DateTime.Now;
									//                        projectDoc.IsLeaf = true;
									//                        projectDoc.IsDelete = false;
									//                        projectDoc.IsHasAttachFile = true;
									//                        projectDoc.ParentId = currentLeafProjectDoc.ParentId ?? currentLeafProjectDoc.ID;
									//                        //projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
									//                        //projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
									//                        //projectDoc.IsssuedDate = contractorTransObj.TransDate;
									//                        // Fill incoming trans info
									//                        //projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
									//                        //projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
									//                        //-------------------------------------------------------------------------------

									//                        var projectDocId = this.PECC2DocumentService.Insert(projectDoc);
									//                        if (projectDocId != null)
									//                        {
									//                            // Update leaf project doc
									//                            currentLeafProjectDoc.IsLeaf = false;
									//                            this.PECC2DocumentService.Update(currentLeafProjectDoc);
									//                            // -------------------------------------------------------------------------------------------------------------

									//                            //Attach doc file to project doc
									//                            this.AttachDocFileToProjectDoc(changeRequestDocFile,
									//                                projectDoc);
									//                            // --------------------------------------------------------------------------------------------------------------
									//                        }
									//                    }
									//                }
									//            }
									//        }
									//        // -------------------------------------------------------------------------------

									//        //var changeRequestInTrans =
									//        //    this.changeRequestService.GetAllByIncomingTrans(
									//        //        changeRequestObj.IncomingTransId.GetValueOrDefault());
									//        //if (changeRequestInTrans.All(t => t.IsWFComplete.GetValueOrDefault()))
									//        //{
									//        //    var incomingTransObj1 =
									//        //        this.pecc2TransmittalService.GetById(
									//        //            changeRequestObj.IncomingTransId.GetValueOrDefault());
									//        //    if (incomingTransObj1 != null)
									//        //    {
									//        //        incomingTransObj1.IsAllDocCompleteWorkflow = true;
									//        //        this.pecc2TransmittalService.Update(incomingTransObj1);
									//        //    }
									//        //}
									//    }
									//    break;
									case 3:
										var ncrsiObj = this.ncrSiService.GetById(objId);
										if (ncrsiObj != null)
										{
											ncrsiObj.ReviewResult = this.ddlNCRSICSReviewCode.SelectedValue;
											this.ncrSiService.Update(ncrsiObj);
											ncrsiObj.IsInWFProcess = false;
											ncrsiObj.IsWFComplete = true;
											ncrsiObj.IsUseIsUseCustomWfFromObj = false;
											ncrsiObj.IsUseCustomWfFromTrans = false;
											this.ncrSiService.Update(ncrsiObj);
										}
										break;
									
									case 4:
										var incomingTransObj3 = this.pecc2TransmittalService.GetById(objId);
										if (incomingTransObj3 != null)
										{
											incomingTransObj3.IsInWFProcess = false;
											incomingTransObj3.IsWFComplete = true;
											incomingTransObj3.IsUseCustomWf = false;
											this.pecc2TransmittalService.Update(incomingTransObj3);
										}
										break;
									case 5:
										var rfiObj = this.rfiService.GetById(objId);
										if (rfiObj != null)
										{
											rfiObj.IsInWFProcess = false;
											rfiObj.IsWFComplete = true;
											rfiObj.IsUseIsUseCustomWfFromObj = false;
											rfiObj.IsUseCustomWfFromTrans = false;
											this.rfiService.Update(rfiObj);
										}
										break;
									case 6:
										var shipObj = this.shipmentService.GetById(objId);
										if (shipObj != null)
										{
											shipObj.IsInWFProcess = false;
											shipObj.IsWFComplete = true;
											shipObj.IsUseIsUseCustomWfFromObj = false;
											shipObj.IsUseCustomWfFromTrans = false;
											this.shipmentService.Update(shipObj);
										}
										break;
								}
							}
						}
					}
				}
				// if (!string.IsNullOrEmpty(this.Request.QueryString["ReloadPage"]))
				this.objAssignedUserService.Update(currentWorkAssignedUser);
				ClientScript.RegisterStartupScript(Page.GetType(), "mykey", !string.IsNullOrEmpty(this.Request.QueryString["flag"]) ? "Close();" : "CloseAndRebind();", true);
			}
		}
        private void NexStepAuto( ObjectAssignedUser currentWorkAssignedUser)
        {

            var wfObj = this.wfService.GetById(currentWorkAssignedUser.WorkflowId.GetValueOrDefault());
            var objId = currentWorkAssignedUser.ObjectID.GetValueOrDefault();
            var objType = currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault();

            var pendingTaskList = this.objAssignedUserService.GetAllIncompleteByDoc(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
            if (!pendingTaskList.Any())
            {
                // Reject Case
                if (this.objAssignedUserService.IsHaveRejectTask(
                        currentWorkAssignedUser.ObjectID.GetValueOrDefault(),
                        currentWorkAssignedUser.CurrentWorkflowStepId.GetValueOrDefault()))
                {
                    // Process for reject
                    var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                    if (currentWorkAssignedWf != null)
                    {
                        var prevStep = this.wfStepService.GetById(currentWorkAssignedWf.RejectWorkflowStepID.GetValueOrDefault());
                        if (prevStep != null)
                        {
                            switch (objType)
                            {
                                case 1:
                                    var docObj = this.pecc2DocumentService.GetById(objId);
                                    if (docObj != null)
                                    {
                                        docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
                                        docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
                                        this.pecc2DocumentService.Update(docObj);

                                        if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, docObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                ? "Trans"
                                                : "Obj";
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, docObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;
                                //case 2:
                                //    var changeRequestObj = this.changeRequestService.GetById(objId);
                                //    if (changeRequestObj != null)
                                //    {
                                //        changeRequestObj.PECC2ReviewResultName = this.ddlChangeRequestReviewCode.SelectedItem.Text;
                                //        changeRequestObj.PECC2ReviewResultId = Convert.ToInt16(this.ddlChangeRequestReviewCode.SelectedValue);
                                //        this.changeRequestService.Update(changeRequestObj);

                                //        if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                //            && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                //        {
                                //            this.ProcessOriginalWorkflow(prevStep, wfObj, changeRequestObj, objType,
                                //                false);
                                //        }
                                //        else
                                //        {
                                //            var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                //                ? "Trans"
                                //                : "Obj";
                                //            this.ProcessCustomizeWorkflow(prevStep, wfObj, changeRequestObj, objType, false, customizeWfFrom);
                                //        }
                                //    }
                                //    break;
                                case 3:
                                    var ncrsiObj = this.ncrSiService.GetById(objId);
                                    if (ncrsiObj != null)
                                    {
                                        ncrsiObj.ReviewResult = this.ddlNCRSICSReviewCode.SelectedValue;
                                        this.ncrSiService.Update(ncrsiObj);

                                        if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, ncrsiObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                ? "Trans"
                                                : "Obj";
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, ncrsiObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;
                                case 2:
                                case 4:
                                    var incomingTransObj = this.pecc2TransmittalService.GetById(objId);
                                    if (incomingTransObj != null)
                                    {
                                        if (!incomingTransObj.IsUseCustomWf.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, incomingTransObj, objType, false);
                                        }
                                        else
                                        {
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, incomingTransObj, objType, false, "Trans");
                                        }
                                    }
                                    break;
                                case 5:
                                    var rfiObj = this.rfiService.GetById(objId);
                                    if (rfiObj != null)
                                    {
                                        if (!rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !rfiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, rfiObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                ? "Trans"
                                                : "Obj";
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, rfiObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;
                                case 6:
                                    var shipObj = this.shipmentService.GetById(objId);
                                    if (shipObj != null)
                                    {
                                        if (!shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !shipObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(prevStep, wfObj, shipObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                ? "Trans"
                                                : "Obj";
                                            this.ProcessCustomizeWorkflow(prevStep, wfObj, shipObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;
                            }

                        }
                    }
                }
                // Move next case
                else
                {
                    var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                    if (currentWorkAssignedWf != null)
                    {
                        var nextStep = this.wfStepService.GetById(currentWorkAssignedWf.NextWorkflowStepID.GetValueOrDefault());
                        if (nextStep != null)
                        {
                            switch (objType)
                            {
                                case 1:
                                    var docObj = this.pecc2DocumentService.GetById(objId);
                                    if (docObj != null)
                                    {
                                        docObj.DocReviewStatusId = Convert.ToInt32(this.ddlDocReviewStatus.SelectedValue);
                                        docObj.DocReviewStatusCode = this.ddlDocReviewStatus.SelectedItem.Text.Split(',')[0];
                                        this.pecc2DocumentService.Update(docObj);

                                        if (!docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !docObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(nextStep, wfObj, docObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = docObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                              ? "Trans"
                                              : "Obj";
                                            this.ProcessCustomizeWorkflow(nextStep, wfObj, docObj, objType, false, customizeWfFrom);
                                        }

                                    }
                                    break;
                                //case 2:
                                //    var changeRequestObj = this.changeRequestService.GetById(objId);
                                //    if (changeRequestObj != null)
                                //    {
                                //        changeRequestObj.PECC2ReviewResultName = this.ddlChangeRequestReviewCode.SelectedItem.Text;
                                //        changeRequestObj.PECC2ReviewResultId = Convert.ToInt16(this.ddlChangeRequestReviewCode.SelectedValue);
                                //        this.changeRequestService.Update(changeRequestObj);

                                //        if (!changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                //            && !changeRequestObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                //        {
                                //            this.ProcessOriginalWorkflow(nextStep, wfObj, changeRequestObj, objType,
                                //                false);
                                //        }
                                //        else
                                //        {
                                //            var customizeWfFrom = changeRequestObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                //              ? "Trans"
                                //              : "Obj";
                                //            this.ProcessCustomizeWorkflow(nextStep, wfObj, changeRequestObj, objType, false, customizeWfFrom);
                                //        }
                                //    }
                                //    break;
                                case 3:
                                    var ncrsiObj = this.ncrSiService.GetById(objId);
                                    if (ncrsiObj != null)
                                    {
                                        ncrsiObj.ReviewResult = this.ddlNCRSICSReviewCode.SelectedValue;
                                        this.ncrSiService.Update(ncrsiObj);
                                        if (!ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !ncrsiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(nextStep, wfObj, ncrsiObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = ncrsiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                              ? "Trans"
                                              : "Obj";
                                            this.ProcessCustomizeWorkflow(nextStep, wfObj, ncrsiObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;

                                case 2:
                                case 4:
                                    var incomingTransObj = this.pecc2TransmittalService.GetById(objId);
                                    if (incomingTransObj != null)
                                    {
                                        if (!incomingTransObj.IsUseCustomWf.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(nextStep, wfObj, incomingTransObj, objType, false);
                                        }
                                        else
                                        {
                                            this.ProcessCustomizeWorkflow(nextStep, wfObj, incomingTransObj, objType, false, "Trans");
                                        }
                                    }
                                    break;
                                case 5:
                                    var rfiObj = this.rfiService.GetById(objId);
                                    if (rfiObj != null)
                                    {
                                        if (!rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !rfiObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(nextStep, wfObj, rfiObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = rfiObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                ? "Trans"
                                                : "Obj";
                                            this.ProcessCustomizeWorkflow(nextStep, wfObj, rfiObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;
                                case 6:
                                    var shipObj = this.shipmentService.GetById(objId);
                                    if (shipObj != null)
                                    {
                                        if (!shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                            && !shipObj.IsUseIsUseCustomWfFromObj.GetValueOrDefault())
                                        {
                                            this.ProcessOriginalWorkflow(nextStep, wfObj, shipObj, objType, false);
                                        }
                                        else
                                        {
                                            var customizeWfFrom = shipObj.IsUseCustomWfFromTrans.GetValueOrDefault()
                                                ? "Trans"
                                                : "Obj";
                                            this.ProcessCustomizeWorkflow(nextStep, wfObj, shipObj, objType, false, customizeWfFrom);
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            // Case: Document completed workflow process => Re-check & Update info for incoming trans
                            switch (objType)
                            {
                                case 1:
                                    var docObj = this.pecc2DocumentService.GetById(objId);
                                    if (docObj != null)
                                    {
                                        docObj.IsInWFProcess = false;
                                        docObj.IsWFComplete = true;
                                        docObj.IsUseIsUseCustomWfFromObj = false;
                                        docObj.IsUseCustomWfFromTrans = false;
                                        this.pecc2DocumentService.Update(docObj);

                                        var docInTrans =
                                            this.pecc2DocumentService.GetAllByIncomingTrans(
                                                docObj.IncomingTransId.GetValueOrDefault());
                                        if (docInTrans.All(t => t.IsWFComplete.GetValueOrDefault()))
                                        {
                                            var incomingTransObj =
                                                this.pecc2TransmittalService.GetById(
                                                    docObj.IncomingTransId.GetValueOrDefault());
                                            if (incomingTransObj != null)
                                            {
                                                incomingTransObj.IsAllDocCompleteWorkflow = true;
                                                this.pecc2TransmittalService.Update(incomingTransObj);
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    var incomingTransObj2 = this.pecc2TransmittalService.GetById(objId);
                                    if (incomingTransObj2 != null)
                                    {
                                        incomingTransObj2.IsInWFProcess = false;
                                        incomingTransObj2.IsWFComplete = true;
                                        incomingTransObj2.IsUseCustomWf = false;
                                        this.pecc2TransmittalService.Update(incomingTransObj2);

                                        // Update review result code for document
                                        var changeRequestObj = this.changeRequestService.GetAllByIncomingTrans(incomingTransObj2.ID).First();
                                        if (changeRequestObj != null)
                                        {
                                            var docList = this.pecc2DocumentService.GetAllByIncomingTrans(incomingTransObj2.ID);
                                            foreach (var pecc2Document in docList)
                                            {
                                                pecc2Document.ChangeRequestReviewResultCode = changeRequestObj.ReviewResultName;
                                                pecc2Document.ChangeRequestReviewResultId = changeRequestObj.ReviewResultId;

                                                this.pecc2DocumentService.Update(pecc2Document);
                                            }
                                        }
                                        // -----------------------------------------------------------------------------------
                                    }
                                    break;
                                //case 2:
                                //    var changeRequestObj = this.changeRequestService.GetById(objId);
                                //    if (changeRequestObj != null)
                                //    {
                                //        changeRequestObj.ReviewResultId = changeRequestObj.PECC2ReviewResultId;
                                //        changeRequestObj.ReviewResultName = changeRequestObj.PECC2ReviewResultName;
                                //        changeRequestObj.IsReject = changeRequestObj.ReviewResultName == "Disapproved";
                                //        changeRequestObj.IsInWFProcess = false;
                                //        changeRequestObj.IsWFComplete = true;
                                //        changeRequestObj.IsUseIsUseCustomWfFromObj = false;
                                //        changeRequestObj.IsUseCustomWfFromTrans = false;
                                //        this.changeRequestService.Update(changeRequestObj);

                                //        // Import Document File
                                //        if (changeRequestObj.ReviewResultName == "Approved")
                                //        {
                                //            var revisionStatus = this.revisionStatuService.GetByCode("FA");
                                //            var docList = this.changeRequestDocFileService.GetAllByChangeRequest( changeRequestObj.ID);
                                //            foreach (var changeRequestDocFile in docList)
                                //            {
                                //                var currentProjectDocList = this.PECC2DocumentService.GetAllByProjectDocNo(changeRequestDocFile.DocumentNo);
                                //                if (currentProjectDocList.Count > 0)
                                //                {
                                //                    var currentLeafProjectDoc = currentProjectDocList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
                                //                    if (currentLeafProjectDoc != null)
                                //                    {
                                //                        var projectDoc = new PECC2Documents();
                                //                        this.CollectProjectDocData(changeRequestDocFile, projectDoc);
                                //                        projectDoc.DocNo = currentLeafProjectDoc.DocNo;
                                //                        projectDoc.DocTitle = currentLeafProjectDoc.DocTitle;
                                //                        projectDoc.ProjectId = currentLeafProjectDoc.ProjectId;
                                //                        projectDoc.ProjectName = currentLeafProjectDoc.ProjectName;
                                //                        projectDoc.RevisionSchemaId = currentLeafProjectDoc.RevisionSchemaId;
                                //                        projectDoc.RevisionSchemaName = currentLeafProjectDoc.RevisionSchemaName;
                                //                        projectDoc.ConfidentialityId = currentLeafProjectDoc.ConfidentialityId;
                                //                        projectDoc.ConfidentialityName = currentLeafProjectDoc.ConfidentialityName;
                                //                        if (revisionStatus != null)
                                //                        {
                                //                            projectDoc.RevStatusId = revisionStatus.ID;
                                //                            projectDoc.RevStatusName = revisionStatus.FullName;
                                //                            if (revisionStatus.Code == "FA")
                                //                            {
                                //                                projectDoc.ActualDate = changeRequestObj.IssuedDate;
                                //                            }
                                //                        }

                                //                        projectDoc.CreatedBy = UserSession.Current.User.Id;
                                //                        projectDoc.CreatedDate = DateTime.Now;
                                //                        projectDoc.IsLeaf = true;
                                //                        projectDoc.IsDelete = false;
                                //                        projectDoc.IsHasAttachFile = true;
                                //                        projectDoc.ParentId = currentLeafProjectDoc.ParentId ?? currentLeafProjectDoc.ID;
                                //                        //projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
                                //                        //projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
                                //                        //projectDoc.IsssuedDate = contractorTransObj.TransDate;
                                //                        // Fill incoming trans info
                                //                        //projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
                                //                        //projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                                //                        //-------------------------------------------------------------------------------

                                //                        var projectDocId = this.PECC2DocumentService.Insert(projectDoc);
                                //                        if (projectDocId != null)
                                //                        {
                                //                            // Update leaf project doc
                                //                            currentLeafProjectDoc.IsLeaf = false;
                                //                            this.PECC2DocumentService.Update(currentLeafProjectDoc);
                                //                            // -------------------------------------------------------------------------------------------------------------

                                //                            //Attach doc file to project doc
                                //                            this.AttachDocFileToProjectDoc(changeRequestDocFile,
                                //                                projectDoc);
                                //                            // --------------------------------------------------------------------------------------------------------------
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //        // -------------------------------------------------------------------------------

                                //        //var changeRequestInTrans =
                                //        //    this.changeRequestService.GetAllByIncomingTrans(
                                //        //        changeRequestObj.IncomingTransId.GetValueOrDefault());
                                //        //if (changeRequestInTrans.All(t => t.IsWFComplete.GetValueOrDefault()))
                                //        //{
                                //        //    var incomingTransObj1 =
                                //        //        this.pecc2TransmittalService.GetById(
                                //        //            changeRequestObj.IncomingTransId.GetValueOrDefault());
                                //        //    if (incomingTransObj1 != null)
                                //        //    {
                                //        //        incomingTransObj1.IsAllDocCompleteWorkflow = true;
                                //        //        this.pecc2TransmittalService.Update(incomingTransObj1);
                                //        //    }
                                //        //}
                                //    }
                                //    break;
                                case 3:
                                    var ncrsiObj = this.ncrSiService.GetById(objId);
                                    if (ncrsiObj != null)
                                    {
                                        ncrsiObj.ReviewResult = this.ddlNCRSICSReviewCode.SelectedValue;
                                        this.ncrSiService.Update(ncrsiObj);
                                        ncrsiObj.IsInWFProcess = false;
                                        ncrsiObj.IsWFComplete = true;
                                        ncrsiObj.IsUseIsUseCustomWfFromObj = false;
                                        ncrsiObj.IsUseCustomWfFromTrans = false;
                                        this.ncrSiService.Update(ncrsiObj);
                                    }
                                    break;

                                case 4:
                                    var incomingTransObj3 = this.pecc2TransmittalService.GetById(objId);
                                    if (incomingTransObj3 != null)
                                    {
                                        incomingTransObj3.IsInWFProcess = false;
                                        incomingTransObj3.IsWFComplete = true;
                                        incomingTransObj3.IsUseCustomWf = false;
                                        this.pecc2TransmittalService.Update(incomingTransObj3);
                                    }
                                    break;
                                case 5:
                                    var rfiObj = this.rfiService.GetById(objId);
                                    if (rfiObj != null)
                                    {
                                        rfiObj.IsInWFProcess = false;
                                        rfiObj.IsWFComplete = true;
                                        rfiObj.IsUseIsUseCustomWfFromObj = false;
                                        rfiObj.IsUseCustomWfFromTrans = false;
                                        this.rfiService.Update(rfiObj);
                                    }
                                    break;
                                case 6:
                                    var shipObj = this.shipmentService.GetById(objId);
                                    if (shipObj != null)
                                    {
                                        shipObj.IsInWFProcess = false;
                                        shipObj.IsWFComplete = true;
                                        shipObj.IsUseIsUseCustomWfFromObj = false;
                                        shipObj.IsUseCustomWfFromTrans = false;
                                        this.shipmentService.Update(shipObj);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

        }
        private void ProcessCustomizeWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj, int objType, bool isReject, string customizeWfFrom)
        {
            var groupId = 0;

            //var infoOnlyUserList = new List<User>();
            CustomizeWorkflowDetail wfDetailObj = null;

            //var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (customizeWfFrom == "Trans")
            {
                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        if (docObj.IncomingTransId != null)
                        {
                            wfDetailObj =
                                this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                    docObj.IncomingTransId.GetValueOrDefault());
                        }
                        break;
                    //case 2:
                    //    var changeRequestObj = (ChangeRequest)obj;
                    //    wfDetailObj =
                    //        this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                    //            changeRequestObj.IncomingTransId.GetValueOrDefault());
                    //    break;
                    case 2:
                    case 4:
                        var incomingTransObj = (PECC2Transmittal)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(wfStepObj.ID,
                                incomingTransObj.ID);
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
                    case 2:
                        var changeRequestObj = (ChangeRequest)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                changeRequestObj.ID);
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        wfDetailObj =
                            this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromObj(wfStepObj.ID,
                                ncrsiObj.ID);
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
                    IsReject = isReject,
                    IsLeaf = true,
                    AssignedBy = UserSession.Current.User.Id,
                    CanReject = wfStepObj.CanReject,
                };

                switch (objType)
                {
                    case 1:
                        var docObj = (PECC2Documents)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf = this.objAssignedWfService.GetLeafBydoc(docObj.ID);
                        if (objAssignedWFLeaf != null)
                        {
                            objAssignedWFLeaf.IsComplete = true;
                            objAssignedWFLeaf.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = docObj.ID;
                        assignWorkFlow.ObjectNumber = docObj.DocNo;
                        assignWorkFlow.ObjectTitle = docObj.DocTitle;
                        assignWorkFlow.ObjectProject = docObj.ProjectName;
                        assignWorkFlow.ObjectType = "Project's Document";
                        groupId = docObj.GroupId.GetValueOrDefault();
                        break;
                    case 2:
                        //var changeRequestObj = (ChangeRequest)obj;
                        //// Update current doc assigned wf leaf.
                        //var objAssignedWFLeaf1 = this.objAssignedWfService.GetLeafBydoc(changeRequestObj.ID);
                        //if (objAssignedWFLeaf1 != null)
                        //{
                        //    objAssignedWFLeaf1.IsComplete = true;
                        //    objAssignedWFLeaf1.IsLeaf = false;
                        //    this.objAssignedWfService.Update(objAssignedWFLeaf1);
                        //}

                        //// Add more info for assign work flow
                        //assignWorkFlow.ObjectID = changeRequestObj.ID;
                        //assignWorkFlow.ObjectNumber = changeRequestObj.Number;
                        //assignWorkFlow.ObjectTitle = changeRequestObj.Description;
                        //assignWorkFlow.ObjectProject = changeRequestObj.ProjectCode;
                        //assignWorkFlow.ObjectType = "Change Request";
                        //groupId = changeRequestObj.GroupId.GetValueOrDefault();

                        var incomingTransObj1 = (PECC2Transmittal)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf4 = this.objAssignedWfService.GetLeafBydoc(incomingTransObj1.ID);
                        if (objAssignedWFLeaf4 != null)
                        {
                            objAssignedWFLeaf4.IsComplete = true;
                            objAssignedWFLeaf4.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf4);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = incomingTransObj1.ID;
                        assignWorkFlow.ObjectNumber = incomingTransObj1.TransmittalNo;
                        assignWorkFlow.ObjectTitle = incomingTransObj1.Description;
                        assignWorkFlow.ObjectProject = incomingTransObj1.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Change Request";
                        groupId = incomingTransObj1.GroupId.GetValueOrDefault();
                        break;
                    case 3:
                        var ncrsiObj = (NCR_SI)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf2 = this.objAssignedWfService.GetLeafBydoc(ncrsiObj.ID);
                        if (objAssignedWFLeaf2 != null)
                        {
                            objAssignedWFLeaf2.IsComplete = true;
                            objAssignedWFLeaf2.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf2);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = ncrsiObj.ID;
                        assignWorkFlow.ObjectNumber = ncrsiObj.Number;
                        assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
                        assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
                        assignWorkFlow.ObjectType = ncrsiObj.Number.Contains("-CS-") ? "CS" : "NCR/SI";
                        groupId = ncrsiObj.GroupId.GetValueOrDefault();
                        break;
                    case 4:
                        var incomingTransObj = (PECC2Transmittal)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf3 = this.objAssignedWfService.GetLeafBydoc(incomingTransObj.ID);
                        if (objAssignedWFLeaf3 != null)
                        {
                            objAssignedWFLeaf3.IsComplete = true;
                            objAssignedWFLeaf3.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf3);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = incomingTransObj.ID;
                        assignWorkFlow.ObjectNumber = incomingTransObj.TransmittalNo;
                        assignWorkFlow.ObjectTitle = incomingTransObj.Description;
                        assignWorkFlow.ObjectProject = incomingTransObj.ProjectCodeName;
                        assignWorkFlow.ObjectType = "Document Transmittal";
                        groupId = incomingTransObj.GroupId.GetValueOrDefault();
                        break;
                    case 5:
                        var rfiObj = (RFI)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf5 = this.objAssignedWfService.GetLeafBydoc(rfiObj.ID);
                        if (objAssignedWFLeaf5 != null)
                        {
                            objAssignedWFLeaf5.IsComplete = true;
                            objAssignedWFLeaf5.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf5);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = rfiObj.ID;
                        assignWorkFlow.ObjectNumber = rfiObj.Number;
                        assignWorkFlow.ObjectTitle = rfiObj.Description;
                        assignWorkFlow.ObjectProject = rfiObj.ProjectCode;
                        assignWorkFlow.ObjectType = "RFI";
                        groupId = rfiObj.GroupId.GetValueOrDefault();
                        break;
                    case 6:
                        var shipObj = (Shipment)obj;
                        // Update current doc assigned wf leaf.
                        var objAssignedWFLeaf6 = this.objAssignedWfService.GetLeafBydoc(shipObj.ID);
                        if (objAssignedWFLeaf6 != null)
                        {
                            objAssignedWFLeaf6.IsComplete = true;
                            objAssignedWFLeaf6.IsLeaf = false;
                            this.objAssignedWfService.Update(objAssignedWFLeaf6);
                        }
                        // Add more info for assign work flow
                        assignWorkFlow.ObjectID = shipObj.ID;
                        assignWorkFlow.ObjectNumber = shipObj.Number;
                        assignWorkFlow.ObjectTitle = shipObj.Description;
                        assignWorkFlow.ObjectProject = shipObj.ProjectName;
                        assignWorkFlow.ObjectType = "Shipment";
                        // groupId = rfiObj.GroupId.GetValueOrDefault();
                        break;
                }

                var assignWorkflowId = this.objAssignedWfService.Insert(assignWorkFlow);
                if (assignWorkflowId != null)
                {
                    var actualDeadline = DateTime.Now;
                    if (wfDetailObj.Duration.GetValueOrDefault() < 1)
                    {
                        actualDeadline = actualDeadline.AddDays(wfDetailObj.Duration.GetValueOrDefault());
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
                    var listUserAction = new List<User>();
                    var listUserInfor = new List<User>();
                    var currentWFStepDetail = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
                    // Create assign user info
                    var wfStepWorkingAssignUser = this.GetAssignUserListForCustomizeWorkflow(wfDetailObj, wfStepObj, groupId);
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
                            ReceivedDate = DateTime.Now,
                            PlanCompleteDate =
                                wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
                                    ? actualDeadline
                                    : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsCompleteReject = false,
                            IsOverDue = false,

                            IsComplete = user.ActionTypeId == 1,
                            IsReject = isReject,
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
                            IsLeaf = true
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
                                //    var changeRequestObj = (ChangeRequest)obj;
                                //    assignWorkingUser.ObjectID = changeRequestObj.ID;
                                //    assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                                //    assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                                //    assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                                //    assignWorkingUser.ObjectProjectId = changeRequestObj.ProjectId;
                                //    assignWorkingUser.Categoryid = 0;
                                //    assignWorkingUser.ObjectType = "Change Request";
                                //    assignWorkingUser.ObjectTypeId = 2;

                                var incomingTransObj1 = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = incomingTransObj1.ID;
                                assignWorkingUser.ObjectNumber = incomingTransObj1.TransmittalNo;
                                assignWorkingUser.ObjectTitle = incomingTransObj1.Description;
                                assignWorkingUser.ObjectProject = incomingTransObj1.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = incomingTransObj1.ProjectCodeId;
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
                                var incomingTransObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = incomingTransObj.ID;
                                assignWorkingUser.ObjectNumber = incomingTransObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = incomingTransObj.Description;
                                assignWorkingUser.ObjectProject = incomingTransObj.ProjectCodeName;
                                assignWorkingUser.ObjectProjectId = incomingTransObj.ProjectCodeId;
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
                        // send notification
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
                            user.ActionTypeId != 1)
                        {
                            switch (objType)
                            {
                                case 4:
                                //this.SendNotification(assignWorkingUser, user);
                                //break;
                                case 2:
                                //this.SendNotificationFCRDCN(assignWorkingUser, user);
                                //break;
                                case 3:
                                    //SendNotificationNCRSI(assignWorkingUser, user);
                                   
                                   // break;
                                case 5:
                                        //  SendNotificationRFI(assignWorkingUser, user);
                                        listUserAction.Add(user);
                                        break;
                            }


                        }
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && user.ActionTypeId == 1)
                        {
                            switch (objType)
                            {
                                case 4:
                                //this.SendNotificationInfor(assignWorkingUser, user);
                                //break;
                                case 2:
                                //this.SendNotificationInforFCRDCN(assignWorkingUser, user);
                                //break;
                                case 3:
                                    //SendNotificationInforNCR(assignWorkingUser, user);
                                   

                                   // break;
                                case 5:
                                        // SendNotificationInforRFI(assignWorkingUser, user);
                                        listUserInfor.Add(user);
                                        break;
                            }

                        }
                    }
                    }

                    // Update current info for Object
                    switch (objType)
                    {
                        case 1:
                            var docObj = (PECC2Documents)obj;
                            docObj.IsInWFProcess =( wfDetailObj.NextWorkflowStepID!= null && wfDetailObj.NextWorkflowStepID!= 0)? true:false;
                            docObj.IsWFComplete = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? false:true;
                            docObj.CurrentWorkflowName = wfObj.Name;
                            docObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            docObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.pecc2DocumentService.Update(docObj);
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
                            ncrsiObj.IsInWFProcess = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? true:false;
                            ncrsiObj.IsWFComplete = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? false:true;
                            ncrsiObj.CurrentWorkflowName = wfObj.Name;
                            ncrsiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            ncrsiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.ncrSiService.Update(ncrsiObj);
                            break;
                        case 2:
                        case 4:
                            var incomingTransObj = (PECC2Transmittal)obj;
                            incomingTransObj.IsInWFProcess = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? true:false;
                            incomingTransObj.IsWFComplete = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? false:true;
                            incomingTransObj.CurrentWorkflowName = wfObj.Name;
                            incomingTransObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            incomingTransObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.pecc2TransmittalService.Update(incomingTransObj);
                            break;
                        case 5:
                            var rfiObj = (RFI)obj;
                            rfiObj.IsInWFProcess = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? true:false;
                            rfiObj.IsWFComplete = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? false:true;
                            rfiObj.CurrentWorkflowName = wfObj.Name;
                            rfiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            rfiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.rfiService.Update(rfiObj);
                            break;
                        case 6:
                            var ShipObj = (Shipment)obj;
                            ShipObj.IsInWFProcess = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? true:false;
                            ShipObj.IsWFComplete = (wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID != 0) ? false:true;
                            ShipObj.CurrentWorkflowName = wfObj.Name;
                            ShipObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
                            ShipObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

                            this.shipmentService.Update(ShipObj);
                            break;

                    }
                    // ----------------------------------------------------------------------------------------------

                    // Assign to re-assign user of workflow when can't find working user
                    if (wfStepWorkingAssignUser.Count == 0 && wfObj.Re_assignUserId != null)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = assignWorkflowId,
                            UserID = wfObj.Re_assignUserId,
                            UserFullName = wfObj.Re_assignUserName,
                            ReceivedDate = DateTime.Now,
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
                            //case 2:
                            //    var changeRequestObj = (ChangeRequest)obj;
                            //    assignWorkingUser.ObjectID = changeRequestObj.ID;
                            //    assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                            //    assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                            //    assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                            //    break;
                            case 3:
                                var ncrsiObj = (NCR_SI)obj;
                                assignWorkingUser.ObjectID = ncrsiObj.ID;
                                assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                                assignWorkingUser.ObjectTitle = ncrsiObj.Description;
                                assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                                break;
                            case 2:
                            case 4:
                                var incomingTransObj = (PECC2Transmittal)obj;
                                assignWorkingUser.ObjectID = incomingTransObj.ID;
                                assignWorkingUser.ObjectNumber = incomingTransObj.TransmittalNo;
                                assignWorkingUser.ObjectTitle = incomingTransObj.Description;
                                assignWorkingUser.ObjectProject = incomingTransObj.ProjectCodeName;
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
                    }



                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && listUserAction.Count > 0)
                    {
                        switch (objType)
                        {
                            case 3:
                                this.SendNotificatioNCRSI(assignWorkingUserInfor, listUserAction, listUserInfor);

                                break;
                            case 2:
                            case 4:
                                this.SendNotification(assignWorkingUserInfor, listUserAction, listUserInfor);

                                break;
                            case 5:
                                this.SendNotificationRFI(assignWorkingUserInfor, listUserAction, listUserInfor);
                                break;
                        }
                    }
                    else if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && listUserAction.Count == 0 && listUserInfor.Count > 0)
                    {

                        switch (objType)
                        {
                            case 3:
                                this.SendNotificationInforNCRSI(assignWorkingUserInfor, listUserInfor);

                                break;
                            case 2:
                            case 4:
                                this.SendNotificationInfor(assignWorkingUserInfor, listUserInfor);

                                break;
                        }
                    }

                if(!wfStepWorkingAssignUser.Where(t=> t.ActionTypeId != 1).ToList().Any()  && wfDetailObj.NextWorkflowStepID != null && wfDetailObj.NextWorkflowStepID!=0 )
                    {
                        this.NexStepAuto(assignWorkingUserInfor);
                    }
                }
            }
        }

		private void ProcessOriginalWorkflow(WorkflowStep wfStepObj, Data.Entities.Workflow wfObj, object obj, int objType, bool isReject)
		{
			var groupId = 0;

			//var infoOnlyUserList = new List<User>();
			var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
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
					IsReject = isReject,
					IsLeaf = true,
					AssignedBy = UserSession.Current.User.Id,
					CanReject = wfStepObj.CanReject,
				};

				switch (objType)
				{
					case 1:
						var docObj = (PECC2Documents)obj;
						// Update current doc assigned wf leaf.
						var objAssignedWFLeaf = this.objAssignedWfService.GetLeafBydoc(docObj.ID);
						if (objAssignedWFLeaf != null)
						{
							objAssignedWFLeaf.IsComplete = true;
							objAssignedWFLeaf.IsLeaf = false;
							this.objAssignedWfService.Update(objAssignedWFLeaf);
						}
						// Add more info for assign work flow
						assignWorkFlow.ObjectID = docObj.ID;
						assignWorkFlow.ObjectNumber = docObj.DocNo;
						assignWorkFlow.ObjectTitle = docObj.DocTitle;
						assignWorkFlow.ObjectProject = docObj.ProjectName;
						assignWorkFlow.ObjectType = "Project's Document";
						groupId = docObj.GroupId.GetValueOrDefault();
						break;
					case 2:
						var incomingTransObj1 = (PECC2Transmittal)obj;
						// Update current doc assigned wf leaf.
						var objAssignedWFLeaf4 = this.objAssignedWfService.GetLeafBydoc(incomingTransObj1.ID);
						if (objAssignedWFLeaf4 != null)
						{
							objAssignedWFLeaf4.IsComplete = true;
							objAssignedWFLeaf4.IsLeaf = false;
							this.objAssignedWfService.Update(objAssignedWFLeaf4);
						}
						// Add more info for assign work flow
						assignWorkFlow.ObjectID = incomingTransObj1.ID;
						assignWorkFlow.ObjectNumber = incomingTransObj1.TransmittalNo;
						assignWorkFlow.ObjectTitle = incomingTransObj1.Description;
						assignWorkFlow.ObjectProject = incomingTransObj1.ProjectCodeName;
						assignWorkFlow.ObjectType = "Change Request";
						groupId = incomingTransObj1.GroupId.GetValueOrDefault();
						break;
					case 3:
						var ncrsiObj = (NCR_SI)obj;
						// Update current doc assigned wf leaf.
						var objAssignedWFLeaf2 = this.objAssignedWfService.GetLeafBydoc(ncrsiObj.ID);
						if (objAssignedWFLeaf2 != null)
						{
							objAssignedWFLeaf2.IsComplete = true;
							objAssignedWFLeaf2.IsLeaf = false;
							this.objAssignedWfService.Update(objAssignedWFLeaf2);
						}
						// Add more info for assign work flow
						assignWorkFlow.ObjectID = ncrsiObj.ID;
						assignWorkFlow.ObjectNumber = ncrsiObj.Number;
						assignWorkFlow.ObjectTitle = ncrsiObj.Subject;
						assignWorkFlow.ObjectProject = ncrsiObj.ProjectName;
						assignWorkFlow.ObjectType = "NCR/SI/CS";
						groupId = ncrsiObj.GroupId.GetValueOrDefault();
						break;
					case 4:
						var incomingTransObj = (PECC2Transmittal)obj;
						// Update current doc assigned wf leaf.
						var objAssignedWFLeaf3 = this.objAssignedWfService.GetLeafBydoc(incomingTransObj.ID);
						if (objAssignedWFLeaf3 != null)
						{
							objAssignedWFLeaf3.IsComplete = true;
							objAssignedWFLeaf3.IsLeaf = false;
							this.objAssignedWfService.Update(objAssignedWFLeaf3);
						}
						// Add more info for assign work flow
						assignWorkFlow.ObjectID = incomingTransObj.ID;
						assignWorkFlow.ObjectNumber = incomingTransObj.TransmittalNo;
						assignWorkFlow.ObjectTitle = incomingTransObj.Description;
						assignWorkFlow.ObjectProject = incomingTransObj.ProjectCodeName;
						assignWorkFlow.ObjectType = "Document Transmittal";
						groupId = incomingTransObj.GroupId.GetValueOrDefault();
						break;
					case 5:
						var rfiObj = (RFI)obj;
						// Update current doc assigned wf leaf.
						var objAssignedWFLeaf5 = this.objAssignedWfService.GetLeafBydoc(rfiObj.ID);
						if (objAssignedWFLeaf5 != null)
						{
							objAssignedWFLeaf5.IsComplete = true;
							objAssignedWFLeaf5.IsLeaf = false;
							this.objAssignedWfService.Update(objAssignedWFLeaf5);
						}
						// Add more info for assign work flow
						assignWorkFlow.ObjectID = rfiObj.ID;
						assignWorkFlow.ObjectNumber = rfiObj.Number;
						assignWorkFlow.ObjectTitle = rfiObj.Description;
						assignWorkFlow.ObjectProject = rfiObj.ProjectCode;
						assignWorkFlow.ObjectType = "RFI";
						groupId = rfiObj.GroupId.GetValueOrDefault();
						break;
				}

				var assignWorkflowId = this.objAssignedWfService.Insert(assignWorkFlow);
				if (assignWorkflowId != null)
				{
					var actualDeadline = DateTime.Now;
					if (wfDetailObj.Duration.GetValueOrDefault() < 1)
					{
						actualDeadline = actualDeadline.AddDays(wfDetailObj.Duration.GetValueOrDefault());
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

					var currentWFStepDetail = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
					// Create assign user info
					var wfStepWorkingAssignUser = this.GetAssignUserList(wfDetailObj, wfStepObj, groupId);
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
							ReceivedDate = DateTime.Now,
							PlanCompleteDate =
								wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault()
									? actualDeadline
									: DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
							IsCompleteReject = false,
							IsOverDue = false,
							
							IsComplete = user.ActionTypeId == 1,
							IsReject = isReject,
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
							IsLeaf = true
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
								var incomingTransObj1 = (PECC2Transmittal)obj;
								assignWorkingUser.ObjectID = incomingTransObj1.ID;
								assignWorkingUser.ObjectNumber = incomingTransObj1.TransmittalNo;
								assignWorkingUser.ObjectTitle = incomingTransObj1.Description;
								assignWorkingUser.ObjectProject = incomingTransObj1.ProjectCodeName;
								assignWorkingUser.ObjectProjectId = incomingTransObj1.ProjectCodeId;
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
								assignWorkingUser.ObjectType = "NCR/SI/CS";
								assignWorkingUser.ObjectTypeId = 3;
								break;
							case 4:
								var incomingTransObj = (PECC2Transmittal)obj;
								assignWorkingUser.ObjectID = incomingTransObj.ID;
								assignWorkingUser.ObjectNumber = incomingTransObj.TransmittalNo;
								assignWorkingUser.ObjectTitle = incomingTransObj.Description;
								assignWorkingUser.ObjectProject = incomingTransObj.ProjectCodeName;
								assignWorkingUser.ObjectProjectId = incomingTransObj.ProjectCodeId;
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
						}

						objAssignedUserService.Insert(assignWorkingUser);
						assignWorkingUserInfor = assignWorkingUser;
						// send notification
						if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) &&
							 user.ActionTypeId != 1)
						{
							switch (objType)
							{
								//case 4:
								//	this.SendNotification(assignWorkingUser, user);
								//	break;
								//case 2:
								//	this.SendNotificationFCRDCN(assignWorkingUser, user);
								//	break;
								//case 3:
								//	//SendNotificationNCRSI(assignWorkingUser, user);
								//	break;
								case 5:
									//SendNotificationRFI(assignWorkingUser, user);
									break;
							}


						}
						if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && user.ActionTypeId == 1)
						{
							switch (objType)
							{
								//case 4:
								//	this.SendNotificationInfor(assignWorkingUser, user);
								//	break;
								//case 2:
								//	this.SendNotificationInforFCRDCN(assignWorkingUser, user);
								//	break;
								//case 3:
								////	SendNotificationInforNCR(assignWorkingUser, user);
								//	break;
								case 5:
								//	SendNotificationInforRFI(assignWorkingUser, user);
									break;
							}

						}
					}

					// Update current info for Object
					switch (objType)
					{
						case 1:
							var docObj = (PECC2Documents)obj;
							docObj.IsInWFProcess = true;
							docObj.IsWFComplete = false;
							docObj.CurrentWorkflowName = wfObj.Name;
							docObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
							docObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

							this.pecc2DocumentService.Update(docObj);
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
							ncrsiObj.IsInWFProcess = true;
							ncrsiObj.IsWFComplete = false;
							ncrsiObj.CurrentWorkflowName = wfObj.Name;
							ncrsiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
							ncrsiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

							this.ncrSiService.Update(ncrsiObj);
							break;
						case 2:
						case 4:
							var incomingTransObj = (PECC2Transmittal)obj;
							incomingTransObj.IsInWFProcess = true;
							incomingTransObj.IsWFComplete = false;
							incomingTransObj.CurrentWorkflowName = wfObj.Name;
							incomingTransObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
							incomingTransObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

							this.pecc2TransmittalService.Update(incomingTransObj);
							break;
						case 5:
							var rfiObj = (RFI)obj;
							rfiObj.IsInWFProcess = true;
							rfiObj.IsWFComplete = false;
							rfiObj.CurrentWorkflowName = wfObj.Name;
							rfiObj.CurrentWorkflowStepName = assignWorkFlow.CurrentWorkflowStepName;
							rfiObj.CurrentAssignUserName = assignWorkingUserInfor.UserFullName;

							this.rfiService.Update(rfiObj);
							break;
					}
					// ----------------------------------------------------------------------------------------------

					// Assign to re-assign user of workflow when can't find working user
					if (wfStepWorkingAssignUser.Count == 0 && wfObj.Re_assignUserId != null)
					{
						var assignWorkingUser = new ObjectAssignedUser
						{
							ID = Guid.NewGuid(),
							ObjectAssignedWorkflowID = assignWorkflowId,
							UserID = wfObj.Re_assignUserId,
							UserFullName = wfObj.Re_assignUserName,
							ReceivedDate = DateTime.Now,
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
							//case 2:
							//    var changeRequestObj = (ChangeRequest)obj;
							//    assignWorkingUser.ObjectID = changeRequestObj.ID;
							//    assignWorkingUser.ObjectNumber = changeRequestObj.Number;
							//    assignWorkingUser.ObjectTitle = changeRequestObj.Description;
							//    assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
							//    break;
							case 3:
								var ncrsiObj = (NCR_SI)obj;
								assignWorkingUser.ObjectID = ncrsiObj.ID;
								assignWorkingUser.ObjectNumber = ncrsiObj.Number;
								assignWorkingUser.ObjectTitle = ncrsiObj.Description;
								assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
								break;
							case 2:
							case 4:
								var incomingTransObj = (PECC2Transmittal)obj;
								assignWorkingUser.ObjectID = incomingTransObj.ID;
								assignWorkingUser.ObjectNumber = incomingTransObj.TransmittalNo;
								assignWorkingUser.ObjectTitle = incomingTransObj.Description;
								assignWorkingUser.ObjectProject = incomingTransObj.ProjectCodeName;
								break;
							case 5:
								var rfiObj = (RFI)obj;
								assignWorkingUser.ObjectID = rfiObj.ID;
								assignWorkingUser.ObjectNumber = rfiObj.Number;
								assignWorkingUser.ObjectTitle = rfiObj.Description;
								assignWorkingUser.ObjectProject = rfiObj.ProjectCode;
								break;
						}

						objAssignedUserService.Insert(assignWorkingUser);
					}
				}
			}
		}

		private List<User> GetAssignUserList(WorkflowDetail wfDetailObj, WorkflowStep wfStepObj, int groupId)
		{
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
				wfDetailObj.DistributionMatrixIDs.Split(';')
					.Where(t => !string.IsNullOrEmpty(t))
					.Select(t => this.matrixService.GetById(Convert.ToInt32(t)));
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
				if (userObj != null)
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
				if (userObj != null)
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
				if (userObj != null)
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
				if (userObj != null)
				{
					userObj.ActionTypeId = 4;
					userObj.ActionTypeName = "A - Approve";
					wfStepWorkingAssignUser.Add(userObj);
				}
			}

			

			return wfStepWorkingAssignUser;
		}

		private List<User> GetAssignUserListForCustomizeWorkflow(CustomizeWorkflowDetail wfDetailObj, WorkflowStep wfStepObj, int groupId)
		{
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

			return wfStepWorkingAssignUser;
		}

        private void SendNotification(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj, List<User> infoUser)
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
                foreach (var userObj in infoUser.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList())
                {
                    try
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
                    catch { }

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
        private void SendNotificationInfor(ObjectAssignedUser assignWorkingUser, List<User> infoUserIds)
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

                var Userlist = infoUserIds.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
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
				message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + ", " + assignWorkingUser.ObjectTitle;
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
							<td class='ch2'>Started Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
		 <td class='ch2'>Target Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"(" + wfdetail.Duration + @" Days Duration) </td>
							</tr>
							<tr>
	
								<td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>" + this.txtMessage.Text + @"</td>
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
				var Listdocrefer = this.pecc2DocumentService.GetAllByReferChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

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
				var ListdocRevised = this.pecc2DocumentService.GetAllByRevisedChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

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
		private void SendNotificationInforFCRDCN(ObjectAssignedUser assignWorkingUser, User assignUserObj)
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
						<span class='msg'>Dear " + (assignUserObj.FullName.Contains(" - ") ? assignUserObj.FullName.Split('-')[0] : assignUserObj.FullName) + @",
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
	
								<td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>" + this.txtMessage.Text + @"</td>
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
				var Listdocrefer = this.pecc2DocumentService.GetAllByReferChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

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
				var ListdocRevised = this.pecc2DocumentService.GetAllByRevisedChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

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
				smtpClient.Send(message);
			}
			catch (Exception ex) { }
		}
		private void SendNotificatioNCRSI(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj, List<User> infoUser)
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
                foreach (var userObj in infoUser.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList())
                {
                    try
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
                    catch { }

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
									<br><b>New To-do-list :</b> NCR/SI <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>action</b>
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
				var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/NCRSINewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
				if (ncrsiObj != null && ncrsiObj.Number.Contains("-CS-"))
				{
					st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/CSNewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
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
		private void SendNotificationInforNCRSI(ObjectAssignedUser assignWorkingUser, List<User> infoUserIds)
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

				var Userlist = infoUserIds.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
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
									<br><b>New To-do-list :</b> NCR/SI <b>" + assignWorkingUser.ObjectNumber + @"</b> is sent to you for your <b>information</b>
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
				var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/NCRSINewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
				if (ncrsiObj != null && ncrsiObj.Number.Contains("-CS-"))
				{
					st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/CSNewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
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
        private void SendNotificationRFI(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj, List<User> infoUser)
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
                foreach (var userObj in infoUser.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList())
                {
                    try
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
                    catch { }

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
                var Listdoc = this.rfideatilService.GetByRFI(RFIObj.ID).OrderBy(t => t.Number);


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
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='8'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='8'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailto + "'>" + emailto + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To-do-list Content</span></td><td  class='font_s' colspan='4'>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Target Date</span></td><td class='font_s' style='color:red' colspan='3'>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>RFI No.</span></td><td class='font_s' style='color:red'colspan='4' >" + assignWorkingUser.ObjectNumber + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s' style='color:#003399' colspan='3'>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
                   
                </tr>
               
                                <tr><td colspan='9' class='ch1'>RFI Details</td></tr>
                                <tr>
                                <td class='ch2'>Group</td>
								<td class='ch2'>Work Title</td>
								<td class='ch2'>Location</td>
								<td class='ch2'>Time</td>
								<td class='ch2'>Inspection type</td>
								<td class='ch2'>Contractor's contact</td>
								<td class='ch2'>Remark</td>
								<td class='ch2'>Action By</td>
								<td class='ch2'>Comment</td>
                                </tr>";
                var Listdoc = this.rfideatilService.GetByRFI(RFIObj.ID).OrderBy(t => t.Number);


                foreach (var document in Listdoc)
                {
                    bodyContent += @"<tr>
                               <td>" + document.GroupName + @"</td>
								<td>" + document.WorkTitle + @" </td>
								<td>" + document.Location + @" </td>
								<td>" + document.Time?.ToString("dd/MM/yyyy HH:mm") + @" </td>
								<td>" + document.InspectionTypeName + @" </td>
								<td>" + document.ContractorContact + @" </td>
								<td>" + document.Remark + @" </td>
								<td>" + document.EngineeringActionName + @" </td>
								<td>" + document.CommentContent + @" </td></tr>";
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

				var message = new MailMessage();
				message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
				message.BodyEncoding = new UTF8Encoding();
				message.IsBodyHtml = true;
				int count = 0;
				message.Subject = "FYA:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + ", " + assignWorkingUser.ObjectTitle;
				// Generate email body
				var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
				var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
				var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
				var ncrsiObj = this.shipmentService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
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
								<td class='ch2'>Started Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @"</td>
								<td class='ch2'>Target Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm") + @" (" + wfdetail.Duration + @" Days Duration)</td>
							</tr>
							<tr>
								<td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>" + this.txtMessage.Text + @"</td>
								<td class='ch2'>Next Step</td><td class='ch2'>:</td><td>" + nextStep?.Name + @"</td>
							</tr>
						   <tr><td colspan='6' class='ch1'>Details</td></tr>
							<tr>
								<td class='ch2'>Number</td><td class='ch2'>:</td><td>" + ncrsiObj.Number + @"</td>
								<td class='ch2'>Title/Description</td><td class='ch2'>:</td><td>" + ncrsiObj.Description + @"</td>
							</tr>
							<tr>
								<td class='ch2'>Issued Date</td><td class='ch2'>:</td><td>" + ncrsiObj.Date.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
								<td class='ch2'>Status</td><td class='ch2'>:</td><td>" + ncrsiObj.ShipmentStatusName + @"</td>
							</tr>";


				var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
				var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
				var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/CostContract/ShipmentList.aspx?ShipmentNo=" + assignWorkingUser.ObjectNumber;
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
		private void SendNotificationInforShipment(ObjectAssignedUser assignWorkingUser, User assignUserObj)
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

				var message = new MailMessage();
				message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
				message.BodyEncoding = new UTF8Encoding();
				message.IsBodyHtml = true;

				message.Subject = "FYI:  " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + assignWorkingUser.ObjectTitle;
				// Generate email body
				var wfdetail = this.wfDetailService.GetByCurrentStep(assignWorkingUser.CurrentWorkflowStepId.GetValueOrDefault());
				var currentWorkAssignedWF = this.objAssignedWfService.GetById(assignWorkingUser.ObjectAssignedWorkflowID.GetValueOrDefault());
				var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
				var ncrsiObj = this.shipmentService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
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
								<td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='info'>For Your Information</td>
								<td class='ch2'>Notification Description</td><td class='ch2'>:</td><td>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
							</tr>
							<tr>
								<td class='ch2'>Started Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
								<td class='ch2'>Target Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @" (" + wfdetail.Duration + @" Days Duration)</td>
							</tr>
							<tr>
								<td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>" + this.txtMessage.Text + @"</td>
								<td class='ch2'>Next Step</td><td class='ch2'>:</td><td>" + nextStep?.Name + @"</td>
							</tr>
						   <tr><td colspan='6' class='ch1'> Details</td></tr>
							<tr>
								<td class='ch2'> Number</td><td class='ch2'>:</td><td>" + ncrsiObj.Number + @"</td>
								<td class='ch2'>Title/Description</td><td class='ch2'>:</td><td>" + ncrsiObj.Description + @"</td>
							</tr>
							<tr>
								<td class='ch2'>Issued Date</td><td class='ch2'>:</td><td>" + ncrsiObj.Date.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
								<td class='ch2'>Status</td><td class='ch2'>:</td><td>" + ncrsiObj.ShipmentStatusName + @"</td>
							</tr>";


				var st = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber;
				var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/ToDoListPage.aspx";
				var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/CostContract/ShipmentList.aspx?ShipmentNo=" + assignWorkingUser.ObjectNumber;
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

				smtpClient.Send(message);
			}
			catch { }
		}
		private bool IsHoliday(DateTime date)
		{
			return Holidays.Contains(date);
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
		
		protected void btncancel_Click(object sender, EventArgs e)
		{
			ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
		}

		private void LoadComboData()
		{
			if (!string.IsNullOrEmpty(this.Request.QueryString["currentAssignId"]))
			{
				var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
				var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);
				//this.divNCRSICSReviewResult.Visible = currentWorkAssignedUser.ObjectTypeId == 3 &&(UserSession.Current.User.Role.IsInternal.GetValueOrDefault());
				if (currentWorkAssignedUser != null)
				{
					this.txtWorkflow.Text = currentWorkAssignedUser.WorkflowName;
					this.txtCurrentStep.Text = currentWorkAssignedUser.CurrentWorkflowStepName;

					var currentWorkAssignedWF = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
					if (currentWorkAssignedWF != null)
					{
						var nextStep = this.wfStepService.GetById(currentWorkAssignedWF.NextWorkflowStepID.GetValueOrDefault());
						if (nextStep != null)
						{
							Session.Add("NextStep", nextStep);
							this.txtNextStep.Text = nextStep.Name;

							var wfDetailObj = this.wfDetailService.GetByCurrentStep(nextStep.ID);
							if (wfDetailObj != null)
							{
								this.txtDuration.Value = wfDetailObj.Duration;
								this.DurationControl.Visible = false; //wfDetailObj.Duration == null;
							}
						}
					}
				}
			}

			//document code
			var documentcodelist = this.documnetCodeSErvie.GetAllReviewStatus();
			documentcodelist.Insert(0, new DocumentCode() { ID = 0, Code = string.Empty });
			this.ddlDocReviewStatus.DataSource = documentcodelist;
			this.ddlDocReviewStatus.DataTextField = "FullName";
			this.ddlDocReviewStatus.DataValueField = "ID";
			this.ddlDocReviewStatus.DataBind();

			var changeRequestReviewResult = this.changeRequestReviewResultCodeService.GetAll();
			//changeRequestReviewResult.Insert(0, new ChangeRequestReviewResultCode() { ID = 0 });
			this.ddlChangeRequestReviewCode.DataSource = changeRequestReviewResult.OrderBy(t => t.Code);
			this.ddlChangeRequestReviewCode.DataTextField = "Code";
			this.ddlChangeRequestReviewCode.DataValueField = "ID";
			this.ddlChangeRequestReviewCode.DataBind();
		}

		private void SendNotification_v1(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<User> infoOnlyUserList, List<User> managementUserList, IntergrateParamConfig configObj)
		{
			// Implement send mail function
			var smtpClient = new SmtpClient
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = configObj.Sync_UseDefaultCredentials.GetValueOrDefault(),
				EnableSsl = configObj.Sync_EnableSsl.GetValueOrDefault(),
				Host = configObj.Sync_MailServer,
				Port = Convert.ToInt32(configObj.Sync_Port),
				Credentials = new NetworkCredential(configObj.Sync_DefaultEmail, configObj.Sync_EmailPwd),
				Timeout = (60 * 5 * 1000)
			};

			var message = new MailMessage();
			message.From = new MailAddress(configObj.Sync_DefaultEmail, configObj.Sync_EmailName);
			message.BodyEncoding = new UTF8Encoding();
			message.IsBodyHtml = true;

			message.Subject = assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + "\" being in workflow process";

			// Generate email body
			var bodyContent = @"<<<FOR ACTION>>>
							REMINDER, you have the following document past due. Please action by due date for " + assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + @""":<br/>
								<table border='1' cellspacing='0'>
									<tr>
										<td style=""width: 200px;"">Current Workflow</td>
										<td style=""width: 500px;"">" + assignWorkingUser.WorkflowName + @"</td>
									</tr>
									<tr>
										<td>Current Workflow Step</td>
										<td>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
									</tr>
									<tr>
										<td>Title</td>
										<td>" + assignWorkingUser.ObjectTitle + @"</td>
									</tr>

									<tr>
										<td>Assign from User</td>
										<td>" + assignUserObj.FullNameWithDeptPosition + @"</td>
									</tr>
									<tr>
										<td>Received Date</td>
										<td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
									</tr>
									<tr>
										<td>Due Date</td>
										<td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
									</tr>
									
								</table></br>

								 EDMS WORKFLOW NOTIFICATION </br>
						[THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

								";
			message.Body = bodyContent;
			if (!string.IsNullOrEmpty(assignUserObj.Email))
			{
				message.To.Add(assignUserObj.Email);
			}

			foreach (var infoOnlyUser in infoOnlyUserList)
			{
				if (!string.IsNullOrEmpty(infoOnlyUser.Email))
				{
					message.CC.Add(infoOnlyUser.Email);
				}
			}

			foreach (var managementUser in managementUserList)
			{
				if (!string.IsNullOrEmpty(managementUser.Email))
				{
					message.CC.Add(managementUser.Email);
				}
			}

			smtpClient.Send(message);
		}

		protected void fileNameValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
		{
			if (this.ddlDocReviewStatus.SelectedItem ==null)
			{
				if (!string.IsNullOrEmpty(this.Request.QueryString["actionType"]) && this.Request.QueryString["actionType"] == "4")
				{
					this.fileNameValidator.ErrorMessage = "Please enter Document Code.";
					this.divDocCode.Style["margin-bottom"] = "-26px;";
					args.IsValid = false;
				}
			}
		}
		private void SendNotificationRFIForEPC(RFI RFIObj, User assignUserObj)
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

				var message = new MailMessage();
				message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
				message.BodyEncoding = new UTF8Encoding();
				message.IsBodyHtml = true;
				int count = 0;
				message.Subject = "FYI: RFI, " + DateTime.Now.ToString("dd/MM/yyyy") + @", " + RFIObj.Number;
				// Generate email body
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
							<br /><br />Please be informed about RFI detail:
							</span>
							<br /><br />
							<table border='1'>
							<tr><td colspan='9' class='ch1'>RFI General</td></tr>
							<tr>
								<td class='ch2'>RFI Number</td><td class='ch2'>:</td>
								<td colspan='7'>" + RFIObj.Number + @"</td>
							</tr>
							<tr>
								<td class='ch2'>Date</td><td class='ch2'>:</td>
								<td colspan='7'>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</td>
							</tr>
							<tr><td colspan='9' class='ch1'>RFI Details</td></tr>
							<tr>
								<td colspan='2' class='ch2'>Group</td>
								<td class='ch2'>Work Title</td>
								<td class='ch2'>Location</td>
								<td class='ch2'>Time</td>
								<td class='ch2'>Inspection type</td>
								<td class='ch2'>Contractor's contact</td>
								<td class='ch2'>Remark</td>
								<td class='ch2'>Action By</td>
								<td class='ch2'>Comment</td>
							</tr>";

				foreach (var document in this.rfideatilService.GetByRFI(RFIObj.ID).OrderBy(t=> t.Number))
				{
					bodyContent += @"<tr>
								<td colspan='2'>" + document.GroupName + @"</td>
								<td>" + document.WorkTitle + @" </td>
								<td>" + document.Location + @" </td>
								<td>" + document.Time?.ToString("dd/MM/yyyy HH:mm") + @" </td>
								<td>" + document.InspectionTypeName + @" </td>
								<td>" + document.ContractorContact + @" </td>
								<td>" + document.Remark + @" </td>
								<td>" + document.EngineeringActionName + @" </td>
								<td>" + document.CommentContent + @" </td>
							</tr> ";
				}

				var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/RFIList.aspx?RFINo=" + RFIObj.Number;

				bodyContent += @"</table>
							<div class='link'>
							<br />
							<u><b>Useful Links:</b></u>
							<ul>
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
                    if (assignUserObj.Email.Contains(";"))
                    {
                        foreach (string stemail in assignUserObj.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                        {
                            message.To.Add(new MailAddress(stemail.Trim()));
                        }
                    }
                    else
                    {
                        message.To.Add(new MailAddress(assignUserObj.Email.Trim()));
                    }

                    smtpClient.Send(message);
				}
			}
			catch (Exception ex) { }
		}
		private void AttachDocFileToProjectDoc(ChangeRequestDocFile changeRequestDocFile, PECC2Documents projectDoc)
		{
			var targetFolder = "../../DocumentLibrary/ProjectDocs";
			var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
							   + "/DocumentLibrary/ProjectDocs";
			var docFileName = changeRequestDocFile.FileName;

			// Path file to save on server disc
			var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileName);
			// Path file to download from server
			var serverFilePath = serverFolder + "/" + docFileName;

			File.Copy(Server.MapPath("../.." + changeRequestDocFile.FilePath), saveFilePath, true);

			var attachFile = new PECC2DocumentAttachFile()
			{
				ID = Guid.NewGuid(),
				ProjectDocumentId = projectDoc.ID,
				FileName = docFileName,
				Extension = changeRequestDocFile.Extension,
				FilePath = serverFilePath,
				ExtensionIcon = changeRequestDocFile.ExtensionIcon,
				FileSize = changeRequestDocFile.FileSize,
				TypeId = 1,
				TypeName = "Document file",
				CreatedBy = UserSession.Current.User.Id,
				CreatedByName = UserSession.Current.User.UserNameWithFullName,
				CreatedDate = DateTime.Now
			};

			projectDoc.IsHasAttachFile = true;
			this.pecc2DocumentService.Update(projectDoc);
			this.pecc2DocumentAttachFileService.Insert(attachFile);
		}

		private void CollectProjectDocData(ChangeRequestDocFile contractorDoc, PECC2Documents obj)
		{
			obj.ID = Guid.NewGuid();
			obj.DocNo = contractorDoc.DocumentNo;
			obj.DocTitle = contractorDoc.DocumentTitle;
			obj.OriginalDocumentNumber = contractorDoc.ContractorRefNo;
			obj.Revision = contractorDoc.Revision;
			obj.Date = contractorDoc.IssueDate;
			obj.Remarks = string.Empty;
			obj.ConfidentialityId = 0;
			obj.ConfidentialityName = string.Empty;
			obj.Revision = contractorDoc.Revision;
			obj.GroupId = contractorDoc.GroupCodeId;
			obj.GroupCode = contractorDoc.GroupCodeName;
			obj.AreaId = contractorDoc.AreaId;
			obj.AreaCode = contractorDoc.AreaName;
			obj.UnitId = contractorDoc.UnitCodeId;
			obj.UnitCode = contractorDoc.UnitCodeName;
			obj.DocTypeId = contractorDoc.DocumentTypeId;
			obj.DocTypeCode = contractorDoc.DocumentTypeName;
			obj.DisciplineId = contractorDoc.DisciplineCodeId;
			obj.DisciplineCode = contractorDoc.DisciplineCodeName;
			obj.Sequence = Convert.ToInt32(contractorDoc.Sequence);
			obj.SequenceText = contractorDoc.Sequence;
			obj.GroupId = contractorDoc.GroupCodeId;
			obj.GroupCode = contractorDoc.GroupCodeName;
			obj.KKSId = contractorDoc.KKSCodeId;
			obj.KKSCode = contractorDoc.KKSCodeName;
			obj.TrainNo = contractorDoc.TrainNo;
			obj.OriginatingOrganisationId = contractorDoc.OriginatingOrganizationId;
			obj.OriginatingOrganisationName = contractorDoc.OriginatingOrganizationName;
			obj.ReceivingOrganisationId = contractorDoc.ReceivingOrganizationId;
			obj.ReceivingOrganisationName = contractorDoc.ReceivingOrganizationName;
			var docTypeObj = this.documentTypeService.GetById(obj.DocTypeId.GetValueOrDefault());
			if (docTypeObj != null)
			{
				obj.CategoryId = Convert.ToInt32(docTypeObj.CategoryIds);
				obj.CategoryName = docTypeObj.CategoryName;
			}

			obj.Description = string.Empty;
			obj.Treatment = string.Empty;
			obj.RevRemarks = string.Empty;
		}
	}
}