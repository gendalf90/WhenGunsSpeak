using System.Threading.Tasks;

namespace RoomsService.Common.CreateRoom
{
    public interface ICreateRoomStrategy
    {
        Task CreateAsync(string ownerId, string header);
    }
}
