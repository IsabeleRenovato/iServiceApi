using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class SpecialScheduleService
    {
        private readonly SpecialScheduleRepository _specialScheduleRepository;

        public SpecialScheduleService(IConfiguration configuration)
        {
            _specialScheduleRepository = new SpecialScheduleRepository(configuration);
        }

        public Result<List<SpecialSchedule>> GetAllSpecialSchedules()
        {
            try
            {
                var specialSchedules = _specialScheduleRepository.Get();
                return Result<List<SpecialSchedule>>.Success(specialSchedules);
            }
            catch (Exception ex)
            {
                return Result<List<SpecialSchedule>>.Failure($"Falha ao obter os horários especiais: {ex.Message}");
            }
        }

        public Result<SpecialSchedule> GetSpecialScheduleById(int scheduleId)
        {
            try
            {
                var specialSchedule = _specialScheduleRepository.GetById(scheduleId);

                if (specialSchedule == null)
                {
                    return Result<SpecialSchedule>.Failure("Horário especial não encontrado.");
                }

                return Result<SpecialSchedule>.Success(specialSchedule);
            }
            catch (Exception ex)
            {
                return Result<SpecialSchedule>.Failure($"Falha ao obter o horário especial: {ex.Message}");
            }
        }

        public Result<SpecialSchedule> AddSpecialSchedule(SpecialScheduleInsert scheduleModel)
        {
            try
            {
                var newSpecialSchedule = _specialScheduleRepository.Insert(scheduleModel);
                return Result<SpecialSchedule>.Success(newSpecialSchedule);
            }
            catch (Exception ex)
            {
                return Result<SpecialSchedule>.Failure($"Falha ao inserir o horário especial: {ex.Message}");
            }
        }

        public Result<SpecialSchedule> UpdateSpecialSchedule(SpecialScheduleUpdate schedule)
        {
            try
            {
                var updatedSpecialSchedule = _specialScheduleRepository.Update(schedule);
                return Result<SpecialSchedule>.Success(updatedSpecialSchedule);
            }
            catch (Exception ex)
            {
                return Result<SpecialSchedule>.Failure($"Falha ao atualizar o horário especial: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int scheduleId, bool isActive)
        {
            try
            {
                _specialScheduleRepository.SetActiveStatus(scheduleId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do horário especial: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int scheduleId, bool isDeleted)
        {
            try
            {
                _specialScheduleRepository.SetDeletedStatus(scheduleId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do horário especial: {ex.Message}");
            }
        }
    }

}
