using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(IConfiguration configuration)
        {
            _addressService = new AddressService(configuration);
        }

        [HttpGet]
        public ActionResult<List<Address>> Get()
        {
            var result = _addressService.GetAllAddresses();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{addressId}")]
        public ActionResult<Address> GetById(int addressId)
        {
            var result = _addressService.GetAddressById(addressId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<Address> Post([FromBody] AddressInsert addressModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _addressService.AddAddress(addressModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { addressId = result.Value.AddressId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{addressId}")]
        public ActionResult<Address> Put(int addressId, [FromBody] AddressUpdate address)
        {
            if (addressId != address.AddressId)
            {
                return BadRequest();
            }

            var result = _addressService.UpdateAddress(address);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{addressId}/SetActive")]
        public ActionResult<bool> SetActive(int addressId, [FromBody] bool isActive)
        {
            var result = _addressService.SetActiveStatus(addressId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{addressId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int addressId, [FromBody] bool isDeleted)
        {
            var result = _addressService.SetDeletedStatus(addressId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}