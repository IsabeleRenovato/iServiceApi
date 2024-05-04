using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
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
                var serviceCategories = _serviceCategoryRepository.Get();
                return Result<List<ServiceCategory>>.Success(serviceCategories);
            }
            catch (Exception ex)
            {
                return Result<List<ServiceCategory>>.Failure($"Falha ao obter as categorias de serviço: {ex.Message}");
            }
        }

        public Result<ServiceCategory> GetServiceCategoryById(int categoryId)
        {
            try
            {
                var serviceCategory = _serviceCategoryRepository.GetById(categoryId);

                if (serviceCategory == null)
                {
                    return Result<ServiceCategory>.Failure("Categoria de serviço não encontrada.");
                }

                return Result<ServiceCategory>.Success(serviceCategory);
            }
            catch (Exception ex)
            {
                return Result<ServiceCategory>.Failure($"Falha ao obter a categoria de serviço: {ex.Message}");
            }
        }

        public Result<ServiceCategory> AddServiceCategory(ServiceCategoryInsert categoryModel)
        {
            try
            {
                var newCategory = _serviceCategoryRepository.Insert(categoryModel);
                return Result<ServiceCategory>.Success(newCategory);
            }
            catch (Exception ex)
            {
                return Result<ServiceCategory>.Failure($"Falha ao inserir a categoria de serviço: {ex.Message}");
            }
        }

        public Result<ServiceCategory> UpdateServiceCategory(ServiceCategoryUpdate category)
        {
            try
            {
                var updatedCategory = _serviceCategoryRepository.Update(category);
                return Result<ServiceCategory>.Success(updatedCategory);
            }
            catch (Exception ex)
            {
                return Result<ServiceCategory>.Failure($"Falha ao atualizar a categoria de serviço: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int categoryId, bool isActive)
        {
            try
            {
                _serviceCategoryRepository.SetActiveStatus(categoryId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo da categoria de serviço: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int categoryId, bool isDeleted)
        {
            try
            {
                _serviceCategoryRepository.SetDeletedStatus(categoryId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído da categoria de serviço: {ex.Message}");
            }
        }
    }
}
