// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Web.Hosting;
    using System.Web.UI;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class IncomingTransmittalEditForm : Page
    {
        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly ToListService toListService;

        private readonly IncomingTransmittalService incomingTransmittalService;

        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public IncomingTransmittalEditForm()
        {
            this.toListService = new ToListService();
            this.incomingTransmittalService = new IncomingTransmittalService();
            this.userService = new UserService();
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
                this.txtReceivedDate.SelectedDate = DateTime.Now;
                this.LoadComboData();
                this.UploadControl.Visible = true;

                if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
                {
                    this.UploadControl.Visible = false;
                    this.CreatedInfo.Visible = true;
                    var incomingTransObj =
                    this.incomingTransmittalService.GetById(Convert.ToInt32(this.Request.QueryString["tranId"]));
                    if (incomingTransObj != null)
                    {
                        this.txtName.Text = incomingTransObj.Name;
                        this.txtTransmittalNumber.Text = incomingTransObj.TransNumber;
                        this.txtProject.Text = incomingTransObj.ProjectName;
                        this.ddlFrom.SelectedValue = incomingTransObj.FromId.GetValueOrDefault().ToString();
                        this.ddlTo.SelectedValue = incomingTransObj.ToId.GetValueOrDefault().ToString();
                        this.txtAttention.Text = incomingTransObj.AttentionName;
                        this.txtReceivedDate.SelectedDate = incomingTransObj.ReceivedDate;

                        var createdUser = this.userService.GetByID(incomingTransObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + incomingTransObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (incomingTransObj.LastUpdatedBy != null && incomingTransObj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(incomingTransObj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + incomingTransObj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
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
            if (!string.IsNullOrEmpty(this.Request.QueryString["tranId"]))
            {
                var incomingTransObj =
                    this.incomingTransmittalService.GetById(Convert.ToInt32(this.Request.QueryString["tranId"]));
                if (incomingTransObj != null)
                {
                    incomingTransObj.Name = this.txtName.Text.Trim();
                    incomingTransObj.TransNumber = this.txtTransmittalNumber.Text.Trim();
                    incomingTransObj.ProjectName = this.txtProject.Text.Trim();
                    incomingTransObj.FromId = Convert.ToInt32(this.ddlFrom.SelectedValue);
                    incomingTransObj.FromName = this.ddlFrom.SelectedItem.Text;
                    incomingTransObj.ToId = Convert.ToInt32(this.ddlTo.SelectedValue);
                    incomingTransObj.ToName = this.ddlTo.SelectedItem.Text;
                    incomingTransObj.AttentionName = this.txtAttention.Text.Trim();
                    incomingTransObj.ReceivedDate = this.txtReceivedDate.SelectedDate;
                    incomingTransObj.LastUpdatedBy = UserSession.Current.User.Id;
                    incomingTransObj.LastUpdatedDate = DateTime.Now;

                    this.incomingTransmittalService.Update(incomingTransObj);
                }
            }
            else
            {
                var incomingTransObj = new IncomingTransmittal()
                    {
                        Name = this.txtName.Text.Trim(),
                        TransNumber = this.txtTransmittalNumber.Text.Trim(),
                        ProjectName = this.txtProject.Text.Trim(),
                        FromId = Convert.ToInt32(this.ddlFrom.SelectedValue),
                        FromName = this.ddlFrom.SelectedItem.Text,
                        ToId = Convert.ToInt32(this.ddlTo.SelectedValue),
                        ToName = this.ddlTo.SelectedItem.Text,
                        AttentionName = this.txtAttention.Text.Trim(),
                        ReceivedDate = this.txtReceivedDate.SelectedDate,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now
                    };

                foreach (UploadedFile transFile in this.docuploader.UploadedFiles)
                {
                    var filePath = Server.MapPath("../../Transmittals/Import/");
                    var fileName = DateTime.Now.ToString("ddMMyyhhmmss") + transFile.FileName;
                    transFile.SaveAs(filePath + fileName, true);

                    incomingTransObj.AttachFileName = transFile.FileName;
                    incomingTransObj.AttachFilePath = "/Transmittals/Import/" + fileName;
                }

                this.incomingTransmittalService.Insert(incomingTransObj);
            }

            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
        }

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var tolist = this.toListService.GetAll();
            tolist.Insert(0, new ToList() { ID = 0, Name = string.Empty });

            this.ddlTo.DataSource = tolist;
            this.ddlTo.DataTextField = "Name";
            this.ddlTo.DataValueField = "ID";
            this.ddlTo.DataBind();
            this.ddlTo.SelectedIndex = 0;

            this.ddlFrom.DataSource = tolist;
            this.ddlFrom.DataTextField = "Name";
            this.ddlFrom.DataValueField = "ID";
            this.ddlFrom.DataBind();
            this.ddlFrom.SelectedIndex = 0;
        }

        /// <summary>
        /// The save upload file.
        /// </summary>
        /// <param name="uploadDocControl">
        /// The upload doc control.
        /// </param>
        /// <param name="objDoc">
        /// The obj Doc.
        /// </param>
        private void SaveUploadFile(RadAsyncUpload uploadDocControl, ref Document objDoc, bool isUpdateOldRev)
        {
            var listUpload = uploadDocControl.UploadedFiles;
            var revisionPath = "../../DocumentLibrary/RevisionHistory/";
            var serverRevisionFolder = HostingEnvironment.ApplicationVirtualPath + "/DocumentLibrary/RevisionHistory/";
            if (listUpload.Count > 0)
            {
                foreach (UploadedFile docFile in listUpload)
                {

                    ////if (isUpdateOldRev)
                    ////{
                    ////    docFile.SaveAs(saveFileRevisionPath, true);
                    ////}
                    ////else
                    ////{
                    ////    docFile.SaveAs(saveFilePath, true);
                    ////    var fileinfo = new FileInfo(saveFilePath);
                    ////    fileinfo.CopyTo(saveFileRevisionPath, true);
                    ////}

                    ////if (Utilities.Utility.ServiceIsAvailable(ServiceName))
                    ////{
                    ////    watcherService.ExecuteCommand(129);
                    ////}
                }
            }
        }
    }
}