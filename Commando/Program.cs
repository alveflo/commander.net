using System;

namespace Commando
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			dynamic program = new Commando()
				.Version("0.0.1")
				.Parameter ("p", "path", "Path to location", true)
				.Switch ("v", "version", "Version of program", true)
				.Switch ("h", "help", "Get help", false)
				.Parse ("-p path/ -h -v");

			Console.WriteLine (program.path);
			Console.WriteLine (program.version);
			Console.WriteLine (program.help);
		}
	}
}
