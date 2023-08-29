using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebAppPdf.WebViewer.lib.html5
{
    public partial class ReaderControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

        }
        void myBtn_Click(Object sender, EventArgs e)
        {
            CreateLayer();
        }
        public void CreateLayer()
        {

            string MarkupPath = Server.MapPath(EditPDFControlPara.MarkupPath);
            if (!System.IO.Directory.Exists(MarkupPath))
                System.IO.Directory.CreateDirectory(MarkupPath);
            using (XmlWriter xWr = XmlWriter.Create(MarkupPath + "/Layer2.xml"))
            {
                xWr.WriteStartDocument();

                xWr.WriteStartElement("Layers");
                xWr.WriteStartElement("List");

                // ADD FEW ELEMENTS.
                xWr.WriteElementString("BookName", "Computer Architecture");
                xWr.WriteElementString("Category", "Computers");
                xWr.WriteElementString("Price", "125.60");

                xWr.WriteEndElement();          // CLOSE LIST.
                xWr.WriteEndElement();          // CLOSE LIBRARY.

                xWr.WriteEndDocument();         // END DOCUMENT.

                // FLUSH AND CLOSE.
                xWr.Flush();
                xWr.Close();

                // SHOW A MESSAGE IN A DIV.
                //  div_xml.InnerText = "File created.";
            }
        }
    }
}