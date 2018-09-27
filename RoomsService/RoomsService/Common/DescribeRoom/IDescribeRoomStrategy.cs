using System.Threading.Tasks;

namespace RoomsService.Common.DescribeRoom
{
    public interface IDescribeRoomStrategy
    {
        Task DescribeAsync(string roomId, string description);
    }
}
