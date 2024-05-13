using MySql.Data.MySqlClient;
using System.Data;

namespace iServiceRepositories
{
    public class MySqlConnectionSingleton : IAsyncDisposable
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        public MySqlConnectionSingleton(string connectionString)
        {
            _connectionString = connectionString;
            _connection = CreateConnectionAsync().Result; // Chamada síncrona para estabelecer a conexão inicial
        }

        private async Task<MySqlConnection> CreateConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<MySqlConnection> GetConnectionAsync()
        {
            if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
            {
                await DisposeAsync(); // Dispose the old connection
                _connection = await CreateConnectionAsync(); // Create a new connection asynchronously
            }

            return _connection;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}
