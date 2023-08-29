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
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Workflow;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ObjectWorkflowDetailEdit : Page
    {

        private readonly WorkflowStepService wfStepService = new WorkflowStepService();
        private readonly WorkflowService wfService = new WorkflowService();
       
        private readonly ScopeProjectService projectService = new ScopeProjectService();
        private readonly ObjectWorkflowDetailService ObjWfDetailService = new ObjectWorkflowDetailService();
        private readonly TemplateWorkflowDetailService templateWFDetailService = new TemplateWorkflowDetailService();
        private readonly DistributionMatrixService matrixService= new DistributionMatrixService();
        private readonly DistributionMatrixDetailService matrixDetailService= new DistributionMatrixDetailService();

        private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService= new ContractorTransmittalDocFileService();
        private readonly DQRETransmittalService dqreTransmittalService= new DQRETransmittalService();
        private readonly AttachDocToTransmittalService attachDocToTransmittalService= new AttachDocToTransmittalService();

        private readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();

        private readonly RoleService roleService= new RoleService();
        private readonly ObjectWorkflowDetailService objectWorkflowDetailService= new ObjectWorkflowDetailService();
        private readonly HolidayConfigService holidayConfigService= new HolidayConfigService();
        private readonly IntergrateParamConfigService configService= new IntergrateParamConfigService();

        private readonly DQREDocumentService documentService= new DQREDocumentService();
        private readonly UserService userService= new UserService();
        private readonly ObjectAssignedWorkflowService objAssignedWfService= new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService= new ObjectAssignedUserService();
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
        
        private ScopeProject ProjectObj
        {
            get { return this.projectService.GetById(this.WorkflowObj.ProjectID.GetValueOrDefault()); }
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
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.IsPostBack)
            {
                this.divMessage.Visible = false;
                var firstStep = this.wfStepService.GetAllByWorkflow(WorkflowId).FirstOrDefault(t => t.IsFirst.GetValueOrDefault());
                if (firstStep != null)
                {
                    this.txtFirstStep.Text = firstStep.WorkflowName;
                }
               
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var listTemplate = this.objectWorkflowDetailService.GetAllByObjectWorkflow(this.WorkflowId, this.ObjId).ToList();
            if (listTemplate.Count > 0)
            {
                this.grdDocument.DataSource = listTemplate;
            }
            else
            {
                this.grdDocument.DataSource = new List<ObjectWorkflowDetail>();
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
                var wfDetailID = Convert.ToInt32(newValues["ID"].ToString());
                var wfDetailObj = this.objectWorkflowDetailService.GetById(wfDetailID);

                if (wfDetailObj != null &&  wfDetailObj.CanEdit.GetValueOrDefault())
                {

                    var nextWorkflowStepID = Convert.ToInt32(newValues["NextWorkflowStepID"].ToString());
                    var nextWorkflowStepObj = this.wfStepService.GetById(nextWorkflowStepID);
                    var rejectWorkflowStepID = Convert.ToInt32(newValues["RejectWorkflowStepID"].ToString());
                    var rejectWorkflowStepObj = this.wfStepService.GetById(rejectWorkflowStepID);

                    wfDetailObj.NextWorkflowStepID = nextWorkflowStepObj != null
                        ? nextWorkflowStepObj.ID
                        : 0;
                    wfDetailObj.NextWorkflowStepName = nextWorkflowStepObj != null
                        ? nextWorkflowStepObj.Name
                        : string.Empty;

                    wfDetailObj.RejectWorkflowStepID = rejectWorkflowStepObj != null
                        ? rejectWorkflowStepObj.ID
                        : 0;
                    wfDetailObj.RejectWorkflowStepName = rejectWorkflowStepObj != null
                        ? rejectWorkflowStepObj.Name
                        : string.Empty;

                    wfDetailObj.Duration = !string.IsNullOrEmpty(newValues["Duration"].ToString()) 
                                        ? Convert.ToInt32(newValues["Duration"].ToString())
                                        : (double?) null;
                    wfDetailObj.IsOnlyWorkingDay = true;

                    this.objectWorkflowDetailService.Update(wfDetailObj);
                    List<int> ListObjWFDetail = new List<int>();
                    if(Session["ListEdit"]!= null)
                    {  
                        ListObjWFDetail = (List<int>)Session["ListEdit"] ;
                        ListObjWFDetail.Add(wfDetailObj.ID);
                        Session["ListEdit"] = ListObjWFDetail;
                    }
                    else
                    {
                        ListObjWFDetail.Add(wfDetailObj.ID);
                        Session["ListEdit"] = ListObjWFDetail;
                    }
                  
                }
              
            }
        }

        protected void grdDocument_OnItemUpdated(object sender, GridUpdatedEventArgs e)
        {
            var x = 0;
            var item = (GridEditableItem)e.Item;
            var id = item.GetDataKeyValue("ID").ToString();
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var temp = e.CommandName;
        }
      
       
        protected void grdDocument_OnPreRender(object sender, EventArgs e)
        {
            var wfStepList = this.wfStepService.GetAllByWorkflow(this.WorkflowId);

            var ddlAcceptStep = (this.grdDocument.MasterTableView.GetBatchEditorContainer("NextWorkflowStepID") as Panel).FindControl("ddlAcceptStep") as RadComboBox;
            var ddlRejectStep = (this.grdDocument.MasterTableView.GetBatchEditorContainer("RejectWorkflowStepID") as Panel).FindControl("ddlRejectStep") as RadComboBox;

            if (ddlAcceptStep != null && ddlRejectStep != null)
            {
                wfStepList.Insert(0, new WorkflowStep() {ID = 0});
                ddlAcceptStep.DataSource = wfStepList;
                ddlAcceptStep.DataTextField = "Name";
                ddlAcceptStep.DataValueField = "ID";
                ddlAcceptStep.DataBind();

                ddlRejectStep.DataSource = wfStepList;
                ddlRejectStep.DataTextField = "Name";
                ddlRejectStep.DataValueField = "ID";
                ddlRejectStep.DataBind();
            }
            }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["ListEdit"] != null)
            {
                List<int> ListObjWFDetail = new List<int>();
                ListObjWFDetail = (List<int>)Session["ListEdit"];
                foreach (var item in ListObjWFDetail.Distinct())
                {
                    var ObjWFDetail = this.objectWorkflowDetailService.GetById(item);
                    var documentObj = this.documentService.GetById(this.ObjId);
                    var WFObj = this.wfService.GetById(ObjWFDetail.WorkflowID.GetValueOrDefault());
                    var WFStep = this.wfStepService.GetById(ObjWFDetail.CurrentWorkflowStepID.GetValueOrDefault());
                    var ObjAssignWF = this.objAssignedWfService.ObjWfDetail(ObjWFDetail.ID);
                    var date = DateTime.Now;
                    // delete in table objectassignuser
                    var listUserWF = this.objAssignedUserService.GetAllListObjAssignWF(ObjAssignWF.ID);
                    foreach (var obj in listUserWF)
                    {
                        date = obj.ReceivedDate.GetValueOrDefault();
                        this.objAssignedUserService.Delete(obj);
                    }


                    this.ProcessWorkflow(WFStep, ObjWFDetail, WFObj, documentObj, ObjAssignWF, date);
                }
            }
            this.lblError.Text = "Message:  Edit workflow successful!";
            this.divMessage.Visible = true;
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

        private void ProcessWorkflow(WorkflowStep wfStepObj,ObjectWorkflowDetail wfDetailObj, Data.Entities.Workflow wfObj, DQREDocument docObj, ObjectAssignedWorkflow ObjAssignWF, DateTime CurrenDate)
        {
            //var wfDetailObj = this.objectWorkflowDetailService.GetById(ID);
            if (wfDetailObj != null)
            {
               
                if (ObjAssignWF != null)
                {
                    // Get actual deadline if workflow step detail use only working day
                    var actualDeadline = CurrenDate;
                    if (wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault())
                    {
                        for (int i = 1; i <= wfDetailObj.Duration.GetValueOrDefault(); i++)
                        {
                            actualDeadline = this.GetNextWorkingDay(actualDeadline);
                        }
                    }
                    // -------------------------------------------------------------------------

                    // Get assign User List
                    var wfStepWorkingAssignUser = new List<User>();
                    var infoUserIds = wfDetailObj.InformationOnlyUserIDs != null ? wfDetailObj.InformationOnlyUserIDs.Split(';').ToList() : new List<string>();
                    var commentUserIds = wfDetailObj.CommentUserIds != null ? wfDetailObj.CommentUserIds.Split(';').ToList() : new List<string>();
                    var reviewUserIds = wfDetailObj.ReviewUserIds != null ? wfDetailObj.ReviewUserIds.Split(';').ToList() : new List<string>();
                    var approveUserIds = wfDetailObj.ApproveUserIds != null ? wfDetailObj.ApproveUserIds.Split(';').ToList() : new List<string>();
                    var managementUserIds = wfDetailObj.ManagementUserIds != null ? wfDetailObj.ManagementUserIds.Split(';').ToList() : new List<string>();

                    var matrixList = wfDetailObj.DistributionMatrixIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => this.matrixService.GetById(Convert.ToInt32(t)));
                    foreach (var matrix in matrixList)
                    {
                        var matrixDetailValid = new List<DistributionMatrixDetail>();
                        var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID);

                        // Filter follow matrix rule
                        switch (matrix.TypeId)
                        {
                            // Document have Material/Work Code
                            case 1:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
                                break;
                            // Document have Drawing Code (00) Matrix
                            case 2:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
                                break;
                            // Document have Drawing Code Matrix
                            case 3:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.DisciplineId == docObj.M_DisciplineId).ToList();
                                break;
                            // AU, CO, PLG, QIR, GTC, PO Matrix
                            case 4:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId).ToList();
                                break;
                            // EL, ML Matrix
                            case 5:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.UnitCodeId == docObj.M_UnitId).ToList();
                                break;
                            // PP Matrix
                            case 6:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.SerialNo == docObj.SerialNo).ToList();
                                break;
                            // Vendor Document Matrix
                            case 7:
                                matrixDetailValid = matrixDetailList.Where(t => t.DocTypeId == docObj.M_DocumentTypeId && t.MaterialCodeId == docObj.M_MaterialCodeId).ToList();
                                break;
                        }


                        infoUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 1).Select(t => t.UserId.ToString()));
                        commentUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 2).Select(t => t.UserId.ToString()));
                        reviewUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 3).Select(t => t.UserId.ToString()));
                        approveUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 4).Select(t => t.UserId.ToString()));
                        managementUserIds.AddRange(matrixDetailValid.Where(t => t.ActionTypeId == 5).Select(t => t.UserId.ToString()));
                    }

                    foreach (var userId in commentUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 2;
                            userObj.ActionTypeName = "C - Comment";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (var userId in reviewUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 3;
                            userObj.ActionTypeName = "R - Review";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (var userId in approveUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 4;
                            userObj.ActionTypeName = "A - Approve";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }

                    foreach (var userId in managementUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 5;
                            userObj.ActionTypeName = "M - Management";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }



                    foreach (var userId in infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)))
                    {
                        var userObj = this.userService.GetByID(userId);
                        if (userObj != null)
                        {
                            userObj.ActionTypeId = 1;
                            userObj.ActionTypeName = "I - For Information";
                            wfStepWorkingAssignUser.Add(userObj);
                        }
                    }
                    // ---------------------------------------------------------------------------
                   
                    // Create assign user info
                    foreach (var user in wfStepWorkingAssignUser)
                    {
                        var assignWorkingUser = new ObjectAssignedUser
                        {
                            ID = Guid.NewGuid(),
                            ObjectAssignedWorkflowID = ObjAssignWF.ID,
                            UserID = user.Id,
                            UserFullName=user.FullName,
                            ReceivedDate = CurrenDate,
                            PlanCompleteDate = wfDetailObj.IsOnlyWorkingDay.GetValueOrDefault() ? actualDeadline : DateTime.Now.AddDays(wfDetailObj.Duration.GetValueOrDefault()),
                            IsOverDue = false,
                            IsComplete = user.ActionTypeId == 1,
                            IsReject = false,
                            AssignedBy = UserSession.Current.User.Id,
                            Status= wfDetailObj.IsFirst.GetValueOrDefault()?"RS":"NR",
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

                            ObjectID = docObj.ID,
                            ObjectNumber = docObj.DocumentNo,
                            ObjectTitle = docObj.DocumentTitle,
                            ObjectProject = docObj.ProjectCodeName,
                            Revision = docObj.Revision,
                            IsMainWorkflow = true,
                            IsReassign = false,
                            IsLeaf = false
                        };

                        objAssignedUserService.Insert(assignWorkingUser);
                  
                    }

                }
            }
        }
        private void SendNotification(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<string> infoUserIds)
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
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Host"] + "\\" + ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };


                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "ASSIGNMENT: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

                // Generate email body
                var bodyContent = @"<<<< FOR ACTION >>>>
                             Please action by due date for " + assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + @""":<br/>
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
		                                <td>Assign From User</td>
                                        <td>" + this.userService.GetByID(assignWorkingUser.AssignedBy.GetValueOrDefault()).FullNameWithDeptPosition + @"</td>
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
                                   <br/>
                                   <br/>
                                    &nbsp;Click on the this link to access the PEDMS system&nbsp;:&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                           + "/ToDoListPage.aspx?DocNo=" + assignWorkingUser.ObjectNumber + "'>" + ConfigurationSettings.AppSettings.Get("WebAddress") + @"</a>
                                    </br>
                         &nbsp;&nbsp;&nbsp; EDMS WORKFLOW NOTIFICATION </br>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

                                ";
                message.Body = bodyContent;
                if (!string.IsNullOrEmpty(assignUserObj.Email))
                {
                    message.To.Add(assignUserObj.Email);
                }
                smtpClient.Send(message);
            }
            catch { }
        }
        private void SendNotificationInfor(ObjectAssignedUser assignWorkingUser, User assignUserObj, List<string> infoUserIds)
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
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Host"] + "\\" + ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
                };


                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
                message.BodyEncoding = new UTF8Encoding();
                message.IsBodyHtml = true;

                message.Subject = "INFORMATION: " + assignWorkingUser.ObjectNumber + ", " + assignWorkingUser.ObjectTitle + ", " + assignWorkingUser.CurrentWorkflowStepName + ", " + assignWorkingUser.PlanCompleteDate.GetValueOrDefault().ToString("dd/MM/yyyy");

                // Generate email body
                var bodyContent = @"<<<< FOR INFORMATION >>>>
                             Please be informed for " + assignWorkingUser.ObjectType + " \"" + assignWorkingUser.ObjectNumber + @""":<br/>
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
		                                <td>Assign From User</td>
                                        <td>" + this.userService.GetByID(assignWorkingUser.AssignedBy.GetValueOrDefault()).FullNameWithDeptPosition + @"</td>
	                                </tr>
                                </table></br>
                                   <br/>

                                    &nbsp;Click on the this link to access the PEDMS system&nbsp;:&nbsp; <a href='" + ConfigurationSettings.AppSettings.Get("WebAddress")
                                                   + "/ProjectDocumentsList.aspx'>" + ConfigurationSettings.AppSettings.Get("WebAddress") + @"</a>
                                    </br>
                         &nbsp;&nbsp;&nbsp; EDMS WORKFLOW NOTIFICATION </br>
                        [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]

                                ";
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
            catch { }
        }

        //protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item is GridDataItem)
        //    {
        //        var item = e.Item as GridDataItem;
        //        if (item["CanEdit"].Text == "False")
        //        {
        //            item.Enabled = false;
        //            item["Recipients"].Controls[0].Visible = false;
        //            item["Duration"].Controls[0].Visible = false;
        //        }
        //    }
        //}
    }
}