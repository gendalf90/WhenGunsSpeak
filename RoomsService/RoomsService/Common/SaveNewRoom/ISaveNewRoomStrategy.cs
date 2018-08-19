using System.Threading.Tasks;

namespace RoomsService.Common.SaveNewRoom
{
    interface ISaveNewRoomStrategy
    {
        Task SaveAsync(string roomId, string ownerId);
    }
}
