using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class ServiceService
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly ServiceCategoryRepository _serviceCategoryRepository;
        public ServiceService(IConfiguration configuration)
        {
            _serviceRepository = new ServiceRepository(configuration);
            _serviceCategoryRepository = new ServiceCategoryRepository(configuration);
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

        public Result<Service> AddService(ServiceInsert serviceModel)
        {
            try
            {
                var serviceCategory = _serviceCategoryRepository.GetByFilter(serviceModel.UserProfileId, serviceModel.ServiceCategoryId);

                if (serviceCategory?.ServiceCategoryId > 0 == false)
                {
                    return Result<Service>.Failure($"Falha ao buscar a categoria.");
                }

                var newService = _serviceRepository.Insert(serviceModel);

                if (newService?.ServiceId > 0 == false)
                {
                    return Result<Service>.Failure("Falha ao inserir o serviço.");
                }

                if (serviceModel.File != null)
                {
                    serviceModel.ServiceImage = UpdateServiceImage(new ImageModel
                    {
                        Id = newService.ServiceId,
                        File = serviceModel.File,
                    }).Value;
                }

                return Result<Service>.Failure("Falha ao inserir o serviço.");
            }
            catch (Exception ex)
            {
                return Result<Service>.Failure($"Falha ao inserir o serviço: {ex.Message}");
            }
        }

        public Result<Service> UpdateService(ServiceUpdate service)
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

        public Result<string> UpdateServiceImage(ImageModel model)
        {
            try
            {
                var userProfile = _serviceRepository.GetById(model.Id);

                if (userProfile?.ServiceId > 0 == false)
                {
                    return Result<string>.Failure("Serviço não encontrado.");
                }

                if (model.File == null)
                {
                    return Result<string>.Failure("Falha ao ler o arquivo.");
                }

                var path = new FtpServices().UploadFile(model.File, "profile", $"profile{model.Id}.png");

                if (string.IsNullOrEmpty(path))
                {
                    return Result<string>.Failure($"Falha ao subir o arquivo de imagem.");
                }

                if (_serviceRepository.UpdateServiceImage(model.Id, path))
                {
                    return Result<string>.Success(path);
                }

                return Result<string>.Failure("Falha ao atualizar a foto de perfil do usuário.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Falha ao inserir o perfil de cliente: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int serviceId, bool isActive)
        {
            try
            {
                _serviceRepository.SetActiveStatus(serviceId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do serviço: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int serviceId, bool isDeleted)
        {
            try
            {
                _serviceRepository.SetDeletedStatus(serviceId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do serviço: {ex.Message}");
            }
        }
    }
}
