using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;



namespace Clarity_Crate.Services
{
   
    public class FileService
{
        
        public string ExtractTextFromPdf(Stream pdfStream)
        {
            string extractedText = string.Empty;
            using (PdfReader reader = new PdfReader(pdfStream))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    extractedText += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                }
            }


            return extractedText;
           
        }
    }
}
