using Iced.Intel;

namespace Lists
{
    public class Program
    {
        public static void Main()
        {
            // Anonymous objects
            new Program().ListDemo();
            Console.WriteLine(new String('-', 30));
            new Program().MyList1Demo();
            Console.WriteLine(new String('-', 30));
            new Program().MyList2Demo();
            Console.WriteLine(new String('-', 30));
            Benchmarkv3.RunBenchmark();
        }
        private void ListDemo()
        {
            List<int> list = new List<int>();
            
            list.Add(1);
            list.Add(2);
            list.Add(4);
            list.Add(5);
            list.Add(6);

            Console.WriteLine("Count: " + list.Count);

            Console.WriteLine("Capacity: " + list.Capacity);
            // Read (complexity: 1)
            Console.WriteLine("Read: " + list[2]);

            // Search (worst-case complexity: N)
            Console.WriteLine("Search: " + list.IndexOf(5));

            // Insert (worst-case complexity: N + 1) 
            list.Insert(2, 3);
            // Print List
            Console.WriteLine("Print list:");
            foreach (int i in list)
            {
                Console.WriteLine(i);
            }

            // Delete from the list by value (worst-case complexity: 2N)
            list.Remove(3);

            // Delete from the list by value (worst-case complexity: N)
            list.RemoveAt(2);

            // Print List
            Console.WriteLine("Print list:");
            foreach (int i in list)
            {
                Console.WriteLine(i);
            }
        }
        private void MyList1Demo()
        {
            MyList1<int> list = new MyList1<int>();

            list.Insert(1);
            list.Insert(2);
            list.Insert(4);
            list.Insert(5);
            list.Insert(6);

            Console.WriteLine("Count: " + list.Count);

            Console.WriteLine("Capacity: " + list.Capacity);

            // Read (complexity: 1)
            Console.WriteLine("Read: " + list[2]);

            // Search (worst-case complexity: N)
            Console.WriteLine("Search: " + list.Find(5));

            // Insert (worst-case complexity: N + 1) 
            list.Insert(2, 3);
            
            // Print List
            Console.WriteLine("Print list:" + list);

            // Delete from the list by value (worst-case complexity: 2N)
            list.Remove(3);

            // Delete from the list by value (worst-case complexity: N)
            list.RemoveAt(2);

            // Print List
            Console.WriteLine("Print list:" + list);
        }
        private void MyList2Demo()
        {
            MyList2<int> list = new MyList2<int>();

            list.Insert(1);
            list.Insert(2);
            list.Insert(4);
            list.Insert(5);
            list.Insert(6);

            Console.WriteLine("Count: " + list.Count);

            Console.WriteLine("Capacity: " + list.Capacity);

            // Read (complexity: 1)
            Console.WriteLine("Read: " + list[2]);

            // Search (worst-case complexity: N)
            Console.WriteLine("Search: " + list.Find(5));

            // Insert (worst-case complexity: N + 1) 
            list.Insert(2, 3);

            // Print List
            Console.WriteLine("Print list:" + list);

            // Delete from the list by value (worst-case complexity: 2N)
            list.Remove(3);

            // Delete from the list by value (worst-case complexity: N)
            list.RemoveAt(2);

            // Print List
            Console.WriteLine("Print list:" + list);
        }
    }
}
