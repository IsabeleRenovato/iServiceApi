using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using iServiceServices.Services.Models.Auth;
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
            var userCheck = new UserRepository(_configuration).GetByEmail(model.Email);

            if (userCheck != null)
            {
                return Result<UserInfo>.Failure("Usuário já cadastrado.");
            }

            try
            {
                var user = new UserRepository(_configuration).Insert(new UserModel
                {
                    UserRoleID = model.UserRoleID,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Name = model.Name,
                });

                var userRole = new UserRoleRepository(_configuration).GetById(model.UserRoleID);

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
            var user = new UserRepository(_configuration).GetById(model.UserId);
            
            if (user == null)
            {
                return Result<UserInfo>.Failure("Usuário não cadastrado.");
            }

            var userRole = new UserRoleRepository(_configuration).GetById(user.UserRoleID);

            if (userRole == null)
            {
                return Result<UserInfo>.Failure("Falha ao buscar os dados do usuário.");
            }

            try
            {
                var address = new AddressRepository(_configuration).Insert(new AddressModel
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

                if (model.EstablishmentProfile != null)
                {
                    establishmentProfile = new EstablishmentProfileRepository(_configuration).Insert(new EstablishmentProfileModel
                    {
                        UserID = model.UserId,
                        CNPJ = model.EstablishmentProfile.CNPJ,
                        CommercialName = model.EstablishmentProfile.CommercialName,
                        EstablishmentCategoryID = model.EstablishmentProfile.EstablishmentCategoryID,
                        AddressID = address.AddressID,
                        Description = model.EstablishmentProfile.Description,
                        CommercialPhone = model.EstablishmentProfile.CommercialPhone,
                        CommercialEmail = model.EstablishmentProfile.CommercialEmail,
                        Logo = new byte[0]
                    });
                }
                else if (model.ClientProfile != null)
                {
                    clientProfile = new ClientProfileRepository(_configuration).Insert(new ClientProfileModel
                    {
                        UserID = model.UserId,
                        CPF = model.ClientProfile.CPF,
                        DateOfBirth = model.ClientProfile.DateOfBirth,
                        Phone = model.ClientProfile.Phone,
                        AddressID = address.AddressID,
                        ProfilePicture = new byte[0]
                    });
                }
                else
                {
                    return Result<UserInfo>.Failure("Falha no registro do usuário.");
                }

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                    ClientProfile = clientProfile?.ClientProfileID > 0 ? clientProfile : null,
                    EstablishmentProfile = establishmentProfile?.EstablishmentProfileID > 0 ? establishmentProfile : null,
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
                var user = new UserRepository(_configuration).GetByEmail(model.Email);

                if (user == null)
                {
                    return Result<UserInfo>.Failure("Usuário não cadastrado.");
                }

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    return Result<UserInfo>.Failure("Email ou senha incorretos.");
                }

                var userRole = new UserRoleRepository(_configuration).GetById(user.UserRoleID);

                if (userRole == null)
                {
                    return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (UserRole)");
                }

                var profileResult = LoadUserProfile(user.UserID, user.UserRoleID);

                if (!profileResult.IsSuccess)
                {
                    return Result<UserInfo>.Failure(profileResult.ErrorMessage);
                }

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                    EstablishmentProfile = profileResult.Value.EstablishmentProfile?.EstablishmentProfileID > 0 ? profileResult.Value.EstablishmentProfile : null,
                    ClientProfile = profileResult.Value.ClientProfile?.ClientProfileID > 0 ? profileResult.Value.ClientProfile : null,
                    Address = profileResult.Value.Address
                });
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure($"Falha ao realizar o login do usuário: {ex.Message}");
            }
        }

        private Result<UserInfo> LoadUserProfile(int userId, int userRoleId)
        {
            var userInfo = new UserInfo();

            switch (userRoleId)
            {
                case 1:
                    var establishmentProfile = new EstablishmentProfileRepository(_configuration).GetByUserId(userId);
                    if (establishmentProfile == null)
                    {
                        return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (EstablishmentProfile)");
                    }
                    userInfo.EstablishmentProfile = establishmentProfile;
                    break;
                case 2:
                    var clientProfile = new ClientProfileRepository(_configuration).GetByUserId(userId);

                    if (clientProfile == null)
                    {
                        return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (ClientProfile)");
                    }
                    userInfo.ClientProfile = clientProfile;
                    break;
                default:
                    return Result<UserInfo>.Failure("Tipo de usuário desconhecido.");
            }

            userInfo.Address = new AddressRepository(_configuration).GetById(userInfo.EstablishmentProfile?.AddressID ?? userInfo.ClientProfile?.AddressID ?? 0);
            if (userInfo == null)
            {
                return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (Address)");
            }

            return Result<UserInfo>.Success(userInfo);
        }
    }
}
