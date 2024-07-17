using System.Data;
using System.Data.SqlClient;

namespace WebApplicationDapperTest.Context
{
    public class DapperContext 
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ApplicationException("Null configuration stiring");
        }


        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
