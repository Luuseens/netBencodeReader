using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netBencodeReader.Types;

namespace yetAnotherGenericDeserAttempt
{
    public class BencodeContainerContract : BencodeContract
    {
        internal BencodeContract ItemContract { get; set; }

        public BencodeConverter ItemConverter { get; set; }
        

        public BencodeContainerContract(Type underlyingType)
            : base(underlyingType)
        {
            /*JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);

            if (jsonContainerAttribute != null)
            {
                if (jsonContainerAttribute.ItemConverterType != null)
                {
                    ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(
                        jsonContainerAttribute.ItemConverterType,
                        jsonContainerAttribute.ItemConverterParameters);
                }

                ItemIsReference = jsonContainerAttribute._itemIsReference;
                ItemReferenceLoopHandling = jsonContainerAttribute._itemReferenceLoopHandling;
                ItemTypeNameHandling = jsonContainerAttribute._itemTypeNameHandling;
                }
                */
        }

        public override bool CanDeserializeFrom(BEBaseObject sourceObject)
        {
            // Container contract can't deserialize from anything
            return false;
        }

        public override object Deserialize(BEBaseObject beBaseObject)
        {
            return null;
        }
    }
}
