using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPdf
{
    public class EditPDFControlPara
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static int UserPermission { get; set; }
        public static string MarkupPath { get; set; }

        public static string PDFFilePath { get; set; }
        public static Guid DocId { get; set; }
        public static List<string> CurrentMarkupFileList { get; set; }
        public static List<LayerPDF> lstConsolidate { get; set; }
    }
    public class LayerPDF
    {
        public string color { get; set; }
        public string item1 { get; set; }
        public string Content { get; set; }
        public string item2 { get; set; }
        public string Datestr { get; set; }
        public string User { get; set; }

    }
}