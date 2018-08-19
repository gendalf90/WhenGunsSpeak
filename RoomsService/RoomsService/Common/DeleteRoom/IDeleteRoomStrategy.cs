using System.Threading.Tasks;

namespace RoomsService.Common.DeleteRoom
{
    interface IDeleteRoomStrategy
    {
        Task DeleteAsync(string roomId);
    }
}
