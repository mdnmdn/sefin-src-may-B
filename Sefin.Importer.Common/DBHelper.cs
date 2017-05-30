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

        private DbConnection _commonConnection;
        static object _connectionLock = new object();

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

        public DbConnection GetStaticConnection()
        {
            if (_commonConnection == null)
            {
                lock (_connectionLock)
                {
                    if (_commonConnection == null)
                    {
                        _commonConnection = GetConnection();
                        _commonConnection.Open();
                    }
                }
            }
            return _commonConnection;
        }

        public void AddParameter(DbCommand cmd, string param, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = param;
            parameter.Value = value;
            cmd.Parameters.Add(parameter);
        }

        #region Singleton

        static object _singletonLock = new object();

        static DBHelper _instance;

        static public DBHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DBHelper();
                        }
                    }
                }

                return _instance;
            }
        }

        



        #endregion
    }
}
