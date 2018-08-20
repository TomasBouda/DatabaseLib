using System;
using System.Linq;
using TomLabs.SQuirreL.Connection;
using static System.Console;

namespace dbconsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var conn = ConnectionFactory.CreateMssqlConnection(".", "FairCredit");
			var a = conn.ExecuteDataSet("select * from users");
			ReadLine();
		}
	}
}