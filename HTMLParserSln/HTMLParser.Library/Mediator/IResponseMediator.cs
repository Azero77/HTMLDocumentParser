using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library.Mediator
{
    public interface IResponseMediator
    {
        /// <summary>
        /// Handler coming out of IResponseParser to IPrompter to add the lastQuestion of the the last prompt to the new prompt for validating
        /// </summary>
        event Action<RawQuestion> ResponseParsed;

        void OnResponseParsed(RawQuestion lastQuestion);
    }
}
