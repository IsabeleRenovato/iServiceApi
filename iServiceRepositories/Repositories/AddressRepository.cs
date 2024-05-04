using Dapper;
using iServiceRepositories.Repositories.Models;
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
                return connection.Query<Address>("SELECT AddressId, Street, Number, Neighborhood, AdditionalInfo, City, State, Country, PostalCode, Active, Deleted, CreationDate, LastUpdateDate FROM Address").AsList();
            }
        }

        public Address GetById(int addressId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Address>("SELECT AddressId, Street, Number, Neighborhood, AdditionalInfo, City, State, Country, PostalCode, Active, Deleted, CreationDate, LastUpdateDate FROM Address WHERE AddressId = @AddressId", new { AddressId = addressId });
            }
        }

        public Address Insert(AddressInsert addressModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Address (Street, Number, Neighborhood, AdditionalInfo, City, State, Country, PostalCode) VALUES (@Street, @Number, @Neighborhood, @AdditionalInfo, @City, @State, @Country, @PostalCode); SELECT LAST_INSERT_Id();", addressModel);
                return GetById(id);
            }
        }

        public Address Update(AddressUpdate addressUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Address SET Street = @Street, Number = @Number, Neighborhood = @Neighborhood, AdditionalInfo = @AdditionalInfo, City = @City, State = @State, Country = @Country, PostalCode = @PostalCode, LastUpdateDate = NOW() WHERE AddressId = @AddressId", addressUpdateModel);
                return GetById(addressUpdateModel.AddressId);
            }
        }

        public void SetActiveStatus(int addressId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Address SET Active = @IsActive WHERE AddressId = @AddressId", new { IsActive = isActive, AddressId = addressId });
            }
        }

        public void SetDeletedStatus(int addressId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Address SET Deleted = @IsDeleted WHERE AddressId = @AddressId", new { IsDeleted = isDeleted, AddressId = addressId });
            }
        }
    }
}
