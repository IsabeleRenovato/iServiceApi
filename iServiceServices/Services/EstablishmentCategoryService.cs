using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class EstablishmentCategoryService
    {
        private readonly EstablishmentCategoryRepository _establishmentCategoryRepository;

        public EstablishmentCategoryService(IConfiguration configuration)
        {
            _establishmentCategoryRepository = new EstablishmentCategoryRepository(configuration);
        }

        public Result<List<EstablishmentCategory>> GetAllCategories()
        {
            try
            {
                var categories = _establishmentCategoryRepository.Get();
                return Result<List<EstablishmentCategory>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Result<List<EstablishmentCategory>>.Failure($"Falha ao obter as categorias de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentCategory> GetCategoryById(int categoryId)
        {
            try
            {
                var category = _establishmentCategoryRepository.GetById(categoryId);

                if (category == null)
                {
                    return Result<EstablishmentCategory>.Failure("Categoria de estabelecimento não encontrada.");
                }

                return Result<EstablishmentCategory>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentCategory>.Failure($"Falha ao obter a categoria de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentCategory> AddCategory(EstablishmentCategoryModel model)
        {
            try
            {
                var newCategory = _establishmentCategoryRepository.Insert(model);
                return Result<EstablishmentCategory>.Success(newCategory);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentCategory>.Failure($"Falha ao inserir a categoria de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentCategory> UpdateCategory(EstablishmentCategory category)
        {
            try
            {
                var updatedCategory = _establishmentCategoryRepository.Update(category);
                return Result<EstablishmentCategory>.Success(updatedCategory);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentCategory>.Failure($"Falha ao atualizar a categoria de estabelecimento: {ex.Message}");
            }
        }

        public Result<bool> DeleteCategory(int categoryId)
        {
            try
            {
                bool success = _establishmentCategoryRepository.Delete(categoryId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar a categoria de estabelecimento ou categoria não encontrada.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar a categoria de estabelecimento: {ex.Message}");
            }
        }
    }
}
