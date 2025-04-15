using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.ResponseParser
{
    public interface IResponseParser
    {
        public IAsyncEnumerable<RawQuestion> Parse(Stream response);
    }
}
