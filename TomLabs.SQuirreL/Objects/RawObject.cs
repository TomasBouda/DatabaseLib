using System;
using System.Collections.Generic;
using System.Text;

namespace TomLabs.SQuirreL.Objects
{
    public class RawObject
    {
		public string Schema { get; set; }
		public string Name { get; set; }

		public RawObject(string schema, string name)
		{
			Schema = schema;
			Name = name;
		}
    }
}
