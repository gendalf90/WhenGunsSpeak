using Datagrammer;
using Datagrammer.MessagePack;
using System.Net;
using System.Threading.Tasks;

namespace FuckNatService
{
    class RequestHandler : MessagePackHandler<RequestDto>
    {
        private const byte ResponseMessageType = 2;

        public override async Task HandleAsync(IContext context, RequestDto data, IPEndPoint endPoint)
        {
            await context.SendByMessagePackAsync(new ResponseDto
            {
                MessageType = ResponseMessageType,
                UserId = data.UserId,
                Address = endPoint.Address.GetAddressBytes(),
                Port = endPoint.Port
            }, endPoint);
        }
    }
}
