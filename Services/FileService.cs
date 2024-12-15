using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using IronOcr;
using Clarity_Crate.Data;
using Microsoft.EntityFrameworkCore;

namespace Clarity_Crate.Services
{
    public class FileService
    {

        private readonly ApplicationDbContext _context;


        public FileService(IConfiguration configuration, ApplicationDbContext context)
        {
            string licenseKey = configuration["Authentication:IronOcr:LicenseKey"];
            
            IronOcr.License.LicenseKey = licenseKey;

            _context = context;
        }

        // Method to extract text from PDFs (normal and scanned)
        public string ExtractTextFromPdf(Stream pdfStream)
        {
            string extractedText = string.Empty;

            try
            {
                // Attempt to extract text using iText
                using (PdfReader reader = new PdfReader(pdfStream))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                    {
                        extractedText += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                    }
                }

                // If text is not extracted, fallback to OCR
                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    extractedText = PerformOcrOnPdf(pdfStream);
                }
            }
            catch (Exception ex)
            {
                // Log and handle errors
                extractedText = $"Error during text extraction: {ex.Message}";
            }

            return extractedText;
        }

        // Fallback method: Perform OCR using IronOCR
        private string PerformOcrOnPdf(Stream pdfStream)
        {
            string ocrText = string.Empty;

            try
            {
                var ocrTesseract = new IronTesseract();

                using (var ocrInput = new OcrInput())
                {
                    // Reload the PDF stream because it may have been consumed
                    pdfStream.Position = 0;

                    // Load the PDF into the OCR input
                    ocrInput.LoadPdf(pdfStream);

                    // Perform OCR and extract text
                    var ocrResult = ocrTesseract.Read(ocrInput);
                    ocrText = ocrResult.Text;
                }
            }
            catch (Exception ex)
            {
                // Handle OCR errors
                ocrText = $"Error during OCR: {ex.Message}";
            }

            return ocrText;
        }

        // Method to generate a PDF from text
        public byte[] GeneratePdf(string text)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Create the PDF
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new iText.Layout.Document(pdf);
                        document.Add(new iText.Layout.Element.Paragraph(text));
                        document.Close();
                    }
                }

                return memoryStream.ToArray();
            }
        }
        //Increment the number of documents summarized
        public async Task IncrementDocumentCount()
        {
            

            var summary = await _context.Summary.FirstOrDefaultAsync();
            if (summary != null)
            {
                summary.NumDocumentsSummarized+=1;

                _context.Summary.Update(summary);
                await _context.SaveChangesAsync();
            }
            

        }
    }
}
