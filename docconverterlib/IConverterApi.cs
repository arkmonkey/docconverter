
using System;
using System.Collections.Generic;

namespace docconverterlib
{
    public interface IConverterApi
    {
        void Execute(ConversionFile toBeConverted);
        Action<ConversionFile, IEnumerable<ConversionFile>> Callback { get; set; }
    }
}
