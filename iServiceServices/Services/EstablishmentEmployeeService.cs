using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class EstablishmentEmployeeService
    {
        private readonly EstablishmentEmployeeRepository _establishmentEmployeeRepository;

        public EstablishmentEmployeeService(IConfiguration configuration)
        {
            _establishmentEmployeeRepository = new EstablishmentEmployeeRepository(configuration);
        }

        public async Task<Result<List<EstablishmentEmployee>>> GetAllEstablishmentEmployees()
        {
            try
            {
                var EstablishmentEmployees = await _establishmentEmployeeRepository.GetAsync();
                return Result<List<EstablishmentEmployee>>.Success(EstablishmentEmployees);
            }
            catch (Exception ex)
            {
                return Result<List<EstablishmentEmployee>>.Failure($"Falha ao obter os EstablishmentEmployees: {ex.Message}");
            }
        }

        public async Task<Result<EstablishmentEmployee>> GetEstablishmentEmployeeById(int EstablishmentEmployeeId)
        {
            try
            {
                var EstablishmentEmployee = await _establishmentEmployeeRepository.GetByIdAsync(EstablishmentEmployeeId);

                if (EstablishmentEmployee == null)
                {
                    return Result<EstablishmentEmployee>.Failure("EstablishmentEmployee não encontrado.");
                }

                return Result<EstablishmentEmployee>.Success(EstablishmentEmployee);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentEmployee>.Failure($"Falha ao obter o EstablishmentEmployee: {ex.Message}");
            }
        }

        public async Task<Result<EstablishmentEmployee>> AddEstablishmentEmployee(EstablishmentEmployee EstablishmentEmployeeModel)
        {
            try
            {
                var newEstablishmentEmployee = await _establishmentEmployeeRepository.InsertAsync(EstablishmentEmployeeModel);

                if (newEstablishmentEmployee != null)
                {
                    if (EstablishmentEmployeeModel.File != null)
                    {
                        var path = await new FtpServices().UploadFileAsync(EstablishmentEmployeeModel.File, "employee", $"employee_{newEstablishmentEmployee.EstablishmentEmployeeId}.png");

                        newEstablishmentEmployee = await _establishmentEmployeeRepository.UpdateImageAsync(newEstablishmentEmployee.EstablishmentEmployeeId, path);
                    }
                }

                return Result<EstablishmentEmployee>.Success(newEstablishmentEmployee);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentEmployee>.Failure($"Falha ao inserir o EstablishmentEmployee: {ex.Message}");
            }
        }

        public async Task<Result<EstablishmentEmployee>> UpdateEstablishmentEmployee(EstablishmentEmployee EstablishmentEmployee)
        {
            try
            {
                var updatedEstablishmentEmployee = await _establishmentEmployeeRepository.UpdateAsync(EstablishmentEmployee);

                if (updatedEstablishmentEmployee != null)
                {
                    if (EstablishmentEmployee.File != null)
                    {
                        var path = await new FtpServices().UploadFileAsync(EstablishmentEmployee.File, "employee", $"employee_{EstablishmentEmployee.EstablishmentEmployeeId}.png");

                        updatedEstablishmentEmployee = await _establishmentEmployeeRepository.UpdateImageAsync(EstablishmentEmployee.EstablishmentEmployeeId, path);
                    }
                }

                return Result<EstablishmentEmployee>.Success(updatedEstablishmentEmployee);
            }
            catch (Exception ex)
            {
                return Result<EstablishmentEmployee>.Failure($"Falha ao atualizar o EstablishmentEmployee: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SetActiveStatus(int EstablishmentEmployeeId, bool isActive)
        {
            try
            {
                await _establishmentEmployeeRepository.SetActiveStatusAsync(EstablishmentEmployeeId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do EstablishmentEmployee: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SetDeletedStatus(int EstablishmentEmployeeId, bool isDeleted)
        {
            try
            {
                await _establishmentEmployeeRepository.SetDeletedStatusAsync(EstablishmentEmployeeId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do EstablishmentEmployee: {ex.Message}");
            }
        }
    }
}
