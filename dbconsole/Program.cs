using TomLabs.OpenSource.SQuirreL.DataProviders;
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
				foreach (Tuple<string, string> table in conn.GetTables().OrderBy(o => o).ToList())
					WriteLine($"{table.Item1}.{table.Item2}");
			}

			ReadLine();
		}
	}
}
