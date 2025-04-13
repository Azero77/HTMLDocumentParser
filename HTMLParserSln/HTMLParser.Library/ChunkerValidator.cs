using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Library
{
    public class ChunkerValidator
    {
        public event Action<ValidationResult>? Validated;
        private void OnValidate(ValidationResult result)
        {
            Validated?.Invoke(result);
        }

        public void Validate(string chunk)
        {

        }
    }
}
