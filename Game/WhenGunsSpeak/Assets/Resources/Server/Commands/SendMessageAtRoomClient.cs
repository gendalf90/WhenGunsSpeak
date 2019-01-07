using BinaryProcessing;

namespace Server
{
    public class SendMessageAtRoomClientCommand
    {
        public SendMessageAtRoomClientCommand(Binary data)
        {
            Data = data;
        }

        public Binary Data { get; private set; }
    }
}
