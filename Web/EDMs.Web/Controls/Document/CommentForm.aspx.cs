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
    public partial class CommentForm : Page
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
        public CommentForm()
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
                this.Title = "Comment from " + this.ContObj.Name;
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
            var comResList = this.commentResponseService.GetAllByContDoc(this.ContractorID, this.DocID);
            this.grdDocument.DataSource = comResList;    
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem) e.Item;
            if (e.CommandName == "EditCmd")
            {
                var comResId = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                var comResObj = this.commentResponseService.GetById(comResId);
                if (comResObj != null)
                {
                    Session.Add("EditingId", comResObj.ID);

                    //this.txtVSPSendDate.SelectedDate = comResObj.ManageSendDate;
                    //this.txtVSPSendTrans.Text = comResObj.ManageSendTransNumber;
                    this.txtPlanComment.SelectedDate = comResObj.PlanReceiveDate;
                    this.txtActualComment.SelectedDate = comResObj.ActualReceiveDate;
                    //this.txtTransNo.Text = comResObj.ReceiveTransNumber;
                    this.ddlCode.SelectedValue = comResObj.ReceiveCodeID.ToString();
                    this.txtCommentSheet.Text = comResObj.CommentSheetNumber;
                    this.txtPlanSendCMSToDesign.SelectedDate = comResObj.PlanSendCMSToDesign;
                    this.txtActualSendCMSToDesign.SelectedDate = comResObj.ActualSendCMSToDesign;
                    this.txtSendCMDToDesignTransNo.Text = comResObj.SendCMSToDesignTransName;
                }
            }
        }

        private void ClearData()
        {
            //this.txtVSPSendDate.SelectedDate = null;
            //this.txtVSPSendTrans.Text = string.Empty;
            //this.ddlContractor.SelectedIndex = 0;
            this.txtPlanComment.SelectedDate = null;
            this.txtActualComment.SelectedDate = null;
            this.ddlCode.SelectedIndex = 0;
            this.txtCommentSheet.Text = string.Empty;
            //this.txtTransNo.Text = string.Empty;

            this.txtPlanSendCMSToDesign.SelectedDate = null;
            this.txtActualSendCMSToDesign.SelectedDate = null;
            this.txtSendCMDToDesignTransNo.Text = string.Empty;

            Session.Remove("EditingId");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearData();
        }

        private void LoadComboData()
        {
            var codeList = this.codeService.GetAllByProject(this.DocObj.ProjectId.GetValueOrDefault());
            codeList.Insert(0, new Code() {ID = 0});
            this.ddlCode.DataSource = codeList;
            this.ddlCode.DataTextField = "FullName";
            this.ddlCode.DataValueField = "ID";
            this.ddlCode.DataBind();
        }

        private void CollectData(ref CommentResponse comResObj)
        {
            comResObj.DocumentID = this.DocID;
            comResObj.FromContractorID = this.ContractorID;
            comResObj.FromContractorName = this.ContObj.Name;
            comResObj.FromContractorTypeID = this.ContObj.TypeID;

            //comResObj.ManageSendDate = this.txtVSPSendDate.SelectedDate;
            //comResObj.ManageSendTransNumber = this.txtVSPSendTrans.Text;
            comResObj.PlanReceiveDate = this.txtPlanComment.SelectedDate;
            comResObj.ActualReceiveDate = this.txtActualComment.SelectedDate;
            //comResObj.ReceiveTransNumber = this.txtTransNo.Text;

            comResObj.ReceiveCodeID = this.ddlCode.SelectedItem != null
                ? Convert.ToInt32(this.ddlCode.SelectedValue)
                : 0;
            comResObj.ReceiveCodeName = this.ddlCode.SelectedItem != null
                ? this.ddlCode.SelectedItem.Text
                : string.Empty;

            comResObj.CommentSheetNumber = this.txtCommentSheet.Text;

            comResObj.PlanSendCMSToDesign = this.txtPlanSendCMSToDesign.SelectedDate;
            comResObj.ActualSendCMSToDesign = this.txtActualSendCMSToDesign.SelectedDate;
            comResObj.SendCMSToDesignTransName = this.txtSendCMDToDesignTransNo.Text.Trim();
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
    }
}