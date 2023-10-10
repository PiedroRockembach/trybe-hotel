using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Services;
using System.Linq;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {

            User user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
            if (user == null)
                throw new Exception("Incorrect e-mail or password");
            TokenGenerator tokenGenerator = new ();
            UserDto userDto = new UserDto() {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
            return new UserDto() { Token = tokenGenerator.Generate(userDto)};
            
            
        }
        public UserDto Add(UserDtoInsert user)
        {
            if(_context.Users.Any(u => u.Email == user.Email))
               throw new Exception("User email already exists");

            _context.Users.Add(new User{
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            });
            _context.SaveChanges();
            return _context.Users
                .Where(u => u.Email == user.Email)
                .Select(u => new UserDto{
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    UserType = u.UserType
                })
                .First();
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetUsers()
        {
           throw new NotImplementedException();
        }

    }
}