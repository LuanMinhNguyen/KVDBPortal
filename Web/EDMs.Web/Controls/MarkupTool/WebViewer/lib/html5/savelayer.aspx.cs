using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace WebAppPdf.WebViewer.lib.html5
{
    public partial class savelayer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void SaveFile(string xmlstr, string xmlstr1, string color)
        {
            string MarkupPath = Server.MapPath(EditPDFControlPara.MarkupPath);
            if (!System.IO.Directory.Exists(MarkupPath))
                System.IO.Directory.CreateDirectory(MarkupPath);
            using (XmlWriter xWr = XmlWriter.Create(MarkupPath + "/Layer" + EditPDFControlPara.UserId.ToString() + ".xml"))
            {
                xWr.WriteStartDocument();

                xWr.WriteStartElement("Layers");
                xWr.WriteStartElement("List");
                var xmlelement = xmlstr.Split('|');
                var xmlelement1 = xmlstr1.Split('|');
                int indexitem = 0;
                int indexitem1 = 0;
                foreach (var item in xmlelement)
                {
                    indexitem++;
                    xWr.WriteElementString("item" + indexitem.ToString(), item);
                    xWr.WriteElementString("Content", xmlelement1[indexitem1]);
                    indexitem1++;
                }
                // ADD FEW ELEMENTS.

                xWr.WriteElementString("color", color);
                xWr.WriteElementString("User", EditPDFControlPara.UserName);
                xWr.WriteElementString("Date", DateTime.Now.ToString("dd/MM/yyyy"));
               

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
        public void SaveConsolidate()
        {
            string MarkupPath = Server.MapPath(EditPDFControlPara.MarkupPath);
            if (!System.IO.Directory.Exists(MarkupPath))
                System.IO.Directory.CreateDirectory(MarkupPath);
            string strConsolidatePath = MarkupPath + "/ConsolidateLayer" + EditPDFControlPara.UserId.ToString() + ".xml";
            using (XmlWriter xWr = XmlWriter.Create(MarkupPath + "/Layer" + EditPDFControlPara.UserId.ToString() + ".xml"))
            {
                xWr.WriteStartDocument();

                xWr.WriteStartElement("ConsolidateLayer");
                xWr.WriteStartElement("List");
                foreach (var item in WebAppPdf.EditPDFControlPara.lstConsolidate)
                {
                    xWr.WriteElementString("item1", item.item1);
                    xWr.WriteElementString("Content", item.Content);
                    xWr.WriteElementString("item2", item.item2);
                    xWr.WriteElementString("color", item.color);
                    xWr.WriteElementString("User", item.User);
                    xWr.WriteElementString("Date", item.Datestr);
                    //  string fileXml = Server.MapPath(item);
                    // if (System.IO.File.Exists(fileXml))
                    // {

                    /*
                        XDocument xDoc = XDocument.Load(fileXml);
                        string color = xDoc.Descendants(XName.Get("color")).First().Value;
                        string item1 = xDoc.Descendants(XName.Get("item1")).First().Value;
                        string Content = xDoc.Descendants(XName.Get("Content")).First().Value;
                        string item2 = xDoc.Descendants(XName.Get("item2")).First().Value;
                        string Datestr = xDoc.Descendants(XName.Get("Date")).First().Value;
                        string User = xDoc.Descendants(XName.Get("User")).First().Value;

                       
                    }*/
                }
                xWr.WriteElementString("UserConsolidate", EditPDFControlPara.UserId.ToString());
                xWr.WriteElementString("DateConsolidate", DateTime.Now.ToString("dd/MM/yyyy"));

                xWr.WriteEndElement();          // CLOSE LIST.
                xWr.WriteEndElement();          // CLOSE LIBRARY.

                xWr.WriteEndDocument();         // END DOCUMENT.

                // FLUSH AND CLOSE.
                xWr.Flush();
                xWr.Close();
            }

        }
        [System.Web.Services.WebMethod]
        public static string Savelayer(string xml, string xml1, string color)
        {
            savelayer _save = new savelayer();
            _save.SaveFile(xml, xml1, color);
            return "Save layer Success " + DateTime.Now.ToString();
        }
        [System.Web.Services.WebMethod]
        public static string Consolidate()
        {
            savelayer _save = new savelayer();
            _save.SaveConsolidate();
            return "Consolidate layer Success " + DateTime.Now.ToString();
        }

    }
}