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
	public class Benchmarkv3
	{
		public static void RunBenchmark()
		{
			BenchmarkRunner.Run<MyBenchmarkv3>();
		}
	}

	//We care about memory, so we use the MemoryDiagnoser annotation.
	[MemoryDiagnoser]
	[MinIterationCount(5)]
	[MaxIterationCount(12)]
	public class MyBenchmarkv3
	{
		[Benchmark]
		public void TestMyList2()
		{
			MyList2<int> list = new MyList2<int>();
			List<BenchmarkInstructions> instructions = BenchmarkInstructions.GenerateInstructions();
			ExecuteInstructions(list, instructions);
		}

		[Benchmark]
		public void TestMySet()
		{
			MySet<int> list = new MySet<int>();
			List<BenchmarkInstructions> instructions = BenchmarkInstructions.GenerateInstructions();
			ExecuteInstructions(list, instructions);
		}

		[Benchmark]
		public void TestInsertMyList2()
		{
			MyList2<int> list = new MyList2<int>();
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Insert, list, 10000);
			ExecuteInstructions(list, instructions);
		}

        [Benchmark]
        public void TestInsertMySet()
        {
            MySet<int> list = new MySet<int>();
            List<BenchmarkInstructions> instructions =
                BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.Insert, list, 10000);
            ExecuteInstructions(list, instructions);
        }

        [Benchmark]
		public void TestInsertIntoMyList2()
		{
			MyList2<int> list = new MyList2<int>();
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.InsertInto, list, 10000);
			ExecuteInstructions(list, instructions);
		}

		[Benchmark]
		public void TestInsertIntoMySet()
		{
			MySet<int> list = new MySet<int>();
			List<BenchmarkInstructions> instructions = 
				BenchmarkInstructions.GenerateInstructions(BenchmarkInstructions.Op.InsertInto, list, 10000);
			ExecuteInstructions(list, instructions);
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
	}
}
