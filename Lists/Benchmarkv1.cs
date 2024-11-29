/*
* !!!RUN IN RELEASE CONFIGURATION WHEN BENCHMARKING!!!
*/

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lists
{
	public class Benchmarkv1
	{
		public static void RunBenchmark()
		{
			BenchmarkRunner.Run<MyBenchmarkv1>();
		}
	}

	//We care about memory, so we use the MemoryDiagnoser annotation.
	[MemoryDiagnoser]
	[MinIterationCount(5)]
	[MaxIterationCount(10)]
	public class MyBenchmarkv1
	{
		private List<int> list = new List<int>();
        private MyList1<int> list1 = new MyList1<int>();

		[GlobalSetup]
		public void GlobalSetup()
		{
			List<BenchmarkInstructions> instructions = BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Insert);
            ExecuteInstructions(list, instructions);
            ExecuteInstructions(list1, instructions);
		}

        [Benchmark]
        public void TestCSList()
        {
            List<int> list = new List<int>();
            List<BenchmarkInstructions> instructions = BenchmarkInstructions.GenerateInstructions();
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestMyList1()
		{
			MyList1<int> list = new MyList1<int>();
			List<BenchmarkInstructions> instructions = BenchmarkInstructions.GenerateInstructions();
			ExecuteInstructions(list, instructions);
		}

        [Benchmark]
        public void TestInsertCSList()
        {
            List<int> list = new List<int>();
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Insert, list, 10000);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestInsertMyList1()
		{
			MyList1<int> list = new MyList1<int>();
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Insert, list, 10000);
			ExecuteInstructions(list, instructions);
		}

        [Benchmark]
        public void TestInsertIntoCSList()
        {
            List<int> list = new List<int>();
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.InsertInto, list, 10000);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestInsertIntoMyList1()
		{
			MyList1<int> list = new MyList1<int>();
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.InsertInto, list, 10000);
			ExecuteInstructions(list, instructions);
		}

        [Benchmark]
        public void TestSearchCSList()
        {
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Search, list);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestSearchMyList1()
		{
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Search, list1);
			ExecuteInstructions(list1, instructions);
		}

        [Benchmark]
        public void TestRemoveCSList()
        {
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Remove, list, 12000);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestRemoveMyList1()
		{
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Remove, list1, 12000);
			ExecuteInstructions(list1, instructions);
		}

        [Benchmark]
        public void TestRemoveAtCSList()
        {
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.RemoveAt, list, 12000);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestRemoveAtMyList1()
		{
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.RemoveAt, list1, 12000);
			ExecuteInstructions(list1, instructions);
		}

        [Benchmark]
        public void TestClearCSList()
        {
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Clear, list, 25000);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
        public void TestClearMyList1()
		{
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Clear, list1, 25000);
			ExecuteInstructions(list1, instructions);
		}

		private void ExecuteInstructions(IList<int> list, List<BenchmarkInstructions> instructions)
		{
			foreach (BenchmarkInstructions inst in instructions)
			{
				switch (inst.Instruction)
				{
					case BenchmarkInstructions.Op.Insert:
						list.Insert(inst.Number);
						break;
					case BenchmarkInstructions.Op.InsertInto:
						list.Insert(inst.Index, inst.Number);
						break;
					case BenchmarkInstructions.Op.Search:
						list.Find(inst.Number);
						break;
					case BenchmarkInstructions.Op.Remove:
						list.Remove(inst.Number);
						break;
					case BenchmarkInstructions.Op.RemoveAt:
						list.RemoveAt(inst.Index);
						break;
					case BenchmarkInstructions.Op.Clear:
						list.Clear();
						break;
				}
			}
		}

		//This method is to benchmark the C# List
		private void ExecuteInstructions(List<int> list, List<BenchmarkInstructions> instructions)
		{
			foreach (BenchmarkInstructions inst in instructions)
			{
				switch (inst.Instruction)
				{
					case BenchmarkInstructions.Op.Insert:
						list.Add(inst.Number);
						break;
					case BenchmarkInstructions.Op.InsertInto:
						list.Insert(inst.Index, inst.Number);
						break;
					case BenchmarkInstructions.Op.Search:
						list.Find(x => x == inst.Number);
						break;
					case BenchmarkInstructions.Op.Remove:
						list.Remove(inst.Number);
						break;
					case BenchmarkInstructions.Op.RemoveAt:
						list.RemoveAt(inst.Index);
						break;
					case BenchmarkInstructions.Op.Clear:
						list.Clear();
						break;
				}
			}
		}
	}
}
