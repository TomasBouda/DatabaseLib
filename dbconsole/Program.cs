using Database.Lib.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace dbconsole
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var conn = new MSSQL(".", "FairCredit"))
			{
				foreach (string table in conn.GetTables().OrderBy(o => o).ToList())
					WriteLine(table);
			}

			ReadLine();
		}
	}
}
