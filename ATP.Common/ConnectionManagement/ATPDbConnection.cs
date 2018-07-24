using System.Data;

namespace ATP.Common.ConnectionManagement
{
    public class ATPDbConnection : IDbConnection
    {
        public ATPDbConnection(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public IDbConnection DbConnection { get; private set; }

        public string ConnectionString
        {
            get
            {
                return DbConnection.ConnectionString;
            }

            set
            {
                DbConnection.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                return DbConnection.ConnectionTimeout;
            }
        }

        public string Database
        {
            get
            {
                return DbConnection.Database;
            }
        }

        public ConnectionState State
        {
            get
            {
                return DbConnection.State;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return DbConnection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return DbConnection.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            DbConnection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            DbConnection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return DbConnection.CreateCommand();
        }

        public void Open()
        {
            DbConnection.Open();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Restaurar Isolation Level de la conexión antes de devolverla al pool
                    using (var command = DbConnection.CreateCommand())
                    {
                        command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
                        command.ExecuteNonQuery();
                    }

                    DbConnection.Dispose();
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TeletonDbConnection() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
