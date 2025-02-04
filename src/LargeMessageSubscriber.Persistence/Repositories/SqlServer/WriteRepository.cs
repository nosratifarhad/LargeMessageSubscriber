using Dapper;
using LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts;
using LargeMessageSubscriber.Shared.Models;
using System.Data;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer
{
    public class WriteRepository
    {
        private readonly IWriteConnectionFactory _writeConnectionFactory;

        public WriteRepository(IWriteConnectionFactory writeConnectionFactory)
        {
            _writeConnectionFactory = writeConnectionFactory;
        }

        public async Task InsertMessagesAsync(IEnumerable<MockDataPoint> messages)
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

        public async Task UpdateMessagesAsync(IEnumerable<MockDataPoint> messages)
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

    }
}
