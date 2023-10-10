using TrybeHotel.Models;
using TrybeHotel.Dto;
using System.Linq;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        private int hotelIndex = -1;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            return from hotel in _context.Hotels
                   select new HotelDto
                   {
                       HotelId = hotel.HotelId,
                       Name = hotel.Name,
                       Address = hotel.Address,
                       CityId = hotel.CityId,
                       CityName = hotel.City.Name,
                       State = hotel.City.State
                       
                   };
        }
        
        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges(); 
            var created = (from h in _context.Hotels
                          where h.HotelId == hotel.HotelId
                          select h).First();
            return new HotelDto
            {
                HotelId = created.HotelId,
                Name = created.Name,
                Address = created.Address,
                CityId = created.CityId,
                CityName = (from city in _context.Cities
                            where city.CityId == created.CityId
                            select city.Name).First(),
                State = (from city in _context.Cities
                            where city.CityId == created.CityId
                            select city.State).First()
                
            };      
            
        }
    }
}