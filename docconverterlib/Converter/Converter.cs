using System;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

namespace FileConverter.Converter
{
    public class Converter
    {
        private const int DPI = 300;
        public delegate void ConversionStartedDelegate(string identifier);
        public delegate void ConversionDoneDelegate(string identifier);
        public delegate void ConversionErrorDelegate(string identifier);

        public event ConversionStartedDelegate ConversionStarted;
        public event ConversionDoneDelegate ConversionDone;
        public event ConversionErrorDelegate ConversionError;


        private string _fileName;
        private string _outputFileName;

        public Converter(string fileName, string outputFileName)
        {
            _fileName = fileName;
            _outputFileName = outputFileName;
        }

        public void Convert()
        {
            if(ConversionStarted != null) { ConversionStarted(_fileName); }
            try
            {
                DoConvert();
                if(ConversionDone != null) { ConversionDone(_fileName); }
            }
            catch (Exception ex)
            {
                if(ConversionError != null) { ConversionError(_fileName); }
            }
        }

        protected void DoConvert()
        {
            string tempFileName = _outputFileName + ".pdf";
            OfficeFileToPdf(_fileName, tempFileName);
            PdfToImage(tempFileName, _outputFileName, DPI);
            File.Delete(tempFileName);
        }


        protected static void PdfToImage(string file, string outputPath, int dpi)
        {
            //string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Ghostscript.NET.Rasterizer.GhostscriptRasterizer rasterizer = null;
            //Ghostscript.NET.GhostscriptVersionInfo vesion = new Ghostscript.NET.GhostscriptVersionInfo(new System.Version(0, 0, 0), path + @"\gsdll32.dll", string.Empty, Ghostscript.NET.GhostscriptLicense.GPL);

            using (rasterizer = new Ghostscript.NET.Rasterizer.GhostscriptRasterizer())
            {
                rasterizer.Open(file);

                string actualFileName;
                for (int i = 1; i <= rasterizer.PageCount; i++)
                {
                    //string pageFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file) + "-p" + i.ToString() + ".jpg");

                    System.Drawing.Image img = rasterizer.GetPage(dpi, dpi, i);
                    //img.Save(pageFilePath, ImageFormat.Jpeg);
                    int dotIndex = outputPath.LastIndexOf(".", System.StringComparison.InvariantCultureIgnoreCase);
                    if (dotIndex > -1)
                    {
                        actualFileName = outputPath.Substring(0, dotIndex) + "-" + i.ToString() + "." +
                                         outputPath.Substring(dotIndex + 1);
                    }
                    else
                    {
                        actualFileName = outputPath + "-" + i.ToString();
                    }
                    img.Save(actualFileName);

                }

                rasterizer.Close();
            }
        }

        protected static void OfficeFileToPdf(string filename, string pdfPath)
        {
            filename = filename.ToLower();
            if (filename.EndsWith(".doc") || filename.EndsWith(".docx"))
            {
                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                var wordDocument = appWord.Documents.Open(filename, false, true, false);

                wordDocument.ExportAsFixedFormat(pdfPath, WdExportFormat.wdExportFormatPDF);
            }
            else if (filename.EndsWith(".txt"))
            {
                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                var wordDocument = appWord.Documents.Open(filename, false, true, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, WdOpenFormat.wdOpenFormatText);

                wordDocument.ExportAsFixedFormat(pdfPath, WdExportFormat.wdExportFormatPDF);

            }
            else if (filename.EndsWith(".xls") || filename.EndsWith(".xlsx"))
            {
                Microsoft.Office.Interop.Excel.Application appExcel = new Microsoft.Office.Interop.Excel.Application();
                var excelDocument = appExcel.Workbooks.Open(filename, false, true);
                excelDocument.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdfPath);
            }
            else
            {
                throw new Exception("Unrecognized file extension.  Conversion failed.");
            }
        }
    }
}
