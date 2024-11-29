using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lists
{
    public interface IList<T>
    {
        int Count { get; }
        int Capacity { get; }

        // Read
        T this[int index] { get; }

        // Search
        int Find(T match);

        // Insert

        void Insert(T item);
        void Insert(int index, T item);

        // Delete
        bool Remove(T item);
        void RemoveAt(int index);

        // Clear
        void Clear();

        // Print
        String ToString();
    }
}
