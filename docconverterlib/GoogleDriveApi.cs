
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v2.Data.File;

namespace docconverterlib
{
    public class GoogleDriveApi : ConverterApiBase
    {
        private DriveService _service;
        private readonly string[] _scopes = {DriveService.Scope.DriveFile};
        private readonly string _appName = "";
        private readonly string _user = "user";
        private readonly string _privateKeyPath;

        

        
        

        internal GoogleDriveApi(string appName, string privateKeyPath = "")
        {
            _appName = appName;
            _privateKeyPath = privateKeyPath;
        }

        public File UploadFile(Stream stream, string remoteFilename, string mimeType, bool convert = false, string folderId = "")
        {
            File fileInfo = new File();
            fileInfo.Title = remoteFilename;
            fileInfo.MimeType = mimeType;
            if (!string.IsNullOrWhiteSpace(folderId))
            {
                fileInfo.Parents = new[] {new ParentReference {Id = folderId}};
            }

            try
            {
                var insertRequest = this.GoogleDriveService.Files.Insert(fileInfo, stream, mimeType);
                insertRequest.Convert = convert;
                insertRequest.Upload();
                File returnedFile = insertRequest.ResponseBody;
                return returnedFile;
            }
            catch (Exception ex)
            {
                LogError(string.Format("Inserting file {0} threw an exception: {1}", remoteFilename, ex.Message));
            }
            return null;
        }

        public Stream DownloadPdf(string fileId)
        {
            var getRequest = this.GoogleDriveService.Files.Get(fileId);
            File fileInfo = getRequest.Execute();

            var response = this.GoogleDriveService.HttpClient.GetByteArrayAsync(fileInfo.ExportLinks["application/pdf"]);
            byte[] bytes = response.Result;

            return new MemoryStream(bytes);
        }

        public string DeleteFile(string fileId)
        {
            var deleteRequest = this.GoogleDriveService.Files.Delete(fileId);
            string result = deleteRequest.Execute();

            return result;
        }


        private DriveService GoogleDriveService {
            get
            {
                if (_service == null)
                {
                    _service = InstantiateService(_appName, _privateKeyPath, _user, _scopes);
                }
                return _service;
            }
        }
        private static DriveService InstantiateService(string appName, string privateKeyPath, string user, string[] scopes)
        {
            UserCredential credential;
            using (var stream =
                new FileStream(privateKeyPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/" + appName);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    user,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            DriveService service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName,
            });

            return service;
        }

        private static void LogError(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
            throw new NotImplementedException();
        }

        #region IConverterApi

        public override void Execute(ConversionFile toBeConverted)
        {
            File file = this.UploadFile(toBeConverted.GetStream(), toBeConverted.FileName, toBeConverted.MimeType, true);
            Stream stream = this.DownloadPdf(file.Id);

            //TODO: I wonder what the effect would be if the converter callback used QueueBackgroundWorkItem too...
            if (this.Callback != null)
            {
                var dir = Path.GetDirectoryName(toBeConverted.Path);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    var output = ConversionFile.SaveAs(stream,
                    Path.Combine(dir, Path.GetFileNameWithoutExtension(toBeConverted.Path) + ".pdf"));
                    Callback(toBeConverted, new List<ConversionFile>(new[] { output }).AsEnumerable());
                }
                
            }
        }
        #endregion
    }
}
