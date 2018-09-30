using RoomsService.Controllers.Rooms;
using System.Threading.Tasks;

namespace RoomsService.Common.GetRoom
{
    public interface IGetRoomStrategy
    {
        Task<RoomDto> GetByOwnerAsync(string ownerId);
    }
}
