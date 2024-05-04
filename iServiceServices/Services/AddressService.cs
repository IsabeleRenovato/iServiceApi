using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class AddressService
    {
        private readonly AddressRepository _addressRepository;

        public AddressService(IConfiguration configuration)
        {
            _addressRepository = new AddressRepository(configuration);
        }

        public Result<List<Address>> GetAllAddresses()
        {
            try
            {
                var addresses = _addressRepository.Get();
                return Result<List<Address>>.Success(addresses);
            }
            catch (Exception ex)
            {
                return Result<List<Address>>.Failure($"Falha ao obter os endereços: {ex.Message}");
            }
        }

        public Result<Address> GetAddressById(int addressId)
        {
            try
            {
                var address = _addressRepository.GetById(addressId);

                if (address == null)
                {
                    return Result<Address>.Failure("Endereço não encontrado.");
                }

                return Result<Address>.Success(address);
            }
            catch (Exception ex)
            {
                return Result<Address>.Failure($"Falha ao obter o endereço: {ex.Message}");
            }
        }

        public Result<Address> AddAddress(AddressInsert addressModel)
        {
            try
            {
                var newAddress = _addressRepository.Insert(addressModel);
                return Result<Address>.Success(newAddress);
            }
            catch (Exception ex)
            {
                return Result<Address>.Failure($"Falha ao inserir o endereço: {ex.Message}");
            }
        }

        public Result<Address> UpdateAddress(AddressUpdate address)
        {
            try
            {
                var updatedAddress = _addressRepository.Update(address);
                return Result<Address>.Success(updatedAddress);
            }
            catch (Exception ex)
            {
                return Result<Address>.Failure($"Falha ao atualizar o endereço: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int addressId, bool isActive)
        {
            try
            {
                _addressRepository.SetActiveStatus(addressId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do endereço: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int addressId, bool isDeleted)
        {
            try
            {
                _addressRepository.SetDeletedStatus(addressId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do endereço: {ex.Message}");
            }
        }
    }
}
