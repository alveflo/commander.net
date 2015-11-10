using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Commando
{
	public class Commando
	{
		private List<ArgumentSpecification> argumentSpecifications;
		public Commando ()
		{
			argumentSpecifications = new List<ArgumentSpecification> ();
			argumentSpecifications.Add (new ArgumentSpecification {
				Short = "h",
				Long = "help",
				Description = "Get help",
				DataType = null,
				IsSwitch = true,
				IsParameter = false,
				Mandatory = false,
				Value = null
			});

		}

		public Commando Version(string version)
		{
			argumentSpecifications.Add (new ArgumentSpecification {
				Short = "V",
				Long = "Version",
				Description = "Get version",
				DataType = null,
				IsSwitch = true,
				IsParameter = false,
				Mandatory = false,
				Value = version
			});
			return this;
		}

		/// <summary>
		/// Parameter the specified shortName, longName, dataType, description and mandatory.
		/// </summary>
		/// <param name="shortName">Short name.</param>
		/// <param name="longName">Long name.</param>
		/// <param name="dataType">Data type.</param>
		/// <param name="description">Description.</param>
		/// <param name="mandatory">If set to <c>true</c> mandatory.</param>
		public Commando Parameter(string shortName, string longName, string description, bool mandatory)
		{
			argumentSpecifications.Add (new ArgumentSpecification { 
				Short = shortName,
				Long = longName,
				DataType = null,
				Description = description,
				IsSwitch = false,
				IsParameter = true,
				Mandatory = mandatory
			});

			return this;
		}

		/// <summary>
		/// Switch the specified shortName, longName, dataType, description and mandatory.
		/// </summary>
		/// <param name="shortName">Short name.</param>
		/// <param name="longName">Long name.</param>
		/// <param name="dataType">Data type.</param>
		/// <param name="description">Description.</param>
		/// <param name="mandatory">If set to <c>true</c> mandatory.</param>
		public Commando Switch(string shortName, string longName, string description, bool mandatory)
		{
			argumentSpecifications.Add (new ArgumentSpecification { 
				Short = shortName,
				Long = longName,
				DataType = null,
				Description = description,
				IsSwitch = true,
				IsParameter = false,
				Mandatory = mandatory
			});

			return this;
		}


		public ExpandoObject Parse(string[] args) {
			var res = new ExpandoObject () as IDictionary<string, Object>;
			try {
				Dictionary<string, dynamic> parsedResult = new Parser (args, argumentSpecifications).Parse ();
				foreach (KeyValuePair<string, dynamic> keyValue in parsedResult)
					res.Add(keyValue.Key, keyValue.Value);
			} catch (ArgumentException ex) {
				throw ex;
			}
			return res as ExpandoObject;
		}
	}
}

