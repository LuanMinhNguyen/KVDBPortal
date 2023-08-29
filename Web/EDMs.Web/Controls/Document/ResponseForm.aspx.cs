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
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Scope;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class ResponseForm : Page
    {

        private readonly CodeService codeService = new CodeService();
        private readonly ContractorService contractorService = new ContractorService();
        private readonly CommentResponseService commentResponseService = new CommentResponseService();
        private readonly DocumentPackageService documentPackageService = new DocumentPackageService();
        private int ContractorID
        {
            get
            {
                return Convert.ToInt32(this.Request.QueryString["contId"]);
            }
        }

        private Contractor ContObj
        {
            get { return this.contractorService.GetById(this.ContractorID); }
        }

        private int DocID
        {
            get
            { return Convert.ToInt32(this.Request.QueryString["docId"]); }
        }

        private DocumentPackage DocObj
        {
            get { return this.documentPackageService.GetById(this.DocID); }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public ResponseForm()
        {
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
            if (!this.IsPostBack)
            {
                this.LoadComboData();
                this.ClearData();
                this.InitControl();
            }
        }

        private void InitControl()
        {
            if (this.ContObj != null)
            {
                this.Title = "Response from " + this.ContObj.Name;

                // Init label text
                ////this.lblResPlanDesignToManage.Text = "Plan (" + this.ContObj.Name + " - " + "PVCFC" + ")";
                ////this.lblResActualDesignToManage.Text = "Actual (" + this.ContObj.Name + " - " + "PVCFC" + ")";

                //this.lblResPlanManageToReview.Text = "Plan (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";
                //this.lblResActualManageToReview.Text = "Actual (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";

                ////this.lblResPlanReviewToManage.Text = "Plan (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";
                ////this.lblResActualReviewToManage.Text = "Actual (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";

                ////this.lblResPlanManageToDesign.Text = "Plan (PVCFC - " + this.ContObj.Name + ")";
                ////this.lblResActualManageToDesign.Text = "Actual (PVCFC - " + this.ContObj.Name + ")";

                // Init header text of grid column
                //this.grdDocument.MasterTableView.GetColumn("ResPlanDesignToManage").HeaderText = "Plan (" + this.ContObj.Name + " - " + "PVCFC" + ")";
                //this.grdDocument.MasterTableView.GetColumn("ResActualDesignToManage").HeaderText = "Actual (" + this.ContObj.Name + " - " + "PVCFC" + ")";

                //this.grdDocument.MasterTableView.GetColumn("ResPlanManageToReview").HeaderText = "Plan (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";
                //this.grdDocument.MasterTableView.GetColumn("ResActualManageToReview").HeaderText = "Actual (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";

                //this.grdDocument.MasterTableView.GetColumn("ResPlanReviewToManage").HeaderText = "Plan (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";
                //this.grdDocument.MasterTableView.GetColumn("ResActualReviewToManage").HeaderText = "Actual (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";

                //this.grdDocument.MasterTableView.GetColumn("ResPlanManageToDesign").HeaderText = "Plan (PVCFC - " + this.ContObj.Name + ")";
                //this.grdDocument.MasterTableView.GetColumn("ResActualManageToDesign").HeaderText = "Actual (PVCFC - " + this.ContObj.Name + ")";
            }
        }

        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                if (Session["EditingId"] != null)
                {
                    var comResId = Convert.ToInt32(Session["EditingId"]);
                    var comResObj = this.commentResponseService.GetById(comResId);
                    if (comResObj != null)
                    {
                        this.CollectData(ref comResObj);

                        comResObj.UpdatedBy = UserSession.Current.User.Id;
                        comResObj.UpdatedDate = DateTime.Now;

                        this.commentResponseService.Update(comResObj);

                        this.UpdatedocFinalCode();
                    }

                    Session.Remove("EditingId");
                }
                else
                {
                    var comResObj = new CommentResponse();

                    comResObj.CreatedBy = UserSession.Current.User.Id;
                    comResObj.CreatedDate = DateTime.Now;

                    this.CollectData(ref comResObj);
                    var wpId = this.commentResponseService.Insert(comResObj);

                    this.UpdatedocFinalCode();
                }
            }

            this.ClearData();
            this.grdDocument.Rebind();
        }



        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            var comResId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            this.commentResponseService.Delete(comResId);
            this.grdDocument.Rebind();
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.ContObj.TypeID == 1)
            {
                var responseForContID = this.ddlContractor.SelectedItem != null
                    ? Convert.ToInt32(this.ddlContractor.SelectedItem.Value)
                    : 0;
                var comResList = this.commentResponseService.GetAllByContDoc(this.ContractorID, this.DocID, responseForContID);
                this.grdDocument.DataSource = comResList;
            }
            else
            {
                var comResList = this.commentResponseService.GetAllByContDoc(this.ContractorID, this.DocID);
                this.grdDocument.DataSource = comResList;    
            }
            
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
            if (e.CommandName == "EditCmd")
            {
                var comResId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                var comResObj = this.commentResponseService.GetById(comResId);
                if (comResObj != null)
                {
                    Session.Add("EditingId", comResObj.ID);
                    this.ddlContractor.SelectedValue = comResObj.ToContractorID.ToString();

                    //this.txtResActualDesignToManage.SelectedDate = comResObj.ResActualDesignToManage;
                    //this.txtResActualManageToDesign.SelectedDate = comResObj.ResActualManageToDesign;
                    this.txtResActualManageToReview.SelectedDate = comResObj.ResActualManageToReview;
                    //this.txtResActualReviewToManage.SelectedDate = comResObj.ResActualReviewToManage;

                    //this.txtResDesignToManageTransName.Text = comResObj.ResDesignToManageTransName;
                    //this.txtResManageToDesignTransName.Text = comResObj.ResManageToDesignTransName;
                    this.txtResManageToReviewTransName.Text = comResObj.ResManageToReviewTransName;
                    //this.txtResReviewToManageTransName.Text = comResObj.ResReviewToManageTransName;

                    //this.txtResPlanDesignToManage.SelectedDate = comResObj.ResPlanDesignToManage;
                    //this.txtResPlanManageToDesign.SelectedDate = comResObj.ResPlanManageToDesign;
                    this.txtResPlanManageToReview.SelectedDate = comResObj.ResPlanManageToReview;
                    //this.txtResPlanReviewToManage.SelectedDate = comResObj.ResPlanReviewToManage;
                }
            }
        }

        private void ClearData()
        {
            //this.txtResActualDesignToManage.SelectedDate = null;
            //this.txtResActualManageToDesign.SelectedDate = null;
            this.txtResActualManageToReview.SelectedDate = null;
            //this.txtResActualReviewToManage.SelectedDate = null;

            //this.txtResDesignToManageTransName.Text = string.Empty;
            //this.txtResManageToDesignTransName.Text = string.Empty;
            this.txtResManageToReviewTransName.Text = string.Empty;
            //this.txtResReviewToManageTransName.Text = string.Empty;

            //this.txtResPlanDesignToManage.SelectedDate = null;
            //this.txtResPlanManageToDesign.SelectedDate = null;
            this.txtResPlanManageToReview.SelectedDate = null;
            //this.txtResPlanReviewToManage.SelectedDate = null;
            Session.Remove("EditingId");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearData();
        }

        private void LoadComboData()
        {
            var contList = this.contractorService.GetAll().Where(t => t.ID != this.ContractorID);
            this.ddlContractor.DataSource = contList;
            this.ddlContractor.DataTextField = "Name";
            this.ddlContractor.DataValueField = "ID";
            this.ddlContractor.DataBind();
        }

        private void CollectData(ref CommentResponse comResObj)
        {
            comResObj.DocumentID = this.DocID;
            comResObj.FromContractorID = this.ContractorID;
            comResObj.FromContractorName = this.ContObj.Name;
            comResObj.FromContractorTypeID = this.ContObj.TypeID;

            comResObj.ToContractorID = this.ddlContractor.SelectedItem != null
                ? Convert.ToInt32(this.ddlContractor.SelectedValue)
                : 0;
            comResObj.ToContractorName = this.ddlContractor.SelectedItem != null
                ? this.ddlContractor.SelectedItem.Text
                : string.Empty;

            //comResObj.ResActualDesignToManage = this.txtResActualDesignToManage.SelectedDate;
            //comResObj.ResActualManageToDesign = this.txtResActualManageToDesign.SelectedDate;
            comResObj.ResActualManageToReview = this.txtResActualManageToReview.SelectedDate;
            //comResObj.ResActualReviewToManage = this.txtResActualReviewToManage.SelectedDate;

            //comResObj.ResDesignToManageTransName = this.txtResDesignToManageTransName.Text;
            //comResObj.ResManageToDesignTransName = this.txtResManageToDesignTransName.Text;
            comResObj.ResManageToReviewTransName = this.txtResManageToReviewTransName.Text;
            //comResObj.ResReviewToManageTransName = this.txtResReviewToManageTransName.Text;

            //comResObj.ResPlanDesignToManage = this.txtResPlanDesignToManage.SelectedDate;
            //comResObj.ResPlanManageToDesign = this.txtResPlanManageToDesign.SelectedDate;
            comResObj.ResPlanManageToReview = this.txtResPlanManageToReview.SelectedDate;
            //comResObj.ResPlanReviewToManage = this.txtResPlanReviewToManage.SelectedDate;
        }

        private void UpdatedocFinalCode()
        {
            var comResList = this.commentResponseService.GetAllReviewByDoc(this.DocID);

            var codeList =
                comResList.Where(t => t.ReceiveCodeID.GetValueOrDefault() != 0)
                    .Select(t => t.ReceiveCodeID)
                    .Distinct()
                    .Select(t => this.codeService.GetById(t.GetValueOrDefault()))
                    .ToList();
            var completeCodeList = codeList.Where(t => t.IsFinal.GetValueOrDefault()).OrderBy(t => t.ID).ToList();
            var uncompleteCodeList = codeList.Where(t => !t.IsFinal.GetValueOrDefault()).OrderByDescending(t => t.ID).ToList();

            if (uncompleteCodeList.Any())
            {
                this.DocObj.FinalCodeID = uncompleteCodeList[0].ID;
                this.DocObj.FinalCodeName = uncompleteCodeList[0].Name;
                this.documentPackageService.Update(this.DocObj);
            }
            else if (completeCodeList.Any())
            {
                this.DocObj.FinalCodeID = completeCodeList[0].ID;
                this.DocObj.FinalCodeName = completeCodeList[0].Name;
                this.documentPackageService.Update(this.DocObj);
            }
        }

        protected void ddlContractor_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Init label text
            //this.lblResPlanDesignToManage.Text = "Plan (" + this.ContObj.Name + " - " + "PVCFC" + ")";
            //this.lblResActualDesignToManage.Text = "Actual (" + this.ContObj.Name + " - " + "PVCFC" + ")";

            //this.lblResPlanManageToReview.Text = "Plan (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";
            //this.lblResActualManageToReview.Text = "Actual (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";

            //this.lblResPlanReviewToManage.Text = "Plan (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";
            //this.lblResActualReviewToManage.Text = "Actual (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";

            //this.lblResPlanManageToDesign.Text = "Plan (PVCFC - " + this.ContObj.Name + ")";
            //this.lblResActualManageToDesign.Text = "Actual (PVCFC - " + this.ContObj.Name + ")";

            // Init header text of grid column
            //this.grdDocument.MasterTableView.GetColumn("ResPlanDesignToManage").HeaderText = "Plan (" + this.ContObj.Name + " - " + "PVCFC" + ")";
            //this.grdDocument.MasterTableView.GetColumn("ResActualDesignToManage").HeaderText = "Actual (" + this.ContObj.Name + " - " + "PVCFC" + ")";

            //this.grdDocument.MasterTableView.GetColumn("ResPlanManageToReview").HeaderText = "Plan (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";
            //this.grdDocument.MasterTableView.GetColumn("ResActualManageToReview").HeaderText = "Actual (PVCFC - " + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + ")";

            //this.grdDocument.MasterTableView.GetColumn("ResPlanReviewToManage").HeaderText = "Plan (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";
            //this.grdDocument.MasterTableView.GetColumn("ResActualReviewToManage").HeaderText = "Actual (" + (this.ddlContractor.SelectedItem != null ? this.ddlContractor.SelectedItem.Text : string.Empty) + " - " + "PVCFC)";

            //this.grdDocument.MasterTableView.GetColumn("ResPlanManageToDesign").HeaderText = "Plan (PVCFC - " + this.ContObj.Name + ")";
            //this.grdDocument.MasterTableView.GetColumn("ResActualManageToDesign").HeaderText = "Actual (PVCFC - " + this.ContObj.Name + ")";

            this.grdDocument.Rebind();
        }
    }
}