using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yetAnotherGenericDeserAttempt.Contracts
{
    class ContractResolver
    {
        private Dictionary<Type, BencodeContract> contractCache;

        private static Type[] primitives = new[]
        {
            typeof(bool),
            //typeof(char),
            //typeof(sbyte),
            typeof(byte),
            typeof(byte[]),
            typeof(short),
            //typeof(ushort),
            typeof(int),
            //typeof(uint),
            typeof(long),
            //typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            //typeof(DateTime),
            typeof(string)
        };

        public ContractResolver()
        {
            this.contractCache = new Dictionary<Type, BencodeContract>();
        }

        public BencodeContract ResolveContract(Type t)
        {
            // TODO: threadsafe
            if (!this.contractCache.TryGetValue(t, out var resolvedContract))
            {
                resolvedContract = this.CreateContract(t);
                this.contractCache[t] = resolvedContract;
            }

            return resolvedContract;
        }

        private BencodeContract CreateContract(Type t)
        {
            if (IsPrimitiveType(t))
            {
                return CreatePrimitiveContract(t);
            }

            return null;
        }

        private BencodePrimitiveContract CreatePrimitiveContract(Type objectType)
        {
            var contract = new BencodePrimitiveContract(objectType);
            return contract;
        }

        private bool IsPrimitiveType(Type t)
        {
            return primitives.Contains(t);
        }
    }
}
