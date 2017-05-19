using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Importer.Common
{
    public class DBHelper
    {
        private ConnectionStringSettings _connectionString;
        private DbProviderFactory _factory;

        public void Init(ConnectionStringSettings connectionString)
        {
            _connectionString = connectionString;
            _factory = DbProviderFactories.GetFactory(_connectionString.ProviderName);
        }

        public DbConnection GetConnection()
        {
            var connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString.ConnectionString;
            return connection;
        }


        public void AddParameter(DbCommand cmd, string param, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = param;
            parameter.Value = value;
            cmd.Parameters.Add(parameter);
        }

        #region Singleton

        static object _lock = new object();

        static DBHelper _instance;

        static public DBHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DBHelper();
                }

                return _instance;
            }
        }

        

        #endregion
    }
}
