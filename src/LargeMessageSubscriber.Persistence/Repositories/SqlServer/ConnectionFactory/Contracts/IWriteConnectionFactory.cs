using System.Data;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts
{
    public interface IWriteConnectionFactory : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction DbTransaction { get; }

        IDbConnection CreateConnection(IDbConnection dbConnection);

        void Commit();

        void Rollback();
    }
}
