using System;

namespace Commando
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			dynamic program = new Commando()
				.Version("0.0.1")
				.Parameter("p", "pizza", "Some pizza", true)
				.Parameter("d", "drink", "Some drink", true)
				.Switch("v", "vegetables", "Want vegetables?", false)
				.Parse(new string[] { "-p", "Margherita", "-d", "coke" });

			if (program.pizza != "Margherita")
				Console.WriteLine("We only serve margheritas!");
			if (program.vegetables)
				Console.WriteLine("We're out of vegetables!");

			Console.WriteLine(String.Format ("You've ordered a {0} with {1}", 
				program.pizza, 
				program.drink));
		}
	}
}
