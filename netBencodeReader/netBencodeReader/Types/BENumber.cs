using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netBencodeReader.Types
{
    public class BENumber: BEBaseObject
    {
        private readonly string raw;

        public BENumber(string source)
        {
            this.raw = source;
        }

        public override string ToString()
        {
            return this.raw;
        }

        public int ToInt()
        {
            return int.Parse(this.raw);
        }

        public long ToLong()
        {
            return long.Parse(this.raw);
        }
    }
}
