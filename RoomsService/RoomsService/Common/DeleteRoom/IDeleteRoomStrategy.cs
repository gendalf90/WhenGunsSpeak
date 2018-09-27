using System.Threading.Tasks;

namespace RoomsService.Common.DeleteRoom
{
    public interface IDeleteRoomStrategy
    {
        Task DeleteAsync(string roomId);
    }
}
