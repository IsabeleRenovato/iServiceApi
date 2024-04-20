using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class AppointmentStatusService
    {
        private readonly AppointmentStatusRepository _appointmentStatusRepository;

        public AppointmentStatusService(IConfiguration configuration)
        {
            _appointmentStatusRepository = new AppointmentStatusRepository(configuration);
        }

        public Result<List<AppointmentStatus>> GetAllAppointmentStatuses()
        {
            try
            {
                var statuses = _appointmentStatusRepository.Get();
                return Result<List<AppointmentStatus>>.Success(statuses);
            }
            catch (Exception ex)
            {
                return Result<List<AppointmentStatus>>.Failure($"Falha ao obter os status de agendamento: {ex.Message}");
            }
        }

        public Result<AppointmentStatus> GetAppointmentStatusById(int appointmentStatusId)
        {
            try
            {
                var status = _appointmentStatusRepository.GetById(appointmentStatusId);

                if (status == null)
                {
                    return Result<AppointmentStatus>.Failure("Status de agendamento não encontrado.");
                }

                return Result<AppointmentStatus>.Success(status);
            }
            catch (Exception ex)
            {
                return Result<AppointmentStatus>.Failure($"Falha ao obter o status de agendamento: {ex.Message}");
            }
        }

        public Result<AppointmentStatus> AddAppointmentStatus(AppointmentStatusModel model)
        {
            try
            {
                var newStatus = _appointmentStatusRepository.Insert(model);
                return Result<AppointmentStatus>.Success(newStatus);
            }
            catch (Exception ex)
            {
                return Result<AppointmentStatus>.Failure($"Falha ao criar o status de agendamento: {ex.Message}");
            }
        }

        public Result<AppointmentStatus> UpdateAppointmentStatus(AppointmentStatus appointmentStatus)
        {
            try
            {
                var updatedStatus = _appointmentStatusRepository.Update(appointmentStatus);
                return Result<AppointmentStatus>.Success(updatedStatus);
            }
            catch (Exception ex)
            {
                return Result<AppointmentStatus>.Failure($"Falha ao atualizar o status de agendamento: {ex.Message}");
            }
        }

        public Result<bool> DeleteAppointmentStatus(int appointmentStatusId)
        {
            try
            {
                bool success = _appointmentStatusRepository.Delete(appointmentStatusId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o status de agendamento ou status não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o status de agendamento: {ex.Message}");
            }
        }
    }
}
