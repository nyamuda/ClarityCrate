using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using IronOcr;
using Clarity_Crate.Data;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;


namespace Clarity_Crate.Services
{
    public class FileService
    {

        private readonly ApplicationDbContext _context;
        private readonly AppService _appService;


        public FileService(IConfiguration configuration, ApplicationDbContext context, AppService appService)
        {
            string licenseKey = configuration["Authentication:IronOcr:LicenseKey"];

            IronOcr.License.LicenseKey = licenseKey;

            _context = context;
            _appService = appService;
        }

        // Method to extract text from PDFs (normal and scanned)
        public string ExtractTextFromPdf(Stream pdfStream)
        {
            string extractedText = string.Empty;

            try
            {
                // Attempt to extract text using iText
                using (PdfReader reader = new PdfReader(pdfStream))
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))
                {
                    for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                    {
                        extractedText += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                    }
                }

                // If text is not extracted, fallback to OCR
                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    //show snack bar to notify the user OCR has started
                    string message = "This appears to be a scanned document. Text extraction might take a little longer. Please wait.";
                    _appService.ShowSnackBar(message: message, severity: "info");
                    extractedText = PerformOcrOnPdf(pdfStream);
                }
            }
            catch (Exception ex)
            {
                //show snack bar to notify the user OCR has started
                string message = "Text extraction failed. This document may not be supported.";
                _appService.ShowSnackBar(message: message, severity: "error");
                
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
                    using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
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
                summary.NumDocumentsSummarized += 1;

                _context.Summary.Update(summary);
                await _context.SaveChangesAsync();
            }


        }
        public Stream ConvertExcelToPdf(Stream excelFileStream)
        {
            // Ensure the input stream is not null
            if (excelFileStream == null)
                throw new ArgumentNullException(nameof(excelFileStream));

            // Initialize the Excel engine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                // Open the Excel workbook from the stream
                IWorkbook workbook = application.Workbooks.Open(excelFileStream);

                // Initialize the XlsIO renderer
                XlsIORenderer renderer = new XlsIORenderer();

                // Convert the Excel document into a PDF document
                Syncfusion.Pdf.PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

                // Save the PDF document into a memory stream
                MemoryStream pdfStream = new MemoryStream();
                pdfDocument.Save(pdfStream);

                // Reset the stream position to the beginning
                pdfStream.Position = 0;

                return pdfStream;
            }
        }

    }
}
