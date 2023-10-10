using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
           try {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                return Created("",_repository.Add(bookingInsert, email!));
           } catch(Exception e) {
            return BadRequest(new ErrorMessageDto{ Message = e.Message });
           }
            
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "client")]
        public IActionResult GetBooking(int Bookingid){
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
           try {
                return Ok(_repository.GetBooking(Bookingid, email));
           } catch(Exception e) {
            return Unauthorized();
           }
        }
    }
}