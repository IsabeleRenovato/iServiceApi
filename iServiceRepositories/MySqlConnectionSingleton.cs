using MySql.Data.MySqlClient;
using System.Data;

namespace iServiceRepositories
{
    public class MySqlConnectionSingleton : IDisposable
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        public MySqlConnectionSingleton(string connectionString)
        {
            _connectionString = connectionString;
            _connection = CreateConnection();
        }

        private MySqlConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public MySqlConnection GetConnection()
        {
            if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
            {
                _connection.Dispose(); // Dispose the old connection
                _connection = CreateConnection(); // Create a new connection
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
