using System.Threading.Tasks;

namespace RoomsService.Common.SaveNewRoom
{
    public interface ISaveNewRoomStrategy
    {
        Task SaveAsync(string roomId, string ownerId);
    }
}
