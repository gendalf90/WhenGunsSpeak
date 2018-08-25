using Microsoft.Extensions.Options;

namespace FuckNatService.Messages
{
    class RequestCreator : IRequestCreator
    {
        private readonly IOptions<SecurityOptions> securityOptions;

        public RequestCreator(IOptions<SecurityOptions> securityOptions)
        {
            this.securityOptions = securityOptions;
        }

        public IRequest Create()
        {
            return new Request(securityOptions.Value.SecurityKey);
        }
    }
}
