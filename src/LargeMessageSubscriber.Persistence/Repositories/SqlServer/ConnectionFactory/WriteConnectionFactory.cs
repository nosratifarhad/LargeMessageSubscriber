using System.Data;
using System.Data.SqlClient;
using LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory
{
    public class WriteConnectionFactory : IWriteConnectionFactory
    {
        private bool disposedValue;

        protected IDbConnection _dbConnection;

        protected IDbTransaction _dbTransaction;

        private readonly string _connectionString;

        public WriteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            if (dbConnection == null || dbConnection.State != ConnectionState.Open)
            {
                _dbConnection = new SqlConnection(_connectionString);
                _dbConnection.Open();
            }
            _dbTransaction = _dbConnection.BeginTransaction();
            return _dbConnection;
        }

        public IDbConnection Connection
        {
            get
            {
                return _dbConnection;
            }
        }

        public IDbTransaction DbTransaction
        {
            get
            {
                return _dbTransaction;
            }
        }

        public virtual void Commit()
        {
            _dbTransaction.Commit();
            _dbTransaction.Dispose();
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        public virtual void Rollback()
        {
            _dbTransaction.Rollback();
            _dbTransaction.Dispose();
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
