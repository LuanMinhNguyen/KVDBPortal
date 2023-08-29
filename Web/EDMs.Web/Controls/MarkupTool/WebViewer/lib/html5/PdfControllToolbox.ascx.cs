using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppPdf.WebViewer.lib.html5
{
    public partial class PdfControllToolbox : System.Web.UI.UserControl
    {
        public int UserId { get; set; }
        public int UserPermission { get; set; }
        public string MarkupPath { get; set; }

        public string PDFFilePath { get; set; }

        public List<string> CurrentMarkupFileList { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}