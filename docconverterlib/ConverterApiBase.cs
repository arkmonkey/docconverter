using System;
using System.Collections.Generic;
using System.IO;


namespace docconverterlib
{
    public abstract class ConverterApiBase : IConverterApi
    {
        public abstract void Execute(ConverterFile toBeConverted);
        public Action<ConverterFile, IEnumerable<ConverterFile>> Callback { get; set; }
    }
}
