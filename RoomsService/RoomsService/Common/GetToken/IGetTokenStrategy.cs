using System.Threading.Tasks;

namespace RoomsService.Common.GetToken
{
    public interface IGetTokenStrategy
    {
        Task<string> GetAsync();
    }
}
