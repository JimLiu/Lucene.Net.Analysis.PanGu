/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.Framework
{

    public class AppendList<T> : IList<T>, System.Collections.IList
    {
        private const int _DefaultCapacity = 4;
        T[] _Items;
        int _Size = 0;
        int _Version = 0;
        static T[] _EmptyArray = new T[0];

        private static bool IsCompatibleObject(object value)
        {
            if ((value is T) || (value == null && !typeof(T).IsValueType))
            {
                return true;
            }
            return false;
        }

        private static void VerifyValueType(object value)
        {
            if (!IsCompatibleObject(value))
            {
                throw new ArgumentException("ThrowWrongValueTypeArgumentException");
            }
        }
        
        // Ensures that the capacity of this list is at least the given minimum
        // value. If the currect capacity of the list is less than min, the 
        // capacity is increased to twice the current capacity or to min, 
        // whichever is larger.
        private void EnsureCapacity(int min)
        {
            if (_Items.Length < min)
            {
                int newCapacity = _Items.Length == 0 ? _DefaultCapacity : _Items.Length * 2;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        } 
 


        public AppendList()
        {
            _Items = _EmptyArray;
        }

        public AppendList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            _Items = new T[capacity];
        }

        // Gets and sets the capacity of this list.  The capacity is the size of
        // the internal array used to hold items.  When set, the internal 
        // array of the list is reallocated to the given capacity. 
        //
        public int Capacity
        {
            get { return _Items.Length; }

            set
            {
                if (value != _Items.Length)
                {
                    if (value < _Size)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_Size > 0)
                        {
                            Array.Copy(_Items, 0, newItems, 0, _Size);
                        }
                        _Items = newItems;
                    }
                    else
                    {
                        _Items = _EmptyArray;
                    }
                }
            }
        }

        public T[] Items
        {
            get
            {
                return _Items;
            }

        }

        public void ReduceSize(int targetSize)
        {
            if (targetSize >= _Size || targetSize <= 0)
            {
                return;
            }

            _Size = targetSize;
            _Version++;
        }

        // Sorts the elements in this list.  Uses the default comparer and
        // Array.Sort. 
        public void Sort()
        {
            Sort(0, Count, null);
        }

        // Sorts the elements in this list.  Uses Array.Sort with the
        // provided comparer. 
        public void Sort(IComparer<T> comparer)
        {
            Sort(0, Count, comparer);
        }

        // Sorts the elements in a section of this list. The sort compares the
        // elements to each other using the given IComparer interface. If
        // comparer is null, the elements are compared to each other using
        // the IComparable interface, which in that case must be implemented by all 
        // elements of the list.
        // 
        // This method uses the Array.Sort method to sort the elements. 
        //
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (index < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_Size - index < count)
            {
                throw new ArgumentException("Argument_InvalidOffLen");
            }

            Array.Sort<T>(_Items, index, count, comparer);
            _Version++;
        }


        // ToArray returns a new Object array containing the contents of the List.
        // This requires copying the List, which is an O(n) operation. 
        public T[] ToArray()
        {
            T[] array = new T[_Size];
            Array.Copy(_Items, 0, array, 0, _Size);
            return array;
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return Array.IndexOf(_Items, item, 0, _Size);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                if (index >= _Size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _Items[index]; 
            }

            set
            {
                if (index >= _Size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _Items[index] = (T)value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            if (_Size == _Items.Length) EnsureCapacity(_Size + 1);
            _Items[_Size++] = item;
            _Version++;
        }

        public void Clear()
        {
            _Size = 0;
            _Version++; 
        }

        public bool Contains(T item)
        {
            if ((Object)item == null)
            {
                for (int i = 0; i < _Size; i++)
                    if ((Object)_Items[i] == null)
                        return true;
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < _Size; i++)
                {
                    if (c.Equals(_Items[i], item)) return true;
                }
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            // Delegate rest of error checking to Array.Copy.
            Array.Copy(_Items, 0, array, arrayIndex, _Size);
        }

        public int Count
        {
            get 
            {
                return _Size;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region IList Members

        public int Add(object value)
        {
            VerifyValueType(value);
            Add((T)value);
            return Count - 1;
        }

        public bool Contains(object value)
        {
            if (IsCompatibleObject(value))
            {
                return Contains((T)value);
            }
            return false;
        }

        public int IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return IndexOf((T)value);
            }
            return -1;
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get 
            {
                return false;
            }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                VerifyValueType(value);
                this[index] = (T)value;
            } 
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get 
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        [Serializable()]
        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private AppendList<T> list;
            private int index;
            private int version;
            private T current;

            internal Enumerator(AppendList<T> list)
            {
                this.list = list;
                index = 0;
                version = list._Version;
                current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (version != list._Version)
                {
                    throw new InvalidOperationException();
                }

                if ((uint)index < (uint)list._Size)
                {
                    current = list._Items[index];
                    index++;
                    return true;
                }
                index = list._Size + 1;
                current = default(T);
                return false;
            }

            public T Current
            {
                get
                {
                    return current;
                }
            }

            Object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == list._Size + 1)
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != list._Version)
                {
                    throw new InvalidOperationException();
                }

                index = 0;
                current = default(T);
            }
        }

    }
}
