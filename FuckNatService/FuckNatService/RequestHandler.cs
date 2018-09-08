using Datagrammer;
using Datagrammer.MessagePack;
using System.Net;
using System.Threading.Tasks;

namespace FuckNatService
{
    class RequestHandler : MessagePackHandler<RequestDto>
    {
        private readonly IDatagramSender sender;

        public RequestHandler(IDatagramSender sender)
        {
            this.sender = sender;
        }

        public override async Task HandleAsync(RequestDto data, IPEndPoint endPoint)
        {
            await sender.SendByMessagePackAsync(new ResponseDto
            {
                SessionID = data.SessionID,
                Address = endPoint.Address.GetAddressBytes(),
                Port = endPoint.Port
            }, endPoint);
        }
    }
}
