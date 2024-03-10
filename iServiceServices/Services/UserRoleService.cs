using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class UserRoleService
    {
        private readonly UserRoleRepository _userRoleRepository;

        public UserRoleService(IConfiguration configuration)
        {
            _userRoleRepository = new UserRoleRepository(configuration);
        }

        public Result<List<UserRole>> GetAllUserRoles()
        {
            try
            {
                var roles = _userRoleRepository.Get();
                return Result<List<UserRole>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Result<List<UserRole>>.Failure($"Erro ao buscar os perfis de usuário: {ex.Message}");
            }
        }

        public Result<UserRole> GetUserRoleById(int id)
        {
            try
            {
                var role = _userRoleRepository.GetById(id);
                if (role == null) return Result<UserRole>.Failure("Perfil de usuário não encontrado.");
                return Result<UserRole>.Success(role);
            }
            catch (Exception ex)
            {
                return Result<UserRole>.Failure($"Erro ao buscar o perfil de usuário: {ex.Message}");
            }
        }

        public Result<UserRole> AddUserRole(UserRoleModel userRoleModel)
        {
            try
            {
                var newUserRole = _userRoleRepository.Insert(userRoleModel);
                return Result<UserRole>.Success(newUserRole);
            }
            catch (Exception ex)
            {
                return Result<UserRole>.Failure($"Erro ao adicionar o perfil de usuário: {ex.Message}");
            }
        }

        public Result<UserRole> UpdateUserRole(UserRole userRole)
        {
            try
            {
                var updatedUserRole = _userRoleRepository.Update(userRole);
                return Result<UserRole>.Success(updatedUserRole);
            }
            catch (Exception ex)
            {
                return Result<UserRole>.Failure($"Erro ao atualizar o perfil de usuário: {ex.Message}");
            }
        }

        public Result<bool> DeleteUserRole(int id)
        {
            try
            {
                var success = _userRoleRepository.Delete(id);
                if (!success) return Result<bool>.Failure("Perfil de usuário não encontrado ou erro ao deletar.");
                return Result<bool>.Success(success);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Erro ao deletar o perfil de usuário: {ex.Message}");
            }
        }
    }

}
