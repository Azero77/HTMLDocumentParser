using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Formatter
{
    public interface IResponseFormatter
    {
        IAsyncEnumerable<Question> Format(Stream response);
    }
}
