using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class ScheduleService
    {
        private readonly IConfiguration _configuration;
        private readonly ScheduleRepository _scheduleRepository;

        public ScheduleService(IConfiguration configuration)
        {
            _configuration = configuration;
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
                return Result<List<Schedule>>.Failure($"Falha ao obter as agendas: {ex.Message}");
            }
        }

        public Result<Schedule> GetScheduleById(int scheduleId)
        {
            try
            {
                var schedule = _scheduleRepository.GetById(scheduleId);

                if (schedule == null)
                {
                    return Result<Schedule>.Failure("Agenda não encontrada.");
                }

                return Result<Schedule>.Success(schedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao obter a agenda: {ex.Message}");
            }
        }

        public Result<Schedule> GetByEstablishmentProfileId(int establishmentProfileId)
        {
            try
            {
                var schedule = _scheduleRepository.GetByEstablishmentProfileId(establishmentProfileId);

                if (schedule == null)
                {
                    return Result<Schedule>.Failure("Agenda não encontrada.");
                }

                return Result<Schedule>.Success(schedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao obter a agenda: {ex.Message}");
            }
        }

        public Result<Schedule> AddSchedule(ScheduleModel model)
        {
            try
            {
                var establishmentProfile = new EstablishmentProfileRepository(_configuration).GetById(model.EstablishmentProfileId);

                if (establishmentProfile?.EstablishmentProfileId > 0 == false)
                {
                    return Result<Schedule>.Failure($"Estabelecimento não encontrado.");
                }

                var newSchedule = _scheduleRepository.Insert(model);
                return Result<Schedule>.Success(newSchedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao inserir a agenda: {ex.Message}");
            }
        }

        public Result<Schedule> UpdateSchedule(Schedule schedule)
        {
            try
            {
                var updatedSchedule = _scheduleRepository.Update(schedule);
                return Result<Schedule>.Success(updatedSchedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule>.Failure($"Falha ao atualizar a agenda: {ex.Message}");
            }
        }

        public Result<bool> DeleteSchedule(int scheduleId)
        {
            try
            {
                bool success = _scheduleRepository.Delete(scheduleId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar a agenda ou agenda não encontrada.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar a agenda: {ex.Message}");
            }
        }
    }
}
