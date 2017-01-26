using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{

	public abstract class DbObject<T> where T : class, IDB, new()
	{
		protected T DB { get; set; }

		public string Schema { get; set; }

		public string Name { get; set; }

		public bool IsLoaded { get; set; }

		public DbObject() { }

		public DbObject(string schema, string name)
		{
			Schema = schema;
			Name = name;
		}

		public override string ToString()
		{
			return Schema + "." + Name;
		}
	}
}
