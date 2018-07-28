using System;
using System.Collections.Generic;
using System.Text;

namespace TomLabs.SQuirreL.Objects
{
    public class RawDbObject
    {
		public string Schema { get; set; }
		public string Name { get; set; }

		public RawDbObject(string schema, string name)
		{
			Schema = schema;
			Name = name;
		}
    }
}
