using netBencodeReader.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netBencodeReader.InternalDeser;
using netBencodeReader.Tokenizer;
using yetAnotherGenericDeserAttempt.Contracts;

namespace yetAnotherGenericDeserAttempt
{
    class BencodeConvert
    {
        private ContractResolver contractResolver = new ContractResolver();

        public T Deserialize<T>(string value)
        {
            return (T)this.Deserialize(value, typeof(T));
        }

        // TODO: string value might already be a bencode val
        public object Deserialize(string value, Type type)
        {
            var tokenizer = BencodeReader.Create(new StringReader(value));
            var deser = new Deserializer();
            var bo = deser.GetBaseObject(tokenizer);
            return this.Deserialize(bo, type);
        }

        private object Deserialize(BEBaseObject r, Type type)
        {
            var contract = contractResolver.ResolveContract(type);
            if (contract == null)
            {
                if (Nullable.GetUnderlyingType(type) == null)
                {
                    throw new Exception($"Can't deserialize non-nullable type {type.Name}.");
                }
                else
                {
                    // TODO: flag to check if nulls are ok..
                    return null;
                }
            }

            if (contract.CanDeserializeFrom(r))
            {
                return contract.Deserialize(r);
            }
            else
            {
                throw new Exception($"Type mismatch, can't deserialize type {type.Name} twith contract {contract}.");
            }
             

            return null;

        }
    }
}
