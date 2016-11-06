using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lib.DBMS
{
	public abstract class DB : IDisposable
	{
		protected DB() { }

		public IDbConnection Connection { get; set; }

		public IDbTransaction Transaction { get; set; }

		public virtual bool IsConnected
		{
			get { return Connection?.State == ConnectionState.Open; }
		}

		/// <summary>
		/// Connects to a DB using given connection string
		/// Use: Connect<TConncetion, TException>(connectionString, c => new TConncetion(c))
		/// </summary>
		/// <typeparam name="TConncetion"></typeparam>
		/// <typeparam name="TException"></typeparam>
		/// <param name="connectionString"></param>
		/// <param name="conn"></param>
		/// <returns></returns>
		public bool Connect<TConncetion, TException>(string connectionString, Func<string, TConncetion> conn) 
			where TConncetion : IDbConnection, new()
			where TException : DbException
		{
			Connection = conn(connectionString);

			try
			{
				Connection.Open();
				return true;
			}
			catch (TException ex)	// TODO
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
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
