using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace docconverterlib
{
    public class MimeType
    {
        public string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                return string.Empty;
            }

            string mimeType = string.Empty;
            fileExtension = fileExtension.ToLowerInvariant();
            switch (fileExtension)  //See comment below for list of office-related extensions
            {
                case "docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                
            }

            return mimeType;
        }

        /*
            FROM: http://stackoverflow.com/questions/4212861/what-is-a-correct-mime-type-for-docx-pptx-etc
         
            .xlsx   application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            .xltx   application/vnd.openxmlformats-officedocument.spreadsheetml.template
            .potx   application/vnd.openxmlformats-officedocument.presentationml.template
            .ppsx   application/vnd.openxmlformats-officedocument.presentationml.slideshow
            .pptx   application/vnd.openxmlformats-officedocument.presentationml.presentation
            .sldx   application/vnd.openxmlformats-officedocument.presentationml.slide
            .docx   application/vnd.openxmlformats-officedocument.wordprocessingml.document
            .dotx   application/vnd.openxmlformats-officedocument.wordprocessingml.template
            .xlsm   application/vnd.ms-excel.addin.macroEnabled.12
            .xlsb   application/vnd.ms-excel.sheet.binary.macroEnabled.12
            .doc    application/msword
            .dot    application/msword
            .xls    application/vnd.ms-excel
            .xlt    application/vnd.ms-excel
            .xla    application/vnd.ms-excel
            .ppt    application/vnd.ms-powerpoint
            .pot    application/vnd.ms-powerpoint
            .pps    application/vnd.ms-powerpoint
            .ppa    application/vnd.ms-powerpoint
         */
    }
}
