// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using EDMs.Business.Services.Workflow;
using Telerik.Web.UI;
using System.Drawing;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class WorkflowProcessHistory : Page
    {
        private readonly ObjectAssignedUserService objAssignedUserService = new ObjectAssignedUserService();
        
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
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
                {
                    var objId = new Guid(this.Request.QueryString["objId"]);
                   var current = this.objAssignedUserService.GetAllWorkingHistoryByObj(objId).OrderBy(t => t.CurrentWorkflowStepId).FirstOrDefault();
                    if(current.ObjectTypeId == 2)
                    {
                        this.grdDocument.MasterTableView.GetColumn("Status").Visible = true;
                        this.grdDocument.MasterTableView.GetColumn("ActionTypeName").Visible = false;
                    }
                    
                }
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                this.grdDocument.DataSource = this.objAssignedUserService.GetAllWorkingHistoryByObj(objId).OrderBy(t => t.ReceivedDate);
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            //Is it a GridDataItem
            if (e.Item is GridDataItem)
            {


                var item = e.Item as GridDataItem;
                if (item["ST"].Text == "SO")
                {
                    item["Status"].BackColor =Color.LimeGreen;
                    item["Status"].BorderColor = Color.LimeGreen;
                }
                else if (item["ST"].Text == "RJ")
                {
                    item["Status"].BackColor =Color.Pink;
                    item["Status"].BorderColor = Color.Pink;
                }
                else if (item["ST"].Text == "RS")
                {
                    item["Status"].BackColor = Color.Yellow;
                    item["Status"].BorderColor = Color.Yellow;
                }
                else if (item["ST"].Text == "NR")
                {
                    item["Status"].BackColor = Color.White;
                    item["Status"].BorderColor = Color.White;
                }

                if (item["ActionTypeId"].Text == "2"|| item["ActionTypeId"].Text == "3" || item["ActionTypeId"].Text == "4")
                {
                    item["ActionTypeName"].Text = "Assigned";
                }
                else if (item["ActionTypeId"].Text == "1")
                {
                    item["ActionTypeName"].Text = "Informed";
                }

                ////    //Get the instance of the right type
                ////    GridDataItem dataBoundItem = e.Item as GridDataItem;

                ////    //Check the formatting condition
                ////    if (!string.IsNullOrEmpty(dataBoundItem["PlanCompleteDate"].Text) 
                ////        && DateTime.ParseExact(dataBoundItem["PlanCompleteDate"].Text, "dd/MM/yyyy", null) < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))
                ////    {
                ////        dataBoundItem["IsComplete"].ForeColor = Color.Red;
                ////    }
            }
            }
    }
}