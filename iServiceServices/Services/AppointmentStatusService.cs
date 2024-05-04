using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
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
                var appointmentStatuses = _appointmentStatusRepository.Get();
                return Result<List<AppointmentStatus>>.Success(appointmentStatuses);
            }
            catch (Exception ex)
            {
                return Result<List<AppointmentStatus>>.Failure($"Falha ao obter os status dos agendamentos: {ex.Message}");
            }
        }

        public Result<AppointmentStatus> GetAppointmentStatusById(int appointmentStatusId)
        {
            try
            {
                var appointmentStatus = _appointmentStatusRepository.GetById(appointmentStatusId);

                if (appointmentStatus == null)
                {
                    return Result<AppointmentStatus>.Failure("Status do agendamento não encontrado.");
                }

                return Result<AppointmentStatus>.Success(appointmentStatus);
            }
            catch (Exception ex)
            {
                return Result<AppointmentStatus>.Failure($"Falha ao obter o status do agendamento: {ex.Message}");
            }
        }

        public Result<AppointmentStatus> AddAppointmentStatus(AppointmentStatusInsert appointmentStatusModel)
        {
            try
            {
                var newAppointmentStatus = _appointmentStatusRepository.Insert(appointmentStatusModel);
                return Result<AppointmentStatus>.Success(newAppointmentStatus);
            }
            catch (Exception ex)
            {
                return Result<AppointmentStatus>.Failure($"Falha ao inserir o status do agendamento: {ex.Message}");
            }
        }

        public Result<AppointmentStatus> UpdateAppointmentStatus(AppointmentStatusUpdate appointmentStatus)
        {
            try
            {
                var updatedAppointmentStatus = _appointmentStatusRepository.Update(appointmentStatus);
                return Result<AppointmentStatus>.Success(updatedAppointmentStatus);
            }
            catch (Exception ex)
            {
                return Result<AppointmentStatus>.Failure($"Falha ao atualizar o status do agendamento: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int appointmentStatusId, bool isActive)
        {
            try
            {
                _appointmentStatusRepository.SetActiveStatus(appointmentStatusId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do status do agendamento: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int appointmentStatusId, bool isDeleted)
        {
            try
            {
                _appointmentStatusRepository.SetDeletedStatus(appointmentStatusId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do status do agendamento: {ex.Message}");
            }
        }
    }
}
