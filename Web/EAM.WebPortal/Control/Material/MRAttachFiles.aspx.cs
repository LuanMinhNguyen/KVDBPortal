// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EAM.Business.Services.Material;
using EAM.Data.Entities;
using Telerik.Web.UI;

namespace EAM.WebPortal.Control.Material
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class MRAttachFiles : Page
    {
        /// <summary>
        /// The document service.
        /// </summary>
        private readonly AA_MaterialRequestAttachFileService mrAttachFileService = new AA_MaterialRequestAttachFileService();
        private readonly AA_MaterialRequestService mrService = new AA_MaterialRequestService();


        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionHistory"/> class.
        /// </summary>
        public MRAttachFiles()
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
            var docList = new List<AA_MaterialRequestAttachFile>();
            var mrList = this.mrService.GetAllMRWaitingApprove();
            foreach (var mrItem in mrList)
            {
                docList.AddRange(this.mrAttachFileService.GetByMR(mrItem.ID));
            }

            this.grdDocument.DataSource = docList.OrderBy(t => t.CreatedByName);
        }
    }
}