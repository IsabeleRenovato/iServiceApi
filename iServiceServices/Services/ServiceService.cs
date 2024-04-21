using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class ServiceService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceRepository _serviceRepository;

        public ServiceService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceRepository = new ServiceRepository(configuration);
        }

        public Result<List<Service>> GetAllServices()
        {
            try
            {
                var services = _serviceRepository.Get();
                return Result<List<Service>>.Success(services);
            }
            catch (Exception ex)
            {
                return Result<List<Service>>.Failure($"Falha ao obter os serviços: {ex.Message}");
            }
        }

        public Result<List<string>> GetAvailableTimes(int serviceId, DateTime date)
        {
            try
            {
                var schedules = new List<string>();
                var service = new ServiceRepository(_configuration).GetById(serviceId);
                var schedule = new ScheduleRepository(_configuration).GetByEstablishmentProfileID(service.EstablishmentProfileId);
                var appointments = new AppointmentRepository(_configuration).GetByEstablishmentAndDate(service.EstablishmentProfileId, date);
                var specialDays = new SpecialDayRepository(_configuration).GetByEstablishmentAndDate(service.EstablishmentProfileId, date);

                var appointmentFinder = new AppointmentFinderService();
                var availableSlots = appointmentFinder.FindAvailableSlots(schedule, specialDays, service, date, appointments);
                if (availableSlots.Any())
                {
                    foreach (var slot in availableSlots)
                    {
                        schedules.Add(slot.ToString());
                    }
                }
                else
                {
                    return Result<List<string>>.Failure($"Nenhum horário disponível.");
                }

                return Result<List<string>>.Success(schedules);
            }
            catch (Exception ex)
            {
                return Result<List<string>>.Failure($"Falha ao obter os serviços: {ex.Message}");
            }
        }

        public Result<Service> GetServiceById(int serviceId)
        {
            try
            {
                var service = _serviceRepository.GetById(serviceId);

                if (service == null)
                {
                    return Result<Service>.Failure("Serviço não encontrado.");
                }

                return Result<Service>.Success(service);
            }
            catch (Exception ex)
            {
                return Result<Service>.Failure($"Falha ao obter o serviço: {ex.Message}");
            }
        }

        public Result<Service> GetByServiceCategoryId(int serviceCategoryId)
        {
            try
            {
                var service = _serviceRepository.GetByServiceCategoryId(serviceCategoryId);

                if (service == null)
                {
                    return Result<Service>.Failure("Serviço não encontrado.");
                }

                return Result<Service>.Success(service);
            }
            catch (Exception ex)
            {
                return Result<Service>.Failure($"Falha ao obter o serviço: {ex.Message}");
            }
        }

        public Result<Service> AddService(ServiceModel model)
        {
            try
            {
                var establishmentProfile = new EstablishmentProfileRepository(_configuration).GetById(model.EstablishmentProfileId);

                if (establishmentProfile?.EstablishmentProfileId > 0 == false)
                {
                    return Result<Service>.Failure($"Estabelecimento não encontrado.");
                }

                var newService = _serviceRepository.Insert(model);
                return Result<Service>.Success(newService);
            }
            catch (Exception ex)
            {
                return Result<Service>.Failure($"Falha ao inserir o serviço: {ex.Message}");
            }
        }

        public Result<Service> UpdateService(Service service)
        {
            try
            {
                var updatedService = _serviceRepository.Update(service);
                return Result<Service>.Success(updatedService);
            }
            catch (Exception ex)
            {
                return Result<Service>.Failure($"Falha ao atualizar o serviço: {ex.Message}");
            }
        }

        public Result<bool> DeleteService(int serviceId)
        {
            try
            {
                bool success = _serviceRepository.Delete(serviceId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o serviço ou serviço não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o serviço: {ex.Message}");
            }
        }
    }
}
