using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lists
{
    public class MySet<T> : MyList2<T>
    {
        public override void Insert(int index, T item)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException();
            }

            if (Find(item) != -1)
            {
                return;
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
    }
}
