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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Scope;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.WMS
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class TrackingSailList : Page
    {
        private readonly TrackingSailService trackingSailService = new TrackingSailService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

        private readonly FunctionPermissionService fncPermissionService = new FunctionPermissionService();
        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

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
            Session.Add("SelectedMainMenu", "Tracking Management");

            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.GetFuncPermissionConfig();
                this.LoadComboData();
            }
        }

        private void GetFuncPermissionConfig()
        {
            // Get Function permission of user for MR Module
            var fncPermission = this.fncPermissionService.GetOne(UserSession.Current.User.Id, 11);
            if (fncPermission != null)
            {
                this.IsView.Value = fncPermission.IsView.GetValueOrDefault().ToString();
                this.IsCreate.Value = fncPermission.IsCreate.GetValueOrDefault().ToString();
                this.IsUpdate.Value = fncPermission.IsUpdate.GetValueOrDefault().ToString();
                this.IsCancel.Value = fncPermission.IsCancel.GetValueOrDefault().ToString();
                this.IsAttachWF.Value = fncPermission.IsAttachWorkflow.GetValueOrDefault().ToString();
            }
            else
            {
                this.IsView.Value = "False";
                this.IsCreate.Value = "False";
                this.IsUpdate.Value = "False";
                this.IsCancel.Value = "False";
                this.IsAttachWF.Value = "False";
            }
            // ----------------------------------------------------------------------------------------
        }

        private void LoadComboData()
        {
            var projectInPermission = this.scopeProjectService.GetAll().OrderBy(t => t.Name).ToList();
            if (UserSession.Current.User.LocationId == 2)
            {
                projectInPermission = projectInPermission.Where(t => t.ID == UserSession.Current.User.ProjectId.GetValueOrDefault()).ToList();
            }

            this.ddlProject.DataSource = projectInPermission;
            this.ddlProject.DataTextField = "FullName";
            this.ddlProject.DataValueField = "ID";
            this.ddlProject.DataBind();

            if (Session["SelectedProject"] != null)
            {
                this.ddlProject.SelectedValue = Session["SelectedProject"].ToString();
            }

            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();
                Session.Add("SelectedProject", projectId);
            }

            // Show hide function Control
            this.CustomerMenu.Items[0].Visible = Convert.ToBoolean(this.IsCreate.Value);
            this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = Convert.ToBoolean(this.IsUpdate.Value);
            // -----------------------------------------------------------------------------------------------------------
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
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.MasterTableView.SortExpressions.Clear();
                this.grdDocument.MasterTableView.GroupByExpressions.Clear();
                this.grdDocument.MasterTableView.CurrentPageIndex = this.grdDocument.MasterTableView.PageCount - 1;
                this.grdDocument.Rebind();
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
            if (this.ddlProject.SelectedItem!= null)
            {
                if (this.IsView.Value == "True")
                {
                    //var ddlStatus = (DropDownList)this.CustomerMenu.Items[2].FindControl("ddlStatus");
                    var txtSearchAllField = (TextBox) this.CustomerMenu.Items[2].FindControl("txtSearchAllField");


                    var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                    var objList = this.trackingSailService.GetAllRevTrackingSailOfProject(projectId,
                        txtSearchAllField.Text);
                    //var objList = new List<TrackingSail>();
                    //switch (ddlStatus.SelectedValue)
                    //{
                    //    case "1":
                    //        objList = this.trackingSailService.GetAllRevTrackingSailOfProject(projectId, txtSearchAllField.Text)
                    //                                    .ToList();
                    //        break;
                    //    case "2":
                    //        objList = this.trackingSailService.GetAllTrackingSailOfProject(projectId, txtSearchAllField.Text)
                    //                                    .ToList();
                    //        break;
                    //    case "3":
                    //        objList = this.trackingSailService.GetAllComingDueTrackingSailOfProject(projectId, txtSearchAllField.Text)
                    //                                    .ToList();
                    //        break;
                    //    case "4":
                    //        objList = this.trackingSailService.GetAllOverDueTrackingSailOfProject(projectId, txtSearchAllField.Text)
                    //                                    .ToList();
                    //        break;
                    //    case "5":
                    //        objList = this.trackingSailService.GetAllCompleteTrackingSailOfProject(projectId, txtSearchAllField.Text)
                    //                                    .ToList();
                    //        break;
                    //    case "6":
                    //        objList = this.trackingSailService.GetAllInCompleteTrackingSailOfProject(projectId, txtSearchAllField.Text)
                    //                                    .ToList();
                    //        break;
                    //}

                    this.grdDocument.DataSource = objList;
                }
                else
                {
                    this.grdDocument.DataSource = new List<TrackingSail>();
                }
            }
            else
            {
                this.grdDocument.DataSource = new List<TrackingSail>();
            }
        }

        /// <summary>
        /// Grid KhacHang item created
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (Image)e.Item.FindControl("EditLink");
                editLink.Attributes["href"] = "#";
                editLink.Attributes["onclick"] = string.Format(
                    "return ShowEditForm('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);

                var attachLink = (Image)e.Item.FindControl("AttachmentLink");
                attachLink.Attributes["href"] = "#";
                attachLink.Attributes["onclick"] = string.Format(
                    "return ShowAttachment('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);

                var historyLink = (Image)e.Item.FindControl("HistoryLink");
                historyLink.Attributes["href"] = "#";
                historyLink.Attributes["onclick"] = string.Format(
                    "return ShowHistory('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
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
            var objId = new Guid(item.GetDataKeyValue("ID").ToString());
            this.trackingSailService.Delete(objId);

            this.grdDocument.Rebind();
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
            string abc = e.CommandName;
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
            }
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            Session.Add("SelectedProject", projectId);
            this.grdDocument.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
    }
}

