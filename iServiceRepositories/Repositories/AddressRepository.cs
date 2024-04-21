using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class AddressRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public AddressRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<Address> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Address>("SELECT AddressID, Street, Number, AdditionalInfo, City, State, Country, PostalCode, CreationDate, LastUpdateDate FROM Address").AsList();
            }
        }

        public Address GetById(int addressId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Address>("SELECT AddressID, Street, Number, AdditionalInfo, City, State, Country, PostalCode, CreationDate, LastUpdateDate FROM Address WHERE AddressID = @AddressID", new { AddressID = addressId });
            }
        }

        public Address Insert(AddressModel addressModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Address (Street, Number, AdditionalInfo, City, State, Country, PostalCode) VALUES (@Street, @Number, @AdditionalInfo, @City, @State, @Country, @PostalCode); SELECT LAST_INSERT_ID();", addressModel);
                return GetById(id);
            }
        }

        public Address Update(Address address)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Address SET Street = @Street, Number = @Number, AdditionalInfo = @AdditionalInfo, City = @City, State = @State, Country = @Country, PostalCode = @PostalCode, LastUpdateDate = NOW() WHERE AddressID = @AddressID", address);
                return GetById(address.AddressId);
            }
        }

        public bool Delete(int addressId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Address WHERE AddressID = @AddressID", new { AddressID = addressId });
                return affectedRows > 0;
            }
        }
    }
}
