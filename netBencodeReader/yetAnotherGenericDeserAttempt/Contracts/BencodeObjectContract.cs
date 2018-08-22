using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netBencodeReader.Types;

namespace yetAnotherGenericDeserAttempt
{
    public class BencodeObjectContract : BencodeContainerContract
    {
        public BencodeObjectContract(Type underlyingType)
            : base(underlyingType)
        {
            this.ContractType = BencodeContractType.Object;

            this.Properties = new Dictionary<string, BencodeProperty>();
        }

        public Dictionary<string, BencodeProperty> Properties { get; }

        public override bool CanDeserializeFrom(BEBaseObject sourceObject)
        {
            return sourceObject is BEDictionary;
        }
    }

}
