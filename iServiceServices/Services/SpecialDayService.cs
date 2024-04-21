using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class SpecialDayService
    {
        private readonly IConfiguration _configuration;
        private readonly SpecialDayRepository _specialDayRepository;

        public SpecialDayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _specialDayRepository = new SpecialDayRepository(configuration);
        }

        public Result<List<SpecialDay>> GetAllSpecialDays()
        {
            try
            {
                var specialDays = _specialDayRepository.Get();
                return Result<List<SpecialDay>>.Success(specialDays);
            }
            catch (Exception ex)
            {
                return Result<List<SpecialDay>>.Failure($"Falha ao obter os dias especiais: {ex.Message}");
            }
        }

        public Result<SpecialDay> GetSpecialDayById(int specialDayId)
        {
            try
            {
                var specialDay = _specialDayRepository.GetById(specialDayId);

                if (specialDay == null)
                {
                    return Result<SpecialDay>.Failure("Dia especial não encontrado.");
                }

                return Result<SpecialDay>.Success(specialDay);
            }
            catch (Exception ex)
            {
                return Result<SpecialDay>.Failure($"Falha ao obter o dia especial: {ex.Message}");
            }
        }

        public Result<List<SpecialDay>> GetByEstablishmentAndDate(int establishmentProfileId, DateTime date)
        {
            try
            {
                var appointments = _specialDayRepository.GetByEstablishmentAndDate(establishmentProfileId, date);

                if (appointments == null)
                {
                    return Result<List<SpecialDay>>.Failure("Dia especial não encontrado.");
                }

                return Result<List<SpecialDay>>.Success(appointments);
            }
            catch (Exception ex)
            {
                return Result<List<SpecialDay>>.Failure($"Falha ao obter o dia especial: {ex.Message}");
            }
        }

        public Result<SpecialDay> AddSpecialDay(SpecialDayModel model)
        {
            try
            {
                var establishmentProfile = new EstablishmentProfileRepository(_configuration).GetById(model.EstablishmentProfileId);

                if (establishmentProfile?.EstablishmentProfileId > 0 == false)
                {
                    return Result<SpecialDay>.Failure($"Estabelecimento não encontrado.");
                }

                var newSpecialDay = _specialDayRepository.Insert(model);
                return Result<SpecialDay>.Success(newSpecialDay);
            }
            catch (Exception ex)
            {
                return Result<SpecialDay>.Failure($"Falha ao criar o dia especial: {ex.Message}");
            }
        }

        public Result<SpecialDay> UpdateSpecialDay(SpecialDay specialDay)
        {
            try
            {
                var updatedSpecialDay = _specialDayRepository.Update(specialDay);
                return Result<SpecialDay>.Success(updatedSpecialDay);
            }
            catch (Exception ex)
            {
                return Result<SpecialDay>.Failure($"Falha ao atualizar o dia especial: {ex.Message}");
            }
        }

        public Result<bool> DeleteSpecialDay(int specialDayId)
        {
            try
            {
                bool success = _specialDayRepository.Delete(specialDayId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o dia especial ou dia especial não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o dia especial: {ex.Message}");
            }
        }
    }

}
