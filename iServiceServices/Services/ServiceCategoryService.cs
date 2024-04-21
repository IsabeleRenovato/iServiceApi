using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class ServiceCategoryService
    {
        private readonly ServiceCategoryRepository _serviceCategoryRepository;

        public ServiceCategoryService(IConfiguration configuration)
        {
            _serviceCategoryRepository = new ServiceCategoryRepository(configuration);
        }

        public Result<List<ServiceCategory>> GetAllServiceCategories()
        {
            try
            {
                var categories = _serviceCategoryRepository.Get();
                return Result<List<ServiceCategory>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Result<List<ServiceCategory>>.Failure($"Falha ao obter as categorias de serviços: {ex.Message}");
            }
        }

        public Result<ServiceCategory> GetServiceCategoryById(int serviceCategoryId)
        {
            try
            {
                var category = _serviceCategoryRepository.GetById(serviceCategoryId);

                if (category == null)
                {
                    return Result<ServiceCategory>.Failure("Categoria de serviço não encontrada.");
                }

                return Result<ServiceCategory>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<ServiceCategory>.Failure($"Falha ao obter a categoria de serviço: {ex.Message}");
            }
        }

        public Result<ServiceCategory> AddServiceCategory(ServiceCategoryModel model)
        {
            try
            {
                var newCategory = _serviceCategoryRepository.Insert(model);
                return Result<ServiceCategory>.Success(newCategory);
            }
            catch (Exception ex)
            {
                return Result<ServiceCategory>.Failure($"Falha ao criar a categoria de serviço: {ex.Message}");
            }
        }

        public Result<ServiceCategory> UpdateServiceCategory(ServiceCategory serviceCategory)
        {
            try
            {
                var updatedCategory = _serviceCategoryRepository.Update(serviceCategory);
                return Result<ServiceCategory>.Success(updatedCategory);
            }
            catch (Exception ex)
            {
                return Result<ServiceCategory>.Failure($"Falha ao atualizar a categoria de serviço: {ex.Message}");
            }
        }

        public Result<bool> DeleteServiceCategory(int serviceCategoryId)
        {
            try
            {
                bool success = _serviceCategoryRepository.Delete(serviceCategoryId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar a categoria de serviço ou categoria não encontrada.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar a categoria de serviço: {ex.Message}");
            }
        }
    }
}
