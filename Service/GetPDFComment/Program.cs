using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

//using iTextSharp.text.pdf;

namespace GetPDFComment
{
    class Program
    {
        static void Main(string[] args)
        {
            //PdfReader pdfRead = new FdfReader(@"D:\01. Desktop\Temp Files\App_SalonCare.pdf");
            //for (int page = 1; page <= pdfRead.NumberOfPages; ++page)
            //{
            //    PdfDictionary pagedic = pdfRead.GetPageN(page);
            //    PdfArray annotarray = (PdfArray) PdfReader.GetPdfObject(pagedic.Get(PdfName.ANNOTS));

            //    string all_string = "";
            //    foreach (PdfObject A in annotarray)
            //    {
            //        PdfDictionary AnnotationDictionary = (PdfDictionary)PdfReader.GetPdfObject(A);

            //        if (AnnotationDictionary.Get(PdfName.SUBTYPE).Equals(PdfName.TEXT))
            //        {
            //            all_string += AnnotationDictionary.GetAsString(PdfName.T).ToString() + "\n";
            //            all_string += AnnotationDictionary.GetAsString(PdfName.CONTENTS).ToString() + "\n";
            //        }
            //    }
            //}

            using (PdfDocument one = PdfReader.Open(@"D:\CMMS_BM01_20219091405.pdf", PdfDocumentOpenMode.Import))
            using (PdfDocument two = PdfReader.Open(@"D:\temp.pdf", PdfDocumentOpenMode.Import))
            using (PdfDocument outPdf = new PdfDocument())
            {
                CopyPages(one, outPdf);
                CopyPages(two, outPdf);

                outPdf.Save(@"D:\Merge.pdf");
            }

            void CopyPages(PdfDocument from, PdfDocument to)
            {
                for (int i = 0; i < from.PageCount; i++)
                {
                    to.AddPage(from.Pages[i]);
                }
            }


        }
    }
}
