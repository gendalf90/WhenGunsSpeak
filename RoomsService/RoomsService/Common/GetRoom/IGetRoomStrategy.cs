using RoomsService.Controllers.Rooms;
using System.Threading.Tasks;

namespace RoomsService.Common.GetRoom
{
    interface IGetRoomStrategy
    {
        Task<RoomDto> GetByIdAsync(string id);
    }
}
