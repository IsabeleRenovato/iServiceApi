using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace iServiceServices.Services
{
    public class UserProfileService
    {
        private readonly UserProfileRepository _userProfileRepository;

        public UserProfileService(IConfiguration configuration)
        {
            _userProfileRepository = new UserProfileRepository(configuration);
        }

        public Result<List<UserProfile>> GetAllUserProfiles()
        {
            try
            {
                var userProfiles = _userProfileRepository.Get();
                return Result<List<UserProfile>>.Success(userProfiles);
            }
            catch (Exception ex)
            {
                return Result<List<UserProfile>>.Failure($"Falha ao obter os perfis de usuário: {ex.Message}");
            }
        }

        public Result<UserProfile> GetUserProfileById(int userProfileId)
        {
            try
            {
                var userProfile = _userProfileRepository.GetById(userProfileId);

                if (userProfile == null)
                {
                    return Result<UserProfile>.Failure("Perfil de usuário não encontrado.");
                }

                return Result<UserProfile>.Success(userProfile);
            }
            catch (Exception ex)
            {
                return Result<UserProfile>.Failure($"Falha ao obter o perfil de usuário: {ex.Message}");
            }
        }

        public Result<UserProfile> AddUserProfile(UserProfileInsert profileModel)
        {
            try
            {
                var newUserProfile = _userProfileRepository.Insert(profileModel);
                return Result<UserProfile>.Success(newUserProfile);
            }
            catch (Exception ex)
            {
                return Result<UserProfile>.Failure($"Falha ao inserir o perfil de usuário: {ex.Message}");
            }
        }

        public Result<UserProfile> UpdateUserProfile(UserProfileUpdate profile)
        {
            try
            {
                var updatedUserProfile = _userProfileRepository.Update(profile);
                return Result<UserProfile>.Success(updatedUserProfile);
            }
            catch (Exception ex)
            {
                return Result<UserProfile>.Failure($"Falha ao atualizar o perfil de usuário: {ex.Message}");
            }
        }

        public Result<string> UpdateProfileImage(ImageModel model)
        {
            try
            {
                var userProfile = _userProfileRepository.GetById(model.Id);

                if (userProfile?.UserProfileId > 0 == false)
                {
                    return Result<string>.Failure("Usuário sem pré-cadastro.");
                }

                if (model.File == null)
                {
                    return Result<string>.Failure("Falha ao ler o arquivo.");
                }

                var path = new FtpServices().UploadFile(model.File, "profile", $"profile{model.Id}.png");

                if (string.IsNullOrEmpty(path))
                {
                    return Result<string>.Failure($"Falha ao subir o arquivo de imagem.");
                }

                if (_userProfileRepository.UpdateProfileImage(model.Id, path))
                {
                    return Result<string>.Success(path);
                }

                return Result<string>.Failure("Falha ao atualizar a foto de perfil do usuário.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Falha ao inserir o perfil de cliente: {ex.Message}");
            }
        }

        public Result<bool> DeleteUserProfile(int userProfileId)
        {
            try
            {
                bool success = _userProfileRepository.Delete(userProfileId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao excluir o perfil de usuário ou perfil não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao excluir o perfil de usuário: {ex.Message}");
            }
        }
    }

}
