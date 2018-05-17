using System;
using System.Linq;
using TomLabs.SQuirreL.DataProviders;
using static System.Console;

namespace dbconsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var conn = new MSSQL(".", "FairCredit"))
			{
				foreach (Tuple<string, string> table in conn.GetTables().OrderBy(o => o).ToList())
					WriteLine($"{table.Item1}.{table.Item2}");
			}

			ReadLine();
		}
	}
}