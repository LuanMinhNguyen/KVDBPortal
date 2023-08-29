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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Document;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class VersionHistory : Page
    {
        /// <summary>
        /// The document service.
        /// </summary>
        private readonly DocumentService documentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public VersionHistory()
        {
            this.documentService = new DocumentService();
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
            if(Request.QueryString["docId"] != null)
            {
                var docId = Convert.ToInt32(Request.QueryString["docId"]);
                var objDoc = this.documentService.GetById(docId);
                if (objDoc != null)
                {
                    if (objDoc.ParentID == null)
                    {
                        this.grdDocument.DataSource = this.documentService.GetAllDocVersion(objDoc.ID);
                    }
                    else
                    {
                        this.grdDocument.DataSource = this.documentService.GetAllDocVersion(objDoc.ParentID.GetValueOrDefault());
                    }
                }
                else
                {
                    this.grdDocument.DataSource = new List<Document>(); 
                }
            }
        }
    }
}