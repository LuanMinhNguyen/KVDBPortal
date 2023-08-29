using System;
using System.Collections.Generic;
using System.Web.UI;
using EDMs.Business.Services.Document;
using EDMs.Web.Utilities.Sessions;
using iTextSharp.text.pdf;

namespace EDMs.Web.Controls.MarkupTool
{
    public partial class MarkupControl : Page
    {
        private readonly DQREDocumentService documentService = new DQREDocumentService();
        private readonly DQREDocumentAttachFileService attachFileService = new DQREDocumentAttachFileService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]) && !string.IsNullOrEmpty(this.Request.QueryString["userId"]))
            {
                var attachDocObj = this.attachFileService.GetById(new Guid(this.Request.QueryString["docId"]));
                var markupLayerList = new List<string>()
                {
                    "~/DocumentLibrary/MarkupFile/Layer1.xml",
                    //"~/DocumentLibrary/MarkupFile/layer2.xml",
                    //"~/DocumentLibrary/MarkupFile/layer3.xml",

                };
                this.pdfMarkupControl.SetParameter(1,UserSession.Current.User.UserNameWithFullName, 1, "~/DocumentLibrary/MarkupFile", attachDocObj.FilePath, markupLayerList, attachDocObj.ProjectDocumentId.GetValueOrDefault());
            }

            

        }


    }
}