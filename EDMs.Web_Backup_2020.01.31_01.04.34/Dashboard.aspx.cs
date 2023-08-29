// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.WMS;
using EDMs.Business.Services.Workflow;
using EDMs.Data.Entities;
using Telerik.Web.UI.Calendar;

namespace EDMs.Web
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class Dashboard : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

        private readonly MaterialRequisitionService mrService = new MaterialRequisitionService();
        

        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();

        private readonly PECC2DocumentsService pecc2DocumentsService = new PECC2DocumentsService();
        private readonly DocumentCodeServices documentCodeServices = new DocumentCodeServices();
        private DropDownList ddlProject
        {
            get { return (DropDownList)this.CustomerMenu.Items[6].FindControl("ddlProject"); }
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

            Session.Add("SelectedMainMenu", "Home");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;

            if (!Page.IsPostBack)
            {
                var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtFromDate");
                var txtToDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtToDate");

                txtToDate.SelectedDate = DateTime.Now;
                txtFromDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);

                this.lblToDate.Value = txtToDate.SelectedDate != null ? txtToDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
                this.lblFromDate.Value = txtFromDate.SelectedDate != null ? txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
                Session.Add("FromDate", txtFromDate.SelectedDate);
                Session.Add("ToDate", txtToDate.SelectedDate);


                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[1].Visible = false;
                    this.CustomerMenu.Items[2].Visible = false;
                    this.CustomerMenu.Items[3].Visible = false;
                    this.CustomerMenu.Items[4].Visible = false;
                }

                this.InitData();
                this.LoadProcessReport();
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RefreshProgressReport")
            {
                this.LoadProcessReport();
            }
            
        }

        private void LoadProcessReport()
        {
            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);

                // Binding data for MRChart
                var docList = this.pecc2DocumentsService.GetAllProjectCode(projectId);
                var reviewStatusList = this.documentCodeServices.GetAllReviewStatus();
                //var docPieChart = (PieSeries)this.DocumentChart.PlotArea.Series[0];
                //if (docPieChart != null)
                //{
                //    foreach (var documentCode in reviewStatusList)
                //    {
                //        var temp = new PieSeriesItem(docList.Count(t => t.DocReviewStatusId == documentCode.ID));
                //        docPieChart.SeriesItems.Add(temp);
                //    }
                //}
                // -------------------------------------------------------------------------------------------------------------------------
            }
        }

        private void InitData()
        {
            this.lblCurrentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.lblCurrentDate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.lblCurrentDate2.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.lblCurrentDate3.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.lblCurrentDate4.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.lblCurrentDate5.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.lblCurrentDate6.Text = DateTime.Now.ToString("dd/MM/yyyy");
            var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Name).ToList();
            if (UserSession.Current.User.LocationId == 2)
            {
                projectList = projectList.Where(t => t.ID == UserSession.Current.User.ProjectId.GetValueOrDefault()).ToList();
            }

            //projectList.Insert(0, new ProjectCode() { ID = 0, Name = "For All Project" });
            var project = projectList.ToList();
            this.ddlProject.DataSource = project;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            if (projectList.Count > 1)
            {
                this.ddlProject.SelectedIndex = 1;
            }

            this.ProjectId.Value = this.ddlProject.SelectedValue;
            this.ProjectName.Value = this.ddlProject.SelectedItem.Text;
        }

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }

                //Mo file de doc
                var fs = new FileStream(strFileName, FileMode.Open);
                var streamLength = Convert.ToInt32(fs.Length);
                var data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected void ddlProject_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ProjectId.Value = this.ddlProject.SelectedValue;
            this.ProjectName.Value = this.ddlProject.SelectedItem.Text;
            this.LoadProcessReport();
        }

        protected void ddlRecoveryPlan_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadProcessReport();
        }

        protected void txtFromDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            var txtFromDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtFromDate");
            var txtToDate = (RadDatePicker)this.CustomerMenu.Items[8].FindControl("txtToDate");

            this.lblToDate.Value = txtToDate.SelectedDate != null ? txtToDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
            this.lblFromDate.Value = txtFromDate.SelectedDate != null ? txtFromDate.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") : "null";
            Session.Add("FromDate", txtFromDate.SelectedDate);
            Session.Add("ToDate", txtToDate.SelectedDate);

            this.LoadProcessReport();
        }
    }
}

