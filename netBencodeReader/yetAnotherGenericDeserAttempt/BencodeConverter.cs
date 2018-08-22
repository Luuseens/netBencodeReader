using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yetAnotherGenericDeserAttempt
{
    public abstract class BencodeConverter
    {
        public abstract bool CanConvert(Type objectType);

        public abstract object ReadBencode(/*JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer*/);
    }
}
