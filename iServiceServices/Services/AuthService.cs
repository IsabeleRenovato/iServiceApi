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

        public Result<UserInfo> PreRegister(PreRegister model)
        {
            if (new UserRepository(_configuration).CheckUser(model.Email))
            {
                return Result<UserInfo>.Failure("Usuário já cadastrado.");
            }

            try
            {
                var user = new UserRepository(_configuration).Insert(new User
                {
                    UserRoleID = model.UserRoleID,
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                });

                var userRole = new UserRoleRepository(_configuration).Get(model.UserRoleID);

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = user,
                    UserRole = userRole
                });
            }
            catch (Exception)
            {
                return Result<UserInfo>.Failure("Falha no registro do usuário.");
            }
        }

        public Result<UserInfo> Register(Models.Auth.Register model)
        {
            var auth = new UserRepository(_configuration).GetUser(model.UserId);

            if (auth.User == null)
            {
                return Result<UserInfo>.Failure("Usuário não cadastrado.");
            }

            try
            {
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

                if (string.IsNullOrEmpty(model?.Client?.CPF))
                {
                    establishmentProfile = new EstablishmentRepository(_configuration).Insert(new EstablishmentProfile
                    {
                        UserID = model.UserId,
                        CNPJ = model.Establishment.CNPJ,
                        CommercialName = model.Establishment.CommercialName,
                        AddressID = address.AddressID.GetValueOrDefault(),
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
                        DateOfBirth = model.Client.DateOfBirth,
                        Phone = model.Client.Phone,
                        AddressID = address.AddressID.GetValueOrDefault(),
                        ProfilePicture = new byte[0]
                    });
                }

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = auth.User,
                    UserRole = auth.UserRole,
                    ClientProfile = clientProfile,
                    EstablishmentProfile = establishmentProfile,
                    Address = address
                });
            }
            catch (Exception)
            {
                return Result<UserInfo>.Failure("Falha no registro do usuário.");
            }
        }

        public Result<UserInfo> Login(Login model)
        {
            var userInfo = new UserInfo();
            userInfo.User = new UserRepository(_configuration).Login(model.Email, model.Password);

            return Result<UserInfo>.Failure("Usuário não cadastrado.");
        }
    }
}
