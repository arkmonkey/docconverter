using System;

namespace FileConverter.Converter
{
    public class Converter
    {
        public delegate void ConversionStartedDelegate(string identifier);
        public delegate void ConversionDoneDelegate(string identifier);
        public delegate void ConversionErrorDelegate(string identifier);

        public event ConversionStartedDelegate ConversionStarted;
        public event ConversionDoneDelegate ConversionDone;
        public event ConversionErrorDelegate ConversionError;


        private string _fileName;

        public Converter(string fileName)
        {
            _fileName = fileName;
        }

        public void Convert()
        {
            ConversionStarted(_fileName);
            try
            {
                DoConvert();
                ConversionDone(_fileName);
            }
            catch (Exception ex)
            {
                ConversionError(_fileName);
            }
        }

        protected void DoConvert()
        {
            //call the PIA + Ghostscript library here
        }
    }
}
