using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace LargeMessageSubscriber.Persistence.Repositories.SqlServer.ConnectionFactory.Contracts
{
    public interface IReadConnectionFactory : IDisposable
    {
        IDbConnection CreateConnection(IDbConnection dbConnection);

        void CloseConnection(IDbConnection dbConnection);
    }
}
