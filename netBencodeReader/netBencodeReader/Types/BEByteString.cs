using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netBencodeReader.Types
{
    public class BEByteString : BEBaseObject
    {
        public byte[] Value { get; }

        public BEByteString(byte[] bytes)
        {
            this.Value = bytes;
        }

        public override bool Equals(object obj)
        {
            var bebs2 = obj as BEByteString;

            if (this.Value == null || bebs2?.Value.Length != this.Value.Length)
            {
                return false;
            }

            for (int i = 0; i < this.Value.Length; i++)
            {
                if (this.Value[i] != bebs2.Value[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(this.Value);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
