using Microsoft.Data.SqlClient;

namespace DevLearning.CourseAPI.Database
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
