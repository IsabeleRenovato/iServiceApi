using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository _scheduleRepository;

        public ScheduleService(IConfiguration configuration)
        {
            _scheduleRepository = new ScheduleRepository(configuration);
        }

        public Result<List<Schedule>> GetAllSchedules()
        {
            try
            {
                var schedules = _scheduleRepository.Get();
                return Result<List<Schedule>>.Success(schedules);
            }
            catch (Exception ex)
            {
                return Result<List<Schedule>>.Failure($"Falha ao obter os horários: {ex.Message}");
            }
        }

        public Result<Schedule> GetScheduleById(int scheduleId)
        {
            try
            {
                var schedule = _scheduleRepository.GetById(scheduleId);

                if (schedule == null)
                {
                    return Result<Schedule>.Failure("Horário não encontrado.");
                }

                return Result<Schedule>.Success(schedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao obter o horário: {ex.Message}");
            }
        }

        public Result<Schedule> AddSchedule(ScheduleInsert scheduleModel)
        {
            try
            {
                var newSchedule = _scheduleRepository.Insert(scheduleModel);
                return Result<Schedule>.Success(newSchedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao inserir o horário: {ex.Message}");
            }
        }

        public Result<Schedule> UpdateSchedule(ScheduleUpdate schedule)
        {
            try
            {
                var updatedSchedule = _scheduleRepository.Update(schedule);
                return Result<Schedule>.Success(updatedSchedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao atualizar o horário: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int scheduleId, bool isActive)
        {
            try
            {
                _scheduleRepository.SetActiveStatus(scheduleId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do horário: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int scheduleId, bool isDeleted)
        {
            try
            {
                _scheduleRepository.SetDeletedStatus(scheduleId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do horário: {ex.Message}");
            }
        }
    }
}
