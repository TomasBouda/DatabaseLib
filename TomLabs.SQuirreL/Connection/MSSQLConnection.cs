using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TomLabs.SQuirreL.Connection.ConnectionParams;
using TomLabs.SQuirreL.Data;
using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Connection
{
	public class MSSQLConnection : IConnection
	{
		private MSSQL DB { get; set; }

		public event EventHandler Connected;

		public event EventHandler Disconnected;

		public IEnumerable<IDbObject> Objects { get; private set; }

		public bool IsConnected => DB.IsConnected;

		public string ConnectionString { get; private set; }

		public MSSQLConnection()
		{
			DB = new MSSQL();
		}

		internal MSSQLConnection(IConnectionParams @params, bool connect = true) : this()
		{
			if (connect)
			{
				Connect(@params);
			}
		}

		public bool Connect(IConnectionParams @params)
		{
			if (@params is MSSQLConnectionParams)
			{
				DB.Connect(@params);

				if (IsConnected)
				{
					ConnectionString = DB.Connection.ConnectionString;

					Connected?.Invoke(this, new EventArgs());

					return true;
				}

				return false;
			}
			else
				throw new ArgumentException("Invalid ConnectionParams passed!");
		}

		public void Disconnect()
		{
			DB?.Disconnect();
			Disconnected?.Invoke(this, new EventArgs());
		}

		public IEnumerable<IDbObject> SearchInDb(string query = "", ObjectTypes searchIn = ObjectTypes.All, Sort sort = Sort.None)
		{
			return null;
		}

		public int Execute(string query)
		{
			return DB.Execute(query);
		}

		public IDataReader ExecuteReader(string query)
		{
			return DB.ExecuteReader(query);
		}

		public DataSet ExecuteDataSet(string query)
		{
			return DB.ExecuteDataSet(query);
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					Disconnect();
					DB.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~MSSQLConnection() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion IDisposable Support
	}
}