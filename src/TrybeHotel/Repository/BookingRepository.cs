using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            Room room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
            User user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            if (room.Capacity < booking.GuestQuant)
            {
                throw new Exception("Guest quantity over room capacity");
            }
            var newBook = _context.Bookings!.Add(new Booking()
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = user.UserId,
                RoomId = booking.RoomId
            });
            _context.SaveChanges();

            return (
                 from book in _context.Bookings
                 where book.RoomId == booking.RoomId
                 join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                 join city in _context.Cities on hotel.CityId equals city.CityId
                 select new BookingResponse()
                 {
                     BookingId = newBook.Entity.BookingId,
                     CheckIn = book.CheckIn,
                     CheckOut = book.CheckOut,
                     GuestQuant = book.GuestQuant,
                     Room = new RoomDto()
                     {
                         RoomId = room.RoomId,
                         Name = room.Name,
                         Capacity = room.Capacity,
                         Image = room.Image,
                         Hotel = new HotelDto()
                         {
                             HotelId = hotel.HotelId,
                             Name = hotel.Name,
                             Address = hotel.Address,
                             CityId = hotel.CityId,
                             State = city.State,
                         }
                     }
                 }
                 ).First();

        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var booking = _context.Bookings!.FirstOrDefault(b => b.BookingId == bookingId);
            if(booking.UserId != user.UserId) throw new Exception();
            return (
                 from book in _context.Bookings
                 where book.RoomId == booking.RoomId
                 join room in _context.Rooms on book.RoomId equals room.RoomId
                 join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                 join city in _context.Cities on hotel.CityId equals city.CityId
                 select new BookingResponse()
                 {
                     BookingId = book.BookingId,
                     CheckIn = book.CheckIn,
                     CheckOut = book.CheckOut,
                     GuestQuant = book.GuestQuant,
                     Room = new RoomDto()
                     {
                         RoomId = room.RoomId,
                         Name = room.Name,
                         Capacity = room.Capacity,
                         Image = room.Image,
                         Hotel = new HotelDto()
                         {
                             HotelId = hotel.HotelId,
                             Name = hotel.Name,
                             Address = hotel.Address,
                             CityId = hotel.CityId,
                                State = city.State,
                         }
                     }
                 }
                 ).First();


        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

    }

}