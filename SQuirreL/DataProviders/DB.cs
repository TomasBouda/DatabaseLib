using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomLabs.OpenSource.SQuirreL.DataProviders
{
	public abstract class DB : IDisposable
	{
		protected DB() { }

		public IDbConnection Connection { get; protected set; }

		public IDbTransaction Transaction { get; protected set; }

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
		protected void Connect<TConncetion, TException>(string connectionString, Func<string, TConncetion> conn) 
			where TConncetion : IDbConnection, new()
			where TException : DbException
		{
			Connection = conn(connectionString);

			try
			{
				Connection.Open();
			}
			catch (TException ex)	// TODO
			{
				Console.WriteLine(ex.Message);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public void BeginTransaction()
		{
			Transaction = Connection?.BeginTransaction();
		}

		public void CommitTransaction()
		{
			Transaction.Commit();
		}

		public void RollBackTransaction()
		{
			Transaction.Rollback();
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

		protected virtual IList<Tuple<string,string>> GetCollection<TConnection>(string collectionName, string[] restrictions = null) where TConnection : DbConnection
		{
			List<Tuple<string, string>> objects = new List<Tuple<string, string>>();
			DataTable dt = ((TConnection)Connection).GetSchema(collectionName, restrictions);
			foreach (DataRow row in dt.Rows)
			{
				string objectSchema = (string)row[1];
				string objectName = (string)row[2];
				objects.Add(new Tuple<string, string>(objectSchema, objectName));
			}
			return objects;
		}
	}
}
