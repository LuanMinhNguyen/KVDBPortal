// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.IO;
using Aspose.Cells;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Web.Utilities;
using Telerik.Web.UI.Calendar;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;
    using CheckBox = System.Web.UI.WebControls.CheckBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class PerformanceReview : Page
    {

        private readonly FolderService folderService = new FolderService();

        private readonly GroupDataPermissionService groupDataPermissionService = new GroupDataPermissionService();

        private readonly UserService userService = new UserService();

        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        private readonly TrackingMorningCallService morningCallService = new TrackingMorningCallService();
        private readonly TrackingWCRService wcrService = new TrackingWCRService();
        private readonly TrackingPunchService punchService = new TrackingPunchService();
        private readonly TrackingSailService sailService = new TrackingSailService();
        private readonly TrackingProcedureService procedureService = new TrackingProcedureService();
        private readonly TrackingGeneralWorkingService generalWorkingService = new TrackingGeneralWorkingService();

        private readonly RoleService roleService = new RoleService();

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
            Session.Add("SelectedMainMenu", "Performance Review");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            //var temp = (RadPane)this.Master.FindControl("leftPane");
            //temp.Collapsed = true;
            if (!Page.IsPostBack)
            {
                Session.Remove("PerformanceReviewList");
                this.txtToDate.SelectedDate = new DateTime(DateTime.Now.Year,
                                                               DateTime.Now.Month,
                                                               DateTime.DaysInMonth(DateTime.Now.Year,
                                                                                    DateTime.Now.Month));
                this.txtFromDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); 

                var deptList = new List<Role>();

                if (UserSession.Current.User.Id == 1
                    || UserSession.Current.User.TitleId == 6
                    || UserSession.Current.User.TitleId == 1002
                    )
                {
                    deptList = this.roleService.GetAll(false).Where(t => t.TypeId == 1).ToList();
                    this.ddlDepartment.DataTextField = "FullNameWithLocation";

                }
                else
                {
                    deptList = new List<Role>() {new Role()
                    {
                        Id = UserSession.Current.User.RoleId.GetValueOrDefault(),
                        FullNameWithLocation = UserSession.Current.User.Role.FullNameWithLocation
                    } };
                }

                this.ddlDepartment.DataSource = deptList;
                this.ddlDepartment.DataTextField = "FullNameWithLocation";
                this.ddlDepartment.DataValueField = "Id";
                this.ddlDepartment.DataBind();

                if (this.ddlDepartment.SelectedItem != null)
                {
                    int deptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);

                    var employeeList = new List<User>();

                    if (UserSession.Current.User.Id == 1
                        || UserSession.Current.User.TitleId == 6
                        || UserSession.Current.User.TitleId == 1002
                        || UserSession.Current.User.TitleId == 1003
                        || UserSession.Current.User.TitleId == 1004
                        )
                    {
                        employeeList = this.userService.GetAllByRoleId(deptId).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition).ToList();
                    }
                    else
                    {
                        employeeList = new List<User>()
                        {
                            new User()
                            {
                                Id = UserSession.Current.User.Id,
                                FullNameWithPosition = UserSession.Current.User.FullNameWithPosition
                            }
                        };
                    }

                    this.rtvEmployee.DataSource = employeeList;
                    this.rtvEmployee.DataTextField = "FullNameWithPosition";
                    this.rtvEmployee.DataValueField = "Id";
                    this.rtvEmployee.DataFieldID = "Id";
                    this.rtvEmployee.DataBind();

                    if (employeeList.Any())
                    {
                        this.rtvEmployee.Nodes[0].Selected = true;
                        this.lblUserId.Value = this.rtvEmployee.Nodes[0].Value;
                    }
                }
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
            else if (e.Argument == "ExportExcel")
            {
                if (this.rtvEmployee.SelectedNode != null && Session["PerformanceReviewList"] != null)
                {
                    var selectedUser = this.userService.GetByID(Convert.ToInt32(this.rtvEmployee.SelectedNode.Value));
                    if (selectedUser != null)
                    {
                        var filePath = Server.MapPath("~/Exports") + @"\";
                        var workbook = new Workbook();
                        workbook.Open(filePath + @"Template\PerformanceReviewTemplate.xlsm");

                        var dataSheet = workbook.Worksheets[0];
                        dataSheet.Cells["A1"].PutValue("PHIẾU GIAO VIỆC VÀ ĐÁNH GIÁ MỨC ĐỘ HOÀN THÀNH CÔNG VIỆC THÁNG " + this.txtFromDate.SelectedDate.GetValueOrDefault().Month + " NĂM " + this.txtFromDate.SelectedDate.GetValueOrDefault().Year);
                        dataSheet.Cells["A2"].PutValue("Họ & tên Người được đánh giá: " + selectedUser.FullName);
                        dataSheet.Cells["E2"].PutValue("Chức danh công việc: " + selectedUser.TitleName);
                        dataSheet.Cells["M2"].PutValue("Phòng: " + selectedUser.Role.Name);
                        dataSheet.Cells["A3"].PutValue("I - Công việc từ ngày " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + " đến ngày " + this.txtToDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy"));

                        dataSheet.Cells["B14"].PutValue("Tổng hợp điểm đánh giá tháng " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM/yyyy") + ":");
                        dataSheet.Cells["B15"].PutValue("Điểm đánh giá tháng " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM/yyyy") + " làm tròn:");
                        dataSheet.Cells["B16"].PutValue("Loại đánh giá MĐHTCV tháng " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM/yyyy") + ":");


                        var performanceList = (List<PerformanceReviewObj>)Session["PerformanceReviewList"];
                        for (int i = 0; i < performanceList.Count; i++)
                        {
                            dataSheet.Cells["B" + (7 + i)].PutValue(performanceList[i].Title);
                            dataSheet.Cells["C" + (7 + i)].PutValue(performanceList[i].TotalObj);
                            dataSheet.Cells["D" + (7 + i)].PutValue(performanceList[i].CompletedObj);
                            dataSheet.Cells["E" + (7 + i)].PutValue(performanceList[i].OverdueObj);
                            dataSheet.Cells["F" + (7 + i)].PutValue(performanceList[i].IncompleteObj);
                        }

                        var filename = "PerformanceReview_" + selectedUser.Username + "_" + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM-yyyy") + ".xlsm";
                        workbook.Save(filePath + filename);
                        this.Download_File(filePath + filename);
                    }
                }
            }
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
            var performanceList = new List<PerformanceReviewObj>();
            if (this.rtvEmployee.SelectedNode != null)
            {
                var userId = Convert.ToInt32(this.rtvEmployee.SelectedValue);
                var fromDate = this.txtFromDate.SelectedDate;
                var toDate = this.txtToDate.SelectedDate;
                /*
                Breakdown Report
                ECR
                Material Requisition
                MOC
                Shutdown Report
                Work Request
                */

                // Collect MR jobs
                var mrJobList = this.objAssignedUserService.GetAllByUser(userId, "Material Requisition")
                                                            .Where(t => (fromDate == null || t.PlanCompleteDate.GetValueOrDefault() >= fromDate)
                                                                        && (toDate == null || t.PlanCompleteDate.GetValueOrDefault() < toDate.Value.AddDays(1))).ToList();

                var mrJobOverdueList = mrJobList.Where(t => t.IsOverDue.GetValueOrDefault()
                                                            || (!t.IsComplete.GetValueOrDefault()
                                                                && t.PlanCompleteDate != null
                                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))).ToList();
                var mrJobCompletedList = mrJobList.Where(t => t.IsComplete.GetValueOrDefault()).ToList();
                var mrJobIncompleteList = mrJobList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
                var mrPerformance = new PerformanceReviewObj()
                {
                    ID = 1,
                    Title = "Material Requisition",
                    TotalObj = mrJobList.Count,
                    OverdueObj = mrJobOverdueList.Count,
                    CompletedObj = mrJobCompletedList.Count,
                    IncompleteObj = mrJobIncompleteList.Count,

                };

                performanceList.Add(mrPerformance);
                // ----------------------------------------------------------------------------------

                // Collect WR jobs
                var wrJobList = this.objAssignedUserService.GetAllByUser(userId, "Work Request")
                                                            .Where(t => (fromDate == null || t.PlanCompleteDate.GetValueOrDefault() >= fromDate)
                                                                        && (toDate == null || t.PlanCompleteDate.GetValueOrDefault() < toDate.Value.AddDays(1))).ToList();

                var wrJobOverdueList = wrJobList.Where(t => t.IsOverDue.GetValueOrDefault()
                                                            || (!t.IsComplete.GetValueOrDefault()
                                                                && t.PlanCompleteDate != null
                                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))).ToList();
                var wrJobCompletedList = wrJobList.Where(t => t.IsComplete.GetValueOrDefault()).ToList();
                var wrJobIncompleteList = wrJobList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
                var wrPerformance = new PerformanceReviewObj()
                {
                    ID = 1,
                    Title = "Work Request",
                    TotalObj = wrJobList.Count,
                    OverdueObj = wrJobOverdueList.Count,
                    CompletedObj = wrJobCompletedList.Count,
                    IncompleteObj = wrJobIncompleteList.Count,

                };

                performanceList.Add(wrPerformance);
                // ----------------------------------------------------------------------------------

                // Collect MOC jobs
                var mocJobList = this.objAssignedUserService.GetAllByUser(userId, "MOC")
                                                            .Where(t => (fromDate == null || t.PlanCompleteDate.GetValueOrDefault() >= fromDate)
                                                                        && (toDate == null || t.PlanCompleteDate.GetValueOrDefault() < toDate.Value.AddDays(1))).ToList();

                var mocJobOverdueList = mocJobList.Where(t => t.IsOverDue.GetValueOrDefault()
                                                            || (!t.IsComplete.GetValueOrDefault()
                                                                && t.PlanCompleteDate != null
                                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))).ToList();
                var mocJobCompletedList = mocJobList.Where(t => t.IsComplete.GetValueOrDefault()).ToList();
                var mocJobIncompleteList = mocJobList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
                var mocPerformance = new PerformanceReviewObj()
                {
                    ID = 1,
                    Title = "MOC",
                    TotalObj = mocJobList.Count,
                    OverdueObj = mocJobOverdueList.Count,
                    CompletedObj = mocJobCompletedList.Count,
                    IncompleteObj = mocJobIncompleteList.Count,

                };

                performanceList.Add(mocPerformance);
                // ----------------------------------------------------------------------------------

                // Collect ECR jobs
                var ecrJobList = this.objAssignedUserService.GetAllByUser(userId, "ECR")
                                                            .Where(t => (fromDate == null || t.PlanCompleteDate.GetValueOrDefault() >= fromDate)
                                                                        && (toDate == null || t.PlanCompleteDate.GetValueOrDefault() < toDate.Value.AddDays(1))).ToList();

                var ecrJobOverdueList = ecrJobList.Where(t => t.IsOverDue.GetValueOrDefault()
                                                            || (!t.IsComplete.GetValueOrDefault()
                                                                && t.PlanCompleteDate != null
                                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))).ToList();
                var ecrJobCompletedList = ecrJobList.Where(t => t.IsComplete.GetValueOrDefault()).ToList();
                var ecrJobIncompleteList = ecrJobList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
                var ecrPerformance = new PerformanceReviewObj()
                {
                    ID = 1,
                    Title = "ECR",
                    TotalObj = ecrJobList.Count,
                    OverdueObj = ecrJobOverdueList.Count,
                    CompletedObj = ecrJobCompletedList.Count,
                    IncompleteObj = ecrJobIncompleteList.Count,

                };

                performanceList.Add(ecrPerformance);
                // ----------------------------------------------------------------------------------

                // Collect Breakdown Report jobs
                var brJobList = this.objAssignedUserService.GetAllByUser(userId, "Breakdown Report")
                                                            .Where(t => (fromDate == null || t.PlanCompleteDate.GetValueOrDefault() >= fromDate)
                                                                        && (toDate == null || t.PlanCompleteDate.GetValueOrDefault() < toDate.Value.AddDays(1))).ToList();

                var brJobOverdueList = brJobList.Where(t => t.IsOverDue.GetValueOrDefault()
                                                            || (!t.IsComplete.GetValueOrDefault()
                                                                && t.PlanCompleteDate != null
                                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))).ToList();
                var brJobCompletedList = brJobList.Where(t => t.IsComplete.GetValueOrDefault()).ToList();
                var brJobIncompleteList = brJobList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
                var brPerformance = new PerformanceReviewObj()
                {
                    ID = 1,
                    Title = "Breakdown Report",
                    TotalObj = brJobList.Count,
                    OverdueObj = brJobOverdueList.Count,
                    CompletedObj = brJobCompletedList.Count,
                    IncompleteObj = brJobIncompleteList.Count,

                };

                performanceList.Add(brPerformance);
                // ----------------------------------------------------------------------------------

                // Collect Shutdown Report jobs
                var srJobList = this.objAssignedUserService.GetAllByUser(userId, "Shutdown Report")
                                                            .Where(t => (fromDate == null || t.PlanCompleteDate.GetValueOrDefault() >= fromDate)
                                                                        && (toDate == null || t.PlanCompleteDate.GetValueOrDefault() < toDate.Value.AddDays(1))).ToList();

                var srJobOverdueList = srJobList.Where(t => t.IsOverDue.GetValueOrDefault()
                                                            || (!t.IsComplete.GetValueOrDefault()
                                                                && t.PlanCompleteDate != null
                                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))).ToList();
                var srJobCompletedList = srJobList.Where(t => t.IsComplete.GetValueOrDefault()).ToList();
                var srJobIncompleteList = srJobList.Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
                var srPerformance = new PerformanceReviewObj()
                {
                    ID = 1,
                    Title = "Shutdown Report",
                    TotalObj = srJobList.Count,
                    OverdueObj = srJobOverdueList.Count,
                    CompletedObj = srJobCompletedList.Count,
                    IncompleteObj = srJobIncompleteList.Count,

                };

                performanceList.Add(srPerformance);
                // ----------------------------------------------------------------------------------
            }


            this.grdDocument.DataSource = performanceList;
            Session.Add("PerformanceReviewList", performanceList);
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

            //if (e.Item is GridDataItem)
            //{
            //    var item = e.Item as GridDataItem;
            //    if (item["ActionTypeId"].Text == "1")
            //    {
            //        item["ActionTypeName"].BackColor = Color.Coral;
            //        item["ActionTypeName"].BorderColor = Color.Coral;
            //    }
            //    else if (item["ActionTypeId"].Text == "2")
            //    {
            //        item["ActionTypeName"].BackColor = Color.Aqua;
            //        item["ActionTypeName"].BorderColor = Color.Aqua;
            //    }
            //}
        }

        protected void radTreeFolder_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSideCallBack);
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

        private void PopulateNodeOnDemand(RadTreeNodeEventArgs e, TreeNodeExpandMode expandMode)
        {
            var categoryId = this.lblCategoryId.Value;
            var folderPermission =
                this.groupDataPermissionService.GetByRoleId(UserSession.Current.User.RoleId.GetValueOrDefault()).Where(
                    t => t.CategoryIdList == categoryId).Select(t => Convert.ToInt32(t.FolderIdList)).ToList();

            var listFolChild = this.folderService.GetAllByParentId(Convert.ToInt32(e.Node.Value), folderPermission);
            foreach (var folderChild in listFolChild)
            {
                var nodeFolder = new RadTreeNode();
                nodeFolder.Text = folderChild.Name;
                nodeFolder.Value = folderChild.ID.ToString();
                nodeFolder.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                nodeFolder.ImageUrl = "Images/folderdir16.png";
                e.Node.Nodes.Add(nodeFolder);
            }

            e.Node.Expanded = true;
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ////this.CustomerMenu.Items[2].Visible = false;
            int deptId = Convert.ToInt32(this.ddlDepartment.SelectedValue);

            var employeeList = this.userService.GetAllByRoleId(deptId).OrderByDescending(t => t.IsDeptManager).ThenBy(t => t.FullNameWithPosition);

            this.rtvEmployee.DataSource = employeeList;
            this.rtvEmployee.DataTextField = "FullNameWithPosition";
            this.rtvEmployee.DataValueField = "ID";
            this.rtvEmployee.DataFieldID = "ID";
            this.rtvEmployee.DataBind();

            if (employeeList.Any())
            {
                this.rtvEmployee.Nodes[0].Selected = true;
                this.lblUserId.Value = this.rtvEmployee.Nodes[0].Value;
            }
            this.grdDocument.Rebind();
        }

        protected void grdDocument_Init(object sender, EventArgs e)
        {
        }

        protected void grdDocument_DataBound(object sender, EventArgs e)
        {
        }

        protected void rtvEmployee_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.lblUserId.Value = this.rtvEmployee.SelectedNode.Value;
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


        protected void rtvEmployee_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            //e.Node.ImageUrl = @"Images/user.png";
        }

        protected void ddlDepartment_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/group.png";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void grdTracking_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
        }

        protected void txtFromDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void btnExportForm_OnClick(object sender, EventArgs e)
        {
            if (this.rtvEmployee.SelectedNode != null && Session["PerformanceReviewList"] != null)
            {
                var selectedUser = this.userService.GetByID(Convert.ToInt32(this.rtvEmployee.SelectedNode.Value));
                if (selectedUser != null)
                {
                    var filePath = Server.MapPath("~/Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\PerformanceReviewTemplate.xlsm");

                    var dataSheet = workbook.Worksheets[0];
                    dataSheet.Cells["A1"].PutValue("PHIẾU GIAO VIỆC VÀ ĐÁNH GIÁ MỨC ĐỘ HOÀN THÀNH CÔNG VIỆC THÁNG "+ this.txtFromDate.SelectedDate.GetValueOrDefault().Month + " NĂM " + this.txtFromDate.SelectedDate.GetValueOrDefault().Year);
                    dataSheet.Cells["A2"].PutValue("Họ & tên Người được đánh giá: " + selectedUser.FullName);
                    dataSheet.Cells["E2"].PutValue("Chức danh công việc: " + selectedUser.TitleName);
                    dataSheet.Cells["M2"].PutValue("Phòng: " + selectedUser.Role.Name);
                    dataSheet.Cells["A3"].PutValue("I - Công việc từ ngày "+ this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + " đến ngày " + this.txtToDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy"));

                    dataSheet.Cells["B14"].PutValue("Tổng hợp điểm đánh giá tháng " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM/yyyy") + ":");
                    dataSheet.Cells["B15"].PutValue("Điểm đánh giá tháng " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM/yyyy") + " làm tròn:");
                    dataSheet.Cells["B16"].PutValue("Loại đánh giá MĐHTCV tháng " + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM/yyyy") + ":");


                    var performanceList = (List<PerformanceReviewObj>) Session["PerformanceReviewList"];
                    for (int i = 0; i < performanceList.Count; i++)
                    {
                        dataSheet.Cells["B" + (7 + i)].PutValue(performanceList[i].Title);
                        dataSheet.Cells["C" + (7 + i)].PutValue(performanceList[i].TotalObj);
                        dataSheet.Cells["D" + (7 + i)].PutValue(performanceList[i].CompletedObj);
                        dataSheet.Cells["E" + (7 + i)].PutValue(performanceList[i].OverdueObj);
                        dataSheet.Cells["F" + (7 + i)].PutValue(performanceList[i].IncompleteObj);
                    }

                    var filename = "PerformanceReview_" + selectedUser.Username + "_" + this.txtFromDate.SelectedDate.GetValueOrDefault().ToString("MM-yyyy") + ".xlsm";
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);
                }
            }
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }
    }
}