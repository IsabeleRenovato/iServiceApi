﻿using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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

        public Result<Address> AddAddress(AddressModel addressModel)
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

        public Result<Address> UpdateAddress(Address address)
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

        public Result<bool> DeleteAddress(int addressId)
        {
            try
            {
                bool success = _addressRepository.Delete(addressId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o endereço ou endereço não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o endereço: {ex.Message}");
            }
        }
    }
}
