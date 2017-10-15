using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace netBencodeReader.Types
{
    public class BEDictionary : BEBaseObject, IDictionary<BEByteString, BEBaseObject>
    {
        private Dictionary<BEByteString, BEBaseObject> internalDictionary;

        public BEDictionary()
        {
            this.internalDictionary = new Dictionary<BEByteString, BEBaseObject>();
        }

        public IEnumerator<KeyValuePair<BEByteString, BEBaseObject>> GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<BEByteString, BEBaseObject> item)
        {
            this.internalDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.internalDictionary.Clear();
        }

        public bool Contains(KeyValuePair<BEByteString, BEBaseObject> item)
        {
            return internalDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<BEByteString, BEBaseObject>[] array, int arrayIndex)
        {
            throw new NotImplementedException();

        }

        public bool Remove(KeyValuePair<BEByteString, BEBaseObject> item)
        {
            throw new NotImplementedException();
        }

        public int Count => internalDictionary.Count;

        public bool IsReadOnly => false;

        public bool ContainsKey(BEByteString key)
        {
            return this.internalDictionary.ContainsKey(key);
        }

        public void Add(BEByteString key, BEBaseObject value)
        {
            this.internalDictionary.Add(key, value);
        }

        public bool Remove(BEByteString key)
        {
            return this.internalDictionary.Remove(key);
        }

        public bool TryGetValue(BEByteString key, out BEBaseObject value)
        {
            return this.internalDictionary.TryGetValue(key, out value);
        }

        public BEBaseObject this[BEByteString key]
        {
            get => this.internalDictionary[key];
            set => this.internalDictionary[key] = value;
        }

        public ICollection<BEByteString> Keys => this.internalDictionary.Keys;
        public ICollection<BEBaseObject> Values => this.internalDictionary.Values;

        public override string ToString()
        {
            var kvps = this.internalDictionary.Select(kvp => $"{kvp.Key}: {kvp.Value}");
            return "{" + string.Join("; ", kvps) + "}";
        }
    }
}
