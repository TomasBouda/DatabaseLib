using Database.Lib.DBMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Data
{

	public abstract class DbObject<T> where T : class, IDB<T>, new()
	{
		protected T DB { get; set; }

		public string Name { get; set; }

		public bool IsLoaded { get; set; }

		public DbObject() { }

		public DbObject(string name)
		{
			Name = name;
		}
	}
}
