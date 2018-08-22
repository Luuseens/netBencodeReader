using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yetAnotherGenericDeserAttempt
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = "i25e";
            var convert = new BencodeConvert();

            // TODO : var val = convert.Deserialize<bool?>(s);
            var val = convert.Deserialize<decimal>(s);
        }
    }
}
