using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netBencodeReader.InternalDeser;
using netBencodeReader.Tokenizer;

namespace bencodeConsole
{
    class Blah
    {
        public string A { get; set; }

        public string B { get; private set; } = "Assd";
    }

    class Program
    {


        static void Main(string[] args)
        {
            var s = "l4:spam2:mei29ei11ee";

            var tokenizer = BencodeReader.Create(new StringReader(s));
            var deser = new Deserializer();
            var bo = deser.GetBaseObject(tokenizer);

            var b = new Blah();
            IEnumerable<int> z;

            var rez = ToObject.Deserialize(bo, out b);
        }
    }
}
