using System.Threading.Tasks;

namespace RoomsService.Common.GetToken
{
    interface IGetTokenStrategy
    {
        Task<string> GetAsync();
    }
}
