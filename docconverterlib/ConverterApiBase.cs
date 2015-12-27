using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace docconverterlib
{
    public abstract class ConverterApiBase : IConverterApi
    {
        public abstract void Execute(ConversionFile toBeConverted);
        public Action<ConversionFile, IEnumerable<ConversionFile>> Callback { get; set; }

        /// <summary>
        /// Subclasses should call this, instead of "Callback()" directly.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected void RaiseCallbackEvent(ConversionFile src, IEnumerable<ConversionFile> dest)
        {
            if (this.Callback != null)
            {
                Task.Run(() => this.Callback(src, dest));
            }
        }
    }
}
