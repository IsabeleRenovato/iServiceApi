using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentService(IConfiguration configuration)
        {
            _appointmentRepository = new AppointmentRepository(configuration);
        }

        public async Task<Result<List<Appointment>>> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAsync();
                return Result<List<Appointment>>.Success(appointments);
            }
            catch (Exception ex)
            {
                return Result<List<Appointment>>.Failure($"Falha ao obter os agendamentos: {ex.Message}");
            }
        }

        public async Task<Result<Appointment>> GetAppointmentById(int appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

                if (appointment == null)
                {
                    return Result<Appointment>.Failure("Agendamento não encontrado.");
                }

                return Result<Appointment>.Success(appointment);
            }
            catch (Exception ex)
            {
                return Result<Appointment>.Failure($"Falha ao obter o agendamento: {ex.Message}");
            }
        }

        public async Task<Result<Appointment>> AddAppointment(Appointment appointmentModel)
        {
            try
            {
                var newAppointment = await _appointmentRepository.InsertAsync(appointmentModel);
                return Result<Appointment>.Success(newAppointment);
            }
            catch (Exception ex)
            {
                return Result<Appointment>.Failure($"Falha ao inserir o agendamento: {ex.Message}");
            }
        }

        public async Task<Result<Appointment>> UpdateAppointment(Appointment appointment)
        {
            try
            {
                var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);
                return Result<Appointment>.Success(updatedAppointment);
            }
            catch (Exception ex)
            {
                return Result<Appointment>.Failure($"Falha ao atualizar o agendamento: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SetActiveStatus(int appointmentId, bool isActive)
        {
            try
            {
                await _appointmentRepository.SetActiveStatusAsync(appointmentId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do agendamento: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SetDeletedStatus(int appointmentId, bool isDeleted)
        {
            try
            {
                await _appointmentRepository.SetDeletedStatusAsync(appointmentId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do agendamento: {ex.Message}");
            }
        }
    }
}
