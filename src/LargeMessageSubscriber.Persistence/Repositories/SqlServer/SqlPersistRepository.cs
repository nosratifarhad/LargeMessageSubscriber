using Dapper;
using LargeMessageSubscriber.Domain.LargeMessage;
using LargeMessageSubscriber.Domain.LargeMessage.Entities;
using LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts;
using System.Data;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer
{
    public class SqlPersistRepository : IRepository
    {
        private bool disposedValue;

        private readonly IWriteConnectionFactory _writeConnectionFactory;
        private readonly IReadConnectionFactory _readConnectionFactory;
        private IDbConnection _dbConnection;

        public SqlPersistRepository(
            IWriteConnectionFactory writeConnectionFactory,
            IReadConnectionFactory readConnectionFactory)
        {
            _writeConnectionFactory = writeConnectionFactory;
        }

        public async Task<bool> IsExistMessage(string dataPointName)
        {
            string query = GetIsExistMessageQuery();

            var parameters = GetIsExistMessageParamters(dataPointName);

            var connection = GetConnection();

            var result = await connection.QueryFirstOrDefaultAsync<bool>(query, parameters)
                 .ConfigureAwait(false);

            return result;
        }

        public async Task InsertMessagesAsync(IEnumerable<DataPoint> messages)
        {
            var command = @"INSERT INTO DailyDataSubset 
                                    (Name,Timestamp, Value, CreatedAt) 
                                    VALUES 
                                    (@Name, @Timestamp,@Value, @CreatedAt)";

            foreach (var message in messages)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", message.Name, DbType.String);
                parameters.Add("@Timestamp", message.Timestamp, DbType.DateTime);
                parameters.Add("@Value", message.Value, DbType.Double);
                parameters.Add("@CreatedAt", DateTime.UtcNow, DbType.DateTime);

                await _writeConnectionFactory.Connection.ExecuteAsync(command, parameters, 
                    transaction: _writeConnectionFactory.DbTransaction).ConfigureAwait(false);
            }

            _writeConnectionFactory.Commit();
        }

        public async Task UpdateMessagesAsync(IEnumerable<DataPoint> messages)
        {
            var command = @"UPDATE DailyDataSubset 
                            SET 
                            Value = @Value, 
                            UpdatedAt = @UpdatedAt
                            WHERE Timestamp = @Timestamp";

            foreach (var message in messages)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", message.Name, DbType.String);
                parameters.Add("@Timestamp", message.Timestamp, DbType.DateTime);
                parameters.Add("@Value", message.Value, DbType.Double);
                parameters.Add("@UpdatedAt", DateTime.UtcNow, DbType.DateTime);

                await _writeConnectionFactory.Connection.ExecuteAsync(command, parameters,
                    transaction: _writeConnectionFactory.DbTransaction).ConfigureAwait(false);
            }

            _writeConnectionFactory.Commit();
        }

        public Task<IEnumerable<object>> GetDataPointPagination(int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        private string GetIsExistMessageQuery()
                => @"SELECT TOP 1 1 FROM DailyDataSubset WHERE Id = @DataPointId";

        private DynamicParameters GetIsExistMessageParamters(string dataPointId)
        {
            var paramters = new DynamicParameters();

            paramters.Add("DataPointId", dataPointId, DbType.Guid);

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
