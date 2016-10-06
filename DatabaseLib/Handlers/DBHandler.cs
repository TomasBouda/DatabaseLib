using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.Handlers
{
	public abstract class DBHandler
	{
		protected DBHandler() { }

		public IDbConnection Connection { get; set; }

		public IDbTransaction Transaction { get; set; }

		public virtual bool IsConnected
		{
			get { return Connection?.State == ConnectionState.Open; }
		}

		public bool Disconnect()
		{
			try
			{
				Connection.Close();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public void Dispose()
		{
			if (Disconnect())
				Connection = null;
		}

		protected virtual IList<string> GetCollection<TConnection>(string collectionName, string[] restrictions = null) where TConnection : DbConnection
		{
			List<string> tables = new List<string>();
			DataTable dt = ((TConnection)Connection).GetSchema(collectionName, restrictions);
			foreach (DataRow row in dt.Rows)
			{
				string tablename = (string)row[2];
				tables.Add(tablename);
			}
			return tables;
		}
	}
}
