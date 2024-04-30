using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class ClientProfileService
    {
        private readonly ClientProfileRepository _clientProfileRepository;

        public ClientProfileService(IConfiguration configuration)
        {
            _clientProfileRepository = new ClientProfileRepository(configuration);
        }

        public Result<List<ClientProfile>> GetAllProfiles()
        {
            try
            {
                var profiles = _clientProfileRepository.Get();
                return Result<List<ClientProfile>>.Success(profiles);
            }
            catch (Exception ex)
            {
                return Result<List<ClientProfile>>.Failure($"Falha ao obter os perfis de cliente: {ex.Message}");
            }
        }

        public Result<ClientProfile> GetProfileById(int profileId)
        {
            try
            {
                var profile = _clientProfileRepository.GetById(profileId);

                if (profile == null)
                {
                    return Result<ClientProfile>.Failure("Perfil de cliente não encontrado.");
                }

                return Result<ClientProfile>.Success(profile);
            }
            catch (Exception ex)
            {
                return Result<ClientProfile>.Failure($"Falha ao obter o perfil de cliente: {ex.Message}");
            }
        }

        public Result<ClientProfile> GetProfileByUserId(int userId)
        {
            try
            {
                var profile = _clientProfileRepository.GetByUserId(userId);

                if (profile == null)
                {
                    return Result<ClientProfile>.Failure("Perfil de cliente não encontrado.");
                }

                return Result<ClientProfile>.Success(profile);
            }
            catch (Exception ex)
            {
                return Result<ClientProfile>.Failure($"Falha ao obter o perfil de cliente: {ex.Message}");
            }
        }

        public Result<ClientProfile> AddProfile(ClientProfileModel model)
        {
            try
            {
                var newProfile = _clientProfileRepository.Insert(model);
                return Result<ClientProfile>.Success(newProfile);
            }
            catch (Exception ex)
            {
                return Result<ClientProfile>.Failure($"Falha ao inserir o perfil de cliente: {ex.Message}");
            }
        }

        public Result<ClientProfile> UpdateProfile(ClientProfile profile)
        {
            try
            {
                var updatedProfile = _clientProfileRepository.Update(profile);
                return Result<ClientProfile>.Success(updatedProfile);
            }
            catch (Exception ex)
            {
                return Result<ClientProfile>.Failure($"Falha ao atualizar o perfil de cliente: {ex.Message}");
            }
        }

        public Result<bool> UpdatePhoto(ImageModel model)
        {
            try
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                model.Photo = fileBytes;

                var newPhoto = _clientProfileRepository.UpdatePhoto(model);
                return Result<bool>.Success(newPhoto);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao inserir o perfil de cliente: {ex.Message}");
            }
        }

        public Result<bool> DeleteProfile(int profileId)
        {
            try
            {
                bool success = _clientProfileRepository.Delete(profileId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o perfil de cliente ou perfil não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o perfil de cliente: {ex.Message}");
            }
        }
    }

}
