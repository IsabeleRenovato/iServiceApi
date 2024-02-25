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
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
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
            try
            {
                var user = new UserRepository(_configuration).Login(model.Email, model.Password);
                if (user?.UserID == null || !BCrypt.Net.BCrypt.Verify(model.Password, user?.Password))
                {
                    return Result<UserInfo>.Failure("Email ou senha incorretos.");
                }

                var userRole = new UserRoleRepository(_configuration).Get(user.UserRoleID);
                if (userRole?.UserRoleID == null)
                {
                    return Result<UserInfo>.Failure("Falha ao realizar o login do usuário. (UserRole)");
                }

                var profileResult = LoadUserProfile(user.UserRoleID, user.UserID.GetValueOrDefault());
                if (!profileResult.IsSuccess)
                {
                    return Result<UserInfo>.Failure(profileResult.ErrorMessage);
                }

                var userInfo = new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                    EstablishmentProfile = profileResult.Value.EstablishmentProfile,
                    ClientProfile = profileResult.Value.ClientProfile,
                    Address = profileResult.Value.Address
                };

                return Result<UserInfo>.Success(userInfo);
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure($"Falha ao realizar o login do usuário: {ex.Message}");
            }
        }

        private Result<UserInfo> LoadUserProfile(int userRoleId, int userId)
        {
            var userInfo = new UserInfo();

            switch (userRoleId)
            {
                case 1:
                    var establishmentProfile = new EstablishmentRepository(_configuration).Get(userId);
                    if (establishmentProfile?.EstablishmentProfileID == null)
                    {
                        return Result<UserInfo>.Failure("Falha ao realizar o login do usuário. (EstablishmentProfile)");
                    }
                    userInfo.EstablishmentProfile = establishmentProfile;
                    break;
                case 2:
                    var clientProfile = new ClientRepository(_configuration).Get(userId);
                    if (clientProfile?.ClientProfileID == null)
                    {
                        return Result<UserInfo>.Failure("Falha ao realizar o login do usuário. (ClientProfile)");
                    }
                    userInfo.ClientProfile = clientProfile;
                    break;
                default:
                    return Result<UserInfo>.Failure("Tipo de usuário desconhecido.");
            }

            userInfo.Address = new AddressRepository(_configuration).Get(userInfo.EstablishmentProfile?.AddressID ?? userInfo.ClientProfile?.AddressID ?? 0);
            if (userInfo.Address?.AddressID == null)
            {
                return Result<UserInfo>.Failure("Falha ao realizar o login do usuário. (Address)");
            }

            return Result<UserInfo>.Success(userInfo);
        }
    }
}
