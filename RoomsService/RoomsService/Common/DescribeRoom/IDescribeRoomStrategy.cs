using System.Threading.Tasks;

namespace RoomsService.Common.DescribeRoom
{
    interface IDescribeRoomStrategy
    {
        Task DescribeAsync(string roomId, string description);
    }
}
