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
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Business.Services.WMS;
using EDMs.Data.Entities;
using EDMs.Web.Controls.Document;
using EDMs.Web.Utilities.Sessions;

namespace EDMs.Web.Controls.Library
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MRCheckListEditForm : Page
    {
        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly MaterialRequisitionCheckListDefineService mrCheckListDefineService;
        
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public MRCheckListEditForm()
        {
            this.userService = new UserService();
            this.mrCheckListDefineService = new MaterialRequisitionCheckListDefineService();
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
                if (this.Request.QueryString["action"] == "Update")
                {
                    var checkListItem = this.mrCheckListDefineService.GetById(Convert.ToInt32(this.Request.QueryString["checkListId"]));

                    if (checkListItem != null)
                    {
                        this.txtDescription.Text = checkListItem.Description;
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
            if (this.Page.IsValid)
            {
                var mrCheckListId = Convert.ToInt32(this.Request.QueryString["checkListId"]);
                if (this.Request.QueryString["action"] == "New")
                {
                    var parentObj = this.mrCheckListDefineService.GetById(mrCheckListId);

                    var obj = new MaterialRequisitionCheckListDefine()
                    {
                        Description = this.txtDescription.Text.Trim(),
                        ParentId = mrCheckListId,
                        ParentDescription = parentObj.Description
                    };

                    this.mrCheckListDefineService.Insert(obj);
                }
                else if (this.Request.QueryString["action"] == "Update")
                {
                    var obj = this.mrCheckListDefineService.GetById(mrCheckListId);
                    if (obj != null)
                    {
                        obj.Description = this.txtDescription.Text.Trim();
                        this.mrCheckListDefineService.Update(obj);

                        var childNode = this.mrCheckListDefineService.GetAll().Where(t => t.ParentId == obj.ID);
                        foreach (var item in childNode)
                        {
                            item.ParentDescription = obj.Description;
                            this.mrCheckListDefineService.Update(item);

                        }
                    }

                    

                    
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
            }
        }

        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }

    }
}