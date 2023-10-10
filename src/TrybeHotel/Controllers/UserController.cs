using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult GetUsers(){
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            if(user.Email == null || user.Name == null || user.Password == null)
                return BadRequest(new ErrorMessageDto{ Message = "Invalid data" });
           try {
             return Created("",_repository.Add(user));
           } catch(Exception e) {
            return Conflict(new ErrorMessageDto{ Message = e.Message });
           }
        }
    }
}