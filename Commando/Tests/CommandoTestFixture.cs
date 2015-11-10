using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Commando
{
	[TestFixture]
	public class CommandoTestFixture
	{
		[Test]
		public void TestSwitchValue () {
			dynamic program = new Commando ()
				.Switch ("p", "pizza", "Test case", false)
				.Switch ("d", "drink", "Test case", true)
				.Parse (new string[] { "-d" });
			Assert.AreEqual (program.pizza, false);

			program = new Commando ()
				.Switch ("p", "pizza", "Test case", false)
				.Parse (new string[] { "-p" });
			Assert.AreEqual (program.pizza, true);
		}

		[Test]
		public void TestParameterValue () {
			dynamic program = new Commando ()
				.Parameter ("p", "pizza", "Test case", true)
				.Parse ("-p Vesuvio".Split (' '));
			Assert.AreEqual (program.pizza, "Vesuvio");
		}

		[Test]
		public void TestParser () {
			dynamic program = new Commando ()
				.Parameter ("p", "pizza", "Test case", true)
				.Parameter ("d", "drink", "Test case", true)
				.Switch ("t", "take-away", "Test case", true)
				.Switch ("v", "vegetables", "Test case", false)
				.Parse ("-p Vesuvio --drink Coke --take-away".Split(' '));

			Assert.AreEqual (program.pizza, "Vesuvio");
			Assert.AreEqual (program.drink, "Coke");
			Assert.AreEqual (program.takeAway, true);
			Assert.AreEqual (program.vegetables, false);

		}
	}
}

