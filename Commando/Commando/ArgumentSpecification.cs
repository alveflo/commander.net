using System;

namespace Commando
{
	public struct ArgumentSpecification
	{
		public string Short;
		public string Long;
		public string Description;
		public Type DataType;
		public bool IsSwitch;
		public bool IsParameter;
		public bool	Mandatory;
		public string Value;
	}
}

