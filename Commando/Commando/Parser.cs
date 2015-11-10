using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando
{
	internal class Parser
	{
		private string[] arguments;
		private List<ArgumentSpecification> argumentSpecifications;
		private Dictionary<string, object> parsedArguments;
		private Queue<string> argQueue;

		public Parser (string[] arguments, List<ArgumentSpecification> argumentSpecifications)
		{
			parsedArguments = new Dictionary<string, object> ();
			argQueue = new Queue<string> ();
			this.argumentSpecifications = argumentSpecifications;
			this.arguments = arguments;
		}

		public Dictionary<string, object> Parse() {
			foreach (string arg in arguments)
				argQueue.Enqueue (arg);

			while (argQueue.Count > 0) {
				string arg = argQueue.Dequeue ();
				if (arg.StartsWith ("-")) {
					string pArg = arg.TrimStart ('-');
					ArgumentSpecification spec;
					if (IsSpecifiedArgument (pArg, out spec)) {
						if (spec.IsParameter) {
							if (argQueue.Peek ().StartsWith ("-")) {
								throw new ArgumentException ("Expected value, got parameter instead: " + arg + " " + argQueue.Peek ());
							} else {
								parsedArguments.Add (ReplaceDashes(spec.Long), argQueue.Dequeue ());
							}
						} else {
							if (spec.Long == "version") {
								Console.WriteLine (spec.Value);
								Environment.Exit (0);
							} else if (spec.Long == "help") {
								WriteHelp ();
								Environment.Exit (0);
							}
							parsedArguments.Add (ReplaceDashes(spec.Long), true);
						}
					} else {
						throw new ArgumentException ("Unrecognized argument " + arg);
					}
				}
			}

			parsedArguments = AddMissingNonMandatoryArguments (parsedArguments);
			Validate ();
			return parsedArguments;
		}

		private string ReplaceDashes(string str) {
			while (str.Contains("-")) {
				int dashIndex = str.IndexOf ("-");
				if (dashIndex < str.Length - 1)
					str = str.Replace (str [dashIndex].ToString(), str [++dashIndex].ToString ().ToUpper ());
				str = str.Remove (dashIndex, 1);
			}
			return str;
		}


		private Dictionary<string, object> AddMissingNonMandatoryArguments(Dictionary<string, object> parsedArguments)
		{
			IEnumerable<ArgumentSpecification> mandatoriesMissing = argumentSpecifications.Where (i => !parsedArguments.Keys.Contains (i.Long) && !i.Mandatory);
			foreach (ArgumentSpecification spec in mandatoriesMissing)
				parsedArguments.Add (spec.Long, false);
			return parsedArguments;
		}

		private void Validate()
		{
			IEnumerable<ArgumentSpecification> mandatoriesMissing = argumentSpecifications.Where (i => !parsedArguments.Keys.Contains (ReplaceDashes(i.Long)) && i.Mandatory);
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

		private void WriteHelp () {
			Console.WriteLine ("\n\n\tUsage: <program> [options]\n\n\tOptions:");
			int longestArgName = argumentSpecifications.Select (i => i.Long.Length).Max ();
			int longestDescription = argumentSpecifications.Select (i => i.Description).Max ().Length;

			foreach (ArgumentSpecification spec in argumentSpecifications) {
				string argNameWhiteSpaces = new string (' ', 2 + longestArgName - spec.Long.Length);
				string descWhiteSpaces = new string (' ', 2 + longestDescription - spec.Description.Length);
				string str = $"\t-{spec.Short}, --{spec.Long}{argNameWhiteSpaces}{spec.Description}{descWhiteSpaces}";
				if (spec.Mandatory)
					str += "(Mandatory)";
				Console.WriteLine(str);
			}
		}
	}
}

