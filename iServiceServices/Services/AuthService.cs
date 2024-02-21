using iServiceRepositories.Models;
using iServiceRepositories.Models.Auth;
using iServiceRepositories.Repositories;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (User User, UserRole UserRole) Get(Login model)
        {
            var user = new UserRepository(_configuration).Get(model.Email, model.Password);

            return user;
        }

        public (User User, UserRole UserRole) PreRegister(PreRegister model)
        {
            new UserRepository(_configuration).Insert(model);

            var user = new UserRepository(_configuration).Get(model.Email, model.Password);

            return user;
        }

        public (User User, UserRole UserRole, ClientProfile ClientProfile, EstablishmentProfile EstablishmentProfile, Address Adress) Register(Models.Auth.Register model)
        {
            var auth = new UserRepository(_configuration).GetUser(model.UserId);

            if (auth.User == null)
            {
                return (null, null, null, null, null);
            }

            var address = new AddressRepository(_configuration).Insert(new Address
            {
                Street = model.Address.Street,
                Number = model.Address.Number,
                AdditionalInfo = model.Address.AdditionalInfo,
                City = model.Address.City,
                State = model.Address.State,
                Country = model.Address.Country,
                PostalCode = model.Address.PostalCode
            });

            var clientProfile = new ClientProfile();
            var establishmentProfile = new EstablishmentProfile();

            if (string.IsNullOrEmpty(model.Client.CPF))
            {
                establishmentProfile = new EstablishmentRepository(_configuration).Insert(new EstablishmentProfile
                {
                    UserID = model.UserId,
                    CNPJ = model.Establishment.CNPJ,
                    CommercialName = model.Establishment.CommercialName,
                    AddressID = address.AddressID,
                    Description = model.Establishment.Description,
                    CommercialPhone = model.Establishment.CommercialPhone,
                    CommercialEmail = model.Establishment.CommercialEmail,
                    Logo = new byte[0]
                });
            }
            else
            {
                clientProfile = new ClientRepository(_configuration).Insert(new ClientProfile
                {
                    UserID = model.UserId,
                    CPF = model.Client.CPF,
                    DateOfBirth = model.Client.DateOfBirth.GetValueOrDefault(),
                    Phone = model.Client.Phone,
                    AddressID = address.AddressID,
                    ProfilePicture = new byte[0]
                });
            }

            return (auth.User, auth.UserRole, clientProfile, establishmentProfile, address);
        }
    }
}
