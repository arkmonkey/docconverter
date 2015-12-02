using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace docconverterlib
{
    public class ConverterApiFactory
    {
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
                case "docx":
                    return GetGoogleDriveApi();
                default:
                    //TODO: error-log when it reaches this file extension
                    throw new Exception("file not supported at the moment");
            }
        }

        protected GoogleDriveApi GetGoogleDriveApi()
        {
            const string appName = "malzahar";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "client_secret.json");

            GoogleDriveApi driveApi = new GoogleDriveApi(appName, path);  //TODO: Is this really hardcoded?

            return driveApi;
        }

        #region Helpers

        private string MassageFileExtension(string ext)
        {
            return ext.Substring(ext.StartsWith(".") ? 1 : 0).ToLowerInvariant();
        }

        #endregion //Helpers
    }
}
