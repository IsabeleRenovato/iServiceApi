using MySql.Data.MySqlClient;
using System.Data;

namespace iServiceRepositories
{
    public class MySqlConnectionSingleton
    {
        private readonly string _connectionString;
        private static MySqlConnection _connection;

        public MySqlConnectionSingleton(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new MySqlConnection(_connectionString);
        }

        public MySqlConnection GetConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
