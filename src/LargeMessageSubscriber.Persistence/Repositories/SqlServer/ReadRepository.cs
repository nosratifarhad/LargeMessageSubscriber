using Dapper;
using LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts;
using System.Data;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer
{
    public class ReadRepository
    {
        private bool disposedValue;

        private readonly IReadConnectionFactory _readConnectionFactory;
        private IDbConnection _dbConnection;

        public ReadRepository(IReadConnectionFactory readConnectionFactory)
        {
            _readConnectionFactory = readConnectionFactory;
        }

        public async Task<bool> IsExistMessage(string phoneNumber)
        {
            string query = GetIsExistMessageQuery();

            var parameters = GetIsExistMessageParamters(phoneNumber);

            var connection = GetConnection();

            var result = await connection.QueryFirstOrDefaultAsync<bool>(query, parameters)
                 .ConfigureAwait(false);

            return result;
        }

        private string GetIsExistMessageQuery()
            => @"";

        private DynamicParameters GetIsExistMessageParamters(string phoneNumber)
        {
            var paramters = new DynamicParameters();

            paramters.Add("PhoneNumber", phoneNumber, DbType.String);

            return paramters;
        }


        private IDbConnection GetConnection()
        {
            _dbConnection = _readConnectionFactory.CreateConnection(_dbConnection);
            return _dbConnection;
        }

        private void CloseConnection()
        {
            _readConnectionFactory.CloseConnection(_dbConnection);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbConnection?.Dispose();
                    CloseConnection();
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
