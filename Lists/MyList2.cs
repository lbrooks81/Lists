using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lists
{
    public class MyList2<T> : IList<T>
    {
        protected T[] items;
        protected int size;
        public int Count => size;

        public int Capacity => items.Length;

        // Overloading index operator to make it seem like it's an array
        // since Lists don't support using the index operator
        public T this[int index]
        {
            get
            {
                if (index >= size || index < 0)
                    throw new IndexOutOfRangeException();
                return items[index];
            }
            set
            {
                if (index >= size || index < 0)
                    throw new IndexOutOfRangeException();
                items[index] = value;
            }
        }

        public MyList2() => items = Array.Empty<T>();

        public MyList2(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity),
                    "Capacity must be greater than or equal to zero.");
            }

            items = new T[capacity];
        }


        // Search
        // Does the item exist in the array?
        // Returns the index of the item in the array
        // Returns -1 if there is no match
        public int Find(T match)
        {
            for (int i = 0; i < size; i++)
            {
                /*
                Protects against null reference exceptions
                if (items[i] == null)
                {
                    continue;
                }
                */
                if (items[i]!.Equals(match))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(T item)
        {
            Insert(size, item);
        }

        public virtual void Insert(int index, T item)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException();
            }

            if (size + 1 >= Capacity)
            {
                Grow();
            }

            for (int i = items.Length - 1; i > index; i--)
            {
                items[i] = items[i - 1];
            }

            items[index] = item;

            size++;
        }

        protected void Grow()
        {
            T[] tempArray = new T[items.Length == 0 ? 1 : items.Length * 2];

            for (int i = 0; i < items.Length; i++)
            {
                tempArray[i] = items[i];
            }

            items = tempArray; //This works because it's changing what address in memory items is pointing to
        }


        public bool Remove(T item)
        {
            int index = Find(item);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > size)
                throw new IndexOutOfRangeException();

            for (int i = index; i < items.Length - 1; i++)
            {
                items[i] = items[i + 1];
            }

            if (--size < Capacity / 4)
            // --size decrements before the comparison
            // size-- would decrement after
            // Capacity / 4 because it gives you more than one spaces to insert before growing again
            // There's a balance between having a good memory footprint and having a good time complexity
            // Oftentimes, one will have to be sacrificed for the other
            {
                Shrink();
            }
        }

        public void Shrink()
        {
            T[] tempArray = new T[items.Length / 2];

            // When moving items, you need to compare against the smaller of the two values
            // int min = Math.Min(tempArray.Length(), items.Length())
            // use i < min here v
            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = items[i];
            }

            items = tempArray;
        }

        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (size > 0)
                {
                    Array.Clear(items, 0, size);
                    // array to clear, index to start from, length to clear
                    // Will null out any reference types, allowing the garbage collector to clear out memory
                }
            }

            size = 0;
        }

        public override string ToString()
        {
            // Always use StringBuilder when using a lot of modifications to strings
            // Doesn't create new strings every time
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < size; i++)
            {
                sb.Append(items[i]);
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
    }
}
