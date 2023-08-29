// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.Net;
using System.Net.Mail;
using System.Text;
using EDMs.Web.Utilities;

namespace EDMs.Web.Controls.Document
{
	/// <summary>
	/// Class customer
	/// </summary>
	public partial class PECC2TransmittalList : Page
	{
		private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

		private readonly ContractorTransmittalService contractorTransmittalService = new ContractorTransmittalService();

		private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();

		private readonly PECC2TransmittalService PECC2TransmittalService = new PECC2TransmittalService();

		private readonly OrganizationCodeService organizationCodeService = new OrganizationCodeService();

		private readonly AttachDocToTransmittalService attachDocToTransmittalService = new AttachDocToTransmittalService();

		private readonly PECC2DocumentsService pecc2DocumentsService = new PECC2DocumentsService();

		private readonly PECC2DocumentAttachFileService documentAttachFileService = new PECC2DocumentAttachFileService();

		private readonly RevisionStatuService revisionStatusService = new RevisionStatuService();

		private readonly PurposeCodeService purposeCodeService = new PurposeCodeService();

		private readonly UserService userService = new UserService();

		private readonly RoleService roleService = new RoleService();

		private readonly ObjectAssignedUserService objectAssignedUser = new ObjectAssignedUserService();

		private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

		private readonly ChangeRequestService changeRequestService = new ChangeRequestService();

		private readonly ChangeRequestAttachFileService changeRequestAttachFileService = new ChangeRequestAttachFileService();

		private readonly PECC2TransmittalAttachFileService pecc2TransmittalAttachFileService = new PECC2TransmittalAttachFileService();

		private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService = new ContractorTransmittalAttachFileService();

		private readonly ObjectAssignedWorkflowService objAssignedWfService = new ObjectAssignedWorkflowService();
		private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        private readonly CustomizeReceivedEmailService ReceivedEmailService = new CustomizeReceivedEmailService();
        private readonly DistributionMatrixService matrixService = new DistributionMatrixService();
        private readonly DistributionMatrixDetailService matrixDetailService = new DistributionMatrixDetailService();
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
            if (!UserSession.Current.User.Role.IsInternal.GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["TransNoContractor"]))
                {
                    Response.Redirect("/Controls/Document/ContractorTransmittalList.aspx?TransNoContractor=" + this.Request.QueryString["TransNoContractor"]); }
                else if (!string.IsNullOrEmpty(this.Request.QueryString["TransNoPecc2"]))
                {
                    Response.Redirect("/Controls/Document/ContractorTransmittalList.aspx?TransNoPecc2=" + this.Request.QueryString["TransNoPecc2"]);
                
                 }
                else
                {
                    Response.Redirect("/Controls/Document/ContractorTransmittalList.aspx");
                }
            }
            else
            {
                Session.Add("SelectedMainMenu", "Transmittals Management");

                this.Title = ConfigurationManager.AppSettings.Get("AppName");
                this.ServerName.Value = ConfigurationManager.AppSettings.Get("ServerName");
                var temp = (RadPane)this.Master.FindControl("leftPane");
                temp.Collapsed = true;
                if (!Page.IsPostBack)
                {
                    this.LoadComboData();
                    if (!string.IsNullOrEmpty(this.Request.QueryString["TransNoContractor"]))
                    {
                        var txtSearchIncoming = (TextBox)this.radMenuIncoming.Items[1].FindControl("txtSearchIncoming");
                        txtSearchIncoming.Text = Request.QueryString["TransNoContractor"];
                       
                    }
                    if (!string.IsNullOrEmpty(this.Request.QueryString["TransNoPecc2"]))
                    {
                        var txtSearchOutgoing = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearchOutgoing");
                        txtSearchOutgoing.Text = Request.QueryString["TransNoPecc2"];
                        var ddlStatusOutgoing = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatusOutgoing");
                        if (ddlStatusOutgoing != null)
                        {
                            ddlStatusOutgoing.SelectedIndex = 0;
                        }
                        this.OutgoingTransView.Selected = true;
                        RadTab tab1 = RadTabStrip1.Tabs.FindTabByText("Outgoing Transmittals");
                        tab1.Selected = true;
                    }
                }
            }
		}

		private void LoadComboData()
		{
			var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
			var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
			var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);

			if (ddlProjectOutgoing != null)
			{
				ddlProjectOutgoing.DataSource = projectList;
				ddlProjectOutgoing.DataTextField = "FullName";
				ddlProjectOutgoing.DataValueField = "ID";
				ddlProjectOutgoing.DataBind();

				int projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);
				this.lblProjectOutgoingId.Value = projectId.ToString();
				Session.Add("SelectedProject", projectId);
			}

			if (ddlProjectIncoming != null)
			{
				ddlProjectIncoming.DataSource = projectList;
				ddlProjectIncoming.DataTextField = "FullName";
				ddlProjectIncoming.DataValueField = "ID";
				ddlProjectIncoming.DataBind();

				int projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);
				this.lblProjectIncomingId.Value = projectId.ToString();
			}

			var ddlStatusOutgoing = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatusOutgoing");
			if (ddlStatusOutgoing != null)
			{
				ddlStatusOutgoing.SelectedIndex = 0;
			}

			this.grdIncomingTrans.MasterTableView.GetColumn("DeleteWorkflow").Visible = (UserSession.Current.User.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault());
			var ddlStatusIncoming = (DropDownList)this.radMenuIncoming.Items[1].FindControl("ddlStatusIncoming");
			if (ddlStatusIncoming != null)
			{
				ddlStatusIncoming.SelectedIndex = 1;

			   // this.grdIncomingTrans.MasterTableView.GetColumn("RejectColumn").Visible = ddlStatusIncoming.SelectedValue == "All" || ddlStatusIncoming.SelectedValue == "WaitingImport";
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
				this.grdIncomingTrans.Rebind();
			}else if (e.Argument.Contains("DeleteWorkflow_")){
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.PECC2TransmittalService.GetById(objId);
				if(transObj != null)
				{
					//delete assing user
					var listassign = this.objAssignedWfService.GetAllByObj(transObj.ID);
					foreach(var item in listassign)
					{
						this.objAssignedWfService.Delete(item);
					}
					var listUserAssign = this.objAssignedUserService.GetAllListObjID(transObj.ID);
					foreach (var item in listUserAssign)
					{
						this.objAssignedUserService.Delete(item);
					}
					transObj.IsAttachWorkflow = false;
					transObj.IsInWFProcess = false;
					transObj.IsWFComplete = false;
					this.PECC2TransmittalService.Update(transObj);
				}
				this.grdIncomingTrans.Rebind();
			}
			else if (e.Argument == "RebinOutrans")
			{
				this.grdOutgoingTrans.Rebind();
			}
			else if (e.Argument.Contains("DeleteTrans_"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.PECC2TransmittalService.GetById(objId);
				if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
				{
					var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
					if (Directory.Exists(folderPath))
					{
						Directory.Delete(folderPath, true);
					}
				}

				this.PECC2TransmittalService.Delete(objId);
				this.grdOutgoingTrans.Rebind();
			}
			else if (e.Argument.Contains("OpenFolder_"))
			{
				//var objId = new Guid(e.Argument.Split('_')[1]);
				//var transObj = this.PECC2TransmittalService.GetById(objId);
				//if (transObj != null)
				//{
				//    var contractorfolder = this.contractorTransmittalService.GetById(transObj.ContractorTransId.GetValueOrDefault());
				//    if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
				//    {
				//        var store = transObj.StoreFolderPath.Replace("/DocumentLibrary/", string.Empty);

				//        // string jsFunc = "SendvalueToCopy(" + store + ");";
				//        this.StorePath.Value = store;
				//        this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "SendvalueToCopy();", true);
				//        //    var servername= ConfigurationManager.AppSettings.Get("ServerName");

				//        //var pathcopy = servername + pathfodler;
				//        //System.Diagnostics.Process.Start("explorer.exe", pathcopy);
				//    }

				//}
			}
			else if (e.Argument.Contains("ExportContractorETRM"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.contractorTransmittalService.GetById(objId);
				if (transObj != null)
				{
					this.ExportContractorETRM(transObj);
				}
			}
			else if (e.Argument.Contains("ExportPECC2ETRM"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.PECC2TransmittalService.GetById(objId);
				if (transObj != null)
				{
					this.ExportETRM(transObj, e.Argument.Split('_')[2]);
				}
			}
			else if(e.Argument.Contains("ExportPECC2ApproveLetter"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.PECC2TransmittalService.GetById(objId);
				if (transObj != null)
				{
					this.ExportApproveLetter(transObj, e.Argument.Split('_')[2]);
				}
			}

			else if (e.Argument.Contains("ImportDocument"))
			{
				var contractorTransId = new Guid(e.Argument.Split('_')[1]);
				var pecc2IncomingTransId = new Guid(e.Argument.Split('_')[2]);
				var contractorTransObj = this.contractorTransmittalService.GetById(contractorTransId);
				var PECC2TransIn = this.PECC2TransmittalService.GetById(pecc2IncomingTransId);
				if (contractorTransObj != null)
				{
					this.ImportTransDocument(contractorTransObj, pecc2IncomingTransId);
					//if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
					//{ NotifiImportTransmittal(contractorTransObj); }
					var txtSearchOutgoing = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearchOutgoing");
					txtSearchOutgoing.Text = PECC2TransIn.TransmittalNo;
					var ddlStatusIncoming = (DropDownList)this.radMenuIncoming.Items[1].FindControl("ddlStatusIncoming");
					if (ddlStatusIncoming != null)
					{
						ddlStatusIncoming.SelectedIndex = 3;
					}
					this.grdIncomingTrans.Rebind();
				}
			}
			else if (e.Argument.Contains("SendTrans"))
			{
				try
				{
					var objId = new Guid(e.Argument.Split('_')[1]);
					var forSendId = Convert.ToInt32(e.Argument.Split('_')[2]);
					var transObj = this.PECC2TransmittalService.GetById(objId);
					if (transObj != null)
					{
						var contractorTrans = new ContractorTransmittal();
						contractorTrans.ID = Guid.NewGuid();
						contractorTrans.PECC2TransId = transObj.ID;
						contractorTrans.Status = string.Empty;
						contractorTrans.IsValid = false;
						contractorTrans.Status = "Waiting processing";
						contractorTrans.ErrorMessage = "Waiting for DC processing.";
						contractorTrans.IsSend = false;
						contractorTrans.IsOpen = false;
						contractorTrans.ForSentId = forSendId;
						contractorTrans.ForSentName = forSendId == 1 ? "Project's Document" : "Change Request";
						contractorTrans.Priority = transObj.Priority;
						contractorTrans.ProjectId = transObj.ProjectCodeId;
						contractorTrans.ProjectName = transObj.ProjectCodeName;
						contractorTrans.TransNo = transObj.TransmittalNo;
						contractorTrans.TransDate = transObj.IssuedDate;
						contractorTrans.Description = transObj.Description;
						contractorTrans.OriginatingOrganizationId = transObj.OriginatingOrganizationId;
						contractorTrans.OriginatingOrganizationName = transObj.OriginatingOrganizationName;
						contractorTrans.ReceivingOrganizationId = transObj.ReceivingOrganizationId;
						contractorTrans.ReceivingOrganizationName = transObj.ReceivingOrganizationName;
						contractorTrans.FromValue = transObj.FromValue;
						contractorTrans.ToValue = transObj.ToValue;
						contractorTrans.CCValue = transObj.CCValue;
						contractorTrans.GroupId = transObj.GroupId;
						contractorTrans.GroupCode = transObj.GroupCode;
						contractorTrans.TransmittedByName = transObj.TransmittedByName;
						contractorTrans.TransmittedByDesignation = transObj.TransmittedByDesignation;
						contractorTrans.AcknowledgedByName = transObj.AcknowledgedByName;
						contractorTrans.AcknowledgedByDesignation = transObj.AcknowledgedByDesignation;
						contractorTrans.Remark = transObj.Remark;
                        contractorTrans.CategoryId = transObj.CategoryId;
                        contractorTrans.CategoryName = transObj.CategoryName;
                        contractorTrans.VolumeNumber = transObj.VolumeNumber;
						contractorTrans.Year = transObj.Year;
						contractorTrans.DueDate = transObj.DueDate;
						contractorTrans.ReceivedDate = DateTime.Now;
						contractorTrans.TypeId = 1;
						contractorTrans.StoreFolderPath = transObj.StoreFolderPath;

						var contractorTransId = this.contractorTransmittalService.Insert(contractorTrans);
						if (contractorTransId != null)
						{
							transObj.ContractorTransId = contractorTransId;
							transObj.IsSend = true;
							transObj.ReceivedDate = DateTime.Now;

							this.PECC2TransmittalService.Update(transObj);
							//update transout on documentproject
							this.UpdateTransOutToDocument(transObj, forSendId);

							//

							//send email to dc contractors
							if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
							{
								this.NotifiNewTransmittal(transObj);
							}
							////send email to all user 
							//if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
							//{
							//    this.NotifiNewTransmittalOut(transObj);
							//}
						}

						this.grdOutgoingTrans.Rebind();
					}
			}
				catch { }
		}
			else if (e.Argument.Contains("Undo"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.PECC2TransmittalService.GetById(objId);
				if (transObj != null)
				{
					var contractorTrans = this.contractorTransmittalService.GetById(transObj.ContractorTransId.GetValueOrDefault());
					if (contractorTrans != null)
					{
						this.contractorTransmittalService.Delete(contractorTrans);
						transObj.ContractorTransId = null;
						transObj.IsSend = false;
						transObj.ReceivedDate = null;
						this.PECC2TransmittalService.Update(transObj);
						this.grdOutgoingTrans.Rebind();
					}

				}
			}
		}

		private void CreateCRSFile(PECC2Transmittal pecc2IncomingTrans)
		{
			// Create CRS file
			var docOfTrans = this.attachDocToTransmittalService.GetAllByTransId(pecc2IncomingTrans.ID)
				.Select(t => this.pecc2DocumentsService.GetById(t.DocumentId.GetValueOrDefault()))
				.Where(t => t != null)
				.OrderBy(t => t.DocNo)
				.ToList();
			var filePath = Server.MapPath("../../Exports") + @"\";
			var workbook = new Workbook();
			if (pecc2IncomingTrans.IsFirstTrans.GetValueOrDefault())
			{
				workbook = new Workbook(filePath + @"Template\PECC2_CRSTemplate_New.xlsm");
			}
			else
			{
				var responseCRSAttachFile = this.contractorTransmittalAttachFileService.GetByTrans(pecc2IncomingTrans.ContractorTransId.GetValueOrDefault()).FirstOrDefault(t => t.TypeId == 2);
				if (responseCRSAttachFile != null)
				{
					workbook = new Workbook(Server.MapPath("../.." + responseCRSAttachFile.FilePath));
				}
			}

			var dataControlSheet = workbook.Worksheets[0];
			var currentSheetIndex = Convert.ToInt32(dataControlSheet.Cells["A1"].Value);
			var transSheet = workbook.Worksheets[currentSheetIndex];

			if (currentSheetIndex != 1)
			{
				transSheet.IsVisible = true;
			}

			// Update new current sheet
			dataControlSheet.Cells["A1"].Value = currentSheetIndex + 1;
			// -------------------------------------------------------------------------------------------------------

			if (pecc2IncomingTrans.IsFirstTrans.GetValueOrDefault())
			{
				transSheet.Cells["E2"].PutValue(pecc2IncomingTrans.TransmittalNo);
				transSheet.Cells["J2"].PutValue(pecc2IncomingTrans.IssuedDate);
				transSheet.Cells["E3"].PutValue(pecc2IncomingTrans.Description);
			}
			else
			{
				transSheet.Cells[1, 4 + 7 * (currentSheetIndex - 1)].PutValue(pecc2IncomingTrans.TransmittalNo);
				transSheet.Cells[1, 9 + 7 * (currentSheetIndex - 1)].PutValue(pecc2IncomingTrans.IssuedDate);
				transSheet.Cells[2, 4 + 7 * (currentSheetIndex - 1)].PutValue(pecc2IncomingTrans.Description);
			}

			var dtDocListFull = new DataTable();
			dtDocListFull.Columns.AddRange(new[]
			{
					new DataColumn("DocNo", typeof(String)),
					new DataColumn("Empty1", typeof(String)),
					new DataColumn("Rev1", typeof(String)),
					new DataColumn("ReviewStatus1", typeof(String)),
					new DataColumn("Empty2", typeof(String)),
					new DataColumn("Title1", typeof(String)),
					new DataColumn("Empty3", typeof(String)),
					new DataColumn("Empty4", typeof(String)),
					new DataColumn("Empty35", typeof(String)),


					new DataColumn("Rev2", typeof(String)),
					new DataColumn("ReviewStatus2", typeof(String)),
					new DataColumn("Empty5", typeof(String)),
					new DataColumn("Title2", typeof(String)),
					new DataColumn("Empty6", typeof(String)),
					new DataColumn("Empty7", typeof(String)),
					new DataColumn("Empty36", typeof(String)),

					new DataColumn("Rev3", typeof(String)),
					new DataColumn("ReviewStatus3", typeof(String)),
					new DataColumn("Empty8", typeof(String)),
					new DataColumn("Title3", typeof(String)),
					new DataColumn("Empty9", typeof(String)),
					new DataColumn("Empty10", typeof(String)),
					new DataColumn("Empty37", typeof(String)),

					new DataColumn("Rev4", typeof(String)),
					new DataColumn("ReviewStatus4", typeof(String)),
					new DataColumn("Empty11", typeof(String)),
					new DataColumn("Title4", typeof(String)),
					new DataColumn("Empty13", typeof(String)),
					new DataColumn("Empty14", typeof(String)),
					new DataColumn("Empty38", typeof(String)),

					new DataColumn("Rev5", typeof(String)),
					new DataColumn("ReviewStatus5", typeof(String)),
					new DataColumn("Empty15", typeof(String)),
					new DataColumn("Title5", typeof(String)),
					new DataColumn("Empty17", typeof(String)),
					new DataColumn("Empty18", typeof(String)),
					new DataColumn("Empty39", typeof(String)),

					new DataColumn("Rev6", typeof(String)),
					new DataColumn("ReviewStatus6", typeof(String)),
					new DataColumn("Empty19", typeof(String)),
					new DataColumn("Title6", typeof(String)),
					new DataColumn("Empty21", typeof(String)),
					new DataColumn("Empty22", typeof(String)),
					new DataColumn("Empty40", typeof(String)),

					new DataColumn("Rev7", typeof(String)),
					new DataColumn("ReviewStatus7", typeof(String)),
					new DataColumn("Empty23", typeof(String)),
					new DataColumn("Title7", typeof(String)),
					new DataColumn("Empty24", typeof(String)),
					new DataColumn("Empty25", typeof(String)),
					new DataColumn("Empty41", typeof(String)),

					new DataColumn("Rev8", typeof(String)),
					new DataColumn("ReviewStatus8", typeof(String)),
					new DataColumn("Empty26", typeof(String)),
					new DataColumn("Title8", typeof(String)),
					new DataColumn("Empty27", typeof(String)),
					new DataColumn("Empty28", typeof(String)),
					new DataColumn("Empty42", typeof(String)),

					new DataColumn("Rev9", typeof(String)),
					new DataColumn("ReviewStatus9", typeof(String)),
					new DataColumn("Empty29", typeof(String)),
					new DataColumn("Title9", typeof(String)),
					new DataColumn("Empty30", typeof(String)),
					new DataColumn("Empty31", typeof(String)),
					new DataColumn("Empty43", typeof(String)),

					new DataColumn("Rev10", typeof(String)),
					new DataColumn("ReviewStatus10", typeof(String)),
					new DataColumn("Empty32", typeof(String)),
					new DataColumn("Title10", typeof(String)),
					new DataColumn("Empty33", typeof(String)),
					new DataColumn("Empty34", typeof(String)),
					new DataColumn("Empty44", typeof(String)),

				});


			var dtCommentList = new DataTable();
			dtCommentList.Columns.AddRange(new[]
			{
					new DataColumn("Index", typeof(String)),
					new DataColumn("DocNo", typeof(String)),

					new DataColumn("Comment1", typeof(String)),
					new DataColumn("Empty1", typeof(String)),
					new DataColumn("Empty2", typeof(String)),
					new DataColumn("Empty3", typeof(String)),
					new DataColumn("Response1", typeof(String)),
					new DataColumn("Empty4", typeof(String)),
					new DataColumn("Empty5", typeof(String)),

					new DataColumn("Comment2", typeof(String)),
					new DataColumn("Empty6", typeof(String)),
					new DataColumn("Empty7", typeof(String)),
					new DataColumn("Empty8", typeof(String)),
					new DataColumn("Response2", typeof(String)),
					new DataColumn("Empty9", typeof(String)),
					new DataColumn("Empty10", typeof(String)),

					new DataColumn("Comment3", typeof(String)),
					new DataColumn("Empty11", typeof(String)),
					new DataColumn("Empty12", typeof(String)),
					new DataColumn("Empty13", typeof(String)),
					new DataColumn("Response3", typeof(String)),
					new DataColumn("Empty14", typeof(String)),
					new DataColumn("Empty15", typeof(String)),

					new DataColumn("Comment4", typeof(String)),
					new DataColumn("Empty16", typeof(String)),
					new DataColumn("Empty17", typeof(String)),
					new DataColumn("Empty18", typeof(String)),
					new DataColumn("Response4", typeof(String)),
					new DataColumn("Empty19", typeof(String)),
					new DataColumn("Empty20", typeof(String)),

					new DataColumn("Comment5", typeof(String)),
					new DataColumn("Empty21", typeof(String)),
					new DataColumn("Empty22", typeof(String)),
					new DataColumn("Empty23", typeof(String)),
					new DataColumn("Response5", typeof(String)),
					new DataColumn("Empty24", typeof(String)),
					new DataColumn("Empty25", typeof(String)),

					new DataColumn("Comment6", typeof(String)),
					new DataColumn("Empty26", typeof(String)),
					new DataColumn("Empty27", typeof(String)),
					new DataColumn("Empty28", typeof(String)),
					new DataColumn("Response6", typeof(String)),
					new DataColumn("Empty29", typeof(String)),
					new DataColumn("Empty30", typeof(String)),

					new DataColumn("Comment7", typeof(String)),
					new DataColumn("Empty31", typeof(String)),
					new DataColumn("Empty32", typeof(String)),
					new DataColumn("Empty33", typeof(String)),
					new DataColumn("Response7", typeof(String)),
					new DataColumn("Empty34", typeof(String)),
					new DataColumn("Empty35", typeof(String)),

					new DataColumn("Comment8", typeof(String)),
					new DataColumn("Empty36", typeof(String)),
					new DataColumn("Empty37", typeof(String)),
					new DataColumn("Empty38", typeof(String)),
					new DataColumn("Response8", typeof(String)),
					new DataColumn("Empty39", typeof(String)),
					new DataColumn("Empty40", typeof(String)),

					new DataColumn("Comment9", typeof(String)),
					new DataColumn("Empty41", typeof(String)),
					new DataColumn("Empty42", typeof(String)),
					new DataColumn("Empty43", typeof(String)),
					new DataColumn("Response9", typeof(String)),
					new DataColumn("Empty44", typeof(String)),
					new DataColumn("Empty45", typeof(String)),

					new DataColumn("Comment10", typeof(String)),
					new DataColumn("Empty46", typeof(String)),
					new DataColumn("Empty47", typeof(String)),
					new DataColumn("Empty48", typeof(String)),
					new DataColumn("Response10", typeof(String)),
					new DataColumn("Empty49", typeof(String)),
					new DataColumn("Empty50", typeof(String)),
				});

			var dtGeneralCommentList = new DataTable();
			dtGeneralCommentList.Columns.AddRange(new[]
			{
					new DataColumn("Index", typeof(String)),
					new DataColumn("DocNo", typeof(String)),

					new DataColumn("Comment1", typeof(String)),
					new DataColumn("Empty1", typeof(String)),
					new DataColumn("Empty2", typeof(String)),
					new DataColumn("Empty3", typeof(String)),
					new DataColumn("Response1", typeof(String)),
					new DataColumn("Empty4", typeof(String)),
					new DataColumn("Empty5", typeof(String)),

					new DataColumn("Comment2", typeof(String)),
					new DataColumn("Empty6", typeof(String)),
					new DataColumn("Empty7", typeof(String)),
					new DataColumn("Empty8", typeof(String)),
					new DataColumn("Response2", typeof(String)),
					new DataColumn("Empty9", typeof(String)),
					new DataColumn("Empty10", typeof(String)),

					new DataColumn("Comment3", typeof(String)),
					new DataColumn("Empty11", typeof(String)),
					new DataColumn("Empty12", typeof(String)),
					new DataColumn("Empty13", typeof(String)),
					new DataColumn("Response3", typeof(String)),
					new DataColumn("Empty14", typeof(String)),
					new DataColumn("Empty15", typeof(String)),

					new DataColumn("Comment4", typeof(String)),
					new DataColumn("Empty16", typeof(String)),
					new DataColumn("Empty17", typeof(String)),
					new DataColumn("Empty18", typeof(String)),
					new DataColumn("Response4", typeof(String)),
					new DataColumn("Empty19", typeof(String)),
					new DataColumn("Empty20", typeof(String)),

					new DataColumn("Comment5", typeof(String)),
					new DataColumn("Empty21", typeof(String)),
					new DataColumn("Empty22", typeof(String)),
					new DataColumn("Empty23", typeof(String)),
					new DataColumn("Response5", typeof(String)),
					new DataColumn("Empty24", typeof(String)),
					new DataColumn("Empty25", typeof(String)),

					new DataColumn("Comment6", typeof(String)),
					new DataColumn("Empty26", typeof(String)),
					new DataColumn("Empty27", typeof(String)),
					new DataColumn("Empty28", typeof(String)),
					new DataColumn("Response6", typeof(String)),
					new DataColumn("Empty29", typeof(String)),
					new DataColumn("Empty30", typeof(String)),

					new DataColumn("Comment7", typeof(String)),
					new DataColumn("Empty31", typeof(String)),
					new DataColumn("Empty32", typeof(String)),
					new DataColumn("Empty33", typeof(String)),
					new DataColumn("Response7", typeof(String)),
					new DataColumn("Empty34", typeof(String)),
					new DataColumn("Empty35", typeof(String)),

					new DataColumn("Comment8", typeof(String)),
					new DataColumn("Empty36", typeof(String)),
					new DataColumn("Empty37", typeof(String)),
					new DataColumn("Empty38", typeof(String)),
					new DataColumn("Response8", typeof(String)),
					new DataColumn("Empty39", typeof(String)),
					new DataColumn("Empty40", typeof(String)),

					new DataColumn("Comment9", typeof(String)),
					new DataColumn("Empty41", typeof(String)),
					new DataColumn("Empty42", typeof(String)),
					new DataColumn("Empty43", typeof(String)),
					new DataColumn("Response9", typeof(String)),
					new DataColumn("Empty44", typeof(String)),
					new DataColumn("Empty45", typeof(String)),

					new DataColumn("Comment10", typeof(String)),
					new DataColumn("Empty46", typeof(String)),
					new DataColumn("Empty47", typeof(String)),
					new DataColumn("Empty48", typeof(String)),
					new DataColumn("Response10", typeof(String)),
					new DataColumn("Empty49", typeof(String)),
					new DataColumn("Empty50", typeof(String)),
				});

			var prevDocCount = 0;
			var prevCommentCount = 0;
			var prevGeneralCommentCount = 0;
			if (pecc2IncomingTrans.IsFirstTrans.GetValueOrDefault())
			{
				var countComment = 1;
				// Get info of change request doc first
				if (pecc2IncomingTrans.ForSentId == 2)
				{
					var changeRequestObj = this.changeRequestService.GetAllByIncomingTrans(pecc2IncomingTrans.ID).FirstOrDefault();
					if (changeRequestObj != null)
					{
						var docRow = dtDocListFull.NewRow();
						docRow["DocNo"] = changeRequestObj.Number;
						docRow["Rev" + currentSheetIndex] = changeRequestObj.Revision;
						docRow["ReviewStatus" + currentSheetIndex] = changeRequestObj.ReviewResultName;
						docRow["Title" + currentSheetIndex] = changeRequestObj.Title;
						dtDocListFull.Rows.Add(docRow);
					}
				}
				// ------------------------------------------------------------------------------------------------------------

				foreach (var docObj in docOfTrans)
				{
					var docRow = dtDocListFull.NewRow();
					docRow["DocNo"] = docObj.DocNo;
					docRow["Rev" + currentSheetIndex] = docObj.Revision;
					docRow["ReviewStatus" + currentSheetIndex] = docObj.DocReviewStatusCode;
					docRow["Title" + currentSheetIndex] = docObj.DocTitle;
					dtDocListFull.Rows.Add(docRow);
				}
			}
			else
			{
				prevDocCount = Convert.ToInt32(dataControlSheet.Cells["A2"].Value);
				try {
                  prevCommentCount = dataControlSheet.Cells["A3"].Value.ToString() == "#VALUE!" ? 0 : Convert.ToInt32(dataControlSheet.Cells["A3"].Value);
                }
                catch { }
                if (prevCommentCount < 0) { prevCommentCount = 0; }
				prevGeneralCommentCount = Convert.ToInt32(dataControlSheet.Cells["A4"].Value);
				var prevTransSheet = workbook.Worksheets[currentSheetIndex - 1];
				var prevDtDocList = prevTransSheet.Cells.ExportDataTable(5, 2, prevDocCount, 2 + (7 * (currentSheetIndex - 1))).AsEnumerable().ToList();

				var prevDtGeneralCommentList = prevTransSheet.Cells.ExportDataTable(9 + prevDocCount, 2, prevGeneralCommentCount, 2 + (7 * (currentSheetIndex - 1))).AsEnumerable().ToList();
                
                if (prevCommentCount > 0) { 
				 var prevDtCommentList = prevTransSheet.Cells.ExportDataTable(9 + prevDocCount + 3 + prevGeneralCommentCount, 2, prevCommentCount, 2 + (7 * (currentSheetIndex - 1))).AsEnumerable().ToList();
                    foreach (DataRow commentItem in prevDtCommentList)
                    {
                        var commentRow = dtCommentList.NewRow();
                        commentRow["Index"] = commentItem[0].ToString();
                        commentRow["DocNo"] = commentItem[1].ToString();
                        for (int i = 0; i < currentSheetIndex - 1; i++)
                        {
                            commentRow["Comment" + (i + 1)] = commentItem[2 + (7 * i)].ToString();
                            commentRow["Response" + (i + 1)] = commentItem[6 + (7 * i)].ToString();
                        }
                        dtCommentList.Rows.Add(commentRow);
                    }
                }
				foreach (DataRow docItem in prevDtDocList)
				{
					var docRow = dtDocListFull.NewRow();
					docRow["DocNo"] = docItem[0];
					for (int i = 0; i < currentSheetIndex - 1; i++)
					{
						docRow["Rev" + (i + 1)] = docItem[2 + (7 * i)];
						docRow["ReviewStatus" + (i + 1)] = docItem[3 + (7 * i)];
						docRow["Title" + (i + 1)] = docItem[5 + (7 * i)];
					}
					dtDocListFull.Rows.Add(docRow);
				}

				foreach (DataRow commentItem in prevDtGeneralCommentList)
				{
					var generalCommentRow = dtGeneralCommentList.NewRow();
					generalCommentRow["Index"] = commentItem[0].ToString(); 
					generalCommentRow["DocNo"] = commentItem[1].ToString(); 
					for (int i = 0; i < currentSheetIndex - 1; i++)
					{
						generalCommentRow["Comment" + (i + 1)] = commentItem[2 + (7 * i)].ToString();
						generalCommentRow["Response" + (i + 1)] = commentItem[6 + (7 * i)].ToString();
					}
					dtGeneralCommentList.Rows.Add(generalCommentRow);
				}

				
                if (dtCommentList.Rows.Count == 0)
                {
                    var commentRow = dtCommentList.NewRow();
                    commentRow["Index"] = 1;
                    dtCommentList.Rows.Add(commentRow);
                }
                // Fill new rev info for exist doc of trans
                if (pecc2IncomingTrans.ForSentId == 2)
				{
					var changeRequestObj = this.changeRequestService.GetAllByIncomingTrans(pecc2IncomingTrans.ID).FirstOrDefault();
					if (changeRequestObj != null)
					{
						var existRow = dtDocListFull.AsEnumerable().FirstOrDefault(t => t[0].ToString() == changeRequestObj.Number);
						if (existRow != null)
						{
							existRow["Rev" + currentSheetIndex] = changeRequestObj.Revision;
							existRow["ReviewStatus" + currentSheetIndex] = changeRequestObj.ReviewResultName;
							existRow["Title" + currentSheetIndex] = changeRequestObj.Title;
						}
					}
				}

				foreach (var docObj in docOfTrans)
				{
					var existRow = dtDocListFull.AsEnumerable().FirstOrDefault(t => t[0].ToString() == docObj.DocNo);
					if (existRow != null)
					{
						existRow["Rev" + currentSheetIndex] = docObj.Revision;
						existRow["ReviewStatus" + currentSheetIndex] = docObj.DocReviewStatusCode;
						existRow["Title" + currentSheetIndex] = docObj.DocTitle;
					}
				}
				// --------------------------------------------------------------------------

				// Filter new doc of trans
				var newDoc = docOfTrans.Where(t => !prevDtDocList.Select(x => x[0]).Contains(t.DocNo)).ToList();
				foreach (var docObj in newDoc)
				{
					var docRow = dtDocListFull.NewRow();
					docRow["DocNo"] = docObj.DocNo;
					docRow["Rev" + currentSheetIndex] = docObj.Revision;
					docRow["ReviewStatus" + currentSheetIndex] = docObj.DocReviewStatusCode;
					docRow["Title" + currentSheetIndex] = docObj.DocTitle;
					dtDocListFull.Rows.Add(docRow);
				}
				// --------------------------------------------------------------------------
			}

			if (dtCommentList.Rows.Count > 0)
			{
				dtCommentList = dtCommentList.AsEnumerable().OrderBy(t => t["DocNo"]).CopyToDataTable();
			}

			if (dtGeneralCommentList.Rows.Count > 0)
			{
				dtGeneralCommentList = dtGeneralCommentList.AsEnumerable().OrderBy(t => t["DocNo"]).CopyToDataTable();
			}

			// Insert row when General Comment more than 5 row
			if (prevGeneralCommentCount > 5)
			{
				transSheet.Cells.InsertRows(9 + dtDocListFull.Rows.Count, prevGeneralCommentCount - 5);
			}

			for (int i = 1; i <= prevGeneralCommentCount - 5; i++)
			{
				for (int j = 0; j < currentSheetIndex; j++)
				{
					transSheet.Cells.Merge(10 + i, 4 + (7 * j), 1, 4);
					transSheet.Cells.Merge(10 + i, 8 + (7 * j), 1, 2);
				}
			}
			// ------------------------------------------------------------------------

			transSheet.Cells.ImportDataTable(dtDocListFull, false, 5, 2, dtDocListFull.Rows.Count, dtDocListFull.Columns.Count, true);

			transSheet.Cells.ImportDataTable(dtGeneralCommentList, false, 9 + dtDocListFull.Rows.Count, 2, dtGeneralCommentList.Rows.Count, dtGeneralCommentList.Columns.Count, false);

			transSheet.Cells.ImportDataTable(dtCommentList, false, 9 + dtDocListFull.Rows.Count + 3 + dtGeneralCommentList.Rows.Count, 2, dtCommentList.Rows.Count, dtCommentList.Columns.Count, false);

            //transSheet.Cells.DeleteRow(9 + dtDocListFull.Rows.Count + dtCommentList.Rows.Count);
            transSheet.Cells.DeleteRow(5 + dtDocListFull.Rows.Count);
            // Merge Cell
            for (int i = 1; i <= dtDocListFull.Rows.Count; i++)
			{
				transSheet.Cells.Merge(5 + i, 2, 1, 2);
				for (int j = 0; j < currentSheetIndex; j++)
				{
					transSheet.Cells.Merge(5 + i, 7 + (7 * j), 1, 3);
				}
			}

			for (int i = 1; i <= dtCommentList.Rows.Count; i++)
			{
				for (int j = 0; j < currentSheetIndex; j++)
				{
					transSheet.Cells.Merge(9 + i + dtDocListFull.Rows.Count + 3 + dtGeneralCommentList.Rows.Count, 4 + (7 * j), 1, 4);
					transSheet.Cells.Merge(9 + i + dtDocListFull.Rows.Count + 3 + dtGeneralCommentList.Rows.Count, 8 + (7 * j), 1, 2);
				}
			}
			// ------------------------------------------------------------------------

			//dataControlSheet.Cells["A2"].PutValue(dtDocListFull.Rows.Count);

			var options = new AutoFitterOptions();
			options.AutoFitMergedCells = true;
			transSheet.AutoFitRows(options);

			transSheet.Cells.SetRowHeight(1, 23.25);
			transSheet.Cells.SetRowHeight(2, 23.25);
			transSheet.Cells.SetRowHeight(3, 23.25);
			transSheet.Cells.SetRowHeight(4, 23.25);

			transSheet.Cells.SetRowHeight(6 + dtDocListFull.Rows.Count, 23.25);
			transSheet.Cells.SetRowHeight(7 + dtDocListFull.Rows.Count, 23.25);
			transSheet.Cells.SetRowHeight(8 + dtDocListFull.Rows.Count, 23.25);

			transSheet.Cells.SetRowHeight(6 + dtDocListFull.Rows.Count + prevGeneralCommentCount + 3, 23.25);
			transSheet.Cells.SetRowHeight(7 + dtDocListFull.Rows.Count + prevGeneralCommentCount + 3, 23.25);
			transSheet.Cells.SetRowHeight(8 + dtDocListFull.Rows.Count + prevGeneralCommentCount + 3, 23.25);

			if (!pecc2IncomingTrans.IsFirstTrans.GetValueOrDefault())
			{
				var prevTransSheet = workbook.Worksheets[currentSheetIndex - 1];
				for (int i = 0; i < prevCommentCount; i++)
				{
					transSheet.Cells.SetRowHeight(5 + dtDocListFull.Rows.Count + 4 + i, prevTransSheet.Cells.GetRowHeight(5 + prevDocCount + 4 + i));
				}
			}

			var filename = Utility.RemoveSpecialCharacterFileName(pecc2IncomingTrans.TransmittalNo) + "_CRS.xlsm";
			var saveFilePath = Server.MapPath("../.." + pecc2IncomingTrans.StoreFolderPath + "/eTRM File/" + filename);
			workbook.Save(saveFilePath);

			var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
							   + pecc2IncomingTrans.StoreFolderPath + "/eTRM File";
			// Path file to download from server
			var serverFilePath = serverFolder + "/" + filename;
			// Attach CRS to document Obj
			var fileInfo = new FileInfo(saveFilePath);
			if (fileInfo.Exists && this.pecc2TransmittalAttachFileService.GetByTrans(pecc2IncomingTrans.ID).All(t => t.TypeId != 2))
			{

				var attachFile = new PECC2TransmittalAttachFiles()
				{
					ID = Guid.NewGuid(),
					TransId = pecc2IncomingTrans.ID,
					Filename = filename,
					Extension = "xlsm",
					FilePath = serverFilePath,
					ExtensionIcon = "~/images/excelfile.png",
					FileSize = (double)fileInfo.Length / 1024,
					CreatedBy = UserSession.Current.User.Id,
					CreatedByName = UserSession.Current.User.UserNameWithFullName,
					CreatedDate = DateTime.Now,
					TypeId = 2,
					TypeName = "CRS File",
					CheckoutByName = string.Empty,
					IsCheckOut = false,
				};

				this.pecc2TransmittalAttachFileService.Insert(attachFile);
			}
			// --------------------------------------------------------------------------------------------
		}




		private void ImportTransDocument(ContractorTransmittal contractorTransObj, Guid objPECC2Id)
		{
			var pecc2IncomingTrans = this.PECC2TransmittalService.GetById(contractorTransObj.PECC2TransId.GetValueOrDefault());
			var transPurpose = this.purposeCodeService.GetById(contractorTransObj.PurposeId.GetValueOrDefault());
			var revisionStatus = this.revisionStatusService.GetByCode(transPurpose.Code);

			var fullDocList = this.contractorTransmittalDocFileService.GetAllByTrans(contractorTransObj.ID);
			var filterDocList = new List<ContractorTransmittalDocFile>();

			// Remove duplicate Document
			foreach (var document in fullDocList)
			{
				if (filterDocList.All(t => t.DocumentNo != document.DocumentNo))
				{
					filterDocList.Add(document);
				}
			}
			// --------------------------------------------------------------------------------------------

			

			// Process import
			if (pecc2IncomingTrans.ForSentId == 1)
			{
				foreach (var contractorDoc in filterDocList)
				{
					this.ProcessImportProjectDocument(fullDocList, contractorDoc, pecc2IncomingTrans, revisionStatus, objPECC2Id);
				}
			}
			else
			{
				var designChangeDoc = fullDocList.FirstOrDefault(t => t.TypeId == 2);
				if (designChangeDoc != null)
				{
					// Collect info of change request
					var changeRequestObj = new ChangeRequest()
					{
						ID = Guid.NewGuid(),
						Number = designChangeDoc.DocumentNo,
						Title = designChangeDoc.DocumentTitle,
						Description = designChangeDoc.Description,
						AreaId = designChangeDoc.AreaId,
						AreaCode = designChangeDoc.AreaName,
						UnitId = designChangeDoc.UnitCodeId,
						UnitCode = designChangeDoc.UnitCodeName,
						TypeId = designChangeDoc.ChangeRequestTypeId,
						TypeName = designChangeDoc.ChangeRequestTypeName,

						GroupId = designChangeDoc.GroupCodeId,
						GroupName = designChangeDoc.GroupCodeName,
						Year = Convert.ToInt32(designChangeDoc.Year),
						SequentialNumber = designChangeDoc.Sequence,
						ReasonForChange = designChangeDoc.ReasonForChange,
						ExistingCondition = designChangeDoc.ExistingCondition,
						ProjectId = designChangeDoc.ProjectId,
						ProjectCode = designChangeDoc.ProjectName,
						IssuedDate = designChangeDoc.IssuedDate,
						ChangeGradeCodeId = designChangeDoc.ChangeGradeCodeId,
						ChangeGradeCodeName = designChangeDoc.ChangeGradeCode,
						Status = designChangeDoc.Status,
						RefDocId = designChangeDoc.RefDocId,
						RefDocNo = designChangeDoc.RefDocNo,
						DocToBeRevisedNo = string.Empty,
						DocToBeRevisedId = String.Empty,
						IsLeaf = true,
						Revision = designChangeDoc.Revision,
						IsDelete = false,
						ErrorMessage = string.Empty,
						IsValid = true,
						IncomingTransId = pecc2IncomingTrans.ID,
						IncomingTransNo = pecc2IncomingTrans.TransmittalNo,
						ActionCodeId = pecc2IncomingTrans.PurposeId,
						ActionCodeName = pecc2IncomingTrans.PurposeName.Contains(",")? pecc2IncomingTrans.PurposeName.Split(',')[0]: pecc2IncomingTrans.PurposeName,
						CreatedBy = UserSession.Current.User.Id,
						CreatedByName = UserSession.Current.User.FullName,
						CreatedDate = DateTime.Now
					};
					
					var currentChangeRequestList = this.changeRequestService.GetByChangeRequestNo(designChangeDoc.DocumentNo);
					if (currentChangeRequestList.Count > 0)
					{
						var changeRequestLeafObj = currentChangeRequestList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
						if (changeRequestLeafObj != null)
						{
							changeRequestObj.ParentId = changeRequestLeafObj.ParentId ?? changeRequestLeafObj.ID;
							changeRequestLeafObj.IsLeaf = false;
							this.changeRequestService.Update(changeRequestLeafObj);
						}
					}
					// ---------------------------------------------------

					this.changeRequestService.Insert(changeRequestObj);
					
					// Update PECC2 Incoming trans info
					pecc2IncomingTrans.Status = string.Empty;
					pecc2IncomingTrans.ErrorMessage = string.Empty;
					pecc2IncomingTrans.IsImport = true;

					this.PECC2TransmittalService.Update(pecc2IncomingTrans);
					// -----------------------------------------------------------------------------------

					//Attach doc file to change request obj
					this.AttachDocFileToChangeRequest(fullDocList.Where(t => t.TypeId == 2).ToList(), changeRequestObj);
					// ------------------------------------------------------------------------------

					foreach (var contractorDoc in filterDocList.Where(t => t.TypeId == 1))
					{
						this.ProcessImportProjectDocument(fullDocList, contractorDoc, pecc2IncomingTrans, revisionStatus, objPECC2Id, changeRequestObj);
					}

					//update Referen Doc/ Revised doc
					var listRefernce = changeRequestObj.RefDocId != null ? changeRequestObj.RefDocId.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList() : new List<string>();
					foreach( var iddoc in listRefernce)
					{
						var docobj = this.pecc2DocumentsService.GetById(new Guid(iddoc));
						if(docobj != null)
						{
							docobj.ChangeRequestId = changeRequestObj.ID;
							docobj.ChangeRequestNo = changeRequestObj.Number;
							this.pecc2DocumentsService.Update(docobj);
						}
					}

					var listRevised = changeRequestObj.DocToBeRevisedId != null ? changeRequestObj.DocToBeRevisedId.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList() : new List<string>();
					foreach (var iddoc in listRevised)
					{
						var docobj = this.pecc2DocumentsService.GetById(new Guid(iddoc));
						if (docobj != null)
						{
							docobj.ChangeRequestIdFoRevised = changeRequestObj.ID;
							docobj.ChangeRequestNoFoRevised = changeRequestObj.Number;
							this.pecc2DocumentsService.Update(docobj);
						}
					}
				}
			}

			// Create CRS File
			this.CreateCRSFile(pecc2IncomingTrans);
			// --------------------------------------------------------------------------------------------
		}

		private void ProcessImportChangeRequest(List<ContractorTransmittalDocFile> fullDocList, ContractorTransmittalDocFile contractorDoc, PECC2Transmittal pecc2IncomingTrans, Guid objPECC2Id)
		{
			// Get attach doc list
			var contractorDocAttach = fullDocList.Where(t => t.DocumentNo == contractorDoc.DocumentNo).ToList();
			// ----------------------------------------------------------------------------------------

			var currentChangeRequest = this.changeRequestService.GetAllByChangeRequestNo(contractorDoc.DocumentNo);
			if (currentChangeRequest != null)
			{
				// Fill incoming trans info
				currentChangeRequest.IncomingTransId = pecc2IncomingTrans.ID;
				currentChangeRequest.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
				currentChangeRequest.IsHasAttachFile = true;
				this.changeRequestService.Update(currentChangeRequest);
				//-------------------------------------------------------------------------------

				if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
				{
					var attachDoc = new AttachDocToTransmittal()
					{
						TransmittalId = objPECC2Id,
						DocumentId = currentChangeRequest.ID
					};

					if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, currentChangeRequest.ID))
					{
						this.attachDocToTransmittalService.Insert(attachDoc);
					}
				}

				//Attach doc file to change request obj
				this.AttachDocFileToChangeRequest(contractorDocAttach, currentChangeRequest);
				// ------------------------------------------------------------------------------

				// Update PECC2 Incoming trans info
				pecc2IncomingTrans.Status = string.Empty;
				pecc2IncomingTrans.ErrorMessage = string.Empty;
				pecc2IncomingTrans.IsImport = true;

				this.PECC2TransmittalService.Update(pecc2IncomingTrans);
				// -------------------------------------------------------------------------------
				contractorDoc.PECC2ProjectDocId = currentChangeRequest.ID;
				contractorDoc.IsReject = false;
				this.contractorTransmittalDocFileService.Update(contractorDoc);
			}
			else
			{
				// Collect new project doc info
				var changeRequestObj = new ChangeRequest();
				this.CollectChangeRequestData(contractorDoc, changeRequestObj);

				changeRequestObj.ProjectId = contractorDoc.ProjectId;
				changeRequestObj.ProjectCode = contractorDoc.ProjectName;

				changeRequestObj.CreatedBy = UserSession.Current.User.Id;
				changeRequestObj.CreatedDate = DateTime.Now;
				changeRequestObj.IsHasAttachFile = true;
				changeRequestObj.IsDelete = false;

				// Fill incoming trans info
				changeRequestObj.IncomingTransId = pecc2IncomingTrans.ID;
				changeRequestObj.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
				//-------------------------------------------------------------------------------

				// get master info
				////this.CollectMasterInfo(masterDoc, projectDoc);
				// -----------------------------------------------------------------------------------------------------

				this.changeRequestService.Insert(changeRequestObj);
				if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
				{
					var attachDoc = new AttachDocToTransmittal()
					{
						TransmittalId = objPECC2Id,
						DocumentId = changeRequestObj.ID
					};

					if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, changeRequestObj.ID))
					{
						this.attachDocToTransmittalService.Insert(attachDoc);
					}
				}

				//update contractorTransDocFileObj
				//Attach doc file to project doc
				this.AttachDocFileToChangeRequest(contractorDocAttach, changeRequestObj);
				// --------------------------------------------------------------------------------------------------------------

				// Update PECC2 Incoming trans info
				pecc2IncomingTrans.Status = string.Empty;
				pecc2IncomingTrans.ErrorMessage = string.Empty;
				pecc2IncomingTrans.IsImport = true;

				this.PECC2TransmittalService.Update(pecc2IncomingTrans);
				// --------------------------------------------------------------------------------------------------------------

				contractorDoc.PECC2ProjectDocId = changeRequestObj.ID;
				contractorDoc.IsReject = false;
				this.contractorTransmittalDocFileService.Update(contractorDoc);
			}
		}

		private void ProcessImportProjectDocument(List<ContractorTransmittalDocFile> fullDocList, ContractorTransmittalDocFile contractorDoc, PECC2Transmittal pecc2IncomingTrans, RevisionStatu revisionStatus, Guid objPECC2Id)
		{
			// Get attach doc list
			var contractorDocAttach = fullDocList.Where(t => t.DocumentNo == contractorDoc.DocumentNo).ToList();
			// ----------------------------------------------------------------------------------------

			var currentProjectDocList = this.pecc2DocumentsService.GetAllByProjectDocNo(contractorDoc.DocumentNo);

			Guid PECC2DocId = contractorDoc.ID;
			var projectDoc = new PECC2Documents();
			// Case: Already have previous document
			if (currentProjectDocList.Count > 0)
			{
				var currentLeafProjectDoc = currentProjectDocList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
				if (currentLeafProjectDoc != null)
				{
					projectDoc = currentLeafProjectDoc;

					if (projectDoc.Revision.ToLower().Trim() == contractorDoc.Revision.ToLower().Trim())
					{
						// Fill incoming trans info
						projectDoc.GroupId = contractorDoc.GroupCodeId;
						projectDoc.GroupCode = contractorDoc.GroupCodeName;
						projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
						projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
						projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
						projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
						this.pecc2DocumentsService.Update(projectDoc);
						//-------------------------------------------------------------------------------

						// -------------------------------------------------------------------------------
						if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
						{
							var attachDoc = new AttachDocToTransmittal()
							{
								TransmittalId = objPECC2Id,
								DocumentId = projectDoc.ID
							};
							if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
							{
								this.attachDocToTransmittalService.Insert(attachDoc);
							}
						}
					}
					else
					{
						// Collect new project doc info
						projectDoc = new PECC2Documents();
						this.CollectProjectDocData(contractorDoc, projectDoc);
						projectDoc.DocNo = currentLeafProjectDoc.DocNo;
						projectDoc.DocTitle = currentLeafProjectDoc.DocTitle;
						projectDoc.ProjectId = currentLeafProjectDoc.ProjectId;
						projectDoc.ProjectName = currentLeafProjectDoc.ProjectName;
						projectDoc.RevisionSchemaId = currentLeafProjectDoc.RevisionSchemaId;
						projectDoc.RevisionSchemaName = currentLeafProjectDoc.RevisionSchemaName;
						projectDoc.ConfidentialityId = currentLeafProjectDoc.ConfidentialityId;
						projectDoc.ConfidentialityName = currentLeafProjectDoc.ConfidentialityName;
						if (revisionStatus != null)
						{
							projectDoc.RevStatusId = revisionStatus.ID;
							projectDoc.RevStatusName = revisionStatus.FullName;

							if (revisionStatus.Code == "FA")
							{
								projectDoc.ActualDate = pecc2IncomingTrans.IssuedDate;
							}
						}

						projectDoc.CreatedBy = UserSession.Current.User.Id;
						projectDoc.CreatedDate = DateTime.Now;
						projectDoc.IsLeaf = true;
						projectDoc.IsDelete = false;
						projectDoc.IsHasAttachFile = true;
						projectDoc.ParentId = currentLeafProjectDoc.ParentId ?? currentLeafProjectDoc.ID;
						projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
						projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
						//projectDoc.IsssuedDate = contractorTransObj.TransDate;
						// Fill incoming trans info
						projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
						projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                        projectDoc.CategoryId = pecc2IncomingTrans.CategoryId;
                        projectDoc.CategoryName = pecc2IncomingTrans.CategoryName;
                        //-------------------------------------------------------------------------------

                        this.pecc2DocumentsService.Insert(projectDoc);
						PECC2DocId = projectDoc.ID;
						// -------------------------------------------------------------------------------
						if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
						{
							var attachDoc = new AttachDocToTransmittal()
							{
								TransmittalId = objPECC2Id,
								DocumentId = projectDoc.ID
							};
							if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
							{
								this.attachDocToTransmittalService.Insert(attachDoc);
							}
						}

						// Update leaf project doc
						currentLeafProjectDoc.IsLeaf = false;
						this.pecc2DocumentsService.Update(currentLeafProjectDoc);
						// -------------------------------------------------------------------------------------------------------------
					}

					//Attach doc file to project doc
					this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
					// --------------------------------------------------------------------------------------------------------------

					// Update PECC2 Incoming trans info
					pecc2IncomingTrans.Status = string.Empty;
					pecc2IncomingTrans.ErrorMessage = string.Empty;
					pecc2IncomingTrans.IsImport = true;

					this.PECC2TransmittalService.Update(pecc2IncomingTrans);
					// --------------------------------------------------------------------------------------------------------------
				}
			}
			// -----------------------------------------------------------------------------------------------------

			// Case: Document sent by contractor is new doc
			else
			{
				// Collect new project doc info
				projectDoc = new PECC2Documents();
				this.CollectProjectDocData(contractorDoc, projectDoc);

				projectDoc.ProjectId = contractorDoc.ProjectId;
				projectDoc.ProjectName = contractorDoc.ProjectName;
				projectDoc.RevisionSchemaId = 0;
				projectDoc.RevisionSchemaName = string.Empty;
				if (revisionStatus != null)
				{
					projectDoc.RevStatusId = revisionStatus.ID;
					projectDoc.RevStatusName = revisionStatus.FullName;
				}

				projectDoc.CreatedBy = UserSession.Current.User.Id;
				projectDoc.CreatedDate = DateTime.Now;
				projectDoc.IsHasAttachFile = true;
				projectDoc.IsLeaf = true;
				projectDoc.IsDelete = false;

				// Fill incoming trans info
				projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
				projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
				projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
				projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
				//-------------------------------------------------------------------------------

				// get master info
				////this.CollectMasterInfo(masterDoc, projectDoc);
				// -----------------------------------------------------------------------------------------------------

				this.pecc2DocumentsService.Insert(projectDoc);

				if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
				{
					var attachDoc = new AttachDocToTransmittal()
					{
						TransmittalId = objPECC2Id,
						DocumentId = projectDoc.ID
					};
					if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
					{
						this.attachDocToTransmittalService.Insert(attachDoc);
					}
				}
				//update contractorTransDocFileObj
				//Attach doc file to project doc
				this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
				// --------------------------------------------------------------------------------------------------------------

				// Update PECC2 Incoming trans info
				pecc2IncomingTrans.Status = string.Empty;
				pecc2IncomingTrans.ErrorMessage = string.Empty;
				pecc2IncomingTrans.IsImport = true;

				this.PECC2TransmittalService.Update(pecc2IncomingTrans);
				// --------------------------------------------------------------------------------------------------------------
			}
			// -----------------------------------------------------------------------------------------------------

			contractorDoc.PECC2ProjectDocId = projectDoc.ID;
			contractorDoc.IsReject = false;
			this.contractorTransmittalDocFileService.Update(contractorDoc);
		}

		private void ProcessImportProjectDocument(List<ContractorTransmittalDocFile> fullDocList, ContractorTransmittalDocFile contractorDoc, PECC2Transmittal pecc2IncomingTrans, RevisionStatu revisionStatus, Guid objPECC2Id, ChangeRequest changeRequestObj)
		{
			// Get attach doc list
			var contractorDocAttach = fullDocList.Where(t => t.DocumentNo == contractorDoc.DocumentNo).ToList();
			// ----------------------------------------------------------------------------------------

			var currentProjectDocList = this.pecc2DocumentsService.GetAllByProjectDocNo(contractorDoc.DocumentNo);

			Guid PECC2DocId = contractorDoc.ID;
			var projectDoc = new PECC2Documents();
			// Case: Already have previous document
			if (currentProjectDocList.Count > 0)
			{
				var currentLeafProjectDoc = currentProjectDocList.FirstOrDefault(t => t.IsLeaf.GetValueOrDefault());
				if (currentLeafProjectDoc != null)
				{
					projectDoc = currentLeafProjectDoc;

					if (projectDoc.Revision.ToLower().Trim() == contractorDoc.Revision.ToLower().Trim())
					{
						// Fill incoming trans info
						projectDoc.GroupId = contractorDoc.GroupCodeId;
						projectDoc.GroupCode = contractorDoc.GroupCodeName;
						projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
						projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
						projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
						projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
						this.pecc2DocumentsService.Update(projectDoc);
						//-------------------------------------------------------------------------------

						// -------------------------------------------------------------------------------
						if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
						{
							var attachDoc = new AttachDocToTransmittal()
							{
								TransmittalId = objPECC2Id,
								DocumentId = projectDoc.ID
							};
							if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
							{
								this.attachDocToTransmittalService.Insert(attachDoc);
							}
						}
					}
					else
					{
						// Collect new project doc info
						projectDoc = new PECC2Documents();
						this.CollectProjectDocData(contractorDoc, projectDoc);
						projectDoc.DocNo = currentLeafProjectDoc.DocNo;
						projectDoc.DocTitle = currentLeafProjectDoc.DocTitle;
						projectDoc.ProjectId = currentLeafProjectDoc.ProjectId;
						projectDoc.ProjectName = currentLeafProjectDoc.ProjectName;
						projectDoc.RevisionSchemaId = currentLeafProjectDoc.RevisionSchemaId;
						projectDoc.RevisionSchemaName = currentLeafProjectDoc.RevisionSchemaName;
						projectDoc.ConfidentialityId = currentLeafProjectDoc.ConfidentialityId;
						projectDoc.ConfidentialityName = currentLeafProjectDoc.ConfidentialityName;

						// Fill change request Info
					  //  projectDoc.ChangeRequestId = changeRequestObj.ID;
					   // projectDoc.ChangeRequestNo = changeRequestObj.Number;
						projectDoc.ChangeRequestReviewResultId = 0;
						projectDoc.ChangeRequestReviewResultCode = string.Empty;
						// ----------------------------------------------------------------------------

						if (revisionStatus != null)
						{
							projectDoc.RevStatusId = revisionStatus.ID;
							projectDoc.RevStatusName = revisionStatus.FullName;

							if (revisionStatus.Code == "FA")
							{
								projectDoc.ActualDate = pecc2IncomingTrans.IssuedDate;
							}
						}

						projectDoc.CreatedBy = UserSession.Current.User.Id;
						projectDoc.CreatedDate = DateTime.Now;
						projectDoc.IsLeaf = true;
						projectDoc.IsDelete = false;
						projectDoc.IsHasAttachFile = true;
						projectDoc.ParentId = currentLeafProjectDoc.ParentId ?? currentLeafProjectDoc.ID;
						projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
						projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
						//projectDoc.IsssuedDate = contractorTransObj.TransDate;
						// Fill incoming trans info
						projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
						projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
                        projectDoc.CategoryId = pecc2IncomingTrans.CategoryId;
                        projectDoc.CategoryName = pecc2IncomingTrans.CategoryName;
                        //-------------------------------------------------------------------------------

                        this.pecc2DocumentsService.Insert(projectDoc);
						PECC2DocId = projectDoc.ID;
						// -------------------------------------------------------------------------------
						if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
						{
							var attachDoc = new AttachDocToTransmittal()
							{
								TransmittalId = objPECC2Id,
								DocumentId = projectDoc.ID
							};
							if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
							{
								this.attachDocToTransmittalService.Insert(attachDoc);
							}
						}

						// Update leaf project doc
						currentLeafProjectDoc.IsLeaf = false;
						this.pecc2DocumentsService.Update(currentLeafProjectDoc);
						// -------------------------------------------------------------------------------------------------------------
					}

					//Attach doc file to project doc
					this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
					// --------------------------------------------------------------------------------------------------------------

					// Update PECC2 Incoming trans info
					pecc2IncomingTrans.Status = string.Empty;
					pecc2IncomingTrans.ErrorMessage = string.Empty;
					pecc2IncomingTrans.IsImport = true;

					this.PECC2TransmittalService.Update(pecc2IncomingTrans);
					// --------------------------------------------------------------------------------------------------------------
				}
			}
			// -----------------------------------------------------------------------------------------------------

			// Case: Document sent by contractor is new doc
			else
			{
				// Collect new project doc info
				projectDoc = new PECC2Documents();
				this.CollectProjectDocData(contractorDoc, projectDoc);

				projectDoc.ProjectId = contractorDoc.ProjectId;
				projectDoc.ProjectName = contractorDoc.ProjectName;
                projectDoc.CategoryId = pecc2IncomingTrans.CategoryId;
                projectDoc.CategoryName = pecc2IncomingTrans.CategoryName;

                projectDoc.RevisionSchemaId = 0;
				projectDoc.RevisionSchemaName = string.Empty;
				if (revisionStatus != null)
				{
					projectDoc.RevStatusId = revisionStatus.ID;
					projectDoc.RevStatusName = revisionStatus.FullName;
				}

				projectDoc.CreatedBy = UserSession.Current.User.Id;
				projectDoc.CreatedDate = DateTime.Now;
				projectDoc.IsHasAttachFile = true;
				projectDoc.IsLeaf = true;
				projectDoc.IsDelete = false;

				// Fill incoming trans info
				projectDoc.IncomingTransId = pecc2IncomingTrans.ID;
				projectDoc.IncomingTransNo = pecc2IncomingTrans.TransmittalNo;
				projectDoc.DocActionId = pecc2IncomingTrans.PurposeId;
				projectDoc.DocActionCode = pecc2IncomingTrans.PurposeName.Split(',')[0];
				//-------------------------------------------------------------------------------


				// Fill change request Info
			   // projectDoc.ChangeRequestId = changeRequestObj.ID;
			   // projectDoc.ChangeRequestNo = changeRequestObj.Number;
				projectDoc.ChangeRequestReviewResultId = 0;
				projectDoc.ChangeRequestReviewResultCode = string.Empty;
				// ----------------------------------------------------------------------------

				// get master info
				////this.CollectMasterInfo(masterDoc, projectDoc);
				// -----------------------------------------------------------------------------------------------------

				this.pecc2DocumentsService.Insert(projectDoc);

				if (objPECC2Id != null && !string.IsNullOrEmpty(objPECC2Id.ToString()))
				{
					var attachDoc = new AttachDocToTransmittal()
					{
						TransmittalId = objPECC2Id,
						DocumentId = projectDoc.ID
					};
					if (!this.attachDocToTransmittalService.IsExist(objPECC2Id, projectDoc.ID))
					{
						this.attachDocToTransmittalService.Insert(attachDoc);
					}
				}
				//update contractorTransDocFileObj
				//Attach doc file to project doc
				this.AttachDocFileToProjectDoc(contractorDocAttach, projectDoc);
				// --------------------------------------------------------------------------------------------------------------

				// Update PECC2 Incoming trans info
				pecc2IncomingTrans.Status = string.Empty;
				pecc2IncomingTrans.ErrorMessage = string.Empty;
				pecc2IncomingTrans.IsImport = true;

				this.PECC2TransmittalService.Update(pecc2IncomingTrans);
				// --------------------------------------------------------------------------------------------------------------
			}
			// -----------------------------------------------------------------------------------------------------

			// Update project doc ID after insert for Contractor Attach doc file
			contractorDoc.PECC2ProjectDocId = projectDoc.ID;
			contractorDoc.IsReject = false;
			this.contractorTransmittalDocFileService.Update(contractorDoc);
			// -----------------------------------------------------------------

			// Update refDoc for DCN
			if (changeRequestObj.TypeName == "DCN")
			{
				changeRequestObj.DocToBeRevisedId += projectDoc.ID + ";";
				changeRequestObj.DocToBeRevisedNo += projectDoc.DocNoWithRev + Environment.NewLine;
				this.changeRequestService.Update(changeRequestObj);
			}
			// -----------------------------------------------------------------------------
		}

		private void AttachDocFileToProjectDoc(List<ContractorTransmittalDocFile> attachList, PECC2Documents projectDoc)
		{
			var targetFolder = "../../DocumentLibrary/ProjectDocs";
			var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
				+ "/DocumentLibrary/ProjectDocs";
			foreach (var contractorAttachFile in attachList)
			{
				var docFileName = contractorAttachFile.FileName;

				// Path file to save on server disc
				var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileName);
				// Path file to download from server
				var serverFilePath = serverFolder + "/" + docFileName;

				File.Copy(Server.MapPath("../.." + contractorAttachFile.FilePath), saveFilePath, true);

				var attachFile = new PECC2DocumentAttachFile()
				{
					ID = Guid.NewGuid(),
					ProjectDocumentId = projectDoc.ID,
					FileName = docFileName,
					Extension = contractorAttachFile.Extension,
					FilePath = serverFilePath,
					ExtensionIcon = contractorAttachFile.ExtensionIcon,
					FileSize = contractorAttachFile.FileSize,
					TypeId = 1,
					TypeName = "Document file",
					CreatedBy = UserSession.Current.User.Id,
					CreatedByName = UserSession.Current.User.UserNameWithFullName,
					CreatedDate = DateTime.Now
				};

				projectDoc.IsHasAttachFile = true;
				this.pecc2DocumentsService.Update(projectDoc);
				this.documentAttachFileService.Insert(attachFile);
			}
		}

		private void AttachDocFileToChangeRequest(List<ContractorTransmittalDocFile> attachList, ChangeRequest changeRequestObj)
		{
			var targetFolder = "../../DocumentLibrary/ChangeRequest";
			var serverFolder = (HostingEnvironment.ApplicationVirtualPath == "/" ? string.Empty : HostingEnvironment.ApplicationVirtualPath)
				+ "/DocumentLibrary/ChangeRequest";
			foreach (var contractorAttachFile in attachList)
			{
				var docFileName = contractorAttachFile.FileName;

				// Path file to save on server disc
				var saveFilePath = Path.Combine(Server.MapPath(targetFolder), docFileName);
				// Path file to download from server
				var serverFilePath = serverFolder + "/" + docFileName;

				File.Copy(Server.MapPath("../.." + contractorAttachFile.FilePath), saveFilePath, true);

				var attachFile = new ChangeRequestAttachFile()
				{
					ID = Guid.NewGuid(),
					ChangeRequestId = changeRequestObj.ID,
					FileName = docFileName,
					Extension = contractorAttachFile.Extension,
					FilePath = serverFilePath,
					ExtensionIcon = contractorAttachFile.ExtensionIcon,
					FileSize = contractorAttachFile.FileSize,
					CreatedBy = UserSession.Current.User.Id,
					CreatedByName = UserSession.Current.User.UserNameWithFullName,
					CreatedDate = DateTime.Now,
					TypeId = 1,
					TypeName = changeRequestObj.TypeName + " Form",
				};

				this.changeRequestAttachFileService.Insert(attachFile);

				contractorAttachFile.PECC2ProjectDocId = changeRequestObj.ID;
				this.contractorTransmittalDocFileService.Update(contractorAttachFile);
			}
		}

		private void UpdateTransOutToDocument(PECC2Transmittal transObj, int forSendId)
		{
			var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
			foreach (var docobj in attachDocToTrans)
			{
				if (forSendId == 1)
				{
					var projectDoc = this.pecc2DocumentsService.GetById(docobj.DocumentId.GetValueOrDefault());
					projectDoc.OutgoingTransId = transObj.ID;
					projectDoc.OutgoingTransNo = transObj.TransmittalNo;
					projectDoc.IsCreateOutgoingTrans = true;
					this.pecc2DocumentsService.Update(projectDoc);
				}
				else
				{
					var changeRequest = this.changeRequestService.GetById(docobj.DocumentId.GetValueOrDefault());
					if (changeRequest != null)
					{
						changeRequest.OutgoingTransId = transObj.ID;
						changeRequest.OutgoingTransNo = transObj.TransmittalNo;
						changeRequest.IsCreateOutgoingTrans = true;
						this.changeRequestService.Update(changeRequest);
					}
					else
					{
						var projectDoc = this.pecc2DocumentsService.GetById(docobj.DocumentId.GetValueOrDefault());
						projectDoc.OutgoingTransId = transObj.ID;
						projectDoc.OutgoingTransNo = transObj.TransmittalNo;
						projectDoc.IsCreateOutgoingTrans = true;
						this.pecc2DocumentsService.Update(projectDoc);
					}
				}

			}
		}

		private void CollectProjectDocData(ContractorTransmittalDocFile contractorDoc, PECC2Documents obj)
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
			//var docTypeObj = this.documentTypeService.GetById(obj.DocTypeId.GetValueOrDefault());
			//if (docTypeObj != null)
			//{
				obj.CategoryId = contractorDoc.DocumentTypeGroupId;
				obj.CategoryName = contractorDoc.DocumentTypeGroupName;
			//}

			obj.Description = string.Empty;
			obj.Treatment = string.Empty;
			obj.RevRemarks = string.Empty;
		}

		private void CollectChangeRequestData(ContractorTransmittalDocFile contractorDoc, ChangeRequest obj)
		{
			obj.ID = Guid.NewGuid();
			obj.Number = contractorDoc.DocumentNo;
			obj.Description = contractorDoc.DocumentTitle;
			obj.ConfidentialityId = 0;
			obj.ConfidentialityName = string.Empty;
			obj.GroupId = contractorDoc.GroupCodeId;
			obj.GroupName = contractorDoc.GroupCodeName;
			obj.AreaId = contractorDoc.AreaId;
			obj.AreaCode = contractorDoc.AreaName;
			obj.UnitId = contractorDoc.UnitCodeId;
			obj.UnitCode = contractorDoc.UnitCodeName;
			obj.Sequence = Convert.ToInt32(contractorDoc.Sequence);
			obj.SequentialNumber = contractorDoc.Sequence;
			obj.Year = Convert.ToInt32(contractorDoc.Year);
			obj.TypeId = contractorDoc.ChangeRequestTypeId;
			obj.TypeName = contractorDoc.ChangeRequestTypeName;
		}


		private void Download_File(string FilePath)
		{
			Response.ContentType = ContentType;
			Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
			Response.WriteFile(FilePath);
			Response.End();
		}

		private void ExportContractorETRM(ContractorTransmittal transObj)
		{
			var attachDocFullList = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID);
			var attachDocFileFilter = new List<ContractorTransmittalDocFile>();

			// Remove duplicate Document
			foreach (var document in attachDocFullList)
			{
				if (attachDocFileFilter.All(t => t.DocumentNo != document.DocumentNo))
				{
					attachDocFileFilter.Add(document);
				}
			}
			// --------------------------------------------------------------------------------------------

			var filePath = Server.MapPath("../../Exports") + @"\";
			var workbook = new Workbook();
			workbook.Open(filePath + @"Template\PECC2_ContractorTransTemplate.xlsm");
			var workSheets = workbook.Worksheets;
			var transSheet = workSheets[0];
			//var fileListSheet = workSheets[9];
			// Export trans Info
			var dtFull = new DataTable();
			dtFull.Columns.AddRange(new[]
			{
				new DataColumn("DocNo", typeof(String)),
				new DataColumn("1Empty", typeof(String)),
				new DataColumn("Revision", typeof(string)),
				new DataColumn("ActionCode", typeof(String)),
				new DataColumn("DocTitle", typeof(String)),
				new DataColumn("2Empty", typeof(String)),
				new DataColumn("3Empty", typeof(String)),
				new DataColumn("4Empty", typeof(String)),
				new DataColumn("5Empty", typeof(String)),
				new DataColumn("6Empty", typeof(String)),
				new DataColumn("RevRemark", typeof(String)),
				new DataColumn("7Empty", typeof(String)),
			});

			var count = 1;
			foreach (var doc in attachDocFileFilter.OrderBy(t=>t.DocumentNo))
			{
				var dataRow = dtFull.NewRow();
				dataRow["DocNo"] = doc.DocumentNo;
				dataRow["DocTitle"] = doc.DocumentTitle;
				dataRow["Revision"] = doc.Revision;
				dataRow["ActionCode"] = doc.PurposeName;
				dataRow["RevRemark"] = doc.RevRemark;
				dtFull.Rows.Add(dataRow);
				count += 1;
			}

			transSheet.Cells.ImportDataTable(dtFull, false, 8, 0, dtFull.Rows.Count, dtFull.Columns.Count, true);

			for (int i = 0; i < attachDocFileFilter.Count; i++)
			{
				transSheet.Cells.Merge(8 + i, 0, 1, 2);
				transSheet.Cells.Merge(8 + i, 4, 1, 6);
				transSheet.Cells.Merge(8 + i, 10, 1, 2);
			}

			//transSheet.Cells.DeleteRow(19 + attachDocFileFilter.Count);
			var organisationObj =
				this.organizationCodeService.GetById(transObj.OriginatingOrganizationId.GetValueOrDefault());
			var projectObj = this.projectCodeService.GetById(transObj.ProjectId.GetValueOrDefault());
			transSheet.Cells["A2"].PutValue(projectObj.FullName);
			transSheet.Cells["J1"].PutValue(transObj.TransNo);
			transSheet.Cells["H5"].PutValue(projectObj.Code);
			transSheet.Cells["H2"].PutValue(transObj.TransDate.GetValueOrDefault().ToString("yyyy-MM-dd"));
			transSheet.Cells["C3"].PutValue(transObj.OriginatingOrganizationName);
			if (organisationObj != null)
			{
				transSheet.Cells["C4"].PutValue(organisationObj.HeadOffice);
				transSheet.Cells["C5"].PutValue(organisationObj.Phone);
				transSheet.Cells["C6"].PutValue(organisationObj.Fax);
			}

			// ---------------------------------------------------------------------

			var savePath = Server.MapPath("../.." + transObj.StoreFolderPath) + "\\eTRM File\\";
			var fileName = transObj.TransNo + "_eTRM_" +
						   transObj.TransDate.GetValueOrDefault().ToString("dd-MM-yyyy") + ".xlsm";
			workbook.Save(savePath + fileName);

			this.Download_File(savePath + fileName);
		}
		private void ExportETRM(PECC2Transmittal transObj, string forSend)
		{
			var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
			if (attachDocToTrans != null)
			{
				var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
				//var filePath = Server.MapPath("Exports") + @"\";
				var workbook = new Workbook();
				workbook.Open(filePath + @"Template\PECC2TransmittalTemplate_New.xlsm");

				var dataSheet = workbook.Worksheets[0];

				var dtFull = new DataTable();

				dtFull.Columns.AddRange(new[]
				{
					new DataColumn("DocumentNo", typeof(String)),
					new DataColumn("Empty1", typeof(String)),
					new DataColumn("Empty2", typeof(String)),
					new DataColumn("Revision", typeof(String)),
					new DataColumn("DocumentTitle", typeof(String)),
					new DataColumn("Empty3", typeof(String)),
					new DataColumn("Empty4", typeof(String)),
					new DataColumn("Empty5", typeof(String)),
					new DataColumn("Empty6", typeof(String)),
					new DataColumn("Empty7", typeof(String)),
					new DataColumn("DocReviewStatusCode", typeof(String)),
				});

				foreach (var docobj in attachDocToTrans)
				{
					var dataRow = dtFull.NewRow();
					switch (forSend)
					{
						case "1":
							var documentObj = this.pecc2DocumentsService.GetById(docobj.DocumentId.GetValueOrDefault());
							if (documentObj != null)
							{
								dataRow["DocumentNo"] = documentObj.DocNo;
								dataRow["Revision"] = documentObj.Revision;
								dataRow["DocumentTitle"] = documentObj.DocTitle;
								dataRow["DocReviewStatusCode"] = documentObj.DocReviewStatusCode;
							}
							
							break;
						case "2":
							var changeRequestObj =
								this.changeRequestService.GetById(docobj.DocumentId.GetValueOrDefault());
							if (changeRequestObj != null)
							{
								dataRow["DocumentNo"] = changeRequestObj.Number;
								dataRow["Revision"] = changeRequestObj.Revision;
								dataRow["DocumentTitle"] = changeRequestObj.Title;
								dataRow["DocReviewStatusCode"] = changeRequestObj.ReviewResultName;
							}

							var documentObj1 = this.pecc2DocumentsService.GetById(docobj.DocumentId.GetValueOrDefault());
							if (documentObj1 != null)
							{
								dataRow["DocumentNo"] = documentObj1.DocNo;
								dataRow["Revision"] = documentObj1.Revision;
								dataRow["DocumentTitle"] = documentObj1.DocTitle;
								dataRow["DocReviewStatusCode"] = documentObj1.DocReviewStatusCode;
							}

							break;
					}

					dtFull.Rows.Add(dataRow);
				}

				DataView dv = dtFull.DefaultView;
				dv.Sort = "DocumentNo asc";
				dtFull = dv.ToTable();

				var projectObj = this.projectCodeService.GetById(transObj.ProjectCodeId.GetValueOrDefault());

				var filename = transObj.TransmittalNo + "_Transmittal Cover Sheet.xlsm";
				dataSheet.Cells["K5"].PutValue(transObj.TransmittalNo);
				dataSheet.Cells["K6"].PutValue(DateTime.Now.ToString("dd-MMM-yy"));
				dataSheet.Cells["C5"].PutValue(projectObj.FullName);
				dataSheet.Cells["C8"].PutValue(transObj.Description);
				dataSheet.Cells["K7"].PutValue(transObj.RefTransNo);
				//dataSheet.Cells["B10"].PutValue(transObj.ToValue);
				//dataSheet.Cells["B12"].PutValue(transObj.CCValue);


				dataSheet.Cells.ImportDataTable(dtFull, false, 21, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);

				for (int i = 0; i < dtFull.Rows.Count; i++)
				{
					dataSheet.Cells.Merge(21 + i, 1, 1, 3);
					dataSheet.Cells.Merge(21 + i, 5, 1, 6);
				}

				// Fill Signed
				//if (!string.IsNullOrEmpty(UserSession.Current.User.SignImageUrl))
				//{
				//    dataSheet.Pictures.Add(23 + dtFull.Rows.Count, 2, Server.MapPath("../.." + UserSession.Current.User.SignImageUrl));
				//}
				// ---------------------------------------------------------------------
				var options = new AutoFitterOptions();
				options.AutoFitMergedCells = true;
				dataSheet.AutoFitRows(options);

				workbook.Save(filePath + filename);
				this.Download_File(filePath + filename);
			}
		}

		private void ExportApproveLetter(PECC2Transmittal transObj, string forSend)
		{
			var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
			if (attachDocToTrans != null)
			{
				var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
				//var filePath = Server.MapPath("Exports") + @"\";
				var workbook = new Workbook();
				workbook.Open(filePath + @"Template\PECC2ApproveLetter.xlsm");

				var dataSheet = workbook.Worksheets[0];

				var dtFull = new DataTable();

				dtFull.Columns.AddRange(new[]
				{
					new DataColumn("TransNo", typeof(String)),
					new DataColumn("Empty1", typeof(String)),
					new DataColumn("TransDate", typeof(String)),
					new DataColumn("ProjectCode", typeof(String)),
					new DataColumn("DocumentType", typeof(String)),
					new DataColumn("UnitCode", typeof(String)),
					new DataColumn("KKS", typeof(String)),
					new DataColumn("TrainNo", typeof(String)),
					new DataColumn("Discipline", typeof(String)),
					new DataColumn("SerialNo", typeof(String)),
					new DataColumn("DocumentTitle", typeof(String)),
					new DataColumn("Empty2", typeof(String)),
					new DataColumn("Revision", typeof(String)),
					new DataColumn("DocReviewStatusCode", typeof(String)),
					new DataColumn("ReFC", typeof(String)), 
				});
				switch (forSend)
				{
					case "1":
						var transinobj = this.PECC2TransmittalService.GetById(transObj.RefTransId.GetValueOrDefault());
						var doclist = this.pecc2DocumentsService.GetAllDocList(attachDocToTrans.Select(t => t.DocumentId.GetValueOrDefault()).ToList());
						foreach (var documentObj in doclist.OrderBy(t => t.DocNo))
						{
							var dataRow = dtFull.NewRow();

							//  var documentObj = this.pecc2DocumentsService.GetById(docobj.DocumentId.GetValueOrDefault());
							dataRow["TransNo"] = transObj.TransmittalNo.Replace("-T-","-L-");
							dataRow["TransDate"] = transinobj.ReceivedDate.GetValueOrDefault().ToString("dd-MMM-yy");
							dataRow["ProjectCode"] = documentObj.ProjectName;
							dataRow["DocumentType"] = documentObj.DocTypeCode;
							dataRow["UnitCode"] = documentObj.UnitCode;
							dataRow["KKS"] = documentObj.KKSCode;
							dataRow["TrainNo"] = documentObj.TrainNo;
							dataRow["Discipline"] = documentObj.DisciplineCode;
							dataRow["SerialNo"] = documentObj.SequenceText;
							dataRow["DocumentTitle"] = documentObj.DocTitle;
							dataRow["DocReviewStatusCode"] = documentObj.DocReviewStatusCode;
							dataRow["ReFC"] = documentObj.DocReviewStatusCode2;
							dataRow["Revision"] = documentObj.Revision;

							dtFull.Rows.Add(dataRow);
						   
						}
						break;
					case "2":
						var transinobj1 = this.PECC2TransmittalService.GetById(transObj.RefTransId.GetValueOrDefault());
					   
						var changeRequestObjList =
							this.changeRequestService.GetAllByOutTransTrans(transObj.ID);
						foreach (var changeRequestObj in changeRequestObjList)
						{ var dataRow = dtFull.NewRow();
							dataRow["TransNo"] = transObj.TransmittalNo.Replace("-T-", "-L-");
							dataRow["TransDate"] = transinobj1.ReceivedDate.GetValueOrDefault().ToString("dd-MMM-yy");
							dataRow["ProjectCode"] = changeRequestObj.Number;
							dataRow["Revision"] = changeRequestObj.Revision;
							dataRow["DocumentTitle"] = changeRequestObj.Description;
							dataRow["DocReviewStatusCode"] = changeRequestObj.ReviewResultName;
							dtFull.Rows.Add(dataRow);
						}
						var docpecc2ObjList = pecc2DocumentsService.GetAllByOutgoingTrans(transObj.ID);
						foreach (var documentObj in docpecc2ObjList)
						{
							var dataRow = dtFull.NewRow();
							dataRow["TransNo"] = transObj.TransmittalNo.Replace("-T-", "-L-");
							dataRow["TransDate"] = transinobj1.ReceivedDate.GetValueOrDefault().ToString("dd-MMM-yy");
							dataRow["ProjectCode"] = documentObj.ProjectName;
							dataRow["DocumentType"] = documentObj.DocTypeCode;
							dataRow["UnitCode"] = documentObj.UnitCode;
							dataRow["KKS"] = documentObj.KKSCode;
							dataRow["TrainNo"] = documentObj.TrainNo;
							dataRow["Discipline"] = documentObj.DisciplineCode;
							dataRow["SerialNo"] = documentObj.SequenceText;
							dataRow["DocumentTitle"] = documentObj.DocTitle;
							dataRow["DocReviewStatusCode"] = documentObj.DocReviewStatusCode;
							dataRow["ReFC"] = documentObj.DocReviewStatusCode2;
							dataRow["Revision"] = documentObj.Revision;

							dtFull.Rows.Add(dataRow);

						}

						break;


				}

				DataView dv = dtFull.DefaultView;
				dv.Sort = "SerialNo asc";
				dtFull = dv.ToTable();

				var projectObj = this.projectCodeService.GetById(transObj.ProjectCodeId.GetValueOrDefault());

				var filename = transObj.TransmittalNo.Replace("-T-", "-L-") + "_Approve Letter.xlsm";
				dataSheet.Cells["M6"].PutValue(transObj.TransmittalNo.Replace("-T-", "-L-"));
				dataSheet.Cells["M7"].PutValue(DateTime.Now.ToString("dd-MMM-yy"));
				dataSheet.Cells["B16"].PutValue("Subject: " + projectObj.Description);
				dataSheet.Cells["B18"].PutValue(transObj.TransmittalNo.Replace("-T-", "-L-")+"_"+transObj.Description);
				dataSheet.Cells["B20"].PutValue("You will find attached document/ drawing on the subject documentation which we received under your transmittal No." + transObj.RefTransNo);



				dataSheet.Cells.ImportDataTable(dtFull, false, 22, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);

				if (forSend == "2")
				{
					dataSheet.Cells.Merge(22, 4, 1, 7);
				}

				for (int i = 0; i < dtFull.Rows.Count; i++)
				{
					dataSheet.Cells.Merge(22 + i, 1, 1, 2);
					dataSheet.Cells.Merge(22 + i, 11, 1, 2);
				}

				// Fill Signed
				//if (!string.IsNullOrEmpty(UserSession.Current.User.SignImageUrl))
				//{
				//    dataSheet.Pictures.Add(23 + dtFull.Rows.Count, 2, Server.MapPath("../.." + UserSession.Current.User.SignImageUrl));
				//}
				// ---------------------------------------------------------------------
				var options = new AutoFitterOptions();
				options.AutoFitMergedCells = true;
				dataSheet.AutoFitRows(options);

				workbook.Save(filePath + filename);
				this.Download_File(filePath + filename);
			
			}
		}

		protected void ddlProjectOutgoing_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
			int projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);
			this.lblProjectOutgoingId.Value = projectId.ToString();
			this.grdOutgoingTrans.Rebind();
		}

		protected void ddlProjectIncoming_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
			int projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);
			this.lblProjectIncomingId.Value = projectId.ToString();
			this.grdIncomingTrans.Rebind();
		}

		protected void ddlProjectOutgoing_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
		{
			e.Item.ImageUrl = @"~/Images/project.png";
		}

		protected void ddlProjectIncoming_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
		{
			e.Item.ImageUrl = @"~/Images/project.png";
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{

		}

		protected void grdOutgoingTrans_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
		{
			var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
			var ddlStatusOutgoing = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatusOutgoing");
			var txtSearchOutgoing = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearchOutgoing");
			var outgoingTransList = new List<PECC2Transmittal>();

			if (ddlProjectOutgoing != null && ddlProjectOutgoing.SelectedItem != null)
			{
				var projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);

				outgoingTransList = this.PECC2TransmittalService.GetAllByProject(projectId, 2, txtSearchOutgoing.Text).OrderByDescending(t => t.TransmittalNo).ToList();

				if (ddlStatusOutgoing != null)
				{
					switch (ddlStatusOutgoing.SelectedValue)
					{
						case "Invalid":
							outgoingTransList = outgoingTransList.Where(t => !t.IsValid.GetValueOrDefault()).ToList();
							break;
						case "Waiting":
							outgoingTransList = outgoingTransList.Where(t => t.IsValid.GetValueOrDefault() && !t.IsSend.GetValueOrDefault()).ToList();
							break;
						case "Sent":
							outgoingTransList = outgoingTransList.Where(t => t.IsSend.GetValueOrDefault()).ToList();
							break;
						case "Waiting&Invalid":
							outgoingTransList = outgoingTransList.Where(t => !t.IsValid.GetValueOrDefault() || !t.IsSend.GetValueOrDefault()).ToList();
							break;
					}
				}


			}

			this.grdOutgoingTrans.DataSource = outgoingTransList.OrderByDescending(t => t.IssuedDate.GetValueOrDefault());
		}

		protected void grdIncomingTrans_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
		{
			var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
			var ddlStatusIncoming = (DropDownList)this.radMenuIncoming.Items[1].FindControl("ddlStatusIncoming");
			var txtSearchIncoming = (TextBox)this.radMenuIncoming.Items[1].FindControl("txtSearchIncoming");
			var incomingTransList = new List<PECC2Transmittal>();
			if (ddlProjectIncoming != null && ddlProjectIncoming.SelectedItem != null)
			{
				var projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);

				incomingTransList = this.PECC2TransmittalService.GetAllByProject(projectId, 1, txtSearchIncoming.Text).OrderByDescending(t => t.TransmittalNo).ToList();

				if (ddlStatusIncoming != null)
				{
					switch (ddlStatusIncoming.SelectedValue)
					{
						case "WaitingValidate":
							incomingTransList = incomingTransList.Where(t => !t.IsImport.GetValueOrDefault()).ToList();
							break;
						case "Rejected":
							incomingTransList = incomingTransList.Where(t => t.IsReject.GetValueOrDefault()).ToList();
							break;
						case "Validated":
							incomingTransList = incomingTransList.Where(t => t.IsImport.GetValueOrDefault() && !t.IsAttachWorkflow.GetValueOrDefault()).ToList();
							break;
						case "Processing":
							incomingTransList = incomingTransList.Where(t => t.IsAttachWorkflow.GetValueOrDefault() && !t.IsWFComplete.GetValueOrDefault()).ToList();
							break;
						case "Processed":
							incomingTransList = incomingTransList.Where(t => !t.IsCreateOutGoingTrans.GetValueOrDefault() && t.IsWFComplete.GetValueOrDefault()).ToList();
							break;
					}
				}
			}

			this.grdIncomingTrans.DataSource = incomingTransList.OrderByDescending(t => t.CreatedDate.GetValueOrDefault());
		}

		protected void ddlStatusOutgoing_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			this.grdOutgoingTrans.Rebind();
		}

		protected void btnSearchOutgoing_Click(object sender, EventArgs e)
		{

			this.grdOutgoingTrans.Rebind();
		}

		protected void ddlStatusIncoming_OnSelectedIndexChanged(object sender, EventArgs e)
		{
		  //  this.grdIncomingTrans.MasterTableView.GetColumn("RejectColumn").Visible = ((DropDownList)sender).SelectedValue == "All" || ((DropDownList)sender).SelectedValue == "WaitingImport";

			this.grdIncomingTrans.Rebind();
		}

		protected void btnSearchIncoming_Click(object sender, EventArgs e)
		{
			this.grdIncomingTrans.Rebind();
		}

		protected void grdOutgoingTrans_OnDeleteCommand(object sender, GridCommandEventArgs e)
		{
			var item = (GridDataItem)e.Item;
			var objId = new Guid(item.GetDataKeyValue("ID").ToString());
			var transObj = this.PECC2TransmittalService.GetById(objId);
			if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
			{
				var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
				if (Directory.Exists(folderPath))
				{
					Directory.Delete(folderPath);
				}
			}
			if(transObj.RefTransId!= null && string.IsNullOrEmpty(transObj.RefTransId.ToString()))
			{
				var transinObj = this.PECC2TransmittalService.GetById(transObj.RefTransId.GetValueOrDefault());
				transinObj.IsCreateOutGoingTrans = false;
				this.PECC2TransmittalService.Update(transinObj);
			}

			this.PECC2TransmittalService.Delete(objId);
			this.grdOutgoingTrans.Rebind();
		}
		private void NotifiNewTransmittal(PECC2Transmittal transmittal)
		{
			try
            {
                var customObj = this.ReceivedEmailService.GetByType(1, 1);
                if (transmittal != null && customObj != null)
                {

                    var userListid = this.userService.GetListUser(customObj.ToUserIDs.Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).ToList());
                    var projctobj = this.projectCodeService.GetById(transmittal.ProjectCodeId.GetValueOrDefault());

					var smtpClient = new SmtpClient
					{
						DeliveryMethod = SmtpDeliveryMethod.Network,
						UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
						EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
						Host = ConfigurationManager.AppSettings["Host"],
						Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
						Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
					};
					int count = 0;
					var containtable = string.Empty;

					var subject = "FYI: Transmittal Response, " + transmittal.TransmittalNo + ", " + transmittal.IssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + transmittal.Description;

					var message = new MailMessage();
					message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
					message.Subject = subject;
					message.BodyEncoding = new UTF8Encoding();
					message.IsBodyHtml = true;
                    var ToEmail = string.Empty;
                    var Userlist = userListid.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                    foreach (var user in Userlist)
                    {
                        try
                        {
                            if (user.Email.Contains(";"))
                            {
                                foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.To.Add(new MailAddress(stemail));
                                    ToEmail += stemail + "; ";
                                }
                            }
                            else
                            {
                                message.To.Add(new MailAddress(user.Email));
                                ToEmail += user.Email + "; ";
                            }
                            
                        }
                        catch { }
                    }
                    var infoUserIds = customObj.CCUserIDs != null
                       ? customObj.CCUserIDs.Split(';').ToList()
                       : new List<string>();
                    var matrixList =
                      this.matrixService.GetAllByList(customObj.DistributionMatrixCCIDs.Split(';')
                           .Where(t => !string.IsNullOrEmpty(t))
                           .Select(t => Convert.ToInt32(t)).ToList());
                    foreach (var matrix in matrixList)
                    {
                        var matrixDetailList = this.matrixDetailService.GetAllByDM(matrix.ID).Where(t => t.GroupCodeId == transmittal.GroupId);
                        infoUserIds.AddRange(matrixDetailList.Select(t => t.UserId.ToString()));
                    }

                    var emailCC = string.Empty;
                    var UsserCC = this.userService.GetListUser(infoUserIds.Distinct().Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).ToList());
                    var listCC = UsserCC.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
                    foreach (var user in listCC)
                    {
                        try
                        {
                            if (user.Email.Contains(";"))
                            {
                                foreach (string stemail in user.Email.Split(';').Where(t => !string.IsNullOrEmpty(t)).ToList())
                                {
                                    message.CC.Add(new MailAddress(stemail));
                                    emailCC += stemail + "; ";
                                }
                            }
                            else
                            {
                                message.CC.Add(new MailAddress(user.Email));
                                emailCC += user.Email + "; ";
                            }
                           
                        }
                        catch { }

                    }



                    var listdocCodepecc2 = "";
                    listdocCodepecc2=this.pecc2DocumentsService.GetAllByOutgoingTrans(transmittal.ID).Select(t=> t.DocReviewStatusCode).Distinct().ToList().Aggregate(listdocCodepecc2,(curent,t)=> curent+t+" ;");
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
									<br>Response (Ref. Transmittal No: " + transmittal.TransmittalNo + ", " + transmittal.Description + @") has been sent to you
								</span>
						</p>
						<div align='center' style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                    <table border='1'>
                <tr>
                    <td class='ch2' style='width:167px' ><span class='font_s' style='color:#003399'>From</span> </td><td colspan='3'><span class='font_s' style='color:#003399'>" + UserSession.Current.User.FullNameWithPosition + @"</span></td>
                   
                </tr>
                <tr>
                     <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>To</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + ToEmail+ "'>" + ToEmail + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>CC</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailCC + "'>" + emailCC + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal No.</span></td><td colspan='3' class='font_s' style='color:red'>" + transmittal.TransmittalNo + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal Title</span></td><td class='font_s' colspan='3'>" + transmittal.Description + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s'>" + transmittal.IssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Review Result Code</span></td><td class='font_s' style='color:red'>" + listdocCodepecc2 + @"</td>
                  </tr>
                  </table>
                  </div>";

					var st = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx?TransNoPecc2=" + transmittal.TransmittalNo;
					var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx";
					bodyContent += @"<p style='margin-bottom:12.0pt'>
			            <span class='font_m'>
				            <u><b>Useful Links:</b></u>
				            <ul class='font_m'>
							<li>
								Click <a href='" + st + @"'>here</a> to show <u>this transmittal</u> in DMDC System
							</li>
							<li>
								Click <a href= '" + st1 + @"' > here</a> to show <u>all transmittals</u> in DMDC System
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

					
					
					smtpClient.Send(message);
				}
			}
			catch { }
		}

		private void NotifiNewTransmittalOut(PECC2Transmittal transmittal)
		{
			try
			{
				if (transmittal != null)
				{
					var smtpClient = new SmtpClient
					{
						DeliveryMethod = SmtpDeliveryMethod.Network,
						UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
						EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
						Host = ConfigurationManager.AppSettings["Host"],
						Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
						Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
					};
					int count = 0;
					var containtable = string.Empty;

					var subject = "New Transmittal (#Trans#) has been sent to contractor (" + transmittal.ReceivingOrganizationName + ")";

					var message = new MailMessage();
					message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "EDMS System");
					message.Subject = subject.Replace("#Trans#", transmittal.TransmittalNo);
					message.BodyEncoding = new UTF8Encoding();
					message.IsBodyHtml = true;

					var bodyContent = @"<div style=‘text-align: center;’> 
									<span class=‘Apple-tab-span’>Dear All,&nbsp;</span><br />
								   
									<p style=‘text-align: center;’><strong><span style=‘font-size: 18px;’>Please be informed the document of transmittal (#Trans#)</span></strong></p><br/><br/>

									   <table border='1' cellspacing='0'>
									   <tr>
									   <th style='text-align:center; width:40px'>No.</th>
									   <th style='text-align:center; width:330px'>Document Number</th>
									   <th style='text-align:center; width:60px'>Revision</th>
									   <th style='text-align:center; width:330px'>Document Title</th>
										<th style='text-align:center; width:330px'>Project</th>
										<th style='text-align:center; width:330px'>Code</th>
									   </tr>";
					var listDocument = new List<PECC2Documents>();

					var attachDocList = this.attachDocToTransmittalService.GetAllByTransId(transmittal.ID);
					foreach (var item in attachDocList)
					{
						var docObj = this.pecc2DocumentsService.GetById(item.DocumentId.GetValueOrDefault());
						if (docObj != null)
						{
							listDocument.Add(docObj);
						}
					}

					var deadline = string.Empty;
					deadline = transmittal.DueDate != null ? transmittal.DueDate.Value.ToString("dd/MM/yyyy") : "";

					foreach (var document in listDocument)
					{

						count += 1;

						bodyContent += @"<tr>
							   <td>" + count + @"</td>
							   <td>" + document.DocNo + @"</td>
							   <td>"
									   + document.Revision + @"</td>
							   <td>"
									   + document.DocTitle + @"</td>
							   <td>"
									   + document.ProjectName + @"</td>
							   <td>"
									   + string.Empty + @"</td></tr>";

					}
					var st = ConfigurationManager.AppSettings["WebAddress"] + @"/AdvanceSearch.aspx?TransOut=" + transmittal.TransmittalNo;
					bodyContent += @" </table>
									   <br/>
									   <span><br />
									&nbsp;This link to access&nbsp;:&nbsp; <a href='" + st + "'>" + st + "</a>" +
								 @" <br/> &nbsp; EDMS TRANSMITTAL NOTIFICATION </br>
						<p><b> [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]</b></p></span>";
					message.Body = bodyContent.Replace("#Trans#", transmittal.TransmittalNo); 


					List<Guid> ListattachDocList = this.attachDocToTransmittalService.GetAllByTransId(transmittal.ID).Select(t => (Guid)t.DocumentId).ToList();
					var ListAllUserInWf = this.objectAssignedUser.GetAllListByDoc(ListattachDocList).Select(t => (int)t.UserID).Distinct().ToList();
					var Userlist = this.userService.GetListUser(ListAllUserInWf);
					foreach (var user in Userlist)
					{
						try
						{
							message.To.Add(new MailAddress(user.Email));
						}
						catch { }

					}
					smtpClient.Send(message);
				}
			}
			catch { }
		}

		private void NotifiImportTransmittal(ContractorTransmittal transmittal)
		{
			try
			{
				if (transmittal != null)
				{

					var userListid = this.userService.GetAllByDC();
					var projctobj = this.projectCodeService.GetById(transmittal.ProjectId.GetValueOrDefault());

					var smtpClient = new SmtpClient
					{
						DeliveryMethod = SmtpDeliveryMethod.Network,
						UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
						EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
						Host = ConfigurationManager.AppSettings["Host"],
						Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
						Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailAccount"], ConfigurationManager.AppSettings["EmailPass"])
					};
					int count = 0;
					var containtable = string.Empty;

					var subject = "FYI: New transmittal validated and imported,  " + transmittal.TransNo + ", " + transmittal.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + transmittal.PurposeName.Split(',')[1] + ", " + transmittal.Description;

					var message = new MailMessage();
					message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
					message.Subject = subject;
					message.BodyEncoding = new UTF8Encoding();
					message.IsBodyHtml = true;
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
				<h2 style='font-family:'Arial Rounded MT Bold';'><u>Transmittal Notification</u></h2>
				<span class='msg'>Dear All,
				<br /><br />Please be informed about new transmittal validated and imported: 
				</span>
				<br /><br />
				<table border='1'>
				<tr><td colspan='6' class='ch1'>Transmittal Details</td></tr>
				<tr>
					<td class='ch2'>Transmittal No.</td><td class='ch2'>:</td><td>" + transmittal.TransNo + @"</td>
					<td class='ch2'>Description</td><td class='ch2'>:</td><td>" + transmittal.Description + @"</td>
				</tr>
				<tr>
					<td class='ch2'>From</td><td class='ch2'>:</td><td>" + transmittal.OriginatingOrganizationName + @"</td>
					<td class='ch2'>To</td><td class='ch2'>:</td><td>" + transmittal.ReceivingOrganizationName + @"</td>
				</tr>
				<tr>
					<td class='ch2'>Issued Date</td><td class='ch2'>:</td><td>" + transmittal.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
					<td class='ch2'>CC</td><td class='ch2'>:</td><td>" + transmittal.CCOrganizationName + @"</td>
				</tr>
				<tr>
					<td class='ch2'>Sent Date</td><td class='ch2'>:</td><td>" + DateTime.Now.ToString("dd/MM/yyyy") + @"</td>
					<td class='ch2'>Action Code</td><td class='ch2'>:</td><td>" + transmittal.PurposeName + @"</td>
				</tr>
				<tr><td colspan='6' class='ch1'>Transmitted Documents</td></tr>
				<tr>
					<td colspan='2' class='ch2'>Document No.</td>
					<td class='ch2'>Rev.</td>
					<td colspan='3' class='ch2'>Title</td>
				</tr>";

					var Listdoc = this.contractorTransmittalDocFileService.GetAllByTrans(transmittal.ID).OrderBy(t=> t.DocumentNo);
					List<string> ListDoc = new List<string>();
					foreach (var document in Listdoc)
					{
						if (!ListDoc.Where(t => t == document.DocumentNo).Any())
						{
							ListDoc.Add(document.DocumentNo);
							bodyContent += @"<tr>
							   <td colspan='2'>" + document.DocumentNo + @"</td>

							   <td>"
										   + document.Revision + @"</td>
							   <td colspan='3'>"
										   + document.DocumentTitle + @"</td></tr>";
						}
					}
					var st = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx?TransNoContractor=" + transmittal.TransNo;
					var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx";
					bodyContent += @"</table>
						<div class='link'>
						<br />
						<u><b>Useful Links:</b></u>
						<ul>
							<li>
								Click <a href='" + st + @"'>here</a> to show <u>this transmittal</u> in DMDC System
							</li>
							<li>
								Click <a href= '" + st1 + @"' > here</a> to show <u>all transmittals</u> in DMDC System
							</li>
						</ul>
						</div>
						<br />
						<h2 class='company'>Power Engineering Consulting JSC 2 (PECC2)</h2>
						<hr />
						<h3 class='system'>DMDC System</h3>
						<br />
						<span class='footer'>[THIS IS SYSTEM AUTO-GENERATED NOTIFICATION]</span></body>";
					message.Body = bodyContent;

					var Userlist = userListid.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
					foreach (var user in Userlist)
					{
						try
						{
							message.CC.Add(new MailAddress(user.Email));
						}
						catch { }

					}
					var UsserCC = this.userService.GetAllByRoleId(this.roleService.GetByContractor(transmittal.OriginatingOrganizationId.GetValueOrDefault()).Id);
					var listCC = UsserCC.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
					foreach (var user in listCC)
					{
						try
						{
							message.To.Add(new MailAddress(user.Email));
						}
						catch { }

					}
					smtpClient.Send(message);
				}
			}
			catch { }
		}
	}
}