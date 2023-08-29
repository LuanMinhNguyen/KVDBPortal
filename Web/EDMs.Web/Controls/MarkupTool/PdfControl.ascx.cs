using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppPdf
{
    public partial class PdfControl : System.Web.UI.UserControl
    {
        public int UserId { get; set; }
        public int UserPermission { get; set; }
        public string MarkupPath { get; set; }

        public string PDFFilePath { get; set; }

        public List<string> CurrentMarkupFileList { get; set; }
        public void SetParameter(int _UserId, string _UserName, int _UserPermission, string _MarkupPath, string _PDFFilePath, List<string> _CurrentMarkupFileList, Guid _DocId)
        {
            EditPDFControlPara.UserId = _UserId;
            EditPDFControlPara.UserName = _UserName;
            EditPDFControlPara.UserPermission = _UserPermission;
            EditPDFControlPara.MarkupPath = _MarkupPath;
            EditPDFControlPara.PDFFilePath = _PDFFilePath;
            EditPDFControlPara.CurrentMarkupFileList = _CurrentMarkupFileList;
            EditPDFControlPara.DocId = _DocId;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
    }
}