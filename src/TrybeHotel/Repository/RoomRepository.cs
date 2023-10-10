using TrybeHotel.Models;
using TrybeHotel.Dto;
using System;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return (from hotel in _context.Hotels
                    where hotel.HotelId == HotelId
                    join room in _context.Rooms on hotel.HotelId equals room.HotelId

                    select new RoomDto
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
                            CityName = hotel.City.Name,
                            State = hotel.City.State
                        }
                    }).ToList();
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return (from hotel in _context.Hotels
                    where hotel.HotelId == room.HotelId

                    select new RoomDto
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
                            CityName = hotel.City.Name,
                            State = hotel.City.State
                        }
                    }).First();
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId) {
            var room = from r in _context.Rooms
                       where r.RoomId == RoomId
                       select r;
            _context.Rooms.Remove(room.First());
            _context.SaveChanges();
        }
    }
}