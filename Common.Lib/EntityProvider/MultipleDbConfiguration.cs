using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;

namespace Common.Lib.EntityProvider
{
    public class MultipleDbConfiguration : DbConfiguration
    {
        #region Constructors

        public MultipleDbConfiguration()
        {
            SetProviderServices(MySqlProviderInvariantName.ProviderName, new MySqlProviderServices());
        }

        #endregion Constructors

        #region Public methods

        public static DbConnection GetMySqlConnection(string connectionString)
        {
            var connectionFactory = new MySqlConnectionFactory();

            return connectionFactory.CreateConnection(connectionString);
        }

        public static DbConnection GetSqlConnection(string connectionString)
        {
            var connectionFactory = new SqlConnectionFactory();

            return connectionFactory.CreateConnection(connectionString);
        }

        #endregion Public methods
    }
}