using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando
{
	internal class Parser
	{
		private string arguments;
		private List<ArgumentSpecification> argumentSpecifications;
		private Dictionary<string, dynamic> parsedArguments;
		private Queue<string> argQueue;

		public Parser (string arguments, List<ArgumentSpecification> argumentSpecifications)
		{
			parsedArguments = new Dictionary<string, dynamic> ();
			argQueue = new Queue<string> ();
			this.argumentSpecifications = argumentSpecifications;
			this.arguments = arguments;
		}

		public Dictionary<string, dynamic> Parse() {
			string[] splittedArguments = arguments.Split (" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string arg in splittedArguments)
				argQueue.Enqueue (arg);

			while (argQueue.Count > 0) {
				string arg = argQueue.Dequeue ();
				if (arg.StartsWith ("-")) {
					string pArg = arg.Replace ("-", "");
					ArgumentSpecification spec;
					if (IsSpecifiedArgument (pArg, out spec)) {
						if (spec.IsParameter) {
							if (argQueue.Peek ().StartsWith ("-")) {
								throw new ArgumentException ("Expected value, got argument instead: " + arg + " " + argQueue.Peek ());
							} else {
								parsedArguments.Add (spec.Long, argQueue.Dequeue ());
							}
						} else {
							if (spec.Long == "version") {
								Console.WriteLine (spec.Value);
								Environment.Exit (0);
							}
							parsedArguments.Add (spec.Long, true);
						}
					} else {
						throw new ArgumentException ("Unrecognized argument " + arg);
					}
				}
			}


			Validate ();
			return parsedArguments;
		}

		private void Validate()
		{
			IEnumerable<ArgumentSpecification> mandatoriesMissing = argumentSpecifications.Where (i => !parsedArguments.Keys.Contains (i.Long) && i.Mandatory);
			if (mandatoriesMissing.Count() > 0) {
				StringBuilder strb = new StringBuilder ("Missing mandatory arguments:\n");
				foreach (ArgumentSpecification arg in mandatoriesMissing)
					strb.AppendLine (arg.Long);
				throw new ArgumentException (strb.ToString());
			}
		}

		private bool IsSpecifiedArgument(string arg, out ArgumentSpecification spec) {
			spec = argumentSpecifications.Where (i => i.Short.Equals (arg) || i.Long.Equals (arg)).FirstOrDefault ();
			return !spec.Equals(default(ArgumentSpecification));
		}

	}
}

