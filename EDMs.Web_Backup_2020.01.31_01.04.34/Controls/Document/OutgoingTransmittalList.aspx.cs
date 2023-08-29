// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Library;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Security;
    using Telerik.Web.UI;
    using System.IO;
    using Aspose.Cells;
    using System.Data;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class OutgoingTransmittalList : Page
    {
        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly PermissionService permissionService = new PermissionService();

        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly DQRETransmittalService transmittalService = new DQRETransmittalService();

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string unreadPattern = @"\(\d+\)";

        private readonly ProjectCodeService projectCodeService = new ProjectCodeService();

        private readonly DQREDocumentService documentService = new DQREDocumentService();

        private readonly OrganizationCodeService organizationCodeService = new OrganizationCodeService();

        private readonly AttachDocToTransmittalService attachDocToTransmittalService = new AttachDocToTransmittalService();


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
            Session.Add("SelectedMainMenu", "Document Management");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            var temp = (RadPane)this.Master.FindControl("leftPane");
            temp.Collapsed = true;
            if (!this.Page.IsPostBack)
            {
                var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
                if (ddlProject != null)
                {
                    var projectList = this.projectCodeService.GetAll().OrderBy(t => t.Code);
                    ddlProject.DataSource = projectList;
                    ddlProject.DataTextField = "FullName";
                    ddlProject.DataValueField = "ID";
                    ddlProject.DataBind();

                    int projectId = Convert.ToInt32(ddlProject.SelectedValue);
                    this.lblProjectId.Value = projectId.ToString();
                    Session.Add("SelectedProject", projectId);
                }
                //this.LoadSystemPanel();
            }
        }


        /// <summary>
        /// The search customer.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void SearchDocument(string search, bool isbind = false)
        {
            ////if (this.radTreeFolder.SelectedNode != null)
            ////{
            ////    var folderId = Convert.ToInt32(this.radTreeFolder.SelectedNode.Value);
            ////    if(!string.IsNullOrEmpty(search))
            ////    {
            ////        this.grdDocument.DataSource = this.documentService.QuickSearch(search, folderId);    
            ////    }
            ////    else
            ////    {
            ////        this.grdDocument.DataSource =  this.documentService.GetAllByFolder(folderId);
            ////    }


            ////    if (isbind)
            ////    {
            ////        this.grdDocument.DataBind();
            ////    }
            ////}
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
                this.grdDocument.Rebind();
            }
            else if (e.Argument.Contains("Export"))
            {
                var transID = new Guid(e.Argument.Split('_')[1]);
                //var transID = new Guid(this.Request.QueryString["tranId"]);
                var attachDocToTrans = this.attachDocToTransmittalService.GetAllByTransId(transID);
                if (attachDocToTrans != null)
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Exports") + @"\";
                    //var filePath = Server.MapPath("Exports") + @"\";
                    var workbook = new Workbook();
                    workbook.Open(filePath + @"Template\TransTemplate.xlsx");

                    var dataSheet = workbook.Worksheets[0];

                    var dtFull = new DataTable();

                    dtFull.Columns.AddRange(new[]
                    {
                        new DataColumn("DocumentNo", typeof (String)),
                        new DataColumn("Revision", typeof (String)),
                        new DataColumn("IsssuedDate", typeof (String)),
                        new DataColumn("DocumentTitle", typeof (String)),
                        new DataColumn("DocumentClassName", typeof (String)),
                        new DataColumn("DocumentCodeName", typeof (String))
                    });

                    foreach (var docobj in attachDocToTrans)
                    {
                        var dataRow = dtFull.NewRow();
                        var documentObj = this.documentService.GetById(docobj.DocumentId.GetValueOrDefault());
                        dataRow["DocumentNo"] = documentObj.DocumentNo;
                        dataRow["Revision"] = documentObj.Revision;
                        dataRow["IsssuedDate"] = Convert.ToDateTime(documentObj.IsssuedDate).ToString("dd-MMM-yy");
                        dataRow["DocumentTitle"] = documentObj.DocumentTitle;
                        dataRow["DocumentClassName"] = documentObj.DocumentClassName;
                        dataRow["DocumentCodeName"] = documentObj.DocumentCodeName;
                        dtFull.Rows.Add(dataRow);
                    }
                    var transObj = this.transmittalService.GetById(transID);
                    var filename = transObj.TransmittalNo + "_Trans_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    dataSheet.Cells["G3"].PutValue(transObj.TransmittalNo);
                    dataSheet.Cells["E4"].PutValue(DateTime.Now.ToString("dd-MMM-yy"));

                    var fromObj = this.organizationCodeService.GetById(transObj.OriginatingOrganizationId.GetValueOrDefault());
                    var toObj = this.organizationCodeService.GetById(transObj.ReceivingOrganizationId.GetValueOrDefault());
                    dataSheet.Cells["E5"].PutValue(toObj.Description);
                    dataSheet.Cells["I5"].PutValue(toObj.Phone);
                    dataSheet.Cells["I6"].PutValue(toObj.Fax);
                    dataSheet.Cells["E8"].PutValue(fromObj.Description);
                    dataSheet.Cells["I8"].PutValue(fromObj.Phone);
                    dataSheet.Cells["I9"].PutValue(fromObj.Fax);

                    dataSheet.Cells["C30"].PutValue(fromObj.Description);

                    int firstrow = 12;
                    for (int i = 0; i < dtFull.Rows.Count; i++)
                    {
                        firstrow++;
                        dataSheet.Cells["C" + firstrow].PutValue(dtFull.Rows[i]["DocumentNo"]);
                        dataSheet.Cells["E" + firstrow].PutValue(dtFull.Rows[i]["Revision"]);
                        dataSheet.Cells["F" + firstrow].PutValue(dtFull.Rows[i]["IsssuedDate"]);
                        dataSheet.Cells["G" + firstrow].PutValue(dtFull.Rows[i]["DocumentTitle"]);
                        dataSheet.Cells["J" + firstrow].PutValue(dtFull.Rows[i]["DocumentClassName"]);
                        dataSheet.Cells["K" + firstrow].PutValue(dtFull.Rows[i]["DocumentCodeName"]);
                    }
                    workbook.Save(filePath + filename);
                    this.Download_File(filePath + filename);
                }
            }
        }

        private void Download_File(string FilePath)
        {
            Response.Clear();
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void ddlProject_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var ddlProject = (RadComboBox)this.CustomerMenu.Items[2].FindControl("ddlProject");
            int projectId = Convert.ToInt32(ddlProject.SelectedValue);
            this.lblProjectId.Value = projectId.ToString();
            this.grdDocument.Rebind();

            Session.Add("SelectedProject", projectId);
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"~/Images/project.png";
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

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
            if (ddlProject != null && ddlProject.SelectedItem != null)
            {
                var projectId = Convert.ToInt32(ddlProject.SelectedValue);

                var outgoingTransList = this.transmittalService.GetAllByProject(projectId, 2, string.Empty).OrderByDescending(t => t.TransmittalNo);
                this.grdDocument.DataSource = outgoingTransList;
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
                    "return ShowEditForm('{0}');", new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString()));
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
            Guid tranId = new Guid(item.GetDataKeyValue("ID").ToString());
            this.transmittalService.Delete(tranId);

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
                var item = e.Item as GridDataItem;
                var newIcon = (Image)e.Item.FindControl("newicon");

                if ((DateTime.Now - Convert.ToDateTime(DataBinder.Eval(item.DataItem, "CreatedDate"))).TotalHours < 24)
                {
                    newIcon.Visible = true;
                }
            }
        }
    }
}

