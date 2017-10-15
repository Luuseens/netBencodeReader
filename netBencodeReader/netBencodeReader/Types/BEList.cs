using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netBencodeReader.Types
{
    public class BEList: BEBaseObject, IList<BEBaseObject> 
    {
        private List<BEBaseObject> internalList;

        public BEList()
        {
            this.internalList = new List<BEBaseObject>();
        }

        public IEnumerator<BEBaseObject> GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(BEBaseObject item)
        {
            this.internalList.Add(item);
        }

        public void Clear()
        {
            this.internalList.Clear();
        }

        public bool Contains(BEBaseObject item)
        {
            return this.internalList.Contains(item);
        }

        public void CopyTo(BEBaseObject[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(BEBaseObject item)
        {
            return this.internalList.Remove(item);
        }

        public int Count => this.internalList.Count;
        public bool IsReadOnly => false;

        public int IndexOf(BEBaseObject item)
        {
            return this.internalList.IndexOf(item);
        }

        public void Insert(int index, BEBaseObject item)
        {
            this.internalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.internalList.RemoveAt(index);
        }

        public BEBaseObject this[int index]
        {
            get => this.internalList[index];
            set => this.internalList[index] = value;
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", this.internalList) + "]";
        }
    }
}
