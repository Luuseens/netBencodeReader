using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netBencodeReader.Types;

namespace yetAnotherGenericDeserAttempt
{
    internal enum ReadType
    {
        Read,
        ReadAsInt32,
        ReadAsInt64,
        ReadAsBytes,
        ReadAsString,
        ReadAsDecimal,
        ReadAsDateTime,
        ReadAsDouble,
        ReadAsFloat,
        ReadAsBoolean
    }

    class BencodePrimitiveContract : BencodeContract
    {
        public BencodePrimitiveContract(Type underlyingType)
            : base(underlyingType)
        {
            this.ContractType = BencodeContractType.Primitive;
            
            if (ReadTypeMap.TryGetValue(this.UnderlyingType, out ReadType readType))
            {
                this.InternalReadType = readType;
            }
        }

        internal ReadType InternalReadType;

        private static readonly Dictionary<Type, ReadType> ReadTypeMap = new Dictionary<Type, ReadType>
        {
            [typeof(byte[])] = ReadType.ReadAsBytes,
            [typeof(byte)] = ReadType.ReadAsInt32,
            [typeof(short)] = ReadType.ReadAsInt32,
            [typeof(int)] = ReadType.ReadAsInt32,
            [typeof(decimal)] = ReadType.ReadAsDecimal,
            [typeof(bool)] = ReadType.ReadAsBoolean,
            [typeof(string)] = ReadType.ReadAsString,
            //[typeof(DateTime)] = ReadType.ReadAsDateTime,
            [typeof(float)] = ReadType.ReadAsFloat,
            [typeof(double)] = ReadType.ReadAsDouble,
            [typeof(long)] = ReadType.ReadAsInt64
        };

        public override bool CanDeserializeFrom(BEBaseObject sourceObject)
        {
            if (sourceObject is BENumber)
            {
                return this.InternalReadType == ReadType.ReadAsDecimal
                       || this.InternalReadType == ReadType.ReadAsDouble
                       || this.InternalReadType == ReadType.ReadAsFloat
                       || this.InternalReadType == ReadType.ReadAsInt32
                       || this.InternalReadType == ReadType.ReadAsInt64
                       || this.InternalReadType == ReadType.ReadAsString;
            }

            if (sourceObject is BEByteString)
            {
                return this.InternalReadType == ReadType.ReadAsString
                       || this.InternalReadType == ReadType.ReadAsBytes
                       || this.InternalReadType == ReadType.ReadAsBoolean;
            }

            return false;
        }


        public override object Deserialize(BEBaseObject beBaseObject)
        {
            if (!this.CanDeserializeFrom(beBaseObject))
            {
                return null;
            }

            if (beBaseObject is BEByteString beString)
            {
                if (this.InternalReadType == ReadType.ReadAsString)
                {
                    return beString.ToString();
                }

                if (this.InternalReadType == ReadType.ReadAsBytes)
                {
                    return beString.Value;
                }

                if (this.InternalReadType == ReadType.ReadAsBoolean)
                {
                    return bool.Parse(beString.ToString());
                }
            }

            if (beBaseObject is BENumber beNumber)
            {
                if (this.InternalReadType == ReadType.ReadAsInt64)
                {
                    return beNumber.ToLong();
                }

                if (this.InternalReadType == ReadType.ReadAsInt32)
                {
                    return (int)beNumber.ToLong();
                }

                if (this.InternalReadType == ReadType.ReadAsDecimal)
                {
                    return (decimal)beNumber.ToLong();
                }
                
                if (this.InternalReadType == ReadType.ReadAsDouble)
                {
                    return (double)beNumber.ToLong();
                }
                
                if (this.InternalReadType == ReadType.ReadAsFloat)
                {
                    return (float)beNumber.ToLong();
                }
                
                if (this.InternalReadType == ReadType.ReadAsString)
                {
                    return beNumber.ToLong().ToString();
                }
            }

            return null;
        }
    }
}
