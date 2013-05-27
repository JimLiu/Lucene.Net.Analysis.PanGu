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

namespace System.Collections.Generic
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable()]
    [System.Runtime.InteropServices.ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class SuperLinkedList<T> : ICollection<T>, System.Collections.ICollection, ISerializable, IDeserializationCallback
    {
        // This SuperLinkedList is a doubly-Linked circular list.
        internal SuperLinkedListNode<T> head;
        internal int count;
        internal int version;
        private Object _syncRoot;

        private SerializationInfo siInfo; //A temporary variable which we need during deserialization. 

        // names for serialization
        const String VersionName = "Version";
        const String CountName = "Count";
        const String ValuesName = "Data";

        public SuperLinkedList()
        {
        }

        public SuperLinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        protected SuperLinkedList(SerializationInfo info, StreamingContext context)
        {
            siInfo = info;
        }

        public int Count
        {
            get { return count; }
        }

        public SuperLinkedListNode<T> First
        {
            get { return head; }
        }

        public SuperLinkedListNode<T> Last
        {
            get { return head == null ? null : head.prev; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }

        public SuperLinkedListNode<T> AddAfter(SuperLinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            SuperLinkedListNode<T> result = new SuperLinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node.next, result);
            return result;
        }

        public void AddAfter(SuperLinkedListNode<T> node, SuperLinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node.next, newNode);
            newNode.list = this;
        }

        public SuperLinkedListNode<T> AddAfter(SuperLinkedListNode<T> node, SuperLinkedList<T> newLinkedList)
        {
            if (newLinkedList.Count <= 0)
            {
                return node;
            }

            SuperLinkedListNode<T> cur = node;

            foreach (T t in newLinkedList)
            {
                cur = this.AddAfter(cur, t);
            }

            return cur;

            //SuperLinkedListNode<T> nodeNext = node.Next;
            //SuperLinkedListNode<T> newLast = newLinkedList.Last;

            //node.next = newLinkedList.First;
            //newLinkedList.First.prev = node;

            //if (nodeNext != null)
            //{
            //    newLast.next = nodeNext;
            //    nodeNext.prev = newLast;
            //}
            //else
            //{
            //    newLast.next = head;
            //    head.prev = newLast;
            //}

            //count += newLinkedList.Count;
        }

        public SuperLinkedListNode<T> AddBefore(SuperLinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            SuperLinkedListNode<T> result = new SuperLinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node, result);
            if (node == head)
            {
                head = result;
            }
            return result;
        }

        public void AddBefore(SuperLinkedListNode<T> node, SuperLinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node, newNode);
            newNode.list = this;
            if (node == head)
            {
                head = newNode;
            }
        }

        public SuperLinkedListNode<T> AddFirst(T value)
        {
            SuperLinkedListNode<T> result = new SuperLinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
                head = result;
            }
            return result;
        }

        public void AddFirst(SuperLinkedListNode<T> node)
        {
            ValidateNewNode(node);

            if (head == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
                head = node;
            }
            node.list = this;
        }

        public SuperLinkedListNode<T> AddLast(T value)
        {
            SuperLinkedListNode<T> result = new SuperLinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
            }
            return result;
        }

        public void AddLast(SuperLinkedListNode<T> node)
        {
            ValidateNewNode(node);

            if (head == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
            }
            node.list = this;
        }

        public void Clear()
        {
            SuperLinkedListNode<T> current = head;
            while (current != null)
            {
                SuperLinkedListNode<T> temp = current;
                current = current.Next;   // use Next the instead of "next", otherwise it will loop forever 
                temp.Invalidate();
            }

            head = null;
            count = 0;
            version++;
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index", index.ToString());
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("SR.Arg_InsufficientSpace");
            }

            SuperLinkedListNode<T> node = head;
            if (node != null)
            {
                do
                {
                    array[index++] = node.item;
                    node = node.next;
                } while (node != head);
            }
        }

        public SuperLinkedListNode<T> Find(T value)
        {
            SuperLinkedListNode<T> node = head;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (c.Equals(node.item, value))
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
                else
                {
                    do
                    {
                        if (node.item == null)
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
            }
            return null;
        }

        public SuperLinkedListNode<T> FindLast(T value)
        {
            if (head == null) return null;

            SuperLinkedListNode<T> last = head.prev;
            SuperLinkedListNode<T> node = last;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (c.Equals(node.item, value))
                        {
                            return node;
                        }

                        node = node.prev;
                    } while (node != last);
                }
                else
                {
                    do
                    {
                        if (node.item == null)
                        {
                            return node;
                        }
                        node = node.prev;
                    } while (node != last);
                }
            }
            return null;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T value)
        {
            SuperLinkedListNode<T> node = Find(value);
            if (node != null)
            {
                InternalRemoveNode(node);
                return true;
            }
            return false;
        }

        public void Remove(SuperLinkedListNode<T> node)
        {
            ValidateNode(node);
            InternalRemoveNode(node);
        }

        public void RemoveFirst()
        {
            if (head == null) { throw new InvalidOperationException("LinkedListEmpty"); }
            InternalRemoveNode(head);
        }

        public void RemoveLast()
        {
            if (head == null) { throw new InvalidOperationException("SR.LinkedListEmpty"); }
            InternalRemoveNode(head.prev);
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Customized serialization for SuperLinkedList.
            // We need to do this because it will be too expensive to Serialize each node. 
            // This will give us the flexiblility to change internal implementation freely in future.
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue(VersionName, version);
            info.AddValue(CountName, count); //This is the length of the bucket array. 
            if (count != 0)
            {
                T[] array = new T[Count];
                CopyTo(array, 0);
                info.AddValue(ValuesName, array, typeof(T[]));
            }
        }

        public virtual void OnDeserialization(Object sender)
        {
            if (siInfo == null)
            {
                return; //Somebody had a dependency on this Dictionary and fixed us up before the ObjectManager got to it. 
            }

            int realVersion = siInfo.GetInt32(VersionName);
            int count = siInfo.GetInt32(CountName);

            if (count != 0)
            {
                T[] array = (T[])siInfo.GetValue(ValuesName, typeof(T[]));

                if (array == null)
                {
                    throw new SerializationException("SR.Serialization_MissingValues");
                }
                for (int i = 0; i < array.Length; i++)
                {
                    AddLast(array[i]);
                }
            }
            else
            {
                head = null;
            }

            version = realVersion;
            siInfo = null;
        }


        private void InternalInsertNodeBefore(SuperLinkedListNode<T> node, SuperLinkedListNode<T> newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            version++;
            count++;
        }

        private void InternalInsertNodeToEmptyList(SuperLinkedListNode<T> newNode)
        {
            Debug.Assert(head == null && count == 0, "SuperLinkedList must be empty when this method is called!");
            newNode.next = newNode;
            newNode.prev = newNode;
            head = newNode;
            version++;
            count++;
        }

        internal void InternalRemoveNode(SuperLinkedListNode<T> node)
        {
            Debug.Assert(node.list == this, "Deleting the node from another list!");
            Debug.Assert(head != null, "This method shouldn't be called on empty list!");
            if (node.next == node)
            {
                Debug.Assert(count == 1 && head == node, "this should only be true for a list with only one node");
                head = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (head == node)
                {
                    head = node.next;
                }
            }
            node.Invalidate();
            count--;
            version++;
        }

        internal void ValidateNewNode(SuperLinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != null)
            {
                throw new InvalidOperationException("SR.LinkedListNodeIsAttached");
            }
        }


        internal void ValidateNode(SuperLinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != this)
            {
                throw new InvalidOperationException("SR.ExternalLinkedListNode");
            }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("SR.Arg_MultiRank");
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("SR.Arg_NonZeroLowerBound");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index.ToString());
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("SR.Arg_InsufficientSpace");
            }

            T[] tArray = array as T[];
            if (tArray != null)
            {
                CopyTo(tArray, index);
            }
            else
            {
                //
                // Catch the obvious case assignment will fail. 
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T, 
                // we can't figure out if we can successfully copy the element beforehand. 
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                {
                    throw new ArgumentException("SR.Invalid_Array_Type");
                }

                object[] objects = array as object[];
                if (objects == null)
                {
                    throw new ArgumentException("SR.Invalid_Array_Type");
                }
                SuperLinkedListNode<T> node = head;
                try
                {
                    if (node != null)
                    {
                        do
                        {
                            objects[index++] = node.item;
                            node = node.next;
                        } while (node != head);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("SR.Invalid_Array_Type");
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        [Serializable()]
        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator, ISerializable, IDeserializationCallback
        {
            private SuperLinkedList<T> list;
            private SuperLinkedListNode<T> node;
            private int version;
            private T current;
            private int index;
            private SerializationInfo siInfo; //A temporary variable which we need during deserialization.

            const string LinkedListName = "LinkedList";
            const string CurrentValueName = "Current";
            const string VersionName = "Version";
            const string IndexName = "Index";

            internal Enumerator(SuperLinkedList<T> list)
            {
                this.list = list;
                version = list.version;
                node = list.head;
                current = default(T);
                index = 0;
                siInfo = null;
            }

            internal Enumerator(SerializationInfo info, StreamingContext context)
            {
                siInfo = info;
                list = null;
                version = 0;
                node = null;
                current = default(T);
                index = 0;
            }

            public T Current
            {
                get { return current; }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || (index == list.Count + 1))
                    {
                        throw new InvalidOperationException("ExceptionResource.InvalidOperation_EnumOpCantHappen");
                    }

                    return current;
                }
            }

            public bool MoveNext()
            {
                if (version != list.version)
                {
                    throw new InvalidOperationException("SR.InvalidOperation_EnumFailedVersion");
                }

                if (node == null)
                {
                    index = list.Count + 1;
                    return false;
                }

                ++index;
                current = node.item;
                node = node.next;
                if (node == list.head)
                {
                    node = null;
                }
                return true;
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != list.version)
                {
                    throw new InvalidOperationException("SR.InvalidOperation_EnumFailedVersion");
                }

                current = default(T);
                node = list.head;
                index = 0;
            }

            public void Dispose()
            {
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException("info");
                }

                info.AddValue(LinkedListName, list);
                info.AddValue(VersionName, version);
                info.AddValue(CurrentValueName, current);
                info.AddValue(IndexName, index);
            }

            void IDeserializationCallback.OnDeserialization(Object sender)
            {
                if (list != null)
                {
                    return; //Somebody had a dependency on this Dictionary and fixed us up before the ObjectManager got to it. 
                }

                if (siInfo == null)
                {
                    throw new SerializationException("SR.Serialization_InvalidOnDeser");
                }

                list = (SuperLinkedList<T>)siInfo.GetValue(LinkedListName, typeof(SuperLinkedList<T>));
                version = siInfo.GetInt32(VersionName);
                current = (T)siInfo.GetValue(CurrentValueName, typeof(T));
                index = siInfo.GetInt32(IndexName);

                if (list.siInfo != null)
                {
                    list.OnDeserialization(sender);
                }

                if (index == list.Count + 1)
                {  // end of enumeration
                    node = null;
                }
                else
                {
                    node = list.First;
                    // We don't care if we can point to the correct node if the LinkedList was changed 
                    // MoveNext will throw upon next call and Current has the correct value.
                    if (node != null && index != 0)
                    {
                        for (int i = 0; i < index; i++)
                        {
                            node = node.next;
                        }
                        if (node == list.First)
                        {
                            node = null;
                        }
                    }
                }
                siInfo = null;
            }
        }

    }

    // Note following class is not serializable since we customized the serialization of LinkedList. 
    [System.Runtime.InteropServices.ComVisible(false)]
    public sealed class SuperLinkedListNode<T>
    {
        internal SuperLinkedList<T> list;
        internal SuperLinkedListNode<T> next;
        internal SuperLinkedListNode<T> prev;
        internal T item;

        public SuperLinkedListNode(T value)
        {
            this.item = value;
        }

        internal SuperLinkedListNode(SuperLinkedList<T> list, T value)
        {
            this.list = list;
            this.item = value;
        }

        public SuperLinkedList<T> List
        {
            get { return list; }
        }

        public SuperLinkedListNode<T> Next
        {
            get { return next == null || next == list.head ? null : next; }
        }

        public SuperLinkedListNode<T> Previous
        {
            get { return prev == null || this == list.head ? null : prev; }
        }

        public T Value
        {
            get { return item; }
            set { item = value; }
        }

        internal void Invalidate()
        {
            list = null;
            next = null;
            prev = null;
        }
    }
}

