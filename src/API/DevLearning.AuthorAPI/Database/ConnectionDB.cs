using Microsoft.Data.SqlClient;

namespace DevLearning.AuthorAPI.Database
{
    public class ConnectionDB
    {
        private readonly string _connectionString;
        public ConnectionDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer")!;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
