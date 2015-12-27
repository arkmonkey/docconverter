using System;
using System.IO;

namespace docconverterlib
{
    /// <summary>
    /// This class returns references to various file format converters.
    /// 
    /// * the google api converter needs a "myprivatekey.p12" file located at the domain's root dir.
    /// This is obtained from your google accounts developer console service account creation page.
    /// </summary>
    public class ConverterApiFactory
    {
        private readonly string _rootOutputPath;
        
        public ConverterApiFactory(string rootOutputPath)
        {
            _rootOutputPath = rootOutputPath;
        }

        public IConverterApi GetConverterApi(
            string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                throw new Exception("must pass a valid extension.");
            }
            fileExtension = MassageFileExtension(fileExtension);

            switch (fileExtension)
            {
                case "doc":
                case "docx":
                case "xls":
                case "xlsx":
                    return GetGoogleDriveApi();
                case "pdf":
                    return GetGhostScriptApi();
                default:
                    //TODO: error-log when it reaches this file extension
                    throw new Exception("file not supported at the moment");
            }
        }

        internal GoogleDriveApi GetGoogleDriveApi()
        {
            const string appName = "docconverter";

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "myprivatekey.p12");
            GoogleDriveApi driveApi = new GoogleDriveApi(appName, path);

            return driveApi;
        }

        internal GhostScriptConverter GetGhostScriptApi()
        {
            return new GhostScriptConverter(_rootOutputPath);
        }

        #region Helpers

        private string MassageFileExtension(string ext)
        {
            return ext.Substring(ext.StartsWith(".") ? 1 : 0).ToLowerInvariant();
        }

        #endregion //Helpers
    }
}
