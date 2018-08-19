using RoomsService.Controllers.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomsService.Common.GetAllRooms
{
    interface IGetAllRoomsStrategy
    {
        Task<IEnumerable<RoomShortDto>> GetAsync();
    }
}
