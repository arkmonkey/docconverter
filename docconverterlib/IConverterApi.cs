
using System;
using System.Collections.Generic;

namespace docconverterlib
{
    public interface IConverterApi
    {
        void Execute(ConverterFile toBeConverted);
        Action<ConverterFile, IEnumerable<ConverterFile>> Callback { get; set; }
    }
}
