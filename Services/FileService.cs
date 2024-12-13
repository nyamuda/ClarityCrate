using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Layout.Element;
using iText.Layout;
using iText.Pdfocr;


namespace Clarity_Crate.Services
{

    public class FileService
    {
        private SummarizationService _summarizationService;

        public FileService(SummarizationService summarizationService)
        {
            _summarizationService = summarizationService;
        }

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

        public byte[] GeneratePdf(string text)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Create the PDF
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        document.Add(new Paragraph(text));
                        document.Close();
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}