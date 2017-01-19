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
    /**/
    /// <summary>
    /// Quick Sort
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuickSort<T>
    {
        //Partition for int
        private static int PartitionInt(int[] array, int low, int high, int pivotIndex)
        {
            int pivotValue = array[pivotIndex];
            array[pivotIndex] = array[low];
            array[low] = pivotValue;

            while (low < high)
            {
                while (array[high] >= pivotValue && high > low)
                {
                    --high;
                }

                if (high > low)
                {
                    array[low] = array[high];
                }

                while (array[low] <= pivotValue && high > low)
                {
                    ++low;
                }

                if (high > low)
                {
                    array[high] = array[low];
                }

            }

            array[low] = pivotValue;

            return low;
        }

        //Partition for long 
        private static int PartitionLong(long[] array, int low, int high, int pivotIndex)
        {
            long pivotValue = array[pivotIndex];
            array[pivotIndex] = array[low];
            array[low] = pivotValue;

            while (low < high)
            {
                while (array[high] >= pivotValue && high > low)
                {
                    --high;
                }

                if (high > low)
                {
                    array[low] = array[high];
                }

                while (array[low] <= pivotValue && high > low)
                {
                    ++low;
                }

                if (high > low)
                {
                    array[high] = array[low];
                }

            }

            array[low] = pivotValue;

            return low;
        }

        //Normal Partition
        private static int Partition(T[] array, int low, int high, int pivotIndex, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                Array arr = array;

                if (typeof(T) == typeof(int))
                {
                    return PartitionInt((int[])arr, low, high, pivotIndex);
                }
                else if (typeof(T) == typeof(long))
                {
                    return PartitionLong((long[])arr, low, high, pivotIndex);
                }
            }

            T pivotValue = array[pivotIndex];
            T pLow = array[low];

            while (low < high)
            {
                while (comparer.Compare(array[high], pivotValue) >= 0 && high > low)
                {
                    --high;
                }

                if (high > low)
                {
                    array[low] = array[high];
                }

                while (comparer.Compare(array[low], pivotValue) <= 0 && high > low)
                {
                    ++low;
                }

                if (high > low)
                {
                    array[high] = array[low];
                }

            }

            array[low] = pLow;

            return low;
        }

        public static void TopSort(T[] array, int top)
        {
            TopSort(array, top, null);
        }

        public static void TopSort(T[] array, int top, IComparer<T> comparer)
        {
            TopSort(array, array.Length, top, comparer);
        }

        public static void TopSort(T[] array, int arrayLen, int top, IComparer<T> comparer)
        {
            //If comparer is null
            if (comparer == null)
            {
                Array arr = array;

                if (typeof(T) != typeof(int) &&
                    typeof(T) != typeof(long))
                {
                    Array.Sort(array, 0, arrayLen);
                    return;
                }
            }

            //Judge input
            if (arrayLen <= 2 || top >= arrayLen / 2)
            {
                Array.Sort(array,0, arrayLen, comparer);
                return;
            }

            //One time partition
            int pivot = Partition(array, 0, arrayLen - 1, arrayLen / 2, comparer);
            int lastPivot = pivot;

            //Run until pivot near the top
            while ((!(lastPivot >= top && pivot <= top)))
            {
                lastPivot = pivot;

                if (pivot > top)
                {
                    pivot = Partition(array, 0, pivot, pivot / 2, comparer);

                    if (pivot == lastPivot)
                    {
                        pivot--;
                    }
                }
                else
                {
                    if (pivot >= arrayLen - 1)
                    {
                        lastPivot = arrayLen - 1;
                        break;
                    }

                    pivot = Partition(array, pivot + 1, arrayLen - 1, (arrayLen - pivot) / 2, comparer);
                }
            }

            //Finally sort
            if (lastPivot < arrayLen)
            {
                Array.Sort(array, 0, lastPivot + 1, comparer);
            }
            else
            {
                Array.Sort(array, 0, lastPivot, comparer);
            }
        }
    }
}
