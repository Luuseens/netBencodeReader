using System;
using System.Linq;
using System.Reflection;
using netBencodeReader.Types;

namespace yetAnotherGenericDeserAttempt
{
    internal enum BencodeContractType
    {
        None = 0,
        Object = 1,
        Array = 2,
        Primitive = 3,
        String = 4,
        Dictionary = 5,
        Serializable = 6,
        // TODO: Dynamic
    }

    public abstract class BencodeContract //TODO : why not <T> ?
    {
        internal BencodeContractType ContractType;

        // internal bool IsNullable;

        internal bool IsEnum;

        public Type UnderlyingType { get; }

        internal bool IsInstantiable => this.UnderlyingType.IsInterface || this.UnderlyingType.IsAbstract;

        public BencodeConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the default creator method used to create the object.
        /// </summary>
        /// <value>The default creator method used to create the object.</value>
        public Func<object> DefaultCreator { get; set; } // TODO: why not <T>?
          
        public BencodeContract(Type underlyingType)
        {
            this.UnderlyingType = underlyingType;
            IsEnum = this.UnderlyingType.IsEnum;
            this.SetDefaultConstructor();
           
            //IsNullable = ReflectionUtils.IsNullable(underlyingType);

        }

        public abstract bool CanDeserializeFrom(BEBaseObject sourceObject);

        private void SetDefaultConstructor()
        {
            if (this.UnderlyingType.IsValueType)
            {
                this.DefaultCreator = () => Activator.CreateInstance(this.UnderlyingType);
            }

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            /*if (nonPublic)
            {
                bindingFlags = bindingFlags | BindingFlags.NonPublic;
            }*/

            // Get the default non-public constructor that takes no params
            var constructorInfo = this.UnderlyingType.GetConstructors(bindingFlags).SingleOrDefault(c => !c.GetParameters().Any());

            this.DefaultCreator = () => constructorInfo?.Invoke(null);
        }

        public abstract object Deserialize(BEBaseObject beBaseObject);
    }
}