using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using netBencodeReader.Tokenizer;
using netBencodeReader.Types;

namespace netBencodeReader.InternalDeser
{
    internal static class ReflectionUtils
    {
        public static bool IsNullable(Type t)
        {
            if (t.IsValueType)
            {
                return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return true;
        }
    }

    /*
    internal class SerdeContract
    {
        internal Type UnderlyingType { get; set; }
        internal bool IsConvertable;
        internal bool IsNullable;
        internal bool DefaultCreatorNonPublic { get; set; }

        /// <summary>
        /// Gets or sets the default creator method used to create the object.
        /// </summary>
        /// <value>The default creator method used to create the object.</value>
        internal Func<object> DefaultCreator { get; set; }

        internal SerdeContract(Type underlyingType)
        {
            UnderlyingType = underlyingType;

            IsNullable = ReflectionUtils.IsNullable(underlyingType);

            DefaultCreator = GetDefaultCreator(underlyingType);

            DefaultCreatorNonPublic = (!contract.CreatedType.IsValueType() &&
                                                ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null);

            NonNullableUnderlyingType = (IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType;

            CreatedType = NonNullableUnderlyingType;

            IsConvertable = ConvertUtils.IsConvertible(NonNullableUnderlyingType);
            IsEnum = NonNullableUnderlyingType.IsEnum();

            InternalReadType = ReadType.Read;
        }

        private Func<object> GetDefaultCreator(Type createdType)
        {
            return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
        }

        public Func<T> CreateDefaultConstructor<T>(Type type)
        {
            if (type.IsValueType)
            {
                return () => (T)Activator.CreateInstance(type);
            }
            
            // TODO: next to fill in, Randy!! 
        }
    }*/

    public class ToObject
    {
        private static bool Deserialize(Type t, BEBaseObject source, out object result)
        {
            result = t.IsValueType ? Activator.CreateInstance(t) : null;


            if (t.IsValueType)
            {
                if (t == typeof(int) && source is BENumber benumber)
                {
                    result = benumber.ToInt();
                    return true;
                }

                if (t == typeof(long) && source is BENumber belong)
                {
                    result = belong.ToLong();
                    return true;
                }

                // TODO: add other types that don't map up 1:1, like byte, ulong, bool, etc
                //throw new Exception($"Cannot map value type {newObjectType.Name} to the given BEncode object.");
                Debug.WriteLine($"Cannot map value type {t.Name} to the given BEncode object.");
                return false;
            }
            else
            {
                if (t == typeof(string) && source is BEByteString bestring)
                {
                    result = bestring.ToString();
                    return true;
                }

                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>) && source is BEList belist)
                {
                    var genericType = t.GetGenericArguments()[0];
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(genericType);
                    var list = Activator.CreateInstance(constructedListType);

                    var addMethod = constructedListType.GetMethod("Add");

                    foreach (var listElement in belist)
                    {
                        if (Deserialize(genericType, listElement, out var o))
                        {
                            addMethod.Invoke(list, new object[] { o });
                        }
                    }

                    result = list;
                    return true;
                }

                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && source is BEDictionary beDictionary)
                {
                    
                }

                // If dictionary or a some POCO with a constructor...
                // https://stackoverflow.com/questions/824802/c-how-to-get-all-public-both-get-and-set-string-properties-of-a-type

                var obj = CreateNewObject(t);
                if (obj != null && source is BEDictionary bedict)
                {
                    var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite && p.GetSetMethod(false) != null).ToList();
                    
                }

                return false;
            }
        }

        public static bool Deserialize<T>(BEBaseObject source, out T result) where T:class
        {
            object o;
            var success = Deserialize(typeof(T), source, out o);

            if (success)
            {
                result = o as T;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }

        private static object CreateNewObject(Type t) // (SerdeContract objectContract, JsonProperty containerMember, JsonProperty containerProperty, string id, out bool createdFromNonDefaultCreator)
        {
            var constructor = t.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
            {
                // throw new Exception("Only objects with a default constructor currently supported.");
                Debug.WriteLine("Only objects with a default constructor currently supported.");
                return null;
            }

            return constructor.Invoke(null);

            /* object newObject = null;

            if (objectContract.DefaultCreator != null)
            {
                // use the default constructor if it is...
                // public
                // non-public and the user has change constructor handling settings
                // non-public and there is no other creator
                newObject = objectContract.DefaultCreator();
            }

            if (newObject == null)
            {
                if (!objectContract.IsInstantiable)
                {
                    throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, objectContract.UnderlyingType));
                }

                throw JsonSerializationException.Create(reader, "Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith(CultureInfo.InvariantCulture, objectContract.UnderlyingType));
            }

            createdFromNonDefaultCreator = false;
            return newObject; 
            */


        }
    }
}
