// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.CostContract;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.Library;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;
namespace EDMs.Web.Controls.CostContract
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class EquipmentList : Page
    {
        private readonly PermissionService permissionService = new PermissionService();

        private readonly EquipmentService equimentService = new EquipmentService();

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

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

            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;
            if (!this.Page.IsPostBack)
            {
                LoadComboData();
            }
        }

        private void LoadComboData()
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);

            if (ddlProject != null)
            {
                ddlProject.DataSource = projectList;
                ddlProject.DataTextField = "FullName";
                ddlProject.DataValueField = "ID";
                ddlProject.DataBind();

                int projectId = Convert.ToInt32(ddlProject.SelectedValue);
                this.lblProjectId.Value = ddlProject.SelectedValue;
                Session.Add("SelectedProject", projectId);
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
            else if (e.Argument == "RebindAndNavigate")
            {
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
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            if (ddlProject != null)
            {
                var projectId = ddlProject.SelectedItem != null ? Convert.ToInt32(ddlProject.SelectedValue) : 0;
                var projectList = this.equimentService.GetAllByProject(projectId);
                this.grdDocument.DataSource = projectList;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }
        ///// <summary>
        ///// Grid KhacHang item created
        ///// </summary>
        ///// <param name="sender">
        ///// The sender.
        ///// </param>
        ///// <param name="e">
        ///// The e.
        ///// </param>
        //protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item is GridDataItem)
        //    {
        //        var editLink = (Image)e.Item.FindControl("EditLink");
        //        editLink.Attributes["href"] = "#";
        //        editLink.Attributes["onclick"] = string.Format(
        //            "return ShowEditForm('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
        //    }
        //}

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        //protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        //{
        //    var item = (GridDataItem)e.Item;
        //    var disId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
        //    this.equimentService.Delete(disId);

        //    this.grdDocument.Rebind();
        //}

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

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            //AddPageView(e.Tab);
            e.Tab.PageView.Selected = true;
        }

        protected void radMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            throw new NotImplementedException();
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

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }



        protected void grdDocument_UpdateCommand(object sender, TreeListCommandEventArgs e)
        {
            Hashtable table = new Hashtable();
            TreeListEditFormItem item = e.Item as TreeListEditFormItem;
            item.ExtractValues(table);

            ConvertEmptyValuesToDBNull(table);


            int Id = Convert.ToInt32(item.ParentItem.GetDataKeyValue("ID").ToString());
            var EquitmentObj = this.equimentService.GetById(Id);
            if (EquitmentObj != null)
            {
                EquitmentObj.Number = table["Number"].ToString();
                EquitmentObj.EquipmentName = table["EquipmentName"].ToString();
                EquitmentObj.Quantity = Convert.ToInt32(table["Quantity"].ToString());
                EquitmentObj.CalculationUnit = table["CalculationUnit"].ToString();
                EquitmentObj.ExpectedPrice = Convert.ToDouble(table["ExpectedPrice"].ToString());
                EquitmentObj.Remark = table["Remark"].ToString();
                EquitmentObj.UpdatedBy = UserSession.Current.User.Id;
                EquitmentObj.UpdatedDate = DateTime.Now;
                EquitmentObj.LastUpdatedByName = UserSession.Current.User.FullName;

                this.equimentService.Update(EquitmentObj);
            }
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }
        protected void grdDocument_InsertCommand(object sender, TreeListCommandEventArgs e)
        {
            Hashtable table = new Hashtable();
            TreeListEditableItem item = e.Item as TreeListEditableItem;
            item.ExtractValues(table);

            ConvertEmptyValuesToDBNull(table);

            var EquitmentObj = new Equipment();
            EquitmentObj.Number = table["Number"].ToString();
            EquitmentObj.EquipmentName = table["EquipmentName"].ToString();
            EquitmentObj.Quantity = Convert.ToInt32(table["Quantity"].ToString());
            EquitmentObj.ExpectedPrice = Convert.ToDouble(table["ExpectedPrice"].ToString());
            EquitmentObj.CalculationUnit = table["CalculationUnit"].ToString();
            EquitmentObj.Remark = table["Remark"].ToString();
            EquitmentObj.CreatedBy = UserSession.Current.User.Id;
            EquitmentObj.CreatedDate = DateTime.Now;
            EquitmentObj.CreatedByName = UserSession.Current.User.FullName;
            TreeListEditFormInsertItem itemparrent = e.Item as TreeListEditFormInsertItem;
            if (itemparrent.ParentItem != null)
            {
                EquitmentObj.ParentId = Convert.ToInt32(itemparrent.ParentItem.GetDataKeyValue("ID"));
            }

            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            if (ddlProject != null)
            {
                var projectId = ddlProject.SelectedItem != null ? Convert.ToInt32(ddlProject.SelectedValue) : 0;
                var proejctObj = this.projectCodeService.GetById(projectId);
                EquitmentObj.ProjectID = proejctObj?.ID;
                EquitmentObj.ProjectName = proejctObj.Code;
            }
            this.equimentService.Insert(EquitmentObj);



        }

        protected void grdDocument_DeleteCommand(object sender, TreeListCommandEventArgs e)
        {
            TreeListDataItem item = e.Item as TreeListDataItem;
            if (item.CanExpand)
            {
                RadWindowManager1.RadAlert("Please delete all child items before deleting their parent item!", 330, 180, "Server Alert", "", "");
                e.Canceled = true;
                return;
            }
            int Id = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            this.equimentService.Delete(Id);
        }

        protected void grdDocument_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            var txtSearch = (System.Web.UI.WebControls.TextBox)this.CustomerMenu.Items[2].FindControl("txtSearch");
            if (ddlProject != null)
            {
                var projectId = ddlProject.SelectedItem != null ? Convert.ToInt32(ddlProject.SelectedValue) : 0;
                var projectList = this.equimentService.GetAllByProject(projectId);
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    var listkey = txtSearch.Text.ToLower().Split(' ').ToArray();
                    projectList = projectList.Where(t => listkey.All(k => (t.Number.ToLower() + " " + t.EquipmentName.ToLower()).Contains(k))).ToList();

                }
                this.grdDocument.DataSource = projectList;
            }
        }

        private void ConvertEmptyValuesToDBNull(Hashtable values)
        {
            List<object> keysToDbNull = new List<object>();

            foreach (DictionaryEntry entry in values)
            {
                if (entry.Value == null || (entry.Value is String && String.IsNullOrEmpty((String)entry.Value)))
                {
                    keysToDbNull.Add(entry.Key);
                }
            }

            foreach (object key in keysToDbNull)
            {
                if (key == "Quantity" || key == "ExpectedPrice")
                {
                    values[key] = 0;
                }
                else
                {
                    values[key] = "";
                }

            }
        }

        protected void grdDocument_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            if (e.Item.ItemType == TreeListItemType.AlternatingItem || e.Item.ItemType == TreeListItemType.Item)
            {
                TreeListDataItem item = e.Item as TreeListDataItem;
                int level = item.HierarchyIndex.NestedLevel;
                if (level == 3)
                {
                    item["InsertCommandColumn"].Enabled = false;
                }
            }
        }
    }
}

