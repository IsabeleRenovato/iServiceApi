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

        public Result<List<Appointment>> GetAllAppointments()
        {
            try
            {
                var appointments = _appointmentRepository.Get();
                return Result<List<Appointment>>.Success(appointments);
            }
            catch (Exception ex)
            {
                return Result<List<Appointment>>.Failure($"Falha ao obter os agendamentos: {ex.Message}");
            }
        }

        public Result<Appointment> GetAppointmentById(int appointmentId)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(appointmentId);

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

        public Result<Appointment> AddAppointment(AppointmentInsert appointmentModel)
        {
            try
            {
                var newAppointment = _appointmentRepository.Insert(appointmentModel);
                return Result<Appointment>.Success(newAppointment);
            }
            catch (Exception ex)
            {
                return Result<Appointment>.Failure($"Falha ao inserir o agendamento: {ex.Message}");
            }
        }

        public Result<Appointment> UpdateAppointment(AppointmentUpdate appointment)
        {
            try
            {
                var updatedAppointment = _appointmentRepository.Update(appointment);
                return Result<Appointment>.Success(updatedAppointment);
            }
            catch (Exception ex)
            {
                return Result<Appointment>.Failure($"Falha ao atualizar o agendamento: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int appointmentId, bool isActive)
        {
            try
            {
                _appointmentRepository.SetActiveStatus(appointmentId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do agendamento: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int appointmentId, bool isDeleted)
        {
            try
            {
                _appointmentRepository.SetDeletedStatus(appointmentId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do agendamento: {ex.Message}");
            }
        }
    }
}
