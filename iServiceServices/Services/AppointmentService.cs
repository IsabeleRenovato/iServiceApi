using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class AppointmentService
    {
        private readonly IConfiguration _configuration;
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentService(IConfiguration configuration)
        {
            _configuration = configuration;
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
        
        public Result<List<Appointment>> GetByEstablishmentAndDate(int establishmentProfileId, DateTime startDateTime)
        {
            try
            {
                var appointments = _appointmentRepository.GetByEstablishmentAndDate(establishmentProfileId, startDateTime);

                if (appointments == null)
                {
                    return Result<List<Appointment>>.Failure("Agendamento não encontrado.");
                }

                return Result<List<Appointment>>.Success(appointments);
            }
            catch (Exception ex)
            {
                return Result<List<Appointment>>.Failure($"Falha ao obter o agendamento: {ex.Message}");
            }
        }
        public Result<Appointment> AddAppointment(AppointmentModel model)
        {
            try
            {
                var clientProfile = new ClientProfileRepository(_configuration).GetById(model.ClientProfileID);

                if (clientProfile?.ClientProfileID > 0 == false)
                {
                    return Result<Appointment>.Failure($"Cliente não encontrado.");
                }

                var establishmentProfile = new EstablishmentProfileRepository(_configuration).GetById(model.EstablishmentProfileID);

                if (establishmentProfile?.EstablishmentProfileID > 0 == false)
                {
                    return Result<Appointment>.Failure($"Estabelecimento não encontrado.");
                }

                var newAppointment = _appointmentRepository.Insert(model);
                return Result<Appointment>.Success(newAppointment);
            }
            catch (Exception ex)
            {
                return Result<Appointment>.Failure($"Falha ao criar o agendamento: {ex.Message}");
            }
        }

        public Result<Appointment> UpdateAppointment(Appointment appointment)
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

        public Result<bool> DeleteAppointment(int appointmentId)
        {
            try
            {
                bool success = _appointmentRepository.Delete(appointmentId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o agendamento ou agendamento não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o agendamento: {ex.Message}");
            }
        }
    }

}
