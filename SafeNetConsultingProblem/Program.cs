using System;
using System.Collections.Generic;
using System.Linq;

namespace SafeNetConsultingProblem
{
    class Program
    {
        public static void Main(string[] args)
        {
			do
			{
				while (!Console.KeyAvailable)
				{
                    string input = Console.ReadLine().ToUpper();

                    if (input == "R")
                    {
                        Helpers.Restock();
                    }
                    else if (input.Substring(0, 1) == "W")
                    {
                        string parsedInput = new string(input.Where(p => char.IsDigit(p)).ToArray());
                        if (parsedInput.Length > 0)
                        {
							Helpers.WithdrawStart(Convert.ToInt32(parsedInput));
                        }
                    }
                    else if (input.Substring(0, 1) == "I")
                    {
                        List<int> numbers = Helpers.ParseInput(input);
                            
                        if (numbers.Count() > 0)
                        {
                            Helpers.OutputInventory(numbers);
                        }
                        else 
                        {
                            Helpers.OutputInventory();
                        }
					}
                    else
                    {
                        Console.WriteLine("Failure: Invalid Command");
                    }
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Q);
        }
    }
    public static class Helpers
    {
		static Dictionary<int, int> dictDenomonations = new Dictionary<int, int>()
		{
			{100, 10},
			{50, 10},
			{20, 10},
            {10, 10},
            {5, 10},
			{1, 10}
		};

        static Dictionary<int, int> tempDenomonations = new Dictionary<int, int>();

		public static void Restock()
		{
            Console.WriteLine("Machine Balance: ");
			foreach (var item in dictDenomonations.Keys.ToList())
			{
                dictDenomonations[item] = 10;
				Console.WriteLine("$" + item + " - " + dictDenomonations[item]);
			}
		}

		public static void OutputInventory()
		{
			foreach (KeyValuePair<int, int> pair in dictDenomonations)
			{
				Console.WriteLine("$" + pair.Key + " - " + pair.Value);
			}
		}

		public static void OutputInventory(List<int> Denoms)
		{
			foreach (var demon in Denoms)
			{
                try
                {
					Console.WriteLine("$" + demon + " - " + dictDenomonations[demon]);                    
                }
                catch
                {
                    Console.WriteLine("$" + demon + " - does not exist");
				}
			}
		}

        public static void WithdrawStart(int Amount)
        {
			tempDenomonations = new Dictionary<int, int>(dictDenomonations);   
            Withdraw(Amount, tempDenomonations);
        }

		public static void Withdraw(int Amount, Dictionary<int,int> tempDenomonations)
		{
            if (tempDenomonations.Count() == 0 )
            {
				Console.WriteLine("Failure: insufficient funds");
				tempDenomonations = dictDenomonations;
				return;
            }
            int remainingAmount = Amount;
			foreach (var key in tempDenomonations.Keys.OrderByDescending(key => key).ToList())
			{
                remainingAmount = ReturnQuantity(key, remainingAmount);
                if (remainingAmount == 0)
                {
                    Console.WriteLine("Success: Dispensed $" + Amount);
                    Console.WriteLine("Machine balance: ");
                    foreach (var item in dictDenomonations.Keys.ToList())
                    {
                        if (tempDenomonations.ContainsKey(item))
							dictDenomonations[item] = tempDenomonations[item];
						Console.WriteLine("$" + item + " - " + dictDenomonations[item]);
                    }
                    return;
                }
				else if (key == 1)
                {
                    tempDenomonations.Remove(tempDenomonations.Keys.Max());
                    Withdraw(Amount, tempDenomonations);
                }
			}
		}

		public static int ReturnQuantity(int denomonation, int remainingAmount)
		{
            int multiple = remainingAmount / denomonation;
            if (multiple <= tempDenomonations[denomonation])
            {
                tempDenomonations[denomonation] = tempDenomonations[denomonation] - multiple;
                remainingAmount = remainingAmount % denomonation;    
            }
            else
            {
                remainingAmount = tempDenomonations[denomonation] == 0 ? remainingAmount : remainingAmount - tempDenomonations[denomonation] * denomonation;
                tempDenomonations[denomonation] = 0;
            }
			return remainingAmount;
		}

        public static List<int> ParseInput(string input)
        {
			List<string> splitInput = input.Split(' ').ToList();
			List<int> numbers = new List<int>();

			foreach (string item in splitInput)
			{
				int itemInt;
				bool IsInt = Int32.TryParse(new string(item.Where(p => char.IsDigit(p)).ToArray()), out itemInt);
				if (IsInt == true)
				{
					numbers.Add(itemInt);
				}
			}
            return numbers;
        }
    }
}
