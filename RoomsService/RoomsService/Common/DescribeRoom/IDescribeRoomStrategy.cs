using System.Threading.Tasks;

namespace RoomsService.Common.DescribeRoom
{
    public interface IDescribeRoomStrategy
    {
        Task DescribeAsync(string ownerId, string description);
    }
}
