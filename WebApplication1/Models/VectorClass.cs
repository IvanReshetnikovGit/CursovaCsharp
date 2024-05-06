using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class VectorClass<T> : IEnumerable<T>
    {
        private T[] arr;
        private int capacity;
        private int current;

        public VectorClass()
        {
            arr = new T[1];
            capacity = 1;
            current = 0;
        }

        public void Push(T data)
        {
            if (current == capacity)
            {
                T[] temp = new T[2 * capacity];
                Array.Copy(arr, temp, capacity);
                capacity *= 2;
                arr = temp;
            }
            arr[current] = data;
            current++;
        }

        public void Push(T data, int index)
        {
            if (index == capacity)
            {
                Push(data);
            }
            else
            {
                arr[index] = data;
            }
        }

        public T Get(int index)
        {
            return arr[index];
        }

        public T this[int index]
        {
            get { return arr[index]; }
            set { arr[index] = value; }
        }

        public void Pop()
        {
            current--;
        }

        public int Size()
        {
            return current;
        }

        public int GetCapacity()
        {
            return capacity;
        }

        public void Print()
        {
            for (int i = 0; i < current; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < current; i++)
            {
                yield return arr[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Iterator
        {
            private T[] arr;
            private int index;

            public Iterator(T[] array, int startIndex)
            {
                arr = array;
                index = startIndex;
            }

            public bool MoveNext()
            {
                index++;
                return index < arr.Length;
            }

            public T Current
            {
                get { return arr[index]; }
            }
        }

        public Iterator GetIterator()
        {
            return new Iterator(arr, -1);
        }
    }
}
