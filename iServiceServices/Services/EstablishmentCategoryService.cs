using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
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

        public Result<List<EstablishmentCategory>> GetAllEstablishmentCategories()
        {
            try
            {
                var establishmentCategories = _establishmentCategoryRepository.Get();
                return Result<List<EstablishmentCategory>>.Success(establishmentCategories);
            }
            catch (Exception ex)
            {
                return Result<List<EstablishmentCategory>>.Failure($"Falha ao obter as categorias de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentCategory> GetEstablishmentCategoryById(int categoryId)
        {
            try
            {
                var establishmentCategory = _establishmentCategoryRepository.GetById(categoryId);

                if (establishmentCategory == null)
                {
                    return Result<EstablishmentCategory>.Failure("Categoria de estabelecimento não encontrada.");
                }

                return Result<EstablishmentCategory>.Success(establishmentCategory);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentCategory>.Failure($"Falha ao obter a categoria de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentCategory> AddEstablishmentCategory(EstablishmentCategoryInsert categoryModel)
        {
            try
            {
                var newCategory = _establishmentCategoryRepository.Insert(categoryModel);
                return Result<EstablishmentCategory>.Success(newCategory);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentCategory>.Failure($"Falha ao inserir a categoria de estabelecimento: {ex.Message}");
            }
        }

        public Result<EstablishmentCategory> UpdateEstablishmentCategory(EstablishmentCategoryUpdate category)
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

        public Result<bool> SetActiveStatus(int categoryId, bool isActive)
        {
            try
            {
                _establishmentCategoryRepository.SetActiveStatus(categoryId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo da categoria de estabelecimento: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int categoryId, bool isDeleted)
        {
            try
            {
                _establishmentCategoryRepository.SetDeletedStatus(categoryId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído da categoria de estabelecimento: {ex.Message}");
            }
        }
    }
}
