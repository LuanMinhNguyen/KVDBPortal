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
using System.Web.UI.WebControls;
using EDMs.Business.Services.Workflow;
using EDMs.Business.Services.Document;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Workflow
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ObjectWorkflowPage : Page
    {
        private readonly WorkflowService wfService = new WorkflowService();
        private readonly ObjectAssignedWorkflowService objAssignedWorkflowService= new ObjectAssignedWorkflowService();
        private readonly ObjectAssignedUserService objAssignedUserService= new ObjectAssignedUserService();
        private readonly DQREDocumentService dqreDocumentService= new DQREDocumentService();

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
                this.DocId.Value = this.Request.QueryString["objId"];
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["objId"]))
            {
                var objId = new Guid(this.Request.QueryString["objId"]);
                var Docobj = this.dqreDocumentService.GetById(objId);
                var listWF = this.objAssignedWorkflowService.GetAllByObj(objId).Select(t => t.WorkflowID.GetValueOrDefault()).Distinct().ToList();

                if (listWF.Count == 0)
                {
                    Docobj.IsInWFProcess = false;
                    Docobj.IsWFComplete = false;
                    Docobj.CurrentWorkflowName = "";
                    Docobj.CurrentWorkflowStepName = "";
                    this.dqreDocumentService.Update(Docobj);
                }

                this.grdDocument.DataSource = this.wfService.GetAllByListWF(listWF).OrderBy(t => t.Name).ToList();
            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }
        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (Image)e.Item.FindControl("EditLink");
                editLink.Attributes["href"] = "#";
                editLink.Attributes["onclick"] = string.Format(
                    "return ShowEditForm('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
            }
        }
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var wfId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var objId = new Guid(this.Request.QueryString["objId"]);
            // this.wfService.Delete(wfId);
            // delete in table objectassignuser
            var listUserWF = this.objAssignedUserService.GetAllListObjWF(wfId, objId);
            foreach (var obj in listUserWF)
            {
                this.objAssignedUserService.Delete(obj);
            }
            //delte in table objectassignWorkflow
            var listassignWF = this.objAssignedWorkflowService.GetAllObjWf(wfId, objId);
            foreach (var obj in listassignWF)
            {
                this.objAssignedWorkflowService.Delete(obj);
            }

            this.grdDocument.Rebind();
        }
        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
            }
        }
        protected void grdDocument_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            //////Is it a GridDataItem
            ////if (e.Item is GridDataItem)
            ////{
            ////    //Get the instance of the right type
            ////    GridDataItem dataBoundItem = e.Item as GridDataItem;

            ////    //Check the formatting condition
            ////    if (!string.IsNullOrEmpty(dataBoundItem["PlanCompleteDate"].Text) 
            ////        && DateTime.ParseExact(dataBoundItem["PlanCompleteDate"].Text, "dd/MM/yyyy", null) < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null))
            ////    {
            ////        dataBoundItem["IsComplete"].ForeColor = Color.Red;
            ////    }
            ////}
        }
    }
}