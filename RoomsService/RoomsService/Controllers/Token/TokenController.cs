using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoomsService.Common.GetToken;

namespace RoomsService.Controllers.Token
{
    [Route("api/token")]
    class TokenController : ControllerBase
    {
        private IGetTokenStrategy getTokenStrategy;

        public TokenController(IGetTokenStrategy getTokenStrategy)
        {
            this.getTokenStrategy = getTokenStrategy;
        }

        [HttpGet]
        public async Task<IActionResult> GetTokenAsync()
        {
            var token = await getTokenStrategy.GetAsync();
            return Ok(token);
        }
    }
}