
using System;
using System.IO;

namespace docconverterlib
{
    public class ConverterFile
    {
        public enum FileType
        {
            Local,
            AzureCloud
        }

        private Stream _stream;
        private string _path;

        public ConverterFile()
        {
            Type = FileType.Local;
            Path = string.Empty;
            _stream = null;
        }

        public Guid RowId { get; set; }             //The database identity field

        public string GetHash()
        {
            return Convert.ToBase64String(RowId.ToByteArray()); 
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value; 
                
                //update mime type
                var ext = System.IO.Path.GetExtension(this.Path);
                if (!string.IsNullOrWhiteSpace(ext))
                {
                    MimeType mt = new MimeType();
                    MimeType = mt.GetMimeType(ext.Substring(1));        //exclude the "."
                }
            }
        }

        public FileType Type { get; set; }
        

        public string FileName
        {
            get { return System.IO.Path.GetFileName(this.Path); }
        }


        public string MimeType { get; set; }

        public Stream GetStream ()
        {
            return _stream ?? (_stream = InternalGetStream());
        }

        /// <summary>
        /// This could return the local file stream, or a web stream to a cloud storage
        /// </summary>
        /// <returns></returns>
        private Stream InternalGetStream()
        {
            return new MemoryStream(System.IO.File.ReadAllBytes(this.Path));
        }


        #region static method/s

        public static ConverterFile SaveAs(Stream stream, string path)
        {
            try
            {
                using (var fileStream = System.IO.File.Create(path))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
                ConverterFile f = new ConverterFile
                {
                    Path = path,
                    Type = FileType.Local
                };
                return f;
            }
// ReSharper disable once RedundantCatchClause
            catch (Exception ex)
            {
                //TODO: log File.SaveAs errors
                System.Diagnostics.Debug.WriteLine("SaveAs() Exception: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
