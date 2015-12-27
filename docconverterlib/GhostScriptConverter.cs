using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Ghostscript.NET;

namespace docconverterlib
{
    internal class GhostScriptConverter : ConverterApiBase
    {
        private readonly string _rootOutputPath;

        internal GhostScriptConverter(string rootOutputPath)
        {
            _rootOutputPath = rootOutputPath;
        }

        public override void Execute(ConversionFile toBeConverted)
        {

            var outputDir = GetOutputDir(toBeConverted);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            var fileNames = PdfToImages(toBeConverted.Path, outputDir, 120);

            List<ConversionFile> outputFiles = new List<ConversionFile>();
            foreach (string file in fileNames)
            {
                ConversionFile f = new ConversionFile
                {
                    Path = file
                };
                outputFiles.Add(f);
            }

            this.RaiseCallbackEvent(toBeConverted, outputFiles);
        }

        private string GetOutputDir(ConversionFile toBeConverted)
        {
            if (toBeConverted == null)
            {
                return string.Empty;
            }

            //string hash = Hash.GetEncodedHash(toBeConverted.RowId);
            return Path.Combine(
                _rootOutputPath,
                toBeConverted.GetHashBasedOfRowId(),
                "images");
        }

        protected List<string> PdfToImages(string file, string outputDir, int dpi)
        {
            try
            {
                List<string> fileNames = new List<string>();

                Ghostscript.NET.Rasterizer.GhostscriptRasterizer rasterizer = null;

                string gsDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Binaries\gsdll32.dll");
                GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);

                using (rasterizer = new Ghostscript.NET.Rasterizer.GhostscriptRasterizer())
                {
                    rasterizer.Open(file, gvi, true);
                    
                    for (int i = 1; i <= rasterizer.PageCount; i++)
                    {
                        System.Drawing.Image img = rasterizer.GetPage(dpi, dpi, i);
                        string fileName = Path.Combine(outputDir, string.Format("{0}.jpg", i));
                        img.Save(fileName, ImageFormat.Jpeg);
                        fileNames.Add(fileName);
                    }

                    rasterizer.Close();
                }

                return fileNames;
            }
            catch (Exception ex)
            {
                //potentially log the exception
                throw;
            }

        }
    }
}
