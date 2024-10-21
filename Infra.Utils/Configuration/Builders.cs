using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infra.Utils.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class Builders
    {
        public static string BuildConnectionString(IConfiguration configuration)
        {
            var sqlBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("SqlServer"))
            {
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                ConnectTimeout = 15
            };
            return sqlBuilder.ConnectionString;
        }
    }
}
