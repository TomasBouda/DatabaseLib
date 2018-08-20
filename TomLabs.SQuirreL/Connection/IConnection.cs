using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TomLabs.SQuirreL.Connection.ConnectionParams;
using TomLabs.SQuirreL.Data;
using TomLabs.SQuirreL.DataProviders;

namespace TomLabs.SQuirreL.Connection
{
	public interface IConnection : IDisposable
	{
		IEnumerable<IDbObject> Objects { get; }

		bool IsConnected { get; }
		string ConnectionString { get; }

		bool Connect(IConnectionParams @params);

		void Disconnect();

		event EventHandler Connected;

		event EventHandler Disconnected;

		IEnumerable<IDbObject> SearchInDb(string query = "", ObjectTypes searchIn = ObjectTypes.All, Sort sort = Sort.None);

		int Execute(string query);

		IDataReader ExecuteReader(string query);

		DataSet ExecuteDataSet(string query);
	}
}