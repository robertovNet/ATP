using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ATP.Common.ConnectionManagement
{
    public static class ConnectionFactory
    {
        private static readonly ConnectionInfo connectionInfo;

        static ConnectionFactory()
        {
            connectionInfo = GetConnectionInfo("ATP.Database");
        }

        private static ConnectionInfo GetConnectionInfo(string connectionStringKey)
        {
            var connString = ConfigurationManager.ConnectionStrings[connectionStringKey];
            if (connString == null || String.IsNullOrEmpty(connString.ConnectionString)
                || String.IsNullOrEmpty(connString.ProviderName) || String.IsNullOrEmpty(connString.Name))
                throw new Exception(string.Format(Messages.ConnectionStringsNotFound, connectionStringKey));

            return new ConnectionInfo(connString.ConnectionString, connString.ProviderName, connString.Name);
        }

        public static IDbConnection GetOpenConnection()
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(connectionInfo.DbProvider);
                var conn = factory.CreateConnection();
                conn.ConnectionString = connectionInfo.ConnectionString;
                conn.Open();
                return new ATPDbConnection(conn);
            }
            catch (Exception e)
            {
                string message = String.Format("Ha ocurrido un error inesperado al intentar iniciar la conexión con la base de datos [{0}].", connectionInfo.ConnectionName);
                throw new Exception(message, e);
            }
        }

        public static async Task<IDbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(connectionInfo.DbProvider);
                var conn = factory.CreateConnection();
                conn.ConnectionString = connectionInfo.ConnectionString;
                await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
                return new ATPDbConnection(conn);
            }
            catch (Exception e)
            {
                string message = String.Format("Ha ocurrido un error inesperado al intentar iniciar la conexión con la base de datos [{0}].", connectionInfo.ConnectionName);
                throw new Exception(message, e);
            }
        }

        public static IDbConnection GetOpenConnection(string connectionName)
        {
            try
            {
                var connection = GetConnectionInfo("Teleton.His.Database");
                var factory = DbProviderFactories.GetFactory(connection.DbProvider);
                var conn = factory.CreateConnection();
                conn.ConnectionString = connectionInfo.ConnectionString;
                conn.Open();
                return new ATPDbConnection(conn);
            }
            catch (Exception e)
            {
                string message = String.Format("Ha ocurrido un error inesperado al intentar iniciar la conexión con la base de datos [{0}].", connectionInfo.ConnectionName);
                throw new Exception(message, e);
            }
        }

        public static async Task<IDbConnection> GetOpenConnectionAsync(string connectionName, CancellationToken cancellationToken)
        {
            try
            {
                var connection = GetConnectionInfo(connectionName);
                var factory = DbProviderFactories.GetFactory(connection.DbProvider);

                var conn = factory.CreateConnection();
                conn.ConnectionString = connection.ConnectionString;

                await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
                return new ATPDbConnection(conn);
            }
            catch (Exception e)
            {
                string message = String.Format("Ha ocurrido un error inesperado al intentar iniciar la conexión con la base de datos [{0}].", connectionInfo.ConnectionName);
                throw new Exception(message, e);
            }
        }
    }

    public class ConnectionInfo
    {
        public ConnectionInfo(string connectionString, string dbProvider, string connectionName)
        {
            ConnectionString = connectionString;
            DbProvider = dbProvider;
            ConnectionName = connectionName;
        }

        public string ConnectionString { get; private set; }

        public string DbProvider { get; private set; }

        public string ConnectionName { get; private set; }
    }
}
