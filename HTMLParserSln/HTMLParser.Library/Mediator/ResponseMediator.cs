using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Mediator
{
    internal class ResponseMediator : IResponseMediator
    {
        public event Action<RawQuestion>? ResponseParsed;

        public void OnResponseParsed(RawQuestion lastQuestion)
        {
            ResponseParsed?.Invoke(lastQuestion);
        }
    }
}
