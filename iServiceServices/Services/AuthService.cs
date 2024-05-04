using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
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
            try
            {
                if (new UserRepository(_configuration).CheckUser(model.Email))
                {
                    return Result<UserInfo>.Failure("Usuário já cadastrado.");
                }

                var userRole = new UserRoleRepository(_configuration).GetById(model.UserRoleId);

                if (userRole.UserRoleId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Falha ao recuperar a Role do usuário.");
                }

                var user = new UserRepository(_configuration).Insert(new UserInsert
                {
                    UserRoleId = model.UserRoleId,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Name = model.Name
                });

                if (user.UserId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Falha no registro do usuário.");
                }

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                });
            }
            catch (Exception)
            {
                return Result<UserInfo>.Failure("Falha no registro do usuário.");
            }
        }
        public Result<UserInfo> Register(Register model)
        {
            try
            {
                var typeDocument = new UtilService().ValidadorDeDocumento(model.UserProfile.Document);

                if (typeDocument && model.Address == null)
                {
                    return Result<UserInfo>.Failure("Por favor informe o endereço.");
                }
            }
            catch (Exception e)
            {
                return Result<UserInfo>.Failure(e.Message);
            }

            var user = new UserRepository(_configuration).GetById(model.UserProfile.UserId);

            if (user?.UserId > 0 == false)
            {
                return Result<UserInfo>.Failure("Usuário sem pré-cadastro.");
            }

            var userRole = new UserRoleRepository(_configuration).GetById(user.UserRoleId);

            if (userRole?.UserRoleId > 0 == false)
            {
                return Result<UserInfo>.Failure("Falha ao buscar os dados do usuário.");
            }

            if (model?.UserProfile?.EstablishmentCategoryId > 0)
            {
                var establishmentCategory = new EstablishmentCategoryRepository(_configuration).GetById(model.UserProfile.EstablishmentCategoryId.GetValueOrDefault());

                if (establishmentCategory?.EstablishmentCategoryId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Categoria não encontrada.");
                }
            }

            try
            {
                var userProfile = new UserProfileRepository(_configuration).Insert(new UserProfileInsert
                {
                    UserId = model.UserProfile.UserId,
                    EstablishmentCategoryId = model.UserProfile.EstablishmentCategoryId,
                    AddressId = null,
                    Document = model.UserProfile.Document,
                    DateOfBirth = model.UserProfile.DateOfBirth,
                    Phone = model.UserProfile.Phone,
                    CommercialName = model.UserProfile.CommercialName,
                    CommercialPhone = model.UserProfile.CommercialPhone,
                    CommercialEmail = model.UserProfile.CommercialEmail,
                    Description = model.UserProfile.Description,
                    ProfileImage = model.UserProfile.ProfileImage
                });

                Address? address = null;

                if (model.Address != null)
                {
                    address = new AddressRepository(_configuration).Insert(new AddressInsert
                    {
                        Street = model.Address.Street,
                        Number = model.Address.Number,
                        Neighborhood = model.Address.Neighborhood,
                        AdditionalInfo = model.Address.AdditionalInfo,
                        City = model.Address.City,
                        State = model.Address.State,
                        Country = model.Address.Country,
                        PostalCode = model.Address.PostalCode
                    });

                    if (address?.AddressId > 0 == false)
                    {
                        return Result<UserInfo>.Failure("Falha ao cadastrar o endereço do usuário.");
                    }

                    if (!new UserProfileRepository(_configuration).UpdateAddress(userProfile.UserProfileId, address.AddressId))
                    {
                        return Result<UserInfo>.Failure("Falha ao atualizar o endereço do usuário.");
                    }

                    userProfile.AddressId = address.AddressId;
                }

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = user,
                    UserProfile = userProfile,
                    Address = address
                });
            }
            catch (Exception)
            {
                return Result<UserInfo>.Failure("Falha no registro do usuário.");
            }
        }
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }
        public static bool VerifyPassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }

        public Result<UserInfo> Login(Login model)
        {
            try
            {
                var user = new UserRepository(_configuration).GetByEmail(model.Email);

                if (user.UserId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Usuário sem pré-cadastro.");
                }

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    return Result<UserInfo>.Failure("Email ou senha incorretos.");
                }

                var userRole = new UserRoleRepository(_configuration).GetById(user.UserRoleId);

                if (userRole == null)
                {
                    return Result<UserInfo>.Failure("Falha ao recuperar os dados do usuário. (UserRole)");
                }

                var userProfile = new UserProfileRepository(_configuration).GetByUserId(user.UserId);

                if (userProfile.UserProfileId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Falha ao recuperar os dados do perfil do usuário. (UserProfile)");
                }

                var address = new AddressRepository(_configuration).GetById(userProfile.AddressId.GetValueOrDefault());

                return Result<UserInfo>.Success(new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                    UserProfile = userProfile,
                    Address = address
                });
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure($"Falha ao realizar o login do usuário: {ex.Message}");
            }
        }
    }
}
