using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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

        [HttpGet("GetById/{addressId}")]
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
        public ActionResult<Address> Post([FromBody] AddressModel addressModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _addressService.AddAddress(addressModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { addressId = result.Value.AddressID }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{addressId}")]
        public ActionResult<Address> Put(int addressId, [FromBody] Address address)
        {
            if (addressId != address.AddressID)
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

        [HttpDelete("{addressId}")]
        public IActionResult Delete(int addressId)
        {
            var result = _addressService.DeleteAddress(addressId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}