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
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ReassignUser : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly WorkflowService wfService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly ObjectAssignedWorkflowService objAssignedWfService;
        private readonly ObjectAssignedUserService objAssignedUserService;
        private readonly WorkflowStepService wfStepService;
        private readonly WorkflowDetailService wfDetailService;
        private readonly MaterialRequisitionService mrService;
        private readonly WorkRequestService wrService;

        private readonly TrackingMOCService mocService;

        private readonly TrackingECRService ecrService;

        private readonly TrackingShutdownReportService shutdownReportService;

        private readonly TrackingBreakdownReportService breakdownReportService;

        private readonly HolidayConfigService holidayConfigService;

        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();

        private readonly RoleService roleService = new RoleService();
        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();
        private readonly PECC2DocumentsService PECC2DocumentService = new PECC2DocumentsService();
        private readonly ChangeRequestService changeRequestService = new ChangeRequestService();
        private readonly NCR_SIService ncrSiService = new NCR_SIService();
        private readonly PECC2TransmittalService pecc2TransmittalService = new PECC2TransmittalService();
        private readonly CustomizeWorkflowDetailService customizeWorkflowDetailService;
        private readonly RFIService rfiservice;
        private readonly ChangeGradeCodeService changeGradeCodeService;
        private readonly RFIDetailService rfideatilService;
        private readonly ShipmentService shipmentService;
        private int ObjId
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["objId"]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfoEditForm"/> class.
        /// </summary>
        public ReassignUser()
        {
            this.userService = new UserService();
            this.wfService = new WorkflowService();

            this.objAssignedUserService = new ObjectAssignedUserService();
            this.objAssignedWfService = new ObjectAssignedWorkflowService();

            this.wfStepService = new WorkflowStepService();
            this.wfDetailService = new WorkflowDetailService();
            this.mrService = new MaterialRequisitionService();
            this.wrService = new WorkRequestService();

            this.mocService = new TrackingMOCService();
            this.ecrService = new TrackingECRService();
            this.breakdownReportService = new TrackingBreakdownReportService();
            this.shutdownReportService = new TrackingShutdownReportService();
            this.holidayConfigService = new HolidayConfigService();
            this.customizeWorkflowDetailService = new CustomizeWorkflowDetailService();
            this.rfiservice = new RFIService();
            this.changeGradeCodeService = new ChangeGradeCodeService();
            this.rfideatilService = new RFIDetailService();
            this.shipmentService = new ShipmentService();
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
            var listuser = this.rtvUser.CheckedNodes.Where(t => !string.IsNullOrEmpty(t.Value) && t.Target == "User");
            if (currentWorkAssignedUser != null && listuser.Any())
            {
                var wfDetailObj = this.wfDetailService.GetByCurrentStep(currentWorkAssignedUser.CurrentWorkflowStepId.GetValueOrDefault());
                var actualDeadline = currentWorkAssignedUser.PlanCompleteDate;
              

                List<User> ListUserAction = new List<Data.Entities.User>();
                // Reassign to checked user
                var usserconsolidate = "";
                foreach (RadTreeNode user in listuser)
                {
                    var userObj = this.userService.GetByID(Convert.ToInt32(user.Value));
                    usserconsolidate = userObj.Id.ToString() ;
                    var assignWorkingUser = new ObjectAssignedUser
                    {
                        ID = Guid.NewGuid(),
                        ObjectAssignedWorkflowID = currentWorkAssignedUser.ObjectAssignedWorkflowID,
                        UserID = userObj.Id,
                        UserFullName = userObj.FullName,
                        ReceivedDate = DateTime.Now,
                        PlanCompleteDate = actualDeadline,
                        IsOverDue = false,
                        IsComplete = false,
                        IsReject = false,
                        IsLeaf=true,
                        AssignedBy = UserSession.Current.User.Id,
                        WorkflowId = currentWorkAssignedUser.WorkflowId,
                        WorkflowName = currentWorkAssignedUser.WorkflowName,
                        CurrentWorkflowStepName = currentWorkAssignedUser.CurrentWorkflowStepName,
                        CurrentWorkflowStepId = currentWorkAssignedUser.CurrentWorkflowStepId,
                        CanReject = currentWorkAssignedUser.CanReject,
                        IsCanCreateOutgoingTrans = wfDetailObj.IsCanCreateOutgoingTrans,
                        IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                        ActionTypeId = currentWorkAssignedUser.ActionTypeId,
                        ActionTypeName = currentWorkAssignedUser.ActionTypeName,
                        WorkingStatus = string.Empty,

                        IsMainWorkflow = currentWorkAssignedUser.IsMainWorkflow,
                        IsAddAnotherDisciplineLead = true,
                    };

                    switch (currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault())
                    {
                        case 1:
                            var docObj = this.PECC2DocumentService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault());
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
                        //case 2:
                        //    var changeRequestObj = this.changeRequestService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault()); ;
                        //    assignWorkingUser.ObjectID = changeRequestObj.ID;
                        //    assignWorkingUser.ObjectNumber = changeRequestObj.Number;
                        //    assignWorkingUser.ObjectTitle = changeRequestObj.Description;
                        //    assignWorkingUser.ObjectProject = changeRequestObj.ProjectCode;
                        //    assignWorkingUser.ObjectProjectId = changeRequestObj.ProjectId;
                        //    assignWorkingUser.Categoryid = 0;
                        //    assignWorkingUser.ObjectType = "Change Request";
                        //    assignWorkingUser.ObjectTypeId = 2;
                        //    break;
                        case 3:
                            var ncrsiObj = this.ncrSiService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault()); ;
                            assignWorkingUser.ObjectID = ncrsiObj.ID;
                            assignWorkingUser.ObjectNumber = ncrsiObj.Number;
                            assignWorkingUser.ObjectTitle = ncrsiObj.Subject;
                            assignWorkingUser.ObjectProject = ncrsiObj.ProjectName;
                            assignWorkingUser.ObjectProjectId = ncrsiObj.ProjectId;
                            assignWorkingUser.Categoryid = 0;
                            assignWorkingUser.ObjectType = "NCR/SI/CS";
                            assignWorkingUser.ObjectTypeId = 3;
                            break;
                        case 2:
                        case 4:
                            assignWorkingUser.ObjectID = currentWorkAssignedUser.ObjectID;
                            assignWorkingUser.ObjectNumber = currentWorkAssignedUser.ObjectNumber;
                            assignWorkingUser.ObjectTitle = currentWorkAssignedUser.ObjectTitle;
                            assignWorkingUser.ObjectProject = currentWorkAssignedUser.ObjectProject;
                            assignWorkingUser.ObjectProjectId = currentWorkAssignedUser.ObjectProjectId;
                            assignWorkingUser.Revision = currentWorkAssignedUser.Revision;
                            assignWorkingUser.Categoryid = currentWorkAssignedUser.Categoryid;
                            assignWorkingUser.ObjectType = currentWorkAssignedUser.ObjectType;
                            assignWorkingUser.ObjectTypeId = currentWorkAssignedUser.ObjectTypeId;
                            break;
                        case 5:
                            var rfiObj = this.rfiservice.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault()); ;
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
                            var shipObj = this.shipmentService.GetById(currentWorkAssignedUser.ObjectID.GetValueOrDefault()); ;
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

                    objAssignedUserService.Insert(assignWorkingUser);

                    // Send email notification
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                    {
                        switch (currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault())
                        {
                            case 4:
                                //this.SendNotification(assignWorkingUser, this.userService.GetByID(Convert.ToInt32(user.Value)));
                                //break;
                            case 2:
                                //this.SendNotificationFCRDCN(assignWorkingUser, this.userService.GetByID(Convert.ToInt32(user.Value)));
                                //break;
                            case 3:
                                // this.SendNotificationNCRSI(assignWorkingUser, this.userService.GetByID(Convert.ToInt32(user.Value)));
                                ListUserAction.Add(userObj);
                                break;
                            case 5:
                                this.SendNotificationRFI(assignWorkingUser, this.userService.GetByID(Convert.ToInt32(user.Value)));
                                break;
                            case 6:
                                this.SendNotificationShipment(assignWorkingUser, this.userService.GetByID(Convert.ToInt32(user.Value)));
                                break;
                        }
                    }
                    // -----------------------------------------------------------------------------------------
                }

                // Update Current work assign
                currentWorkAssignedUser.CommentContent = this.txtMessage.Text.Trim();
                currentWorkAssignedUser.IsReject = false;
                currentWorkAssignedUser.IsComplete = true;
                currentWorkAssignedUser.IsLeaf = false;
                currentWorkAssignedUser.ActualDate = DateTime.Now;
                currentWorkAssignedUser.IsOverDue = currentWorkAssignedUser.PlanCompleteDate.GetValueOrDefault().Date < currentWorkAssignedUser.ActualDate.GetValueOrDefault().Date;
                this.objAssignedUserService.Update(currentWorkAssignedUser);
                // ---------------------------------------------------------------------------------------------
                //update Wf detail step2 consolidate
                if (listuser.Count() >0)
                {
                    switch (currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault())
                    {
                        case 4:
                        case 2:
                           
                            var currentWorkAssignedWf = this.objAssignedWfService.GetById(currentWorkAssignedUser.ObjectAssignedWorkflowID.GetValueOrDefault());
                            if (currentWorkAssignedWf != null)
                            {
                                var objstep = this.wfStepService.GetById(currentWorkAssignedWf.CurrentWorkflowStepID.GetValueOrDefault());
                                var nextStep = this.wfStepService.GetById(currentWorkAssignedWf.NextWorkflowStepID.GetValueOrDefault());
                                if (nextStep != null)
                                {
                                    var currentwfDetail = this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(objstep.ID,
                                    currentWorkAssignedUser.ObjectID.GetValueOrDefault());
                                    var EditwfDetailObj =
                                this.customizeWorkflowDetailService.GetByCurrentStepCustomizeFromTrans(nextStep.ID,
                                    currentWorkAssignedUser.ObjectID.GetValueOrDefault());
                                  if(currentwfDetail.IsFirst.GetValueOrDefault() && nextStep.ActionApplyCode.Contains("C"))
                                    {
                                        EditwfDetailObj.ConsolidateUserIds = usserconsolidate + ";";
                                        EditwfDetailObj.DistributionMatrixIDs = string.Empty;
                                        this.customizeWorkflowDetailService.Update(EditwfDetailObj);

                                    }

                                }
                            }
                                break;

                    }
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                {
                    switch (currentWorkAssignedUser.ObjectTypeId.GetValueOrDefault())
                    {
                        case 3:
                            this.SendNotificatioNCRSI(currentWorkAssignedUser, ListUserAction);
                            break;
                        case 2:
                        case 4:
                            this.SendNotification(currentWorkAssignedUser, ListUserAction);
                            break;
                    }
                }
                    }
            
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", !string.IsNullOrEmpty(this.Request.QueryString["flag"]) ? "Close();" : "CloseAndRebind();", true);
        }

        private void ProcessWorkflow(WorkflowStep wfStepObj, int wfId, object obj, int assignedBy, string objType, int assignUserId, Guid assignWorkflowId)
        {
            var assignUserObj = this.userService.GetByID(assignUserId);
            var wfObj = this.wfService.GetById(wfId);
            var wfDetailObj = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);
            if (wfDetailObj != null)
            {
                var actualDeadline = DateTime.Now;
                if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                {
                    for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                    {
                        actualDeadline = this.GetNextWorkingDay(actualDeadline);
                    }
                }

                var currentWFStepDetail = this.wfDetailService.GetByCurrentStep(wfStepObj.ID);

                // Create assign user info
                var assignUser = new ObjectAssignedUser
                {
                    ID = Guid.NewGuid(),
                    ObjectAssignedWorkflowID = assignWorkflowId,
                    UserID = assignUserId,
                    ReceivedDate = DateTime.Now,
                    PlanCompleteDate = wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault() ? actualDeadline : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                    IsOverDue = false,
                    IsComplete = false,
                    IsReject = false,
                    AssignedBy = assignedBy,
                    WorkflowId = wfObj.ID,
                    WorkflowName = wfObj.Name,
                    CurrentWorkflowStepName = wfStepObj.Name,
                    CurrentWorkflowStepId = wfStepObj.ID,
                    CanReject = wfStepObj.CanReject,

                    IsDistributeOnshore = currentWFStepDetail.NextWorkflowStepID == 0,
                    IsOnshoreComment = false,
                    IsFinal = wfDetailObj.NextWorkflowStepID == 0,
                    ActionTypeId = 1,
                    ActionTypeName = "Working",
                    WorkingStatus = string.Empty,
                };

                switch (objType)
                {
                    case "Material Requisition":
                        var mrObj = (MaterialRequisition)obj;
                        assignUser.ObjectID = mrObj.ID;
                        assignUser.ObjectNumber = mrObj.MRNo;
                        assignUser.ObjectTitle = mrObj.Justification;
                        assignUser.ObjectProject = mrObj.ProjectName;
                        assignUser.ObjectType = "Material Requisition";
                        break;
                    case "Work Request":
                        var wrObj = (WorkRequest)obj;
                        assignUser.ObjectID = wrObj.ID;
                        assignUser.ObjectNumber = wrObj.WRNo;
                        assignUser.ObjectTitle = wrObj.WRTitle;
                        assignUser.ObjectProject = wrObj.ProjectName;
                        assignUser.ObjectType = "Work Request";
                        break;
                    case "MOC":
                        var mocObj = (TrackingMOC)obj;
                        assignUser.ObjectID = mocObj.ID;
                        assignUser.ObjectNumber = mocObj.Code;
                        assignUser.ObjectTitle = "<b>Equiment/system: </b>" + mocObj.SystemName + "</br> <b>Description of change: </b>" + mocObj.DescriptionOfChange;
                        assignUser.ObjectProject = mocObj.ProjectName;
                        assignUser.ObjectType = "MOC";
                        break;
                    case "ECR":
                        var ecrObj = (TrackingECR)obj;
                        assignUser.ObjectID = ecrObj.ID;
                        assignUser.ObjectNumber = ecrObj.Code;
                        assignUser.ObjectTitle = "<b>ECT Title: </b>" + ecrObj.Title + "</br> <b>ECR Description: </b>" + ecrObj.Description;
                        assignUser.ObjectProject = ecrObj.ProjectName;
                        assignUser.ObjectType = "ECR";
                        break;
                    case "Breakdown Report":
                        var brObj = (TrackingBreakdownReport)obj;
                        assignUser.ObjectID = brObj.ID;
                        assignUser.ObjectNumber = brObj.Code;
                        assignUser.ObjectTitle = "<b>Name of Breakdown Equipments/System: </b>" + brObj.BreakdownSystemName + "</br> <b>Breakdown Equipment Name or Tag No: </b>" + brObj.TagNo + "</br> <b>Equipment/System Name: </b>" + brObj.SystemName + "</br> <b>Defective/Event Descriptions: </b>" + brObj.Description;
                        assignUser.ObjectProject = brObj.ProjectName;
                        assignUser.ObjectType = "Breakdown Report";
                        break;
                    case "Shutdown Report":
                        var srObj = (TrackingShutdownReport)obj;
                        assignUser.ObjectID = srObj.ID;
                        assignUser.ObjectNumber = srObj.Code;
                        assignUser.ObjectTitle = "<b>Date of shutdown: </b>" + srObj.DateOfShutdown.GetValueOrDefault().ToString("dd/MM/yyyy") + "</br> <b>Time of Shutdown: </b>" + srObj.TimeOfShutdown.GetValueOrDefault().ToString("HH:mm") + "</br> <b>Cause Of Shutdown: </b>" + srObj.CauseShutdown;
                        assignUser.ObjectProject = srObj.ProjectName;
                        assignUser.ObjectType = "Shutdown Report";
                        break;
                }

                objAssignedUserService.Insert(assignUser);

                // Update current info for Object
                switch (objType)
                {
                    case "Material Requisition":
                        var mrObj = (MaterialRequisition)obj;
                        mrObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.mrService.Update(mrObj);
                        break;
                    case "Work Request":
                        var wrObj = (WorkRequest)obj;
                        wrObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.wrService.Update(wrObj);
                        break;
                    case "MOC":
                        var mocObj = (TrackingMOC)obj;
                        mocObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.mocService.Update(mocObj);
                        break;
                    case "ECR":
                        var ecrObj = (TrackingECR)obj;
                        ecrObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.ecrService.Update(ecrObj);
                        break;
                    case "Breakdown Report":
                        var brObj = (TrackingBreakdownReport)obj;
                        brObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.breakdownReportService.Update(brObj);
                        break;
                    case "Shutdown Report":
                        var srObj = (TrackingShutdownReport)obj;
                        srObj.CurrentAssignUserName = assignUserObj.FullNameWithDeptPosition;
                        this.shutdownReportService.Update(srObj);
                        break;
                }
                // ----------------------------------------------------------------------------------------------
            }
        }
        private void SendNotification(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj)
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
                                message.To.Add(new MailAddress(stemail));
                            }
                        }
                        else
                        {
                            message.To.Add(new MailAddress(user.Email));
                        }
                        emailto += user.Email + ";";
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
                            <td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='action'>Action is Reassigned to You</td>
                            <td class='ch2'>Notification Description</td><td class='ch2'>:</td><td>" + assignWorkingUser.CurrentWorkflowStepName + @"</td>
                        </tr>
                        <tr>
                            <td class='ch2'>Started Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.ReceivedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                            <td class='ch2'>Target Date</td><td class='ch2'>:</td><td>" + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd / MM / yyyy") + @"(" + wfdetail.Duration + @" Days Duration) </ td >
                            </ tr >
                            < tr >
    
                                < td class='ch2'>Message From Previous Step</td><td class='ch2'>:</td><td>" + this.txtMessage.Text + @"</td>
                            <td class='ch2'>Next Step</td><td class='ch2'>:</td><td>" + nextStep?.Name + @"</td>
                        </tr>
                        <tr><td colspan = '6' class='ch1'>DCN/FCR Details</td></tr>
                        <tr>
                            <td class='ch2'>DCN/FCR Number</td><td class='ch2'>:</td><td>" + changerequestObj.Number + @"</td>
                            <td class='ch2'>Remarks/ Title</td><td class='ch2'>:</td><td>" + changerequestObj.Description + @"</td>
                        </tr>
                        <tr>
                            <td class='ch2'>Rev</td><td class='ch2'>:</td><td>" + changerequestObj.Revision + @"</td>
                            <td class='ch2'>Grade</td><td class='ch2'>:</td><td>" + this.changeGradeCodeService.GetById(changerequestObj.ChangeGradeCodeId.GetValueOrDefault()).FullName + @"</td>
                        </tr>
                        <tr><td colspan = '6' class='ch1'>Reference Documents</td></tr>
                        <tr>
                            <td colspan = '2' class='ch2'>Document No.</td>
                            <td class='ch2'>Rev.</td>
                            <td colspan = '3' class='ch2'>Remarks/ Title</td>
                        </tr>";
                var Listdocrefer = this.PECC2DocumentService.GetAllByReferChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

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
                var ListdocRevised = this.PECC2DocumentService.GetAllByRevisedChangeRequest(changerequestObj.ID).OrderBy(t => t.DocNo);

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
        private void SendNotificatioNCRSI(ObjectAssignedUser assignWorkingUser, List<User> assignUserObj)
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
                        message.To.Add(new MailAddress(user.Email));
                        emailto += user.Email + ";";
                    }
                    catch { }
                }

                //var ncrsiObj = this.ncrsiService.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());
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
                var st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/NCRSINewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
                if (assignWorkingUser.ObjectNumber.Contains("-CS-"))
                {
                    st2 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/CSNewList.aspx?NCRSINo=" + assignWorkingUser.ObjectNumber;
                }
                bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
                                <li>
                                    Click <a href='" + st2 + @"'>here</a> to show <u>" + (assignWorkingUser.ObjectNumber.Contains("-CS-") ? "this CS" : "this NCR/SI") + @"</u> in DMDC System
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
        private void SendNotificationRFI(ObjectAssignedUser assignWorkingUser, User assignUserObj)
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

                var RFIObj = this.rfiservice.GetById(assignWorkingUser.ObjectID.GetValueOrDefault());

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
                            <tr><td colspan='6' class='ch1'>Workflow Step Details</td></tr>
                            <tr>
                                <td class='ch2'>Notification Purpose</td><td class='ch2'>:</td><td class='action'>Action is Reassigned to You</td>
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
                           <tr><td colspan='6' class='ch1'>RFI General</td></tr>
                                <tr>
                                    <td class='ch2'>RFI Number</td><td class='ch2'>:</td><td>" + RFIObj.Number + @"</td>
                                    <td class='ch2'>Date</td><td class='ch2'>:</td><td>" + RFIObj.IssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                                </tr>
                                <tr><td colspan='6' class='ch1'>RFI Details</td></tr>
                                <tr>
                                    <td class='ch2'>Group</td>
                                    <td colspan='5' class='ch2'>Work Title</td>
                                </tr>";
                var Listdoc = this.rfideatilService.GetByRFI(RFIObj.ID).OrderBy(t => t.Number);


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
                            <div class='link'>
                            <br />
                            <u><b>Useful Links:</b></u>
                            <ul>
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
            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
        }

        private void LoadComboData()
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["currentAssignId"]))
            {
                var currentWorkAssignedUserId = new Guid(this.Request.QueryString["currentAssignId"]);
                var currentWorkAssignedUser = this.objAssignedUserService.GetById(currentWorkAssignedUserId);

                if (currentWorkAssignedUser != null)
                {
                    this.txtWorkflow.Text = currentWorkAssignedUser.WorkflowName;
                    this.txtCurrentStep.Text = currentWorkAssignedUser.CurrentWorkflowStepName;
                }
            }

            var roleList = UserSession.Current.User.Role.IsAdmin.GetValueOrDefault()? this.roleService.GetAll(false).Where(t => t.Id != 1).OrderBy(t => t.FullNameWithLocation) : this.roleService.GetAll(false).Where(t => t.Id != 1 && t.ContractorId== UserSession.Current.User.Role.ContractorId);
            if (UserSession.Current.User.IsEngineer.GetValueOrDefault())
            {
                roleList = roleList.Where(t => t.Id == UserSession.Current.User.RoleId);
            }
            foreach (var role in roleList)
            {
                var roldeNode = new RadTreeNode(role.FullNameWithLocation, role.Id.ToString());
                roldeNode.Target = "Role";
                var userList = this.userService.GetAllByRoleId(role.Id);
                foreach (var user in userList)
                {
                    var userNode = new RadTreeNode(user.UserNameWithFullNamePosition, user.Id.ToString());
                    userNode.Target = "User";
                    roldeNode.Nodes.Add(userNode);
                }

                this.rtvUser.Nodes.Add(roldeNode);
                roldeNode.Expanded = true;
            }
        }
    }
}