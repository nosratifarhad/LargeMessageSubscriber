using LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts;
using System.Data;
using System.Data.SqlClient;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory
{
    internal class ReadDbConnectionFactory : IReadConnectionFactory
    {
        private bool disposedValue;

        private readonly string _connectionString;
        protected IDbConnection _dbConnection;

        public ReadDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            if (_dbConnection == null || dbConnection.State != ConnectionState.Open)
            {
                _dbConnection = new SqlConnection(_connectionString);
                _dbConnection.Open();
            }
            return _dbConnection;
        }

        public void CloseConnection(IDbConnection dbConnection)
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                try
                {
                    dbConnection.Close();
                }
                catch
                {
                }
                finally
                {
                    Dispose();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            CloseConnection(_dbConnection);
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
