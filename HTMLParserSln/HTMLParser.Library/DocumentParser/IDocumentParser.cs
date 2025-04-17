using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.DocumentParser
{
    /// <summary>
    /// The main api for parsing html files into json
    /// </summary>
    public interface IDocumentParser : IDisposable
    {
        Task Parse(string filePath);
    }
}
