using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yetAnotherGenericDeserAttempt
{
    public class BencodeProperty
    {
        public string PropertyName { get; set; }

        public Action ValueSetter { get; set; }
    }
}
