using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lists
{
	public class BenchmarkInstructions
	{
		public enum Op
		{
			Insert,
			InsertInto,
			Search,
			Remove,
			RemoveAt,
			Clear
		}

		public Op Instruction { get; private set; }
		public int Number { get; private set; }
		public int Index { get; private set; }

		private BenchmarkInstructions() { }
		private BenchmarkInstructions(Op instruction) => (Instruction) = (instruction);
		private BenchmarkInstructions(Op instruction, int number) => (Instruction, Number) = (instruction, number);
		private BenchmarkInstructions(Op instruction, int number, int index) =>
			(Instruction, Number, Index) = (instruction, number, index);

		public static List<BenchmarkInstructions> GenerateInstructions(int amount = 120000, int seed = 10)
		{
			return GenerateInstructions(null);
		}

		public static List<BenchmarkInstructions> GenerateInstructions(Op[]? opsToIgnore, int amount = 120000, int seed = 10)
		{
			Random rand = new Random(seed);
			List<BenchmarkInstructions> instructions = new List<BenchmarkInstructions>(amount);
			List<int> numbersInserted = [];
			int[] stats = new int[6];

			for (int i = 0; i < amount; i++)
			{
				if (i < amount * 0.11f)//Insert
				{
					int randomNumber = rand.Next(int.MinValue, int.MaxValue);
					instructions.Add(new BenchmarkInstructions(Op.Insert, randomNumber));
					numbersInserted.Add(randomNumber);
					stats[0]++;
				}
				else
				{
					int opChoice = rand.Next(0, 100);

					if (opChoice < 10)//Insert
					{
						int randomNumber = rand.Next(int.MinValue, int.MaxValue);
						instructions.Add(new BenchmarkInstructions(Op.Insert, randomNumber));
						numbersInserted.Add(randomNumber);
						stats[0]++;
					}
					else if (opChoice < 30)//Insert Into
					{
						if (opsToIgnore != null && opsToIgnore.Contains(Op.InsertInto))
						{
							i--;
							continue;
						}

						int randomNumber = rand.Next(int.MinValue, int.MaxValue);
						int randomIndex = rand.Next(0, numbersInserted.Count);
						instructions.Add(new BenchmarkInstructions(Op.InsertInto, randomNumber, randomIndex));
						numbersInserted.Add(randomNumber);
						stats[1]++;
					}
					else if (opChoice < 50)//Search for item definitely in the list
					{
						if (numbersInserted.Count == 0 || (opsToIgnore != null && opsToIgnore.Contains(Op.Search)))
						{
							i--;
							continue;
						}

						int randomNumber = numbersInserted[rand.Next(0, numbersInserted.Count)];
						instructions.Add(new BenchmarkInstructions(Op.Search, randomNumber));
						stats[2]++;
					}
					else if (opChoice < 58)//Random search
					{
						if (opsToIgnore != null && opsToIgnore.Contains(Op.Search))
						{
							i--;
							continue;
						}

						int randomNumber = rand.Next(
							Math.Max(numbersInserted.Count * -1, int.MinValue), 
							Math.Min(numbersInserted.Count * 2, int.MaxValue)
							);
						instructions.Add(new BenchmarkInstructions(Op.Search, randomNumber));
						stats[2]++;
					}
					else if (opChoice < 79)//Remove
					{
						if (numbersInserted.Count == 0 || (opsToIgnore != null && opsToIgnore.Contains(Op.Remove)))
						{
							i--;
							continue;
						}

						int randomNumber = numbersInserted[rand.Next(0, numbersInserted.Count)];
						numbersInserted.Remove(randomNumber);
						instructions.Add(new BenchmarkInstructions(Op.Remove, randomNumber));
						stats[3]++;
					}
					else if (opChoice < 99)//Remove at
					{
						if (numbersInserted.Count == 0 || (opsToIgnore != null && opsToIgnore.Contains(Op.RemoveAt)))
						{
							i--;
							continue;
						}

						int randomIndex = rand.Next(0, numbersInserted.Count);
						numbersInserted.RemoveAt(randomIndex);
						instructions.Add(new BenchmarkInstructions(Op.RemoveAt, 0, randomIndex));
						stats[4]++;
					}
					else//Clear
					{
						if (opsToIgnore != null && opsToIgnore.Contains(Op.Clear))
						{
							i--;
							continue;
						}

						instructions.Add(new BenchmarkInstructions(Op.Clear));
						numbersInserted.Clear();
						stats[5]++;
					}
				}
			}

#if DEBUG
			Console.WriteLine($"Using seed {seed}, generated {stats[0]} insert instructions, {stats[1]} insert into instructions, {stats[2]} search instructions, {stats[3]} remove instructions, {stats[4]} remove at instructions, and {stats[5]} clear instructions.");
#endif

			return instructions;
		}

		public static List<BenchmarkInstructions> GenerateInstructions(Op instruction, IList<int>? list = null, int amount = 50000, int seed = 10)
		{
			Random rand = new Random(seed);
			List<BenchmarkInstructions> instructions = new List<BenchmarkInstructions>(amount);
			List<int> numbersInserted = [];

			if(list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					numbersInserted.Add(list[i]);
				}
			}
			else if (list == null && 
				(instruction == Op.Search || instruction == Op.Remove || instruction == Op.RemoveAt || instruction == Op.Clear))
			{
				throw new ArgumentException("List cannot be null if instruction is not Search, Remove, RemoveAt, or Clear.");
			}
			
			int randomNumber;
			int randomIndex;

			switch (instruction)
			{
				case Op.Insert:
					for (int i = 0; i < amount; i++)
					{
						randomNumber = rand.Next(int.MinValue, int.MaxValue);
						instructions.Add(new BenchmarkInstructions(Op.Insert, randomNumber));
					}
					break;
				case Op.InsertInto:
					randomNumber = rand.Next(int.MinValue, int.MaxValue);
					instructions.Add(new BenchmarkInstructions(Op.InsertInto, randomNumber, 0));
					for (int i = 0; i < amount - 1; i++)
					{
						randomNumber = rand.Next(int.MinValue, int.MaxValue);
						randomIndex = rand.Next(0, instructions.Count);
						instructions.Add(new BenchmarkInstructions(Op.InsertInto, randomNumber, randomIndex));
					}
					break;
				case Op.Search:
					for (int i = 0; i < amount; i++)
					{
						bool inList = rand.Next(0, 8) != 0;

						randomNumber = inList ? numbersInserted[rand.Next(0, numbersInserted.Count)]
							: rand.Next(int.MinValue, int.MaxValue);
						instructions.Add(new BenchmarkInstructions(Op.Search, randomNumber));
					}
					break;
				case Op.Remove:
					for (int i = 0; i < Math.Min(amount, numbersInserted.Count); i++)
					{
						randomNumber = numbersInserted[rand.Next(0, numbersInserted.Count)];
						numbersInserted.Remove(randomNumber);
						instructions.Add(new BenchmarkInstructions(Op.Remove, randomNumber));
					}
					break;
				case Op.RemoveAt:
					for (int i = 0; i < Math.Min(amount, numbersInserted.Count); i++)
					{
						randomIndex = rand.Next(0, numbersInserted.Count);
						numbersInserted.RemoveAt(randomIndex);
						instructions.Add(new BenchmarkInstructions(Op.RemoveAt, 0, randomIndex));
					}
					break;
				case Op.Clear:
					instructions.Add(new BenchmarkInstructions(Op.Clear));
					break;
			}

			return instructions;
		}

        public static List<BenchmarkInstructions> GenerateInstructions(Op instruction, List<int> list, int amount = 50000, int seed = 10)
        {
            Random rand = new Random(seed);
            List<BenchmarkInstructions> instructions = new List<BenchmarkInstructions>(amount);
            List<int> numbersInserted = [];

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    numbersInserted.Add(list[i]);
                }
            }
            else if (list == null &&
                (instruction == Op.Search || instruction == Op.Remove || instruction == Op.RemoveAt || instruction == Op.Clear))
            {
                throw new ArgumentException("List cannot be null if instruction is not Search, Remove, RemoveAt, or Clear.");
            }

            int randomNumber;
            int randomIndex;

            switch (instruction)
            {
                case Op.Insert:
                    for (int i = 0; i < amount; i++)
                    {
                        randomNumber = rand.Next(int.MinValue, int.MaxValue);
                        instructions.Add(new BenchmarkInstructions(Op.Insert, randomNumber));
                    }
                    break;
                case Op.InsertInto:
                    randomNumber = rand.Next(int.MinValue, int.MaxValue);
                    instructions.Add(new BenchmarkInstructions(Op.InsertInto, randomNumber, 0));
                    for (int i = 0; i < amount - 1; i++)
                    {
                        randomNumber = rand.Next(int.MinValue, int.MaxValue);
                        randomIndex = rand.Next(0, instructions.Count);
                        instructions.Add(new BenchmarkInstructions(Op.InsertInto, randomNumber, randomIndex));
                    }
                    break;
                case Op.Search:
                    for (int i = 0; i < amount; i++)
                    {
                        bool inList = rand.Next(0, 8) != 0;

                        randomNumber = inList ? numbersInserted[rand.Next(0, numbersInserted.Count)]
                            : rand.Next(int.MinValue, int.MaxValue);
                        instructions.Add(new BenchmarkInstructions(Op.Search, randomNumber));
                    }
                    break;
                case Op.Remove:
                    for (int i = 0; i < Math.Min(amount, numbersInserted.Count); i++)
                    {
                        randomNumber = numbersInserted[rand.Next(0, numbersInserted.Count)];
                        numbersInserted.Remove(randomNumber);
                        instructions.Add(new BenchmarkInstructions(Op.Remove, randomNumber));
                    }
                    break;
                case Op.RemoveAt:
                    for (int i = 0; i < Math.Min(amount, numbersInserted.Count); i++)
                    {
                        randomIndex = rand.Next(0, numbersInserted.Count);
                        numbersInserted.RemoveAt(randomIndex);
                        instructions.Add(new BenchmarkInstructions(Op.RemoveAt, 0, randomIndex));
                    }
                    break;
                case Op.Clear:
                    instructions.Add(new BenchmarkInstructions(Op.Clear));
                    break;
            }

            return instructions;
        }
    }
}
