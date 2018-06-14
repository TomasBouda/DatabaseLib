using System;
using System.Collections.Generic;
using System.Data;
using TomLabs.SQuirreL.Connection.ConnectionParams;
using TomLabs.SQuirreL.Data;

namespace TomLabs.SQuirreL.DataProviders
{
	public interface IDB : IDisposable
	{
		bool IsConnected { get; }
		IDbConnection Connection { get; }
		IDbTransaction Transaction { get; }

		void Connect(string connectionString);

		void Connect<TConn>(TConn @params) where TConn : IConnectionParams;

		bool Disconnect();


		#region Execute

		int Execute(string query);

		string ExecuteScalar(string query);

		IDataReader ExecuteReader(string query);

		DataSet ExecuteDataSet(string query); 

		#endregion


		IList<IDbObject> GetObjects(ObjectTypes including = ObjectTypes.All);

		string GetScriptFor(string objectName, ObjectTypes objType = ObjectTypes.None);

		DataSet GetColumnsInfo(string schema, string tableName);

		IList<string> FindColumn(string columnName);

		IList<string> FindInScripts(string query);
	}
}