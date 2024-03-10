using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class EstablishmentProfileService
    {
        private readonly EstablishmentProfileRepository _establishmentProfileRepository;

        public EstablishmentProfileService(IConfiguration configuration)
        {
            _establishmentProfileRepository = new EstablishmentProfileRepository(configuration);
        }

        public Result<List<EstablishmentProfile>> GetAllProfiles()
        {
            try
            {
                var profiles = _establishmentProfileRepository.Get();
                return Result<List<EstablishmentProfile>>.Success(profiles);
            }
            catch (Exception ex)
            {
                return Result<List<EstablishmentProfile>>.Failure($"Falha ao obter os perfis de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentProfile> GetProfileById(int profileId)
        {
            try
            {
                var profile = _establishmentProfileRepository.GetById(profileId);

                if (profile == null)
                {
                    return Result<EstablishmentProfile>.Failure("Perfil de estabelecimento não encontrado.");
                }

                return Result<EstablishmentProfile>.Success(profile);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentProfile>.Failure($"Falha ao obter o perfil de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentProfile> GetProfileByUserId(int userId)
        {
            try
            {
                var profile = _establishmentProfileRepository.GetByUserId(userId);

                if (profile == null)
                {
                    return Result<EstablishmentProfile>.Failure("Perfil de estabelecimento não encontrado.");
                }

                return Result<EstablishmentProfile>.Success(profile);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentProfile>.Failure($"Falha ao obter o perfil de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentProfile> AddProfile(EstablishmentProfileModel model)
        {
            try
            {
                var newProfile = _establishmentProfileRepository.Insert(model);
                return Result<EstablishmentProfile>.Success(newProfile);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentProfile>.Failure($"Falha ao inserir o perfil de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentProfile> UpdateProfile(EstablishmentProfile profile)
        {
            try
            {
                var updatedProfile = _establishmentProfileRepository.Update(profile);
                return Result<EstablishmentProfile>.Success(updatedProfile);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentProfile>.Failure($"Falha ao atualizar o perfil de estabelecimento: {ex.Message}");
            }
        }

        public Result<bool> DeleteProfile(int profileId)
        {
            try
            {
                bool success = _establishmentProfileRepository.Delete(profileId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o perfil de estabelecimento ou perfil não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o perfil de estabelecimento: {ex.Message}");
            }
        }
    }


}
