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
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Hosting;


namespace EDMs.Web.Controls.Document
{
	/// <summary>
	/// Class customer
	/// </summary>
	public partial class ContractorTransmittalList : Page
	{
		private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

		private readonly ContractorTransmittalService contractorTransmittalService = new ContractorTransmittalService();

		private readonly ContractorTransmittalDocFileService contractorTransmittalDocFileService = new ContractorTransmittalDocFileService();

		private readonly PECC2TransmittalService pecc2TransmittalService = new PECC2TransmittalService();

		private readonly PECC2DocumentsService documentService = new PECC2DocumentsService();

		private readonly OrganizationCodeService organizationCodeService = new OrganizationCodeService();
        private readonly DocumentCodeServices documentCodeServices = new DocumentCodeServices();
        private readonly AttachDocToTransmittalService attachDocToTransmittalService = new AttachDocToTransmittalService();
		private readonly ContractorTransmittalAttachFileService contractorTransmittalAttachFileService= new ContractorTransmittalAttachFileService();
		private readonly HashSet<DateTime> holidays = new HashSet<DateTime>();
		private readonly UserService userService = new UserService();
		private readonly RoleService roleService = new RoleService();
		private readonly HolidayConfigService holidayConfigService= new HolidayConfigService();
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
			Session.Add("SelectedMainMenu", "Contractor Transmittals Management");

			this.Title = ConfigurationManager.AppSettings.Get("AppName");
			this.ServerName.Value = ConfigurationManager.AppSettings.Get("ServerName");
			var temp = (RadPane)this.Master.FindControl("leftPane");
			temp.Collapsed = true;
            if (!Page.IsPostBack)
            {
                this.LoadComboData();

                if (!string.IsNullOrEmpty(this.Request.QueryString["TransNoContractor"]))
                {
                    var txtSearchOutgoing = (TextBox)this.radMenuOutgoing.Items[2].FindControl("txtSearchOutgoing");
                    txtSearchOutgoing.Text = Request.QueryString["TransNoContractor"];
                    var ddlStatusOutgoing = (DropDownList)this.radMenuOutgoing.Items[2].FindControl("ddlStatusOutgoing");
                    if (ddlStatusOutgoing != null)
                    {
                        ddlStatusOutgoing.SelectedIndex = 0;
                    }
                    this.OutgoingTransView.Selected = true;
                    this.OutgoingTransView.Selected = true;
                    RadTab tab1 = RadTabStrip1.Tabs.FindTabByText("Outgoing Transmittals");
                    tab1.Selected = true;
                }
                if (!string.IsNullOrEmpty(this.Request.QueryString["TransNoPecc2"]))
                {
                    var txtSearchIncoming = (TextBox)this.radMenuIncoming.Items[1].FindControl("txtSearchIncoming");
                    txtSearchIncoming.Text = Request.QueryString["TransNoPecc2"];
                }

                var holidayList = this.holidayConfigService.GetAll();
                foreach (var holidayConfig in holidayList)
                {
                    for (DateTime i = holidayConfig.FromDate.GetValueOrDefault(); i < holidayConfig.ToDate.GetValueOrDefault(); i = i.AddDays(1))
                    {
                        this.holidays.Add(i);
                    }
                }

                if (UserSession.Current.User.IsEngineer.GetValueOrDefault() || UserSession.Current.User.IsLeader.GetValueOrDefault())
                {
                    this.radMenuOutgoing.Items[0].Visible = false;
                    this.grdOutgoingTrans.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdOutgoingTrans.MasterTableView.GetColumn("DeleteColumn").Visible = false;
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
			if(ddlStatusOutgoing != null){
				ddlStatusOutgoing.SelectedIndex = 3;
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
			if (e.Argument.Contains("ExportContractorETRM"))
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
				var transObj = this.pecc2TransmittalService.GetById(objId);
				if (transObj != null)
				{
					this.ExportPECC2ETRM(transObj);
				}
			}
			else if (e.Argument.Contains("DeleteTrans"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var transObj = this.contractorTransmittalService.GetById(objId);
				if (transObj != null)
				{
					if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
					{
						var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
						if (Directory.Exists(folderPath))
						{
							Directory.Delete(folderPath, true);
						}
					}

					this.contractorTransmittalService.Delete(objId);
					this.grdOutgoingTrans.Rebind();
				}
			}
			else if (e.Argument.Contains("SendTrans"))
			{
				var objId = new Guid(e.Argument.Split('_')[1]);
				var forSend = Convert.ToInt32(e.Argument.Split('_')[2]);

				var transObj = this.contractorTransmittalService.GetById(objId);
				if (transObj != null && transObj.IsSend == false)
				{
                    transObj.TransDate = DateTime.Now.Date;
                    transObj.ConsultantDeadline = GetDate(6, transObj.TransDate.GetValueOrDefault());
                    transObj.DueDate = transObj.ConsultantDeadline;
                    transObj.OwnerDeadline = GetDate(7, transObj.TransDate.GetValueOrDefault());
                    this.contractorTransmittalService.Update(transObj);
                    if (!string.IsNullOrEmpty(transObj.RefTransNo) &&
						!this.contractorTransmittalAttachFileService.GetByTrans(transObj.ID).Where(t => t.TypeId == 2).Any())
					{
						this.RadWindowManager1.RadAlert("Please attach CRS file!",300,50,"Warring","");
					}
					else
					{

						if (transObj.IsReject.GetValueOrDefault())
						{
							var pecc2Trans = this.pecc2TransmittalService.GetById(transObj.PECC2TransId.GetValueOrDefault());
							if (pecc2Trans != null)
							{
								// Update pecc2 transmittal
								pecc2Trans.IsOpen = false;
								pecc2Trans.IsImport = false;
								pecc2Trans.Status = "Waiting for DC review";
								pecc2Trans.ErrorMessage = "Waiting for DC review";
								pecc2Trans.IsReject = false;
								pecc2Trans.CurrentRejectReason = string.Empty;
								pecc2Trans.TransmittalNo = transObj.TransNo;
								pecc2Trans.ProjectCodeId = transObj.ProjectId;
								pecc2Trans.ProjectCodeName = transObj.ProjectName;
								pecc2Trans.IssuedDate = transObj.TransDate;
                                pecc2Trans.Priority = transObj.Priority;                               
								pecc2Trans.Description = transObj.Description;
								pecc2Trans.OriginatingOrganizationId = transObj.OriginatingOrganizationId;
								pecc2Trans.OriginatingOrganizationName = transObj.OriginatingOrganizationName;
								pecc2Trans.ReceivingOrganizationId = transObj.ReceivingOrganizationId;
								pecc2Trans.ReceivingOrganizationName = transObj.ReceivingOrganizationName;
								pecc2Trans.FromValue = transObj.FromValue;
								pecc2Trans.ToValue = transObj.ToValue;
								pecc2Trans.CCValue = transObj.CCValue;
								pecc2Trans.GroupId = transObj.GroupId;
								pecc2Trans.GroupCode = transObj.GroupCode;
								pecc2Trans.TransmittedByName = transObj.TransmittedByName;
								pecc2Trans.TransmittedByDesignation = transObj.TransmittedByDesignation;
								pecc2Trans.AcknowledgedByName = transObj.AcknowledgedByName;
								pecc2Trans.AcknowledgedByDesignation = transObj.AcknowledgedByDesignation;
								pecc2Trans.Remark = transObj.Remark;
								pecc2Trans.Year = transObj.Year;
								pecc2Trans.DueDate = transObj.DueDate;
								pecc2Trans.ReceivedDate = DateTime.Now;
								pecc2Trans.IsAllDocCompleteWorkflow = false;
								pecc2Trans.CCOrganizationId = transObj.CCOrganizationId;
								pecc2Trans.CCOrganizationName = transObj.CCOrganizationName;
								pecc2Trans.PurposeId = transObj.PurposeId;
								pecc2Trans.PurposeName = transObj.PurposeName;
								pecc2Trans.IsFirstTrans = transObj.RefTransId == null;
								pecc2Trans.CreatedDate = DateTime.Now;
                                pecc2Trans.VolumeNumber = transObj.VolumeNumber;

                                pecc2Trans.CategoryId = transObj.CategoryId;
                                pecc2Trans.CategoryName = transObj.CategoryName;
                                this.pecc2TransmittalService.Update(pecc2Trans);
								// -------------------------------------------------

								// Update Contractor Trans
								transObj.IsSend = true;
								transObj.IsReject = false;
								transObj.Status = string.Empty;
								transObj.ErrorMessage = string.Empty;
								this.contractorTransmittalService.Update(transObj);
								// -------------------------------------------------

							}
						}
						else
						{
							var pecc2Trans = new PECC2Transmittal();
							pecc2Trans.ID = Guid.NewGuid();
							pecc2Trans.ContractorTransId = transObj.ID;
							pecc2Trans.IsOpen = false;
							pecc2Trans.IsImport = false;
							pecc2Trans.Status = "Waiting for DC review";
							pecc2Trans.ErrorMessage = "Waiting for DC review";
							pecc2Trans.TypeId = 1;
							pecc2Trans.ForSentId = forSend;
							pecc2Trans.ForSentName = forSend == 1 ? "Project's Document" : "Change Request";                         
							pecc2Trans.TransmittalNo = transObj.TransNo;
							pecc2Trans.ProjectCodeId = transObj.ProjectId;
							pecc2Trans.ProjectCodeName = transObj.ProjectName;
							pecc2Trans.IssuedDate = transObj.TransDate;
							pecc2Trans.Description = transObj.Description;
							pecc2Trans.OriginatingOrganizationId = transObj.OriginatingOrganizationId;
							pecc2Trans.OriginatingOrganizationName = transObj.OriginatingOrganizationName;
							pecc2Trans.ReceivingOrganizationId = transObj.ReceivingOrganizationId;
							pecc2Trans.ReceivingOrganizationName = transObj.ReceivingOrganizationName;
							pecc2Trans.FromValue = transObj.FromValue;
							pecc2Trans.ToValue = transObj.ToValue;
							pecc2Trans.CCValue = transObj.CCValue;
							pecc2Trans.GroupId = transObj.GroupId;
							pecc2Trans.GroupCode = transObj.GroupCode;
							pecc2Trans.TransmittedByName = transObj.TransmittedByName;
							pecc2Trans.TransmittedByDesignation = transObj.TransmittedByDesignation;
							pecc2Trans.AcknowledgedByName = transObj.AcknowledgedByName;
							pecc2Trans.AcknowledgedByDesignation = transObj.AcknowledgedByDesignation;
							pecc2Trans.Remark = transObj.Remark;
							pecc2Trans.Year = transObj.Year;
							pecc2Trans.DueDate = transObj.DueDate;
							pecc2Trans.ReceivedDate = DateTime.Now;
							pecc2Trans.IsAllDocCompleteWorkflow = false;
							pecc2Trans.CCOrganizationId = transObj.CCOrganizationId;
							pecc2Trans.CCOrganizationName = transObj.CCOrganizationName;
							pecc2Trans.PurposeId = transObj.PurposeId;
							pecc2Trans.PurposeName = transObj.PurposeName;
							pecc2Trans.IsReject = false;
							pecc2Trans.CurrentRejectReason = string.Empty;
							pecc2Trans.IsFirstTrans = transObj.RefTransId == null;
							pecc2Trans.CreatedDate = DateTime.Now;
							pecc2Trans.StoreFolderPathContractor = transObj.StoreFolderPath;
                            pecc2Trans.Priority = transObj.Priority;
                            pecc2Trans.VolumeNumber = transObj.VolumeNumber;

                            pecc2Trans.CategoryId = transObj.CategoryId;
                            pecc2Trans.CategoryName = transObj.CategoryName;
							var pecc2TransId = this.pecc2TransmittalService.Insert(pecc2Trans);


							if (pecc2TransId != null)
							{
								// Create store folder
								var physicalStoreFolder = Server.MapPath("../../DocumentLibrary/PECC2Transmittal/" + pecc2Trans.TransmittalNo + "_" + pecc2Trans.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss"));
								Directory.CreateDirectory(physicalStoreFolder);
								Directory.CreateDirectory(physicalStoreFolder + @"\eTRM File");


								var serverStoreFolder = (HostingEnvironment.ApplicationVirtualPath == "/"
									? string.Empty
									: HostingEnvironment.ApplicationVirtualPath) + "/DocumentLibrary/PECC2Transmittal/" + pecc2Trans.TransmittalNo + "_" + pecc2Trans.CreatedDate.GetValueOrDefault().ToString("ddMMyyyyHHmmss");
								pecc2Trans.StoreFolderPath = serverStoreFolder;
								this.pecc2TransmittalService.Update(pecc2Trans);
								// --------------------------------------------------------------------------

								transObj.PECC2TransId = pecc2TransId;
								transObj.IsSend = true;

								this.contractorTransmittalService.Update(transObj);
							}
						}

						if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
						{
                            // this.ContrctorSendemailtest(transObj);
                            this.ContrctorSendemail(transObj);
						}
						this.grdOutgoingTrans.Rebind();
					}
				}
			}
			else if(e.Argument== "RebinOutrans")
			{
				this.grdOutgoingTrans.Rebind();
			}
			else if(e.Argument.Contains("AcknowledgedTrans_"))
			{
				//var objId = new Guid(e.Argument.Split('_')[1]);
				//var transObj = this.contractorTransmittalService.GetById(objId);
				//  if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]) && transObj.IsOpen!= null && !transObj.IsOpen.GetValueOrDefault() && !transObj.IsValid.GetValueOrDefault())
				//  {
				//			 this.NotifiAcknowledgedTransmittal(transObj);
			 //  }
			}
		}

		private void ExportPECC2ETRM(PECC2Transmittal transObj)
		{
			var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transObj.ID);
			if (attachDocToTrans != null)
			{
				var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
				//var filePath = Server.MapPath("Exports") + @"\";
				var workbook = new Workbook();
				workbook.Open(filePath + @"Template\PECC2TransmittalTemplate.xlsm");

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
					new DataColumn("Empty8", typeof(String)),
				});
				var listid = attachDocToTrans.Select(t => t.DocumentId.GetValueOrDefault()).ToList();

				var documentObjList = this.documentService.GetAllDocList(listid);
				foreach (var documentObj in documentObjList.OrderBy(t=> t.DocNo))
				{
					var dataRow = dtFull.NewRow();
				   
					dataRow["DocumentNo"] = documentObj.DocNo;
					dataRow["Revision"] = documentObj.Revision;
					dataRow["DocumentTitle"] = documentObj.DocTitle;
					dtFull.Rows.Add(dataRow);
				}

				var projectObj = this.projectCodeService.GetById(transObj.ProjectCodeId.GetValueOrDefault());

				var filename = transObj.TransmittalNo + "_Trans_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
				dataSheet.Cells["J5"].PutValue(transObj.TransmittalNo);
				dataSheet.Cells["J6"].PutValue(DateTime.Now.ToString("dd-MMM-yy"));
				dataSheet.Cells["B5"].PutValue(projectObj.FullName);
				dataSheet.Cells["B7"].PutValue(transObj.Description);
				dataSheet.Cells["B8"].PutValue(transObj.FromValue);
				dataSheet.Cells["B10"].PutValue(transObj.ToValue);
				dataSheet.Cells["B12"].PutValue(transObj.CCValue);

				dataSheet.Cells.ImportDataTable(dtFull, false, 20, 0, dtFull.Rows.Count, dtFull.Columns.Count, true);

				for (int i = 0; i < dtFull.Rows.Count; i++)
				{
					dataSheet.Cells.Merge(20 + i, 0, 1, 3);
					dataSheet.Cells.Merge(20 + i, 4, 1, 7);
				}

				workbook.Save(filePath + filename);
				this.Download_File(filePath + filename);
			}
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
			var attachDocFullList = this.contractorTransmittalDocFileService.GetAllByTrans(transObj.ID).OrderByDescending(t => t.TypeId);
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
			workbook.Open(filePath + @"Template\PECC2_ContractorTransNewTemplate.xlsm");
			var workSheets = workbook.Worksheets;
			var transSheet = workSheets[0];
			//var fileListSheet = workSheets[9];
			// Export trans Info
			var dtFull = new DataTable();
			dtFull.Columns.AddRange(new[]
			{
				new DataColumn("DocNo", typeof(String)),
				new DataColumn("1Empty", typeof(String)),
				new DataColumn("2Empty", typeof(String)),
				new DataColumn("Revision", typeof(string)),
				new DataColumn("ActionCode", typeof(String)),
				new DataColumn("DocTitle", typeof(String)),
				new DataColumn("3Empty", typeof(String)),
				new DataColumn("4Empty", typeof(String)),
				new DataColumn("RevRemark", typeof(String)),
				new DataColumn("5Empty", typeof(String)),
			});

			var count = 1;
			foreach (var doc in attachDocFileFilter.OrderBy(t=> t.DocumentNo))
			{
				var dataRow = dtFull.NewRow();
				dataRow["DocNo"] = doc.DocumentNo;
				dataRow["DocTitle"] = doc.DocumentTitle;
				dataRow["Revision"] = doc.Revision;
				dataRow["ActionCode"] = doc.PurposeName.Split(',')[0];
				dataRow["RevRemark"] = doc.RevRemark;
				dtFull.Rows.Add(dataRow);
				count ++;
			}

			transSheet.Cells.ImportDataTable(dtFull, false, 12, 1, dtFull.Rows.Count, dtFull.Columns.Count, true);
			for (int i = 0; i < attachDocFileFilter.Count; i++)
			{
				transSheet.Cells.Merge(12 + i, 1, 1, 3);
				transSheet.Cells.Merge(12 + i, 6, 1, 3);
				transSheet.Cells.Merge(12 + i, 9, 1, 2);
			}



			//transSheet.Cells.DeleteRow(19 + attachDocFileFilter.Count);
			var organisationObj = this.organizationCodeService.GetById(transObj.OriginatingOrganizationId.GetValueOrDefault());
			var projectObj = this.projectCodeService.GetById(transObj.ProjectId.GetValueOrDefault());
			transSheet.Cells["D4"].PutValue(projectObj.FullName);
			transSheet.Cells["I3"].PutValue(transObj.TransNo);
			transSheet.Cells["H8"].PutValue(projectObj.Code);
			transSheet.Cells["H4"].PutValue(transObj.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
			//transSheet.Cells["C3"].PutValue(transObj.OriginatingOrganizationName);
			//if (organisationObj != null)
			//{
			//    transSheet.Cells["C4"].PutValue(organisationObj.HeadOffice);
			//    transSheet.Cells["C5"].PutValue(organisationObj.Phone);
			//    transSheet.Cells["C6"].PutValue(organisationObj.Fax);
			//}
			
			// ---------------------------------------------------------------------
			
			// Fill Signed
			//if (!string.IsNullOrEmpty(UserSession.Current.User.SignImageUrl))
			//{
			//    transSheet.Pictures.Add(13 + dtFull.Rows.Count, 7, Server.MapPath("../.." + UserSession.Current.User.SignImageUrl));
			//}
			// ---------------------------------------------------------------------
			var options = new AutoFitterOptions();
			options.AutoFitMergedCells = true;
			transSheet.AutoFitRows(options);
			transSheet.Cells.DeleteRow(12 + dtFull.Rows.Count);
			var savePath = Server.MapPath("../.." + transObj.StoreFolderPath) + "\\eTRM File\\";
			var fileName = transObj.TransNo + "_Transmittal Cover Sheet.xlsm";
			workbook.Save(savePath + fileName);

			this.Download_File(savePath + fileName);
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
			else if (e.CommandName == RadGrid.ExportToExcelCommandName)
			{

			}
		}
		
		protected void ddlProjectOutgoing_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			var ddlProjectOutgoing = (RadComboBox)this.radMenuOutgoing.Items[2].FindControl("ddlProjectOutgoing");
			int projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);
			this.lblProjectOutgoingId.Value = projectId.ToString();
			this.grdOutgoingTrans.Rebind();

			Session.Add("SelectedProject", projectId);
		}

		protected void ddlProjectOutgoing_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
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
			var outgoingTransList = new List<ContractorTransmittal>();

			if (ddlProjectOutgoing != null && ddlProjectOutgoing.SelectedItem != null)
			{
				var projectId = Convert.ToInt32(ddlProjectOutgoing.SelectedValue);

				if (UserSession.Current.User.RoleId == 1
					|| UserSession.Current.User.IsAdmin.GetValueOrDefault())
				{
					outgoingTransList = this.contractorTransmittalService.GetAllByProject(projectId, 2, txtSearchOutgoing.Text.Trim()).OrderByDescending(t => t.TransNo).ToList();
				}
				else
				{
					if (UserSession.Current.User.Role.ContractorId != null)
					{
						outgoingTransList = this.contractorTransmittalService.GetAllByProject(projectId, 2, txtSearchOutgoing.Text.Trim())
							.Where(t => t.OriginatingOrganizationId.GetValueOrDefault() == UserSession.Current.User.Role.ContractorId)
							.OrderByDescending(t => t.TransNo).ToList();
					}
				}

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

			this.grdOutgoingTrans.DataSource = outgoingTransList.OrderByDescending(t=> t.CreatedDate.GetValueOrDefault());
		}

		protected void ddlStatusOutgoing_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			this.grdOutgoingTrans.Rebind();
		}

		protected void btnSearchOutgoing_Click(object sender, EventArgs e)
		{
			this.grdOutgoingTrans.Rebind();
		}

		protected void grdOutgoingTrans_OnDeleteCommand(object sender, GridCommandEventArgs e)
		{
			var item = (GridDataItem)e.Item;
			var objId = new Guid(item.GetDataKeyValue("ID").ToString());
			var transObj = this.contractorTransmittalService.GetById(objId);
			if (!string.IsNullOrEmpty(transObj?.StoreFolderPath))
			{
				var folderPath = Server.MapPath("../.." + transObj.StoreFolderPath);
				if (Directory.Exists(folderPath))
				{
					Directory.Delete(folderPath);
				}
			}

			this.contractorTransmittalService.Delete(objId);
			this.grdOutgoingTrans.Rebind();
		}

		protected void btnSearchIncoming_Click(object sender, EventArgs e)
		{
			this.grdIncomingTrans.Rebind();
		}

		protected void grdIncomingTrans_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
		{
			var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
			var txtSearchIncoming = (TextBox)this.radMenuIncoming.Items[1].FindControl("txtSearchIncoming");
			var incomingTransList = new List<ContractorTransmittal>();
			if (ddlProjectIncoming != null && ddlProjectIncoming.SelectedItem != null)
			{
				var projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);
				if (UserSession.Current.User.RoleId == 1
					|| UserSession.Current.User.IsAdmin.GetValueOrDefault())
				{
					incomingTransList =
						this.contractorTransmittalService.GetAllByProject(projectId, 1, txtSearchIncoming.Text)
							.OrderByDescending(t => t.TransNo)
							.ToList();
				}
				else
				{
					if (UserSession.Current.User.Role.ContractorId != null)
					{
						incomingTransList =
							this.contractorTransmittalService.GetAllByProject(projectId, 1, txtSearchIncoming.Text)
								.Where(
									t =>
										t.ReceivingOrganizationId.GetValueOrDefault() ==
										UserSession.Current.User.Role.ContractorId)
								.OrderByDescending(t => t.TransNo)
								.ToList();
					}
				}
			}
			this.grdIncomingTrans.DataSource = incomingTransList.OrderByDescending(t=> t.TransDate.GetValueOrDefault());
		}

		protected void ddlProjectIncoming_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
		{
			e.Item.ImageUrl = @"~/Images/project.png";
		}

		protected void ddlProjectIncoming_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
		{
			var ddlProjectIncoming = (RadComboBox)this.radMenuIncoming.Items[1].FindControl("ddlProjectIncoming");
			int projectId = Convert.ToInt32(ddlProjectIncoming.SelectedValue);
			this.lblProjectIncomingId.Value = projectId.ToString();
			this.grdIncomingTrans.Rebind();
		}
        private void ContrctorSendemail(ContractorTransmittal transmittal)
        {
            try
            {
                var customObj = this.ReceivedEmailService.GetByType(1, 2);
                if (transmittal != null && customObj != null)
                {
                    
                    var userListid = this.userService.GetListUser(customObj.ToUserIDs.Split(';').Where(t=> !string.IsNullOrEmpty(t)).Select(t=> Convert.ToInt32(t)).ToList());
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

                    var subject = "FYI: New transmittal submitted, " + transmittal.TransNo + ", " + transmittal.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy") + ", " + transmittal.PurposeName.Split(',')[1] + ", " + transmittal.Description;

                    var message = new MailMessage();
                    message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
                    message.Subject = subject;
                    message.BodyEncoding = new UTF8Encoding();
                    message.IsBodyHtml = true;
                    var emailto = string.Empty;
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
                                    emailto += stemail + "; ";
                                }
                            }
                            else
                            {
                                message.To.Add(new MailAddress(user.Email));
                                emailto += user.Email + "; ";
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
								<br><b>New Transmittal :</b> Transmittal <b>" + transmittal.TransNo + @"</b> has been sent to you for your review
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
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>CC</span></td><td colspan='3'><span class='font_s' style='color:#003399'>
								<a href='mailto:" + emailCC + "'>" + emailCC + @"</a>
							</span></td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal No.</span></td><td colspan='3' class='font_s' style='color:red'>" + transmittal.TransNo + @"</td>
                   
                </tr>
                      <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Transmittal Title</span></td><td class='font_s' colspan='3'>" + transmittal.Description + @"</td>
                   
                </tr>
                <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Issued Date</span></td><td class='font_s'>" + transmittal.TransDate.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Action Code</span></td><td class='font_s' style='color:red'>" + this.documentCodeServices.GetById(transmittal.PurposeId.GetValueOrDefault()).Description + @"</td>
                  </tr>
                   <tr>
                    <td class='ch2' style='width:167px'><span class='font_s' style='color:#003399'>Consultant Deadline</span></td><td class='font_s' style='color:red'>" + transmittal.ConsultantDeadline.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                    <td class='ch2'><span class='font_s' style='color:#003399'>Owner Deadline</span></td><td class='font_s' style='color:red'>" + transmittal.OwnerDeadline.GetValueOrDefault().ToString("dd/MM/yyyy") + @"</td>
                  </tr>
                  </table>
                  </div>";
               
                    var st = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx?TransNoContractor=" + transmittal.TransNo;
                    var st1 = ConfigurationManager.AppSettings["WebAddress"] + @"/Controls/Document/PECC2TransmittalList.aspx";
                    bodyContent += @"  <p style='margin-bottom:12.0pt'>
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
        private DateTime GetDate(int day, DateTime transdate)
        {
            var actualDeadline = transdate;
            for (int i = 1; i <= day; i++)
            {
                actualDeadline = this.GetNextWorkingDay(actualDeadline);
            }
            return actualDeadline;
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
        private void NotifiAcknowledgedTransmittal(ContractorTransmittal transmittal)
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

					var subject = "[DMDC " + projctobj.Code + "] Transmittal (#Trans#) is acknowledged by " + transmittal.ReceivingOrganizationName;

					var message = new MailMessage();
					message.From = new MailAddress(ConfigurationManager.AppSettings["EmailAccount"], "DMDC System");
					message.Subject = subject.Replace("#Trans#", transmittal.TransNo);
					message.BodyEncoding = new UTF8Encoding();
					message.IsBodyHtml = true;

					var bodyContent = @"<div style=‘text-align: center;’> 
									<span class=‘Apple-tab-span’>Dear All,&nbsp;</span>
										<br />
									<p style=‘text-align: center;’><strong><span style=‘font-size: 18px;’>
								   Please be notified the transmittal #Trans# is acknowledged by (" + transmittal.OriginatingOrganizationName + " at "+DateTime.Now.ToString("dd/MM/yyyy hh:mm")+ @"):</span></strong></p>
									   <br/>
									   
					   <p><b> [THIS IS SYSTEM GENERATED NOTIFICATION PLEASE DO NOT REPLY]</b></p></span>";
					message.Body = bodyContent.Replace("#Trans#", transmittal.TransNo); 

					var Userlist = userListid.Where(t => !string.IsNullOrEmpty(t.Email)).Distinct().ToList();
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
	}
}