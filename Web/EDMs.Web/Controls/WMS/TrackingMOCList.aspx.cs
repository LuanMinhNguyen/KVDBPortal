﻿// --------------------------------------------------------------------------------------------------------------------
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
    public partial class TrackingMOCList : Page
    {
        private readonly TrackingMOCService trackingMOCService = new TrackingMOCService();

        private readonly ScopeProjectService scopeProjectService = new ScopeProjectService();

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
            Session.Add("SelectedMainMenu", "Other Info Management");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            if (!this.Page.IsPostBack)
            {
                this.LoadComboData();
            }
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

            if (this.ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                this.lblProjectId.Value = projectId.ToString();
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
            else if (e.Argument.Contains("Cancel"))
            {
                var mocId = new Guid(e.Argument.Split('_')[1]);
                var mocObj = this.trackingMOCService.GetById(mocId);
                if (mocObj != null)
                {
                    mocObj.IsCancel = true;

                    this.trackingMOCService.Update(mocObj);
                    this.grdDocument.Rebind();
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
            if (this.ddlProject.SelectedItem!= null)
            {
                var ddlStatus = (DropDownList)this.CustomerMenu.Items[2].FindControl("ddlStatus");
                var txtDeadlineFrom = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtDeadlineFrom");
                var txtDeadlineTo = (RadDatePicker)this.CustomerMenu.Items[2].FindControl("txtDeadlineTo");
                var txtSearchAllField = (TextBox)this.CustomerMenu.Items[2].FindControl("txtSearchAllField");


                var projectId = Convert.ToInt32(this.ddlProject.SelectedValue);
                var objList = new List<TrackingMOC>();
                switch (ddlStatus.SelectedValue)
                {
                    case "1":
                        objList = this.trackingMOCService.GetAllRevTrackingMOCOfProject(projectId, txtSearchAllField.Text)
                                                    .ToList();
                        break;
                    case "2":
                        objList = this.trackingMOCService.GetAllTrackingMOCOfProject(projectId, txtSearchAllField.Text)
                                                    .ToList();
                        break;
                    case "3":
                        objList = this.trackingMOCService.GetAllComingDueTrackingMOCOfProject(projectId, txtSearchAllField.Text)
                                                    .ToList();
                        break;
                    case "4":
                        objList = this.trackingMOCService.GetAllOverDueTrackingMOCOfProject(projectId, txtSearchAllField.Text)
                                                    .ToList();
                        break;
                    case "5":
                        objList = this.trackingMOCService.GetAllCompleteTrackingMOCOfProject(projectId, txtSearchAllField.Text)
                                                    .ToList();
                        break;
                    case "6":
                        objList = this.trackingMOCService.GetAllInCompleteTrackingMOCOfProject(projectId, txtSearchAllField.Text)
                                                    .ToList();
                        break;
                }

                this.grdDocument.DataSource = objList;
            }
            else
            {
                this.grdDocument.DataSource = new List<TrackingMOC>();
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
            this.trackingMOCService.Delete(objId);

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

