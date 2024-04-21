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
                var userRole = _userRoleRepository.GetById(user.UserRoleID) ?? throw new InvalidOperationException("Cargo do usuário não encontrado."); 
                var clientProfile = _clientProfileRepository.GetByUserId(user.UserID);
                var establishmentProfile = _establishmentProfileRepository.GetByUserId(user.UserID);

                int? addressId = clientProfile?.AddressID > 0 ? clientProfile.AddressID : establishmentProfile?.AddressID;
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
