using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly UserRoleRepository _userRoleRepository;
        private readonly ClientProfileRepository _clientProfileRepository;
        private readonly EstablishmentProfileRepository _establishmentProfileRepository;
        private readonly AddressRepository _addressRepository;

        public UserService(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
            _userRoleRepository = new UserRoleRepository(configuration);
            _clientProfileRepository = new ClientProfileRepository(configuration);
            _establishmentProfileRepository = new EstablishmentProfileRepository(configuration);
            _addressRepository = new AddressRepository(configuration);
        }

        public Result<List<User>> GetAllUsers()
        {
            try
            {
                var users = _userRepository.Get();
                return Result<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<List<User>>.Failure($"Falha ao obter os usuários: {ex.Message}");
            }
        }

        public Result<User> GetUserById(int userId)
        {
            try
            {
                var user = _userRepository.GetById(userId);

                if (user == null)
                {
                    return Result<User>.Failure("Usuário não encontrado.");
                }

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Falha ao obter o usuário: {ex.Message}");
            }
        }

        public Result<UserInfo> GetUserInfoById(int userId)
        {
            try
            {
                var user = _userRepository.GetById(userId) ?? throw new InvalidOperationException("Usuário não encontrado.");
                var userRole = _userRoleRepository.GetById(user.UserRoleId) ?? throw new InvalidOperationException("Cargo do usuário não encontrado.");
                var clientProfile = _clientProfileRepository.GetByUserId(user.UserId);
                var establishmentProfile = _establishmentProfileRepository.GetByUserId(user.UserId);

                int? addressId = clientProfile?.AddressId > 0 ? clientProfile.AddressId : establishmentProfile?.AddressId;
                var address = addressId.HasValue ? _addressRepository.GetById(addressId.Value) : throw new InvalidOperationException("Endereço não encontrado."); ;

                var userInfo = new UserInfo
                {
                    User = user,
                    UserRole = userRole,
                    ClientProfile = clientProfile,
                    EstablishmentProfile = establishmentProfile,
                    Address = address
                };

                return Result<UserInfo>.Success(userInfo);
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure($"Falha ao obter o usuário: {ex.Message}");
            }
        }

        public Result<User> GetUserByEmail(string email)
        {
            try
            {
                var user = _userRepository.GetByEmail(email);

                if (user == null)
                {
                    return Result<User>.Failure("Usuário não encontrado.");
                }

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Falha ao obter o usuário: {ex.Message}");
            }
        }

        public Result<User> AddUser(UserModel userModel)
        {
            try
            {
                var newUser = _userRepository.Insert(userModel);
                return Result<User>.Success(newUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Falha ao inserir o usuário: {ex.Message}");
            }
        }

        public Result<User> UpdateUser(User user)
        {
            try
            {
                var updatedUser = _userRepository.Update(user);
                return Result<User>.Success(updatedUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Falha ao atualizar o usuário: {ex.Message}");
            }
        }

        public Result<UserInfo> UpdateUserProfile(ProfileUpdate request)
        {
            try
            {
                var clientProfile = request.ClientProfile;
                var establishmentProfile = request.EstablishmentProfile;

                var userId = clientProfile?.UserId ?? establishmentProfile?.UserId;

                if (userId > 0 == false)
                {
                    return Result<UserInfo>.Failure("Usuário não encontrado.");
                }

                var user = _userRepository.GetById(userId.GetValueOrDefault());

                if (user == null)
                {
                    return Result<UserInfo>.Failure("Usuário não encontrado.");
                }

                user.Name = request.Name;

                var updatedUser = _userRepository.Update(user);

                if (clientProfile != null)
                {
                    var client = _clientProfileRepository.GetById(request.ClientProfile.ClientProfileId);
                    _clientProfileRepository.Update(new ClientProfile
                    {
                        ClientProfileId = request.ClientProfile.ClientProfileId,
                        UserId = userId.GetValueOrDefault(),
                        CPF = request.ClientProfile.CPF,
                        DateOfBirth = request.ClientProfile.DateOfBirth,
                        Phone = request.ClientProfile.Phone,
                        AddressId = client.AddressId,
                        Photo = client.Photo,
                    });
                }
                else if (establishmentProfile != null)
                {
                    var establishment = _establishmentProfileRepository.GetById(request.EstablishmentProfile.EstablishmentProfileId);
                    _establishmentProfileRepository.Update(new EstablishmentProfile
                    {
                        EstablishmentProfileId = request.EstablishmentProfile.EstablishmentProfileId,
                        UserId = userId.GetValueOrDefault(),
                        CNPJ = request.EstablishmentProfile.CNPJ,
                        CommercialName = request.EstablishmentProfile?.CommercialName,
                        EstablishmentCategoryId = request.EstablishmentProfile.EstablishmentCategoryId,
                        Description = request.EstablishmentProfile.Description,
                        CommercialPhone = request.EstablishmentProfile.CommercialPhone,
                        CommercialEmail = request.EstablishmentProfile.CommercialEmail,
                        AddressId = establishment.AddressId,
                        Photo = establishment.Photo,
                    });
                }
                else
                {
                    return Result<UserInfo>.Failure("Perfil não encontrado.");
                }

                var result = GetUserInfoById(userId.GetValueOrDefault());

                if (result.IsSuccess) 
                {
                    return Result<UserInfo>.Success(result.Value);
                }

                return Result<UserInfo>.Failure("Ocorreu um erro inesperado.");
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure($"Falha ao atualizar o usuário: {ex.Message}");
            }
        }

        public Result<bool> DeleteUser(int userId)
        {
            try
            {
                bool success = _userRepository.Delete(userId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o usuário ou usuário não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o usuário: {ex.Message}");
            }
        }
    }
}
